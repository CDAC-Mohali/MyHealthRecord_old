//
//  ProceduresTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ProceduresTableViewCell : UITableViewCell

@property (weak, nonatomic) IBOutlet UILabel *procedureNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *surgeonNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateTimeLabel;
@property (weak, nonatomic) IBOutlet UIImageView *userImage;

@end
