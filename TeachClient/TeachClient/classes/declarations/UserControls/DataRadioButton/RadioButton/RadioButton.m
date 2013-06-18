//
//  RadioButton.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "RadioButton.h"

@implementation RadioButton

@synthesize container;
@synthesize selectedState;


- (void)setSelectedState:(BOOL)aSelected
{
    selectedState = aSelected;
    
    [self setBackgroundImage:[UIImage imageNamed:aSelected ? @"CellSelected.png" : @"CellNotSelected.png"] forState:UIControlStateNormal];
    
    if (aSelected)
    {
        [self.container radioDidSelected];
    }
}

- (void)setSelected:(BOOL)selected
{
    [super setSelected:selected];
    [self setSelectedState:selected];
}

@end
