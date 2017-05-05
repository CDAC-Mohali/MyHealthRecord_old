//
//  DashboardViewController.m
//  PHR
//
//  Created by CDAC HIED on 16/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "DashboardViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"
#import "DashboardTableViewCell.h"
#import "BloodPresureViewController.h"
#import "DiabetesViewController.h"
#import "ActivityViewController.h"
#import "WeightViewController.h"
#import "UserProfileViewController.h"
#import "DayAxisValueFormatter.h"
#import "PHR-Bridging-Header.h"
#import "GSHealthKitManager.h"
#import "HealthDataTableViewCell.h"
#import <EventKit/EventKit.h>

@import HealthKitUI;
@import HealthKit;

@import Charts;

@interface DashboardViewController ()<UITableViewDataSource,UITableViewDelegate, ChartViewDelegate, IChartAxisValueFormatter>{
    SWRevealViewController *revealController;
    NSMutableArray *sectionsArray;
    NSMutableArray *bloodPressureArray;
    NSMutableArray *bloodGlucoseArray;
    NSMutableArray *activityArray;
    NSMutableArray* weightArray;
//    NSMutableArray *dashboardArray;
    BarChartData* bChartData;
    
    NSMutableArray<NSString *> *barChartDateArray;
    NSMutableArray<NSString *> *lineChartDateArray;
    NSArray<NSString *> *activityNameArray;
    NSMutableArray<NSString *> *bmilineChartDateArray;
    
    double run;
    int runTimeDuration;
    double steps;
    int stepsTimeDuration;
    int flights;
    
    CGFloat screenWidth;
    CGFloat screenHeight;
}
@property (nonatomic, strong) CombinedChartView *barChartView;
@property (nonatomic, strong) CombinedChartView *lineChartView;
@property (nonatomic, strong) CombinedChartView *bmiChartView;
@property (nonatomic, strong) PieChartView *pieChartView;
//@property (weak, nonatomic) IBOutlet UILabel *fitnessTitleLabel;
//@property (weak, nonatomic) IBOutlet UILabel *walkingLabel;
//@property (weak, nonatomic) IBOutlet UILabel *walkingDistanceLabel;
//@property (weak, nonatomic) IBOutlet UILabel *runningLabel;
//@property (weak, nonatomic) IBOutlet UILabel *runningDistanceLabel;
//@property (weak, nonatomic) IBOutlet UILabel *stepsLabel;
//@property (weak, nonatomic) IBOutlet UILabel *stepsCountLabel;
//
@property (weak, nonatomic) IBOutlet UILabel *usernameLabel;
//@property (weak, nonatomic) IBOutlet UIButton *syncButton;

@property (strong, nonatomic) IBOutlet UIView *backgroundView;
@property (weak, nonatomic) IBOutlet UITableView *dashboardTableView;
@property (weak, nonatomic) IBOutlet UIButton *profilePhotoButton;
@property(nonatomic, strong) HKActivitySummary *activitySummary;
@property(nonatomic, strong) HKHealthStore *healthStore;

- (IBAction)profilePhotoButtonAction:(id)sender;
//- (IBAction)syncButtonAction:(id)sender;

-(UITableViewCell*)BPChartCreation:(UITableView*) tableView;
-(UITableViewCell*)BGChartCreation:(UITableView*) tableView;
-(UITableViewCell*)activityChartCreation:(UITableView*) tableView;
-(UITableViewCell*)bmiChartCreation:(UITableView*) tableView;

@property (nonatomic, retain) NSArray *chartValues;

@end

@import Firebase;
@import FirebaseInstanceID;
@import FirebaseMessaging;

@implementation DashboardViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    run = 0.0;
    flights = 0;
    steps = 0.0;
    runTimeDuration = 0;
    stepsTimeDuration = 0;
    
    lineChartDateArray = [NSMutableArray new];
    barChartDateArray = [NSMutableArray new];
    bmilineChartDateArray = [NSMutableArray new];
    
    _dashboardTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    
    sectionsArray = [[NSMutableArray alloc]init];//WithObjects:@"Activities",@"Blood Pressure",@"Blood Glucose",@"BMI", nil];
    
//    if ([kAppDelegate dashboardCount]==0) {
//        [self.dashboardTableView setContentInset:UIEdgeInsetsMake(0,0,0,0)];
//        [kAppDelegate setDashboardCount:1];
//    }
    
    activityNameArray = @[
                          @"Walking + Steps", @"Running", @"Cycling", @"Swimming"
                          ];

    
    UIDeviceOrientation Orientation = [[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    
    UIButton* sharingButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [sharingButton addTarget:self action:@selector(sharingButtonAction) forControlEvents:UIControlEventTouchUpInside];
    [sharingButton setImage:[UIImage imageNamed:@"sharing"] forState:UIControlStateNormal];
    [self.view addSubview:sharingButton];
    [self.view bringSubviewToFront:sharingButton];
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
//        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
//        [dateFormatter setDateFormat:@"dd MMM, yyyy"];
        
//        NSString* todayDate = [dateFormatter stringFromDate:[NSDate date]];
        
//        self.fitnessTitleLabel.text = [NSString stringWithFormat:@"Health App Data(%@)",todayDate];
        
        screenHeight = 350;
        screenWidth = screenRect.size.width-30;
        
////        activityNameArray = @[
//                              @"Walking + Steps", @"Running", @"Cycling", @"Swimming", @"HA- Running", @"HA- Flights", @"HA- Steps"
//                              ];
        [sharingButton setFrame:CGRectMake(self.view.frame.size.width-85, self.view.frame.size.height-85, 45, 45)];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:24]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"MyHealthRecord"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
//        [self.profilePhotoButton.layer setCornerRadius:35];
        
        self.usernameLabel.font = [UIFont systemFontOfSize:20.0f];
        
//        self.fitnessTitleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
//        self.walkingLabel.font = [UIFont systemFontOfSize:12.0f weight:-1];
//        self.walkingDistanceLabel.font = [UIFont systemFontOfSize:10.0f weight:-1];
//        self.runningLabel.font = [UIFont systemFontOfSize:12.0f weight:-1];
//        self.runningDistanceLabel.font = [UIFont systemFontOfSize:10.0f weight:-1];
//        self.stepsLabel.font = [UIFont systemFontOfSize:12.0f weight:-1];
//        self.stepsCountLabel.font = [UIFont systemFontOfSize:10.0f weight:-1];
        [self.profilePhotoButton.layer setCornerRadius:35];
        
        [self GetHealthAppData];
    }
    else{
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            screenHeight = 430;
        }
        else{
            screenHeight = 450;
        }
        screenWidth = screenRect.size.width-100;
        
//        activityNameArray = @[
//                              @"Walking + Steps", @"Running", @"Cycling", @"Swimming"
//                              ];
        
        [sharingButton setFrame:CGRectMake(self.view.frame.size.width-140, self.view.frame.size.height-140, 60, 60)];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:34 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"MyHealthRecord"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 40)];
        titleLabel.attributedText = attributedText;
        [self.profilePhotoButton.layer setCornerRadius:45];
        
        self.navigationItem.titleView=titleLabel;
    }
    
    [self.profilePhotoButton.layer setMasksToBounds:YES];
    
    //Set Left Bar Button Item
    [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
        [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
        [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
        self.navigationItem.leftBarButtonItem=barItem;
    }];
    
    [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
        [barButton addTarget:self action:@selector(logoutButtonAction:) forControlEvents:UIControlEventTouchUpInside];
        [barButton setFrame:CGRectMake(barButton.frame.origin.x, barButton.frame.origin.y, 25, 25)];
        [barButton setImage:[UIImage imageNamed:@"logoutUser"] forState:UIControlStateNormal];
        self.navigationItem.rightBarButtonItem=barItem;
    }];
    
//    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
//        
//        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
//        
//        dispatch_async(dispatch_get_main_queue(), ^{
//            
//            if (imageData) {
//                [_profilePhotoButton setImage:[UIImage imageWithData:imageData] forState:UIControlStateNormal];
//            }
//            else{
//                [_profilePhotoButton setImage:[UIImage imageNamed:@"userImage"] forState:UIControlStateNormal];
//            }
//        });
//    });
    
//    [self.usernameLabel setText:[NSString stringWithFormat:@"Welcome \n%@",[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]];
    
    bloodPressureArray = [NSMutableArray new];
    bloodGlucoseArray = [NSMutableArray new];
    activityArray = [NSMutableArray new];
    weightArray = [NSMutableArray new];
    
//    dashboardArray = [NSMutableArray new];
    
//    [self.backgroundView setBackgroundColor:[UIColor colorWithHexRed:216.0/255.0f weight:-1 green:238.0/255.0f weight:-1 blue:207.0/255.0f weight:-1 alpha:1]];
    
//    [self GetUserProfileImageAPICall];
    
    UIRefreshControl *refreshControl = [[UIRefreshControl alloc] init];
    [refreshControl addTarget:self action:@selector(refresh:) forControlEvents:UIControlEventValueChanged];
    [self.dashboardTableView addSubview:refreshControl];
    refreshControl.backgroundColor = [UIColor lightGrayColor];
    
//    NSDateFormatter *dateTimeformatter = [[NSDateFormatter alloc]init];
//    [dateTimeformatter setDateFormat:@"MM/dd/yyyy hh:mm a"];
//    
//    NSString* dateString = [dateTimeformatter stringFromDate:[NSDate date]];
//
//    NSArray* foo = [dateString componentsSeparatedByString: @" "];
//    NSString* day = [foo objectAtIndex: 0];
//
//    NSString* str = [NSString stringWithFormat:@"%@ %@",day,[notificationArray objectAtIndex:0]];
//
//    NSDate *alarmTime = [dateTimeformatter dateFromString:@"02/06/2017 02:30 PM"];
////    alarmTime = [alarmTime dateByAddingTimeInterval:60*60*24*1+1];
//    
//    UILocalNotification* alarm = [[UILocalNotification alloc] init];
//    alarm.fireDate = alarmTime;
//    alarm.alertBody = [NSString stringWithFormat:@"%@: Today's health tip %@",kAppTitle,[notificationArray objectAtIndex:0]];
//    alarm.soundName = UILocalNotificationDefaultSoundName;
//    alarm.timeZone = [NSTimeZone localTimeZone];
//    
//    NSDictionary *infoDict = [NSDictionary dictionaryWithObject:[notificationArray objectAtIndex:0] forKey:@"Time"];
//    alarm.userInfo = infoDict;
//    
//    NSLog(@"alarm set for date %@ and time %@",[alarmTime dateByAddingTimeInterval:60*60*24*1+1] ,[notificationArray objectAtIndex:0]);
//    [[UIApplication sharedApplication] scheduleLocalNotification:alarm];
}

//-(void)viewWillDisappear:(BOOL)animated{
//    [_lineChartView removeFromSuperview];
//    [_pieChartView removeFromSuperview];
//    [_barChartView removeFromSuperview];
//    [_bmiChartView removeFromSuperview];
//
//    _lineChartView = nil;
//    _pieChartView = nil;
//    _barChartView = nil;
//    _bmiChartView = nil;
//}

-(void)viewWillAppear:(BOOL)animated{
    
    [self getDashboardDataAPI];
//    [self getWeightDataAPI];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [self getUserProfileImageAPI];
    });
    
//    if ([kAppDelegate isUserProfileUpdated]==1) {
//        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
//            
////            [kAppDelegate setIsUserProfileUpdated:0];
//            
//            NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
//            
//            dispatch_async(dispatch_get_main_queue(), ^{
//                
//                if (imageData) {
//                    [_profilePhotoButton setImage:[UIImage imageWithData:imageData] forState:UIControlStateNormal];
//                }
//                else{
//                    [_profilePhotoButton setImage:[UIImage imageNamed:@"userImage"] forState:UIControlStateNormal];
//                }
//            });
//        });
//    }
    
    [self.usernameLabel setText:[NSString stringWithFormat:@"Welcome \n%@",[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]];
    
    self.revealViewController.panGestureRecognizer.enabled = YES;
}

#pragma mark Sharing Button Action 
-(void)sharingButtonAction{
    SharingViewController* objSharingViewController = [self.storyboard instantiateViewControllerWithIdentifier:@"SharingStoryboardIdentity"];
    objSharingViewController.isFromDashboard = YES;
    [self.navigationController pushViewController:objSharingViewController animated:YES];
}

#pragma mark Get Health App Data 
-(void)GetHealthAppData{
    
    NSDate* currentDate;
    if ([[NSUserDefaults standardUserDefaults] objectForKey:lastHealthDataSyncTime]==nil) {
        currentDate = [NSDate date];
    }
    else{
        currentDate = (NSDate*)[[NSUserDefaults standardUserDefaults] objectForKey:lastHealthDataSyncTime];
    }
    
    NSDictionary* dicRunning = [[GSHealthKitManager sharedManager] readRunningDistance:currentDate];
    run = [[dicRunning valueForKey:@"Distance"] doubleValue];
    runTimeDuration = [[dicRunning valueForKey:@"Time"] intValue];
    
    NSDictionary* dicSteps = [[GSHealthKitManager sharedManager] readStepsCount:currentDate];
    steps = [[dicSteps valueForKey:@"Distance"] intValue];
    stepsTimeDuration = [[dicSteps valueForKey:@"Time"] intValue];
    
    flights = [[GSHealthKitManager sharedManager] readFlightsClimbed];

    steps = steps/1320;
    
//    self.walkingDistanceLabel.text = [NSString stringWithFormat:@"%.01f KM",run];
//    self.runningDistanceLabel.text = [NSString stringWithFormat:@"%d Floor's",flights];
//    self.stepsCountLabel.text = [NSString stringWithFormat:@"%.01f KM",steps];
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{

    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    CGRect screenRect = [[UIScreen mainScreen] bounds];
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        screenHeight = 350;
        screenWidth = screenRect.size.width-30;
    }
    else{
        if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
            screenHeight = 430;
        }
        else{
            screenHeight = 450;
        }
        screenWidth = screenRect.size.width-100;
    }
    
    [UIView animateWithDuration:0.75 animations:^{
        [_barChartView setFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
        [_pieChartView setFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
        [_lineChartView setFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
        [_bmiChartView setFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
    }];
}

#pragma mark Pull To Refresh Controller 
- (void)refresh:(UIRefreshControl *)refreshControl {
    
    [refreshControl endRefreshing];
    
//    [sectionsArray removeAllObjects];
    [bloodGlucoseArray removeAllObjects];
    [bloodPressureArray removeAllObjects];
    [activityArray removeAllObjects];
    [weightArray removeAllObjects];
    
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MMM d, h:mm a"];
    NSString *title = [NSString stringWithFormat:@"Last updated: %@", [formatter stringFromDate:[NSDate date]]];
    NSDictionary *attrsDictionary = [NSDictionary dictionaryWithObject:[UIColor whiteColor] forKey:NSForegroundColorAttributeName];
    
    NSAttributedString *attributedTitle = [[NSAttributedString alloc] initWithString:title attributes:attrsDictionary];
    refreshControl.attributedTitle = attributedTitle;
    
    [self getDashboardDataAPI];
    if (![[kAppDelegate checkDeviceType]isEqualToString:iPad]) {
        [self GetHealthAppData];
    }
//    [self getWeightDataAPI];
}

-(void)getUserProfileImageAPI{
    if ([kAppDelegate hasInternetConnection]) {
//        [kAppDelegate showLoadingIndicator:@"Getting image..."];//Show loading indicator
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
//            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
//                dashboardArray = [responseObject valueForKey:@"response"];
                
                NSString* userImage = @"";
                userImage = [userImage stringByReplacingOccurrencesOfString:@"\\"
                                                                 withString:@"/"];
                
                [[NSUserDefaults standardUserDefaults] setValue:nil forKey:USERIMAGE];
                [[NSUserDefaults standardUserDefaults] setValue:userImage forKey:USERIMAGE];
                
                NSString* firstName = [[responseObject valueForKey:@"response"] valueForKey:@"FirstName"];
                NSString* lastName = [[responseObject valueForKey:@"response"] valueForKey:@"LastName"];
                NSString* userName = [NSString stringWithFormat:@"%@ %@",firstName,lastName];
                [[NSUserDefaults standardUserDefaults] setValue:nil forKey:USERNAME];
                
                [[NSUserDefaults standardUserDefaults] setValue:userName forKey:USERNAME];
                
                [self.usernameLabel setText:[NSString stringWithFormat:@"Welcome \n%@",[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]];
                dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
                
                    NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
                    
                    dispatch_async(dispatch_get_main_queue(), ^{
                
                        if (imageData) {
                            [_profilePhotoButton setImage:[UIImage imageWithData:imageData] forState:UIControlStateNormal];
                        }
                        else{
                            [_profilePhotoButton setImage:[UIImage imageNamed:@"userImage"] forState:UIControlStateNormal];
                        }
                    });
                });
            }
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
//            [kAppDelegate hideLoadingIndicator];
//            [kAppDelegate showAlertView:@"failed getting profile photo"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

-(void)getWeightDataAPI{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Getting..."];//Show loading indicator
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                [weightArray removeAllObjects];
                for (int i=0; i< [[responseObject valueForKey:@"response"] count];i++) {
                    int sourceID = [[[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"SourceId"]intValue];
                    if (sourceID!=2 && sourceID!=5) {
                        [weightArray addObject:[[responseObject valueForKey:@"response"] objectAtIndex:i]];
                    }
                }
//                weightArray = [responseObject valueForKey:@"response"];
            }
            
            
            [_dashboardTableView setDataSource:self];
            [_dashboardTableView setDelegate:self];
            [_dashboardTableView reloadData];
            
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

-(void)getDashboardDataAPI{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Getting data..."];//Show loading indicator
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
//                dashboardArray = [responseObject valueForKey:@"response"];
                
                [bloodGlucoseArray removeAllObjects];
                [bloodPressureArray removeAllObjects];
                [activityArray removeAllObjects];
                for (int i=0; i< [[[responseObject valueForKey:@"response"] valueForKey:@"oActivityViewModel"] count];i++) {
                    int activitySourceID = [[[[[responseObject valueForKey:@"response"] valueForKey:@"oActivityViewModel"] objectAtIndex:i] valueForKey:@"SourceId"]intValue];
                    if (activitySourceID!=2 && activitySourceID!=5) {
                        [activityArray addObject:[[[responseObject valueForKey:@"response"] valueForKey:@"oActivityViewModel"] objectAtIndex:i]];
                    }
                }
                for (int i=0; i< [[[responseObject valueForKey:@"response"] valueForKey:@"oBloodPressureAndPulseViewModel"] count];i++) {
                    int bpSourceID = [[[[[responseObject valueForKey:@"response"] valueForKey:@"oBloodPressureAndPulseViewModel"] objectAtIndex:i] valueForKey:@"SourceId"]intValue];
                    
                    if (bpSourceID!=2 && bpSourceID!=5) {
                        [bloodPressureArray addObject:[[[responseObject valueForKey:@"response"] valueForKey:@"oBloodPressureAndPulseViewModel"] objectAtIndex:i]];
                    }
                }
                for (int i=0; i< [[[responseObject valueForKey:@"response"] valueForKey:@"oBloodGluscoseViewModel"] count];i++) {
                    int bgSourceID = [[[[[responseObject valueForKey:@"response"] valueForKey:@"oBloodGluscoseViewModel"] objectAtIndex:i] valueForKey:@"SourceId"]intValue];
                
                    if (bgSourceID!=2 && bgSourceID!=5) {
                        [bloodGlucoseArray addObject:[[[responseObject valueForKey:@"response"] valueForKey:@"oBloodGluscoseViewModel"] objectAtIndex:i]];
                    }
                }
                
//                bloodPressureArray = [[responseObject valueForKey:@"response"] valueForKey:@"oBloodPressureAndPulseViewModel"];
//                bloodGlucoseArray = [[responseObject valueForKey:@"response"] valueForKey:@"oBloodGluscoseViewModel"];
//                activityArray = [[responseObject valueForKey:@"response"] valueForKey:@"oActivityViewModel"];
                
            }
            else{
                [kAppDelegate showAlertView:@"No data!"];
            }
            
            [self getWeightDataAPI];
            
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

-(void)GetUserProfileImageAPICall{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Getting..."];//Show loading indicator
        
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==0) {
                [_profilePhotoButton setImage:[UIImage imageNamed:@"userImage"] forState:UIControlStateNormal];
                return;
            }
            NSString* userImage = [[responseObject valueForKey:@"response"] valueForKey:@"FileName"];
//            userImage = @"16ec791b-8f1d-44c6-b119-8f93bfd681a2.jpeg";
            
            NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [NSString stringWithFormat:@"http://10.228.12.146:8082%@",userImage]]];
            
            [_profilePhotoButton setImage:[UIImage imageWithData:imageData] forState:UIControlStateNormal];
            
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"Login failed"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

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

#pragma mark Logout Button Action 
-(void)logoutButtonAction:(id)sender{
    [self showAlertView];
}

-(void)showAlertView{
    self.logoutAlertView=[[UIAlertView alloc]initWithTitle:kAppTitle message:@"Are you sure you want to logout?" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"OK", nil];
    [self.logoutAlertView setTag:2000];
    [self.logoutAlertView show];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    if (buttonIndex==1) {
        if (alertView.tag == 2000) {
            if ([[NSUserDefaults standardUserDefaults] boolForKey:FCMTokenRegistered]) {
                [self unregisterFCMTokenForNotification];
            }
            
            [[NSUserDefaults standardUserDefaults] setObject:nil forKey:USERNAME];
            [[NSUserDefaults standardUserDefaults] setObject:nil forKey:USEREMAILID];
            [[NSUserDefaults standardUserDefaults] setObject:nil forKey:USERID];
            [[NSUserDefaults standardUserDefaults] synchronize];
            [kAppDelegate setDashboardCount:0];
            
            UIStoryboard *storyboard;
            if ([[kAppDelegate checkDeviceType] isEqualToString:iPad]) {
                storyboard = [UIStoryboard storyboardWithName:@"Main-iPad" bundle: nil];
            }
            else{
                storyboard = [UIStoryboard storyboardWithName:@"Main" bundle: nil];
            }
            LoginController* viewController = [storyboard instantiateViewControllerWithIdentifier:@"LoginController"];
            
            [[kAppDelegate window]setRootViewController:viewController];
            
        }
        else{
            [self SyncHealthAppData];
        }
    }
}

#pragma mark Unregister FCM Token For Notifications 
-(void)unregisterFCMTokenForNotification{
    if ([kAppDelegate hasInternetConnection]) {
        
        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"userID"];
        
        NSString* strToken = [[FIRInstanceID instanceID] token];
        [dicParams setObject:strToken forKey:@"tokenID"];
        [dicParams setObject:@"3" forKey:@"SourceID"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        
        [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
        //            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
        
        [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
        
        manager.requestSerializer = requestSerializer;
        [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"]intValue] == 1) {
                [[NSUserDefaults standardUserDefaults] setBool:NO forKey:FCMTokenRegistered];
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

-(void)revealMedicalContactsView:(id)sender{
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:kAppTitle message:[NSString stringWithFormat:@"Adding and sharing of medical contacts"] preferredStyle:UIAlertControllerStyleActionSheet];
    
    UIAlertAction* medicalContactsButton = [UIAlertAction actionWithTitle:@"Medical Contacts" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        
        MedicalContactsViewController* objMedicalContactController = [self.storyboard instantiateViewControllerWithIdentifier:@"MedicalContactStoryboardIdentity"];
        
        [self.navigationController pushViewController:objMedicalContactController animated:YES];
    }];
    
    UIAlertAction* sharingButton = [UIAlertAction actionWithTitle:@"Sharing" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        SharingViewController* objSharingViewController = [self.storyboard instantiateViewControllerWithIdentifier:@"SharingStoryboardIdentity"];
        
        [self.navigationController pushViewController:objSharingViewController animated:YES];
    }];
    
    alertController.popoverPresentationController.barButtonItem = nil;
    alertController.popoverPresentationController.sourceView = self.view;
    alertController.popoverPresentationController.sourceRect = CGRectMake(self.view.bounds.size.width-100, 70, 1.0, 1.0);
    
    [alertController addAction:medicalContactsButton];
    [alertController addAction:sharingButton];
    [alertController addAction:[UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
        // Called when user taps outside
        [self dismissViewControllerAnimated:YES completion:nil];
    }]];
    
    [alertController setModalPresentationStyle:UIModalPresentationPopover];
    
    [self presentViewController:alertController animated:YES completion:nil];
}

#pragma mark - UITableView Datasource
- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    
    int counter = 0;
//    if ([activityArray count]>0 || (run > 0.0 || flights > 0 || steps > 0.0)) {
    if (run>=0.01 || steps>=0.01 || flights>0) {
        counter += 1;
        [sectionsArray addObject:@"Apple Health App Data"];
    }
    if ([activityArray count]>0) {
        counter += 1;
        [sectionsArray addObject:@"Activities"];
    }
    if ([bloodPressureArray count]>0) {
        counter += 1;
        [sectionsArray addObject:@"Blood Pressure"];
    }
    if ([bloodGlucoseArray count]>0) {
        counter += 1;
        [sectionsArray addObject:@"Blood Glucose"];
    }
    if ([weightArray count]>0) {
        counter += 1;
        [sectionsArray addObject:@"BMI"];
    }
    
    if (counter == 0) {
        UILabel *noDataLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, tableView.bounds.size.width, tableView.bounds.size.height)];
        noDataLabel.text             = @"No data available";
        noDataLabel.textColor        = [UIColor blackColor];
        noDataLabel.textAlignment    = NSTextAlignmentCenter;
        //yourTableView.backgroundView = noDataLabel;
        //yourTableView.separatorStyle = UITableViewCellSeparatorStyleNone;
        tableView.backgroundView = noDataLabel;
        tableView.separatorStyle = UITableViewCellSeparatorStyleNone;
    }
    
    return counter;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {

    if (section==0) {
        return 1;
    }
    else if (section==1) {
//        if ([activityArray count]==0 && (run == 0.0 && flights == 0 && steps == 0.0)) {
        if ([activityArray count]==0) {
            if ([bloodPressureArray count]==0) {
                if ([bloodGlucoseArray count]==0) {
                    if ([weightArray count]==0) {
                        return 0;
                    }
                }
            }
        }
        return 1;
    }
    else if (section==2) {
        if ([bloodPressureArray count]==0) {
            if ([bloodGlucoseArray count]==0) {
                if ([weightArray count]==0) {
                    return 0;
                }
            }
        }
        return 1;
    }
    else if (section==3) {
        if ([bloodGlucoseArray count]==0) {
            if ([weightArray count]==0) {
                return 0;
            }
        }
        return 1;
    }
    else{
        if ([weightArray count]==0) {
            return 0;
        }else{
            return 1;
        }
    }
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    
    NSLog(@"index path %ld",(long)indexPath.section);
    UITableViewCell *cell;
    
    if ([[sectionsArray objectAtIndex:indexPath.section] isEqualToString:@"Apple Health App Data"]) {
        
        static NSString *CellIdentifier = @"healthIdentifier";
        HealthDataTableViewCell *healthCell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
        
        if (healthCell == nil) {
            NSArray* nib = [[NSBundle mainBundle]loadNibNamed:@"HealthDataTableViewCell" owner:self options:nil];
            healthCell = [nib objectAtIndex:0];
        }
        
//        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
//        [dateFormatter setDateFormat:@"dd MMM, yyyy"];
//        NSString* todayDate = [dateFormatter stringFromDate:[NSDate date]];
//        
//        healthCell.fitnessTitleLabel.text = [NSString stringWithFormat:@"Health App Data(%@)",todayDate];
//        
//        healthCell.fitnessTitleLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        healthCell.walkingLabel.font = [UIFont systemFontOfSize:12.0f weight:-1];
        healthCell.walkingDistanceLabel.font = [UIFont systemFontOfSize:10.0f weight:-1];
        healthCell.runningLabel.font = [UIFont systemFontOfSize:12.0f weight:-1];
        healthCell.runningDistanceLabel.font = [UIFont systemFontOfSize:10.0f weight:-1];
        healthCell.stepsLabel.font = [UIFont systemFontOfSize:12.0f weight:-1];
        healthCell.stepsCountLabel.font = [UIFont systemFontOfSize:10.0f weight:-1];
        
        [healthCell.syncButton addTarget:self action:@selector(syncButtonAction) forControlEvents:UIControlEventTouchUpInside];
        
        healthCell.selectionStyle = UITableViewCellSelectionStyleNone;
        
        healthCell.walkingDistanceLabel.text = [NSString stringWithFormat:@"%.2f KM",run];
        healthCell.runningDistanceLabel.text = [NSString stringWithFormat:@"%d Floor's",flights];
        healthCell.stepsCountLabel.text = [NSString stringWithFormat:@"%.2f KM",steps];
        
        return healthCell;
    }
    else if ([[sectionsArray objectAtIndex:indexPath.section] isEqualToString:@"Activities"]) {
        cell = [self activityChartCreation:tableView];
    }
    else if ([[sectionsArray objectAtIndex:indexPath.section] isEqualToString:@"Blood Pressure"]){
        cell = [self BPChartCreation:tableView];
    }
    else if ([[sectionsArray objectAtIndex:indexPath.section] isEqualToString:@"Blood Glucose"]){
        cell = [self BGChartCreation:tableView];
    }
    else if ([[sectionsArray objectAtIndex:indexPath.section] isEqualToString:@"BMI"]){
        cell = [self bmiChartCreation:tableView];
    }
    
    return cell;
    
   /* if (indexPath.section==1) {
        
        UITableViewCell *cell;
        if ([bloodPressureArray count]==0 || isBPGraphPlotted) {
            if ([bloodGlucoseArray count]==0 || isBGGraphPlotted) {
                if ([weightArray count]) {
                    cell = [self bmiChartCreation:tableView];
                }
            }
            else{
                cell = [self BGChartCreation:tableView];
            }
        }
        else{
            cell = [self BPChartCreation:tableView];
        }
        
        return cell;
    }
    else if (indexPath.section==2){
        
        UITableViewCell *cell;
        
        if ([bloodGlucoseArray count]==0 || isBGGraphPlotted) {
            if ([weightArray count]) {
                cell = [self bmiChartCreation:tableView];
            }
        }
        else{
            cell = [self BGChartCreation:tableView];
        }
        
        return cell;
    }
    else if (indexPath.section==3) {
        
        UITableViewCell *bmiCell;
        
        if ([weightArray count]) {
            
            bmiCell = [self bmiChartCreation:tableView];
            
            return bmiCell;
        }
        
        return bmiCell;
    }
    else {
        UITableViewCell *cellPie;
        
        if ([activityArray count]==0 && (run == 0.0 && flights == 0 && steps == 0)) {
            if ([bloodPressureArray count]==0) {
                if ([bloodGlucoseArray count]==0) {
                    if ([weightArray count]==0) {
                        NSDictionary *attrs = @{
                        NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1]};
            
                        NSMutableAttributedString *attributedText =
                        [[NSMutableAttributedString alloc] initWithString:@"No data!"attributes:attrs];
                        
                        cellPie.textLabel.textAlignment = NSTextAlignmentCenter;
                        cellPie.textLabel.attributedText = attributedText;
                    }
                    else{
                        cellPie = [self bmiChartCreation:tableView];
                    }
                }
                else{
                    cellPie = [self BGChartCreation:tableView];
                }
            }
            else{
                cellPie = [self BPChartCreation:tableView];
            }
        }
        else{
            cellPie = [self activityChartCreation:tableView];
        }
        
        return cellPie;
    }*/
}

#pragma mark Rows Arrow Selector Methods 
-(void)BPRowArrow{
    BloodPresureViewController* objBPController = [self.storyboard instantiateViewControllerWithIdentifier:@"BPStoryboardIdentity"];
    objBPController.isFromDashboard = YES;
    objBPController.bloodPressureArray = bloodPressureArray;
    [self.navigationController pushViewController:objBPController animated:YES];
}

-(void)BGRowArrow{
    DiabetesViewController* objDiabetesController = [self.storyboard instantiateViewControllerWithIdentifier:@"DiabetesStoryboardIdentity"];
    objDiabetesController.isFromDashboard = YES;
    objDiabetesController.bloodGlucoseArray = bloodGlucoseArray;
    [self.navigationController pushViewController:objDiabetesController animated:YES];
}

-(void)BMIRowArrow{
    WeightViewController* objBMIController = [self.storyboard instantiateViewControllerWithIdentifier:@"WeightStoryboardIdentity"];
    objBMIController.isFromDashboard = YES;
    objBMIController.weightArray = weightArray;
    [self.navigationController pushViewController:objBMIController animated:YES];
}

#pragma mark Blood Pressure Chart Creation 
-(UITableViewCell*)BPChartCreation:(UITableView*) tableView {
    static NSString *cellIdentifier = @"dashboardCellIdentifier";
    
//    DashboardTableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:cellIdentifier];
//    
//    if(cell == nil) {
//        cell = [[[NSBundle mainBundle]loadNibNamed:@"DashboardTableViewCell" owner:nil options:nil] firstObject];
//    }
//    static NSString *cellIdentifier = @"newCell";
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if (cell == nil) {
        cell = [[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:cellIdentifier];
    }
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(bloodPressureSingleTap:)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    UIButton* arrowButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [arrowButton setFrame:CGRectMake(screenWidth-10, 0, 30, 30)];
    [arrowButton setImage:[UIImage imageNamed:@"arrow"] forState:UIControlStateNormal];
    [arrowButton addTarget:self action:@selector(bloodPressureSingleTap:) forControlEvents:UIControlEventTouchUpInside];
    [cell addSubview:arrowButton];
    
    [cell addGestureRecognizer:singleFingerTap];
    if (!_barChartView) {
        
//        _barChartView.delegate = self;
        
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5])
            _barChartView = [[CombinedChartView alloc] initWithFrame:CGRectMake(5, 30, screenWidth+30, screenHeight)];
        else
            _barChartView = [[CombinedChartView alloc] initWithFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
        
//        _barChartView.descriptionText = @"";
//        _barChartView.noDataTextDescription = @"You need to provide data for the chart.";
        
//        _barChartView.drawGridBackgroundEnabled = NO;
//        _barChartView.drawBarShadowEnabled = NO;
//        
//        _barChartView.drawOrder = @[
//                                    @(CombinedChartDrawOrderBar),
//                                    @(CombinedChartDrawOrderBubble),
//                                    @(CombinedChartDrawOrderCandle),
//                                    @(CombinedChartDrawOrderLine),
//                                    @(CombinedChartDrawOrderScatter)
//                                    ];
//        
//        ChartLegend *l = _barChartView.legend;
//        l.wordWrapEnabled = YES;
//        l.position = ChartLegendPositionAboveChartLeft;
//        
//        ChartYAxis *rightAxis = _barChartView.rightAxis;
//        rightAxis.drawGridLinesEnabled = NO;
//        rightAxis.axisMinValue = 0.0; // this replaces startAtZero = YES
//        rightAxis.drawLabelsEnabled = NO;
//        
//        NSNumberFormatter *pFormatter = [[NSNumberFormatter alloc] init];
//        pFormatter.numberStyle = NSNumberFormatterNoStyle;
//        pFormatter.maximumFractionDigits = 1;
//        
//        ChartYAxis *leftAxis = _barChartView.leftAxis;
//        leftAxis.drawGridLinesEnabled = NO;
//        leftAxis.axisMinValue = 0.0; // this replaces startAtZero = YES
//        [leftAxis setValueFormatter : [[ChartDefaultValueFormatter alloc] initWithFormatter:pFormatter]];
//        
//        ChartXAxis *xAxis = _barChartView.xAxis;
//        xAxis.labelPosition = XAxisLabelPositionBottom;
        
        _barChartView.delegate = self;
        
        _barChartView.descriptionText = @"";
        
        _barChartView.drawGridBackgroundEnabled = NO;
        _barChartView.drawBarShadowEnabled = NO;
        _barChartView.highlightFullBarEnabled = NO;
        
        _barChartView.drawOrder = @[
                                 @(CombinedChartDrawOrderBar),
                                 @(CombinedChartDrawOrderBubble),
                                 @(CombinedChartDrawOrderCandle),
                                 @(CombinedChartDrawOrderLine),
                                 @(CombinedChartDrawOrderScatter)
                                 ];
        
        ChartLegend *l = _barChartView.legend;
        l.wordWrapEnabled = YES;
        l.position = ChartLegendPositionAboveChartLeft;
//        l.horizontalAlignment = ChartLegendHorizontalAlignmentLeft;
//        l.verticalAlignment = ChartLegendVerticalAlignmentTop;
//        l.orientation = ChartLegendOrientationHorizontal;
//        l.drawInside = NO;
        
        ChartYAxis *rightAxis = _barChartView.rightAxis;
        rightAxis.drawGridLinesEnabled = NO;
        rightAxis.axisMinimum = 0.0; // this replaces startAtZero = YES
        _barChartView.rightAxis.enabled = NO;
        
        ChartYAxis *leftAxis = _barChartView.leftAxis;
        leftAxis.drawGridLinesEnabled = NO;
        leftAxis.axisMinimum = 0.0; // this replaces startAtZero = YES
        
        ChartXAxis *xAxis = _barChartView.xAxis;
        xAxis.labelPosition = XAxisLabelPositionBottom;
        xAxis.axisMinimum = 0.0;
        xAxis.granularity = 1.0;
        xAxis.valueFormatter = self;
        xAxis.centerAxisLabelsEnabled = YES;
        xAxis.labelFont = [UIFont systemFontOfSize:7.0f];
//        [self.view addSubview:_barChartView];
        
//        [self updateChartData];
        
        [cell addSubview:_barChartView];
    }
    [self setBarChartData];
    
    [_barChartView animateWithYAxisDuration:2.0];
    
    return cell;
}

#pragma mark Blood Glucose Chart Creation 
-(UITableViewCell*)BGChartCreation:(UITableView*) tableView {
    static NSString *cellIdentifier = @"dashboardCellIdentifier";
    
//    DashboardTableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:cellIdentifier];
//    
//    if(cell == nil) {
//        cell = [[[NSBundle mainBundle]loadNibNamed:@"DashboardTableViewCell" owner:nil options:nil] firstObject];
//    }
//    static NSString *cellIdentifier = @"newCell";
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if (cell == nil) {
        cell = [[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:cellIdentifier];
    }
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(diabetesSingleTap:)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    UIButton* arrowButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [arrowButton setFrame:CGRectMake(screenWidth-10, 0, 30, 30)];
    [arrowButton setImage:[UIImage imageNamed:@"arrow"] forState:UIControlStateNormal];
    [arrowButton addTarget:self action:@selector(diabetesSingleTap:) forControlEvents:UIControlEventTouchUpInside];
    [cell addSubview:arrowButton];
    
    [cell addGestureRecognizer:singleFingerTap];
    
    if (!_lineChartView) {
        
        _lineChartView.delegate = self;
        
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5])
            _lineChartView = [[CombinedChartView alloc] initWithFrame:CGRectMake(5, 30, screenWidth+15, screenHeight)];
        else
            _lineChartView = [[CombinedChartView alloc] initWithFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
        
        _lineChartView.descriptionText = @"";
        //                _lineChartView.noDataTextDescription = @"You need to provide data for the chart.";
        
        _lineChartView.drawGridBackgroundEnabled = NO;
        _lineChartView.drawBarShadowEnabled = NO;
        _lineChartView.highlightFullBarEnabled = NO;
        
        _lineChartView.drawOrder = @[
                                     @(CombinedChartDrawOrderBar),
                                     @(CombinedChartDrawOrderBubble),
                                     @(CombinedChartDrawOrderCandle),
                                     @(CombinedChartDrawOrderLine),
                                     @(CombinedChartDrawOrderScatter)
                                     ];
        
        ChartLegend *l = _lineChartView.legend;
        l.wordWrapEnabled = YES;
        l.position = ChartLegendPositionAboveChartLeft;
        
        ChartLimitLine *ll1 = [[ChartLimitLine alloc] initWithLimit:150.0 label:@"Upper Limit"];
        ll1.lineWidth = 4.0;
        ll1.lineDashLengths = @[@5.f, @5.f];
        ll1.labelPosition = ChartLimitLabelPositionRightTop;
        ll1.valueFont = [UIFont systemFontOfSize:10.0];
        
        ChartLimitLine *ll2 = [[ChartLimitLine alloc] initWithLimit:80.0 label:@"Lower Limit"];
        ll2.lineWidth = 4.0;
        ll2.lineDashLengths = @[@5.f, @5.f];
        ll2.labelPosition = ChartLimitLabelPositionRightBottom;
        ll2.valueFont = [UIFont systemFontOfSize:10.0];
        
        ChartYAxis *rightAxis = _lineChartView.rightAxis;
        rightAxis.drawGridLinesEnabled = NO;
        rightAxis.axisMinValue = 0.0; // this replaces startAtZero = YES
        rightAxis.drawLabelsEnabled = NO;
        
        NSNumberFormatter *pFormatter = [[NSNumberFormatter alloc] init];
        pFormatter.numberStyle = NSNumberFormatterNoStyle;
        pFormatter.maximumFractionDigits = 1;
//        pFormatter.multiplier = @1.f;
        
        ChartYAxis *leftAxis = _lineChartView.leftAxis;
        leftAxis.drawGridLinesEnabled = NO;
        leftAxis.axisMinValue = 0.0; // this replaces startAtZero = YES
//        leftAxis.labelPosition = YAxisLabelPositionOutsideChart;
        leftAxis.labelFont = [UIFont fontWithName:@"HelveticaNeue" size:10];
//        leftAxis.drawAxisLineEnabled = YES;
//        leftAxis.valueFormatter = [[ChartDefaultValueFormatter alloc] initWithFormatter:pFormatter];
        
        [leftAxis addLimitLine:ll1];
        [leftAxis addLimitLine:ll2];
        
        ChartXAxis *xAxis = _lineChartView.xAxis;
        xAxis.labelPosition = XAxisLabelPositionBottom;
        xAxis.axisMinimum = 0.0;
        xAxis.granularity = 1.0;
        xAxis.valueFormatter = self;
        
        [cell addSubview:_lineChartView];
    }
    [self setChartData];
    
    [_lineChartView animateWithYAxisDuration:2.0];
   
    return cell;
}

#pragma mark Activity Chart Creation 
-(UITableViewCell*)activityChartCreation:(UITableView*) tableView {
    
    static NSString *CellIdentifier = @"newCell";
    UITableViewCell *cellPie = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    
    if (cellPie == nil) {
        cellPie = [[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier];
    }
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(activitySingleTap:)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    UIButton* arrowButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [arrowButton setFrame:CGRectMake(screenWidth-10, 0, 30, 30)];
    [arrowButton setImage:[UIImage imageNamed:@"arrow"] forState:UIControlStateNormal];
    [arrowButton addTarget:self action:@selector(activitySingleTap:) forControlEvents:UIControlEventTouchUpInside];
    [cellPie addSubview:arrowButton];
    
    cellPie.selectionStyle = UITableViewCellSelectionStyleNone;
    if (!_pieChartView) {
        
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5])
            _pieChartView = [[PieChartView alloc] initWithFrame:CGRectMake(20, 30, screenWidth, screenHeight)];
        else
            _pieChartView = [[PieChartView alloc] initWithFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
        
        [self setupPieChartView:_pieChartView];
        
        ChartLegend *l = _pieChartView.legend;
        l.wordWrapEnabled = YES;
        l.position = ChartLegendPositionAboveChartCenter;
        
        _pieChartView.delegate = self;
        
        [cellPie addSubview:_pieChartView];
    }
    int count = (int)[activityNameArray count];
    [self setPieChartDataCount:count range:150];
    
    [_pieChartView animateWithXAxisDuration:2.0 easingOption:ChartEasingOptionEaseOutBack];
    
    return cellPie;
}

#pragma mark BMI Chart Creation 
-(UITableViewCell*)bmiChartCreation:(UITableView*) tableView {
    
    static NSString *identifier = @"cellIdentifier";
//    static NSString *identifier = @"newCell";
    UITableViewCell *bmiCell = [tableView dequeueReusableCellWithIdentifier:identifier];
    
    if(bmiCell == nil) {
        bmiCell = [[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:identifier];
    }
    
    UITapGestureRecognizer *singleFingerTap = [[UITapGestureRecognizer alloc]
                                               initWithTarget:self action:@selector(bmiSingleTap:)];
    
    singleFingerTap.numberOfTapsRequired = 1;
    
    UIButton* arrowButton = [UIButton buttonWithType:UIButtonTypeCustom];
    [arrowButton setFrame:CGRectMake(screenWidth-10, 0, 30, 30)];
    [arrowButton setImage:[UIImage imageNamed:@"arrow"] forState:UIControlStateNormal];
    [arrowButton addTarget:self action:@selector(bmiSingleTap:) forControlEvents:UIControlEventTouchUpInside];
    [bmiCell addSubview:arrowButton];
    
    [bmiCell addGestureRecognizer:singleFingerTap];
    
    if (!_bmiChartView) {
        
        //                _bmiChartView.delegate = self;
        
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5])
            _bmiChartView = [[CombinedChartView alloc] initWithFrame:CGRectMake(5, 30, screenWidth+15, screenHeight)];
        else
            _bmiChartView = [[CombinedChartView alloc] initWithFrame:CGRectMake(30, 30, screenWidth, screenHeight)];
        
        _bmiChartView.descriptionText = @"";
        //                _lineChartView.noDataTextDescription = @"You need to provide data for the chart.";
        
        _bmiChartView.drawGridBackgroundEnabled = NO;
        _bmiChartView.drawBarShadowEnabled = NO;
        _bmiChartView.highlightFullBarEnabled = NO;
        
        _bmiChartView.drawOrder = @[
                                    @(CombinedChartDrawOrderBar),
                                    @(CombinedChartDrawOrderBubble),
                                    @(CombinedChartDrawOrderCandle),
                                    @(CombinedChartDrawOrderLine),
                                    @(CombinedChartDrawOrderScatter)
                                    ];
        
        ChartLegend *l = _bmiChartView.legend;
        l.wordWrapEnabled = YES;
        l.position = ChartLegendPositionAboveChartLeft;
        
        ChartYAxis *rightAxis = _bmiChartView.rightAxis;
        rightAxis.drawGridLinesEnabled = NO;
        rightAxis.axisMinValue = 0.0; // this replaces startAtZero = YES
        rightAxis.drawLabelsEnabled = NO;
        
        NSNumberFormatter *pFormatter = [[NSNumberFormatter alloc] init];
        pFormatter.numberStyle = NSNumberFormatterNoStyle;
        pFormatter.maximumFractionDigits = 1;
        
        ChartYAxis *leftAxis = _bmiChartView.leftAxis;
        leftAxis.drawGridLinesEnabled = NO;
        leftAxis.axisMinValue = 0.0; // this replaces startAtZero = YES
//        leftAxis.valueFormatter = pFormatter;
        
        ChartXAxis *xAxis = _bmiChartView.xAxis;
        xAxis.labelPosition = XAxisLabelPositionBottom;
        xAxis.axisMinimum = 0.0;
        xAxis.granularity = 1.0;
        xAxis.valueFormatter = self;
        
        [bmiCell addSubview:_bmiChartView];
    }
    [self setBMIChartData];
    
    [_bmiChartView animateWithYAxisDuration:2.0];
    
    return bmiCell;
}

#pragma mark Pie Chart Methods 
- (void)setupPieChartView:(PieChartView *)chartView
{
    chartView.usePercentValuesEnabled = YES;
    chartView.drawSlicesUnderHoleEnabled = NO;
    chartView.holeRadiusPercent = 0.58;
    chartView.transparentCircleRadiusPercent = 0.61;
    chartView.descriptionText = @"";
    [chartView setExtraOffsetsWithLeft:5.f top:10.f right:5.f bottom:5.f];
    
    chartView.drawCenterTextEnabled = YES;
    
    NSMutableParagraphStyle *paragraphStyle = [[NSParagraphStyle defaultParagraphStyle] mutableCopy];
    paragraphStyle.lineBreakMode = NSLineBreakByTruncatingTail;
    paragraphStyle.alignment = NSTextAlignmentCenter;
    
//    NSMutableAttributedString *centerText = [[NSMutableAttributedString alloc] initWithString:@"Activities"];
//    [centerText setAttributes:@{
//                                NSFontAttributeName: [UIFont fontWithName:@"HelveticaNeue-Light" size:14.f],
//                                NSParagraphStyleAttributeName: paragraphStyle
//                                } range:NSMakeRange(0, centerText.length)];
//    [centerText addAttributes:@{
//                                NSFontAttributeName: [UIFont fontWithName:@"HelveticaNeue-Light" size:11.f],
//                                NSForegroundColorAttributeName: UIColor.grayColor
//                                } range:NSMakeRange(10, centerText.length - 10)];
//    [centerText addAttributes:@{
//                                NSFontAttributeName: [UIFont fontWithName:@"HelveticaNeue-LightItalic" size:11.f],
//                                NSForegroundColorAttributeName: [UIColor colorWithRed:51/255.f green:181/255.f blue:229/255.f alpha:1.f]
//                                } range:NSMakeRange(centerText.length - 19, 19)];
//    chartView.centerAttributedText = centerText;
    
    chartView.drawHoleEnabled = YES;
    chartView.rotationAngle = 0.0;
    chartView.rotationEnabled = YES;
    chartView.highlightPerTapEnabled = YES;
    
    ChartLegend *l = chartView.legend;
    l.position = ChartLegendPositionRightOfChart;
    l.xEntrySpace = 7.0;
    l.yEntrySpace = 0.0;
    l.yOffset = 0.0;
}

- (void)setPieChartDataCount:(int)count range:(double)range
{
    NSMutableArray *values = [[NSMutableArray alloc] init];
    
    // IMPORTANT: In a PieChart, no values (Entry) should have the same xIndex (even if from different DataSets), since no values can be drawn above each other.
    double walkingValue = 0.0;
    double swimmingValue = 0.0;
    double runningValue = 0.0;
    double cyclingValue = 0.0;
    //                NSString* stepsTaken = @"";
    for (int i=0; i<[activityArray count]; i++) {
        
        if ([[[activityArray objectAtIndex:i] valueForKey:@"ActivityId"]intValue] ==1) {
            walkingValue += [[[activityArray objectAtIndex:i] valueForKey:@"Distance"] doubleValue];
        }
        else if ([[[activityArray objectAtIndex:i] valueForKey:@"ActivityId"]intValue] == 3) {
            cyclingValue += [[[activityArray objectAtIndex:i] valueForKey:@"Distance"] doubleValue];
        }
        else if ([[[activityArray objectAtIndex:i] valueForKey:@"ActivityId"]intValue] == 2) {
            runningValue += [[[activityArray objectAtIndex:i] valueForKey:@"Distance"] doubleValue];
        }
        else if ([[[activityArray objectAtIndex:i] valueForKey:@"ActivityId"]intValue] == 4) {
            swimmingValue += [[[activityArray objectAtIndex:i] valueForKey:@"Distance"] doubleValue];
        }
    }
//    NSArray* arr = [[NSArray alloc] initWithObjects:[NSNumber numberWithDouble:walkingValue ], [NSNumber numberWithDouble:runningValue ], [NSNumber numberWithDouble:cyclingValue], [NSNumber numberWithDouble: swimmingValue],[NSNumber numberWithDouble: run],[NSNumber numberWithInt: flights],[NSNumber numberWithDouble: steps], nil];

    NSArray* arr = [[NSArray alloc] initWithObjects:[NSNumber numberWithDouble:walkingValue ], [NSNumber numberWithDouble:runningValue ], [NSNumber numberWithDouble:cyclingValue], [NSNumber numberWithDouble: swimmingValue], nil];
    
    for (int i = 0; i < count; i++)
    {
//        [values addObject:[[PieChartDataEntry alloc] initWithValue:(arc4random_uniform(mult) + mult / 5) label:activityNameArray[i % activityNameArray.count]]];
        [values addObject:[[PieChartDataEntry alloc] initWithValue:[[arr objectAtIndex:i] doubleValue] label:[activityNameArray objectAtIndex:i]]];
//        if ([[arr objectAtIndex:i] doubleValue] != 0.0) {
//            [values addObject:[[BarChartDataEntry alloc] initWithValue:[[arr objectAtIndex:i] doubleValue] xIndex:i]];
//        }
    }
    
    double total = 0.0;
//    total = walkingValue + runningValue + cyclingValue + swimmingValue + run + flights + steps;
    
    total = walkingValue + runningValue + cyclingValue + swimmingValue;
    self.pieChartView.centerText = [NSString stringWithFormat:@"Activities\n Total Distance\n %.2f KM",total];
    
    PieChartDataSet *dataSet = [[PieChartDataSet alloc] initWithValues:values label:@""];
//    PieChartDataSet *dataSet = [[PieChartDataSet alloc] initWithYVals:values label:@""];
    dataSet.sliceSpace = 2.0;
    
    // add a lot of colors
    
    NSMutableArray *colors = [[NSMutableArray alloc] init];
    [colors addObjectsFromArray:ChartColorTemplates.vordiplom];
    [colors addObjectsFromArray:ChartColorTemplates.joyful];
    [colors addObjectsFromArray:ChartColorTemplates.colorful];
    [colors addObjectsFromArray:ChartColorTemplates.liberty];
    [colors addObjectsFromArray:ChartColorTemplates.pastel];
    [colors addObject:[UIColor colorWithRed:51/255.f green:181/255.f blue:229/255.f alpha:1.f]];
    
    dataSet.colors = colors;
    [dataSet setYValuePosition:PieChartValuePositionOutsideSlice];
    
    PieChartData *data = [[PieChartData alloc] initWithDataSet:dataSet];
//    PieChartData *data = [[PieChartData alloc] initWithXVals:activityNameArray dataSet:dataSet];
    
    NSNumberFormatter *pFormatter = [[NSNumberFormatter alloc] init];
    pFormatter.numberStyle = NSNumberFormatterPercentStyle;
    pFormatter.maximumFractionDigits = 1;
    pFormatter.multiplier = @1.f;
    pFormatter.percentSymbol = @" %";
    [data setValueFormatter:[[ChartDefaultValueFormatter alloc] initWithFormatter:pFormatter]];
    [data setValueFont:[UIFont fontWithName:@"HelveticaNeue-Light" size:11.f]];
    [data setValueTextColor:[UIColor darkGrayColor]];
    
    _pieChartView.data = data;
    [_pieChartView highlightValues:nil];
}

#pragma mark - ChartViewDelegate

- (void)chartValueSelected:(ChartViewBase * __nonnull)chartView entry:(ChartDataEntry * __nonnull)entry highlight:(ChartHighlight * __nonnull)highlight
{
    if (entry==nil) {
        return;
    }
    else{
        self.pieChartView.centerText = [NSString stringWithFormat:@"Total Distance\n %.2f KM",entry.y];
    }
//    if (entry.xIndex==5) {
//        self.pieChartView.centerText = [NSString stringWithFormat:@"Total Distance\n %.2f Floor's",entry.value];
//    }
//    else{
//        self.pieChartView.centerText = [NSString stringWithFormat:@"Total Distance\n %.2f KM",entry.value];
//    }
}

- (void)chartValueNothingSelected:(ChartViewBase * __nonnull)chartView
{
    double total = 0.0;
    for (int i=0; i<[chartView.data entryCount]; i++) {
        ChartDataEntry* entry = (ChartDataEntry*)[[chartView.data.dataSets objectAtIndex:0] entryForIndex:i];
        
        total+= entry.y;
    }
//    self.pieChartView.centerText = [NSString stringWithFormat:@"Activities\n Total Distance\n %.2f KM",total];
    
    self.pieChartView.centerText = [NSString stringWithFormat:@"Total Distance\n %.2f KM",total];
}

#pragma mark Bar Chart Methods 
- (void)setupBarLineChartView:(BarLineChartViewBase *)chartView
{
    chartView.descriptionText = @"";
//    chartView.noDataTextDescription = @"You need to provide data for the chart.";
    
    chartView.drawGridBackgroundEnabled = NO;
    
    chartView.dragEnabled = YES;
    [chartView setScaleEnabled:YES];
    chartView.pinchZoomEnabled = NO;
    
    // ChartYAxis *leftAxis = chartView.leftAxis;
    
    ChartXAxis *xAxis = chartView.xAxis;
    xAxis.labelPosition = XAxisLabelPositionBottom;
    
    chartView.rightAxis.enabled = NO;
}

/*- (void)setDataCount:(int)count range:(double)range
{
    double start = 0.0;
    
    _barChartView.xAxis.axisMinimum = start;
    _barChartView.xAxis.axisMaximum = start + count + 2;
    
    NSMutableArray *yVals = [[NSMutableArray alloc] init];
    
    for (int i = start; i < start + count + 1; i++)
    {
        double mult = (range + 1);
        double val = (double) (arc4random_uniform(mult));
        [yVals addObject:[[BarChartDataEntry alloc] initWithX:(double)i + 1.0 y:val]];
//        [yVals addObject:[[BarChartDataEntry alloc] initWithValue:val xIndex:(double)i + 1.0]];
    }
    
    BarChartDataSet *set1 = nil;
    if (_barChartView.data.dataSetCount > 0)
    {
        set1 = (BarChartDataSet *)_barChartView.data.dataSets[0];
        set1.yVals = yVals;
        [_barChartView.data notifyDataChanged];
        [_barChartView notifyDataSetChanged];
    }
    else
    {
        set1 = [[BarChartDataSet alloc] initWithYVals:yVals label:@"The year 2017"];
        [set1 setColors:ChartColorTemplates.material];
        
        NSMutableArray *dataSets = [[NSMutableArray alloc] init];
        [dataSets addObject:set1];
        
        BarChartData *data = [[BarChartData alloc] initWithXVals:nil dataSets:dataSets];
        [data setValueFont:[UIFont fontWithName:@"HelveticaNeue-Light" size:10.f]];
        
//        data.barWidth = 0.9f;
        
        _barChartView.data = data;
    }
}*/

#pragma mark Combined Bar Chart Methods 
- (void)setBarChartData
{
//    [barChartDateArray removeAllObjects];
//    for (int index = 0; index < [bloodPressureArray count]; index++)
//    {
//        [barChartDateArray addObject:[[bloodPressureArray objectAtIndex:index] valueForKey:@"strCollectionDate"]];
//    }
    
    CombinedChartData *data = [[CombinedChartData alloc] init ];//WithXVals:barChartDateArray];
    data.lineData = [self generatePulseLineData];
    data.barData = [self generateBarData];
    
    _barChartView.xAxis.axisMaximum = data.xMax + 1.0;
//    _barChartView.xAxis._axisMaximum = data.yMax + 1.0;
//    NSLog(@"x value %ld",(long)data.yMax);
    
    _barChartView.data = data;
}

- (BarChartData *)generateBarData
{
    NSMutableArray *systolicEntry = [[NSMutableArray alloc] init];
    NSMutableArray *diastolicEntry = [[NSMutableArray alloc] init];
    
    [barChartDateArray removeAllObjects];
    for (int index = 0; index < [bloodPressureArray count]; index++)
    {
        double systolicValue = [[[bloodPressureArray objectAtIndex:index] valueForKey:@"ResSystolic"] doubleValue];
        double diastolicValue = [[[bloodPressureArray objectAtIndex:index] valueForKey:@"ResDiastolic"] doubleValue];
        
        [barChartDateArray addObject:[[bloodPressureArray objectAtIndex:index] valueForKey:@"strCollectionDate"]];
        [systolicEntry addObject:[[BarChartDataEntry alloc] initWithX:index y:systolicValue]];
        
        // stacked
        [diastolicEntry addObject:[[BarChartDataEntry alloc] initWithX:index y:diastolicValue]];
        
//        [systolicEntry addObject:[[BarChartDataEntry alloc] initWithValue:systolicValue xIndex:index]];
//        
//        // stacked
//        [diastolicEntry addObject:[[BarChartDataEntry alloc] initWithValue:diastolicValue xIndex:index]];
    }
    NSNumberFormatter *pFormatter = [[NSNumberFormatter alloc] init];
    pFormatter.numberStyle = NSNumberFormatterNoStyle;
    pFormatter.maximumFractionDigits = 1;
    
//    BarChartDataSet *set1 = [[BarChartDataSet alloc] initWithYVals:systolicEntry label:@"Systolic"];
    BarChartDataSet *set1 = [[BarChartDataSet alloc] initWithValues:systolicEntry label:@"Systolic"];
    [set1 setColor:[UIColor colorWithRed:0/255.f green:117/255.f blue:194/255.f alpha:1.f]];
    set1.valueTextColor = [UIColor colorWithRed:0/255.f green:117/255.f blue:194/255.f alpha:1.f];
    set1.valueFont = [UIFont systemFontOfSize:10.f];
    set1.axisDependency = AxisDependencyLeft;
//    set1.valueFormatter = [[ChartDefaultValueFormatter alloc] initWithFormatter:pFormatter];
    
//    BarChartDataSet *set2 = [[BarChartDataSet alloc] initWithYVals:diastolicEntry label:@"Diastolic"];
    BarChartDataSet *set2 = [[BarChartDataSet alloc] initWithValues:diastolicEntry label:@"Diastolic"];
    [set2 setColor:[UIColor colorWithRed:26/255.f green:175/255.f blue:93/255.f alpha:1.f]];
    set2.valueTextColor = [UIColor colorWithRed:26/255.f green:175/255.f blue:93/255.f alpha:1.f];
    set2.valueFont = [UIFont systemFontOfSize:10.f];
    set2.axisDependency = AxisDependencyLeft;
//    set2.valueFormatter = [[ChartDefaultValueFormatter alloc] initWithFormatter:pFormatter];
    
    float groupSpace = 0.06f;
    float barSpace = 0.02f; // x2 dataset
    float barWidth = 0.45f; // x2 dataset
    // (0.45 + 0.02) * 2 + 0.06 = 1.00 -> interval per "group"
    
//    NSMutableArray *dataSets = [[NSMutableArray alloc] init];
//    [dataSets addObject:set1];
//    [dataSets addObject:set2];
    
//    BarChartData *d = [[BarChartData alloc] init];
//    d.barWidth = barWidth;
    BarChartData *d = [[BarChartData alloc] initWithDataSets:@[set1, set2]];
    d.barWidth = barWidth;
    
//    [d addDataSet:set1];
//    [d addDataSet:set2];
    
//    [set1 setBarSpace:barSpace];
//    [set2 setBarSpace:barSpace];
    [d groupBarsFromX:0.0 groupSpace:groupSpace barSpace:barSpace];

    return d;
}

- (LineChartData *)generatePulseLineData
{
    LineChartData *d = [[LineChartData alloc] init];
    
    NSMutableArray *pulseEntry = [[NSMutableArray alloc] init];
    
    for (int index = 0; index < [bloodPressureArray count]; index++)
    {
        double pulseValue = [[[bloodPressureArray objectAtIndex:index] valueForKey:@"ResPulse"] doubleValue];
        
//        [pulseEntry addObject:[[ChartDataEntry alloc] initWithValue:pulseValue xIndex:index]];
        [pulseEntry addObject:[[ChartDataEntry alloc] initWithX:index + 0.5 y:pulseValue]];
    }
    
    NSNumberFormatter *pFormatter = [[NSNumberFormatter alloc] init];
    pFormatter.numberStyle = NSNumberFormatterNoStyle;
    pFormatter.maximumFractionDigits = 1;
    
//    LineChartDataSet *set = [[LineChartDataSet alloc] initWithYVals:pulseEntry label:@"Pulse"];
    LineChartDataSet *set = [[LineChartDataSet alloc] initWithValues:pulseEntry label:@"Pulse"];
    [set setColor:[UIColor yellowColor]];
    set.lineWidth = 2.5;
    [set setCircleColor:[UIColor yellowColor]];
    set.circleRadius = 5.0;
    set.circleHoleRadius = 2.5;
    set.fillColor = [UIColor greenColor];
    set.mode = LineChartModeLinear;
    set.drawValuesEnabled = YES;
    set.valueFont = [UIFont systemFontOfSize:10.f];
    set.valueTextColor = [UIColor blackColor];
//    set.valueFormatter = [[ChartDefaultValueFormatter alloc] initWithFormatter:pFormatter];
    
    set.axisDependency = AxisDependencyLeft;

    [d addDataSet:set];

    return d;
}

#pragma mark Line Chart Methods 
- (void)setChartData
{
    
//    for (int index = 0; index < [bloodGlucoseArray count]; index++)
//    {
//        [lineChartDateArray addObject:[[bloodGlucoseArray objectAtIndex:index] valueForKey:@"strCollectionDate"]];
//    }
//    CombinedChartData *data = [[CombinedChartData alloc] initWithXVals:lineChartDateArray];
    
    CombinedChartData *data = [[CombinedChartData alloc] init];
    data.lineData = [self generateLineData];
    
    _lineChartView.xAxis.axisMaximum = data.xMax + 0.25;
    
    _lineChartView.data = data;
}

- (LineChartData *)generateLineData
{
    LineChartData *d = [[LineChartData alloc] init];
    
    NSMutableArray *glucoseEntry = [[NSMutableArray alloc] init];
    
    [lineChartDateArray removeAllObjects];
    for (int index = 0; index < [bloodGlucoseArray count]; index++)
    {
        double glucoseValue = [[[bloodGlucoseArray objectAtIndex:index] valueForKey:@"Result"] doubleValue];
        
//        [glucoseEntry addObject:[[ChartDataEntry alloc] initWithValue:glucoseValue xIndex:index]];
        [lineChartDateArray addObject:[[bloodGlucoseArray objectAtIndex:index] valueForKey:@"strCollectionDate"]];
        [glucoseEntry addObject:[[ChartDataEntry alloc] initWithX:index y:glucoseValue]];
    }
    
//    LineChartDataSet *set = [[LineChartDataSet alloc] initWithYVals:glucoseEntry label:@"Glucose"];
    LineChartDataSet *set = [[LineChartDataSet alloc] initWithValues:glucoseEntry label:@"Glucose"];
    [set setColor:[UIColor colorWithRed:139/255.f green:233/255.f blue:254/255.f alpha:1.f]];
    set.lineWidth = 2.5;
    [set setCircleColor:[UIColor colorWithRed:139/255.f green:233/255.f blue:254/255.f alpha:1.f]];
    set.circleRadius = 5.0;
    set.circleHoleRadius = 2.5;
    set.fillColor = [UIColor greenColor];
//    set.mode = LineChartModeLinear;
    
    NSNumberFormatter *pFormatter = [[NSNumberFormatter alloc] init];
    pFormatter.numberStyle = NSNumberFormatterNoStyle;
    pFormatter.maximumFractionDigits = 1;
    
    set.drawValuesEnabled = YES;
    set.valueFormatter = [[ChartDefaultValueFormatter alloc] initWithFormatter:pFormatter];
//    set.formLineWidth = 1.0;
//    set.formSize = 15.0;
    set.valueFont = [UIFont systemFontOfSize:8.f];
    set.valueTextColor = [UIColor blackColor];
    
    set.axisDependency = AxisDependencyLeft;
    
    [d addDataSet:set];
    
    return d;
}

#pragma mark BMI Line Chart Methods 
- (void)setBMIChartData
{
    [bmilineChartDateArray removeAllObjects];
//    for (int index = 0; index < [weightArray count]; index++)
//    {
//        [bmilineChartDateArray addObject:[[weightArray objectAtIndex:index] valueForKey:@"strCollectionDate"]];
//    }
//    
//    CombinedChartData *data = [[CombinedChartData alloc] initWithXVals:bmilineChartDateArray];
    
    CombinedChartData *data = [[CombinedChartData alloc] init];
    data.lineData = [self generateBMILineData];
    
    _bmiChartView.xAxis.axisMaximum = data.xMax + 0.25;
    
    _bmiChartView.data = data;
}

- (LineChartData *)generateBMILineData
{
    LineChartData *d = [[LineChartData alloc] init];
    
    NSMutableArray *bmiEntry = [[NSMutableArray alloc] init];
    
    for (int index = 0; index < [weightArray count]; index++)
    {
        int weightInt = -1;
        NSString* weight = [[weightArray objectAtIndex:index] valueForKey:@"Result"];
        if ([weight isKindOfClass:[NSNull class]] || [weight isEqualToString:@"null"] || [weight isEqualToString:@""]) {
            weightInt = 0;
        }
        else{
            weightInt = [weight intValue];
        }
        
        int heightInt = -1;
        NSString* height = [[weightArray objectAtIndex:index] valueForKey:@"Goal"];
        if ([height isKindOfClass:[NSNull class]] || [height isEqualToString:@"null"] || [height isEqualToString:@""]) {
            heightInt = 0;
        }
        else{
            heightInt = [height intValue];
        }
        
        NSString* bmi = [self BMICalculator:weightInt bodyHeight:heightInt];
        
//        [bmiEntry addObject:[[ChartDataEntry alloc] initWithValue:[bmi doubleValue] xIndex:index]];
        [bmilineChartDateArray addObject:[[weightArray objectAtIndex:index] valueForKey:@"strCollectionDate"]];
        [bmiEntry addObject:[[ChartDataEntry alloc] initWithX:index y:[bmi doubleValue]]];
    }
    
//    LineChartDataSet *set = [[LineChartDataSet alloc] initWithYVals:bmiEntry label:@"BMI"];
    LineChartDataSet *set = [[LineChartDataSet alloc] initWithValues:bmiEntry label:@"BMI"];
    [set setColor:[UIColor colorWithRed:255/255.f green:208/255.f blue:140/255.f alpha:1.f]];
    set.lineWidth = 2.5;
    [set setCircleColor:[UIColor colorWithRed:255/255.f green:208/255.f blue:140/255.f alpha:1.f]];
    set.circleRadius = 5.0;
    set.circleHoleRadius = 2.5;
    set.fillColor = [UIColor greenColor];
    //    set.mode = LineChartModeLinear;
    set.drawValuesEnabled = YES;
    //    set.formLineWidth = 1.0;
    //    set.formSize = 15.0;
    set.valueFont = [UIFont systemFontOfSize:10.f];
    set.valueTextColor = [UIColor blackColor];
    
    set.axisDependency = AxisDependencyLeft;
    
    [d addDataSet:set];
    
    return d;
}

#pragma mark BMI Calculator 
-(NSString*)BMICalculator:(int)weight bodyHeight:(int)height{
    
    double heigthInMetre = height/100.0f;
    
    double totalHeight = (heigthInMetre*heigthInMetre);
    double bmi = weight/totalHeight;
    
    return [NSString stringWithFormat:@"%.02f",bmi];
}

#pragma mark - IAxisValueFormatter

- (NSString *)stringForValue:(double)value
                        axis:(ChartAxisBase *)axis
{
    if(_barChartView.xAxis == axis){
        if (value<[barChartDateArray count]) {
            return barChartDateArray[(int)value % barChartDateArray.count];
        }
        return nil;
    }
    else if(_lineChartView.xAxis == axis){
        return lineChartDateArray[(int)value % lineChartDateArray.count];
    }
    else {
        return bmilineChartDateArray[(int)value % bmilineChartDateArray.count];
    }
}

-(UIColor*)colorWithHexString:(NSString*)hex
{
    NSString *cString = [[hex stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] uppercaseString];
    
    // String should be 6 or 8 characters
    if ([cString length] < 6) return [UIColor grayColor];
    
    // strip 0X if it appears
    if ([cString hasPrefix:@"0X"]) cString = [cString substringFromIndex:2];
    
    if ([cString length] != 6) return  [UIColor grayColor];
    
    // Separate into r, g, b substrings
    NSRange range;
    range.location = 0;
    range.length = 2;
    NSString *rString = [cString substringWithRange:range];
    
    range.location = 2;
    NSString *gString = [cString substringWithRange:range];
    
    range.location = 4;
    NSString *bString = [cString substringWithRange:range];
    
    // Scan values
    unsigned int r, g, b;
    [[NSScanner scannerWithString:rString] scanHexInt:&r];
    [[NSScanner scannerWithString:gString] scanHexInt:&g];
    [[NSScanner scannerWithString:bString] scanHexInt:&b];
    
    return [UIColor colorWithRed:((float) r / 255.0f)
                           green:((float) g / 255.0f)
                            blue:((float) b / 255.0f)
                           alpha:1.0f];
}

#pragma mark BP Graph Tap Action
-(void)bloodPressureSingleTap:(int)indexpath{
    BloodPresureViewController* objBPController = [self.storyboard instantiateViewControllerWithIdentifier:@"BPStoryboardIdentity"];
    objBPController.isFromDashboard = YES;
    objBPController.bloodPressureArray = bloodPressureArray;
    [self.navigationController pushViewController:objBPController animated:YES];
}

#pragma mark Diabetes Graph Tap Action
-(void)diabetesSingleTap:(int)indexpath{
    DiabetesViewController* objDiabetesController = [self.storyboard instantiateViewControllerWithIdentifier:@"DiabetesStoryboardIdentity"];
    objDiabetesController.isFromDashboard = YES;
    objDiabetesController.bloodGlucoseArray = bloodGlucoseArray;
    [self.navigationController pushViewController:objDiabetesController animated:YES];
}

#pragma mark BMI Graph Tap Action
-(void)bmiSingleTap:(int)indexpath{
    WeightViewController* objBMIController = [self.storyboard instantiateViewControllerWithIdentifier:@"WeightStoryboardIdentity"];
    objBMIController.isFromDashboard = YES;
    objBMIController.weightArray = weightArray;
    [self.navigationController pushViewController:objBMIController animated:YES];
}

#pragma mark Activity Graph Tap Action
-(void)activitySingleTap:(int)indexPath{
    ActivityViewController* objActivityController = [self.storyboard instantiateViewControllerWithIdentifier:@"ActivityStoryboardIdentity"];
    objActivityController.isFromDashboard = YES;
    objActivityController.activityArray = activityArray;
    [self.navigationController pushViewController:objActivityController animated:YES];
}

#pragma mark UITableView Delegate Methods
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
    
    switch (indexPath.section) {
        case 2:{
            BloodPresureViewController* objBPController = [self.storyboard instantiateViewControllerWithIdentifier:@"BPStoryboardIdentity"];
            objBPController.isFromDashboard = YES;
            objBPController.bloodPressureArray = bloodPressureArray;
            [self.navigationController pushViewController:objBPController animated:YES];
            break;
        }
        case 3:{
            DiabetesViewController* objDiabetesController = [self.storyboard instantiateViewControllerWithIdentifier:@"DiabetesStoryboardIdentity"];
            objDiabetesController.isFromDashboard = YES;
            objDiabetesController.bloodGlucoseArray = bloodGlucoseArray;
            [self.navigationController pushViewController:objDiabetesController animated:YES];
            break;
        }
        case 4:{
            WeightViewController* objBMIController = [self.storyboard instantiateViewControllerWithIdentifier:@"WeightStoryboardIdentity"];
            objBMIController.isFromDashboard = YES;
            objBMIController.weightArray = weightArray;
            [self.navigationController pushViewController:objBMIController animated:YES];
            break;
        }
        case 1:{
            ActivityViewController* objActivityController = [self.storyboard instantiateViewControllerWithIdentifier:@"ActivityStoryboardIdentity"];
            objActivityController.isFromDashboard = YES;
            objActivityController.activityArray = activityArray;
            [self.navigationController pushViewController:objActivityController animated:YES];
            break;
        }
        default:
            break;
    }
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad])
        return indexPath.section>3?300:500;
    else
        if (run>=0.01 || flights>0 || steps>=0.01) {
            return indexPath.section>0?400:120;
        }
        else{
            return indexPath.section>3?200:400;
        }
}

- (CGFloat)tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5])
        return 24;
    else
        return 30;
}

- (UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section{
    CGRect frame = CGRectMake(0,0,[UIScreen mainScreen].bounds.size.width-100,30);
    UILabel *label = [[UILabel alloc]initWithFrame:frame];
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        label.font = [UIFont systemFontOfSize:18];
    }
    else{
        label.font = [UIFont systemFontOfSize:28];
    }
    label.backgroundColor = [[UIColor grayColor]colorWithAlphaComponent:0.3];
    label.text = [sectionsArray objectAtIndex:section];
    label.textColor = [UIColor darkGrayColor];
    label.textAlignment = NSTextAlignmentCenter;
    return label;
}

- (UIColor*)randomColor
{
    CGFloat hue = ( arc4random() % 256 / 256.0 );  //  0.0 to 1.0
    CGFloat saturation = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from white
    CGFloat brightness = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from black
    return [UIColor colorWithHue:hue saturation:saturation brightness:brightness alpha:1];
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

- (IBAction)profilePhotoButtonAction:(id)sender {
    
    UserProfileViewController* objUserProfileController = [self.storyboard instantiateViewControllerWithIdentifier:@"userPersonalProfileIdentity"];
    objUserProfileController.isFromDashboard = YES;
    [self.navigationController pushViewController:objUserProfileController animated:YES];
    
//    dispatch_async(dispatch_get_main_queue(), ^{
//        // your navigation controller action goes here
//        
//    });
}

- (void)syncButtonAction {
    [[[UIAlertView alloc]initWithTitle:kAppTitle message:[NSString stringWithFormat:@"Do you want to sync Apple Health App activity data(today's only) with %@ server",kAppTitle] delegate:self cancelButtonTitle:kCancelButtonText otherButtonTitles:kOKButtonText, nil]show];
}

#pragma mark Sync Health App Data 
-(void)SyncHealthAppData{
    
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"pushing data..."];//Show loading indicator.
        
        //            NSString *uuid = [[NSUUID UUID] UUIDString];
        
        NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
        [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
        
        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
        [dateFormatter setDateFormat:@"dd-MM-yyyy"];
        
        //        NSString* deviceDate = self.dateButton.titleLabel.text;
        //        NSDate* date = [NSDate date];
        //
        //        NSString* activityDateString = [dateFormat stringFromDate:date];
        //            activityDateString = [NSString stringWithFormat:@"%@ 00:00:00",activityDateString];
        
        NSString* dateString = [dateFormat stringFromDate:[NSDate date]];
        NSArray* array = [dateString componentsSeparatedByString:@"+"];
        dateString = [array objectAtIndex:0];
        
        //        NSString* timeString = self.timeButton.titleLabel.text;
        //        NSArray* timeArray = [timeString componentsSeparatedByString:@":"];
        //        int hours = [[timeArray objectAtIndex:0] intValue];
        //        int minutes = [[timeArray objectAtIndex:1] intValue];
        //
        //        int hourInMinutes = hours*60;
        //        int totalTimeInMinutes = hourInMinutes+minutes;
        
        NSString* activityNameID;
        NSString* distance;
        if (run >= 0.01) {
            activityNameID = @"2";
            distance = [NSString stringWithFormat:@"%.02f",run];
            
            NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
            [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"UserId"];
            [dicParams setObject:activityNameID forKey:@"ActivityId"];
            [dicParams setObject:@"Apple Health App" forKey:@"PathName"];
            [dicParams setObject:distance forKey:@"Distance"];
            [dicParams setObject:dateString forKey:@"CollectionDate"];
            [dicParams setObject:dateString forKey:@"CreatedDate"];
            [dicParams setObject:dateString forKey:@"ModifiedDate"];
            [dicParams setObject:[NSString stringWithFormat:@"%d",runTimeDuration] forKey:@"FinishTime"];
            //        [dicParams setObject:self.commentsTextview.text forKey:@"Comments"];
            [dicParams setObject:SourceId forKey:@"SourceId"];
            [dicParams setObject:@"false" forKey:@"DeleteFlag"];
            
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
                //  NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]!=1) {
                    [kAppDelegate showAlertView:@"Operation failed"];
                }
            } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
                NSLog(@"Error: %@", error);
                //                [kAppDelegate hideLoadingIndicator];
                [kAppDelegate showAlertView:@"Request failed"];
            }];
        }
        
        if (steps >= 0.01) {
            activityNameID = @"1";
            distance = [NSString stringWithFormat:@"%.02f",steps];
            
            NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
            [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"UserId"];
            [dicParams setObject:activityNameID forKey:@"ActivityId"];
            [dicParams setObject:@"Apple Health App" forKey:@"PathName"];
            [dicParams setObject:distance forKey:@"Distance"];
            [dicParams setObject:dateString forKey:@"CollectionDate"];
            [dicParams setObject:dateString forKey:@"CreatedDate"];
            [dicParams setObject:dateString forKey:@"ModifiedDate"];
            [dicParams setObject:[NSString stringWithFormat:@"%d",stepsTimeDuration] forKey:@"FinishTime"];
            //        [dicParams setObject:self.commentsTextview.text forKey:@"Comments"];
            [dicParams setObject:SourceId forKey:@"SourceId"];
            [dicParams setObject:@"false" forKey:@"DeleteFlag"];
            
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
                //  NSLog(@"Result dict %@",responseObject);
                
                if ([[responseObject valueForKey:@"status"] integerValue]!=1) {
                    [kAppDelegate showAlertView:@"Operation failed"];
                }
                
            } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
                NSLog(@"Error: %@", error);
                [kAppDelegate hideLoadingIndicator];
                [kAppDelegate showAlertView:@"Request failed"];
            }];
        }
        
        if (run < 0.01 && steps < 0.01) {
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"No health data available to sync"];
        }
        else{
            [kAppDelegate showAlertView:[NSString stringWithFormat:@"Activities are synced with %@ server", kAppTitle]];
            
//            NSString *format = @"yyyy-MM-dd HH:mm:ss";
//            
//            NSDateFormatter *outDateFormatter = [[NSDateFormatter alloc] init];
//            outDateFormatter.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"IST"];
//            outDateFormatter.dateFormat = format;
//            NSString *outDateStr = [outDateFormatter stringFromDate:[NSDate date]];
//            
//            [[NSUserDefaults standardUserDefaults] setObject:[NSDate date] forKey:lastHealthDataSyncTime];
            
            [[NSUserDefaults standardUserDefaults] setObject:[NSDate date] forKey:lastHealthDataSyncTime];
            
//            self.walkingDistanceLabel.text = @"0.0 KM";
//            self.stepsCountLabel.text = @"0.0 KM";
            
            [self GetHealthAppData];
            
            [sectionsArray removeAllObjects];
            [_dashboardTableView setDataSource:self];
            [_dashboardTableView setDelegate:self];
            [_dashboardTableView reloadData];
        }
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

//#pragma mark AlertView Delegate Method 
//- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
//    if (buttonIndex==1) {
//        [self SyncHealthAppData];
//    }
//}

@end
