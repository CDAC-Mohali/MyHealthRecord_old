//
//  SlideOutViewController.m
//  Medibook
//
//  Created by Gagandeep Singh on 12/10/14.
//  Copyright (c) 2014. All rights reserved.
//

#import "SlideOutViewController.h"
#import "Constants.h"

@interface SlideOutViewController ()

@end

@import Firebase;
@import FirebaseInstanceID;
@import FirebaseMessaging;

@implementation SlideOutViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
//    if ([[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]) {
//        self.userName.text = [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME];
//    }
    
//    int status=[[kAppDelegate roleID] intValue];
//    if (status == 4) {
//        self.userName.text = [NSString stringWithFormat:@"%@",[kAppDelegate userName]];
//
    self.dashboardScreensArray = [[NSMutableArray alloc]initWithObjects:@"Dashboard", nil];
    self.screensArray=[[NSMutableArray alloc]initWithObjects:@"Allergies",@"Problems",@"Immunizations",@"Procedures",@"Lab Tests",@"Prescriptions",@"Medications", nil];
    self.dailyWorkoutScreensArray=[[NSMutableArray alloc]initWithObjects:@"Activities",@"Blood Pressure",@"Blood Glucose",@"BMI", nil];
    self.settingScreensArray=[[NSMutableArray alloc]initWithObjects:@"Medical Contacts",@"Sharing",@"About Us",@"Logout", nil];
    self.userProfileScreensArray=[[NSMutableArray alloc]initWithObjects:@"Personal Information",@"Emergency Information",@"Employer Information",@"Insurance Information",@"Preferred Hospital", nil];

    self.dashboardScreenImagesArray=[[NSMutableArray alloc] initWithObjects:@"dashboard", nil];
    self.dailyWorkoutScreenImagesArray=[[NSMutableArray alloc]initWithObjects:@"activity",@"bp",@"bg",@"bmi", nil];
    self.settingScreenImagesArray=[[NSMutableArray alloc]initWithObjects:@"doctor-enter",@"sharegrey",@"aboutus",@"logout", nil];
    self.screenImagesArray=[[NSMutableArray alloc]initWithObjects:@"allergy",@"healthCondition",@"immunization",@"procedures",@"labsTests",@"prescription",@"medication", nil];
    self.userProfileScreenImagesArray=[[NSMutableArray alloc]initWithObjects:@"userprofile",@"emergencycontact",@"employer",@"insurance",@"special", nil];

    self.sectionsArray = [[NSMutableArray alloc]initWithObjects:@"Dashboard Activities",@"PHR Modules",@"Wellness",@"User Profile",@"Sharing & Logout", nil];
//    self.sectionsArray=[[NSMutableArray alloc]initWithObjects:@"Dashboard Activities",@"PHR Modules",@"Daily Workout",@"Settings & Logout", nil];
    
//
//    }
//    
//    else{
//        self.userName.text = [NSString stringWithFormat:@"Dr. %@",[kAppDelegate userName]];
//
//        self.screensArray=[[NSMutableArray alloc]initWithObjects:@"Prescriptions",@"Appointments",@"Patients",@"Doctors",@"Notifications",@"Logout", nil];
//    }
   
    
    
//    NSString *bloodGroupNameFilePath = [[NSBundle mainBundle] pathForResource:@"BloodGroups" ofType:@"plist"];
//    NSMutableArray *bloodGroupArray = [[NSMutableArray alloc] initWithContentsOfFile:bloodGroupNameFilePath];
    
    /*NSData *data = [[[NSUserDefaults standardUserDefaults] valueForKey:USERPROFILE] dataUsingEncoding:NSUTF8StringEncoding];
    id json = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
    
    NSDictionary* dict = [[NSDictionary alloc] initWithDictionary:json];
    
    //Getting Gender of user
    NSString* gender = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"UserPersonalProfileViewModel"]  valueForKey:@"strGender"]];
    if ([gender isEqualToString:@"M"]) {
        gender = @"Male";
    }
    else if ([gender isEqualToString:@"F"]){
        gender = @"Female";
    }
    else{
        gender = @"Undefined";
    }
   
    
    //Calculating DOB of user
    NSString* dob = [NSString stringWithFormat:@"%@",[[dict valueForKey:@"UserPersonalProfileViewModel"]  valueForKey:@"DOB"]];
    NSArray* arr = [dob componentsSeparatedByString:@"T"];
    dob = [arr objectAtIndex:0];
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"yyyy/MM/dd"];
    NSDate* birthday = [dateFormatter dateFromString:dob];
    NSDate* nowDate = [NSDate date];
    
    NSDateComponents* ageComponents = [[NSCalendar currentCalendar]
                                       components:NSCalendarUnitYear
                                       fromDate:birthday
                                       toDate:nowDate
                                       options:0];
    NSInteger age = [ageComponents year];*/
    //******//
    
//    self.userName.text = [NSString stringWithFormat:@"%@ \n\n%@ %ld yrs \nHeight: 188 cm",[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME],gender,(long)age];
    
    
//    NSDictionary *attrs = @{
//                            NSFontAttributeName:[UIFont systemFontOfSize:20 weight:-5],
//                            NSForegroundColorAttributeName:[UIColor blackColor]
//                            };
//    NSDictionary *subAttrs = @{
//                               NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
//                               };
//    
//    NSMutableAttributedString *attributedText =
//    [[NSMutableAttributedString alloc] initWithString:[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]
//                                           attributes:subAttrs];
//    [attributedText setAttributes:subAttrs range:NSMakeRange(0, [[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME] length])];
    
//    self.userName.attributedText = attributedText;
    
//    int bloodID = [[[dict valueForKey:@"UserPersonalProfileViewModel"] valueForKey:@"BloodType"]intValue];
//    
//    
//    self.bloodGroupLabel.text = [NSString stringWithFormat:@"Blood Group\n\t   %@",[[bloodGroupArray objectAtIndex:bloodID-1] valueForKey:@"Name"]];
    
//    NSLog(@"username is %@",[kAppDelegate userName]);
    
    self.screensTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        [_userImageView.layer setCornerRadius:45];
        self.userName.font = [UIFont systemFontOfSize:26.0f weight:-1];
        self.emailLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.userName.textAlignment = NSTextAlignmentLeft;
        self.emailLabel.textAlignment = NSTextAlignmentLeft;
    }
    else{
        self.userName.textAlignment = NSTextAlignmentLeft;
        self.emailLabel.textAlignment = NSTextAlignmentCenter;
        [_userImageView.layer setCornerRadius:60];
    }

    [_userImageView.layer setMasksToBounds:YES];
//    [_userImageView.layer setBorderWidth:1.0];

    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
        
        dispatch_async(dispatch_get_main_queue(), ^{
            
            if (imageData) {
                [_userImageView setImage:[UIImage imageWithData:imageData]];
            }
            else{
                [_userImageView setImage:[UIImage imageNamed:@"userImage"]];
            }
        });
    });
    
    self.userName.text = [NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]];
    self.emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
    
    //[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(downloadUpdatedImage) name:@"ProfileImageUpdated" object:nil];
//    [self downloadUpdatedImage];
    [self.screensTableView setSeparatorStyle:UITableViewCellSeparatorStyleSingleLine];

//    [self updateUserInfo];
    // Do any additional setup after loading the view.
}

#pragma mark - Call UpdateUserInfo Webservice
//-(void)updateUserInfo{
//    [kAppDelegate getCurrentLocationLatLong];
//
//    NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
//    [dicParams setObject:[kAppDelegate userID] forKey:@"UserID"];
//    [dicParams setObject:[kAppDelegate deviceToken] forKey:@"Token"];
//    [dicParams setObject:[kAppDelegate latitude] forKey:@"Latitude"];
//    [dicParams setObject:[kAppDelegate longitude] forKey:@"Longitude"];
//
//    
//    //AFNetworking methods.
//    AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
//    AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
//    manager.requestSerializer = requestSerializer;
//    
//    NSString *urlString=[NSString stringWithFormat:@"%@%@",kBaseURL,kUpdateUserInfo];//Url string.
//    
//    [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
//        if (![responseObject isKindOfClass:[NSNull class]]) {
//            
//        }
//    } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
//        [kAppDelegate hideLoadingIndicator];
//        [kAppDelegate showAlertbannerFromTop:1 alertBannerTitle:@"Medibook Alert" alertBannerSubtitle:@"Registration failed"];
//    }];
//    
//}

-(void)viewWillAppear:(BOOL)animated{
//    [self setUserImage];
    
//    if ([kAppDelegate isUserProfileUpdated]==1) {
        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
            
//            [kAppDelegate setIsUserProfileUpdated:0];
            
            NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
            
            dispatch_async(dispatch_get_main_queue(), ^{
                
                if (imageData) {
                    [_userImageView setImage:[UIImage imageWithData:imageData]];
                }
                else{
                    [_userImageView setImage:[UIImage imageNamed:@"userImage"]];
                }
            });
        });
        self.userName.text = [NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]];
//    }
}

//-(void)setUserImage{
//    NSData *imageData=[[NSData alloc]initWithBase64EncodedString:[[[kAppDelegate userProfileArray] objectAtIndex:0] valueForKey:@"Image"] options:NSDataBase64DecodingIgnoreUnknownCharacters];
//    if (imageData.length>0) {
//        _userImageView.image=[UIImage imageWithData:imageData];
//
//    }
//}
//
//-(void)downloadUpdatedImage{
//    
//    [self runBlockInMainQueueIfNecessary:^{
//        NSString* imgURL = [NSString stringWithFormat:@"%@",[kAppDelegate userProfileImage]];
//        
//        UIImage* serverImage = [UIImage imageWithData:[NSData dataWithContentsOfURL:[NSURL URLWithString: imgURL]]];
//        if (serverImage==nil) {
//            [self runBlockInMainQueueIfNecessary:^{
//                int status=[[kAppDelegate roleID] intValue];
//                if (status != 4) {
//                    _userImageView.image=[UIImage imageNamed:@"doctor_iphone"];
//                    
//                }
//                else{
//                    
//                    _userImageView.image=[UIImage imageNamed:@"pat_iphone"];
//                }
//                
//            }];
//        }
//        else{
//            [self runBlockInMainQueueIfNecessary:^{
//                _userImageView.image = serverImage;
//                
//            }];
//        }
//    }];
//}
//
//- (void)runBlockInMainQueueIfNecessary:(void (^)(void))block {
//    if ([NSThread isMainThread]) {
//        block();
//    } else {
//        dispatch_sync(dispatch_get_main_queue(), block);
//    }
//}
//

//#pragma mark ----------- UITableView Delegate Methods --------------

// Table View Delegate Methods Implementation.
- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView{
    return [_sectionsArray count];
}

- (nullable NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section{
    return [self.sectionsArray objectAtIndex:section];
}

-(UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section
{
    UILabel *label;
    UIView *view;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        label = [[UILabel alloc] initWithFrame:CGRectMake(5, 0, tableView.frame.size.width, 22)];
        
        view = [[UIView alloc] initWithFrame:CGRectMake(0, 5, tableView.frame.size.width, 10)];
        [label setFont:[UIFont systemFontOfSize:18]];
    }
    else{
        label = [[UILabel alloc] initWithFrame:CGRectMake(5, 5, tableView.frame.size.width, 22)];
        
        view = [[UIView alloc] initWithFrame:CGRectMake(0, 5, tableView.frame.size.width, 10)];
        [label setFont:[UIFont systemFontOfSize:20]];
    }
    /* Create custom view to display section header... */
    
    NSString *string = [self.sectionsArray objectAtIndex:section];
    /* Section header is in 0th index... */
    [label setText:string];
    [label setTextColor:[UIColor blackColor]];
    [view addSubview:label];
    [view setBackgroundColor:[UIColor whiteColor]]; //your background color...
    return view;
}

-(CGFloat)tableView:(UITableView *)tableView heightForFooterInSection:(NSInteger)section{
    return 0.01f;
}

-(CGFloat)tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        return 20.01f;
    }
    else{
        return 28.01f;
    }
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section{
    
    if (section==0) {
        return [self.dashboardScreensArray count];
    }
    else if (section==1){
        return [self.screensArray count];
    }
    else if (section==2){
        return [self.dailyWorkoutScreensArray count];
    }
    else if (section==3){
        return [self.userProfileScreensArray count];
    }
    else{
        return [self.settingScreensArray count];
    }
}

- (void)tableView:(UITableView *)tableView willDisplayCell:(UITableViewCell *)cell forRowAtIndexPath:(NSIndexPath *)indexPath{
    cell.textLabel.textColor=[UIColor blackColor];
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        return 40.0f;
    }
    else{
        return 50.0f;
    }
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *CellIdentifier = @"Cell";
    
    if (indexPath.section==0) {
        CellIdentifier = @"dashboard";
    }
    else if (indexPath.section==1){
        switch (indexPath.row){
            case 0:
                CellIdentifier = @"allergies";
                break;
            
            case 1:
                CellIdentifier = @"healthcondition";
                break;
                
            case 2:
                CellIdentifier = @"immunization";
                break;
                
            case 3:
                CellIdentifier = @"procedures";
                break;
                
            case 4:
                CellIdentifier = @"labstests";
                break;
                
            case 5:
                CellIdentifier = @"prescription";
                break;
                
            case 6:
                CellIdentifier = @"medication";
                break;
        }
    }
    else if (indexPath.section==2){
        switch ( indexPath.row ){
            case 0:
                CellIdentifier = @"activity";
                break;
                
            case 1:
                CellIdentifier = @"bloodpressure";
                break;
                
            case 2:
                CellIdentifier = @"bloodglucose";
                break;
                
            case 3:
                CellIdentifier = @"weight";
                break;
        }
    }
    else if (indexPath.section==3){
        switch ( indexPath.row ){
            case 0:
                CellIdentifier = @"personalInfo";
                break;
                
            case 1:
                CellIdentifier = @"emergencyInfo";
                break;
                
            case 2:
                CellIdentifier = @"employerInfo";
                break;
                
            case 3:
                CellIdentifier = @"insuranceInfo";
                break;
                
            case 4:
                CellIdentifier = @"preferenceInfo";
                break;
        }
    }
    else if (indexPath.section==4){
        switch ( indexPath.row ){
//            case 0:
//                CellIdentifier = @"configure";
//                break;
            case 0:
                CellIdentifier = @"medicalcontacts";
                break;
            case 1:
                CellIdentifier = @"sharing";
                break;
            case 2:
                CellIdentifier = @"aboutus";
                break;
            case 3:
                CellIdentifier = @"logout";
                break;
        }
    }
    
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier: CellIdentifier forIndexPath: indexPath];
    
//    cell.textLabel.text = [self.screensArray objectAtIndex:indexPath.row];
    
    NSDictionary *attrs;
    if (![[kAppDelegate checkDeviceType]isEqualToString:iPad]){
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone5] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone4]) {
            attrs = @{
                      NSFontAttributeName:[UIFont systemFontOfSize:20 weight:-1],
                      NSForegroundColorAttributeName:[UIColor lightGrayColor],
                      //                            NSForegroundColorAttributeName:[UIFont fontWithName:@"HelveticaNeue" size:30],
                      };
        }
        else if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone6Plus]) {
            attrs = @{
                      NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1],
                      NSForegroundColorAttributeName:[UIColor lightGrayColor],
                      //                            NSForegroundColorAttributeName:[UIFont fontWithName:@"HelveticaNeue" size:30],
                      };
        }
        
    }
    else{
        attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1],
                                NSForegroundColorAttributeName:[UIColor blackColor],
                                //                            NSForegroundColorAttributeName:[UIFont fontWithName:@"HelveticaNeue" size:30],
                                };
    }
    
    //[attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    NSMutableAttributedString *attributedText;
    UIImage* cellImage;
    switch (indexPath.section) {
        case 0:
            attributedText =
            [[NSMutableAttributedString alloc] initWithString:[self.dashboardScreensArray objectAtIndex:indexPath.row] attributes:attrs];
            cellImage = [UIImage imageNamed:[self.dashboardScreenImagesArray objectAtIndex:indexPath.row]];
            break;
            
        case 1:
            attributedText =
            [[NSMutableAttributedString alloc] initWithString:[self.screensArray objectAtIndex:indexPath.row] attributes:attrs];
            cellImage = [UIImage imageNamed:[self.screenImagesArray objectAtIndex:indexPath.row]];
            break;
            
        case 2:
            attributedText =
            [[NSMutableAttributedString alloc] initWithString:[self.dailyWorkoutScreensArray objectAtIndex:indexPath.row] attributes:attrs];
            cellImage = [UIImage imageNamed:[self.dailyWorkoutScreenImagesArray objectAtIndex:indexPath.row]];
            break;
            
        case 3:
            attributedText =
            [[NSMutableAttributedString alloc] initWithString:[self.userProfileScreensArray objectAtIndex:indexPath.row] attributes:attrs];
            cellImage = [UIImage imageNamed:[self.userProfileScreenImagesArray objectAtIndex:indexPath.row]];
            break;
        case 4:
            attributedText =
            [[NSMutableAttributedString alloc] initWithString:[self.settingScreensArray objectAtIndex:indexPath.row] attributes:attrs];
            cellImage = [UIImage imageNamed:[self.settingScreenImagesArray objectAtIndex:indexPath.row]];
            break;
        default:
            break;
    }
    cell.textLabel.attributedText = attributedText;
    cell.imageView.image = cellImage;
    tableView.separatorColor = [UIColor orangeColor];
    tableView.separatorInset = UIEdgeInsetsZero;
    tableView.separatorStyle = UITableViewCellSeparatorStyleSingleLine;
    
    [cell setSelectionStyle:UITableViewCellSelectionStyleDefault];
    return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
    
    if (indexPath.section==4) {
        if (indexPath.row==3) {
            [self.navigationController popViewControllerAnimated:YES];
            //        [[NSNotificationCenter defaultCenter] postNotificationName:@"showLoginView" object:nil];
            
            [self showAlertView];
        }
    }
}

-(void)showAlertView{
    self.logoutAlertView=[[UIAlertView alloc]initWithTitle:kAppTitle message:@"Are you sure you want to logout?" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"OK", nil];
    [self.logoutAlertView show];
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
    if (buttonIndex==1) {
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
}

#pragma mark Unregister FCM Token For Notifications 
-(void)unregisterFCMTokenForNotification{
    if ([kAppDelegate hasInternetConnection]) {
        
        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"userID"];
        
        NSString* strToken = [[FIRInstanceID instanceID] token];
        
        [dicParams setObject:strToken forKey:@"tokenID"];
        [dicParams setObject:@"3" forKey:@"SourceID"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your API url"];//Url
        
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

//-(IBAction)profileButtonAction:(id)sender{
//    [kAppDelegate setPresentProfileIdentity:@"UserProfile"];
//
//}

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
