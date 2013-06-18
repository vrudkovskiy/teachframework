//
//  RadioButton.h
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "DataRadioButtonViewController.h"

@interface RadioButton : UIButton

@property (nonatomic, retain) DataRadioButtonViewController *container;
@property (nonatomic, assign) BOOL selectedState;

@end
