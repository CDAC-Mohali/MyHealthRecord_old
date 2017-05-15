//
//  AddHealthConditionViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AddHealthConditionViewController.h"
#import "HealthConditionTableViewController.h"
#import "Constants.h"

typedef enum : NSUInteger {
    datePickerTag = 400,
    dateDoneButtonTag,
    diagnosisButtonTag,
    serviceButtonTag,
    
} HealthConditionTags;

@interface AddHealthConditionViewController (){
    UIView* diagnosisDatePickerView;
    
    NSString* isStill_Have_Allergy;
    int diagnosisFlag;
    
    NSMutableArray *healthProblemNameArray;
}

- (IBAction)yesButtonAction:(id)sender;
- (IBAction)noButtonAction:(id)sender;

- (IBAction)selectDiagnosisButtonAction:(id)sender;
//- (IBAction)selectServiceDateButtonAction:(id)sender;
- (IBAction)cancelButtonAction:(id)sender;
- (IBAction)addButtonAction:(id)sender;
- (IBAction)selectProblemButtonAction:(id)sender;

@property (weak, nonatomic) IBOutlet UILabel *problemNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *providerLabel;
@property (weak, nonatomic) IBOutlet UILabel *notesLabel;
@property (weak, nonatomic) IBOutlet UILabel *stillHaveLabel;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;

@property (weak, nonatomic) IBOutlet UIButton *selectDiagnosisDateButton;
@property (weak, nonatomic) IBOutlet UIButton *selectProblemButton;
//@property (weak, nonatomic) IBOutlet UIButton *selectServiceDateButton;
@property (weak, nonatomic) IBOutlet UITextField *providerTextfield;
@property (weak, nonatomic) IBOutlet UITextView *notesTextview;
@property (weak, nonatomic) IBOutlet UIButton *noButton;
@property (weak, nonatomic) IBOutlet UIButton *yesButton;
@property (weak, nonatomic) IBOutlet UIScrollView *healthConditonScrollView;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@end

@implementation AddHealthConditionViewController


-(void)viewWillLayoutSubviews{
    CGSize scrollableSize = CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+150);
    
    [self.healthConditonScrollView setContentSize:scrollableSize];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
//    dispatch_async(dispatch_get_main_queue(), ^{
//        [self getHealthProblemNameList];
//    });
    
//    NSString *healthProblemFilePath = [[NSBundle mainBundle] pathForResource:@"HealthProblems" ofType:@"plist"];
//    healthProblemNameArray = [[NSMutableArray alloc] initWithContentsOfFile:healthProblemFilePath];
    
    healthProblemNameArray = [NSMutableArray new];
    
    [self.selectDiagnosisDateButton setTag:diagnosisButtonTag];
//    [self.selectServiceDateButton setTag:serviceButtonTag];
    
    diagnosisFlag = 0;
    [self addDatePicker];
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.healthConditonScrollView addGestureRecognizer:singleFingerTap];
    
    isStill_Have_Allergy = @"";
    [kAppDelegate setHealthProblemNameButtonString:nil];

    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
    
        self.titleLabel.font = [UIFont systemFontOfSize:25 weight:-1];
        
        [self.healthConditonScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
        
        self.selectProblemButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.yesButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.noButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.selectDiagnosisDateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.providerTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.notesTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.problemNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.providerLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.notesLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.stillHaveLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        //        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    self.selectProblemButton.layer.cornerRadius = 3;
    self.selectProblemButton.clipsToBounds = YES;
    self.selectDiagnosisDateButton.layer.cornerRadius = 3;
    self.selectDiagnosisDateButton.clipsToBounds = YES;
    self.notesTextview.layer.cornerRadius = 3;
    self.notesTextview.clipsToBounds = YES;
    self.notesTextview.layer.borderWidth = 0.75f;
    self.notesTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
}

-(void)viewWillAppear:(BOOL)animated{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.healthConditonScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+350)];
    }
    else{
        [self.healthConditonScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+150)];
    }
    
    if ([kAppDelegate healthProblemNameButtonString].length) {
        [self.selectProblemButton setTitle:[[kAppDelegate healthProblemNameButtonString] capitalizedString] forState:UIControlStateNormal];
    }
    else{
        [self.selectProblemButton setTitle:@"Select Health Problem" forState:UIControlStateNormal];
    }
    
    [self prefersStatusBarHidden];
//    [[UIApplication sharedApplication] setStatusBarHidden:NO];
}

- (BOOL)prefersStatusBarHidden {
    
    return NO;
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    CGFloat screenWidth = screenRect.size.width;
    CGFloat screenHeight = screenRect.size.height;
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.healthConditonScrollView setContentSize:CGSizeMake(screenWidth, self.view.frame.size.height+350)];
        [self.healthConditonScrollView setFrame:CGRectMake(screenRect.origin.x, screenRect.origin.y, screenWidth, screenHeight)];
    }
    else{
        [self.healthConditonScrollView setContentSize:CGSizeMake(screenWidth, self.view.frame.size.height+150)];
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

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextView *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create Date picker custom view 
-(void)addDatePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        diagnosisDatePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 200)];
    }
    else{
        diagnosisDatePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(doneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:dateDoneButtonTag];
    [diagnosisDatePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(cancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [diagnosisDatePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIDatePicker* datePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    datePicker.datePickerMode = UIDatePickerModeDate;
    datePicker.date = [NSDate date];
    datePicker.maximumDate = [NSDate date];
    
    [datePicker setTag:datePickerTag];
    [diagnosisDatePickerView addSubview:datePicker];
    
    diagnosisDatePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:diagnosisDatePickerView];
}

-(void)doneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    //    dateFormatter.dateFormat = @"yyyy-MM-dd";
    dateFormatter.dateFormat = @"dd-MM-yyyy";
    
    UIDatePicker* datePicker = [diagnosisDatePickerView viewWithTag:datePickerTag];
    
    if (diagnosisFlag) {
        [self.selectDiagnosisDateButton setTitle:[dateFormatter stringFromDate:[datePicker date]] forState:UIControlStateNormal];
    }
//    else{
//        [self.selectServiceDateButton setTitle:[dateFormatter stringFromDate:[datePicker date]] forState:UIControlStateNormal];
//    }
}

-(void)cancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Get Health Problem name list API call
-(void)getHealthProblemNameList{
    
    if ([kAppDelegate hasInternetConnection]) {
//        [kAppDelegate showLoadingIndicator:@"getting..."];//Show loading indicator.
        
        NSURLSessionConfiguration *sessionConfiguration = [NSURLSessionConfiguration defaultSessionConfiguration];
        sessionConfiguration.HTTPAdditionalHeaders = @{
                                                       @"api-key"       : @"API_KEY",
                                                       @"Content-Type"  : @"application/json"
                                                       };
        NSURLSession *session = [NSURLSession sessionWithConfiguration:sessionConfiguration delegate:self delegateQueue:nil];
        
        NSString *searchString = @"\"a\"";
        
        NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"enter your web API url"]];
        NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
        request.HTTPBody = [searchString dataUsingEncoding:NSUTF8StringEncoding];
        request.HTTPMethod = @"POST";
        NSURLSessionDataTask *postDataTask = [session dataTaskWithRequest:request completionHandler:^(NSData *data, NSURLResponse *response, NSError *error) {
//            [kAppDelegate hideLoadingIndicator];
            if (!error) {
                id json = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
                NSLog(@"response is %@",json);
                
                healthProblemNameArray = json;
            }
            
            // The server answers with an error because it doesn't receive the params
        }];
        
        [postDataTask resume];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

- (IBAction)yesButtonAction:(id)sender {
    
    isStill_Have_Allergy = @"true";
    
    [self.yesButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
    [self.noButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
}

- (IBAction)noButtonAction:(id)sender {
    
    isStill_Have_Allergy = @"false";
    
    [self.yesButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    [self.noButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
}

- (IBAction)selectDiagnosisButtonAction:(id)sender {

    [self.view endEditing:YES];
    
    diagnosisFlag = 1;
    
    UIDatePicker* datePicker = [diagnosisDatePickerView viewWithTag:datePickerTag];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            datePicker.frame = CGRectMake(0, 30, diagnosisDatePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[diagnosisDatePickerView viewWithTag:dateDoneButtonTag];
            doneButton.frame = CGRectMake(diagnosisDatePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
        
//        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

- (IBAction)selectServiceDateButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    
    diagnosisFlag = 0;
    
    UIDatePicker* datePicker = [diagnosisDatePickerView viewWithTag:datePickerTag];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            datePicker.frame = CGRectMake(0, 30, diagnosisDatePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[diagnosisDatePickerView viewWithTag:dateDoneButtonTag];
            doneButton.frame = CGRectMake(diagnosisDatePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
        
        //        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
}

- (IBAction)cancelButtonAction:(id)sender {
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (IBAction)addButtonAction:(id)sender {
    if ([self.selectProblemButton.titleLabel.text isEqualToString:@"Select Health Problem"]) {
        [kAppDelegate showAlertView:@"Select problem name"];
    }
    else if ([self.selectDiagnosisDateButton.titleLabel.text isEqualToString:@"Select Date"]) {
        [kAppDelegate showAlertView:@"Select diagnosis date"];
    }
//    else if ([self.selectServiceDateButton.titleLabel.text isEqualToString:@"Select Date"]) {
//        [kAppDelegate showAlertView:@"Select service date"];
//    }
//    else if (self.providerTextfield.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter provider/facility"];
//    }
//    else if (self.notesTextview.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter notes detail"];
//    }
    else if ([isStill_Have_Allergy isEqualToString:@""]) {
        [kAppDelegate showAlertView:@"Select still have this problem"];
    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.selectDiagnosisDateButton.titleLabel.text;
            NSDate* date = [dateFormatter dateFromString:deviceDate];
            
            NSString* diagnosisDateString = [dateFormat stringFromDate:date];
//            NSString* diagnosisDateString = [NSString stringWithFormat:@"%@ 05:32:10",[self.selectDiagnosisDateButton titleLabel].text];
//            NSString* serviceDateString = [NSString stringWithFormat:@"%@ 00:00:00",[self.selectServiceDateButton titleLabel].text];
            
//            NSDate* dat = [dateFormat dateFromString:diagnosisDateString];
//            NSString* str = [dateFormat stringFromDate:dat];
            
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
                    [kAppDelegate showAlertView:@"Health Problem added successfully"];
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

- (IBAction)selectProblemButtonAction:(id)sender {
    
    [UIView animateWithDuration:0.75 animations:^{
        
        diagnosisDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
//    if ([healthProblemNameArray count]) {
        HealthConditionTableViewController* obj = [[HealthConditionTableViewController alloc]initWithNibName:@"HealthConditionTableViewController" bundle:nil];
        obj.healthConditionNameArray = healthProblemNameArray;
        [self presentViewController:obj animated:YES completion:nil];
//    }
//    else{
//        [kAppDelegate showAlertView:@"Getting list! Try later"];
//    }
    
}

@end
