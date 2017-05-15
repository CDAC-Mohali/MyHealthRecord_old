//
//  OTPViewController.m
//  PHR
//
//  Created by CDAC HIED on 06/04/17.
//  Copyright © 2017 CDAC HIED. All rights reserved.
//

#import "OTPViewController.h"
#import "Constants.h"

@interface OTPViewController ()
@property (weak, nonatomic) IBOutlet UITextField *otpTextfield;
@property (weak, nonatomic) IBOutlet UILabel *verificationLabel;

- (IBAction)submitButtonAction:(id)sender;
- (IBAction)resendOTPButtonAction:(id)sender;
@end

@implementation OTPViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSString* strMobileNo = [kAppDelegate strRegistrationMobileNo];
    strMobileNo = [strMobileNo substringWithRange:NSMakeRange(6, 4)];
    
    self.verificationLabel.text = [NSString stringWithFormat:@"Enter verification code received on registered Email-Id and mobile XXXXXX%@",strMobileNo];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

/*
#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    // Get the new view controller using [segue destinationViewController].
    // Pass the selected object to the new view controller.
}
*/

#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
}

- (IBAction)submitButtonAction:(id)sender {
    if (self.otpTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter OTP"];
        [self.otpTextfield becomeFirstResponder];
    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"registering..."];//Show loading indicator.
            
            NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
            [dicParams setObject:[kAppDelegate strOTPID] forKey:@"Id"];
            [dicParams setObject:self.otpTextfield.text forKey:@"OTP"];
            
            NSString *urlString = [NSString stringWithFormat:@"enter your API url"];//Url
            
            //AFNetworking methods.
            AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
            AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
            
            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
            //            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
            
            [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
            
            manager.requestSerializer = requestSerializer;
            manager.responseSerializer = [AFJSONResponseSerializer serializerWithReadingOptions:NSJSONReadingAllowFragments];
            [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
                [kAppDelegate hideLoadingIndicator];
                NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"response"] integerValue]==1) {
                    
                    int status = [[responseObject valueForKey:@"Status"] intValue];
                    if (status == 2) {
                        [kAppDelegate showAlertView:@"User registered successfully"];
                        
                        UIStoryboard *storyboard;
                        if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
                            storyboard = [UIStoryboard storyboardWithName:@"Main-iPad" bundle: nil];
                        }
                        else{
                            storyboard = [UIStoryboard storyboardWithName:@"Main" bundle: nil];
                        }
                        
                        LoginController *viewController = [storyboard instantiateViewControllerWithIdentifier:@"LoginController"];
                        
                        [[kAppDelegate window] setRootViewController:viewController];
                    }
                    else if (status == -1) {
                        [kAppDelegate showAlertView:@"Process failure"];
                    }
                    else if (status == -2) {
                        [kAppDelegate showAlertView:@"Entered OTP isn't matched!! Try again"];
                        [self.otpTextfield becomeFirstResponder];
                    }
                }
                else{
                    [kAppDelegate showAlertView:@"Operation failed"];
                }
                
            } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
                NSLog(@"Error: %@", error);
                [kAppDelegate hideLoadingIndicator];
                [kAppDelegate showAlertView:@"Request failed"];
            }];
        }
        else{
            [kAppDelegate showNetworkAlert];
        }
    }
}

- (IBAction)resendOTPButtonAction:(id)sender {
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"sending..."];//Show loading indicator.
        
        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        [dicParams setObject:[kAppDelegate strOTPID] forKey:@"Id"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your API url"];//Url
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        
        [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
        //            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
        
        [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
        
        manager.requestSerializer = requestSerializer;
        manager.responseSerializer = [AFJSONResponseSerializer serializerWithReadingOptions:NSJSONReadingAllowFragments];
        [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            NSLog(@"Result dict %@",responseObject);
            
            if ([[responseObject valueForKey:@"response"] integerValue]==0) {
                [kAppDelegate showAlertView:@"Operation failed!! Try again"];
            }
            
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"Request failed"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}
@end
