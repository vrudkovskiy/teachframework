//
//  LimitationsAreaViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 20.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "LimitationsAreaViewController.h"

@interface LimitationsAreaViewController ()

@property (nonatomic, retain) IBOutlet UITextView *tvLimitations;

@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *limitations;
@property (nonatomic, assign) BOOL editable;

@end

@implementation LimitationsAreaViewController

- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary
{
    self = [super initWithNibName:@"LimitationsAreaViewController" bundle:nil];
    if (self)
    {/*
        self.name = [descriptionDictionary objectForKey:@"Name"];
        self.formula = [descriptionDictionary objectForKey:@"Text"];
        self.target = [descriptionDictionary objectForKey:@"Value"];
        self.editable = [[descriptionDictionary objectForKey:@"Editable"] boolValue];*/
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
