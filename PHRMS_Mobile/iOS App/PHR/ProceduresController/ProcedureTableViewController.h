//
//  ProcedureTableViewController.h
//  PHR
//
//  Created by CDAC HIED on 12/04/16.
//  Copyright © 2016 CDAC HIED. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ProcedureTableViewController : UIViewController<NSURLSessionDelegate>

@property (nonatomic, retain) NSMutableArray* proceduresNameArray;
@property (weak, nonatomic) IBOutlet UITableView *procedureNameTableView;
@property (weak, nonatomic) IBOutlet UISearchBar *searchBar;

@end
