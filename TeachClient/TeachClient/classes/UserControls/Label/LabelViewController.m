//
//  LabelViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 17.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "LabelViewController.h"

@interface LabelViewController ()

@property (nonatomic, retain) UILabel *lblText;

@end

@implementation LabelViewController

@synthesize lblText;

- (id)initWithUiDescriptionItem:(NSString *)jsonString
{
    self = [super initWithNibName:@"LabelViewController" bundle:nil];
    if (self)
    {
        // Custom initialization
    }
    return self;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    
}

@end
