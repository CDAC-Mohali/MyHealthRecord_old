//
//  HealthDataTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 02/01/17.
//  Copyright © 2017 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface HealthDataTableViewCell : UITableViewCell

@property (weak, nonatomic) IBOutlet UILabel *fitnessTitleLabel;
@property (weak, nonatomic) IBOutlet UILabel *walkingLabel;
@property (weak, nonatomic) IBOutlet UILabel *walkingDistanceLabel;
@property (weak, nonatomic) IBOutlet UILabel *runningLabel;
@property (weak, nonatomic) IBOutlet UILabel *runningDistanceLabel;
@property (weak, nonatomic) IBOutlet UILabel *stepsLabel;
@property (weak, nonatomic) IBOutlet UILabel *stepsCountLabel;

@property (weak, nonatomic) IBOutlet UIButton *syncButton;

//- (IBAction)syncButtonAction:(id)sender;

@end
