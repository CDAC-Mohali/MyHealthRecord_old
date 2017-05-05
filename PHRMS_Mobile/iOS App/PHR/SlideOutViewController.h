//
//  SlideOutViewController.h
//  Medibook
//
//  Created by Gagandeep Singh on 12/10/14.
//  Copyright (c) 2014. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Constants.h"

@interface SlideOutViewController : UIViewController

@property (strong, nonatomic) NSMutableArray *sectionsArray;

@property (strong, nonatomic) NSMutableArray *screensArray;
@property (strong, nonatomic) NSMutableArray *dashboardScreensArray;
@property (strong, nonatomic) NSMutableArray *settingScreensArray;
@property (strong, nonatomic) NSMutableArray *dailyWorkoutScreensArray;
@property (strong, nonatomic) NSMutableArray *userProfileScreensArray;

@property (strong, nonatomic) NSMutableArray *screenImagesArray;
@property (strong, nonatomic) NSMutableArray *dashboardScreenImagesArray;
@property (strong, nonatomic) NSMutableArray *settingScreenImagesArray;
@property (strong, nonatomic) NSMutableArray *dailyWorkoutScreenImagesArray;
@property (strong, nonatomic) NSMutableArray *userProfileScreenImagesArray;


@property (strong, nonatomic) IBOutlet UITableView *screensTableView;
@property (weak, nonatomic) IBOutlet UIImageView *userImageView;
@property (strong, nonatomic) IBOutlet UILabel *userName;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;
@property (nonatomic,strong) UIAlertView *logoutAlertView;


@end
