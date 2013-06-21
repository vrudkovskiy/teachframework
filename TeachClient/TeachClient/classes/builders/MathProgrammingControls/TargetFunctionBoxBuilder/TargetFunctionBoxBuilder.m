//
//  TargetFunctionBoxBuilder.m
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 20.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "TargetFunctionBoxBuilder.h"
#import "TargetFunctionBoxViewController.h"

@implementation TargetFunctionBoxBuilder

- (id<UiItemProtocol>)createWithUiDescription:(NSDictionary *)uiDescription
{
    return [[[TargetFunctionBoxViewController alloc] initWithUiDescriptionItem:uiDescription] autorelease];
}

- (NSString *)description
{
    return @"TargetFunctionBox";
}

@end
