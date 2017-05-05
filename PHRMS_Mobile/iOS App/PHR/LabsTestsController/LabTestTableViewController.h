//
//  LabTestTableViewController.h
//  PHR
//
//  Created by CDAC HIED on 13/04/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface LabTestTableViewController : UIViewController<NSURLSessionDelegate>

@property (nonatomic, retain) NSMutableArray* labTestNameArray;
@property (weak, nonatomic) IBOutlet UITableView *testNameTableView;
@property (weak, nonatomic) IBOutlet UISearchBar *searchBar;

@end
