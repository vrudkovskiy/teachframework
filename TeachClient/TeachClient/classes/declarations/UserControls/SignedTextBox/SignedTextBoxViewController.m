//
//  SignedTextBoxViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "SignedTextBoxViewController.h"

@interface SignedTextBoxViewController ()

@property (nonatomic, retain) IBOutlet UILabel *lblSign;
@property (nonatomic, retain) IBOutlet UITextField *tfText;

@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *sign;
@property (nonatomic, retain) NSString *text;
@property (nonatomic, assign) BOOL editable;

@end

@implementation SignedTextBoxViewController

@synthesize lblSign;
@synthesize tfText;

@synthesize name;
@synthesize sign;
@synthesize text;
@synthesize editable;


- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary
{
    self = [super initWithNibName:@"SignedTextBoxViewController" bundle:nil];
    if (self)
    {
        self.name = [descriptionDictionary objectForKey:@"Name"];
        self.sign = [descriptionDictionary objectForKey:@"Text"];
        self.text = [descriptionDictionary objectForKey:@"Value"];
        self.editable = [[descriptionDictionary objectForKey:@"Editable"] boolValue];
    }
    return self;
}

- (NSString *)jsonRepresentation
{
    return [NSString stringWithFormat:@"{\"Name\": \"%@\", \"Text\": \"%@\", \"Value\": \"%@\"}", self.name, self.sign, self.tfText.text];
}

- (void)dealloc
{
    self.lblSign = nil;
    self.tfText = nil;
    self.name = nil;
    self.text = nil;
    self.sign = nil;
    
    [super dealloc];
}


- (void)viewDidLoad
{
    [super viewDidLoad];
    
    self.lblSign.text = self.sign;
    self.tfText.text = self.text;
    self.tfText.enabled = self.editable;
}

@end
