//
//  DiabetesTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 23/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface DiabetesTableViewCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *diabetesValueLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateTimeLabel;
@property (weak, nonatomic) IBOutlet UILabel *valueTypeLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImage;

@end
