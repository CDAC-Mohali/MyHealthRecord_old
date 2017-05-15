//
//  ImmunzationNameTableViewController.m
//  PHR
//
//  Created by CDAC HIED on 30/03/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "ImmunzationNameTableViewController.h"
#import "ImmunzationNameTableViewCell.h"
#import "Constants.h"

@interface ImmunzationNameTableViewController (){
    NSArray *searchResults;
    BOOL resultNil;
}
@property (weak, nonatomic) IBOutlet UISearchBar *searchBarField;
- (IBAction)backButtonAction:(id)sender;

@end

@implementation ImmunzationNameTableViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    searchResults = [[NSArray alloc] init];
    
    _immunizationTableView.tableFooterView = [[UIView alloc] initWithFrame:CGRectZero];
    
    // Uncomment the following line to preserve selection between presentations.
    // self.clearsSelectionOnViewWillAppear = NO;
    
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    [self prefersStatusBarHidden];
//    [[UIApplication sharedApplication] setStatusBarHidden:YES];
}

-(BOOL)prefersStatusBarHidden{
    return YES;
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - Table view data source

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
    return 1;
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath{
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        return 50.0f;
    }
    else{
        return 80.0f;
    }
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    if ([searchResults count]) {
        return [searchResults count];
        
    } else if(!resultNil){
        return [self.immunzationNameArray count];
    }
    else {
        return 1;
    }
}

//-(UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section
//{
////     [tableView setTableHeaderView:self.searchBarField];
//    return self.searchBarField;
//}

//-(void)scrollViewDidScroll:(UIScrollView *)scrollView
//{
//    UISearchBar *searchBar = self.searchDisplayController.searchBar;
//    CGRect rect = searchBar.frame;
//    rect.origin.y = MIN(0, scrollView.contentOffset.y);
//    searchBar.frame = rect;
//}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *cellIdentifier = @"ImmunizationCellIdentifier";
    
    ImmunzationNameTableViewCell *cell = (ImmunzationNameTableViewCell *)[tableView dequeueReusableCellWithIdentifier:cellIdentifier];
    
    if (cell == nil) {
        NSArray* nib = [[NSBundle mainBundle] loadNibNamed:@"ImmunzationNameTableViewCell" owner:self options:nil];
        cell = [nib objectAtIndex:0];
    }
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]){
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:20 weight:-1]
                  };
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText;
    
    if ([searchResults count]) {
        
        attributedText = [[NSMutableAttributedString alloc] initWithString:[[[searchResults objectAtIndex:indexPath.row] valueForKey:@"ImmunizationName"] capitalizedString]
                                                                attributes:attrs];
        
    } else if(!resultNil) {
        attributedText = [[NSMutableAttributedString alloc] initWithString:[[[self.immunzationNameArray objectAtIndex:indexPath.row] valueForKey:@"ImmunizationName"] capitalizedString]
                                                                attributes:attrs];
    }else {
        cell.textLabel.text = @"no result found";
        return cell;
    }
    
    cell.textLabel.attributedText = attributedText;
    
    cell.selectionStyle = UITableViewCellSelectionStyleBlue;
    //    cell.accessoryType = UITableViewCellAccessoryDisclosureIndicator;
    cell.backgroundColor = [UIColor lightGrayColor];
    
    return cell;
}

#pragma mark - Table view delegate

// In a xib-based application, navigation from a table can be handled in -tableView:didSelectRowAtIndexPath:
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    if ([searchResults count]) {
        [kAppDelegate setImmunizationNameButtonString:[[searchResults objectAtIndex:indexPath.row] valueForKey:@"ImmunizationName"]];
        [kAppDelegate setImmunizationNameID:[[searchResults objectAtIndex:indexPath.row] valueForKey:@"ImmunizationsTypeId"]];
    }
    else if(!resultNil){
        [kAppDelegate setImmunizationNameButtonString:[[self.immunzationNameArray objectAtIndex:indexPath.row] valueForKey:@"ImmunizationName"]];
        [kAppDelegate setImmunizationNameID:[[self.immunzationNameArray objectAtIndex:indexPath.row] valueForKey:@"ImmunizationsTypeId"]];
    }
    else{
        return;
    }
    
    [self dismissViewControllerAnimated:YES completion:nil];
}

- (void)scrollViewDidScroll:(UIScrollView *)scrollView{
    [self.searchBar endEditing:YES];
}

- (void)searchBar:(UISearchBar *)searchBar textDidChange:(NSString *)searchText{
    if (searchText.length >=2) {
        [self searchImmunization:searchText];
    }
    else if (searchText.length == 0){
        
        searchResults = NULL;
        dispatch_async(dispatch_get_main_queue(), ^{
            [kAppDelegate hideLoadingIndicator];
            [self.immunizationTableView reloadData];
        });

    }
}

-(void)searchImmunization:(NSString*)searchText{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"getting..."];
        
        NSURLSessionConfiguration *sessionConfiguration = [NSURLSessionConfiguration defaultSessionConfiguration];
        sessionConfiguration.HTTPAdditionalHeaders = @{
                                                       @"api-key"       : @"API_KEY",
                                                       @"Content-Type"  : @"application/json"
                                                       };
        NSURLSession *session = [NSURLSession sessionWithConfiguration:sessionConfiguration delegate:self delegateQueue:nil];
        
        NSString *searchString = [NSString stringWithFormat:@"\"%@\"",searchText];
        
        NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"enter your web API url"]];
        NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
        request.HTTPBody = [searchString dataUsingEncoding:NSUTF8StringEncoding];
        request.HTTPMethod = @"POST";
        NSURLSessionDataTask *postDataTask = [session dataTaskWithRequest:request completionHandler:^(NSData *data, NSURLResponse *response, NSError *error) {
            //            [kAppDelegate hideLoadingIndicator];
            
            if (!error) {
                
                id json = [NSJSONSerialization JSONObjectWithData:data options:0 error:nil];
                //                NSLog(@"response is %@",json);
                searchResults = NULL;
                searchResults = json;
                if (![searchResults count]) {
                    resultNil = YES;
                }
                else{
                    resultNil = NO;
                }
                
                dispatch_async(dispatch_get_main_queue(), ^{
                    [kAppDelegate hideLoadingIndicator];
                    [self.immunizationTableView reloadData];
                });
            }
        }];
        
        [postDataTask resume];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
}

- (void)searchBarCancelButtonClicked:(UISearchBar *)searchBar{
    self.searchBar.text = @"";
    [self.searchBar resignFirstResponder];
    
    searchResults = NULL;
    resultNil = NO;
    
    dispatch_async(dispatch_get_main_queue(), ^{
        [kAppDelegate hideLoadingIndicator];
        [self.immunizationTableView reloadData];
    });

//    [self dismissViewControllerAnimated:YES completion:nil];
    
//    dispatch_async(dispatch_get_main_queue(), ^{
//        [self.immunizationTableView reloadData];
//    });
}

/*
#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    // Get the new view controller using [segue destinationViewController].
    // Pass the selected object to the new view controller.
}
*/

- (IBAction)backButtonAction:(id)sender {
    
    [self dismissViewControllerAnimated:YES completion:nil];
}
@end
