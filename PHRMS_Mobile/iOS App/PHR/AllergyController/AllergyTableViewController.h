//
//  AllergyTableViewController.h
//  PHR
//
//  Created by CDAC HIED on 07/03/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface AllergyTableViewController : UIViewController<NSURLSessionDelegate>

@property (nonatomic, retain) NSMutableArray* allergyNameArray;
@property (weak, nonatomic) IBOutlet UILabel *allergyNameLabel;
@property (weak, nonatomic) IBOutlet UITableView *allergyNameTableView;
@property (weak, nonatomic) IBOutlet UISearchBar *searchBar;

@end
