using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.Hub
{
	// Token: 0x02000CC0 RID: 3264
	public class HubResourceItem
	{
		// Token: 0x0600621A RID: 25114 RVA: 0x0025A50C File Offset: 0x0025890C
		public HubResourceItem(JSONClass resource, HubBrowse hubBrowse, bool queueImagesImmediate = false)
		{
			this.useQueueImmediate = queueImagesImmediate;
			this.browser = hubBrowse;
			this.resource_id = resource["resource_id"];
			this.discussion_thread_id = resource["discussion_thread_id"];
			string startingValue = resource["title"];
			string startingValue2 = resource["tag_line"];
			string startingValue3 = resource["version_string"];
			string startingValue4 = resource["category"];
			string startingValue5 = resource["type"];
			string startingValue6 = resource["username"];
			string text = resource["icon_url"];
			string text2 = resource["image_url"];
			bool asBool = resource["hubDownloadable"].AsBool;
			bool asBool2 = resource["hubHosted"].AsBool;
			int asInt = resource["dependency_count"].AsInt;
			bool startingValue7 = asInt > 0;
			string startingValue8 = resource["download_count"];
			string startingValue9 = resource["rating_count"];
			float asFloat = resource["rating_avg"].AsFloat;
			int asInt2 = resource["last_update"].AsInt;
			this.LastUpdateTimestamp = this.UnixTimeStampToDateTime(asInt2);
			string startingValue10;
			if ((DateTime.Now - this.LastUpdateTimestamp).TotalDays > 7.0)
			{
				startingValue10 = this.LastUpdateTimestamp.ToString("MMM d, yyyy");
			}
			else
			{
				startingValue10 = this.LastUpdateTimestamp.ToString("dddd h:mm tt");
			}
			this.varFilesJSONArray = resource["hubFiles"].AsArray;
			this.titleJSON = new JSONStorableString("title", startingValue);
			this.tagLineJSON = new JSONStorableString("tagLine", startingValue2);
			this.versionNumberJSON = new JSONStorableString("versionNumber", startingValue3);
			this.payTypeJSON = new JSONStorableString("payType", startingValue4);
			this.categoryJSON = new JSONStorableString("category", startingValue5);
			this.payTypeAndCategorySelectAction = new JSONStorableAction("PayTypeAndCategorySelect", new JSONStorableAction.ActionCallback(this.PayTypeAndCategorySelect));
			this.creatorJSON = new JSONStorableString("creator", startingValue6);
			this.creatorSelectAction = new JSONStorableAction("CreatorSelect", new JSONStorableAction.ActionCallback(this.CreatorSelect));
			this.creatorIconUrlJSON = new JSONStorableUrl("creatorIconUrl", text, new JSONStorableString.SetStringCallback(this.SyncCreatorIconUrl));
			this.SyncCreatorIconUrl(text);
			this.thumbnailUrlJSON = new JSONStorableUrl("thumbnailUrl", text2, new JSONStorableString.SetStringCallback(this.SyncThumbnailUrl));
			this.SyncThumbnailUrl(text2);
			this.hubDownloadableJSON = new JSONStorableBool("hubDownloadable", asBool);
			this.hubHostedJSON = new JSONStorableBool("hubHosted", asBool2);
			this.hasDependenciesJSON = new JSONStorableBool("hasDependencies", startingValue7);
			this.dependencyCountJSON = new JSONStorableString("dependencyCount", asInt.ToString() + " Hub-Hosted Dependencies");
			this.downloadCountJSON = new JSONStorableString("downloadCount", startingValue8);
			this.ratingsCountJSON = new JSONStorableString("ratingsCount", startingValue9);
			this.ratingJSON = new JSONStorableFloat("rating", asFloat, 0f, 5f, true, false);
			this.lastUpdateJSON = new JSONStorableString("lastUpdate", startingValue10);
			this.openDetailAction = new JSONStorableAction("OpenDetail", new JSONStorableAction.ActionCallback(this.OpenDetail));
			this.inLibraryJSON = new JSONStorableBool("inLibrary", false);
			this.updateAvailableJSON = new JSONStorableBool("updateAvailable", false);
			this.updateMsgJSON = new JSONStorableString("updateMsg", "Update Available");
			this.licenseTypeJSON = new JSONStorableString("licenseType", string.Empty);
			this.hasLicenseJSON = new JSONStorableBool("hasLicense", false);
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x0600621B RID: 25115 RVA: 0x0025A8FC File Offset: 0x00258CFC
		public string ResourceId
		{
			get
			{
				return this.resource_id;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x0600621C RID: 25116 RVA: 0x0025A904 File Offset: 0x00258D04
		public string Title
		{
			get
			{
				return this.titleJSON.val;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x0600621D RID: 25117 RVA: 0x0025A911 File Offset: 0x00258D11
		public string TagLine
		{
			get
			{
				return this.tagLineJSON.val;
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x0600621E RID: 25118 RVA: 0x0025A91E File Offset: 0x00258D1E
		public string VersionNumber
		{
			get
			{
				return this.versionNumberJSON.val;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x0600621F RID: 25119 RVA: 0x0025A92B File Offset: 0x00258D2B
		public string PayType
		{
			get
			{
				return this.payTypeJSON.val;
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06006220 RID: 25120 RVA: 0x0025A938 File Offset: 0x00258D38
		public string Category
		{
			get
			{
				return this.categoryJSON.val;
			}
		}

		// Token: 0x06006221 RID: 25121 RVA: 0x0025A945 File Offset: 0x00258D45
		protected void PayTypeAndCategorySelect()
		{
			this.browser.SetPayTypeAndCategoryFilter(this.payTypeJSON.val, this.categoryJSON.val, true);
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06006222 RID: 25122 RVA: 0x0025A969 File Offset: 0x00258D69
		public string Creator
		{
			get
			{
				return this.creatorJSON.val;
			}
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x0025A976 File Offset: 0x00258D76
		protected void CreatorSelect()
		{
			this.browser.CreatorFilterOnly = this.creatorJSON.val;
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x0025A990 File Offset: 0x00258D90
		protected void SyncCreatorIconTexture(ImageLoaderThreaded.QueuedImage qi)
		{
			this.creatorIconTexture = qi.tex;
			if (this.creatorIconImage != null && this.creatorIconTexture != null)
			{
				this.creatorIconImage.texture = this.creatorIconTexture;
			}
		}

		// Token: 0x06006225 RID: 25125 RVA: 0x0025A9DC File Offset: 0x00258DDC
		protected void SyncCreatorIconUrl(string url)
		{
			if (ImageLoaderThreaded.singleton != null && url != null && url != string.Empty)
			{
				ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
				queuedImage.imgPath = url;
				queuedImage.callback = new ImageLoaderThreaded.ImageLoaderCallback(this.SyncCreatorIconTexture);
				this.creatorIconQueuedImage = queuedImage;
				if (this.useQueueImmediate)
				{
					ImageLoaderThreaded.singleton.QueueThumbnailImmediate(queuedImage);
				}
				else
				{
					ImageLoaderThreaded.singleton.QueueThumbnail(queuedImage);
				}
			}
		}

		// Token: 0x06006226 RID: 25126 RVA: 0x0025AA5C File Offset: 0x00258E5C
		protected void SyncThumbnailTexture(ImageLoaderThreaded.QueuedImage qi)
		{
			this.thumbnailTexture = qi.tex;
			if (this.thumbnailImage != null && this.thumbnailTexture != null)
			{
				this.thumbnailImage.texture = this.thumbnailTexture;
			}
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x0025AAA8 File Offset: 0x00258EA8
		protected void SyncThumbnailUrl(string url)
		{
			if (ImageLoaderThreaded.singleton != null && url != null && url != string.Empty)
			{
				ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
				queuedImage.imgPath = url;
				queuedImage.callback = new ImageLoaderThreaded.ImageLoaderCallback(this.SyncThumbnailTexture);
				this.thumbnailQueuedImage = queuedImage;
				if (this.useQueueImmediate)
				{
					ImageLoaderThreaded.singleton.QueueThumbnailImmediate(queuedImage);
				}
				else
				{
					ImageLoaderThreaded.singleton.QueueThumbnail(queuedImage);
				}
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06006228 RID: 25128 RVA: 0x0025AB28 File Offset: 0x00258F28
		public int DownloadCount
		{
			get
			{
				int result;
				if (int.TryParse(this.downloadCountJSON.val, out result))
				{
					return result;
				}
				return 0;
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x06006229 RID: 25129 RVA: 0x0025AB50 File Offset: 0x00258F50
		public int RatingsCount
		{
			get
			{
				int result;
				if (int.TryParse(this.ratingsCountJSON.val, out result))
				{
					return result;
				}
				return 0;
			}
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x0600622A RID: 25130 RVA: 0x0025AB77 File Offset: 0x00258F77
		public float Rating
		{
			get
			{
				return this.ratingJSON.val;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x0600622B RID: 25131 RVA: 0x0025AB84 File Offset: 0x00258F84
		// (set) Token: 0x0600622C RID: 25132 RVA: 0x0025AB8C File Offset: 0x00258F8C
		public DateTime LastUpdateTimestamp
		{
			[CompilerGenerated]
			get
			{
				return this.<LastUpdateTimestamp>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<LastUpdateTimestamp>k__BackingField = value;
			}
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x0025AB98 File Offset: 0x00258F98
		protected DateTime UnixTimeStampToDateTime(int unixTimeStamp)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			result = result.AddSeconds((double)unixTimeStamp).ToLocalTime();
			return result;
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x0025ABCC File Offset: 0x00258FCC
		protected void SyncLicenseColor()
		{
			if (this.licenseColorImage != null && this.licenseTypeJSON.val != string.Empty)
			{
				string val = this.licenseTypeJSON.val;
				if (val != null)
				{
					if (val == "FC" || val == "CC BY" || val == "CC BY-SA")
					{
						this.licenseColorImage.color = HubResourceItem.greenColor;
						return;
					}
					if (val == "CC BY-ND")
					{
						this.licenseColorImage.color = HubResourceItem.yellowColor;
						return;
					}
				}
				this.licenseColorImage.color = HubResourceItem.redColor;
			}
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x0025AC9C File Offset: 0x0025909C
		public virtual void Refresh()
		{
			if (this.hubDownloadableJSON.val && this.varFilesJSONArray != null && this.varFilesJSONArray.Count > 0)
			{
				bool val = true;
				bool val2 = false;
				string text = string.Empty;
				bool val3 = false;
				IEnumerator enumerator = this.varFilesJSONArray.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONNode jsonnode = (JSONNode)obj;
						string text2 = jsonnode["filename"];
						if (text2 != null)
						{
							text = jsonnode["licenseType"];
							val3 = (text != null);
							text2 = Regex.Replace(text2, ".var$", string.Empty);
							string packageGroupUid = Regex.Replace(text2, "(.*)\\..*", "$1");
							string s = Regex.Replace(text2, ".*\\.([0-9]+)$", "$1");
							int num;
							if (int.TryParse(s, out num) && FileManager.GetPackage(text2) == null)
							{
								VarPackageGroup packageGroup = FileManager.GetPackageGroup(packageGroupUid);
								if (packageGroup == null || packageGroup.NewestPackage == null)
								{
									val = false;
									break;
								}
								if (packageGroup.NewestPackage.Version < num)
								{
									val2 = true;
									this.updateMsgJSON.val = string.Concat(new object[]
									{
										"Update Available ",
										packageGroup.NewestEnabledPackage.Version,
										" -> ",
										num
									});
								}
							}
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this.inLibraryJSON.val = val;
				this.updateAvailableJSON.val = val2;
				this.licenseTypeJSON.val = text;
				this.hasLicenseJSON.val = val3;
				this.SyncLicenseColor();
			}
			else
			{
				this.inLibraryJSON.val = false;
				this.updateAvailableJSON.val = false;
				this.licenseTypeJSON.val = string.Empty;
				this.hasLicenseJSON.val = false;
			}
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x0025AEC8 File Offset: 0x002592C8
		public void OpenDetail()
		{
			this.browser.OpenDetail(this.resource_id, false);
		}

		// Token: 0x06006231 RID: 25137 RVA: 0x0025AEDC File Offset: 0x002592DC
		public void Hide()
		{
			if (this.creatorIconQueuedImage != null)
			{
				this.creatorIconQueuedImage.cancel = true;
			}
			if (this.thumbnailQueuedImage != null)
			{
				this.thumbnailQueuedImage.cancel = true;
			}
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x0025AF0C File Offset: 0x0025930C
		public void Show()
		{
			if (this.creatorIconQueuedImage != null && !this.creatorIconQueuedImage.preprocessed)
			{
				this.creatorIconQueuedImage.cancel = false;
				if (this.useQueueImmediate)
				{
					ImageLoaderThreaded.singleton.QueueThumbnailImmediate(this.creatorIconQueuedImage);
				}
				else
				{
					ImageLoaderThreaded.singleton.QueueThumbnail(this.creatorIconQueuedImage);
				}
			}
			if (this.thumbnailQueuedImage != null && !this.thumbnailQueuedImage.preprocessed)
			{
				this.thumbnailQueuedImage.cancel = false;
				if (this.useQueueImmediate)
				{
					ImageLoaderThreaded.singleton.QueueThumbnailImmediate(this.thumbnailQueuedImage);
				}
				else
				{
					ImageLoaderThreaded.singleton.QueueThumbnail(this.thumbnailQueuedImage);
				}
			}
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x0025AFC7 File Offset: 0x002593C7
		public void Destroy()
		{
			if (this.creatorIconQueuedImage != null)
			{
				this.creatorIconQueuedImage.cancel = true;
			}
			if (this.thumbnailQueuedImage != null)
			{
				this.thumbnailQueuedImage.cancel = true;
			}
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x0025AFF8 File Offset: 0x002593F8
		public virtual void RegisterUI(HubResourceItemUI ui)
		{
			if (ui != null)
			{
				ui.connectedItem = this;
				this.titleJSON.text = ui.titleText;
				this.tagLineJSON.text = ui.tagLineText;
				this.versionNumberJSON.text = ui.versionText;
				this.payTypeJSON.text = ui.payTypeText;
				this.categoryJSON.text = ui.categoryText;
				this.payTypeAndCategorySelectAction.button = ui.payTypeAndCategorySelectButton;
				this.creatorSelectAction.button = ui.creatorSelectButton;
				this.creatorJSON.text = ui.creatorText;
				this.creatorIconImage = ui.creatorIconImage;
				if (this.creatorIconImage != null && this.creatorIconTexture != null)
				{
					this.creatorIconImage.texture = this.creatorIconTexture;
				}
				this.thumbnailImage = ui.thumbnailImage;
				if (this.thumbnailImage != null && this.thumbnailTexture != null)
				{
					this.thumbnailImage.texture = this.thumbnailTexture;
				}
				this.hubDownloadableJSON.indicator = ui.hubDownloadableIndicator;
				this.hubDownloadableJSON.negativeIndicator = ui.hubDownloadableNegativeIndicator;
				this.hubHostedJSON.indicator = ui.hubHostedIndicator;
				this.hubHostedJSON.negativeIndicator = ui.hubHostedNegativeIndicator;
				this.hasDependenciesJSON.indicator = ui.hasDependenciesIndicator;
				this.hasDependenciesJSON.negativeIndicator = ui.hasDependenciesNegativeIndicator;
				this.dependencyCountJSON.text = ui.dependencyCountText;
				this.downloadCountJSON.text = ui.downloadCountText;
				this.ratingsCountJSON.text = ui.ratingsCountText;
				this.ratingJSON.slider = ui.ratingSlider;
				this.lastUpdateJSON.text = ui.lastUpdateText;
				this.openDetailAction.button = ui.openDetailButton;
				this.inLibraryJSON.indicator = ui.inLibraryIndicator;
				this.licenseColorImage = ui.licenseColorImage;
				this.updateAvailableJSON.indicator = ui.updateAvailableIndicator;
				this.updateMsgJSON.text = ui.updateMsgText;
				this.hasLicenseJSON.indicator = ui.hasLicenseIndicator;
				this.licenseTypeJSON.text = ui.licenseTypeText;
				this.SyncLicenseColor();
			}
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x0025B254 File Offset: 0x00259654
		// Note: this type is marked as 'beforefieldinit'.
		static HubResourceItem()
		{
		}

		// Token: 0x040052AF RID: 21167
		protected HubBrowse browser;

		// Token: 0x040052B0 RID: 21168
		protected string resource_id;

		// Token: 0x040052B1 RID: 21169
		protected string discussion_thread_id;

		// Token: 0x040052B2 RID: 21170
		protected JSONStorableString titleJSON;

		// Token: 0x040052B3 RID: 21171
		protected JSONStorableString tagLineJSON;

		// Token: 0x040052B4 RID: 21172
		protected JSONStorableString versionNumberJSON;

		// Token: 0x040052B5 RID: 21173
		protected JSONStorableString payTypeJSON;

		// Token: 0x040052B6 RID: 21174
		protected JSONStorableString categoryJSON;

		// Token: 0x040052B7 RID: 21175
		protected JSONStorableAction payTypeAndCategorySelectAction;

		// Token: 0x040052B8 RID: 21176
		protected JSONStorableString creatorJSON;

		// Token: 0x040052B9 RID: 21177
		protected JSONStorableAction creatorSelectAction;

		// Token: 0x040052BA RID: 21178
		protected RawImage creatorIconImage;

		// Token: 0x040052BB RID: 21179
		protected Texture2D creatorIconTexture;

		// Token: 0x040052BC RID: 21180
		protected bool useQueueImmediate;

		// Token: 0x040052BD RID: 21181
		protected ImageLoaderThreaded.QueuedImage creatorIconQueuedImage;

		// Token: 0x040052BE RID: 21182
		protected JSONStorableUrl creatorIconUrlJSON;

		// Token: 0x040052BF RID: 21183
		protected RawImage thumbnailImage;

		// Token: 0x040052C0 RID: 21184
		protected Texture2D thumbnailTexture;

		// Token: 0x040052C1 RID: 21185
		protected ImageLoaderThreaded.QueuedImage thumbnailQueuedImage;

		// Token: 0x040052C2 RID: 21186
		protected JSONStorableUrl thumbnailUrlJSON;

		// Token: 0x040052C3 RID: 21187
		protected JSONStorableBool hubDownloadableJSON;

		// Token: 0x040052C4 RID: 21188
		protected JSONStorableBool hubHostedJSON;

		// Token: 0x040052C5 RID: 21189
		protected JSONStorableString dependencyCountJSON;

		// Token: 0x040052C6 RID: 21190
		protected JSONStorableBool hasDependenciesJSON;

		// Token: 0x040052C7 RID: 21191
		protected JSONStorableBool hasLicenseJSON;

		// Token: 0x040052C8 RID: 21192
		protected JSONStorableString licenseTypeJSON;

		// Token: 0x040052C9 RID: 21193
		protected Image licenseColorImage;

		// Token: 0x040052CA RID: 21194
		protected JSONStorableString downloadCountJSON;

		// Token: 0x040052CB RID: 21195
		protected JSONStorableString ratingsCountJSON;

		// Token: 0x040052CC RID: 21196
		protected JSONStorableFloat ratingJSON;

		// Token: 0x040052CD RID: 21197
		protected JSONStorableString lastUpdateJSON;

		// Token: 0x040052CE RID: 21198
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime <LastUpdateTimestamp>k__BackingField;

		// Token: 0x040052CF RID: 21199
		protected JSONArray varFilesJSONArray;

		// Token: 0x040052D0 RID: 21200
		protected JSONStorableBool inLibraryJSON;

		// Token: 0x040052D1 RID: 21201
		protected JSONStorableBool updateAvailableJSON;

		// Token: 0x040052D2 RID: 21202
		protected JSONStorableString updateMsgJSON;

		// Token: 0x040052D3 RID: 21203
		protected static Color redColor = new Color(1f, 0.48f, 0.48f);

		// Token: 0x040052D4 RID: 21204
		protected static Color yellowColor = new Color(1f, 1f, 0.5f);

		// Token: 0x040052D5 RID: 21205
		protected static Color greenColor = new Color(0.52f, 1f, 0.52f);

		// Token: 0x040052D6 RID: 21206
		protected JSONStorableAction openDetailAction;
	}
}
