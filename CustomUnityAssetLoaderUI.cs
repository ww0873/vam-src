using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C0D RID: 3085
public class CustomUnityAssetLoaderUI : UIProvider
{
	// Token: 0x060059C1 RID: 22977 RVA: 0x002105E3 File Offset: 0x0020E9E3
	public CustomUnityAssetLoaderUI()
	{
	}

	// Token: 0x040049D5 RID: 18901
	public Button fileBrowseButton;

	// Token: 0x040049D6 RID: 18902
	public Button clearButton;

	// Token: 0x040049D7 RID: 18903
	public Text urlText;

	// Token: 0x040049D8 RID: 18904
	public Toggle importLightmapsToggle;

	// Token: 0x040049D9 RID: 18905
	public Toggle importLightProbesToggle;

	// Token: 0x040049DA RID: 18906
	public Toggle registerCanvasesToggle;

	// Token: 0x040049DB RID: 18907
	public Toggle showCanvasesToggle;

	// Token: 0x040049DC RID: 18908
	public Toggle loadDllToggle;

	// Token: 0x040049DD RID: 18909
	public UIPopup assetSelectionPopup;

	// Token: 0x040049DE RID: 18910
	public RectTransform loadingIndicator;
}
