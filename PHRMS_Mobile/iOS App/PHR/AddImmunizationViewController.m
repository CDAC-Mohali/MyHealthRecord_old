//
//  AddImmunizationViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AddImmunizationViewController.h"
#import "ImmunzationNameTableViewController.h"
#import "Constants.h"

typedef enum
{
    datePickerTag = 500,
    doneButtonTag,
    canelButtonTag
    
}immunzationTags;

@interface AddImmunizationViewController (){
    UIView* datePickerView;
    
    NSMutableArray* immunizationNameArray;
}
- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)addButtonAction:(id)sender;
- (IBAction)immunizationNameButtonAction:(id)sender;
- (IBAction)immunizationDateButtonAction:(id)sender;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;

@property (weak, nonatomic) IBOutlet UIButton *immunzationNameButton;
@property (weak, nonatomic) IBOutlet UIButton *immunzationDateButton;
@property (weak, nonatomic) IBOutlet UITextView *commentsTextView;
@property (weak, nonatomic) IBOutlet UIScrollView *immunzationScrollview;

@property (weak, nonatomic) IBOutlet UILabel *immunizationLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;
@property (weak, nonatomic) IBOutlet UILabel *takenOnLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@end

@implementation AddImmunizationViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    immunizationNameArray = [[NSMutableArray alloc] init];
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.immunzationScrollview addGestureRecognizer:singleFingerTap];
    [kAppDelegate setImmunizationNameButtonString:nil];
    
    dispatch_async(dispatch_get_main_queue(), ^{
//        [self getImmunizationNameList];
        [self addDateOfBirthPicker];
    });
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
        [self.immunzationScrollview setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+80)];
        
        self.titleLabel.font = [UIFont systemFontOfSize:25 weight:-1];
        
        self.immunizationLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.takenOnLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentsLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.immunzationDateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.immunzationNameButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.commentsTextView.font = [UIFont systemFontOfSize:16.0f weight:-1];
    }
    self.immunzationNameButton.layer.cornerRadius = 3;
    self.immunzationNameButton.clipsToBounds = YES;
    self.immunzationDateButton.layer.cornerRadius = 3;
    self.immunzationDateButton.clipsToBounds = YES;
    self.commentsTextView.layer.cornerRadius = 3;
    self.commentsTextView.clipsToBounds = YES;
    self.commentsTextView.layer.borderWidth = 0.75f;
    self.commentsTextView.layer.borderColor = [[UIColor lightGrayColor] CGColor];

}

-(void)viewWillAppear:(BOOL)animated{
    
    if ([kAppDelegate immunizationNameButtonString].length) {
        [self.immunzationNameButton setTitle:[[kAppDelegate immunizationNameButtonString] capitalizedString] forState:UIControlStateNormal];
    }
    else{
        [self.immunzationNameButton setTitle:@"Select Name" forState:UIControlStateNormal];
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
        datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}


#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Get Health Problem name list API call
-(void)getImmunizationNameList{
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
                
                immunizationNameArray = json;
            }
            
            // The server answers with an error because it doesn't receive the params
        }];
        
        [postDataTask resume];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

#pragma mark Create datepicker custom view 
-(void)addDateOfBirthPicker{
    
    // creating custom view for DOB
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        datePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+1000, self.view.frame.size.width, 200)];
    }
    else{
        datePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(pickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:doneButtonTag];
    [datePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(pickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [datePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIDatePicker* dobDatePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //[dobDatePicker setDate:[NSDate date]];
    [dobDatePicker setDatePickerMode:UIDatePickerModeDate];
    dobDatePicker.maximumDate = [NSDate date];
    [dobDatePicker setTag:datePickerTag];
    [datePickerView addSubview:dobDatePicker];
    
    datePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:datePickerView];
}

-(void)pickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    //    dateFormatter.dateFormat = @"yyyy-MM-dd";
    dateFormatter.dateFormat = @"dd-MM-yyyy";
    
    UIDatePicker* dobDatePicker = (UIDatePicker*)[datePickerView viewWithTag:datePickerTag];
    NSString* dateString = [dateFormatter stringFromDate:dobDatePicker.date];
    
    [self.immunzationDateButton setTitle:dateString forState:UIControlStateNormal];
}

-(void)pickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

- (IBAction)dismissButtonAction:(id)sender {
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (IBAction)addButtonAction:(id)sender {
    if ([self.immunzationNameButton.titleLabel.text isEqualToString:@"Select Name"]) {
        [kAppDelegate showAlertView:@"Select immunization name"];
    }
    else if ([self.immunzationDateButton.titleLabel.text isEqualToString:@"Select Date"]) {
        [kAppDelegate showAlertView:@"Select immunization date"];
    }
//    else if (self.commentsTextView.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter comments"];
//    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.immunzationDateButton.titleLabel.text;
            NSDate* date = [dateFormatter dateFromString:deviceDate];
            
            NSString* immunizationDateString = [dateFormat stringFromDate:date];
            
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
                NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                    
                    [kAppDelegate showAlertView:@"Immunization added successfully"];
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

- (IBAction)immunizationNameButtonAction:(id)sender {
    
    [UIView animateWithDuration:0.75 animations:^{
        
        datePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
//    if ([immunizationNameArray count]) {
        ImmunzationNameTableViewController* obj = [[ImmunzationNameTableViewController alloc]initWithNibName:@"ImmunzationNameTableViewController" bundle:nil];
        obj.immunzationNameArray = immunizationNameArray;
        [self presentViewController:obj animated:YES completion:nil];
//    }
//    else{
//        [kAppDelegate showAlertView:@"Getting list! Try later"];
//    }
}

- (IBAction)immunizationDateButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    
    UIDatePicker* picker = (UIDatePicker*)[datePickerView viewWithTag:datePickerTag];
//    [picker setMinimumDate:[NSDate date]];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            picker.frame = CGRectMake(0, 30, datePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[datePickerView viewWithTag:doneButtonTag];
            doneButton.frame = CGRectMake(datePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            datePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
}
@end
