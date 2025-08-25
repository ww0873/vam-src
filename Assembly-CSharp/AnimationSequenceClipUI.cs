using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B5E RID: 2910
public class AnimationSequenceClipUI : UIProvider
{
	// Token: 0x06005137 RID: 20791 RVA: 0x001D4BD7 File Offset: 0x001D2FD7
	public AnimationSequenceClipUI()
	{
	}

	// Token: 0x040040F9 RID: 16633
	public Text nameText;

	// Token: 0x040040FA RID: 16634
	public GameObject isPlayingIndicator;

	// Token: 0x040040FB RID: 16635
	public Slider playProgressSlider;

	// Token: 0x040040FC RID: 16636
	public GameObject useCrossFadeIndicator;

	// Token: 0x040040FD RID: 16637
	public Slider crossFadeTimeSlider;

	// Token: 0x040040FE RID: 16638
	public Button removeButton;

	// Token: 0x040040FF RID: 16639
	public Button moveBackwardButton;

	// Token: 0x04004100 RID: 16640
	public Button moveForwardButton;
}
