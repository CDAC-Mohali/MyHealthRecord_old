//
//  AddProceduresViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AddProceduresViewController.h"
#import "Constants.h"
#import "ProcedureTableViewController.h"

typedef enum : NSUInteger {
    dateDoneButtonTag=700,
    datePickerTag,
} procedureTags;

@interface AddProceduresViewController (){
    UIView* prescribedDatePickerView;
    
    NSData *attachmentImageData;
    
    NSMutableArray *procedureNameArray;
    NSMutableArray* base64ImagesArray;
}

@property (weak, nonatomic) IBOutlet UIScrollView *procedureScrollView;
@property (weak, nonatomic) IBOutlet UIButton *selectProcedureButton;
@property (weak, nonatomic) IBOutlet UIButton *procedureEndedOnButton;
@property (weak, nonatomic) IBOutlet UITextField *surgeonTextfield;
@property (weak, nonatomic) IBOutlet UITextView *notesTextview;
@property (weak, nonatomic) IBOutlet UIImageView *attachment1ImageView;
//@property (weak, nonatomic) IBOutlet UIImageView *attachment2ImageView;

@property (weak, nonatomic) IBOutlet UILabel *procedureNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *diagnosisLabel;
@property (weak, nonatomic) IBOutlet UILabel *surgeonLabel;
@property (weak, nonatomic) IBOutlet UILabel *notesLabel;
@property (weak, nonatomic) IBOutlet UILabel *dischargeLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;
@property (weak, nonatomic) IBOutlet UIButton *attachmentButton;

- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)saveButtonAction:(id)sender;
- (IBAction)selectProcedureButtonAction:(id)sender;
- (IBAction)procedureEndedOnButtonAction:(id)sender;
- (IBAction)attachmentButtonAction:(id)sender;

@end

@implementation AddProceduresViewController

-(void)viewWillLayoutSubviews{
    CGSize scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+150);
    
    [self.procedureScrollView setContentSize:scrollableSize];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
    initWithTarget:self action:@selector(handleSingleTap)];
                                               
    singleFingerTap.numberOfTapsRequired = 1;
                                               
    [self.procedureScrollView addGestureRecognizer:singleFingerTap];
    
    attachmentImageData = UIImagePNGRepresentation(self.attachment1ImageView.image);
    
//    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"ProcedureSnowMed" ofType:@"plist"];
//    procedureNameArray = [[NSMutableArray alloc] initWithContentsOfFile:filePath];
    
    procedureNameArray = [NSMutableArray new];
    
    [kAppDelegate setProcedureNameButtonString:nil];
    base64ImagesArray = [NSMutableArray new];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [self addPrescribedDatePicker];
        
//        [self getProcedureNameList];
    });
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
        [self.procedureScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+120)];
        
        self.titleLabel.font = [UIFont systemFontOfSize:25 weight:-1];
        
        self.selectProcedureButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.procedureEndedOnButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.surgeonTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.notesTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.attachmentButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.procedureNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.diagnosisLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.surgeonLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.notesLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dischargeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
    }
    self.selectProcedureButton.layer.cornerRadius = 3;
    self.selectProcedureButton.clipsToBounds = YES;
    self.procedureEndedOnButton.layer.cornerRadius = 3;
    self.procedureEndedOnButton.clipsToBounds = YES;
    self.notesTextview.layer.cornerRadius = 3;
    self.notesTextview.clipsToBounds = YES;
    self.notesTextview.layer.borderWidth = 0.75f;
    self.notesTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
    self.attachmentButton.layer.cornerRadius = 3;
    self.attachmentButton.clipsToBounds = YES;
    
}

-(void)viewWillAppear:(BOOL)animated{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.procedureScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+250)];
    }
    else{
        [self.procedureScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+50)];
    }
    
    if ([kAppDelegate procedureNameButtonString].length) {
        [self.selectProcedureButton setTitle:[[kAppDelegate procedureNameButtonString] capitalizedString] forState:UIControlStateNormal];
    }
    else{
        [self.selectProcedureButton setTitle:@"Select Procedure" forState:UIControlStateNormal];
    }
    
    [self prefersStatusBarHidden];
//    [[UIApplication sharedApplication] setStatusBarHidden:NO];
}

- (BOOL)prefersStatusBarHidden {
    return NO;
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.procedureScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+250)];
    }
    else{
        [self.procedureScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+50)];
    }
}

#pragma mark Get Health Problem name list API call
-(void)getProcedureNameList{
    
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
                
                procedureNameArray = json;
            }
            
            // The server answers with an error because it doesn't receive the params
        }];
        
        [postDataTask resume];
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
    
    [self.procedureEndedOnButton setTitle:[dateFormatter stringFromDate:[datePicker date]] forState:UIControlStateNormal];
    
}

-(void)cancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextView *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
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

#pragma mark Save Button Action
- (IBAction)saveButtonAction:(id)sender {
    
    if ([self.selectProcedureButton.titleLabel.text isEqualToString:@"Select Procedure"]) {
        [kAppDelegate showAlertView:@"Select procedure name"];
    }
    else if ([self.procedureEndedOnButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select procedure end date"];
    }
//    else if (self.surgeonTextfield.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter surgeon"];
//        [self.surgeonTextfield becomeFirstResponder];
//    }
//    else if (self.notesTextview.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter notes"];
//        [self.notesTextview becomeFirstResponder];
//    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.procedureEndedOnButton.titleLabel.text;
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
                    [kAppDelegate showAlertView:@"Procedure added successfully"];
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

#pragma mark Select Procedure Name
- (IBAction)selectProcedureButtonAction:(id)sender {
    
//    if ([procedureNameArray count]) {
        ProcedureTableViewController* obj = [[ProcedureTableViewController alloc]initWithNibName:@"ProcedureTableViewController" bundle:nil];
        obj.proceduresNameArray = procedureNameArray;
        [self presentViewController:obj animated:YES completion:nil];
//    }
//    else{
//        [kAppDelegate showAlertView:@"Getting list! Try later"];
//    }
    
}

- (IBAction)procedureEndedOnButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    
    UIDatePicker* datePicker = [prescribedDatePickerView viewWithTag:datePickerTag];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            datePicker.frame = CGRectMake(0, 30, prescribedDatePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[prescribedDatePickerView viewWithTag:dateDoneButtonTag];
            doneButton.frame = CGRectMake(prescribedDatePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            prescribedDatePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
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

- (IBAction)attachmentButtonAction:(id)sender {
    
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
@end
