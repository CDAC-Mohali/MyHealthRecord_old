//
//  ActivityDetailsViewController.m
//  PHR
//
//  Created by CDAC HIED on 23/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "ActivityDetailsViewController.h"

@interface ActivityDetailsViewController ()

@property (weak, nonatomic) IBOutlet UIScrollView *activityScrollView;
@property (weak, nonatomic) IBOutlet UITextField *distanceText;
@property (weak, nonatomic) IBOutlet UITextField *dateText;
@property (weak, nonatomic) IBOutlet UITextView *commentText;
@property (weak, nonatomic) IBOutlet UITextField *pathNameText;
@property (weak, nonatomic) IBOutlet UITextField *timeTakenText;
@property (weak, nonatomic) IBOutlet UITextField *activityNameText;

@property (weak, nonatomic) IBOutlet UILabel *distanceLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentLabel;
@property (weak, nonatomic) IBOutlet UILabel *pathLabel;
@property (weak, nonatomic) IBOutlet UILabel *timeLabel;
@property (weak, nonatomic) IBOutlet UILabel *activityLabel;

@end

@implementation ActivityDetailsViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    [self.activityScrollView setContentSize:CGSizeMake([UIScreen mainScreen].bounds.size.width, self.view.frame.size.height+150)];
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1]
                  };
        
        self.distanceText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dateText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.pathNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.timeTakenText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.activityNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.distanceLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.pathLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.timeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.activityLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Activity Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    NSString* distance  = [NSString stringWithFormat:@"%@",[self.activitiesDataArray valueForKey:@"Distance"]];
    
    self.distanceText.text = distance;
    
    NSString* pathName  = [NSString stringWithFormat:@"%@",[self.activitiesDataArray valueForKey:@"PathName"]];
    
    self.pathNameText.text = pathName;
    
    self.activityNameText.text = [NSString stringWithFormat:@"%@",[self.activitiesDataArray valueForKey:@"ActivityName"]];
    
    NSString* time = [self.activitiesDataArray valueForKey:@"FinishTime"];
    if ([time isKindOfClass:[NSNull class]] || [time isEqualToString:@"<null>"] || [time isEqualToString:@""]) {
        self.timeTakenText.text = @"-";
    }
    else{
        self.timeTakenText.text = time;
    }
    
    NSString* comments = [self.activitiesDataArray valueForKey:@"Comments"];
    if ([comments isKindOfClass:[NSNull class]] || [comments isEqualToString:@"<null>"] ||[comments isEqualToString:@""]) {
        self.commentText.text = @"-";
    }
    else{
        self.commentText.text = comments;
    }
    
    NSString* datte = [self.activitiesDataArray valueForKey:@"strCollectionDate"];
    
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
