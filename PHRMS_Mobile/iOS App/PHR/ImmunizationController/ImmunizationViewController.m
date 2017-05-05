//
//  ImmunizationViewController.m
//  PHR
//
//  Created by CDAC HIED on 16/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "ImmunizationViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"
#import "ImmunizationTableViewCell.h"
#import "AddImmunizationViewController.h"
#import "ImmunizationDetailViewController.h"

@interface ImmunizationViewController (){
    SWRevealViewController *revealController;
    NSMutableArray* immunizationArray;
    int rowIndexPath;
}
@property (weak, nonatomic) IBOutlet UITableView *immunizationTableView;

@end

@implementation ImmunizationViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    _immunizationTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    
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
    [[NSMutableAttributedString alloc] initWithString:@"Immunizations"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"+" style:UIBarButtonItemStylePlain target:self action:@selector(addImmunizationController)];
    
    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
    
//    [kAppDelegate setComeFromInsertion:YES];
    
    UIRefreshControl *refreshControl = [[UIRefreshControl alloc] init];
    [refreshControl addTarget:self action:@selector(refresh:) forControlEvents:UIControlEventValueChanged];
    [self.immunizationTableView addSubview:refreshControl];
    refreshControl.backgroundColor = [UIColor lightGrayColor];
    
    immunizationArray = [NSMutableArray new];
}

-(void)viewWillAppear:(BOOL)animated{
    [self getImmunizations];
//    [self.immunizationTableView reloadSections:1 withRowAnimation:UITableViewRowAnimationFade];
}

#pragma mark Pull To Refresh Controller 
- (void)refresh:(UIRefreshControl *)refreshControl {
    
    [refreshControl endRefreshing];
    
    [immunizationArray removeAllObjects];
    
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"MMM d, h:mm a"];
    NSString *title = [NSString stringWithFormat:@"Last updated: %@", [formatter stringFromDate:[NSDate date]]];
    NSDictionary *attrsDictionary = [NSDictionary dictionaryWithObject:[UIColor whiteColor] forKey:NSForegroundColorAttributeName];
    
    NSAttributedString *attributedTitle = [[NSAttributedString alloc] initWithString:title attributes:attrsDictionary];
    refreshControl.attributedTitle = attributedTitle;
    
    [self getImmunizations];
}

#pragma mark Get Immunization API Call
-(void)getImmunizations{
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
                [immunizationArray removeAllObjects];
                for (int i=0; i< [[responseObject valueForKey:@"response"] count];i++) {
                    int sourceID = [[[[responseObject valueForKey:@"response"] objectAtIndex:i] valueForKey:@"SourceId"]intValue];
                    if (sourceID!=2 && sourceID!=5) {
                        [immunizationArray addObject:[[responseObject valueForKey:@"response"] objectAtIndex:i]];
                    }
                }
                if ([immunizationArray count]==0) {
                    [kAppDelegate showAlertView:@"No immunization data exists!!"];
                }
//                immunizationArray = [responseObject valueForKey:@"response"];
                
//                NSSortDescriptor *sortDescriptor;
//                sortDescriptor = [NSSortDescriptor sortDescriptorWithKey:@"strModifiedDate" ascending:YES
//                                                             ];
//                NSArray *sortDescriptors = [NSArray arrayWithObject:sortDescriptor];
//                NSArray *sortedArray = [immunizationArray sortedArrayUsingDescriptors:sortDescriptors];
                
                [_immunizationTableView setDelegate:self];
                [_immunizationTableView setDataSource:self];
                [_immunizationTableView reloadData];
            }
            else{
                [kAppDelegate showAlertView:@"No immunization data exists!!"];
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

-(void)addImmunizationController{
    
    AddImmunizationViewController* obj =[[AddImmunizationViewController alloc]initWithNibName:@"AddImmunizationViewController" bundle:nil];
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
    return [immunizationArray count];
}

//- (BOOL)tableView:(UITableView *)tableView shouldHighlightRowAtIndexPath:(NSIndexPath *)indexPath {
//    return YES;
//}
//
//- (void)tableView:(UITableView *)tableView didHighlightRowAtIndexPath:(NSIndexPath *)indexPath {
//    // do something here
//}

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
//    [[NSMutableAttributedString alloc] initWithString:[NSString stringWithFormat:@"%@ Immunization Listing", [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME]]
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
    
    static NSString *cellIdentifier = @"ImmunizationCellIdentifier";
    
    ImmunizationTableViewCell *cell = (ImmunizationTableViewCell *)[theTableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if (cell == nil) {
        NSArray* nib;
        if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
            nib = [[NSBundle mainBundle] loadNibNamed:@"ImmunizationTableViewCelliPhone" owner:self options:nil];
        }
        else{
            nib = [[NSBundle mainBundle] loadNibNamed:@"ImmunizationTableViewCell" owner:self options:nil];
        }
        cell = [nib objectAtIndex:0];
    }
    
    cell.immunizationNameLabel.text = [[[immunizationArray objectAtIndex:indexPath.row] valueForKey:@"ImmunizationName"] capitalizedString];
    
    NSString* comments = [[immunizationArray objectAtIndex:indexPath.row] valueForKey:@"Comments"];
    if ([comments isEqualToString:@""]) {
        cell.commentLabel.text = @"Comments: -";
    }
    else{
        cell.commentLabel.text = [NSString stringWithFormat:@"Comments:%@",comments];
    }
    
    cell.dateTimeLabel.text = [[immunizationArray objectAtIndex:indexPath.row] valueForKey:@"strCreatedDate"];
    
    /*if (indexPath.row==0 && [kAppDelegate comeFromInsertion]) {
        
        cell.contentView.backgroundColor = [UIColor colorWithRed:255.0/255.0 green:203.0/255.0 blue:20.0/255.0 alpha:0.75];
//        cell.accessoryView.backgroundColor = [UIColor colorWithRed:255.0/255.0 green:203.0/255.0 blue:20.0/255.0 alpha:0.75];
//        cell.accessoryView.opaque = false;

//        [UIView animateWithDuration:3.0 animations:^{
//            cell.contentView.backgroundColor = [UIColor colorWithRed:253.0/255.0 green:139.0/255.0 blue:53.0/255.0 alpha:1.0];
//        } completion:^(BOOL finished) {
//            [UIView animateWithDuration:2.0 animations:^{
//                cell.contentView.backgroundColor = [UIColor colorWithRed:253.0/255.0 green:139.0/255.0 blue:53.0/255.0 alpha:0.0];
//            }];
//        }];
        
        [UIView animateWithDuration:2.50 animations:^{
            cell.contentView.backgroundColor = [UIColor colorWithRed:255.0/255.0 green:203.0/255.0 blue:20.0/255.0 alpha:0.0];
            cell.editingAccessoryView.backgroundColor = [UIColor colorWithRed:255.0/255.0 green:203.0/255.0 blue:20.0/255.0 alpha:0.0];
        }];
    }*/
    
    int sourceID = [[[immunizationArray objectAtIndex:indexPath.row] valueForKey:@"SourceId"] intValue];
    if (sourceID==2 || sourceID==5) {
        cell.userImage.image = [UIImage imageNamed:@"doctor-enter"];
    }
    else{
        cell.userImage.image = [UIImage imageNamed:@"user-enter"];
    }

    cell.selectionStyle = UITableViewCellSelectionStyleBlue;
    cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
    cell.backgroundColor = [UIColor whiteColor];
    theTableView.separatorColor = [UIColor redColor];
    
//    [self.tableView reloadRowsAtIndexPaths:indArr withRowAnimation:UITableViewRowAnimationNone];
    
    return cell;
}

-(void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath{
    
//    [kAppDelegate setComeFromInsertion:NO];
    rowIndexPath = (int)indexPath.row;
    [self performSegueWithIdentifier:@"ImmunizationDetailController" sender:self];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    
    ImmunizationDetailViewController *vc = [segue destinationViewController];
    
    [vc setImmunizationDataArray:[immunizationArray objectAtIndex:rowIndexPath]];
//    [vc setIndexNumber:rowIndexPath];
}

@end
