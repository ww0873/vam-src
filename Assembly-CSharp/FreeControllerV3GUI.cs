using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BA8 RID: 2984
public class FreeControllerV3GUI : MonoBehaviour
{
	// Token: 0x06005542 RID: 21826 RVA: 0x001F2CF8 File Offset: 0x001F10F8
	public FreeControllerV3GUI()
	{
	}

	// Token: 0x06005543 RID: 21827 RVA: 0x001F2D00 File Offset: 0x001F1100
	protected void ResyncController()
	{
		if (this._controller != null)
		{
			this._controller.linkToSelectionPopup = this.linkToSelectionPopup;
			this._controller.linkToAtomSelectionPopup = this.linkToAtomSelectionPopup;
			this._controller.xPositionText = this.xPositionText;
			this._controller.yPositionText = this.yPositionText;
			this._controller.zPositionText = this.zPositionText;
			this._controller.xRotationText = this.xRotationText;
			this._controller.yRotationText = this.yRotationText;
			this._controller.zRotationText = this.zRotationText;
			this._controller.UITransforms = new Transform[1];
			this._controller.UITransforms[0] = base.transform;
			if (this.UIDText != null)
			{
				this.UIDText.text = this._controller.name;
			}
			this._controller.UIDText = this.UIDText;
			if (this.headerText != null)
			{
				this.headerText.text = string.Empty;
			}
		}
		if (this.controllerUI != null)
		{
			this.controllerUI.positionToggleGroup = this.positionToggleGroup;
			this.controllerUI.rotationToggleGroup = this.rotationToggleGroup;
			this.controllerUI.holdPositionSpringSlider = this.holdPositionSpringSlider;
			this.controllerUI.holdPositionDamperSlider = this.holdPositionDamperSlider;
			this.controllerUI.holdPositionMaxForceSlider = this.holdPositionMaxForceSlider;
			this.controllerUI.holdRotationSpringSlider = this.holdRotationSpringSlider;
			this.controllerUI.holdRotationDamperSlider = this.holdRotationDamperSlider;
			this.controllerUI.holdRotationMaxForceSlider = this.holdRotationMaxForceSlider;
			this.controllerUI.linkPositionSpringSlider = this.linkPositionSpringSlider;
			this.controllerUI.linkPositionDamperSlider = this.linkPositionDamperSlider;
			this.controllerUI.linkPositionMaxForceSlider = this.linkPositionMaxForceSlider;
			this.controllerUI.linkRotationSpringSlider = this.linkRotationSpringSlider;
			this.controllerUI.linkRotationDamperSlider = this.linkRotationDamperSlider;
			this.controllerUI.linkRotationMaxForceSlider = this.linkRotationMaxForceSlider;
			this.controllerUI.linkToSelectionPopup = this.linkToSelectionPopup;
			this.controllerUI.linkToAtomSelectionPopup = this.linkToAtomSelectionPopup;
			this.controllerUI.selectLinkToFromSceneButton = this.selectLinkToFromSceneButton;
			this.controllerUI.selectAlignToFromSceneButton = this.selectAlignToFromSceneButton;
			this.controllerUI.jointRotationDriveSpringSlider = this.jointRotationDriveSpringSlider;
			this.controllerUI.jointRotationDriveDamperSlider = this.jointRotationDriveDamperSlider;
			this.controllerUI.jointRotationDriveMaxForceSlider = this.jointRotationDriveMaxForceSlider;
			this.controllerUI.jointRotationDriveXTargetSlider = this.jointRotationDriveXTargetSlider;
			this.controllerUI.jointRotationDriveYTargetSlider = this.jointRotationDriveYTargetSlider;
			this.controllerUI.jointRotationDriveZTargetSlider = this.jointRotationDriveZTargetSlider;
			this.controllerUI.massSlider = this.massSlider;
			this.controllerUI.physicsEnabledToggle = this.physicsEnabledToggle;
			this.controllerUI.onToggle = this.onToggle;
			this.controllerUI.collisionEnabledToggle = this.collisionEnabledToggle;
			this.controllerUI.useGravityWhenOffToggle = this.useGravityWhenOffToggle;
			this.controllerUI.interactableInPlayModeToggle = this.interactableInPlayModeToggle;
			this.controllerUI.possessableToggle = this.possessableToggle;
			this.controllerUI.canGrabPositionToggle = this.canGrabPositionToggle;
			this.controllerUI.canGrabRotationToggle = this.canGrabRotationToggle;
			this.controllerUI.xPositionLockToggle = this.xPositionLockToggle;
			this.controllerUI.yPositionLockToggle = this.yPositionLockToggle;
			this.controllerUI.zPositionLockToggle = this.zPositionLockToggle;
			this.controllerUI.xRotationLockToggle = this.xRotationLockToggle;
			this.controllerUI.yRotationLockToggle = this.yRotationLockToggle;
			this.controllerUI.zRotationLockToggle = this.zRotationLockToggle;
			this.controllerUI.xPositionMinus1Button = this.xPositionMinus1Button;
			this.controllerUI.xPositionMinusPoint1Button = this.xPositionMinusPoint1Button;
			this.controllerUI.xPositionMinusPoint01Button = this.xPositionMinusPoint01Button;
			this.controllerUI.xPositionPlusPoint01Button = this.xPositionPlusPoint01Button;
			this.controllerUI.xPositionPlusPoint1Button = this.xPositionPlusPoint1Button;
			this.controllerUI.xPositionPlus1Button = this.xPositionPlus1Button;
			this.controllerUI.xPosition0Button = this.xPosition0Button;
			this.controllerUI.xPositionText = this.xPositionText;
			this.controllerUI.xPositionSnapPoint1Button = this.xPositionSnapPoint1Button;
			this.controllerUI.yPositionMinus1Button = this.yPositionMinus1Button;
			this.controllerUI.yPositionMinusPoint1Button = this.yPositionMinusPoint1Button;
			this.controllerUI.yPositionMinusPoint01Button = this.yPositionMinusPoint01Button;
			this.controllerUI.yPositionPlusPoint01Button = this.yPositionPlusPoint01Button;
			this.controllerUI.yPositionPlusPoint1Button = this.yPositionPlusPoint1Button;
			this.controllerUI.yPositionPlus1Button = this.yPositionPlus1Button;
			this.controllerUI.yPosition0Button = this.yPosition0Button;
			this.controllerUI.yPositionText = this.yPositionText;
			this.controllerUI.yPositionSnapPoint1Button = this.yPositionSnapPoint1Button;
			this.controllerUI.zPositionMinus1Button = this.zPositionMinus1Button;
			this.controllerUI.zPositionMinusPoint1Button = this.zPositionMinusPoint1Button;
			this.controllerUI.zPositionMinusPoint01Button = this.zPositionMinusPoint01Button;
			this.controllerUI.zPositionPlusPoint01Button = this.zPositionPlusPoint01Button;
			this.controllerUI.zPositionPlusPoint1Button = this.zPositionPlusPoint1Button;
			this.controllerUI.zPositionPlus1Button = this.zPositionPlus1Button;
			this.controllerUI.zPosition0Button = this.zPosition0Button;
			this.controllerUI.zPositionText = this.zPositionText;
			this.controllerUI.zPositionSnapPoint1Button = this.zPositionSnapPoint1Button;
			this.controllerUI.xRotationMinus45Button = this.xRotationMinus45Button;
			this.controllerUI.xRotationMinus5Button = this.xRotationMinus5Button;
			this.controllerUI.xRotationMinusPoint5Button = this.xRotationMinusPoint5Button;
			this.controllerUI.xRotationPlusPoint5Button = this.xRotationPlusPoint5Button;
			this.controllerUI.xRotationPlus5Button = this.xRotationPlus5Button;
			this.controllerUI.xRotationPlus45Button = this.xRotationPlus45Button;
			this.controllerUI.xRotation0Button = this.xRotation0Button;
			this.controllerUI.xRotationText = this.xRotationText;
			this.controllerUI.xRotationSnap1Button = this.xRotationSnap1Button;
			this.controllerUI.yRotationMinus45Button = this.yRotationMinus45Button;
			this.controllerUI.yRotationMinus5Button = this.yRotationMinus5Button;
			this.controllerUI.yRotationMinusPoint5Button = this.yRotationMinusPoint5Button;
			this.controllerUI.yRotationPlusPoint5Button = this.yRotationPlusPoint5Button;
			this.controllerUI.yRotationPlus5Button = this.yRotationPlus5Button;
			this.controllerUI.yRotationPlus45Button = this.yRotationPlus45Button;
			this.controllerUI.yRotation0Button = this.yRotation0Button;
			this.controllerUI.yRotationText = this.yRotationText;
			this.controllerUI.yRotationSnap1Button = this.yRotationSnap1Button;
			this.controllerUI.zRotationMinus45Button = this.zRotationMinus45Button;
			this.controllerUI.zRotationMinus5Button = this.zRotationMinus5Button;
			this.controllerUI.zRotationMinusPoint5Button = this.zRotationMinusPoint5Button;
			this.controllerUI.zRotationPlusPoint5Button = this.zRotationPlusPoint5Button;
			this.controllerUI.zRotationPlus5Button = this.zRotationPlus5Button;
			this.controllerUI.zRotationPlus45Button = this.zRotationPlus45Button;
			this.controllerUI.zRotation0Button = this.zRotation0Button;
			this.controllerUI.zRotationText = this.zRotationText;
			this.controllerUI.zRotationSnap1Button = this.zRotationSnap1Button;
			this.controllerUI.UIDText = this.UIDText;
		}
	}

	// Token: 0x17000C77 RID: 3191
	// (get) Token: 0x06005544 RID: 21828 RVA: 0x001F3472 File Offset: 0x001F1872
	// (set) Token: 0x06005545 RID: 21829 RVA: 0x001F347A File Offset: 0x001F187A
	public FreeControllerV3 controller
	{
		get
		{
			return this._controller;
		}
		set
		{
			if (this._controller != value)
			{
				this._controller = value;
				this.ResyncController();
			}
		}
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x001F349C File Offset: 0x001F189C
	protected void ResyncMotionController()
	{
		if (this._motionController != null && this._controller != null)
		{
			this._motionController.overrideId = this._controller.name + "Animation";
		}
	}

	// Token: 0x17000C78 RID: 3192
	// (get) Token: 0x06005547 RID: 21831 RVA: 0x001F34EB File Offset: 0x001F18EB
	// (set) Token: 0x06005548 RID: 21832 RVA: 0x001F34F3 File Offset: 0x001F18F3
	public MotionAnimationControl motionController
	{
		get
		{
			return this._motionController;
		}
		set
		{
			if (this._motionController != value)
			{
				this._motionController = value;
				this.ResyncMotionController();
			}
		}
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x001F3513 File Offset: 0x001F1913
	public void ResyncAll()
	{
		this.ResyncController();
		this.ResyncMotionController();
	}

	// Token: 0x0400450C RID: 17676
	[SerializeField]
	[HideInInspector]
	protected FreeControllerV3 _controller;

	// Token: 0x0400450D RID: 17677
	public FreeControllerV3UI controllerUI;

	// Token: 0x0400450E RID: 17678
	[SerializeField]
	[HideInInspector]
	protected MotionAnimationControl _motionController;

	// Token: 0x0400450F RID: 17679
	public Text headerText;

	// Token: 0x04004510 RID: 17680
	public Text UIDText;

	// Token: 0x04004511 RID: 17681
	public ToggleGroupValue positionToggleGroup;

	// Token: 0x04004512 RID: 17682
	public ToggleGroupValue rotationToggleGroup;

	// Token: 0x04004513 RID: 17683
	public Slider holdPositionSpringSlider;

	// Token: 0x04004514 RID: 17684
	public Slider holdPositionDamperSlider;

	// Token: 0x04004515 RID: 17685
	public Slider holdPositionMaxForceSlider;

	// Token: 0x04004516 RID: 17686
	public Slider holdRotationSpringSlider;

	// Token: 0x04004517 RID: 17687
	public Slider holdRotationDamperSlider;

	// Token: 0x04004518 RID: 17688
	public Slider holdRotationMaxForceSlider;

	// Token: 0x04004519 RID: 17689
	public Slider linkPositionSpringSlider;

	// Token: 0x0400451A RID: 17690
	public Slider linkPositionDamperSlider;

	// Token: 0x0400451B RID: 17691
	public Slider linkPositionMaxForceSlider;

	// Token: 0x0400451C RID: 17692
	public Slider linkRotationSpringSlider;

	// Token: 0x0400451D RID: 17693
	public Slider linkRotationDamperSlider;

	// Token: 0x0400451E RID: 17694
	public Slider linkRotationMaxForceSlider;

	// Token: 0x0400451F RID: 17695
	public UIPopup linkToSelectionPopup;

	// Token: 0x04004520 RID: 17696
	public UIPopup linkToAtomSelectionPopup;

	// Token: 0x04004521 RID: 17697
	public Slider jointRotationDriveSpringSlider;

	// Token: 0x04004522 RID: 17698
	public Slider jointRotationDriveDamperSlider;

	// Token: 0x04004523 RID: 17699
	public Slider jointRotationDriveMaxForceSlider;

	// Token: 0x04004524 RID: 17700
	public Slider jointRotationDriveXTargetSlider;

	// Token: 0x04004525 RID: 17701
	public Slider jointRotationDriveYTargetSlider;

	// Token: 0x04004526 RID: 17702
	public Slider jointRotationDriveZTargetSlider;

	// Token: 0x04004527 RID: 17703
	public Button selectLinkToFromSceneButton;

	// Token: 0x04004528 RID: 17704
	public Button selectAlignToFromSceneButton;

	// Token: 0x04004529 RID: 17705
	public Toggle onToggle;

	// Token: 0x0400452A RID: 17706
	public Slider massSlider;

	// Token: 0x0400452B RID: 17707
	public Toggle physicsEnabledToggle;

	// Token: 0x0400452C RID: 17708
	public Toggle collisionEnabledToggle;

	// Token: 0x0400452D RID: 17709
	public Toggle useGravityWhenOffToggle;

	// Token: 0x0400452E RID: 17710
	public Toggle interactableInPlayModeToggle;

	// Token: 0x0400452F RID: 17711
	public Toggle possessableToggle;

	// Token: 0x04004530 RID: 17712
	public Toggle canGrabPositionToggle;

	// Token: 0x04004531 RID: 17713
	public Toggle canGrabRotationToggle;

	// Token: 0x04004532 RID: 17714
	public SetTextFromFloat xPositionText;

	// Token: 0x04004533 RID: 17715
	public Button xPositionMinus1Button;

	// Token: 0x04004534 RID: 17716
	public Button xPositionMinusPoint1Button;

	// Token: 0x04004535 RID: 17717
	public Button xPositionMinusPoint01Button;

	// Token: 0x04004536 RID: 17718
	public Button xPosition0Button;

	// Token: 0x04004537 RID: 17719
	public Button xPositionPlusPoint01Button;

	// Token: 0x04004538 RID: 17720
	public Button xPositionPlusPoint1Button;

	// Token: 0x04004539 RID: 17721
	public Button xPositionPlus1Button;

	// Token: 0x0400453A RID: 17722
	public Button xPositionSnapPoint1Button;

	// Token: 0x0400453B RID: 17723
	public Toggle xPositionLockToggle;

	// Token: 0x0400453C RID: 17724
	public SetTextFromFloat yPositionText;

	// Token: 0x0400453D RID: 17725
	public Button yPositionMinus1Button;

	// Token: 0x0400453E RID: 17726
	public Button yPositionMinusPoint1Button;

	// Token: 0x0400453F RID: 17727
	public Button yPositionMinusPoint01Button;

	// Token: 0x04004540 RID: 17728
	public Button yPosition0Button;

	// Token: 0x04004541 RID: 17729
	public Button yPositionPlusPoint01Button;

	// Token: 0x04004542 RID: 17730
	public Button yPositionPlusPoint1Button;

	// Token: 0x04004543 RID: 17731
	public Button yPositionPlus1Button;

	// Token: 0x04004544 RID: 17732
	public Button yPositionSnapPoint1Button;

	// Token: 0x04004545 RID: 17733
	public Toggle yPositionLockToggle;

	// Token: 0x04004546 RID: 17734
	public SetTextFromFloat zPositionText;

	// Token: 0x04004547 RID: 17735
	public Button zPositionMinus1Button;

	// Token: 0x04004548 RID: 17736
	public Button zPositionMinusPoint1Button;

	// Token: 0x04004549 RID: 17737
	public Button zPositionMinusPoint01Button;

	// Token: 0x0400454A RID: 17738
	public Button zPosition0Button;

	// Token: 0x0400454B RID: 17739
	public Button zPositionPlusPoint01Button;

	// Token: 0x0400454C RID: 17740
	public Button zPositionPlusPoint1Button;

	// Token: 0x0400454D RID: 17741
	public Button zPositionPlus1Button;

	// Token: 0x0400454E RID: 17742
	public Button zPositionSnapPoint1Button;

	// Token: 0x0400454F RID: 17743
	public Toggle zPositionLockToggle;

	// Token: 0x04004550 RID: 17744
	public SetTextFromFloat xRotationText;

	// Token: 0x04004551 RID: 17745
	public Button xRotationMinus45Button;

	// Token: 0x04004552 RID: 17746
	public Button xRotationMinus5Button;

	// Token: 0x04004553 RID: 17747
	public Button xRotationMinusPoint5Button;

	// Token: 0x04004554 RID: 17748
	public Button xRotation0Button;

	// Token: 0x04004555 RID: 17749
	public Button xRotationPlusPoint5Button;

	// Token: 0x04004556 RID: 17750
	public Button xRotationPlus5Button;

	// Token: 0x04004557 RID: 17751
	public Button xRotationPlus45Button;

	// Token: 0x04004558 RID: 17752
	public Button xRotationSnap1Button;

	// Token: 0x04004559 RID: 17753
	public Toggle xRotationLockToggle;

	// Token: 0x0400455A RID: 17754
	public SetTextFromFloat yRotationText;

	// Token: 0x0400455B RID: 17755
	public Button yRotationMinus45Button;

	// Token: 0x0400455C RID: 17756
	public Button yRotationMinus5Button;

	// Token: 0x0400455D RID: 17757
	public Button yRotationMinusPoint5Button;

	// Token: 0x0400455E RID: 17758
	public Button yRotation0Button;

	// Token: 0x0400455F RID: 17759
	public Button yRotationPlusPoint5Button;

	// Token: 0x04004560 RID: 17760
	public Button yRotationPlus5Button;

	// Token: 0x04004561 RID: 17761
	public Button yRotationPlus45Button;

	// Token: 0x04004562 RID: 17762
	public Button yRotationSnap1Button;

	// Token: 0x04004563 RID: 17763
	public Toggle yRotationLockToggle;

	// Token: 0x04004564 RID: 17764
	public SetTextFromFloat zRotationText;

	// Token: 0x04004565 RID: 17765
	public Button zRotationMinus45Button;

	// Token: 0x04004566 RID: 17766
	public Button zRotationMinus5Button;

	// Token: 0x04004567 RID: 17767
	public Button zRotationMinusPoint5Button;

	// Token: 0x04004568 RID: 17768
	public Button zRotation0Button;

	// Token: 0x04004569 RID: 17769
	public Button zRotationPlusPoint5Button;

	// Token: 0x0400456A RID: 17770
	public Button zRotationPlus5Button;

	// Token: 0x0400456B RID: 17771
	public Button zRotationPlus45Button;

	// Token: 0x0400456C RID: 17772
	public Button zRotationSnap1Button;

	// Token: 0x0400456D RID: 17773
	public Toggle zRotationLockToggle;

	// Token: 0x0400456E RID: 17774
	public Toggle armedForRecordToggle;

	// Token: 0x0400456F RID: 17775
	public Toggle playbackEnabledToggle;

	// Token: 0x04004570 RID: 17776
	public Button clearAnimationButton;
}
