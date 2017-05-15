//
//  ProceduresDetailViewController.h
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ProceduresDetailViewController : UIViewController<UIImagePickerControllerDelegate, UINavigationControllerDelegate, UIGestureRecognizerDelegate>

@property (nonatomic, readwrite)NSMutableArray* proceduresDataArray;
//@property (nonatomic, readwrite)int indexNumber;

@end
