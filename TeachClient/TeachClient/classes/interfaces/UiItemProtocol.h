//
//  UiItemProtocol.h
//  TeachClient
//
//  Created by Vladislav Rudkovskiy on 18.06.13.
//  Copyright (c) 2013 QAP. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol UiItemProtocol <NSObject>

- (id)initWithUiDescriptionItem:(NSDictionary *)descriptionDictionary;
- (NSString *)jsonRepresentation;

@end
