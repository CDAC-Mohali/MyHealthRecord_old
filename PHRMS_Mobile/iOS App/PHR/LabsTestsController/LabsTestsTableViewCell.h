//
//  LabsTestsTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface LabsTestsTableViewCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *labTestsNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *resultLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateTimeLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImage;

@end
