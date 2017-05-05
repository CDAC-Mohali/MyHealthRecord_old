//
//  Constants.h
//  mSwasthya-VaccinationAlertsApp
//
//  Created by CDAC HIED on 20/03/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//


#import "AppDelegate.h"
#import "Reachability.h"
#import "AFNetworking.h"
#import "UIImageView+AFNetworking.h"
#import "NSData+Base64.h"
#import "BarButton_Block.h"
#import "LoginController.h"
#import "AddAllergyViewController.h"
#import "AllergyController/AllergyDetailViewController.h"
#import "UserProfileViewController.h"
#import "MedicalContactsViewController.h"
#import "MedicalContactDetailViewController.h"
#import "SharingViewController.h"


//Offline Constants

#define offlineName @"offlineUserName"
#define offlineEmail @"offlineEmailAddress"
#define offlineGender @"offlineGender"
#define offlineUserReg @"offlineUserRegistration"

#define firstTimeLogin @"firstTimeUserLogin"
#define isTouchIDConfigured @"isTouchIDConfigured"

#define lastHealthDataSyncTime @"LastHealthDataSyncTime"

#define kAppDelegate (AppDelegate*)[[UIApplication sharedApplication]delegate]
#define kAppTitle @"MyHealthRecord"
#define kOKButtonText @"OK"
#define kCancelButtonText @"Cancel"
#define USERID @"userID"
#define SourceId @"3"
#define USERNAME @"username"
#define USEREMAILID @"userEmailID"
#define USERPROFILE @"userProfileData"
#define USERIMAGE @"userProfileImage"
#define NotificationCount @"notificationCount"
#define NotificationON @"notificationOnOff"
#define FCMTokenRegistered @"isFCMTokenRegistered"

#define iPhone4 @"iPhone4"
#define iPhone5 @"iPhone5"
#define iPhone6 @"iPhone6"
#define iPhone6Plus @"iPhone6Plus"
#define iPad @"iPad"
#define iPhone @"iPhone"

#define reloadTableViewData @"reload_tableview_data"
#define reloadChildListData @"reload_childlist_data"


