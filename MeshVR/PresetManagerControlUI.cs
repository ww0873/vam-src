using System;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000CFC RID: 3324
	public class PresetManagerControlUI : UIProvider
	{
		// Token: 0x06006533 RID: 25907 RVA: 0x001AE481 File Offset: 0x001AC881
		public PresetManagerControlUI()
		{
		}

		// Token: 0x040054DE RID: 21726
		public Button browsePresetsButton;

		// Token: 0x040054DF RID: 21727
		public Button openPresetBrowsePathInExplorerButton;

		// Token: 0x040054E0 RID: 21728
		public InputField presetNameField;

		// Token: 0x040054E1 RID: 21729
		public Toggle storePresetNameToggle;

		// Token: 0x040054E2 RID: 21730
		public Toggle loadPresetOnSelectToggle;

		// Token: 0x040054E3 RID: 21731
		public Toggle useMergeLoadToggle;

		// Token: 0x040054E4 RID: 21732
		public Toggle useMergeLoadBrowserToggle;

		// Token: 0x040054E5 RID: 21733
		public UIPopup favoriteSelectionPopup;

		// Token: 0x040054E6 RID: 21734
		public UIDynamicButton storePresetButton;

		// Token: 0x040054E7 RID: 21735
		public UIDynamicButton storePresetWithScreenshotButton;

		// Token: 0x040054E8 RID: 21736
		public UIDynamicButton storeOverlayPresetButton;

		// Token: 0x040054E9 RID: 21737
		public UIDynamicButton storeOverlayPresetWithScreenshotButton;

		// Token: 0x040054EA RID: 21738
		public UIDynamicButton loadPresetButton;

		// Token: 0x040054EB RID: 21739
		public UIDynamicButton loadDefaultsButton;

		// Token: 0x040054EC RID: 21740
		public UIDynamicButton loadUserDefaultsButton;

		// Token: 0x040054ED RID: 21741
		public UIDynamicButton storeUserDefaultsButton;

		// Token: 0x040054EE RID: 21742
		public UIDynamicButton clearUserDefaultsButton;

		// Token: 0x040054EF RID: 21743
		public Toggle favoriteToggle;

		// Token: 0x040054F0 RID: 21744
		public Toggle storeOptionalToggle;

		// Token: 0x040054F1 RID: 21745
		public Toggle storeOptional2Toggle;

		// Token: 0x040054F2 RID: 21746
		public Toggle storeOptional3Toggle;

		// Token: 0x040054F3 RID: 21747
		public Toggle storePresetBinaryToggle;

		// Token: 0x040054F4 RID: 21748
		public Toggle includeOptionalToggle;

		// Token: 0x040054F5 RID: 21749
		public Toggle includeOptional2Toggle;

		// Token: 0x040054F6 RID: 21750
		public Toggle includeOptional3Toggle;

		// Token: 0x040054F7 RID: 21751
		public Toggle includePhysicalToggle;

		// Token: 0x040054F8 RID: 21752
		public Toggle includeAppearanceToggle;

		// Token: 0x040054F9 RID: 21753
		public Toggle lockParamsToggle;

		// Token: 0x040054FA RID: 21754
		public Text statusText;
	}
}
