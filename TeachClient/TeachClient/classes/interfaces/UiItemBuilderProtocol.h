//
//  UiItemBuilderProtocol.h
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import "UiItemProtocol.h"

@protocol UiItemBuilderProtocol <NSObject>

- (NSString *)description;
- (id<UiItemProtocol>)createWithUiDescription:(NSDictionary *)uiDescription;

@end
