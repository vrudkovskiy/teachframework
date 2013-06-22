//
//  ViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 17.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "ViewController.h"

#import "AFHTTPClient.h"
#import "AFJSONRequestOperation.h"

#import "DataRadioButtonBuilder.h"
#import "LabelBuilder.h"
#import "SignedTextBoxBuilder.h"

#import "TargetFunctionBoxBuilder.h"
#import "LimitationsAreaBuilder.h"
#import "LppViewBuilder.h"

@interface ViewController ()

@property (nonatomic, retain) IBOutlet UIScrollView *controlsPanel;
@property (nonatomic, retain) IBOutlet UIView *activityView;

@property (nonatomic, retain) NSMutableDictionary *controlBuilders;
@property (nonatomic, retain) NSMutableArray *controls;

- (IBAction)onResetClick:(id)sender;
- (IBAction)onSubmitClick:(id)sender;
- (IBAction)onAutoClick:(id)sender;

@end

@implementation ViewController

@synthesize controlsPanel;
@synthesize activityView;

@synthesize controlBuilders;
@synthesize controls;

- (void)dealloc
{
    self.controlsPanel = nil;
    self.activityView = nil;
    
    self.controlBuilders = nil;
    self.controls = nil;
    
    [super dealloc];
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    [self initControlBuilders];
    self.controls = [NSMutableArray array];
    
    [self submit];
}

- (IBAction)onResetClick:(id)sender
{
    [self reset];
}

- (IBAction)onSubmitClick:(id)sender
{
    [self submit];
}

- (IBAction)onAutoClick:(id)sender
{
    [self automatic];
}

- (void)initControlBuilders
{
    self.controlBuilders = [NSMutableDictionary dictionary];
    
    id<UiItemBuilderProtocol> builder = [[[DataRadioButtonBuilder alloc] init] autorelease];
    [self.controlBuilders setObject:builder forKey:builder.description];
    
    builder = [[[LabelBuilder alloc] init] autorelease];
    [self.controlBuilders setObject:builder forKey:builder.description];
    
    builder = [[[SignedTextBoxBuilder alloc] init] autorelease];
    [self.controlBuilders setObject:builder forKey:builder.description];
    
    
    builder = [[[TargetFunctionBoxBuilder alloc] init] autorelease];
    [self.controlBuilders setObject:builder forKey:builder.description];
    
    builder = [[[LimitationsAreaBuilder alloc] init] autorelease];
    [self.controlBuilders setObject:builder forKey:builder.description];
    
    builder = [[[LppViewBuilder alloc] init] autorelease];
    [self.controlBuilders setObject:builder forKey:builder.description];
}

- (void)setDataControls:(NSArray *)jsonArray
{    
    float coordY = 44;
    for (NSDictionary *dictionary in jsonArray)
    {
        NSString *controlType = [dictionary objectForKey:@"ControlType"];
        id<UiItemBuilderProtocol> builder = [self.controlBuilders objectForKey:controlType];
        UIViewController *control = (UIViewController *)[builder createWithUiDescription:dictionary];
        [self.controls addObject:control];
        
        control.view.frame = CGRectMake(0, coordY, control.view.frame.size.width, control.view.frame.size.height);
        [self.controlsPanel addSubview:control.view];
        [self addChildViewController:control];
        
        coordY += control.view.frame.size.height + 10;
    }
}

- (NSString *)jsonRepresentation
{
    NSString *controlsString = @"";
    for (id<UiItemProtocol> control in self.controls)
    {
        controlsString = [controlsString stringByAppendingString:control.jsonRepresentation];
        if (control != [self.controls lastObject])
        {
            controlsString = [controlsString stringByAppendingString:@", "];
        }
    }
    
    return [NSString stringWithFormat:@"[ %@ ]", controlsString != nil ? controlsString : @""];
}

- (void)clearControls
{
    for (UIViewController *viewController in self.controls)
    {
        [viewController.view removeFromSuperview];
    }
    self.controls = [NSMutableArray array];
}

- (void)submit
{
    [self sendRequestWithUrl:@"submit" key:@"SubmitResult"];
}

- (void)reset
{
    [self sendRequestWithUrl:@"reset" key:@"ResetResult"];
}

- (void)automatic
{
    [self sendRequestWithUrl:@"auto" key :@"AutoResult"];
}

- (void)sendRequestWithUrl:(NSString *)urlStr key:(NSString *)key
{
    [self showActivityIndicator];
    
    NSURL *url = [NSURL URLWithString:@"http://46.219.18.138/wcf-service/TeachService.svc/"];
    AFHTTPClient *httpClient = [[AFHTTPClient alloc] initWithBaseURL:url];
    
    httpClient.parameterEncoding = AFJSONParameterEncoding;
    NSDictionary *params = @{ @"Controls": self.jsonRepresentation };
    NSLog (@"%@ ", [params objectForKey:@"Controls"]);
    NSMutableURLRequest *request = [httpClient requestWithMethod:@"POST" path:urlStr parameters:params];
    
    AFHTTPRequestOperation *operation = [AFJSONRequestOperation JSONRequestOperationWithRequest:request
                                                                                        success:^(NSURLRequest *request, NSHTTPURLResponse *response, id JSON)
                                         {
                                             [self clearControls];
                                             NSString *respStr = [[JSON valueForKeyPath:key] stringByReplacingOccurrencesOfString:@"\r\n" withString:@""];
                                             NSLog (@"%@", respStr);
                                             NSError *err = nil;
                                             NSDictionary *result = [NSJSONSerialization JSONObjectWithData:[respStr dataUsingEncoding:NSUTF8StringEncoding]
                                                                                                     options:NSJSONReadingMutableContainers
                                                                                                       error:&err];
                                             if (err != nil)
                                             NSLog (@"%@", err.localizedDescription);
                                             [self setDataControls:[result objectForKey:@"Controls"]];
                                             [self hideAvtivityIndicator];
                                             
                                             if (((NSString *)[result objectForKey:@"Message"]).length > 0)
                                             {
                                                 NSLog (@"Message: %@", [result objectForKey:@"Message"]);
                                                 UIAlertView *alert = [[UIAlertView alloc] initWithTitle:nil
                                                                                                 message:[result objectForKey:@"Message"]
                                                                                                delegate:nil
                                                                                       cancelButtonTitle:@"OK"
                                                                                       otherButtonTitles: nil];
                                                 [alert show];
                                                 [alert release];
                                             }
                                         }
                                                                                        failure:^(NSURLRequest *request, NSHTTPURLResponse *response, NSError *error, id JSON)
                                         {
                                             [self hideAvtivityIndicator];
                                             
                                             UIAlertView *alert = [[UIAlertView alloc] initWithTitle:nil
                                                                                             message:error.localizedDescription
                                                                                            delegate:nil
                                                                                   cancelButtonTitle:@"OK"
                                                                                   otherButtonTitles: nil];
                                             [alert show];
                                             [alert release];
                                         }];
    [operation start];
}

- (void)showActivityIndicator
{
    self.activityView.hidden = NO;
}

- (void)hideAvtivityIndicator
{
    self.activityView.hidden = YES;
}

@end
