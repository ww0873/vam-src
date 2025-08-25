using System;
using UnityEngine.UI;

// Token: 0x02000D48 RID: 3400
public class AdjustJointsUI : UIProvider
{
	// Token: 0x0600685E RID: 26718 RVA: 0x00271FC0 File Offset: 0x002703C0
	public AdjustJointsUI()
	{
	}

	// Token: 0x04005939 RID: 22841
	public Slider massSlider;

	// Token: 0x0400593A RID: 22842
	public Slider centerOfGravityPercentSlider;

	// Token: 0x0400593B RID: 22843
	public Slider springSlider;

	// Token: 0x0400593C RID: 22844
	public Slider damperSlider;

	// Token: 0x0400593D RID: 22845
	public Slider positionSpringXSlider;

	// Token: 0x0400593E RID: 22846
	public Slider positionSpringYSlider;

	// Token: 0x0400593F RID: 22847
	public Slider positionSpringZSlider;

	// Token: 0x04005940 RID: 22848
	public Slider positionDamperXSlider;

	// Token: 0x04005941 RID: 22849
	public Slider positionDamperYSlider;

	// Token: 0x04005942 RID: 22850
	public Slider positionDamperZSlider;

	// Token: 0x04005943 RID: 22851
	public Slider smoothTargetRotationSpringSlider;

	// Token: 0x04005944 RID: 22852
	public Slider smoothTargetRotationDamperSlider;

	// Token: 0x04005945 RID: 22853
	public Slider targetRotationXSlider;

	// Token: 0x04005946 RID: 22854
	public Slider targetRotationYSlider;

	// Token: 0x04005947 RID: 22855
	public Slider targetRotationZSlider;

	// Token: 0x04005948 RID: 22856
	public Toggle driveXRotationFromAudioSourceToggle;

	// Token: 0x04005949 RID: 22857
	public Slider driveXRotationFromAudioSourceMultiplierSlider;

	// Token: 0x0400594A RID: 22858
	public Slider driveXRotationFromAudioSourceAdditionalAngleSlider;

	// Token: 0x0400594B RID: 22859
	public Slider driveXRotationFromAudioSourceMaxAngleSlider;
}
