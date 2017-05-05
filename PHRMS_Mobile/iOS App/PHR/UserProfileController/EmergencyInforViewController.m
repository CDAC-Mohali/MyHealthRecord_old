//
//  EmergencyInforViewController.m
//  PHR
//
//  Created by CDAC HIED on 25/02/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "EmergencyInforViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"
#import <ContactsUI/ContactsUI.h>
#import <AddressBookUI/AddressBookUI.h>

typedef enum
{
    statePickerTag = 1100,
    stateDoneButtonTag,
    relationshipPickerTag,
    relationshipDoneButtonTag
    
}userEmergencyProfileTags;

@interface EmergencyInforViewController ()<CNContactViewControllerDelegate,CNContactPickerDelegate>{
    SWRevealViewController *revealController;
    
    NSMutableArray* statesArray;
    NSString* stateString;
    NSString* stateID;
    
    NSMutableArray* relationshipArray;
    NSString* relationshipString;
    NSString* relationshipID;
    
    NSMutableArray* userEmergencyInfoArray;
    
    UIView* statePickerView;
    UIView* relationshipPickerView;
    
    BOOL isStatePickerView;
    BOOL isPrimaryNumber;
}

@property (nonatomic, strong) CNContactPickerViewController *addressBookController;
@property (weak, nonatomic) IBOutlet UILabel *usernameLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImageView;
@property (weak, nonatomic) IBOutlet UIScrollView *emergencyInfoScrollView;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;
@property (weak, nonatomic) IBOutlet UITextField *nameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *primaryPhoneTextfield;
@property (weak, nonatomic) IBOutlet UITextField *secondaryPhoneTextfield;
@property (weak, nonatomic) IBOutlet UITextView *addressLine1Textfield;
@property (weak, nonatomic) IBOutlet UITextView *addressLine2Textfield;
@property (weak, nonatomic) IBOutlet UITextField *districtTextfield;
@property (weak, nonatomic) IBOutlet UITextField *city_villageTextfield;
@property (weak, nonatomic) IBOutlet UIButton *stateButton;
@property (weak, nonatomic) IBOutlet UITextField *pinTextfield;
@property (weak, nonatomic) IBOutlet UIButton *relationshipButton;

@property (weak, nonatomic) IBOutlet UIImageView *addressLine1Image;
@property (weak, nonatomic) IBOutlet UIImageView *cityImage;
@property (weak, nonatomic) IBOutlet UIImageView *nameImage;
@property (weak, nonatomic) IBOutlet UIImageView *primaryImage;
@property (weak, nonatomic) IBOutlet UIImageView *secondaryImage;
@property (weak, nonatomic) IBOutlet UIImageView *addressLine2Image;
@property (weak, nonatomic) IBOutlet UIImageView *districtImage;
@property (weak, nonatomic) IBOutlet UIImageView *pinImage;

@property (weak, nonatomic) IBOutlet UILabel *nameLabel;
@property (weak, nonatomic) IBOutlet UILabel *relationshipLabel;
@property (weak, nonatomic) IBOutlet UILabel *districtLabel;
@property (weak, nonatomic) IBOutlet UILabel *address1Label;
@property (weak, nonatomic) IBOutlet UILabel *address2Label;
@property (weak, nonatomic) IBOutlet UILabel *cityLabel;
@property (weak, nonatomic) IBOutlet UILabel *stateLabel;
@property (weak, nonatomic) IBOutlet UILabel *primaryPhoneLabel;
@property (weak, nonatomic) IBOutlet UILabel *pinLabel;
@property (weak, nonatomic) IBOutlet UILabel *secondaryPhoneLabel;
- (IBAction)primaryContactBookButtonAction:(id)sender;
- (IBAction)secondaryContactBookButtonAction:(id)sender;

- (IBAction)relationshipButtonAction:(id)sender;
- (IBAction)stateButtonAction:(id)sender;

@end

@implementation EmergencyInforViewController

-(void)viewWillLayoutSubviews{
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
        UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
        
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            [self.emergencyInfoScrollView setContentSize:CGSizeMake(self.emergencyInfoScrollView.frame.size.width, self.emergencyInfoScrollView.frame.size.height+200)];
        }
        else{
            [self.emergencyInfoScrollView setContentSize:CGSizeMake(self.emergencyInfoScrollView.frame.size.width, self.emergencyInfoScrollView.frame.size.height+50)];
        }
    }
    else{
        [self.emergencyInfoScrollView setContentSize:CGSizeMake(self.emergencyInfoScrollView.frame.size.width, self.emergencyInfoScrollView.frame.size.height+350)];
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
        self.nameLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.relationshipLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address1Label.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address2Label.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cityLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.primaryPhoneLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.secondaryPhoneLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.nameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.relationshipButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.addressLine1Textfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.addressLine2Textfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.city_villageTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.primaryPhoneTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.secondaryPhoneTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.nameTextfield.textAlignment = NSTextAlignmentLeft;
        self.relationshipLabel.textAlignment = NSTextAlignmentLeft;
        self.cityLabel.textAlignment = NSTextAlignmentLeft;
        self.address1Label.textAlignment = NSTextAlignmentLeft;
        self.address2Label.textAlignment = NSTextAlignmentLeft;
        self.districtLabel.textAlignment = NSTextAlignmentLeft;
        self.stateLabel.textAlignment = NSTextAlignmentLeft;
        self.pinLabel.textAlignment = NSTextAlignmentLeft;
        self.primaryPhoneLabel.textAlignment = NSTextAlignmentLeft;
        self.secondaryPhoneLabel.textAlignment = NSTextAlignmentLeft;
        
        self.relationshipButton.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        self.addressLine2Textfield.textAlignment = NSTextAlignmentLeft;
        self.districtTextfield.textAlignment = NSTextAlignmentLeft;
        self.pinTextfield.textAlignment = NSTextAlignmentLeft;
        self.secondaryPhoneTextfield.textAlignment = NSTextAlignmentLeft;
        
        self.usernameLabel.font = [UIFont systemFontOfSize:22.0f];
        self.emailLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailLabel.textAlignment = NSTextAlignmentLeft;
        
        [self.userImageView.layer setCornerRadius:35];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:20 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Emergency Information"
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
//        [self.emergencyInfoScrollView setContentInset:UIEdgeInsetsMake(-64,0,0,0)];
        
        [self.userImageView.layer setCornerRadius:50];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Emergency Information"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        [titleLabel setTextAlignment:NSTextAlignmentCenter];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1 ];
        attributes = @{NSFontAttributeName: font};
    }
    
    NSString *statesNameFilePath = [[NSBundle mainBundle] pathForResource:@"States" ofType:@"plist"];
    statesArray = [[NSMutableArray alloc] initWithContentsOfFile:statesNameFilePath];
    
    NSString *relationshipNameFilePath = [[NSBundle mainBundle] pathForResource:@"relationship" ofType:@"plist"];
    relationshipArray = [[NSMutableArray alloc] initWithContentsOfFile:relationshipNameFilePath];
    
    userEmergencyInfoArray = [NSMutableArray new];
    
    _usernameLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME];
    self.emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
    
//    [self.userImageView.layer setCornerRadius:50];
    [self.userImageView.layer setMasksToBounds:YES];
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
        
        dispatch_async(dispatch_get_main_queue(), ^{
            
            [self addStatePicker];
            [self addRelationshipPicker];
            
            if (imageData) {
                [self.userImageView setImage:[UIImage imageWithData:imageData]];
            }
            else{
                [self.userImageView setImage:[UIImage imageNamed:@"userImage"]];
            }
        });
    });
    
    // Do any additional setup after loading the view.
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.emergencyInfoScrollView addGestureRecognizer:singleFingerTap];
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"Edit" style:UIBarButtonItemStylePlain target:self action:@selector(editableControls)];
    
//    UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1 weight:-1];
//    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
    [self enableEditing:NO];
    [self getUserEmergencyInfoAPI];
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
}

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    //    CGRect screenRect = [[UIScreen mainScreen] bounds];
    //    CGFloat screenWidth = screenRect.size.width;
    //    CGFloat screenHeight = screenRect.size.height;
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.emergencyInfoScrollView setContentSize:CGSizeMake(self.emergencyInfoScrollView.frame.size.width, self.emergencyInfoScrollView.frame.size.height+200)];
    }
    else{
        [self.emergencyInfoScrollView setContentSize:CGSizeMake(self.emergencyInfoScrollView.frame.size.width, self.emergencyInfoScrollView.frame.size.height+50)];
    }
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        relationshipPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    [UIView animateWithDuration:0.75 animations:^{
//        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
//        genderPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
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
    
    if (textField==self.primaryPhoneTextfield || textField==self.secondaryPhoneTextfield) {
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
        
        if ([self.nameTextfield.text isEqualToString:@"-"]) {
            self.nameTextfield.text = @"";
        }
        if ([self.addressLine1Textfield.text isEqualToString:@"-"]) {
            self.addressLine1Textfield.text = @"";
        }
        if ([self.addressLine2Textfield.text isEqualToString:@"-"]) {
            self.addressLine2Textfield.text = @"";
        }
        
        self.addressLine1Textfield.layer.cornerRadius = 3;
        self.addressLine1Textfield.clipsToBounds = YES;
        self.addressLine1Textfield.layer.borderWidth = 0.75f;
        self.addressLine1Textfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
        self.addressLine2Textfield.layer.cornerRadius = 3;
        self.addressLine2Textfield.clipsToBounds = YES;
        self.addressLine2Textfield.layer.borderWidth = 0.75f;
        self.addressLine2Textfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
    }
    else{
        [self updateEmergencyProfile];
    }
}

#pragma mark Enable Control For Editing Method
-(void)enableEditing: (BOOL)value{
    
    self.nameTextfield.enabled = value;
//    self.relationshipButton.enabled = value;
    self.primaryPhoneTextfield.enabled = value;
    self.secondaryPhoneTextfield.enabled = value;
    self.addressLine1Textfield.editable = value;
    self.addressLine2Textfield.editable = value;
    self.districtTextfield.enabled = value;
//    self.stateButton.enabled = value;
    self.city_villageTextfield.enabled = value;
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
//
//    self.relationshipButton.titleLabel.textColor = color;
//    self.stateButton.titleLabel.textColor = color;
//    self.addressLine1Textfield.backgroundColor = color;
//    self.addressLine2Textfield.backgroundColor = color;
   
    self.nameImage.hidden = value;
    self.primaryImage.hidden = value;
    self.secondaryImage.hidden = value;
    self.addressLine1Image.hidden = value;
    self.addressLine2Image.hidden = value;
    self.districtImage.hidden = value;
    self.cityImage.hidden = value;
    self.pinImage.hidden = value;
}

#pragma mark API Call
-(void)getUserEmergencyInfoAPI{
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
                userEmergencyInfoArray = [responseObject valueForKey:@"response"];
                
                NSString* name= [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"Primary_Emergency_Contact"]];
                if ([name isKindOfClass:[NSNull class]] || [name isEqualToString:@"<null>"] || [name isEqualToString:@""]) {
                    _nameTextfield.text = @"-";
                }
                else{
                    _nameTextfield.text = name;
                }
                
                relationshipID = [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_Relationship"]];
                
                [_relationshipButton setTitle:[NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"strPC_Relationship"]] forState:UIControlStateNormal];
                
//                bloodGroupID = [NSString stringWithFormat:@"%@",[[userEmergencyInfoArray valueForKey:@"i"] valueForKey:@"Id"]];
                
                NSString* addressLine1= [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_AddressLine1"]];
                if ([addressLine1 isKindOfClass:[NSNull class]] || [addressLine1 isEqualToString:@"<null>"] || [addressLine1 isEqualToString:@""]) {
                    _addressLine1Textfield.text = @"-";
                }
                else{
                    _addressLine1Textfield.text = addressLine1;
                }
                
                NSString* addressLine2= [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_AddressLine2"]];
                if ([addressLine2 isKindOfClass:[NSNull class]] || [addressLine2 isEqualToString:@"<null>"] || [addressLine2 isEqualToString:@""]) {
                    _addressLine2Textfield.text = @"-";
                }
                else{
                    _addressLine2Textfield.text = addressLine2;
                }
                
                NSString* district = [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_District"]];
                if ([district isKindOfClass:[NSNull class]] || [district isEqualToString:@"<null>"]) {
                    _districtTextfield.text = @"-";
                }
                else{
                    _districtTextfield.text = district;
                }
                
                NSString* city = [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_City_Vill_Town"]];
                if ([city isKindOfClass:[NSNull class]] || [city isEqualToString:@"<null>"]) {
                    _city_villageTextfield.text = @"-";
                }
                else{
                    _city_villageTextfield.text = city;
                }
                
//                stateID = [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_State"]];
                
                int stateNo = [[userEmergencyInfoArray valueForKey:@"PC_State"] intValue];
                
                stateID = [NSString stringWithFormat:@"%d",stateNo];
                
//                for (int i=0; i<[statesArray count]; i++) {
//                    if ([stateID isEqualToString:[[statesArray objectAtIndex:i] valueForKey:@"StateId"]]) {
//                [_stateButton setTitle:[NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"strPC_State"]] forState:UIControlStateNormal];
                if (stateNo!=0) {
                    [_stateButton setTitle:[NSString stringWithFormat:@"%@",[[statesArray objectAtIndex:stateNo-1] valueForKey:@"StateName"]] forState:UIControlStateNormal];
                }
//                        break;
//                    }
//                }
                
                NSString* phone = [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_Phone1"]];
                if ([phone isEqualToString:@"<null>"] || [phone isKindOfClass:[NSNull class]]) {
                    _primaryPhoneTextfield.text = @"-";
                }
                else{
                    _primaryPhoneTextfield.text = phone;
                }
                
                NSString* phone2 = [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_Phone2"]];
                if ([phone2 isEqualToString:@"<null>"] || [phone2 isKindOfClass:[NSNull class]]) {
                    _secondaryPhoneTextfield.text = @"-";
                }
                else{
                    _secondaryPhoneTextfield.text = phone2;
                }
                
                NSString* pin = [NSString stringWithFormat:@"%@",[userEmergencyInfoArray valueForKey:@"PC_Pin"]];
                if ([pin isEqualToString:@"<null>"] || [pin isKindOfClass:[NSNull class]]) {
                    _pinTextfield.text = @"-";
                }
                else{
                    _pinTextfield.text = pin;
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
-(void)updateEmergencyProfile{
    if ([self.stateButton.titleLabel.text isEqualToString:@"Select"] || [self.stateButton.titleLabel.text isEqualToString:@""]) {
        [kAppDelegate showAlertView:@"Select state"];
    }
    //    else if ([self.testDateButton.titleLabel.text isEqualToString:@"Select"]) {
    //        [kAppDelegate showAlertView:@"Select test date"];
    //    }
        else if (self.primaryPhoneTextfield.text.length>0 && self.primaryPhoneTextfield.text.length<10) {
            [kAppDelegate showAlertView:@"Invalid primary phone no."];
            [self.primaryPhoneTextfield becomeFirstResponder];
        }
        else if (self.secondaryPhoneTextfield.text.length>0 && self.secondaryPhoneTextfield.text.length<10) {
            [kAppDelegate showAlertView:@"Invalid secondary phone no."];
            [self.secondaryPhoneTextfield becomeFirstResponder];
        }
        else if (self.pinTextfield.text.length<6 && self.pinTextfield.text.length>0) {
            [kAppDelegate showAlertView:@"Invalid pin no."];
            [self.pinTextfield becomeFirstResponder];
        }
    else{
         if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"Updating..."];//Show loading indicator.
             
             if ([self.addressLine1Textfield.text isEqualToString:@"-"]) {
                 self.addressLine1Textfield.text = @"";
             }
             if ([self.addressLine2Textfield.text isEqualToString:@"-"]) {
                 self.addressLine2Textfield.text = @"";
             }
             
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
                    [kAppDelegate showAlertView:@"Emergency information updated"];
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

#pragma mark Create Relationship picker custom view 
-(void)addRelationshipPicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        relationshipPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
    }
    else{
        relationshipPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(relationshipPickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:relationshipDoneButtonTag];
    [relationshipPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(relationshipPickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [relationshipPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* bloodGroupPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    bloodGroupPicker.dataSource = self;
    //    bloodGroupPicker.delegate = self;
    
    [bloodGroupPicker setTag:relationshipPickerTag];
    [relationshipPickerView addSubview:bloodGroupPicker];
    
    relationshipPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:relationshipPickerView];
}

-(void)relationshipPickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        relationshipPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
//    [self.relationshipButton setTitle:relationshipString forState:UIControlStateNormal];
}

-(void)relationshipPickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        relationshipPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark UIPickerView Delegates

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView{
    return 1;
}

- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component{
    if (isStatePickerView) {
        return [statesArray count];
    }
    else{
        return [relationshipArray count];
    }
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    if (isStatePickerView) {
        stateString = [[statesArray objectAtIndex:row] valueForKey:@"StateName"];
        return stateString;
    }
    else{
        relationshipString = [[relationshipArray objectAtIndex:row] valueForKey:@"Relation"];
        return relationshipString;
    }
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    if (isStatePickerView) {
        [self.stateButton setTitle:[[statesArray objectAtIndex:row] valueForKey:@"StateName"] forState:UIControlStateNormal];
        stateID = [[statesArray objectAtIndex:row] valueForKey:@"StateId"];
    }
    else{
        [self.relationshipButton setTitle:[[relationshipArray objectAtIndex:row] valueForKey:@"Relation"] forState:UIControlStateNormal];
        relationshipID = [[relationshipArray objectAtIndex:row] valueForKey:@"Id"];
    }
}

#pragma mark Buttons Action
- (IBAction)primaryContactBookButtonAction:(id)sender {
    
    isPrimaryNumber = YES;
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        _addressBookController = [[CNContactPickerViewController alloc] init];
        [_addressBookController setDelegate:self];
        [self presentViewController:_addressBookController animated:YES completion:nil];
    }
}

- (void)contactPicker:(CNContactPickerViewController *)picker didSelectContact:(CNContact *)contact{
    
    NSArray <CNLabeledValue<CNPhoneNumber *> *> *phoneNumbers = contact.phoneNumbers;
    CNLabeledValue<CNPhoneNumber *> *firstPhone = [phoneNumbers firstObject];
    CNPhoneNumber *number = firstPhone.value;
    NSString *digits = number.stringValue; // 1234567890
    
    digits = [digits stringByReplacingOccurrencesOfString:@"-" withString:@""];
    if ([digits length]>10) {
        digits = [digits
                  stringByReplacingOccurrencesOfString:@" " withString:@""];
        digits = [digits substringFromIndex: [digits length] - 10];
    }
    
    NSString *displayName = [NSString stringWithFormat:@"%@ %@",contact.givenName,contact.familyName];
    
    if (isPrimaryNumber) {
        self.primaryPhoneTextfield.text = [NSString stringWithFormat:@"%@", digits];
        self.nameTextfield.text = [NSString stringWithFormat:@"%@", displayName];
    }
    else{
        self.secondaryPhoneTextfield.text = [NSString stringWithFormat:@"%@", digits];
    }

    
}

- (IBAction)secondaryContactBookButtonAction:(id)sender {
    
    isPrimaryNumber = NO;
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        _addressBookController = [[CNContactPickerViewController alloc] init];
        [_addressBookController setDelegate:self];
        [self presentViewController:_addressBookController animated:YES completion:nil];
    }
}

-(void)relationshipButtonAction:(id)sender{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        isStatePickerView = NO;
        
        [self enableEditing:YES];
        [self.view endEditing:YES];
        //    [self gpPickerCancelButtonAction];
        //    isStartDateButton = YES;
        
        UIPickerView* picker = (UIPickerView*)[relationshipPickerView viewWithTag:relationshipPickerTag];
        //        [picker setMinimumDate:[NSDate date]];
        picker.dataSource = self;
        picker.delegate = self;
        
        [UIView animateWithDuration:0.75 animations:^{
            
            if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
                relationshipPickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
                
                picker.frame = CGRectMake(0, 30, relationshipPickerView.frame.size.width, 200);
                
                UIButton* doneButton = (UIButton*)[relationshipPickerView viewWithTag:relationshipDoneButtonTag];
                doneButton.frame = CGRectMake(relationshipPickerView.frame.size.width-70, 2, 60, 30);
            }
            else{
                relationshipPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            }
        }];
    }
}

-(void)stateButtonAction:(id)sender{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        isStatePickerView = YES;
        
        [self enableEditing:YES];
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
