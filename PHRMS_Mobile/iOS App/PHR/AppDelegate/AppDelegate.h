//
//  AppDelegate.h
//  PHR
//
//  Created by CDAC HIED on 12/10/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <UserNotifications/UserNotifications.h>

@interface AppDelegate : UIResponder <UIApplicationDelegate, UNUserNotificationCenterDelegate>{
    UIActivityIndicatorView* loadingIndicator;
    
    UIView* viewLoading;
    UILabel* lblLoadingStatus;
    
    NSMutableArray* notificationArray;
}

-(void)showNetworkAlert;
-(BOOL)hasInternetConnection;
-(void)showAlertView:(NSString*)alertMessage;
-(void)showLoadingIndicator:(NSString*)loadingStatus;
-(void)hideLoadingIndicator;
-(NSString*)checkDeviceType;
-(UIImage*)scaleAndRotateImage:(UIImage *)image;
-(void)tokenRefreshNotification:(NSNotification *)notification;

@property (strong, nonatomic) UIWindow *window;
@property (readwrite ,strong) NSString* allergyNameButtonString;
@property (readwrite ,strong) NSString* allergyNameID;
@property (readwrite ,strong) NSString* healthProblemNameButtonString;
@property (readwrite ,strong) NSString* healthProblemNameID;
@property (readwrite ,strong) NSString* immunizationNameButtonString;
@property (readwrite ,strong) NSString* immunizationNameID;
@property (readwrite ,strong) NSString* medicationNameButtonString;
@property (readwrite ,strong) NSString* medicationNameID;
@property (readwrite ,strong) NSString* procedureNameButtonString;
@property (readwrite ,strong) NSString* procedureNameID;
@property (readwrite ,strong) NSString* labTestNameButtonString;
@property (readwrite ,strong) NSString* labTestNameID;
@property (readwrite) int isUserProfileUpdated;
@property (readwrite) int dashboardCount;
@property (readwrite) NSString* strOTPID;
@property (readwrite) NSString* strRegistrationMobileNo;

//@property (readwrite) BOOL comeFromInsertion;

@end

