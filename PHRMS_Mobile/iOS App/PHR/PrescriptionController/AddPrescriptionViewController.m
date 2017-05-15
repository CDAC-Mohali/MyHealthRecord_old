//
//  AddPrescriptionViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AddPrescriptionViewController.h"
#import "Constants.h"

typedef enum
{
    datePickerTag = 300,
    doneButtonTag,
    canelButtonTag
    
}prescriptionTags;

@interface AddPrescriptionViewController (){
    UIView* dobPickerView;
    int chodayi;
    
    NSData *attachmentImageData;
    NSMutableArray* base64ImagesArray;
}
@property (weak, nonatomic) IBOutlet UIScrollView *prescriptionScrollView;
@property (weak, nonatomic) IBOutlet UITextField *doctorNameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *hospitalNameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *addressTextfield;
@property (weak, nonatomic) IBOutlet UITextField *phoneTextfield;
@property (weak, nonatomic) IBOutlet UIButton *prescriptionDateButton;
@property (weak, nonatomic) IBOutlet UITextView *presriptionDetailTextview;
@property (weak, nonatomic) IBOutlet UIButton *attachmentButton;
@property (weak, nonatomic) IBOutlet UIImageView *attachmentImageView1;
//@property (weak, nonatomic) IBOutlet UIImageView *attachmentImageView2;
//@property (weak, nonatomic) IBOutlet UIImageView *attachmentImageView3;

- (IBAction)dismissButtonAction:(id)sender;
- (IBAction)addButtonAction:(id)sender;
- (IBAction)prescriptionDateButtonAction:(id)sender;
- (IBAction)attachmentButtonAction:(id)sender;

@property (weak, nonatomic) IBOutlet UIButton *cancelButton;
@property (weak, nonatomic) IBOutlet UIButton *saveButton;


@property (weak, nonatomic) IBOutlet UILabel *doctorNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *hospitalNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *addressLabel;
@property (weak, nonatomic) IBOutlet UILabel *phoneLabel;
@property (weak, nonatomic) IBOutlet UILabel *remarksLabel;
@property (weak, nonatomic) IBOutlet UILabel *prescriptionDateLabel;
@property (weak, nonatomic) IBOutlet UILabel *attachmentLabel;
@property (weak, nonatomic) IBOutlet UILabel *titleLabel;

@end

@implementation AddPrescriptionViewController

-(void)viewWillLayoutSubviews{
    CGSize scrollableSize = CGSizeMake([[UIScreen mainScreen] bounds].size.width, self.view.frame.size.height+320);
    
    [self.prescriptionScrollView setContentSize:scrollableSize];
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        self.addressLabel.frame = CGRectMake(self.hospitalNameLabel.frame.origin.x, self.addressLabel.frame.origin.y, self.addressLabel.frame.size.width, self.addressLabel.frame.size.height);
        self.phoneLabel.frame = CGRectMake(self.hospitalNameLabel.frame.origin.x, self.phoneLabel.frame.origin.y, self.phoneLabel.frame.size.width, self.phoneLabel.frame.size.height);
        
        self.addressTextfield.frame = CGRectMake(self.hospitalNameTextfield.frame.origin.x, self.addressTextfield.frame.origin.y, self.hospitalNameTextfield.frame.size.width, self.addressLabel.frame.size.height);
        self.phoneTextfield.frame = CGRectMake(self.hospitalNameTextfield.frame.origin.x, self.phoneLabel.frame.origin.y, self.hospitalNameTextfield.frame.size.width, self.phoneTextfield.frame.size.height);
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.prescriptionScrollView addGestureRecognizer:singleFingerTap];
    
    attachmentImageData = UIImagePNGRepresentation(self.attachmentImageView1.image);
    
    [self.doctorNameTextfield setDelegate:self];
    [self.hospitalNameTextfield setDelegate:self];
//    [self.addressTextfield setDelegate:self];
//    [self.phoneTextfield setDelegate:self];
    [self.presriptionDetailTextview setDelegate:self];
    
    base64ImagesArray = [NSMutableArray new];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [self addDateOfBirthPicker];
    });
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
        self.titleLabel.font = [UIFont systemFontOfSize:25 weight:-1];
        
        self.doctorNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cancelButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.saveButton.titleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.hospitalNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.prescriptionDateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.presriptionDetailTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.attachmentButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.doctorNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.hospitalNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.addressLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.phoneLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.remarksLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.prescriptionDateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.attachmentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        
        self.addressLabel.frame = CGRectMake(self.hospitalNameLabel.frame.origin.x, self.addressLabel.frame.origin.y, self.addressLabel.frame.size.width, self.addressLabel.frame.size.height);
        self.phoneLabel.frame = CGRectMake(self.hospitalNameLabel.frame.origin.x, self.phoneLabel.frame.origin.y, self.phoneLabel.frame.size.width, self.phoneLabel.frame.size.height);
        
        self.addressTextfield.frame = CGRectMake(self.hospitalNameTextfield.frame.origin.x, self.addressTextfield.frame.origin.y, self.hospitalNameTextfield.frame.size.width, self.addressLabel.frame.size.height);
        self.phoneTextfield.frame = CGRectMake(self.hospitalNameTextfield.frame.origin.x, self.phoneLabel.frame.origin.y, self.hospitalNameTextfield.frame.size.width, self.phoneTextfield.frame.size.height);
        //        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
    }
    self.prescriptionDateButton.layer.cornerRadius = 3;
    self.prescriptionDateButton.clipsToBounds = YES;
    self.presriptionDetailTextview.layer.cornerRadius = 3;
    self.presriptionDetailTextview.clipsToBounds = YES;
    self.attachmentButton.layer.cornerRadius = 3;
    self.attachmentButton.clipsToBounds = YES;
    self.presriptionDetailTextview.layer.borderWidth = 0.75f;
    self.presriptionDetailTextview.layer.borderColor = [[UIColor lightGrayColor] CGColor];
}

-(void)viewWillAppear:(BOOL)animated{
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.prescriptionScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+520)];
    }
    else{
        [self.prescriptionScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+320)];
    }
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    CGFloat screenWidth = screenRect.size.width;
    CGFloat screenHeight = screenRect.size.height;
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.prescriptionScrollView setContentSize:CGSizeMake(screenWidth, self.view.frame.size.height+520)];
        [self.prescriptionScrollView setFrame:CGRectMake(screenRect.origin.x, screenRect.origin.y, screenWidth, screenHeight)];
    }
    else{
        [self.prescriptionScrollView setContentSize:CGSizeMake(screenWidth, self.view.frame.size.height+320)];
    }
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}


#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text View Delegate
-(void)textViewDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create DOB datepicker custom view 
-(void)addDateOfBirthPicker{
    
    // creating custom view for DOB
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        dobPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+1000, self.view.frame.size.width, 200)];
    }
    else{
        dobPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(pickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:doneButtonTag];
    [dobPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(pickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [dobPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIDatePicker* dobDatePicker = [[UIDatePicker alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //[dobDatePicker setDate:[NSDate date]];
    [dobDatePicker setDatePickerMode:UIDatePickerModeDate];
    dobDatePicker.maximumDate=[NSDate date];
    [dobDatePicker setTag:datePickerTag];
    [dobPickerView addSubview:dobDatePicker];
    
    dobPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:dobPickerView];
}

-(void)pickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
    
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    //    dateFormatter.dateFormat = @"yyyy-MM-dd";
    dateFormatter.dateFormat = @"dd-MM-yyyy";
    
    UIDatePicker* dobDatePicker = (UIDatePicker*)[dobPickerView viewWithTag:datePickerTag];
    NSString* dateString = [dateFormatter stringFromDate:dobDatePicker.date];
    
    [self.prescriptionDateButton setTitle:dateString forState:UIControlStateNormal];
}

-(void)pickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
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

- (IBAction)addButtonAction:(id)sender {
    if (self.doctorNameTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter doctor name"];
        [self.doctorNameTextfield becomeFirstResponder];
    }
    else if (self.hospitalNameTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter hospital/clinic name"];
        [self.hospitalNameTextfield becomeFirstResponder];
    }
//    else if (self.addressTextfield.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter address"];
//    }
//    else if (self.phoneTextfield.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter phone number"];
//    }
    else if ([self.prescriptionDateButton.titleLabel.text isEqualToString:@"Select"]) {
        [kAppDelegate showAlertView:@"Select prescription date"];
    }
//    else if (self.presriptionDetailTextview.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter prescription details"];
//        [self.presriptionDetailTextview becomeFirstResponder];
//    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"submitting..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
            [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd-MM-yyyy"];
            
            NSString* deviceDate = self.prescriptionDateButton.titleLabel.text;
            NSDate* date = [dateFormatter dateFromString:deviceDate];
            
            NSString* prescriptionDateString = [dateFormat stringFromDate:date];
            
            NSString* dateString = [dateFormat stringFromDate:[NSDate date]];
            NSArray* array = [dateString componentsSeparatedByString:@"+"];
            dateString = [array objectAtIndex:0];
            
//            NSString* prescriptionDateString = [NSString stringWithFormat:@"%@ 00:00:00",[self.prescriptionDateButton titleLabel].text];
            
            NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
            
            NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url
            
            //AFNetworking methods.
            AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
            AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
            
            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
//            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
            
            [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
            
            manager.requestSerializer = requestSerializer;
            manager.responseSerializer = [AFJSONResponseSerializer serializerWithReadingOptions:NSJSONReadingAllowFragments];
            [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
                [kAppDelegate hideLoadingIndicator];
                NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                    [kAppDelegate showAlertView:@"Prescription added successfully"];
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

- (IBAction)prescriptionDateButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    
    UIDatePicker* picker = (UIDatePicker*)[dobPickerView viewWithTag:datePickerTag];
//    [picker setMinimumDate:[NSDate date]];
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            dobPickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            picker.frame = CGRectMake(0, 30, dobPickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[dobPickerView viewWithTag:doneButtonTag];
            doneButton.frame = CGRectMake(dobPickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            dobPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
    
}

#pragma mark Photo Selection Controller 
- (IBAction)attachmentButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    
//    if (![attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachmentImageView1.image)] && ![attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachmentImageView2.image)] && ![attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachmentImageView3.image)]) {
//        [kAppDelegate showAlertView:@"Max no. of files are 3"];
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
     
    
//    else{
//        UIImagePickerController * picker = [[UIImagePickerController alloc] init];
//        
//        // Don't forget to add UIImagePickerControllerDelegate in your .h
//        picker.delegate = self;
//        
//        //    if((UIButton *) sender == choosePhotoBtn) {
//        picker.sourceType = UIImagePickerControllerSourceTypeSavedPhotosAlbum;
//        //    } else {
//        //        picker.sourceType = UIImagePickerControllerSourceTypeCamera;
//        //    }
//        
//        [self presentViewController:picker animated:YES completion:nil];
//    }
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info {
    
    [picker dismissViewControllerAnimated:YES completion:nil];
    
    UIImage *image = [info objectForKey:UIImagePickerControllerOriginalImage];
    
    image = [kAppDelegate scaleAndRotateImage:image];
    
    NSString* strImage = [self imageToNSString:image];
    [base64ImagesArray addObject:strImage];
    
//    if ([attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachmentImageView1.image)]) {
        self.attachmentImageView1.image = image;
//    }
//    else if (![attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachmentImageView1.image)] && [attachmentImageData isEqualToData:UIImagePNGRepresentation(self.attachmentImageView2.image)]) {
//        self.attachmentImageView2.image = image;
//    }
//    else{
//        self.attachmentImageView3.image = image;
//    }
//    [self uploadFile];
//    [self sendImageToServer:strImage];
}

//- (void) uploadFile
//{
//    //----- get the file to upload as an NSData object
//    NSString *applicationDocumentsDir = [NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES) objectAtIndex:0];
//    NSString *filepath = [NSString stringWithFormat: @"%@/%@", applicationDocumentsDir, @"image.jpg"];
//    uploadData = [NSData dataWithContentsOfFile: filepath];
//    
//    uploadFile = [[BRRequestUpload alloc] initWithDelegate:self];
//    uploadFile.path = path.text;
//    uploadFile.hostname = host.text;
//    uploadFile.username = username.text;
//    uploadFile.password = password.text;
//    
//    [uploadFile start];
//}

- (void)sendImageToServer:(NSString*)image {
//    UIImage *yourImage= [UIImage imageNamed:@"image.png"];
//    NSData *imageData = UIImagePNGRepresentation(image);
//    NSString *postLength = [NSString stringWithFormat:@"%lu", (unsigned long)[imageData length]];
//    
//    // Init the URLRequest
//    NSMutableURLRequest *request = [[NSMutableURLRequest alloc] init];
//    [request setHTTPMethod:@"POST"];
//    [request setURL:[NSURL URLWithString:@"http://10.228.12.36:8087/api/Prescription/SaveImage"]];
//    
//    [request setValue:@"application/x-www-form-urlencoded" forHTTPHeaderField:@"Content-Type"];
//    [request setValue:postLength forHTTPHeaderField:@"Content-Length"];
//    [request setHTTPBody:imageData];
//    
//    NSURLConnection *connection = [[NSURLConnection alloc] initWithRequest:request delegate:self];
//    if (connection) {
//        NSLog(@"");
//        // response data of the request
//    }
    
    NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
    [dicParams setObject:[base64ImagesArray objectAtIndex:0] forKey:@"Text"];
    
    NSString *urlString = [NSString stringWithFormat:@"http://10.228.12.36:8087/Images/Prescription"];//Url
    
    //AFNetworking methods.
    AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
    AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
    
    [requestSerializer setValue:@" application/json; charset=utf-8" forHTTPHeaderField:@"Content-Type"];
    [requestSerializer setValue:@"text/html" forHTTPHeaderField:@"Content-Type"];
    [requestSerializer setValue:@"image/png" forHTTPHeaderField:@"Accept"];
    
    manager.requestSerializer = requestSerializer;
    [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
        [kAppDelegate hideLoadingIndicator];
        NSLog(@"Result dict %@",responseObject);
        
        if ([[responseObject valueForKey:@"status"] integerValue]==1) {
            [kAppDelegate showAlertView:@"Prescription added successfully"];
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

//    [request release];
//}

-(NSString *)imageToNSString:(UIImage *)image
{
    NSData *imageData = UIImagePNGRepresentation(image);
    return [imageData base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
}

@end
