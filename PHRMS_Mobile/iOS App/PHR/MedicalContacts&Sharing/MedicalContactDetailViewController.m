//
//  MedicalContactDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 26/12/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "MedicalContactDetailViewController.h"
#import "Constants.h"

@interface MedicalContactDetailViewController ()

@property (weak, nonatomic) IBOutlet UITextField *contactNameText;
@property (weak, nonatomic) IBOutlet UILabel *contactTypeText;
@property (weak, nonatomic) IBOutlet UITextField *clinicNameText;
@property (weak, nonatomic) IBOutlet UITextField *address1Text;
@property (weak, nonatomic) IBOutlet UITextField *address2Text;
@property (weak, nonatomic) IBOutlet UITextField *cityText;
@property (weak, nonatomic) IBOutlet UITextField *stateText;
@property (weak, nonatomic) IBOutlet UITextField *districtText;
@property (weak, nonatomic) IBOutlet UITextField *pinText;
@property (weak, nonatomic) IBOutlet UITextField *mobileText;
@property (weak, nonatomic) IBOutlet UITextField *emailText;

@property (weak, nonatomic) IBOutlet UILabel *contactNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *contactTypeLabel;
@property (weak, nonatomic) IBOutlet UILabel *clinicNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *address1Label;
@property (weak, nonatomic) IBOutlet UILabel *address2Label;
@property (weak, nonatomic) IBOutlet UILabel *cityLabel;
@property (weak, nonatomic) IBOutlet UILabel *districtLabel;
@property (weak, nonatomic) IBOutlet UILabel *stateLabel;
@property (weak, nonatomic) IBOutlet UILabel *pinLabel;
@property (weak, nonatomic) IBOutlet UILabel *mobileLabel;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;

@property (weak, nonatomic) IBOutlet UIScrollView *medicalContactScrollView;

@end

@implementation MedicalContactDetailViewController
@synthesize strRecordID;

-(void)viewWillLayoutSubviews{
    
    [self.medicalContactScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+650)];
    
    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        self.contactNameText.frame = CGRectMake(self.contactNameText.frame.origin.x, self.contactNameText.frame.origin.y, 170, self.contactNameText.frame.size.height);
        self.contactTypeText.frame = CGRectMake(self.contactTypeText.frame.origin.x, self.contactTypeText.frame.origin.y, 170, self.contactTypeText.frame.size.height);
        self.clinicNameText.frame = CGRectMake(self.clinicNameText.frame.origin.x, self.clinicNameText.frame.origin.y, 170, self.clinicNameText.frame.size.height);
        self.address1Text.frame = CGRectMake(self.address1Text.frame.origin.x, self.address1Text.frame.origin.y, 170, self.address1Text.frame.size.height);
        self.address2Text.frame = CGRectMake(self.address2Text.frame.origin.x, self.address2Text.frame.origin.y, 170, self.address2Text.frame.size.height);
        self.cityText.frame = CGRectMake(self.cityText.frame.origin.x, self.cityText.frame.origin.y, 170, self.cityText.frame.size.height);
        self.districtText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.mobileText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailText.font = [UIFont systemFontOfSize:16.0f weight:-1];
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1]
                  };
        
        self.contactNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.contactTypeText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.clinicNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address1Text.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.address2Text.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.cityText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.districtText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stateText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pinText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.mobileText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.contactNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.contactTypeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.clinicNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.address1Label.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.address2Label.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.cityLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.districtLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.stateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.pinLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.mobileLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.emailLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
//    [self.medicalContactScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+650)];
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Medical Contact Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView = titleLabel;
    
    [self getMedicalContactDetailAPI];
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.medicalContactScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+650)];
    }
    else{
        [self.medicalContactScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+400)];
    }
}

#pragma mark Get Medical Contact Details
-(void)getMedicalContactDetailAPI{
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
                
                NSString* contactName = [[responseObject valueForKey:@"response"] valueForKey:@"ContactName"];
                if ([contactName isKindOfClass:[NSNull class]] || [contactName isEqualToString:@"<null>"] || [contactName isEqualToString:@""]) {
                    self.contactNameText.text = @"-";
                }
                else{
                    self.contactNameText.text = contactName;
                }
                
                NSString* contactType = [[responseObject valueForKey:@"response"] valueForKey:@"strMedContType"];
                if ([contactType isKindOfClass:[NSNull class]] || [contactType isEqualToString:@"<null>"] || [contactType isEqualToString:@""]) {
                    self.contactTypeText.text = @"-";
                }
                else{
                    self.contactTypeText.text = contactType;
                }
                
                NSString* clinicName = [[responseObject valueForKey:@"response"] valueForKey:@"ClinicName"];
                if ([clinicName isKindOfClass:[NSNull class]] || [clinicName isEqualToString:@"<null>"] || [clinicName isEqualToString:@""]) {
                    self.clinicNameText.text = @"-";
                }
                else{
                    self.clinicNameText.text = clinicName;
                }
                
                NSString* address1 = [[responseObject valueForKey:@"response"] valueForKey:@"Address1"];
                if ([address1 isKindOfClass:[NSNull class]] || [address1 isEqualToString:@"<null>"] || [address1 isEqualToString:@""]) {
                    self.address1Text.text = @"-";
                }
                else{
                    self.address1Text.text = address1;
                }
                
                NSString* address2 = [[responseObject valueForKey:@"response"] valueForKey:@"Address2"];
                if ([address2 isKindOfClass:[NSNull class]] || [address2 isEqualToString:@"<null>"] || [address2 isEqualToString:@""]) {
                    self.address2Text.text = @"-";
                }
                else{
                    self.address2Text.text = address2;
                }
                
                NSString* city = [[responseObject valueForKey:@"response"] valueForKey:@"CityVillage"];
                if ([city isKindOfClass:[NSNull class]] || [city isEqualToString:@"<null>"] || [city isEqualToString:@""]) {
                    self.cityText.text = @"-";
                }
                else{
                    self.cityText.text = city;
                }
                
                NSString* district = [[responseObject valueForKey:@"response"] valueForKey:@"District"];
                if ([district isKindOfClass:[NSNull class]] || [district isEqualToString:@"<null>"] || [district isEqualToString:@""]) {
                    self.districtText.text = @"-";
                }
                else{
                    self.districtText.text = district;
                }
                
                NSString* state = [[responseObject valueForKey:@"response"] valueForKey:@"strState"];
                if ([state isKindOfClass:[NSNull class]] || [state isEqualToString:@"<null>"] || [state isEqualToString:@""]) {
                    self.stateText.text = @"-";
                }
                else{
                    self.stateText.text = state;
                }
                
                NSString* pin = [[responseObject valueForKey:@"response"] valueForKey:@"PIN"];
                if ([pin isKindOfClass:[NSNull class]] || [pin isEqualToString:@"<null>"] || [pin isEqualToString:@""]) {
                    self.pinText.text = @"-";
                }
                else{
                    self.pinText.text = pin;
                }
                
                NSString* mobile = [[responseObject valueForKey:@"response"] valueForKey:@"PrimaryPhone"];
                if ([mobile isKindOfClass:[NSNull class]] || [mobile isEqualToString:@"<null>"] || [mobile isEqualToString:@""]) {
                    self.mobileText.text = @"-";
                }
                else{
                    self.mobileText.text = mobile;
                }
                
                NSString* email = [[responseObject valueForKey:@"response"] valueForKey:@"EmailAddress"];
                if ([email isKindOfClass:[NSNull class]] || [email isEqualToString:@"<null>"] || [email isEqualToString:@""]) {
                    self.emailText.text = @"-";
                }
                else{
                    self.emailText.text = email;
                }
            }
            else{
                [kAppDelegate showAlertView:@"No details found!!"];
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
