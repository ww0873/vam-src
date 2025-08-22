using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BEC RID: 3052
	public class PackageInfoUI : UIProvider
	{
		// Token: 0x060057C5 RID: 22469 RVA: 0x002041AC File Offset: 0x002025AC
		public PackageInfoUI()
		{
		}

		// Token: 0x04004885 RID: 18565
		public RectTransform packageInfoPanel;

		// Token: 0x04004886 RID: 18566
		public Text packageUidText;

		// Token: 0x04004887 RID: 18567
		public Button openPackageInManagerButton;

		// Token: 0x04004888 RID: 18568
		public Button openOnHubButton;

		// Token: 0x04004889 RID: 18569
		public Button promotionalButton;

		// Token: 0x0400488A RID: 18570
		public Text promotionalButtonText;

		// Token: 0x0400488B RID: 18571
		public Button copyPromotionalLinkButton;

		// Token: 0x0400488C RID: 18572
		public RectTransform descriptionContainer;

		// Token: 0x0400488D RID: 18573
		public InputField descriptionField;

		// Token: 0x0400488E RID: 18574
		public RectTransform instructionsContainer;

		// Token: 0x0400488F RID: 18575
		public InputField instructionsField;
	}
}
