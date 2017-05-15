//
//  AddActivityViewController.m
//  PHR
//
//  Created by CDAC HIED on 23/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "AddActivityViewController.h"
#import "Constants.h"

typedef enum : NSUInteger {
    activityDoneButtonTag=1500,
    activityPickerTag,
    dateDoneButtonTag,
    datePickerTag,
    timeDoneButtonTag,
    timePickerTag,
} activityTags;

@interface AddActivityViewController (){
    
    UIView* activityPickerView;
    UIView* datePickerView;
    UIView* timePickerView;
    
    NSMutableArray* activityNameArray;
    NSString* activityString;
    NSString* activityNameID;
}

- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)saveButtonAction:(id)sender;
- (IBAction)dateButtonAction:(id)sender;
- (IBAction)timeButtonAction:(id)sender;
- (IBAction)activityButtonAction:(id)sender;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;

@property (weak, nonatomic) IBOutlet UITextField *pathTextfield;
@property (weak, nonatomic) IBOutlet UITextField *distanceTextfield;
@property (weak, nonatomic) IBOutlet UIButton *dateButton;
@property (weak, nonatomic) IBOutlet UIButton *timeButton;
@property (weak, nonatomic) IBOutlet UIButton *activityButton;
@property (weak, nonatomic) IBOutlet UITextView *commentsTextview;
@property (weak, nonatomic) IBOutlet UIScrollView *activityScrollView;

@property (weak, nonatomic) IBOutlet UILabel *distanceLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentLabel;
@property (weak, nonatomic) IBOutlet UILabel *pathLabel;
@property (weak, nonatomic) IBOutlet UILabel *timeLabel;
@property (weak, nonatomic) IBOutlet UILabel *activityLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@end

@implementation AddActivityViewController

-(void)viewDidLayoutSubviews{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.activityScrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, self.view.frame.size.height+350)];
    }
    else{
        [self.activityScrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, self.view.frame.size.height+200)];
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
//    activityNameArray = [[NSMutableArray alloc]initWithObjects:@"Walking",@"Running",@"Cycling",@"StepsTaken",@"Swimming", nil];
    NSString *medicationNameFilePath = [[NSBundle mainBundle] pathForResource:@"ActivityName" ofType:@"plist"];
    activityNameArray = [[NSMutableArray alloc] initWithContentsOfFile:medicationNameFilePath];
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.activityScrollView addGestureRecognizer:singleFingerTap];
    
    activityNameID = @"";
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [self addActivityPickerView];
        [self addDatePickerView];
        [self addTimePickerView];
    });
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
//        [self.healthConditonScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
        self.titleLabel.font = [UIFont systemFontOfSize:22 weight:-1];
        
        self.activityButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.timeButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pathTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.distanceTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentsTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.distanceLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.pathLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.timeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.activityLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        //        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    self.activityButton.layer.cornerRadius = 3;
    self.activityButton.clipsToBounds = YES;
    self.dateButton.layer.cornerRadius = 3;
    self.dateButton.clipsToBounds = YES;
    self.timeButton.layer.cornerRadius = 3;
    self.timeButton.clipsToBounds = YES;
    self.commentsTextview.layer.cornerRadius = 3;
    self.commentsTextview.clipsToBounds = YES;
    self.commentsTextview.layer.borderWidth = 0.75f;
    self.commentsTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
    }];
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.activityScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+350)];
    }
    else{
        [self.activityScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+200)];
    }
}

#pragma mark Add Activity View
-(void)addActivityPickerView{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        activityPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 300)];
    }
    else{
        activityPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(activityPickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:activityDoneButtonTag];
    [activityPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(activityPickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [activityPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* activityPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 250)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
//    [activityPicker setBackgroundColor:[UIColor redColor]];
    [activityPicker setTag:activityPickerTag];
    [activityPickerView addSubview:activityPicker];
    
    activityPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:activityPickerView];
}

-(void)activityPickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
    }];
    
    if ([activityNameID isEqualToString:@""]) {
        activityNameID = [[activityNameArray objectAtIndex:0]valueForKey:@"ActivityId"];
    }
    
    [self.activityButton setTitle:activityString forState:UIControlStateNormal];
}

-(void)activityPickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
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
    
    return [activityNameArray count];
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    activityString = [[activityNameArray objectAtIndex:row] valueForKey:@"ActivityName"];
    return activityString;
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    [self.activityButton setTitle:[[activityNameArray objectAtIndex:row] valueForKey:@"ActivityName"] forState:UIControlStateNormal];
    activityNameID = [[activityNameArray objectAtIndex:row]valueForKey:@"ActivityId"];
    activityString = [[activityNameArray objectAtIndex:row] valueForKey:@"ActivityName"];
}

#pragma mark Create Date picker custom view 
-(void)addDatePickerView{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        datePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 300)];
    }
    else{
        datePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(doneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:dateDoneButtonTag];
    [datePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(cancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [datePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIDatePicker* datePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    datePicker.datePickerMode = UIDatePickerModeDate;
    datePicker.date = [NSDate date];
    datePicker.maximumDate = [NSDate date];
    
    [datePicker setTag:datePickerTag];
    [datePickerView addSubview:datePicker];
    
    datePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:datePickerView];
}

-(void)doneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
    }];
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
//    dateFormatter.dateFormat = @"yyyy-MM-dd";
    dateFormatter.dateFormat = @"dd-MM-yyyy";
    
    UIDatePicker* datePicker = [datePickerView viewWithTag:datePickerTag];
    
    [self.dateButton setTitle:[dateFormatter stringFromDate:[datePicker date]] forState:UIControlStateNormal];
    
}

-(void)cancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
    }];
}

#pragma mark Create Time picker custom view 
-(void)addTimePickerView{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        timePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 300)];
    }
    else{
        timePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(timeDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:timeDoneButtonTag];
    [timePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(timeCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [timePickerView addSubview:cancelButton];
    
//    NSDateFormatter* timeFormatter = [[NSDateFormatter alloc] init];
//    [timeFormatter setLocale:[[NSLocale alloc]
//                              initWithLocaleIdentifier:@"en_US"]];
//    timeFormatter.dateFormat = @"hh:mm";
    
    // adding Date Picker
    UIDatePicker* timePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    timePicker.datePickerMode = UIDatePickerModeTime;
    
    NSLocale *locale = [[NSLocale alloc] initWithLocaleIdentifier:@"NL"];
    
    [timePicker setLocale:locale];
    
    [timePicker setTag:timePickerTag];
    [timePickerView addSubview:timePicker];
    
    timePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:timePickerView];
}

-(void)timeDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
    }];
    NSDateFormatter* timeFormatter = [[NSDateFormatter alloc] init];
    [timeFormatter setLocale:[[NSLocale alloc]
                               initWithLocaleIdentifier:@"en_US"]];
    timeFormatter.dateFormat = @"HH:mm";
    
    UIDatePicker* datePicker = [timePickerView viewWithTag:timePickerTag];
    NSString* time = [timeFormatter stringFromDate:[datePicker date]];
    
    [self.timeButton setTitle:time forState:UIControlStateNormal];
    
}

-(void)timeCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
    }];
}

#pragma mark Text Field Delegate
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
    
    if (textField==self.distanceTextfield) {
        NSString *newString = [textField.text stringByReplacingCharactersInRange:range withString:string];
        if(([newString length] > 3)){
            [kAppDelegate showAlertView:@"Distance not more than 999"];
            return NO;
        }
    }
    return YES;
}

#pragma mark UIButton Actions
- (IBAction)saveButtonAction:(id)sender{
    
    if ([self.activityButton.titleLabel.text isEqualToString:@"Select Activity"]) {
        [kAppDelegate showAlertView:@"Select activity"];
    }
    else if (self.pathTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter path name"];
        [self.pathTextfield becomeFirstResponder];
    }
    else if (self.distanceTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter distance"];
        [self.distanceTextfield becomeFirstResponder];
    }
    else if ([self.dateButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select date"];
    }
    else if ([self.timeButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select time"];
    }
    //    else if (self.commentsTextview.text.length==0) {
    //        [kAppDelegate showAlertView:@"Enter comments"];
    //        [self.commentsTextview becomeFirstResponder];
    //    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.dateButton.titleLabel.text;
            NSDate* date = [dateFormatter dateFromString:deviceDate];
            
            NSString* activityDateString = [dateFormat stringFromDate:date];
//            activityDateString = [NSString stringWithFormat:@"%@ 00:00:00",activityDateString];
            
            NSString* dateString = [dateFormat stringFromDate:[NSDate date]];
            NSArray* array = [dateString componentsSeparatedByString:@"+"];
            dateString = [array objectAtIndex:0];
            
            NSString* timeString = self.timeButton.titleLabel.text;
            NSArray* timeArray = [timeString componentsSeparatedByString:@":"];
            int hours = [[timeArray objectAtIndex:0] intValue];
            int minutes = [[timeArray objectAtIndex:1] intValue];
            
            int hourInMinutes = hours*60;
            int totalTimeInMinutes = hourInMinutes+minutes;
            
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
                //  NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                    [kAppDelegate showAlertView:@"Activity added successfully"];
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

- (IBAction)dateButtonAction:(id)sender{
    [self.view endEditing:YES];
    
    UIDatePicker* datePicker = [datePickerView viewWithTag:datePickerTag];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            datePicker.frame = CGRectMake(0, 30, datePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[datePickerView viewWithTag:dateDoneButtonTag];
            doneButton.frame = CGRectMake(datePickerView.frame.size.width-70, 2, 60, 30);
            
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
        
    }];

}

- (IBAction)timeButtonAction:(id)sender{
    [self.view endEditing:YES];
    
    UIDatePicker* timePicker = [timePickerView viewWithTag:timePickerTag];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            timePicker.frame = CGRectMake(0, 30, timePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[timePickerView viewWithTag:timeDoneButtonTag];
            doneButton.frame = CGRectMake(timePickerView.frame.size.width-70, 2, 60, 30);
            
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
        
    }];

}

- (IBAction)activityButtonAction:(id)sender{
    UIPickerView* pickerView = [activityPickerView viewWithTag:activityPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            pickerView.frame = CGRectMake(0, 30, self.view.frame.size.width, 300);
            
            UIButton* doneButton = (UIButton*)[pickerView viewWithTag:activityDoneButtonTag];
            doneButton.frame = CGRectMake(pickerView.frame.size.width-70, 2, 60, 30);
            
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
        }
        else{
            activityPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            
            timePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        }
    }];
}

- (IBAction)dismissButtonAction:(id)sender {
    
    [self dismissViewControllerAnimated:YES completion:nil];
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
