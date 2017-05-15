//
//  SurgeryViewController.m
//  PHR
//
//  Created by CDAC HIED on 16/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "PrescriptionViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"
#import "PrescriptionTableViewCell.h"
#import "AddPrescriptionViewController.h"
#import "PrescriptionDetailViewController.h"

@interface PrescriptionViewController (){
    SWRevealViewController *revealController;
    NSMutableArray* prescriptionArray;
    int rowIndexPath;
}

@property (weak, nonatomic) IBOutlet UITableView *prescriptionTableView;

@end

@implementation PrescriptionViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    _prescriptionTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    
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
    [[NSMutableAttributedString alloc] initWithString:@"Prescriptions"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"+" style:UIBarButtonItemStylePlain target:self action:@selector(addPrescriptionController)];
    
//    UIFont * font = [UIFont systemFontOfSize:46.0f weight:-1];
    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
    UIRefreshControl *refreshControl = [[UIRefreshControl alloc] init];
    [refreshControl addTarget:self action:@selector(refresh:) forControlEvents:UIControlEventValueChanged];
    [self.prescriptionTableView addSubview:refreshControl];
    refreshControl.backgroundColor = [UIColor lightGrayColor];
    
    prescriptionArray = [NSMutableArray new];
}

-(void)viewWillAppear:(BOOL)animated{
    [self getPrescriptions];
}

#pragma mark Pull To Refresh Controller 
- (void)refresh:(UIRefreshControl *)refreshControl {
    
    [refreshControl endRefreshing];
    
//    [prescriptionArray removeAllObjects];
    prescriptionArray = nil;
    
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MMM d, h:mm a"];
    NSString *title = [NSString stringWithFormat:@"Last updated: %@", [formatter stringFromDate:[NSDate date]]];
    NSDictionary *attrsDictionary = [NSDictionary dictionaryWithObject:[UIColor whiteColor] forKey:NSForegroundColorAttributeName];
    
    NSAttributedString *attributedTitle = [[NSAttributedString alloc] initWithString:title attributes:attrsDictionary];
    refreshControl.attributedTitle = attributedTitle;
    
    [self getPrescriptions];
}

#pragma mark Get Allergy API Call
-(void)getPrescriptions{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Getting..."];//Show loading indicator.
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                prescriptionArray = [responseObject valueForKey:@"response"];
                [_prescriptionTableView setDelegate:self];
                [_prescriptionTableView setDataSource:self];
                [_prescriptionTableView reloadData];
            }
            else{
                [kAppDelegate showAlertView:@"No prescription exists!!"];
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

-(void)addPrescriptionController{
    
    AddPrescriptionViewController* obj;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        obj = [[AddPrescriptionViewController alloc]initWithNibName:@"AddPrescriptionViewControlleriPhone" bundle:nil];
    }
    else{
        obj = [[AddPrescriptionViewController alloc]initWithNibName:@"AddPrescriptionViewControlleriPad" bundle:nil];
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
- (NSInteger)tableView:(UITableView *)theTableView numberOfRowsInSection:(NSInteger)section
{
    return [prescriptionArray count];
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
//
//    NSMutableAttributedString *attributedText =
//    [[NSMutableAttributedString alloc] initWithString:[NSString stringWithFormat:@"%@ Prescription Listing", [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]
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
    
    UIImageView* medicalImage = [[UIImageView alloc]initWithFrame:CGRectMake(210, 3, 16, 16)];
    medicalImage.image = [UIImage imageNamed:@"medical-enter"];
    medicalImage.contentMode = UIViewContentModeScaleAspectFit;
    [view addSubview:medicalImage];
    
    UILabel *medicalLabel = [[UILabel alloc]initWithFrame:CGRectMake(230, 5, 150, 15)];
    medicalLabel.text = @"- Medical Contact Entered";
    [medicalLabel setFont:[UIFont fontWithName:@"HelveticaNeue" size:10.f]];
    medicalLabel.textColor = [UIColor whiteColor];
    [view addSubview:medicalLabel];
    
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
    
    static NSString *cellIdentifier = @"PrescriptionCellIdentifier";
    
    PrescriptionTableViewCell *cell = (PrescriptionTableViewCell *)[theTableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if (cell == nil) {
        NSArray* nib;
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
            nib = [[NSBundle mainBundle] loadNibNamed:@"PrescriptionTableViewCelliPhone" owner:self options:nil];
        }
        else{
            nib = [[NSBundle mainBundle] loadNibNamed:@"PrescriptionTableViewCell" owner:self options:nil];
        }
        cell = [nib objectAtIndex:0];
    }
    
    NSString* pres = [[prescriptionArray objectAtIndex:indexPath.row] valueForKey:@"ClinicName"];
    if ([pres isKindOfClass:[NSNull class]] || [pres isEqualToString:@""]) {
        cell.prescriptionDescriptionlabel.text = @" - ";
    }
    else{
        cell.prescriptionDescriptionlabel.text = [pres capitalizedString];
    }
    cell.doctorNameLabel.text = [[NSString stringWithFormat:@"Doctor Name: %@",[[prescriptionArray objectAtIndex:indexPath.row] valueForKey:@"DocName"]] capitalizedString];
    cell.clinicNameLabel.text = [[NSString stringWithFormat:@"Hospital/Clinic Name: %@",[[prescriptionArray objectAtIndex:indexPath.row] valueForKey:@"ClinicName"]] capitalizedString];
    cell.dateLabel.text = [[prescriptionArray objectAtIndex:indexPath.row] valueForKey:@"strCreatedDate"];
    
    cell.backgroundColor = [UIColor lightGrayColor];
    
    cell.selectionStyle = UITableViewCellSelectionStyleDefault;
    cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
    cell.backgroundColor = [UIColor whiteColor];
    theTableView.separatorColor = [UIColor orangeColor];
    
    int sourceID = [[[prescriptionArray objectAtIndex:indexPath.row] valueForKey:@"SourceId"] intValue];
    if (sourceID==2) {
        cell.userImage.image = [UIImage imageNamed:@"doctor-enter"];
    }
    else if (sourceID == 5){
        cell.userImage.image = [UIImage imageNamed:@"medical-enter"];
    }
    else{
        cell.userImage.image = [UIImage imageNamed:@"user-enter"];
    }
    
    return cell;
}

-(void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    
    rowIndexPath = (int)indexPath.row;
    [self performSegueWithIdentifier:@"PrescriptionDetailController" sender:self];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    //     Get the new view controller using [segue destinationViewController].
    //     Pass the selected object to the new view controller.
    if ([[segue identifier] isEqualToString:@"PrescriptionDetailController"])
    {
        // Get reference to the destination view controller
        PrescriptionDetailViewController *vc = [segue destinationViewController];
        
        [vc setPrescriptionDataArray:[prescriptionArray objectAtIndex:rowIndexPath]];
//        [vc setIndexNumber:rowIndexPath];
    }
    
}

@end
