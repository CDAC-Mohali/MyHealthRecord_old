//
//  ActivityViewController.m
//  PHR
//
//  Created by CDAC HIED on 22/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "ActivityViewController.h"
#import "AddActivityViewController.h"
#import "ActivityDetailsViewController.h"
#import "SWRevealViewController.h"
#import "ActivityTableViewCell.h"
#import "Constants.h"

@interface ActivityViewController (){
    SWRevealViewController *revealController;
    int rowIndexPath;
}

@property (weak, nonatomic) IBOutlet UITableView *activityTableView;

@end

@implementation ActivityViewController
@synthesize isFromDashboard, activityArray;

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    self.activityArray = [NSMutableArray new];
    
    _activityTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    
    NSDictionary *attrs;
    UIFont * font;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1]
                  };
        
        font = [UIFont systemFontOfSize:36.0f weight:-1];
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
        
        font = [UIFont systemFontOfSize:46.0f weight:-1];
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Activites"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"+" style:UIBarButtonItemStylePlain target:self action:@selector(addActivityController)];
//    UIFont * font = [UIFont systemFontOfSize:46.0f weight:-1];
    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
    if (!isFromDashboard) {
        //Set Left Bar Button Item
        [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
            [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
            [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
            self.navigationItem.leftBarButtonItem=barItem;
        
        }];
    }
    
    UIRefreshControl *refreshControl = [[UIRefreshControl alloc] init];
    [refreshControl addTarget:self action:@selector(refresh:) forControlEvents:UIControlEventValueChanged];
    [self.activityTableView addSubview:refreshControl];
    refreshControl.backgroundColor = [UIColor lightGrayColor];
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
    [self getActivityDataAPI];
}
#pragma mark Pull To Refresh Controller 
- (void)refresh:(UIRefreshControl *)refreshControl {
    
    [refreshControl endRefreshing];
    
//    099[activityArray removeAllObjects];
    
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MMM d, h:mm a"];
    NSString *title = [NSString stringWithFormat:@"Last updated: %@", [formatter stringFromDate:[NSDate date]]];
    NSDictionary *attrsDictionary = [NSDictionary dictionaryWithObject:[UIColor whiteColor] forKey:NSForegroundColorAttributeName];
    
    NSAttributedString *attributedTitle = [[NSAttributedString alloc] initWithString:title attributes:attrsDictionary];
    refreshControl.attributedTitle = attributedTitle;
    
    [self getActivityDataAPI];
}

-(void)getActivityDataAPI{
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
                [self.activityArray removeAllObjects];
                for (int i=0; i< [[responseObject valueForKey:@"response"] count];i++) {
                    int sourceID = [[[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"SourceId"]intValue];
                    if (sourceID!=2 && sourceID!=5) {
                        [self.activityArray addObject:[[responseObject valueForKey:@"response"] objectAtIndex:i]];
                    }
                }
                if ([self.activityArray count]==0) {
                    [kAppDelegate showAlertView:@"No activity value exists!!"];
                }
//                self.activityArray = [responseObject valueForKey:@"response"];
                [_activityTableView reloadData];
            }
            else{
                [kAppDelegate showAlertView:@"No activity value exists!!"];
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

-(void)addActivityController{
    
    AddActivityViewController* obj =[[AddActivityViewController alloc]initWithNibName:@"AddActivityViewController" bundle:nil];
    [self presentViewController:obj animated:YES completion:nil];
}

#pragma mark - UITableView Datasource

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    return [activityArray count];
}

-(UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section {
    
//    NSDictionary *attrs;
//    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
//        attrs = @{
//                  NSFontAttributeName:[UIFont systemFontOfSize:16 weight:-1]
//                  };
//    }
//    else{
//        attrs = @{
//                  NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1]
//                  };
//    }
//    NSMutableAttributedString *attributedText =
//    [[NSMutableAttributedString alloc] initWithString:[NSString stringWithFormat:@"%@ Activities Listing", [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]
//                                           attributes:attrs];
    
    UIView* view = [[UIView alloc]initWithFrame:CGRectMake(0, 0, [[UIScreen mainScreen] bounds].size.width, 30)];
    view.backgroundColor = [UIColor colorWithRed:135.0/255.0f green:206.0/255.0f blue:250.0/255.0f alpha:1.0];
    
    CGRect rect;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone5]) {
        
        UIImageView* userImage = [[UIImageView alloc]initWithFrame:CGRectMake(0, 2, 20, 20)];
        userImage.image = [UIImage imageNamed:@"user-enter"];
        userImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:userImage];
        
        UILabel *userLabel = [[UILabel alloc]initWithFrame:CGRectMake(20, 5, 100, 15)];
        //    titleLabel.attributedText = attributedText;
        userLabel.text = @"- User Entered";
        [userLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        //    titleLabel.backgroundColor = [UIColor colorWithRed:135.0/255.0f green:206.0/255.0f blue:250.0/255.0f alpha:1.0];
        userLabel.textColor = [UIColor whiteColor];
        [view addSubview:userLabel];
        
        UIImageView* doctorImage = [[UIImageView alloc]initWithFrame:CGRectMake(90, 3, 16, 16)];
        doctorImage.image = [UIImage imageNamed:@"doctor-enter"];
        doctorImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:doctorImage];
        
        UILabel *doctorLabel = [[UILabel alloc]initWithFrame:CGRectMake(108, 5, 100, 15)];
        doctorLabel.text = @"- Doctor Entered";
        [doctorLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        doctorLabel.textColor = [UIColor whiteColor];
        [view addSubview:doctorLabel];
        
        UIImageView* iPhoneImage = [[UIImageView alloc]initWithFrame:CGRectMake(190, 3, 16, 16)];
        iPhoneImage.image = [UIImage imageNamed:@"iOS"];
        iPhoneImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:iPhoneImage];
        
        UILabel *iPhoneLabel = [[UILabel alloc]initWithFrame:CGRectMake(207, 5, 150, 15)];
        iPhoneLabel.text = @"- iOS App";
        [iPhoneLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        iPhoneLabel.textColor = [UIColor whiteColor];
        [view addSubview:iPhoneLabel];
        
        UIImageView* androidImage = [[UIImageView alloc]initWithFrame:CGRectMake(255, 3, 16, 16)];
        androidImage.image = [UIImage imageNamed:@"android"];
        androidImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:androidImage];
        
        UILabel *androidLabel = [[UILabel alloc]initWithFrame:CGRectMake(275, 5, 150, 15)];
        androidLabel.text = @"- Android App";
        [androidLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        androidLabel.textColor = [UIColor whiteColor];
        [view addSubview:androidLabel];
    }
    else{
        rect = CGRectMake(210, 3, 16, 16);
        
        UIImageView* userImage = [[UIImageView alloc]initWithFrame:CGRectMake(10, 2, 20, 20)];
        userImage.image = [UIImage imageNamed:@"user-enter"];
        userImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:userImage];
        
        UILabel *userLabel = [[UILabel alloc]initWithFrame:CGRectMake(30, 5, 100, 15)];
        //    titleLabel.attributedText = attributedText;
        userLabel.text = @"- User Entered";
        [userLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        //    titleLabel.backgroundColor = [UIColor colorWithRed:135.0/255.0f green:206.0/255.0f blue:250.0/255.0f alpha:1.0];
        userLabel.textColor = [UIColor whiteColor];
        [view addSubview:userLabel];
        
        UIImageView* doctorImage = [[UIImageView alloc]initWithFrame:CGRectMake(110, 3, 16, 16)];
        doctorImage.image = [UIImage imageNamed:@"doctor-enter"];
        doctorImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:doctorImage];
        
        UILabel *doctorLabel = [[UILabel alloc]initWithFrame:CGRectMake(130, 5, 100, 15)];
        doctorLabel.text = @"- Doctor Entered";
        [doctorLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        doctorLabel.textColor = [UIColor whiteColor];
        [view addSubview:doctorLabel];
        
        UIImageView* iPhoneImage = [[UIImageView alloc]initWithFrame:CGRectMake(210, 3, 16, 16)];
        iPhoneImage.image = [UIImage imageNamed:@"iOS"];
        iPhoneImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:iPhoneImage];
        
        UILabel *iPhoneLabel = [[UILabel alloc]initWithFrame:CGRectMake(230, 5, 150, 15)];
        iPhoneLabel.text = @"- iOS App";
        [iPhoneLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        iPhoneLabel.textColor = [UIColor whiteColor];
        [view addSubview:iPhoneLabel];
        
        UIImageView* androidImage = [[UIImageView alloc]initWithFrame:CGRectMake(280, 3, 16, 16)];
        androidImage.image = [UIImage imageNamed:@"android"];
        androidImage.contentMode = UIViewContentModeScaleAspectFit;
        [view addSubview:androidImage];
        
        UILabel *androidLabel = [[UILabel alloc]initWithFrame:CGRectMake(300, 5, 150, 15)];
        androidLabel.text = @"- Android App";
        [androidLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
        androidLabel.textColor = [UIColor whiteColor];
        [view addSubview:androidLabel];
    }
    
    return view;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *cellIdentifier = @"activityCellIdentifier";
    
    ActivityTableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if(cell == nil) {
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone4]){
            cell = [[[NSBundle mainBundle]loadNibNamed:@"ActivityTableViewCelliPhone" owner:nil options:nil] firstObject];
        }
        else{
            cell = [[[NSBundle mainBundle]loadNibNamed:@"ActivityTableViewCell" owner:nil options:nil] firstObject];
        }
    }
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPad]){
    cell.pathNameLabel.text = [[self.activityArray objectAtIndex:indexPath.row] valueForKey:@"PathName"];
    }
    
    cell.activityNameLabel.text = [[self.activityArray objectAtIndex:indexPath.row] valueForKey:@"ActivityName"];
    
    NSString* strDistance = [[[self.activityArray objectAtIndex:indexPath.row] valueForKey:@"Distance"] stringValue];
    if ([strDistance isEqualToString:@"<null>"] || [strDistance isEqualToString:@""]) {
        cell.distanceLabel.text = @"Distance(km): - ";
    }
    else{
        cell.distanceLabel.text = [NSString stringWithFormat:@"Distance: %@ km",strDistance];
    }
    
    NSString* strTime = [[self.activityArray objectAtIndex:indexPath.row] valueForKey:@"FinishTime"];
    if ([strTime isKindOfClass:[NSNull class]] || [strTime isEqualToString:@"<null>"] || [strTime isEqualToString:@""]) {
        cell.timeLabel.text = [NSString stringWithFormat:@"Time taken: -"];
    }
    else{
        cell.timeLabel.text = [NSString stringWithFormat:@"Time taken: %@ min",strTime];
    }
    
    cell.dateTimeLabel.text = [[self.activityArray objectAtIndex:indexPath.row] valueForKey:@"strCreatedDate"];
    
    int sourceID = [[[self.activityArray objectAtIndex:indexPath.row] valueForKey:@"SourceId"] intValue];
    if (sourceID == 2 || sourceID == 5) {
        cell.userImage.image = [UIImage imageNamed:@"doctor-enter"];
    }
    else if(sourceID == 3){
        cell.userImage.image = [UIImage imageNamed:@"iOS"];
    }
    else if(sourceID == 4){
        cell.userImage.image = [UIImage imageNamed:@"android"];
    }
    else{
        cell.userImage.image = [UIImage imageNamed:@"user-enter"];
    }
    cell.selectionStyle = UITableViewCellSelectionStyleBlue;
    cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
    
    cell.backgroundColor = [UIColor whiteColor];
    
    return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    
    rowIndexPath = (int)indexPath.row;
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
    
    [self performSegueWithIdentifier:@"activityDetailController" sender:self];
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        return 70.0f;
    }
    else{
        return 130.0f;
    }
}

- (CGFloat)tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section {
    return 23;
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    // Get the new view controller using [segue destinationViewController].
    // Pass the selected object to the new view controller.
    if ([[segue identifier] isEqualToString:@"activityDetailController"])
    {
        // Get reference to the destination view controller
        ActivityDetailsViewController *vc = [segue destinationViewController];
        
        [vc setActivitiesDataArray:[self.activityArray objectAtIndex:rowIndexPath]];
//        [vc setIndexNumber:rowIndexPath];
    }
}

@end
