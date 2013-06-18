//
//  DataRadioButtonViewController.h
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "UiItemProtocol.h"

@interface DataRadioButtonViewController : UIViewController<UiItemProtocol>

@property (nonatomic, assign) BOOL checked;

- (void)radioDidSelected;

@end
