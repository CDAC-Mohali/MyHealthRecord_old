//
//  BloodPresureTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 22/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface BloodPresureTableViewCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *DiaSysValuesLabel;
@property (weak, nonatomic) IBOutlet UILabel *pulseValueLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateTimeLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImage;

@end
