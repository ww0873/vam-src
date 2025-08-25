using System;
using UnityEngine.UI;

// Token: 0x02000D62 RID: 3426
public class ForceProducerV2UI : UIProvider
{
	// Token: 0x06006974 RID: 26996 RVA: 0x002769DB File Offset: 0x00274DDB
	public ForceProducerV2UI()
	{
	}

	// Token: 0x04005A70 RID: 23152
	public Toggle onToggle;

	// Token: 0x04005A71 RID: 23153
	public Toggle useForceToggle;

	// Token: 0x04005A72 RID: 23154
	public Toggle useTorqueToggle;

	// Token: 0x04005A73 RID: 23155
	public Slider forceFactorSlider;

	// Token: 0x04005A74 RID: 23156
	public Slider torqueFactorSlider;

	// Token: 0x04005A75 RID: 23157
	public Slider maxForceSlider;

	// Token: 0x04005A76 RID: 23158
	public Slider maxTorqueSlider;

	// Token: 0x04005A77 RID: 23159
	public Slider forceQuicknessSlider;

	// Token: 0x04005A78 RID: 23160
	public Slider torqueQuicknessSlider;

	// Token: 0x04005A79 RID: 23161
	public Button selectReceiverAtomFromSceneButton;

	// Token: 0x04005A7A RID: 23162
	public UIPopup receiverAtomSelectionPopup;

	// Token: 0x04005A7B RID: 23163
	public UIPopup receiverSelectionPopup;
}
