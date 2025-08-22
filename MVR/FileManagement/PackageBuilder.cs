using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using MVR.FileManagementSecure;
using MVR.Hub;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BE4 RID: 3044
	public class PackageBuilder : JSONStorable
	{
		// Token: 0x0600571A RID: 22298 RVA: 0x001FD2E4 File Offset: 0x001FB6E4
		public PackageBuilder()
		{
		}

		// Token: 0x0600571B RID: 22299 RVA: 0x001FD51C File Offset: 0x001FB91C
		public void ClearAll()
		{
			this.packageNameJSON.SetValToDefault();
			this.ClearStatus();
			this.ClearContentItems();
			this.ClearReferenceItems();
			this.ClearPackageReferenceItems();
			this.descriptionJSON.SetValToDefault();
			this.creditsJSON.SetValToDefault();
			this.instructionsJSON.SetValToDefault();
			this.promotionalLinkJSON.SetValToDefault();
			this.licenseTypeJSON.SetValToDefault();
			this.secondaryLicenseTypeJSON.SetValToDefault();
			this.EAYearJSON.SetValToDefault();
			this.EAMonthJSON.SetValToDefault();
			this.EADayJSON.SetValToDefault();
		}

		// Token: 0x0600571C RID: 22300 RVA: 0x001FD5B0 File Offset: 0x001FB9B0
		public void LoadMetaFromPackageUid(string uid, bool includeCreatorVersionAndRefs = true)
		{
			VarPackage package = FileManager.GetPackage(uid);
			if (package != null)
			{
				this.LoadMetaFromPackage(package, includeCreatorVersionAndRefs);
			}
			else
			{
				this.ShowErrorStatus("Package " + uid + " not found");
				SuperController.LogError("Package " + uid + " not found");
			}
		}

		// Token: 0x0600571D RID: 22301 RVA: 0x001FD604 File Offset: 0x001FBA04
		protected void LoadMetaFromPackage(string path)
		{
			if (path != string.Empty)
			{
				VarPackage package = FileManager.GetPackage(path);
				if (package != null)
				{
					this.LoadMetaFromPackage(package, false);
				}
				else
				{
					this.ShowErrorStatus("Package " + path + " not found");
					SuperController.LogError("Package " + path + " not found");
				}
			}
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x001FD668 File Offset: 0x001FBA68
		protected void LoadMetaFromPackage(VarPackage vp, bool includeCreatorVersionAndRefs)
		{
			if (vp != null)
			{
				if (!vp.invalid)
				{
					if (this.isManager)
					{
						this.currentPackage = vp;
						List<FileEntry> list = new List<FileEntry>();
						vp.FindFiles("Saves/scene", "*.json", list);
						if (list.Count > 0)
						{
							FileEntry fileEntry = list[0];
							this.currentPackageScenePath = fileEntry.Uid;
							this.currentPackageHasSceneJSON.val = true;
						}
						else
						{
							this.currentPackageScenePath = null;
							this.currentPackageHasSceneJSON.val = false;
						}
						if (this.hubBrowse != null && !SuperController.singleton.hubDisabled)
						{
							this.currentPackageResourceId = this.hubBrowse.GetPackageHubResourceId(this.currentPackage.Uid);
							this.currentPackageIsOnHubJSON.val = (this.currentPackageResourceId != null);
						}
						else
						{
							this.currentPackageResourceId = null;
							this.currentPackageIsOnHubJSON.val = false;
						}
						this.packageEnabledJSON.valNoCallback = vp.Enabled;
						this.userNotesJSON.valNoCallback = vp.Group.UserNotes;
						this._userNotes = vp.Group.UserNotes;
						this.pluginsAlwaysEnabledJSON.valNoCallback = vp.PluginsAlwaysEnabled;
						this._pluginsAlwaysEnabled = vp.PluginsAlwaysEnabled;
						this.pluginsAlwaysDisabledJSON.valNoCallback = vp.PluginsAlwaysDisabled;
						this._pluginsAlwaysDisabled = vp.PluginsAlwaysDisabled;
						this.ignoreMissingDependencyErrorsJSON.valNoCallback = vp.IgnoreMissingDependencyErrors;
						this._ignoreMissingDependencyErrors = vp.IgnoreMissingDependencyErrors;
						if (this.repackAction.button != null)
						{
							this.repackAction.button.gameObject.SetActive(vp.IsSimulated);
						}
						if (this.unpackAction.button != null)
						{
							this.unpackAction.button.gameObject.SetActive(!vp.IsSimulated);
						}
						if (this.restoreFromOriginalAction.button != null)
						{
							this.restoreFromOriginalAction.button.gameObject.SetActive(vp.HasOriginalCopy);
						}
						this.hadReferenceIssuesJSON.val = vp.HadReferenceIssues;
					}
					this.ClearAll();
					this.packageNameJSON.val = vp.Name;
					if (includeCreatorVersionAndRefs)
					{
						this.creatorNameJSON.val = vp.Creator;
						this.Version = vp.Version;
						foreach (string packageId in vp.PackageDependencies)
						{
							this.AddPackageReferenceItem(packageId);
						}
					}
					this.standardReferenceVersionOptionJSON.val = vp.StandardReferenceVersionOption.ToString();
					this.scriptReferenceVersionOptionJSON.val = vp.ScriptReferenceVersionOption.ToString();
					this.descriptionJSON.val = vp.Description;
					this.creditsJSON.val = vp.Credits;
					this.instructionsJSON.val = vp.Instructions;
					this.promotionalLinkJSON.val = vp.PromotionalLink;
					this.licenseTypeJSON.val = vp.LicenseType;
					if (vp.SecondaryLicenseType != null)
					{
						this.secondaryLicenseTypeJSON.val = vp.SecondaryLicenseType;
					}
					else
					{
						this.secondaryLicenseTypeJSON.SetValToDefault();
					}
					if (vp.EAEndYear != null)
					{
						this.EAYearJSON.val = vp.EAEndYear;
					}
					else
					{
						this.EAYearJSON.SetValToDefault();
					}
					if (vp.EAEndMonth != null)
					{
						this.EAMonthJSON.val = vp.EAEndMonth;
					}
					else
					{
						this.EAMonthJSON.SetValToDefault();
					}
					if (vp.EAEndDay != null)
					{
						this.EADayJSON.val = vp.EAEndDay;
					}
					else
					{
						this.EADayJSON.SetValToDefault();
					}
					foreach (string itemPath in vp.Contents)
					{
						this.AddContentItem(itemPath);
					}
					if (this.isManager)
					{
						foreach (KeyValuePair<string, PackageBuilderPackageItem> keyValuePair in this.packageItems)
						{
							string key = keyValuePair.Key;
							PackageBuilderPackageItem value = keyValuePair.Value;
							VarPackage package = FileManager.GetPackage(key);
							if (this.currentPackage.Uid == key)
							{
								if (package.Enabled)
								{
									value.SetColor(this.readyColor);
								}
								else
								{
									value.SetColor(this.readyColor);
								}
							}
							else if (package.Enabled)
							{
								if (package.HasMissingDependencies)
								{
									value.SetColor(this.errorColor);
								}
								else
								{
									value.SetColor(Color.white);
								}
							}
							else
							{
								value.SetColor(this.warningColor);
							}
						}
						foreach (VarPackageCustomOption varPackageCustomOption in this.customOptions)
						{
							varPackageCustomOption.ValueNoCallback = vp.Group.GetCustomOption(varPackageCustomOption.name);
						}
					}
					else
					{
						foreach (VarPackageCustomOption varPackageCustomOption2 in this.customOptions)
						{
							varPackageCustomOption2.ValueNoCallback = vp.GetCustomOption(varPackageCustomOption2.name);
						}
					}
				}
				else
				{
					this.ShowErrorStatus("Package " + vp.Uid + " meta file is not valid");
					SuperController.LogError("Package " + vp.Uid + " meta file is not valid");
				}
			}
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x001FDC68 File Offset: 0x001FC068
		public void LoadMetaFromExistingPackage()
		{
			SuperController.singleton.GetMediaPathDialog(new FileBrowserCallback(this.LoadMetaFromPackage), "var", FileManager.PackageFolder, false, true, false, Regex.Escape(this.CreatorName) + ".", false, null, false, false);
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x001FDCB4 File Offset: 0x001FC0B4
		protected string FixNameToBeValidPathName(string s)
		{
			string text = s;
			if (this.invalidChars == null)
			{
				this.invalidChars = new List<char>(Path.GetInvalidFileNameChars())
				{
					'.'
				}.ToArray();
			}
			bool flag = true;
			while (flag)
			{
				int num = text.IndexOfAny(this.invalidChars);
				if (num >= 0)
				{
					text = text.Replace(text[num], '_');
				}
				else if (text.IndexOf(' ') >= 0)
				{
					text = text.Replace(' ', '_');
				}
				else
				{
					flag = false;
				}
			}
			return text;
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x001FDD44 File Offset: 0x001FC144
		protected void SyncCreatorName(string s)
		{
			string text = this.FixNameToBeValidPathName(s);
			if (text != s)
			{
				this.ShowErrorStatus("Creator name was fixed to convert invalid characters to _");
				this.creatorNameJSON.valNoCallback = text;
			}
			else
			{
				this.ClearStatus();
			}
			this._creatorName = text;
			this.NeedsPrep = true;
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06005722 RID: 22306 RVA: 0x001FDD95 File Offset: 0x001FC195
		// (set) Token: 0x06005723 RID: 22307 RVA: 0x001FDD9D File Offset: 0x001FC19D
		public string CreatorName
		{
			get
			{
				return this._creatorName;
			}
			protected set
			{
				this.creatorNameJSON.val = value;
			}
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x001FDDAC File Offset: 0x001FC1AC
		protected void SyncPackageName(string s)
		{
			string text = this.FixNameToBeValidPathName(s);
			if (text != s)
			{
				this.ShowErrorStatus("Package name was fixed to convert invalid characters to _");
				this.packageNameJSON.valNoCallback = text;
			}
			else
			{
				this.ClearStatus();
			}
			this._packageName = text;
			this.NeedsPrep = true;
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06005725 RID: 22309 RVA: 0x001FDDFD File Offset: 0x001FC1FD
		// (set) Token: 0x06005726 RID: 22310 RVA: 0x001FDE05 File Offset: 0x001FC205
		public string PackageName
		{
			get
			{
				return this._packageName;
			}
			set
			{
				this.packageNameJSON.val = value;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06005727 RID: 22311 RVA: 0x001FDE13 File Offset: 0x001FC213
		// (set) Token: 0x06005728 RID: 22312 RVA: 0x001FDE1C File Offset: 0x001FC21C
		public int Version
		{
			get
			{
				return this._version;
			}
			protected set
			{
				if (this._version != value)
				{
					this._version = value;
					if (this.versionField != null)
					{
						if (this._version == 0)
						{
							this.versionField.text = string.Empty;
						}
						else
						{
							this.versionField.text = this._version.ToString();
						}
					}
				}
			}
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x001FDE89 File Offset: 0x001FC289
		protected void ShowStatus(string status, Color color)
		{
			if (this.statusText != null)
			{
				this.statusText.color = color;
				this.statusText.text = status;
			}
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x001FDEB4 File Offset: 0x001FC2B4
		protected void ShowErrorStatus(string status)
		{
			this.ShowStatus(status, this.errorColor);
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x001FDEC3 File Offset: 0x001FC2C3
		protected void ShowStatus(string status)
		{
			this.ShowStatus(status, this.normalColor);
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x001FDED2 File Offset: 0x001FC2D2
		protected void ClearStatus()
		{
			this.ShowStatus(string.Empty);
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x001FDEE0 File Offset: 0x001FC2E0
		public List<string> MissingPackageNames
		{
			get
			{
				if (this.missingPackageItems != null)
				{
					List<string> list = this.missingPackageItems.Keys.ToList<string>();
					list.Sort();
					return list;
				}
				return new List<string>();
			}
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x001FDF18 File Offset: 0x001FC318
		protected void ClearPackageReferenceItems()
		{
			if (this.packageReferenceItems != null)
			{
				foreach (PackageBuilderPackageItem packageBuilderPackageItem in this.packageReferenceItems)
				{
					UnityEngine.Object.Destroy(packageBuilderPackageItem.gameObject);
				}
				this.packageReferenceItems.Clear();
			}
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x001FDF90 File Offset: 0x001FC390
		protected void AddPackageReferenceItem(string packageId)
		{
			PackageBuilder.<AddPackageReferenceItem>c__AnonStorey3 <AddPackageReferenceItem>c__AnonStorey = new PackageBuilder.<AddPackageReferenceItem>c__AnonStorey3();
			<AddPackageReferenceItem>c__AnonStorey.packageId = packageId;
			<AddPackageReferenceItem>c__AnonStorey.$this = this;
			if (this.packageReferenceItems != null && this.packageReferencesContainer != null && this.packageItemPrefab != null)
			{
				RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.packageItemPrefab);
				PackageBuilderPackageItem component = rectTransform.GetComponent<PackageBuilderPackageItem>();
				if (component != null)
				{
					rectTransform.SetParent(this.packageReferencesContainer, false);
					if (component.button != null)
					{
						component.button.onClick.AddListener(new UnityAction(<AddPackageReferenceItem>c__AnonStorey.<>m__0));
					}
					component.Package = <AddPackageReferenceItem>c__AnonStorey.packageId;
					VarPackage package = FileManager.GetPackage(<AddPackageReferenceItem>c__AnonStorey.packageId);
					if (package == null || !package.Enabled)
					{
						string packageGroupUid = FileManager.PackageIDToPackageGroupID(<AddPackageReferenceItem>c__AnonStorey.packageId);
						if (FileManager.GetPackageGroup(packageGroupUid) == null)
						{
							component.SetColor(this.errorColor);
						}
						else
						{
							component.SetColor(this.warningColor);
						}
					}
					this.packageReferenceItems.Add(component);
				}
			}
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x001FE0A8 File Offset: 0x001FC4A8
		protected void SyncPackagesList()
		{
			string text = null;
			if (this.currentPackage != null)
			{
				text = this.currentPackage.Uid;
			}
			this.ClearAll();
			foreach (PackageBuilderPackageItem packageBuilderPackageItem in this.packageItems.Values)
			{
				UnityEngine.Object.Destroy(packageBuilderPackageItem.gameObject);
			}
			this.packageItems.Clear();
			foreach (PackageBuilderPackageItem packageBuilderPackageItem2 in this.missingPackageItems.Values)
			{
				UnityEngine.Object.Destroy(packageBuilderPackageItem2.gameObject);
			}
			this.missingPackageItems.Clear();
			bool flag = false;
			List<string> list;
			if (this.currentCategory != null)
			{
				string text2 = this.currentCategory;
				if (text2 != null)
				{
					if (text2 == "Preload Morphs")
					{
						list = new List<string>();
						foreach (VarPackage varPackage in FileManager.GetPackages())
						{
							if (varPackage.Group.GetCustomOption("preloadMorphs"))
							{
								list.Add(varPackage.Uid);
							}
						}
						goto IL_349;
					}
					if (text2 == "Plugins Always Disabled")
					{
						list = new List<string>();
						foreach (VarPackage varPackage2 in FileManager.GetPackages())
						{
							if (varPackage2.PluginsAlwaysDisabled)
							{
								list.Add(varPackage2.Uid);
							}
						}
						goto IL_349;
					}
					if (text2 == "Plugins Always Enabled")
					{
						list = new List<string>();
						foreach (VarPackage varPackage3 in FileManager.GetPackages())
						{
							if (varPackage3.PluginsAlwaysEnabled)
							{
								list.Add(varPackage3.Uid);
							}
						}
						goto IL_349;
					}
					if (text2 == "Disabled")
					{
						flag = true;
						list = new List<string>();
						foreach (VarPackage varPackage4 in FileManager.GetPackages())
						{
							if (!varPackage4.Enabled)
							{
								list.Add(varPackage4.Uid);
							}
						}
						goto IL_349;
					}
					if (text2 == "Missing Dependencies")
					{
						list = new List<string>();
						foreach (VarPackage varPackage5 in FileManager.GetPackages())
						{
							if (varPackage5.HasMissingDependencies)
							{
								list.Add(varPackage5.Uid);
							}
						}
						goto IL_349;
					}
				}
				if (!this.categoryToPackageUids.TryGetValue(this.currentCategory, out list))
				{
					list = FileManager.GetPackageUids();
				}
				IL_349:;
			}
			else
			{
				list = FileManager.GetPackageUids();
			}
			using (List<string>.Enumerator enumerator8 = list.GetEnumerator())
			{
				while (enumerator8.MoveNext())
				{
					PackageBuilder.<SyncPackagesList>c__AnonStorey4 <SyncPackagesList>c__AnonStorey = new PackageBuilder.<SyncPackagesList>c__AnonStorey4();
					<SyncPackagesList>c__AnonStorey.vpuid = enumerator8.Current;
					<SyncPackagesList>c__AnonStorey.$this = this;
					VarPackage package = FileManager.GetPackage(<SyncPackagesList>c__AnonStorey.vpuid);
					if (package != null && (flag || this.showDisabledJSON.val || package.Enabled))
					{
						RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.packageItemPrefab);
						PackageBuilderPackageItem component = rectTransform.GetComponent<PackageBuilderPackageItem>();
						if (component != null)
						{
							rectTransform.SetParent(this.packagesContainer, false);
							if (component.button != null)
							{
								component.button.onClick.AddListener(new UnityAction(<SyncPackagesList>c__AnonStorey.<>m__0));
							}
							if (!package.Enabled)
							{
								component.SetColor(this.warningColor);
							}
							else if (package.HasMissingDependencies || package.invalid)
							{
								component.SetColor(this.errorColor);
							}
							component.Package = <SyncPackagesList>c__AnonStorey.vpuid;
							this.packageItems.Add(<SyncPackagesList>c__AnonStorey.vpuid, component);
						}
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (VarPackage varPackage6 in FileManager.GetPackages())
			{
				if (!varPackage6.IgnoreMissingDependencyErrors)
				{
					foreach (string item in varPackage6.PackageDependenciesMissing)
					{
						hashSet.Add(item);
					}
				}
			}
			List<string> list2 = hashSet.ToList<string>();
			list2.Sort();
			foreach (string text3 in list2)
			{
				RectTransform rectTransform2 = UnityEngine.Object.Instantiate<RectTransform>(this.packageItemPrefab);
				PackageBuilderPackageItem component2 = rectTransform2.GetComponent<PackageBuilderPackageItem>();
				if (component2 != null)
				{
					rectTransform2.SetParent(this.missingPackagesContainer, false);
					string packageGroupUid = FileManager.PackageIDToPackageGroupID(text3);
					if (FileManager.GetPackageGroup(packageGroupUid) == null)
					{
						component2.SetColor(this.errorColor);
					}
					else
					{
						component2.SetColor(this.warningColor);
					}
					component2.Package = text3;
					this.missingPackageItems.Add(text3, component2);
				}
			}
			if (text != null)
			{
				this.LoadMetaFromPackageUid(text, true);
			}
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x001FE7C4 File Offset: 0x001FCBC4
		protected void SyncPackages()
		{
			if (this.packageItems != null && this.packagesContainer != null && this.packageItemPrefab != null && this.missingPackageItems != null && this.missingPackagesContainer != null)
			{
				this.SyncCategoryToPackageUids();
				this.SyncPackagesList();
			}
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x001FE826 File Offset: 0x001FCC26
		protected void ScanHubForMissingPackages()
		{
			if (this.hubBrowse != null)
			{
				this.hubBrowse.OpenMissingPackagesPanel();
			}
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x001FE844 File Offset: 0x001FCC44
		protected void SelectCurrentScenePackage()
		{
			string topPackageUid = FileManager.TopPackageUid;
			if (topPackageUid != null)
			{
				this.LoadMetaFromPackageUid(topPackageUid, true);
			}
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x001FE865 File Offset: 0x001FCC65
		protected void SyncShowDisabled(bool b)
		{
			this.SyncPackagesList();
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x001FE870 File Offset: 0x001FCC70
		protected void SyncCategoryToPackageUids()
		{
			if (this.categoryToPackageUids == null)
			{
				this.categoryToPackageUids = new Dictionary<string, List<string>>();
			}
			else
			{
				this.categoryToPackageUids.Clear();
			}
			List<string> packageUids = FileManager.GetPackageUids();
			foreach (PackageBuilder.CategoryFilter categoryFilter in this.categoryFilters)
			{
				List<string> list = new List<string>();
				this.categoryToPackageUids.Add(categoryFilter.name, list);
				foreach (string text in packageUids)
				{
					if (categoryFilter.matchDirectoryPath == string.Empty)
					{
						list.Add(text);
					}
					else
					{
						VarPackage package = FileManager.GetPackage(text);
						if (package.HasMatchingDirectories(categoryFilter.matchDirectoryPath))
						{
							list.Add(text);
						}
					}
				}
			}
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x001FE970 File Offset: 0x001FCD70
		protected void CategoryChangeCallback(string category)
		{
			this.currentCategory = category;
			this.SyncPackagesList();
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x001FE97F File Offset: 0x001FCD7F
		protected void GoToPromotionalLink()
		{
			if (this.promotionalButtonText != null)
			{
				SuperController.singleton.OpenLinkInBrowser(this.promotionalButtonText.text);
			}
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x001FE9A7 File Offset: 0x001FCDA7
		protected void CopyPromotionalLink()
		{
			if (this.promotionalButtonText != null)
			{
				GUIUtility.systemCopyBuffer = this.promotionalButtonText.text;
			}
		}

		// Token: 0x06005739 RID: 22329 RVA: 0x001FE9CA File Offset: 0x001FCDCA
		protected void SyncPackageEnabled(bool b)
		{
			if (this.currentPackage != null)
			{
				this.currentPackage.Enabled = b;
			}
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x001FE9E3 File Offset: 0x001FCDE3
		protected void OpenOnHub()
		{
			if (this.hubBrowse != null && this.currentPackageResourceId != null && !SuperController.singleton.hubDisabled)
			{
				this.hubBrowse.OpenDetail(this.currentPackageResourceId, false);
			}
		}

		// Token: 0x0600573B RID: 22331 RVA: 0x001FEA24 File Offset: 0x001FCE24
		protected void OpenInHubDownloader()
		{
			if (this.hubDownloader != null && this.currentPackageResourceId != null && !SuperController.singleton.hubDisabled)
			{
				this.hubDownloader.FindResource(this.currentPackageResourceId, false);
				this.hubDownloader.OpenPanel();
			}
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x001FEA79 File Offset: 0x001FCE79
		protected void OpenScene()
		{
			if (this.currentPackageScenePath != null)
			{
				SuperController.singleton.Load(this.currentPackageScenePath);
			}
		}

		// Token: 0x0600573D RID: 22333 RVA: 0x001FEA98 File Offset: 0x001FCE98
		protected void DeletePackage()
		{
			if (this.currentPackage != null)
			{
				if (this.confirmDeletePackagePanel != null)
				{
					this.confirmDeletePackagePanel.gameObject.SetActive(true);
				}
				if (this.confirmDeletePackageText != null)
				{
					this.confirmDeletePackageText.text = "Delete " + this.currentPackage.Uid + "?";
				}
			}
		}

		// Token: 0x0600573E RID: 22334 RVA: 0x001FEB08 File Offset: 0x001FCF08
		protected void ConfirmDeletePackage()
		{
			if (this.currentPackage != null)
			{
				VarPackage varPackage = this.currentPackage;
				this.currentPackage = null;
				this.currentPackageIsOnHubJSON.val = false;
				this.currentPackageResourceId = null;
				this.currentPackageHasSceneJSON.val = false;
				this.currentPackageScenePath = null;
				varPackage.Delete();
			}
			this.CancelDeletePackage();
		}

		// Token: 0x0600573F RID: 22335 RVA: 0x001FEB60 File Offset: 0x001FCF60
		protected void CancelDeletePackage()
		{
			if (this.confirmDeletePackagePanel != null)
			{
				this.confirmDeletePackagePanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x001FEB84 File Offset: 0x001FCF84
		protected void UnpackComplete()
		{
			this.unpackFlag.Raise();
			if (this.packPanel != null)
			{
				this.packPanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x001FEBB4 File Offset: 0x001FCFB4
		protected IEnumerator UnpackCo()
		{
			this.unpackFlag = new AsyncFlag("UnpackPackage");
			SuperController.singleton.SetLoadingIconFlag(this.unpackFlag);
			if (this.packPanel != null)
			{
				this.packPanel.gameObject.SetActive(true);
			}
			yield return null;
			if (this.packProgressSlider != null)
			{
				this.packProgressSlider.value = 0f;
			}
			this.currentPackage.Unpack();
			while (this.currentPackage.IsUnpacking)
			{
				yield return null;
				if (this.packProgressSlider != null)
				{
					this.packProgressSlider.value = this.currentPackage.packProgress;
				}
			}
			if (this.currentPackage.packThreadError != null)
			{
				this.ShowErrorStatus("Exception during unpack");
				SuperController.LogError("Exception during package unpack: " + this.currentPackage.packThreadError);
			}
			else
			{
				FileManager.Refresh();
			}
			this.UnpackComplete();
			yield break;
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x001FEBCF File Offset: 0x001FCFCF
		protected void Unpack()
		{
			if (this.confirmUnpackPanel != null)
			{
				this.confirmUnpackPanel.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x001FEBF4 File Offset: 0x001FCFF4
		protected void ConfirmUnpack()
		{
			if (this.confirmUnpackPanel != null)
			{
				this.confirmUnpackPanel.gameObject.SetActive(false);
			}
			if (this.currentPackage != null && !this.currentPackage.IsSimulated)
			{
				base.StartCoroutine(this.UnpackCo());
			}
		}

		// Token: 0x06005744 RID: 22340 RVA: 0x001FEC4B File Offset: 0x001FD04B
		protected void CancelUnpack()
		{
			if (this.confirmUnpackPanel != null)
			{
				this.confirmUnpackPanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005745 RID: 22341 RVA: 0x001FEC6F File Offset: 0x001FD06F
		protected void RepackComplete()
		{
			this.repackFlag.Raise();
			if (this.packPanel != null)
			{
				this.packPanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005746 RID: 22342 RVA: 0x001FECA0 File Offset: 0x001FD0A0
		protected IEnumerator RepackCo()
		{
			this.repackFlag = new AsyncFlag("RepackPackage");
			SuperController.singleton.SetLoadingIconFlag(this.repackFlag);
			if (this.packPanel != null)
			{
				this.packPanel.gameObject.SetActive(true);
			}
			yield return null;
			if (this.packProgressSlider != null)
			{
				this.packProgressSlider.value = 0f;
			}
			this.currentPackage.Repack();
			while (this.currentPackage.IsRepacking)
			{
				yield return null;
				if (this.packProgressSlider != null)
				{
					this.packProgressSlider.value = this.currentPackage.packProgress;
				}
			}
			if (this.currentPackage.packThreadError != null)
			{
				this.ShowErrorStatus("Exception during repack");
				SuperController.LogError("Exception during package repack: " + this.currentPackage.packThreadError);
			}
			else
			{
				FileManager.Refresh();
			}
			this.RepackComplete();
			yield break;
		}

		// Token: 0x06005747 RID: 22343 RVA: 0x001FECBB File Offset: 0x001FD0BB
		protected void Repack()
		{
			if (this.currentPackage != null && this.currentPackage.IsSimulated)
			{
				base.StartCoroutine(this.RepackCo());
			}
		}

		// Token: 0x06005748 RID: 22344 RVA: 0x001FECE5 File Offset: 0x001FD0E5
		protected void RestoreFromOriginal()
		{
			if (this.confirmRestoreFromOriginalPanel != null)
			{
				this.confirmRestoreFromOriginalPanel.gameObject.SetActive(true);
			}
		}

		// Token: 0x06005749 RID: 22345 RVA: 0x001FED0C File Offset: 0x001FD10C
		protected void ConfirmRestoreFromOriginal()
		{
			if (this.confirmRestoreFromOriginalPanel != null)
			{
				this.confirmRestoreFromOriginalPanel.gameObject.SetActive(false);
			}
			if (this.currentPackage != null)
			{
				this.currentPackage.RestoreFromOriginal();
				FileManager.UnregisterPackage(this.currentPackage);
				FileManager.Refresh();
			}
		}

		// Token: 0x0600574A RID: 22346 RVA: 0x001FED61 File Offset: 0x001FD161
		protected void CancelRestoreFromOriginal()
		{
			if (this.confirmRestoreFromOriginalPanel != null)
			{
				this.confirmRestoreFromOriginalPanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600574B RID: 22347 RVA: 0x001FED85 File Offset: 0x001FD185
		protected void CustomOptionChange(JSONStorableBool customBool)
		{
			if (this.isManager && this.currentPackage != null)
			{
				this.currentPackage.Group.SetCustomOption(customBool.name, customBool.val);
			}
		}

		// Token: 0x0600574C RID: 22348 RVA: 0x001FEDB9 File Offset: 0x001FD1B9
		protected void SyncUserNotes(string s)
		{
			this._userNotes = s;
			if (this.currentPackage != null)
			{
				this.currentPackage.Group.UserNotes = this._userNotes;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x0600574D RID: 22349 RVA: 0x001FEDE3 File Offset: 0x001FD1E3
		// (set) Token: 0x0600574E RID: 22350 RVA: 0x001FEDEB File Offset: 0x001FD1EB
		public string UserNotes
		{
			get
			{
				return this._userNotes;
			}
			set
			{
				this.userNotesJSON.val = value;
			}
		}

		// Token: 0x0600574F RID: 22351 RVA: 0x001FEDF9 File Offset: 0x001FD1F9
		protected void SyncPluginsAlwaysEnabled(bool b)
		{
			this._pluginsAlwaysEnabled = b;
			if (this.currentPackage != null)
			{
				this.currentPackage.PluginsAlwaysEnabled = this._pluginsAlwaysEnabled;
				if (this._pluginsAlwaysEnabled)
				{
					this.pluginsAlwaysDisabledJSON.val = false;
				}
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06005750 RID: 22352 RVA: 0x001FEE35 File Offset: 0x001FD235
		// (set) Token: 0x06005751 RID: 22353 RVA: 0x001FEE3D File Offset: 0x001FD23D
		public bool PluginsAlwaysEnabled
		{
			get
			{
				return this._pluginsAlwaysEnabled;
			}
			set
			{
				this.pluginsAlwaysEnabledJSON.val = value;
			}
		}

		// Token: 0x06005752 RID: 22354 RVA: 0x001FEE4B File Offset: 0x001FD24B
		protected void SyncPluginsAlwaysDisabled(bool b)
		{
			this._pluginsAlwaysDisabled = b;
			if (this.currentPackage != null)
			{
				this.currentPackage.PluginsAlwaysDisabled = this._pluginsAlwaysDisabled;
				if (this._pluginsAlwaysDisabled)
				{
					this.pluginsAlwaysEnabledJSON.val = false;
				}
			}
		}

		// Token: 0x17000CAC RID: 3244
		// (get) Token: 0x06005753 RID: 22355 RVA: 0x001FEE87 File Offset: 0x001FD287
		// (set) Token: 0x06005754 RID: 22356 RVA: 0x001FEE8F File Offset: 0x001FD28F
		public bool PluginsAlwaysDisabled
		{
			get
			{
				return this._pluginsAlwaysDisabled;
			}
			set
			{
				this.pluginsAlwaysDisabledJSON.val = value;
			}
		}

		// Token: 0x06005755 RID: 22357 RVA: 0x001FEE9D File Offset: 0x001FD29D
		protected void SyncIgnoreMissingDependencyErrors(bool b)
		{
			this._ignoreMissingDependencyErrors = b;
			if (this.currentPackage != null)
			{
				this.currentPackage.IgnoreMissingDependencyErrors = this._ignoreMissingDependencyErrors;
			}
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x06005756 RID: 22358 RVA: 0x001FEEC2 File Offset: 0x001FD2C2
		// (set) Token: 0x06005757 RID: 22359 RVA: 0x001FEECA File Offset: 0x001FD2CA
		public bool IgnoreMissingDependencyErrors
		{
			get
			{
				return this._ignoreMissingDependencyErrors;
			}
			set
			{
				this.ignoreMissingDependencyErrorsJSON.val = value;
			}
		}

		// Token: 0x06005758 RID: 22360 RVA: 0x001FEED8 File Offset: 0x001FD2D8
		public void ClearContentItems()
		{
			foreach (PackageBuilderContentItem packageBuilderContentItem in this.contentItems.Values)
			{
				UnityEngine.Object.Destroy(packageBuilderContentItem.gameObject);
			}
			this.contentItems.Clear();
			this.NeedsPrep = true;
			this.ClearStatus();
		}

		// Token: 0x06005759 RID: 22361 RVA: 0x001FEF58 File Offset: 0x001FD358
		public PackageBuilderContentItem AddContentItem(string itemPath)
		{
			PackageBuilderContentItem packageBuilderContentItem = null;
			if (this.contentItems != null && !this.contentItems.ContainsKey(itemPath) && this.contentItemPrefab != null && this.contentContainer != null)
			{
				RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.contentItemPrefab);
				packageBuilderContentItem = rectTransform.GetComponent<PackageBuilderContentItem>();
				if (packageBuilderContentItem != null)
				{
					rectTransform.SetParent(this.contentContainer, false);
					packageBuilderContentItem.ItemPath = itemPath;
					this.contentItems.Add(itemPath, packageBuilderContentItem);
					this.NeedsPrep = true;
					this.ClearStatus();
				}
			}
			if (this._packageName == null || this._packageName == string.Empty)
			{
				this.packageNameJSON.val = Path.GetFileNameWithoutExtension(itemPath);
			}
			if (Regex.IsMatch(itemPath, "\\.(json|vap|vam|scene|assetbundle)$"))
			{
				string text = Regex.Replace(itemPath, "\\.(json|vac|vap|vam|scene|assetbundle)$", ".jpg");
				if (File.Exists(text))
				{
					this.AddContentItem(text);
				}
			}
			if (Regex.IsMatch(itemPath, "\\.vam$"))
			{
				string text2 = Regex.Replace(itemPath, "\\.vam$", ".vaj");
				if (File.Exists(text2))
				{
					this.AddContentItem(text2);
				}
				string text3 = Regex.Replace(itemPath, "\\.vam$", ".vab");
				if (File.Exists(text3))
				{
					this.AddContentItem(text3);
				}
			}
			else if (Regex.IsMatch(itemPath, "\\.vmi"))
			{
				string text4 = Regex.Replace(itemPath, "\\.vmi$", ".vmb");
				if (File.Exists(text4))
				{
					this.AddContentItem(text4);
				}
			}
			return packageBuilderContentItem;
		}

		// Token: 0x0600575A RID: 22362 RVA: 0x001FF0EF File Offset: 0x001FD4EF
		public bool RemoveItem(PackageBuilderContentItem contentItem)
		{
			if (this.contentItems != null && this.contentItems.Remove(contentItem.ItemPath))
			{
				UnityEngine.Object.Destroy(contentItem.gameObject);
				this.NeedsPrep = true;
				this.ClearStatus();
				return true;
			}
			return false;
		}

		// Token: 0x0600575B RID: 22363 RVA: 0x001FF130 File Offset: 0x001FD530
		public PackageBuilderContentItem GetItem(string itemPath)
		{
			PackageBuilderContentItem result = null;
			if (this.contentItems != null)
			{
				this.contentItems.TryGetValue(itemPath, out result);
			}
			return result;
		}

		// Token: 0x0600575C RID: 22364 RVA: 0x001FF15C File Offset: 0x001FD55C
		protected void AddDirectoryCallback(string dir)
		{
			if (dir != null && dir != string.Empty)
			{
				dir = FileManager.NormalizePath(dir);
				this.suggestedDirectory = Path.GetDirectoryName(dir);
				this.suggestedDirectory = this.suggestedDirectory.Replace('/', '\\');
				this.AddContentItem(dir);
			}
		}

		// Token: 0x0600575D RID: 22365 RVA: 0x001FF1B0 File Offset: 0x001FD5B0
		public void AddDirectory()
		{
			SuperController.singleton.GetDirectoryPathDialog(new FileBrowserCallback(this.AddDirectoryCallback), this.suggestedDirectory, this.shortCuts, false);
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x001FF1D8 File Offset: 0x001FD5D8
		protected void AddFileCallback(string file)
		{
			if (file != null && file != string.Empty)
			{
				file = FileManager.NormalizePath(file);
				this.suggestedDirectory = Path.GetDirectoryName(file);
				this.suggestedDirectory = this.suggestedDirectory.Replace('/', '\\');
				this.AddContentItem(file);
			}
		}

		// Token: 0x0600575F RID: 22367 RVA: 0x001FF22C File Offset: 0x001FD62C
		public void AddFile()
		{
			SuperController.singleton.GetMediaPathDialog(new FileBrowserCallback(this.AddFileCallback), string.Empty, this.suggestedDirectory, false, true, false, null, false, this.shortCuts, true, false);
		}

		// Token: 0x06005760 RID: 22368 RVA: 0x001FF268 File Offset: 0x001FD668
		public void RemoveSelected()
		{
			if (this.contentItems != null)
			{
				List<PackageBuilderContentItem> list = new List<PackageBuilderContentItem>();
				foreach (PackageBuilderContentItem packageBuilderContentItem in this.contentItems.Values)
				{
					if (packageBuilderContentItem.IsSelected)
					{
						list.Add(packageBuilderContentItem);
					}
				}
				foreach (PackageBuilderContentItem contentItem in list)
				{
					this.RemoveItem(contentItem);
				}
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x06005761 RID: 22369 RVA: 0x001FF330 File Offset: 0x001FD730
		// (set) Token: 0x06005762 RID: 22370 RVA: 0x001FF338 File Offset: 0x001FD738
		protected bool NeedsPrep
		{
			get
			{
				return this._needsPrep;
			}
			set
			{
				if (this._needsPrep != value)
				{
					this._needsPrep = value;
					if (this._needsPrep)
					{
						this.ClearReferenceItems();
						this.Version = 0;
					}
					if (this.prepPackageAction != null && this.prepPackageAction.dynamicButton != null)
					{
						if (this._needsPrep)
						{
							this.prepPackageAction.dynamicButton.buttonColor = this.readyColor;
						}
						else
						{
							this.prepPackageAction.dynamicButton.buttonColor = this.disabledColor;
						}
					}
					if (this.openPrepFolderInExplorerNotice != null)
					{
						this.openPrepFolderInExplorerNotice.gameObject.SetActive(!this._needsPrep);
					}
					if (this.openPrepFolderInExplorerAction != null && this.openPrepFolderInExplorerAction.button != null)
					{
						this.openPrepFolderInExplorerAction.button.interactable = !this._needsPrep;
					}
					if (this.finalizePackageAction != null && this.finalizePackageAction.button != null)
					{
						this.finalizePackageAction.button.interactable = !this._needsPrep;
					}
				}
			}
		}

		// Token: 0x06005763 RID: 22371 RVA: 0x001FF46C File Offset: 0x001FD86C
		protected void SyncStandardReferenceVersionOption(string s)
		{
			try
			{
				VarPackage.ReferenceVersionOption standardReferenceVersionOption = (VarPackage.ReferenceVersionOption)Enum.Parse(typeof(VarPackage.ReferenceVersionOption), s);
				this._standardReferenceVersionOption = standardReferenceVersionOption;
			}
			catch (ArgumentException)
			{
				SuperController.LogError("Error while setting reference version option. Bad choice " + s);
			}
		}

		// Token: 0x06005764 RID: 22372 RVA: 0x001FF4C4 File Offset: 0x001FD8C4
		protected void SyncScriptReferenceVersionOption(string s)
		{
			try
			{
				VarPackage.ReferenceVersionOption scriptReferenceVersionOption = (VarPackage.ReferenceVersionOption)Enum.Parse(typeof(VarPackage.ReferenceVersionOption), s);
				this._scriptReferenceVersionOption = scriptReferenceVersionOption;
			}
			catch (ArgumentException)
			{
				SuperController.LogError("Error while setting reference version option. Bad choice " + s);
			}
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x001FF51C File Offset: 0x001FD91C
		protected void CopyAndFixRefs(string packageGroup, string fromFile, string toFile, HashSet<string> internalFilesHashSet, Dictionary<string, string> vamIDsToPaths)
		{
			string directoryName = Path.GetDirectoryName(fromFile);
			using (StreamReader streamReader = new StreamReader(fromFile))
			{
				using (StreamWriter streamWriter = new StreamWriter(toFile))
				{
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						if (text.Contains("SELF:/"))
						{
							text = text.Replace("SELF:/", string.Empty);
						}
						Match match = Regex.Match(text, "\\\"([^\\\"]*)\\\"\\s*:\\s*\\\"(.*\\.)(mp3|ogg|wav|jpg|jpeg|png|gif|tif|tiff|avi|mp4|assetbundle|scene|json|vmi|vam|vap|cs|cslist|dll)\\\"", RegexOptions.IgnoreCase);
						if (match.Success)
						{
							string value = match.Groups[1].Value;
							if (value != "name" && value != "displayName" && value != "audioClip")
							{
								string text2 = match.Groups[2].Value + match.Groups[3].Value;
								bool flag = false;
								if (value == "receiverTargetName")
								{
									if (text2.StartsWith("toggle:"))
									{
										text2 = Regex.Replace(text2, "^toggle:", string.Empty);
										flag = true;
									}
									else if (text2.StartsWith("hair:"))
									{
										text2 = Regex.Replace(text2, "^hair:", string.Empty);
										flag = true;
									}
									else if (text2.StartsWith("clothing:"))
									{
										text2 = Regex.Replace(text2, "^clothing:", string.Empty);
										flag = true;
									}
								}
								if (text2.Contains(":/"))
								{
									string text3 = Regex.Replace(text2, ":/.*", string.Empty);
									string[] array = text3.Split(new char[]
									{
										'.'
									});
									if (array.Length > 2)
									{
										string text4 = array[0] + "." + array[1];
										string str = array[2];
										if (packageGroup == text4)
										{
											string text5 = Regex.Replace(text2, ".*:/", string.Empty);
											if (internalFilesHashSet.Contains(text5))
											{
												text = text.Replace(text2, "SELF:/" + text5);
											}
											else if (!this.referenceReports.ContainsKey(text2))
											{
												if (File.Exists(text5))
												{
													PackageBuilder.ReferenceReport value2 = new PackageBuilder.ReferenceReport(text2, null, null, true, "FIXABLE: References older version of this package, but file not included in new one", true, text5);
													this.referenceReports.Add(text2, value2);
												}
												else
												{
													PackageBuilder.ReferenceReport value3 = new PackageBuilder.ReferenceReport(text2, null, null, true, "BROKEN: References older version of same package, but file not included and file is missing", false, null);
													this.referenceReports.Add(text2, value3);
												}
											}
										}
										else
										{
											string text6 = text2;
											string text7 = text3;
											VarPackageGroup packageGroup2 = FileManager.GetPackageGroup(text4);
											VarPackage varPackage = FileManager.GetPackage(text3);
											if (varPackage == null && packageGroup2 != null)
											{
												varPackage = packageGroup2.NewestPackage;
												str = varPackage.Version.ToString();
											}
											bool flag2 = varPackage != null && varPackage.HasMatchingDirectories("Custom/Scripts");
											VarPackage.ReferenceVersionOption referenceVersionOption;
											if (flag2)
											{
												referenceVersionOption = this._scriptReferenceVersionOption;
											}
											else
											{
												referenceVersionOption = this._standardReferenceVersionOption;
											}
											if (!flag)
											{
												if (referenceVersionOption == VarPackage.ReferenceVersionOption.Latest)
												{
													text7 = text4 + ".latest";
													text6 = text2.Replace(text3, text7);
													text = text.Replace(text2, text6);
													if (packageGroup2 != null)
													{
														varPackage = packageGroup2.NewestPackage;
													}
												}
												else if (referenceVersionOption == VarPackage.ReferenceVersionOption.Minimum)
												{
													text7 = text4 + ".min" + str;
													text6 = text2.Replace(text3, text7);
													text = text.Replace(text2, text6);
												}
											}
											if (!this.referenceReports.ContainsKey(text6))
											{
												if (varPackage == null)
												{
													PackageBuilder.ReferenceReport value4 = new PackageBuilder.ReferenceReport(text2, null, text7, true, "BROKEN: Missing package that is referenced", false, null);
													this.referenceReports.Add(text6, value4);
												}
												else
												{
													PackageBuilder.ReferenceReport value5 = new PackageBuilder.ReferenceReport(text6, varPackage, text7, false, string.Empty, false, null);
													this.referenceReports.Add(text6, value5);
												}
											}
										}
									}
									else if (!this.referenceReports.ContainsKey(text2))
									{
										PackageBuilder.ReferenceReport value6 = new PackageBuilder.ReferenceReport(text2, null, null, true, "BROKEN: References file outside of install directory", false, null);
										this.referenceReports.Add(text2, value6);
									}
								}
								else if (!text2.StartsWith("./") && text2.Contains("/"))
								{
									if (internalFilesHashSet.Contains(text2))
									{
										text = text.Replace(text2, "SELF:/" + text2);
									}
									else if (!this.referenceReports.ContainsKey(text2))
									{
										if (File.Exists(text2))
										{
											PackageBuilder.ReferenceReport value7 = new PackageBuilder.ReferenceReport(text2, null, null, true, "FIXABLE: References local file not included in package", true, null);
											this.referenceReports.Add(text2, value7);
										}
										else
										{
											PackageBuilder.ReferenceReport value8 = new PackageBuilder.ReferenceReport(text2, null, null, true, "BROKEN: References local file not included in package. File is missing", false, null);
											this.referenceReports.Add(text2, value8);
										}
									}
								}
								else
								{
									text2 = Regex.Replace(text2, "^\\./", string.Empty);
									string text8 = directoryName + "/" + text2;
									if (!internalFilesHashSet.Contains(text8))
									{
										if (!this.referenceReports.ContainsKey(text8))
										{
											if (File.Exists(text8))
											{
												PackageBuilder.ReferenceReport value9 = new PackageBuilder.ReferenceReport(text8, null, null, true, "FIXABLE: References local relative file " + text2 + " not included in package", true, null);
												this.referenceReports.Add(text8, value9);
											}
											else
											{
												PackageBuilder.ReferenceReport value10 = new PackageBuilder.ReferenceReport(text8, null, null, true, "BROKEN: References local relative file " + text2 + " not included in package. File is missing", true, null);
												this.referenceReports.Add(text8, value10);
											}
										}
									}
								}
							}
						}
						else
						{
							match = Regex.Match(text, "\\\"id\\\" : \\\"(.*)\\\"");
							if (match.Success)
							{
								string value11 = match.Groups[1].Value;
								string str2;
								if (vamIDsToPaths.TryGetValue(value11, out str2))
								{
									text = text.Replace(value11, "SELF:/" + str2);
								}
							}
							else
							{
								match = Regex.Match(text, "\\\"presetName\\\" : \\\"(.*)\\\"");
								if (match.Success)
								{
									string value12 = match.Groups[1].Value;
									if (value12.Contains(":"))
									{
										string text9 = Regex.Replace(value12, ":.*", string.Empty);
										string[] array2 = text9.Split(new char[]
										{
											'.'
										});
										if (array2.Length > 2)
										{
											string text10 = array2[0] + "." + array2[1];
											string str3 = array2[2];
											if (packageGroup == text10)
											{
												text = text.Replace(text9, "SELF");
											}
											else
											{
												string text11 = value12;
												string text12 = text9;
												VarPackage varPackage2 = FileManager.GetPackage(text9);
												VarPackageGroup packageGroup3 = FileManager.GetPackageGroup(text10);
												if (varPackage2 == null && packageGroup3 != null)
												{
													varPackage2 = packageGroup3.NewestPackage;
													str3 = varPackage2.Version.ToString();
												}
												bool flag3 = varPackage2 != null && varPackage2.HasMatchingDirectories("Custom/Scripts");
												VarPackage.ReferenceVersionOption referenceVersionOption2;
												if (flag3)
												{
													referenceVersionOption2 = this._scriptReferenceVersionOption;
												}
												else
												{
													referenceVersionOption2 = this._standardReferenceVersionOption;
												}
												if (referenceVersionOption2 == VarPackage.ReferenceVersionOption.Latest)
												{
													text12 = text10 + ".latest";
													text11 = value12.Replace(text9, text12);
													text = text.Replace(value12, text11);
													if (packageGroup3 != null)
													{
														varPackage2 = packageGroup3.NewestPackage;
													}
												}
												else if (referenceVersionOption2 == VarPackage.ReferenceVersionOption.Minimum)
												{
													text12 = text10 + ".min" + str3;
													text11 = value12.Replace(text9, text12);
													text = text.Replace(value12, text11);
												}
												if (!this.referenceReports.ContainsKey(text11))
												{
													if (varPackage2 == null)
													{
														PackageBuilder.ReferenceReport value13 = new PackageBuilder.ReferenceReport(value12, null, text12, true, "BROKEN: Missing package that is referenced", false, null);
														this.referenceReports.Add(text11, value13);
													}
													else
													{
														PackageBuilder.ReferenceReport value14 = new PackageBuilder.ReferenceReport(text11, varPackage2, text12, false, string.Empty, false, null);
														this.referenceReports.Add(text11, value14);
													}
												}
											}
										}
									}
								}
							}
						}
						streamWriter.WriteLine(text);
					}
				}
			}
			DateTime lastWriteTime = File.GetLastWriteTime(fromFile);
			File.SetLastWriteTime(toFile, lastWriteTime);
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x001FFD7C File Offset: 0x001FE17C
		protected void FindCslistRefs(string cslist, HashSet<string> internalFilesHashSet)
		{
			string directoryName = Path.GetDirectoryName(cslist);
			using (StreamReader streamReader = new StreamReader(cslist))
			{
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					string text2 = text.Trim();
					if (text2 != string.Empty)
					{
						if (directoryName != string.Empty)
						{
							text2 = directoryName + "/" + text2;
						}
						text2 = text2.Replace('\\', '/');
						if (!internalFilesHashSet.Contains(text2) && !this.referenceReports.ContainsKey(text2))
						{
							if (File.Exists(text2))
							{
								PackageBuilder.ReferenceReport value = new PackageBuilder.ReferenceReport(text2, null, null, true, "FIXABLE: References local relative file " + text2 + " not included in package", true, null);
								this.referenceReports.Add(text2, value);
							}
							else
							{
								PackageBuilder.ReferenceReport value2 = new PackageBuilder.ReferenceReport(text2, null, null, true, "BROKEN: References local relative file " + text2 + " not included in package. File is missing", true, null);
								this.referenceReports.Add(text2, value2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x001FFE90 File Offset: 0x001FE290
		protected void ClearReferenceItems()
		{
			foreach (PackageBuilderReferenceItem packageBuilderReferenceItem in this.referenceItems)
			{
				UnityEngine.Object.Destroy(packageBuilderReferenceItem.gameObject);
			}
			this.referenceItems.Clear();
			foreach (PackageBuilderReferenceItem packageBuilderReferenceItem2 in this.licenseReportItems)
			{
				UnityEngine.Object.Destroy(packageBuilderReferenceItem2.gameObject);
			}
			this.licenseReportItems.Clear();
			this.allReferencedPackages.Clear();
			this.allReferencedPackageUids.Clear();
			this.allPackagesDependencyTree = null;
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x001FFF74 File Offset: 0x001FE374
		protected void AddReferenceItem(PackageBuilder.ReferenceReport referenceReport)
		{
			RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.referencesItemPrefab);
			PackageBuilderReferenceItem component = rectTransform.GetComponent<PackageBuilderReferenceItem>();
			if (component != null)
			{
				rectTransform.SetParent(this.referencesContainer, false);
				component.Reference = referenceReport.Reference;
				if (referenceReport.HasIssue)
				{
					component.SetIssueColor(this.errorColor);
					component.Issue = referenceReport.Issue;
				}
				else
				{
					component.Issue = string.Empty;
				}
				this.referenceItems.Add(component);
			}
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x001FFFF8 File Offset: 0x001FE3F8
		protected void SyncReferences()
		{
			this.ClearReferenceItems();
			if (this.referenceReports != null && this.referencesItemPrefab != null && this.referencesContainer != null)
			{
				bool active = false;
				foreach (PackageBuilder.ReferenceReport referenceReport in this.referenceReports.Values)
				{
					if (referenceReport.HasIssue && !referenceReport.Fixable)
					{
						this.AddReferenceItem(referenceReport);
					}
				}
				foreach (PackageBuilder.ReferenceReport referenceReport2 in this.referenceReports.Values)
				{
					if (referenceReport2.HasIssue && referenceReport2.Fixable)
					{
						active = true;
						this.AddReferenceItem(referenceReport2);
					}
				}
				foreach (PackageBuilder.ReferenceReport referenceReport3 in this.referenceReports.Values)
				{
					if (!referenceReport3.HasIssue)
					{
						this.AddReferenceItem(referenceReport3);
					}
				}
				if (this.fixReferencesAction.button != null)
				{
					this.fixReferencesAction.button.gameObject.SetActive(active);
				}
			}
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x00200198 File Offset: 0x001FE598
		public static void GetPackageDependenciesRecursive(VarPackage currentPackage, string currentPackageUid, HashSet<string> visited, HashSet<VarPackage> allReferencedPackages, HashSet<string> allReferencedPackageUids, JSONClass jc)
		{
			visited.Add(currentPackageUid);
			allReferencedPackageUids.Add(currentPackageUid);
			JSONClass jsonclass = new JSONClass();
			jc[currentPackageUid] = jsonclass;
			if (currentPackage == null)
			{
				jsonclass["missing"].AsBool = true;
				jsonclass["licenseType"] = "MISSING";
			}
			else
			{
				jsonclass["licenseType"] = currentPackage.LicenseType;
			}
			JSONClass jsonclass2 = new JSONClass();
			jsonclass["dependencies"] = jsonclass2;
			if (currentPackage != null)
			{
				allReferencedPackages.Add(currentPackage);
				foreach (string text in currentPackage.PackageDependencies)
				{
					if (!visited.Contains(text))
					{
						VarPackage package = FileManager.GetPackage(text);
						PackageBuilder.GetPackageDependenciesRecursive(package, text, visited, allReferencedPackages, allReferencedPackageUids, jsonclass2);
					}
				}
			}
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x00200298 File Offset: 0x001FE698
		protected void BuildDependencyTree()
		{
			this.allPackagesDependencyTree = new JSONClass();
			this.allReferencedPackages = new HashSet<VarPackage>();
			this.allReferencedPackageUids = new HashSet<string>();
			HashSet<string> hashSet = new HashSet<string>();
			foreach (PackageBuilder.ReferenceReport referenceReport in this.referenceReports.Values)
			{
				if (referenceReport.PackageUid != null && !hashSet.Contains(referenceReport.PackageUid))
				{
					hashSet.Add(referenceReport.PackageUid);
					HashSet<string> visited = new HashSet<string>();
					PackageBuilder.GetPackageDependenciesRecursive(referenceReport.Package, referenceReport.PackageUid, visited, this.allReferencedPackages, this.allReferencedPackageUids, this.allPackagesDependencyTree);
				}
			}
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x0020036C File Offset: 0x001FE76C
		protected void AddLicenseReportItem(string packageId, string licenseType, Color c)
		{
			if (this.licenseReportContainer != null && this.referencesItemPrefab != null)
			{
				RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.referencesItemPrefab);
				PackageBuilderReferenceItem component = rectTransform.GetComponent<PackageBuilderReferenceItem>();
				if (component != null)
				{
					rectTransform.SetParent(this.licenseReportContainer, false);
					component.Reference = packageId;
					component.Issue = licenseType;
					component.SetIssueColor(c);
					this.licenseReportItems.Add(component);
				}
			}
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x002003E8 File Offset: 0x001FE7E8
		protected void SyncDependencyLicenseReport()
		{
			foreach (PackageBuilderReferenceItem packageBuilderReferenceItem in this.licenseReportItems)
			{
				UnityEngine.Object.Destroy(packageBuilderReferenceItem.gameObject);
			}
			this.licenseReportItems.Clear();
			this.licenseIssue = false;
			this.nonCommercialLicenseIssue = false;
			if (this.alreadyReported == null)
			{
				this.alreadyReported = new HashSet<string>();
			}
			else
			{
				this.alreadyReported.Clear();
			}
			if (this._licenseType == "PC" || this._licenseType == "PC EA")
			{
				foreach (VarPackage varPackage in this.allReferencedPackages)
				{
					for (int i = 0; i < this.licenseTypes.Length; i++)
					{
						if (varPackage.LicenseType == this.licenseTypes[i] && this.licenseNonCommercialFlags[i])
						{
							if (!(varPackage.LicenseType == "PC EA"))
							{
								this.licenseIssue = true;
								this.nonCommercialLicenseIssue = true;
								this.alreadyReported.Add(varPackage.Uid);
								this.AddLicenseReportItem(varPackage.Uid, varPackage.LicenseType, this.errorColor);
								break;
							}
							DateTime today = DateTime.Today;
							int year = today.Year;
							int month = today.Month;
							int day = today.Day;
							int num = this.MonthStringToMonthInt(varPackage.EAEndMonth);
							bool flag = false;
							int num2;
							int num3;
							if (num != 0 && int.TryParse(varPackage.EAEndYear, out num2) && int.TryParse(varPackage.EAEndDay, out num3))
							{
								if (year > num2)
								{
									flag = true;
								}
								else if (year == num2)
								{
									if (month > num)
									{
										flag = true;
									}
									else if (month == num && day > num3)
									{
										flag = true;
									}
								}
							}
							else
							{
								flag = true;
							}
							if (!flag)
							{
								this.licenseIssue = true;
								this.nonCommercialLicenseIssue = true;
								this.alreadyReported.Add(varPackage.Uid);
								this.AddLicenseReportItem(varPackage.Uid, varPackage.LicenseType, this.errorColor);
								break;
							}
							for (int j = 0; j < this.licenseTypes.Length; j++)
							{
								if (varPackage.SecondaryLicenseType == this.licenseTypes[j] && this.licenseNonCommercialFlags[j])
								{
									this.licenseIssue = true;
									this.nonCommercialLicenseIssue = true;
									this.alreadyReported.Add(varPackage.Uid);
									this.AddLicenseReportItem(varPackage.Uid, varPackage.LicenseType, this.errorColor);
									break;
								}
							}
						}
					}
				}
			}
			foreach (VarPackage varPackage2 in this.allReferencedPackages)
			{
				if (!this.alreadyReported.Contains(varPackage2.Uid))
				{
					for (int k = 0; k < this.licenseTypes.Length; k++)
					{
						if (varPackage2.LicenseType == this.licenseTypes[k] && !this.licenseDistributableFlags[k])
						{
							if (!(varPackage2.LicenseType == "PC EA"))
							{
								this.licenseIssue = true;
								this.alreadyReported.Add(varPackage2.Uid);
								this.AddLicenseReportItem(varPackage2.Uid, varPackage2.LicenseType, this.errorColor);
								break;
							}
							DateTime today2 = DateTime.Today;
							int year2 = today2.Year;
							int month2 = today2.Month;
							int day2 = today2.Day;
							int num4 = this.MonthStringToMonthInt(varPackage2.EAEndMonth);
							bool flag2 = false;
							int num5;
							int num6;
							if (num4 != 0 && int.TryParse(varPackage2.EAEndYear, out num5) && int.TryParse(varPackage2.EAEndDay, out num6))
							{
								if (year2 > num5)
								{
									flag2 = true;
								}
								else if (year2 == num5)
								{
									if (month2 > num4)
									{
										flag2 = true;
									}
									else if (month2 == num4 && day2 > num6)
									{
										flag2 = true;
									}
								}
							}
							else
							{
								flag2 = true;
							}
							if (!flag2)
							{
								this.licenseIssue = true;
								this.alreadyReported.Add(varPackage2.Uid);
								this.AddLicenseReportItem(varPackage2.Uid, varPackage2.LicenseType, this.errorColor);
								break;
							}
							bool flag3 = true;
							for (int l = 0; l < this.licenseTypes.Length; l++)
							{
								if (varPackage2.SecondaryLicenseType == this.licenseTypes[l] && !this.licenseDistributableFlags[l])
								{
									this.licenseIssue = true;
									this.alreadyReported.Add(varPackage2.Uid);
									this.AddLicenseReportItem(varPackage2.Uid, varPackage2.LicenseType, this.errorColor);
									flag3 = false;
									break;
								}
							}
							if (flag3)
							{
								this.alreadyReported.Add(varPackage2.Uid);
								this.AddLicenseReportItem(varPackage2.Uid, varPackage2.SecondaryLicenseType, this.normalColor);
							}
						}
					}
				}
			}
			foreach (VarPackage varPackage3 in this.allReferencedPackages)
			{
				if (!this.alreadyReported.Contains(varPackage3.Uid))
				{
					bool flag4 = false;
					for (int m = 0; m < this.licenseTypes.Length; m++)
					{
						if (varPackage3.LicenseType == this.licenseTypes[m])
						{
							flag4 = true;
							break;
						}
					}
					if (!flag4)
					{
						this.alreadyReported.Add(varPackage3.Uid);
						this.AddLicenseReportItem(varPackage3.Uid, varPackage3.LicenseType, this.warningColor);
					}
				}
			}
			foreach (VarPackage varPackage4 in this.allReferencedPackages)
			{
				if (!this.alreadyReported.Contains(varPackage4.Uid))
				{
					for (int n = 0; n < this.licenseTypes.Length; n++)
					{
						if (varPackage4.LicenseType == this.licenseTypes[n] && this.licenseDistributableFlags[n])
						{
							this.AddLicenseReportItem(varPackage4.Uid, varPackage4.LicenseType, this.normalColor);
							break;
						}
					}
				}
			}
			if (this.licenseReportIssueText != null)
			{
				this.licenseReportIssueText.gameObject.SetActive(this.licenseIssue && !this.nonCommercialLicenseIssue);
			}
			if (this.nonCommercialLicenseReportIssueText != null)
			{
				this.nonCommercialLicenseReportIssueText.gameObject.SetActive(this.nonCommercialLicenseIssue);
			}
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x00200BCC File Offset: 0x001FEFCC
		public void PrepPackage()
		{
			this.ClearStatus();
			string creatorName = UserPreferences.singleton.creatorName;
			if (creatorName == "Anonymous")
			{
				this.ShowErrorStatus("Creator name cannot be Anonymous. Please set your creator name in User Preferences.");
				return;
			}
			if (this._packageName == null || this._packageName == string.Empty)
			{
				this.ShowErrorStatus("Package name is not set " + this._packageName);
				return;
			}
			if (this.contentItems.Keys.Count == 0)
			{
				this.ShowErrorStatus("No files or directory contents selected");
				return;
			}
			string text = FileManager.PackageFolder;
			text = text.TrimEnd(new char[]
			{
				'/',
				'\\'
			});
			text += "Builder/";
			int num = 1;
			string text2 = creatorName + "." + this._packageName;
			VarPackageGroup packageGroup = FileManager.GetPackageGroup(text2);
			if (packageGroup != null)
			{
				num = packageGroup.NewestVersion + 1;
			}
			this.Version = num;
			try
			{
				this._preppedDir = string.Concat(new object[]
				{
					text,
					creatorName,
					".",
					this._packageName,
					".",
					num,
					".var"
				});
				if (File.Exists(this._preppedDir))
				{
					this.ShowErrorStatus("Unexpected error during prep. File " + this._preppedDir + " already exists ");
				}
				else
				{
					if (Directory.Exists(this._preppedDir))
					{
						Directory.Delete(this._preppedDir, true);
					}
					Directory.CreateDirectory(this._preppedDir);
					HashSet<string> hashSet = new HashSet<string>();
					foreach (string text3 in this.contentItems.Keys)
					{
						if (Directory.Exists(text3))
						{
							string[] files = Directory.GetFiles(text3, "*", SearchOption.AllDirectories);
							foreach (string text4 in files)
							{
								string item = text4.Replace('\\', '/');
								hashSet.Add(item);
							}
						}
						else
						{
							if (!File.Exists(text3))
							{
								this.ShowErrorStatus("Unexpected error during prep. Missing content item " + text3);
								return;
							}
							hashSet.Add(text3);
						}
					}
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					foreach (string text5 in hashSet)
					{
						if (text5.EndsWith(".vam"))
						{
							using (StreamReader streamReader = new StreamReader(text5))
							{
								string aJSON = streamReader.ReadToEnd();
								JSONClass asObject = JSON.Parse(aJSON).AsObject;
								if (asObject["uid"] != null)
								{
									string key = asObject["uid"];
									if (!dictionary.ContainsKey(key))
									{
										dictionary.Add(key, text5);
									}
								}
							}
						}
					}
					this.referenceReports.Clear();
					foreach (string text6 in hashSet)
					{
						if (!text6.EndsWith(".fav"))
						{
							string text7 = this._preppedDir + "/" + text6;
							string directoryName = Path.GetDirectoryName(text7);
							if (!Directory.Exists(directoryName))
							{
								Directory.CreateDirectory(directoryName);
							}
							if (Regex.IsMatch(text6, "\\.(json|vap|vaj)$"))
							{
								this.CopyAndFixRefs(text2, text6, text7, hashSet, dictionary);
							}
							else if (Regex.IsMatch(text6, "\\.cslist$"))
							{
								this.FindCslistRefs(text6, hashSet);
								File.Copy(text6, text7, true);
							}
							else
							{
								File.Copy(text6, text7, true);
							}
						}
					}
					this.SyncReferences();
					this.BuildDependencyTree();
					this.SyncDependencyLicenseReport();
					this.NeedsPrep = false;
					this.ShowStatus("Prep complete. Please complete forms and finalization.");
				}
			}
			catch (Exception arg)
			{
				this.ShowErrorStatus("Exception during prep");
				SuperController.LogError("Exception during package prep: " + arg);
			}
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x00201098 File Offset: 0x001FF498
		protected void FixReferences()
		{
			if (this.referenceReports != null)
			{
				foreach (PackageBuilder.ReferenceReport referenceReport in this.referenceReports.Values)
				{
					if (referenceReport.HasIssue && referenceReport.Fixable)
					{
						if (referenceReport.FixedReference != null)
						{
							this.AddContentItem(referenceReport.FixedReference);
						}
						else
						{
							this.AddContentItem(referenceReport.Reference);
						}
					}
				}
			}
			this.PrepPackage();
		}

		// Token: 0x06005770 RID: 22384 RVA: 0x00201144 File Offset: 0x001FF544
		protected void SyncDescription(string s)
		{
			this._description = s;
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06005771 RID: 22385 RVA: 0x0020114D File Offset: 0x001FF54D
		// (set) Token: 0x06005772 RID: 22386 RVA: 0x00201155 File Offset: 0x001FF555
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this.descriptionJSON.val = value;
			}
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x00201163 File Offset: 0x001FF563
		protected void SyncCredits(string s)
		{
			this._credits = s;
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06005774 RID: 22388 RVA: 0x0020116C File Offset: 0x001FF56C
		// (set) Token: 0x06005775 RID: 22389 RVA: 0x00201174 File Offset: 0x001FF574
		public string Credits
		{
			get
			{
				return this._credits;
			}
			set
			{
				this.creditsJSON.val = value;
			}
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x00201182 File Offset: 0x001FF582
		protected void SyncInstructions(string s)
		{
			this._instructions = s;
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06005777 RID: 22391 RVA: 0x0020118B File Offset: 0x001FF58B
		// (set) Token: 0x06005778 RID: 22392 RVA: 0x00201193 File Offset: 0x001FF593
		public string Instructions
		{
			get
			{
				return this._instructions;
			}
			set
			{
				this.instructionsJSON.val = value;
			}
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x002011A4 File Offset: 0x001FF5A4
		protected void SyncPromotionalLink(string s)
		{
			this._promotionalLink = s;
			if (this.promotionalButton != null)
			{
				if (!SuperController.singleton.promotionalDisabled && s != null && s != string.Empty)
				{
					this.promotionalButton.gameObject.SetActive(true);
				}
				else
				{
					this.promotionalButton.gameObject.SetActive(false);
				}
			}
			if (this.promotionalButtonText != null)
			{
				this.promotionalButtonText.text = s;
			}
			if (this.copyPromotionalLinkAction != null && this.copyPromotionalLinkAction.button != null)
			{
				if (!SuperController.singleton.promotionalDisabled && s != null && s != string.Empty)
				{
					this.copyPromotionalLinkAction.button.gameObject.SetActive(true);
				}
				else
				{
					this.copyPromotionalLinkAction.button.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x0600577A RID: 22394 RVA: 0x002012A9 File Offset: 0x001FF6A9
		// (set) Token: 0x0600577B RID: 22395 RVA: 0x002012B1 File Offset: 0x001FF6B1
		public string PromotionalLink
		{
			get
			{
				return this._promotionalLink;
			}
			set
			{
				this.promotionalLinkJSON.val = value;
			}
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x002012C0 File Offset: 0x001FF6C0
		protected void SyncLicenseDescriptionText()
		{
			bool flag = this._licenseType == "PC EA";
			if (this.licenseDescriptionText != null)
			{
				bool flag2 = false;
				for (int i = 0; i < this.licenseTypes.Length; i++)
				{
					if (this._licenseType == this.licenseTypes[i])
					{
						this.licenseDescriptionText.text = this.licenseDescriptions[i];
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					this.licenseDescriptionText.text = "Unknown license type";
				}
				if (flag)
				{
					flag2 = false;
					for (int j = 0; j < this.licenseTypes.Length; j++)
					{
						if (this._secondaryLicenseType == this.licenseTypes[j])
						{
							Text text = this.licenseDescriptionText;
							text.text = text.text + "\nSecondary license: " + this.licenseDescriptions[j];
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						Text text2 = this.licenseDescriptionText;
						text2.text += "\nSecondary license: Unknown license type";
					}
				}
			}
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x002013D8 File Offset: 0x001FF7D8
		protected void SyncLicenseType(string s)
		{
			this._licenseType = s;
			this.SyncLicenseDescriptionText();
			bool active = this._licenseType == "PC EA";
			if (this.secondaryLicenseTypeJSON != null && this.secondaryLicenseTypeJSON.popup != null)
			{
				this.secondaryLicenseTypeJSON.popup.gameObject.SetActive(active);
			}
			if (this.EAYearJSON != null && this.EAYearJSON.popup != null)
			{
				this.EAYearJSON.popup.gameObject.SetActive(active);
			}
			if (this.EAMonthJSON != null && this.EAMonthJSON.popup != null)
			{
				this.EAMonthJSON.popup.gameObject.SetActive(active);
			}
			if (this.EADayJSON != null && this.EADayJSON.popup != null)
			{
				this.EADayJSON.popup.gameObject.SetActive(active);
			}
			this.SyncDependencyLicenseReport();
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x0600577E RID: 22398 RVA: 0x002014E5 File Offset: 0x001FF8E5
		// (set) Token: 0x0600577F RID: 22399 RVA: 0x002014ED File Offset: 0x001FF8ED
		public string LicenseType
		{
			get
			{
				return this._licenseType;
			}
			set
			{
				this.licenseTypeJSON.val = value;
			}
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x002014FB File Offset: 0x001FF8FB
		protected void SyncSecondaryLicenseType(string s)
		{
			this._secondaryLicenseType = s;
			this.SyncLicenseDescriptionText();
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06005781 RID: 22401 RVA: 0x0020150A File Offset: 0x001FF90A
		// (set) Token: 0x06005782 RID: 22402 RVA: 0x00201512 File Offset: 0x001FF912
		public string SecondaryLicenseType
		{
			get
			{
				return this._secondaryLicenseType;
			}
			set
			{
				this.secondaryLicenseTypeJSON.val = value;
			}
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x00201520 File Offset: 0x001FF920
		protected int MonthStringToMonthInt(string monthString)
		{
			switch (monthString)
			{
			case "Jan":
				return 1;
			case "Feb":
				return 2;
			case "Mar":
				return 3;
			case "Apr":
				return 4;
			case "May":
				return 5;
			case "Jun":
				return 6;
			case "Jul":
				return 7;
			case "Aug":
				return 8;
			case "Sep":
				return 9;
			case "Oct":
				return 10;
			case "Nov":
				return 11;
			case "Dec":
				return 12;
			}
			return 0;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x00201648 File Offset: 0x001FFA48
		public void OpenPrepFolderInExplorer()
		{
			SuperController.singleton.OpenFolderInExplorer(this._preppedDir);
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x0020165C File Offset: 0x001FFA5C
		protected void ProcessFileMethod(object sender, ScanEventArgs args)
		{
			this.finalizeFileProgressCount++;
			if (this.finalizeTotalFileCount != 0)
			{
				this.finalizeProgress = (float)this.finalizeFileProgressCount / (float)this.finalizeTotalFileCount;
			}
			if (this.finalizeThreadAbort)
			{
				args.ContinueRunning = false;
			}
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x002016AC File Offset: 0x001FFAAC
		protected void FinalizePackageThreaded()
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(this._preppedDir + "/meta.json"))
				{
					JSONClass jsonclass = new JSONClass();
					this.licenseTypeJSON.StoreJSON(jsonclass, true, true, true);
					if (this.licenseTypeJSON.val == "PC EA")
					{
						this.EAYearJSON.StoreJSON(jsonclass, true, true, true);
						this.EAMonthJSON.StoreJSON(jsonclass, true, true, true);
						this.EADayJSON.StoreJSON(jsonclass, true, true, true);
						this.secondaryLicenseTypeJSON.StoreJSON(jsonclass, true, true, true);
					}
					this.creatorNameJSON.StoreJSON(jsonclass, true, true, true);
					this.packageNameJSON.StoreJSON(jsonclass, true, true, true);
					this.standardReferenceVersionOptionJSON.StoreJSON(jsonclass, true, true, true);
					this.scriptReferenceVersionOptionJSON.StoreJSON(jsonclass, true, true, true);
					this.descriptionJSON.StoreJSON(jsonclass, true, true, true);
					this.creditsJSON.StoreJSON(jsonclass, true, true, true);
					this.instructionsJSON.StoreJSON(jsonclass, true, true, true);
					this.promotionalLinkJSON.StoreJSON(jsonclass, true, true, true);
					jsonclass["programVersion"] = SuperController.singleton.GetVersion();
					JSONArray jsonarray = new JSONArray();
					jsonclass["contentList"] = jsonarray;
					foreach (string s in this.contentItems.Keys)
					{
						jsonarray.Add(s);
					}
					jsonclass["dependencies"] = this.allPackagesDependencyTree;
					JSONClass jsonclass2 = new JSONClass();
					jsonclass["customOptions"] = jsonclass2;
					foreach (VarPackageCustomOption varPackageCustomOption in this.customOptions)
					{
						varPackageCustomOption.StoreJSON(jsonclass2);
					}
					bool asBool = false;
					JSONArray jsonarray2 = new JSONArray();
					foreach (PackageBuilder.ReferenceReport referenceReport in this.referenceReports.Values)
					{
						if (referenceReport.HasIssue)
						{
							asBool = true;
							JSONClass jsonclass3 = new JSONClass();
							jsonclass3["reference"] = referenceReport.Reference;
							jsonclass3["issue"] = referenceReport.Issue;
							jsonarray2.Add(jsonclass3);
						}
					}
					jsonclass["hadReferenceIssues"].AsBool = asBool;
					jsonclass["referenceIssues"] = jsonarray2;
					streamWriter.Write(jsonclass.ToString(string.Empty));
				}
				string text = FileManager.PackageFolder + "/" + Path.GetFileName(this._preppedDir);
				using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(text)))
				{
					string[] files = Directory.GetFiles(this._preppedDir, "*", SearchOption.AllDirectories);
					byte[] buffer = new byte[32768];
					foreach (string text2 in files)
					{
						string text3 = Regex.Replace(text2, "\\\\", "/");
						text3 = Regex.Replace(text3, "^" + Regex.Escape(this._preppedDir) + "/", string.Empty);
						ZipEntry zipEntry = new ZipEntry(text3);
						zipEntry.IsUnicodeText = true;
						string extension = Path.GetExtension(text2);
						if (Regex.IsMatch(extension, "(mp3|ogg|wav|jpg|jpeg|png|gif|tif|tiff|avi|mp4|assetbundle|scene|vac|zip)", RegexOptions.IgnoreCase))
						{
							zipOutputStream.SetLevel(0);
						}
						else
						{
							zipOutputStream.SetLevel(9);
						}
						zipOutputStream.PutNextEntry(zipEntry);
						using (FileEntryStream fileEntryStream = FileManager.OpenStream(text2, false))
						{
							StreamUtils.Copy(fileEntryStream.Stream, zipOutputStream, buffer);
						}
						zipEntry.DateTime = File.GetLastWriteTime(text2);
						zipOutputStream.CloseEntry();
						this.finalizeFileProgressCount++;
						if (this.finalizeTotalFileCount != 0)
						{
							this.finalizeProgress = (float)this.finalizeFileProgressCount / (float)this.finalizeTotalFileCount;
						}
						if (this.finalizeThreadAbort)
						{
							return;
						}
					}
				}
				if (this.allReferencedPackageUids.Count > 0)
				{
					using (StreamWriter streamWriter2 = new StreamWriter(text + ".depend.txt"))
					{
						List<string> list = this.allReferencedPackageUids.ToList<string>();
						list.Sort();
						foreach (string text4 in list)
						{
							VarPackage package = FileManager.GetPackage(text4);
							if (package != null)
							{
								string text5 = text4.PadRight(45);
								string text6 = " By: " + package.Creator;
								text5 += text6.PadRight(25);
								string text7 = " License: " + package.LicenseType;
								text5 += text7.PadRight(25);
								if (package.PromotionalLink != null && package.PromotionalLink != string.Empty)
								{
									text5 = text5 + " Link: " + package.PromotionalLink;
								}
								streamWriter2.WriteLine(text5);
							}
							else
							{
								UnityEngine.Debug.LogError("Could not find referenced pacakge with id " + text4);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.finalizeThreadError = ex.ToString();
			}
		}

		// Token: 0x06005787 RID: 22407 RVA: 0x00201D08 File Offset: 0x00200108
		protected void AbortFinalizePackageThreaded(bool wait = true)
		{
			if (this.finalizeThread != null && this.finalizeThread.IsAlive)
			{
				this.finalizeThreadAbort = true;
				if (wait)
				{
					while (this.finalizeThread.IsAlive)
					{
						Thread.Sleep(0);
					}
				}
				this.finalizeThread = null;
			}
		}

		// Token: 0x06005788 RID: 22408 RVA: 0x00201D5F File Offset: 0x0020015F
		protected void FinalizeComplete()
		{
			this.finalizeFlag.Raise();
			if (this.finalizingPanel != null)
			{
				this.finalizingPanel.gameObject.SetActive(false);
			}
			this.isFinalizing = false;
		}

		// Token: 0x06005789 RID: 22409 RVA: 0x00201D98 File Offset: 0x00200198
		protected IEnumerator FinalizePackageCo()
		{
			this.finalizeFlag = new AsyncFlag("FinalizePackage");
			SuperController.singleton.SetLoadingIconFlag(this.finalizeFlag);
			if (this.finalizingPanel != null)
			{
				this.finalizingPanel.gameObject.SetActive(true);
			}
			yield return null;
			this.finalizeThreadError = null;
			this.finalizeThreadAbort = false;
			this.finalizeProgress = 0f;
			this.finalizeFileProgressCount = 0;
			if (this.finalizeProgressSlider != null)
			{
				this.finalizeProgressSlider.value = 0f;
			}
			this.finalizeTotalFileCount = FileManager.FolderContentsCount(this._preppedDir);
			this.finalizeThread = new Thread(new ThreadStart(this.FinalizePackageThreaded));
			this.finalizeThread.Start();
			while (this.finalizeThread.IsAlive)
			{
				yield return null;
				if (this.finalizeProgressSlider != null)
				{
					this.finalizeProgressSlider.value = this.finalizeProgress;
				}
			}
			this.finalizeThread = null;
			if (this.finalizeThreadError != null)
			{
				this.ShowErrorStatus("Exception during finalize");
				SuperController.LogError("Exception during package finalize: " + this.finalizeThreadError);
			}
			else
			{
				FileManager.Refresh();
				this.NeedsPrep = true;
				this.ShowStatus("Package complete and ready for use");
			}
			this.FinalizeComplete();
			yield break;
		}

		// Token: 0x0600578A RID: 22410 RVA: 0x00201DB4 File Offset: 0x002001B4
		protected void FinalizeCheckConfirm()
		{
			if (this.finalizeCheckPanel != null)
			{
				this.finalizeCheckPanel.gameObject.SetActive(false);
			}
			if (!this.NeedsPrep && !this.isFinalizing)
			{
				this.isFinalizing = true;
				base.StartCoroutine(this.FinalizePackageCo());
			}
		}

		// Token: 0x0600578B RID: 22411 RVA: 0x00201E0D File Offset: 0x0020020D
		protected void FinalizeCheckCancel()
		{
			if (this.finalizeCheckPanel != null)
			{
				this.finalizeCheckPanel.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600578C RID: 22412 RVA: 0x00201E31 File Offset: 0x00200231
		protected void FinalizeCheck()
		{
			if (this.finalizeCheckPanel != null)
			{
				this.finalizeCheckPanel.gameObject.SetActive(true);
			}
			else
			{
				this.FinalizeCheckConfirm();
			}
		}

		// Token: 0x0600578D RID: 22413 RVA: 0x00201E60 File Offset: 0x00200260
		public void FinalizePackage()
		{
			if (!this.NeedsPrep && !this.isFinalizing)
			{
				bool flag = false;
				foreach (PackageBuilder.ReferenceReport referenceReport in this.referenceReports.Values)
				{
					if (referenceReport.HasIssue)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.FinalizeCheck();
				}
				else if (this.licenseIssue || this.nonCommercialLicenseIssue)
				{
					string text = string.Empty;
					if (this.licenseIssue)
					{
						text += "Your package has references to other packages that are not re-distributable (PC, PC EA, or Questionable). These references may make it harder for those using your package to use it correctly and your var package may not be designated as Hub Hosted if uploaded to the Hub. Click Cancel if you want to go back and review or fix.\n\n";
					}
					if (this.nonCommercialLicenseIssue)
					{
						text += "Your package is marked as PC or PC EA (Paid Content) and has references to non-commercial (NC) licensed packages. You are likely in violation of the licenses unless you are the creator of the other packages or have special permission. Click Cancel to go back and review reference licenses in the Reference License Report section. Only click OK if you have explicit permission.";
					}
					SuperController.AlertUser(text, new UnityAction(this.FinalizeCheckConfirm), new UnityAction(this.FinalizeCheckCancel), SuperController.DisplayUIChoice.Auto);
				}
				else
				{
					this.FinalizeCheckConfirm();
				}
			}
		}

		// Token: 0x0600578E RID: 22414 RVA: 0x00201F6C File Offset: 0x0020036C
		protected void CancelFinalize()
		{
			if (this.isFinalizing)
			{
				base.StopAllCoroutines();
				this.AbortFinalizePackageThreaded(true);
				string path = FileManager.PackageFolder + "/" + Path.GetFileName(this._preppedDir);
				FileManager.DeleteFile(path);
				this.ShowErrorStatus("Finalize aborted");
				this.FinalizeComplete();
			}
		}

		// Token: 0x0600578F RID: 22415 RVA: 0x00201FC4 File Offset: 0x002003C4
		protected void Update()
		{
			if (!this.isManager)
			{
				string val = this.FixNameToBeValidPathName(UserPreferences.singleton.creatorName);
				this.creatorNameJSON.val = val;
			}
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x00201FFC File Offset: 0x002003FC
		protected void Init()
		{
			this.shortCuts = new List<ShortCut>();
			ShortCut shortCut = new ShortCut();
			shortCut.displayName = "Saves";
			shortCut.path = "Saves";
			this.shortCuts.Add(shortCut);
			shortCut = new ShortCut();
			shortCut.displayName = "Saves\\scene";
			shortCut.path = "Saves\\scene";
			this.shortCuts.Add(shortCut);
			shortCut = new ShortCut();
			shortCut.displayName = "Custom";
			shortCut.path = "Custom";
			this.shortCuts.Add(shortCut);
			shortCut = new ShortCut();
			shortCut.displayName = "Custom\\Atom\\Person";
			shortCut.path = "Custom\\Atom\\Person";
			this.shortCuts.Add(shortCut);
			this.clearAllJSON = new JSONStorableAction("ClearAll", new JSONStorableAction.ActionCallback(this.ClearAll));
			base.RegisterAction(this.clearAllJSON);
			this.loadMetaFromExistingPackageJSON = new JSONStorableAction("LoadMetaFromExistingPackage", new JSONStorableAction.ActionCallback(this.LoadMetaFromExistingPackage));
			base.RegisterAction(this.loadMetaFromExistingPackageJSON);
			if (this.isManager)
			{
				this.packageItems = new Dictionary<string, PackageBuilderPackageItem>();
				this.missingPackageItems = new Dictionary<string, PackageBuilderPackageItem>();
				this.packageReferenceItems = new List<PackageBuilderPackageItem>();
				this.scanHubForMissingPackagesAction = new JSONStorableAction("ScanHubForMissingPackages", new JSONStorableAction.ActionCallback(this.ScanHubForMissingPackages));
				base.RegisterAction(this.scanHubForMissingPackagesAction);
				this.selectCurrentScenePackageAction = new JSONStorableAction("SelectCurrentScenePackage", new JSONStorableAction.ActionCallback(this.SelectCurrentScenePackage));
				base.RegisterAction(this.selectCurrentScenePackageAction);
				this.showDisabledJSON = new JSONStorableBool("showDisabled", false, new JSONStorableBool.SetBoolCallback(this.SyncShowDisabled));
				base.RegisterBool(this.showDisabledJSON);
				List<string> list = new List<string>();
				foreach (PackageBuilder.CategoryFilter categoryFilter in this.categoryFilters)
				{
					list.Add(categoryFilter.name);
				}
				this.categoryJSON = new JSONStorableStringChooser("category", list, "All", "Category", new JSONStorableStringChooser.SetStringCallback(this.CategoryChangeCallback));
				base.RegisterStringChooser(this.categoryJSON);
				this.goToPromotionalLinkAction = new JSONStorableAction("GoToPromotionalLink", new JSONStorableAction.ActionCallback(this.GoToPromotionalLink));
				this.copyPromotionalLinkAction = new JSONStorableAction("CopyPromotionalLink", new JSONStorableAction.ActionCallback(this.CopyPromotionalLink));
				this.currentPackageIsOnHubJSON = new JSONStorableBool("currentPackageIsOnHub", false);
				this.openOnHubAction = new JSONStorableAction("OpenOnHub", new JSONStorableAction.ActionCallback(this.OpenOnHub));
				this.openInHubDownloaderAction = new JSONStorableAction("OpenInHubDownloader", new JSONStorableAction.ActionCallback(this.OpenInHubDownloader));
				this.currentPackageHasSceneJSON = new JSONStorableBool("currentPackageHasScene", false);
				this.openSceneAction = new JSONStorableAction("OpenScene", new JSONStorableAction.ActionCallback(this.OpenScene));
				FileManager.RegisterRefreshHandler(new OnRefresh(this.SyncPackages));
				this.packageEnabledJSON = new JSONStorableBool("packageEnabled", true, new JSONStorableBool.SetBoolCallback(this.SyncPackageEnabled));
				this.deletePackageAction = new JSONStorableAction("DeletePackage", new JSONStorableAction.ActionCallback(this.DeletePackage));
				this.confirmDeletePackageAction = new JSONStorableAction("ConfirmDeletePackage", new JSONStorableAction.ActionCallback(this.ConfirmDeletePackage));
				this.cancelDeletePackageAction = new JSONStorableAction("CancelDeletePackage", new JSONStorableAction.ActionCallback(this.CancelDeletePackage));
				this.hadReferenceIssuesJSON = new JSONStorableBool("hadReferenceIssues", false);
				this.userNotesJSON = new JSONStorableString("userNotes", string.Empty, new JSONStorableString.SetStringCallback(this.SyncUserNotes));
				this.pluginsAlwaysEnabledJSON = new JSONStorableBool("pluginsAlwaysEnabled", false, new JSONStorableBool.SetBoolCallback(this.SyncPluginsAlwaysEnabled));
				this.pluginsAlwaysDisabledJSON = new JSONStorableBool("pluginsAlwaysDisabled", false, new JSONStorableBool.SetBoolCallback(this.SyncPluginsAlwaysDisabled));
				this.ignoreMissingDependencyErrorsJSON = new JSONStorableBool("ignoreMissingDependencyErrors", false, new JSONStorableBool.SetBoolCallback(this.SyncIgnoreMissingDependencyErrors));
				this.unpackAction = new JSONStorableAction("Unpack", new JSONStorableAction.ActionCallback(this.Unpack));
				this.confirmUnpackAction = new JSONStorableAction("ConfirmUnpack", new JSONStorableAction.ActionCallback(this.ConfirmUnpack));
				this.cancelUnpackAction = new JSONStorableAction("CancelUnpack", new JSONStorableAction.ActionCallback(this.CancelUnpack));
				this.repackAction = new JSONStorableAction("Repack", new JSONStorableAction.ActionCallback(this.Repack));
				this.restoreFromOriginalAction = new JSONStorableAction("RestoreFromOriginal", new JSONStorableAction.ActionCallback(this.RestoreFromOriginal));
				this.confirmRestoreFromOriginalAction = new JSONStorableAction("ConfirmRestoreFromOriginal", new JSONStorableAction.ActionCallback(this.ConfirmRestoreFromOriginal));
				this.cancelRestoreFromOriginalAction = new JSONStorableAction("CancelRestoreFromOriginal", new JSONStorableAction.ActionCallback(this.CancelRestoreFromOriginal));
			}
			this.creatorNameJSON = new JSONStorableString("creatorName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncCreatorName));
			this.creatorNameJSON.interactable = false;
			this.packageNameJSON = new JSONStorableString("packageName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPackageName));
			this.packageNameJSON.enableOnChange = true;
			if (this.isManager)
			{
				this.packageNameJSON.interactable = false;
			}
			base.RegisterString(this.packageNameJSON);
			foreach (VarPackageCustomOption varPackageCustomOption in this.customOptions)
			{
				varPackageCustomOption.Init(new JSONStorableBool.SetJSONBoolCallback(this.CustomOptionChange));
			}
			this.contentItems = new Dictionary<string, PackageBuilderContentItem>();
			this.addDirectoryAction = new JSONStorableAction("AddDirectory", new JSONStorableAction.ActionCallback(this.AddDirectory));
			base.RegisterAction(this.addDirectoryAction);
			this.addFileAction = new JSONStorableAction("AddFile", new JSONStorableAction.ActionCallback(this.AddFile));
			base.RegisterAction(this.addFileAction);
			this.removeSelectedAction = new JSONStorableAction("RemoveSelected", new JSONStorableAction.ActionCallback(this.RemoveSelected));
			base.RegisterAction(this.removeSelectedAction);
			this.removeAllAction = new JSONStorableAction("RemoveAll", new JSONStorableAction.ActionCallback(this.ClearContentItems));
			base.RegisterAction(this.removeAllAction);
			this.referenceReports = new Dictionary<string, PackageBuilder.ReferenceReport>();
			this.referenceItems = new List<PackageBuilderReferenceItem>();
			this.licenseReportItems = new List<PackageBuilderReferenceItem>();
			this.allReferencedPackages = new HashSet<VarPackage>();
			this.allReferencedPackageUids = new HashSet<string>();
			List<string> list2 = new List<string>();
			list2.Add("Latest");
			list2.Add("Minimum");
			list2.Add("Exact");
			this.standardReferenceVersionOptionJSON = new JSONStorableStringChooser("standardReferenceVersionOption", list2, new List<string>
			{
				"Latest (Recommended)",
				"Minimum",
				"Exact"
			}, "Latest", "Standard reference version option", new JSONStorableStringChooser.SetStringCallback(this.SyncStandardReferenceVersionOption));
			base.RegisterStringChooser(this.standardReferenceVersionOptionJSON);
			this.scriptReferenceVersionOptionJSON = new JSONStorableStringChooser("scriptReferenceVersionOption", list2, new List<string>
			{
				"Latest",
				"MinVersion",
				"Exact (Recommended)"
			}, "Exact", "Script reference version option", new JSONStorableStringChooser.SetStringCallback(this.SyncScriptReferenceVersionOption));
			base.RegisterStringChooser(this.scriptReferenceVersionOptionJSON);
			if (!this.isManager)
			{
				this.prepPackageAction = new JSONStorableAction("PrepPackage", new JSONStorableAction.ActionCallback(this.PrepPackage));
				base.RegisterAction(this.prepPackageAction);
				this.fixReferencesAction = new JSONStorableAction("FixReferences", new JSONStorableAction.ActionCallback(this.FixReferences));
				base.RegisterAction(this.fixReferencesAction);
			}
			this.descriptionJSON = new JSONStorableString("description", string.Empty, new JSONStorableString.SetStringCallback(this.SyncDescription));
			base.RegisterString(this.descriptionJSON);
			this.creditsJSON = new JSONStorableString("credits", string.Empty, new JSONStorableString.SetStringCallback(this.SyncCredits));
			base.RegisterString(this.creditsJSON);
			this.instructionsJSON = new JSONStorableString("instructions", string.Empty, new JSONStorableString.SetStringCallback(this.SyncInstructions));
			base.RegisterString(this.instructionsJSON);
			if (this.isManager)
			{
				this.descriptionJSON.interactable = false;
				this.creditsJSON.interactable = false;
				this.instructionsJSON.interactable = false;
			}
			this.promotionalLinkJSON = new JSONStorableString("promotionalLink", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPromotionalLink));
			base.RegisterString(this.promotionalLinkJSON);
			List<string> choicesList = new List<string>(this.licenseTypes);
			this.licenseTypeJSON = new JSONStorableStringChooser("licenseType", choicesList, this._licenseType, "License Type", new JSONStorableStringChooser.SetStringCallback(this.SyncLicenseType));
			base.RegisterStringChooser(this.licenseTypeJSON);
			List<string> choicesList2 = new List<string>(this.yearChoices);
			List<string> choicesList3 = new List<string>(this.monthChoices);
			List<string> list3 = new List<string>();
			for (int k = 1; k <= 31; k++)
			{
				list3.Add(k.ToString());
			}
			this.EAYearJSON = new JSONStorableStringChooser("EAEndYear", choicesList2, "2020", "EA Ends");
			base.RegisterStringChooser(this.EAYearJSON);
			this.EAMonthJSON = new JSONStorableStringChooser("EAEndMonth", choicesList3, "Jan", string.Empty);
			base.RegisterStringChooser(this.EAMonthJSON);
			this.EADayJSON = new JSONStorableStringChooser("EAEndDay", list3, "1", string.Empty);
			base.RegisterStringChooser(this.EADayJSON);
			List<string> list4 = new List<string>(this.licenseTypes);
			list4.RemoveRange(0, 3);
			this.secondaryLicenseTypeJSON = new JSONStorableStringChooser("secondaryLicenseType", list4, this._secondaryLicenseType, "Secondary License Type", new JSONStorableStringChooser.SetStringCallback(this.SyncSecondaryLicenseType));
			base.RegisterStringChooser(this.secondaryLicenseTypeJSON);
			if (!this.isManager)
			{
				this.openPrepFolderInExplorerAction = new JSONStorableAction("OpenPrepFolderInExplorer", new JSONStorableAction.ActionCallback(this.OpenPrepFolderInExplorer));
				base.RegisterAction(this.openPrepFolderInExplorerAction);
				this.finalizeCheckConfirmAction = new JSONStorableAction("FinalizeCheckConfirm", new JSONStorableAction.ActionCallback(this.FinalizeCheckConfirm));
				base.RegisterAction(this.finalizeCheckConfirmAction);
				this.finalizeCheckCancelAction = new JSONStorableAction("FinalizeCheckCancel", new JSONStorableAction.ActionCallback(this.FinalizeCheckCancel));
				base.RegisterAction(this.finalizeCheckCancelAction);
				this.finalizePackageAction = new JSONStorableAction("FinalizePackage", new JSONStorableAction.ActionCallback(this.FinalizePackage));
				base.RegisterAction(this.finalizePackageAction);
				this.cancelFinalizeAction = new JSONStorableAction("CancelFinalize", new JSONStorableAction.ActionCallback(this.CancelFinalize));
				base.RegisterAction(this.cancelFinalizeAction);
			}
		}

		// Token: 0x06005791 RID: 22417 RVA: 0x00202A78 File Offset: 0x00200E78
		protected override void InitUI(Transform t, bool isAlt)
		{
			if (t != null)
			{
				PackageBuilderUI componentInChildren = t.GetComponentInChildren<PackageBuilderUI>(true);
				if (componentInChildren != null)
				{
					this.clearAllJSON.RegisterButton(componentInChildren.clearAllButton, isAlt);
					this.loadMetaFromExistingPackageJSON.RegisterButton(componentInChildren.loadMetaFromExistingPackageButton, isAlt);
					this.creatorNameJSON.RegisterInputField(componentInChildren.creatorField, isAlt);
					this.packageNameJSON.RegisterInputField(componentInChildren.packageNameField, isAlt);
					if (!isAlt)
					{
						this.versionField = componentInChildren.versionField;
					}
					if (!isAlt)
					{
						this.statusText = componentInChildren.statusText;
					}
					if (this.isManager)
					{
						this.scanHubForMissingPackagesAction.RegisterButton(componentInChildren.scanHubForMissingPackagesButton, isAlt);
						this.selectCurrentScenePackageAction.RegisterButton(componentInChildren.selectCurrentScenePackageButton, isAlt);
						this.showDisabledJSON.RegisterToggle(componentInChildren.showDisabledToggle, isAlt);
						this.goToPromotionalLinkAction.RegisterButton(componentInChildren.promotionalButton, isAlt);
						this.copyPromotionalLinkAction.RegisterButton(componentInChildren.copyPromotionalLinkButton, isAlt);
						this.openOnHubAction.RegisterButton(componentInChildren.openOnHubButton, isAlt);
						this.openInHubDownloaderAction.RegisterButton(componentInChildren.openInHubDownloaderButton, isAlt);
						this.currentPackageHasSceneJSON.RegisterIndicator(componentInChildren.currentPackageHasSceneIndicator, isAlt);
						this.openSceneAction.RegisterButton(componentInChildren.openSceneButton, isAlt);
						if (!isAlt)
						{
							this.currentPackageIsOnHubJSON.RegisterIndicator(componentInChildren.currentPackageIsOnHubIndicator, false);
							this.currentPackageIsOnHubJSON.RegisterIndicator(componentInChildren.currentPackageIsOnHubIndicator2, true);
							this.packageReferencesContainer = componentInChildren.packageReferencesContainer;
							this.packagesContainer = componentInChildren.packagesContainer;
							this.missingPackagesContainer = componentInChildren.missingPackagesContainer;
							this.packageCategoryPanel = componentInChildren.packageCategoryPanel;
							if (this.packageCategoryPanel != null && this.categoryTogglePrefab != null)
							{
								ToggleGroupValue toggleGroupValue = this.packageCategoryPanel.gameObject.AddComponent<ToggleGroupValue>();
								foreach (PackageBuilder.CategoryFilter categoryFilter in this.categoryFilters)
								{
									RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.categoryTogglePrefab);
									rectTransform.SetParent(this.packageCategoryPanel, false);
									rectTransform.gameObject.name = categoryFilter.name;
									Text componentInChildren2 = rectTransform.GetComponentInChildren<Text>(true);
									if (componentInChildren2 != null)
									{
										componentInChildren2.text = categoryFilter.name;
									}
									Toggle component = rectTransform.GetComponent<Toggle>();
									if (component != null)
									{
										component.onValueChanged.AddListener(new UnityAction<bool>(toggleGroupValue.ToggleChanged));
									}
								}
								toggleGroupValue.Init();
								this.categoryJSON.toggleGroupValue = toggleGroupValue;
							}
							this.promotionalButton = componentInChildren.promotionalButton;
							this.promotionalButtonText = componentInChildren.promotionalButtonText;
							this.SyncPromotionalLink(this.promotionalLinkJSON.val);
							this.confirmDeletePackagePanel = componentInChildren.confirmDeletePackagePanel;
							this.confirmDeletePackageText = componentInChildren.confirmDeletePackageText;
							this.confirmUnpackPanel = componentInChildren.confirmUnpackPanel;
							this.confirmRestoreFromOriginalPanel = componentInChildren.confirmRestoreFromOriginalPanel;
							this.packPanel = componentInChildren.packPanel;
							this.packProgressSlider = componentInChildren.packProgressSlider;
						}
						this.packageEnabledJSON.RegisterToggle(componentInChildren.packageEnabledToggle, isAlt);
						this.deletePackageAction.RegisterButton(componentInChildren.deletePackageButton, isAlt);
						this.confirmDeletePackageAction.RegisterButton(componentInChildren.confirmDeletePackageButton, isAlt);
						this.cancelDeletePackageAction.RegisterButton(componentInChildren.cancelDeletePackageButton, isAlt);
						this.userNotesJSON.RegisterInputField(componentInChildren.userNotesField, isAlt);
						this.pluginsAlwaysEnabledJSON.RegisterToggle(componentInChildren.pluginsAlwaysEnabledToggle, isAlt);
						this.pluginsAlwaysDisabledJSON.RegisterToggle(componentInChildren.pluginsAlwaysDisabledToggle, isAlt);
						this.ignoreMissingDependencyErrorsJSON.RegisterToggle(componentInChildren.ignoreMissingDependencyErrorsToggle, isAlt);
						this.unpackAction.RegisterButton(componentInChildren.unpackButton, isAlt);
						this.confirmUnpackAction.RegisterButton(componentInChildren.confirmUnpackButton, isAlt);
						this.cancelUnpackAction.RegisterButton(componentInChildren.cancelUnpackButton, isAlt);
						this.repackAction.RegisterButton(componentInChildren.repackButton, isAlt);
						this.restoreFromOriginalAction.RegisterButton(componentInChildren.restoreFromOriginalButton, isAlt);
						this.confirmRestoreFromOriginalAction.RegisterButton(componentInChildren.confirmRestoreFromOriginalButton, isAlt);
						this.cancelRestoreFromOriginalAction.RegisterButton(componentInChildren.cancelRestoreFromOriginalButton, isAlt);
						this.hadReferenceIssuesJSON.RegisterIndicator(componentInChildren.hadReferenceIssuesIndicator, isAlt);
					}
					if (!isAlt && this.customOptions != null && componentInChildren.customOptionToggles != null && this.customOptions.Length == componentInChildren.customOptionToggles.Length)
					{
						for (int j = 0; j < this.customOptions.Length; j++)
						{
							this.customOptions[j].SetToggle(componentInChildren.customOptionToggles[j]);
						}
					}
					if (!isAlt)
					{
						this.contentContainer = componentInChildren.contentContainer;
					}
					this.addDirectoryAction.RegisterButton(componentInChildren.addDirectoryButton, isAlt);
					this.addFileAction.RegisterButton(componentInChildren.addFileButton, isAlt);
					this.removeSelectedAction.RegisterButton(componentInChildren.removeSelectedButton, isAlt);
					this.removeAllAction.RegisterButton(componentInChildren.removeAllButton, isAlt);
					if (!isAlt)
					{
						this.referencesContainer = componentInChildren.referencesContainer;
						this.licenseReportContainer = componentInChildren.licenseReportContainer;
						this.licenseReportIssueText = componentInChildren.licenseReportIssueText;
						if (this.licenseReportIssueText != null)
						{
							this.licenseReportIssueText.gameObject.SetActive(false);
						}
						this.nonCommercialLicenseReportIssueText = componentInChildren.nonCommercialLicenseReportIssueText;
						if (this.nonCommercialLicenseReportIssueText != null)
						{
							this.nonCommercialLicenseReportIssueText.gameObject.SetActive(false);
						}
					}
					this.standardReferenceVersionOptionJSON.RegisterPopup(componentInChildren.standardReferenceVersionOptionPopup, isAlt);
					this.scriptReferenceVersionOptionJSON.RegisterPopup(componentInChildren.scriptReferenceVersionOptionPopup, isAlt);
					if (!this.isManager)
					{
						this.prepPackageAction.RegisterButton(componentInChildren.prepPackageButton, isAlt);
						this.fixReferencesAction.RegisterButton(componentInChildren.fixReferencesButton, isAlt);
					}
					if (componentInChildren.fixReferencesButton != null)
					{
						componentInChildren.fixReferencesButton.gameObject.SetActive(false);
					}
					this.descriptionJSON.RegisterInputField(componentInChildren.descriptionField, isAlt);
					this.creditsJSON.RegisterInputField(componentInChildren.creditsField, isAlt);
					this.instructionsJSON.RegisterInputField(componentInChildren.instructionsField, isAlt);
					this.promotionalLinkJSON.RegisterInputField(componentInChildren.promotionalField, isAlt);
					this.licenseTypeJSON.RegisterPopup(componentInChildren.licensePopup, isAlt);
					this.secondaryLicenseTypeJSON.RegisterPopup(componentInChildren.secondaryLicensePopup, isAlt);
					this.EAYearJSON.RegisterPopup(componentInChildren.EAYearPopup, isAlt);
					this.EAMonthJSON.RegisterPopup(componentInChildren.EAMonthPopup, isAlt);
					this.EADayJSON.RegisterPopup(componentInChildren.EADayPopup, isAlt);
					if (!isAlt)
					{
						this.licenseDescriptionText = componentInChildren.licenseDescriptionText;
						this.openPrepFolderInExplorerNotice = componentInChildren.openPrepFolderInExplorerNotice;
					}
					if (!this.isManager)
					{
						this.openPrepFolderInExplorerAction.RegisterButton(componentInChildren.openPrepFolderInExplorerButton, isAlt);
						this.finalizeCheckConfirmAction.RegisterButton(componentInChildren.finalizeCheckConfirmButton, isAlt);
						this.finalizeCheckCancelAction.RegisterButton(componentInChildren.finalizeCheckCancelButton, isAlt);
						this.finalizePackageAction.RegisterButton(componentInChildren.finalizeButton, isAlt);
						this.cancelFinalizeAction.RegisterButton(componentInChildren.cancelFinalizeButton, isAlt);
						if (!isAlt)
						{
							this.finalizeProgressSlider = componentInChildren.finalizeProgressSlider;
							this.finalizeCheckPanel = componentInChildren.finalizeCheckPanel;
							this.finalizingPanel = componentInChildren.finalizingPanel;
						}
					}
					this.SyncLicenseType(this._licenseType);
					if (this.isManager)
					{
						this.SyncPackages();
					}
				}
			}
		}

		// Token: 0x06005792 RID: 22418 RVA: 0x002031AB File Offset: 0x002015AB
		protected override void Awake()
		{
			if (!this.awakecalled)
			{
				base.Awake();
				this.Init();
				this.InitUI();
				this.InitUIAlt();
				this.NeedsPrep = true;
			}
		}

		// Token: 0x06005793 RID: 22419 RVA: 0x002031D7 File Offset: 0x002015D7
		protected void OnDestroy()
		{
			this.AbortFinalizePackageThreaded(false);
		}

		// Token: 0x04004782 RID: 18306
		protected JSONStorableAction clearAllJSON;

		// Token: 0x04004783 RID: 18307
		protected VarPackage currentPackage;

		// Token: 0x04004784 RID: 18308
		protected string currentPackageResourceId;

		// Token: 0x04004785 RID: 18309
		protected string currentPackageScenePath;

		// Token: 0x04004786 RID: 18310
		protected JSONStorableBool currentPackageIsOnHubJSON;

		// Token: 0x04004787 RID: 18311
		protected JSONStorableBool currentPackageHasSceneJSON;

		// Token: 0x04004788 RID: 18312
		protected JSONStorableAction loadMetaFromExistingPackageJSON;

		// Token: 0x04004789 RID: 18313
		protected char[] invalidChars;

		// Token: 0x0400478A RID: 18314
		protected string _creatorName;

		// Token: 0x0400478B RID: 18315
		protected JSONStorableString creatorNameJSON;

		// Token: 0x0400478C RID: 18316
		protected string _packageName;

		// Token: 0x0400478D RID: 18317
		protected JSONStorableString packageNameJSON;

		// Token: 0x0400478E RID: 18318
		protected InputField versionField;

		// Token: 0x0400478F RID: 18319
		protected int _version;

		// Token: 0x04004790 RID: 18320
		public Color errorColor = Color.red;

		// Token: 0x04004791 RID: 18321
		public Color warningColor = Color.yellow;

		// Token: 0x04004792 RID: 18322
		public Color normalColor = Color.black;

		// Token: 0x04004793 RID: 18323
		public Color disabledColor = Color.gray;

		// Token: 0x04004794 RID: 18324
		public Color readyColor = Color.green;

		// Token: 0x04004795 RID: 18325
		protected Text statusText;

		// Token: 0x04004796 RID: 18326
		public bool isManager;

		// Token: 0x04004797 RID: 18327
		protected RectTransform packagesContainer;

		// Token: 0x04004798 RID: 18328
		public RectTransform packageItemPrefab;

		// Token: 0x04004799 RID: 18329
		protected Dictionary<string, PackageBuilderPackageItem> packageItems;

		// Token: 0x0400479A RID: 18330
		protected RectTransform missingPackagesContainer;

		// Token: 0x0400479B RID: 18331
		protected Dictionary<string, PackageBuilderPackageItem> missingPackageItems;

		// Token: 0x0400479C RID: 18332
		protected RectTransform packageReferencesContainer;

		// Token: 0x0400479D RID: 18333
		protected List<PackageBuilderPackageItem> packageReferenceItems;

		// Token: 0x0400479E RID: 18334
		public HubBrowse hubBrowse;

		// Token: 0x0400479F RID: 18335
		protected JSONStorableAction scanHubForMissingPackagesAction;

		// Token: 0x040047A0 RID: 18336
		protected JSONStorableAction selectCurrentScenePackageAction;

		// Token: 0x040047A1 RID: 18337
		protected JSONStorableBool showDisabledJSON;

		// Token: 0x040047A2 RID: 18338
		protected RectTransform packageCategoryPanel;

		// Token: 0x040047A3 RID: 18339
		public RectTransform categoryTogglePrefab;

		// Token: 0x040047A4 RID: 18340
		public PackageBuilder.CategoryFilter[] categoryFilters;

		// Token: 0x040047A5 RID: 18341
		protected Dictionary<string, List<string>> categoryToPackageUids;

		// Token: 0x040047A6 RID: 18342
		protected string currentCategory;

		// Token: 0x040047A7 RID: 18343
		protected JSONStorableStringChooser categoryJSON;

		// Token: 0x040047A8 RID: 18344
		protected Button promotionalButton;

		// Token: 0x040047A9 RID: 18345
		protected Text promotionalButtonText;

		// Token: 0x040047AA RID: 18346
		protected JSONStorableAction goToPromotionalLinkAction;

		// Token: 0x040047AB RID: 18347
		protected JSONStorableAction copyPromotionalLinkAction;

		// Token: 0x040047AC RID: 18348
		protected JSONStorableBool packageEnabledJSON;

		// Token: 0x040047AD RID: 18349
		protected JSONStorableAction openOnHubAction;

		// Token: 0x040047AE RID: 18350
		public HubDownloader hubDownloader;

		// Token: 0x040047AF RID: 18351
		protected JSONStorableAction openInHubDownloaderAction;

		// Token: 0x040047B0 RID: 18352
		protected JSONStorableAction openSceneAction;

		// Token: 0x040047B1 RID: 18353
		protected RectTransform confirmDeletePackagePanel;

		// Token: 0x040047B2 RID: 18354
		protected Text confirmDeletePackageText;

		// Token: 0x040047B3 RID: 18355
		protected JSONStorableAction deletePackageAction;

		// Token: 0x040047B4 RID: 18356
		protected JSONStorableAction confirmDeletePackageAction;

		// Token: 0x040047B5 RID: 18357
		protected JSONStorableAction cancelDeletePackageAction;

		// Token: 0x040047B6 RID: 18358
		protected JSONStorableBool hadReferenceIssuesJSON;

		// Token: 0x040047B7 RID: 18359
		protected RectTransform packPanel;

		// Token: 0x040047B8 RID: 18360
		protected Slider packProgressSlider;

		// Token: 0x040047B9 RID: 18361
		protected AsyncFlag unpackFlag;

		// Token: 0x040047BA RID: 18362
		protected RectTransform confirmUnpackPanel;

		// Token: 0x040047BB RID: 18363
		protected JSONStorableAction unpackAction;

		// Token: 0x040047BC RID: 18364
		protected JSONStorableAction confirmUnpackAction;

		// Token: 0x040047BD RID: 18365
		protected JSONStorableAction cancelUnpackAction;

		// Token: 0x040047BE RID: 18366
		protected AsyncFlag repackFlag;

		// Token: 0x040047BF RID: 18367
		protected JSONStorableAction repackAction;

		// Token: 0x040047C0 RID: 18368
		protected RectTransform confirmRestoreFromOriginalPanel;

		// Token: 0x040047C1 RID: 18369
		protected JSONStorableAction restoreFromOriginalAction;

		// Token: 0x040047C2 RID: 18370
		protected JSONStorableAction confirmRestoreFromOriginalAction;

		// Token: 0x040047C3 RID: 18371
		protected JSONStorableAction cancelRestoreFromOriginalAction;

		// Token: 0x040047C4 RID: 18372
		public VarPackageCustomOption[] customOptions;

		// Token: 0x040047C5 RID: 18373
		protected string _userNotes;

		// Token: 0x040047C6 RID: 18374
		protected JSONStorableString userNotesJSON;

		// Token: 0x040047C7 RID: 18375
		protected bool _pluginsAlwaysEnabled;

		// Token: 0x040047C8 RID: 18376
		protected JSONStorableBool pluginsAlwaysEnabledJSON;

		// Token: 0x040047C9 RID: 18377
		protected bool _pluginsAlwaysDisabled;

		// Token: 0x040047CA RID: 18378
		protected JSONStorableBool pluginsAlwaysDisabledJSON;

		// Token: 0x040047CB RID: 18379
		protected bool _ignoreMissingDependencyErrors;

		// Token: 0x040047CC RID: 18380
		protected JSONStorableBool ignoreMissingDependencyErrorsJSON;

		// Token: 0x040047CD RID: 18381
		protected RectTransform contentContainer;

		// Token: 0x040047CE RID: 18382
		public RectTransform contentItemPrefab;

		// Token: 0x040047CF RID: 18383
		protected Dictionary<string, PackageBuilderContentItem> contentItems;

		// Token: 0x040047D0 RID: 18384
		protected string suggestedDirectory = "Custom";

		// Token: 0x040047D1 RID: 18385
		protected List<ShortCut> shortCuts;

		// Token: 0x040047D2 RID: 18386
		protected JSONStorableAction addDirectoryAction;

		// Token: 0x040047D3 RID: 18387
		protected JSONStorableAction addFileAction;

		// Token: 0x040047D4 RID: 18388
		protected JSONStorableAction removeSelectedAction;

		// Token: 0x040047D5 RID: 18389
		protected JSONStorableAction removeAllAction;

		// Token: 0x040047D6 RID: 18390
		protected string _preppedDir;

		// Token: 0x040047D7 RID: 18391
		protected bool _needsPrep;

		// Token: 0x040047D8 RID: 18392
		protected RectTransform referencesContainer;

		// Token: 0x040047D9 RID: 18393
		public RectTransform referencesItemPrefab;

		// Token: 0x040047DA RID: 18394
		protected VarPackage.ReferenceVersionOption _standardReferenceVersionOption;

		// Token: 0x040047DB RID: 18395
		protected JSONStorableStringChooser standardReferenceVersionOptionJSON;

		// Token: 0x040047DC RID: 18396
		protected VarPackage.ReferenceVersionOption _scriptReferenceVersionOption = VarPackage.ReferenceVersionOption.Exact;

		// Token: 0x040047DD RID: 18397
		protected JSONStorableStringChooser scriptReferenceVersionOptionJSON;

		// Token: 0x040047DE RID: 18398
		protected Dictionary<string, PackageBuilder.ReferenceReport> referenceReports;

		// Token: 0x040047DF RID: 18399
		protected List<PackageBuilderReferenceItem> referenceItems;

		// Token: 0x040047E0 RID: 18400
		protected HashSet<VarPackage> allReferencedPackages;

		// Token: 0x040047E1 RID: 18401
		protected HashSet<string> allReferencedPackageUids;

		// Token: 0x040047E2 RID: 18402
		protected JSONClass allPackagesDependencyTree;

		// Token: 0x040047E3 RID: 18403
		protected List<PackageBuilderReferenceItem> licenseReportItems;

		// Token: 0x040047E4 RID: 18404
		protected RectTransform licenseReportContainer;

		// Token: 0x040047E5 RID: 18405
		protected Text licenseReportIssueText;

		// Token: 0x040047E6 RID: 18406
		protected Text nonCommercialLicenseReportIssueText;

		// Token: 0x040047E7 RID: 18407
		protected bool licenseIssue;

		// Token: 0x040047E8 RID: 18408
		protected bool nonCommercialLicenseIssue;

		// Token: 0x040047E9 RID: 18409
		protected HashSet<string> alreadyReported;

		// Token: 0x040047EA RID: 18410
		protected JSONStorableAction prepPackageAction;

		// Token: 0x040047EB RID: 18411
		protected JSONStorableAction fixReferencesAction;

		// Token: 0x040047EC RID: 18412
		protected string _description;

		// Token: 0x040047ED RID: 18413
		protected JSONStorableString descriptionJSON;

		// Token: 0x040047EE RID: 18414
		protected string _credits;

		// Token: 0x040047EF RID: 18415
		protected JSONStorableString creditsJSON;

		// Token: 0x040047F0 RID: 18416
		protected string _instructions;

		// Token: 0x040047F1 RID: 18417
		protected JSONStorableString instructionsJSON;

		// Token: 0x040047F2 RID: 18418
		protected string _promotionalLink;

		// Token: 0x040047F3 RID: 18419
		protected JSONStorableString promotionalLinkJSON;

		// Token: 0x040047F4 RID: 18420
		protected string[] licenseTypes = new string[]
		{
			"PC",
			"PC EA",
			"Questionable",
			"FC",
			"CC BY",
			"CC BY-SA",
			"CC BY-ND",
			"CC BY-NC",
			"CC BY-NC-SA",
			"CC BY-NC-ND"
		};

		// Token: 0x040047F5 RID: 18421
		protected bool[] licenseDistributableFlags = new bool[]
		{
			false,
			false,
			false,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};

		// Token: 0x040047F6 RID: 18422
		protected bool[] licenseNonCommercialFlags = new bool[]
		{
			true,
			true,
			true,
			false,
			false,
			false,
			false,
			true,
			true,
			true
		};

		// Token: 0x040047F7 RID: 18423
		protected string[] licenseDescriptions = new string[]
		{
			"PC-Paid Content: cannot distribute, remix, tweak, or build upon work",
			"PC EA-Paid Content Early Access: cannot distribute, remix, tweak, or build upon work until the EA (Early Access) end date, at which point the secondary license is in effect.",
			"Questionable: should not distribute",
			"FC-Free Content: can distribute, remix, tweak, and build upon work, even commercially. No credit required.",
			"CC BY: can distribute, remix, tweak, and build upon work, even commercially. Must credit creator.",
			"CC BY-SA: can distribute, remix, tweak, and build upon work, even commercially, as long as license for new creations is identical. Must credit creator.",
			"CC BY-ND: can distribute, commercially or non-commercially, as long as passed along unchanged. Must credit creator.",
			"CC BY-NC: can distribute, remix, tweak, and build upon work, but only non-commercially. Must credit creator.",
			"CC BY-NC-SA: can distribute, remix, tweak, and build upon work, but only non-commercially, and as long as license for new creations is identical. Must credit creator.",
			"CC BY-NC-ND: can distribute, non-commercially, as long as passed along unchanged. Must credit creator."
		};

		// Token: 0x040047F8 RID: 18424
		protected Text licenseDescriptionText;

		// Token: 0x040047F9 RID: 18425
		protected string _licenseType = "CC BY";

		// Token: 0x040047FA RID: 18426
		protected JSONStorableStringChooser licenseTypeJSON;

		// Token: 0x040047FB RID: 18427
		protected string _secondaryLicenseType = "CC BY";

		// Token: 0x040047FC RID: 18428
		protected JSONStorableStringChooser secondaryLicenseTypeJSON;

		// Token: 0x040047FD RID: 18429
		protected string[] yearChoices = new string[]
		{
			"2020",
			"2021",
			"2022",
			"2023",
			"2024",
			"2025",
			"2026",
			"2027",
			"2028",
			"2029",
			"2030"
		};

		// Token: 0x040047FE RID: 18430
		protected string[] monthChoices = new string[]
		{
			"Jan",
			"Feb",
			"Mar",
			"Apr",
			"May",
			"Jun",
			"Jul",
			"Aug",
			"Sep",
			"Oct",
			"Nov",
			"Dec"
		};

		// Token: 0x040047FF RID: 18431
		protected JSONStorableStringChooser EAYearJSON;

		// Token: 0x04004800 RID: 18432
		protected JSONStorableStringChooser EAMonthJSON;

		// Token: 0x04004801 RID: 18433
		protected JSONStorableStringChooser EADayJSON;

		// Token: 0x04004802 RID: 18434
		protected RectTransform openPrepFolderInExplorerNotice;

		// Token: 0x04004803 RID: 18435
		protected JSONStorableAction openPrepFolderInExplorerAction;

		// Token: 0x04004804 RID: 18436
		protected RectTransform finalizingPanel;

		// Token: 0x04004805 RID: 18437
		protected bool isFinalizing;

		// Token: 0x04004806 RID: 18438
		protected Thread finalizeThread;

		// Token: 0x04004807 RID: 18439
		protected bool finalizeThreadAbort;

		// Token: 0x04004808 RID: 18440
		protected string finalizeThreadError;

		// Token: 0x04004809 RID: 18441
		protected int finalizeFileProgressCount;

		// Token: 0x0400480A RID: 18442
		protected int finalizeTotalFileCount;

		// Token: 0x0400480B RID: 18443
		protected Slider finalizeProgressSlider;

		// Token: 0x0400480C RID: 18444
		protected float finalizeProgress;

		// Token: 0x0400480D RID: 18445
		protected AsyncFlag finalizeFlag;

		// Token: 0x0400480E RID: 18446
		protected JSONStorableAction finalizeCheckConfirmAction;

		// Token: 0x0400480F RID: 18447
		protected JSONStorableAction finalizeCheckCancelAction;

		// Token: 0x04004810 RID: 18448
		protected RectTransform finalizeCheckPanel;

		// Token: 0x04004811 RID: 18449
		protected JSONStorableAction finalizePackageAction;

		// Token: 0x04004812 RID: 18450
		protected JSONStorableAction cancelFinalizeAction;

		// Token: 0x04004813 RID: 18451
		[CompilerGenerated]
		private static Dictionary<string, int> <>f__switch$map2;

		// Token: 0x02000BE5 RID: 3045
		[Serializable]
		public class CategoryFilter
		{
			// Token: 0x06005794 RID: 22420 RVA: 0x002031E0 File Offset: 0x002015E0
			public CategoryFilter()
			{
			}

			// Token: 0x04004814 RID: 18452
			public string name;

			// Token: 0x04004815 RID: 18453
			public string matchDirectoryPath;
		}

		// Token: 0x02000BE6 RID: 3046
		protected class ReferenceReport
		{
			// Token: 0x06005795 RID: 22421 RVA: 0x002031E8 File Offset: 0x002015E8
			public ReferenceReport(string reference, VarPackage package = null, string packageUid = null, bool hasIssue = false, string issue = "", bool fixable = false, string fixedReference = null)
			{
				this.Reference = reference;
				this.Package = package;
				this.PackageUid = packageUid;
				this.HasIssue = hasIssue;
				this.Issue = issue;
				this.Fixable = fixable;
				this.FixedReference = fixedReference;
			}

			// Token: 0x17000CB5 RID: 3253
			// (get) Token: 0x06005796 RID: 22422 RVA: 0x00203225 File Offset: 0x00201625
			// (set) Token: 0x06005797 RID: 22423 RVA: 0x0020322D File Offset: 0x0020162D
			public string Reference
			{
				[CompilerGenerated]
				get
				{
					return this.<Reference>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<Reference>k__BackingField = value;
				}
			}

			// Token: 0x17000CB6 RID: 3254
			// (get) Token: 0x06005798 RID: 22424 RVA: 0x00203236 File Offset: 0x00201636
			// (set) Token: 0x06005799 RID: 22425 RVA: 0x0020323E File Offset: 0x0020163E
			public VarPackage Package
			{
				[CompilerGenerated]
				get
				{
					return this.<Package>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<Package>k__BackingField = value;
				}
			}

			// Token: 0x17000CB7 RID: 3255
			// (get) Token: 0x0600579A RID: 22426 RVA: 0x00203247 File Offset: 0x00201647
			// (set) Token: 0x0600579B RID: 22427 RVA: 0x0020324F File Offset: 0x0020164F
			public string PackageUid
			{
				[CompilerGenerated]
				get
				{
					return this.<PackageUid>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<PackageUid>k__BackingField = value;
				}
			}

			// Token: 0x17000CB8 RID: 3256
			// (get) Token: 0x0600579C RID: 22428 RVA: 0x00203258 File Offset: 0x00201658
			// (set) Token: 0x0600579D RID: 22429 RVA: 0x00203260 File Offset: 0x00201660
			public bool HasIssue
			{
				[CompilerGenerated]
				get
				{
					return this.<HasIssue>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<HasIssue>k__BackingField = value;
				}
			}

			// Token: 0x17000CB9 RID: 3257
			// (get) Token: 0x0600579E RID: 22430 RVA: 0x00203269 File Offset: 0x00201669
			// (set) Token: 0x0600579F RID: 22431 RVA: 0x00203271 File Offset: 0x00201671
			public string Issue
			{
				[CompilerGenerated]
				get
				{
					return this.<Issue>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<Issue>k__BackingField = value;
				}
			}

			// Token: 0x17000CBA RID: 3258
			// (get) Token: 0x060057A0 RID: 22432 RVA: 0x0020327A File Offset: 0x0020167A
			// (set) Token: 0x060057A1 RID: 22433 RVA: 0x00203282 File Offset: 0x00201682
			public bool Fixable
			{
				[CompilerGenerated]
				get
				{
					return this.<Fixable>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<Fixable>k__BackingField = value;
				}
			}

			// Token: 0x17000CBB RID: 3259
			// (get) Token: 0x060057A2 RID: 22434 RVA: 0x0020328B File Offset: 0x0020168B
			// (set) Token: 0x060057A3 RID: 22435 RVA: 0x00203293 File Offset: 0x00201693
			public string FixedReference
			{
				[CompilerGenerated]
				get
				{
					return this.<FixedReference>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<FixedReference>k__BackingField = value;
				}
			}

			// Token: 0x04004816 RID: 18454
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <Reference>k__BackingField;

			// Token: 0x04004817 RID: 18455
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private VarPackage <Package>k__BackingField;

			// Token: 0x04004818 RID: 18456
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <PackageUid>k__BackingField;

			// Token: 0x04004819 RID: 18457
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private bool <HasIssue>k__BackingField;

			// Token: 0x0400481A RID: 18458
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <Issue>k__BackingField;

			// Token: 0x0400481B RID: 18459
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private bool <Fixable>k__BackingField;

			// Token: 0x0400481C RID: 18460
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <FixedReference>k__BackingField;
		}

		// Token: 0x02000FF2 RID: 4082
		[CompilerGenerated]
		private sealed class <AddPackageReferenceItem>c__AnonStorey3
		{
			// Token: 0x06007621 RID: 30241 RVA: 0x0020329C File Offset: 0x0020169C
			public <AddPackageReferenceItem>c__AnonStorey3()
			{
			}

			// Token: 0x06007622 RID: 30242 RVA: 0x002032A4 File Offset: 0x002016A4
			internal void <>m__0()
			{
				this.$this.LoadMetaFromPackageUid(this.packageId, true);
			}

			// Token: 0x040069F4 RID: 27124
			internal string packageId;

			// Token: 0x040069F5 RID: 27125
			internal PackageBuilder $this;
		}

		// Token: 0x02000FF3 RID: 4083
		[CompilerGenerated]
		private sealed class <SyncPackagesList>c__AnonStorey4
		{
			// Token: 0x06007623 RID: 30243 RVA: 0x002032B8 File Offset: 0x002016B8
			public <SyncPackagesList>c__AnonStorey4()
			{
			}

			// Token: 0x06007624 RID: 30244 RVA: 0x002032C0 File Offset: 0x002016C0
			internal void <>m__0()
			{
				this.$this.LoadMetaFromPackageUid(this.vpuid, true);
			}

			// Token: 0x040069F6 RID: 27126
			internal string vpuid;

			// Token: 0x040069F7 RID: 27127
			internal PackageBuilder $this;
		}

		// Token: 0x02000FF4 RID: 4084
		[CompilerGenerated]
		private sealed class <UnpackCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007625 RID: 30245 RVA: 0x002032D4 File Offset: 0x002016D4
			[DebuggerHidden]
			public <UnpackCo>c__Iterator0()
			{
			}

			// Token: 0x06007626 RID: 30246 RVA: 0x002032DC File Offset: 0x002016DC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.unpackFlag = new AsyncFlag("UnpackPackage");
					SuperController.singleton.SetLoadingIconFlag(this.unpackFlag);
					if (this.packPanel != null)
					{
						this.packPanel.gameObject.SetActive(true);
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (this.packProgressSlider != null)
					{
						this.packProgressSlider.value = 0f;
					}
					this.currentPackage.Unpack();
					break;
				case 2U:
					if (this.packProgressSlider != null)
					{
						this.packProgressSlider.value = this.currentPackage.packProgress;
					}
					break;
				default:
					return false;
				}
				if (this.currentPackage.IsUnpacking)
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				if (this.currentPackage.packThreadError != null)
				{
					base.ShowErrorStatus("Exception during unpack");
					SuperController.LogError("Exception during package unpack: " + this.currentPackage.packThreadError);
				}
				else
				{
					FileManager.Refresh();
				}
				base.UnpackComplete();
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700116B RID: 4459
			// (get) Token: 0x06007627 RID: 30247 RVA: 0x00203488 File Offset: 0x00201888
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700116C RID: 4460
			// (get) Token: 0x06007628 RID: 30248 RVA: 0x00203490 File Offset: 0x00201890
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007629 RID: 30249 RVA: 0x00203498 File Offset: 0x00201898
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600762A RID: 30250 RVA: 0x002034A8 File Offset: 0x002018A8
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040069F8 RID: 27128
			internal PackageBuilder $this;

			// Token: 0x040069F9 RID: 27129
			internal object $current;

			// Token: 0x040069FA RID: 27130
			internal bool $disposing;

			// Token: 0x040069FB RID: 27131
			internal int $PC;
		}

		// Token: 0x02000FF5 RID: 4085
		[CompilerGenerated]
		private sealed class <RepackCo>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600762B RID: 30251 RVA: 0x002034AF File Offset: 0x002018AF
			[DebuggerHidden]
			public <RepackCo>c__Iterator1()
			{
			}

			// Token: 0x0600762C RID: 30252 RVA: 0x002034B8 File Offset: 0x002018B8
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.repackFlag = new AsyncFlag("RepackPackage");
					SuperController.singleton.SetLoadingIconFlag(this.repackFlag);
					if (this.packPanel != null)
					{
						this.packPanel.gameObject.SetActive(true);
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (this.packProgressSlider != null)
					{
						this.packProgressSlider.value = 0f;
					}
					this.currentPackage.Repack();
					break;
				case 2U:
					if (this.packProgressSlider != null)
					{
						this.packProgressSlider.value = this.currentPackage.packProgress;
					}
					break;
				default:
					return false;
				}
				if (this.currentPackage.IsRepacking)
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				if (this.currentPackage.packThreadError != null)
				{
					base.ShowErrorStatus("Exception during repack");
					SuperController.LogError("Exception during package repack: " + this.currentPackage.packThreadError);
				}
				else
				{
					FileManager.Refresh();
				}
				base.RepackComplete();
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700116D RID: 4461
			// (get) Token: 0x0600762D RID: 30253 RVA: 0x00203664 File Offset: 0x00201A64
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700116E RID: 4462
			// (get) Token: 0x0600762E RID: 30254 RVA: 0x0020366C File Offset: 0x00201A6C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600762F RID: 30255 RVA: 0x00203674 File Offset: 0x00201A74
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007630 RID: 30256 RVA: 0x00203684 File Offset: 0x00201A84
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040069FC RID: 27132
			internal PackageBuilder $this;

			// Token: 0x040069FD RID: 27133
			internal object $current;

			// Token: 0x040069FE RID: 27134
			internal bool $disposing;

			// Token: 0x040069FF RID: 27135
			internal int $PC;
		}

		// Token: 0x02000FF6 RID: 4086
		[CompilerGenerated]
		private sealed class <FinalizePackageCo>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007631 RID: 30257 RVA: 0x0020368B File Offset: 0x00201A8B
			[DebuggerHidden]
			public <FinalizePackageCo>c__Iterator2()
			{
			}

			// Token: 0x06007632 RID: 30258 RVA: 0x00203694 File Offset: 0x00201A94
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.finalizeFlag = new AsyncFlag("FinalizePackage");
					SuperController.singleton.SetLoadingIconFlag(this.finalizeFlag);
					if (this.finalizingPanel != null)
					{
						this.finalizingPanel.gameObject.SetActive(true);
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.finalizeThreadError = null;
					this.finalizeThreadAbort = false;
					this.finalizeProgress = 0f;
					this.finalizeFileProgressCount = 0;
					if (this.finalizeProgressSlider != null)
					{
						this.finalizeProgressSlider.value = 0f;
					}
					this.finalizeTotalFileCount = FileManager.FolderContentsCount(this._preppedDir);
					this.finalizeThread = new Thread(new ThreadStart(base.FinalizePackageThreaded));
					this.finalizeThread.Start();
					break;
				case 2U:
					if (this.finalizeProgressSlider != null)
					{
						this.finalizeProgressSlider.value = this.finalizeProgress;
					}
					break;
				default:
					return false;
				}
				if (this.finalizeThread.IsAlive)
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				this.finalizeThread = null;
				if (this.finalizeThreadError != null)
				{
					base.ShowErrorStatus("Exception during finalize");
					SuperController.LogError("Exception during package finalize: " + this.finalizeThreadError);
				}
				else
				{
					FileManager.Refresh();
					base.NeedsPrep = true;
					base.ShowStatus("Package complete and ready for use");
				}
				base.FinalizeComplete();
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700116F RID: 4463
			// (get) Token: 0x06007633 RID: 30259 RVA: 0x002038C9 File Offset: 0x00201CC9
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001170 RID: 4464
			// (get) Token: 0x06007634 RID: 30260 RVA: 0x002038D1 File Offset: 0x00201CD1
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007635 RID: 30261 RVA: 0x002038D9 File Offset: 0x00201CD9
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007636 RID: 30262 RVA: 0x002038E9 File Offset: 0x00201CE9
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006A00 RID: 27136
			internal PackageBuilder $this;

			// Token: 0x04006A01 RID: 27137
			internal object $current;

			// Token: 0x04006A02 RID: 27138
			internal bool $disposing;

			// Token: 0x04006A03 RID: 27139
			internal int $PC;
		}
	}
}
