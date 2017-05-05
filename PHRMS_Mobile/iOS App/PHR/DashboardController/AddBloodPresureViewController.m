//
//  AddBloodPresureViewController.m
//  PHR
//
//  Created by CDAC HIED on 22/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "AddBloodPresureViewController.h"
#import "Constants.h"

typedef enum : NSUInteger {
    dateDoneButtonTag=900,
    datePickerTag,
} bloodPressureTags;

@interface AddBloodPresureViewController (){
    
    UIView* collectionDatePickerView;
}
- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)saveButtonAction:(id)sender;
- (IBAction)collectionDateButtonAction:(id)sender;
@property (weak, nonatomic) IBOutlet UIButton *collectionDateButton;
@property (weak, nonatomic) IBOutlet UITextView *commentsTextview;
@property (weak, nonatomic) IBOutlet UITextField *systolicTextfield;
@property (weak, nonatomic) IBOutlet UITextField *diastolicTextfield;
@property (weak, nonatomic) IBOutlet UITextField *pulseTextfield;
@property (weak, nonatomic) IBOutlet UIScrollView *bpScrollView;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;

@property (weak, nonatomic) IBOutlet UILabel *resultLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@end

@implementation AddBloodPresureViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
        initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
                                               
    [self.bpScrollView addGestureRecognizer:singleFingerTap];
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        self.titleLabel.font = [UIFont systemFontOfSize:22 weight:-1];
        
        self.collectionDateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.systolicTextfield.font = [UIFont systemFontOfSize:12.0f weight:-1];
        self.diastolicTextfield.font = [UIFont systemFontOfSize:12.0f weight:-1];
        self.pulseTextfield.font = [UIFont systemFontOfSize:12.0f weight:-1];
        self.commentsTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.resultLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
    }
    self.collectionDateButton.layer.cornerRadius = 3;
    self.collectionDateButton.clipsToBounds = YES;
    self.commentsTextview.layer.cornerRadius = 3;
    self.commentsTextview.clipsToBounds = YES;
    self.commentsTextview.layer.borderWidth = 0.75f;
    self.commentsTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
    
    [self addCollectionDatePicker];
}

-(void)viewWillAppear:(BOOL)animated{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.bpScrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, self.view.frame.size.height+50)];
    }
    else{
        [self.bpScrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, self.view.frame.size.height+50)];
    }
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.bpScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+50)];
    }
    else{
        [self.bpScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+50)];
    }
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

-(void)textFieldDidEndEditing:(UITextField *)textField {
    
    /*if (textField==self.systolicTextfield) {
        
        NSString *newString = textField.text;
        if (newString.integerValue<90) {
            [kAppDelegate showAlertView:@"Systolic should be in between 90-250"];
            [self.systolicTextfield becomeFirstResponder];
        }
    }
    else if (textField==self.diastolicTextfield){
        NSString *newString = textField.text;
        if (newString.integerValue<60) {
            [kAppDelegate showAlertView:@"Diastolic should be in between 60-140"];
            [self.diastolicTextfield becomeFirstResponder];
        }
    }
    else if (textField==self.pulseTextfield){
        NSString *newString = textField.text;
        if (newString.integerValue<60) {
            [kAppDelegate showAlertView:@"Pulse should be in between 60-100"];
            [self.pulseTextfield becomeFirstResponder];
        }
    }*/
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
    
    if (textField==self.systolicTextfield) {
        
        NSString *newString = [textField.text stringByReplacingCharactersInRange:range withString:string];
        if(([newString length] > 3)){
            
            [kAppDelegate showAlertView:@"Systolic should be in between 90-250"];
            return NO;
        }
        else if(([newString length] > 2)){
            if (newString.integerValue>250 || newString.integerValue<90) {
                [kAppDelegate showAlertView:@"Systolic should be in between 90-250"];
                return NO;
            }
            return YES;
        }
    }
    else if (textField==self.diastolicTextfield) {
        
        NSString *newString = [textField.text stringByReplacingCharactersInRange:range withString:string];
        if(([newString length] > 3)){
            
            [kAppDelegate showAlertView:@"Diastolic should be in between 60-140"];
            return NO;
        }
        else if(([newString length] > 2)){
            if (newString.integerValue>140 || newString.integerValue<60) {
                [kAppDelegate showAlertView:@"Diastolic should be in between 60-140"];
                return NO;
            }
            return YES;
        }
    }
    else if (textField==self.pulseTextfield) {
        
        NSString *newString = [textField.text stringByReplacingCharactersInRange:range withString:string];
        if(([newString length] > 3)){
            
            [kAppDelegate showAlertView:@"Pulse should be in between 60-100"];
            return NO;
        }
        else if(([newString length] > 2)){
            if (newString.integerValue>100 || newString.integerValue<60) {
                [kAppDelegate showAlertView:@"Pulse should be in between 60-100"];
                return NO;
            }
            return YES;
        }
    }
    
    return YES;
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextView *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create Date picker custom view 
-(void)addCollectionDatePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        collectionDatePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 200)];
    }
    else{
        collectionDatePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(doneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:dateDoneButtonTag];
    [collectionDatePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(cancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [collectionDatePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIDatePicker* datePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    datePicker.datePickerMode = UIDatePickerModeDate;
    datePicker.date = [NSDate date];
    datePicker.maximumDate = [NSDate date];
    
    [datePicker setTag:datePickerTag];
    [collectionDatePickerView addSubview:datePicker];
    
    collectionDatePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:collectionDatePickerView];
}

-(void)doneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    //    dateFormatter.dateFormat = @"yyyy-MM-dd";
    dateFormatter.dateFormat = @"dd-MM-yyyy";
    
    UIDatePicker* datePicker = [collectionDatePickerView viewWithTag:datePickerTag];
    
    [self.collectionDateButton setTitle:[dateFormatter stringFromDate:[datePicker date]] forState:UIControlStateNormal];
    
}

-(void)cancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
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

- (IBAction)dismissButtonAction:(id)sender {
    
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (IBAction)saveButtonAction:(id)sender {
    
    if (self.systolicTextfield.text.length==0) {
        [self.systolicTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Enter systolic value"];
    }
    else if (self.systolicTextfield.text.integerValue<90 || self.systolicTextfield.text.integerValue>250){
        [self.systolicTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Systolic should be in between 90-250"];
    }
    else if (self.diastolicTextfield.text.length==0) {
        [self.diastolicTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Enter diastolic value"];
    }
    else if (self.diastolicTextfield.text.integerValue<60 || self.diastolicTextfield.text.integerValue>140) {
        [self.diastolicTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Diastolic should be in between 60-140"];
    }
    else if (self.pulseTextfield.text.length==0) {
        [self.pulseTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Enter pulse value"];
    }
    else if (self.pulseTextfield.text.integerValue<60 || self.pulseTextfield.text.integerValue>100){
        [self.pulseTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Pulse should be in between 60-100"];
    }
    else if ([self.collectionDateButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select BP collection date"];
    }
//    else if (self.commentsTextview.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter comments"];
//        [self.commentsTextview becomeFirstResponder];
//    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.collectionDateButton.titleLabel.text;
            NSDate* date = [dateFormatter dateFromString:deviceDate];
            
            NSString* prescribedDateString = [dateFormat stringFromDate:date];
            
            NSString* dateString = [dateFormat stringFromDate:[NSDate date]];
            NSArray* array = [dateString componentsSeparatedByString:@"+"];
            dateString = [array objectAtIndex:0];
            
            NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
            
            NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url
            
            //AFNetworking methods.
            AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
            AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
            
            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
            
            //            [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
            
            manager.requestSerializer = requestSerializer;
            [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
                [kAppDelegate hideLoadingIndicator];
//                NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                    [kAppDelegate showAlertView:@"Blood pressure values added successfully"];
                    [self dismissViewControllerAnimated:YES completion:nil];
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

- (IBAction)collectionDateButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    
    UIDatePicker* datePicker = [collectionDatePickerView viewWithTag:datePickerTag];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            datePicker.frame = CGRectMake(0, 30, collectionDatePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[collectionDatePickerView viewWithTag:dateDoneButtonTag];
            doneButton.frame = CGRectMake(collectionDatePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    
    }];
}
@end
