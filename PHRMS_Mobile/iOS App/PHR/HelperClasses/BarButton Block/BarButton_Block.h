//
//  BarButton_Block.h
//  Aleph3_iPod
//
//  Created by Lokesh Jain on 11/09/13.
//  Copyright (c) 2013 lokesh. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

typedef void (^barButtonCompletionBlock) (UIButton *barButton,UIBarButtonItem *barItem);
typedef void (^rightBarButtonBlock) (UIBarButtonItem *barItem);

@interface BarButton_Block : NSObject

/** Creating Bar Button Item Method using Block.
 Completion Block is used to handle Button Creation.
 */

+ (void)setCustomBarButtonItem:(barButtonCompletionBlock)completionBlock;
//+ (void)setRightBarButtonItem:(id)target andBlock:(rightBarButtonBlock)completionBlock;


@end
