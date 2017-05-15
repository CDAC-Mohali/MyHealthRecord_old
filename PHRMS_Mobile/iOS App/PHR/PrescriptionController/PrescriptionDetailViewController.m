//
//  PrescriptionDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "PrescriptionDetailViewController.h"
#import "Constants.h"
#import "XLMediaZoom.h"
#import "XLVideoZoom.h"


@interface PrescriptionDetailViewController ()

@property (strong, nonatomic) XLMediaZoom *imageZoomView;

@property (weak, nonatomic) IBOutlet UIScrollView *prescriptionScrollView;
@property (weak, nonatomic) IBOutlet UILabel *doctorNameTextfield;
@property (weak, nonatomic) IBOutlet UILabel *clinicNameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *addressTextfield;
@property (weak, nonatomic) IBOutlet UITextField *phoneTextfield;
@property (weak, nonatomic) IBOutlet UIButton *prescriptionDateButton;
@property (weak, nonatomic) IBOutlet UIImageView *attachment1ImageView;
@property (weak, nonatomic) IBOutlet UITextField *attachmentText;
@property (weak, nonatomic) IBOutlet UIImageView *attachment3ImageView;
@property (weak, nonatomic) IBOutlet UITextView *prescriptionDetailsTextview;
//@property (weak, nonatomic) IBOutlet UIButton *attachmentButton;
//- (IBAction)attachmentButtonAction:(id)sender;
//@property (weak, nonatomic) IBOutlet UIButton *prescriptionDateButtonAction;
@property (weak, nonatomic) IBOutlet UIButton *clickButton;
- (IBAction)clickButtonAction:(id)sender;

@property (weak, nonatomic) IBOutlet UILabel *doctorNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *hospitalNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *addressLabel;
@property (weak, nonatomic) IBOutlet UILabel *phoneLabel;
@property (weak, nonatomic) IBOutlet UILabel *remarksLabel;
@property (weak, nonatomic) IBOutlet UILabel *prescriptionDateLabel;
@property (weak, nonatomic) IBOutlet UILabel *attachmentLabel;

@end

@implementation PrescriptionDetailViewController
@synthesize imageZoomView   = _imageZoomView;
@synthesize prescriptionDataArray;

-(void)viewWillLayoutSubviews{
    
    [self.prescriptionScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+250)];
}

- (void)viewDidLoad {
    
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.doctorNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.clinicNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.addressTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.phoneTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.prescriptionDetailsTextview.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.prescriptionDateButton.titleLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.attachmentText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.doctorNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.hospitalNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.addressLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.phoneLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.remarksLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.prescriptionDateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.attachmentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Prescription Details"
                                           attributes:attrs];
    //    [attributedText setAttributes:subAttrs range:NSMakeRange(0, 14)];
    
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    [self.attachment1ImageView addGestureRecognizer:[[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(imageDidTouch:)]];
    
    UILongPressGestureRecognizer *lpgr = [[UILongPressGestureRecognizer alloc]
                                          initWithTarget:self action:@selector(handleLongPress:)];
    lpgr.delegate = self;
    lpgr.delaysTouchesBegan = YES;
    [self.view addGestureRecognizer:lpgr];
    
    _doctorNameTextfield.text = [[self.prescriptionDataArray valueForKey:@"DocName"] capitalizedString];
    _clinicNameTextfield.text = [[self.prescriptionDataArray valueForKey:@"ClinicName"] capitalizedString];
    
    if ([[self.prescriptionDataArray valueForKey:@"DocAddress"]isEqualToString:@""]) {
         _addressTextfield.text = @"-";
    }
    else{
        _addressTextfield.text = [[self.prescriptionDataArray valueForKey:@"DocAddress"] capitalizedString];
    }
    if ([[self.prescriptionDataArray valueForKey:@"DocPhone"]isEqualToString:@""]) {
        _phoneTextfield.text = @"-";
    }
    else{
        _phoneTextfield.text = [[self.prescriptionDataArray valueForKey:@"DocPhone"] capitalizedString];
    }
    
    NSString* remarks = [self.prescriptionDataArray valueForKey:@"Prescription"];
    if ([remarks isKindOfClass:[NSNull class]] || [remarks isEqualToString:@""]) {
        _prescriptionDetailsTextview.text = @" - ";
    }
    else{
        _prescriptionDetailsTextview.text = [remarks capitalizedString];
    }
    
//    NSArray* foo = [[[self.prescriptionDataArray objectAtIndex:self.indexNumber] valueForKey:@"strPresDate"] componentsSeparatedByString: @"T"];
//    NSString* firstBit = [foo objectAtIndex: 0];
    
    [_prescriptionDateButton setTitle:[self.prescriptionDataArray valueForKey:@"strPresDate"] forState:UIControlStateNormal];
    
//    [_attachmentButton setTitle:[[self.prescriptionDataArray objectAtIndex:self.indexNumber] valueForKey:@"PresDate"] forState:UIControlStateNormal];
    
//    UIBarButtonItem *editButton = [[UIBarButtonItem alloc] initWithTitle:@"Edit" style:UIBarButtonItemStylePlain target:self action:@selector(editButtonAction)];
//    [editButton setTitleTextAttributes:attrs forState:UIControlStateNormal];
//    self.navigationItem.rightBarButtonItem = editButton;
    
    UITapGestureRecognizer *recognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(touch)];
    [recognizer setNumberOfTapsRequired:1];
    [recognizer setNumberOfTouchesRequired:1];
//    [_prescriptionScrollView addGestureRecognizer:recognizer];
    
    int sourceID = [[self.prescriptionDataArray valueForKey:@"SourceId"] intValue];
    
    if (sourceID == 2) {
        self.attachmentLabel.text = @"Prescription Attachment";
        self.attachmentLabel.numberOfLines = 2;
        
        self.attachmentText.hidden = YES;
        self.attachment1ImageView.hidden = YES;
        
        [self.clickButton setTitle:@"Click Here" forState:UIControlStateNormal];
    }
    else if (sourceID == 5){
        self.attachmentLabel.text = @"Doctor Opinion Attachment";
        self.attachmentLabel.numberOfLines = 2;
        
        self.attachmentText.hidden = YES;
        self.attachment1ImageView.hidden = YES;
        
        [self.clickButton setTitle:@"Click Here" forState:UIControlStateNormal];
    }
    else{
        [self.clickButton setHidden:YES];
        
        NSString* imageString = [self.prescriptionDataArray valueForKey:@"arrImages"];
        NSString* userImage = @"";
        
        if (![imageString isEqualToString:@""]) {
            userImage = [NSString stringWithFormat:@""];
        }
        
        dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_HIGH, 0), ^{
            NSData * imageData = [[NSData alloc] initWithContentsOfURL: [NSURL URLWithString:userImage]];
            
            self.attachmentText.hidden = NO;
            self.attachment1ImageView.hidden = NO;
            
            dispatch_async(dispatch_get_main_queue(), ^{
                
                //    [self.attachmentImage setHidden:NO];
                //    self.attachmentText.hidden = NO;
                
                if (imageData) {
                    [self.attachment1ImageView setImage:[UIImage imageWithData:imageData]];
                    self.attachmentText.hidden = YES;
                }
                else{
                    [self.attachment1ImageView setHidden:YES];
                    self.attachmentText.text = @"No attachment";
                }
            });
        });
    }
    
//    _doctorNameTextfield.enabled = NO;
//    _clinicNameTextfield.enabled = NO;
    _addressTextfield.enabled = NO;
    _phoneTextfield.enabled = NO;
    _prescriptionDetailsTextview.editable = NO;
    _prescriptionDateButton.enabled = NO;
//    _attachmentButton.enabled = NO;
    
//    UIColor *color = [UIColor lightGrayColor];
//    self.allergyNameTextfield.attributedPlaceholder = [[NSAttributedString alloc] initWithString:@"Enter allergy name" attributes:@{NSForegroundColorAttributeName: color,
//                                                                                                                                    NSFontAttributeName:[UIFont systemFontOfSize:22 weight:-1],}];
    
}

#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{

    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];

    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.prescriptionScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+450)];
    }
    else{
        [self.prescriptionScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+250)];
    }
}

#pragma mark Enable/Disable Fields Method 
-(void)enableFields:(BOOL)fix{
    _doctorNameTextfield.enabled = fix;
    _clinicNameTextfield.enabled = fix;
    _addressTextfield.enabled = fix;
    _phoneTextfield.enabled = fix;
    _prescriptionDetailsTextview.editable = fix;
    _prescriptionDateButton.enabled = fix;
//    _attachmentButton.enabled = fix;
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
        
        UIImageWriteToSavedPhotosAlbum(self.attachment1ImageView.image, self, nil, nil);
        
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
    
    _imageZoomView = [[XLMediaZoom alloc] initWithAnimationTime:@(0.5) image:self.attachment1ImageView blurEffect:YES];
    _imageZoomView.tag = 1;
    _imageZoomView.backgroundColor = [UIColor colorWithRed:0.0 green:0.05 blue:0.3 alpha:1.0];
    
    return _imageZoomView;
}

-(void)editButtonAction{
    if ([[self.navigationItem.rightBarButtonItem title] isEqualToString:@"Edit"]) {
        [self enableFields:YES];
        [self.navigationItem.rightBarButtonItem setTitle:@"Update"];
    }
    
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

- (IBAction)attachmentButtonAction:(id)sender {
}
- (IBAction)clickButtonAction:(id)sender {
    
    int sourceID = [[self.prescriptionDataArray valueForKey:@"SourceId"] intValue];
    NSString* presId = [self.prescriptionDataArray valueForKey:@"Id"];
    if (sourceID == 2) {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@""]];
    }
    else if (sourceID == 5){
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@""]];
    }
}
@end
