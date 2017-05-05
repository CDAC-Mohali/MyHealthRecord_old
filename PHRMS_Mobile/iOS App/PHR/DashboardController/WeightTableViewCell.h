//
//  WeightTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 19/07/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface WeightTableViewCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *weightLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;
@property (weak, nonatomic) IBOutlet UILabel *heightLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateTimeLabel;
@property (weak, nonatomic) IBOutlet UILabel *bmiLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImage;

@end
