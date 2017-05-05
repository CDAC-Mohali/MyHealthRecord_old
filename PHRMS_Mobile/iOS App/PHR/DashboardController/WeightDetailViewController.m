//
//  WeightDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 19/07/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import "WeightDetailViewController.h"

@interface WeightDetailViewController ()

@property (weak, nonatomic) IBOutlet UITextField *weightText;
@property (weak, nonatomic) IBOutlet UITextField *dateText;
@property (weak, nonatomic) IBOutlet UITextView *commentText;
@property (weak, nonatomic) IBOutlet UITextField *heightText;
@property (weak, nonatomic) IBOutlet UITextField *bmiText;

@property (weak, nonatomic) IBOutlet UILabel *weightLabel;
@property (weak, nonatomic) IBOutlet UILabel *heightLabel;
@property (weak, nonatomic) IBOutlet UILabel *bmiLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;

@end

@implementation WeightDetailViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1]
                  };
        
        self.weightText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.heightText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.bmiText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dateText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.weightLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.heightLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.bmiLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentsLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"BMI Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
//    NSString* weight  = [NSString stringWithFormat:@"%@",[self.weightDataArray valueForKey:@"Result"]];
//    
//    self.weightText.text = weight;
//    
//    NSString* height  = [NSString stringWithFormat:@"%@",[self.weightDataArray valueForKey:@"Goal"]];
//    
//    self.heightText.text = height;
    
    NSString* comments = [self.weightDataArray valueForKey:@"Comments"];
    if ([comments isEqualToString:@""]) {
        self.commentText.text = @"-";
    }
    else{
        self.commentText.text = comments;
    }
    
    NSString* datte = [self.weightDataArray valueForKey:@"strCollectionDate"];
    
    //    NSArray* arr = [datte componentsSeparatedByString:@"T"];
    //    datte = [arr objectAtIndex:0];
    
    self.dateText.text = datte;
    
//    int integerWeight = [weight intValue];
//    int integerHeight = [height intValue];
    
    
    int weightInt = -1;
    NSString* weight = [NSString stringWithFormat:@"%@",[self.weightDataArray valueForKey:@"Result"]];
    if ([weight isKindOfClass:[NSNull class]] || [weight isEqualToString:@"null"] || [weight isEqualToString:@""]) {
        self.weightText.text = @"-";
    }
    else{
        self.weightText.text = weight;
        weightInt = [self.weightText.text intValue];
    }
    
    int heightInt = -1;
    NSString* height = [NSString stringWithFormat:@"%@",[self.weightDataArray valueForKey:@"Goal"]];
    if ([height isKindOfClass:[NSNull class]] || [height isEqualToString:@"null"] || [height isEqualToString:@""]) {
        self.heightText.text = @"-";
    }
    else{
        self.heightText.text = height;
        heightInt = [self.heightText.text intValue];
    }
    
    if (weightInt!=-1 && heightInt!=-1) {
        self.bmiLabel.text = [self BMICalculator:weightInt bodyHeight:heightInt];
    }
//    self.bmiText.text = [self BMICalculator:integerWeight bodyHeight:integerHeight];
}

#pragma mark BMI Calculator 
-(NSString*)BMICalculator:(int)weight bodyHeight:(int)height{
    
    double heigthInMetre = height/100.0f;
    
    double totalHeight = (heigthInMetre*heigthInMetre);
    double bmi = weight/totalHeight;
    
    return [NSString stringWithFormat:@"Your BMI: %.02f",bmi];
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
