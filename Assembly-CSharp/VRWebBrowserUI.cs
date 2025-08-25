using System;
using UnityEngine.UI;

// Token: 0x02000E32 RID: 3634
public class VRWebBrowserUI : UIProvider
{
	// Token: 0x06007018 RID: 28696 RVA: 0x002A2C58 File Offset: 0x002A1058
	public VRWebBrowserUI()
	{
	}

	// Token: 0x040061BF RID: 25023
	public Toggle fullMouseClickOnDownToggle;

	// Token: 0x040061C0 RID: 25024
	public Toggle disableInteractionToggle;

	// Token: 0x040061C1 RID: 25025
	public InputField urlInput;

	// Token: 0x040061C2 RID: 25026
	public InputFieldAction urlInputAction;

	// Token: 0x040061C3 RID: 25027
	public Text navigatedURLText;

	// Token: 0x040061C4 RID: 25028
	public Text hoveredURLText;

	// Token: 0x040061C5 RID: 25029
	public Button goButton;

	// Token: 0x040061C6 RID: 25030
	public Button backButton;

	// Token: 0x040061C7 RID: 25031
	public Button forwardButton;

	// Token: 0x040061C8 RID: 25032
	public Button copyToClipboardButton;

	// Token: 0x040061C9 RID: 25033
	public Button copyFromClipboardButton;

	// Token: 0x040061CA RID: 25034
	public Button homeButton;

	// Token: 0x040061CB RID: 25035
	public UIPopup quickSitesPopup;
}
