//
//  AddDiabetesViewController.m
//  PHR
//
//  Created by CDAC HIED on 23/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "AddDiabetesViewController.h"
#import "Constants.h"

typedef enum : NSUInteger {
    dateDoneButtonTag=1000,
    datePickerTag,
    valueTypePickerViewDoneButtonTag,
    valueTypePickerViewPickerTag,
} bloodGlucoseTags;

@interface AddDiabetesViewController (){
    
    UIView* collectionDatePickerView;
    UIView* valueTypePickerView;
    
    NSMutableArray* valueTypeArray;
}
- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)saveButtonAction:(id)sender;
- (IBAction)collectionDateButtonAction:(id)sender;
- (IBAction)valueTypeButtonAction:(id)sender;

@property (weak, nonatomic) IBOutlet UILabel *resultLabel;
@property (weak, nonatomic) IBOutlet UILabel *collectionDateLabel;
@property (weak, nonatomic) IBOutlet UILabel *valueTypeLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;

@property (weak, nonatomic) IBOutlet UITextField *resultTextfield;
@property (weak, nonatomic) IBOutlet UIButton *collectionDateButton;
@property (weak, nonatomic) IBOutlet UIButton *valueTypeButton;
@property (weak, nonatomic) IBOutlet UITextView *commentsTextview;
@property (weak, nonatomic) IBOutlet UIScrollView *diabetesScrollView;

@end

@implementation AddDiabetesViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.diabetesScrollView addGestureRecognizer:singleFingerTap];
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
//        [self.diabetesScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
//        [self.diabetesScrollView setFrame:CGRectMake(0, 0, [[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
        self.titleLabel.font = [UIFont systemFontOfSize:22 weight:-1];
        
        self.collectionDateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.valueTypeButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.resultTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentsTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.resultLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.collectionDateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentsLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.valueTypeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        //        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    
    valueTypeArray = [[NSMutableArray alloc] initWithObjects:@"Fasting",@"Pre Breakfast",@"After Breakfast",@"Pre Noon Meal",@"After Noon Meal",@"Pre Dinner",@"After Dinner",@"Different Food",@"Bed Time",@"During Night",@"Pre Exercise",@"After Exercise", nil];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [self addCollectionDatePicker];
        [self valueTypePickerView];
    });
    
    self.collectionDateButton.layer.cornerRadius = 3;
    self.collectionDateButton.clipsToBounds = YES;
    self.valueTypeButton.layer.cornerRadius = 3;
    self.valueTypeButton.clipsToBounds = YES;
    self.commentsTextview.layer.cornerRadius = 3;
    self.commentsTextview.clipsToBounds = YES;
    self.commentsTextview.layer.borderWidth = 0.75f;
    self.commentsTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
}

-(void)viewWillAppear:(BOOL)animated{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.diabetesScrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, self.view.frame.size.height+50)];
    }
    else{
        [self.diabetesScrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, self.view.frame.size.height+50)];
    }
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.diabetesScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+50)];
    }
    else{
        [self.diabetesScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+50)];
    }
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
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
    if(!([newString length] > 3)){
        if (newString.integerValue>500) {
            [kAppDelegate showAlertView:@"Result value should not be more than 500"];
            return NO;
        }
        return YES;
    }
    else{
        [kAppDelegate showAlertView:@"Result value should not be more than 500"];
        return NO;
    }
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextView *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
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

#pragma mark Create Value Type PickerView custom view 
-(void)valueTypePickerView{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        valueTypePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 300)];
    }
    else{
        valueTypePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(allInOnePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:valueTypePickerViewDoneButtonTag];
    [valueTypePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(allInOnePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [valueTypePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* allergyTimePicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 250)];
//    genderPicker.dataSource = self;
//    genderPicker.delegate = self;
    
    [allergyTimePicker setTag:valueTypePickerViewPickerTag];
    [valueTypePickerView addSubview:allergyTimePicker];
    
    valueTypePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:valueTypePickerView];
}

-(void)allInOnePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
    
//    [self.valueTypeButton setTitle:routeString forState:UIControlStateNormal];

}

-(void)allInOnePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
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
   
    return [valueTypeArray count];
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component{
    
    return [valueTypeArray objectAtIndex:row];
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component{
   
    [self.valueTypeButton setTitle:[valueTypeArray objectAtIndex:row] forState:UIControlStateNormal];
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
    
    if (self.resultTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter result values"];
        [self.resultTextfield becomeFirstResponder];
    }
    else if ([self.collectionDateButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select collection date"];
    }
    else if ([self.valueTypeButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select value type"];
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
                    [kAppDelegate showAlertView:@"Blood glucose values added successfully"];
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
            
            valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
            
            datePicker.frame = CGRectMake(0, 30, collectionDatePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[collectionDatePickerView viewWithTag:dateDoneButtonTag];
            doneButton.frame = CGRectMake(collectionDatePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
        
    }];
}

- (IBAction)valueTypeButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    
    UIPickerView* datePicker = [valueTypePickerView viewWithTag:valueTypePickerViewPickerTag];
    [datePicker setDataSource:self];
    [datePicker setDelegate:self];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            collectionDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 250);
            
            datePicker.frame = CGRectMake(0, 30, valueTypePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[valueTypePickerView viewWithTag:valueTypePickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(valueTypePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            valueTypePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
}
@end
