//
//  DiabetesDetailsViewController.m
//  PHR
//
//  Created by CDAC HIED on 23/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "DiabetesDetailsViewController.h"

@interface DiabetesDetailsViewController ()

@property (weak, nonatomic) IBOutlet UITextField *resultText;
@property (weak, nonatomic) IBOutlet UITextField *dateText;
@property (weak, nonatomic) IBOutlet UITextView *commentText;
@property (weak, nonatomic) IBOutlet UITextField *valueTypeText;

@property (weak, nonatomic) IBOutlet UILabel *resultLabel;
@property (weak, nonatomic) IBOutlet UILabel *collectionDateLabel;
@property (weak, nonatomic) IBOutlet UILabel *valueTypeLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;

@end

@implementation DiabetesDetailsViewController
@synthesize diabetesDataArray;

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
        self.valueTypeText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.resultLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.collectionDateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentsLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.valueTypeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Blood Glucose Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    NSString* result  = [NSString stringWithFormat:@"%@",[self.diabetesDataArray  valueForKey:@"Result"]];
    
    self.resultText.text = result;
    
    NSString* valueType  = [NSString stringWithFormat:@"%@",[self.diabetesDataArray  valueForKey:@"ValueType"]];
    
    self.valueTypeText.text = valueType;
    
    NSString* comments = [self.diabetesDataArray  valueForKey:@"Comments"];
    if ([comments isEqualToString:@""]) {
        self.commentText.text = @"-";
    }
    else{
        self.commentText.text = comments;
    }
    
    NSString* datte = [self.diabetesDataArray valueForKey:@"strCollectionDate"];
    
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
