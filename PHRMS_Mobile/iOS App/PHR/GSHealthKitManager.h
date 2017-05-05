//
//  GSHealthKitManager.h
//  HealthBasics
//
//  Created by Lukas Petr on 24/07/15.
//  Copyright (c) 2015 Tuts+. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <HealthKit/HealthKit.h>

@interface GSHealthKitManager : NSObject

+ (GSHealthKitManager *)sharedManager;

- (void)requestAuthorization;

- (NSDate *)readBirthDate;
- (NSDictionary*)readRunningDistance:(NSDate*)currentDateTime;
- (NSDictionary*)readStepsCount:(NSDate*)currentDateTime;
- (int)readFlightsClimbed;
- (void)writeWeightSample:(CGFloat)weight;

- (void)writeWorkoutDataFromModelObject:(id)workoutModelObject;

@end
