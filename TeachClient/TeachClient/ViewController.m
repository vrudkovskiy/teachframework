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
}

- (void)setDataControls:(NSString *)jsonArrayString
{    
    NSArray *controlsDictioneries = [NSJSONSerialization JSONObjectWithData:[jsonArrayString dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:nil];
    
    float coordY = 10;
    for (NSDictionary *dictionary in controlsDictioneries)
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
    NSString *controlsString = [self.controls componentsJoinedByString:@", "];
    return [NSString stringWithFormat:@"[ %@ ]", controlsString != nil ? controlsString : @""];
}

- (void)clearControls
{
    for (UIViewController *viewController in self.controls)
    {
        [viewController.view removeFromSuperview];
    }
    self.controls = nil;
}

- (void)submit
{
    [self sendRequestWithUrl:@"submit"];
}

- (void)reset
{
    [self sendRequestWithUrl:@"reset"];
}

- (void)automatic
{
    [self sendRequestWithUrl:@"auto"];
}

- (void)sendRequestWithUrl:(NSString *)urlStr
{
    [self showActivityIndicator];
    
    NSURL *url = [NSURL URLWithString:@"http://46.219.18.138/wcf-service/TeachService.svc/"];
    AFHTTPClient *httpClient = [[AFHTTPClient alloc] initWithBaseURL:url];
    
    httpClient.parameterEncoding = AFJSONParameterEncoding;
    NSDictionary *params = @{ @"Controls": self.jsonRepresentation };
    NSMutableURLRequest *request = [httpClient requestWithMethod:@"POST" path:urlStr parameters:params];
    
    AFHTTPRequestOperation *operation = [AFJSONRequestOperation JSONRequestOperationWithRequest:request
                                                                                        success:^(NSURLRequest *request, NSHTTPURLResponse *response, id JSON)
                                         {
                                             NSLog (@"%@", JSON);
                                             
                                             [self setDataControls:[JSON valueForKeyPath:@"GetDataResult"]];
                                             [self hideAvtivityIndicator];
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
    self.activityView.hidden = YES;
}

- (void)hideAvtivityIndicator
{
    self.activityView.hidden = YES;
}

@end
