//
//  GSHealthKitManager.m
//  HealthBasics
//
//  Created by Lukas Petr on 24/07/15.
//  Copyright (c) 2015 Tuts+. All rights reserved.
//

#import "GSHealthKitManager.h"
#import <HealthKit/HealthKit.h>
#import "Constants.h"

@interface GSHealthKitManager ()

@property (nonatomic, retain) HKHealthStore *healthStore;

@end


@implementation GSHealthKitManager

+ (GSHealthKitManager *)sharedManager {
    static dispatch_once_t pred = 0;
    static GSHealthKitManager *instance = nil;
    dispatch_once(&pred, ^{
        instance = [[GSHealthKitManager alloc] init];
        instance.healthStore = [[HKHealthStore alloc] init];
    });
    return instance;
}

- (void)requestAuthorization {
    
    if ([HKHealthStore isHealthDataAvailable] == NO) {
        // If our device doesn't support HealthKit -> return.
        return;
    }
    
    NSArray *readTypes = @[[HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceWalkingRunning],[HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierStepCount],[HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierFlightsClimbed]];
    
//    NSArray *runningTypes = @[[HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceWalkingRunning]];
    
//    NSArray *writeTypes = @[[HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierBodyMass],
//                            [HKObjectType workoutType]];
    
    [self.healthStore requestAuthorizationToShareTypes:nil
                                             readTypes:[NSSet setWithArray:readTypes]
                                            completion:^(BOOL success, NSError * _Nullable error) {
                                                NSLog(@"requestAuthorization completion block");
                                            }];
}

- (NSDate *)readBirthDate {
    NSError *error;
    NSDate *dateOfBirth = [self.healthStore dateOfBirthWithError:&error];   // Convenience method of HKHealthStore to get date of birth directly.
    
    if (!dateOfBirth) {
        NSLog(@"Either an error occured fetching the user's age information or none has been stored yet. In your app, try to handle this gracefully.");
    }
    
    return dateOfBirth;
}

- (NSDictionary*)readRunningDistance:(NSDate*)currentDateTime {
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
    dateFormatter.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"IST"];
    
    NSDateFormatter *dateFormatter2 = [[NSDateFormatter alloc] init];
    [dateFormatter2 setDateFormat:@"yyyy-MM-dd"];
    dateFormatter2.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"IST"];
    
    NSString* strSynDate = [NSString stringWithFormat:@"%@",currentDateTime];
    NSArray* foo = [strSynDate componentsSeparatedByString: @"+"];
    strSynDate = [foo objectAtIndex: 0];
    NSDate* syncDateDummy = [dateFormatter dateFromString:strSynDate];
    
    NSString* syncDateString = [dateFormatter2 stringFromDate:syncDateDummy];
    NSDate* syncDate = [dateFormatter2 dateFromString:syncDateString];
    
    NSString* todayDateString = [dateFormatter2 stringFromDate:[NSDate date]];
    NSDate* todayDate = [dateFormatter2 dateFromString:todayDateString];
    
    NSDate* queryDate;
    
    NSComparisonResult result = [todayDate compare:syncDate];
    NSCalendar *calendar = [NSCalendar currentCalendar];
    //    NSDate *queryDate = [NSDate date];
    
    NSDateComponents *components;
    if (result==NSOrderedSame) {
        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
        [dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
        dateFormatter.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"IST"];
        
        NSString* str = [dateFormatter stringFromDate:currentDateTime];
        queryDate = [dateFormatter dateFromString:str];
        
        if ([[NSUserDefaults standardUserDefaults] objectForKey:lastHealthDataSyncTime]) {
            components = [calendar components:NSCalendarUnitYear|NSCalendarUnitMonth|NSCalendarUnitDay|NSCalendarUnitHour|NSCalendarUnitMinute fromDate:queryDate];
        }
        else{
            components = [calendar components:NSCalendarUnitYear|NSCalendarUnitMonth|NSCalendarUnitDay fromDate:queryDate];
        }
        
    }
    else{
        queryDate = [NSDate date];
        
        components = [calendar components:NSCalendarUnitYear|NSCalendarUnitMonth|NSCalendarUnitDay fromDate:queryDate];
    }
    
    HKQuantityType *running = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierDistanceWalkingRunning];
    
//    [self.healthStore ]
//    HKQuantityType *running = [self.healthStore ];   // Convenience method of HKHealthStore to get date of birth directly.
    
//    NSSortDescriptor *timeSortDescription = [[NSSortDescriptor alloc] initWithKey:HKSampleSortIdentifierEndDate ascending:NO];
    
    __block float totalRunning = 0.0;
    __block int totalTimeInMintues = 0;
    
    
    NSDate *startDate = [calendar dateFromComponents:components];
    NSDate *endDate = [calendar dateByAddingUnit:NSCalendarUnitDay value:1 toDate:startDate options:0];
    
    NSPredicate *predicate = [HKQuery predicateForSamplesWithStartDate:startDate endDate:endDate options:HKQueryOptionStrictStartDate];
    
    HKSampleQuery  *query = [[HKSampleQuery alloc] initWithSampleType:running
                    predicate:predicate limit:HKObjectQueryNoLimit
                    sortDescriptors:nil
                    resultsHandler:^(HKSampleQuery *query, NSArray *result, NSError *error){
                        
                             NSLog(@"RESULT  : =>  %@",result);
                             if(!error && result)
                             {
                                 float totalKms=0.0;
                                 int totalTime=0;
                                 NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
                                 [dateFormat setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
                                 for(HKQuantitySample *quantitySample in result)
                                 {
                                     // your code here
                                     
                                     HKQuantity *quantity = quantitySample.quantity;
                                     NSDate* startTime = quantitySample.startDate;
                                     NSDate* endTime = quantitySample.endDate;
                                     
                                     NSTimeInterval time = [endTime timeIntervalSinceDate:startTime];
                                     
                                     int timeInMins = time/60;
                                     
                                     totalTime+=timeInMins;
                                     
                                     //HKQuantity *quantity = quantitySample.quantity;
                                     NSString *string = [NSString stringWithFormat:@"%@",quantity];
                                     NSString *newString1 = [string stringByReplacingOccurrencesOfString:@" count" withString:@""];
                                     
                                     float runningKMs = [newString1 floatValue]/1000;
                                     runningKMs = [[NSString stringWithFormat:@"%.02f",runningKMs] floatValue];
                                     
                                     totalKms += runningKMs;
                                 }
                                 totalRunning = totalKms;
                                 //using total steps
                                 totalTimeInMintues = totalTime;
                             }
                             else{
                                 NSLog(@"%@",error);
                             }
                         }];
    
    [self.healthStore executeQuery:query];
    sleep(1);
    
//    NSLog(@"%@",[NSDictionary dictionaryWithObjectsAndKeys:[NSNumber numberWithInt:totalTimeInMintues], @"Time", [NSNumber numberWithFloat:totalRunning], @"Distance", nil]);
    return [NSDictionary dictionaryWithObjectsAndKeys:[NSNumber numberWithInt:totalTimeInMintues], @"Time", [NSNumber numberWithFloat:totalRunning], @"Distance", nil];
//    return totalRunning;
}

- (NSDictionary*)readStepsCount:(NSDate*)currentDateTime {
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
    dateFormatter.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"IST"];
    
    NSDateFormatter *dateFormatter2 = [[NSDateFormatter alloc] init];
    [dateFormatter2 setDateFormat:@"yyyy-MM-dd"];
    dateFormatter2.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"IST"];
    
    NSString* strSynDate = [NSString stringWithFormat:@"%@",currentDateTime];
    NSArray* foo = [strSynDate componentsSeparatedByString: @"+"];
    strSynDate = [foo objectAtIndex: 0];
    NSDate* syncDateDummy = [dateFormatter dateFromString:strSynDate];
    
    NSString* syncDateString = [dateFormatter2 stringFromDate:syncDateDummy];
    NSDate* syncDate = [dateFormatter2 dateFromString:syncDateString];
    
    NSString* todayDateString = [dateFormatter2 stringFromDate:[NSDate date]];
    NSDate* todayDate = [dateFormatter2 dateFromString:todayDateString];
    
    NSDate* queryDate;
    
    NSComparisonResult result = [todayDate compare:syncDate];
    NSCalendar *calendar = [NSCalendar currentCalendar];
    
    //    NSDate *now = [NSDate date];
    NSDateComponents *components;
    if (result==NSOrderedSame) {
        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
        [dateFormatter setDateFormat:@"yyyy-MM-dd HH:mm:ss"];
        dateFormatter.timeZone = [NSTimeZone timeZoneWithAbbreviation:@"IST"];
        
        NSString* str = [dateFormatter stringFromDate:currentDateTime];
        queryDate = [dateFormatter dateFromString:str];
        
        if ([[NSUserDefaults standardUserDefaults] objectForKey:lastHealthDataSyncTime]) {
            components = [calendar components:NSCalendarUnitYear|NSCalendarUnitMonth|NSCalendarUnitDay|NSCalendarUnitHour|NSCalendarUnitMinute fromDate:queryDate];
        }
        else{
            components = [calendar components:NSCalendarUnitYear|NSCalendarUnitMonth|NSCalendarUnitDay fromDate:queryDate];
        }
    }
    else{
        queryDate = [NSDate date];
        
        components = [calendar components:NSCalendarUnitYear|NSCalendarUnitMonth|NSCalendarUnitDay fromDate:queryDate];
    }
    
    HKQuantityType *steps = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierStepCount];
    
    //    [self.healthStore ]
    //    HKQuantityType *running = [self.healthStore ];   // Convenience method of HKHealthStore to get date of birth directly.
    
    //    NSSortDescriptor *timeSortDescription = [[NSSortDescriptor alloc] initWithKey:HKSampleSortIdentifierEndDate ascending:NO];
    
    __block int totalSteps = 0;
    __block int totalStepsTime = 0;
    
    
    NSDate *startDate = [calendar dateFromComponents:components];
    NSDate *endDate = [calendar dateByAddingUnit:NSCalendarUnitDay value:1 toDate:startDate options:0];
    
    NSPredicate *predicate = [HKQuery predicateForSamplesWithStartDate:startDate endDate:endDate options:HKQueryOptionStrictStartDate];
    
    HKSampleQuery  *query = [[HKSampleQuery alloc] initWithSampleType:steps
                                                            predicate:predicate limit:HKObjectQueryNoLimit
                                                      sortDescriptors:nil
                                                       resultsHandler:^(HKSampleQuery *query, NSArray *result, NSError *error){
                                                           
                                                           NSLog(@"RESULT  : =>  %@",result);
                                                           if(!error && result)
                                                           {
                                                               int stepsWalked=0;
                                                               int stepsWalkedTime=0;
                                                               for(HKQuantitySample *quantitySample in result)
                                                               {
                                                                   // your code here
                                                                   
                                                                   HKQuantity *quantity = quantitySample.quantity;
                                                                   //HKQuantity *quantity = quantitySample.quantity;
                                                                   
                                                                   NSDate* startTime = quantitySample.startDate;
                                                                   NSDate* endTime = quantitySample.endDate;
                                                                   
                                                                   NSTimeInterval time = [endTime timeIntervalSinceDate:startTime];
                                                                   
                                                                   int timeInMins = time/60;
                                                                   
                                                                   stepsWalkedTime+=timeInMins;
                                                                   
                                                                   
                                                                   NSString *string = [NSString stringWithFormat:@"%@",quantity];
                                                                   NSString *newString1 = [string stringByReplacingOccurrencesOfString:@" count" withString:@""];
                                                                   
                                                                    NSInteger count = [newString1 integerValue];
                                                                   stepsWalked += count;
                                                               }
                                                               totalSteps = stepsWalked;
                                                               totalStepsTime = stepsWalkedTime;
                                                               //using total steps
                                                           }
                                                           else{
                                                               NSLog(@"%@",error);
                                                           }
                                                       }];
    
    [self.healthStore executeQuery:query];
    sleep(1);
    
    return [NSDictionary dictionaryWithObjectsAndKeys:[NSNumber numberWithInt:totalStepsTime], @"Time", [NSNumber numberWithInt:totalSteps], @"Distance", nil];
//    return totalSteps;
}

- (int)readFlightsClimbed {
    
    HKQuantityType *flight = [HKObjectType quantityTypeForIdentifier:HKQuantityTypeIdentifierFlightsClimbed];
    
    //    [self.healthStore ]
    //    HKQuantityType *running = [self.healthStore ];   // Convenience method of HKHealthStore to get date of birth directly.
    
    //    NSSortDescriptor *timeSortDescription = [[NSSortDescriptor alloc] initWithKey:HKSampleSortIdentifierEndDate ascending:NO];
    
    __block int totalFlights = 0;
    
    NSCalendar *calendar = [NSCalendar currentCalendar];
    
    NSDate *now = [NSDate date];
    
    NSDateComponents *components = [calendar components:NSCalendarUnitYear|NSCalendarUnitMonth|NSCalendarUnitDay fromDate:now];
    
    NSDate *startDate = [calendar dateFromComponents:components];
    NSDate *endDate = [calendar dateByAddingUnit:NSCalendarUnitDay value:1 toDate:startDate options:0];
    
    NSPredicate *predicate = [HKQuery predicateForSamplesWithStartDate:startDate endDate:endDate options:HKQueryOptionStrictStartDate];
    
    HKSampleQuery  *query = [[HKSampleQuery alloc] initWithSampleType:flight
                                                            predicate:predicate limit:HKObjectQueryNoLimit
                                                      sortDescriptors:nil
                                                       resultsHandler:^(HKSampleQuery *query, NSArray *result, NSError *error){
                                                           
                                                           NSLog(@"RESULT  : =>  %@",result);
                                                           if(!error && result)
                                                           {
                                                               int stepsWalked=0;
                                                               for(HKQuantitySample *quantitySample in result)
                                                               {
                                                                   // your code here
                                                                   
                                                                   HKQuantity *quantity = quantitySample.quantity;
                                                                   //HKQuantity *quantity = quantitySample.quantity;
                                                                   NSString *string = [NSString stringWithFormat:@"%@",quantity];
                                                                   NSString *newString1 = [string stringByReplacingOccurrencesOfString:@" count" withString:@""];
                                                                   
                                                                   NSInteger count = [newString1 integerValue];
                                                                   stepsWalked += count;
                                                               }
                                                               totalFlights = stepsWalked;
                                                               //using total steps
                                                           }
                                                           else{
                                                               NSLog(@"%@",error);
                                                           }
                                                       }];
    
    [self.healthStore executeQuery:query];
    sleep(1);
    
    return totalFlights;
}


- (void)writeWeightSample:(CGFloat)weight {
    
    // Each quantity consists of a value and a unit.
    HKUnit *kilogramUnit = [HKUnit gramUnitWithMetricPrefix:HKMetricPrefixKilo];
    HKQuantity *weightQuantity = [HKQuantity quantityWithUnit:kilogramUnit doubleValue:weight];
    
    HKQuantityType *weightType = [HKQuantityType quantityTypeForIdentifier:HKQuantityTypeIdentifierBodyMass];
    NSDate *now = [NSDate date];
    
    // For every sample, we need a sample type, quantity and a date.
    HKQuantitySample *weightSample = [HKQuantitySample quantitySampleWithType:weightType quantity:weightQuantity startDate:now endDate:now];
    
    [self.healthStore saveObject:weightSample withCompletion:^(BOOL success, NSError *error) {
        if (!success) {
            NSLog(@"Error while saving weight (%f) to Health Store: %@.", weight, error);
        }
    }];
}

- (void)writeWorkoutDataFromModelObject:(id)workoutModelObject {
    
    // In a real world app, you would pass in a model object representing your workout data, and you would pull the relevant data here and pass it to the HealthKit workout method.
    
    // For the sake of simplicity of this example, we will just set arbitrary data.
    NSDate *startDate = [NSDate date];
    NSDate *endDate = [startDate dateByAddingTimeInterval:60 * 60 * 2];
    NSTimeInterval duration = [endDate timeIntervalSinceDate:startDate];
    CGFloat distanceInMeters = 57000.;
    
    HKQuantity *distanceQuantity = [HKQuantity quantityWithUnit:[HKUnit meterUnit] doubleValue:(double)distanceInMeters];
    
    HKWorkout *workout = [HKWorkout workoutWithActivityType:HKWorkoutActivityTypeRunning
                                                  startDate:startDate
                                                    endDate:endDate
                                                   duration:duration
                                          totalEnergyBurned:nil
                                              totalDistance:distanceQuantity
                                                   metadata:nil];
    
    [self.healthStore saveObject:workout withCompletion:^(BOOL success, NSError *error) {
        NSLog(@"Saving workout to healthStore - success: %@", success ? @"YES" : @"NO");
        if (error != nil) {
            NSLog(@"error: %@", error);
        }
    }];
}

@end
