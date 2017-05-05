//
//  BloodPresureDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 22/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "BloodPresureDetailViewController.h"

@interface BloodPresureDetailViewController ()
@property (weak, nonatomic) IBOutlet UITextField *resultText;
@property (weak, nonatomic) IBOutlet UITextField *dateText;
@property (weak, nonatomic) IBOutlet UITextView *commentText;

@property (weak, nonatomic) IBOutlet UILabel *resultLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentLabel;

@end

@implementation BloodPresureDetailViewController
@synthesize bpDataArray;

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1]
                  };
        
        self.resultText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dateText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.resultLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Blood Pressure Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    NSString* result  = [NSString stringWithFormat:@"%@/ %@/ %@", [self.bpDataArray valueForKey:@"ResSystolic"],[self.bpDataArray valueForKey:@"ResDiastolic"], [self.bpDataArray valueForKey:@"ResPulse"]];
    
    self.resultText.text = result;
    
    NSString* comments = [self.bpDataArray valueForKey:@"Comments"];
    self.commentText.text = comments;
    
    NSString* datte = [self.bpDataArray valueForKey:@"strCollectionDate"];
    
//    NSArray* arr = [datte componentsSeparatedByString:@"T"];
//    datte = [arr objectAtIndex:0];
    
    self.dateText.text = datte;
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
