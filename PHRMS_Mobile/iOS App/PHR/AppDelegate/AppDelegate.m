//
//  AppDelegate.m
//  PHR
//
//  Created by CDAC HIED on 12/10/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AppDelegate.h"
#import "Constants.h"
#import "SWRevealViewController.h"
#import <UserNotifications/UserNotifications.h>
//#import <FirebaseMessaging/FirebaseMessaging.h>
//#import <FirebaseInstanceID/FirebaseInstanceID.h>
//#import <FirebaseCore/FirebaseCore.h>
//#import <FirebaseAnalytics/FirebaseAnalytics.h>

#define SYSTEM_VERSION_GRATERTHAN_OR_EQUALTO(v)  ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] != NSOrderedAscending)
@import Firebase;
@import FirebaseInstanceID;
@import FirebaseMessaging;

@interface AppDelegate () <FIRMessagingDelegate>

@end

@implementation AppDelegate
NSString *const kGCMMessageIDKey = @"gcm.message_id";

@synthesize allergyNameButtonString, allergyNameID, healthProblemNameButtonString, healthProblemNameID, immunizationNameButtonString, immunizationNameID, medicationNameButtonString, medicationNameID, procedureNameButtonString, procedureNameID, labTestNameButtonString, labTestNameID, isUserProfileUpdated, dashboardCount, strOTPID, strRegistrationMobileNo;


- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    // Override point for customization after application launch.
    sleep(1);
    
    [self loadingIndicatorCreationMethod];
    
    self.dashboardCount = 0;
    self.strOTPID = @"";
    
    UIStoryboard *storyboard;
    if ([[self checkDeviceType] isEqualToString:iPad]) {
        storyboard = [UIStoryboard storyboardWithName:@"Main-iPad" bundle: nil];
    }
    else{
        storyboard = [UIStoryboard storyboardWithName:@"Main" bundle: nil];
    }
    
    if ([[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]!=nil) {
        
        SWRevealViewController*viewController = [storyboard instantiateViewControllerWithIdentifier:@"signInCompleteID"];
        
        [[kAppDelegate window] setRootViewController:viewController];
    }
    else{
        LoginController *viewController = [storyboard instantiateViewControllerWithIdentifier:@"LoginController"];
        
        [[kAppDelegate window] setRootViewController:viewController];
    }
    
    // Register for remote notifications. This shows a permission dialog on first run, to
    // show the dialog at a more appropriate time move this registration accordingly.
    if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_7_1) {
        // iOS 7.1 or earlier. Disable the deprecation warnings.
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Wdeprecated-declarations"
        UIRemoteNotificationType allNotificationTypes =
        (UIRemoteNotificationTypeSound |
         UIRemoteNotificationTypeAlert |
         UIRemoteNotificationTypeBadge);
        [application registerForRemoteNotificationTypes:allNotificationTypes];
#pragma clang diagnostic pop
    } else {
        // iOS 8 or later
        // [START register_for_notifications]
        if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_9_x_Max) {
            UIUserNotificationType allNotificationTypes =
            (UIUserNotificationTypeSound | UIUserNotificationTypeAlert | UIUserNotificationTypeBadge);
            UIUserNotificationSettings *settings =
            [UIUserNotificationSettings settingsForTypes:allNotificationTypes categories:nil];
            [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
        } else {
            // iOS 10 or later
#if defined(__IPHONE_10_0) && __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
            UNAuthorizationOptions authOptions =
            UNAuthorizationOptionAlert
            | UNAuthorizationOptionSound
            | UNAuthorizationOptionBadge;
            [[UNUserNotificationCenter currentNotificationCenter] requestAuthorizationWithOptions:authOptions completionHandler:^(BOOL granted, NSError * _Nullable error) {
            }];
            
            // For iOS 10 display notification (sent via APNS)
            [UNUserNotificationCenter currentNotificationCenter].delegate = self;
            // For iOS 10 data message (sent via FCM)
//            [FIRMessaging messaging].remoteMessageDelegate = self;
#endif
        }
        
        [[UIApplication sharedApplication] registerForRemoteNotifications];
        // [END register_for_notifications]
    }
    
//     [START configure_firebase]
    [FIRApp configure];
    // [END configure_firebase]
    // [START add_token_refresh_observer]
    // Add observer for InstanceID token refresh callback.
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(tokenRefreshNotification:)name:kFIRInstanceIDTokenRefreshNotification object:nil];
    // [END add_token_refresh_observer]
    
    /*if([UIApplication instancesRespondToSelector:@selector(registerUserNotificationSettings:)]) {
        
        [[UIApplication sharedApplication] registerForRemoteNotifications];
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeBadge | UIUserNotificationTypeSound | UIUserNotificationTypeAlert) categories:nil];
        [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
    }
    
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];

    if (![[NSUserDefaults standardUserDefaults] boolForKey:NotificationON]) {
        
        [[NSUserDefaults standardUserDefaults] setBool:YES forKey:NotificationON];
        notificationArray = [NSMutableArray new];
        
        NSString *routeFilePath = [[NSBundle mainBundle] pathForResource:@"HealthTip" ofType:@"plist"];
        notificationArray = [[NSMutableArray alloc] initWithContentsOfFile:routeFilePath];
        
        [self localNotificationHealthTip];
    }*/
    
    return YES;
}

#pragma mark Local notification delegates 
-(void)localNotificationHealthTip{
    
    for (int i=0; i<[notificationArray count]; i++) {
        
        NSDateFormatter *dateTimeformatter = [[NSDateFormatter alloc]init];
        [dateTimeformatter setDateFormat:@"MM/dd/yyyy hh:mm a"];
        
        NSDateComponents *dayComponent = [[NSDateComponents alloc] init];
        dayComponent.day = i;
        
        NSString* dateString = [dateTimeformatter stringFromDate:[NSDate date]];
        
        NSArray* foo = [dateString componentsSeparatedByString: @" "];
        NSString* day = [foo objectAtIndex: 0];
        
        NSCalendar *theCalendar = [NSCalendar currentCalendar];
        NSDate *nextDate = [theCalendar dateByAddingComponents:dayComponent toDate:[dateTimeformatter dateFromString:[NSString stringWithFormat:@"%@ 02:42 PM",day]] options:0];
        
        UILocalNotification* alarm = [[UILocalNotification alloc] init];
        alarm.fireDate = nextDate;
        alarm.alertBody = [NSString stringWithFormat:@"Today's Health Tip: %@",[[notificationArray objectAtIndex:i] valueForKey:@"Tip"]];
        alarm.soundName = UILocalNotificationDefaultSoundName;
        alarm.timeZone = [NSTimeZone localTimeZone];
        
        NSDictionary *infoDict = [NSDictionary dictionaryWithObject:[notificationArray objectAtIndex:i] forKey:@"Time"];
        alarm.userInfo = infoDict;
        
        [[UIApplication sharedApplication] scheduleLocalNotification:alarm];
    }
}

#pragma mark APNS Delegates 

// [START receive_message]
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
    // If you are receiving a notification message while your app is in the background,
    // this callback will not be fired till the user taps on the notification launching the application.
     // TODO: Handle data of notification
    
    // Print message ID.
    if (userInfo[kGCMMessageIDKey]) {
        NSLog(@"Message ID: %@", userInfo[kGCMMessageIDKey]);
    }
    
    // Print full message.
    NSLog(@"%@", userInfo);
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo
fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler {
    // If you are receiving a notification message while your app is in the background,
    // this callback will not be fired till the user taps on the notification launching the application.
    // TODO: Handle data of notification
    
    // Print message ID.
    if (userInfo[kGCMMessageIDKey]) {
        NSLog(@"Message ID: %@", userInfo[kGCMMessageIDKey]);
    }
    
    // Print full message.
    NSLog(@"%@", userInfo);
    
    completionHandler(UIBackgroundFetchResultNewData);
}
// [END receive_message]

// [START ios_10_message_handling]
// Receive displayed notifications for iOS 10 devices.
#if defined(__IPHONE_10_0) && __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
// Handle incoming notification messages while app is in the foreground.
- (void)userNotificationCenter:(UNUserNotificationCenter *)center
       willPresentNotification:(UNNotification *)notification
         withCompletionHandler:(void (^)(UNNotificationPresentationOptions))completionHandler {
    // Print message ID.
    NSDictionary *userInfo = notification.request.content.userInfo;
    if (userInfo[kGCMMessageIDKey]) {
        NSLog(@"Message ID: %@", userInfo[kGCMMessageIDKey]);
    }
    
    // Print full message.
    NSLog(@"%@", userInfo);
    NSString* strNotificationText = [userInfo valueForKey:@"message"];
    
    [kAppDelegate showAlertView:strNotificationText];
    
    // Change this to your preferred presentation option
    completionHandler(UNNotificationPresentationOptionNone);
}

// Handle notification messages after display notification is tapped by the user.
- (void)userNotificationCenter:(UNUserNotificationCenter *)center
didReceiveNotificationResponse:(UNNotificationResponse *)response
         withCompletionHandler:(void (^)())completionHandler {
    NSDictionary *userInfo = response.notification.request.content.userInfo;
    if (userInfo[kGCMMessageIDKey]) {
        NSLog(@"Message ID: %@", userInfo[kGCMMessageIDKey]);
    }
    
    // Print full message.
    NSLog(@"%@", userInfo);
    
    NSString* strNotificationText = [userInfo valueForKey:@"message"];
    
    [kAppDelegate showAlertView:strNotificationText];
    
    completionHandler();
}
#endif
// [END ios_10_message_handling]

// [START ios_10_data_message_handling]
#if defined(__IPHONE_10_0) && __IPHONE_OS_VERSION_MAX_ALLOWED >= __IPHONE_10_0
// Receive data message on iOS 10 devices while app is in the foreground.
- (void)applicationReceivedRemoteMessage:(FIRMessagingRemoteMessage *)remoteMessage {
    // Print full message
    NSLog(@"testing notification %@", remoteMessage.appData);
    NSDictionary* dic = remoteMessage.appData;
}
#endif
// [END ios_10_data_message_handling]

// [START refresh_token]
- (void)tokenRefreshNotification:(NSNotification *)notification {
    // Note that this callback will be fired everytime a new token is generated, including the first
    // time. So if you need to retrieve the token as soon as it is available this is where that
    // should be done.
    NSString *refreshedToken = [[FIRInstanceID instanceID] token];
    NSLog(@"InstanceID token: %@", refreshedToken);
    
    // Connect to FCM since connection may have failed when attempted before having a token.
    [self connectToFcm];
    
    // TODO: If necessary send token to application server.
}
// [END refresh_token]

// [START connect_to_fcm]
- (void)connectToFcm {
    // Won't connect since there is no token
    if (![[FIRInstanceID instanceID] token]) {
        return;
    }
    
    // Disconnect previous FCM connection if it exists.
    [[FIRMessaging messaging] disconnect];
    
    [[FIRMessaging messaging] connectWithCompletion:^(NSError * _Nullable error) {
        if (error != nil) {
            NSLog(@"Unable to connect to FCM. %@", error);
        } else {
            NSLog(@"Connected to FCM.");
        }
    }];
}
// [END connect_to_fcm]

- (void)application:(UIApplication *)application didFailToRegisterForRemoteNotificationsWithError:(NSError *)error {
    NSLog(@"Unable to register for remote notifications: %@", error);
}

// This function is added here only for debugging purposes, and can be removed if swizzling is enabled.
// If swizzling is disabled then this function must be implemented so that the APNs token can be paired to
// the InstanceID token.
- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
    NSLog(@"APNs token retrieved: %@", deviceToken);
    
    // With swizzling disabled you must set the APNs token here.
//     [[FIRInstanceID instanceID] setAPNSToken:deviceToken type:FIRInstanceIDAPNSTokenTypeSandbox];
}


/*- (void)registerForRemoteNotifications {
    if(SYSTEM_VERSION_GRATERTHAN_OR_EQUALTO(@"10.0")){
        UNUserNotificationCenter *center = [UNUserNotificationCenter currentNotificationCenter];
        center.delegate = self;
        [center requestAuthorizationWithOptions:(UNAuthorizationOptionSound | UNAuthorizationOptionAlert | UNAuthorizationOptionBadge) completionHandler:^(BOOL granted, NSError * _Nullable error){
            if(!error){
                [[UIApplication sharedApplication] registerForRemoteNotifications];
            }
        }];
    }
    else {
        // Code for old versions
        
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeBadge | UIUserNotificationTypeSound | UIUserNotificationTypeAlert) categories:nil];
        [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
    }
}

- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
    
//    [MMDevice updateCurentDeviceToken:deviceToken success:^{
//        NSLog(@"Device token updated.");
//    } failure:^(NSError * error) {
//        NSLog(@"Device token error = %@",error);
//    }];
}

//Called when a notification is delivered to a foreground app.
-(void)userNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(UNNotificationPresentationOptions options))completionHandler{
    NSLog(@"User Info : %@",notification.request.content.userInfo);
    completionHandler(UNAuthorizationOptionSound | UNAuthorizationOptionAlert | UNAuthorizationOptionBadge);
}

//Called to let your app know which action was selected by the user for a given notification.
-(void)userNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void(^)())completionHandler{
    NSLog(@"User Info : %@",response.notification.request.content.userInfo);
    completionHandler();
}

- (void)application:(UIApplication *)application didFailToRegisterForRemoteNotificationsWithError:(NSError *)error {
    NSLog(@"didFailToRegisterForRemoteNotificationsWithError = %@",error.localizedDescription);
}

- (void)application:(UIApplication *)application didRegisterUserNotificationSettings:(UIUserNotificationSettings *)notificationSettings {
    //register to receive notifications
    [application registerForRemoteNotifications];
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler {
    //In the case of a silent notification use the following code to see if it is a wakeup notification
//    if ([MMXRemoteNotification isWakeupRemoteNotification:userInfo]) {
//        //Send local notification to the user or connect via MMXUser logInWithCredential:success:failure:
//        completionHandler(UIBackgroundFetchResultNewData);
//    } else if ([MMXRemoteNotification isMMXRemoteNotification:userInfo]) {
//        NSLog(@"userInfo = %@",userInfo);
//        //Check if the message is designed to wake up the client
//        [MMXRemoteNotification acknowledgeRemoteNotification:userInfo completion:^(BOOL success) {
//            completionHandler(UIBackgroundFetchResultNewData);
//        }];
//    } else {
//        completionHandler(UIBackgroundFetchResultNoData);
//    }
}

- (void)application:(UIApplication *)application didReceiveLocalNotification:(UILocalNotification *)notification {
    //    UIApplicationState state = [application applicationState];
    //    if (state == UIApplicationStateActive) {
    //        ;
    //    }
    //    UILocalNotification *localNotif = [notification object];
    //    NSDictionary *userInfo = [notification userInfo];
    //    NSString* beaconUUID = [userInfo objectForKey:@"Med_Name"];
}*/

#pragma mark Camera Image Rotation 
- (UIImage*)scaleAndRotateImage:(UIImage *)image
{
    int kMaxResolution = 320; // Or whatever
    
    CGImageRef imgRef = image.CGImage;
    
    CGFloat width = CGImageGetWidth(imgRef);
    CGFloat height = CGImageGetHeight(imgRef);
    
    CGAffineTransform transform = CGAffineTransformIdentity;
    CGRect bounds = CGRectMake(0, 0, width, height);
    if (width > kMaxResolution || height > kMaxResolution) {
        CGFloat ratio = width/height;
        if (ratio > 1) {
            bounds.size.width = kMaxResolution;
            bounds.size.height = bounds.size.width / ratio;
        }
        else {
            bounds.size.height = kMaxResolution;
            bounds.size.width = bounds.size.height * ratio;
        }
    }
    
    CGFloat scaleRatio = bounds.size.width / width;
    CGSize imageSize = CGSizeMake(CGImageGetWidth(imgRef), CGImageGetHeight(imgRef));
    CGFloat boundHeight;
    UIImageOrientation orient = image.imageOrientation;
    switch(orient) {
            
        case UIImageOrientationUp: //EXIF = 1
            transform = CGAffineTransformIdentity;
            break;
            
        case UIImageOrientationUpMirrored: //EXIF = 2
            transform = CGAffineTransformMakeTranslation(imageSize.width, 0.0);
            transform = CGAffineTransformScale(transform, -1.0, 1.0);
            break;
            
        case UIImageOrientationDown: //EXIF = 3
            transform = CGAffineTransformMakeTranslation(imageSize.width, imageSize.height);
            transform = CGAffineTransformRotate(transform, M_PI);
            break;
            
        case UIImageOrientationDownMirrored: //EXIF = 4
            transform = CGAffineTransformMakeTranslation(0.0, imageSize.height);
            transform = CGAffineTransformScale(transform, 1.0, -1.0);
            break;
            
        case UIImageOrientationLeftMirrored: //EXIF = 5
            boundHeight = bounds.size.height;
            bounds.size.height = bounds.size.width;
            bounds.size.width = boundHeight;
            transform = CGAffineTransformMakeTranslation(imageSize.height, imageSize.width);
            transform = CGAffineTransformScale(transform, -1.0, 1.0);
            transform = CGAffineTransformRotate(transform, 3.0 * M_PI / 2.0);
            break;
            
        case UIImageOrientationLeft: //EXIF = 6
            boundHeight = bounds.size.height;
            bounds.size.height = bounds.size.width;
            bounds.size.width = boundHeight;
            transform = CGAffineTransformMakeTranslation(0.0, imageSize.width);
            transform = CGAffineTransformRotate(transform, 3.0 * M_PI / 2.0);
            break;
            
        case UIImageOrientationRightMirrored: //EXIF = 7
            boundHeight = bounds.size.height;
            bounds.size.height = bounds.size.width;
            bounds.size.width = boundHeight;
            transform = CGAffineTransformMakeScale(-1.0, 1.0);
            transform = CGAffineTransformRotate(transform, M_PI / 2.0);
            break;
            
        case UIImageOrientationRight: //EXIF = 8
            boundHeight = bounds.size.height;
            bounds.size.height = bounds.size.width;
            bounds.size.width = boundHeight;
            transform = CGAffineTransformMakeTranslation(imageSize.height, 0.0);
            transform = CGAffineTransformRotate(transform, M_PI / 2.0);
            break;
            
        default:
            [NSException raise:NSInternalInconsistencyException format:@"Invalid image orientation"];
            
    }
    
    UIGraphicsBeginImageContext(bounds.size);
    
    CGContextRef context = UIGraphicsGetCurrentContext();
    
    if (orient == UIImageOrientationRight || orient == UIImageOrientationLeft) {
        CGContextScaleCTM(context, -scaleRatio, scaleRatio);
        CGContextTranslateCTM(context, -height, 0);
    }
    else {
        CGContextScaleCTM(context, scaleRatio, -scaleRatio);
        CGContextTranslateCTM(context, 0, -height);
    }
    
    CGContextConcatCTM(context, transform);
    
    CGContextDrawImage(UIGraphicsGetCurrentContext(), CGRectMake(0, 0, width, height), imgRef);
    UIImage *imageCopy = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    //    [self setRotatedImage:imageCopy];
    return imageCopy;
}

#pragma mark Show AlertView Method
-(void)showAlertView:(NSString*)alertMessage
{
    [[[UIAlertView alloc]initWithTitle:kAppTitle message:alertMessage delegate:nil cancelButtonTitle:kOKButtonText otherButtonTitles:nil, nil]show];
}

#pragma mark Network check methods 
-(void)showNetworkAlert
{
    [[[UIAlertView alloc] initWithTitle:kAppTitle message:@"No Network Available" delegate:nil cancelButtonTitle:kOKButtonText otherButtonTitles:nil] show];
}

-(BOOL)hasInternetConnection{
    
    NetworkStatus netStatus = [[Reachability reachabilityForInternetConnection]
                               currentReachabilityStatus];
    BOOL isInternetActive = YES;
    if(netStatus == NotReachable){
        isInternetActive = NO;
    }
    return isInternetActive;
}

#pragma mark - Check iOS Device Model 
-(NSString*)checkDeviceType{
    if(UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPhone)
    {
        //its iPhone. Find out which one?
        
        NSString* str = @"";
        CGSize result = [[UIScreen mainScreen] bounds].size;
        if(result.height == 480)
        {
            str = iPhone4;
        }
        else if(result.height == 568)
        {
            str = iPhone5;
        }
        else if(result.height == 667)
        {
            str = iPhone6;
            str = iPhone;
        }
        else if(result.height == 736)
        {
            str = iPhone6Plus;
            str = iPhone;
        }
        return str;
    }
    else
    {
        return iPad;
        //its iPad
    }
    return @"";
}

#pragma mark - Loading Indicator 
-(void)loadingIndicatorCreationMethod
{
    lblLoadingStatus = [[UILabel alloc]init];
    
    CGRect viewframe;
    CGRect indicatorframe;
    CGRect textframe;
    if ([[self checkDeviceType] isEqualToString:iPhone4]) {
        viewframe = CGRectMake(100,[[UIScreen mainScreen]bounds].size.height/2-30, 90.0, 80.0);
        indicatorframe = CGRectMake(25,18,100,50);
        textframe = CGRectMake(15, 70, 120, 20);
        lblLoadingStatus.font = [UIFont systemFontOfSize:10.0f];
    }
    else if ([[self checkDeviceType] isEqualToString:iPhone5]) {
        viewframe = CGRectMake(110,[[UIScreen mainScreen]bounds].size.height/2-30, 110.0, 100.0);
        indicatorframe = CGRectMake(5,18,100,50);
        textframe = CGRectMake(0, 70, 120, 20);
        lblLoadingStatus.font = [UIFont systemFontOfSize:12.0f];
    }
    else if([[self checkDeviceType] isEqualToString:iPhone6]){
        viewframe = CGRectMake(125,[[UIScreen mainScreen]bounds].size.height/2-30, 110.0, 100.0);
        indicatorframe = CGRectMake(25,18,100,50);
        textframe = CGRectMake(15, 70, 120, 20);
    }
    else if([[self checkDeviceType] isEqualToString:iPhone6Plus]){
        viewframe = CGRectMake(150,[[UIScreen mainScreen]bounds].size.height/2-30, 120.0, 110.0);
        indicatorframe = CGRectMake(25,18,100,50);
        textframe = CGRectMake(15, 70, 120, 20);
    }
    else{
        viewframe = CGRectMake([[UIScreen mainScreen]bounds].size.width/2-75,[[UIScreen mainScreen]bounds].size.height/2, 150.0, 120.0);
        indicatorframe = CGRectMake(25,18,100,50);
        textframe = CGRectMake(15, 70, 120, 20);
    }
    
    viewLoading = [[UIView alloc] initWithFrame:viewframe];
    [viewLoading setBackgroundColor:[UIColor blackColor]];
    [viewLoading setAlpha:0.8];
    [[viewLoading layer] setCornerRadius:10.0];
    [viewLoading setHidden:YES];
    [self.window addSubview:viewLoading];
    
    loadingIndicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
    [loadingIndicator setFrame:indicatorframe];
    [loadingIndicator startAnimating];
    [viewLoading addSubview:loadingIndicator];
    
    [lblLoadingStatus setFrame:textframe];
    [lblLoadingStatus setTextColor:[UIColor whiteColor]];
    [lblLoadingStatus setTextAlignment:NSTextAlignmentCenter];
    [viewLoading addSubview:lblLoadingStatus];
    
}

-(void)showLoadingIndicator:(NSString*)loadingStatus
{
    [self.window bringSubviewToFront:viewLoading];
    [self.window bringSubviewToFront:loadingIndicator];
    [self.window setUserInteractionEnabled:NO];
    
    [lblLoadingStatus setText:loadingStatus];
    [loadingIndicator setHidden:NO];
    [viewLoading setHidden:NO];
}

-(void)hideLoadingIndicator
{
    //test
    [self.window setUserInteractionEnabled:YES];
    [loadingIndicator setHidden:YES];
    [viewLoading setHidden:YES];
}

- (void)applicationWillResignActive:(UIApplication *)application {
    // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
    // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
}

- (void)applicationDidEnterBackground:(UIApplication *)application {
    // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
    // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
}

- (void)applicationWillEnterForeground:(UIApplication *)application {
    // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
}

- (void)applicationDidBecomeActive:(UIApplication *)application {
    // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
}

- (void)applicationWillTerminate:(UIApplication *)application {
    // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
}

@end
