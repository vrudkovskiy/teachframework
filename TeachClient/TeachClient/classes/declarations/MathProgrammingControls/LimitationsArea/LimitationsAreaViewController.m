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
@property (nonatomic, retain) IBOutlet UIImageView *ivBorder;

@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *limitations;
@property (nonatomic, assign) BOOL editable;

@end

@implementation LimitationsAreaViewController

- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary
{
    self = [super initWithNibName:@"LimitationsAreaViewController" bundle:nil];
    if (self)
    {
        self.name = [descriptionDictionary objectForKey:@"Name"];
        self.limitations = [descriptionDictionary objectForKey:@"Value"];
        self.editable = [[descriptionDictionary objectForKey:@"Editable"] boolValue];
    }
    return self;
}

- (NSString *)jsonRepresentation
{
    return [NSString stringWithFormat:@"{\"Name\" : \"%@\", \"Value\" : \"%@\"}", self.name, self.tvLimitations.text];
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    self.tvLimitations.text = self.limitations;
    self.tvLimitations.editable = self.editable;
    
    self.ivBorder.image = [[UIImage imageNamed:@"textView_bg_stretchable.png"] stretchableImageWithLeftCapWidth:5 topCapHeight:5];
}

@end
