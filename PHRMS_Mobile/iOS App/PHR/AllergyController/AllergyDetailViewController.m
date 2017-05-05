//
//  AllergyDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 17/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AllergyDetailViewController.h"
#import "constants.h"

@interface AllergyDetailViewController (){

}

@property (weak, nonatomic) IBOutlet UIImageView *allergyNameImageView;
@property (weak, nonatomic) IBOutlet UIScrollView *allergyScrollView;
@property (weak, nonatomic) IBOutlet UILabel *allergyNameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *stillAllergyTextfield;
@property (weak, nonatomic) IBOutlet UITextField *fromTextfield;
@property (weak, nonatomic) IBOutlet UITextField *severityTextfield;
@property (weak, nonatomic) IBOutlet UITextView *notesTextfield;
@property (weak, nonatomic) IBOutlet UILabel *allergyLabel;
@property (weak, nonatomic) IBOutlet UILabel *stillAllergyLabel;
@property (weak, nonatomic) IBOutlet UILabel *fromLabel;
@property (weak, nonatomic) IBOutlet UILabel *severityLabel;
@property (weak, nonatomic) IBOutlet UILabel *notesLabel;



@end

@implementation AllergyDetailViewController
@synthesize allergyDataArray;

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.allergyNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stillAllergyTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.fromTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.severityTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.notesTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.allergyLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.stillAllergyLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.fromLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.severityLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.notesLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        self.allergyScrollView.frame = CGRectMake(0, 0, [[UIScreen mainScreen]bounds].size.width, [[UIScreen mainScreen]bounds].size.height);
        
        [self.allergyScrollView setContentSize:CGSizeMake([[UIScreen mainScreen] bounds].size.width, [[UIScreen mainScreen] bounds].size.height+100)];
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Allergy Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
//    UIColor *color = [UIColor lightGrayColor];
//    self.allergyNameTextfield.attributedPlaceholder = [[NSAttributedString alloc] initWithString:@"Enter allergy name" attributes:@{NSForegroundColorAttributeName: color,
//                                                NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1],}];
    self.navigationItem.titleView=titleLabel;
    
//    [self enableFields:NO];
//    self.allergyNameTextfield.enabled = NO;
    self.stillAllergyTextfield.enabled = NO;
    self.severityTextfield.enabled = NO;
//    self.notesTextfield.enabled = NO;
    self.fromTextfield.enabled = NO;
    self.allergyNameImageView.hidden = YES;
    
    _allergyNameTextfield.text = [[self.allergyDataArray valueForKey:@"AllergyName"] capitalizedString];
    

    NSString* havingAllergy = [self.allergyDataArray valueForKey:@"strStill_Have"];
    _stillAllergyTextfield.text = havingAllergy;
    _fromTextfield.text = [self.allergyDataArray valueForKey:@"strDuration"];
    _severityTextfield.text = [self.allergyDataArray valueForKey:@"strSeverity"];
    
    NSString* noteString = [self.allergyDataArray valueForKey:@"Comments"];
    if ([noteString isEqualToString:@""]) {
        _notesTextfield.text = @"-";
    }
    else{
        _notesTextfield.text = noteString;
    }
    
    UITapGestureRecognizer *recognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(touch)];
    [recognizer setNumberOfTapsRequired:1];
    [recognizer setNumberOfTouchesRequired:1];
    [_allergyScrollView addGestureRecognizer:recognizer];
    
//    UIBarButtonItem *editButton = [[UIBarButtonItem alloc] initWithTitle:@"Edit" style:UIBarButtonItemStylePlain target:self action:@selector(editButtonAction)];
//    [editButton setTitleTextAttributes:attrs forState:UIControlStateNormal];
//    
//    self.navigationItem.rightBarButtonItem = editButton;
}

#pragma mark Enable/Disable Fields Method 
-(void)enableFields:(BOOL)fix{
    _allergyNameTextfield.enabled = fix;
    
    UIColor* color;
    UIColor* textViewColor;
    if (fix) {
        fix = NO;
        color = [UIColor darkGrayColor];
        textViewColor = [UIColor whiteColor];
    }
    else{
        fix = YES;
        color = [UIColor clearColor];
        textViewColor = [UIColor clearColor];
    }
    _allergyNameImageView.hidden = fix;
    
}

#pragma mark touch Methods 
-(void)touch{
    [self.view endEditing:YES];
}

#pragma mark Update Allergy API Call
-(void)updateAllergy{
    if ([kAppDelegate hasInternetConnection]) {
        [kAppDelegate showLoadingIndicator:@"Getting..."];//Show loading indicator.
        
        NSString *urlString = [NSString stringWithFormat:@"enter your API url"];//Url string.
        
        //AFNetworking methods.
        AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
        AFJSONRequestSerializer *requestSerializer = [AFJSONRequestSerializer serializer];
        manager.requestSerializer = requestSerializer;
        [manager GET:urlString parameters:nil success:^(AFHTTPRequestOperation *operation, id responseObject) {
            [kAppDelegate hideLoadingIndicator];
            
            NSLog(@"response = %@",responseObject);
            
            if ([[responseObject valueForKey:@"status"] integerValue]==1) {
                
                [kAppDelegate showAlertView:@"Allergy updated"];
                [self.navigationController popViewControllerAnimated:YES];
            }
            else{
                [kAppDelegate showAlertView:@"No allergy exists!!"];
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

- (IBAction)allergyStartOnButtonAction:(id)sender {
}

- (IBAction)allergyEndedOnButtonAction:(id)sender {
}

- (IBAction)allergySeverityButtonAction:(id)sender {
}

- (void)editButtonAction{
    
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [self enableFields:YES];
        [self.navigationItem.rightBarButtonItem setTitle:@"Update"];
    }
    else{
//        [self updateAllergy];
    }
    
}
@end
