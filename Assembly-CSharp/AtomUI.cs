using System;
using UnityEngine.UI;

// Token: 0x02000C04 RID: 3076
public class AtomUI : UIProvider
{
	// Token: 0x06005988 RID: 22920 RVA: 0x0020F230 File Offset: 0x0020D630
	public AtomUI()
	{
	}

	// Token: 0x0400499A RID: 18842
	public Toggle onToggle;

	// Token: 0x0400499B RID: 18843
	public Toggle hiddenToggle;

	// Token: 0x0400499C RID: 18844
	public Toggle collisionEnabledToggle;

	// Token: 0x0400499D RID: 18845
	public Toggle freezePhysicsToggle;

	// Token: 0x0400499E RID: 18846
	public Button resetPhysicsButton;

	// Token: 0x0400499F RID: 18847
	public Slider resetPhysicsProgressSlider;

	// Token: 0x040049A0 RID: 18848
	public Button selectAtomParentFromSceneButton;

	// Token: 0x040049A1 RID: 18849
	public UIPopup parentAtomSelectionPopup;

	// Token: 0x040049A2 RID: 18850
	public InputField idText;

	// Token: 0x040049A3 RID: 18851
	public InputFieldAction idTextAction;

	// Token: 0x040049A4 RID: 18852
	public Text descriptionText;

	// Token: 0x040049A5 RID: 18853
	public Button savePresetButton;

	// Token: 0x040049A6 RID: 18854
	public Button saveAppearancePresetButton;

	// Token: 0x040049A7 RID: 18855
	public Button savePhysicalPresetButton;

	// Token: 0x040049A8 RID: 18856
	public Button loadPresetButton;

	// Token: 0x040049A9 RID: 18857
	public Button loadAppearancePresetButton;

	// Token: 0x040049AA RID: 18858
	public Button loadPhysicalPresetButton;

	// Token: 0x040049AB RID: 18859
	public Button resetButton;

	// Token: 0x040049AC RID: 18860
	public Button resetPhysicalButton;

	// Token: 0x040049AD RID: 18861
	public Button resetAppearanceButton;

	// Token: 0x040049AE RID: 18862
	public Button removeButton;

	// Token: 0x040049AF RID: 18863
	public Toggle keepParamLocksWhenPuttingBackInPoolToggle;
}
