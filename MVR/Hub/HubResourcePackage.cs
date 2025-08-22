using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using SimpleJSON;

namespace MVR.Hub
{
	// Token: 0x02000CC4 RID: 3268
	public class HubResourcePackage
	{
		// Token: 0x06006248 RID: 25160 RVA: 0x0025BFCC File Offset: 0x0025A3CC
		public HubResourcePackage(JSONClass package, HubBrowse hubBrowse, bool isDependency)
		{
			this.browser = hubBrowse;
			this.package_id = package["package_id"];
			this.resource_id = package["resource_id"];
			string text = package["filename"];
			text = Regex.Replace(text, ".var$", string.Empty);
			this.GroupName = Regex.Replace(text, "(.*)\\..*", "$1");
			this.Creator = Regex.Replace(this.GroupName, "(.*)\\..*", "$1");
			this.PublishingUser = package["username"];
			if (this.PublishingUser == null)
			{
				this.PublishingUser = "Unknown";
			}
			this.Version = package["version"];
			if (this.Version == null)
			{
				this.Version = Regex.Replace(text, ".*\\.([0-9]+)$", "$1");
			}
			this.resolvedVarName = this.GroupName + "." + this.Version + ".var";
			string text2 = package["latest_version"];
			if (text2 == null)
			{
				text2 = this.Version;
			}
			if (text2 != null)
			{
				int latestVersion;
				if (int.TryParse(text2, out latestVersion))
				{
					this.LatestVersion = latestVersion;
				}
				else
				{
					this.LatestVersion = -1;
				}
			}
			string startingValue = package["licenseType"];
			string s = package["file_size"];
			this.SyncFileSize(s);
			string startingValue2 = HubResourcePackage.SizeSuffix(this.FileSize, 1);
			this.downloadUrl = package["downloadUrl"];
			if (this.downloadUrl == null)
			{
				this.downloadUrl = package["urlHosted"];
			}
			this.latestUrl = package["latestUrl"];
			if (this.latestUrl == null)
			{
				this.latestUrl = this.downloadUrl;
			}
			bool startingValue3 = this.downloadUrl == "null";
			this.promotionalUrl = package["promotional_link"];
			this.goToResourceAction = new JSONStorableAction("GoToResource", new JSONStorableAction.ActionCallback(this.GoToResource));
			this.isDependencyJSON = new JSONStorableBool("isDependency", isDependency);
			this.nameJSON = new JSONStorableString("name", text);
			this.licenseTypeJSON = new JSONStorableString("licenseType", startingValue);
			this.fileSizeJSON = new JSONStorableString("fileSize", startingValue2);
			this.alreadyHaveJSON = new JSONStorableBool("alreadyHave", false);
			this.alreadyHaveSceneJSON = new JSONStorableBool("alreadyHaveScene", false);
			this.updateAvailableJSON = new JSONStorableBool("updateAvailable", false);
			this.updateMsgJSON = new JSONStorableString("updateMsg", "Update");
			this.updateAction = new JSONStorableAction("Update", new JSONStorableAction.ActionCallback(this.Update));
			this.notOnHubJSON = new JSONStorableBool("notOnHub", startingValue3);
			this.downloadAction = new JSONStorableAction("Download", new JSONStorableAction.ActionCallback(this.Download));
			this.isDownloadQueuedJSON = new JSONStorableBool("isDownloadQueued", false);
			this.isDownloadingJSON = new JSONStorableBool("isDownloading", false);
			this.isDownloadedJSON = new JSONStorableBool("isDownloaded", false);
			this.downloadProgressJSON = new JSONStorableFloat("downloadProgress", 0f, 0f, 1f, true, false);
			this.openInPackageManagerAction = new JSONStorableAction("OpenInPackageManager", new JSONStorableAction.ActionCallback(this.OpenInPackageManager));
			this.openSceneAction = new JSONStorableAction("OpenScene", new JSONStorableAction.ActionCallback(this.OpenScene));
			this.Refresh();
		}

		// Token: 0x06006249 RID: 25161 RVA: 0x0025C378 File Offset: 0x0025A778
		private static string SizeSuffix(int value, int decimalPlaces = 1)
		{
			if (value < 0)
			{
				return "-" + HubResourcePackage.SizeSuffix(-value, 1);
			}
			if (value == 0)
			{
				return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
			}
			int num = (int)Math.Log((double)value, 1024.0);
			decimal num2 = value / (1L << num * 10);
			if (Math.Round(num2, decimalPlaces) >= 1000m)
			{
				num++;
				num2 /= 1024m;
			}
			return string.Format("{0:n" + decimalPlaces + "} {1}", num2, HubResourcePackage.SizeSuffixes[num]);
		}

		// Token: 0x0600624A RID: 25162 RVA: 0x0025C44A File Offset: 0x0025A84A
		protected void GoToResource()
		{
			if (this.resource_id != "null")
			{
				this.browser.OpenDetail(this.resource_id, false);
			}
		}

		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x0600624B RID: 25163 RVA: 0x0025C473 File Offset: 0x0025A873
		// (set) Token: 0x0600624C RID: 25164 RVA: 0x0025C47B File Offset: 0x0025A87B
		public string GroupName
		{
			[CompilerGenerated]
			get
			{
				return this.<GroupName>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<GroupName>k__BackingField = value;
			}
		}

		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x0600624D RID: 25165 RVA: 0x0025C484 File Offset: 0x0025A884
		// (set) Token: 0x0600624E RID: 25166 RVA: 0x0025C48C File Offset: 0x0025A88C
		public string Creator
		{
			[CompilerGenerated]
			get
			{
				return this.<Creator>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Creator>k__BackingField = value;
			}
		}

		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x0600624F RID: 25167 RVA: 0x0025C495 File Offset: 0x0025A895
		// (set) Token: 0x06006250 RID: 25168 RVA: 0x0025C49D File Offset: 0x0025A89D
		public string Version
		{
			[CompilerGenerated]
			get
			{
				return this.<Version>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Version>k__BackingField = value;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06006251 RID: 25169 RVA: 0x0025C4A6 File Offset: 0x0025A8A6
		// (set) Token: 0x06006252 RID: 25170 RVA: 0x0025C4AE File Offset: 0x0025A8AE
		public int LatestVersion
		{
			[CompilerGenerated]
			get
			{
				return this.<LatestVersion>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<LatestVersion>k__BackingField = value;
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06006253 RID: 25171 RVA: 0x0025C4B7 File Offset: 0x0025A8B7
		// (set) Token: 0x06006254 RID: 25172 RVA: 0x0025C4BF File Offset: 0x0025A8BF
		public string PublishingUser
		{
			[CompilerGenerated]
			get
			{
				return this.<PublishingUser>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<PublishingUser>k__BackingField = value;
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06006255 RID: 25173 RVA: 0x0025C4C8 File Offset: 0x0025A8C8
		public string Name
		{
			get
			{
				return this.nameJSON.val;
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06006256 RID: 25174 RVA: 0x0025C4D5 File Offset: 0x0025A8D5
		public string LicenseType
		{
			get
			{
				return this.licenseTypeJSON.val;
			}
		}

		// Token: 0x06006257 RID: 25175 RVA: 0x0025C4E4 File Offset: 0x0025A8E4
		protected void SyncFileSize(string s)
		{
			int fileSize;
			if (int.TryParse(s, out fileSize))
			{
				this.FileSize = fileSize;
			}
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06006258 RID: 25176 RVA: 0x0025C505 File Offset: 0x0025A905
		// (set) Token: 0x06006259 RID: 25177 RVA: 0x0025C50D File Offset: 0x0025A90D
		public int FileSize
		{
			[CompilerGenerated]
			get
			{
				return this.<FileSize>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<FileSize>k__BackingField = value;
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x0600625A RID: 25178 RVA: 0x0025C516 File Offset: 0x0025A916
		public bool NeedsDownload
		{
			get
			{
				return !this.alreadyHaveJSON.val || this.updateAvailableJSON.val;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x0600625B RID: 25179 RVA: 0x0025C536 File Offset: 0x0025A936
		public bool IsDownloading
		{
			get
			{
				return this.isDownloadingJSON.val;
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x0600625C RID: 25180 RVA: 0x0025C543 File Offset: 0x0025A943
		public bool IsDownloadQueued
		{
			get
			{
				return this.isDownloadQueuedJSON.val;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x0600625D RID: 25181 RVA: 0x0025C550 File Offset: 0x0025A950
		public bool HadDownloadError
		{
			get
			{
				return this.hadDownloadError;
			}
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x0025C558 File Offset: 0x0025A958
		protected void DownloadStarted()
		{
			this.isDownloadQueuedJSON.val = false;
			this.isDownloadingJSON.val = true;
			if (this.downloadStartCallback != null)
			{
				this.downloadStartCallback(this);
			}
		}

		// Token: 0x0600625F RID: 25183 RVA: 0x0025C589 File Offset: 0x0025A989
		protected void DownloadProgress(float f)
		{
			this.downloadProgressJSON.val = f;
			if (this.downloadProgressCallback != null)
			{
				this.downloadProgressCallback(this, f);
			}
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x0025C5B0 File Offset: 0x0025A9B0
		protected void DownloadComplete(byte[] data, Dictionary<string, string> responseHeaders)
		{
			this.isDownloadingJSON.val = false;
			this.isDownloadedJSON.val = true;
			string input;
			string str;
			if (responseHeaders.TryGetValue("Content-Disposition", out input))
			{
				input = Regex.Replace(input, ";$", string.Empty);
				str = Regex.Replace(input, ".*filename=\"?([^\"]+)\"?.*", "$1");
			}
			else
			{
				str = this.resolvedVarName;
			}
			try
			{
				FileManager.WriteAllBytes("AddonPackages/" + str, data);
				if (this.downloadCompleteCallback != null)
				{
					this.downloadCompleteCallback(this, false);
				}
			}
			catch (Exception ex)
			{
				SuperController.LogError("Error while trying to save file AddonPackages/" + str + " after download");
				this.isDownloadQueuedJSON.val = false;
				this.isDownloadingJSON.val = false;
				if (this.downloadErrorCallback != null)
				{
					this.downloadErrorCallback(this, ex.Message);
				}
			}
		}

		// Token: 0x06006261 RID: 25185 RVA: 0x0025C6A4 File Offset: 0x0025AAA4
		protected void DownloadError(string err)
		{
			this.isDownloadQueuedJSON.val = false;
			this.isDownloadingJSON.val = false;
			this.hadDownloadError = true;
			SuperController.LogError("Error while downloading " + this.Name + ": " + err);
			if (this.downloadErrorCallback != null)
			{
				this.downloadErrorCallback(this, err);
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06006262 RID: 25186 RVA: 0x0025C704 File Offset: 0x0025AB04
		public bool CanBeDownloaded
		{
			get
			{
				return this.browser != null && this.downloadUrl != null && this.downloadUrl != string.Empty && this.downloadUrl != "null" && !this.isDownloadQueuedJSON.val;
			}
		}

		// Token: 0x06006263 RID: 25187 RVA: 0x0025C768 File Offset: 0x0025AB68
		public void Download()
		{
			if (this.CanBeDownloaded)
			{
				this.hadDownloadError = false;
				if (this.downloadQueuedCallback != null)
				{
					this.downloadQueuedCallback(this);
				}
				if (!this.alreadyHaveJSON.val)
				{
					this.isDownloadQueuedJSON.val = true;
					this.browser.QueueDownload(this.downloadUrl, this.promotionalUrl, new HubBrowse.BinaryRequestStartedCallback(this.DownloadStarted), new HubBrowse.RequestProgressCallback(this.DownloadProgress), new HubBrowse.BinaryRequestSuccessCallback(this.DownloadComplete), new HubBrowse.RequestErrorCallback(this.DownloadError));
				}
				else if (this.updateAvailableJSON.val && this.latestUrl != null && this.latestUrl != string.Empty && this.latestUrl != "null")
				{
					this.isDownloadQueuedJSON.val = true;
					this.browser.QueueDownload(this.latestUrl, this.promotionalUrl, new HubBrowse.BinaryRequestStartedCallback(this.DownloadStarted), new HubBrowse.RequestProgressCallback(this.DownloadProgress), new HubBrowse.BinaryRequestSuccessCallback(this.DownloadComplete), new HubBrowse.RequestErrorCallback(this.DownloadError));
				}
				else if (this.downloadCompleteCallback != null)
				{
					this.downloadCompleteCallback(this, true);
				}
			}
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x0025C8BC File Offset: 0x0025ACBC
		public void Update()
		{
			if (this.browser != null && this.latestUrl != null && this.latestUrl != string.Empty && !this.isDownloadQueuedJSON.val && this.updateAvailableJSON.val)
			{
				this.isDownloadQueuedJSON.val = true;
				this.browser.QueueDownload(this.latestUrl, this.promotionalUrl, new HubBrowse.BinaryRequestStartedCallback(this.DownloadStarted), new HubBrowse.RequestProgressCallback(this.DownloadProgress), new HubBrowse.BinaryRequestSuccessCallback(this.DownloadComplete), new HubBrowse.RequestErrorCallback(this.DownloadError));
			}
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x0025C970 File Offset: 0x0025AD70
		public void OpenInPackageManager()
		{
			VarPackage package = FileManager.GetPackage(this.nameJSON.val);
			if (package != null)
			{
				SuperController.singleton.OpenPackageInManager(this.nameJSON.val);
			}
		}

		// Token: 0x06006266 RID: 25190 RVA: 0x0025C9A9 File Offset: 0x0025ADA9
		protected void OpenScene()
		{
			if (this.alreadyHaveScenePath != null)
			{
				SuperController.singleton.Load(this.alreadyHaveScenePath);
			}
		}

		// Token: 0x06006267 RID: 25191 RVA: 0x0025C9C8 File Offset: 0x0025ADC8
		public void Refresh()
		{
			this.isDownloadedJSON.val = false;
			VarPackage package;
			if (this.isDependencyJSON.val)
			{
				package = FileManager.GetPackage(this.nameJSON.val);
			}
			else
			{
				string str = FileManager.PackageIDToPackageGroupID(this.nameJSON.val);
				string packageUidOrPath = str + ".latest";
				package = FileManager.GetPackage(packageUidOrPath);
			}
			if (package != null)
			{
				this.alreadyHaveJSON.val = true;
				if ((this.Version == "latest" || !this.isDependencyJSON.val) && this.LatestVersion != -1)
				{
					if (package.Version < this.LatestVersion)
					{
						this.updateAvailableJSON.val = true;
						this.updateMsgJSON.val = string.Concat(new object[]
						{
							"Update ",
							package.Version,
							" -> ",
							this.LatestVersion
						});
					}
					else if (package.Version > this.LatestVersion)
					{
						this.updateAvailableJSON.val = false;
						if (FileManager.GetPackage(this.nameJSON.val) == null)
						{
							this.alreadyHaveJSON.val = false;
						}
					}
					else
					{
						this.updateAvailableJSON.val = false;
					}
				}
				else
				{
					this.updateAvailableJSON.val = false;
				}
				if (this.alreadyHaveJSON.val)
				{
					List<FileEntry> list = new List<FileEntry>();
					package.FindFiles("Saves/scene", "*.json", list);
					if (list.Count > 0)
					{
						FileEntry fileEntry = list[0];
						this.alreadyHaveScenePath = fileEntry.Uid;
						this.alreadyHaveSceneJSON.val = true;
					}
					else
					{
						this.alreadyHaveScenePath = null;
						this.alreadyHaveSceneJSON.val = false;
					}
				}
				else
				{
					this.alreadyHaveScenePath = null;
					this.alreadyHaveSceneJSON.val = false;
				}
			}
			else
			{
				this.alreadyHaveJSON.val = false;
				this.alreadyHaveScenePath = null;
				this.alreadyHaveSceneJSON.val = false;
			}
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x0025CBE4 File Offset: 0x0025AFE4
		public void RegisterUI(HubResourcePackageUI ui)
		{
			if (ui != null)
			{
				ui.connectedItem = this;
				this.goToResourceAction.button = ui.resourceButton;
				if (ui.resourceButton != null)
				{
					ui.resourceButton.interactable = (!this.notOnHubJSON.val && this.isDependencyJSON.val);
				}
				this.isDependencyJSON.indicator = ui.isDependencyIndicator;
				this.nameJSON.text = ui.nameText;
				this.licenseTypeJSON.text = ui.licenseTypeText;
				this.fileSizeJSON.text = ui.fileSizeText;
				this.notOnHubJSON.indicator = ui.notOnHubIndicator;
				this.alreadyHaveJSON.indicator = ui.alreadyHaveIndicator;
				this.alreadyHaveSceneJSON.indicator = ui.alreadyHaveSceneIndicator;
				this.updateAvailableJSON.indicator = ui.updateAvailableIndicator;
				this.updateMsgJSON.text = ui.updateMsgText;
				this.updateAction.button = ui.updateButton;
				this.downloadAction.button = ui.downloadButton;
				this.isDownloadQueuedJSON.indicator = ui.isDownloadQueuedIndicator;
				this.isDownloadingJSON.indicator = ui.isDownloadingIndicator;
				this.isDownloadedJSON.indicator = ui.isDownloadedIndicator;
				this.downloadProgressJSON.slider = ui.progressSlider;
				this.openInPackageManagerAction.button = ui.openInPackageManagerButton;
				this.openSceneAction.button = ui.openSceneButton;
			}
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x0025CD70 File Offset: 0x0025B170
		// Note: this type is marked as 'beforefieldinit'.
		static HubResourcePackage()
		{
		}

		// Token: 0x0400532E RID: 21294
		protected HubBrowse browser;

		// Token: 0x0400532F RID: 21295
		private static readonly string[] SizeSuffixes = new string[]
		{
			"bytes",
			"KB",
			"MB",
			"GB",
			"TB",
			"PB",
			"EB",
			"ZB",
			"YB"
		};

		// Token: 0x04005330 RID: 21296
		protected string package_id;

		// Token: 0x04005331 RID: 21297
		protected string resource_id;

		// Token: 0x04005332 RID: 21298
		protected string resolvedVarName;

		// Token: 0x04005333 RID: 21299
		protected JSONStorableAction goToResourceAction;

		// Token: 0x04005334 RID: 21300
		protected JSONStorableBool isDependencyJSON;

		// Token: 0x04005335 RID: 21301
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <GroupName>k__BackingField;

		// Token: 0x04005336 RID: 21302
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Creator>k__BackingField;

		// Token: 0x04005337 RID: 21303
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Version>k__BackingField;

		// Token: 0x04005338 RID: 21304
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <LatestVersion>k__BackingField;

		// Token: 0x04005339 RID: 21305
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <PublishingUser>k__BackingField;

		// Token: 0x0400533A RID: 21306
		protected JSONStorableString nameJSON;

		// Token: 0x0400533B RID: 21307
		protected JSONStorableString licenseTypeJSON;

		// Token: 0x0400533C RID: 21308
		protected JSONStorableString fileSizeJSON;

		// Token: 0x0400533D RID: 21309
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <FileSize>k__BackingField;

		// Token: 0x0400533E RID: 21310
		protected JSONStorableBool alreadyHaveJSON;

		// Token: 0x0400533F RID: 21311
		protected JSONStorableString updateMsgJSON;

		// Token: 0x04005340 RID: 21312
		protected JSONStorableBool updateAvailableJSON;

		// Token: 0x04005341 RID: 21313
		protected JSONStorableBool alreadyHaveSceneJSON;

		// Token: 0x04005342 RID: 21314
		protected string alreadyHaveScenePath;

		// Token: 0x04005343 RID: 21315
		protected JSONStorableBool notOnHubJSON;

		// Token: 0x04005344 RID: 21316
		public string promotionalUrl;

		// Token: 0x04005345 RID: 21317
		protected string downloadUrl;

		// Token: 0x04005346 RID: 21318
		protected string latestUrl;

		// Token: 0x04005347 RID: 21319
		public HubResourcePackage.DownloadQueuedCallback downloadQueuedCallback;

		// Token: 0x04005348 RID: 21320
		public HubResourcePackage.DownloadStartCallback downloadStartCallback;

		// Token: 0x04005349 RID: 21321
		public HubResourcePackage.DownloadCompleteCallback downloadCompleteCallback;

		// Token: 0x0400534A RID: 21322
		public HubResourcePackage.DownloadProgressCallback downloadProgressCallback;

		// Token: 0x0400534B RID: 21323
		public HubResourcePackage.DownloadErrorCallback downloadErrorCallback;

		// Token: 0x0400534C RID: 21324
		protected JSONStorableFloat downloadProgressJSON;

		// Token: 0x0400534D RID: 21325
		protected JSONStorableBool isDownloadQueuedJSON;

		// Token: 0x0400534E RID: 21326
		protected JSONStorableBool isDownloadingJSON;

		// Token: 0x0400534F RID: 21327
		protected JSONStorableBool isDownloadedJSON;

		// Token: 0x04005350 RID: 21328
		protected bool hadDownloadError;

		// Token: 0x04005351 RID: 21329
		protected JSONStorableAction downloadAction;

		// Token: 0x04005352 RID: 21330
		protected JSONStorableAction updateAction;

		// Token: 0x04005353 RID: 21331
		protected JSONStorableAction openInPackageManagerAction;

		// Token: 0x04005354 RID: 21332
		protected JSONStorableAction openSceneAction;

		// Token: 0x02000CC5 RID: 3269
		// (Invoke) Token: 0x0600626B RID: 25195
		public delegate void DownloadQueuedCallback(HubResourcePackage hrp);

		// Token: 0x02000CC6 RID: 3270
		// (Invoke) Token: 0x0600626F RID: 25199
		public delegate void DownloadStartCallback(HubResourcePackage hrp);

		// Token: 0x02000CC7 RID: 3271
		// (Invoke) Token: 0x06006273 RID: 25203
		public delegate void DownloadCompleteCallback(HubResourcePackage hrp, bool alreadyHad);

		// Token: 0x02000CC8 RID: 3272
		// (Invoke) Token: 0x06006277 RID: 25207
		public delegate void DownloadProgressCallback(HubResourcePackage hrp, float f);

		// Token: 0x02000CC9 RID: 3273
		// (Invoke) Token: 0x0600627B RID: 25211
		public delegate void DownloadErrorCallback(HubResourcePackage hrp, string err);
	}
}
