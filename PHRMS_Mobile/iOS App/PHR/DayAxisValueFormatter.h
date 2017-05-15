//
//  DayAxisValueFormatter.h
//  ChartsDemo
//  Copyright © 2016 dcg. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "PHR-Bridging-Header.h"
#import <Charts/Charts-Swift.h>

@interface DayAxisValueFormatter : NSObject <IChartAxisValueFormatter>

- (id)initForChart:(BarLineChartViewBase *)chart;

@end
