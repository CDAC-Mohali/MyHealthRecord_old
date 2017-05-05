//
//  DashboardTableViewCell.m
//  PHR
//
//  Created by CDAC HIED on 10/12/15.
//  Copyright © 2015 CDAC HIED. All rights reserved.
//

#import "DashboardTableViewCell.h"

@interface DashboardTableViewCell ()
{
    NSIndexPath *path;
    NSArray* arrBP;
    NSArray* arrBG;
}
@end

@implementation DashboardTableViewCell

- (void)awakeFromNib {
    // Initialization code
    arrBP = [NSArray new];
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated {
    [super setSelected:selected animated:animated];

    // Configure the view for the selected state
}

@end
