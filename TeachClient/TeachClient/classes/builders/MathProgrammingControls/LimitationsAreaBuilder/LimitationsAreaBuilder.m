//
//  LimitationsAreaBuilder.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 20.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "LimitationsAreaBuilder.h"
#import "LimitationsAreaViewController.h"

@implementation LimitationsAreaBuilder

- (id<UiItemProtocol>)createWithUiDescription:(NSDictionary *)uiDescription
{
    return [[[LimitationsAreaViewController alloc] initWithUiDescriptionItem:uiDescription] autorelease];
}

- (NSString *)description
{
    return @"LimitationsArea";
}

@end
