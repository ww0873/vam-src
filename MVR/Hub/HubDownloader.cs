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

namespace MVR.Hub
{
	// Token: 0x02000CB9 RID: 3257
	public class HubDownloader : JSONStorable
	{
		// Token: 0x060061EA RID: 25066 RVA: 0x00258C5A File Offset: 0x0025705A
		public HubDownloader()
		{
		}

		// Token: 0x060061EB RID: 25067 RVA: 0x00258C64 File Offset: 0x00257064
		private IEnumerator PostRequest(string uri, string postData, HubDownloader.RequestSuccessCallback callback, HubDownloader.RequestErrorCallback errorCallback)
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
					JSONNode jsonnode = JSON.Parse(webRequest.downloadHandler.text);
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

		// Token: 0x060061EC RID: 25068 RVA: 0x00258C98 File Offset: 0x00257098
		public void OpenPanelWithInfo(string info, bool disableCloseButton = false)
		{
			this.OpenPanel();
			if (disableCloseButton && this.closePanelAction.button != null)
			{
				this.closePanelAction.button.gameObject.SetActive(false);
			}
			if (this.packageNameString.inputField != null)
			{
				this.packageNameString.inputField.gameObject.SetActive(false);
			}
			if (this.findPackageAction.button != null)
			{
				this.findPackageAction.button.gameObject.SetActive(false);
			}
			if (this.downloadAllTrackedPackagesAction.button != null)
			{
				this.downloadAllTrackedPackagesAction.button.gameObject.SetActive(false);
			}
			if (this.clearTrackedPackagesAction.button != null)
			{
				this.clearTrackedPackagesAction.button.gameObject.SetActive(false);
			}
			if (this.infoText != null)
			{
				this.infoText.gameObject.SetActive(true);
				this.infoText.text = info;
			}
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x00258DBC File Offset: 0x002571BC
		public void OpenPanel()
		{
			if (this.panel != null)
			{
				this.panel.gameObject.SetActive(true);
			}
			if (this.closePanelAction.button != null)
			{
				this.closePanelAction.button.gameObject.SetActive(true);
			}
			if (this.packageNameString.inputField != null)
			{
				this.packageNameString.inputField.gameObject.SetActive(true);
			}
			if (this.findPackageAction.button != null)
			{
				this.findPackageAction.button.gameObject.SetActive(true);
			}
			if (this.downloadAllTrackedPackagesAction.button != null)
			{
				this.downloadAllTrackedPackagesAction.button.gameObject.SetActive(true);
			}
			if (this.clearTrackedPackagesAction.button != null)
			{
				this.clearTrackedPackagesAction.button.gameObject.SetActive(true);
			}
			if (this.infoText != null)
			{
				this.infoText.gameObject.SetActive(false);
				this.infoText.text = string.Empty;
			}
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x00258EF9 File Offset: 0x002572F9
		public void ClosePanel()
		{
			if (this.panel != null)
			{
				this.panel.gameObject.SetActive(false);
			}
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x00258F20 File Offset: 0x00257320
		public void ClearTrackedPackages()
		{
			foreach (HubResourcePackageUI hubResourcePackageUI in this.trackedPackages)
			{
				UnityEngine.Object.Destroy(hubResourcePackageUI.gameObject);
			}
			this.trackedPackages.Clear();
			this.packageNameString.val = string.Empty;
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x060061F0 RID: 25072 RVA: 0x00258F9C File Offset: 0x0025739C
		public int PendingResourceDownloads
		{
			get
			{
				return this.pendingResourceDownloads;
			}
		}

		// Token: 0x060061F1 RID: 25073 RVA: 0x00258FA4 File Offset: 0x002573A4
		protected void ResourceDownloadQueued(HubResourcePackage hrp)
		{
			this.pendingResourceDownloads++;
		}

		// Token: 0x060061F2 RID: 25074 RVA: 0x00258FB4 File Offset: 0x002573B4
		protected void ResourceDownloadComplete(HubResourcePackage hrp, bool alreadyHad)
		{
			if (!alreadyHad)
			{
				FileManager.PackageUIDAndPublisher packageUIDAndPublisher = new FileManager.PackageUIDAndPublisher();
				packageUIDAndPublisher.uid = hrp.Name;
				packageUIDAndPublisher.publisher = hrp.PublishingUser;
				this.packagesThatWereDownloaded.Add(packageUIDAndPublisher);
			}
			this.pendingResourceDownloads--;
		}

		// Token: 0x060061F3 RID: 25075 RVA: 0x00259000 File Offset: 0x00257400
		protected void ResourceDownloadError(HubResourcePackage hrp, string err)
		{
			this.errors.Add(err);
			this.pendingResourceDownloads--;
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x0025901C File Offset: 0x0025741C
		protected void ResourceDetailCallback(JSONNode jn, bool download = false)
		{
			this.pendingResourceDetailCallbacks--;
			JSONClass asObject = jn.AsObject;
			if (asObject != null)
			{
				JSONClass asObject2 = asObject["dependencies"].AsObject;
				JSONArray asArray = asObject["hubFiles"].AsArray;
				if (asArray != null)
				{
					IEnumerator enumerator = asArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							JSONClass package = (JSONClass)obj;
							HubResourcePackage hubResourcePackage = new HubResourcePackage(package, this.hubBrowse, false);
							RectTransform rectTransform = this.hubBrowse.CreateDownloadPrefabInstance();
							if (rectTransform != null)
							{
								rectTransform.SetParent(this.packagesContainer, false);
								HubResourcePackageUI component = rectTransform.GetComponent<HubResourcePackageUI>();
								if (component != null)
								{
									hubResourcePackage.RegisterUI(component);
									this.trackedPackages.Add(component);
								}
								if (asObject2 != null)
								{
									JSONArray asArray2 = asObject2[hubResourcePackage.GroupName].AsArray;
									if (asArray2 != null)
									{
										IEnumerator enumerator2 = asArray2.GetEnumerator();
										try
										{
											while (enumerator2.MoveNext())
											{
												object obj2 = enumerator2.Current;
												JSONClass package2 = (JSONClass)obj2;
												HubResourcePackage hubResourcePackage2 = new HubResourcePackage(package2, this.hubBrowse, true);
												RectTransform rectTransform2 = this.hubBrowse.CreateDownloadPrefabInstance();
												if (rectTransform2 != null)
												{
													rectTransform2.SetParent(this.packagesContainer, false);
													HubResourcePackageUI component2 = rectTransform2.GetComponent<HubResourcePackageUI>();
													if (component2 != null)
													{
														hubResourcePackage2.RegisterUI(component2);
														this.trackedPackages.Add(component2);
														HubResourcePackage hubResourcePackage3 = hubResourcePackage2;
														hubResourcePackage3.downloadQueuedCallback = (HubResourcePackage.DownloadQueuedCallback)Delegate.Combine(hubResourcePackage3.downloadQueuedCallback, new HubResourcePackage.DownloadQueuedCallback(this.ResourceDownloadQueued));
														HubResourcePackage hubResourcePackage4 = hubResourcePackage2;
														hubResourcePackage4.downloadCompleteCallback = (HubResourcePackage.DownloadCompleteCallback)Delegate.Combine(hubResourcePackage4.downloadCompleteCallback, new HubResourcePackage.DownloadCompleteCallback(this.ResourceDownloadComplete));
														HubResourcePackage hubResourcePackage5 = hubResourcePackage2;
														hubResourcePackage5.downloadErrorCallback = (HubResourcePackage.DownloadErrorCallback)Delegate.Combine(hubResourcePackage5.downloadErrorCallback, new HubResourcePackage.DownloadErrorCallback(this.ResourceDownloadError));
														if (download)
														{
															hubResourcePackage2.Download();
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
								HubResourcePackage hubResourcePackage6 = hubResourcePackage;
								hubResourcePackage6.downloadQueuedCallback = (HubResourcePackage.DownloadQueuedCallback)Delegate.Combine(hubResourcePackage6.downloadQueuedCallback, new HubResourcePackage.DownloadQueuedCallback(this.ResourceDownloadQueued));
								HubResourcePackage hubResourcePackage7 = hubResourcePackage;
								hubResourcePackage7.downloadCompleteCallback = (HubResourcePackage.DownloadCompleteCallback)Delegate.Combine(hubResourcePackage7.downloadCompleteCallback, new HubResourcePackage.DownloadCompleteCallback(this.ResourceDownloadComplete));
								HubResourcePackage hubResourcePackage8 = hubResourcePackage;
								hubResourcePackage8.downloadErrorCallback = (HubResourcePackage.DownloadErrorCallback)Delegate.Combine(hubResourcePackage8.downloadErrorCallback, new HubResourcePackage.DownloadErrorCallback(this.ResourceDownloadError));
								if (download)
								{
									hubResourcePackage.Download();
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
				}
			}
		}

		// Token: 0x060061F5 RID: 25077 RVA: 0x00259314 File Offset: 0x00257714
		protected void ResourceDetailErrorCallback(string err, string resource_id, bool suppressLogError = false)
		{
			string text = "Error " + err + " during get resource detail for " + resource_id;
			if (!suppressLogError)
			{
				SuperController.LogError(text);
			}
			this.errors.Add(text);
			this.pendingResourceDetailCallbacks--;
		}

		// Token: 0x060061F6 RID: 25078 RVA: 0x0025935C File Offset: 0x0025775C
		public void FindResource(string resourceId, bool download = false)
		{
			HubDownloader.<FindResource>c__AnonStorey1 <FindResource>c__AnonStorey = new HubDownloader.<FindResource>c__AnonStorey1();
			<FindResource>c__AnonStorey.download = download;
			<FindResource>c__AnonStorey.resourceId = resourceId;
			<FindResource>c__AnonStorey.$this = this;
			JSONClass jsonclass = new JSONClass();
			jsonclass["source"] = "VaM";
			jsonclass["action"] = "getResourceDetail";
			jsonclass["latest_image"] = "Y";
			jsonclass["resource_id"] = <FindResource>c__AnonStorey.resourceId;
			string postData = jsonclass.ToString();
			this.pendingResourceDetailCallbacks++;
			base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubDownloader.RequestSuccessCallback(<FindResource>c__AnonStorey.<>m__0), new HubDownloader.RequestErrorCallback(<FindResource>c__AnonStorey.<>m__1)));
		}

		// Token: 0x060061F7 RID: 25079 RVA: 0x00259420 File Offset: 0x00257820
		public void FindPackage(string packageName, bool download = false)
		{
			HubDownloader.<FindPackage>c__AnonStorey2 <FindPackage>c__AnonStorey = new HubDownloader.<FindPackage>c__AnonStorey2();
			<FindPackage>c__AnonStorey.download = download;
			<FindPackage>c__AnonStorey.packageName = packageName;
			<FindPackage>c__AnonStorey.$this = this;
			JSONClass jsonclass = new JSONClass();
			jsonclass["source"] = "VaM";
			jsonclass["action"] = "getResourceDetail";
			jsonclass["latest_image"] = "Y";
			jsonclass["package_name"] = <FindPackage>c__AnonStorey.packageName;
			string postData = jsonclass.ToString();
			this.pendingResourceDetailCallbacks++;
			base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubDownloader.RequestSuccessCallback(<FindPackage>c__AnonStorey.<>m__0), new HubDownloader.RequestErrorCallback(<FindPackage>c__AnonStorey.<>m__1)));
		}

		// Token: 0x060061F8 RID: 25080 RVA: 0x002594E4 File Offset: 0x002578E4
		protected void FindPackage()
		{
			if (!string.IsNullOrEmpty(this.packageNameString.val))
			{
				if (this.packageNameString.val.StartsWith("https://hub.virtamate.com"))
				{
					string text = this.packageNameString.val;
					UnityEngine.Debug.Log("Resource id is " + text);
					text = Regex.Replace(text, ".*\\.([0-9]+)/?", "$1");
					UnityEngine.Debug.Log("Got resource id " + text);
					this.FindResource(text, false);
				}
				else
				{
					this.FindPackage(this.packageNameString.val, false);
				}
			}
		}

		// Token: 0x060061F9 RID: 25081 RVA: 0x0025957C File Offset: 0x0025797C
		protected void DownloadAllTrackedPackages()
		{
			if (this.trackedPackages != null)
			{
				foreach (HubResourcePackageUI hubResourcePackageUI in this.trackedPackages)
				{
					hubResourcePackageUI.connectedItem.Download();
				}
			}
		}

		// Token: 0x060061FA RID: 25082 RVA: 0x002595E8 File Offset: 0x002579E8
		public bool DownloadPackages(HubDownloader.SuccessCallback successCallback, HubDownloader.ErrorCallback errorCallback, params string[] packagesList)
		{
			bool result = false;
			if (this._hubDownloaderEnabled)
			{
				if (!this.processingMassDownload)
				{
					this.errors.Clear();
					this.processingMassDownload = true;
					this.massDownloadSuccessCallback = successCallback;
					this.massDownloadErrorCallback = errorCallback;
					for (int i = 0; i < packagesList.Length; i++)
					{
						HubDownloader.<DownloadPackages>c__AnonStorey3 <DownloadPackages>c__AnonStorey = new HubDownloader.<DownloadPackages>c__AnonStorey3();
						<DownloadPackages>c__AnonStorey.package = packagesList[i];
						<DownloadPackages>c__AnonStorey.$this = this;
						JSONClass jsonclass = new JSONClass();
						jsonclass["source"] = "VaM";
						jsonclass["action"] = "getResourceDetail";
						jsonclass["latest_image"] = "Y";
						jsonclass["package_name"] = <DownloadPackages>c__AnonStorey.package;
						string postData = jsonclass.ToString();
						this.pendingResourceDetailCallbacks++;
						base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubDownloader.RequestSuccessCallback(<DownloadPackages>c__AnonStorey.<>m__0), new HubDownloader.RequestErrorCallback(<DownloadPackages>c__AnonStorey.<>m__1)));
						result = true;
					}
				}
				else if (errorCallback != null)
				{
					errorCallback("Cannot process DownloadPackages as mass download is already running");
				}
			}
			else
			{
				if (!this.hasPendingMassDownload)
				{
					this.hasPendingMassDownload = true;
					this.hasPendingDownloadPackages = true;
					this.pendingPackagesList = packagesList;
					this.pendingSuccessCallback = successCallback;
					this.pendingErrorCallback = errorCallback;
					return true;
				}
				if (errorCallback != null)
				{
					errorCallback("Cannot process DownloadPackages as mass download is already running");
				}
			}
			return result;
		}

		// Token: 0x060061FB RID: 25083 RVA: 0x0025975C File Offset: 0x00257B5C
		protected void FindMissingPackagesErrorCallback(string err)
		{
			string item = "Error during FindMissingPackages " + err;
			this.errors.Add(item);
			this.pendingFindMissingPackages = false;
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x00259788 File Offset: 0x00257B88
		protected void FindMissingPackagesCallback(JSONNode jsonNode)
		{
			this.pendingFindMissingPackages = false;
			if (jsonNode != null)
			{
				JSONClass asObject = jsonNode.AsObject;
				if (asObject != null)
				{
					string text = asObject["status"];
					if (text != null && text == "error")
					{
						string item = jsonNode["error"];
						this.errors.Add(item);
					}
					else
					{
						JSONClass asObject2 = jsonNode["packages"].AsObject;
						if (asObject2 != null)
						{
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
								HubResourcePackage hubResourcePackage = new HubResourcePackage(jsonclass, this.hubBrowse, true);
								RectTransform rectTransform = this.hubBrowse.CreateDownloadPrefabInstance();
								if (rectTransform != null)
								{
									rectTransform.SetParent(this.packagesContainer, false);
									HubResourcePackageUI component = rectTransform.GetComponent<HubResourcePackageUI>();
									if (component != null)
									{
										this.trackedPackages.Add(component);
										hubResourcePackage.RegisterUI(component);
										HubResourcePackage hubResourcePackage2 = hubResourcePackage;
										hubResourcePackage2.downloadQueuedCallback = (HubResourcePackage.DownloadQueuedCallback)Delegate.Combine(hubResourcePackage2.downloadQueuedCallback, new HubResourcePackage.DownloadQueuedCallback(this.ResourceDownloadQueued));
										HubResourcePackage hubResourcePackage3 = hubResourcePackage;
										hubResourcePackage3.downloadCompleteCallback = (HubResourcePackage.DownloadCompleteCallback)Delegate.Combine(hubResourcePackage3.downloadCompleteCallback, new HubResourcePackage.DownloadCompleteCallback(this.ResourceDownloadComplete));
										HubResourcePackage hubResourcePackage4 = hubResourcePackage;
										hubResourcePackage4.downloadErrorCallback = (HubResourcePackage.DownloadErrorCallback)Delegate.Combine(hubResourcePackage4.downloadErrorCallback, new HubResourcePackage.DownloadErrorCallback(this.ResourceDownloadError));
										hubResourcePackage.Download();
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060061FD RID: 25085 RVA: 0x00259AE4 File Offset: 0x00257EE4
		public bool DownloadAllMissingPackages(HubDownloader.SuccessCallback successCallback, HubDownloader.ErrorCallback errorCallback)
		{
			if (this.packageManager != null)
			{
				if (this._hubDownloaderEnabled)
				{
					List<string> missingPackageNames = this.packageManager.MissingPackageNames;
					if (missingPackageNames.Count > 0)
					{
						if (!this.processingMassDownload)
						{
							this.errors.Clear();
							this.processingMassDownload = true;
							this.massDownloadSuccessCallback = successCallback;
							this.massDownloadErrorCallback = errorCallback;
							JSONClass jsonclass = new JSONClass();
							jsonclass["source"] = "VaM";
							jsonclass["action"] = "findPackages";
							this.checkMissingPackageNames = missingPackageNames;
							jsonclass["packages"] = string.Join(",", missingPackageNames.ToArray());
							string postData = jsonclass.ToString();
							this.pendingFindMissingPackages = true;
							base.StartCoroutine(this.PostRequest(this.apiUrl, postData, new HubDownloader.RequestSuccessCallback(this.FindMissingPackagesCallback), new HubDownloader.RequestErrorCallback(this.FindMissingPackagesErrorCallback)));
							return true;
						}
						if (errorCallback != null)
						{
							errorCallback("Cannot process DownloadAllMissingPackages as mass download is already running");
						}
					}
					else if (successCallback != null)
					{
						successCallback();
					}
				}
				else
				{
					if (!this.hasPendingMassDownload)
					{
						this.hasPendingMassDownload = true;
						this.hasPendingDownloadAllMissingPackages = true;
						this.pendingSuccessCallback = successCallback;
						this.pendingErrorCallback = errorCallback;
						return true;
					}
					if (errorCallback != null)
					{
						errorCallback("Cannot process DownloadAllMissingPackages as mass download is already running");
					}
				}
			}
			return false;
		}

		// Token: 0x060061FE RID: 25086 RVA: 0x00259C48 File Offset: 0x00258048
		protected void OnPackageRefresh()
		{
			foreach (HubResourcePackageUI hubResourcePackageUI in this.trackedPackages)
			{
				if (hubResourcePackageUI != null)
				{
					hubResourcePackageUI.connectedItem.Refresh();
				}
			}
			FileManager.SyncAutoAlwaysAllowPlugins(this.packagesThatWereDownloaded);
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x00259CC0 File Offset: 0x002580C0
		protected void SyncHubDownloaderEnabled(bool b)
		{
			this._hubDownloaderEnabled = b;
			if (this._hubDownloaderEnabled && this.hasPendingMassDownload)
			{
				this.hasPendingMassDownload = false;
				if (this.hasPendingDownloadPackages)
				{
					this.hasPendingDownloadPackages = false;
					this.DownloadPackages(this.pendingSuccessCallback, this.pendingErrorCallback, this.pendingPackagesList);
					this.pendingSuccessCallback = null;
					this.pendingErrorCallback = null;
				}
				else if (this.hasPendingDownloadAllMissingPackages)
				{
					this.hasPendingDownloadAllMissingPackages = false;
					this.DownloadAllMissingPackages(this.pendingSuccessCallback, this.pendingErrorCallback);
					this.pendingSuccessCallback = null;
					this.pendingErrorCallback = null;
				}
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06006200 RID: 25088 RVA: 0x00259D62 File Offset: 0x00258162
		// (set) Token: 0x06006201 RID: 25089 RVA: 0x00259D6A File Offset: 0x0025816A
		public bool HubDownloaderEnabled
		{
			get
			{
				return this._hubDownloaderEnabled;
			}
			set
			{
				if (this.hubDownloaderEnabledJSON != null)
				{
					this.hubDownloaderEnabledJSON.val = value;
				}
				else
				{
					this._hubDownloaderEnabled = value;
				}
			}
		}

		// Token: 0x06006202 RID: 25090 RVA: 0x00259D8F File Offset: 0x0025818F
		protected void EnableHubDownloader()
		{
			UserPreferences.singleton.enableHubDownloader = true;
		}

		// Token: 0x06006203 RID: 25091 RVA: 0x00259D9C File Offset: 0x0025819C
		protected void RejectHubDownloader()
		{
			if (this.hasPendingMassDownload)
			{
				if (this.pendingErrorCallback != null)
				{
					this.pendingErrorCallback("User rejected downloader enable");
				}
				this.hasPendingMassDownload = false;
				this.hasPendingDownloadPackages = false;
				this.hasPendingDownloadAllMissingPackages = false;
				this.pendingSuccessCallback = null;
				this.pendingErrorCallback = null;
			}
			this.ClosePanel();
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x00259DF8 File Offset: 0x002581F8
		protected void Init()
		{
			HubDownloader.singleton = this;
			this.errors = new List<string>();
			this.packagesThatWereDownloaded = new HashSet<FileManager.PackageUIDAndPublisher>();
			this.trackedPackages = new List<HubResourcePackageUI>();
			FileManager.RegisterRefreshHandler(new OnRefresh(this.OnPackageRefresh));
			this.openPanelAction = new JSONStorableAction("OpenPanel", new JSONStorableAction.ActionCallback(this.OpenPanel));
			base.RegisterAction(this.openPanelAction);
			this.closePanelAction = new JSONStorableAction("ClosePanel", new JSONStorableAction.ActionCallback(this.ClosePanel));
			base.RegisterAction(this.closePanelAction);
			this.clearTrackedPackagesAction = new JSONStorableAction("ClearTrackedPackages", new JSONStorableAction.ActionCallback(this.ClearTrackedPackages));
			base.RegisterAction(this.clearTrackedPackagesAction);
			this.packageNameString = new JSONStorableString("packageName", string.Empty);
			this.packageNameString.isStorable = false;
			this.packageNameString.isRestorable = false;
			base.RegisterString(this.packageNameString);
			this.findPackageAction = new JSONStorableAction("FindPackage", new JSONStorableAction.ActionCallback(this.FindPackage));
			base.RegisterAction(this.findPackageAction);
			this.downloadAllTrackedPackagesAction = new JSONStorableAction("DownloadAllTrackedPackages", new JSONStorableAction.ActionCallback(this.DownloadAllTrackedPackages));
			base.RegisterAction(this.downloadAllTrackedPackagesAction);
			this.hubDownloaderEnabledJSON = new JSONStorableBool("hubDownloaderEnabled", this._hubDownloaderEnabled, new JSONStorableBool.SetBoolCallback(this.SyncHubDownloaderEnabled));
			this.enableHubDownloaderAction = new JSONStorableAction("EnableHubDownloader", new JSONStorableAction.ActionCallback(this.EnableHubDownloader));
			this.rejectHubDownloaderAction = new JSONStorableAction("RejectHubDownloader", new JSONStorableAction.ActionCallback(this.RejectHubDownloader));
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x00259F98 File Offset: 0x00258398
		protected override void InitUI(Transform t, bool isAlt)
		{
			if (t != null)
			{
				HubDownloaderUI componentInChildren = t.GetComponentInChildren<HubDownloaderUI>();
				if (componentInChildren != null)
				{
					if (!isAlt)
					{
						this.panel = componentInChildren.panel;
						this.packagesContainer = componentInChildren.packagesContainer;
						this.infoText = componentInChildren.infoText;
						this.downloadInfoText = componentInChildren.downloadInfoText;
					}
					this.openPanelAction.RegisterButton(componentInChildren.openPanelButton, isAlt);
					this.closePanelAction.RegisterButton(componentInChildren.closePanelButton, isAlt);
					this.clearTrackedPackagesAction.RegisterButton(componentInChildren.clearTrackedPackagesButton, isAlt);
					this.packageNameString.RegisterInputField(componentInChildren.packageNameInputField, isAlt);
					this.findPackageAction.RegisterButton(componentInChildren.findPackageButton, isAlt);
					this.downloadAllTrackedPackagesAction.RegisterButton(componentInChildren.downloadAllTrackedPackagesButton, isAlt);
					this.hubDownloaderEnabledJSON.RegisterNegativeIndicator(componentInChildren.disabledIndicator, isAlt);
					this.enableHubDownloaderAction.RegisterButton(componentInChildren.enableHubDownloaderButton, isAlt);
					this.rejectHubDownloaderAction.RegisterButton(componentInChildren.rejectHubDownloaderButton, isAlt);
				}
			}
		}

		// Token: 0x06006206 RID: 25094 RVA: 0x0025A09C File Offset: 0x0025849C
		protected override void Awake()
		{
			if (!this.awakecalled)
			{
				base.Awake();
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x0025A0C4 File Offset: 0x002584C4
		protected void Update()
		{
			if (this.processingMassDownload && !this.pendingFindMissingPackages && this.pendingResourceDetailCallbacks == 0 && this.pendingResourceDownloads == 0)
			{
				UnityEngine.Debug.Log("Mass download complete");
				this.processingMassDownload = false;
				if (this.errors.Count > 0)
				{
					UnityEngine.Debug.Log("Mass download had errors");
					if (this.massDownloadErrorCallback != null)
					{
						this.massDownloadErrorCallback(string.Join("\n", this.errors.ToArray()));
					}
				}
				else if (this.massDownloadSuccessCallback != null)
				{
					UnityEngine.Debug.Log("Mass download success");
					this.massDownloadSuccessCallback();
				}
			}
			if (this.downloadInfoText != null)
			{
				if (this.pendingResourceDownloads > 0)
				{
					this.downloadInfoText.text = "Queued downloads count: " + this.pendingResourceDownloads;
				}
				else
				{
					this.downloadInfoText.text = "No queued downloads";
				}
			}
		}

		// Token: 0x0400527B RID: 21115
		public static HubDownloader singleton;

		// Token: 0x0400527C RID: 21116
		protected RectTransform panel;

		// Token: 0x0400527D RID: 21117
		protected RectTransform packagesContainer;

		// Token: 0x0400527E RID: 21118
		protected Text infoText;

		// Token: 0x0400527F RID: 21119
		protected Text downloadInfoText;

		// Token: 0x04005280 RID: 21120
		protected Button enableButton;

		// Token: 0x04005281 RID: 21121
		protected RectTransform disabledIndicator;

		// Token: 0x04005282 RID: 21122
		protected JSONStorableAction openPanelAction;

		// Token: 0x04005283 RID: 21123
		protected JSONStorableAction closePanelAction;

		// Token: 0x04005284 RID: 21124
		protected List<HubResourcePackageUI> trackedPackages;

		// Token: 0x04005285 RID: 21125
		protected JSONStorableAction clearTrackedPackagesAction;

		// Token: 0x04005286 RID: 21126
		protected int pendingResourceDetailCallbacks;

		// Token: 0x04005287 RID: 21127
		protected int pendingResourceDownloads;

		// Token: 0x04005288 RID: 21128
		protected List<string> errors;

		// Token: 0x04005289 RID: 21129
		protected HashSet<FileManager.PackageUIDAndPublisher> packagesThatWereDownloaded;

		// Token: 0x0400528A RID: 21130
		protected JSONStorableString packageNameString;

		// Token: 0x0400528B RID: 21131
		protected JSONStorableAction findPackageAction;

		// Token: 0x0400528C RID: 21132
		protected JSONStorableAction downloadAllTrackedPackagesAction;

		// Token: 0x0400528D RID: 21133
		protected HubDownloader.SuccessCallback massDownloadSuccessCallback;

		// Token: 0x0400528E RID: 21134
		protected HubDownloader.ErrorCallback massDownloadErrorCallback;

		// Token: 0x0400528F RID: 21135
		protected bool processingMassDownload;

		// Token: 0x04005290 RID: 21136
		protected bool hasPendingMassDownload;

		// Token: 0x04005291 RID: 21137
		protected bool hasPendingDownloadPackages;

		// Token: 0x04005292 RID: 21138
		protected bool hasPendingDownloadAllMissingPackages;

		// Token: 0x04005293 RID: 21139
		protected string[] pendingPackagesList;

		// Token: 0x04005294 RID: 21140
		protected HubDownloader.SuccessCallback pendingSuccessCallback;

		// Token: 0x04005295 RID: 21141
		protected HubDownloader.ErrorCallback pendingErrorCallback;

		// Token: 0x04005296 RID: 21142
		protected bool pendingFindMissingPackages;

		// Token: 0x04005297 RID: 21143
		protected List<string> checkMissingPackageNames;

		// Token: 0x04005298 RID: 21144
		public PackageBuilder packageManager;

		// Token: 0x04005299 RID: 21145
		public HubBrowse hubBrowse;

		// Token: 0x0400529A RID: 21146
		[SerializeField]
		protected string apiUrl;

		// Token: 0x0400529B RID: 21147
		protected bool _hubDownloaderEnabled;

		// Token: 0x0400529C RID: 21148
		protected JSONStorableBool hubDownloaderEnabledJSON;

		// Token: 0x0400529D RID: 21149
		protected JSONStorableAction enableHubDownloaderAction;

		// Token: 0x0400529E RID: 21150
		protected JSONStorableAction rejectHubDownloaderAction;

		// Token: 0x02000CBA RID: 3258
		// (Invoke) Token: 0x06006209 RID: 25097
		protected delegate void RequestSuccessCallback(JSONNode jsonNode);

		// Token: 0x02000CBB RID: 3259
		// (Invoke) Token: 0x0600620D RID: 25101
		protected delegate void RequestErrorCallback(string err);

		// Token: 0x02000CBC RID: 3260
		// (Invoke) Token: 0x06006211 RID: 25105
		public delegate void ErrorCallback(string err);

		// Token: 0x02000CBD RID: 3261
		// (Invoke) Token: 0x06006215 RID: 25109
		public delegate void SuccessCallback();

		// Token: 0x02001025 RID: 4133
		[CompilerGenerated]
		private sealed class <PostRequest>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007713 RID: 30483 RVA: 0x0025A1CB File Offset: 0x002585CB
			[DebuggerHidden]
			public <PostRequest>c__Iterator0()
			{
			}

			// Token: 0x06007714 RID: 30484 RVA: 0x0025A1D4 File Offset: 0x002585D4
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
							JSONNode jsonnode = JSON.Parse(webRequest.downloadHandler.text);
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

			// Token: 0x170011AB RID: 4523
			// (get) Token: 0x06007715 RID: 30485 RVA: 0x0025A3E8 File Offset: 0x002587E8
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170011AC RID: 4524
			// (get) Token: 0x06007716 RID: 30486 RVA: 0x0025A3F0 File Offset: 0x002587F0
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007717 RID: 30487 RVA: 0x0025A3F8 File Offset: 0x002587F8
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

			// Token: 0x06007718 RID: 30488 RVA: 0x0025A44C File Offset: 0x0025884C
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007719 RID: 30489 RVA: 0x0025A453 File Offset: 0x00258853
			private void <>__Finally0()
			{
				if (webRequest != null)
				{
					((IDisposable)webRequest).Dispose();
				}
			}

			// Token: 0x04006B25 RID: 27429
			internal UnityWebRequest <webRequest>__1;

			// Token: 0x04006B26 RID: 27430
			internal string uri;

			// Token: 0x04006B27 RID: 27431
			internal string postData;

			// Token: 0x04006B28 RID: 27432
			internal string[] <pages>__2;

			// Token: 0x04006B29 RID: 27433
			internal int <page>__2;

			// Token: 0x04006B2A RID: 27434
			internal HubDownloader.RequestErrorCallback errorCallback;

			// Token: 0x04006B2B RID: 27435
			internal HubDownloader.RequestSuccessCallback callback;

			// Token: 0x04006B2C RID: 27436
			internal object $current;

			// Token: 0x04006B2D RID: 27437
			internal bool $disposing;

			// Token: 0x04006B2E RID: 27438
			internal int $PC;
		}

		// Token: 0x02001026 RID: 4134
		[CompilerGenerated]
		private sealed class <FindResource>c__AnonStorey1
		{
			// Token: 0x0600771A RID: 30490 RVA: 0x0025A46B File Offset: 0x0025886B
			public <FindResource>c__AnonStorey1()
			{
			}

			// Token: 0x0600771B RID: 30491 RVA: 0x0025A473 File Offset: 0x00258873
			internal void <>m__0(JSONNode jsonNode)
			{
				this.$this.ResourceDetailCallback(jsonNode, this.download);
			}

			// Token: 0x0600771C RID: 30492 RVA: 0x0025A487 File Offset: 0x00258887
			internal void <>m__1(string err)
			{
				this.$this.ResourceDetailErrorCallback(err, this.resourceId, false);
			}

			// Token: 0x04006B2F RID: 27439
			internal bool download;

			// Token: 0x04006B30 RID: 27440
			internal string resourceId;

			// Token: 0x04006B31 RID: 27441
			internal HubDownloader $this;
		}

		// Token: 0x02001027 RID: 4135
		[CompilerGenerated]
		private sealed class <FindPackage>c__AnonStorey2
		{
			// Token: 0x0600771D RID: 30493 RVA: 0x0025A49C File Offset: 0x0025889C
			public <FindPackage>c__AnonStorey2()
			{
			}

			// Token: 0x0600771E RID: 30494 RVA: 0x0025A4A4 File Offset: 0x002588A4
			internal void <>m__0(JSONNode jsonNode)
			{
				this.$this.ResourceDetailCallback(jsonNode, this.download);
			}

			// Token: 0x0600771F RID: 30495 RVA: 0x0025A4B8 File Offset: 0x002588B8
			internal void <>m__1(string err)
			{
				this.$this.ResourceDetailErrorCallback(err, this.packageName, false);
			}

			// Token: 0x04006B32 RID: 27442
			internal bool download;

			// Token: 0x04006B33 RID: 27443
			internal string packageName;

			// Token: 0x04006B34 RID: 27444
			internal HubDownloader $this;
		}

		// Token: 0x02001028 RID: 4136
		[CompilerGenerated]
		private sealed class <DownloadPackages>c__AnonStorey3
		{
			// Token: 0x06007720 RID: 30496 RVA: 0x0025A4CD File Offset: 0x002588CD
			public <DownloadPackages>c__AnonStorey3()
			{
			}

			// Token: 0x06007721 RID: 30497 RVA: 0x0025A4D5 File Offset: 0x002588D5
			internal void <>m__0(JSONNode jsonNode)
			{
				this.$this.ResourceDetailCallback(jsonNode, true);
			}

			// Token: 0x06007722 RID: 30498 RVA: 0x0025A4E4 File Offset: 0x002588E4
			internal void <>m__1(string err)
			{
				this.$this.ResourceDetailErrorCallback(err, this.package, true);
			}

			// Token: 0x04006B35 RID: 27445
			internal string package;

			// Token: 0x04006B36 RID: 27446
			internal HubDownloader $this;
		}
	}
}
