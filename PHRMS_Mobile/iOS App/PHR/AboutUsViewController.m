//
//  AboutUsViewController.m
//  mSwasthya-VaccinationAlertsApp
//
//  Created by Gagandeep Singh on 09/04/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "AboutUsViewController.h"
#import "Constants.h"
#import "SWRevealViewController.h"

@interface AboutUsViewController (){
    SWRevealViewController *revealController;
}

@property (weak, nonatomic) IBOutlet UILabel *phrLabel;
@property (weak, nonatomic) IBOutlet UILabel *versionLabel;
@property (weak, nonatomic) IBOutlet UILabel *productLabel;
@property (weak, nonatomic) IBOutlet UILabel *yearLabel;
@property (weak, nonatomic) IBOutlet UILabel *emailLabel;

@end

@implementation AboutUsViewController

//-(void)viewDidLayoutSubviews{
//    if ([[kAppDelegate checkDeviceType]isEqualToString:iPhone5] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone4]) {
//        self.titleLine1.font = [UIFont fontWithName:nil size:14];
//        self.titleLine2.font = [UIFont fontWithName:nil size:14];
//        self.titleLine3.font = [UIFont fontWithName:nil size:14];
//        self.titleLine4.font = [UIFont fontWithName:nil size:14];
//        self.titleLine5.font = [UIFont fontWithName:nil size:14];
//    }
//}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.

//    [self.view setBackgroundColor:[UIColor colorWithPatternImage:[UIImage imageNamed:@"greenBG"]]];
    
    NSDictionary * attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        
//        self.phrLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
//        self.productLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
//        self.yearLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
//        self.versionLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
//        self.emailLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
        attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                                };
    }
    else{
        attrs = @{
                                NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                                };
    }

    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"About Us"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    NSDictionary * navBarTitleTextAttributes = @{ NSForegroundColorAttributeName : [UIColor blackColor]};
    
    self.navigationController.navigationBar.titleTextAttributes=navBarTitleTextAttributes;
    
    //Set Left Bar Button Item
    [BarButton_Block setCustomBarButtonItem:^(UIButton *barButton, UIBarButtonItem *barItem) {
        [barButton addTarget:self action:@selector(revealAppointmentView:) forControlEvents:UIControlEventTouchUpInside];
        [barButton setImage:[UIImage imageNamed:@"bars_black"] forState:UIControlStateNormal];
        self.navigationItem.leftBarButtonItem=barItem;
    }];
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

#pragma mark Device Orientation Method
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
}

#pragma mark touch Methods 
-(void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
    [self dismissViewControllerAnimated:YES completion:nil];
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
