//
//  LabTestsDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "LabTestsDetailViewController.h"
#import "Constants.h"
#import "XLMediaZoom.h"
#import "XLVideoZoom.h"

@interface LabTestsDetailViewController (){
    
}
@property (strong, nonatomic) XLMediaZoom *imageZoomView;

@property (weak, nonatomic) IBOutlet UITextField *attachmentText;
@property (weak, nonatomic) IBOutlet UILabel *testNameText;
@property (weak, nonatomic) IBOutlet UITextField *resultText;
@property (weak, nonatomic) IBOutlet UITextField *unitText;
@property (weak, nonatomic) IBOutlet UIImageView *attachmentImage;
@property (weak, nonatomic) IBOutlet UITextView *commentText;

@property (weak, nonatomic) IBOutlet UILabel *testNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *resultLabel;
@property (weak, nonatomic) IBOutlet UILabel *unitLabel;
@property (weak, nonatomic) IBOutlet UILabel *commentsLabel;
@property (weak, nonatomic) IBOutlet UILabel *attachmentLabel;

@property (weak, nonatomic) IBOutlet UIScrollView *labTestScrollView;

@end

@implementation LabTestsDetailViewController
@synthesize imageZoomView   = _imageZoomView;
@synthesize labTestsDataArray;

-(void)viewWillLayoutSubviews{
    
    [self.labTestScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+150)];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.testNameText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.resultText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.unitText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.commentText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.attachmentText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.testNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.resultLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.unitLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.commentsLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.attachmentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Lab Test Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    [self.attachmentImage addGestureRecognizer:[[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(imageDidTouch:)]];
    
    UILongPressGestureRecognizer *lpgr = [[UILongPressGestureRecognizer alloc]
                                          initWithTarget:self action:@selector(handleLongPress:)];
    lpgr.delegate = self;
    lpgr.delaysTouchesBegan = YES;
    [self.view addGestureRecognizer:lpgr];
    
    self.testNameText.text = [NSString stringWithFormat:@"%@",[[self.labTestsDataArray valueForKey:@"TestName"] capitalizedString] ];
    
    NSString* result = [self.labTestsDataArray valueForKey:@"Result"];
    self.resultText.text = result;
    self.unitText.text = [self.labTestsDataArray valueForKey:@"Unit"];
    
    NSString* comments = [self.labTestsDataArray valueForKey:@"Comments"];
    if ([comments isEqualToString:@""]) {
        self.commentText.text = @"-";
    }
    else{
        self.commentText.text = comments;
    }
    
    NSString* imageString = [self.labTestsDataArray valueForKey:@"arrImages"];
    NSString* userImage = @"";
    
    if (![imageString isEqualToString:@""]) {
        userImage = [NSString stringWithFormat:@""];
    }
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
        
        NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString:userImage]];
        
        dispatch_async(dispatch_get_main_queue(), ^{
            
            //    [self.attachmentImage setHidden:NO];
            //    self.attachmentText.hidden = NO;
            
            if (imageData) {
                [self.attachmentImage setImage:[UIImage imageWithData:imageData]];
                //        [self.attachmentImage.layer setBorderColor:(__bridge CGColorRef _Nullable)([UIColor blackColor])];
                self.attachmentText.hidden = YES;
            }
            else{
                self.attachmentText.text = @"No attachment";
                [self.attachmentImage setHidden:YES];
            }
        });
    });
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.labTestScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+250)];
    }
    else{
        [self.labTestScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+150)];
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
        
        UIImageWriteToSavedPhotosAlbum(self.attachmentImage.image, self, nil, nil);
        
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
    
    _imageZoomView = [[XLMediaZoom alloc] initWithAnimationTime:@(0.5) image:self.attachmentImage blurEffect:YES];
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
