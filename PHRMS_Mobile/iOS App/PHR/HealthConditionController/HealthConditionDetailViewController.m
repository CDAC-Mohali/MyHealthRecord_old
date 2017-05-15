//
//  HealthConditionDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "HealthConditionDetailViewController.h"

@interface HealthConditionDetailViewController ()
@property (weak, nonatomic) IBOutlet UILabel *problemNameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *diagmosisDateTextfiled;
//@property (weak, nonatomic) IBOutlet UITextField *serviceDateTextfield;
@property (weak, nonatomic) IBOutlet UITextView *providerTextfield;
@property (weak, nonatomic) IBOutlet UITextView *notesTextfield;
@property (weak, nonatomic) IBOutlet UITextField *stillAllergyTextfield;

@property (weak, nonatomic) IBOutlet UILabel *problemNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *providerLabel;
@property (weak, nonatomic) IBOutlet UILabel *notesLabel;
@property (weak, nonatomic) IBOutlet UILabel *stillHaveLabel;


@end

@implementation HealthConditionDetailViewController
@synthesize healthConditionDataArray;

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.problemNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.diagmosisDateTextfiled.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.providerTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.notesTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.stillAllergyTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.problemNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.providerLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.notesLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.stillHaveLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Problem Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    //    UIColor *color = [UIColor lightGrayColor];
    //    self.allergyNameTextfield.attributedPlaceholder = [[NSAttributedString alloc] initWithString:@"Enter allergy name" attributes:@{NSForegroundColorAttributeName: color,
    //                                                NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1],}];
    self.navigationItem.titleView = titleLabel;
    
    self.problemNameTextfield.text = [[self.healthConditionDataArray valueForKey:@"HealthCondition"] capitalizedString];
    
    NSString* havingAllergy = [self.healthConditionDataArray valueForKey:@"strStillHaveCondition"];
    self.stillAllergyTextfield.text = havingAllergy;
    self.diagmosisDateTextfiled.text = [self.healthConditionDataArray valueForKey:@"strDiagnosisDate"];
    self.providerTextfield.text = [self.healthConditionDataArray valueForKey:@"Provider"];
    
    NSString* notesString = [self.healthConditionDataArray valueForKey:@"Notes"];
    if ([notesString isEqualToString:@""]) {
        self.notesTextfield.text = @"-";
    }
    else{
        self.notesTextfield.text = notesString;
    }
    
    self.diagmosisDateTextfiled.text = [self.healthConditionDataArray valueForKey:@"strDiagnosisDate"];
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
