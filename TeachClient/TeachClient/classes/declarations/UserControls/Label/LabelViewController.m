//
//  LabelViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 17.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "LabelViewController.h"

@interface LabelViewController ()

@property (nonatomic, retain) IBOutlet UILabel *lblText;

@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *text;

@end

@implementation LabelViewController

@synthesize lblText;
@synthesize name;
@synthesize text;

- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary
{
    self = [super initWithNibName:@"LabelViewController" bundle:nil];
    if (self)
    {
        self.text = [descriptionDictionary objectForKey:@"Value"];
        self.name = [descriptionDictionary objectForKey:@"Name"];
    }
    return self;
}

- (NSString *)jsonRepresentation
{
    return [NSString stringWithFormat:@"{\"Name\": \"%@\", \"Value\": \"%@\"}", self.name, self.lblText.text];
}

- (void)dealloc
{
    self.lblText = nil;
    self.name = nil;
    self.text = nil;
    
    [super dealloc];
}


- (void)viewDidLoad
{
    [super viewDidLoad];
    
    self.lblText.text = self.text;
}

@end
