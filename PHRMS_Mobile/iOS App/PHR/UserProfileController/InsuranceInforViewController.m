//
//  InsuranceInforViewController.m
//  PHR
//
//  Created by CDAC HIED on 25/02/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "InsuranceInforViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"

typedef enum
{
    validTillPickerTag = 1300,
    validTillDoneButtonTag
    
}userInsuranceProfileTags;

@interface InsuranceInforViewController (){
    SWRevealViewController *revealController;
    
    NSMutableArray* userInsuranceInfoArray;
    
    UIView* validTillPickerView;
}

@property (weak, nonatomic) IBOutlet UILabel *usernameLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImageView;
@property (weak, nonatomic) IBOutlet UIScrollView *insuranceInfoScrollView;

@property (weak, nonatomic) IBOutlet UITextField *insuranceProviderTextfield;
@property (weak, nonatomic) IBOutlet UITextField *policyNumberTextfield;
@property (weak, nonatomic) IBOutlet UITextField *policyNameTextfield;
@property (weak, nonatomic) IBOutlet UIButton *validTillButton;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;
@property (weak, nonatomic) IBOutlet UIImageView *insuranceProviderImage;
@property (weak, nonatomic) IBOutlet UIImageView *policyNumberImage;
@property (weak, nonatomic) IBOutlet UIImageView *policyNameImage;

@property (weak, nonatomic) IBOutlet UILabel *insuranceProviderLabel;
@property (weak, nonatomic) IBOutlet UILabel *policyNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *policyNumberLabel;
@property (weak, nonatomic) IBOutlet UILabel *validTillLabel;

-(IBAction)validTillButtonAction:(id)sender;
@end

@implementation InsuranceInforViewController

-(void)viewWillLayoutSubviews{
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
        UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
        
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            [self.insuranceInfoScrollView setContentSize:CGSizeMake(self.insuranceInfoScrollView.frame.size.width, self.insuranceInfoScrollView.frame.size.height+200)];
        }
        else{
            [self.insuranceInfoScrollView setContentSize:CGSizeMake(self.insuranceInfoScrollView.frame.size.width, self.insuranceInfoScrollView.frame.size.height+50)];
        }
    }
    else{
        [self.insuranceInfoScrollView setContentSize:CGSizeMake(self.insuranceInfoScrollView.frame.size.width, self.insuranceInfoScrollView.frame.size.height+150)];
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    
    [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
        [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
        [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
        self.navigationItem.leftBarButtonItem=barItem;
    }];
    
    NSDictionary * attributes;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        self.insuranceProviderLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.policyNameLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.policyNumberLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.validTillLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.insuranceProviderTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.validTillButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.policyNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.policyNumberTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.insuranceProviderLabel.textAlignment = NSTextAlignmentLeft;
        self.policyNameLabel.textAlignment = NSTextAlignmentLeft;
        self.policyNumberLabel.textAlignment = NSTextAlignmentLeft;
        self.validTillLabel.textAlignment = NSTextAlignmentLeft;
        
        self.validTillButton.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        self.policyNumberTextfield.textAlignment = NSTextAlignmentLeft;
        
        self.usernameLabel.font = [UIFont systemFontOfSize:22.0f];
        self.emailLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailLabel.textAlignment = NSTextAlignmentLeft;
        
        [self.userImageView.layer setCornerRadius:35];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:20 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Insurance Information"
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
//        [self.insuranceInfoScrollView setContentInset:UIEdgeInsetsMake(-64,0,0,0)];
        
        [self.userImageView.layer setCornerRadius:50];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Insurance Information"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        [titleLabel setTextAlignment:NSTextAlignmentCenter];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1 ];
        attributes = @{NSFontAttributeName: font};
    }

    // Do any additional setup after loading the view.
    
    userInsuranceInfoArray = [NSMutableArray new];
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.insuranceInfoScrollView addGestureRecognizer:singleFingerTap];
    
//    [self.userImageView.layer setCornerRadius:50];
    [self.userImageView.layer setMasksToBounds:YES];
    
    _usernameLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME];
    self.emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
        
        dispatch_async(dispatch_get_main_queue(), ^{
            if (imageData) {
                [self.userImageView setImage:[UIImage imageWithData:imageData]];
            }
            else{
                [self.userImageView setImage:[UIImage imageNamed:@"userImage"]];
            }
            [self addValidTillDatePicker];
        });
    });
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"Edit" style:UIBarButtonItemStylePlain target:self action:@selector(editableControls)];
    
//    UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1 weight:-1];
//    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
    [self enableEditing:NO];
    [self getInsuranceProfile];
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
        [self.insuranceInfoScrollView setContentSize:CGSizeMake(self.insuranceInfoScrollView.frame.size.width, self.insuranceInfoScrollView.frame.size.height+200)];
    }
    else{
        [self.insuranceInfoScrollView setContentSize:CGSizeMake(self.insuranceInfoScrollView.frame.size.width, self.insuranceInfoScrollView.frame.size.height+50)];
    }
}

#pragma mark Enable Editing Mode
-(void)editableControls{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [self enableEditing:YES];
        [self.navigationItem.rightBarButtonItem setTitle:@"Update"];
        
        if ([self.insuranceProviderTextfield.text isEqualToString:@"-"]) {
            self.insuranceProviderTextfield.text = @"";
        }
        if ([self.policyNameTextfield.text isEqualToString:@"-"]) {
            self.policyNameTextfield.text = @"";
        }
        if ([self.validTillButton.titleLabel.text isEqualToString:@"-"]) {
            [self.validTillButton setTitle:@"Select" forState:UIControlStateNormal];
        }

    }
    else{
        [self updateInsuranceProfile];
    }
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        validTillPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}


#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        validTillPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    [UIView animateWithDuration:0.75 animations:^{
        validTillPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Enable Control For Editing Method
-(void)enableEditing: (BOOL)value{
    
    self.insuranceProviderTextfield.enabled = value;
    self.policyNameTextfield.enabled = value;
    self.policyNumberTextfield.enabled = value;
    
    UIColor* color;
    if (value) {
        value = NO;
        color = [UIColor blueColor];
    }
    else{
        value = YES;
        color = [UIColor darkGrayColor];
    }
    
//    self.validTillButton.titleLabel.textColor = color;
    
    self.insuranceProviderImage.hidden = value;
    self.policyNumberImage.hidden = value;
    self.policyNameImage.hidden = value;

}

#pragma mark Create DOB datepicker custom view 
-(void)addValidTillDatePicker{
    
    // creating custom view for DOB
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        validTillPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
    }
    else{
        validTillPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(pickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:validTillDoneButtonTag];
    [validTillPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(pickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [validTillPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIDatePicker* dobDatePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //[dobDatePicker setDate:[NSDate date]];
    [dobDatePicker setDatePickerMode:UIDatePickerModeDate];
    dobDatePicker.minimumDate=[NSDate date];
    [dobDatePicker setTag:validTillPickerTag];
    [validTillPickerView addSubview:dobDatePicker];
    
    validTillPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:validTillPickerView];
}

-(void)pickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        validTillPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 200);
    }];
    
    NSDateFormatter *formatter = [[NSDateFormatter alloc]init];
    [formatter setDateFormat:@"dd/MM/yyyy"];
    
    UIDatePicker* dobDatePicker = (UIDatePicker*)[validTillPickerView viewWithTag:validTillPickerTag];
    NSString* dateString = [formatter stringFromDate:dobDatePicker.date];
    
    [self.validTillButton setTitle:dateString forState:UIControlStateNormal];
}

-(void)pickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        validTillPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 200);
    }];
}

#pragma mark API Call
-(void)getInsuranceProfile{
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
                userInsuranceInfoArray = [responseObject valueForKey:@"response"];
                
                NSString* insurance= [NSString stringWithFormat:@"%@",[userInsuranceInfoArray valueForKey:@"Insu_Org_Name"]];
                if ([insurance isKindOfClass:[NSNull class]] || [insurance isEqualToString:@"<null>"] || [insurance isEqualToString:@""]) {
                    self.insuranceProviderTextfield.text = @"-";
                }
                else{
                    self.insuranceProviderTextfield.text = insurance;
                }
                
                NSString* policyName= [NSString stringWithFormat:@"%@",[userInsuranceInfoArray valueForKey:@"Insu_Org_Grp_Num"]];
                if ([policyName isKindOfClass:[NSNull class]] || [policyName isEqualToString:@"<null>"] || [policyName isEqualToString:@""]) {
                    self.policyNameTextfield.text = @"-";
                }
                else{
                    self.policyNameTextfield.text = policyName;
                }
                
                NSString* policyNumber = [NSString stringWithFormat:@"%@",[userInsuranceInfoArray valueForKey:@"Insu_Org_Phone"]];
                if ([policyNumber isKindOfClass:[NSNull class]] || [policyNumber isEqualToString:@"<null>"]) {
                    self.policyNumberTextfield.text = @"-";
                }
                else{
                    self.policyNumberTextfield.text = policyNumber;
                }
                
//                self.policyNumberTextfield.text = [NSString stringWithFormat:@"%@",[userInsuranceInfoArray valueForKey:@"Insu_Org_Phone"]];
                
                NSString* valid = [NSString stringWithFormat:@"%@",[userInsuranceInfoArray valueForKey:@"strValidTill"]];
//                NSArray* arr = [valid componentsSeparatedByString:@"T"];
//                valid = [arr objectAtIndex:0];
                if ([valid isKindOfClass:[NSNull class]] || [valid isEqualToString:@""] || [valid isEqualToString:@"0001-01-01"]) {
                    [self.validTillButton setTitle:@"-" forState:UIControlStateNormal];
                }
                else{
                    [self.validTillButton setTitle:valid forState:UIControlStateNormal];
                }
            }
            else{
                [kAppDelegate showAlertView:@"Request failed"];
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
-(void)updateInsuranceProfile{
    
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Updating..."];//Show loading indicator.
        
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
                [kAppDelegate showAlertView:@"Insurance information updated"];
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
    //    }
}

-(void)validTillButtonAction:(id)sender{
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        
        [self enableEditing:YES];
        [self.view endEditing:YES];
        
        UIDatePicker* picker = (UIDatePicker*)[validTillPickerView viewWithTag:validTillPickerTag];
        [picker setMinimumDate:[NSDate date]];
        
        [UIView animateWithDuration:0.75 animations:^{
            
            if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
                validTillPickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
                
                picker.frame = CGRectMake(0, 30, validTillPickerView.frame.size.width, 200);
                
                UIButton* doneButton = (UIButton*)[validTillPickerView viewWithTag:validTillDoneButtonTag];
                doneButton.frame = CGRectMake(validTillPickerView.frame.size.width-70, 2, 60, 30);
            }
            else{
                validTillPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
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
