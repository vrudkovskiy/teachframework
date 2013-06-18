//
//  SignedTextBoxBuilder.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "SignedTextBoxBuilder.h"
#import "SignedTextBoxViewController.h"

@implementation SignedTextBoxBuilder

- (id<UiItemProtocol>)createWithUiDescription:(NSDictionary *)uiDescription
{
    return [[[SignedTextBoxViewController alloc] initWithUiDescriptionItem:uiDescription] autorelease];
}

- (NSString *)description
{
    return @"SignedTextBox";
}

@end
