using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D17 RID: 3351
public class ImageControlUI : UIProvider
{
	// Token: 0x0600667B RID: 26235 RVA: 0x0026C23E File Offset: 0x0026A63E
	public ImageControlUI()
	{
	}

	// Token: 0x040055FF RID: 22015
	public Button loadButton;

	// Token: 0x04005600 RID: 22016
	public Button copyToClipboardButton;

	// Token: 0x04005601 RID: 22017
	public Button copyFromClipboardButton;

	// Token: 0x04005602 RID: 22018
	public Button clearUrlButton;

	// Token: 0x04005603 RID: 22019
	public Button fileBrowseButton;

	// Token: 0x04005604 RID: 22020
	public InputField urlInputField;

	// Token: 0x04005605 RID: 22021
	public InputFieldAction urlInputFieldAction;

	// Token: 0x04005606 RID: 22022
	public Toggle allowImageTilingToggle;

	// Token: 0x04005607 RID: 22023
	public Toggle playVideoWhenReadyToggle;

	// Token: 0x04005608 RID: 22024
	public Toggle useAnamorphicVideoAspectRatioToggle;

	// Token: 0x04005609 RID: 22025
	public GameObject videoIsReadyIndicator;

	// Token: 0x0400560A RID: 22026
	public GameObject videoIsLoadingIndicator;

	// Token: 0x0400560B RID: 22027
	public GameObject videoHadErrorIndicator;

	// Token: 0x0400560C RID: 22028
	public Toggle loopVideoToggle;

	// Token: 0x0400560D RID: 22029
	public Slider playbackTimeSilder;

	// Token: 0x0400560E RID: 22030
	public Button playVideoButton;

	// Token: 0x0400560F RID: 22031
	public Button pauseVideoButton;

	// Token: 0x04005610 RID: 22032
	public Button stopVideoButton;

	// Token: 0x04005611 RID: 22033
	public Button seekToVideoStartButton;
}
