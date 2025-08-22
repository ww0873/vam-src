using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

namespace MVR.Hub
{
	// Token: 0x02000CC1 RID: 3265
	public class HubResourceItemDetail : HubResourceItem
	{
		// Token: 0x06006236 RID: 25142 RVA: 0x0025B2AC File Offset: 0x002596AC
		public HubResourceItemDetail(JSONClass resource, HubBrowse hubBrowse) : base(resource, hubBrowse, true)
		{
			if (this.resource_id != null)
			{
				this.resourceOverviewUrl = "https://hub.virtamate.com/resources/" + this.resource_id + "/overview-panel";
				this.resourceUpdatesUrl = "https://hub.virtamate.com/resources/" + this.resource_id + "/updates-panel";
				this.resourceReviewsUrl = "https://hub.virtamate.com/resources/" + this.resource_id + "/review-panel";
				this.resourceHistoryUrl = "https://hub.virtamate.com/resources/" + this.resource_id + "/history-panel";
			}
			if (this.discussion_thread_id != null)
			{
				this.resourceDiscussionUrl = "https://hub.virtamate.com/threads/" + this.discussion_thread_id + "/discussion-panel";
			}
			bool flag = false;
			string startingValue = string.Empty;
			string text = resource["status"];
			if (text != null && text == "error")
			{
				flag = true;
				startingValue = resource["error"];
			}
			string startingValue2 = resource["download_url"];
			this.promotionalUrl = resource["promotional_link"];
			this.dependencies = resource["dependencies"].AsObject;
			int asInt = resource["review_count"].AsInt;
			bool startingValue3 = asInt > 0;
			int asInt2 = resource["update_count"].AsInt;
			bool startingValue4 = asInt2 > 0;
			if (flag)
			{
				this.browser.NavigateWebPanel("about:blank");
			}
			else
			{
				this.NavigateToOverview();
			}
			this.hadErrorJSON = new JSONStorableBool("hadError", flag);
			this.errorJSON = new JSONStorableString("error", startingValue);
			this.closeDetailAction = new JSONStorableAction("CloseDetail", new JSONStorableAction.ActionCallback(this.CloseDetail));
			this.navigateToOverviewAction = new JSONStorableAction("NavigateToOverview", new JSONStorableAction.ActionCallback(this.NavigateToOverview));
			this.navigateToUpdatesAction = new JSONStorableAction("NavigateToUpdates", new JSONStorableAction.ActionCallback(this.NavigateToUpdates));
			this.hasUpdatesJSON = new JSONStorableBool("hasUpdates", startingValue4);
			this.updatesTextJSON = new JSONStorableString("updatesText", "Updates (" + asInt2 + ")");
			this.navigateToReviewsAction = new JSONStorableAction("NavigateToReviews", new JSONStorableAction.ActionCallback(this.NavigateToReviews));
			this.hasReviewsJSON = new JSONStorableBool("hasReviews", startingValue3);
			this.reviewsTextJSON = new JSONStorableString("reviewsText", "Reviews (" + asInt + ")");
			this.navigateToHistoryAction = new JSONStorableAction("NavigateToHistory", new JSONStorableAction.ActionCallback(this.NavigateToHistory));
			this.navigateToDiscussionAction = new JSONStorableAction("NavigateToDiscussion", new JSONStorableAction.ActionCallback(this.NavigateToDiscussion));
			this.hasPromotionalLinkJSON = new JSONStorableBool("hasPromotionalLink", this.promotionalUrl != null && this.promotionalUrl != string.Empty && this.promotionalUrl != "null");
			this.navigateToPromotionalLinkAction = new JSONStorableAction("NavigateToPromotionalLink", new JSONStorableAction.ActionCallback(this.NavigateToPromotionalLink));
			this.promotionalLinkTextJSON = new JSONStorableString("promotionalLinkText", base.Creator);
			this.hasOtherCreatorsJSON = new JSONStorableBool("hasOtherCreators", false);
			this.externalDownloadUrl = new JSONStorableString("externalDownloadUrl", startingValue2);
			this.goToExternalDownloadAction = new JSONStorableAction("GoToExternalDownload", new JSONStorableAction.ActionCallback(this.GoToExternalDownload));
			this.downloadAllAction = new JSONStorableAction("DownloadAll", new JSONStorableAction.ActionCallback(this.DownloadAll));
			this.downloadAvailableJSON = new JSONStorableBool("downloadAvailable", false);
			this.downloadPackages = new List<HubResourcePackage>();
		}

		// Token: 0x06006237 RID: 25143 RVA: 0x0025B656 File Offset: 0x00259A56
		public void CloseDetail()
		{
			this.browser.CloseDetail(this.resource_id);
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x0025B66C File Offset: 0x00259A6C
		public override void Refresh()
		{
			base.Refresh();
			if (this.downloadPackages != null)
			{
				foreach (HubResourcePackage hubResourcePackage in this.downloadPackages)
				{
					hubResourcePackage.Refresh();
				}
			}
			this.SyncDownloadAvailable();
		}

		// Token: 0x06006239 RID: 25145 RVA: 0x0025B6E0 File Offset: 0x00259AE0
		public void NavigateToOverview()
		{
			if (this.resourceOverviewUrl != null)
			{
				this.browser.NavigateWebPanel(this.resourceOverviewUrl);
			}
		}

		// Token: 0x0600623A RID: 25146 RVA: 0x0025B6FE File Offset: 0x00259AFE
		public void NavigateToUpdates()
		{
			if (this.resourceUpdatesUrl != null)
			{
				this.browser.NavigateWebPanel(this.resourceUpdatesUrl);
			}
		}

		// Token: 0x0600623B RID: 25147 RVA: 0x0025B71C File Offset: 0x00259B1C
		public void NavigateToReviews()
		{
			if (this.resourceReviewsUrl != null)
			{
				this.browser.NavigateWebPanel(this.resourceReviewsUrl);
			}
		}

		// Token: 0x0600623C RID: 25148 RVA: 0x0025B73A File Offset: 0x00259B3A
		public void NavigateToHistory()
		{
			if (this.resourceHistoryUrl != null)
			{
				this.browser.NavigateWebPanel(this.resourceHistoryUrl);
			}
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x0025B758 File Offset: 0x00259B58
		public void NavigateToDiscussion()
		{
			if (this.resourceDiscussionUrl != null)
			{
				this.browser.NavigateWebPanel(this.resourceDiscussionUrl);
			}
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x0025B776 File Offset: 0x00259B76
		public void NavigateToPromotionalLink()
		{
			if (this.promotionalUrl != null)
			{
				this.browser.NavigateWebPanel(this.promotionalUrl);
			}
		}

		// Token: 0x0600623F RID: 25151 RVA: 0x0025B794 File Offset: 0x00259B94
		protected void GoToExternalDownload()
		{
			if (this.externalDownloadUrl != null && this.externalDownloadUrl.val != null)
			{
				this.browser.NavigateWebPanel(this.externalDownloadUrl.val);
			}
		}

		// Token: 0x06006240 RID: 25152 RVA: 0x0025B7C8 File Offset: 0x00259BC8
		public void DownloadAll()
		{
			foreach (HubResourcePackage hubResourcePackage in this.downloadPackages)
			{
				hubResourcePackage.Download();
			}
		}

		// Token: 0x06006241 RID: 25153 RVA: 0x0025B824 File Offset: 0x00259C24
		protected void SyncDownloadAvailable()
		{
			bool val = false;
			if (this.downloadPackages != null)
			{
				foreach (HubResourcePackage hubResourcePackage in this.downloadPackages)
				{
					if (hubResourcePackage.NeedsDownload)
					{
						val = true;
					}
				}
			}
			this.downloadAvailableJSON.val = val;
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x06006242 RID: 25154 RVA: 0x0025B8A0 File Offset: 0x00259CA0
		public bool IsDownloading
		{
			get
			{
				if (this.downloadPackages != null)
				{
					foreach (HubResourcePackage hubResourcePackage in this.downloadPackages)
					{
						if (hubResourcePackage.IsDownloading || hubResourcePackage.IsDownloadQueued)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x06006243 RID: 25155 RVA: 0x0025B920 File Offset: 0x00259D20
		public void RegisterUI(HubResourceItemDetailUI ui)
		{
			base.RegisterUI(ui);
			if (ui != null)
			{
				ui.connectedItem = this;
				this.hadErrorJSON.indicator = ui.hadErrorIndicator;
				this.errorJSON.text = ui.errorText;
				this.closeDetailAction.button = ui.closeDetailButton;
				this.closeDetailAction.buttonAlt = ui.closeDetailButtonAlt;
				this.navigateToOverviewAction.button = ui.navigateToOverviewButton;
				this.navigateToUpdatesAction.button = ui.navigateToUpdatesButton;
				this.hasUpdatesJSON.indicator = ui.hasUpdatesIndicator;
				this.updatesTextJSON.text = ui.updatesText;
				this.navigateToReviewsAction.button = ui.navigateToReviewsButton;
				this.hasReviewsJSON.indicator = ui.hasReviewsIndicator;
				this.reviewsTextJSON.text = ui.reviewsText;
				this.navigateToHistoryAction.button = ui.navigateToHistoryButton;
				this.navigateToDiscussionAction.button = ui.navigateToDiscussionButton;
				if (!SuperController.singleton.promotionalDisabled)
				{
					this.hasPromotionalLinkJSON.indicator = ui.hasPromotionalLinkIndicator;
					this.navigateToPromotionalLinkAction.button = ui.navigateToPromotionalLinkButton;
					this.promotionalLinkTextJSON.text = ui.promotionalLinkText;
				}
				this.hubDownloadableJSON.indicatorAlt = ui.hubDownloadableIndicatorAlt;
				this.hubDownloadableJSON.negativeIndicatorAlt = ui.hubDownloadableNegativeIndicatorAlt;
				this.externalDownloadUrl.text = ui.externalDownloadUrl;
				this.goToExternalDownloadAction.button = ui.goToExternalDownloadUrlButton;
				this.downloadAllAction.button = ui.downloadAllButton;
				this.downloadAvailableJSON.indicator = ui.downloadAvailableIndicator;
				this.hasOtherCreatorsJSON.indicator = ui.hasOtherCreatorsIndicator;
				if (this.hasPromotionalLinkJSON.val && ui.promtionalLinkButtonEnterExitAction != null)
				{
					ui.promtionalLinkButtonEnterExitAction.onEnterActions = new PointerEnterExitAction.OnEnterAction(this.<RegisterUI>m__0);
					ui.promtionalLinkButtonEnterExitAction.onExitActions = new PointerEnterExitAction.OnExitAction(this.<RegisterUI>m__1);
				}
				this.packageContent = ui.packageContent;
				this.creatorSupportContent = ui.creatorSupportContent;
				if (this.packageContent != null)
				{
					IEnumerator enumerator = this.varFilesJSONArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							JSONNode jsonnode = (JSONNode)obj;
							JSONClass asObject = jsonnode.AsObject;
							if (asObject != null)
							{
								HubResourcePackage hubResourcePackage = new HubResourcePackage(asObject, this.browser, false);
								HubResourcePackage hubResourcePackage2 = hubResourcePackage;
								hubResourcePackage2.downloadCompleteCallback = (HubResourcePackage.DownloadCompleteCallback)Delegate.Combine(hubResourcePackage2.downloadCompleteCallback, new HubResourcePackage.DownloadCompleteCallback(this.browser.ResourceDownloadComplete));
								hubResourcePackage.promotionalUrl = this.promotionalUrl;
								this.downloadPackages.Add(hubResourcePackage);
								RectTransform rectTransform = this.browser.CreateDownloadPrefabInstance();
								rectTransform.SetParent(this.packageContent, false);
								HubResourcePackageUI component = rectTransform.GetComponent<HubResourcePackageUI>();
								if (component != null)
								{
									hubResourcePackage.RegisterUI(component);
								}
								if (this.dependencies != null)
								{
									HashSet<string> hashSet = new HashSet<string>();
									hashSet.Add(hubResourcePackage.Creator);
									JSONArray asArray = this.dependencies[hubResourcePackage.GroupName].AsArray;
									if (asArray != null)
									{
										IEnumerator enumerator2 = asArray.GetEnumerator();
										try
										{
											while (enumerator2.MoveNext())
											{
												object obj2 = enumerator2.Current;
												JSONNode jsonnode2 = (JSONNode)obj2;
												JSONClass asObject2 = jsonnode2.AsObject;
												if (asObject2 != null)
												{
													HubResourceItemDetail.<RegisterUI>c__AnonStorey0 <RegisterUI>c__AnonStorey = new HubResourceItemDetail.<RegisterUI>c__AnonStorey0();
													<RegisterUI>c__AnonStorey.$this = this;
													<RegisterUI>c__AnonStorey.dhrp = new HubResourcePackage(asObject2, this.browser, true);
													HubResourcePackage dhrp = <RegisterUI>c__AnonStorey.dhrp;
													dhrp.downloadCompleteCallback = (HubResourcePackage.DownloadCompleteCallback)Delegate.Combine(dhrp.downloadCompleteCallback, new HubResourcePackage.DownloadCompleteCallback(this.browser.ResourceDownloadComplete));
													this.downloadPackages.Add(<RegisterUI>c__AnonStorey.dhrp);
													RectTransform rectTransform2 = this.browser.CreateDownloadPrefabInstance();
													if (rectTransform2 != null)
													{
														rectTransform2.SetParent(this.packageContent, false);
														HubResourcePackageUI component2 = rectTransform2.GetComponent<HubResourcePackageUI>();
														if (component2 != null)
														{
															<RegisterUI>c__AnonStorey.dhrp.RegisterUI(component2);
														}
													}
													if (!SuperController.singleton.promotionalDisabled && this.creatorSupportContent != null && <RegisterUI>c__AnonStorey.dhrp.promotionalUrl != null && <RegisterUI>c__AnonStorey.dhrp.promotionalUrl != string.Empty && <RegisterUI>c__AnonStorey.dhrp.promotionalUrl != "null" && !hashSet.Contains(<RegisterUI>c__AnonStorey.dhrp.Creator))
													{
														this.hasOtherCreatorsJSON.val = true;
														hashSet.Add(<RegisterUI>c__AnonStorey.dhrp.Creator);
														RectTransform rectTransform3 = this.browser.CreateCreatorSupportButtonPrefabInstance();
														if (rectTransform3 != null)
														{
															rectTransform3.SetParent(this.creatorSupportContent, false);
															HubResourceCreatorSupportUI component3 = rectTransform3.GetComponent<HubResourceCreatorSupportUI>();
															if (component3 != null)
															{
																if (component3.linkButton != null)
																{
																	component3.linkButton.onClick.AddListener(new UnityAction(<RegisterUI>c__AnonStorey.<>m__0));
																}
																if (component3.creatorNameText != null)
																{
																	component3.creatorNameText.text = <RegisterUI>c__AnonStorey.dhrp.Creator;
																}
																if (component3.pointerEnterExitAction != null)
																{
																	component3.pointerEnterExitAction.onEnterActions = new PointerEnterExitAction.OnEnterAction(<RegisterUI>c__AnonStorey.<>m__1);
																	component3.pointerEnterExitAction.onExitActions = new PointerEnterExitAction.OnExitAction(<RegisterUI>c__AnonStorey.<>m__2);
																}
															}
														}
													}
												}
											}
										}
										finally
										{
											IDisposable disposable;
											if ((disposable = (enumerator2 as IDisposable)) != null)
											{
												disposable.Dispose();
											}
										}
									}
								}
							}
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
					this.SyncDownloadAvailable();
				}
			}
		}

		// Token: 0x06006244 RID: 25156 RVA: 0x0025BF3C File Offset: 0x0025A33C
		[CompilerGenerated]
		private void <RegisterUI>m__0()
		{
			this.browser.ShowHoverUrl(this.promotionalUrl);
		}

		// Token: 0x06006245 RID: 25157 RVA: 0x0025BF4F File Offset: 0x0025A34F
		[CompilerGenerated]
		private void <RegisterUI>m__1()
		{
			this.browser.ShowHoverUrl(string.Empty);
		}

		// Token: 0x040052D7 RID: 21207
		protected JSONStorableBool hadErrorJSON;

		// Token: 0x040052D8 RID: 21208
		protected JSONStorableString errorJSON;

		// Token: 0x040052D9 RID: 21209
		protected JSONStorableAction closeDetailAction;

		// Token: 0x040052DA RID: 21210
		protected List<HubResourcePackage> downloadPackages;

		// Token: 0x040052DB RID: 21211
		protected string resourceOverviewUrl;

		// Token: 0x040052DC RID: 21212
		protected JSONStorableAction navigateToOverviewAction;

		// Token: 0x040052DD RID: 21213
		protected string resourceUpdatesUrl;

		// Token: 0x040052DE RID: 21214
		protected JSONStorableAction navigateToUpdatesAction;

		// Token: 0x040052DF RID: 21215
		protected JSONStorableBool hasUpdatesJSON;

		// Token: 0x040052E0 RID: 21216
		protected JSONStorableString updatesTextJSON;

		// Token: 0x040052E1 RID: 21217
		protected string resourceReviewsUrl;

		// Token: 0x040052E2 RID: 21218
		protected JSONStorableAction navigateToReviewsAction;

		// Token: 0x040052E3 RID: 21219
		protected JSONStorableBool hasReviewsJSON;

		// Token: 0x040052E4 RID: 21220
		protected JSONStorableString reviewsTextJSON;

		// Token: 0x040052E5 RID: 21221
		protected string resourceHistoryUrl;

		// Token: 0x040052E6 RID: 21222
		protected JSONStorableAction navigateToHistoryAction;

		// Token: 0x040052E7 RID: 21223
		protected string resourceDiscussionUrl;

		// Token: 0x040052E8 RID: 21224
		protected JSONStorableAction navigateToDiscussionAction;

		// Token: 0x040052E9 RID: 21225
		protected JSONStorableBool hasPromotionalLinkJSON;

		// Token: 0x040052EA RID: 21226
		protected string promotionalUrl;

		// Token: 0x040052EB RID: 21227
		protected JSONStorableAction navigateToPromotionalLinkAction;

		// Token: 0x040052EC RID: 21228
		protected JSONStorableString promotionalLinkTextJSON;

		// Token: 0x040052ED RID: 21229
		protected JSONStorableString externalDownloadUrl;

		// Token: 0x040052EE RID: 21230
		protected JSONStorableAction goToExternalDownloadAction;

		// Token: 0x040052EF RID: 21231
		protected RectTransform packagePrefab;

		// Token: 0x040052F0 RID: 21232
		protected RectTransform packageContent;

		// Token: 0x040052F1 RID: 21233
		protected RectTransform creatorSupportContent;

		// Token: 0x040052F2 RID: 21234
		protected JSONStorableBool hasOtherCreatorsJSON;

		// Token: 0x040052F3 RID: 21235
		protected JSONClass dependencies;

		// Token: 0x040052F4 RID: 21236
		protected JSONStorableAction downloadAllAction;

		// Token: 0x040052F5 RID: 21237
		protected JSONStorableBool downloadAvailableJSON;

		// Token: 0x02001029 RID: 4137
		[CompilerGenerated]
		private sealed class <RegisterUI>c__AnonStorey0
		{
			// Token: 0x06007723 RID: 30499 RVA: 0x0025BF61 File Offset: 0x0025A361
			public <RegisterUI>c__AnonStorey0()
			{
			}

			// Token: 0x06007724 RID: 30500 RVA: 0x0025BF69 File Offset: 0x0025A369
			internal void <>m__0()
			{
				this.$this.browser.NavigateWebPanel(this.dhrp.promotionalUrl);
			}

			// Token: 0x06007725 RID: 30501 RVA: 0x0025BF86 File Offset: 0x0025A386
			internal void <>m__1()
			{
				this.$this.browser.ShowHoverUrl(this.dhrp.promotionalUrl);
			}

			// Token: 0x06007726 RID: 30502 RVA: 0x0025BFA3 File Offset: 0x0025A3A3
			internal void <>m__2()
			{
				this.$this.browser.ShowHoverUrl(string.Empty);
			}

			// Token: 0x04006B37 RID: 27447
			internal HubResourcePackage dhrp;

			// Token: 0x04006B38 RID: 27448
			internal HubResourceItemDetail $this;
		}
	}
}
