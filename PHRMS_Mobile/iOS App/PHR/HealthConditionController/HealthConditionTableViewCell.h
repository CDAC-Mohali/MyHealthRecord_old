//
//  HealthConditionTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface HealthConditionTableViewCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *healthConditionNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *stillHavingLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateTimeLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImage;

@end
