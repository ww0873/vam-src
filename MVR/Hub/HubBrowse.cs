using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

namespace MVR.Hub
{
	// Token: 0x02000CAE RID: 3246
	public class HubBrowse : JSONStorable
	{
		// Token: 0x06006169 RID: 24937 RVA: 0x00254B60 File Offset: 0x00252F60
		public HubBrowse()
		{
		}

		// Token: 0x0600616A RID: 24938 RVA: 0x00254BF0 File Offset: 0x00252FF0
		private IEnumerator GetRequest(string uri, HubBrowse.RequestSuccessCallback callback, HubBrowse.RequestErrorCallback errorCallback)
		{
			using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
			{
				webRequest.SendWebRequest();
				while (!webRequest.isDone)
				{
					yield return null;
				}
				if (webRequest.isNetworkError)
				{
					UnityEngine.Debug.LogError(uri + ": Error: " + webRequest.error);
					if (errorCallback != null)
					{
						errorCallback(webRequest.error);
					}
				}
				else
				{
					SimpleJSON.JSONNode jsonNode = JSON.Parse(webRequest.downloadHandler.text);
					if (callback != null)
					{
						callback(jsonNode);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600616B RID: 24939 RVA: 0x00254C1C File Offset: 0x0025301C
		private IEnumerator BinaryGetRequest(string uri, HubBrowse.BinaryRequestStartedCallback startedCallback, HubBrowse.BinaryRequestSuccessCallback successCallback, HubBrowse.RequestErrorCallback errorCallback, HubBrowse.RequestProgressCallback progressCallback, List<string> cookies = null)
		{
			using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
			{
				string cookieHeader = "vamhubconsent=1";
				if (cookies != null)
				{
					foreach (string str in cookies)
					{
						cookieHeader = cookieHeader + ";" + str;
					}
				}
				try
				{
					webRequest.SetRequestHeader("Cookie", cookieHeader);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Concat(new string[]
					{
						"Bad request header ",
						cookieHeader,
						" for Hub download of ",
						uri,
						": ",
						ex.ToString()
					}));
					webRequest.SetRequestHeader("Cookie", "vamhubconsent=1");
				}
				webRequest.SendWebRequest();
				if (startedCallback != null)
				{
					startedCallback();
				}
				while (!webRequest.isDone)
				{
					if (progressCallback != null)
					{
						progressCallback(webRequest.downloadProgress);
					}
					yield return null;
				}
				if (webRequest.isNetworkError)
				{
					UnityEngine.Debug.LogError(uri + ": Error: " + webRequest.error);
					if (errorCallback != null)
					{
						errorCallback(webRequest.error);
					}
				}
				else
				{
					Dictionary<string, string> responseHeaders = webRequest.GetResponseHeaders();
					if (successCallback != null)
					{
						successCallback(webRequest.downloadHandler.data, responseHeaders);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600616C RID: 24940 RVA: 0x00254C60 File Offset: 0x00253060
		private IEnumerator PostRequest(string uri, string postData, HubBrowse.RequestSuccessCallback callback, HubBrowse.RequestErrorCallback errorCallback)
		{
			using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, postData))
			{
				webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
				webRequest.SetRequestHeader("Content-Type", "application/json");
				webRequest.SetRequestHeader("Accept", "application/json");
				yield return webRequest.SendWebRequest();
				string[] pages = uri.Split(new char[]
				{
					'/'
				});
				int page = pages.Length - 1;
				if (webRequest.isNetworkError)
				{
					UnityEngine.Debug.LogError(pages[page] + ": Error: " + webRequest.error);
					if (errorCallback != null)
					{
						errorCallback(webRequest.error);
					}
				}
				else
				{
					SimpleJSON.JSONNode jsonnode = JSON.Parse(webRequest.downloadHandler.text);
					if (jsonnode == null)
					{
						string text = "Error - Invalid JSON response: " + webRequest.downloadHandler.text;
						UnityEngine.Debug.LogError(pages[page] + ": " + text);
						if (errorCallback != null)
						{
							errorCallback(text);
						}
					}
					else if (callback != null)
					{
						callback(jsonnode);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x00254C91 File Offset: 0x00253091
		protected void SyncHubEnabled(bool b)
		{
			this._hubEnabled = b;
			if (this._hubEnabled)
			{
				this.GetHubInfo();
				if (this._isShowing)
				{
					this.RefreshResources();
				}
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x0600616E RID: 24942 RVA: 0x00254CBC File Offset: 0x002530BC
		// (set) Token: 0x0600616F RID: 24943 RVA: 0x00254CC4 File Offset: 0x002530C4
		public bool HubEnabled
		{
			get
			{
				return this._hubEnabled;
			}
			set
			{
				if (this.hubEnabledJSON != null)
				{
					this.hubEnabledJSON.val = value;
				}
				else
				{
					this._hubEnabled = value;
				}
			}
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x00254CE9 File Offset: 0x002530E9
		protected void EnableHub()
		{
			if (this.enableHubCallbacks != null)
			{
				this.enableHubCallbacks();
			}
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x00254D04 File Offset: 0x00253104
		protected void SyncWebBrowserEnabled(bool b)
		{
			this._webBrowserEnabled = b;
			if (this._webBrowserEnabled && this.resourceDetailStack != null && this.resourceDetailStack.Count > 0)
			{
				HubResourceItemDetailUI hubResourceItemDetailUI = this.resourceDetailStack.Peek();
				if (hubResourceItemDetailUI.connectedItem != null)
				{
					hubResourceItemDetailUI.connectedItem.NavigateToOverview();
				}
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06006172 RID: 24946 RVA: 0x00254D61 File Offset: 0x00253161
		// (set) Token: 0x06006173 RID: 24947 RVA: 0x00254D69 File Offset: 0x00253169
		public bool WebBrowserEnabled
		{
			get
			{
				return this._webBrowserEnabled;
			}
			set
			{
				if (this.webBrowserEnabledJSON != null)
				{
					this.webBrowserEnabledJSON.val = value;
				}
				else
				{
					this._webBrowserEnabled = value;
				}
			}
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x00254D8E File Offset: 0x0025318E
		protected void EnableWebBrowser()
		{
			if (this.enableWebBrowserCallbacks != null)
			{
				this.enableWebBrowserCallbacks();
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06006175 RID: 24949 RVA: 0x00254DA6 File Offset: 0x002531A6
		public bool IsShowing
		{
			get
			{
				return this._isShowing;
			}
		}

		// Token: 0x06006176 RID: 24950 RVA: 0x00254DB0 File Offset: 0x002531B0
		public void Show()
		{
			if (this.preShowCallbacks != null)
			{
				this.preShowCallbacks();
			}
			this._isShowing = true;
			if (this.hubBrowseUI != null)
			{
				this.hubBrowseUI.gameObject.SetActive(true);
			}
			else if (this.UITransform != null)
			{
				this.UITransform.gameObject.SetActive(true);
			}
			if (this._hubEnabled)
			{
				if (this._hasBeenRefreshed)
				{
					if (this.items != null)
					{
						foreach (HubResourceItemUI hubResourceItemUI in this.items)
						{
							if (hubResourceItemUI.connectedItem != null)
							{
								hubResourceItemUI.connectedItem.Show();
							}
						}
					}
				}
				else
				{
					this.RefreshResources();
				}
			}
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x00254EB0 File Offset: 0x002532B0
		public void Hide()
		{
			this._isShowing = false;
			if (this.hubBrowseUI != null)
			{
				this.hubBrowseUI.gameObject.SetActive(false);
			}
			if (this.items != null)
			{
				foreach (HubResourceItemUI hubResourceItemUI in this.items)
				{
					if (hubResourceItemUI.connectedItem != null)
					{
						hubResourceItemUI.connectedItem.Hide();
					}
				}
			}
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x00254F50 File Offset: 0x00253350
		protected void RefreshErrorCallback(string err)
		{
			if (this.refreshIndicator != null)
			{
				this.refreshIndicator.SetActive(false);
			}
			SuperController.LogError("Error during hub request " + err);
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x00254F80 File Offset: 0x00253380
		protected void RefreshCallback(SimpleJSON.JSONNode jsonNode)
		{
			if (this.refreshIndicator != null)
			{
				this.refreshIndicator.SetActive(false);
			}
			if (jsonNode != null)
			{
				JSONClass asObject = jsonNode.AsObject;
				if (asObject != null)
				{
					string a = asObject["status"];
					if (a == "success")
					{
						JSONClass asObject2 = asObject["pagination"].AsObject;
						if (asObject2 != null)
						{
							this.numResourcesJSON.val = "Total: " + asObject2["total_found"];
							this.numPagesJSON.val = asObject2["total_pages"];
							if (this.items != null)
							{
								foreach (HubResourceItemUI hubResourceItemUI in this.items)
								{
									if (hubResourceItemUI.connectedItem != null)
									{
										hubResourceItemUI.connectedItem.Destroy();
									}
									UnityEngine.Object.Destroy(hubResourceItemUI.gameObject);
								}
								this.items.Clear();
							}
							else
							{
								this.items = new List<HubResourceItemUI>();
							}
							if (this.itemScrollRect != null)
							{
								this.itemScrollRect.verticalNormalizedPosition = 1f;
							}
							JSONArray asArray = asObject["resources"].AsArray;
							if (this.itemContainer != null && this.itemPrefab != null && asArray != null)
							{
								IEnumerator enumerator2 = asArray.GetEnumerator();
								try
								{
									while (enumerator2.MoveNext())
									{
										object obj = enumerator2.Current;
										JSONClass resource = (JSONClass)obj;
										HubResourceItem hubResourceItem = new HubResourceItem(resource, this, false);
										hubResourceItem.Refresh();
										RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.itemPrefab);
										rectTransform.SetParent(this.itemContainer, false);
										HubResourceItemUI component = rectTransform.GetComponent<HubResourceItemUI>();
										if (component != null)
										{
											hubResourceItem.RegisterUI(component);
											this.items.Add(component);
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
					else
					{
						string str = jsonNode["error"];
						SuperController.LogError("Refresh returned error " + str);
					}
				}
			}
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x0025520C File Offset: 0x0025360C
		public void RefreshResources()
		{
			this._hasBeenRefreshed = true;
			if (this._hubEnabled)
			{
				if (this.refreshResourcesRoutine != null)
				{
					base.StopCoroutine(this.refreshResourcesRoutine);
				}
				JSONClass jsonclass = new JSONClass();
				jsonclass["source"] = "VaM";
				jsonclass["action"] = "getResources";
				jsonclass["latest_image"] = "Y";
				jsonclass["perpage"] = this._numPerPageInt.ToString();
				jsonclass["page"] = this._currentPageString;
				if (this._hostedOption != "All")
				{
					jsonclass["location"] = this._hostedOption;
				}
				if (this._searchFilter != string.Empty)
				{
					jsonclass["search"] = this._searchFilter;
					jsonclass["searchall"] = "true";
				}
				if (this._payTypeFilter != "All")
				{
					jsonclass["category"] = this._payTypeFilter;
				}
				if (this._categoryFilter != "All")
				{
					jsonclass["type"] = this._categoryFilter;
				}
				if (this._creatorFilter != "All")
				{
					jsonclass["username"] = this._creatorFilter;
				}
				if (this._tagsFilter != "All")
				{
					jsonclass["tags"] = this._tagsFilter;
				}
				string text = this._sortPrimary;
				if (this._sortSecondary != null && this._sortSecondary != string.Empty && this._sortSecondary != "None")
				{
					text = text + "," + this._sortSecondary;
				}
				jsonclass["sort"] = text;
				string postData = jsonclass.ToString();
				this.refreshResourcesRoutine = base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubBrowse.RequestSuccessCallback(this.RefreshCallback), new HubBrowse.RequestErrorCallback(this.RefreshErrorCallback)));
				if (this.refreshIndicator != null)
				{
					this.refreshIndicator.SetActive(true);
				}
			}
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x00255489 File Offset: 0x00253889
		protected void SyncNumResources(string s)
		{
		}

		// Token: 0x0600617C RID: 24956 RVA: 0x0025548B File Offset: 0x0025388B
		protected void SetPageInfo()
		{
			this.pageInfoJSON.val = "Page " + this.currentPageJSON.val + " of " + this.numPagesJSON.val;
		}

		// Token: 0x0600617D RID: 24957 RVA: 0x002554C0 File Offset: 0x002538C0
		protected void SyncNumPages(string s)
		{
			int numPagesInt;
			if (int.TryParse(s, out numPagesInt))
			{
				this._numPagesInt = numPagesInt;
			}
			this.SetPageInfo();
		}

		// Token: 0x0600617E RID: 24958 RVA: 0x002554E7 File Offset: 0x002538E7
		protected void SyncNumPerPage(float f)
		{
			this._numPerPageInt = (int)f;
			this.ResetRefresh();
		}

		// Token: 0x0600617F RID: 24959 RVA: 0x002554F7 File Offset: 0x002538F7
		protected void ResetRefresh()
		{
			this._currentPageString = "1";
			this._currentPageInt = 1;
			this.currentPageJSON.valNoCallback = this._currentPageString;
			this.SetPageInfo();
			this.RefreshResources();
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x00255528 File Offset: 0x00253928
		protected void SyncCurrentPage(string s)
		{
			this._currentPageString = s;
			int currentPageInt;
			if (int.TryParse(s, out currentPageInt))
			{
				this._currentPageInt = currentPageInt;
			}
			this.SetPageInfo();
			this.RefreshResources();
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x0025555C File Offset: 0x0025395C
		protected void FirstPage()
		{
			this.currentPageJSON.val = "1";
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x00255570 File Offset: 0x00253970
		protected void PreviousPage()
		{
			if (this._currentPageInt > 1)
			{
				this.currentPageJSON.val = (this._currentPageInt - 1).ToString();
			}
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x002555AC File Offset: 0x002539AC
		protected void NextPage()
		{
			if (this._currentPageInt < this._numPagesInt)
			{
				this.currentPageJSON.val = (this._currentPageInt + 1).ToString();
			}
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x002555EC File Offset: 0x002539EC
		protected void ResetFilters()
		{
			this._hostedOption = "Hub And Dependencies";
			this.hostedOptionChooser.valNoCallback = "Hub And Dependencies";
			this._payTypeFilter = "Free";
			this.payTypeFilterChooser.valNoCallback = "Free";
			this._searchFilter = string.Empty;
			this.searchFilterJSON.valNoCallback = string.Empty;
			this._categoryFilter = "All";
			this.categoryFilterChooser.valNoCallback = "All";
			this._creatorFilter = "All";
			this.creatorFilterChooser.valNoCallback = "All";
			this._tagsFilter = "All";
			this.tagsFilterChooser.valNoCallback = "All";
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x0025569B File Offset: 0x00253A9B
		protected void ResetFiltersAndRefresh()
		{
			this.ResetFilters();
			this.ResetRefresh();
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x002556A9 File Offset: 0x00253AA9
		protected void SyncHostedOption(string s)
		{
			this._hostedOption = s;
			this.ResetRefresh();
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06006187 RID: 24967 RVA: 0x002556B8 File Offset: 0x00253AB8
		// (set) Token: 0x06006188 RID: 24968 RVA: 0x002556C0 File Offset: 0x00253AC0
		public string HostedOption
		{
			get
			{
				return this._hostedOption;
			}
			set
			{
				this.hostedOptionChooser.val = value;
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x002556D0 File Offset: 0x00253AD0
		protected void SyncPayTypeFilter(string s)
		{
			this._payTypeFilter = s;
			if (this._payTypeFilter != "Free" && this._hostedOption != "All")
			{
				this.hostedOptionChooser.val = "All";
			}
			else
			{
				this.ResetRefresh();
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x0600618A RID: 24970 RVA: 0x00255729 File Offset: 0x00253B29
		// (set) Token: 0x0600618B RID: 24971 RVA: 0x00255731 File Offset: 0x00253B31
		public string PayTypeFilter
		{
			get
			{
				return this._payTypeFilter;
			}
			set
			{
				this.payTypeFilterChooser.val = value;
			}
		}

		// Token: 0x0600618C RID: 24972 RVA: 0x00255740 File Offset: 0x00253B40
		protected IEnumerator TriggerResetRefesh()
		{
			while (this.triggerCountdown > 0f)
			{
				this.triggerCountdown -= Time.unscaledDeltaTime;
				yield return null;
			}
			this.triggerResetRefreshRoutine = null;
			this.ResetRefresh();
			yield break;
		}

		// Token: 0x0600618D RID: 24973 RVA: 0x0025575C File Offset: 0x00253B5C
		protected void SyncSearchFilter(string s)
		{
			this._searchFilter = s;
			bool flag = false;
			if (this._searchFilter.Length > 2)
			{
				if (this._minLengthSearchFilter != this._searchFilter)
				{
					this._minLengthSearchFilter = this._searchFilter;
					flag = true;
				}
			}
			else if (this._minLengthSearchFilter != string.Empty)
			{
				this._minLengthSearchFilter = string.Empty;
				flag = true;
			}
			if (flag)
			{
				this.triggerCountdown = 0.5f;
				if (this.triggerResetRefreshRoutine == null)
				{
					this.triggerResetRefreshRoutine = base.StartCoroutine(this.TriggerResetRefesh());
				}
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x0600618E RID: 24974 RVA: 0x002557FC File Offset: 0x00253BFC
		// (set) Token: 0x0600618F RID: 24975 RVA: 0x00255804 File Offset: 0x00253C04
		public string SearchFilter
		{
			get
			{
				return this._searchFilter;
			}
			set
			{
				this.searchFilterJSON.val = value;
			}
		}

		// Token: 0x06006190 RID: 24976 RVA: 0x00255812 File Offset: 0x00253C12
		protected void SyncCategoryFilter(string s)
		{
			this._categoryFilter = s;
			this.ResetRefresh();
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06006191 RID: 24977 RVA: 0x00255821 File Offset: 0x00253C21
		// (set) Token: 0x06006192 RID: 24978 RVA: 0x00255829 File Offset: 0x00253C29
		public string CategoryFilter
		{
			get
			{
				return this._categoryFilter;
			}
			set
			{
				this.categoryFilterChooser.val = value;
			}
		}

		// Token: 0x06006193 RID: 24979 RVA: 0x00255838 File Offset: 0x00253C38
		public void SetPayTypeAndCategoryFilter(string payType, string category, bool onlyTheseFilters = true)
		{
			if (onlyTheseFilters)
			{
				this.CloseAllDetails();
				this.ResetFilters();
			}
			if (!SuperController.singleton.promotionalDisabled)
			{
				this._payTypeFilter = payType;
				this.payTypeFilterChooser.valNoCallback = payType;
			}
			this._categoryFilter = category;
			this.categoryFilterChooser.valNoCallback = category;
			this.ResetRefresh();
		}

		// Token: 0x06006194 RID: 24980 RVA: 0x00255892 File Offset: 0x00253C92
		protected void SyncCreatorFilter(string s)
		{
			this._creatorFilter = s;
			this.ResetRefresh();
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06006195 RID: 24981 RVA: 0x002558A1 File Offset: 0x00253CA1
		// (set) Token: 0x06006196 RID: 24982 RVA: 0x002558A9 File Offset: 0x00253CA9
		public string CreatorFilter
		{
			get
			{
				return this._creatorFilter;
			}
			set
			{
				if (!SuperController.singleton.promotionalDisabled)
				{
					this._hostedOption = "All";
					this.hostedOptionChooser.valNoCallback = "All";
				}
				this.creatorFilterChooser.val = value;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06006197 RID: 24983 RVA: 0x002558E1 File Offset: 0x00253CE1
		// (set) Token: 0x06006198 RID: 24984 RVA: 0x002558EC File Offset: 0x00253CEC
		public string CreatorFilterOnly
		{
			get
			{
				return this._creatorFilter;
			}
			set
			{
				this.CloseAllDetails();
				this.ResetFilters();
				if (!SuperController.singleton.promotionalDisabled)
				{
					this._payTypeFilter = "All";
					this.payTypeFilterChooser.valNoCallback = "All";
					this._hostedOption = "All";
					this.hostedOptionChooser.valNoCallback = "All";
				}
				this.creatorFilterChooser.val = value;
			}
		}

		// Token: 0x06006199 RID: 24985 RVA: 0x00255956 File Offset: 0x00253D56
		protected void SyncTagsFilter(string s)
		{
			this._tagsFilter = s;
			this.ResetRefresh();
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x0600619A RID: 24986 RVA: 0x00255965 File Offset: 0x00253D65
		// (set) Token: 0x0600619B RID: 24987 RVA: 0x0025596D File Offset: 0x00253D6D
		public string TagsFilter
		{
			get
			{
				return this._tagsFilter;
			}
			set
			{
				this.tagsFilterChooser.val = value;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x0600619C RID: 24988 RVA: 0x0025597B File Offset: 0x00253D7B
		// (set) Token: 0x0600619D RID: 24989 RVA: 0x00255983 File Offset: 0x00253D83
		public string TagsFilterOnly
		{
			get
			{
				return this._tagsFilter;
			}
			set
			{
				this.ResetFilters();
				this.tagsFilterChooser.val = value;
			}
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x00255997 File Offset: 0x00253D97
		protected void SyncSortPrimary(string s)
		{
			this._sortPrimary = s;
			this.ResetRefresh();
		}

		// Token: 0x0600619F RID: 24991 RVA: 0x002559A6 File Offset: 0x00253DA6
		protected void SyncSortSecondary(string s)
		{
			this._sortSecondary = s;
			this.ResetRefresh();
		}

		// Token: 0x060061A0 RID: 24992 RVA: 0x002559B8 File Offset: 0x00253DB8
		public void NavigateWebPanel(string url)
		{
			if (this.webBrowser != null && this.webBrowser.url != url && this._webBrowserEnabled)
			{
				if (this.isWebLoadingIndicator != null)
				{
					this.isWebLoadingIndicator.SetActive(true);
				}
				this.webBrowser.url = url;
			}
		}

		// Token: 0x060061A1 RID: 24993 RVA: 0x00255A20 File Offset: 0x00253E20
		public void ShowHoverUrl(string url)
		{
			if (this.webBrowser != null)
			{
				this.webBrowser.HoveredURL = url;
			}
		}

		// Token: 0x060061A2 RID: 24994 RVA: 0x00255A3F File Offset: 0x00253E3F
		protected void GetResourceDetailErrorCallback(string err, HubResourceItemDetailUI hridui)
		{
			SuperController.LogError("Error during fetch of resource detail from Hub");
			this.CloseDetail(null);
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x00255A54 File Offset: 0x00253E54
		protected void GetResourceDetailCallback(SimpleJSON.JSONNode jsonNode, HubResourceItemDetailUI hridui)
		{
			if (jsonNode != null && hridui != null)
			{
				JSONClass asObject = jsonNode.AsObject;
				if (asObject != null)
				{
					HubResourceItemDetail hubResourceItemDetail = new HubResourceItemDetail(asObject, this);
					hubResourceItemDetail.Refresh();
					hubResourceItemDetail.RegisterUI(hridui);
				}
			}
		}

		// Token: 0x060061A4 RID: 24996 RVA: 0x00255AA4 File Offset: 0x00253EA4
		public void OpenDetail(string resource_id, bool isPackageName = false)
		{
			if (this._hubEnabled)
			{
				if (this.resourceDetailPrefab != null && this.resourceDetailContainer != null)
				{
					HubBrowse.<OpenDetail>c__AnonStorey6 <OpenDetail>c__AnonStorey = new HubBrowse.<OpenDetail>c__AnonStorey6();
					<OpenDetail>c__AnonStorey.$this = this;
					this.Show();
					if (this.savedResourceDetailsPanels.TryGetValue(resource_id, out <OpenDetail>c__AnonStorey.hridui))
					{
						this.savedResourceDetailsPanels.Remove(resource_id);
						<OpenDetail>c__AnonStorey.hridui.gameObject.SetActive(true);
						this.resourceDetailStack.Push(<OpenDetail>c__AnonStorey.hridui);
					}
					else
					{
						RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.resourceDetailPrefab);
						rectTransform.SetParent(this.resourceDetailContainer, false);
						<OpenDetail>c__AnonStorey.hridui = rectTransform.GetComponent<HubResourceItemDetailUI>();
						if (SuperController.singleton.promotionalDisabled)
						{
							if (<OpenDetail>c__AnonStorey.hridui.hasPromotionalLinkIndicator != null)
							{
								<OpenDetail>c__AnonStorey.hridui.hasPromotionalLinkIndicator.gameObject.SetActive(false);
							}
							if (<OpenDetail>c__AnonStorey.hridui.navigateToPromotionalLinkButton != null)
							{
								<OpenDetail>c__AnonStorey.hridui.navigateToPromotionalLinkButton.gameObject.SetActive(false);
							}
							if (<OpenDetail>c__AnonStorey.hridui.hasOtherCreatorsIndicator != null)
							{
								<OpenDetail>c__AnonStorey.hridui.hasOtherCreatorsIndicator.gameObject.SetActive(false);
							}
						}
						this.resourceDetailStack.Push(<OpenDetail>c__AnonStorey.hridui);
						JSONClass jsonclass = new JSONClass();
						jsonclass["source"] = "VaM";
						jsonclass["action"] = "getResourceDetail";
						jsonclass["latest_image"] = "Y";
						if (isPackageName)
						{
							jsonclass["package_name"] = resource_id;
						}
						else
						{
							jsonclass["resource_id"] = resource_id;
						}
						string postData = jsonclass.ToString();
						base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubBrowse.RequestSuccessCallback(<OpenDetail>c__AnonStorey.<>m__0), new HubBrowse.RequestErrorCallback(<OpenDetail>c__AnonStorey.<>m__1)));
					}
					if (this.detailPanel != null)
					{
						this.detailPanel.SetActive(true);
					}
				}
			}
			else
			{
				SuperController.LogError("Cannot perform action. Hub is disabled in User Preferences");
			}
		}

		// Token: 0x060061A5 RID: 24997 RVA: 0x00255CD8 File Offset: 0x002540D8
		public void CloseDetail(string resource_id)
		{
			if (this.resourceDetailStack.Count > 0)
			{
				HubResourceItemDetailUI hubResourceItemDetailUI = this.resourceDetailStack.Pop();
				if (hubResourceItemDetailUI.connectedItem != null && hubResourceItemDetailUI.connectedItem.IsDownloading)
				{
					hubResourceItemDetailUI.gameObject.SetActive(false);
					this.savedResourceDetailsPanels.Add(resource_id, hubResourceItemDetailUI);
				}
				else
				{
					if (resource_id != null)
					{
						this.savedResourceDetailsPanels.Remove(resource_id);
					}
					UnityEngine.Object.Destroy(hubResourceItemDetailUI.gameObject);
				}
			}
			if (this.resourceDetailStack.Count == 0)
			{
				if (this.detailPanel != null)
				{
					this.detailPanel.SetActive(false);
				}
			}
			else
			{
				HubResourceItemDetailUI hubResourceItemDetailUI2 = this.resourceDetailStack.Peek();
				if (hubResourceItemDetailUI2.connectedItem != null)
				{
					hubResourceItemDetailUI2.connectedItem.NavigateToOverview();
				}
			}
		}

		// Token: 0x060061A6 RID: 24998 RVA: 0x00255DB0 File Offset: 0x002541B0
		protected void CloseAllDetails()
		{
			while (this.resourceDetailStack.Count > 0)
			{
				HubResourceItemDetailUI hubResourceItemDetailUI = this.resourceDetailStack.Pop();
				if (hubResourceItemDetailUI.connectedItem != null && hubResourceItemDetailUI.connectedItem.IsDownloading)
				{
					hubResourceItemDetailUI.gameObject.SetActive(false);
					this.savedResourceDetailsPanels.Add(hubResourceItemDetailUI.connectedItem.ResourceId, hubResourceItemDetailUI);
				}
				else
				{
					if (hubResourceItemDetailUI.connectedItem != null)
					{
						this.savedResourceDetailsPanels.Remove(hubResourceItemDetailUI.connectedItem.ResourceId);
					}
					UnityEngine.Object.Destroy(hubResourceItemDetailUI.gameObject);
				}
			}
			if (this.detailPanel != null)
			{
				this.detailPanel.SetActive(false);
			}
		}

		// Token: 0x060061A7 RID: 24999 RVA: 0x00255E6C File Offset: 0x0025426C
		public RectTransform CreateDownloadPrefabInstance()
		{
			RectTransform result = null;
			if (this.packageDownloadPrefab != null)
			{
				result = UnityEngine.Object.Instantiate<RectTransform>(this.packageDownloadPrefab);
			}
			return result;
		}

		// Token: 0x060061A8 RID: 25000 RVA: 0x00255E9C File Offset: 0x0025429C
		public RectTransform CreateCreatorSupportButtonPrefabInstance()
		{
			RectTransform result = null;
			if (this.creatorSupportButtonPrefab != null)
			{
				result = UnityEngine.Object.Instantiate<RectTransform>(this.creatorSupportButtonPrefab);
			}
			return result;
		}

		// Token: 0x060061A9 RID: 25001 RVA: 0x00255EC9 File Offset: 0x002542C9
		protected void FindMissingPackagesErrorCallback(string err)
		{
			SuperController.LogError("Error during hub request " + err);
		}

		// Token: 0x060061AA RID: 25002 RVA: 0x00255EDC File Offset: 0x002542DC
		protected void FindMissingPackagesCallback(SimpleJSON.JSONNode jsonNode)
		{
			if (jsonNode != null)
			{
				JSONClass asObject = jsonNode.AsObject;
				if (asObject != null)
				{
					string text = asObject["status"];
					if (text != null && text == "error")
					{
						string str = jsonNode["error"];
						SuperController.LogError("findPackages returned error " + str);
					}
					else
					{
						JSONClass asObject2 = jsonNode["packages"].AsObject;
						if (asObject2 != null)
						{
							if (this.missingPackages != null)
							{
								foreach (HubResourcePackageUI hubResourcePackageUI in this.missingPackages)
								{
									UnityEngine.Object.Destroy(hubResourcePackageUI.gameObject);
								}
								this.missingPackages.Clear();
							}
							else
							{
								this.missingPackages = new List<HubResourcePackageUI>();
							}
							foreach (string text2 in this.checkMissingPackageNames)
							{
								JSONClass jsonclass = asObject2[text2].AsObject;
								if (jsonclass == null)
								{
									jsonclass = new JSONClass();
									jsonclass["filename"] = text2;
									jsonclass["downloadUrl"] = "null";
								}
								else
								{
									if (Regex.IsMatch(text2, "[0-9]+$"))
									{
										string text3 = jsonclass["filename"];
										if (text3 == null || text3 == "null" || text3 != text2 + ".var")
										{
											UnityEngine.Debug.LogError("Missing file name " + text3 + " does not match missing package name " + text2);
											jsonclass["filename"] = text2;
											jsonclass["file_size"] = "null";
											jsonclass["licenseType"] = "null";
											jsonclass["downloadUrl"] = "null";
										}
									}
									else
									{
										string text4 = jsonclass["filename"];
										if (text4 == null || text4 == "null")
										{
											jsonclass["filename"] = text2;
										}
									}
									string text5 = jsonclass["resource_id"];
									if (text5 == null || text5 == "null")
									{
										jsonclass["downloadUrl"] = "null";
									}
								}
								HubResourcePackage hubResourcePackage = new HubResourcePackage(jsonclass, this, true);
								HubResourcePackage hubResourcePackage2 = hubResourcePackage;
								hubResourcePackage2.downloadCompleteCallback = (HubResourcePackage.DownloadCompleteCallback)Delegate.Combine(hubResourcePackage2.downloadCompleteCallback, new HubResourcePackage.DownloadCompleteCallback(this.ResourceDownloadComplete));
								RectTransform rectTransform = this.CreateDownloadPrefabInstance();
								if (rectTransform != null)
								{
									rectTransform.SetParent(this.missingPackagesContainer, false);
									HubResourcePackageUI component = rectTransform.GetComponent<HubResourcePackageUI>();
									if (component != null)
									{
										this.missingPackages.Add(component);
										hubResourcePackage.RegisterUI(component);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060061AB RID: 25003 RVA: 0x00256260 File Offset: 0x00254660
		public void OpenMissingPackagesPanel()
		{
			if (this._hubEnabled)
			{
				if (this.packageManager != null && this.missingPackagesPanel != null && this.missingPackagesContainer != null)
				{
					this.Show();
					if (this.missingPackagesPanel != null)
					{
						this.missingPackagesPanel.SetActive(true);
					}
					if (this.downloadQueue.Count == 0)
					{
						List<string> missingPackageNames = this.packageManager.MissingPackageNames;
						if (missingPackageNames.Count > 0)
						{
							List<string> list = new List<string>();
							foreach (string text in missingPackageNames)
							{
								Match match;
								if ((match = Regex.Match(text, "^([^\\.]+\\.[^\\.]+)\\.min([0-9]+)$")).Success)
								{
									string value = match.Groups[1].Value;
									UnityEngine.Debug.Log("Matched on min version. Fixed missing package is " + value + ".latest");
									list.Add(value + ".latest");
								}
								else
								{
									list.Add(text);
								}
							}
							JSONClass jsonclass = new JSONClass();
							jsonclass["source"] = "VaM";
							jsonclass["action"] = "findPackages";
							this.checkMissingPackageNames = list;
							jsonclass["packages"] = string.Join(",", list.ToArray());
							string postData = jsonclass.ToString();
							base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubBrowse.RequestSuccessCallback(this.FindMissingPackagesCallback), new HubBrowse.RequestErrorCallback(this.FindMissingPackagesErrorCallback)));
						}
						else if (this.missingPackages != null)
						{
							foreach (HubResourcePackageUI hubResourcePackageUI in this.missingPackages)
							{
								UnityEngine.Object.Destroy(hubResourcePackageUI.gameObject);
							}
							this.missingPackages.Clear();
						}
						else
						{
							this.missingPackages = new List<HubResourcePackageUI>();
						}
					}
				}
			}
			else
			{
				this.Show();
			}
		}

		// Token: 0x060061AC RID: 25004 RVA: 0x002564B8 File Offset: 0x002548B8
		public void CloseMissingPackagesPanel()
		{
			if (this.missingPackagesPanel != null)
			{
				this.missingPackagesPanel.SetActive(false);
			}
		}

		// Token: 0x060061AD RID: 25005 RVA: 0x002564D8 File Offset: 0x002548D8
		public void DownloadAllMissingPackages()
		{
			if (this.missingPackages != null)
			{
				foreach (HubResourcePackageUI hubResourcePackageUI in this.missingPackages)
				{
					hubResourcePackageUI.connectedItem.Download();
				}
			}
		}

		// Token: 0x060061AE RID: 25006 RVA: 0x00256544 File Offset: 0x00254944
		public string GetPackageHubResourceId(string packageId)
		{
			string result = null;
			if (this.packageIdToResourceId != null)
			{
				this.packageIdToResourceId.TryGetValue(packageId, out result);
			}
			return result;
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x0025656E File Offset: 0x0025496E
		protected void GetPackagesJSONErrorCallback(string err)
		{
			SuperController.LogError("Error during hub request for packages.json " + err);
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x00256580 File Offset: 0x00254980
		protected void GetPackagesJSONCallback(SimpleJSON.JSONNode jsonNode)
		{
			if (jsonNode != null)
			{
				JSONClass asObject = jsonNode.AsObject;
				if (asObject != null)
				{
					this.packageGroupToLatestVersion = new Dictionary<string, int>();
					this.packageIdToResourceId = new Dictionary<string, string>();
					foreach (string text in asObject.Keys)
					{
						string text2 = Regex.Replace(text, "\\.var$", string.Empty);
						string text3 = FileManager.PackageIDToPackageVersion(text2);
						int num;
						if (text3 != null && int.TryParse(text3, out num))
						{
							string value = asObject[text];
							this.packageIdToResourceId.Add(text2, value);
							string key = FileManager.PackageIDToPackageGroupID(text2);
							int num2;
							if (this.packageGroupToLatestVersion.TryGetValue(key, out num2))
							{
								if (num > num2)
								{
									this.packageGroupToLatestVersion.Remove(key);
									this.packageGroupToLatestVersion.Add(key, num);
								}
							}
							else
							{
								this.packageGroupToLatestVersion.Add(key, num);
							}
						}
					}
				}
			}
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x002566A8 File Offset: 0x00254AA8
		protected void FindUpdatesErrorCallback(string err)
		{
			SuperController.LogError("Error during hub request " + err);
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x002566BC File Offset: 0x00254ABC
		protected void FindUpdatesCallback(SimpleJSON.JSONNode jsonNode)
		{
			if (jsonNode != null)
			{
				JSONClass asObject = jsonNode.AsObject;
				if (asObject != null)
				{
					string text = asObject["status"];
					if (text != null && text == "error")
					{
						string str = jsonNode["error"];
						SuperController.LogError("findPackages returned error " + str);
					}
					else
					{
						JSONClass asObject2 = jsonNode["packages"].AsObject;
						if (asObject2 != null)
						{
							if (this.updates != null)
							{
								foreach (HubResourcePackageUI hubResourcePackageUI in this.updates)
								{
									UnityEngine.Object.Destroy(hubResourcePackageUI.gameObject);
								}
								this.updates.Clear();
							}
							else
							{
								this.updates = new List<HubResourcePackageUI>();
							}
							foreach (string text2 in this.checkUpdateNames)
							{
								JSONClass jsonclass = asObject2[text2].AsObject;
								if (jsonclass == null)
								{
									jsonclass = new JSONClass();
									jsonclass["filename"] = text2;
									jsonclass["downloadUrl"] = "null";
								}
								else
								{
									string text3 = jsonclass["filename"];
									if (text3 == null || text3 == "null")
									{
										jsonclass["filename"] = text2;
									}
								}
								HubResourcePackage hubResourcePackage = new HubResourcePackage(jsonclass, this, false);
								HubResourcePackage hubResourcePackage2 = hubResourcePackage;
								hubResourcePackage2.downloadCompleteCallback = (HubResourcePackage.DownloadCompleteCallback)Delegate.Combine(hubResourcePackage2.downloadCompleteCallback, new HubResourcePackage.DownloadCompleteCallback(this.ResourceDownloadComplete));
								RectTransform rectTransform = this.CreateDownloadPrefabInstance();
								if (rectTransform != null)
								{
									rectTransform.SetParent(this.updatesContainer, false);
									HubResourcePackageUI component = rectTransform.GetComponent<HubResourcePackageUI>();
									if (component != null)
									{
										this.updates.Add(component);
										hubResourcePackage.RegisterUI(component);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x00256938 File Offset: 0x00254D38
		public void OpenUpdatesPanel()
		{
			if (this._hubEnabled)
			{
				if (this.packageManager != null && this.updatesPanel != null && this.updatesContainer != null)
				{
					this.Show();
					if (this.updatesPanel != null)
					{
						this.updatesPanel.SetActive(true);
					}
					if (this.downloadQueue.Count == 0)
					{
						this.checkUpdateNames = new List<string>();
						if (this.packageGroupToLatestVersion != null)
						{
							foreach (VarPackageGroup varPackageGroup in FileManager.GetPackageGroups())
							{
								int num;
								if (this.packageGroupToLatestVersion.TryGetValue(varPackageGroup.Name, out num) && varPackageGroup.NewestVersion < num)
								{
									this.checkUpdateNames.Add(varPackageGroup.Name + ".latest");
								}
							}
						}
						if (this.checkUpdateNames.Count > 0)
						{
							JSONClass jsonclass = new JSONClass();
							jsonclass["source"] = "VaM";
							jsonclass["action"] = "findPackages";
							jsonclass["packages"] = string.Join(",", this.checkUpdateNames.ToArray());
							string postData = jsonclass.ToString();
							base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubBrowse.RequestSuccessCallback(this.FindUpdatesCallback), new HubBrowse.RequestErrorCallback(this.FindUpdatesErrorCallback)));
						}
						else if (this.updates != null)
						{
							foreach (HubResourcePackageUI hubResourcePackageUI in this.updates)
							{
								UnityEngine.Object.Destroy(hubResourcePackageUI.gameObject);
							}
							this.updates.Clear();
						}
						else
						{
							this.updates = new List<HubResourcePackageUI>();
						}
					}
				}
			}
			else
			{
				SuperController.LogError("Cannot perform action. Hub is disabled in User Preferences");
			}
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x00256B7C File Offset: 0x00254F7C
		public void CloseUpdatesPanel()
		{
			if (this.updatesPanel != null)
			{
				this.updatesPanel.SetActive(false);
			}
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x00256B9C File Offset: 0x00254F9C
		public void DownloadAllUpdates()
		{
			if (this.updates != null)
			{
				foreach (HubResourcePackageUI hubResourcePackageUI in this.updates)
				{
					hubResourcePackageUI.connectedItem.Download();
				}
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x060061B6 RID: 25014 RVA: 0x00256C08 File Offset: 0x00255008
		public bool IsDownloading
		{
			get
			{
				return this.isDownloadingJSON != null && this.isDownloadingJSON.val;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x060061B7 RID: 25015 RVA: 0x00256C22 File Offset: 0x00255022
		public int DownloadCount
		{
			get
			{
				if (this.downloadQueue != null)
				{
					return this.downloadQueue.Count;
				}
				return 0;
			}
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x00256C3C File Offset: 0x0025503C
		protected void RefreshCookies()
		{
			if (this.GetBrowserCookiesRoutine == null && this.browser != null)
			{
				base.StartCoroutine(this.GetBrowserCookies());
			}
		}

		// Token: 0x060061B9 RID: 25017 RVA: 0x00256C68 File Offset: 0x00255068
		protected IEnumerator GetBrowserCookies()
		{
			if (this.hubCookies == null)
			{
				this.hubCookies = new List<string>();
			}
			while (!this.browser.IsReady)
			{
				yield return null;
			}
			IPromise<List<Cookie>> promise = this.browser.CookieManager.GetCookies();
			yield return promise.ToWaitFor(true);
			this.hubCookies.Clear();
			foreach (Cookie cookie in promise.Value)
			{
				if (cookie.domain == this.cookieHost)
				{
					this.hubCookies.Add(string.Format("{0}={1}", cookie.name, cookie.value));
				}
			}
			this.GetBrowserCookiesRoutine = null;
			yield break;
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x00256C84 File Offset: 0x00255084
		protected IEnumerator DownloadRoutine()
		{
			for (;;)
			{
				if (this.downloadQueue.Count > 0)
				{
					this.isDownloadingJSON.val = true;
					this.downloadQueuedCountJSON.val = "Queued: " + this.downloadQueue.Count;
					HubBrowse.DownloadRequest request = this.downloadQueue.Dequeue();
					yield return this.BinaryGetRequest(request.url, request.startedCallback, request.successCallback, request.errorCallback, request.progressCallback, this.hubCookies);
					if (this.downloadQueue.Count == 0)
					{
						FileManager.Refresh();
					}
				}
				else
				{
					this.isDownloadingJSON.val = false;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060061BB RID: 25019 RVA: 0x00256CA0 File Offset: 0x002550A0
		protected void OnPackageRefresh()
		{
			if (this.items != null)
			{
				foreach (HubResourceItemUI hubResourceItemUI in this.items)
				{
					hubResourceItemUI.connectedItem.Refresh();
				}
			}
			if (this.missingPackages != null)
			{
				foreach (HubResourcePackageUI hubResourcePackageUI in this.missingPackages)
				{
					hubResourcePackageUI.connectedItem.Refresh();
				}
			}
			if (this.updates != null)
			{
				foreach (HubResourcePackageUI hubResourcePackageUI2 in this.updates)
				{
					hubResourcePackageUI2.connectedItem.Refresh();
				}
			}
			if (this.resourceDetailStack != null)
			{
				foreach (HubResourceItemDetailUI hubResourceItemDetailUI in this.resourceDetailStack)
				{
					hubResourceItemDetailUI.connectedItem.Refresh();
				}
			}
			FileManager.SyncAutoAlwaysAllowPlugins(this.packagesThatWereDownloaded);
		}

		// Token: 0x060061BC RID: 25020 RVA: 0x00256E2C File Offset: 0x0025522C
		public void ResourceDownloadComplete(HubResourcePackage hrp, bool alreadyHad)
		{
			if (!alreadyHad)
			{
				FileManager.PackageUIDAndPublisher packageUIDAndPublisher = new FileManager.PackageUIDAndPublisher();
				packageUIDAndPublisher.uid = hrp.Name;
				packageUIDAndPublisher.publisher = hrp.PublishingUser;
				this.packagesThatWereDownloaded.Add(packageUIDAndPublisher);
			}
		}

		// Token: 0x060061BD RID: 25021 RVA: 0x00256E6C File Offset: 0x0025526C
		public void QueueDownload(string url, string promotionalUrl, HubBrowse.BinaryRequestStartedCallback startedCallback, HubBrowse.RequestProgressCallback progressCallback, HubBrowse.BinaryRequestSuccessCallback successCallback, HubBrowse.RequestErrorCallback errorCallback)
		{
			HubBrowse.DownloadRequest downloadRequest = new HubBrowse.DownloadRequest();
			downloadRequest.url = url;
			if (this.downloadQueue.Count == 0)
			{
				downloadRequest.promotionalUrl = promotionalUrl;
			}
			downloadRequest.startedCallback = startedCallback;
			downloadRequest.progressCallback = progressCallback;
			downloadRequest.successCallback = successCallback;
			downloadRequest.errorCallback = errorCallback;
			this.downloadQueue.Enqueue(downloadRequest);
		}

		// Token: 0x060061BE RID: 25022 RVA: 0x00256EC8 File Offset: 0x002552C8
		protected void OpenDownloading()
		{
			if (this.savedResourceDetailsPanels != null)
			{
				foreach (string resource_id in this.savedResourceDetailsPanels.Keys)
				{
					this.OpenDetail(resource_id, false);
				}
			}
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x00256F38 File Offset: 0x00255338
		protected void GetInfoCallback(SimpleJSON.JSONNode jsonNode)
		{
			if (this.refreshingGetInfoPanel != null)
			{
				this.refreshingGetInfoPanel.gameObject.SetActive(false);
			}
			if (this.failedGetInfoPanel != null)
			{
				this.failedGetInfoPanel.gameObject.SetActive(false);
			}
			this.hubInfoCoroutine = null;
			this.hubInfoRefreshing = false;
			this.hubInfoSuccess = true;
			JSONClass asObject = jsonNode.AsObject;
			if (asObject != null)
			{
				if (asObject["location"] != null)
				{
					JSONArray asArray = asObject["location"].AsArray;
					if (asArray != null)
					{
						List<string> list = new List<string>();
						list.Add("All");
						IEnumerator enumerator = asArray.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								SimpleJSON.JSONNode d = (SimpleJSON.JSONNode)obj;
								list.Add(d);
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
						this.hostedOptionChooser.choices = list;
					}
				}
				if (asObject["category"] != null)
				{
					JSONArray asArray2 = asObject["category"].AsArray;
					if (asArray2 != null)
					{
						List<string> list2 = new List<string>();
						list2.Add("All");
						IEnumerator enumerator2 = asArray2.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								SimpleJSON.JSONNode d2 = (SimpleJSON.JSONNode)obj2;
								list2.Add(d2);
							}
						}
						finally
						{
							IDisposable disposable2;
							if ((disposable2 = (enumerator2 as IDisposable)) != null)
							{
								disposable2.Dispose();
							}
						}
						this.payTypeFilterChooser.choices = list2;
					}
				}
				if (asObject["type"] != null)
				{
					JSONArray asArray3 = asObject["type"].AsArray;
					if (asArray3 != null)
					{
						List<string> list3 = new List<string>();
						list3.Add("All");
						IEnumerator enumerator3 = asArray3.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								object obj3 = enumerator3.Current;
								SimpleJSON.JSONNode d3 = (SimpleJSON.JSONNode)obj3;
								list3.Add(d3);
							}
						}
						finally
						{
							IDisposable disposable3;
							if ((disposable3 = (enumerator3 as IDisposable)) != null)
							{
								disposable3.Dispose();
							}
						}
						this.categoryFilterChooser.choices = list3;
					}
				}
				if (asObject["users"] != null)
				{
					JSONClass asObject2 = asObject["users"].AsObject;
					if (asObject2 != null)
					{
						List<string> list4 = new List<string>();
						list4.Add("All");
						foreach (string item in asObject2.Keys)
						{
							list4.Add(item);
						}
						this.creatorFilterChooser.choices = list4;
					}
				}
				if (asObject["tags"] != null)
				{
					JSONClass asObject3 = asObject["tags"].AsObject;
					if (asObject3 != null)
					{
						List<string> list5 = new List<string>();
						list5.Add("All");
						foreach (string item2 in asObject3.Keys)
						{
							list5.Add(item2);
						}
						this.tagsFilterChooser.choices = list5;
					}
				}
				if (asObject["sort"] != null)
				{
					JSONArray asArray4 = asObject["sort"].AsArray;
					if (asArray4 != null)
					{
						List<string> list6 = new List<string>();
						List<string> list7 = new List<string>();
						list7.Add("None");
						IEnumerator enumerator6 = asArray4.GetEnumerator();
						try
						{
							while (enumerator6.MoveNext())
							{
								object obj4 = enumerator6.Current;
								SimpleJSON.JSONNode d4 = (SimpleJSON.JSONNode)obj4;
								list6.Add(d4);
								list7.Add(d4);
							}
						}
						finally
						{
							IDisposable disposable4;
							if ((disposable4 = (enumerator6 as IDisposable)) != null)
							{
								disposable4.Dispose();
							}
						}
						this.sortPrimaryChooser.choices = list6;
						this.sortSecondaryChooser.choices = list7;
					}
				}
				string text = asObject["last_update"];
				if (this.packagesJSONUrl != null && this.packagesJSONUrl != string.Empty && text != null)
				{
					string uri = this.packagesJSONUrl + "?" + text;
					base.StartCoroutine(this.GetRequest(uri, new HubBrowse.RequestSuccessCallback(this.GetPackagesJSONCallback), new HubBrowse.RequestErrorCallback(this.GetPackagesJSONErrorCallback)));
				}
			}
		}

		// Token: 0x060061C0 RID: 25024 RVA: 0x0025745C File Offset: 0x0025585C
		protected void GetInfoErrorCallback(string err)
		{
			if (this.refreshingGetInfoPanel != null)
			{
				this.refreshingGetInfoPanel.gameObject.SetActive(false);
			}
			if (this.failedGetInfoPanel != null)
			{
				this.failedGetInfoPanel.gameObject.SetActive(true);
			}
			if (this.getInfoErrorText != null)
			{
				this.getInfoErrorText.text = err;
			}
			this.hubInfoCoroutine = null;
			this.hubInfoRefreshing = false;
			this.hubInfoSuccess = false;
		}

		// Token: 0x060061C1 RID: 25025 RVA: 0x002574E0 File Offset: 0x002558E0
		protected void GetHubInfo()
		{
			if (!this.hubInfoRefreshing)
			{
				if (this.failedGetInfoPanel != null)
				{
					this.failedGetInfoPanel.gameObject.SetActive(false);
				}
				JSONClass jsonclass = new JSONClass();
				jsonclass["source"] = "VaM";
				jsonclass["action"] = "getInfo";
				string postData = jsonclass.ToString();
				this.hubInfoRefreshing = true;
				if (this.refreshingGetInfoPanel != null)
				{
					this.refreshingGetInfoPanel.gameObject.SetActive(true);
				}
				this.hubInfoCoroutine = base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubBrowse.RequestSuccessCallback(this.GetInfoCallback), new HubBrowse.RequestErrorCallback(this.GetInfoErrorCallback)));
			}
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x002575AB File Offset: 0x002559AB
		protected void CancelGetHubInfo()
		{
			if (this.hubInfoRefreshing && this.hubInfoCoroutine != null)
			{
				base.StopCoroutine(this.hubInfoCoroutine);
				this.GetInfoErrorCallback("Cancelled");
			}
		}

		// Token: 0x060061C3 RID: 25027 RVA: 0x002575DC File Offset: 0x002559DC
		protected void Init()
		{
			this.hubEnabledJSON = new JSONStorableBool("hubEnabled", this._hubEnabled, new JSONStorableBool.SetBoolCallback(this.SyncHubEnabled));
			this.enableHubAction = new JSONStorableAction("EnableHub", new JSONStorableAction.ActionCallback(this.EnableHub));
			this.webBrowserEnabledJSON = new JSONStorableBool("webBrowserEnabled", this._webBrowserEnabled, new JSONStorableBool.SetBoolCallback(this.SyncWebBrowserEnabled));
			this.enableWebBrowserAction = new JSONStorableAction("EnableWebBrowser", new JSONStorableAction.ActionCallback(this.EnableWebBrowser));
			this.cancelGetHubInfoAction = new JSONStorableAction("CancelGetHubInfo", new JSONStorableAction.ActionCallback(this.CancelGetHubInfo));
			this.retryGetHubInfoAction = new JSONStorableAction("RetryGetHubInfo", new JSONStorableAction.ActionCallback(this.GetHubInfo));
			this.numResourcesJSON = new JSONStorableString("numResources", "0", new JSONStorableString.SetStringCallback(this.SyncNumResources));
			this.pageInfoJSON = new JSONStorableString("pageInfo", "Page 0 of 0");
			this.numPagesJSON = new JSONStorableString("numPages", "0", new JSONStorableString.SetStringCallback(this.SyncNumPages));
			this.currentPageJSON = new JSONStorableString("currentPage", "1", new JSONStorableString.SetStringCallback(this.SyncCurrentPage));
			this.firstPageAction = new JSONStorableAction("FirstPage", new JSONStorableAction.ActionCallback(this.FirstPage));
			this.previousPageAction = new JSONStorableAction("PreviousPage", new JSONStorableAction.ActionCallback(this.PreviousPage));
			base.RegisterAction(this.previousPageAction);
			this.nextPageAction = new JSONStorableAction("NextPage", new JSONStorableAction.ActionCallback(this.NextPage));
			base.RegisterAction(this.nextPageAction);
			this.refreshResourcesAction = new JSONStorableAction("RefreshResources", new JSONStorableAction.ActionCallback(this.ResetRefresh));
			base.RegisterAction(this.refreshResourcesAction);
			this.clearFiltersAction = new JSONStorableAction("ResetFilters", new JSONStorableAction.ActionCallback(this.ResetFiltersAndRefresh));
			base.RegisterAction(this.clearFiltersAction);
			List<string> choicesList = new List<string>
			{
				"All"
			};
			this.hostedOptionChooser = new JSONStorableStringChooser("hostedOption", choicesList, this._hostedOption, "Hosted Option", new JSONStorableStringChooser.SetStringCallback(this.SyncHostedOption));
			this.hostedOptionChooser.isStorable = false;
			this.hostedOptionChooser.isRestorable = false;
			base.RegisterStringChooser(this.hostedOptionChooser);
			this.searchFilterJSON = new JSONStorableString("searchFilter", string.Empty, new JSONStorableString.SetStringCallback(this.SyncSearchFilter));
			this.searchFilterJSON.enableOnChange = true;
			this.searchFilterJSON.isStorable = false;
			this.searchFilterJSON.isRestorable = false;
			base.RegisterString(this.searchFilterJSON);
			List<string> choicesList2 = new List<string>
			{
				"All"
			};
			this.payTypeFilterChooser = new JSONStorableStringChooser("payType", choicesList2, this._payTypeFilter, "Pay Type", new JSONStorableStringChooser.SetStringCallback(this.SyncPayTypeFilter));
			this.payTypeFilterChooser.isStorable = false;
			this.payTypeFilterChooser.isRestorable = false;
			base.RegisterStringChooser(this.payTypeFilterChooser);
			List<string> choicesList3 = new List<string>
			{
				"All"
			};
			this.categoryFilterChooser = new JSONStorableStringChooser("category", choicesList3, this._categoryFilter, "Category", new JSONStorableStringChooser.SetStringCallback(this.SyncCategoryFilter));
			this.categoryFilterChooser.isStorable = false;
			this.categoryFilterChooser.isRestorable = false;
			base.RegisterStringChooser(this.categoryFilterChooser);
			List<string> choicesList4 = new List<string>
			{
				"All"
			};
			this.creatorFilterChooser = new JSONStorableStringChooser("creator", choicesList4, this._creatorFilter, "Creator", new JSONStorableStringChooser.SetStringCallback(this.SyncCreatorFilter));
			this.creatorFilterChooser.isStorable = false;
			this.creatorFilterChooser.isRestorable = false;
			base.RegisterStringChooser(this.creatorFilterChooser);
			List<string> choicesList5 = new List<string>
			{
				"All"
			};
			this.tagsFilterChooser = new JSONStorableStringChooser("tags", choicesList5, this._tagsFilter, "Tags", new JSONStorableStringChooser.SetStringCallback(this.SyncTagsFilter));
			this.tagsFilterChooser.isStorable = false;
			this.tagsFilterChooser.isRestorable = false;
			base.RegisterStringChooser(this.tagsFilterChooser);
			List<string> choicesList6 = new List<string>
			{
				"Latest Update"
			};
			this.sortPrimaryChooser = new JSONStorableStringChooser("sortPrimary", choicesList6, this._sortPrimary, "Primary Sort", new JSONStorableStringChooser.SetStringCallback(this.SyncSortPrimary));
			this.sortPrimaryChooser.isStorable = false;
			this.sortPrimaryChooser.isRestorable = false;
			base.RegisterStringChooser(this.sortPrimaryChooser);
			List<string> choicesList7 = new List<string>
			{
				"None"
			};
			this.sortSecondaryChooser = new JSONStorableStringChooser("sortSecondary", choicesList7, this._sortSecondary, "Secondary Sort", new JSONStorableStringChooser.SetStringCallback(this.SyncSortSecondary));
			this.sortSecondaryChooser.isStorable = false;
			this.sortSecondaryChooser.isRestorable = false;
			base.RegisterStringChooser(this.sortSecondaryChooser);
			this.openMissingPackagesPanelAction = new JSONStorableAction("OpenMissingPackagesPanel", new JSONStorableAction.ActionCallback(this.OpenMissingPackagesPanel));
			base.RegisterAction(this.openMissingPackagesPanelAction);
			this.closeMissingPackagesPanelAction = new JSONStorableAction("CloseMissingPackagesPanel", new JSONStorableAction.ActionCallback(this.CloseMissingPackagesPanel));
			base.RegisterAction(this.closeMissingPackagesPanelAction);
			this.downloadAllMissingPackagesAction = new JSONStorableAction("DownloadAllMissingPackages", new JSONStorableAction.ActionCallback(this.DownloadAllMissingPackages));
			base.RegisterAction(this.downloadAllMissingPackagesAction);
			this.openUpdatesPanelAction = new JSONStorableAction("OpenUpdatesPanel", new JSONStorableAction.ActionCallback(this.OpenUpdatesPanel));
			base.RegisterAction(this.openUpdatesPanelAction);
			this.closeUpdatesPanelAction = new JSONStorableAction("CloseUpdatesPanel", new JSONStorableAction.ActionCallback(this.CloseUpdatesPanel));
			base.RegisterAction(this.closeUpdatesPanelAction);
			this.downloadAllUpdatesAction = new JSONStorableAction("DownloadAllUpdates", new JSONStorableAction.ActionCallback(this.DownloadAllUpdates));
			base.RegisterAction(this.downloadAllUpdatesAction);
			this.isDownloadingJSON = new JSONStorableBool("isDownloading", false);
			this.downloadQueuedCountJSON = new JSONStorableString("downloadQueuedCount", "Queued: 0");
			this.openDownloadingAction = new JSONStorableAction("OpenDownloading", new JSONStorableAction.ActionCallback(this.OpenDownloading));
			base.RegisterAction(this.openDownloadingAction);
			this.resourceDetailStack = new Stack<HubResourceItemDetailUI>();
			this.savedResourceDetailsPanels = new Dictionary<string, HubResourceItemDetailUI>();
			this.downloadQueue = new Queue<HubBrowse.DownloadRequest>();
			this.packagesThatWereDownloaded = new HashSet<FileManager.PackageUIDAndPublisher>();
			base.StartCoroutine(this.DownloadRoutine());
			FileManager.RegisterRefreshHandler(new OnRefresh(this.OnPackageRefresh));
		}

		// Token: 0x060061C4 RID: 25028 RVA: 0x00257C4C File Offset: 0x0025604C
		protected override void InitUI(Transform t, bool isAlt)
		{
			if (t != null)
			{
				HubBrowseUI componentInChildren = t.GetComponentInChildren<HubBrowseUI>(true);
				if (componentInChildren != null)
				{
					if (!isAlt)
					{
						this.hubBrowseUI = componentInChildren;
						this.itemContainer = componentInChildren.itemContainer;
						this.itemScrollRect = componentInChildren.itemScrollRect;
						this.refreshingGetInfoPanel = componentInChildren.refreshingGetInfoPanel;
						if (this.refreshingGetInfoPanel != null && this.hubInfoRefreshing)
						{
							this.refreshingGetInfoPanel.gameObject.SetActive(true);
						}
						this.failedGetInfoPanel = componentInChildren.failedGetInfoPanel;
						if (this.failedGetInfoPanel != null && !this.hubInfoSuccess && !this.hubInfoRefreshing)
						{
							this.failedGetInfoPanel.gameObject.SetActive(true);
						}
						this.getInfoErrorText = componentInChildren.getInfoErrorText;
						this.detailPanel = componentInChildren.detailPanel;
						this.resourceDetailContainer = componentInChildren.resourceDetailContainer;
						this.browser = componentInChildren.browser;
						this.webBrowser = componentInChildren.webBrowser;
						this.isWebLoadingIndicator = componentInChildren.isWebLoadingIndicator;
						this.refreshIndicator = componentInChildren.refreshIndicator;
						this.missingPackagesPanel = componentInChildren.missingPackagesPanel;
						this.missingPackagesContainer = componentInChildren.missingPackagesContainer;
						this.updatesPanel = componentInChildren.updatesPanel;
						this.updatesContainer = componentInChildren.updatesContainer;
					}
					this.hubEnabledJSON.RegisterNegativeIndicator(componentInChildren.hubEnabledNegativeIndicator, isAlt);
					this.enableHubAction.RegisterButton(componentInChildren.enableHubButton, isAlt);
					this.webBrowserEnabledJSON.RegisterNegativeIndicator(componentInChildren.webBrowserEnabledNegativeIndicator, isAlt);
					this.enableWebBrowserAction.RegisterButton(componentInChildren.enabledWebBrowserButton, isAlt);
					this.cancelGetHubInfoAction.RegisterButton(componentInChildren.cancelGetHubInfoButton, isAlt);
					this.retryGetHubInfoAction.RegisterButton(componentInChildren.retryGetHubInfoButton, isAlt);
					this.pageInfoJSON.RegisterText(componentInChildren.pageInfoText, isAlt);
					this.numResourcesJSON.RegisterText(componentInChildren.numResourcesText, isAlt);
					this.firstPageAction.RegisterButton(componentInChildren.firstPageButton, isAlt);
					this.previousPageAction.RegisterButton(componentInChildren.previousPageButton, isAlt);
					this.nextPageAction.RegisterButton(componentInChildren.nextPageButton, isAlt);
					this.refreshResourcesAction.RegisterButton(componentInChildren.refreshButton, isAlt);
					this.clearFiltersAction.RegisterButton(componentInChildren.clearFiltersButton, isAlt);
					this.hostedOptionChooser.RegisterPopup(componentInChildren.hostedOptionPopup, isAlt);
					this.searchFilterJSON.RegisterInputField(componentInChildren.searchInputField, isAlt);
					this.payTypeFilterChooser.RegisterPopup(componentInChildren.payTypeFilterPopup, isAlt);
					this.categoryFilterChooser.RegisterPopup(componentInChildren.categoryFilterPopup, isAlt);
					this.creatorFilterChooser.RegisterPopup(componentInChildren.creatorFilterPopup, isAlt);
					this.tagsFilterChooser.RegisterPopup(componentInChildren.tagsFilterPopup, isAlt);
					this.sortPrimaryChooser.RegisterPopup(componentInChildren.sortPrimaryPopup, isAlt);
					this.sortSecondaryChooser.RegisterPopup(componentInChildren.sortSecondaryPopup, isAlt);
					this.openMissingPackagesPanelAction.RegisterButton(componentInChildren.openMissingPackagesPanelButton, isAlt);
					this.closeMissingPackagesPanelAction.RegisterButton(componentInChildren.closeMissingPackagesPanelButton, isAlt);
					this.downloadAllMissingPackagesAction.RegisterButton(componentInChildren.downloadAllMissingPackagesButton, isAlt);
					this.openUpdatesPanelAction.RegisterButton(componentInChildren.openUpdatesPanelButton, isAlt);
					this.closeUpdatesPanelAction.RegisterButton(componentInChildren.closeUpdatesPanelButton, isAlt);
					this.downloadAllUpdatesAction.RegisterButton(componentInChildren.downloadAllUpdatesButton, isAlt);
					this.isDownloadingJSON.RegisterIndicator(componentInChildren.isDownloadingIndicator, isAlt);
					this.downloadQueuedCountJSON.RegisterText(componentInChildren.downloadQueuedCountText, isAlt);
					this.openDownloadingAction.RegisterButton(componentInChildren.openDownloadingButton, isAlt);
				}
			}
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x00257FBB File Offset: 0x002563BB
		protected void OnLoad(ZenFulcrum.EmbeddedBrowser.JSONNode loadData)
		{
			this.browser.EvalJS("\r\n\t\t\t\twindow.scrollTo(0,0);\r\n\t\t\t", "scripted command");
			this.RefreshCookies();
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x00257FDC File Offset: 0x002563DC
		protected override void Awake()
		{
			if (!this.awakecalled)
			{
				HubBrowse.singleton = this;
				base.Awake();
				this.Init();
				this.InitUI();
				this.InitUIAlt();
				if (this.browser != null)
				{
					this.browser.onLoad += this.OnLoad;
				}
			}
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x0025803A File Offset: 0x0025643A
		protected void Update()
		{
			if (this.browser != null && this.isWebLoadingIndicator != null)
			{
				this.isWebLoadingIndicator.SetActive(this.browser.IsLoadingRaw);
			}
		}

		// Token: 0x040051E4 RID: 20964
		public static HubBrowse singleton;

		// Token: 0x040051E5 RID: 20965
		[SerializeField]
		protected string cookieHost;

		// Token: 0x040051E6 RID: 20966
		[SerializeField]
		protected string apiUrl;

		// Token: 0x040051E7 RID: 20967
		[SerializeField]
		protected string packagesJSONUrl;

		// Token: 0x040051E8 RID: 20968
		protected bool _hubEnabled;

		// Token: 0x040051E9 RID: 20969
		protected JSONStorableBool hubEnabledJSON;

		// Token: 0x040051EA RID: 20970
		public HubBrowse.EnableHubCallback enableHubCallbacks;

		// Token: 0x040051EB RID: 20971
		protected JSONStorableAction enableHubAction;

		// Token: 0x040051EC RID: 20972
		protected bool _webBrowserEnabled;

		// Token: 0x040051ED RID: 20973
		protected JSONStorableBool webBrowserEnabledJSON;

		// Token: 0x040051EE RID: 20974
		public HubBrowse.EnableWebBrowserCallback enableWebBrowserCallbacks;

		// Token: 0x040051EF RID: 20975
		protected JSONStorableAction enableWebBrowserAction;

		// Token: 0x040051F0 RID: 20976
		protected HubBrowseUI hubBrowseUI;

		// Token: 0x040051F1 RID: 20977
		public RectTransform itemPrefab;

		// Token: 0x040051F2 RID: 20978
		protected RectTransform itemContainer;

		// Token: 0x040051F3 RID: 20979
		protected ScrollRect itemScrollRect;

		// Token: 0x040051F4 RID: 20980
		protected List<HubResourceItemUI> items;

		// Token: 0x040051F5 RID: 20981
		public RectTransform resourceDetailPrefab;

		// Token: 0x040051F6 RID: 20982
		protected GameObject detailPanel;

		// Token: 0x040051F7 RID: 20983
		public RectTransform packageDownloadPrefab;

		// Token: 0x040051F8 RID: 20984
		public RectTransform creatorSupportButtonPrefab;

		// Token: 0x040051F9 RID: 20985
		protected RectTransform resourceDetailContainer;

		// Token: 0x040051FA RID: 20986
		protected Browser browser;

		// Token: 0x040051FB RID: 20987
		protected VRWebBrowser webBrowser;

		// Token: 0x040051FC RID: 20988
		protected GameObject isWebLoadingIndicator;

		// Token: 0x040051FD RID: 20989
		protected GameObject refreshIndicator;

		// Token: 0x040051FE RID: 20990
		protected bool _isShowing;

		// Token: 0x040051FF RID: 20991
		public HubBrowse.PreShowCallback preShowCallbacks;

		// Token: 0x04005200 RID: 20992
		protected bool _hasBeenRefreshed;

		// Token: 0x04005201 RID: 20993
		protected Coroutine refreshResourcesRoutine;

		// Token: 0x04005202 RID: 20994
		protected JSONStorableAction refreshResourcesAction;

		// Token: 0x04005203 RID: 20995
		protected JSONStorableString numResourcesJSON;

		// Token: 0x04005204 RID: 20996
		protected JSONStorableString pageInfoJSON;

		// Token: 0x04005205 RID: 20997
		protected int _numPagesInt;

		// Token: 0x04005206 RID: 20998
		protected JSONStorableString numPagesJSON;

		// Token: 0x04005207 RID: 20999
		protected int _numPerPageInt = 48;

		// Token: 0x04005208 RID: 21000
		protected JSONStorableFloat numPerPageJSON;

		// Token: 0x04005209 RID: 21001
		protected string _currentPageString = "1";

		// Token: 0x0400520A RID: 21002
		protected int _currentPageInt = 1;

		// Token: 0x0400520B RID: 21003
		protected JSONStorableString currentPageJSON;

		// Token: 0x0400520C RID: 21004
		protected JSONStorableAction firstPageAction;

		// Token: 0x0400520D RID: 21005
		protected JSONStorableAction previousPageAction;

		// Token: 0x0400520E RID: 21006
		protected JSONStorableAction nextPageAction;

		// Token: 0x0400520F RID: 21007
		protected JSONStorableAction clearFiltersAction;

		// Token: 0x04005210 RID: 21008
		protected string _hostedOption = "Hub And Dependencies";

		// Token: 0x04005211 RID: 21009
		protected JSONStorableStringChooser hostedOptionChooser;

		// Token: 0x04005212 RID: 21010
		protected string _payTypeFilter = "Free";

		// Token: 0x04005213 RID: 21011
		protected JSONStorableStringChooser payTypeFilterChooser;

		// Token: 0x04005214 RID: 21012
		protected const float _triggerDelay = 0.5f;

		// Token: 0x04005215 RID: 21013
		protected float triggerCountdown;

		// Token: 0x04005216 RID: 21014
		protected Coroutine triggerResetRefreshRoutine;

		// Token: 0x04005217 RID: 21015
		protected string _minLengthSearchFilter = string.Empty;

		// Token: 0x04005218 RID: 21016
		protected string _searchFilter = string.Empty;

		// Token: 0x04005219 RID: 21017
		protected JSONStorableString searchFilterJSON;

		// Token: 0x0400521A RID: 21018
		protected string _categoryFilter = "All";

		// Token: 0x0400521B RID: 21019
		protected JSONStorableStringChooser categoryFilterChooser;

		// Token: 0x0400521C RID: 21020
		protected string _creatorFilter = "All";

		// Token: 0x0400521D RID: 21021
		protected JSONStorableStringChooser creatorFilterChooser;

		// Token: 0x0400521E RID: 21022
		protected string _tagsFilter = "All";

		// Token: 0x0400521F RID: 21023
		protected JSONStorableStringChooser tagsFilterChooser;

		// Token: 0x04005220 RID: 21024
		protected string _sortPrimary = "Latest Update";

		// Token: 0x04005221 RID: 21025
		protected JSONStorableStringChooser sortPrimaryChooser;

		// Token: 0x04005222 RID: 21026
		protected string _sortSecondary = "None";

		// Token: 0x04005223 RID: 21027
		protected JSONStorableStringChooser sortSecondaryChooser;

		// Token: 0x04005224 RID: 21028
		protected Dictionary<string, HubResourceItemDetailUI> savedResourceDetailsPanels;

		// Token: 0x04005225 RID: 21029
		protected Stack<HubResourceItemDetailUI> resourceDetailStack;

		// Token: 0x04005226 RID: 21030
		public PackageBuilder packageManager;

		// Token: 0x04005227 RID: 21031
		protected GameObject missingPackagesPanel;

		// Token: 0x04005228 RID: 21032
		protected RectTransform missingPackagesContainer;

		// Token: 0x04005229 RID: 21033
		protected List<string> checkMissingPackageNames;

		// Token: 0x0400522A RID: 21034
		protected List<HubResourcePackageUI> missingPackages;

		// Token: 0x0400522B RID: 21035
		protected JSONStorableAction openMissingPackagesPanelAction;

		// Token: 0x0400522C RID: 21036
		protected JSONStorableAction closeMissingPackagesPanelAction;

		// Token: 0x0400522D RID: 21037
		protected JSONStorableAction downloadAllMissingPackagesAction;

		// Token: 0x0400522E RID: 21038
		protected GameObject updatesPanel;

		// Token: 0x0400522F RID: 21039
		protected RectTransform updatesContainer;

		// Token: 0x04005230 RID: 21040
		protected List<string> checkUpdateNames;

		// Token: 0x04005231 RID: 21041
		protected List<HubResourcePackageUI> updates;

		// Token: 0x04005232 RID: 21042
		protected Dictionary<string, int> packageGroupToLatestVersion;

		// Token: 0x04005233 RID: 21043
		protected Dictionary<string, string> packageIdToResourceId;

		// Token: 0x04005234 RID: 21044
		protected JSONStorableAction openUpdatesPanelAction;

		// Token: 0x04005235 RID: 21045
		protected JSONStorableAction closeUpdatesPanelAction;

		// Token: 0x04005236 RID: 21046
		protected JSONStorableAction downloadAllUpdatesAction;

		// Token: 0x04005237 RID: 21047
		protected JSONStorableBool isDownloadingJSON;

		// Token: 0x04005238 RID: 21048
		protected JSONStorableString downloadQueuedCountJSON;

		// Token: 0x04005239 RID: 21049
		protected Queue<HubBrowse.DownloadRequest> downloadQueue;

		// Token: 0x0400523A RID: 21050
		protected List<string> hubCookies;

		// Token: 0x0400523B RID: 21051
		protected Coroutine GetBrowserCookiesRoutine;

		// Token: 0x0400523C RID: 21052
		protected HashSet<FileManager.PackageUIDAndPublisher> packagesThatWereDownloaded;

		// Token: 0x0400523D RID: 21053
		protected JSONStorableAction openDownloadingAction;

		// Token: 0x0400523E RID: 21054
		protected RectTransform refreshingGetInfoPanel;

		// Token: 0x0400523F RID: 21055
		protected RectTransform failedGetInfoPanel;

		// Token: 0x04005240 RID: 21056
		protected Text getInfoErrorText;

		// Token: 0x04005241 RID: 21057
		protected bool hubInfoSuccess;

		// Token: 0x04005242 RID: 21058
		protected bool hubInfoCompleted;

		// Token: 0x04005243 RID: 21059
		protected bool hubInfoRefreshing;

		// Token: 0x04005244 RID: 21060
		protected Coroutine hubInfoCoroutine;

		// Token: 0x04005245 RID: 21061
		protected JSONStorableAction cancelGetHubInfoAction;

		// Token: 0x04005246 RID: 21062
		protected JSONStorableAction retryGetHubInfoAction;

		// Token: 0x02000CAF RID: 3247
		// (Invoke) Token: 0x060061C9 RID: 25033
		public delegate void BinaryRequestStartedCallback();

		// Token: 0x02000CB0 RID: 3248
		// (Invoke) Token: 0x060061CD RID: 25037
		public delegate void BinaryRequestSuccessCallback(byte[] data, Dictionary<string, string> responseHeaders);

		// Token: 0x02000CB1 RID: 3249
		// (Invoke) Token: 0x060061D1 RID: 25041
		public delegate void RequestSuccessCallback(SimpleJSON.JSONNode jsonNode);

		// Token: 0x02000CB2 RID: 3250
		// (Invoke) Token: 0x060061D5 RID: 25045
		public delegate void RequestErrorCallback(string err);

		// Token: 0x02000CB3 RID: 3251
		// (Invoke) Token: 0x060061D9 RID: 25049
		public delegate void RequestProgressCallback(float progress);

		// Token: 0x02000CB4 RID: 3252
		// (Invoke) Token: 0x060061DD RID: 25053
		public delegate void EnableHubCallback();

		// Token: 0x02000CB5 RID: 3253
		// (Invoke) Token: 0x060061E1 RID: 25057
		public delegate void EnableWebBrowserCallback();

		// Token: 0x02000CB6 RID: 3254
		// (Invoke) Token: 0x060061E5 RID: 25061
		public delegate void PreShowCallback();

		// Token: 0x02000CB7 RID: 3255
		protected class DownloadRequest
		{
			// Token: 0x060061E8 RID: 25064 RVA: 0x00258074 File Offset: 0x00256474
			public DownloadRequest()
			{
			}

			// Token: 0x04005247 RID: 21063
			public string url;

			// Token: 0x04005248 RID: 21064
			public string promotionalUrl;

			// Token: 0x04005249 RID: 21065
			public HubBrowse.BinaryRequestStartedCallback startedCallback;

			// Token: 0x0400524A RID: 21066
			public HubBrowse.RequestProgressCallback progressCallback;

			// Token: 0x0400524B RID: 21067
			public HubBrowse.BinaryRequestSuccessCallback successCallback;

			// Token: 0x0400524C RID: 21068
			public HubBrowse.RequestErrorCallback errorCallback;
		}

		// Token: 0x0200101E RID: 4126
		[CompilerGenerated]
		private sealed class <GetRequest>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060076E9 RID: 30441 RVA: 0x0025807C File Offset: 0x0025647C
			[DebuggerHidden]
			public <GetRequest>c__Iterator0()
			{
			}

			// Token: 0x060076EA RID: 30442 RVA: 0x00258084 File Offset: 0x00256484
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					webRequest = UnityWebRequest.Get(uri);
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1U:
						break;
					default:
						webRequest.SendWebRequest();
						break;
					}
					if (!webRequest.isDone)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
					if (webRequest.isNetworkError)
					{
						UnityEngine.Debug.LogError(uri + ": Error: " + webRequest.error);
						if (errorCallback != null)
						{
							errorCallback(webRequest.error);
						}
					}
					else
					{
						SimpleJSON.JSONNode jsonNode = JSON.Parse(webRequest.downloadHandler.text);
						if (callback != null)
						{
							callback(jsonNode);
						}
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700119F RID: 4511
			// (get) Token: 0x060076EB RID: 30443 RVA: 0x002581C0 File Offset: 0x002565C0
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011A0 RID: 4512
			// (get) Token: 0x060076EC RID: 30444 RVA: 0x002581C8 File Offset: 0x002565C8
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060076ED RID: 30445 RVA: 0x002581D0 File Offset: 0x002565D0
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			// Token: 0x060076EE RID: 30446 RVA: 0x00258224 File Offset: 0x00256624
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060076EF RID: 30447 RVA: 0x0025822B File Offset: 0x0025662B
			private void <>__Finally0()
			{
				if (webRequest != null)
				{
					((IDisposable)webRequest).Dispose();
				}
			}

			// Token: 0x04006AF8 RID: 27384
			internal UnityWebRequest <webRequest>__1;

			// Token: 0x04006AF9 RID: 27385
			internal string uri;

			// Token: 0x04006AFA RID: 27386
			internal HubBrowse.RequestErrorCallback errorCallback;

			// Token: 0x04006AFB RID: 27387
			internal HubBrowse.RequestSuccessCallback callback;

			// Token: 0x04006AFC RID: 27388
			internal object $current;

			// Token: 0x04006AFD RID: 27389
			internal bool $disposing;

			// Token: 0x04006AFE RID: 27390
			internal int $PC;
		}

		// Token: 0x0200101F RID: 4127
		[CompilerGenerated]
		private sealed class <BinaryGetRequest>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060076F0 RID: 30448 RVA: 0x00258243 File Offset: 0x00256643
			[DebuggerHidden]
			public <BinaryGetRequest>c__Iterator1()
			{
			}

			// Token: 0x060076F1 RID: 30449 RVA: 0x0025824C File Offset: 0x0025664C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					webRequest = UnityWebRequest.Get(uri);
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1U:
						break;
					default:
						cookieHeader = "vamhubconsent=1";
						if (cookies != null)
						{
							foreach (string str in cookies)
							{
								cookieHeader = cookieHeader + ";" + str;
							}
						}
						try
						{
							webRequest.SetRequestHeader("Cookie", cookieHeader);
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogError(string.Concat(new string[]
							{
								"Bad request header ",
								cookieHeader,
								" for Hub download of ",
								uri,
								": ",
								ex.ToString()
							}));
							webRequest.SetRequestHeader("Cookie", "vamhubconsent=1");
						}
						webRequest.SendWebRequest();
						if (startedCallback != null)
						{
							startedCallback();
						}
						break;
					}
					if (!webRequest.isDone)
					{
						if (progressCallback != null)
						{
							progressCallback(webRequest.downloadProgress);
						}
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
					if (webRequest.isNetworkError)
					{
						UnityEngine.Debug.LogError(uri + ": Error: " + webRequest.error);
						if (errorCallback != null)
						{
							errorCallback(webRequest.error);
						}
					}
					else
					{
						Dictionary<string, string> responseHeaders = webRequest.GetResponseHeaders();
						if (successCallback != null)
						{
							successCallback(webRequest.downloadHandler.data, responseHeaders);
						}
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011A1 RID: 4513
			// (get) Token: 0x060076F2 RID: 30450 RVA: 0x002584E4 File Offset: 0x002568E4
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011A2 RID: 4514
			// (get) Token: 0x060076F3 RID: 30451 RVA: 0x002584EC File Offset: 0x002568EC
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060076F4 RID: 30452 RVA: 0x002584F4 File Offset: 0x002568F4
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			// Token: 0x060076F5 RID: 30453 RVA: 0x00258548 File Offset: 0x00256948
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060076F6 RID: 30454 RVA: 0x0025854F File Offset: 0x0025694F
			private void <>__Finally0()
			{
				if (webRequest != null)
				{
					((IDisposable)webRequest).Dispose();
				}
			}

			// Token: 0x04006AFF RID: 27391
			internal UnityWebRequest <webRequest>__1;

			// Token: 0x04006B00 RID: 27392
			internal string uri;

			// Token: 0x04006B01 RID: 27393
			internal string <cookieHeader>__2;

			// Token: 0x04006B02 RID: 27394
			internal List<string> cookies;

			// Token: 0x04006B03 RID: 27395
			internal HubBrowse.BinaryRequestStartedCallback startedCallback;

			// Token: 0x04006B04 RID: 27396
			internal HubBrowse.RequestProgressCallback progressCallback;

			// Token: 0x04006B05 RID: 27397
			internal HubBrowse.RequestErrorCallback errorCallback;

			// Token: 0x04006B06 RID: 27398
			internal HubBrowse.BinaryRequestSuccessCallback successCallback;

			// Token: 0x04006B07 RID: 27399
			internal object $current;

			// Token: 0x04006B08 RID: 27400
			internal bool $disposing;

			// Token: 0x04006B09 RID: 27401
			internal int $PC;
		}

		// Token: 0x02001020 RID: 4128
		[CompilerGenerated]
		private sealed class <PostRequest>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060076F7 RID: 30455 RVA: 0x00258567 File Offset: 0x00256967
			[DebuggerHidden]
			public <PostRequest>c__Iterator2()
			{
			}

			// Token: 0x060076F8 RID: 30456 RVA: 0x00258570 File Offset: 0x00256970
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					webRequest = UnityWebRequest.Post(uri, postData);
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1U:
						pages = uri.Split(new char[]
						{
							'/'
						});
						page = pages.Length - 1;
						if (webRequest.isNetworkError)
						{
							UnityEngine.Debug.LogError(pages[page] + ": Error: " + webRequest.error);
							if (errorCallback != null)
							{
								errorCallback(webRequest.error);
							}
						}
						else
						{
							SimpleJSON.JSONNode jsonnode = JSON.Parse(webRequest.downloadHandler.text);
							if (jsonnode == null)
							{
								string text = "Error - Invalid JSON response: " + webRequest.downloadHandler.text;
								UnityEngine.Debug.LogError(pages[page] + ": " + text);
								if (errorCallback != null)
								{
									errorCallback(text);
								}
							}
							else if (callback != null)
							{
								callback(jsonnode);
							}
						}
						break;
					default:
						webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
						webRequest.SetRequestHeader("Content-Type", "application/json");
						webRequest.SetRequestHeader("Accept", "application/json");
						this.$current = webRequest.SendWebRequest();
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011A3 RID: 4515
			// (get) Token: 0x060076F9 RID: 30457 RVA: 0x00258784 File Offset: 0x00256B84
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011A4 RID: 4516
			// (get) Token: 0x060076FA RID: 30458 RVA: 0x0025878C File Offset: 0x00256B8C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060076FB RID: 30459 RVA: 0x00258794 File Offset: 0x00256B94
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			// Token: 0x060076FC RID: 30460 RVA: 0x002587E8 File Offset: 0x00256BE8
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x060076FD RID: 30461 RVA: 0x002587EF File Offset: 0x00256BEF
			private void <>__Finally0()
			{
				if (webRequest != null)
				{
					((IDisposable)webRequest).Dispose();
				}
			}

			// Token: 0x04006B0A RID: 27402
			internal UnityWebRequest <webRequest>__1;

			// Token: 0x04006B0B RID: 27403
			internal string uri;

			// Token: 0x04006B0C RID: 27404
			internal string postData;

			// Token: 0x04006B0D RID: 27405
			internal string[] <pages>__2;

			// Token: 0x04006B0E RID: 27406
			internal int <page>__2;

			// Token: 0x04006B0F RID: 27407
			internal HubBrowse.RequestErrorCallback errorCallback;

			// Token: 0x04006B10 RID: 27408
			internal HubBrowse.RequestSuccessCallback callback;

			// Token: 0x04006B11 RID: 27409
			internal object $current;

			// Token: 0x04006B12 RID: 27410
			internal bool $disposing;

			// Token: 0x04006B13 RID: 27411
			internal int $PC;
		}

		// Token: 0x02001021 RID: 4129
		[CompilerGenerated]
		private sealed class <TriggerResetRefesh>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060076FE RID: 30462 RVA: 0x00258807 File Offset: 0x00256C07
			[DebuggerHidden]
			public <TriggerResetRefesh>c__Iterator3()
			{
			}

			// Token: 0x060076FF RID: 30463 RVA: 0x00258810 File Offset: 0x00256C10
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (this.triggerCountdown > 0f)
				{
					this.triggerCountdown -= Time.unscaledDeltaTime;
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.triggerResetRefreshRoutine = null;
				base.ResetRefresh();
				this.$PC = -1;
				return false;
			}

			// Token: 0x170011A5 RID: 4517
			// (get) Token: 0x06007700 RID: 30464 RVA: 0x002588AB File Offset: 0x00256CAB
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011A6 RID: 4518
			// (get) Token: 0x06007701 RID: 30465 RVA: 0x002588B3 File Offset: 0x00256CB3
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007702 RID: 30466 RVA: 0x002588BB File Offset: 0x00256CBB
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007703 RID: 30467 RVA: 0x002588CB File Offset: 0x00256CCB
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006B14 RID: 27412
			internal HubBrowse $this;

			// Token: 0x04006B15 RID: 27413
			internal object $current;

			// Token: 0x04006B16 RID: 27414
			internal bool $disposing;

			// Token: 0x04006B17 RID: 27415
			internal int $PC;
		}

		// Token: 0x02001022 RID: 4130
		[CompilerGenerated]
		private sealed class <OpenDetail>c__AnonStorey6
		{
			// Token: 0x06007704 RID: 30468 RVA: 0x002588D2 File Offset: 0x00256CD2
			public <OpenDetail>c__AnonStorey6()
			{
			}

			// Token: 0x06007705 RID: 30469 RVA: 0x002588DA File Offset: 0x00256CDA
			internal void <>m__0(SimpleJSON.JSONNode jsonNode)
			{
				this.$this.GetResourceDetailCallback(jsonNode, this.hridui);
			}

			// Token: 0x06007706 RID: 30470 RVA: 0x002588EE File Offset: 0x00256CEE
			internal void <>m__1(string err)
			{
				this.$this.GetResourceDetailErrorCallback(err, this.hridui);
			}

			// Token: 0x04006B18 RID: 27416
			internal HubResourceItemDetailUI hridui;

			// Token: 0x04006B19 RID: 27417
			internal HubBrowse $this;
		}

		// Token: 0x02001023 RID: 4131
		[CompilerGenerated]
		private sealed class <GetBrowserCookies>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007707 RID: 30471 RVA: 0x00258902 File Offset: 0x00256D02
			[DebuggerHidden]
			public <GetBrowserCookies>c__Iterator4()
			{
			}

			// Token: 0x06007708 RID: 30472 RVA: 0x0025890C File Offset: 0x00256D0C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (this.hubCookies == null)
					{
						this.hubCookies = new List<string>();
					}
					break;
				case 1U:
					break;
				case 2U:
					this.hubCookies.Clear();
					enumerator = promise.Value.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Cookie cookie = enumerator.Current;
							if (cookie.domain == this.cookieHost)
							{
								this.hubCookies.Add(string.Format("{0}={1}", cookie.name, cookie.value));
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					this.GetBrowserCookiesRoutine = null;
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (this.browser.IsReady)
				{
					promise = this.browser.CookieManager.GetCookies();
					this.$current = promise.ToWaitFor(true);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
				}
				else
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
				}
				return true;
			}

			// Token: 0x170011A7 RID: 4519
			// (get) Token: 0x06007709 RID: 30473 RVA: 0x00258A9C File Offset: 0x00256E9C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011A8 RID: 4520
			// (get) Token: 0x0600770A RID: 30474 RVA: 0x00258AA4 File Offset: 0x00256EA4
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600770B RID: 30475 RVA: 0x00258AAC File Offset: 0x00256EAC
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600770C RID: 30476 RVA: 0x00258ABC File Offset: 0x00256EBC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006B1A RID: 27418
			internal IPromise<List<Cookie>> <promise>__0;

			// Token: 0x04006B1B RID: 27419
			internal List<Cookie>.Enumerator $locvar0;

			// Token: 0x04006B1C RID: 27420
			internal HubBrowse $this;

			// Token: 0x04006B1D RID: 27421
			internal object $current;

			// Token: 0x04006B1E RID: 27422
			internal bool $disposing;

			// Token: 0x04006B1F RID: 27423
			internal int $PC;
		}

		// Token: 0x02001024 RID: 4132
		[CompilerGenerated]
		private sealed class <DownloadRoutine>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600770D RID: 30477 RVA: 0x00258AC3 File Offset: 0x00256EC3
			[DebuggerHidden]
			public <DownloadRoutine>c__Iterator5()
			{
			}

			// Token: 0x0600770E RID: 30478 RVA: 0x00258ACC File Offset: 0x00256ECC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					break;
				case 1U:
					if (this.downloadQueue.Count == 0)
					{
						FileManager.Refresh();
					}
					goto IL_128;
				case 2U:
					break;
				default:
					return false;
				}
				if (this.downloadQueue.Count > 0)
				{
					this.isDownloadingJSON.val = true;
					this.downloadQueuedCountJSON.val = "Queued: " + this.downloadQueue.Count;
					request = this.downloadQueue.Dequeue();
					this.$current = base.BinaryGetRequest(request.url, request.startedCallback, request.successCallback, request.errorCallback, request.progressCallback, this.hubCookies);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.isDownloadingJSON.val = false;
				IL_128:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}

			// Token: 0x170011A9 RID: 4521
			// (get) Token: 0x0600770F RID: 30479 RVA: 0x00258C2B File Offset: 0x0025702B
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011AA RID: 4522
			// (get) Token: 0x06007710 RID: 30480 RVA: 0x00258C33 File Offset: 0x00257033
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007711 RID: 30481 RVA: 0x00258C3B File Offset: 0x0025703B
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007712 RID: 30482 RVA: 0x00258C4B File Offset: 0x0025704B
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006B20 RID: 27424
			internal HubBrowse.DownloadRequest <request>__1;

			// Token: 0x04006B21 RID: 27425
			internal HubBrowse $this;

			// Token: 0x04006B22 RID: 27426
			internal object $current;

			// Token: 0x04006B23 RID: 27427
			internal bool $disposing;

			// Token: 0x04006B24 RID: 27428
			internal int $PC;
		}
	}
}
