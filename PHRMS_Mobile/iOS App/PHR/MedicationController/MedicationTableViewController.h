//
//  MedicationTableViewController.h
//  PHR
//
//  Created by CDAC HIED on 07/04/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface MedicationTableViewController : UIViewController<NSURLSessionDelegate>

@property (nonatomic, retain) NSMutableArray* medicationNameArray;
@property (weak, nonatomic) IBOutlet UITableView *medicationNameTableView;
@property (weak, nonatomic) IBOutlet UISearchBar *searchBar;

@end
