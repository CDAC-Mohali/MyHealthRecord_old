//
//  SignUpViewController.m
//  PHR
//
//  Created by CDAC HIED on 03/04/17.
//  Copyright © 2017 CDAC HIED. All rights reserved.
//

#import "SignUpViewController.h"
#import "Constants.h"
#import <CommonCrypto/CommonDigest.h>
#import "OTPViewController.h"

typedef enum
{
    statePickerTag = 1700,
    stateDoneButtonTag,
    aadhaarFirstAlertViewTag
}signUpTags;

@interface SignUpViewController (){
    UIView* statePickerView;
    
    NSMutableArray* statesArray;
    
    NSString* stateString;
    NSString* genderString;
    NSString* stateID;
    
    AVCaptureSession *_session;
    AVCaptureDevice *_device;
    AVCaptureDeviceInput *_input;
    AVCaptureMetadataOutput *_output;
    AVCaptureVideoPreviewLayer *_prevLayer;
    
    NSDictionary *xmlDictionary;
}

@property (weak, nonatomic) IBOutlet UIScrollView *registrationScrollView;

@property (weak, nonatomic) IBOutlet UITextField *firstNameText;
@property (weak, nonatomic) IBOutlet UITextField *lastNameTextfield;
@property (weak, nonatomic) IBOutlet UIButton *maleButton;
@property (weak, nonatomic) IBOutlet UIButton *femaleButton;
@property (weak, nonatomic) IBOutlet UIButton *otherButton;
@property (weak, nonatomic) IBOutlet UITextField *mobileNoTextfield;
@property (weak, nonatomic) IBOutlet UITextField *emailTextfield;
@property (weak, nonatomic) IBOutlet UITextField *passwordTextfield;
@property (weak, nonatomic) IBOutlet UITextField *confirmPasswordTextfield;
@property (weak, nonatomic) IBOutlet UITextField *aadhaarTextfield;
@property (weak, nonatomic) IBOutlet UIButton *stateButton;

- (IBAction)stateButtonAction:(id)sender;
- (IBAction)maleButtonAction:(id)sender;
- (IBAction)femaleButtonAction:(id)sender;
- (IBAction)otherButtonAction:(id)sender;
- (IBAction)cancelButtonAction:(id)sender;
- (IBAction)registerButtonAction:(id)sender;
- (IBAction)scanAadhaarCardAction:(id)sender;


@end

@implementation SignUpViewController

-(void) viewDidLayoutSubviews{
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        [self.registrationScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+500)];
    }
    else{
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            [self.registrationScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+500)];
        }
        else{
            [self.registrationScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+200)];
        }
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    [self.navigationController setNavigationBarHidden:NO];
    
    NSString *statesNameFilePath = [[NSBundle mainBundle] pathForResource:@"States" ofType:@"plist"];
    statesArray = [[NSMutableArray alloc] initWithContentsOfFile:statesNameFilePath];
    [self addStatePicker];
    
    genderString = @"";
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.registrationScrollView addGestureRecognizer:singleFingerTap];
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

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        [self.registrationScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+500)];
    }
    else{
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            [self.registrationScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+600)];
        }
        else{
            [self.registrationScrollView setContentSize:CGSizeMake(screenRect.size.width, screenRect.size.height+300)];
        }
    }
}

#pragma mark Create State picker custom view 
-(void)addStatePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        statePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
    }
    else{
        statePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(statePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:stateDoneButtonTag];
    [statePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(statePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [statePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* bloodGroupPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    bloodGroupPicker.dataSource = self;
    //    bloodGroupPicker.delegate = self;
    
    [bloodGroupPicker setTag:statePickerTag];
    [statePickerView addSubview:bloodGroupPicker];
    
    statePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:statePickerView];
}

-(void)statePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
    
    [self.stateButton setTitle:stateString forState:UIControlStateNormal];
}

-(void)statePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
}

#pragma mark UIPickerView Delegates
- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView{
    return 1;
}

- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component{
    return [statesArray count];
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    stateString = [[statesArray objectAtIndex:row] valueForKey:@"StateName"];
    return stateString;
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    stateString = [[statesArray objectAtIndex:row] valueForKey:@"StateName"];
    
    [self.stateButton setTitle:stateString forState:UIControlStateNormal];
    stateID = [[statesArray objectAtIndex:row] valueForKey:@"StateId"];
}

#pragma mark Password Encryption Method 
-(NSString *)createSHA512:(NSString *)string
{
    const char *cstr = [string cStringUsingEncoding:NSUTF8StringEncoding];
    NSData *data = [NSData dataWithBytes:cstr length:string.length];
    uint8_t digest[CC_SHA512_DIGEST_LENGTH];
    CC_SHA512(data.bytes, data.length, digest);
    NSMutableString* output = [NSMutableString  stringWithCapacity:CC_SHA512_DIGEST_LENGTH * 2];
    
    for(int i = 0; i < CC_SHA512_DIGEST_LENGTH; i++)
        [output appendFormat:@"%02x", digest[i]];
    return output;
}

#pragma mark Textfield Delegate Method 
- (BOOL) textFieldShouldEndEditing:(UITextField *)textField {
    
    if (textField == self.passwordTextfield) {
        if (self.passwordTextfield.text.length<6 && self.passwordTextfield.text.length>0) {
            [kAppDelegate showAlertView:@"Password length should be atleast 6 characters"];
            [self.passwordTextfield becomeFirstResponder];
        }
    }
    else if (textField == self.confirmPasswordTextfield) {
        NSString* strPassword = self.passwordTextfield.text;
        NSString* strConfirmPassword = self.confirmPasswordTextfield.text;
        if (self.confirmPasswordTextfield.text.length<6 && self.confirmPasswordTextfield.text.length>0) {
            [kAppDelegate showAlertView:@"Confirm password length should be atleast 6 characters"];
            [self.confirmPasswordTextfield becomeFirstResponder];
        }
        else if (![strPassword isEqualToString:strConfirmPassword]) {
            [kAppDelegate showAlertView:@"Confirm password not matched with password"];
            [self.confirmPasswordTextfield becomeFirstResponder];
        }
    }
    return YES;
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        statePickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Email Validation Method 
-(BOOL)validEmail:(NSString*)myemail
{
    myemail=[myemail stringByReplacingOccurrencesOfString:@" " withString:@""];
    
    if([myemail length]<1)
        return NO;
    else
    {
        NSArray *mailParts=[myemail componentsSeparatedByString:@"@"];
        if(([mailParts count]>2) ||-([mailParts count]<2))
            return NO;
        else
        {
            NSString *lastPart=[mailParts objectAtIndex:[mailParts count]-1];
            NSArray *mailParts2=[lastPart componentsSeparatedByString:@"."];
            //NSRange isRange = [lastPart rangeOfString:@"." options:NSCaseInsensitiveSearch];
            if([lastPart rangeOfString:@".."].length) {
                return NO;
            }
            
            if([mailParts2 count]<2)
                return NO;
            else
            {
                NSString *lastPart2=[mailParts2 objectAtIndex:[mailParts2 count] -1];
                NSString *firstPart=[mailParts2 objectAtIndex:[mailParts2 count] -2];
                if([lastPart2 length]<1)
                    return NO;
                else if([firstPart length]<1)
                    return NO;
                else
                    return YES;
            }
        }
    }
    return NO;
}

#pragma mark Button Action Methods 
- (IBAction)stateButtonAction:(id)sender {
    
    [self.view endEditing:YES];
    stateID = @"1";
    
    UIPickerView* picker = (UIPickerView*)[statePickerView viewWithTag:statePickerTag];
    picker.dataSource = self;
    picker.delegate = self;
    
    [UIView animateWithDuration:0.75 animations:^{
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
            statePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
            
            picker.frame = CGRectMake(0, 30, statePickerView.frame.size.width, 200);
            
            UIButton* doneButton = (UIButton*)[statePickerView viewWithTag:stateDoneButtonTag];
            doneButton.frame = CGRectMake(statePickerView.frame.size.width-70, 2, 60, 30);
        }
        else{
            statePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
        }
    }];
}

- (IBAction)maleButtonAction:(id)sender {
    [self.femaleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    [self.otherButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    [self.maleButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
    
    genderString = @"M";
}

- (IBAction)femaleButtonAction:(id)sender {
    [self.femaleButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
    [self.otherButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    [self.maleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    
    genderString = @"F";
}

- (IBAction)otherButtonAction:(id)sender {
    [self.femaleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    [self.otherButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
    [self.maleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
    
    genderString = @"U";
}

- (IBAction)cancelButtonAction:(id)sender {
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (IBAction)registerButtonAction:(id)sender {
    
//    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
//        OTPViewController * viewController =[[UIStoryboard storyboardWithName:@"Main-iPad" bundle:nil] instantiateViewControllerWithIdentifier:@"OTPViewController"];
//        [self showViewController:viewController sender:self];
//    }
//    else{
//        OTPViewController * viewController =[[UIStoryboard storyboardWithName:@"Main" bundle:nil] instantiateViewControllerWithIdentifier:@"OTPViewController"];
//        [self showViewController:viewController sender:self];
//    }
//    return;
    
    NSString* strPassword = self.passwordTextfield.text;
    NSString* strConfirmPassword = self.confirmPasswordTextfield.text;
    
    if (self.firstNameText.text.length==0) {
        [kAppDelegate showAlertView:@"Enter first name"];
        [self.firstNameText becomeFirstResponder];
    }
    else if (self.lastNameTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter last name"];
        [self.lastNameTextfield becomeFirstResponder];
    }
    else if ([genderString isEqualToString:@""]) {
        [kAppDelegate showAlertView:@"Select gender"];
    }
    else if (self.mobileNoTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter mobile number"];
        [self.mobileNoTextfield becomeFirstResponder];
    }
    else if (self.mobileNoTextfield.text.length>10 || self.mobileNoTextfield.text.length<10) {
        [kAppDelegate showAlertView:@"Invalid mobile no."];
        [self.mobileNoTextfield becomeFirstResponder];
    }
    else if (self.emailTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter email"];
        [self.emailTextfield becomeFirstResponder];
    }
    else if (![self validEmail:self.emailTextfield.text ]){
        [kAppDelegate showAlertView:@"Invalid email id"];
        [self.emailTextfield becomeFirstResponder];
    }
    else if (self.passwordTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter password"];
        [self.passwordTextfield becomeFirstResponder];
    }
    else if (self.confirmPasswordTextfield.text.length==0) {
        [kAppDelegate showAlertView:@"Enter confirm password"];
        [self.confirmPasswordTextfield becomeFirstResponder];
    }
    else if (self.aadhaarTextfield.text.length>12 || ((self.aadhaarTextfield.text.length<12) && (self.aadhaarTextfield.text.length>0)) ){
        [kAppDelegate showAlertView:@"Invalid aadhaar no."];
        [self.aadhaarTextfield becomeFirstResponder];
    }
    else if ([self.stateButton.titleLabel.text isEqualToString:@"Select State"]) {
        [kAppDelegate showAlertView:@"Select state"];
    }
    else if (self.passwordTextfield.text.length<6) {
        [kAppDelegate showAlertView:@"Password length should be atleast 6 characters"];
        [self.passwordTextfield becomeFirstResponder];
    }
    else if (self.confirmPasswordTextfield.text.length<6){
        [kAppDelegate showAlertView:@"Confirm password length should be atleast 6 characters"];
        [self.confirmPasswordTextfield becomeFirstResponder];
    }
    else if (![strPassword isEqualToString:strConfirmPassword]) {
        [kAppDelegate showAlertView:@"Confirm password not matched with password"];
        [self.confirmPasswordTextfield becomeFirstResponder];
    }
    //    else if (self.presriptionDetailTextview.text.length==0) {
    //        [kAppDelegate showAlertView:@"Enter prescription details"];
    //        [self.presriptionDetailTextview becomeFirstResponder];
    //    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"registering..."];//Show loading indicator.
            
            NSString* encrptedPassword = [self createSHA512:self.passwordTextfield.text];
            
            NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
            
            NSString *urlString = [NSString stringWithFormat:@"enter your API url"];//Url
            
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
                
                if ([[responseObject valueForKey:@"response"] integerValue]==1) {
                    [kAppDelegate setStrOTPID:[responseObject valueForKey:@"Status"]];
                    [kAppDelegate setStrRegistrationMobileNo:self.mobileNoTextfield.text];
//                    [self dismissViewControllerAnimated:YES completion:nil];
                    
                    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
                        OTPViewController * viewController =[[UIStoryboard storyboardWithName:@"Main-iPad" bundle:nil] instantiateViewControllerWithIdentifier:@"OTPViewController"];
                        [self showViewController:viewController sender:self];
                    }
                    else{
                        OTPViewController * viewController =[[UIStoryboard storyboardWithName:@"Main" bundle:nil] instantiateViewControllerWithIdentifier:@"OTPViewController"];
                        [self showViewController:viewController sender:self];
                    }
                }
                else if ([[responseObject valueForKey:@"response"] integerValue]==2) {
                    [kAppDelegate showAlertView:@"Mobile no. already registered!! Try with another mobile no."];
                    [self.mobileNoTextfield becomeFirstResponder];
                }
                else if ([[responseObject valueForKey:@"response"] integerValue]==3) {
                    [kAppDelegate showAlertView:@"Email address already registered!! Try with another email id"];
                    [self.emailTextfield becomeFirstResponder];
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

- (IBAction)scanAadhaarCardAction:(id)sender {
    [self showAlertView];
}

#pragma mark UIAlertView Delegate Method 
-(void)showAlertView{
    UIAlertView* aadhaarAlertView=[[UIAlertView alloc]initWithTitle:kAppTitle message:@"Do you want to fill registration information from your Aadhaar Card QR?" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"OK", nil];
    [aadhaarAlertView setTag:aadhaarFirstAlertViewTag];
    [aadhaarAlertView show];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    if (alertView.tag == aadhaarFirstAlertViewTag) {
        if (buttonIndex==1) {
            _session = [[AVCaptureSession alloc] init];
            _device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
            NSError *error = nil;
            
            _input = [AVCaptureDeviceInput deviceInputWithDevice:_device error:&error];
            if (_input) {
                [_session addInput:_input];
            } else {
                NSLog(@"Error: %@", error);
            }
            
            _output = [[AVCaptureMetadataOutput alloc] init];
            [_output setMetadataObjectsDelegate:self queue:dispatch_get_main_queue()];
            [_session addOutput:_output];
            
            _output.metadataObjectTypes = [_output availableMetadataObjectTypes];
            
            _prevLayer = [AVCaptureVideoPreviewLayer layerWithSession:_session];
            _prevLayer.frame = self.view.bounds;
            _prevLayer.videoGravity = AVLayerVideoGravityResizeAspectFill;
            [self.view.layer addSublayer:_prevLayer];
            
            [_session startRunning];
            
            self.navigationItem.leftBarButtonItem = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemCancel target:self action:@selector(cancelCamera:)];
        }
    }
//    else{
//        if (buttonIndex == 0) {
//            [self fillAadhaarValues];
//        }
//    }
}

- (void) cancelCamera: (id) sender{
    [_session stopRunning];
    _session = nil;
    
    [_prevLayer removeFromSuperlayer];
    _prevLayer = nil;
    
    self.navigationItem.leftBarButtonItem = nil;
}

#pragma mark QRScanner Delegate Method 
- (void)captureOutput:(AVCaptureOutput *)captureOutput didOutputMetadataObjects:(NSArray *)metadataObjects fromConnection:(AVCaptureConnection *)connection
{
    CGRect highlightViewRect = CGRectZero;
    AVMetadataMachineReadableCodeObject *barCodeObject;
    NSString *detectionString = nil;
    NSArray *barCodeTypes = @[AVMetadataObjectTypeUPCECode, AVMetadataObjectTypeCode39Code, AVMetadataObjectTypeCode39Mod43Code,
                              AVMetadataObjectTypeEAN13Code, AVMetadataObjectTypeEAN8Code, AVMetadataObjectTypeCode93Code, AVMetadataObjectTypeCode128Code,
                              AVMetadataObjectTypePDF417Code, AVMetadataObjectTypeQRCode, AVMetadataObjectTypeAztecCode];
    
    for (AVMetadataObject *metadata in metadataObjects) {
        for (NSString *type in barCodeTypes) {
            if ([metadata.type isEqualToString:type])
            {
                barCodeObject = (AVMetadataMachineReadableCodeObject *)[_prevLayer transformedMetadataObjectForMetadataObject:(AVMetadataMachineReadableCodeObject *)metadata];
                highlightViewRect = barCodeObject.bounds;
                detectionString = [(AVMetadataMachineReadableCodeObject *)metadata stringValue];
                break;
            }
        }
        
        if (detectionString != nil)
        {
            NSLog(@"scanned string text %@",detectionString);
            //            NSError *err;
            //            NSXMLDocument *xmlDoc = [[NSXMLDocument alloc] initWithContentsOfURL:furl options:(NSXMLDocumentValidate | NSXMLNodePreserveAll) error:&err];
            //
            //            BOOL vaildXML = [xmlDoc validateAndReturnError:&err];
            //            NSLog(@"Error : %@",[err description]);
            
            detectionString = [detectionString stringByReplacingOccurrencesOfString:@"</?xml" withString:@"<?xml"];
            
            NSError *parseError = nil;
            xmlDictionary = [XMLReader dictionaryForXMLString:detectionString error:&parseError];
            
            if (!xmlDictionary) {
                [kAppDelegate showAlertView:@"Invalid document! Scan only your Aadhaar card"];
                [_session stopRunning];
                [_prevLayer removeFromSuperlayer];
            }
            else{
//                if (![[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"name"] isEqualToString:[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]) {
//                    [self showAadhaarAlertView];
//                }
//                else{
//                    [self fillAadhaarValues];
//                }
                
                [self fillAadhaarValues];
                
                [_session stopRunning];
                [_prevLayer removeFromSuperlayer];
            }
            break;
            //            NSError *error;
            //            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:xmlDictionary
            //                                                               options:NSJSONWritingPrettyPrinted // Pass 0 if you don't care about the readability of the generated string
            //                                                                 error:&error];
            //
            //            if (jsonData) {
            //                NSString* jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            //
            //                NSLog(@"%@",jsonString);
            //
            //                if (![[[jsonString valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"name"] isEqualToString:[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]) {
            //                    [self showAadhaarAlertView];
            //                }
            //
            //                [_session stopRunning];
            //                [_prevLayer removeFromSuperlayer];
            //            } else {
            //                NSLog(@"Got an error: %@", error);
            //            }
            
        }
    }
}

//-(void)showAadhaarAlertView{
//    UIAlertView* aadhaarAlertView = [[UIAlertView alloc]initWithTitle:kAppTitle message:[NSString stringWithFormat:@"%@ name isn't matched with scanned Aadhaar name! Do you want to proceed with this Aadhaar details?",kAppTitle] delegate:self cancelButtonTitle:@"Accept" otherButtonTitles:@"Decline", nil];
//    [aadhaarAlertView setTag:aadhaarSecondAlertViewTag];
//    [aadhaarAlertView show];
//}

-(void)fillAadhaarValues{
    
    NSString* firstName = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"name"];
    NSString* lastName;
    NSArray* arr = [firstName componentsSeparatedByString:@" "];
    firstName = [arr objectAtIndex:0];
    self.firstNameText.text = firstName;
    
    if ([arr count]>1) {
        lastName = [arr objectAtIndex:1];
        self.lastNameTextfield.text = lastName;
    }
    
    NSString* strGender = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"gender"];
    if ([strGender isEqualToString:@"M"] || [strGender isEqualToString:@"MALE"]) {
        [self.femaleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        [self.otherButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        [self.maleButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
        
        genderString = @"M";
    }
    else if ([strGender isEqualToString:@"F"] || [strGender isEqualToString:@"FEMALE"]){
        [self.femaleButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
        [self.otherButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        [self.maleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        
        genderString = @"F";
    }
    else{
        [self.femaleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        [self.otherButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
        [self.maleButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        
        genderString = @"U";
    }
    
    self.aadhaarTextfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"uid"];
//    self.districtTextfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"dist"];
    [self.stateButton setTitle:[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"state"] forState:UIControlStateNormal];
//    self.pinTextfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"pc"];
    
//    if (![[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"house"] && ![[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"]) {
//        self.addressLine1Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"vtc"];
//    }
//    else if (![[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"house"] && [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"]){
//        self.addressLine1Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"];
//        
//        self.addressLine2Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"vtc"];
//    }
//    else{
//        NSString* strAddress = [NSString stringWithFormat:@"%@, %@",[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"house"],[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"]];
//        
//        self.addressLine1Textfield.text = strAddress;
//        
//        self.addressLine2Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"vtc"];
//    }
    
//    if ([[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"dob"]){
//        
//        NSDateFormatter *dateFormatDB = [[NSDateFormatter alloc] init];
//        [dateFormatDB setDateFormat:@"yyyy-MM-dd"];
//        
//        NSDateFormatter *dateFormatMobile = [[NSDateFormatter alloc] init];
//        [dateFormatMobile setDateFormat:@"dd-MM-yyyy"];
//        
//        //                    NSString* deviceDate = self.dateButton.titleLabel.text;
//        NSDate* date = [dateFormatDB dateFromString:[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"dob"]];
//        
//        NSString* dobString = [dateFormatMobile stringFromDate:date];
//        
//        [_dobButton setTitle:dobString forState:UIControlStateNormal];
//    }
    
    for (int i=1; i<[statesArray count]; i++) {
        if ([[[_stateButton titleLabel].text uppercaseString] isEqualToString:[[[statesArray objectAtIndex:i] valueForKey:@"StateName"] uppercaseString]]) {
            stateID = [[statesArray objectAtIndex:i] valueForKey:@"StateId"];
            break;
        }
    }
}

@end
