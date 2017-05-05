//
//  MedicalContactTableViewCell.h
//  PHR
//
//  Created by CDAC HIED on 26/12/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface MedicalContactTableViewCell : UITableViewCell
@property (weak, nonatomic) IBOutlet UILabel *ContactNameText;
@property (weak, nonatomic) IBOutlet UILabel *dateTimeText;
@property (weak, nonatomic) IBOutlet UILabel *specialityText;
//@property (weak, nonatomic) IBOutlet UILabel *mobileText;

@end
