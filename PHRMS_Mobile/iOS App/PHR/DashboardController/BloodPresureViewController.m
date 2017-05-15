//
//  BloodPresureViewController.m
//  PHR
//
//  Created by CDAC HIED on 22/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "BloodPresureViewController.h"
#import "AddBloodPresureViewController.h"
#import "BloodPresureTableViewCell.h"
#import "BloodPresureDetailViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"

@interface BloodPresureViewController (){
    SWRevealViewController *revealController;
    
    int rowIndexPath;
}

@property (weak, nonatomic) IBOutlet UITableView *bpTableView;

@end

@implementation BloodPresureViewController
@synthesize isFromDashboard, bloodPressureArray;

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view from its nib.
    
    self.bloodPressureArray = [NSMutableArray new];
    
    _bpTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    
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
    [[NSMutableAttributedString alloc] initWithString:@"Blood Pressure"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"+" style:UIBarButtonItemStylePlain target:self action:@selector(addBPController)];
    
//    UIFont * font = [UIFont systemFontOfSize:46.0f weight:-1];
    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
    rowIndexPath = 0;
    
    if (!isFromDashboard) {
        //Set Left Bar Button Item
        [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
            [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
            [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
            self.navigationItem.leftBarButtonItem=barItem;
            
//            [self getBPDataAPI];
        }];
    }
    
    UIRefreshControl *refreshControl = [[UIRefreshControl alloc] init];
    [refreshControl addTarget:self action:@selector(refresh:) forControlEvents:UIControlEventValueChanged];
    [self.bpTableView addSubview:refreshControl];
    refreshControl.backgroundColor = [UIColor lightGrayColor];
}

-(void)viewWillAppear:(BOOL)animated{
    [self getBPDataAPI];
}

#pragma mark Pull To Refresh Controller 
- (void)refresh:(UIRefreshControl *)refreshControl {
    
    [refreshControl endRefreshing];
    
    [bloodPressureArray removeAllObjects];
    
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MMM d, h:mm a"];
    NSString *title = [NSString stringWithFormat:@"Last updated: %@", [formatter stringFromDate:[NSDate date]]];
    NSDictionary *attrsDictionary = [NSDictionary dictionaryWithObject:[UIColor whiteColor] forKey:NSForegroundColorAttributeName];
    
    NSAttributedString *attributedTitle = [[NSAttributedString alloc] initWithString:title attributes:attrsDictionary];
    refreshControl.attributedTitle = attributedTitle;
    
    [self getBPDataAPI];
}

-(void)getBPDataAPI{
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
                [self.bloodPressureArray removeAllObjects];
                for (int i=0; i< [[responseObject valueForKey:@"response"] count];i++) {
                    int sourceID = [[[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"SourceId"]intValue];
                    if (sourceID!=2 && sourceID!=5) {
                        [self.bloodPressureArray addObject:[[responseObject valueForKey:@"response"] objectAtIndex:i]];
                    }
                }
                if ([self.bloodPressureArray count]==0) {
                    [kAppDelegate showAlertView:@"No blood pressure values exists!!"];
                }
//                self.bloodPressureArray = [responseObject valueForKey:@"response"];
                [_bpTableView reloadData];
            }
            else{
                [kAppDelegate showAlertView:@"No blood pressure values exists!!"];
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

-(void)addBPController{
    
    AddBloodPresureViewController* obj =[[AddBloodPresureViewController alloc]initWithNibName:@"AddBloodPresureViewController" bundle:nil];
    [self presentViewController:obj animated:YES completion:nil];
}

#pragma mark - UITableView Datasource

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    return [self.bloodPressureArray count];
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
//    [[NSMutableAttributedString alloc] initWithString:[NSString stringWithFormat:@"%@ Blood Pressure Listing", [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]
//                                           attributes:attrs];
    
    UIView* view = [[UIView alloc]initWithFrame:CGRectMake(0, 0, [[UIScreen mainScreen] bounds].size.width, 30)];
    view.backgroundColor = [UIColor colorWithRed:135.0/255.0f green:206.0/255.0f blue:250.0/255.0f alpha:1.0];
    
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
    
    return view;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *cellIdentifier = @"bpCellIdentifier";
    
    BloodPresureTableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if(cell == nil) {
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
            cell = [[[NSBundle mainBundle]loadNibNamed:@"BloodPressureTableViewCelliPhone" owner:nil options:nil] firstObject];
        }
        else{
            cell = [[[NSBundle mainBundle]loadNibNamed:@"BloodPresureTableViewCell" owner:nil options:nil] firstObject];
        }
    }
    cell.DiaSysValuesLabel.text = [NSString stringWithFormat:@"%@/%@",[[self.bloodPressureArray objectAtIndex:indexPath.row] valueForKey:@"ResSystolic"], [[self.bloodPressureArray objectAtIndex:indexPath.row] valueForKey:@"ResDiastolic"]];
    cell.pulseValueLabel.text = [[self.bloodPressureArray objectAtIndex:indexPath.row] valueForKey:@"ResPulse"];
    cell.commentsLabel.text = [[[self.bloodPressureArray objectAtIndex:indexPath.row] valueForKey:@"Comments"] capitalizedString];
    cell.dateTimeLabel.text = [[self.bloodPressureArray objectAtIndex:indexPath.row] valueForKey:@"strCreatedDate"];
    
    int sourceID = [[[self.bloodPressureArray objectAtIndex:indexPath.row] valueForKey:@"SourceId"] intValue];
    if (sourceID==2 || sourceID==5) {
        cell.userImage.image = [UIImage imageNamed:@"doctor-enter"];
    }
    else{
        cell.userImage.image = [UIImage imageNamed:@"user-enter"];
    }
    cell.selectionStyle = UITableViewCellSelectionStyleBlue;
    cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
    
    cell.backgroundColor = [UIColor whiteColor];
//    cell.backgroundColor = [UIColor colorWithRed:44.0f/255.0f green:48.0f/255.0f blue:75.0f/255.0f alpha:1.0f];
    
    return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    
    rowIndexPath = (int)indexPath.row;
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
    
    [self performSegueWithIdentifier:@"bloodPressureDetailController" sender:self];
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        return 80.0f;
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
    if ([[segue identifier] isEqualToString:@"bloodPressureDetailController"])
    {
        // Get reference to the destination view controller
        BloodPresureDetailViewController *vc = [segue destinationViewController];
        
        [vc setBpDataArray:[self.bloodPressureArray objectAtIndex:rowIndexPath]];
//        [vc setIndexNumber:rowIndexPath];
    }
}

@end
