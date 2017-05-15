//
//  UserProfileViewController.m
//  mSwasthya-VaccinationAlertsApp
//
//  Created by Gagandeep Singh on 09/04/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "UserProfileViewController.h"
#import "SWRevealViewController.h"
#import "AllergyTableViewCell.h"
#import "XLMediaZoom.h"
#import "XLVideoZoom.h"

typedef enum
{
    datePickerTag = 100,
    doneButtonTag,
    canelButtonTag,
    genderPickerTag,
    genderDoneButtonTag,
    bloodGroupPickerTag,
    bloodGroupDoneButtonTag,
    statePickerTag,
    stateDoneButtonTag,
    disabilityTypePickerTag,
    disabilityTypeDoneButtonTag,
    aadhaarFirstAlertViewTag,
    aadhaarSecondAlertViewTag,
    aadhaarInfoUpdateTag
    
}userProfileTags;

@interface UserProfileViewController (){
    SWRevealViewController *revealController;
    CAGradientLayer* gradient;
    
    UIView* dobPickerView;
    UIView* genderPickerView;
    UIView* bloodGroupPickerView;
    UIView* statePickerView;
    UIView* disabilityTypePickerView;
    
    NSMutableArray* genderArray;
    NSString* genderString;
    NSString* gender;
    
    NSMutableArray* bloodGroupArray;
    NSString* bloodGroupString;
    NSString* bloodGroupID;
    
    NSMutableArray* statesArray;
    NSString* stateString;
    NSString* stateID;
    
    NSMutableArray* disabilityTypeArray;
    NSString* disabilityTypeString;
    NSString* disabilityTypeID;
    
    BOOL isGenderPickerView;
    BOOL isStatePickerView;
    BOOL isBloodGroupPickerView;
    
    NSString* disabledString;
    
    NSMutableArray* userProfileArray;
    NSMutableArray* base64ImagesArray;
    
    AVCaptureSession *_session;
    AVCaptureDevice *_device;
    AVCaptureDeviceInput *_input;
    AVCaptureMetadataOutput *_output;
    AVCaptureVideoPreviewLayer *_prevLayer;
    
    NSDictionary *xmlDictionary;
    BOOL updateAadhaarInfo;
}
@property (weak, nonatomic) IBOutlet UIScrollView *userProfileScrollView;
@property (strong, nonatomic) XLMediaZoom *imageZoomView;

@end

@implementation UserProfileViewController
@synthesize isFromDashboard;

-(void)viewWillLayoutSubviews{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.userProfileScrollView setContentSize:CGSizeMake(self.userProfileScrollView.frame.size.width, self.userProfileScrollView.frame.size.height+350)];
    }
    else{
        if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
            [self.userProfileScrollView setContentSize:CGSizeMake(self.userProfileScrollView.frame.size.width, self.view.frame.size.height+600)];
        }
        else{
            [self.userProfileScrollView setContentSize:CGSizeMake(self.userProfileScrollView.frame.size.width, self.view.frame.size.height+150)];
        }
    }
}

//- (void)viewDidUnload {
//    if (updateAadhaarInfo) {
//        UIAlertView* aadhaarAlertView=[[UIAlertView alloc]initWithTitle:kAppTitle message:@"Do you want to exit without updating the Aadhaar Card information in user profile?" delegate:self cancelButtonTitle:@"Exit" otherButtonTitles:@"Update", nil];
//        [aadhaarAlertView setTag:aadhaarInfoUpdateTag];
//        [aadhaarAlertView show];
//    }
//}

- (void)viewDidLoad {
    [super viewDidLoad];
    
    userProfileArray = [NSMutableArray new];
    
//    CGSize scrollableSize = CGSizeMake(self.userProfileScrollView.frame.size.width, self.userProfileScrollView.frame.size.height+50);
    
    base64ImagesArray = [NSMutableArray new];
    updateAadhaarInfo = NO;
    
//    [self.userProfileScrollView setContentSize:CGSizeMake(self.userProfileScrollView.frame.size.width, self.view.frame.size.height+50)];
    
//    [self.userProfileScrollView setContentSize:scrollableSize];
    
    
    _usernameLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME];
    self.emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
    
    NSDictionary * navBarTitleTextAttributes = @{ NSForegroundColorAttributeName : [UIColor blackColor]};
    
    self.navigationController.navigationBar.titleTextAttributes=navBarTitleTextAttributes;
    
    gradient = [CAGradientLayer layer];
    gradient.frame = [[UIScreen mainScreen]bounds];
    gradient.colors = [NSArray arrayWithObjects:(id)[[UIColor colorWithRed:40.0f/255.0f green:44.0f/255.0f blue:75.0f/255.0f alpha:1.0f] CGColor], (id)[[UIColor colorWithRed:25.0f/255.0f green:50.0f/255.0f blue:127.0f/255.0f alpha:1.0f ] CGColor], nil];
//    [self.view.layer insertSublayer:gradient atIndex:0];
    
    //Set Left Bar Button Item
    if (!isFromDashboard) {
        [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
//            isFromDashboard = NO;
            [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
            [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
            self.navigationItem.leftBarButtonItem=barItem;
        }];
    }
    
    [self.userImageView addGestureRecognizer:[[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(imageDidTouch:)]];
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        
        
        NSString *statesNameFilePath = [[NSBundle mainBundle] pathForResource:@"States" ofType:@"plist"];
        statesArray = [[NSMutableArray alloc] initWithContentsOfFile:statesNameFilePath];
        
        NSString *bloodGroupNameFilePath = [[NSBundle mainBundle] pathForResource:@"BloodGroups" ofType:@"plist"];
        bloodGroupArray = [[NSMutableArray alloc] initWithContentsOfFile:bloodGroupNameFilePath];
        
        NSString *disabilityTypeNameFilePath = [[NSBundle mainBundle] pathForResource:@"DisabilityTypes" ofType:@"plist"];
        disabilityTypeArray = [[NSMutableArray alloc] initWithContentsOfFile:disabilityTypeNameFilePath];
        
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
        
        dispatch_async(dispatch_get_main_queue(), ^{
        
            [self addGenderPicker];
            [self addBloodGroupPicker];
            [self addStatePicker];
            [self addDisabilityTypePicker];
            
            [self addDateOfBirthPicker];
            
            if (imageData) {
                //        NSString *str = [self contentTypeForImageData:imageData];
                //        if ([str isEqualToString:@"image/gif"]) {
                //            NSURL *url = [[NSBundle mainBundle] URLForResource:@"test" withExtension:@"gif"];
                //            [_userImageView setImage:[UIImage imageWithData:imageData]];
                //        }
                //        else{
                [_userImageView setImage:[UIImage imageWithData:imageData]];
                //        }
            }
            else{
                [_userImageView setImage:[UIImage imageNamed:@"userImage"]];
            }
            
        });
    });
    
    /*NSData *data = [[[NSUserDefaults standardUserDefaults] valueForKey:USERPROFILE] dataUsingEncoding:NSUTF8StringEncoding];
    id json = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
    
    NSDictionary* dict = [[NSDictionary alloc] initWithDictionary:json];
    
    _firstNameTextfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"] valueForKey:@"FirstName"]];
    _lastNameTextfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"] valueForKey:@"LastName"]];
    _emailLabel.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"] valueForKey:@"Email"]];
    _aadhaarNoTextfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"] valueForKey:@"Uhid"]];
    
    [_bloodGroupButton setTitle:[NSString stringWithFormat:@"%@",[[dict valueForKey:@"i"] valueForKey:@"Name"]] forState:UIControlStateNormal];
    bloodGroupID = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"i"] valueForKey:@"Id"]];
    
    _addressLine1Textfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"AddressLine1"]];
    _addressLine2Textfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"AddressLine2"]];
    _districtTextfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"District"]];
    _city_villageTextfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"City_Vill_Town"]];
    
    NSString* dob = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]  valueForKey:@"DOB"]];
    NSArray* arr = [dob componentsSeparatedByString:@"T"];
    dob = [arr objectAtIndex:0];
    [_dobButton setTitle:dob forState:UIControlStateNormal];
    
    [_stateButton setTitle:[NSString stringWithFormat:@"%@",[[dict valueForKey:@"m"]  valueForKey:@"Name"]] forState:UIControlStateNormal];
    stateID = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"m"]  valueForKey:@"Id"]];
    
    NSString* phone = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"Home_Phone"]];
    if ([phone isEqualToString:@"<null>"] || [phone isKindOfClass:[NSNull class]]) {
        _phoneNoLabel.text = @"not available";
    }
    else{
        _phoneNoLabel.text = phone;
    }
    
    if ([[NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"DAbilityType"]] isEqualToString:@"0"]) {
        [self.disabilityNoButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
        [self.disabilityYesButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        
        disabledString = @"false";
        disabilityTypeID = @"0";
        
        self.typeButton.hidden = YES;
        self.typeLabel.hidden = YES;
    }
    else{
        [self.disabilityNoButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        [self.disabilityYesButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
        
        disabledString = @"true";
        disabilityTypeID = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"DAbilityType"]];
    }
    
    _pinTextfield.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"AddressLine1"]];
    _mobileNoLabel.text = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]valueForKey:@"Cell_Phone"]];
    [_typeButton setTitle:[NSString stringWithFormat:@"%@",[[dict valueForKey:@"j"]valueForKey:@"Name"]] forState:UIControlStateNormal];
    
    NSString* genderK = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"k"]  valueForKey:@"Gender"]];
    if ([genderK isEqualToString:@"M"]) {
        [_genderButton setTitle:@"Male" forState:UIControlStateNormal];
        gender = @"M";
    }
    else if ([genderK isEqualToString:@"F"]){
        [_genderButton setTitle:@"Female" forState:UIControlStateNormal];
        gender = @"F";
    }
    else{
        [_genderButton setTitle:@"Undefined" forState:UIControlStateNormal];
        gender = @"U";
    }*/
    
    
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(handleSingleTap)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    [self.userProfileScrollView addGestureRecognizer:singleFingerTap];
    
    genderArray = [[NSMutableArray alloc] initWithObjects:@"Male",@"Female",@"Do Not Specify", nil];
//    bloodGroupArray = [[NSMutableArray alloc] initWithObjects:@"A Negative",@"A Positive",@"AB Negative",@"AB Positive",@"B Negative",@"B Positive",@"O Negative",@"O Positive", nil];
    
//    disabilityTypeArray = [[NSMutableArray alloc] initWithObjects:@"Do Not Specify",@"Others",@"Physical Disability",@"Speech and Language Disorder",@"Vision Lose and Blindness", nil];
    
    self.emailAddressLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
    
    bloodGroupID = @""; 
    gender = @"";
//    stateID = @"";
    disabilityTypeID = @"";
    disabledString = @"";
    
//    CGSize scrollableSize = CGSizeMake( _userProfileScrollView.frame.size.height, _userProfileScrollView.frame.size.width);
//    [_userProfileScrollView setContentSize:scrollableSize];
    
    NSDictionary * attributes;
    
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        self.firstNameLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.lastNameLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.e_mailLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.aadharLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dobLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.genderLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.bloodGroupLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address1Label.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address2Label.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cityLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.phoneLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.disableLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.typeLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.mobileLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.firstNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.lastNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailAddressLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.aadhaarNoTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dobButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.genderButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.bloodGroupButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.addressLine1Textfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.addressLine2Textfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.city_villageTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.phoneNoTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.disabilityYesButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.disabilityNoButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.typeButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.mobileNoLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.firstNameTextfield.textAlignment = NSTextAlignmentLeft;
        self.lastNameTextfield.textAlignment = NSTextAlignmentLeft;
        self.genderButton.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        self.aadhaarNoTextfield.textAlignment = NSTextAlignmentLeft;
        self.addressLine1Textfield.textAlignment = NSTextAlignmentLeft;
        self.addressLine2Textfield.textAlignment = NSTextAlignmentLeft;
        self.districtTextfield.textAlignment = NSTextAlignmentLeft;
        self.stateButton.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        self.dobButton.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        self.bloodGroupButton.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
        self.pinTextfield.textAlignment = NSTextAlignmentLeft;
        self.mobileNoLabel.textAlignment = NSTextAlignmentLeft;
        self.emailAddressLabel.textAlignment = NSTextAlignmentLeft;
        self.city_villageTextfield.textAlignment = NSTextAlignmentLeft;
        self.phoneNoTextfield.textAlignment = NSTextAlignmentLeft;
        
        self.usernameLabel.font = [UIFont systemFontOfSize:22.0f];
        self.emailLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailLabel.textAlignment = NSTextAlignmentLeft;
        
        self.genderLabel.textAlignment = NSTextAlignmentLeft;
        self.lastNameLabel.textAlignment = NSTextAlignmentLeft;
        self.aadharLabel.textAlignment = NSTextAlignmentLeft;
        self.address2Label.textAlignment = NSTextAlignmentLeft;
        self.districtLabel.textAlignment = NSTextAlignmentLeft;
        self.stateLabel.textAlignment = NSTextAlignmentLeft;
        self.pinLabel.textAlignment = NSTextAlignmentLeft;
        self.mobileLabel.textAlignment = NSTextAlignmentLeft;
        
        [self.userImageView.layer setCornerRadius:35];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:18 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"User Personal Information"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        titleLabel.attributedText = attributedText;
        titleLabel.textAlignment = NSTextAlignmentCenter;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:22.0f weight:-1];
        attributes = @{NSFontAttributeName: font};
    }
    else{
//        [self.userProfileScrollView setContentInset:UIEdgeInsetsMake(-64,0,0,0)];
        
        [self.userImageView.layer setCornerRadius:50];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"User Personal Information"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1];
        attributes = @{NSFontAttributeName: font};
    }
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"Edit" style:UIBarButtonItemStylePlain target:self action:@selector(editableControls)];
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
    [self.userImageView.layer setMasksToBounds:YES];
    
    [self getUserProfileAPI];
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
    [self enableEditing:NO];
}

#pragma mark Check Image Type
- (NSString *)contentTypeForImageData:(NSData *)data {
    uint8_t c;
    [data getBytes:&c length:1];
    
    switch (c) {
        case 0xFF:
            return @"image/jpeg";
        case 0x89:
            return @"image/png";
        case 0x47:
            return @"image/gif";
        case 0x49:
            break;
        case 0x42:
            return @"image/bmp";
        case 0x4D:
            return @"image/tiff";
    }
    return nil;
}

- (void)imageDidTouch:(UIGestureRecognizer *)recognizer
{
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:nil message:nil preferredStyle:UIAlertControllerStyleActionSheet];
    
    UIAlertAction* camera = [UIAlertAction actionWithTitle:@"Take Photo" style:UIAlertActionStyleDestructive handler:^(UIAlertAction *action) {
        
        UIImagePickerController *picker = [[UIImagePickerController alloc] init];
        picker.delegate = self;
        picker.allowsEditing = YES;
        picker.sourceType = UIImagePickerControllerSourceTypeCamera;
        
        if([[UIDevice currentDevice]orientation] == UIDeviceOrientationFaceUp)
        {
            if([UIApplication sharedApplication].statusBarOrientation == UIInterfaceOrientationLandscapeLeft)
            {
                [[UIDevice currentDevice]setValue:[NSNumber numberWithInteger:UIDeviceOrientationLandscapeRight] forKey:@"orientation"];
            }
            else
            {
                [[UIDevice currentDevice]setValue:[NSNumber numberWithInteger:UIDeviceOrientationLandscapeLeft] forKey:@"orientation"];
            }
        }
        
        [self presentViewController:picker animated:YES completion:NULL];
    }];
    
    UIAlertAction* photoGallary = [UIAlertAction actionWithTitle:@"Choose Photo" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        
        UIImagePickerController *picker = [[UIImagePickerController alloc] init];
        picker.delegate = self;
        picker.allowsEditing = YES;
        picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
        
        [self presentViewController:picker animated:YES completion:NULL];
        
    }];
    
    UIAlertAction* viewPhoto = [UIAlertAction actionWithTitle:@"View Photo" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        
        [self viewProfilePic];
        
    }];
    
    UIAlertAction* cancel = [UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
        
        
    }];
    
    alertController.popoverPresentationController.barButtonItem = nil;
    alertController.popoverPresentationController.sourceView = self.view;
    alertController.popoverPresentationController.sourceRect = CGRectMake(self.view.bounds.size.width/2-250, 200, 1.0, 1.0);
    
    [alertController addAction:camera];
    [alertController addAction:photoGallary];
    [alertController addAction:viewPhoto];
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
    self.userImageView.image = image;
    //    }
    //    else{
    //        self.attachment2ImageView.image = image;
    //    }
    
    [self UpdateUserProfileImageAPI];
}


#pragma mark Tap Gesture
- (void)viewProfilePic
{
    [self.view addSubview:self.imageZoomView];
    [self.imageZoomView show];
}

#pragma mark Delegates
- (XLMediaZoom *)imageZoomView
{
    if (_imageZoomView) return _imageZoomView;
    
    _imageZoomView = [[XLMediaZoom alloc] initWithAnimationTime:@(0.5) image:self.userImageView blurEffect:YES];
    _imageZoomView.tag = 1;
    _imageZoomView.backgroundColor = [UIColor colorWithRed:0.0 green:0.05 blue:0.3 alpha:1.0];
    
    return _imageZoomView;
}

#pragma mark Update User Profile Image 
-(void)UpdateUserProfileImageAPI{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Updating..."];//Show loading indicator.
        
        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"userID"];
        [dicParams setObject:[base64ImagesArray objectAtIndex:0] forKey:@"Text"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        
        [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
//        [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
        
        [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
        
        manager.requestSerializer = requestSerializer;
        [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            NSLog(@"Result dict %@",responseObject);
            
            if ([[responseObject valueForKey:@"Status"] integerValue]==1) {
                [kAppDelegate showAlertView:@"Profile photo updated"];
                
                NSString* userImage = [NSString stringWithFormat:@""];
                userImage = [userImage stringByReplacingOccurrencesOfString:@"\\"
                                                                 withString:@"/"];
                
                [[NSUserDefaults standardUserDefaults] setValue:userImage forKey:USERIMAGE];
            }
            else{
                [kAppDelegate showAlertView:@"Updated"];
            }
            
//            [kAppDelegate setIsUserProfileUpdated:1];
            
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

-(NSString *)imageToNSString:(UIImage *)image
{
    NSData *imageData = UIImagePNGRepresentation(image);
    return [imageData base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
    
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        genderPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
    }];
}

#pragma mark Text Field Delegate
-(void)textFieldDidBeginEditing:(UITextField *)textField {
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
        genderPickerView.frame = CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200);
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
    
    if (textField==self.aadhaarNoTextfield) {
        if(([newString length] > 12)){
            
            [kAppDelegate showAlertView:@"Aadhaar no. max length 12 digits"];
            return NO;
        }
    }
    else if (textField==self.pinTextfield) {
        if(([newString length] > 6)){
            
            [kAppDelegate showAlertView:@"Pin no. max length 6 digits"];
            return NO;
        }
    }
    else if (textField==self.phoneNoTextfield) {
        if(([newString length] > 10)){
            
            [kAppDelegate showAlertView:@"Phone no. max length 10 digits"];
            return NO;
        }
    }
    
    return YES;
}

#pragma mark Enable Editing Mode
-(void)editableControls{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [self enableEditing:YES];
        [self.navigationItem.rightBarButtonItem setTitle:@"Update"];
        
        if ([self.addressLine1Textfield.text isEqualToString:@"-"]) {
            self.addressLine1Textfield.text=@"";
        }
        if ([self.addressLine2Textfield.text isEqualToString:@"-"]) {
            self.addressLine2Textfield.text=@"";
        }
        if ([self.stateButton.titleLabel.text isEqualToString:@"-"]) {
            [self.stateButton setTitle:@"Select" forState:UIControlStateNormal];
        }
        
        self.addressLine1Textfield.layer.cornerRadius = 3;
        self.addressLine1Textfield.clipsToBounds = YES;
        self.addressLine1Textfield.layer.borderWidth = 0.75f;
        self.addressLine1Textfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
        self.addressLine2Textfield.layer.cornerRadius = 3;
        self.addressLine2Textfield.clipsToBounds = YES;
        self.addressLine2Textfield.layer.borderWidth = 0.75f;
        self.addressLine2Textfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
    }
    else{
        [self updatePersonalProfile];
    }
}

#pragma mark Enable Control For Editing Method
-(void)enableEditing:(BOOL)value{
    
    self.firstNameTextfield.enabled = value;
    self.lastNameTextfield.enabled = value;
    self.aadhaarNoTextfield.enabled = value;
    self.addressLine1Textfield.editable = value;
    self.addressLine2Textfield.editable = value;
    self.districtTextfield.enabled = value;
    self.city_villageTextfield.enabled = value;
    self.pinTextfield.enabled = value;
    self.phoneNoTextfield.enabled = value;
    
    UIColor* color;
    if (value) {
        value = NO;
        color = [UIColor blueColor];
    }
    else{
        value = YES;
        color = [UIColor darkGrayColor];
    }
//
//    self.dobButton.titleLabel.textColor = color;
//    self.genderButton.titleLabel.textColor = color;
//    self.bloodGroupButton.titleLabel.textColor = color;
//    self.stateButton.titleLabel.textColor = color;
//    self.addressLine1Textfield.backgroundColor = color;
//    self.addressLine2Textfield.backgroundColor = color;
//    self.disabilityNoButton.backgroundColor = color;
//    self.typeButton.titleLabel.textColor = color;
    
    self.firstNameImage.hidden = value;
    self.lastNameImage.hidden = value;
    self.aadhaarImage.hidden = value;
    self.phoneNoImage.hidden = value;
    self.addressLine2Image.hidden = value;
    self.districtImage.hidden = value;
    self.cityImage.hidden = value;
    self.pinImage.hidden = value;
    
//    self.aadhaarButton.hidden = value;
}

#pragma mark ScrollView Delegate
- (void)scrollViewDidScroll:(UIScrollView *)sender {
    if (sender.contentOffset.x != 0) {
        CGPoint offset = sender.contentOffset;
        offset.x = 0;
        sender.contentOffset = offset;
    }
}

#pragma mark Show Rear View
- (void)revealAppointmentView:(id)sender {
    
    revealController = [self revealViewController];
    [revealController revealToggleAnimated:YES];
    
//    if (self.view.userInteractionEnabled==NO) {
//        [self.view setUserInteractionEnabled:YES];
//    }
//    else{
//        [self.view setUserInteractionEnabled:NO];
//    }
}

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    [UIView animateWithDuration:2.0 animations:^{
        gradient.frame = self.view.bounds;
    }];
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
//    CGRect screenRect = [[UIScreen mainScreen] bounds];
//    CGFloat screenWidth = screenRect.size.width;
//    CGFloat screenHeight = screenRect.size.height;
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.userProfileScrollView setContentSize:CGSizeMake(self.userProfileScrollView.frame.size.width, self.view.frame.size.height+300)];
    }
    else{
        [self.userProfileScrollView setContentSize:CGSizeMake(self.userProfileScrollView.frame.size.width, self.view.frame.size.height+50)];
    }
}

#pragma mark Create DOB datepicker custom view 
-(void)addDateOfBirthPicker{
    
    // creating custom view for DOB
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        dobPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
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
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
    
    NSDateFormatter *formatter=[[NSDateFormatter alloc]init];
    [formatter setDateFormat:@"dd/MM/yyyy"];
    
    UIDatePicker* dobDatePicker = (UIDatePicker*)[dobPickerView viewWithTag:datePickerTag];
    NSString* dateString = [formatter stringFromDate:dobDatePicker.date];
    
    [self.dobButton setTitle:dateString forState:UIControlStateNormal];
}

-(void)pickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        dobPickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create Gender picker custom view 
-(void)addGenderPicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        genderPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
    }
    else{
        genderPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(genderPickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:genderDoneButtonTag];
    [genderPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(genderPickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [genderPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* genderPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
//    genderPicker.dataSource = self;
//    genderPicker.delegate = self;
    
    [genderPicker setTag:genderPickerTag];
    [genderPickerView addSubview:genderPicker];
    
    genderPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:genderPickerView];
}

-(void)genderPickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        genderPickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
    
    [self.genderButton setTitle:genderString forState:UIControlStateNormal];
}

-(void)genderPickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        genderPickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
}

#pragma mark UIPickerView Delegates
- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView{
    return 1;
}

- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component{
    if (isGenderPickerView) {
        return [genderArray count];
    }
    else if (isBloodGroupPickerView){
        return [bloodGroupArray count];
    }
    else if (isStatePickerView){
        return [statesArray count];
    }
    else{
        return [disabilityTypeArray count];
    }
}

- (NSString *)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    if (isGenderPickerView) {
        genderString = [genderArray objectAtIndex:row];
        return genderString;
    }
    else if(isBloodGroupPickerView){
        bloodGroupString = [[bloodGroupArray objectAtIndex:row] valueForKey:@"Name"];
        return bloodGroupString;
    }
    else if(isStatePickerView){
        stateString = [[statesArray objectAtIndex:row] valueForKey:@"StateName"];
        return stateString;
    }
    else{
        disabilityTypeString = [[disabilityTypeArray objectAtIndex:row] valueForKey:@"Name"];
        return disabilityTypeString;
    }
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{
    if (isGenderPickerView) {
        [self.genderButton setTitle:[genderArray objectAtIndex:row] forState:UIControlStateNormal];
        if (row==0) {
            gender = @"M";
        }
        else if (row==1) {
            gender = @"F";
        }
        else{
            gender = @"U";
        }
    }
    else if(isBloodGroupPickerView){
        [self.bloodGroupButton setTitle:[[bloodGroupArray objectAtIndex:row] valueForKey:@"Name"] forState:UIControlStateNormal];
        bloodGroupID = [[bloodGroupArray objectAtIndex:row] valueForKey:@"Id"];
    }
    else if(isStatePickerView){
        stateString = [[statesArray objectAtIndex:row] valueForKey:@"StateName"];
        
        [self.stateButton setTitle:stateString forState:UIControlStateNormal];
        stateID = [[statesArray objectAtIndex:row] valueForKey:@"StateId"];
    }
    else{
        [self.typeButton setTitle:[[disabilityTypeArray objectAtIndex:row] valueForKey:@"Name"] forState:UIControlStateNormal];
        disabilityTypeID = [[disabilityTypeArray objectAtIndex:row] valueForKey:@"Id"];
    }
}

#pragma mark Create Blood Group picker custom view 
-(void)addBloodGroupPicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        bloodGroupPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
    }
    else{
        bloodGroupPickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(bloodGroupPickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:bloodGroupDoneButtonTag];
    [bloodGroupPickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(bloodGroupPickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [bloodGroupPickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* bloodGroupPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
//    bloodGroupPicker.dataSource = self;
//    bloodGroupPicker.delegate = self;
    
    [bloodGroupPicker setTag:bloodGroupPickerTag];
    [bloodGroupPickerView addSubview:bloodGroupPicker];
    
    bloodGroupPickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:bloodGroupPickerView];
}

-(void)bloodGroupPickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        bloodGroupPickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
    
    [self.bloodGroupButton setTitle:bloodGroupString forState:UIControlStateNormal];
}

-(void)bloodGroupPickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        bloodGroupPickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
}

#pragma mark Create Disability Type picker custom view 
-(void)addDisabilityTypePicker{
    
    // creating custom view for gender
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        disabilityTypePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+500, self.view.frame.size.width, 200)];
    }
    else{
        disabilityTypePickerView = [[UIView alloc]initWithFrame:CGRectMake(0, self.view.frame.size.height+200, self.view.frame.size.width, 200)];
    }
    
    // adding DONE button of picker view
    UIButton* doneButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [doneButton setFrame:CGRectMake([[UIScreen mainScreen] bounds].size.width-70, 2, 60, 30)];
    [doneButton setTitle:@"DONE" forState:UIControlStateNormal];
    [doneButton addTarget:self action:@selector(disabilityTypePickerDoneButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [doneButton setTag:disabilityTypeDoneButtonTag];
    [disabilityTypePickerView addSubview:doneButton];
    
    // adding CANCEL button of picker view
    UIButton* cancelButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [cancelButton setFrame:CGRectMake(10, 2, 80, 30)];
    [cancelButton setTitle:@"CANCEL" forState:UIControlStateNormal];
    [cancelButton addTarget:self action:@selector(disabilityTypePickerCancelButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [disabilityTypePickerView addSubview:cancelButton];
    
    // adding Date Picker
    UIPickerView* bloodGroupPicker = [[UIPickerView alloc]initWithFrame:CGRectMake(0, 30, [[UIScreen mainScreen] bounds].size.width, 150)];
    //    bloodGroupPicker.dataSource = self;
    //    bloodGroupPicker.delegate = self;
    
    [bloodGroupPicker setTag:disabilityTypePickerTag];
    [disabilityTypePickerView addSubview:bloodGroupPicker];
    
    disabilityTypePickerView.backgroundColor = [UIColor grayColor];
    [self.view addSubview:disabilityTypePickerView];
}

-(void)disabilityTypePickerDoneButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        disabilityTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
    
//    [self.typeButton setTitle:disabilityTypeString forState:UIControlStateNormal];
}

-(void)disabilityTypePickerCancelButtonAction{
    [UIView animateWithDuration:0.75 animations:^{
        disabilityTypePickerView.frame = CGRectMake(0, self.view.frame.size.height+250, self.view.frame.size.width, 200);
    }];
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

#pragma mark API Call
-(void)getUserProfileAPI{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Getting..."];//Show loading indicator.
        
        //Parameter.
//        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
//        [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"userid"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                userProfileArray = [responseObject valueForKey:@"response"];
                
                _firstNameTextfield.text = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"FirstName"]];
                _lastNameTextfield.text = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"LastName"]];
                _emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
                
                NSString* aadhar = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"Uhid"]];
                if ([aadhar isKindOfClass:[NSNull class]] || [aadhar isEqualToString:@"<null>"] || [aadhar isEqualToString:@""]) {
                    _aadhaarNoTextfield.placeholder = @"-";
                }
                else{
                    _aadhaarNoTextfield.text = aadhar;
                }
                
                [_bloodGroupButton setTitle:[NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"strBloodType"]] forState:UIControlStateNormal];
                bloodGroupID = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"BloodType"]];
                
                NSString* addressLine1= [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"AddressLine1"]];
                if ([addressLine1 isKindOfClass:[NSNull class]] || [addressLine1 isEqualToString:@"<null>"] || [addressLine1 isEqualToString:@""]) {
                    _addressLine1Textfield.text = @"-";
                }
                else{
                    _addressLine1Textfield.text = addressLine1;
                }
                
                NSString* addressLine2= [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"AddressLine2"]];
                if ([addressLine2 isKindOfClass:[NSNull class]] || [addressLine2 isEqualToString:@"<null>"] || [addressLine2 isEqualToString:@""]) {
                    _addressLine2Textfield.text = @"-";
                }
                else{
                    _addressLine2Textfield.text = addressLine2;
                }
                
                NSString* district = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"District"]];
                if ([district isKindOfClass:[NSNull class]] || [district isEqualToString:@"<null>"] || [district isEqualToString:@""]) {
                    _districtTextfield.placeholder = @"-";
                }
                else{
                    _districtTextfield.text = district;
                }
                
                NSString* city = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"City_Vill_Town"]];
                if ([city isKindOfClass:[NSNull class]] || [city isEqualToString:@"<null>"] || [city isEqualToString:@""]) {
                    _city_villageTextfield.placeholder = @"-";
                }
                else{
                    _city_villageTextfield.text = city;
                }
                
                NSString* dob = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"DOB"]];
                NSArray* arr = [dob componentsSeparatedByString:@"T"];
                dob = [arr objectAtIndex:0];
                if (![dob isEqualToString:@"0001-01-01"]) {
                    
                    NSDateFormatter *dateFormatDB = [[NSDateFormatter alloc] init];
                    [dateFormatDB setDateFormat:@"yyyy-MM-dd"];
                    
                    NSDateFormatter *dateFormatMobile = [[NSDateFormatter alloc] init];
                    [dateFormatMobile setDateFormat:@"dd-MM-yyyy"];
                    
//                    NSString* deviceDate = self.dateButton.titleLabel.text;
                    NSDate* date = [dateFormatDB dateFromString:dob];
                    
                    NSString* dobString = [dateFormatMobile stringFromDate:date];
                    
                    [_dobButton setTitle:dobString forState:UIControlStateNormal];
                }
                
//                stateID = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"State"]];
//                if ([stateID intValue]!=0) {
//                    [_stateButton setTitle:[NSString stringWithFormat:@"%@",[[statesArray objectAtIndex:[stateID intValue]-1] valueForKey:@"StateName"]] forState:UIControlStateNormal];
//                }
                
                int stateNo = [[userProfileArray valueForKey:@"State"] intValue];
                if (stateNo==0) {
                    [_stateButton setTitle:@"-" forState:UIControlStateNormal];
                }
                else{
                    [_stateButton setTitle:[NSString stringWithFormat:@"%@",[[statesArray objectAtIndex:stateNo-1] valueForKey:@"StateName"]] forState:UIControlStateNormal];
                    stateID = [NSString stringWithFormat:@"%d",stateNo];
                }
                
                NSString* phone = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"Home_Phone"]];
                if ([phone isKindOfClass:[NSNull class]] || [phone isEqualToString:@"<null>"] || [phone isEqualToString:@""]) {
                    _phoneNoTextfield.text = @"-";
                }
                else{
                    _phoneNoTextfield.text = phone;
                }
                
                if ([[NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"DiffAbled"]] isEqualToString:@"0"]) {
                    [self.disabilityNoButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
                    [self.disabilityYesButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
                    
                    disabledString = @"false";
                    disabilityTypeID = @"5";
                    
                    self.typeButton.hidden = YES;
                    self.typeLabel.hidden = YES;
                }
                else{
                    [self.disabilityNoButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
                    [self.disabilityYesButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
                    
                    disabledString = @"true";
                    disabilityTypeID = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"DAbilityType"]];
                }
                
                NSString* pin = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"Pin"]];
                if ([pin isKindOfClass:[NSNull class]] || [pin isEqualToString:@"<null>"] || [pin isEqualToString:@""]) {
                    _pinTextfield.placeholder = @"-";
                }
                else{
                    _pinTextfield.text = pin;
                }
                
                NSString* mobile = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"Cell_Phone"]];
                if ([mobile isKindOfClass:[NSNull class]] || [mobile isEqualToString:@"<null>"] || [mobile isEqualToString:@""]) {
                    _mobileNoLabel.text = @"-";
                }
                else{
                    _mobileNoLabel.text = mobile;
                }
                
                [_typeButton setTitle:[NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"strDisabType"]] forState:UIControlStateNormal];
                
                NSString* genderK = [NSString stringWithFormat:@"%@",[userProfileArray valueForKey:@"Gender"]];
                if ([genderK isEqualToString:@"M"]) {
                    [_genderButton setTitle:@"Male" forState:UIControlStateNormal];
                    gender = @"M";
                }
                else if ([genderK isEqualToString:@"F"]){
                    [_genderButton setTitle:@"Female" forState:UIControlStateNormal];
                    gender = @"F";
                }
                else{
                    [_genderButton setTitle:@"Undefined" forState:UIControlStateNormal];
                    gender = @"U";
                }
            }
            else{
                NSString* username = [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME];
                NSArray* arr = [username componentsSeparatedByString:@" "];
                
                _firstNameTextfield.text = [arr objectAtIndex:0];
                _lastNameTextfield.text = [arr objectAtIndex:1];
                _emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
                
//                [kAppDelegate showAlertView:@"Request failed"];
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

#pragma mark Update User Info API Call 
-(void)updatePersonalProfile{
    if (self.firstNameTextfield.text.length==0) {
        [self.firstNameTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Enter first name"];
    }
    else if (self.lastNameTextfield.text.length==0) {
        [self.lastNameTextfield becomeFirstResponder];
        [kAppDelegate showAlertView:@"Enter last name"];
    }
//    else if (self.resultTextfield.text.length==0) {
//        [kAppDelegate showAlertView:@"Enter test result value"];
//        [self.resultTextfield becomeFirstResponder];
//    }
    else if (self.aadhaarNoTextfield.text.length!=12) {
        [kAppDelegate showAlertView:@"Aadhaar no. length should be 12 digits"];
        [self.aadhaarNoTextfield becomeFirstResponder];
    }
    else if (self.pinTextfield.text.length<6 && self.pinTextfield.text.length>0) {
        [kAppDelegate showAlertView:@"Invalid pin no."];
        [self.pinTextfield becomeFirstResponder];
    }
//    else if (self.phoneNoTextfield.text.length>0 && self.phoneNoTextfield.text.length!=10) {
//        [kAppDelegate showAlertView:@"Phone no. length should be 10 digits"];
//        [self.phoneNoTextfield becomeFirstResponder];
//    }
    else{
        if ([kAppDelegate hasInternetConnection]) {
            [kAppDelegate showLoadingIndicator:@"Updating..."];//Show loading indicator.
            
//            NSString *uuid = [[NSUUID UUID] UUIDString];
            
            NSString* dob=@"";
            NSLog(@"%lu",[[self.dobButton titleLabel]text].length);
            if ([[[self.dobButton titleLabel]text] isKindOfClass:[NSNull class]] || [[[self.dobButton titleLabel]text]isEqualToString:@""] || [[self.dobButton titleLabel]text].length==0) {
                dob = @"";
            }
            else{
                NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
                [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
                
                NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
                [dateFormatter setDateFormat:@"dd-MM-yyyy"];
                
                NSString* deviceDate = self.dobButton.titleLabel.text;
                NSDate* date = [dateFormatter dateFromString:deviceDate];
                
                dob = [dateFormat stringFromDate:date];
            }
            
//            NSString* dateString = [dateFormat stringFromDate:[NSDate date]];
//            NSArray* array = [dateString componentsSeparatedByString:@"+"];
//            dateString = [array objectAtIndex:0];
            if ([self.addressLine1Textfield.text isEqualToString:@"-"]) {
                self.addressLine1Textfield.text = @"";
            }
            if ([self.addressLine2Textfield.text isEqualToString:@"-"]) {
                self.addressLine2Textfield.text = @"";
            }
            
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
                    [kAppDelegate showAlertView:@"User profile updated"];
                    [self enableEditing:NO];
                    
                    updateAadhaarInfo = NO;
                    
//                    [kAppDelegate setIsUserProfileUpdated:1];
                    
                    NSString* firstName = self.firstNameTextfield.text;
                    NSString* lastName = self.lastNameTextfield.text;
                    NSString* userName = [NSString stringWithFormat:@"%@ %@",firstName,lastName];
                    [[NSUserDefaults standardUserDefaults] setValue:nil forKey:USERNAME];
                    
                    [[NSUserDefaults standardUserDefaults] setValue:userName forKey:USERNAME];
                    
                    self.usernameLabel.text = userName;
                    
//                    NSString* userNameString = [NSString stringWithFormat:@"%@%@",self.firstNameTextfield.text,self.lastNameTextfield.text];
//                    
//                    [[NSUserDefaults standardUserDefaults] setValue:userNameString
//                                                             forKey:USERNAME];

                    [self.navigationItem.rightBarButtonItem setTitle:@"Edit"];
                }
                else{
                    [kAppDelegate showAlertView:@"Please do some changes"];
                    [self enableEditing:NO];
                    [self.navigationItem.rightBarButtonItem setTitle:@"Edit"];
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

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark Button Action Method
- (IBAction)selectDisabilityTypeButtonAction:(id)sender {
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        
        isGenderPickerView = NO;
        isBloodGroupPickerView = NO;
        isStatePickerView = NO;
        
        [self enableEditing:YES];
        [self.view endEditing:YES];
        //    [self gpPickerCancelButtonAction];
        //    isStartDateButton = YES;
        
        UIPickerView* picker = (UIPickerView*)[disabilityTypePickerView viewWithTag:disabilityTypePickerTag];
//        [picker setMinimumDate:[NSDate date]];
        picker.delegate = self;
        picker.dataSource = self;
        
        [UIView animateWithDuration:0.75 animations:^{
            
            if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
                disabilityTypePickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
                
                picker.frame = CGRectMake(0, 30, disabilityTypePickerView.frame.size.width, 200);
                
                UIButton* doneButton = (UIButton*)[disabilityTypePickerView viewWithTag:disabilityTypeDoneButtonTag];
                doneButton.frame = CGRectMake(disabilityTypePickerView.frame.size.width-70, 2, 60, 30);
            }
            else{
                disabilityTypePickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            }
        }];
    }
}

- (IBAction)disabilityYesButtonAction:(id)sender {
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        [self.disabilityNoButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        [self.disabilityYesButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
        
        disabledString = @"true";
        
        [UIView animateWithDuration:1.0 animations:^{
            self.typeButton.hidden = NO;
            self.typeLabel.hidden = NO;
        }];
    }
}

- (IBAction)disabilityNoButtonAction:(id)sender {
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        [self.disabilityNoButton setImage:[UIImage imageNamed:@"checked"] forState:UIControlStateNormal];
        [self.disabilityYesButton setImage:[UIImage imageNamed:@"unchecked"] forState:UIControlStateNormal];
        
        disabledString = @"false";
        
        [UIView animateWithDuration:1.0 animations:^{
            self.typeButton.hidden = YES;
            self.typeLabel.hidden = YES;
        }];
    }
}

- (IBAction)selectStateButtonAction:(id)sender {
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        
        isGenderPickerView = NO;
        isBloodGroupPickerView = NO;
        isStatePickerView = YES;
        
        [self enableEditing:YES];
        [self.view endEditing:YES];
        //    [self gpPickerCancelButtonAction];
        //    isStartDateButton = YES;
        
        UIPickerView* picker = (UIPickerView*)[statePickerView viewWithTag:statePickerTag];
//        [picker setMinimumDate:[NSDate date]];
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
}

- (IBAction)genderButtonAction:(id)sender {
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        
        isGenderPickerView = YES;
        isBloodGroupPickerView = NO;
        isStatePickerView = NO;
        
        [self enableEditing:YES];
        [self.view endEditing:YES];
        //    [self gpPickerCancelButtonAction];
        //    isStartDateButton = YES;
        
        UIPickerView* picker = (UIPickerView*)[genderPickerView viewWithTag:genderPickerTag];
        
        picker.dataSource = self;
        picker.delegate = self;
        
        [UIView animateWithDuration:0.75 animations:^{
            
            if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
                genderPickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
                
                picker.frame = CGRectMake(0, 30, genderPickerView.frame.size.width, 200);
                
                UIButton* doneButton = (UIButton*)[genderPickerView viewWithTag:genderDoneButtonTag];
                doneButton.frame = CGRectMake(genderPickerView.frame.size.width-70, 2, 60, 30);
            }
            else{
                genderPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            }
        }];
    }
}
- (IBAction)selectBloodGroupButtonAction:(id)sender {
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        
        isGenderPickerView = NO;
        isBloodGroupPickerView = YES;
        isStatePickerView = NO;
        
        [self enableEditing:YES];
        [self.view endEditing:YES];
        //    [self gpPickerCancelButtonAction];
        //    isStartDateButton = YES;
        
        UIPickerView* picker = (UIPickerView*)[bloodGroupPickerView viewWithTag:bloodGroupPickerTag];
        picker.delegate = self;
        picker.dataSource = self;
        
        [UIView animateWithDuration:0.75 animations:^{
            
            if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad){
                bloodGroupPickerView.frame = CGRectMake(0, self.view.frame.size.height-250, self.view.frame.size.width, 250);
                
                picker.frame = CGRectMake(0, 30, bloodGroupPickerView.frame.size.width, 200);
                
                UIButton* doneButton = (UIButton*)[bloodGroupPickerView viewWithTag:bloodGroupDoneButtonTag];
                doneButton.frame = CGRectMake(bloodGroupPickerView.frame.size.width-70, 2, 60, 30);
            }
            else{
                bloodGroupPickerView.frame = CGRectMake(0, self.view.frame.size.height-200, self.view.frame.size.width, 200);
            }
        }];
    }
}

- (IBAction)dateOfBirthButtonAction:(id)sender {
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [kAppDelegate showAlertView:@"Enable editing mode"];
    }
    else{
        
        [self enableEditing:YES];
        [self.view endEditing:YES];
        //    [self gpPickerCancelButtonAction];
        //    isStartDateButton = YES;
        
        UIDatePicker* picker = (UIDatePicker*)[dobPickerView viewWithTag:datePickerTag];
        
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
}

- (IBAction)aadhaarButtonAction:(id)sender {
    [self showAlertView];
}

#pragma mark UIAlertView Delegate Method 
-(void)showAlertView{
    UIAlertView* aadhaarAlertView=[[UIAlertView alloc]initWithTitle:kAppTitle message:@"Do you want to fill user personal information from your Aadhaar Card information?" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"OK", nil];
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
        }
    }
    else if (alertView.tag == aadhaarInfoUpdateTag){
        if (buttonIndex == 1) {
            [self updatePersonalProfile];
        }
    }
    else{
        if (buttonIndex == 0) {
            [self fillAadhaarValues];
        }
    }
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
                if (![[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"name"] isEqualToString:[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]) {
                    [self showAadhaarAlertView];
                }
                else{
                    [self fillAadhaarValues];
                }
                
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

-(void)showAadhaarAlertView{
    UIAlertView* aadhaarAlertView = [[UIAlertView alloc]initWithTitle:kAppTitle message:[NSString stringWithFormat:@"%@ name isn't matched with scanned Aadhaar name! Do you want to proceed with this Aadhaar details?",kAppTitle] delegate:self cancelButtonTitle:@"Accept" otherButtonTitles:@"Decline", nil];
    [aadhaarAlertView setTag:aadhaarSecondAlertViewTag];
    [aadhaarAlertView show];
}

-(void)fillAadhaarValues{
    
    updateAadhaarInfo = YES;
    
    NSString* firstName = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"name"];
    NSString* lastName;
    NSArray* arr = [firstName componentsSeparatedByString:@" "];
    firstName = [arr objectAtIndex:0];
    self.firstNameTextfield.text = firstName;
    
    if ([arr count]>1) {
        lastName = [arr objectAtIndex:1];
        self.lastNameTextfield.text = lastName;
    }
    
    NSString* strGender = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"gender"];
    if ([strGender isEqualToString:@"M"] || [strGender isEqualToString:@"MALE"]) {
        [self.genderButton setTitle:@"Male" forState:UIControlStateNormal];
    }
    else{
        [self.genderButton setTitle:@"Female" forState:UIControlStateNormal];
    }
    
    self.aadhaarNoTextfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"uid"];
    self.districtTextfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"dist"];
    [self.stateButton setTitle:[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"state"] forState:UIControlStateNormal];
    self.pinTextfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"pc"];
    
    if (![[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"house"] && ![[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"]) {
        self.addressLine1Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"vtc"];
    }
    else if (![[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"house"] && [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"]){
        self.addressLine1Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"];
        
        self.addressLine2Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"vtc"];
    }
    else{
        NSString* strAddress = [NSString stringWithFormat:@"%@, %@",[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"house"],[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"loc"]];
        
        self.addressLine1Textfield.text = strAddress;
        
        self.addressLine2Textfield.text = [[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"vtc"];
    }
    
    if ([[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"dob"]){
        
        NSDateFormatter *dateFormatDB = [[NSDateFormatter alloc] init];
        [dateFormatDB setDateFormat:@"yyyy-MM-dd"];
        
        NSDateFormatter *dateFormatMobile = [[NSDateFormatter alloc] init];
        [dateFormatMobile setDateFormat:@"dd-MM-yyyy"];
        
        //                    NSString* deviceDate = self.dateButton.titleLabel.text;
        NSDate* date = [dateFormatDB dateFromString:[[xmlDictionary valueForKey:@"PrintLetterBarcodeData"] valueForKey:@"dob"]];
        
        NSString* dobString = [dateFormatMobile stringFromDate:date];
        
        [_dobButton setTitle:dobString forState:UIControlStateNormal];
    }
    
    for (int i=1; i<[statesArray count]; i++) {
        if ([[[_stateButton titleLabel].text uppercaseString] isEqualToString:[[[statesArray objectAtIndex:i] valueForKey:@"StateName"] uppercaseString]]) {
            stateID = [[statesArray objectAtIndex:i] valueForKey:@"StateId"];
            break;
        }
    }
}

@end
