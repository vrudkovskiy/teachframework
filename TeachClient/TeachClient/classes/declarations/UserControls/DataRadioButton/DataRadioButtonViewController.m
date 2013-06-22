//
//  DataRadioButtonViewController.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "DataRadioButtonViewController.h"
#import "RadioButton.h"

@interface DataRadioButtonViewController ()

@property (nonatomic, retain) IBOutlet RadioButton *btnRadio;
@property (nonatomic, retain) IBOutlet UILabel *lblText;

@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *text;
@property (nonatomic, assign) BOOL editable;

- (IBAction)onRadioTap:(id)sender;

@end

@implementation DataRadioButtonViewController

@synthesize btnRadio;
@synthesize lblText;

@synthesize name;
@synthesize checked;
@synthesize text;
@synthesize editable;


- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary
{
    self = [super initWithNibName:@"DataRadioButtonViewController" bundle:nil];
    if (self)
    {
        self.name = [descriptionDictionary objectForKey:@"Name"];
        self.checked = [[descriptionDictionary objectForKey:@"Value"] boolValue];
        self.text = [descriptionDictionary objectForKey:@"Text"];
        self.editable = [[descriptionDictionary objectForKey:@"Editable"] boolValue];
    }
    return self;
}

- (NSString *)jsonRepresentation
{
    return [NSString stringWithFormat:@"{\"Name\": \"%@\", \"Text\": \"%@\", \"Value\": %@}", self.name, self.text, self.btnRadio.selected ? @"true" : @"false"];
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    UITapGestureRecognizer *recognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(onRadioTap:)];
    [self.view addGestureRecognizer:recognizer];
    
    self.btnRadio.selected = self.checked;
    self.btnRadio.enabled = self.editable;
    self.lblText.text = self.text;
    self.btnRadio.container = self;
}

- (void)setChecked:(BOOL)aChecked
{
    checked = aChecked;
    
    self.btnRadio.selected = aChecked;
}

- (IBAction)onRadioTap:(id)sender
{
    [self.btnRadio setSelected:!self.btnRadio.selected];
}

- (void)radioDidSelected
{    
    for (UIViewController *controller in self.parentViewController.childViewControllers)
    {
        if ([controller isKindOfClass:DataRadioButtonViewController.class] && controller != self)
        {
            ((DataRadioButtonViewController *)controller).checked = NO;
        }
    }
}

@end
