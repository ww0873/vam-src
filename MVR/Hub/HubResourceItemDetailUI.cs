using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.Hub
{
	// Token: 0x02000CC2 RID: 3266
	public class HubResourceItemDetailUI : HubResourceItemUI
	{
		// Token: 0x06006246 RID: 25158 RVA: 0x0025BFC2 File Offset: 0x0025A3C2
		public HubResourceItemDetailUI()
		{
		}

		// Token: 0x040052F6 RID: 21238
		public new HubResourceItemDetail connectedItem;

		// Token: 0x040052F7 RID: 21239
		public Button closeDetailButton;

		// Token: 0x040052F8 RID: 21240
		public Button closeDetailButtonAlt;

		// Token: 0x040052F9 RID: 21241
		public GameObject hadErrorIndicator;

		// Token: 0x040052FA RID: 21242
		public Text errorText;

		// Token: 0x040052FB RID: 21243
		public Button navigateToOverviewButton;

		// Token: 0x040052FC RID: 21244
		public GameObject hasUpdatesIndicator;

		// Token: 0x040052FD RID: 21245
		public Text updatesText;

		// Token: 0x040052FE RID: 21246
		public Button navigateToUpdatesButton;

		// Token: 0x040052FF RID: 21247
		public GameObject hasReviewsIndicator;

		// Token: 0x04005300 RID: 21248
		public Text reviewsText;

		// Token: 0x04005301 RID: 21249
		public Button navigateToReviewsButton;

		// Token: 0x04005302 RID: 21250
		public Button navigateToHistoryButton;

		// Token: 0x04005303 RID: 21251
		public Button navigateToDiscussionButton;

		// Token: 0x04005304 RID: 21252
		public GameObject hasPromotionalLinkIndicator;

		// Token: 0x04005305 RID: 21253
		public Text promotionalLinkText;

		// Token: 0x04005306 RID: 21254
		public Button navigateToPromotionalLinkButton;

		// Token: 0x04005307 RID: 21255
		public PointerEnterExitAction promtionalLinkButtonEnterExitAction;

		// Token: 0x04005308 RID: 21256
		public GameObject hasOtherCreatorsIndicator;

		// Token: 0x04005309 RID: 21257
		public RectTransform creatorSupportContent;

		// Token: 0x0400530A RID: 21258
		public GameObject hubDownloadableIndicatorAlt;

		// Token: 0x0400530B RID: 21259
		public GameObject hubDownloadableNegativeIndicatorAlt;

		// Token: 0x0400530C RID: 21260
		public Text externalDownloadUrl;

		// Token: 0x0400530D RID: 21261
		public Button goToExternalDownloadUrlButton;

		// Token: 0x0400530E RID: 21262
		public RectTransform packageContent;

		// Token: 0x0400530F RID: 21263
		public Button downloadAllButton;

		// Token: 0x04005310 RID: 21264
		public GameObject downloadAvailableIndicator;
	}
}
