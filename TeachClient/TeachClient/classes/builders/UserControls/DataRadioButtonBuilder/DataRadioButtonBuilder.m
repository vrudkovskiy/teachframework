//
//  DataRadioButtonBuilder.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "DataRadioButtonBuilder.h"
#import "DataRadioButtonViewController.h"

@implementation DataRadioButtonBuilder

- (id<UiItemProtocol>)createWithUiDescription:(NSDictionary *)uiDescription
{
    return [[[DataRadioButtonViewController alloc] initWithUiDescriptionItem:uiDescription] autorelease];
}

- (NSString *)description
{
    return @"DataRadioButton";
}

@end
