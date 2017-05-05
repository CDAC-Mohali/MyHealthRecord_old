//
//  AddWeightViewController.m
//  PHR
//
//  Created by CDAC HIED on 19/07/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "AddWeightViewController.h"
#import "Constants.h"

typedef enum : NSUInteger {
    dateDoneButtonTag=1600,
    datePickerTag
} weightTags;


@interface AddWeightViewController (){
    UIView* collectionDatePickerView;
}
- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)saveButtonAction:(id)sender;
- (IBAction)collectionDateButtonAction:(id)sender;
@property (weak, nonatomic) IBOutlet UITextField *weightTextfield;
@property (weak, nonatomic) IBOutlet UITextField *heightTextfield;
@property (weak, nonatomic) IBOutlet UIButton *dateButton;
@property (weak, nonatomic) IBOutlet UITextView *commentsTextview;
@property (weak, nonatomic) IBOutlet UIScrollView *weightScrollView;
@property (weak, nonatomic) IBOutlet UILabel *bmiLabel;

@property (weak, nonatomic) IBOutlet UILabel *weightLabel;
@property (weak, nonatomic) IBOutlet UILabel *heightLabel;
@property (weak, nonatomic) IBOutlet UILabel *bmiTitleLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;

@end

@implementation AddWeightViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
//        [self.healthConditonScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
        self.titleLabel.font = [UIFont systemFontOfSize:22 weight:-1];
        
        self.dateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.weightTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.heightTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.bmiLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentsTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.weightLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.heightLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.bmiTitleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentsLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        //        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    
    [self.weightScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+270)];
    
    [self.weightScrollView addGestureRecognizer:singleFingerTap];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [self addCollectionDatePicker];
    });
    
    self.dateButton.layer.cornerRadius = 3;
    self.dateButton.clipsToBounds = YES;
    self.commentsTextview.layer.cornerRadius = 3;
    self.commentsTextview.clipsToBounds = YES;
    self.commentsTextview.layer.borderWidth = 0.75f;
    self.commentsTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark BMI Calculator 
-(NSString*)BMICalculator:(int)weight bodyHeight:(int)height{
    
    double heigthInMetre = height/100.0f;
    
    double totalHeight = (heigthInMetre*heigthInMetre);
    double bmi = weight/totalHeight;
    
    return [NSString stringWithFormat:@"Your BMI: %.02f",bmi];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

-(void)textFieldDidEndEditing:(UITextField *)textField {
    if (textField==self.heightTextfield && self.heightTextfield.text.length>0) {
        if (self.weightTextfield.text.length>0) {
            int weight = [self.weightTextfield.text intValue];
            int height = [self.heightTextfield.text intValue];
            
            self.bmiLabel.text = [self BMICalculator:weight bodyHeight:height];
        }
        else{
            [kAppDelegate showAlertView:@"enter weight"];
            [self.weightTextfield becomeFirstResponder];
        }
    }
    else if (textField==self.weightTextfield && self.heightTextfield.text.length>0) {
        if (self.weightTextfield.text.length>0) {
            int weight = [self.weightTextfield.text intValue];
            int height = [self.heightTextfield.text intValue];
            
            self.bmiLabel.text = [self BMICalculator:weight bodyHeight:height];
        }
        else{
            [kAppDelegate showAlertView:@"enter weight"];
            [self.weightTextfield becomeFirstResponder];
        }
    }
//    else {
//        [kAppDelegate showAlertView:@"enter height"];
//        [self.heightTextfield becomeFirstResponder];
//    }
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
    
    if (textField == self.weightTextfield || textField == self.heightTextfield) {
        NSString *newString = [textField.text stringByReplacingCharactersInRange:range withString:string];
        if (newString.length>3){
            [kAppDelegate showAlertView:@"Value exceed maximum limit"];
            return NO;
        }
    }
    return YES;
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
    
    [self.dateButton setTitle:[dateFormatter stringFromDate:[datePicker date]] forState:UIControlStateNormal];
}

-(void)cancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

- (IBAction)dismissButtonAction:(id)sender{
    
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (IBAction)saveButtonAction:(id)sender{
    
    if (self.weightTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter weight values"];
        [self.weightTextfield becomeFirstResponder];
    }
    else if (self.heightTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter height values"];
        [self.heightTextfield becomeFirstResponder];
    }
    else if ([self.dateButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select collection date"];
    }
    //    else if (self.commentsTextview.text.length==0) {
    //        [kAppDelegate showAlertView:@"Enter comments"];
    //        [self.commentsTextview becomeFirstResponder];
    //    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.dateButton.titleLabel.text;
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
                    [kAppDelegate showAlertView:@"Weight values added successfully"];
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

- (IBAction)collectionDateButtonAction:(id)sender{
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
