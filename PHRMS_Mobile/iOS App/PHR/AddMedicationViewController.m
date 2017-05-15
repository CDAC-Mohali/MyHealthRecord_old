//
//  AddMedicationViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AddMedicationViewController.h"
#import "MedicationController/MedicationTableViewController.h"
#import "Constants.h"

typedef enum : NSUInteger {
    dateDoneButtonTag=600,
    datePickerTag,
    allInOnePickerViewDoneButtonTag,
    allInOnePickerViewPickerTag,
} medicationTags;

@interface AddMedicationViewController (){
    UIView* prescribedDatePickerView;
    UIView* allInOnePickerView;
    
    NSString* isStill_taking_medicine;
    NSString* routeString;
    NSString* dosageUnitString;
    NSString* dosageValueString;
    NSString* frequencyString;
    
    NSString* medicationRouteID;
    NSString* medicationDosageValueID;
    NSString* medicationDosageUnitID;
    NSString* medicationFrequencyID;
    
    NSMutableArray* routeArray;
    NSMutableArray* dosageValueArray;
    NSMutableArray* dosageUnitArray;
    NSMutableArray* frequencyTakenArray;
    
    NSMutableArray* base64ImagesArray;
    
    NSMutableArray* medicationNameArray;
    
    BOOL isRouteButton;
    BOOL isDosageValueButton;
    BOOL isDosageUnitButton;
    BOOL isFrequencyTakenButton;
    
    NSData *attachmentImageData;
}
- (IBAction)medicationNameButtonAction:(id)sender;
- (IBAction)takingMedicineYesButtonAction:(id)sender;
- (IBAction)takingMedicineNoButtonAction:(id)sender;
- (IBAction)saveButtonAction:(id)sender;
- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)prescribedButtonAction:(id)sender;
- (IBAction)routeButtonAction:(id)sender;
- (IBAction)dosageButtonAction:(id)sender;
- (IBAction)tabletButtonAction:(id)sender;
- (IBAction)frequencyButtonAction:(id)sender;
- (IBAction)attachmentButtonAction:(id)sender;

@property (weak, nonatomic) IBOutlet UIView *medicationView;
@property (weak, nonatomic) IBOutlet UIScrollView *medicationScrollView;
@property (weak, nonatomic) IBOutlet UIButton *medicationNameButton;
@property (weak, nonatomic) IBOutlet UIButton *takingMedicineYesButton;
@property (weak, nonatomic) IBOutlet UIButton *takingMedicineNoButton;
@property (weak, nonatomic) IBOutlet UIButton *prescribedButton;
@property (weak, nonatomic) IBOutlet UIButton *routeButton;
@property (weak, nonatomic) IBOutlet UIButton *dosageButton;
@property (weak, nonatomic) IBOutlet UIButton *tabletButton;
@property (weak, nonatomic) IBOutlet UIButton *frequencyButton;
@property (weak, nonatomic) IBOutlet UITextField *strengthTextview;
@property (weak, nonatomic) IBOutlet UITextView *instructionTextView;
@property (weak, nonatomic) IBOutlet UITextView *notesTextView;
@property (weak, nonatomic) IBOutlet UIImageView *attachment1ImageView;
//@property (weak, nonatomic) IBOutlet UIImageView *attachment2ImageView;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;
@property (weak, nonatomic) IBOutlet UIButton *attachmentButton;

@property (weak, nonatomic) IBOutlet UILabel *medicineNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *takingMedicationLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *routeLabel;
@property (weak, nonatomic) IBOutlet UILabel *strengthLabel;
@property (weak, nonatomic) IBOutlet UILabel *dosageLabel;
@property (weak, nonatomic) IBOutlet UILabel *frequencyLabel;
@property (weak, nonatomic) IBOutlet UILabel *instructionNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *noteLabel;
@property (weak, nonatomic) IBOutlet UILabel *attachmentLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@end

@implementation AddMedicationViewController

-(void)viewWillLayoutSubviews{
    
    [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+650)];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
//    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
//                                               initWithTarget:self action:@selector(handleSingleTap)];
//    
//    singleFingerTap.numberOfTapsRequired = 1;
    
    //[self.medicationScrollView addGestureRecognizer:singleFingerTap];
    
    isStill_taking_medicine = @"";
    medicationRouteID = @"";
    medicationDosageValueID = @"";
    medicationDosageUnitID = @"";
    medicationFrequencyID = @"";
    
//    medicationNameArray = [[NSMutableArray alloc] init];
    
    attachmentImageData = UIImagePNGRepresentation(self.attachment1ImageView.image);
    
    routeArray = [[NSMutableArray alloc] init] ;//]WithObjects:@"By mouth",@"To eyes",@"To skin ",@"To ears",@"To nose",@"Into the muscle",@"Injection",@"Inhaled",@"To mucous membrane",@"Intravenous",@"Into a joint",@"To vagina",@"Into the skin",@"Rectal",@"Implant",@"Under the tongue",@"Hemodialysis",@"Epidural",@"Into an artery",@"Into the eye",@"Into the bladder",@"Into the uterus",@"To tongue",@"To urethra",@"Into the trachea",@"To inner cheek",@"Dental",@"Into the penis",@"Into the peritoneum",@"Irrigation",@"Intrathecal",@"Into the pleura",@"In Vitro",@"Misc",@"Perfusion",@"Combination", nil];
    
    dosageValueArray = [[NSMutableArray alloc] init] ;//WithObjects:@"1/4",@"1/2",@"1",@"1 1/2",@"2",@"3",@"4",@"5",@"6",@"7",@"8",@"9",@"10", nil];
    
    dosageUnitArray = [[NSMutableArray alloc] init ];//WithObjects:@"tablet(s)",@"drop(s)",@"capsule(s)",@"tsp(s)",@"tbsp(s)",@"puff(s)",@"application(s)", nil];
    
//    frequencyTakenArray = [[NSMutableArray alloc] init ];//WithObjects:@"1 time per day",@"1 time per day in the morning",@"1 time per day in the evening",@"1 time per day at bedtime",@"2 times per day",@"3 times per day",@"4 times per day",@"Every hour",@"Every 2 hours",@"Every 3 hours",@"Every 4 hours",@"Every 6 hours",@"Every 8 hours",@"Every 12 hours",@"Every 24 hours",@"Every other day",@"1 time per week",@"Every two weeks",@"Every 28 days",@"Every 30 days",@"As needed", nil];
    
//    NSString *medicationNameFilePath = [[NSBundle mainBundle] pathForResource:@"Medication_Snow_Med" ofType:@"plist"];
//    medicationNameArray = [[NSMutableArray alloc] initWithContentsOfFile:medicationNameFilePath];
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
//        [self.healthConditonScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
        self.titleLabel.font = [UIFont systemFontOfSize:25 weight:-1];
        
        self.medicationNameButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.takingMedicineNoButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.takingMedicineYesButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.prescribedButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.routeButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.strengthTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dosageButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.tabletButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.frequencyButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.attachmentButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.medicineNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.takingMedicationLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.routeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.strengthLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dosageLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.frequencyLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.instructionNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.noteLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.attachmentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        //        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    
    medicationNameArray = [NSMutableArray new];
    
//    NSString *routeFilePath = [[NSBundle mainBundle] pathForResource:@"MedicationRouteList" ofType:@"plist"];
//    routeArray = [[NSMutableArray alloc] initWithContentsOfFile:routeFilePath];
//    
//    NSString *dosageValueFilePath = [[NSBundle mainBundle] pathForResource:@"MedicationDosageValueList" ofType:@"plist"];
//    dosageValueArray = [[NSMutableArray alloc] initWithContentsOfFile:dosageValueFilePath];
//    
//    NSString *dosageUnitFilePath = [[NSBundle mainBundle] pathForResource:@"MedicationDosageUnitList" ofType:@"plist"];
//    dosageUnitArray = [[NSMutableArray alloc] initWithContentsOfFile:dosageUnitFilePath];
//    
//    NSString *frequencyFilePath = [[NSBundle mainBundle] pathForResource:@"MedicationFrequencyList" ofType:@"plist"];
//    frequencyTakenArray = [[NSMutableArray alloc] initWithContentsOfFile:frequencyFilePath];
    
    self.medicationScrollView.delaysContentTouches = NO;
    
//    self.medicationView.frame = CGRectMake(self.medicationScrollView.frame.origin.x, self.medicationScrollView.frame.origin.y, self.medicationScrollView.frame.size.width, self.medicationScrollView.frame.size.height);
    
    [kAppDelegate setMedicationNameButtonString:nil];
    
    base64ImagesArray = [NSMutableArray new];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [self addPrescribedDatePicker];
        [self addAllInOnePickerView];
        
        [self getMedicationRouteList];
        [self getMedicationFrequencyList];
        [self getMedicationDosageUnitList];
        [self getMedicationDosageValueList];
    });
    
    self.medicationNameButton.layer.cornerRadius = 3;
    self.medicationNameButton.clipsToBounds = YES;
    self.prescribedButton.layer.cornerRadius = 3;
    self.prescribedButton.clipsToBounds = YES;
    self.routeButton.layer.cornerRadius = 3;
    self.routeButton.clipsToBounds = YES;
    self.dosageButton.layer.cornerRadius = 3;
    self.dosageButton.clipsToBounds = YES;
    self.tabletButton.layer.cornerRadius = 3;
    self.tabletButton.clipsToBounds = YES;
    self.frequencyButton.layer.cornerRadius = 3;
    self.frequencyButton.clipsToBounds = YES;
    self.attachmentButton.layer.cornerRadius = 3;
    self.attachmentButton.clipsToBounds = YES;
    self.instructionTextView.layer.cornerRadius = 3;
    self.instructionTextView.clipsToBounds = YES;
    self.instructionTextView.layer.borderWidth = 0.75f;
    self.instructionTextView.layer.borderColor = [[UIColor lightGrayColor] CGColor];
    self.notesTextView.layer.cornerRadius = 3;
    self.notesTextView.clipsToBounds = YES;
    self.notesTextView.layer.borderWidth = 0.75f;
    self.notesTextView.layer.borderColor = [[UIColor lightGrayColor] CGColor];
}

-(void)viewWillAppear:(BOOL)animated{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+650)];
    }
    else{
        [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+400)];
    }
    
    if ([kAppDelegate medicationNameButtonString].length) {
        [self.medicationNameButton setTitle:[[kAppDelegate medicationNameButtonString] capitalizedString] forState:UIControlStateNormal];
    }
    else{
        [self.medicationNameButton setTitle:@"Select Medication Name" forState:UIControlStateNormal];
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
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+650)];
    }
    else{
        [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+400)];
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

-(void)getMedicationNameList{
    if ([kAppDelegate hasInternetConnection]) {
        //        [kAppDelegate showLoadingIndicator:@"getting..."];//Show loading indicator.
        
        NSURLSessionConfiguration *sessionConfiguration = [NSURLSessionConfiguration defaultSessionConfiguration];
        sessionConfiguration.HTTPAdditionalHeaders = @{
                                                       @"api-key"       : @"API_KEY",
                                                       @"Content-Type"  : @"application/json"
                                                       };
        NSURLSession *session = [NSURLSession sessionWithConfiguration:sessionConfiguration delegate:self delegateQueue:nil];
        
        NSString *searchString = @"\"a\"";
        
        NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"enter your API url"]];
        NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
        request.HTTPBody = [searchString dataUsingEncoding:NSUTF8StringEncoding];
        request.HTTPMethod = @"POST";
        NSURLSessionDataTask *postDataTask = [session dataTaskWithRequest:request completionHandler:^(NSData *data, NSURLResponse *response, NSError *error) {
            //            [kAppDelegate hideLoadingIndicator];
            if (!error) {
                id json = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
                NSLog(@"response is %@",json);
                
                medicationNameArray = json;
            }
            
            // The server answers with an error because it doesn't receive the params
        }];
        
        [postDataTask resume];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
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
//    if (textField.keyboardType == UIKeyboardTypeNumberPad)
//    {
//        if ([string rangeOfCharacterFromSet:[[NSCharacterSet decimalDigitCharacterSet] invertedSet]].location != NSNotFound)
//        {
//            [kAppDelegate showAlertView:@"This field accepts only numeric entries."];
//            return NO;
//        }
//    }
//    
//    if (textField==self.strengthTextview) {
//        
//        NSString *newString = [textField.text stringByReplacingCharactersInRange:range withString:string];
//        if(([newString length] > 5)){
//            [kAppDelegate showAlertView:@"Strength length exceeded"];
//            return NO;
//        }
//    }
//    
//    return YES;
    
    if (textField==self.strengthTextview) {
        NSString *newStr = [textField.text stringByReplacingCharactersInRange:range withString:string];
        
        NSString *expression = @"^([0-9]*)(\\.([0-9]+)?)?$";
        
        NSRegularExpression *regex = [NSRegularExpression regularExpressionWithPattern:expression options:NSRegularExpressionCaseInsensitive error:nil];
        
        NSUInteger noOfMatches = [regex numberOfMatchesInString:newStr
                                                        options:0
                                                          range:NSMakeRange(0, [newStr length])];
        
        if(([newStr length] > 8)){
            [kAppDelegate showAlertView:@"Strength length exceeded"];
            return NO;
        }
        
        if (noOfMatches==0){
            [kAppDelegate showAlertView:@"Only numeric values"];
            return NO;
        }
    }
    return YES;
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextView *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Get Medication Route list API call
-(void)getMedicationRouteList{
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
                routeArray = [responseObject valueForKey:@"response"];
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

#pragma mark Get Medication Dosage Value list API call
-(void)getMedicationDosageValueList{
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
                dosageValueArray = [responseObject valueForKey:@"response"];
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

#pragma mark Get Medication Dosage Unit list API call
-(void)getMedicationDosageUnitList{
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
                dosageUnitArray = [responseObject valueForKey:@"response"];
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

#pragma mark Get Medication Frequency list API call
-(void)getMedicationFrequencyList{
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
                frequencyTakenArray = [responseObject valueForKey:@"response"];
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

#pragma mark Create Date picker custom view 
-(void)addPrescribedDatePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        prescribedDatePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 200)];
    }
    else{
        prescribedDatePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(doneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:dateDoneButtonTag];
    [prescribedDatePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(cancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [prescribedDatePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIDatePicker* datePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    datePicker.datePickerMode = UIDatePickerModeDate;
    datePicker.date = [NSDate date];
    datePicker.maximumDate = [NSDate date];
    
    [datePicker setTag:datePickerTag];
    [prescribedDatePickerView addSubview:datePicker];
    
    prescribedDatePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:prescribedDatePickerView];
}

-(void)doneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    //    dateFormatter.dateFormat = @"yyyy-MM-dd";
    dateFormatter.dateFormat = @"dd-MM-yyyy";
    
    UIDatePicker* datePicker = [prescribedDatePickerView viewWithTag:datePickerTag];
    
    [self.prescribedButton setTitle:[dateFormatter stringFromDate:[datePicker date]] forState:UIControlStateNormal];
    
}

-(void)cancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create AllInOnePickerView custom view 
-(void)addAllInOnePickerView{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        allInOnePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+800, self.view.frame.size.width, 300)];
    }
    else{
        allInOnePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(allInOnePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:allInOnePickerViewDoneButtonTag];
    [allInOnePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(allInOnePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [allInOnePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* allergyTimePicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 250)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    
    [allergyTimePicker setTag:allInOnePickerViewPickerTag];
    [allInOnePickerView addSubview:allergyTimePicker];
    
    allInOnePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:allInOnePickerView];
}

-(void)allInOnePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
    
    if (isRouteButton) {
//        [self.routeButton setTitle:routeString forState:UIControlStateNormal];
        if ([medicationRouteID isEqualToString:@""]) {
            medicationRouteID = [[[routeArray objectAtIndex:0] valueForKey:@"Id"] stringValue] ;
            [self.routeButton setTitle:[[routeArray objectAtIndex:0] valueForKey:@"Route"] forState:UIControlStateNormal];
        }
    }
    else if (isDosageValueButton){
//        [self.dosageButton setTitle:dosageValueString forState:UIControlStateNormal];
        if ([medicationDosageValueID isEqualToString:@""]) {
            medicationDosageValueID = [[[dosageValueArray objectAtIndex:0]valueForKey:@"Id"] stringValue];
            [self.dosageButton setTitle:[[dosageValueArray objectAtIndex:0]valueForKey:@"DosValue"] forState:UIControlStateNormal];
        }
    }
    else if (isDosageUnitButton){
//        [self.tabletButton setTitle:dosageUnitString forState:UIControlStateNormal];
        if ([medicationDosageUnitID isEqualToString:@""]) {
            medicationDosageUnitID = [[[dosageUnitArray objectAtIndex:0]valueForKey:@"Id"] stringValue];
            [self.tabletButton setTitle:[[dosageUnitArray objectAtIndex:0]valueForKey:@"DosUnit"] forState:UIControlStateNormal];
        }
    }
    else{
//        [self.frequencyButton setTitle:frequencyString forState:UIControlStateNormal];
        if ([medicationFrequencyID isEqualToString:@""]) {
            medicationFrequencyID = [[[frequencyTakenArray objectAtIndex:0]valueForKey:@"Id"] stringValue];
            [self.frequencyButton setTitle:[[frequencyTakenArray objectAtIndex:0]valueForKey:@"Frequency"] forState:UIControlStateNormal];
        }
    }
}

-(void)allInOnePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
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
    if (isRouteButton) {
        return [routeArray count];
    }
    else if(isDosageValueButton){
        return [dosageValueArray count];
    }
    else if(isDosageUnitButton){
        return [dosageUnitArray count];
    }
    else{
        return [frequencyTakenArray count];
    }
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    if (isRouteButton) {
        routeString = [[routeArray objectAtIndex:row] valueForKey:@"Route"] ;
        return routeString;
    }
    else if(isDosageValueButton){
        dosageValueString = [[dosageValueArray objectAtIndex:row]valueForKey:@"DosValue"] ;
        return dosageValueString;
    }
    else if(isDosageUnitButton){
        dosageUnitString = [[dosageUnitArray objectAtIndex:row]valueForKey:@"DosUnit"] ;
        return dosageUnitString;
    }
    else{
        frequencyString = [[frequencyTakenArray objectAtIndex:row]valueForKey:@"Frequency"];
        
//        allergyDurationID = [[allergyTimeArray objectAtIndex:row] valueForKey:@"Id"];
        return frequencyString;
    }
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    if (isRouteButton) {
        [self.routeButton setTitle:[[routeArray objectAtIndex:row] valueForKey:@"Route"] forState:UIControlStateNormal];
        medicationRouteID = [[[routeArray objectAtIndex:row] valueForKey:@"Id"] stringValue];
    }
    else if(isDosageValueButton){
        [self.dosageButton setTitle:[[dosageValueArray objectAtIndex:row]valueForKey:@"DosValue"] forState:UIControlStateNormal];
        medicationDosageValueID = [[[dosageValueArray objectAtIndex:row]valueForKey:@"Id"] stringValue];
    }
    else if(isDosageUnitButton){
        [self.tabletButton setTitle:[[dosageUnitArray objectAtIndex:row]valueForKey:@"DosUnit"] forState:UIControlStateNormal];
        medicationDosageUnitID = [[[dosageUnitArray objectAtIndex:row]valueForKey:@"Id"] stringValue] ;
    }
    else{
        [self.frequencyButton setTitle:[[frequencyTakenArray objectAtIndex:row]valueForKey:@"Frequency"] forState:UIControlStateNormal];
        medicationFrequencyID = [[[frequencyTakenArray objectAtIndex:row]valueForKey:@"Id"] stringValue];
    }
}

- (IBAction)medicationNameButtonAction:(id)sender {
    
    [UIView animateWithDuration:0.75 animations:^{
        
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
    
//    if ([medicationNameArray count]) {
        MedicationTableViewController* obj = [[MedicationTableViewController alloc]initWithNibName:@"MedicationTableViewController" bundle:nil];
        obj.medicationNameArray = medicationNameArray;
        [self presentViewController:obj animated:YES completion:nil];
//    }
//    else{
//        [kAppDelegate showAlertView:@"Getting list! Try later"];
//    }
}

#pragma mark Taking Medication YES/NO Buttons Action
- (IBAction)takingMedicineYesButtonAction:(id)sender{
    isStill_taking_medicine = @"true";
    
    [self.takingMedicineYesButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
    [self.takingMedicineNoButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
}

- (IBAction)takingMedicineNoButtonAction:(id)sender{
    isStill_taking_medicine = @"false";
    
    [self.takingMedicineYesButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    [self.takingMedicineNoButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
}

- (IBAction)prescribedButtonAction:(id)sender{
    
    [self.view endEditing:YES];
    
    UIDatePicker* datePicker = [prescribedDatePickerView viewWithTag:datePickerTag];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
            
            datePicker.frame = CGRectMake(0, 30, prescribedDatePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[prescribedDatePickerView viewWithTag:dateDoneButtonTag];
            doneButton.frame = CGRectMake(prescribedDatePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
        
        //        allergyTimePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

- (IBAction)routeButtonAction:(id)sender{
    isRouteButton = YES;
    isDosageValueButton = NO;
    isDosageUnitButton = NO;
    isFrequencyTakenButton = NO;
    
    UIPickerView* pickerView = [allInOnePickerView viewWithTag:allInOnePickerViewPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [pickerView selectRow:0 inComponent:0 animated:YES];
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 250);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            pickerView.frame = CGRectMake(0, 30, allInOnePickerView.frame.size.width, 250);
            
            UIButton* doneButton = (UIButton*)[allInOnePickerView viewWithTag:allInOnePickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(allInOnePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
    
//    [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+1800)];
}

- (IBAction)dosageButtonAction:(id)sender{
    isRouteButton = NO;
    isDosageValueButton = YES;
    isDosageUnitButton = NO;
    isFrequencyTakenButton = NO;
    
    UIPickerView* pickerView = [allInOnePickerView viewWithTag:allInOnePickerViewPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [pickerView selectRow:0 inComponent:0 animated:YES];
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 250);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            pickerView.frame = CGRectMake(0, 30, allInOnePickerView.frame.size.width, 250);
            
            UIButton* doneButton = (UIButton*)[allInOnePickerView viewWithTag:allInOnePickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(allInOnePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
}

- (IBAction)tabletButtonAction:(id)sender{
    isRouteButton = NO;
    isDosageValueButton = NO;
    isDosageUnitButton = YES;
    isFrequencyTakenButton = NO;
    
    UIPickerView* pickerView = [allInOnePickerView viewWithTag:allInOnePickerViewPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [pickerView selectRow:0 inComponent:0 animated:YES];
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 250);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            pickerView.frame = CGRectMake(0, 30, allInOnePickerView.frame.size.width, 250);
            
            UIButton* doneButton = (UIButton*)[allInOnePickerView viewWithTag:allInOnePickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(allInOnePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
}

- (IBAction)frequencyButtonAction:(id)sender{
    isRouteButton = NO;
    isDosageValueButton = NO;
    isDosageUnitButton = NO;
    isFrequencyTakenButton = YES;
    
    UIPickerView* pickerView = [allInOnePickerView viewWithTag:allInOnePickerViewPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [pickerView selectRow:0 inComponent:0 animated:YES];
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 250);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            pickerView.frame = CGRectMake(0, 30, allInOnePickerView.frame.size.width, 250);
            
            UIButton* doneButton = (UIButton*)[allInOnePickerView viewWithTag:allInOnePickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(allInOnePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
            allInOnePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
}

- (IBAction)attachmentButtonAction:(id)sender{
    [self.view endEditing:YES];
    
//    if (![attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachment1ImageView.image)] && ![attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachment2ImageView.image)]) {
//        [kAppDelegate showAlertView:@"Max no. of files are 2"];
//        return;
//    }
    
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:nil message:nil preferredStyle:UIAlertControllerStyleActionSheet];
    
    UIAlertAction* camera = [UIAlertAction actionWithTitle:@"Take Photo" style:UIAlertActionStyleDestructive handler:^(UIAlertAction *action) {
        
        UIImagePickerController *picker = [[UIImagePickerController alloc] init];
        picker.delegate = self;
        picker.allowsEditing = YES;
        picker.sourceType = UIImagePickerControllerSourceTypeCamera;
        
        [self presentViewController:picker animated:YES completion:NULL];
    }];
    
    UIAlertAction* photoGallary = [UIAlertAction actionWithTitle:@"Choose Photo" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        
        UIImagePickerController *picker = [[UIImagePickerController alloc] init];
        picker.delegate = self;
        picker.allowsEditing = YES;
        picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
        
        [self presentViewController:picker animated:YES completion:NULL];
        
    }];
    
    UIAlertAction* cancel = [UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
        
        
    }];
    
    alertController.popoverPresentationController.barButtonItem = nil;
    alertController.popoverPresentationController.sourceView = self.view;
    alertController.popoverPresentationController.sourceRect = CGRectMake(self.view.bounds.size.width/2+50, self.view.bounds.size.height-210, 1.0, 1.0);
    
    [alertController addAction:camera];
    [alertController addAction:photoGallary];
    [alertController addAction:cancel];
    
    [alertController setModalPresentationStyle:UIModalPresentationPopover];
    
    [self presentViewController:alertController animated:YES completion:nil];
}

#pragma mark ImagePicker Delegates
- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info {
    
    [picker dismissViewControllerAnimated:YES completion:nil];
    
    UIImage *image = [info objectForKey:UIImagePickerControllerOriginalImage];
    
    image = [kAppDelegate scaleAndRotateImage:image];
    
    NSString* strImage = [self imageToNSString:image];
    [base64ImagesArray addObject:strImage];
    
//    if ([attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachment1ImageView.image)]) {
        self.attachment1ImageView.image = image;
//    }
//    else{
//        self.attachment2ImageView.image = image;
//    }
}

-(NSString *)imageToNSString:(UIImage *)image
{
    NSData *imageData = UIImagePNGRepresentation(image);
    return [imageData base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
}

- (IBAction)saveButtonAction:(id)sender{
    
    if ([self.medicationNameButton.titleLabel.text isEqualToString:@"Select Medication Name"]) {
        [kAppDelegate showAlertView:@"Select medication name"];
    }
    else if ([isStill_taking_medicine isEqualToString:@""]) {
        [kAppDelegate showAlertView:@"Select taking medicine or not"];
    }
    else if ([[self.prescribedButton titleLabel].text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select prescribed date"];
    }
    else if ([self.routeButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select route"];
    }
//    else if (self.strengthTextview.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter strength"];
//        [self.strengthTextview becomeFirstResponder];
//    }
    else if ([self.dosageButton.titleLabel.text isEqualToString:@"Dosage"]) {
        [kAppDelegate showAlertView:@"Select dosage value"];
    }
    else if ([self.tabletButton.titleLabel.text isEqualToString:@"Dosage Unit"]) {
        [kAppDelegate showAlertView:@"Select dosage unit"];
    }
    else if ([self.frequencyButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select frequency"];
    }
//    else if (self.instructionTextView.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter instruction label"];
//    }
//    else if (self.notesTextView.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter notes"];
//    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.prescribedButton.titleLabel.text;
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
//            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
            
            [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
            
            manager.requestSerializer = requestSerializer;
            [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
                [kAppDelegate hideLoadingIndicator];
                NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                    [kAppDelegate showAlertView:@"Medication added successfully"];
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

- (IBAction)dismissButtonAction:(id)sender {
    [self dismissViewControllerAnimated:YES completion:nil];
}
@end
