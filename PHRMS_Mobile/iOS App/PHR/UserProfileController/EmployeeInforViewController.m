//
//  EmployeeInforViewController.m
//  PHR
//
//  Created by CDAC HIED on 25/02/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "EmployeeInforViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"

typedef enum
{
    statePickerTag = 1200,
    stateDoneButtonTag
    
}userEmployerProfileTags;


@interface EmployeeInforViewController (){
    SWRevealViewController *revealController;
    
    NSMutableArray* statesArray;
    NSString* stateString;
    NSString* stateID;
    
    NSMutableArray* userEmployerInfoArray;
    
    UIView* statePickerView;
}

@property (weak, nonatomic) IBOutlet UILabel *usernameLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImageView;
@property (weak, nonatomic) IBOutlet UIScrollView *employerInfoScrollView;

@property (weak, nonatomic) IBOutlet UITextView *employerTextfield;
@property (weak, nonatomic) IBOutlet UITextField *phoneTextfield;
@property (weak, nonatomic) IBOutlet UITextField *currentOccupationTextfield;
@property (weak, nonatomic) IBOutlet UITextView *addressLine1Textfield;
@property (weak, nonatomic) IBOutlet UITextView *addressLine2Textfield;
@property (weak, nonatomic) IBOutlet UITextField *districtTextfield;
@property (weak, nonatomic) IBOutlet UITextView *city_villageTextfield;
@property (weak, nonatomic) IBOutlet UIButton *stateButton;
@property (weak, nonatomic) IBOutlet UITextField *pinTextfield;
@property (weak, nonatomic) IBOutlet UITextField *cugCodeTextfield;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;
@property (weak, nonatomic) IBOutlet UIImageView *addressLine1Image;
@property (weak, nonatomic) IBOutlet UIImageView *cityImage;
@property (weak, nonatomic) IBOutlet UIImageView *employerImage;
@property (weak, nonatomic) IBOutlet UIImageView *phoneImage;
@property (weak, nonatomic) IBOutlet UIImageView *cugCodeImage;
@property (weak, nonatomic) IBOutlet UIImageView *currentOccupationImage;
@property (weak, nonatomic) IBOutlet UIImageView *addressLine2Image;
@property (weak, nonatomic) IBOutlet UIImageView *districtImage;
@property (weak, nonatomic) IBOutlet UIImageView *pinImage;

@property (weak, nonatomic) IBOutlet UILabel *employerNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *phoneLabel;
@property (weak, nonatomic) IBOutlet UILabel *districtLabel;
@property (weak, nonatomic) IBOutlet UILabel *address1Label;
@property (weak, nonatomic) IBOutlet UILabel *address2Label;
@property (weak, nonatomic) IBOutlet UILabel *cityLabel;
@property (weak, nonatomic) IBOutlet UILabel *stateLabel;
@property (weak, nonatomic) IBOutlet UILabel *cugLabel;
@property (weak, nonatomic) IBOutlet UILabel *pinLabel;
@property (weak, nonatomic) IBOutlet UILabel *occupationLabel;

-(IBAction)stateButtonAction:(id)sender;

@end

@implementation EmployeeInforViewController

-(void)viewWillLayoutSubviews{
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
        UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
        
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            [self.employerInfoScrollView setContentSize:CGSizeMake(self.employerInfoScrollView.frame.size.width, self.employerInfoScrollView.frame.size.height+200)];
        }
        else{
            [self.employerInfoScrollView setContentSize:CGSizeMake(self.employerInfoScrollView.frame.size.width, self.employerInfoScrollView.frame.size.height+50)];
        }
    }
    else{
        [self.employerInfoScrollView setContentSize:CGSizeMake(self.employerInfoScrollView.frame.size.width, self.employerInfoScrollView.frame.size.height+470)];
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    
    stateID = @"0";
    
    [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
        [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
        [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
        self.navigationItem.leftBarButtonItem=barItem;
    }];
    
    NSDictionary * attributes;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        self.employerNameLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.phoneLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address1Label.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address2Label.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cityLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cugLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.occupationLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.employerTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.phoneTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.addressLine1Textfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.addressLine2Textfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.city_villageTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cugCodeTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.currentOccupationTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.employerNameLabel.textAlignment = NSTextAlignmentLeft;
        self.phoneLabel.textAlignment = NSTextAlignmentLeft;
        self.cityLabel.textAlignment = NSTextAlignmentLeft;
        self.address1Label.textAlignment = NSTextAlignmentLeft;
        self.address2Label.textAlignment = NSTextAlignmentLeft;
        self.districtLabel.textAlignment = NSTextAlignmentLeft;
        self.stateLabel.textAlignment = NSTextAlignmentLeft;
        self.pinLabel.textAlignment = NSTextAlignmentLeft;
        self.cugLabel.textAlignment = NSTextAlignmentLeft;
        self.occupationLabel.textAlignment = NSTextAlignmentLeft;
        
        self.phoneTextfield.textAlignment = NSTextAlignmentLeft;
        self.addressLine2Textfield.textAlignment = NSTextAlignmentLeft;
        self.districtTextfield.textAlignment = NSTextAlignmentLeft;
        self.pinTextfield.textAlignment = NSTextAlignmentLeft;
        self.currentOccupationTextfield.textAlignment = NSTextAlignmentLeft;
        
        self.usernameLabel.font = [UIFont systemFontOfSize:22.0f];
        self.emailLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailLabel.textAlignment = NSTextAlignmentLeft;
        
        [self.userImageView.layer setCornerRadius:35];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:20 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Employer Information"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        [titleLabel setTextAlignment:NSTextAlignmentCenter];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:22.0f weight:-1 ];
        attributes = @{NSFontAttributeName: font};
    }
    else{
//        [self.employerInfoScrollView setContentInset:UIEdgeInsetsMake(-64,0,0,0)];
        
        [self.userImageView.layer setCornerRadius:50];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Employer Information"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        [titleLabel setTextAlignment:NSTextAlignmentCenter];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1 ];
        attributes = @{NSFontAttributeName: font};
    }
    
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    NSString *statesNameFilePath = [[NSBundle mainBundle] pathForResource:@"States" ofType:@"plist"];
    statesArray = [[NSMutableArray alloc] initWithContentsOfFile:statesNameFilePath];
    
    userEmployerInfoArray = [NSMutableArray new];
    
    self.usernameLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME];
    self.emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];

    // Do any additional setup after loading the view.
    
    [self.userImageView.layer setMasksToBounds:YES];
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
        
        dispatch_async(dispatch_get_main_queue(), ^{
            if (imageData) {
                [self.userImageView setImage:[UIImage imageWithData:imageData]];
            }
            else{
                [self.userImageView setImage:[UIImage imageNamed:@"userImage"]];
            }
            
            [self addStatePicker];
        });
    });
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.employerInfoScrollView addGestureRecognizer:singleFingerTap];
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"Edit" style:UIBarButtonItemStylePlain target:self action:@selector(editableControls)];
    
//    UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1 weight:-1];
//    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;

}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
    [self enableEditing:NO];
    [self getEmployerProfile];
}

- (void)revealAppointmentView:(id)sender {
    
    revealController = [self revealViewController];
    [revealController revealToggleAnimated:YES];
    
    if (self.view.userInteractionEnabled==NO) {
        [self.view setUserInteractionEnabled:YES];
    }
    else{
        [self.view setUserInteractionEnabled:NO];
    }
}

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    //    CGRect screenRect = [[UIScreen mainScreen] bounds];
    //    CGFloat screenWidth = screenRect.size.width;
    //    CGFloat screenHeight = screenRect.size.height;
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.employerInfoScrollView setContentSize:CGSizeMake(self.employerInfoScrollView.frame.size.width, self.employerInfoScrollView.frame.size.height+200)];
    }
    else{
        [self.employerInfoScrollView setContentSize:CGSizeMake(self.employerInfoScrollView.frame.size.width, self.employerInfoScrollView.frame.size.height+50)];
    }
}

#pragma mark API Call
-(void)getEmployerProfile{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"getting..."];//Show loading indicator.
        
        //Parameter.
        //        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        //        [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"userid"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                userEmployerInfoArray = [responseObject valueForKey:@"response"];
                
                NSString* employer= [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmployerName"]];
                if ([employer isKindOfClass:[NSNull class]] || [employer isEqualToString:@"<null>"] || [employer isEqualToString:@""]) {
                    _employerTextfield.text = @"-";
                }
                else{
                    _employerTextfield.text = employer;
                }
                
                NSString* addressLine1= [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmpAddressLine1"]];
                if ([addressLine1 isKindOfClass:[NSNull class]] || [addressLine1 isEqualToString:@"<null>"] || [addressLine1 isEqualToString:@""]) {
                    _addressLine1Textfield.text = @"-";
                }
                else{
                    _addressLine1Textfield.text = addressLine1;
                }
                
                NSString* addressLine2= [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmpAddressLine2"]];
                if ([addressLine2 isKindOfClass:[NSNull class]] || [addressLine2 isEqualToString:@"<null>"] || [addressLine2 isEqualToString:@""]) {
                    _addressLine2Textfield.text = @"-";
                }
                else{
                    _addressLine2Textfield.text = addressLine2;
                }
                
                NSString* district = [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmpDistrict"]];
                if ([district isKindOfClass:[NSNull class]] || [district isEqualToString:@"<null>"] || [district isEqualToString:@""]) {
                    _districtTextfield.text = @"-";
                }
                else{
                    _districtTextfield.text = district;
                }

                
                NSString* city = [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmpCity_Vill_Town"]];
                if ([city isKindOfClass:[NSNull class]] || [city isEqualToString:@"<null>"] || [city isEqualToString:@""]) {
                    _city_villageTextfield.text = @"-";
                }
                else{
                    _city_villageTextfield.text = city;
                }
                
//                stateID = [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmpState"]];
//                
//                [_stateButton setTitle:[NSString stringWithFormat:@"%@",[NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"strState"]]] forState:UIControlStateNormal];
                int stateNo = [[userEmployerInfoArray valueForKey:@"EmpState"] intValue];
                if (stateNo==0) {
                    [_stateButton setTitle:@"-" forState:UIControlStateNormal];
                }
                else{
                    [_stateButton setTitle:[NSString stringWithFormat:@"%@",[[statesArray objectAtIndex:stateNo-1] valueForKey:@"StateName"]] forState:UIControlStateNormal];
                }
                
                NSString* phone = [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmployerPhone"]];
                if ([phone isEqualToString:@"<null>"] || [phone isKindOfClass:[NSNull class]] || [phone isEqualToString:@""]) {
                    _phoneTextfield.text = @"-";
                }
                else{
                    _phoneTextfield.text = phone;
                }
                
                NSString* pin = [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmpPin"]];
                if ([pin isKindOfClass:[NSNull class]] || [pin isEqualToString:@"<null>"] || [pin isEqualToString:@""]) {
                    _pinTextfield.text = @"-";
                }
                else{
                    _pinTextfield.text = pin;
                }
                
                NSString* cug = [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"CUG"]];
                if ([cug isKindOfClass:[NSNull class]] || [cug isEqualToString:@"<null>"] || [cug isEqualToString:@""]) {
                    _cugCodeTextfield.text = @"-";
                }
                else{
                    _cugCodeTextfield.text = cug;
                }

                NSString* occupation = [NSString stringWithFormat:@"%@",[userEmployerInfoArray valueForKey:@"EmployerOccupation"]];
                if ([occupation isKindOfClass:[NSNull class]] || [occupation isEqualToString:@"<null>"] || [occupation isEqualToString:@""]) {
                    _currentOccupationTextfield.text = @"-";
                }
                else{
                    _currentOccupationTextfield.text = occupation;
                }
                
            }
            else{
//                [kAppDelegate showAlertView:@"Request failed"];
                userEmployerInfoArray = responseObject;
                
//                _employerTextfield.text = @"-";
//                
//                _addressLine1Textfield.text = @"-";
//                _addressLine2Textfield.text = @"-";
//                _districtTextfield.text = @"-";
//                
//                _city_villageTextfield.text = @"-";
//                
//                [_stateButton setTitle:@"Select" forState:UIControlStateNormal];
//                
//                _phoneTextfield.text = @"-";
//                
//                _pinTextfield.text = @"-";
//                _cugCodeTextfield.text = @"-";
//                _currentOccupationTextfield.text = @"-";
            }
            
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

#pragma mark Update User Emergency Info API Call 
-(void)updateEmployerProfile{
    //    if ([self.testNameButton.titleLabel.text isEqualToString:@"Select Test"]) {
    //        [kAppDelegate showAlertView:@"Select test name"];
    //    }
    //    else if ([self.testDateButton.titleLabel.text isEqualToString:@"Select"]) {
    //        [kAppDelegate showAlertView:@"Select test date"];
    //    }
    //    else if (self.resultTextfield.text.length==0) {
    //        [kAppDelegate showAlertView:@"Enter test result value"];
    //        [self.resultTextfield becomeFirstResponder];
    //    }
    if (self.phoneTextfield.text.length<10 && self.phoneTextfield.text.length>0) {
        [kAppDelegate showAlertView:@"Invalid phone no."];
        [self.phoneTextfield becomeFirstResponder];
    }
    else if (self.pinTextfield.text.length<6 && self.pinTextfield.text.length>0) {
        [kAppDelegate showAlertView:@"Invalid pin no."];
        [self.pinTextfield becomeFirstResponder];
    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"Updating..."];//Show loading indicator.
            
//            if ([self.addressLine1Textfield.text isEqualToString:@"-"]) {
//                self.addressLine1Textfield.text = @"";
//            }
//            if ([self.addressLine2Textfield.text isEqualToString:@"-"]) {
//                self.addressLine2Textfield.text = @"";
//            }
//            if ([self.employerTextfield.text isEqualToString:@"-"]) {
//                self.employerTextfield.text = @"";
//            }
//            if ([self.city_villageTextfield.text isEqualToString:@"-"]) {
//                self.city_villageTextfield.text = @"";
//            }
            
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
                [kAppDelegate hideLoadingIndicator];
                NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                    [kAppDelegate showAlertView:@"Employer information updated"];
                    [self enableEditing:NO];
                    
                    [self.navigationItem.rightBarButtonItem setTitle:@"Edit"];
                }
                else{
                    [kAppDelegate showAlertView:@"Please do some changes"];
                    [self enableEditing:NO];
                    [self.navigationItem.rightBarButtonItem setTitle:@"Edit"];
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

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}


#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string
{
    // allow backspace
    if (!string.length)
    {
        return YES;
    }
    
    // Prevent invalid character input, if keyboard is numberpad
    if (textField.keyboardType == UIKeyboardTypeNumberPad)
    {
        if ([string rangeOfCharacterFromSet:[[NSCharacterSet decimalDigitCharacterSet] invertedSet]].location != NSNotFound)
        {
            [kAppDelegate showAlertView:@"This field accepts only numeric entries."];
            return NO;
        }
    }
    
    NSString *newString = [textField.text stringByReplacingCharactersInRange:range withString:string];
    
    if (textField==self.phoneTextfield) {
        if(([newString length] > 10)){
            
            [kAppDelegate showAlertView:@"Maximum Phone no. length is 10 digits"];
            return NO;
        }
    }
    else if (textField==self.pinTextfield) {
        if(([newString length] > 6)){
            
            [kAppDelegate showAlertView:@"Maximum Pin no. length is 6 digits"];
            return NO;
        }
    }
    
    return YES;
}

#pragma mark Enable Editing Mode
-(void)editableControls{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [self enableEditing:YES];
        [self.navigationItem.rightBarButtonItem setTitle:@"Update"];
        if ([self.addressLine1Textfield.text isEqualToString:@"-"]) {
            self.addressLine1Textfield.text = @"";
        }
        if ([self.addressLine2Textfield.text isEqualToString:@"-"]) {
            self.addressLine2Textfield.text = @"";
        }
        if ([self.employerTextfield.text isEqualToString:@"-"]) {
            self.employerTextfield.text = @"";
        }
        if ([self.city_villageTextfield.text isEqualToString:@"-"]) {
            self.city_villageTextfield.text = @"";
        }
        if ([self.districtTextfield.text isEqualToString:@"-"]) {
            self.districtTextfield.text = @"";
        }
        if ([self.stateButton.titleLabel.text isEqualToString:@"-"]) {
            [self.stateButton setTitle:@"Select" forState:UIControlStateNormal];
        }
        if ([self.pinTextfield.text isEqualToString:@"-"]) {
            self.pinTextfield.text = @"";
        }
        if ([self.currentOccupationTextfield.text isEqualToString:@"-"]) {
            self.currentOccupationTextfield.text = @"";
        }
        if ([self.phoneTextfield.text isEqualToString:@"-"]) {
            self.phoneTextfield.text = @"";
        }
        if ([self.cugCodeTextfield.text isEqualToString:@"-"]) {
            self.cugCodeTextfield.text = @"";
        }
        
        self.employerTextfield.layer.cornerRadius = 3;
        self.employerTextfield.clipsToBounds = YES;
        self.employerTextfield.layer.borderWidth = 0.75f;
        self.employerTextfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
        self.addressLine1Textfield.layer.cornerRadius = 3;
        self.addressLine1Textfield.clipsToBounds = YES;
        self.addressLine1Textfield.layer.borderWidth = 0.75f;
        self.addressLine1Textfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
        self.addressLine2Textfield.layer.cornerRadius = 3;
        self.addressLine2Textfield.clipsToBounds = YES;
        self.addressLine2Textfield.layer.borderWidth = 0.75f;
        self.addressLine2Textfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
        self.city_villageTextfield.layer.cornerRadius = 3;
        self.city_villageTextfield.clipsToBounds = YES;
        self.city_villageTextfield.layer.borderWidth = 0.75f;
        self.city_villageTextfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
    }
    else{
        [self updateEmployerProfile];
    }
}

#pragma mark Enable Control For Editing Method
-(void)enableEditing: (BOOL)value{
    
//    if (value) {
//        if ([[userEmployerInfoArray valueForKey:@"status"] intValue] == 0) {
//            self.employerTextfield.placeholder = @"";
//            self.addressLine1Textfield.text = @"";
//            self.addressLine2Textfield.text = @"";
//            self.phoneTextfield.text = @"";
//            self.currentOccupationTextfield.text = @"";
//            self.cugCodeTextfield.text = @"";
//            self.districtTextfield.text = @"";
//            self.city_villageTextfield.text = @"";
//            self.pinTextfield.text = @"";
//        }
//    }
    
    self.employerTextfield.editable = value;
    self.phoneTextfield.enabled = value;
    self.currentOccupationTextfield.enabled = value;
    self.cugCodeTextfield.enabled = value;
    self.addressLine1Textfield.editable = value;
    self.addressLine2Textfield.editable = value;
    self.districtTextfield.enabled = value;
    self.city_villageTextfield.editable = value;
    self.pinTextfield.enabled = value;
    
    UIColor* color;
    if (value) {
        value = NO;
        color = [UIColor blueColor];
    }
    else{
        value = YES;
        color = [UIColor darkGrayColor];
    }
    
//    self.addressLine1Textfield.backgroundColor = color;
//    self.addressLine2Textfield.backgroundColor = color;
//    self.stateButton.titleLabel.textColor = color;
    
//    self.employerImage.hidden = value;
    self.phoneImage.hidden = value;
    self.cugCodeImage.hidden = value;
    self.addressLine1Image.hidden = value;
    self.addressLine2Image.hidden = value;
    self.districtImage.hidden = value;
    self.cityImage.hidden = value;
    self.pinImage.hidden = value;
    self.currentOccupationImage.hidden = value;
}

#pragma mark Create State picker custom view 
-(void)addStatePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        statePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
    }
    else{
        statePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(statePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:stateDoneButtonTag];
    [statePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(statePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [statePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* bloodGroupPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    bloodGroupPicker.dataSource = self;
    //    bloodGroupPicker.delegate = self;
    
    [bloodGroupPicker setTag:statePickerTag];
    [statePickerView addSubview:bloodGroupPicker];
    
    statePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:statePickerView];
}

-(void)statePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
//    [self.stateButton setTitle:stateString forState:UIControlStateNormal];
}

-(void)statePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark UIPickerView Delegates

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView{
    return 1;
}

- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component{
    
    return [statesArray count];
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    stateString = [[statesArray objectAtIndex:row] valueForKey:@"StateName"];
    return stateString;
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    
    [self.stateButton setTitle:[[statesArray objectAtIndex:row] valueForKey:@"StateName"] forState:UIControlStateNormal];
    stateID = [[statesArray objectAtIndex:row] valueForKey:@"StateId"];
    
}

-(void)stateButtonAction:(id)sender{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        
        [self.view endEditing:YES];
        //    [self gpPickerCancelButtonAction];
        //    isStartDateButton = YES;
        
        UIPickerView* picker = (UIPickerView*)[statePickerView viewWithTag:statePickerTag];
        //        [picker setMinimumDate:[NSDate date]];
        picker.dataSource = self;
        picker.delegate = self;
        
        [UIView animateWithDuration:0.75 animations:^{
            
            if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
                statePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
                
                picker.frame = CGRectMake(0, 30, statePickerView.frame.size.width, 200);
                
                UIButton* doneButton = (UIButton*)[statePickerView viewWithTag:stateDoneButtonTag];
                doneButton.frame = CGRectMake(statePickerView.frame.size.width-70, 2, 60, 30);
            }
            else{
                statePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            }
        }];
    }
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

@end
