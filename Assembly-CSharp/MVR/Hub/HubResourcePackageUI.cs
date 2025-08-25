using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.Hub
{
	// Token: 0x02000CCA RID: 3274
	public class HubResourcePackageUI : MonoBehaviour
	{
		// Token: 0x0600627E RID: 25214 RVA: 0x0025CDD1 File Offset: 0x0025B1D1
		public HubResourcePackageUI()
		{
		}

		// Token: 0x04005355 RID: 21333
		public HubResourcePackage connectedItem;

		// Token: 0x04005356 RID: 21334
		public Button resourceButton;

		// Token: 0x04005357 RID: 21335
		public Text nameText;

		// Token: 0x04005358 RID: 21336
		public Text licenseTypeText;

		// Token: 0x04005359 RID: 21337
		public Text fileSizeText;

		// Token: 0x0400535A RID: 21338
		public GameObject isDependencyIndicator;

		// Token: 0x0400535B RID: 21339
		public GameObject notOnHubIndicator;

		// Token: 0x0400535C RID: 21340
		public GameObject alreadyHaveIndicator;

		// Token: 0x0400535D RID: 21341
		public Button openInPackageManagerButton;

		// Token: 0x0400535E RID: 21342
		public GameObject alreadyHaveSceneIndicator;

		// Token: 0x0400535F RID: 21343
		public Button openSceneButton;

		// Token: 0x04005360 RID: 21344
		public Button downloadButton;

		// Token: 0x04005361 RID: 21345
		public GameObject updateAvailableIndicator;

		// Token: 0x04005362 RID: 21346
		public Button updateButton;

		// Token: 0x04005363 RID: 21347
		public Text updateMsgText;

		// Token: 0x04005364 RID: 21348
		public GameObject isDownloadQueuedIndicator;

		// Token: 0x04005365 RID: 21349
		public GameObject isDownloadingIndicator;

		// Token: 0x04005366 RID: 21350
		public GameObject isDownloadedIndicator;

		// Token: 0x04005367 RID: 21351
		public Slider progressSlider;
	}
}
