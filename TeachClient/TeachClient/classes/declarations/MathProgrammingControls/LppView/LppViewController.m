//
//  LppViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 20.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "LppViewController.h"
#import "TargetFunctionBoxViewController.h"
#import "LimitationsAreaViewController.h"

@interface LppViewController ()

@property (nonatomic, retain) IBOutlet UIView *targetPlaceholder;
@property (nonatomic, retain) IBOutlet UIView *limitationsPlaceholder;

@property (nonatomic, retain) NSString *name;
@property (nonatomic, assign) BOOL editable;

@property (nonatomic, retain) TargetFunctionBoxViewController *target;
@property (nonatomic, retain) LimitationsAreaViewController *limitations;

@end

@implementation LppViewController

- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary
{
    self = [super initWithNibName:@"LppViewController" bundle:nil];
    if (self)
    {
        self.name = [descriptionDictionary objectForKey:@"Name"];
        self.editable = [[descriptionDictionary objectForKey:@"Editable"] boolValue];
        
        NSDictionary *lppDictionary = [descriptionDictionary objectForKey:@"Value"];
        self.target = [[[TargetFunctionBoxViewController alloc] initWithUiDescriptionItem:[lppDictionary objectForKey:@"TargetFunction"]] autorelease];
        self.limitations = [[[LimitationsAreaViewController alloc] initWithUiDescriptionItem:[lppDictionary objectForKey:@"LimitationsArea"]] autorelease];
    }
    return self;
}

- (NSString *)jsonRepresentation
{
    NSString *jsonValueStr = [NSString stringWithFormat:@"{ \"TargetFunction\" : \"%@\", \"Limitations\" : \"%@\"}",
                              [self.target.jsonRepresentation stringByReplacingOccurrencesOfString:@"\"" withString:@"<lpp_tf>"],
                              [self.limitations.jsonRepresentation stringByReplacingOccurrencesOfString:@"\"" withString:@"<lpp_lim>"]];
    
    return [NSString stringWithFormat:@"{\"Name\" : \"%@\", \"Value\" : \"%@\" }", self.name, [jsonValueStr stringByReplacingOccurrencesOfString:@"\"" withString:@"<lpp_value>"]];
}

- (void)dealloc
{
    self.name = nil;
    self.target = nil;
    self.limitations = nil;
    
    self.targetPlaceholder = nil;
    self.limitationsPlaceholder = nil;
    
    [super dealloc];
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    self.target.view.frame = self.targetPlaceholder.frame;
    [self.view insertSubview:self.target.view aboveSubview:self.targetPlaceholder];
    
    self.limitations.view.frame = self.limitationsPlaceholder.frame;
    [self.view insertSubview:self.limitations.view aboveSubview:self.limitationsPlaceholder];
}

@end
