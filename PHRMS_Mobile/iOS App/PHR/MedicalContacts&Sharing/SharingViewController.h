//
//  SharingViewController.h
//  PHR
//
//  Created by CDAC HIED on 29/12/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface SharingViewController : UIViewController<UIPickerViewDataSource, UIPickerViewDelegate>

@property (nonatomic,readwrite) BOOL isFromDashboard;

@end
