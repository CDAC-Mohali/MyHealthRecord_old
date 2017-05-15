//
//  SharingViewController.m
//  PHR
//
//  Created by CDAC HIED on 29/12/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "SharingViewController.h"
#import "AddMedicalContactViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"

typedef enum : NSUInteger {
    doctorPickerViewDoneButtonTag = 1500,
    doctorPickerViewPickerTag
} SharingTags;

@interface SharingViewController (){
    float stepValue;
    
    UIView* doctorPickerView;
    NSMutableArray* doctorArray;
    NSString* strSelectedDoctor;
    NSString* strDoctorID;
    
    NSString* strChecks;
    NSString* strValidUpto;
    NSString* strDoctorEmail;
    NSString* strDoctorMobile;
    
    BOOL isLoadingList;
    
    SWRevealViewController *revealController;
}
@property (nonatomic,strong) UIAlertView *logoutAlertView;

@property (weak, nonatomic) IBOutlet UIScrollView *sharingScrollView;
@property (weak, nonatomic) IBOutlet UISlider *sharingSlider;
@property (weak, nonatomic) IBOutlet UITextView *queryTextView;

@property (weak, nonatomic) IBOutlet UIButton *addDoctorButton;
@property (weak, nonatomic) IBOutlet UIButton *selectAllButton;
@property (weak, nonatomic) IBOutlet UIButton *deSelectAllButton;
@property (weak, nonatomic) IBOutlet UIButton *allergyButton;
@property (weak, nonatomic) IBOutlet UIButton *proceduresButton;
@property (weak, nonatomic) IBOutlet UIButton *wellnessActivitiesButton;
@property (weak, nonatomic) IBOutlet UIButton *wellnessBMIButton;
@property (weak, nonatomic) IBOutlet UIButton *wellnessBPButton;
@property (weak, nonatomic) IBOutlet UIButton *wellnessBGButton;
@property (weak, nonatomic) IBOutlet UIButton *immunizationButton;
@property (weak, nonatomic) IBOutlet UIButton *labTestsButton;
@property (weak, nonatomic) IBOutlet UIButton *medicationButton;
@property (weak, nonatomic) IBOutlet UIButton *problemsButton;
@property (weak, nonatomic) IBOutlet UIButton *selectDoctorButton;

- (IBAction)addDoctorButtonAction:(id)sender;
- (IBAction)valueChanged:(id)sender;
- (IBAction)selectAllButtonAction:(id)sender;
- (IBAction)deSelectAllButtonAction:(id)sender;
- (IBAction)allergyButtonAction:(id)sender;
- (IBAction)procedureButtonAction:(id)sender;
- (IBAction)wellnessActivityButtonAction:(id)sender;
- (IBAction)wellnessBMIButtonAction:(id)sender;
- (IBAction)wellnessBPButtonAction:(id)sender;
- (IBAction)wellnessBGButtonAction:(id)sender;
- (IBAction)immunizationButtonAction:(id)sender;
- (IBAction)labTestButtonAction:(id)sender;
- (IBAction)medicationButtonAction:(id)sender;
- (IBAction)problemButtonAction:(id)sender;
- (IBAction)selectDoctorButtonAction:(id)sender;

@end

@implementation SharingViewController
@synthesize isFromDashboard;

-(void)viewWillLayoutSubviews{
    
    CGSize scrollableSize;
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+450);
    }
    else{
        scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+250);
    }
    
    [self.sharingScrollView setContentSize:scrollableSize];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    stepValue = 4.0;
    strDoctorID = @"";
    doctorArray = [NSMutableArray new];
    
    NSDictionary *attrs;
    UIFont * font;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        font = [UIFont systemFontOfSize:18.0f weight:-1];
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
        
        font = [UIFont systemFontOfSize:25.0f weight:-1];
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Share Report"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"Share" style:UIBarButtonItemStylePlain target:self action:@selector(shareReport)];
    //    UIFont * font = [UIFont systemFontOfSize:46.0f weight:-1];
    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;

    UITapGestureRecognizer *sliderTapGesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(sliderTapped:)];
    sliderTapGesture.numberOfTapsRequired = 1;
    [self.sharingSlider addGestureRecognizer:sliderTapGesture];
    
    self.sharingSlider.minimumValue = 0.0;
    self.sharingSlider.maximumValue = 4.0;
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.sharingScrollView addGestureRecognizer:singleFingerTap];
    
    strChecks = @"1";
    strValidUpto = @"1";
    
    if (!isFromDashboard) {
        [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
            //            isFromDashboard = NO;
            [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
            [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
            self.navigationItem.leftBarButtonItem=barItem;
        }];
    }
    
    self.addDoctorButton.layer.cornerRadius = 3;
    self.addDoctorButton.clipsToBounds = YES;
    self.queryTextView.layer.cornerRadius = 3;
    self.queryTextView.clipsToBounds = YES;
    self.queryTextView.layer.borderWidth = 0.75f;
    self.queryTextView.layer.borderColor = [[UIColor lightGrayColor] CGColor];
    
    isLoadingList = NO;
    
    [self addDoctorPickerView];
    [self getDoctorListAPI];
}

#pragma mark Show Rear View
- (void)revealAppointmentView:(id)sender {
    
    revealController = [self revealViewController];
    [revealController revealToggleAnimated:YES];
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.sharingScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+650)];
    }
    else{
        [self.sharingScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+250)];
    }
}

-(void)shareReport{
    if (self.queryTextView.text.length==0) {
        [kAppDelegate showAlertView:@"Enter your query"];
        [self.queryTextView becomeFirstResponder];
    }
    else if ([self.selectDoctorButton.titleLabel.text isEqualToString:@"Select Doctor"]) {
        [kAppDelegate showAlertView:@"Select your doctor"];
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
                    [self showAlertView];
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

-(void)showAlertView{
    self.logoutAlertView=[[UIAlertView alloc]initWithTitle:kAppTitle message:@"Report sent successfully" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
    [self.logoutAlertView show];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    [self.navigationController popViewControllerAnimated:YES];
}

- (IBAction)selectAllButtonAction:(id)sender{
    [self.allergyButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.problemsButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.wellnessActivitiesButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.wellnessBMIButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.wellnessBPButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.wellnessBGButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.immunizationButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.labTestsButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.medicationButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    [self.proceduresButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
    
    strChecks = @"1,7,8,9,10,11,12,13,14,15,16";
    NSLog(@"%@",strChecks);
}

- (IBAction)deSelectAllButtonAction:(id)sender{
    [self.allergyButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.problemsButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.wellnessActivitiesButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.wellnessBMIButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.wellnessBPButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.wellnessBGButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.immunizationButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.labTestsButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.medicationButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    [self.proceduresButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
    strChecks = @"1";
    NSLog(@"%@",strChecks);
}

- (IBAction)allergyButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.allergyButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
                                              
    if ([data1 isEqual:data2]) {
        [self.allergyButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        
        strChecks = [strChecks stringByAppendingString:@",7"];
    }
    else{
        [self.allergyButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",7" withString:@""];
    }
    
    NSLog(@"%@",strChecks);
}

- (IBAction)procedureButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.proceduresButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.proceduresButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",10"];
    }
    else{
        [self.proceduresButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",10" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)wellnessActivityButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.wellnessActivitiesButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.wellnessActivitiesButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",13"];
    }
    else{
        [self.wellnessActivitiesButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",13" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)wellnessBMIButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.wellnessBMIButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.wellnessBMIButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",16"];
    }
    else{
        [self.wellnessBMIButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",16" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)wellnessBPButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.wellnessBPButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.wellnessBPButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",14"];
    }
    else{
        [self.wellnessBPButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",14" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)wellnessBGButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.wellnessBGButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.wellnessBGButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",15"];
    }
    else{
        [self.wellnessBGButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",15" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)immunizationButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.immunizationButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.immunizationButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",8"];
    }
    else{
        [self.immunizationButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",8" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)labTestButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.labTestsButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.labTestsButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",11"];
    }
    else{
        [self.labTestsButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",11" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)medicationButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.medicationButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.medicationButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",9"];
    }
    else{
        [self.medicationButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",9" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)problemButtonAction:(id)sender{
    NSData *data1 = UIImagePNGRepresentation(self.problemsButton.currentImage);
    NSData *data2 = UIImagePNGRepresentation([UIImage imageNamed:@"unCheckedBox"]);
    
    if ([data1 isEqual:data2]) {
        [self.problemsButton setImage:[UIImage imageNamed:@"checkedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByAppendingString:@",12"];
    }
    else{
        [self.problemsButton setImage:[UIImage imageNamed:@"unCheckedBox"] forState:UIControlStateNormal];
        strChecks = [strChecks stringByReplacingOccurrencesOfString:@",12" withString:@""];
    }
    NSLog(@"%@",strChecks);
}

- (IBAction)selectDoctorButtonAction:(id)sender{
    
    if (isLoadingList) {
        [kAppDelegate showAlertView:@"Please wait!! loading doctor's list"];
    }
    else if ([doctorArray count] == 0) {
        [kAppDelegate showAlertView:@"No doctor available in your list! Please add your medical contact by tapping on the side + button"];
        return;
    }
    
    UIPickerView* pickerView = [doctorPickerView viewWithTag:doctorPickerViewPickerTag];
    pickerView.dataSource = self;
    pickerView.delegate = self;
    
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            
            doctorPickerView.frame = CGRectMake(0, self.view.frame.size.height-300, self.view.frame.size.width, 300);
            
            UIButton* doneButton = (UIButton*)[pickerView viewWithTag:doctorPickerViewDoneButtonTag];
            doneButton.frame = CGRectMake(pickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            doctorPickerView.frame = CGRectMake(0, [[UIScreen mainScreen] bounds].size.height-200, [[UIScreen mainScreen] bounds].size.width, 300);
        }
    }];
}

#pragma mark Get Doctor List
-(void)getDoctorListAPI{
    if ([kAppDelegate hasInternetConnection]) {
        
        isLoadingList = YES;
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            
            NSLog(@"Service response = %@",responseObject);
            isLoadingList = NO;
            
            if ([[responseObject valueForKey:@"status"]intValue] == 1) {
                
                [doctorArray removeAllObjects];
                for (int i=0; i<[[responseObject valueForKey:@"response"] count]; i++) {
                    
                    NSMutableDictionary* dic = [NSMutableDictionary new];
                    
                    NSString* ContactName = [[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"ContactName"];
                    if ([ContactName isKindOfClass:[NSNull class]] || [ContactName isEqualToString:@"<null>"] || [ContactName isEqualToString:@""]) {
                        [dic setObject:@"" forKey:@"DocName"];
                    }
                    else{
                        [dic setObject:ContactName forKey:@"DocName"];
                    }
                    
                    NSString* ContactID = [[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"Id"];
                    if ([ContactID isKindOfClass:[NSNull class]] || [ContactID isEqualToString:@"<null>"] || [ContactID isEqualToString:@""]) {
                        [dic setObject:@"" forKey:@"DocID"];
                    }
                    else{
                        [dic setObject:ContactID forKey:@"DocID"];
                    }
                    
                    NSString* PrimaryPhone = [[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"PrimaryPhone"];
                    if ([PrimaryPhone isKindOfClass:[NSNull class]] || [PrimaryPhone isEqualToString:@"<null>"] || [PrimaryPhone isEqualToString:@""]) {
                        [dic setObject:@"" forKey:@"DocMobile"];
                    }
                    else{
                        [dic setObject:PrimaryPhone forKey:@"DocMobile"];
                    }
                    
                    NSString* EmailAddress = [[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"EmailAddress"];
                    if ([EmailAddress isKindOfClass:[NSNull class]] || [EmailAddress isEqualToString:@"<null>"] || [EmailAddress isEqualToString:@""]) {
                        [dic setObject:@"" forKey:@"DocEmail"];
                    }
                    else{
                        [dic setObject:EmailAddress forKey:@"DocEmail"];
                    }
                    [doctorArray addObject:dic];
                }
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

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        doctorPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
}

#pragma mark Create AllInOnePickerView custom view 
-(void)addDoctorPickerView{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        doctorPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, [[UIScreen mainScreen] bounds].size.height+300, [[UIScreen mainScreen] bounds].size.width, 300)];
    }
    else{
        doctorPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, [[UIScreen mainScreen] bounds].size.height+300, [[UIScreen mainScreen] bounds].size.width, 300)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(allInOnePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:doctorPickerViewDoneButtonTag];
    [doctorPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(allInOnePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doctorPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* allergyTimePicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 250)];
    //    genderPicker.dataSource = self;
    //    genderPicker.delegate = self;
    
    [allergyTimePicker setTag:doctorPickerViewPickerTag];
    [doctorPickerView addSubview:allergyTimePicker];
    
    doctorPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:doctorPickerView];
}

-(void)allInOnePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        doctorPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
    }];
    
    if ([strDoctorID isEqualToString:@""] || [strDoctorID isEqual:nil] || [strDoctorID isKindOfClass:[NSNull class]]) {
        strDoctorID = [[doctorArray objectAtIndex:0]valueForKey:@"Id"];
        [self.selectDoctorButton setTitle:[[doctorArray objectAtIndex:0] valueForKey:@"DocName"] forState:UIControlStateNormal];
    }
}

-(void)allInOnePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        doctorPickerView.frame = CGRectMake(0, self.view.frame.size.height+300, self.view.frame.size.width, 300);
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
    return [doctorArray count];
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    strSelectedDoctor = [[doctorArray objectAtIndex:row] valueForKey:@"DocName"];
    return strSelectedDoctor;
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    [self.selectDoctorButton setTitle:[[doctorArray objectAtIndex:row] valueForKey:@"DocName"] forState:UIControlStateNormal];
    strDoctorID = [[doctorArray objectAtIndex:row] valueForKey:@"DocID"];
    strDoctorMobile = [[doctorArray objectAtIndex:row] valueForKey:@"DocMobile"];
    strDoctorEmail = [[doctorArray objectAtIndex:row] valueForKey:@"DocEmail"];
}

#pragma mark Slider Value Change Event 
- (IBAction)addDoctorButtonAction:(id)sender {
    
    AddMedicalContactViewController* obj;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        obj = [[AddMedicalContactViewController alloc]initWithNibName:@"AddMedicalContactViewControlleriPhone" bundle:nil];
    }
    else{
        obj = [[AddMedicalContactViewController alloc]initWithNibName:@"AddMedicalContactViewControlleriPad" bundle:nil];
    }
    
    [self presentViewController:obj animated:YES completion:nil];
}

-(IBAction)valueChanged:(id)sender {
    
    float newStep = roundf(self.sharingSlider.value/self.sharingSlider.maximumValue*stepValue)*self.sharingSlider.maximumValue/stepValue;
    
    // Convert "steps" back to the context of the sliders values.
    [UIView animateWithDuration:0.5 animations:^{
        [self.sharingSlider setValue:newStep animated:YES];
    }];
    
    int step = roundf(newStep);
    //NSLog(@"Steps number on slide: %d",step);
    
    strValidUpto = [NSString stringWithFormat:@"%d",step+1];
//    allergyDurationID = [[[allergyTimeArray objectAtIndex:step] valueForKey:@"Id"] stringValue];
}

- (void)sliderTapped:(UIGestureRecognizer *)g {
    
    if (self.sharingSlider.highlighted)
        return; // tap on thumb, let slider deal with it
    
    CGPoint pt = [g locationInView: self.sharingSlider];
    CGFloat percentage = pt.x / self.sharingSlider.bounds.size.width;
    CGFloat delta = percentage * (self.sharingSlider.maximumValue - self.sharingSlider.minimumValue);
    CGFloat value = self.sharingSlider.minimumValue + delta;
    
    float newStep = roundf(value/self.sharingSlider.maximumValue*stepValue)*self.sharingSlider.maximumValue/stepValue;
    
    [UIView animateWithDuration:0.5 animations:^{
        [self.sharingSlider setValue:newStep animated:YES];
    }];
    
    int step = roundf(newStep);
    //NSLog(@"Steps number on tap: %d",step);
    
    strValidUpto = [NSString stringWithFormat:@"%d",step+1];
    
//    allergyDurationID = [[[allergyTimeArray objectAtIndex:step] valueForKey:@"Id"] stringValue];
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
