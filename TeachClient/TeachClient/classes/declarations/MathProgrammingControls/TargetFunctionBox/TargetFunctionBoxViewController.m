//
//  TargetFunctionBoxViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 20.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "TargetFunctionBoxViewController.h"

@interface TargetFunctionBoxViewController ()

@property (nonatomic, retain) IBOutlet UITextField *tfFormula;
@property (nonatomic, retain) IBOutlet UIButton *btnTarget;

@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *formula;
@property (nonatomic, retain) NSString *target;
@property (nonatomic, assign) BOOL editable;

@end


@implementation TargetFunctionBoxViewController

@synthesize tfFormula;
@synthesize btnTarget;

@synthesize name;
@synthesize formula;
@synthesize target;
@synthesize editable;


- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary
{
    self = [super initWithNibName:@"TargetFunctionBoxViewController" bundle:nil];
    if (self)
    {
        self.name = [descriptionDictionary objectForKey:@"Name"];
        self.editable = [[descriptionDictionary objectForKey:@"Editable"] boolValue];
        
        NSDictionary *targetFunctionDictionary = [NSJSONSerialization JSONObjectWithData:[[descriptionDictionary objectForKey:@"Value"] dataUsingEncoding:NSUTF8StringEncoding]
                                                                                 options:NSJSONReadingMutableContainers
                                                                                   error:nil];
        self.formula = [targetFunctionDictionary objectForKey:@"Formula"];
        self.target = [targetFunctionDictionary objectForKey:@"Target"];
    }
    return self;
}

- (NSString *)jsonRepresentation
{
    return [NSString stringWithFormat:@"{\"Name\" : \"%@\", \"Value\" : \"{ \"Formula\" : \"%@\", \"Target\" : \"%@\"}\"}", self.name, self.tfFormula.text, self.btnTarget.titleLabel.text];
}

- (void)dealloc
{
    self.tfFormula = nil;
    self.btnTarget = nil;
    
    self.name = nil;
    self.formula = nil;
    self.target = nil;
    
    [super dealloc];
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    self.tfFormula.text = self.formula;
    self.tfFormula.enabled = !self.editable;
    self.btnTarget.titleLabel.text = self.target;
    self.tfFormula.enabled = !self.editable;
}

@end
