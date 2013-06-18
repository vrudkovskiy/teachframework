//
//  ViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 17.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "ViewController.h"

#import "DataRadioButtonBuilder.h"
#import "LabelBuilder.h"
#import "SignedTextBoxBuilder.h"

@interface ViewController ()

@property (nonatomic, retain) NSMutableDictionary *controlBuilders;
@property (nonatomic, retain) NSMutableArray *controls;

@end

@implementation ViewController

@synthesize controlBuilders;
@synthesize controls;

- (void)dealloc
{
    self.controlBuilders = nil;
    self.controls = nil;
    
    [super dealloc];
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    [self initControlBuilders];
    [self setDataControls:@"[{\"Editable\": true, \"Name\": \"a\", \"Value\": \"la-la-la\", \"ControlType\": \"Label\"}, {\"Editable\": true, \"Name\": \"b\", \"Value\": \"la-la-la\", \"Text\": \"some text\", \"ControlType\": \"SignedTextBox\"}, {\"Editable\": true, \"Name\": \"c\", \"Value\": true, \"Text\": \"1\", \"ControlType\": \"DataRadioButton\"}, {\"Editable\": true, \"Name\": \"c2\", \"Value\": false, \"Text\": \"2\", \"ControlType\": \"DataRadioButton\"}, {\"Editable\": true, \"Name\": \"c3\", \"Value\": false, \"Text\": \"3\", \"ControlType\": \"DataRadioButton\"}]"];
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
        [self.view addSubview:control.view];
        [self addChildViewController:control];
        
        coordY += control.view.frame.size.height + 10;
    }
}

- (void)clearControls
{
    for (UIViewController *viewController in self.controls)
    {
        [viewController.view removeFromSuperview];
    }
    self.controls = nil;
}

@end
