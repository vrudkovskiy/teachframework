//
//  LabelBuilder.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "LabelBuilder.h"
#import "LabelViewController.h"

@implementation LabelBuilder

- (id<UiItemProtocol>)createWithUiDescription:(NSDictionary *)uiDescription
{
    return [[[LabelViewController alloc] initWithUiDescriptionItem:uiDescription] autorelease];
}

- (NSString *)description
{
    return @"Label";
}

@end
