//
//  ProceduresDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "ProceduresDetailViewController.h"
#import "Constants.h"
#import "XLMediaZoom.h"
#import "XLVideoZoom.h"

@interface ProceduresDetailViewController (){
    
}
@property (strong, nonatomic) XLMediaZoom *imageZoomView;

@property (weak, nonatomic) IBOutlet UILabel *procedureNameText;
@property (weak, nonatomic) IBOutlet UITextField *procedureEndedOnText;
@property (weak, nonatomic) IBOutlet UILabel *surgeonText;
@property (weak, nonatomic) IBOutlet UITextView *notesText;
@property (weak, nonatomic) IBOutlet UITextField *dischargeSummaryText;
@property (weak, nonatomic) IBOutlet UIImageView *dischargeImage;

@property (weak, nonatomic) IBOutlet UILabel *procedureNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *diagnosisLabel;
@property (weak, nonatomic) IBOutlet UILabel *surgeonLabel;
@property (weak, nonatomic) IBOutlet UILabel *notesLabel;
@property (weak, nonatomic) IBOutlet UILabel *dischargeLabel;

@property (weak, nonatomic) IBOutlet UIScrollView *proceduresScrollView;

@end

@implementation ProceduresDetailViewController
@synthesize imageZoomView   = _imageZoomView;
@synthesize proceduresDataArray;

-(void)viewWillLayoutSubviews{
    
    [self.proceduresScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+150)];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.procedureNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.procedureEndedOnText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.surgeonText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.notesText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dischargeSummaryText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.procedureNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.diagnosisLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.surgeonLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.notesLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dischargeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Procedure Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    [self.dischargeImage addGestureRecognizer:[[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(imageDidTouch:)]];
    
    UILongPressGestureRecognizer *lpgr = [[UILongPressGestureRecognizer alloc]
                                          initWithTarget:self action:@selector(handleLongPress:)];
    lpgr.delegate = self;
    lpgr.delaysTouchesBegan = YES;
    [self.view addGestureRecognizer:lpgr];
    
    self.procedureNameText.text = [[self.proceduresDataArray valueForKey:@"ProcedureName"] capitalizedString];
    
    NSString* SurgeonName = [self.proceduresDataArray valueForKey:@"SurgeonName"];
    if ([SurgeonName isKindOfClass:[NSNull class]] || [SurgeonName isEqualToString:@"<null>"] || [SurgeonName isEqualToString:@""]) {
        self.surgeonText.text = @"-";
    }
    else{
        self.surgeonText.text = SurgeonName;
    }
    
    self.procedureEndedOnText.text = [self.proceduresDataArray valueForKey:@"strEndDate"];
    self.dischargeSummaryText.text = @"-";
    
    NSString* notesString = [self.proceduresDataArray valueForKey:@"Comments"];
    if ([notesString isEqualToString:@""]) {
        self.notesText.text = @"-";
    }
    else{
        self.notesText.text = notesString;
    }
    
    NSString* imageString = [self.proceduresDataArray valueForKey:@"arrImages"];
    NSString* userImage = @"";
    
    if (![imageString isEqualToString:@""]) {
        userImage = [NSString stringWithFormat:@""];
    }
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString:userImage]];
        
        self.dischargeSummaryText.hidden = NO;
        self.dischargeImage.hidden = NO;
        
        dispatch_async(dispatch_get_main_queue(), ^{
            
            //    [self.attachmentImage setHidden:NO];
            //    self.attachmentText.hidden = NO;
            
            if (imageData) {
                [self.dischargeImage setImage:[UIImage imageWithData:imageData]];
                self.dischargeSummaryText.hidden = YES;
            }
            else{
                self.dischargeSummaryText.text = @"No attachment";
                [self.dischargeImage setHidden:YES];
            }
        });
    });
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.proceduresScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+250)];
    }
    else{
        [self.proceduresScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+150)];
    }
}

#pragma mark touch Methods 
-(void)touch{
    [self.view endEditing:YES];
}

#pragma mark Long Press Gesture
-(void)handleLongPress:(UILongPressGestureRecognizer *)gestureRecognizer
{
    if (gestureRecognizer.state != UIGestureRecognizerStateEnded) {
        return;
    }
    
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:nil message:nil preferredStyle:UIAlertControllerStyleActionSheet];
    
    UIAlertAction* saveImage = [UIAlertAction actionWithTitle:@"Save Image" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        
        UIImageWriteToSavedPhotosAlbum(self.dischargeImage.image, self, nil, nil);
        
    }];
    
    alertController.popoverPresentationController.barButtonItem = nil;
    alertController.popoverPresentationController.sourceView = self.view;
    alertController.popoverPresentationController.sourceRect = CGRectMake(self.view.bounds.size.width/2+100, self.view.bounds.size.height-700, 1.0, 1.0);
    
    [alertController addAction:saveImage];
    
    [alertController setModalPresentationStyle:UIModalPresentationPopover];
    
    [self presentViewController:alertController animated:YES completion:nil];
}

#pragma mark Tap Gesture
- (void)imageDidTouch:(UIGestureRecognizer *)recognizer
{
    [self.view addSubview:self.imageZoomView];
    [self.imageZoomView show];
}

#pragma mark Delegates
- (XLMediaZoom *)imageZoomView
{
    if (_imageZoomView) return _imageZoomView;
    
    _imageZoomView = [[XLMediaZoom alloc] initWithAnimationTime:@(0.5) image:self.dischargeImage blurEffect:YES];
    _imageZoomView.tag = 1;
    _imageZoomView.backgroundColor = [UIColor colorWithRed:0.0 green:0.05 blue:0.3 alpha:1.0];
    
    return _imageZoomView;
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

/*
#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    // Get the new view controller using [segue destinationViewController].
    // Pass the selected object to the new view controller.
}
*/

@end
