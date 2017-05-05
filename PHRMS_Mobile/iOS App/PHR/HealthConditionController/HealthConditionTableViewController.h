//
//  HealthConditionTableViewController.h
//  PHR
//
//  Created by CDAC HIED on 28/03/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface HealthConditionTableViewController : UIViewController<NSURLSessionDelegate>

@property (nonatomic, retain) NSMutableArray* healthConditionNameArray;
@property (weak, nonatomic) IBOutlet UITableView *problemNameTableView;
@property (weak, nonatomic) IBOutlet UISearchBar *searchBar;

@end
