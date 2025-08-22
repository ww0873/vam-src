using System;
using UnityEngine.UI;

// Token: 0x02000BAA RID: 2986
public class FreeControllerV3UI : UIProvider
{
	// Token: 0x0600554C RID: 21836 RVA: 0x001F3587 File Offset: 0x001F1987
	public FreeControllerV3UI()
	{
	}

	// Token: 0x04004572 RID: 17778
	public Button resetButton;

	// Token: 0x04004573 RID: 17779
	public Button selectRootButton;

	// Token: 0x04004574 RID: 17780
	public Text UIDText;

	// Token: 0x04004575 RID: 17781
	public ToggleGroupValue positionToggleGroup;

	// Token: 0x04004576 RID: 17782
	public ToggleGroupValue rotationToggleGroup;

	// Token: 0x04004577 RID: 17783
	public Slider holdPositionSpringSlider;

	// Token: 0x04004578 RID: 17784
	public Slider holdPositionDamperSlider;

	// Token: 0x04004579 RID: 17785
	public Slider holdPositionMaxForceSlider;

	// Token: 0x0400457A RID: 17786
	public Slider holdRotationSpringSlider;

	// Token: 0x0400457B RID: 17787
	public Slider holdRotationDamperSlider;

	// Token: 0x0400457C RID: 17788
	public Slider holdRotationMaxForceSlider;

	// Token: 0x0400457D RID: 17789
	public Slider complyPositionSpringSlider;

	// Token: 0x0400457E RID: 17790
	public Slider complyPositionDamperSlider;

	// Token: 0x0400457F RID: 17791
	public Slider complyRotationSpringSlider;

	// Token: 0x04004580 RID: 17792
	public Slider complyRotationDamperSlider;

	// Token: 0x04004581 RID: 17793
	public Slider complyJointRotationDriveSpringSlider;

	// Token: 0x04004582 RID: 17794
	public Slider complyPositionThresholdSlider;

	// Token: 0x04004583 RID: 17795
	public Slider complyRotationThresholdSlider;

	// Token: 0x04004584 RID: 17796
	public Slider complySpeedSlider;

	// Token: 0x04004585 RID: 17797
	public Slider linkPositionSpringSlider;

	// Token: 0x04004586 RID: 17798
	public Slider linkPositionDamperSlider;

	// Token: 0x04004587 RID: 17799
	public Slider linkPositionMaxForceSlider;

	// Token: 0x04004588 RID: 17800
	public Slider linkRotationSpringSlider;

	// Token: 0x04004589 RID: 17801
	public Slider linkRotationDamperSlider;

	// Token: 0x0400458A RID: 17802
	public Slider linkRotationMaxForceSlider;

	// Token: 0x0400458B RID: 17803
	public UIPopup linkToSelectionPopup;

	// Token: 0x0400458C RID: 17804
	public UIPopup linkToAtomSelectionPopup;

	// Token: 0x0400458D RID: 17805
	public Slider jointRotationDriveSpringSlider;

	// Token: 0x0400458E RID: 17806
	public Slider jointRotationDriveDamperSlider;

	// Token: 0x0400458F RID: 17807
	public Slider jointRotationDriveMaxForceSlider;

	// Token: 0x04004590 RID: 17808
	public Slider jointRotationDriveXTargetSlider;

	// Token: 0x04004591 RID: 17809
	public Slider jointRotationDriveYTargetSlider;

	// Token: 0x04004592 RID: 17810
	public Slider jointRotationDriveZTargetSlider;

	// Token: 0x04004593 RID: 17811
	public Button selectLinkToFromSceneButton;

	// Token: 0x04004594 RID: 17812
	public Button selectAlignToFromSceneButton;

	// Token: 0x04004595 RID: 17813
	public Toggle onToggle;

	// Token: 0x04004596 RID: 17814
	public Toggle detachControlToggle;

	// Token: 0x04004597 RID: 17815
	public Slider massSlider;

	// Token: 0x04004598 RID: 17816
	public Slider dragSlider;

	// Token: 0x04004599 RID: 17817
	public Toggle maxVelocityEnableToggle;

	// Token: 0x0400459A RID: 17818
	public Slider maxVelocitySlider;

	// Token: 0x0400459B RID: 17819
	public Slider angularDragSlider;

	// Token: 0x0400459C RID: 17820
	public Toggle physicsEnabledToggle;

	// Token: 0x0400459D RID: 17821
	public Toggle collisionEnabledToggle;

	// Token: 0x0400459E RID: 17822
	public Toggle useGravityWhenOffToggle;

	// Token: 0x0400459F RID: 17823
	public Toggle interactableInPlayModeToggle;

	// Token: 0x040045A0 RID: 17824
	public Toggle deactivateOtherControlsOnPossessToggle;

	// Token: 0x040045A1 RID: 17825
	public Text deactivateOtherControlsListText;

	// Token: 0x040045A2 RID: 17826
	public Toggle possessableToggle;

	// Token: 0x040045A3 RID: 17827
	public Toggle canGrabPositionToggle;

	// Token: 0x040045A4 RID: 17828
	public Toggle canGrabRotationToggle;

	// Token: 0x040045A5 RID: 17829
	public Toggle freezeAtomPhysicsWhenGrabbedToggle;

	// Token: 0x040045A6 RID: 17830
	public SetTextFromFloat xPositionText;

	// Token: 0x040045A7 RID: 17831
	public InputField xPositionInputField;

	// Token: 0x040045A8 RID: 17832
	public InputFieldAction xPositionInputFieldAction;

	// Token: 0x040045A9 RID: 17833
	public Button xPositionMinus1Button;

	// Token: 0x040045AA RID: 17834
	public Button xPositionMinusPoint1Button;

	// Token: 0x040045AB RID: 17835
	public Button xPositionMinusPoint01Button;

	// Token: 0x040045AC RID: 17836
	public Button xPosition0Button;

	// Token: 0x040045AD RID: 17837
	public Button xPositionPlusPoint01Button;

	// Token: 0x040045AE RID: 17838
	public Button xPositionPlusPoint1Button;

	// Token: 0x040045AF RID: 17839
	public Button xPositionPlus1Button;

	// Token: 0x040045B0 RID: 17840
	public Button xPositionSnapPoint1Button;

	// Token: 0x040045B1 RID: 17841
	public Toggle xPositionLockToggle;

	// Token: 0x040045B2 RID: 17842
	public Toggle xPositionLocalLockToggle;

	// Token: 0x040045B3 RID: 17843
	public SetTextFromFloat yPositionText;

	// Token: 0x040045B4 RID: 17844
	public InputField yPositionInputField;

	// Token: 0x040045B5 RID: 17845
	public InputFieldAction yPositionInputFieldAction;

	// Token: 0x040045B6 RID: 17846
	public Button yPositionMinus1Button;

	// Token: 0x040045B7 RID: 17847
	public Button yPositionMinusPoint1Button;

	// Token: 0x040045B8 RID: 17848
	public Button yPositionMinusPoint01Button;

	// Token: 0x040045B9 RID: 17849
	public Button yPosition0Button;

	// Token: 0x040045BA RID: 17850
	public Button yPositionPlusPoint01Button;

	// Token: 0x040045BB RID: 17851
	public Button yPositionPlusPoint1Button;

	// Token: 0x040045BC RID: 17852
	public Button yPositionPlus1Button;

	// Token: 0x040045BD RID: 17853
	public Button yPositionSnapPoint1Button;

	// Token: 0x040045BE RID: 17854
	public Toggle yPositionLockToggle;

	// Token: 0x040045BF RID: 17855
	public Toggle yPositionLocalLockToggle;

	// Token: 0x040045C0 RID: 17856
	public SetTextFromFloat zPositionText;

	// Token: 0x040045C1 RID: 17857
	public InputField zPositionInputField;

	// Token: 0x040045C2 RID: 17858
	public InputFieldAction zPositionInputFieldAction;

	// Token: 0x040045C3 RID: 17859
	public Button zPositionMinus1Button;

	// Token: 0x040045C4 RID: 17860
	public Button zPositionMinusPoint1Button;

	// Token: 0x040045C5 RID: 17861
	public Button zPositionMinusPoint01Button;

	// Token: 0x040045C6 RID: 17862
	public Button zPosition0Button;

	// Token: 0x040045C7 RID: 17863
	public Button zPositionPlusPoint01Button;

	// Token: 0x040045C8 RID: 17864
	public Button zPositionPlusPoint1Button;

	// Token: 0x040045C9 RID: 17865
	public Button zPositionPlus1Button;

	// Token: 0x040045CA RID: 17866
	public Button zPositionSnapPoint1Button;

	// Token: 0x040045CB RID: 17867
	public Toggle zPositionLockToggle;

	// Token: 0x040045CC RID: 17868
	public Toggle zPositionLocalLockToggle;

	// Token: 0x040045CD RID: 17869
	public SetTextFromFloat xRotationText;

	// Token: 0x040045CE RID: 17870
	public InputField xRotationInputField;

	// Token: 0x040045CF RID: 17871
	public InputFieldAction xRotationInputFieldAction;

	// Token: 0x040045D0 RID: 17872
	public Button xRotationMinus45Button;

	// Token: 0x040045D1 RID: 17873
	public Button xRotationMinus5Button;

	// Token: 0x040045D2 RID: 17874
	public Button xRotationMinusPoint5Button;

	// Token: 0x040045D3 RID: 17875
	public Button xRotation0Button;

	// Token: 0x040045D4 RID: 17876
	public Button xRotationPlusPoint5Button;

	// Token: 0x040045D5 RID: 17877
	public Button xRotationPlus5Button;

	// Token: 0x040045D6 RID: 17878
	public Button xRotationPlus45Button;

	// Token: 0x040045D7 RID: 17879
	public Button xRotationSnap1Button;

	// Token: 0x040045D8 RID: 17880
	public Toggle xRotationLockToggle;

	// Token: 0x040045D9 RID: 17881
	public SetTextFromFloat yRotationText;

	// Token: 0x040045DA RID: 17882
	public InputField yRotationInputField;

	// Token: 0x040045DB RID: 17883
	public InputFieldAction yRotationInputFieldAction;

	// Token: 0x040045DC RID: 17884
	public Button yRotationMinus45Button;

	// Token: 0x040045DD RID: 17885
	public Button yRotationMinus5Button;

	// Token: 0x040045DE RID: 17886
	public Button yRotationMinusPoint5Button;

	// Token: 0x040045DF RID: 17887
	public Button yRotation0Button;

	// Token: 0x040045E0 RID: 17888
	public Button yRotationPlusPoint5Button;

	// Token: 0x040045E1 RID: 17889
	public Button yRotationPlus5Button;

	// Token: 0x040045E2 RID: 17890
	public Button yRotationPlus45Button;

	// Token: 0x040045E3 RID: 17891
	public Button yRotationSnap1Button;

	// Token: 0x040045E4 RID: 17892
	public Toggle yRotationLockToggle;

	// Token: 0x040045E5 RID: 17893
	public SetTextFromFloat zRotationText;

	// Token: 0x040045E6 RID: 17894
	public InputField zRotationInputField;

	// Token: 0x040045E7 RID: 17895
	public InputFieldAction zRotationInputFieldAction;

	// Token: 0x040045E8 RID: 17896
	public Button zRotationMinus45Button;

	// Token: 0x040045E9 RID: 17897
	public Button zRotationMinus5Button;

	// Token: 0x040045EA RID: 17898
	public Button zRotationMinusPoint5Button;

	// Token: 0x040045EB RID: 17899
	public Button zRotation0Button;

	// Token: 0x040045EC RID: 17900
	public Button zRotationPlusPoint5Button;

	// Token: 0x040045ED RID: 17901
	public Button zRotationPlus5Button;

	// Token: 0x040045EE RID: 17902
	public Button zRotationPlus45Button;

	// Token: 0x040045EF RID: 17903
	public Button zRotationSnap1Button;

	// Token: 0x040045F0 RID: 17904
	public Toggle zRotationLockToggle;

	// Token: 0x040045F1 RID: 17905
	public SetTextFromFloat xLocalPositionText;

	// Token: 0x040045F2 RID: 17906
	public InputField xLocalPositionInputField;

	// Token: 0x040045F3 RID: 17907
	public InputFieldAction xLocalPositionInputFieldAction;

	// Token: 0x040045F4 RID: 17908
	public SetTextFromFloat yLocalPositionText;

	// Token: 0x040045F5 RID: 17909
	public InputField yLocalPositionInputField;

	// Token: 0x040045F6 RID: 17910
	public InputFieldAction yLocalPositionInputFieldAction;

	// Token: 0x040045F7 RID: 17911
	public SetTextFromFloat zLocalPositionText;

	// Token: 0x040045F8 RID: 17912
	public InputField zLocalPositionInputField;

	// Token: 0x040045F9 RID: 17913
	public InputFieldAction zLocalPositionInputFieldAction;

	// Token: 0x040045FA RID: 17914
	public SetTextFromFloat xLocalRotationText;

	// Token: 0x040045FB RID: 17915
	public InputField xLocalRotationInputField;

	// Token: 0x040045FC RID: 17916
	public InputFieldAction xLocalRotationInputFieldAction;

	// Token: 0x040045FD RID: 17917
	public SetTextFromFloat yLocalRotationText;

	// Token: 0x040045FE RID: 17918
	public InputField yLocalRotationInputField;

	// Token: 0x040045FF RID: 17919
	public InputFieldAction yLocalRotationInputFieldAction;

	// Token: 0x04004600 RID: 17920
	public SetTextFromFloat zLocalRotationText;

	// Token: 0x04004601 RID: 17921
	public InputField zLocalRotationInputField;

	// Token: 0x04004602 RID: 17922
	public InputFieldAction zLocalRotationInputFieldAction;

	// Token: 0x04004603 RID: 17923
	public Button zeroXLocalPositionButton;

	// Token: 0x04004604 RID: 17924
	public Button zeroYLocalPositionButton;

	// Token: 0x04004605 RID: 17925
	public Button zeroZLocalPositionButton;

	// Token: 0x04004606 RID: 17926
	public Button zeroXLocalRotationButton;

	// Token: 0x04004607 RID: 17927
	public Button zeroYLocalRotationButton;

	// Token: 0x04004608 RID: 17928
	public Button zeroZLocalRotationButton;

	// Token: 0x04004609 RID: 17929
	public InputField xSelfRelativePositionAdjustInputField;

	// Token: 0x0400460A RID: 17930
	public InputFieldAction xSelfRelativePositionAdjustInputFieldAction;

	// Token: 0x0400460B RID: 17931
	public Button xSelfRelativePositionMinus1Button;

	// Token: 0x0400460C RID: 17932
	public Button xSelfRelativePositionMinusPoint1Button;

	// Token: 0x0400460D RID: 17933
	public Button xSelfRelativePositionMinusPoint01Button;

	// Token: 0x0400460E RID: 17934
	public Button xSelfRelativePositionPlusPoint01Button;

	// Token: 0x0400460F RID: 17935
	public Button xSelfRelativePositionPlusPoint1Button;

	// Token: 0x04004610 RID: 17936
	public Button xSelfRelativePositionPlus1Button;

	// Token: 0x04004611 RID: 17937
	public InputField ySelfRelativePositionAdjustInputField;

	// Token: 0x04004612 RID: 17938
	public InputFieldAction ySelfRelativePositionAdjustInputFieldAction;

	// Token: 0x04004613 RID: 17939
	public Button ySelfRelativePositionMinus1Button;

	// Token: 0x04004614 RID: 17940
	public Button ySelfRelativePositionMinusPoint1Button;

	// Token: 0x04004615 RID: 17941
	public Button ySelfRelativePositionMinusPoint01Button;

	// Token: 0x04004616 RID: 17942
	public Button ySelfRelativePositionPlusPoint01Button;

	// Token: 0x04004617 RID: 17943
	public Button ySelfRelativePositionPlusPoint1Button;

	// Token: 0x04004618 RID: 17944
	public Button ySelfRelativePositionPlus1Button;

	// Token: 0x04004619 RID: 17945
	public InputField zSelfRelativePositionAdjustInputField;

	// Token: 0x0400461A RID: 17946
	public InputFieldAction zSelfRelativePositionAdjustInputFieldAction;

	// Token: 0x0400461B RID: 17947
	public Button zSelfRelativePositionMinus1Button;

	// Token: 0x0400461C RID: 17948
	public Button zSelfRelativePositionMinusPoint1Button;

	// Token: 0x0400461D RID: 17949
	public Button zSelfRelativePositionMinusPoint01Button;

	// Token: 0x0400461E RID: 17950
	public Button zSelfRelativePositionPlusPoint01Button;

	// Token: 0x0400461F RID: 17951
	public Button zSelfRelativePositionPlusPoint1Button;

	// Token: 0x04004620 RID: 17952
	public Button zSelfRelativePositionPlus1Button;

	// Token: 0x04004621 RID: 17953
	public InputField xSelfRelativeRotationAdjustInputField;

	// Token: 0x04004622 RID: 17954
	public InputFieldAction xSelfRelativeRotationAdjustInputFieldAction;

	// Token: 0x04004623 RID: 17955
	public Button xSelfRelativeRotationMinus45Button;

	// Token: 0x04004624 RID: 17956
	public Button xSelfRelativeRotationMinus5Button;

	// Token: 0x04004625 RID: 17957
	public Button xSelfRelativeRotationMinusPoint5Button;

	// Token: 0x04004626 RID: 17958
	public Button xSelfRelativeRotationPlusPoint5Button;

	// Token: 0x04004627 RID: 17959
	public Button xSelfRelativeRotationPlus5Button;

	// Token: 0x04004628 RID: 17960
	public Button xSelfRelativeRotationPlus45Button;

	// Token: 0x04004629 RID: 17961
	public InputField ySelfRelativeRotationAdjustInputField;

	// Token: 0x0400462A RID: 17962
	public InputFieldAction ySelfRelativeRotationAdjustInputFieldAction;

	// Token: 0x0400462B RID: 17963
	public Button ySelfRelativeRotationMinus45Button;

	// Token: 0x0400462C RID: 17964
	public Button ySelfRelativeRotationMinus5Button;

	// Token: 0x0400462D RID: 17965
	public Button ySelfRelativeRotationMinusPoint5Button;

	// Token: 0x0400462E RID: 17966
	public Button ySelfRelativeRotationPlusPoint5Button;

	// Token: 0x0400462F RID: 17967
	public Button ySelfRelativeRotationPlus5Button;

	// Token: 0x04004630 RID: 17968
	public Button ySelfRelativeRotationPlus45Button;

	// Token: 0x04004631 RID: 17969
	public InputField zSelfRelativeRotationAdjustInputField;

	// Token: 0x04004632 RID: 17970
	public InputFieldAction zSelfRelativeRotationAdjustInputFieldAction;

	// Token: 0x04004633 RID: 17971
	public Button zSelfRelativeRotationMinus45Button;

	// Token: 0x04004634 RID: 17972
	public Button zSelfRelativeRotationMinus5Button;

	// Token: 0x04004635 RID: 17973
	public Button zSelfRelativeRotationMinusPoint5Button;

	// Token: 0x04004636 RID: 17974
	public Button zSelfRelativeRotationPlusPoint5Button;

	// Token: 0x04004637 RID: 17975
	public Button zSelfRelativeRotationPlus5Button;

	// Token: 0x04004638 RID: 17976
	public Button zSelfRelativeRotationPlus45Button;

	// Token: 0x04004639 RID: 17977
	public UIPopup positionGridModePopup;

	// Token: 0x0400463A RID: 17978
	public Slider positionGridSlider;

	// Token: 0x0400463B RID: 17979
	public UIPopup rotationGridModePopup;

	// Token: 0x0400463C RID: 17980
	public Slider rotationGridSlider;
}
