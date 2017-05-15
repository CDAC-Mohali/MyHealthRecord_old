//
//  ImmunzationNameTableViewController.h
//  PHR
//
//  Created by CDAC HIED on 30/03/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ImmunzationNameTableViewController : UIViewController<NSURLSessionDelegate>

@property (nonatomic, retain) NSMutableArray* immunzationNameArray;
@property (weak, nonatomic) IBOutlet UITableView *immunizationTableView;
@property (weak, nonatomic) IBOutlet UISearchBar *searchBar;

@end
