//
//  MedicationDetailViewController.h
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface MedicationDetailViewController : UIViewController<UIGestureRecognizerDelegate, UIImagePickerControllerDelegate, UINavigationControllerDelegate>

- (void)imageDidTouch:(UIGestureRecognizer *)recognizer;

@property (nonatomic, readwrite)NSMutableArray* medicationDataArray;
//@property (nonatomic, readwrite)int indexNumber;

@end
