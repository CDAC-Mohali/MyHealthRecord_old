//
//  MedicalContactsViewController.m
//  PHR
//
//  Created by CDAC HIED on 26/12/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "MedicalContactsViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"
#import "MedicalContactTableViewCell.h"
#import "AddMedicalContactViewController.h"

@interface MedicalContactsViewController (){
    SWRevealViewController *revealController;
    NSMutableArray* medicalContactsArray;
    int rowIndexPath;
}

@property (weak, nonatomic) IBOutlet UITableView *medicalContactsTableView;

@end

@implementation MedicalContactsViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
//    _medicalContactsTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    
    //Set Left Bar Button Item
    [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
        [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
        [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
        self.navigationItem.leftBarButtonItem=barItem;
    }];
    
    
    NSDictionary *attrs;
    UIFont * font;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
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
    [[NSMutableAttributedString alloc] initWithString:@"Medical Contacts"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"+" style:UIBarButtonItemStylePlain target:self action:@selector(addMedicalContactsController)];
    //    UIFont * font = [UIFont systemFontOfSize:46.0f weight:-1];
    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
    UIRefreshControl *refreshControl = [[UIRefreshControl alloc] init];
    [refreshControl addTarget:self action:@selector(refresh:) forControlEvents:UIControlEventValueChanged];
    [self.medicalContactsTableView addSubview:refreshControl];
    refreshControl.backgroundColor = [UIColor lightGrayColor];
    
    medicalContactsArray = [NSMutableArray new];
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
    [self getMedicalContactsDataAPI];
}

#pragma mark Pull To Refresh Controller 
- (void)refresh:(UIRefreshControl *)refreshControl {
    
    [refreshControl endRefreshing];
    
    [medicalContactsArray removeAllObjects];
    
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MMM d, h:mm a"];
    NSString *title = [NSString stringWithFormat:@"Last updated: %@", [formatter stringFromDate:[NSDate date]]];
    NSDictionary *attrsDictionary = [NSDictionary dictionaryWithObject:[UIColor whiteColor] forKey:NSForegroundColorAttributeName];
    
    NSAttributedString *attributedTitle = [[NSAttributedString alloc] initWithString:title attributes:attrsDictionary];
    refreshControl.attributedTitle = attributedTitle;
    
    [self getMedicalContactsDataAPI];
}

#pragma mark Get Medical Contact List
-(void)getMedicalContactsDataAPI{
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
            
            if ([[responseObject valueForKey:@"status"]intValue] == 1) {
                [medicalContactsArray removeAllObjects];
                medicalContactsArray = [[responseObject valueForKey:@"response"] mutableCopy];
                
                [_medicalContactsTableView reloadData];
            }
            else{
                [kAppDelegate showAlertView:@"No medical contacts record exists!!"];
            }
        }
            failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"failed"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

-(void)addMedicalContactsController{
    
    //[self performSegueWithIdentifier:@"MedicalContactsDetailController" sender:self];
    AddMedicalContactViewController* obj;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        obj = [[AddMedicalContactViewController alloc]initWithNibName:@"AddMedicalContactViewControlleriPhone" bundle:nil];
    }
    else{
        obj = [[AddMedicalContactViewController alloc]initWithNibName:@"AddMedicalContactViewControlleriPad" bundle:nil];
    }

    [self presentViewController:obj animated:YES completion:nil];
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

#pragma mark - UITableViewDelegates 
- (NSInteger)numberOfSectionsInTableView:(UITableView *)theTableView
{
    return 1;
}

// number of row in the section, I assume there is only 1 row
- (NSInteger)tableView:(UITableView *)theTableView numberOfRowsInSection:(NSInteger)section{
    return [medicalContactsArray count];
}

-(UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section {
    
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

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        return 70.0f;
    }
    else{
        return 100.0f;
    }
}

// the cell will be returned to the tableView
- (UITableViewCell *)tableView:(UITableView *)theTableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    
    static NSString *cellIdentifier = @"MedicalContactsCellIdentifier";
    
    MedicalContactTableViewCell *cell = (MedicalContactTableViewCell *)[theTableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if (cell == nil) {
        NSArray* nib;
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
            nib = [[NSBundle mainBundle] loadNibNamed:@"MedicalContactTableViewCelliPhone" owner:self options:nil];
        }
        else{
            nib = [[NSBundle mainBundle] loadNibNamed:@"MedicalContactTableViewCell" owner:self options:nil];
        }
        cell = [nib objectAtIndex:0];
    }
    
    cell.ContactNameText.text = [[[medicalContactsArray objectAtIndex:indexPath.row] valueForKey:@"ContactName"] capitalizedString];
    cell.specialityText.text = [[[medicalContactsArray objectAtIndex:indexPath.row] valueForKey:@"strMedContType"] capitalizedString];
    cell.dateTimeText.text = [[medicalContactsArray objectAtIndex:indexPath.row] valueForKey:@"strCreatedDate"];
    //cell.mobileText.text = [[medicalContactsArray objectAtIndex:indexPath.row] valueForKey:@"PrimaryPhone"];
//    
//    int sourceID = [[[medicalContactsArray objectAtIndex:indexPath.row] valueForKey:@"SourceId"] intValue];
//    if (sourceID==2 || sourceID==5) {
//        cell.userImage.image = [UIImage imageNamed:@"doctor-enter"];
//    }
//    else{
//        cell.userImage.image = [UIImage imageNamed:@"user-enter"];
//    }
    
    cell.selectionStyle = UITableViewCellSelectionStyleDefault;
    cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
    cell.backgroundColor = [UIColor whiteColor];
    theTableView.separatorColor = [UIColor greenColor];
    
    return cell;
}

-(void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    
    rowIndexPath = (int)indexPath.row;
    [tableView deselectRowAtIndexPath:indexPath animated:YES];
    [self performSegueWithIdentifier:@"MedicalContactsDetailController" sender:self];
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
    if ([[segue identifier] isEqualToString:@"MedicalContactsDetailController"])
    {
        // Get reference to the destination view controller
        MedicalContactDetailViewController *vc = [segue destinationViewController];
        
        [vc setStrRecordID:[[medicalContactsArray objectAtIndex:rowIndexPath]valueForKey:@"Id"]];
        //        [vc setIndexNumber:rowIndexPath];
    }
}

@end
