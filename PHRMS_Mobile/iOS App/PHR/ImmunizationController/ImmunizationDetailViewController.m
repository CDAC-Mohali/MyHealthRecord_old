//
//  ImmunizationDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "ImmunizationDetailViewController.h"

@interface ImmunizationDetailViewController ()
@property (weak, nonatomic) IBOutlet UILabel *immunizationTextfield;
@property (weak, nonatomic) IBOutlet UITextView *commentsTextview;

@property (weak, nonatomic) IBOutlet UILabel *immunizationLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;

@end

@implementation ImmunizationDetailViewController
@synthesize immunizationDataArray;

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.immunizationTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentsTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.immunizationLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentsLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Immunization Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    self.immunizationTextfield.text = [[self.immunizationDataArray valueForKey:@"ImmunizationName"] capitalizedString];
    
    NSString* strComments = [[self.immunizationDataArray valueForKey:@"Comments"] capitalizedString];
    if (strComments.length>0) {
        self.commentsTextview.text = strComments;
    }
    else{
        self.commentsTextview.text = @"-";
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
