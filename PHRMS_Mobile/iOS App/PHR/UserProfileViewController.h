//
//  UserProfileViewController.h
//  mSwasthya-VaccinationAlertsApp
//
//  Created by Gagandeep Singh on 09/04/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>
#import "XMLReader.h"
#import "Constants.h"

@interface UserProfileViewController : UIViewController<UIImagePickerControllerDelegate, UINavigationControllerDelegate,UIPickerViewDelegate, UIPickerViewDataSource, NSURLSessionDelegate,AVCaptureMetadataOutputObjectsDelegate>

@property (weak, nonatomic) IBOutlet UILabel *usernameLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImageView;
@property (weak, nonatomic) IBOutlet UITextField *firstNameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *lastNameTextfield;
@property (weak, nonatomic) IBOutlet UILabel *emailAddressLabel;
@property (weak, nonatomic) IBOutlet UITextField *aadhaarNoTextfield;
@property (weak, nonatomic) IBOutlet UIButton *dobButton;
@property (weak, nonatomic) IBOutlet UIButton *genderButton;
@property (weak, nonatomic) IBOutlet UIButton *bloodGroupButton;
@property (weak, nonatomic) IBOutlet UITextView *addressLine1Textfield;
@property (weak, nonatomic) IBOutlet UITextView *addressLine2Textfield;
@property (weak, nonatomic) IBOutlet UITextField *districtTextfield;
@property (weak, nonatomic) IBOutlet UITextField *city_villageTextfield;
@property (weak, nonatomic) IBOutlet UIButton *stateButton;
@property (weak, nonatomic) IBOutlet UITextField *phoneNoTextfield;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;
@property (weak, nonatomic) IBOutlet UITextField *pinTextfield;
@property (weak, nonatomic) IBOutlet UIButton *disabilityYesButton;
@property (weak, nonatomic) IBOutlet UIButton *disabilityNoButton;
@property (weak, nonatomic) IBOutlet UIButton *typeButton;

@property (weak, nonatomic) IBOutlet UILabel *typeLabel;
@property (weak, nonatomic) IBOutlet UILabel *mobileNoLabel;
@property (weak, nonatomic) IBOutlet UILabel *firstNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *lastNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *e_mailLabel;
@property (weak, nonatomic) IBOutlet UILabel *dobLabel;
@property (weak, nonatomic) IBOutlet UILabel *aadharLabel;
@property (weak, nonatomic) IBOutlet UILabel *genderLabel;
@property (weak, nonatomic) IBOutlet UILabel *districtLabel;
@property (weak, nonatomic) IBOutlet UILabel *bloodGroupLabel;
@property (weak, nonatomic) IBOutlet UILabel *address1Label;
@property (weak, nonatomic) IBOutlet UILabel *address2Label;
@property (weak, nonatomic) IBOutlet UILabel *cityLabel;
@property (weak, nonatomic) IBOutlet UILabel *stateLabel;
@property (weak, nonatomic) IBOutlet UILabel *phoneLabel;
@property (weak, nonatomic) IBOutlet UILabel *pinLabel;
@property (weak, nonatomic) IBOutlet UILabel *disableLabel;
@property (weak, nonatomic) IBOutlet UILabel *mobileLabel;

@property (weak, nonatomic) IBOutlet UIImageView *phoneNoImage;
@property (weak, nonatomic) IBOutlet UIImageView *cityImage;
@property (weak, nonatomic) IBOutlet UIImageView *lastNameImage;
@property (weak, nonatomic) IBOutlet UIImageView *aadhaarImage;
@property (weak, nonatomic) IBOutlet UIImageView *addressLine2Image;
@property (weak, nonatomic) IBOutlet UIImageView *districtImage;
@property (weak, nonatomic) IBOutlet UIImageView *pinImage;

@property (weak, nonatomic) IBOutlet UIImageView *firstNameImage;
@property (nonatomic,readwrite) BOOL isFromDashboard;
@property (weak, nonatomic) IBOutlet UIButton *aadhaarButton;


//- (IBAction)backButtonAction:(id)sender;
- (IBAction)selectDisabilityTypeButtonAction:(id)sender;
- (IBAction)disabilityYesButtonAction:(id)sender;
- (IBAction)disabilityNoButtonAction:(id)sender;
- (IBAction)selectStateButtonAction:(id)sender;
- (IBAction)genderButtonAction:(id)sender;
- (IBAction)selectBloodGroupButtonAction:(id)sender;
- (IBAction)dateOfBirthButtonAction:(id)sender;
- (IBAction)aadhaarButtonAction:(id)sender;


@end
