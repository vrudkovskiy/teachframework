//
//  LppViewBuilder.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 20.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "LppViewBuilder.h"
#import "LppViewController.h"

@implementation LppViewBuilder

- (id<UiItemProtocol>)createWithUiDescription:(NSDictionary *)uiDescription
{
    return [[[LppViewController alloc] initWithUiDescriptionItem:uiDescription] autorelease];
}

- (NSString *)description
{
    return @"LppView";
}

@end
