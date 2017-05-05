//
//  MedicationDetailViewController.m
//  PHR
//
//  Created by CDAC HIED on 18/11/15.
//  Copyright (c) 2015 CDAC HIED. All rights reserved.
//

#import "MedicationDetailViewController.h"
#import "Constants.h"
#import "XLMediaZoom.h"
#import "XLVideoZoom.h"

@interface MedicationDetailViewController (){
    UITapGestureRecognizer* tapGesture;
}

@property (strong, nonatomic) XLMediaZoom *imageZoomView;

@property (weak, nonatomic) IBOutlet UIScrollView *medicationScrollView;
@property (weak, nonatomic) IBOutlet UILabel *medicineNameTextfield;
@property (weak, nonatomic) IBOutlet UITextField *takingMedicineTextfield;
@property (weak, nonatomic) IBOutlet UITextField *prescribedTextfield;
@property (weak, nonatomic) IBOutlet UITextField *routeTextfield;
@property (weak, nonatomic) IBOutlet UITextField *strengthTextfield;
@property (weak, nonatomic) IBOutlet UITextField *dosageTakenTextfield;
@property (weak, nonatomic) IBOutlet UILabel *frequencyTakenLabel;
@property (weak, nonatomic) IBOutlet UITextView *instructionLabel;
@property (weak, nonatomic) IBOutlet UITextView *notesLabel;
@property (weak, nonatomic) IBOutlet UIImageView *attachmentImageView1;
@property (weak, nonatomic) IBOutlet UITextField *attachmentText;

@property (weak, nonatomic) IBOutlet UILabel *medicineNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *takingMedicationLabel;
@property (weak, nonatomic) IBOutlet UILabel *dateLabel;
@property (weak, nonatomic) IBOutlet UILabel *routeLabel;
@property (weak, nonatomic) IBOutlet UILabel *strengthLabel;
@property (weak, nonatomic) IBOutlet UILabel *dosageLabel;
@property (weak, nonatomic) IBOutlet UILabel *frequencyLabel;
@property (weak, nonatomic) IBOutlet UILabel *instructionNameLabel;
@property (weak, nonatomic) IBOutlet UILabel *noteLabel;
@property (weak, nonatomic) IBOutlet UILabel *attachmentLabel;


@end

@implementation MedicationDetailViewController
@synthesize imageZoomView   = _imageZoomView;
@synthesize medicationDataArray, attachmentImageView1;


-(void)viewWillLayoutSubviews{
    
    [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+550)];
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    tapGesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(imageDidTouch:)];
    tapGesture.delegate = self;
    tapGesture.numberOfTapsRequired = 1;
    tapGesture.cancelsTouchesInView = NO;
    
    NSDictionary *attrs;
    if ([[kAppDelegate checkDeviceType] isEqualToString:iPhone] || [[kAppDelegate checkDeviceType]isEqualToString:iPhone5]) {
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:25 weight:-1]
                  };
        
        self.medicineNameTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.takingMedicineTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.prescribedTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.routeTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.strengthTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.dosageTakenTextfield.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.frequencyTakenLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.instructionLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.notesLabel.font = [UIFont systemFontOfSize:16.0f weight:-1];
        self.attachmentText.font = [UIFont systemFontOfSize:16.0f weight:-1];
        
        self.medicineNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.takingMedicationLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dateLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.routeLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.strengthLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.dosageLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.frequencyLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.instructionNameLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.noteLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        self.attachmentLabel.font = [UIFont systemFontOfSize:18.0f weight:-1];
        
    }
    else{
        attrs = @{
                  NSFontAttributeName:[UIFont systemFontOfSize:30 weight:-1]
                  };
    }
    
    NSMutableAttributedString *attributedText =
    [[NSMutableAttributedString alloc] initWithString:@"Medication Details"
                                           attributes:attrs];
    
    UILabel* titleLabel = [[UILabel alloc]initWithFrame:CGRectMake(0, 100, 100, 30)];
    titleLabel.attributedText = attributedText;
    
    self.navigationItem.titleView=titleLabel;
    
    
//    [self.medicationScrollView bringSubviewToFront:self.attachmentImageView1];
    [self.attachmentImageView1 setUserInteractionEnabled:YES];
    [self.attachmentImageView1 addGestureRecognizer:tapGesture];
    
    UILongPressGestureRecognizer *lpgr = [[UILongPressGestureRecognizer alloc]
                        initWithTarget:self action:@selector(handleLongPress:)];
    lpgr.delegate = self;
    lpgr.delaysTouchesBegan = YES;
    [self.view addGestureRecognizer:lpgr];
    
    self.medicineNameTextfield.text = [[self.medicationDataArray valueForKey:@"MedicineName"] capitalizedString];
    
    NSString* takingMedicine = [self.medicationDataArray valueForKey:@"strTakingMedicine"];
    self.takingMedicineTextfield.text = takingMedicine;
    
    NSString* prescribedDate = [self.medicationDataArray valueForKey:@"strPrescribedDate"];
    self.prescribedTextfield.text = prescribedDate;
    
    self.routeTextfield.text = [NSString stringWithFormat:@"%@",[self.medicationDataArray valueForKey:@"strRoute"]];
    
    NSString* notesString = [self.medicationDataArray valueForKey:@"Notes"];
    if ([notesString isEqualToString:@""]) {
        self.notesLabel.text = @"-";
    }
    else{
        self.notesLabel.text = notesString;
    }
    
    NSString* strength = [self.medicationDataArray valueForKey:@"Strength"];
    if ([strength isEqualToString:@""]) {
        self.strengthTextfield.text = @"-";
    }
    else{
        self.strengthTextfield.text = strength;
    }
    
    NSString* instruction = [self.medicationDataArray valueForKey:@"LabelInstructions"];
    if ([instruction isEqualToString:@""]) {
        self.instructionLabel.text = @"-";
    }
    else{
        self.instructionLabel.text = instruction;
    }
    
    self.dosageTakenTextfield.text = [NSString stringWithFormat:@"%@ %@",[self.medicationDataArray valueForKey:@"strDosValue"], [self.medicationDataArray valueForKey:@"strDosUnit"] ];
    self.frequencyTakenLabel.text = [NSString stringWithFormat:@"%@",[self.medicationDataArray valueForKey:@"strFrequency"]];
    
    NSString* imageString = [self.medicationDataArray valueForKey:@"arrImages"];
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
                [self.attachmentImageView1 setImage:[UIImage imageWithData:imageData]];
                self.attachmentText.hidden = YES;
                
            }
            else{
                self.attachmentText.text = @"No attachment";
                [self.attachmentImageView1 setHidden:YES];
            }
        });
    });
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
        
        UIImageWriteToSavedPhotosAlbum(self.attachmentImageView1.image, self, nil, nil);
        
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
    
    _imageZoomView = [[XLMediaZoom alloc] initWithAnimationTime:@(0.5) image:self.attachmentImageView1 blurEffect:YES];
    _imageZoomView.tag = 1;
    _imageZoomView.backgroundColor = [UIColor colorWithRed:0.0 green:0.05 blue:0.3 alpha:1.0];
    
    return _imageZoomView;
}


#pragma mark Device Orientation Method 
-(void) didRotateFromInterfaceOrientation:(UIInterfaceOrientation)fromInterfaceOrientation{
    
    UIDeviceOrientation Orientation=[[UIDevice currentDevice]orientation];
    
    if(Orientation==UIDeviceOrientationLandscapeLeft || Orientation==UIDeviceOrientationLandscapeRight){
        [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+600)];
    }
    else{
        [self.medicationScrollView setContentSize:CGSizeMake(self.view.frame.size.width, self.view.frame.size.height+350)];
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

@end
