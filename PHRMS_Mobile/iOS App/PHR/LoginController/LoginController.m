//
//  ViewController.m
//  PHR
//
//  Created by CDAC HIED on 12/10/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "LoginController.h"
#import "Constants.h"
#import "SWRevealViewController.h"
#import <CommonCrypto/CommonDigest.h>
#import <LocalAuthentication/LocalAuthentication.h>
#import "GSHealthKitManager.h"
#import "SignUpViewController.h"

@interface LoginController (){
    CAGradientLayer* gradient;
    BOOL isNeedToSaveUDID;
}
@property (weak, nonatomic) IBOutlet UILabel *logoLabel;
@property (weak, nonatomic) IBOutlet UIButton *loginButton;
@property (weak, nonatomic) IBOutlet UIButton *signUpButton;
@property (weak, nonatomic) IBOutlet UITextField *emailTextField;
@property (weak, nonatomic) IBOutlet UITextField *passwordTextField;
@property (weak, nonatomic) IBOutlet UILabel *allRightLabel;
@property (weak, nonatomic) IBOutlet UIScrollView *loginScrollView;

- (IBAction)loginButtonAction:(id)sender;
- (IBAction)signUpButtonAction:(id)sender;
- (IBAction)loginWithFacebookButtonAction:(id)sender;
- (IBAction)loginWithGoogleButtonAction:(id)sender;

@end

@import Firebase;
@import FirebaseInstanceID;
@import FirebaseMessaging;

@implementation LoginController

-(void) viewDidLayoutSubviews{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            [self.loginScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+300)];
        }
        else{
            [self.loginScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+50)];
        }
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    
    [self.navigationController setNavigationBarHidden:YES];
    
//    _emailTextField.text = @"love.dhaka18@gmail.com";
//    _passwordTextField.text = @"123456";
    
    if (![[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
            self.logoLabel.font = [UIFont systemFontOfSize:26.0f];
            self.allRightLabel.font = [UIFont systemFontOfSize:12.0f weight:-1];
            self.loginButton.titleLabel.font = [UIFont systemFontOfSize:24.0f weight:-1];
            self.signUpButton.titleLabel.font = [UIFont systemFontOfSize:24.0f weight:-1];
            self.emailTextField.font = [UIFont systemFontOfSize:18.0f weight:-1];
            self.passwordTextField.font = [UIFont systemFontOfSize:18.0f weight:-1];
        }
        else{
            self.logoLabel.font = [UIFont systemFontOfSize:32.0f];
            self.allRightLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
//            self.loginButton.titleLabel.font = [UIFont systemFontOfSize:26.0f weight:-1];
        }
    }
    else{
        UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                                   initWithTarget:self action:@selector(handleSingleTap)];
        
        singleFingerTap.numberOfTapsRequired = 1;
        
        [self.loginScrollView addGestureRecognizer:singleFingerTap];
    }
    
    self.loginButton.backgroundColor = [UIColor colorWithRed:92.0f/255.0f green:184.0f/255.0f blue:92.0f/255.0f alpha:1.0f];
    
    self.signUpButton.backgroundColor = [UIColor colorWithRed:51.0f/255.0f green:122.0f/255.0f blue:183.0f/255.0f alpha:1.0f];
    
//    [self GetUserID];
    
    gradient = [CAGradientLayer layer];
    gradient.frame = [[UIScreen mainScreen]bounds];
    gradient.colors = [NSArray arrayWithObjects:(id)[[UIColor colorWithRed:40.0f/255.0f green:44.0f/255.0f blue:75.0f/255.0f alpha:1.0f] CGColor], (id)[[UIColor colorWithRed:15.0f/255.0f green:20.0f/255.0f blue:27.0f/255.0f alpha:1.0f] CGColor], nil];
//    [self.view.layer insertSublayer:gradient atIndex:0];
    
//    UIColor *color = [UIColor lightGrayColor];
//    self.emailTextField.attributedPlaceholder = [[NSAttributedString alloc] initWithString:@"Email or Mobile No." attributes:@{NSForegroundColorAttributeName: color,
//          NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1],}];
//    self.emailTextField.font = [UIFont systemFontOfSize:30 weight:-1];
//    
//    self.passwordTextField.attributedPlaceholder = [[NSAttributedString alloc]initWithString:@"Password" attributes:@{NSForegroundColorAttributeName: color,
//    NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1],}];
//    self.passwordTextField.font = [UIFont systemFontOfSize:30 weight:-1];
    
//    NSString* uniqueIdentifier = [[[UIDevice currentDevice] identifierForVendor] UUIDString]; // IOS 6+
//    NSLog(@"UDID:: %@", uniqueIdentifier);
    //    21344ec5b7ff8bdfe7a066c1a9a4cb1561e66fce
    
    
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
//    BOOL login = [[NSUserDefaults standardUserDefaults] boolForKey:firstTimeLogin];
    isNeedToSaveUDID = NO;
    
    if ([[NSUserDefaults standardUserDefaults] boolForKey:firstTimeLogin]) {
//        [[NSUserDefaults standardUserDefaults] setBool:YES forKey:firstTimeLogin];
        
        LAContext *context = [[LAContext alloc] init];
        
        NSError *error = nil;
        if ([context canEvaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics error:&error]) {
            [context evaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics
                    localizedReason:@"Login using Apple TouchID"
                              reply:^(BOOL success, NSError *error) {
                                  
                                  if (error) {
                                      
                                      UIAlertController * alert=   [UIAlertController
                                                                    alertControllerWithTitle:@"Error"
                                                                    message:@"There was a problem verifying your identity."
                                                                    preferredStyle:UIAlertControllerStyleAlert];
                                      
                                      UIAlertAction* yesButton = [UIAlertAction
                                                                  actionWithTitle:@"OK"
                                                                  style:UIAlertActionStyleDefault
                                                                  handler:^(UIAlertAction * action)
                                                                  {
                                                                      //Handel your yes please button action here
                                                                      
                                                                      
                                                                  }];
                                      
                                      [alert addAction:yesButton];
                                      
//                                      [self presentViewController:alert animated:YES completion:nil];
                                      
                                      return;
                                  }
                                  
                                  if (success) {
                                      
                                      dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_BACKGROUND, 0), ^{
                                          dispatch_async(dispatch_get_main_queue(), ^{
                                              [self GetUserID];
                                          });
                                      });
                                      
                                      UIAlertController * alert=   [UIAlertController
                                                                    alertControllerWithTitle:@"Success"
                                                                    message:@"You are the device owner!"
                                                                    preferredStyle:UIAlertControllerStyleAlert];
                                      
                                      UIAlertAction* yesButton = [UIAlertAction
                                                                  actionWithTitle:@"OK"
                                                                  style:UIAlertActionStyleDefault
                                                                  handler:^(UIAlertAction * action)
                                                                  {
                                                                      //Handel your yes please button action here
                                                                      
                                                                      
                                                                  }];
                                      
                                      [alert addAction:yesButton];
                                      
                                      //                  [self presentViewController:alert animated:YES completion:nil];
                                      
                                  } else {
                                      
                                      UIAlertController * alert=   [UIAlertController
                                                                    alertControllerWithTitle:@"Error"
                                                                    message:@"You are not the device owner."
                                                                    preferredStyle:UIAlertControllerStyleAlert];
                                      
                                      UIAlertAction* yesButton = [UIAlertAction
                                                                  actionWithTitle:@"OK"
                                                                  style:UIAlertActionStyleDefault
                                                                  handler:^(UIAlertAction * action)
                                                                  {
                                                                      //Handel your yes please button action here
                                                                      
                                                                  }];
                                      
                                      [alert addAction:yesButton];
                                      
                                      //                  [self presentViewController:alert animated:YES completion:nil];
                                      
                                  }
                                  
                              }];
            
        } else {
            
            UIAlertController * alert=   [UIAlertController
                                          alertControllerWithTitle:@"Error"
                                          message:@"Your device cannot authenticate using TouchID."
                                          preferredStyle:UIAlertControllerStyleAlert];
            
            UIAlertAction* yesButton = [UIAlertAction
                                        actionWithTitle:@"OK"
                                        style:UIAlertActionStyleDefault
                                        handler:^(UIAlertAction * action)
                                        {
                                            //Handel your yes please button action here
                                            
                                            
                                        }];
            
            [alert addAction:yesButton];
            
            //            [self presentViewController:alert animated:YES completion:nil];
        }
    }
    else{
        [[NSUserDefaults standardUserDefaults] setBool:YES forKey:firstTimeLogin];
        
        [[GSHealthKitManager sharedManager] requestAuthorization];
    }
    
//    UIStoryboard *storyboard;
//    if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
//        storyboard = [UIStoryboard storyboardWithName:@"Main-iPad" bundle: nil];
//        
//        LoginController *viewController = [storyboard instantiateViewControllerWithIdentifier:@"LoginController"];
//    }
}

//#pragma mark Device Orientation Method 
//-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
//    
//    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
//    
//    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
//        [_logoLabel setFont:[UIFont fontWithName:@"SystemThin" size:45.0f]];
//    }
//    else{
//        [_logoLabel setFont:[UIFont fontWithName:@"SystemThin" size:35.0f]];
//    }
//}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            [self.loginScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+300)];
        }
        else{
            [self.loginScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+70)];
        }
    }
}

#pragma mark Check Numeric Value Method 
-(BOOL)isNumeric:(NSString*)inputString{
    
    BOOL valid;
    
    NSCharacterSet *alphaNums = [NSCharacterSet decimalDigitCharacterSet];
    NSCharacterSet *inStringSet = [NSCharacterSet characterSetWithCharactersInString:inputString];
    
    valid = [alphaNums isSupersetOfSet:inStringSet];
    return valid;
}

#pragma mark Email Validation Method 
-(BOOL)validEmail:(NSString*)myemail
{
    myemail=[myemail stringByReplacingOccurrencesOfString:@" " withString:@""];
    
    if([myemail length]<1)
        return NO;
    else
    {
        NSArray *mailParts=[myemail componentsSeparatedByString:@"@"];
        if(([mailParts count]>2) ||-([mailParts count]<2))
            return NO;
        else
        {
            NSString *lastPart=[mailParts objectAtIndex:[mailParts count]-1];
            NSArray *mailParts2=[lastPart componentsSeparatedByString:@"."];
            //NSRange isRange = [lastPart rangeOfString:@"." options:NSCaseInsensitiveSearch];
            if([lastPart rangeOfString:@".."].length) {
                return NO;
            }
            
            if([mailParts2 count]<2)
                return NO;
            else
            {
                NSString *lastPart2=[mailParts2 objectAtIndex:[mailParts2 count] -1];
                NSString *firstPart=[mailParts2 objectAtIndex:[mailParts2 count] -2];
                if([lastPart2 length]<1)
                    return NO;
                else if([firstPart length]<1)
                    return NO;
                else
                    return YES;
            }
        }
    }
    return NO;
}

#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
}

-(BOOL) textFieldShouldReturn:(UITextField *)textField{
    
    if (textField==self.passwordTextField) {
        [textField resignFirstResponder];
    }
    return YES;
}

#pragma mark Login Button Action Method 
- (IBAction)loginButtonAction:(id)sender {
    
    if (!self.emailTextField.text.length) {
        [kAppDelegate showAlertView:@"Please enter email or phone"];
        [self.emailTextField becomeFirstResponder];
    }
    else if ([self isNumeric:self.emailTextField.text] && _emailTextField.text.length!=10){
        [kAppDelegate showAlertView:@"Invalid phone number"];
    }
    else if (![self isNumeric:self.emailTextField.text] && ![self validEmail:self.emailTextField.text ]){
        [kAppDelegate showAlertView:@"Invalid email id"];
    }
    else if (!self.passwordTextField.text.length){
        [kAppDelegate showAlertView:@"Enter your password"];
        [self.passwordTextField becomeFirstResponder];
    }
    else{
        [self.view endEditing:YES];
        
        if ([self canAuthenticateByTouchId]) {
            if (![[NSUserDefaults standardUserDefaults] boolForKey:isTouchIDConfigured]) {
                
                if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
                    [[[UIAlertView alloc]initWithTitle:kAppTitle message:[NSString stringWithFormat:@"Do you want to integrate this device TouchID with %@ account",kAppTitle] delegate:self cancelButtonTitle:kCancelButtonText otherButtonTitles:kOKButtonText, nil]show];
                }
                else{
                    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:kAppTitle message:[NSString stringWithFormat:@"Do you want to integrate this device TouchID with %@ account",kAppTitle] preferredStyle:UIAlertControllerStyleActionSheet];
                    
                    UIAlertAction* okButton = [UIAlertAction actionWithTitle:@"OK" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
                        isNeedToSaveUDID = YES;
                        [self LoginAction];
                    }];
                    
                    UIAlertAction* cancelButton = [UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
                        isNeedToSaveUDID = NO;
                        [self LoginAction];
                    }];
                    
                    alertController.popoverPresentationController.barButtonItem = nil;
                    alertController.popoverPresentationController.sourceView = self.view;
                    alertController.popoverPresentationController.sourceRect = CGRectMake(self.view.bounds.size.width/2+50, self.view.bounds.size.height-210, 1.0, 1.0);
                    
                    [alertController addAction:okButton];
                    [alertController addAction:cancelButton];
                    
                    [alertController setModalPresentationStyle:UIModalPresentationPopover];
                    
                    [self presentViewController:alertController animated:YES completion:nil];
                }
            }
            else{
                [self LoginAction];
            }
        }else{
            [self LoginAction];
        }
    }
}

#pragma mark AlertView Delegate Method 
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    if (buttonIndex==1) {
        isNeedToSaveUDID = YES;
    }
    else{
        isNeedToSaveUDID = NO;
    }
    [self LoginAction];
}

#pragma mark Login Method 
-(void)LoginAction{
    
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Signing in..."];//Show loading indicator.
        
        //        //Login Parameter.
        //        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        //        [dicParams setObject:self.emailTextField.text forKey:@"name"];
        //        [dicParams setObject:self.passwordTextField.text forKey:@"pwd"];
        //        [dicParams setObject:@"2" forKey:@"AppID"];
        
        //        NSString *deviceToken =[kAppDelegate deviceToken];
        //        [dicParams setObject:deviceToken forKey:@"DeviceToken"];
        //        [dicParams setObject:[kAppDelegate latitude] forKey:@"latitude"];
        //        [dicParams setObject:[kAppDelegate longitude] forKey:@"longitude"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"login response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==0) {
                [kAppDelegate showAlertView:@"Incorrect username or password! Try again"];
                return;
            }
            
            NSString* userID = [[[responseObject valueForKey:@"response"] valueForKey:@"usersViewModel"] valueForKey:@"UserId"];
            NSString* firstName = [[[responseObject valueForKey:@"response"] valueForKey:@"userPersonalProfileViewModel"] valueForKey:@"FirstName"];
            NSString* lastName = [[[responseObject valueForKey:@"response"] valueForKey:@"userPersonalProfileViewModel"] valueForKey:@"LastName"];
            NSString* userName = [NSString stringWithFormat:@"%@ %@",firstName,lastName];
            
            NSString* emailID = [[[responseObject valueForKey:@"response"] valueForKey:@"usersViewModel"] valueForKey:@"Email"];
            
            if ([emailID isKindOfClass:[NSNull class]] || [emailID isEqualToString:@"<null>"] || [emailID isEqualToString:@""]) {
                emailID = @"";
            }
            
            NSString* userImage = [NSString stringWithFormat:@""];
            
            userImage = [userImage stringByReplacingOccurrencesOfString:@"\\"
                                                             withString:@"/"];
            
            [[NSUserDefaults standardUserDefaults] setValue:userID forKey:USERID];
            [[NSUserDefaults standardUserDefaults] setValue:userName forKey:USERNAME];
            [[NSUserDefaults standardUserDefaults] setValue:emailID forKey:USEREMAILID];
            [[NSUserDefaults standardUserDefaults] setValue:userImage forKey:USERIMAGE];
            
            //                NSError *error;
            //                NSData *jsonData = [NSJSONSerialization dataWithJSONObject:[responseObject valueForKey:@"response"]
            //                                                                options:0
            //                                                                     error:&error];
            //                NSString *jsonString;
            //                if (! jsonData) {
            //                    NSLog(@"Got an error: %@", error);
            //                } else {
            //                     jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            //                }
            //
            //                [[NSUserDefaults standardUserDefaults] setValue:jsonString forKey:USERPROFILE];
            
            UIStoryboard *storyboard;
            if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
                storyboard = [UIStoryboard storyboardWithName:@"Main-iPad" bundle: nil];
            }
            else{
                storyboard = [UIStoryboard storyboardWithName:@"Main" bundle: nil];
            }
            SWRevealViewController*viewController = [storyboard instantiateViewControllerWithIdentifier:@"signInCompleteID"];
            
            [[kAppDelegate window] setRootViewController:viewController];
            
            if ([self canAuthenticateByTouchId]) {
#if !(TARGET_IPHONE_SIMULATOR)
                if (isNeedToSaveUDID) {
                    [self SaveDeviceUDID];
                }
                NSLog(@"Running on device");
#endif
            }
            
            if (![[NSUserDefaults standardUserDefaults] boolForKey:FCMTokenRegistered]) {
                dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_BACKGROUND, 0), ^{
                    dispatch_async(dispatch_get_main_queue(), ^{
                        if ([[FIRInstanceID instanceID] token]) {
                            [self registerFCMTokenForNotification];
                        }
                    });
                });
            }
            
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"Login failed"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Register FCM Token For Notifications 
-(void)registerFCMTokenForNotification{
    if ([kAppDelegate hasInternetConnection]) {
        
//        [[NSNotificationCenter defaultCenter] postNotificationName:kFIRInstanceIDTokenRefreshNotification object:nil];
        NSString* strToken = [[FIRInstanceID instanceID] token];
        
        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        
        [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
        //            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
        
        [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
        
        manager.requestSerializer = requestSerializer;
        [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"]intValue] == 1) {
                [[NSUserDefaults standardUserDefaults] setBool:YES forKey:FCMTokenRegistered];
            }
        }
         failure:^(AFHTTPRequestOperation *operation, NSError *error) {
             NSLog(@"Error: %@", error);
         }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Check TouchID Availability 
- (BOOL)canAuthenticateByTouchId {
    if ([LAContext class]) {
        return [[[LAContext alloc] init] canEvaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics error:nil];
    }
    return NO;
}

#pragma mark Save UDID For TouchID Login 
-(void)SaveDeviceUDID{
    
    if ([kAppDelegate hasInternetConnection]) {

        NSString* uniqueIdentifier = [[[UIDevice currentDevice] identifierForVendor] UUIDString]; // IOS 6+
        
        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
    
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        
        [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
        //            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
        
        [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
        
        manager.requestSerializer = requestSerializer;
        [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
            
            NSLog(@"response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"Status"] integerValue]==0) {
//                [kAppDelegate showAlertView:@"Device token not saved!! TouchID"];
//                [[NSUserDefaults standardUserDefaults] setBool:NO forKey:firstTimeLogin];
                return;
            }
            
            [[NSUserDefaults standardUserDefaults] setBool:YES forKey:isTouchIDConfigured];
            
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
//            [[NSUserDefaults standardUserDefaults] setBool:NO forKey:firstTimeLogin];
//            [kAppDelegate showAlertView:@"failed!! while saving device token"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Get UserID On TouchID API Call 
-(void)GetUserID{
    
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Loading..."];//Show loading indicator
        
        NSString* uniqueIdentifier = [[[UIDevice currentDevice] identifierForVendor] UUIDString];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"Status"] integerValue]==0) {
                [kAppDelegate showAlertView:@"TouchID isn't configured with your account, login with your username/mobile and password"];
                //                    [[NSUserDefaults standardUserDefaults] setBool:NO forKey:firstTimeLogin];
                return;
            }
            
            NSString* userID = [[[responseObject valueForKey:@"response"] valueForKey:@"usersViewModel"] valueForKey:@"UserId"];
            NSString* firstName = [[[responseObject valueForKey:@"response"] valueForKey:@"userPersonalProfileViewModel"] valueForKey:@"FirstName"];
            NSString* lastName = [[[responseObject valueForKey:@"response"] valueForKey:@"userPersonalProfileViewModel"] valueForKey:@"LastName"];
            NSString* userName = [NSString stringWithFormat:@"%@ %@",firstName,lastName];
            
            NSString* emailID = [[[responseObject valueForKey:@"response"] valueForKey:@"usersViewModel"] valueForKey:@"Email"];
            NSString* userImage = [NSString stringWithFormat:@""];
            userImage = [userImage stringByReplacingOccurrencesOfString:@"\\"
                                                             withString:@"/"];
            
            [[NSUserDefaults standardUserDefaults] setValue:userID forKey:USERID];
            [[NSUserDefaults standardUserDefaults] setValue:userName forKey:USERNAME];
            [[NSUserDefaults standardUserDefaults] setValue:emailID forKey:USEREMAILID];
            [[NSUserDefaults standardUserDefaults] setValue:userImage forKey:USERIMAGE];
            
            if ([[FIRInstanceID instanceID] token]) {
                [self registerFCMTokenForNotification];
            }
            
            UIStoryboard *storyboard;
            if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
                storyboard = [UIStoryboard storyboardWithName:@"Main-iPad" bundle: nil];
            }
            else{
                storyboard = [UIStoryboard storyboardWithName:@"Main" bundle: nil];
            }
            
            SWRevealViewController*viewController = [storyboard instantiateViewControllerWithIdentifier:@"signInCompleteID"];
            
            [[kAppDelegate window] setRootViewController:viewController];
            
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"failed"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Sign Up Button Action Method 
- (IBAction)signUpButtonAction:(id)sender{
    
//    SignUpViewController* controller = [[SignUpViewController alloc] init];
//    [self showViewController:controller sender:nil];
}

#pragma mark Facebook Button Action Method 
- (IBAction)loginWithFacebookButtonAction:(id)sender {
    
}

#pragma mark Google Button Action Method 
- (IBAction)loginWithGoogleButtonAction:(id)sender {
    
}

@end
