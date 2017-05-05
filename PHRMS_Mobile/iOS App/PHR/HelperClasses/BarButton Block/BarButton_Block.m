//
//  BarButton_Block.m
//  Aleph3_iPod
//
//  Created by Lokesh Jain on 11/09/13.
//  Copyright (c) 2013 lokesh. All rights reserved.
//

#import "BarButton_Block.h"

@implementation BarButton_Block

/**
  *  Creating Custom Bar Button method Implementation.
 **/

+ (void)setCustomBarButtonItem:(barButtonCompletionBlock)completionBlock{
    
    UIButton *gridButton=[UIButton buttonWithType:UIButtonTypeCustom];
    gridButton.frame=CGRectMake(0,0, 20, 15);
    [gridButton setImage:[UIImage imageNamed:@"grid_btn.png"] forState:UIControlStateNormal];
    
    UIBarButtonItem *barButtonItem = [[UIBarButtonItem alloc] initWithCustomView:gridButton];
    
    completionBlock(gridButton,barButtonItem);
}

//+ (void)setRightBarButtonItem:(id)target andBlock:(rightBarButtonBlock)completionBlock{
//    UIBarButtonItem *barButtonItem = [[UIBarButtonItem alloc] initWithTitle:@"Turn Off Chat" style:UIBarButtonItemStyleDone target:target action:@selector(createGroupButtonClicked)];
//    completionBlock(barButtonItem);
//}


@end
