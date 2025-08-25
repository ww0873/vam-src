using System;
using UnityEngine;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

namespace MVR.Hub
{
	// Token: 0x02000CB8 RID: 3256
	public class HubBrowseUI : UIProvider
	{
		// Token: 0x060061E9 RID: 25065 RVA: 0x00258C52 File Offset: 0x00257052
		public HubBrowseUI()
		{
		}

		// Token: 0x0400524D RID: 21069
		public GameObject hubEnabledNegativeIndicator;

		// Token: 0x0400524E RID: 21070
		public Button enableHubButton;

		// Token: 0x0400524F RID: 21071
		public GameObject webBrowserEnabledNegativeIndicator;

		// Token: 0x04005250 RID: 21072
		public Button enabledWebBrowserButton;

		// Token: 0x04005251 RID: 21073
		public RectTransform refreshingGetInfoPanel;

		// Token: 0x04005252 RID: 21074
		public RectTransform failedGetInfoPanel;

		// Token: 0x04005253 RID: 21075
		public Text getInfoErrorText;

		// Token: 0x04005254 RID: 21076
		public Button cancelGetHubInfoButton;

		// Token: 0x04005255 RID: 21077
		public Button retryGetHubInfoButton;

		// Token: 0x04005256 RID: 21078
		public RectTransform itemContainer;

		// Token: 0x04005257 RID: 21079
		public ScrollRect itemScrollRect;

		// Token: 0x04005258 RID: 21080
		public GameObject refreshIndicator;

		// Token: 0x04005259 RID: 21081
		public Button refreshButton;

		// Token: 0x0400525A RID: 21082
		public Text numResourcesText;

		// Token: 0x0400525B RID: 21083
		public Text pageInfoText;

		// Token: 0x0400525C RID: 21084
		public Button firstPageButton;

		// Token: 0x0400525D RID: 21085
		public Button previousPageButton;

		// Token: 0x0400525E RID: 21086
		public Button nextPageButton;

		// Token: 0x0400525F RID: 21087
		public Button clearFiltersButton;

		// Token: 0x04005260 RID: 21088
		public UIPopup hostedOptionPopup;

		// Token: 0x04005261 RID: 21089
		public UIPopup payTypeFilterPopup;

		// Token: 0x04005262 RID: 21090
		public UIPopup categoryFilterPopup;

		// Token: 0x04005263 RID: 21091
		public UIPopup creatorFilterPopup;

		// Token: 0x04005264 RID: 21092
		public UIPopup tagsFilterPopup;

		// Token: 0x04005265 RID: 21093
		public InputField searchInputField;

		// Token: 0x04005266 RID: 21094
		public Toggle searchAllToggle;

		// Token: 0x04005267 RID: 21095
		public UIPopup sortPrimaryPopup;

		// Token: 0x04005268 RID: 21096
		public UIPopup sortSecondaryPopup;

		// Token: 0x04005269 RID: 21097
		public GameObject detailPanel;

		// Token: 0x0400526A RID: 21098
		public RectTransform resourceDetailContainer;

		// Token: 0x0400526B RID: 21099
		public Browser browser;

		// Token: 0x0400526C RID: 21100
		public VRWebBrowser webBrowser;

		// Token: 0x0400526D RID: 21101
		public GameObject isWebLoadingIndicator;

		// Token: 0x0400526E RID: 21102
		public GameObject missingPackagesPanel;

		// Token: 0x0400526F RID: 21103
		public RectTransform missingPackagesContainer;

		// Token: 0x04005270 RID: 21104
		public Button openMissingPackagesPanelButton;

		// Token: 0x04005271 RID: 21105
		public Button closeMissingPackagesPanelButton;

		// Token: 0x04005272 RID: 21106
		public Button downloadAllMissingPackagesButton;

		// Token: 0x04005273 RID: 21107
		public GameObject updatesPanel;

		// Token: 0x04005274 RID: 21108
		public RectTransform updatesContainer;

		// Token: 0x04005275 RID: 21109
		public Button openUpdatesPanelButton;

		// Token: 0x04005276 RID: 21110
		public Button closeUpdatesPanelButton;

		// Token: 0x04005277 RID: 21111
		public Button downloadAllUpdatesButton;

		// Token: 0x04005278 RID: 21112
		public GameObject isDownloadingIndicator;

		// Token: 0x04005279 RID: 21113
		public Text downloadQueuedCountText;

		// Token: 0x0400527A RID: 21114
		public Button openDownloadingButton;
	}
}
