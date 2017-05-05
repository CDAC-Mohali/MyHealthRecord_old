//
//  PreferenceInforViewController.m
//  PHR
//
//  Created by CDAC HIED on 25/02/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "PreferenceInforViewController.h"
#import "SWRevealViewController.h"
#import "Constants.h"

@interface PreferenceInforViewController (){
    SWRevealViewController *revealController;
    
    NSMutableArray* userPreferencesInfoArray;
}

@property (weak, nonatomic) IBOutlet UILabel *usernameLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImageView;
@property (weak, nonatomic) IBOutlet UIScrollView *preferencesInfoScrollView;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;
@property (weak, nonatomic) IBOutlet UITextField *preferredHospitalTextfield;
@property (weak, nonatomic) IBOutlet UITextField *primaryCareTextfield;
@property (weak, nonatomic) IBOutlet UITextView *specialNeedsTextfield;

@property (weak, nonatomic) IBOutlet UIImageView *preferredHospitalImage;
@property (weak, nonatomic) IBOutlet UIImageView *primaryCareImage;
@property (weak, nonatomic) IBOutlet UIImageView *specialNeedsImage;

@property (weak, nonatomic) IBOutlet UILabel *preferredHospitalLabel;
@property (weak, nonatomic) IBOutlet UILabel *primaryHospitalLabel;
@property (weak, nonatomic) IBOutlet UILabel *specialNeedsLabel;

@end

@implementation PreferenceInforViewController

-(void)viewWillLayoutSubviews{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.preferencesInfoScrollView setContentSize:CGSizeMake(self.preferencesInfoScrollView.frame.size.width, self.preferencesInfoScrollView.frame.size.height+200)];
    }
    else{
        [self.preferencesInfoScrollView setContentSize:CGSizeMake(self.preferencesInfoScrollView.frame.size.width, self.preferencesInfoScrollView.frame.size.height+50)];
    }
}

- (void)viewDidLoad {
    [super viewDidLoad];
    
    [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
        [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
        [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
        self.navigationItem.leftBarButtonItem=barItem;
    }];
    
    NSDictionary * attributes;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        self.preferredHospitalTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.primaryCareTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.specialNeedsTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.preferredHospitalLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.primaryHospitalLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.specialNeedsLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.preferredHospitalLabel.textAlignment = NSTextAlignmentLeft;
        self.primaryHospitalLabel.textAlignment = NSTextAlignmentLeft;
        self.specialNeedsLabel.textAlignment = NSTextAlignmentLeft;
        self.primaryCareTextfield.textAlignment = NSTextAlignmentLeft;
        
        self.usernameLabel.font = [UIFont systemFontOfSize:22.0f];
        self.emailLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.emailLabel.textAlignment = NSTextAlignmentLeft;
        
        [self.userImageView.layer setCornerRadius:35];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:20 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Preferred Hospital"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        [titleLabel setTextAlignment:NSTextAlignmentCenter];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:22.0f weight:-1 ];
        attributes = @{NSFontAttributeName: font};
    }
    else{
//        [self.preferencesInfoScrollView setContentInset:UIEdgeInsetsMake(-64,0,0,0)];
        
        [self.userImageView.layer setCornerRadius:50];
        
        NSDictionary *attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:28 weight:-1]
                                };
        
        NSMutableAttributedString *attributedText =
        [[NSMutableAttributedString alloc] initWithString:@"Preferred Hospital"
                                               attributes:attrs];
        //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
        
        UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
        [titleLabel setTextAlignment:NSTextAlignmentCenter];
        titleLabel.attributedText = attributedText;
        
        self.navigationItem.titleView=titleLabel;
        
        UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1 ];
        attributes = @{NSFontAttributeName: font};
    }
    
    // Do any additional setup after loading the view.
    
    _usernameLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USERNAME];
    self.emailLabel.text = [[NSUserDefaults standardUserDefaults] valueForKey:USEREMAILID];
    
//    [self.userImageView.layer setCornerRadius:50];
    [self.userImageView.layer setMasksToBounds:YES];
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString: [[NSUserDefaults standardUserDefaults] valueForKey:USERIMAGE]]];
        
        dispatch_async(dispatch_get_main_queue(), ^{
            if (imageData) {
                [self.userImageView setImage:[UIImage imageWithData:imageData]];
            }
            else{
                [self.userImageView setImage:[UIImage imageNamed:@"userImage"]];
            }
        });
    });
    
    UIBarButtonItem *addButton = [[UIBarButtonItem alloc] initWithTitle:@"Edit" style:UIBarButtonItemStylePlain target:self action:@selector(editableControls)];
    
//    UIFont * font = [UIFont systemFontOfSize:32.0f weight:-1];
//    NSDictionary * attributes = @{NSFontAttributeName: font};
    
    [addButton setTitleTextAttributes:attributes forState:UIControlStateNormal];
    
    self.navigationItem.rightBarButtonItem = addButton;
}

-(void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:YES];
    
    [self enableEditing:NO];
    [self getPreferencesProfile];
}

- (void)revealAppointmentView:(id)sender {
    
    revealController = [self revealViewController];
    [revealController revealToggleAnimated:YES];
    
    if (self.view.userInteractionEnabled==NO) {
        [self.view setUserInteractionEnabled:YES];
    }
    else{
        [self.view setUserInteractionEnabled:NO];
    }
}

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    //    CGRect screenRect = [[UIScreen mainScreen] bounds];
    //    CGFloat screenWidth = screenRect.size.width;
    //    CGFloat screenHeight = screenRect.size.height;
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.preferencesInfoScrollView setContentSize:CGSizeMake(self.preferencesInfoScrollView.frame.size.width, self.preferencesInfoScrollView.frame.size.height+200)];
    }
    else{
        [self.preferencesInfoScrollView setContentSize:CGSizeMake(self.preferencesInfoScrollView.frame.size.width, self.preferencesInfoScrollView.frame.size.height+50)];
    }
}

#pragma mark Tap Gesture Method
-(void)handleSingleTap{
    [self.view endEditing:YES];
}

#pragma mark Enable Editing Mode
-(void)editableControls{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [self enableEditing:YES];
        [self.navigationItem.rightBarButtonItem setTitle:@"Update"];
        
        self.specialNeedsTextfield.layer.cornerRadius = 3;
        self.specialNeedsTextfield.clipsToBounds = YES;
        self.specialNeedsTextfield.layer.borderWidth = 0.75f;
        self.specialNeedsTextfield.layer.borderColor = [[UIColor lightGrayColor] CGColor];
        
        if ([self.preferredHospitalTextfield.text isEqualToString:@"-"]) {
            self.preferredHospitalTextfield.text = @"";
        }
        if ([self.specialNeedsTextfield.text isEqualToString:@"not filled yet"]) {
            self.specialNeedsTextfield.text = @"";
        }

    }
    else{
        [self updatePreferencesProfile];
    }
}

#pragma mark Enable Control For Editing Method
-(void)enableEditing:(BOOL)value{
    
    self.preferredHospitalTextfield.enabled = value;
    self.primaryCareTextfield.enabled = value;
    self.specialNeedsTextfield.editable = value;
    
    UIColor* color;
    if (value) {
        value = NO;
        color = [UIColor blueColor];
    }
    else{
        value = YES;
        color = [UIColor darkGrayColor];
    }

//    self.specialNeedsTextfield.backgroundColor = color;
    
    self.preferredHospitalImage.hidden = value;
    self.primaryCareImage.hidden = value;
//    self.specialNeedsImage.hidden = value;
}

#pragma mark API Call
-(void)getPreferencesProfile{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"getting..."];//Show loading indicator.
        
        //Parameter.
        //        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        //        [dicParams setObject:[[NSUserDefaults standardUserDefaults] valueForKey:USERID] forKey:@"userid"];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"Service response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                userPreferencesInfoArray = [responseObject valueForKey:@"response"];
            
                NSString* preferredHospital= [NSString stringWithFormat:@"%@",[userPreferencesInfoArray valueForKey:@"Pref_Hosp"]];
                if ([preferredHospital isKindOfClass:[NSNull class]] || [preferredHospital isEqualToString:@"<null>"] || [preferredHospital isEqualToString:@""]) {
                    self.preferredHospitalTextfield.text = @"-";
                }
                else{
                    self.preferredHospitalTextfield.text = preferredHospital;
                }
                
                NSString* Prim_Care_Prov = [NSString stringWithFormat:@"%@",[userPreferencesInfoArray valueForKey:@"Prim_Care_Prov"]];
                if ([Prim_Care_Prov isKindOfClass:[NSNull class]] || [Prim_Care_Prov isEqualToString:@"<null>"]) {
                    self.primaryCareTextfield.text = @"-";
                }
                else{
                    self.primaryCareTextfield.text = Prim_Care_Prov;
                }
        
                
                NSString* phone = [NSString stringWithFormat:@"%@",[userPreferencesInfoArray valueForKey:@"Special_Needs"]];
                if ([phone isKindOfClass:[NSNull class]] || [phone isEqualToString:@"<null>"] || [phone isEqualToString:@""]) {
                    self.specialNeedsTextfield.text = @"not filled yet";
                }
                else{
                    self.specialNeedsTextfield.text = phone;
                }
            }
            else{
                [kAppDelegate showAlertView:@"Request failed"];
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

#pragma mark Update User Emergency Info API Call 
-(void)updatePreferencesProfile{
    
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Updating..."];//Show loading indicator.
        
        NSMutableDictionary* dicParams = [[NSMutableDictionary alloc]init];
        
        NSString *urlString = [NSString stringWithFormat:@"enter your web API url"];//Url
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        
        [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
        //            [requestSerializer setValue:@"application/json" forHTTPHeaderField:@"Accept"];
        
        [requestSerializer setValue:@"text/plain" forHTTPHeaderField:@"Accept"];
        
        manager.requestSerializer = requestSerializer;
        [manager POST:urlString parameters:dicParams success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            NSLog(@"Result dict %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                [kAppDelegate showAlertView:@"Preferences information updated"];
                [self enableEditing:NO];
                
                [self.navigationItem.rightBarButtonItem setTitle:@"Edit"];
            }
            else{
                [kAppDelegate showAlertView:@"Please do some changes"];
                [self enableEditing:NO];
                [self.navigationItem.rightBarButtonItem setTitle:@"Edit"];
            }
            
        } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
            NSLog(@"Error: %@", error);
            [kAppDelegate hideLoadingIndicator];
            [kAppDelegate showAlertView:@"Request failed"];
        }];
    }
    else{
        [kAppDelegate showNetworkAlert];
    }
    //    }
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
