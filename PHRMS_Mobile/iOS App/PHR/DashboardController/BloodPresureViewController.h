//
//  BloodPresureViewController.h
//  PHR
//
//  Created by CDAC HIED on 22/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface BloodPresureViewController : UIViewController

@property (nonatomic,readwrite) BOOL isFromDashboard;
@property (nonatomic,readwrite) NSMutableArray* bloodPressureArray;

@end
