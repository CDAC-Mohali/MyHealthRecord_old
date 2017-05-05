//
//  AddMedicalContactViewController.m
//  PHR
//
//  Created by CDAC HIED on 27/12/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "AddMedicalContactViewController.h"
#import "Constants.h"
#import <ContactsUI/ContactsUI.h>
#import <AddressBookUI/AddressBookUI.h>

typedef enum : NSUInteger {
    specialityPickerViewDoneButtonTag = 1400,
    specialityPickerViewPickerTag
} specialityTags;

@interface AddMedicalContactViewController ()<CNContactViewControllerDelegate,CNContactPickerDelegate>{
    UIView* specialityPickerView;
    NSMutableArray* specialityArray;
    NSMutableArray* stateArray;
    
    NSString* strSpeciality;
    NSString* strState;
    NSString* strSpecialityID;
    NSString* strStateID;
    
    BOOL isSpecialityMenu;
}

@property (weak, nonatomic) IBOutlet UIScrollView *specialityScrollView;

- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)saveButtonAction:(id)sender;
- (IBAction)selectSpecialityButtonAction:(id)sender;
- (IBAction)selectStateButtonAction:(id)sender;
- (IBAction)pickAddressBook:(id)sender;

@property (weak, nonatomic) IBOutlet UIButton *addressBookButton;
@property (nonatomic, strong) CNContactPickerViewController *addressBookController;
@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;
@property (weak, nonatomic) IBOutlet UIButton *specialityButton;
@property (weak, nonatomic) IBOutlet UIButton *stateButton;

@property (weak, nonatomic) IBOutlet UITextField *contactNameText;
@property (weak, nonatomic) IBOutlet UITextField *contactTypeText;
@property (weak, nonatomic) IBOutlet UITextField *clinicNameText;
@property (weak, nonatomic) IBOutlet UITextField *address1Text;
@property (weak, nonatomic) IBOutlet UITextField *address2Text;
@property (weak, nonatomic) IBOutlet UITextField *cityText;
@property (weak, nonatomic) IBOutlet UITextField *stateText;
@property (weak, nonatomic) IBOutlet UITextField *districtText;
@property (weak, nonatomic) IBOutlet UITextField *pinText;
@property (weak, nonatomic) IBOutlet UITextField *mobileText;
@property (weak, nonatomic) IBOutlet UITextField *emailText;

@property (weak, nonatomic) IBOutlet UILabel *titleLabel;
@property (weak, nonatomic) IBOutlet UILabel *contactNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *contactTypeLabel;
@property (weak, nonatomic) IBOutlet UILabel *clinicNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *address1Label;
@property (weak, nonatomic) IBOutlet UILabel *address2Label;
@property (weak, nonatomic) IBOutlet UILabel *cityLabel;
@property (weak, nonatomic) IBOutlet UILabel *districtLabel;
@property (weak, nonatomic) IBOutlet UILabel *stateLabel;
@property (weak, nonatomic) IBOutlet UILabel *pinLabel;
@property (weak, nonatomic) IBOutlet UILabel *mobileLabel;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;

@end

@implementation AddMedicalContactViewController

-(void)viewWillLayoutSubviews{
    
    CGSize scrollableSize;
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+650);
    }
    else{
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+450);
        }
        else{
            scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+250);
        }
    }
    
    [self.specialityScrollView setContentSize:scrollableSize];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    specialityArray = [NSMutableArray new];
    isSpecialityMenu = NO;
    strStateID = @"0";
    strSpecialityID = @"";
    strState = @"";
    strSpeciality = @"";
    
    NSString *statesNameFilePath = [[NSBundle mainBundle] pathForResource:@"States" ofType:@"plist"];
    stateArray = [[NSMutableArray alloc] initWithContentsOfFile:statesNameFilePath];
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
        //        [self.labTestScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
        self.titleLabel.font = [UIFont systemFontOfSize:25 weight:-1];
        
        self.specialityButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        self.contactNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.clinicNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address1Text.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address2Text.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cityText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.mobileText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.contactNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.clinicNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.contactTypeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.address1Label.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.address2Label.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.cityLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.districtLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.stateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.pinLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.mobileLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.emailLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        //        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    
    self.stateButton.layer.cornerRadius = 3;
    self.stateButton.clipsToBounds = YES;
    self.specialityButton.layer.cornerRadius = 3;
    self.specialityButton.clipsToBounds = YES;
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                  initWithTarget:self action:@selector(handleSingleTap)];

    singleFingerTap.numberOfTapsRequired = 1;
   
    [self.specialityScrollView addGestureRecognizer:singleFingerTap];
    
    [self addAllInOnePickerView];
    [self getSpecialityListAPI];
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.specialityScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+650)];
    }
    else{
        [self.specialityScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+400)];
    }
}

- (IBAction)dismissButtonAction:(id)sender {
    [self dismissViewControllerAnimated:YES completion:nil];
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        specialityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
}

- (IBAction)selectSpecialityButtonAction:(id)sender{
    isSpecialityMenu = YES;
    
    UIPickerView* pickerView = [specialityPickerView viewWithTag:specialityPickerViewPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            
            specialityPickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            UIButton* doneButton = (UIButton*)[pickerView viewWithTag:specialityPickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(pickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            specialityPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 300);
        }
    }];
}

- (IBAction)selectStateButtonAction:(id)sender{
    isSpecialityMenu = NO;
    
    UIPickerView* pickerView = [specialityPickerView viewWithTag:specialityPickerViewPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            
            specialityPickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            UIButton* doneButton = (UIButton*)[pickerView viewWithTag:specialityPickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(pickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            specialityPickerView.frame = CGRectMake(0, [[UIScreen mainScreen] bounds].size.height-200, [[UIScreen mainScreen] bounds].size.width, 300);
        }
    }];
}

- (IBAction)pickAddressBook:(id)sender {
    
//    _addressBookController = [[ABPeoplePickerNavigationController alloc] init];
//    [_addressBookController setPeoplePickerDelegate:self];
//    [self presentViewController:_addressBookController animated:YES completion:nil];
    _addressBookController = [[CNContactPickerViewController alloc] init];
    [_addressBookController setDelegate:self];
    [self presentViewController:_addressBookController animated:YES completion:nil];
}

//- (void)contactPicker:(CNContactPickerViewController *)picker didSelectContactProperty:(CNContactProperty *)contactProperty{
//    
//    CNPhoneNumber *phoneNumber = contactProperty.value;
//    NSString *phoneStr = phoneNumber.stringValue;
//}

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
    
    CNLabeledValue *emailValue = contact.emailAddresses.firstObject;
    NSString *emailString = emailValue.value;
    
    NSString *displayName = [NSString stringWithFormat:@"%@ %@",contact.givenName,contact.familyName];
    
    if (digits!=NULL) {
        self.mobileText.text = [NSString stringWithFormat:@"%@", digits];
    }
    if (displayName) {
        self.contactNameText.text = [NSString stringWithFormat:@"%@", displayName];
    }
    if (emailString!=NULL) {
        self.emailText.text = [NSString stringWithFormat:@"%@", emailString];
    }

}

/*
- (BOOL)peoplePickerNavigationController: (ABPeoplePickerNavigationController *)peoplePicker
      shouldContinueAfterSelectingPerson:(ABRecordRef)person {
    // This line is new.
//    [self.navigationController dismissViewControllerAnimated:YES completion:nil];
//    [self.delegate controllerDidFinish:self];
    return YES;
}

- (void)peoplePickerNavigationController:(ABPeoplePickerNavigationController *)peoplePicker didSelectPerson:(ABRecordRef)person property:(ABPropertyID)property identifier:(ABMultiValueIdentifier)identifier {
//    [self peoplePickerNavigationController:peoplePicker shouldContinueAfterSelectingPerson:person property:property identifier:identifier];
}

- (void)peoplePickerNavigationController:(ABPeoplePickerNavigationController*)peoplePicker didSelectPerson:(ABRecordRef)person{
    
    NSString *displayName = (__bridge NSString *)ABRecordCopyCompositeName(person);
    ABMultiValueRef phoneNumber = ABRecordCopyValue(person, kABPersonPhoneProperty);
    ABMultiValueRef email = ABRecordCopyValue(person, kABPersonEmailProperty);
    
    CFStringRef phoneNumberRef = ABMultiValueCopyValueAtIndex(phoneNumber, 0);
    CFRelease(phoneNumber);
    NSString *phoneNumberString = (__bridge NSString *) phoneNumberRef;
    phoneNumberString = [phoneNumberString
                                     stringByReplacingOccurrencesOfString:@"-" withString:@""];
    if ([phoneNumberString length]>10) {
        phoneNumberString = [phoneNumberString
                             stringByReplacingOccurrencesOfString:@" " withString:@""];
        phoneNumberString = [phoneNumberString substringFromIndex: [phoneNumberString length] - 10];
    }
    
    //CFRelease(phoneNumberRef);
    
    CFStringRef emailID = ABMultiValueCopyValueAtIndex(email, 0);
//    CFRelease(email);
    NSString *emailIDString = (__bridge NSString *) emailID;
//        CFRelease(emailID);
    
    if (phoneNumberRef!=NULL) {
        self.mobileText.text = [NSString stringWithFormat:@"%@", phoneNumberString];
    }
    if (displayName) {
        self.contactNameText.text = [NSString stringWithFormat:@"%@", displayName];
    }
    if (emailID!=NULL) {
        self.emailText.text = [NSString stringWithFormat:@"%@", emailIDString];
    }
    
    [self dismissViewControllerAnimated:YES completion:nil];
}*/

#pragma mark Get Speciality List
-(void)getSpecialityListAPI{
    if ([kAppDelegate hasInternetConnection]) {
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"]intValue] == 1) {
                [specialityArray removeAllObjects];
                specialityArray = [[responseObject valueForKey:@"response"] mutableCopy];
            }
            else{
                [kAppDelegate showAlertView:@"No speciality exists!!"];
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

- (IBAction)saveButtonAction:(id)sender {
    
    if (self.contactNameText.text.length==0) {
        [kAppDelegate showAlertView:@"Enter contact name"];
        [self.contactNameText becomeFirstResponder];
    }
    else if ([self.specialityButton.titleLabel.text isEqualToString:@"Select Speciality"]) {
        [kAppDelegate showAlertView:@"Select speciality"];
    }
    else if (self.clinicNameText.text.length==0) {
        [kAppDelegate showAlertView:@"Enter clinic name"];
        [self.clinicNameText becomeFirstResponder];
    }
    else if ([self.stateButton.titleLabel.text isEqualToString:@"Select State"]) {
        [kAppDelegate showAlertView:@"Select state"];
    }
    else if (self.mobileText.text.length==0) {
        [kAppDelegate showAlertView:@"Enter mobile number"];
        [self.mobileText becomeFirstResponder];
    }
    else if (self.emailText.text.length==0) {
        [kAppDelegate showAlertView:@"Enter email address"];
        [self.emailText becomeFirstResponder];
    }
    else if (![self validEmail:self.emailText.text ]){
        [kAppDelegate showAlertView:@"Invalid email id"];
        [self.emailText becomeFirstResponder];
    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
            //            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
//            NSString* deviceDate = self.testDateButton.titleLabel.text;
//            NSDate* date = [dateFormatter dateFromString:deviceDate];
            
            NSString* dateString = [dateFormat stringFromDate:[NSDate date]];
            NSArray* array = [dateString componentsSeparatedByString:@"+"];
            dateString = [array objectAtIndex:0];
            
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
                    [kAppDelegate showAlertView:@"Medical Contact added successfully"];
                    [self dismissViewControllerAnimated:YES completion:nil];
                }
                else if ([[responseObject valueForKey:@"status"] integerValue]==2) {
                    [kAppDelegate showAlertView:@"Mobile no. already registered!! Please try with another no."];
                }
                else if ([[responseObject valueForKey:@"status"] integerValue]==3) {
                    [kAppDelegate showAlertView:@"Email already registered!! Please try with email address"];
                }
                else{
                    [kAppDelegate showAlertView:@"Operation failed"];
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
}

#pragma mark Create AllInOnePickerView custom view 
-(void)addAllInOnePickerView{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        specialityPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, [[UIScreen mainScreen] bounds].size.height+300, [[UIScreen mainScreen] bounds].size.width, 300)];
    }
    else{
        specialityPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, [[UIScreen mainScreen] bounds].size.height+300, [[UIScreen mainScreen] bounds].size.width, 300)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(allInOnePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:specialityPickerViewDoneButtonTag];
    [specialityPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(allInOnePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [specialityPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* allergyTimePicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 250)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    
    [allergyTimePicker setTag:specialityPickerViewPickerTag];
    [specialityPickerView addSubview:allergyTimePicker];
    
    specialityPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:specialityPickerView];
}

-(void)allInOnePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        specialityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
   
    if (isSpecialityMenu){
        if ([strSpecialityID isEqualToString:@""]) {
            [self.specialityButton setTitle:[[specialityArray objectAtIndex:0]valueForKey:@"Text"] forState:UIControlStateNormal];
            
            strSpecialityID = [[specialityArray objectAtIndex:0]valueForKey:@"Value"] ;
        }
    }
    else{
        if ([strStateID isEqualToString:@"0"]) {
            [self.stateButton setTitle:[[stateArray objectAtIndex:0] valueForKey:@"StateName"] forState:UIControlStateNormal];
            
            strStateID = [[stateArray objectAtIndex:0] valueForKey:@"StateId"];
        }
    }
}

-(void)allInOnePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        specialityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
}

#pragma mark UIPickerView Delegates
- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView{
    return 1;
}

- (CGFloat)pickerView:(UIPickerView *)pickerView rowHeightForComponent:(NSInteger)component {
    return 45.0f;
}

- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component{
    if(isSpecialityMenu){
        return [specialityArray count];
    }
    else{
        return [stateArray count];
    }
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    if(isSpecialityMenu){
        strSpeciality = [[specialityArray objectAtIndex:row]valueForKey:@"Text"] ;
        return strSpeciality;
    }
    else{
        strState = [[stateArray objectAtIndex:row] valueForKey:@"StateName"];
        return strState;
    }
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    if(isSpecialityMenu){
        [self.specialityButton setTitle:[[specialityArray objectAtIndex:row]valueForKey:@"Text"] forState:UIControlStateNormal];
        strSpecialityID = [[specialityArray objectAtIndex:row]valueForKey:@"Value"] ;
    }
    else{
        [self.stateButton setTitle:[[stateArray objectAtIndex:row] valueForKey:@"StateName"] forState:UIControlStateNormal];
        strStateID = [[stateArray objectAtIndex:row] valueForKey:@"StateId"];
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
