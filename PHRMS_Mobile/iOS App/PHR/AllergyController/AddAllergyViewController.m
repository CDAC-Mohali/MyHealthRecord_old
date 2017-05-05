//
//  AddAllergyViewController.m
//  PHR
//
//  Created by CDAC HIED on 17/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AddAllergyViewController.h"
#import "AllergyTableViewController.h"
#import "Constants.h"

//@import HealthKitUI;
//@import HealthKit;

typedef enum
{
    allergyNamePickerTag = 200,
    doneButtonTag,
    canelButtonTag,
    allergyTimePickerTag,
    allergyTimeDoneButtonTag,
    severityPickerTag,
    severityDoneButtonTag
    
}AddAllergyTags;

@interface AddAllergyViewController (){
    
    BOOL isAlergyName;
    BOOL isAlergySeverity;
    NSString* isStill_Have_Allergy;
    UIView* allergyNamePickerView;
    UIView* allergyTimePickerView;
    UIView* severityPickerView;
    
    NSString* allergyNameString;
    NSString* allergyTimeString;
    NSString* severityString;
    NSString* allergyDurationID;
    NSString* severityID;
    
    NSMutableArray* allergyNameArray;
    NSMutableArray* allergyTimeArray;
    NSMutableArray* severityArray;
    
    float stepValue;
    float severityStepValue;
}
@property (weak, nonatomic) IBOutlet UIScrollView *allergyScrollView;
@property (weak, nonatomic) IBOutlet UIButton *allergyNameButton;
@property (weak, nonatomic) IBOutlet UIButton *yesButton;
@property (weak, nonatomic) IBOutlet UIButton *noButton;
@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;
@property (weak, nonatomic) IBOutlet UIButton *allergyTimeButton;
@property (weak, nonatomic) IBOutlet UIButton *severityButton;
@property (weak, nonatomic) IBOutlet UITextView *notesTextview;
@property (weak, nonatomic) IBOutlet UISlider *allergyTimeSlider;
@property (weak, nonatomic) IBOutlet UISlider *allergySeveritySlider;
//@property(nonatomic, strong) HKActivitySummary *activitySummary;
@property (weak, nonatomic) IBOutlet UIView *allergyView;

@property (weak, nonatomic) IBOutlet UILabel *allergyTitle;
@property (weak, nonatomic) IBOutlet UILabel *allergyLabel;
@property (weak, nonatomic) IBOutlet UILabel *stillAllergyLabel;
@property (weak, nonatomic) IBOutlet UILabel *fromLabel;
@property (weak, nonatomic) IBOutlet UILabel *severityLabel;
@property (weak, nonatomic) IBOutlet UILabel *notesLabel;


- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)allergyNameButtonAction:(id)sender;
- (IBAction)yesButtonAction:(id)sender;
- (IBAction)noButtonAction:(id)sender;

- (IBAction)severityButtonAction:(id)sender;
- (IBAction)addButtonAction:(id)sender;
- (IBAction)allergyTimeButtonAction:(id)sender;
- (IBAction)valueChanged:(id)sender;
- (IBAction)severityValueChanged:(id)sender;

@end

@implementation AddAllergyViewController

-(void)viewWillLayoutSubviews{
    CGSize scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+180);
   
    [self.allergyScrollView setContentSize:scrollableSize];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
//    HKActivityRingView* ringView = [[HKActivityRingView alloc]initWithFrame:CGRectMake(100, 100, 200, 200)];
//    [self.allergyScrollView addSubview:ringView];
//    
//    HKUnit* unit = [HKUnit unitFromString:@"22"];
//    HKQuantity* quantity = [HKQuantity quantityWithUnit:unit doubleValue:2.0]
//    
//    [self.activitySummary setAppleExerciseTime:quantity];
//    
//    [ringView setActivitySummary:self.activitySummary animated:YES];
    
    dispatch_async(dispatch_get_main_queue(), ^{
//        [self getAllergyNameList];
        [self getAllergyDurationList];
        [self getAllergySeverityList];
        
        [self addAllergyNamePicker];
    });
//    [self addAllergyTimePicker];
//    [self addSeverityPicker];
    
    [self.notesTextview setDelegate:self];
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.allergyScrollView addGestureRecognizer:singleFingerTap];
    
    UITapGestureRecognizer *sliderTapGesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(sliderTapped:)];
    sliderTapGesture.numberOfTapsRequired = 1;
    [self.allergyTimeSlider addGestureRecognizer:sliderTapGesture];
    
    UITapGestureRecognizer *severitySliderTapGesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(severitySliderTapped:)];
    severitySliderTapGesture.numberOfTapsRequired = 1;
    [self.allergySeveritySlider addGestureRecognizer:severitySliderTapGesture];
    
    self.allergyTimeSlider.minimumValue = 0.0;
    self.allergyTimeSlider.maximumValue = 3.0;
    
    self.allergySeveritySlider.minimumValue = 0.0;
    self.allergySeveritySlider.maximumValue = 4.0;
    
    stepValue = 3.0;
    severityStepValue = 4.0;
    
    isStill_Have_Allergy = @"false";
    severityID = @"1";
    allergyDurationID = @"1";
    [kAppDelegate setAllergyNameButtonString:nil];
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
        [self.allergyScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+180)];
        
//        [self.allergyView setFrame:CGRectMake(0, 0,[[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+120)];
        
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.allergyNameButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.yesButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.noButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.allergyTitle.font = [UIFont systemFontOfSize:26.0f weight:-1];
        self.notesTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.allergyLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.stillAllergyLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.fromLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.severityLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.notesLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
//        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:32 weight:-1]
                  };
    }
    self.allergyNameButton.layer.cornerRadius = 3;
    self.allergyNameButton.clipsToBounds = YES;
    self.notesTextview.layer.cornerRadius = 3;
    self.notesTextview.clipsToBounds = YES;
    self.notesTextview.layer.borderWidth = 0.75f;
    self.notesTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
    if ([kAppDelegate allergyNameButtonString].length) {
        [self.allergyNameButton setTitle:[[kAppDelegate allergyNameButtonString] capitalizedString] forState:UIControlStateNormal];
    }
    else{
        [self.allergyNameButton setTitle:@"Select Allergy Name" forState:UIControlStateNormal];
    }
    
//    [[UIApplication sharedApplication] setStatusBarHidden:NO];
    [self prefersStatusBarHidden];
}

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
//    [UIView animateWithDuration:2.0 animations:^{
//        gradient.frame = self.view.bounds;
//    }];
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    //    CGRect screenRect = [[UIScreen mainScreen] bounds];
    //    CGFloat screenWidth = screenRect.size.width;
    //    CGFloat screenHeight = screenRect.size.height;
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.allergyScrollView setContentSize:CGSizeMake(self.allergyScrollView.frame.size.width, self.view.frame.size.height+300)];
        
        [self.notesTextview setFrame:CGRectMake(self.notesTextview.frame.origin.x, self.notesTextview.frame.origin.y, self.notesTextview.frame.size.width, 200)];
    }
    else{
        [self.allergyScrollView setContentSize:CGSizeMake(self.allergyScrollView.frame.size.width, self.view.frame.size.height+50)];
    }
}

- (BOOL)prefersStatusBarHidden {
    return NO;
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
        allergyNamePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        severityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        allergyNamePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        severityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        allergyNamePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        severityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Allergy Duration Slider Value Change Event 
-(IBAction)valueChanged:(id)sender {
    
    float newStep = roundf(self.allergyTimeSlider.value/self.allergyTimeSlider.maximumValue*stepValue)*self.allergyTimeSlider.maximumValue/stepValue;
    
    // Convert "steps" back to the context of the sliders values.
    [UIView animateWithDuration:0.5 animations:^{
        [self.allergyTimeSlider setValue:newStep animated:YES];
    }];
    
    int step = roundf(newStep);
//    NSLog(@"Steps number on slide: %d",step);
    
    allergyDurationID = [[[allergyTimeArray objectAtIndex:step] valueForKey:@"Id"] stringValue];
}

- (void)sliderTapped:(UIGestureRecognizer *)g {
    
    if (self.allergyTimeSlider.highlighted)
        return; // tap on thumb, let slider deal with it
    
    CGPoint pt = [g locationInView: self.allergyTimeSlider];
    CGFloat percentage = pt.x / self.allergyTimeSlider.bounds.size.width;
    CGFloat delta = percentage * (self.allergyTimeSlider.maximumValue - self.allergyTimeSlider.minimumValue);
    CGFloat value = self.allergyTimeSlider.minimumValue + delta;
    
    float newStep = roundf(value/self.allergyTimeSlider.maximumValue*stepValue)*self.allergyTimeSlider.maximumValue/stepValue;
    
    [UIView animateWithDuration:0.5 animations:^{
        [self.allergyTimeSlider setValue:newStep animated:YES];
    }];
    
    int step = roundf(newStep);
//    NSLog(@"Steps number on tap: %d",step);
    
    allergyDurationID = [[[allergyTimeArray objectAtIndex:step] valueForKey:@"Id"] stringValue];
}

#pragma mark Severity Slider Value Change Event 
- (IBAction)severityValueChanged:(id)sender{
    
    float newStep = roundf(self.allergySeveritySlider.value/self.allergySeveritySlider.maximumValue*severityStepValue)*self.allergySeveritySlider.maximumValue/severityStepValue;
    
    // Convert "steps" back to the context of the sliders values.
    [UIView animateWithDuration:0.5 animations:^{
        [self.allergySeveritySlider setValue:newStep animated:YES];
    }];
    
    int step = roundf(newStep);
    //    NSLog(@"Steps number on slide: %d",step);
     
    severityID = [[[severityArray objectAtIndex:step] valueForKey:@"Id"] stringValue];
}

- (void)severitySliderTapped:(UIGestureRecognizer *)g {
    
    if (self.allergySeveritySlider.highlighted)
        return; // tap on thumb, let slider deal with it
    
    CGPoint pt = [g locationInView: self.allergySeveritySlider];
    CGFloat percentage = pt.x / self.allergySeveritySlider.bounds.size.width;
    CGFloat delta = percentage * (self.allergySeveritySlider.maximumValue - self.allergySeveritySlider.minimumValue);
    CGFloat value = self.allergySeveritySlider.minimumValue + delta;
    
    float newStep = roundf(value/self.allergySeveritySlider.maximumValue*severityStepValue)*self.allergySeveritySlider.maximumValue/severityStepValue;
    
    [UIView animateWithDuration:0.5 animations:^{
        [self.allergySeveritySlider setValue:newStep animated:YES];
    }];
    
    int step = roundf(newStep);
    //    NSLog(@"Steps number on tap: %d",step);
    
    severityID = [[[severityArray objectAtIndex:step] valueForKey:@"Id"] stringValue];
}

#pragma mark Get Allergy name list API call
-(void)getAllergyNameList{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"getting..."];//Show loading indicator.
        
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
            [kAppDelegate hideLoadingIndicator];
            if (!error) {
                id json = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
                NSLog(@"response is %@",json);
                
                allergyNameArray = json;
            }
            
            // The server answers with an error because it doesn't receive the params
        }];
        
        [postDataTask resume];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Get Allergy severity list API call
-(void)getAllergySeverityList{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"getting..."];//Show loading indicator.
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                severityArray = [responseObject valueForKey:@"response"];
                
//                NSMutableArray* arrString = [NSMutableArray new];
//                for (int i=0; i<[severityArray count]; i++) {
//
//                    NSArray* arr = [[[severityArray objectAtIndex:i] valueForKey:@"Severity"] componentsSeparatedByString:@" "];
//
//                    NSString* str = [arr objectAtIndex:1];
//                    [arrString addObject:str];
//                }
//                severityArray = nil;
//                severityArray = arrString;
//                arrString = nil;
            }
            else{
                [kAppDelegate showAlertView:@"failed"];
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

#pragma mark Get Allergy duration list API call
-(void)getAllergyDurationList{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"getting..."];//Show loading indicator.
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                allergyTimeArray = [responseObject valueForKey:@"response"];
            }
            else{
                [kAppDelegate showAlertView:@"failed"];
            }
            
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"failed"];
        }];    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Create allergy name picker custom view 
-(void)addAllergyNamePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        allergyNamePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 200)];
    }
    else{
        allergyNamePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(allergyNamePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:doneButtonTag];
    [allergyNamePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(allergyNamePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [allergyNamePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* allergyNamePicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    
    [allergyNamePicker setTag:allergyNamePickerTag];
    [allergyNamePickerView addSubview:allergyNamePicker];
    
    allergyNamePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:allergyNamePickerView];
}

-(void)allergyNamePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        allergyNamePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
    [self.allergyNameButton setTitle:allergyNameString forState:UIControlStateNormal];
}

-(void)allergyNamePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        allergyNamePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create allergy time picker custom view 
-(void)addAllergyTimePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        allergyTimePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 200)];
    }
    else{
        allergyTimePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(allergyTimePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:allergyTimeDoneButtonTag];
    [allergyTimePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(allergyTimePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [allergyTimePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* allergyTimePicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    
    [allergyTimePicker setTag:allergyTimePickerTag];
    [allergyTimePickerView addSubview:allergyTimePicker];
    
    allergyTimePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:allergyTimePickerView];
}

-(void)allergyTimePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
    [self.allergyTimeButton setTitle:allergyTimeString forState:UIControlStateNormal];
}

-(void)allergyTimePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create severity picker custom view 
-(void)addSeverityPicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        severityPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 200)];
    }
    else{
        severityPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(severityPickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:severityDoneButtonTag];
    [severityPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(severityPickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [severityPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* severityPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    
    [severityPicker setTag:severityPickerTag];
    [severityPickerView addSubview:severityPicker];
    
    severityPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:severityPickerView];
}

-(void)severityPickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        severityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
    [self.severityButton setTitle:severityString forState:UIControlStateNormal];
}

-(void)severityPickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        severityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
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
//    if (isAlergyName) {
        return [allergyNameArray count];
//    }
//    else if(isAlergySeverity){
//        return [severityArray count];
//    }
//    else{
//        return [allergyTimeArray count];
//    }
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
//    if (isAlergyName) {
        allergyNameString = [[allergyNameArray objectAtIndex:row] valueForKey:@"AllergyName"];
        return allergyNameString;
//    }
//    else if(isAlergySeverity){
//        severityString = [[severityArray objectAtIndex:row] valueForKey:@"Severity"];
////        severityID = [[severityArray objectAtIndex:row] valueForKey:@"Id"];
//        return severityString;
//    }
//    else{
//        allergyTimeString = [[allergyTimeArray objectAtIndex:row] valueForKey:@"Duration"];
////        allergyDurationID = [[allergyTimeArray objectAtIndex:row] valueForKey:@"Id"];
//        return allergyTimeString;
//    }
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
//    if (isAlergyName) {
        allergyNameString = [[allergyNameArray objectAtIndex:row] valueForKey:@"AllergyName"];
        [self.allergyNameButton setTitle:allergyNameString forState:UIControlStateNormal];
//    }
//    else if(isAlergySeverity){
//        severityString = [[severityArray objectAtIndex:row] valueForKey:@"Severity"];
//        severityID = [[[severityArray objectAtIndex:row] valueForKey:@"Id"] stringValue];
//        [self.severityButton setTitle:severityString forState:UIControlStateNormal];
//    }
//    else{
//        allergyTimeString = [[allergyTimeArray objectAtIndex:row] valueForKey:@"Duration"];
//        allergyDurationID = [[[allergyTimeArray objectAtIndex:row] valueForKey:@"Id"] stringValue];
//        [self.allergyTimeButton setTitle:allergyTimeString forState:UIControlStateNormal];
//    }
}

#pragma mark Cancel Button Action
- (IBAction)dismissButtonAction:(id)sender {
    
    [self dismissViewControllerAnimated:YES completion:nil];
}

#pragma mark Select Allergy Button Action
- (IBAction)allergyNameButtonAction:(id)sender {
    isAlergyName = YES;
    isAlergySeverity = NO;
    
//    UIPickerView* picker = (UIPickerView*)[allergyNamePickerView viewWithTag:allergyNamePickerTag];
//    
//    picker.dataSource = self;
//    picker.delegate = self;
    
    [UIView animateWithDuration:0.75 animations:^{
    
        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        severityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
//    if ([allergyNameArray count]) {
        AllergyTableViewController* obj = [[AllergyTableViewController alloc]initWithNibName:@"AllergyTableViewController" bundle:nil];
        obj.allergyNameArray = allergyNameArray;
        [self presentViewController:obj animated:YES completion:nil];
//    }
//    else{
//        [kAppDelegate showAlertView:@"Getting list! Try later"];
//    }
}

#pragma mark YES/NO Buttons Action
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

#pragma mark Severity Button Action
- (IBAction)severityButtonAction:(id)sender {
    isAlergyName = NO;
    isAlergySeverity = YES;
    
    UIPickerView* picker = (UIPickerView*)[severityPickerView viewWithTag:severityPickerTag];
    
    picker.dataSource = self;
    picker.delegate = self;
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            severityPickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            picker.frame = CGRectMake(0, 30, severityPickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[severityPickerView viewWithTag:severityDoneButtonTag];
            doneButton.frame = CGRectMake(severityPickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            severityPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
        
        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Save Button Action
- (IBAction)addButtonAction:(id)sender {
    
    if ([[self.allergyNameButton titleForState:UIControlStateNormal]isEqualToString:@"Select Allergy Name"]) {
        [kAppDelegate showAlertView:@"Select allergy name"];
    }
    else if ([[self.allergyTimeButton titleForState:UIControlStateNormal] isEqualToString:@"Select Time Of Allergy"]){
        [kAppDelegate showAlertView:@"Select allergy duration"];
    }
    else if ([[self.severityButton titleForState:UIControlStateNormal] isEqualToString:@"Select Severity"]){
        [kAppDelegate showAlertView:@"Select allergy severity"];
    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];

            if ([allergyDurationID isEqualToString:@""]) {
                allergyDurationID = [[allergyTimeArray objectAtIndex:0] valueForKey:@"Id"];
            }
            if ([severityID isEqualToString:@""]) {
                severityID = [[severityArray objectAtIndex:0] valueForKey:@"Id"];
            }
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSString* dateString = [dateFormat stringFromDate:[NSDate date]];
            NSArray* array = [dateString componentsSeparatedByString:@"+"];
            dateString = [array objectAtIndex:0];
            
            NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
            
            NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
//            NSError* error;
//            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dicParams options:0 error:&error];
//            NSMutableDictionary *json=[NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingAllowFragments error:&error];
            
            //AFNetworking methods.
            AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
            AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
            
            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];

//            [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
            
            manager.requestSerializer = requestSerializer;
            [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
                [kAppDelegate hideLoadingIndicator];
                NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                    [kAppDelegate showAlertView:@"Allergy added successfully"];
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

#pragma mark Allergy Time Button Action
- (IBAction)allergyTimeButtonAction:(id)sender {
    isAlergyName = NO;
    isAlergySeverity = NO;
    
    UIPickerView* picker = (UIPickerView*)[allergyTimePickerView viewWithTag:allergyTimePickerTag];
    
    picker.dataSource = self;
    picker.delegate = self;
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            picker.frame = CGRectMake(0, 30, allergyTimePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[allergyTimePickerView viewWithTag:allergyTimeDoneButtonTag];
            doneButton.frame = CGRectMake(allergyTimePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
        
        severityPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}
@end
