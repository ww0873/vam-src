using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.Hub
{
	// Token: 0x02000CBE RID: 3262
	public class HubDownloaderUI : UIProvider
	{
		// Token: 0x06006218 RID: 25112 RVA: 0x0025A4F9 File Offset: 0x002588F9
		public HubDownloaderUI()
		{
		}

		// Token: 0x0400529F RID: 21151
		public RectTransform panel;

		// Token: 0x040052A0 RID: 21152
		public RectTransform packagesContainer;

		// Token: 0x040052A1 RID: 21153
		public Text infoText;

		// Token: 0x040052A2 RID: 21154
		public Text downloadInfoText;

		// Token: 0x040052A3 RID: 21155
		public Button openPanelButton;

		// Token: 0x040052A4 RID: 21156
		public Button closePanelButton;

		// Token: 0x040052A5 RID: 21157
		public Button clearTrackedPackagesButton;

		// Token: 0x040052A6 RID: 21158
		public InputField packageNameInputField;

		// Token: 0x040052A7 RID: 21159
		public Button findPackageButton;

		// Token: 0x040052A8 RID: 21160
		public Button downloadAllTrackedPackagesButton;

		// Token: 0x040052A9 RID: 21161
		public GameObject disabledIndicator;

		// Token: 0x040052AA RID: 21162
		public Button enableHubDownloaderButton;

		// Token: 0x040052AB RID: 21163
		public Button rejectHubDownloaderButton;
	}
}
