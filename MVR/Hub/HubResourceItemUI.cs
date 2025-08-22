using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.Hub
{
	// Token: 0x02000CC3 RID: 3267
	public class HubResourceItemUI : MonoBehaviour
	{
		// Token: 0x06006247 RID: 25159 RVA: 0x0025BFBA File Offset: 0x0025A3BA
		public HubResourceItemUI()
		{
		}

		// Token: 0x04005311 RID: 21265
		public HubResourceItem connectedItem;

		// Token: 0x04005312 RID: 21266
		public Text titleText;

		// Token: 0x04005313 RID: 21267
		public Text tagLineText;

		// Token: 0x04005314 RID: 21268
		public Text versionText;

		// Token: 0x04005315 RID: 21269
		public Text payTypeText;

		// Token: 0x04005316 RID: 21270
		public Text categoryText;

		// Token: 0x04005317 RID: 21271
		public Button payTypeAndCategorySelectButton;

		// Token: 0x04005318 RID: 21272
		public Button creatorSelectButton;

		// Token: 0x04005319 RID: 21273
		public Image licenseColorImage;

		// Token: 0x0400531A RID: 21274
		public GameObject hasLicenseIndicator;

		// Token: 0x0400531B RID: 21275
		public Text licenseTypeText;

		// Token: 0x0400531C RID: 21276
		public Text creatorText;

		// Token: 0x0400531D RID: 21277
		public RawImage creatorIconImage;

		// Token: 0x0400531E RID: 21278
		public RawImage thumbnailImage;

		// Token: 0x0400531F RID: 21279
		public GameObject hubDownloadableIndicator;

		// Token: 0x04005320 RID: 21280
		public GameObject hubDownloadableNegativeIndicator;

		// Token: 0x04005321 RID: 21281
		public GameObject hubHostedIndicator;

		// Token: 0x04005322 RID: 21282
		public GameObject hubHostedNegativeIndicator;

		// Token: 0x04005323 RID: 21283
		public GameObject hasDependenciesIndicator;

		// Token: 0x04005324 RID: 21284
		public GameObject hasDependenciesNegativeIndicator;

		// Token: 0x04005325 RID: 21285
		public GameObject inLibraryIndicator;

		// Token: 0x04005326 RID: 21286
		public GameObject updateAvailableIndicator;

		// Token: 0x04005327 RID: 21287
		public Text updateMsgText;

		// Token: 0x04005328 RID: 21288
		public Text dependencyCountText;

		// Token: 0x04005329 RID: 21289
		public Text downloadCountText;

		// Token: 0x0400532A RID: 21290
		public Text ratingsCountText;

		// Token: 0x0400532B RID: 21291
		public Slider ratingSlider;

		// Token: 0x0400532C RID: 21292
		public Text lastUpdateText;

		// Token: 0x0400532D RID: 21293
		public Button openDetailButton;
	}
}
