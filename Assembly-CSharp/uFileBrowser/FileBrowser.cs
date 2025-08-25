using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace uFileBrowser
{
	// Token: 0x0200046A RID: 1130
	public class FileBrowser : MonoBehaviour
	{
		// Token: 0x06001C36 RID: 7222 RVA: 0x0009FB90 File Offset: 0x0009DF90
		public FileBrowser()
		{
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001C37 RID: 7223 RVA: 0x0009FC28 File Offset: 0x0009E028
		public string CurrentPath
		{
			get
			{
				return this.currentPath;
			}
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x0009FC30 File Offset: 0x0009E030
		public void SetShortCuts(List<ShortCut> newShortCuts, bool resetShortCutFilters = false)
		{
			bool flag = resetShortCutFilters;
			if (!flag)
			{
				if (this.shortCuts == null)
				{
					if (newShortCuts != null)
					{
						flag = true;
					}
				}
				else if (newShortCuts == null)
				{
					flag = true;
				}
				else if (this.shortCuts.Count == newShortCuts.Count)
				{
					for (int i = 0; i < this.shortCuts.Count; i++)
					{
						if (!this.shortCuts[i].IsSameAs(newShortCuts[i]))
						{
							flag = true;
							break;
						}
					}
				}
				else
				{
					flag = true;
				}
			}
			this.shortCuts = newShortCuts;
			if (flag)
			{
				this.ResetShortCutsCreators();
				this.ResetShortCutsSearch();
				this.UpdateDirectoryList();
			}
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0009FCE6 File Offset: 0x0009E0E6
		public void ClearCacheImage(string imgPath)
		{
			if (ImageLoaderThreaded.singleton != null)
			{
				ImageLoaderThreaded.singleton.ClearCacheThumbnail(imgPath);
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x0009FD04 File Offset: 0x0009E104
		public void ClearImageQueue()
		{
			foreach (ImageLoaderThreaded.QueuedImage queuedImage in this.queuedThumbnails)
			{
				queuedImage.cancel = true;
			}
			this.queuedThumbnails.Clear();
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x0009FD6C File Offset: 0x0009E16C
		public void MakeNewUniqueFolder()
		{
			string text = this.currentPath + this.slash + "NewFolder";
			int i = 0;
			while (i < 20)
			{
				if (!FileManager.DirectoryExists(text, false, false))
				{
					try
					{
						FileManager.CreateDirectory(text);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError("Could not make directory " + text + " Exception: " + ex.Message);
						if (this.statusField != null)
						{
							this.statusField.text = ex.Message;
						}
					}
					break;
				}
				i++;
				text = this.currentPath + this.slash + "NewFolder" + i.ToString();
			}
			this.UpdateDirectoryList();
			this.UpdateFileList();
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x0009FE48 File Offset: 0x0009E248
		public string SelectedPath
		{
			get
			{
				if (this.selected != null)
				{
					return this.selected.fullPath;
				}
				return null;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x0009FE68 File Offset: 0x0009E268
		// (set) Token: 0x06001C3E RID: 7230 RVA: 0x0009FE70 File Offset: 0x0009E270
		public UserPreferences.DirectoryOption directoryOption
		{
			get
			{
				return this._directoryOption;
			}
			set
			{
				if (this._directoryOption != value)
				{
					this._directoryOption = value;
					if (this.directoryOptionPopup != null)
					{
						this.directoryOptionPopup.currentValueNoCallback = this._directoryOption.ToString();
					}
					this.SyncSort();
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x0009FEC3 File Offset: 0x0009E2C3
		// (set) Token: 0x06001C40 RID: 7232 RVA: 0x0009FECC File Offset: 0x0009E2CC
		protected UserPreferences.DirectoryOption directoryOptionNoSync
		{
			get
			{
				return this._directoryOption;
			}
			set
			{
				if (this._directoryOption != value)
				{
					this._directoryOption = value;
					if (this.directoryOptionPopup != null)
					{
						this.directoryOptionPopup.currentValueNoCallback = this._directoryOption.ToString();
					}
				}
			}
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x0009FF1C File Offset: 0x0009E31C
		public void SetDirectoryOption(string dirOptionString)
		{
			try
			{
				UserPreferences.DirectoryOption directoryOption = (UserPreferences.DirectoryOption)Enum.Parse(typeof(UserPreferences.DirectoryOption), dirOptionString);
				this.directoryOption = directoryOption;
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.fileBrowserDirectoryOption = directoryOption;
				}
			}
			catch (ArgumentException)
			{
				UnityEngine.Debug.LogError("Attempted to set directory option to " + dirOptionString + " which is not a valid type");
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x0009FF94 File Offset: 0x0009E394
		// (set) Token: 0x06001C43 RID: 7235 RVA: 0x0009FF9C File Offset: 0x0009E39C
		public UserPreferences.SortBy sortBy
		{
			get
			{
				return this._sortBy;
			}
			set
			{
				if (this._sortBy != value)
				{
					this._sortBy = value;
					if (this.sortByPopup != null)
					{
						this.sortByPopup.currentValueNoCallback = this._sortBy.ToString();
					}
					this.SyncSort();
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x0009FFEF File Offset: 0x0009E3EF
		// (set) Token: 0x06001C45 RID: 7237 RVA: 0x0009FFF8 File Offset: 0x0009E3F8
		protected UserPreferences.SortBy sortByNoSync
		{
			get
			{
				return this._sortBy;
			}
			set
			{
				if (this._sortBy != value)
				{
					this._sortBy = value;
					if (this.sortByPopup != null)
					{
						this.sortByPopup.currentValueNoCallback = this._sortBy.ToString();
					}
				}
			}
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000A0048 File Offset: 0x0009E448
		public void SetSortBy(string sortByString)
		{
			try
			{
				UserPreferences.SortBy sortBy = (UserPreferences.SortBy)Enum.Parse(typeof(UserPreferences.SortBy), sortByString);
				this.sortBy = sortBy;
				if (UserPreferences.singleton != null)
				{
					UserPreferences.singleton.fileBrowserSortBy = sortBy;
				}
			}
			catch (ArgumentException)
			{
				UnityEngine.Debug.LogError("Attempted to set sort by to " + sortByString + " which is not a valid type");
			}
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000A00C0 File Offset: 0x0009E4C0
		private void SortFilesAndDirs(List<FileBrowser.FileAndDirInfo> fdlist)
		{
			switch (this.sortBy)
			{
			case UserPreferences.SortBy.AtoZ:
				if (FileBrowser.<>f__am$cache0 == null)
				{
					FileBrowser.<>f__am$cache0 = new Comparison<FileBrowser.FileAndDirInfo>(FileBrowser.<SortFilesAndDirs>m__0);
				}
				fdlist.Sort(FileBrowser.<>f__am$cache0);
				break;
			case UserPreferences.SortBy.ZtoA:
				if (FileBrowser.<>f__am$cache1 == null)
				{
					FileBrowser.<>f__am$cache1 = new Comparison<FileBrowser.FileAndDirInfo>(FileBrowser.<SortFilesAndDirs>m__1);
				}
				fdlist.Sort(FileBrowser.<>f__am$cache1);
				break;
			case UserPreferences.SortBy.NewToOld:
				if (FileBrowser.<>f__am$cache2 == null)
				{
					FileBrowser.<>f__am$cache2 = new Comparison<FileBrowser.FileAndDirInfo>(FileBrowser.<SortFilesAndDirs>m__2);
				}
				fdlist.Sort(FileBrowser.<>f__am$cache2);
				break;
			case UserPreferences.SortBy.OldToNew:
				if (FileBrowser.<>f__am$cache3 == null)
				{
					FileBrowser.<>f__am$cache3 = new Comparison<FileBrowser.FileAndDirInfo>(FileBrowser.<SortFilesAndDirs>m__3);
				}
				fdlist.Sort(FileBrowser.<>f__am$cache3);
				break;
			case UserPreferences.SortBy.NewToOldPackage:
				if (FileBrowser.<>f__am$cache4 == null)
				{
					FileBrowser.<>f__am$cache4 = new Comparison<FileBrowser.FileAndDirInfo>(FileBrowser.<SortFilesAndDirs>m__4);
				}
				fdlist.Sort(FileBrowser.<>f__am$cache4);
				break;
			case UserPreferences.SortBy.OldToNewPackage:
				if (FileBrowser.<>f__am$cache5 == null)
				{
					FileBrowser.<>f__am$cache5 = new Comparison<FileBrowser.FileAndDirInfo>(FileBrowser.<SortFilesAndDirs>m__5);
				}
				fdlist.Sort(FileBrowser.<>f__am$cache5);
				break;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000A01E7 File Offset: 0x0009E5E7
		// (set) Token: 0x06001C49 RID: 7241 RVA: 0x000A01EF File Offset: 0x0009E5EF
		public bool keepOpen
		{
			get
			{
				return this._keepOpen;
			}
			set
			{
				if (this._keepOpen != value)
				{
					this._keepOpen = value;
					if (this.keepOpenToggle != null)
					{
						this.keepOpenToggle.isOn = this._keepOpen;
					}
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000A0226 File Offset: 0x0009E626
		// (set) Token: 0x06001C4B RID: 7243 RVA: 0x000A022E File Offset: 0x0009E62E
		public bool onlyShowLatest
		{
			get
			{
				return this._onlyShowLatest;
			}
			set
			{
				if (this._onlyShowLatest != value)
				{
					this._onlyShowLatest = value;
					if (this.onlyShowLatestToggle != null)
					{
						this.onlyShowLatestToggle.isOn = this._onlyShowLatest;
					}
					this.UpdateDirectoryList();
				}
			}
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000A026C File Offset: 0x0009E66C
		protected void ResetShortCutsCreators()
		{
			if (this.shortCutsCreators == null)
			{
				this.shortCutsCreators = new HashSet<string>();
			}
			else
			{
				this.shortCutsCreators.Clear();
			}
			this._shortCutsCreatorFilter = "All";
			if (this.shortCutsCreatorFilterPopup != null)
			{
				this.shortCutsCreatorFilterPopup.currentValueNoCallback = this._shortCutsCreatorFilter;
			}
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000A02CC File Offset: 0x0009E6CC
		protected void AddShortCutsCreator(string creator)
		{
			this.shortCutsCreators.Add(creator);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000A02DC File Offset: 0x0009E6DC
		protected void FinalizeShortCutsCreators()
		{
			if (this.shortCutsCreatorsList == null)
			{
				this.shortCutsCreatorsList = new List<string>();
			}
			else
			{
				this.shortCutsCreatorsList.Clear();
			}
			if (this.shortCutsCreators != null)
			{
				foreach (string item in this.shortCutsCreators)
				{
					this.shortCutsCreatorsList.Add(item);
				}
			}
			this.shortCutsCreatorsList.Sort();
			this.shortCutsCreatorsList.Reverse();
			this.shortCutsCreatorsList.Add("All");
			this.shortCutsCreatorsList.Reverse();
			if (this.shortCutsCreatorFilterPopup != null)
			{
				this.shortCutsCreatorFilterPopup.numPopupValues = this.shortCutsCreatorsList.Count;
				for (int i = 0; i < this.shortCutsCreatorsList.Count; i++)
				{
					this.shortCutsCreatorFilterPopup.setPopupValue(i, this.shortCutsCreatorsList[i]);
				}
			}
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000A03FC File Offset: 0x0009E7FC
		public void SetShortCutsCreatorFilter(string creatorFilter)
		{
			if (this._shortCutsCreatorFilter != creatorFilter)
			{
				this._shortCutsCreatorFilter = creatorFilter;
				if (this.shortCutsCreatorFilterPopup != null)
				{
					this.shortCutsCreatorFilterPopup.currentValueNoCallback = this._shortCutsCreatorFilter;
				}
				this.UpdateDirectoryList();
			}
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000A044C File Offset: 0x0009E84C
		protected void ResetShortCutsSearch()
		{
			this._shortCutsSearch = string.Empty;
			this._shortCutsSearchLower = string.Empty;
			if (this.shortCutsSearchField != null)
			{
				this._ignoreShortCutsSearchChange = true;
				this.shortCutsSearchField.text = string.Empty;
				this._ignoreShortCutsSearchChange = false;
			}
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x000A049E File Offset: 0x0009E89E
		public void ClearShortCutsSearchField()
		{
			if (this.shortCutsSearchField != null)
			{
				this.shortCutsSearchField.text = string.Empty;
			}
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x000A04C1 File Offset: 0x0009E8C1
		public void ShortCutsSearchChanged(string s)
		{
			if (this._ignoreShortCutsSearchChange)
			{
				return;
			}
			this._shortCutsSearch = s.Trim();
			this._shortCutsSearchLower = this._shortCutsSearch.ToLowerInvariant();
			this.UpdateDirectoryList();
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x000A04F2 File Offset: 0x0009E8F2
		protected void SetShowHidden(bool b)
		{
			this.showHidden = b;
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000A04FB File Offset: 0x0009E8FB
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x000A0504 File Offset: 0x0009E904
		public bool showHidden
		{
			get
			{
				return this._showHidden;
			}
			set
			{
				if (this._showHidden != value)
				{
					this._showHidden = value;
					if (this.showHiddenToggle != null)
					{
						this.showHiddenToggle.isOn = this._showHidden;
					}
					this.UpdateDirectoryList();
					this.ResetDisplayedPage();
				}
			}
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x000A0552 File Offset: 0x0009E952
		protected void SetOnlyFavorites(bool b)
		{
			this.onlyFavorites = b;
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x000A055B File Offset: 0x0009E95B
		// (set) Token: 0x06001C58 RID: 7256 RVA: 0x000A0563 File Offset: 0x0009E963
		public bool onlyFavorites
		{
			get
			{
				return this._onlyFavorites;
			}
			set
			{
				if (this._onlyFavorites != value)
				{
					this._onlyFavorites = value;
					if (this.onlyFavoritesToggle != null)
					{
						this.onlyFavoritesToggle.isOn = this._onlyFavorites;
					}
					this.ResetDisplayedPage();
				}
			}
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000A05A0 File Offset: 0x0009E9A0
		protected void SetOnlyTemplates(bool b)
		{
			this.onlyTemplates = b;
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x000A05A9 File Offset: 0x0009E9A9
		// (set) Token: 0x06001C5B RID: 7259 RVA: 0x000A05B1 File Offset: 0x0009E9B1
		public bool onlyTemplates
		{
			get
			{
				return this._onlyTemplates;
			}
			set
			{
				if (this._onlyTemplates != value)
				{
					this._onlyTemplates = value;
					if (this.onlyTemplatesToggle != null)
					{
						this.onlyTemplatesToggle.isOn = this._onlyTemplates;
					}
					this.ResetDisplayedPage();
				}
			}
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000A05EE File Offset: 0x0009E9EE
		protected void ResetDisplayedPage()
		{
			this._page = 1;
			this._totalPages = 1;
			base.StartCoroutine(this.DelaySetScroll(1f));
			this.SyncDisplayed();
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001C5D RID: 7261 RVA: 0x000A0616 File Offset: 0x0009EA16
		// (set) Token: 0x06001C5E RID: 7262 RVA: 0x000A061E File Offset: 0x0009EA1E
		public int page
		{
			get
			{
				return this._page;
			}
			set
			{
				if (this._page != value && value <= this._totalPages && value > 0)
				{
					this._page = value;
					base.StartCoroutine(this.DelaySetScroll(1f));
					this.SyncDisplayed();
				}
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000A065E File Offset: 0x0009EA5E
		public void FirstPage()
		{
			this.page = 1;
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000A0667 File Offset: 0x0009EA67
		public void NextPage()
		{
			this.page++;
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000A0677 File Offset: 0x0009EA77
		public void PrevPage()
		{
			this.page--;
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000A0687 File Offset: 0x0009EA87
		protected void SetLimit(float f)
		{
			this.limit = Mathf.FloorToInt(f);
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001C63 RID: 7267 RVA: 0x000A0695 File Offset: 0x0009EA95
		// (set) Token: 0x06001C64 RID: 7268 RVA: 0x000A06A0 File Offset: 0x0009EAA0
		public int limit
		{
			get
			{
				return this._limit;
			}
			set
			{
				if (this._limit != value)
				{
					this._limit = value;
					this._limitXMultiple = this._limit * this.limitMultiple;
					if (this.limitValueText != null)
					{
						this.limitValueText.text = this._limitXMultiple.ToString("F0");
					}
					if (this.limitSlider != null)
					{
						this.limitSlider.value = (float)this._limit;
					}
					this.ResetDisplayedPage();
				}
			}
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000A0728 File Offset: 0x0009EB28
		protected FileButton CreateFileButton(string text, string path, bool dir, bool writeable, bool hidden, bool hiddenModifiable, bool favorite, bool isTemplate, bool isTemplateModifiable)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.fileButtonPrefab, Vector3.zero, Quaternion.identity);
			FileButton component = gameObject.GetComponent<FileButton>();
			string text2 = text;
			if (this.hideExtension)
			{
				text2 = Regex.Replace(text2, "\\.[^\\.]*$", string.Empty);
			}
			if (this.fileRemovePrefix != null)
			{
				string text3 = Regex.Replace(text2, "^" + this.fileRemovePrefix, string.Empty);
				if (text2 != text3)
				{
					component.removedPrefix = this.fileRemovePrefix;
					text2 = text3;
				}
			}
			component.Set(this, text2, path, dir, writeable, hidden, hiddenModifiable, favorite, this.allowUseFileAsTemplateSelect, this.allowUseFileAsTemplateSelect && isTemplate, isTemplateModifiable);
			if (ImageLoaderThreaded.singleton != null)
			{
				Transform transform = null;
				if (component.fileIcon != null)
				{
					transform = component.fileIcon.transform;
				}
				Transform transform2 = null;
				if (component.altIcon != null)
				{
					transform2 = component.altIcon.transform;
				}
				if (transform != null)
				{
					transform.gameObject.SetActive(true);
					if (transform2 != null)
					{
						transform2.gameObject.SetActive(false);
						RawImage altIcon = component.altIcon;
						if (altIcon != null)
						{
							FileEntry fileEntry = FileManager.GetFileEntry(path, false);
							if (fileEntry != null)
							{
								string text4 = Path.GetExtension(fileEntry.Path);
								string text5;
								if (text4 == ".duf")
								{
									text5 = fileEntry.Path + ".png";
									text4 = ".png";
								}
								else if (text4 == ".json" || text4 == ".vac" || text4 == ".vap" || text4 == ".vam" || text4 == ".scene" || text4 == ".assetbundle")
								{
									text5 = Regex.Replace(fileEntry.Path, "\\.(json|vac|vap|vam|scene|assetbundle)$", ".jpg");
									if (!FileManager.FileExists(text5, false, false))
									{
										text5 = Regex.Replace(text5, "\\.jpg$", ".JPG");
									}
									text4 = ".jpg";
								}
								else
								{
									text5 = fileEntry.Path;
								}
								string a = text4.ToLower();
								if (FileManager.FileExists(text5, false, false) && (a == ".jpg" || a == ".jpeg" || a == ".png" || a == ".tif"))
								{
									component.imgPath = text5;
									transform.gameObject.SetActive(false);
									transform2.gameObject.SetActive(true);
									Texture2D cachedThumbnail = ImageLoaderThreaded.singleton.GetCachedThumbnail(text5);
									if (cachedThumbnail != null)
									{
										altIcon.texture = cachedThumbnail;
									}
								}
							}
						}
					}
				}
			}
			return component;
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000A0A1C File Offset: 0x0009EE1C
		private void SyncFileButtonImages()
		{
			foreach (FileButton fb in this.displayedFileButtons)
			{
				this.SyncFileButtonImage(fb);
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000A0A78 File Offset: 0x0009EE78
		private void SyncFileButtonImage(FileButton fb)
		{
			if (fb.imgPath != null && fb.altIcon != null)
			{
				Texture2D cachedThumbnail = ImageLoaderThreaded.singleton.GetCachedThumbnail(fb.imgPath);
				if (cachedThumbnail != null)
				{
					fb.altIcon.texture = cachedThumbnail;
				}
				else
				{
					ImageLoaderThreaded.QueuedImage queuedImage = new ImageLoaderThreaded.QueuedImage();
					queuedImage.imgPath = fb.imgPath;
					queuedImage.width = 512;
					queuedImage.height = 512;
					queuedImage.setSize = true;
					queuedImage.fillBackground = true;
					queuedImage.rawImageToLoad = fb.altIcon;
					ImageLoaderThreaded.singleton.QueueThumbnail(queuedImage);
					this.queuedThumbnails.Add(queuedImage);
				}
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000A0B2C File Offset: 0x0009EF2C
		private void CreateDirectoryButton(string package, string text, string path, int i)
		{
			if (this.dirContent != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.directoryButtonPrefab, Vector3.zero, Quaternion.identity);
				gameObject.GetComponent<RectTransform>().SetParent(this.dirContent, false);
				DirectoryButton component = gameObject.GetComponent<DirectoryButton>();
				component.Set(this, package, this.currentPackageFilter, text, path);
				this.dirButtons.Add(component);
			}
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000A0B98 File Offset: 0x0009EF98
		private void CreateShortCutButton(ShortCut shortCut, int i)
		{
			if (this.shortCutContent != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.shortCutButtonPrefab, Vector3.zero, Quaternion.identity);
				gameObject.GetComponent<RectTransform>().SetParent(this.shortCutContent, false);
				ShortCutButton component = gameObject.GetComponent<ShortCutButton>();
				component.Set(this, shortCut.package, shortCut.packageFilter, shortCut.flatten, shortCut.includeRegularDirsInFlatten, shortCut.displayName, shortCut.path, i);
				this.shortCutButtons.Add(component);
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000A0C20 File Offset: 0x0009F020
		private void CreateDirectorySpacer()
		{
			if (this.dirContent != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.directorySpacerPrefab, Vector3.zero, Quaternion.identity);
				gameObject.GetComponent<RectTransform>().SetParent(this.dirContent, false);
				this.dirSpacers.Add(gameObject);
			}
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000A0C72 File Offset: 0x0009F072
		public void SetTitle(string title)
		{
			if (this.titleText != null)
			{
				this.titleText.text = title;
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000A0C94 File Offset: 0x0009F094
		protected void ShowInternal(bool changeDirectory = true)
		{
			if (this.statusField != null)
			{
				this.statusField.text = string.Empty;
			}
			if (this.fileEntryField != null)
			{
				this.fileEntryField.text = string.Empty;
			}
			if (UserPreferences.singleton != null)
			{
				this.sortBy = UserPreferences.singleton.fileBrowserSortBy;
				this.directoryOption = UserPreferences.singleton.fileBrowserDirectoryOption;
			}
			if (changeDirectory)
			{
				this.GotoDirectory(this.defaultPath, null, false, false);
			}
			this.UpdateUI();
			if (this.overlay)
			{
				this.overlay.SetActive(true);
			}
			this.window.SetActive(true);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000A0D56 File Offset: 0x0009F156
		public void Show(FileBrowserCallback callback, bool changeDirectory = true)
		{
			if (this.showHandler != null)
			{
				this.showHandler(callback, changeDirectory);
			}
			else
			{
				this.ShowInternal(changeDirectory);
				this.callback = callback;
				this.fullCallback = null;
			}
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000A0D8A File Offset: 0x0009F18A
		public void Show(FileBrowserFullCallback fullCallback, bool changeDirectory = true)
		{
			if (this.showFullHandler != null)
			{
				this.showFullHandler(fullCallback, changeDirectory);
			}
			else
			{
				this.ShowInternal(changeDirectory);
				this.callback = null;
				this.fullCallback = fullCallback;
			}
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000A0DBE File Offset: 0x0009F1BE
		public void ClearCurrentPath()
		{
			this.ClearDirectoryScrollPos();
			this.currentPath = string.Empty;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000A0DD4 File Offset: 0x0009F1D4
		public void Hide()
		{
			if (this.window.activeSelf)
			{
				if (this.selected != null)
				{
					this.selected.Unselect();
				}
				this.ClearImageQueue();
				this.SaveDirectoryScrollPos(this.currentPath);
				if (this.clearCurrentPathOnHide)
				{
					this.currentPath = string.Empty;
				}
				this.selected = null;
				if (this.overlay)
				{
					this.overlay.SetActive(false);
				}
				this.window.SetActive(false);
			}
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000A0E64 File Offset: 0x0009F264
		public bool IsHidden()
		{
			return !this.window.activeSelf;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000A0E74 File Offset: 0x0009F274
		public void UpdateUI()
		{
			if (this.cancelButton)
			{
				this.cancelButton.gameObject.SetActive(this.canCancel);
			}
			if (this.currentPathField != null)
			{
				this.currentPathField.text = this.currentPath;
			}
			if (this.searchField != null)
			{
				this.searchField.text = this.search;
			}
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000A0EEC File Offset: 0x0009F2EC
		public Sprite GetFileIcon(string path)
		{
			string b = string.Empty;
			if (path.Contains("."))
			{
				b = path.Substring(path.LastIndexOf('.') + 1);
				for (int i = 0; i < this.fileIcons.Count; i++)
				{
					if (this.fileIcons[i].extension == b)
					{
						return this.fileIcons[i].icon;
					}
				}
				return this.defaultIcon;
			}
			return this.defaultIcon;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000A0F7C File Offset: 0x0009F37C
		public void OnHiddenChange(FileButton fb, bool b)
		{
			string fullPath = fb.fullPath;
			FileEntry fileEntry = FileManager.GetFileEntry(fullPath, true);
			if (fileEntry != null)
			{
				fileEntry.SetHidden(b);
			}
			else
			{
				DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(fullPath, true);
				if (directoryEntry != null)
				{
					directoryEntry.SetHidden(b);
				}
			}
			this.UpdateDirectoryList();
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000A0FC8 File Offset: 0x0009F3C8
		public void OnFavoriteChange(FileButton fb, bool b)
		{
			string fullPath = fb.fullPath;
			FileEntry fileEntry = FileManager.GetFileEntry(fullPath, true);
			if (fileEntry != null)
			{
				fileEntry.SetFavorite(b);
			}
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000A0FF4 File Offset: 0x0009F3F4
		public void OnUseFileAsTemplateChange(FileButton fb, bool b)
		{
			string fullPath = fb.fullPath;
			FileEntry fileEntry = FileManager.GetFileEntry(fullPath, true);
			if (fileEntry != null)
			{
				fileEntry.SetFlagFile("template", b);
			}
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000A1024 File Offset: 0x0009F424
		private IEnumerator RenameProcess()
		{
			yield return null;
			LookInputModule.SelectGameObject(this.renameField.gameObject);
			this.renameField.ActivateInputField();
			yield break;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000A1040 File Offset: 0x0009F440
		public void OnRenameClick(FileButton fb)
		{
			if (this.statusField != null)
			{
				this.statusField.text = string.Empty;
			}
			this.renameFileButton = fb;
			this.OpenRenameDialog();
			if (this.renameField != null)
			{
				string text = fb.text;
				if (text.EndsWith(".json"))
				{
					text = text.Replace(".json", string.Empty);
				}
				else if (text.EndsWith(".vac"))
				{
					text = text.Replace(".vac", string.Empty);
				}
				else if (text.EndsWith(".vap"))
				{
					text = text.Replace(".vap", string.Empty);
				}
				this.renameField.text = text;
				base.StartCoroutine(this.RenameProcess());
			}
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000A1119 File Offset: 0x0009F519
		protected void OpenRenameDialog()
		{
			if (this.renameContainer != null)
			{
				this.renameContainer.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000A1140 File Offset: 0x0009F540
		public void OnRenameConfirm()
		{
			if (this.renameField != null && this.renameField.text != string.Empty && this.renameFileButton != null)
			{
				string text;
				if (this.renameFileButton.removedPrefix != null)
				{
					text = this.currentPath + this.slash + this.renameFileButton.removedPrefix + this.renameField.text;
				}
				else
				{
					text = this.currentPath + this.slash + this.renameField.text;
				}
				if (this.renameFileButton.isDir)
				{
					string fullPath = this.renameFileButton.fullPath;
					try
					{
						FileManager.AssertNotCalledFromPlugin();
						FileManager.MoveDirectory(fullPath, text);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogError(string.Concat(new string[]
						{
							"Could not move directory ",
							fullPath,
							" to ",
							text,
							" Exception: ",
							ex.Message
						}));
						if (this.statusField != null)
						{
							this.statusField.text = ex.Message;
						}
						this.OnRenameCancel();
						return;
					}
				}
				else
				{
					string fullPath2 = this.renameFileButton.fullPath;
					bool flag = false;
					string oldValue = string.Empty;
					if (fullPath2.EndsWith(".json"))
					{
						flag = true;
						oldValue = ".json";
						if (!text.EndsWith(".json"))
						{
							text += ".json";
						}
					}
					else if (fullPath2.EndsWith(".vac"))
					{
						flag = true;
						oldValue = ".vac";
						if (!text.EndsWith(".vac"))
						{
							text += ".vac";
						}
					}
					else if (fullPath2.EndsWith(".vap"))
					{
						flag = true;
						oldValue = ".vap";
						if (!text.EndsWith(".vap"))
						{
							text += ".vap";
						}
					}
					UnityEngine.Debug.Log("Rename file " + fullPath2 + " to " + text);
					try
					{
						FileManager.AssertNotCalledFromPlugin();
						FileManager.MoveFile(fullPath2, text, false);
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogError(string.Concat(new string[]
						{
							"Could not move file ",
							fullPath2,
							" to ",
							text,
							" Exception: ",
							ex2.Message
						}));
						if (this.statusField != null)
						{
							this.statusField.text = ex2.Message;
						}
						this.OnRenameCancel();
						return;
					}
					if (flag)
					{
						string text2 = fullPath2.Replace(oldValue, ".jpg");
						string text3 = text.Replace(oldValue, ".jpg");
						if (FileManager.FileExists(text2, false, false))
						{
							try
							{
								FileManager.MoveFile(text2, text3, true);
							}
							catch (Exception ex3)
							{
								UnityEngine.Debug.LogError(string.Concat(new string[]
								{
									"Could not move file ",
									text2,
									" to ",
									text3,
									" Exception: ",
									ex3.Message
								}));
								if (this.statusField != null)
								{
									this.statusField.text = ex3.Message;
								}
							}
						}
						string text4 = fullPath2 + ".fav";
						string text5 = text + ".fav";
						if (FileManager.FileExists(text4, false, false))
						{
							try
							{
								FileManager.MoveFile(text4, text5, true);
							}
							catch (Exception ex4)
							{
								UnityEngine.Debug.LogError(string.Concat(new string[]
								{
									"Could not move file ",
									text4,
									" to ",
									text5,
									" Exception: ",
									ex4.Message
								}));
								if (this.statusField != null)
								{
									this.statusField.text = ex4.Message;
								}
							}
						}
					}
				}
				this.UpdateFileList();
				this.UpdateDirectoryList();
			}
			this.OnRenameCancel();
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000A155C File Offset: 0x0009F95C
		public void OnRenameCancel()
		{
			if (this.renameContainer != null)
			{
				this.renameContainer.gameObject.SetActive(false);
			}
			this.renameFileButton = null;
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000A1588 File Offset: 0x0009F988
		public void OnDeleteClick(FileButton fb)
		{
			if (this.statusField != null)
			{
				this.statusField.text = string.Empty;
			}
			this.deleteFileButton = fb;
			this.OpenDeleteDialog();
			if (this.deleteField != null)
			{
				string text = fb.text;
				if (text.EndsWith(".json"))
				{
					text = text.Replace(".json", string.Empty);
				}
				else if (text.EndsWith(".vac"))
				{
					text = text.Replace(".vac", string.Empty);
				}
				else if (text.EndsWith(".vap"))
				{
					text = text.Replace(".vap", string.Empty);
				}
				this.deleteField.text = text;
			}
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000A1654 File Offset: 0x0009FA54
		protected void OpenDeleteDialog()
		{
			if (this.deleteContainer != null)
			{
				this.deleteContainer.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000A1678 File Offset: 0x0009FA78
		public void OnDeleteConfirm()
		{
			if (this.deleteFileButton != null)
			{
				string fullPath = this.deleteFileButton.fullPath;
				if (this.deleteFileButton.isDir)
				{
					if (FileManager.DirectoryExists(fullPath, false, false))
					{
						try
						{
							FileManager.AssertNotCalledFromPlugin();
							FileManager.DeleteDirectory(fullPath, true);
						}
						catch (Exception ex)
						{
							UnityEngine.Debug.LogError("Could not delete directory " + fullPath + " Exception: " + ex.Message);
							if (this.statusField != null)
							{
								this.statusField.text = ex.Message;
							}
							this.OnDeleteCancel();
							return;
						}
					}
				}
				else
				{
					if (FileManager.FileExists(fullPath, false, false))
					{
						try
						{
							FileManager.AssertNotCalledFromPlugin();
							FileManager.DeleteFile(fullPath);
						}
						catch (Exception ex2)
						{
							UnityEngine.Debug.LogError("Could not delete file " + fullPath + " Exception: " + ex2.Message);
							if (this.statusField != null)
							{
								this.statusField.text = ex2.Message;
							}
							this.OnDeleteCancel();
							return;
						}
					}
					string text = string.Empty;
					if (fullPath.EndsWith(".json"))
					{
						text = ".json";
					}
					else if (fullPath.EndsWith(".vac"))
					{
						text = ".vac";
					}
					else if (fullPath.EndsWith(".vap"))
					{
						text = ".vap";
					}
					if (text != string.Empty)
					{
						string text2 = fullPath.Replace(text, ".jpg");
						if (FileManager.FileExists(text2, false, false))
						{
							try
							{
								FileManager.DeleteFile(text2);
							}
							catch (Exception ex3)
							{
								UnityEngine.Debug.LogError("Could not delete file " + text2 + " Exception: " + ex3.Message);
								if (this.statusField != null)
								{
									this.statusField.text = ex3.Message;
								}
							}
						}
					}
					string text3 = fullPath + ".fav";
					if (FileManager.FileExists(text3, false, false))
					{
						try
						{
							FileManager.DeleteFile(text3);
						}
						catch (Exception ex4)
						{
							UnityEngine.Debug.LogError("Could not delete file " + text3 + " Exception: " + ex4.Message);
							if (this.statusField != null)
							{
								this.statusField.text = ex4.Message;
							}
						}
					}
				}
				this.UpdateFileList();
				this.UpdateDirectoryList();
			}
			this.OnDeleteCancel();
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000A190C File Offset: 0x0009FD0C
		public void OnDeleteCancel()
		{
			if (this.deleteContainer != null)
			{
				this.deleteContainer.gameObject.SetActive(false);
			}
			this.deleteFileButton = null;
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000A1938 File Offset: 0x0009FD38
		protected string DeterminePathToGoTo(string pathToGoTo)
		{
			DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(pathToGoTo, false);
			if (!this.selectDirectory && directoryEntry != null && directoryEntry is VarDirectoryEntry)
			{
				DirectoryEntry directoryEntry2 = directoryEntry.FindFirstDirectoryWithFiles();
				string uid = directoryEntry.Uid;
				string uid2 = directoryEntry2.Uid;
				if (uid2 != uid)
				{
					string text = uid2.Replace(uid, string.Empty);
					text = text.Replace('/', '\\');
					pathToGoTo += text;
				}
			}
			return pathToGoTo;
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000A19B0 File Offset: 0x0009FDB0
		public void OnFileClick(FileButton fb)
		{
			if (fb.isDir)
			{
				if (!this.selectDirectory)
				{
					string text = fb.fullPath;
					VarDirectoryEntry varDirectoryEntry = FileManager.GetVarDirectoryEntry(text);
					if (this.currentPackageFilter != null && varDirectoryEntry != null && varDirectoryEntry.Package.RootDirectory == varDirectoryEntry)
					{
						text = text + "\\" + this.currentPackageFilter;
					}
					this.GotoDirectory(this.DeterminePathToGoTo(text), this.currentPackageFilter, false, false);
				}
				else
				{
					this.SelectFile(fb);
				}
			}
			else
			{
				this.SelectFile(fb);
			}
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x000A1A42 File Offset: 0x0009FE42
		public void OnFilePointerEnter(FileButton fb)
		{
			if (this.fileHighlightField != null)
			{
				this.fileHighlightField.text = fb.fullPath;
			}
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x000A1A66 File Offset: 0x0009FE66
		public void OnFilePointerExit(FileButton fb)
		{
			if (this.fileHighlightField != null)
			{
				this.fileHighlightField.text = string.Empty;
			}
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000A1A89 File Offset: 0x0009FE89
		public void OnDirectoryClick(DirectoryButton db)
		{
			this.GotoDirectory(db.fullPath, db.packageFilter, false, false);
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000A1AA0 File Offset: 0x0009FEA0
		public void OnShortCutClick(int i)
		{
			if (i >= this.shortCutButtons.Count)
			{
				UnityEngine.Debug.LogError("uFileBrowser: Button index is bigger than array, something went wrong.");
				return;
			}
			this.GotoDirectory(this.DeterminePathToGoTo(this.shortCutButtons[i].fullPath), this.shortCutButtons[i].packageFilter, this.shortCutButtons[i].flatten, this.shortCutButtons[i].includeRegularDirsInFlatten);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000A1B1C File Offset: 0x0009FF1C
		private IEnumerator DelaySetScroll(float scrollPos)
		{
			yield return null;
			this.filesScrollRect.verticalNormalizedPosition = scrollPos;
			yield break;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000A1B40 File Offset: 0x0009FF40
		private void SaveDirectoryScrollPos(string path)
		{
			if (this.filesScrollRect != null)
			{
				if (this.directoryScrollPositions == null)
				{
					this.directoryScrollPositions = new Dictionary<string, float>();
				}
				string text = this.currentPath;
				if (!text.EndsWith("\\"))
				{
					text += "\\";
				}
				string key = this.fileFormat + ":" + text;
				float num;
				if (this.directoryScrollPositions.TryGetValue(key, out num))
				{
					this.directoryScrollPositions.Remove(key);
				}
				float verticalNormalizedPosition = this.filesScrollRect.verticalNormalizedPosition;
				this.directoryScrollPositions.Add(key, verticalNormalizedPosition);
			}
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000A1BE4 File Offset: 0x0009FFE4
		private void ClearDirectoryScrollPos()
		{
			if (this.currentPath != null && this.fileFormat != null && this.directoryScrollPositions != null)
			{
				string text = this.currentPath;
				if (!text.EndsWith("\\"))
				{
					text += "\\";
				}
				string key = this.fileFormat + ":" + text;
				float num;
				if (this.directoryScrollPositions.TryGetValue(key, out num))
				{
					this.directoryScrollPositions.Remove(key);
				}
			}
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000A1C67 File Offset: 0x000A0067
		private void OpenInExplorer()
		{
			SuperController.singleton.OpenFolderInExplorer(this.currentPath);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000A1C79 File Offset: 0x000A0079
		private void GoToPromotionalLink()
		{
			if (this.promotionalButtonText != null)
			{
				SuperController.singleton.OpenLinkInBrowser(this.promotionalButtonText.text);
			}
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000A1CA1 File Offset: 0x000A00A1
		private void OpenPackageInManager()
		{
			if (this.currentPackageUid != null && this.currentPackageUid != string.Empty)
			{
				SuperController.singleton.OpenPackageInManager(this.currentPackageUid);
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000A1CD4 File Offset: 0x000A00D4
		private void OpenOnHub()
		{
			if (this.currentPackageUid != null && this.currentPackageUid != string.Empty)
			{
				VarPackage package = FileManager.GetPackage(this.currentPackageUid);
				if (package != null)
				{
					package.OpenOnHub();
				}
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000A1D1C File Offset: 0x000A011C
		public void GotoDirectory(string path, string pkgFilter = null, bool flatten = false, bool includeRegularDirs = false)
		{
			if (this.gotoDirectoryHandler != null)
			{
				this.gotoDirectoryHandler(path, pkgFilter, flatten, includeRegularDirs);
			}
			else
			{
				if (path == this.currentPath && path != string.Empty && pkgFilter == this.currentPackageFilter && this.useFlatten == flatten && this.includeRegularDirsInFlatten == includeRegularDirs)
				{
					this.SyncDisplayed();
					return;
				}
				this.currentPackageFilter = pkgFilter;
				this.useFlatten = flatten;
				this.includeRegularDirsInFlatten = includeRegularDirs;
				this.SaveDirectoryScrollPos(this.currentPath);
				if (string.IsNullOrEmpty(path))
				{
					this.currentPath = string.Empty;
				}
				else if (!FileManager.DirectoryExists(path, false, false) && !flatten)
				{
					UnityEngine.Debug.LogError("uFileBrowser: Directory doesn't exist:\n" + path);
					this.currentPath = string.Empty;
				}
				else
				{
					this.currentPath = path;
				}
				if (this.currentPathField)
				{
					this.currentPathField.text = this.currentPath;
				}
				if (this.selectDirectory && this.fileEntryField != null)
				{
					this.fileEntryField.text = string.Empty;
				}
				this.selected = null;
				DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(path, false);
				if (this.openInExplorerButton != null)
				{
					if (directoryEntry is VarDirectoryEntry)
					{
						this.openInExplorerButton.gameObject.SetActive(false);
					}
					else
					{
						this.openInExplorerButton.gameObject.SetActive(true);
					}
				}
				if (this.openPackageButton != null && this.promotionalButton != null && this.promotionalButtonText != null)
				{
					if (directoryEntry is VarDirectoryEntry)
					{
						VarDirectoryEntry varDirectoryEntry = directoryEntry as VarDirectoryEntry;
						VarPackage package = varDirectoryEntry.Package;
						this.currentPackageUid = package.Uid;
						this.openPackageButton.gameObject.SetActive(true);
						if (this.openOnHubButton != null)
						{
							this.openOnHubButton.gameObject.SetActive(package.IsOnHub);
						}
						if (!SuperController.singleton.promotionalDisabled && package.PromotionalLink != null && package.PromotionalLink != string.Empty)
						{
							this.promotionalButton.gameObject.SetActive(true);
							this.promotionalButtonText.text = package.PromotionalLink;
						}
						else
						{
							this.promotionalButton.gameObject.SetActive(false);
						}
					}
					else
					{
						this.currentPackageUid = null;
						if (this.openOnHubButton != null)
						{
							this.openOnHubButton.gameObject.SetActive(false);
						}
						this.openPackageButton.gameObject.SetActive(false);
						this.promotionalButton.gameObject.SetActive(false);
					}
				}
				this.UpdateFileList();
				this.UpdateDirectoryList();
				if (this.filesScrollRect != null)
				{
					float scrollPos = 1f;
					if (this.directoryScrollPositions != null)
					{
						string text = this.currentPath;
						if (!text.EndsWith("\\"))
						{
							text += "\\";
						}
						string key = this.fileFormat + ":" + text;
						if (!this.directoryScrollPositions.TryGetValue(key, out scrollPos))
						{
							scrollPos = 1f;
						}
					}
					base.StartCoroutine(this.DelaySetScroll(scrollPos));
				}
				if (this.gotoDirectoryCallback != null)
				{
					this.gotoDirectoryCallback(this.currentPath);
				}
			}
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000A20AC File Offset: 0x000A04AC
		private void SelectFile(FileButton fb)
		{
			if (fb == this.selected && this.selectDirectory && fb.isDir)
			{
				this.GotoDirectory(fb.fullPath, this.currentPackageFilter, false, false);
				return;
			}
			if (!fb.isDir && this.selectDirectory)
			{
				return;
			}
			if (this.selected != null)
			{
				this.selected.Unselect();
			}
			this.selected = fb;
			fb.Select();
			if (this.fileEntryField != null)
			{
				this.fileEntryField.text = this.selected.text;
				if (this.fileEntryField.text.EndsWith(".json"))
				{
					this.fileEntryField.text = this.fileEntryField.text.Replace(".json", string.Empty);
				}
				else if (this.fileEntryField.text.EndsWith(".vac"))
				{
					this.fileEntryField.text = this.fileEntryField.text.Replace(".vac", string.Empty);
				}
			}
			if (this.selectOnClick)
			{
				this.SelectButtonClicked();
			}
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000A21F0 File Offset: 0x000A05F0
		public void PathFieldEndEdit()
		{
			if (this.currentPathField != null)
			{
				if (FileManager.DirectoryExists(this.currentPathField.text, false, false))
				{
					this.GotoDirectory(this.currentPathField.text, null, false, false);
				}
				else
				{
					this.currentPathField.text = this.currentPath;
				}
			}
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000A2250 File Offset: 0x000A0650
		public void SearchChanged()
		{
			if (!this.searchField || this.ignoreSearchChange)
			{
				return;
			}
			this.search = this.searchField.text.Trim();
			this.searchLower = this.search.ToLowerInvariant();
			this.ResetDisplayedPage();
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000A22A6 File Offset: 0x000A06A6
		public void SearchCancelClick()
		{
			this.searchField.text = string.Empty;
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000A22B8 File Offset: 0x000A06B8
		protected void ClearSearch()
		{
			this.search = string.Empty;
			this.searchLower = string.Empty;
			this.ignoreSearchChange = true;
			this.searchField.text = string.Empty;
			this.ignoreSearchChange = false;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x000A22F0 File Offset: 0x000A06F0
		public void SetTextEntry(bool b)
		{
			if (b)
			{
				this.selectOnClick = false;
				if (this.selectButton != null)
				{
					this.selectButton.gameObject.SetActive(true);
				}
				if (this.fileEntryField != null)
				{
					this.fileEntryField.gameObject.SetActive(true);
				}
				if (this.keepOpenToggle != null)
				{
					this.keepOpenToggle.gameObject.SetActive(false);
				}
			}
			else
			{
				this.selectOnClick = true;
				if (this.selectButton != null)
				{
					this.selectButton.gameObject.SetActive(false);
				}
				if (this.fileEntryField != null)
				{
					this.fileEntryField.gameObject.SetActive(false);
				}
				if (this.keepOpenToggle != null)
				{
					this.keepOpenToggle.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x000A23E4 File Offset: 0x000A07E4
		private IEnumerator ActivateFileNameFieldProcess()
		{
			yield return null;
			LookInputModule.SelectGameObject(this.fileEntryField.gameObject);
			this.fileEntryField.ActivateInputField();
			yield break;
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x000A23FF File Offset: 0x000A07FF
		public void ActivateFileNameField()
		{
			if (this.fileEntryField != null)
			{
				base.StartCoroutine(this.ActivateFileNameFieldProcess());
			}
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000A2420 File Offset: 0x000A0820
		public void SelectButtonClicked()
		{
			if (!this.selectOnClick && this.fileEntryField != null)
			{
				if (this.fileEntryField.text != string.Empty)
				{
					string path = this.currentPath + this.slash + this.fileEntryField.text;
					if (!this._keepOpen)
					{
						this.Hide();
					}
					if (this.callback != null)
					{
						this.callback(path);
					}
					if (this.fullCallback != null)
					{
						this.fullCallback(path, !this._keepOpen);
					}
				}
				else if (this.selectDirectory)
				{
					string path2 = this.currentPath;
					this.Hide();
					if (this.callback != null)
					{
						this.callback(path2);
					}
					if (this.fullCallback != null)
					{
						this.fullCallback(path2, true);
					}
				}
			}
			else if (this.selected != null && ((this.selected.isDir && this.selectDirectory) || (!this.selected.isDir && !this.selectDirectory)))
			{
				string fullPath = this.selected.fullPath;
				if (!this._keepOpen)
				{
					this.Hide();
				}
				if (this.callback != null)
				{
					this.callback(fullPath);
				}
				if (this.fullCallback != null)
				{
					this.fullCallback(fullPath, !this._keepOpen);
				}
			}
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x000A25B4 File Offset: 0x000A09B4
		public void CancelButtonClicked()
		{
			if (!this.canCancel)
			{
				return;
			}
			this.Hide();
			if (this.callback != null)
			{
				this.callback(string.Empty);
			}
			if (this.fullCallback != null)
			{
				this.fullCallback(string.Empty, true);
			}
		}

		// Token: 0x06001C98 RID: 7320 RVA: 0x000A260C File Offset: 0x000A0A0C
		private void SyncSort()
		{
			if (this.cachedFiles != null && this.cachedDirs != null)
			{
				if (this.lastCacheUseFlatten)
				{
					this.sortedFilesAndDirs = this.cachedFiles;
					this.SortFilesAndDirs(this.sortedFilesAndDirs);
				}
				else
				{
					switch (this.directoryOption)
					{
					case UserPreferences.DirectoryOption.ShowFirst:
						this.sortedFilesAndDirs = new List<FileBrowser.FileAndDirInfo>();
						this.SortFilesAndDirs(this.cachedDirs);
						this.sortedFilesAndDirs.AddRange(this.cachedDirs);
						this.SortFilesAndDirs(this.cachedFiles);
						this.sortedFilesAndDirs.AddRange(this.cachedFiles);
						break;
					case UserPreferences.DirectoryOption.ShowLast:
						this.sortedFilesAndDirs = new List<FileBrowser.FileAndDirInfo>();
						this.SortFilesAndDirs(this.cachedFiles);
						this.sortedFilesAndDirs.AddRange(this.cachedFiles);
						this.SortFilesAndDirs(this.cachedDirs);
						this.sortedFilesAndDirs.AddRange(this.cachedDirs);
						break;
					case UserPreferences.DirectoryOption.Intermix:
						this.sortedFilesAndDirs = new List<FileBrowser.FileAndDirInfo>();
						this.sortedFilesAndDirs.AddRange(this.cachedFiles);
						this.sortedFilesAndDirs.AddRange(this.cachedDirs);
						this.SortFilesAndDirs(this.sortedFilesAndDirs);
						break;
					case UserPreferences.DirectoryOption.Hide:
						this.sortedFilesAndDirs = this.cachedFiles;
						this.SortFilesAndDirs(this.sortedFilesAndDirs);
						break;
					}
				}
			}
			this.ResetDisplayedPage();
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000A276E File Offset: 0x000A0B6E
		protected void HideButton(FileButton fb)
		{
			if (this.manageContentTransform && this.displayedFileButtons.Contains(fb))
			{
				fb.gameObject.SetActive(false);
				this.displayedFileButtons.Remove(fb);
			}
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000A27A8 File Offset: 0x000A0BA8
		private void SyncDisplayed()
		{
			if (this.sortedFilesAndDirs != null)
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				Vector2 vector = this.cellSize + this.cellSpacing;
				if (!this.manageContentTransform)
				{
					foreach (FileButton fileButton in this.displayedFileButtons)
					{
						fileButton.gameObject.SetActive(false);
						fileButton.transform.SetParent(null, false);
					}
					this.displayedFileButtons.Clear();
				}
				int num5 = (this._page - 1) * this._limitXMultiple + 1;
				int num6 = this._page * this._limitXMultiple;
				foreach (FileBrowser.FileAndDirInfo fileAndDirInfo in this.sortedFilesAndDirs)
				{
					FileEntry fileEntry = fileAndDirInfo.FileEntry;
					DirectoryEntry directoryEntry = fileAndDirInfo.DirectoryEntry;
					FileButton button = fileAndDirInfo.button;
					if (button != null)
					{
						if (fileEntry != null)
						{
							if (!this._showHidden && fileEntry.IsHidden())
							{
								this.HideButton(button);
								continue;
							}
							if (this._onlyFavorites && !fileAndDirInfo.isFavorite)
							{
								this.HideButton(button);
								continue;
							}
							if (this._onlyTemplates && !fileAndDirInfo.isTemplate)
							{
								this.HideButton(button);
								continue;
							}
							if (!string.IsNullOrEmpty(this.searchLower) && !fileEntry.UidLowerInvariant.Contains(this.searchLower))
							{
								VarFileEntry varFileEntry = fileEntry as VarFileEntry;
								if (varFileEntry == null)
								{
									this.HideButton(button);
									continue;
								}
								if (!varFileEntry.Package.UidLowerInvariant.Contains(this.searchLower))
								{
									this.HideButton(button);
									continue;
								}
							}
						}
						else if (directoryEntry != null)
						{
							if (!this._showHidden && directoryEntry.IsHidden())
							{
								this.HideButton(button);
								continue;
							}
							if (!string.IsNullOrEmpty(this.searchLower) && !directoryEntry.UidLowerInvariant.Contains(this.searchLower))
							{
								VarDirectoryEntry varDirectoryEntry = directoryEntry as VarDirectoryEntry;
								if (varDirectoryEntry == null)
								{
									this.HideButton(button);
									continue;
								}
								if (!varDirectoryEntry.Package.UidLowerInvariant.Contains(this.searchLower))
								{
									this.HideButton(button);
									continue;
								}
							}
						}
						num++;
						if (num < num5 || num > num6)
						{
							this.HideButton(button);
						}
						else
						{
							if (this.manageContentTransform)
							{
								button.gameObject.SetActive(true);
								RectTransform rectTransform = button.rectTransform;
								if (rectTransform != null)
								{
									Vector2 anchoredPosition;
									anchoredPosition.x = (float)num4 * vector.x;
									anchoredPosition.y = (float)(-(float)num3) * vector.y;
									rectTransform.anchoredPosition = anchoredPosition;
									num4++;
									if (num4 == this.columnCount)
									{
										num4 = 0;
										num3++;
									}
									num2++;
								}
							}
							else
							{
								button.gameObject.SetActive(true);
								button.transform.SetParent(this.fileContent, false);
								num2++;
							}
							this.displayedFileButtons.Add(button);
							this.SyncFileButtonImage(button);
						}
					}
				}
				if (this.manageContentTransform)
				{
					float y = (float)(num3 + 1) * vector.y;
					Vector2 sizeDelta = this.fileContent.sizeDelta;
					sizeDelta.y = y;
					this.fileContent.sizeDelta = sizeDelta;
				}
				if (this.showingCountText != null)
				{
					if (num6 > num)
					{
						num6 = num;
					}
					this.showingCountText.text = string.Concat(new object[]
					{
						num5,
						"-",
						num6,
						" of ",
						num
					});
				}
				this._totalPages = (num - 1) / this._limitXMultiple + 1;
			}
		}

		// Token: 0x06001C9B RID: 7323 RVA: 0x000A2BFC File Offset: 0x000A0FFC
		protected List<FileBrowser.FileAndDirInfo> FilterFormat(List<FileBrowser.FileAndDirInfo> files, bool skipFileFormatCheck = false)
		{
			List<FileBrowser.FileAndDirInfo> list = files;
			if (this.fileRemovePrefix != null && this.fileRemovePrefix != string.Empty)
			{
				List<FileBrowser.FileAndDirInfo> list2 = new List<FileBrowser.FileAndDirInfo>();
				for (int i = 0; i < list.Count; i++)
				{
					if (Regex.IsMatch(list[i].Name, "^" + this.fileRemovePrefix))
					{
						list2.Add(list[i]);
					}
				}
				list = list2;
			}
			if (!string.IsNullOrEmpty(this.fileFormat) && !skipFileFormatCheck)
			{
				List<FileBrowser.FileAndDirInfo> list3 = new List<FileBrowser.FileAndDirInfo>();
				string[] array = this.fileFormat.Split(new char[]
				{
					'|'
				});
				for (int j = 0; j < list.Count; j++)
				{
					string a = string.Empty;
					if (list[j].Name.Contains("."))
					{
						a = list[j].Name.Substring(list[j].Name.LastIndexOf('.') + 1).ToLowerInvariant();
					}
					for (int k = 0; k < array.Length; k++)
					{
						if (a == array[k].Trim().ToLowerInvariant())
						{
							list3.Add(list[j]);
						}
					}
				}
				list = list3;
			}
			return list;
		}

		// Token: 0x06001C9C RID: 7324 RVA: 0x000A2D64 File Offset: 0x000A1164
		protected void UpdateFileListCacheThreadSafe()
		{
			List<FileBrowser.FileAndDirInfo> list = new List<FileBrowser.FileAndDirInfo>();
			List<FileBrowser.FileAndDirInfo> list2 = new List<FileBrowser.FileAndDirInfo>();
			if (string.IsNullOrEmpty(this.currentPath))
			{
				try
				{
					for (int i = 0; i < this.drives.Count; i++)
					{
						DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(this.drives[i], false);
						FileBrowser.FileAndDirInfo item = new FileBrowser.FileAndDirInfo(directoryEntry, string.Empty);
						list2.Add(item);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("uFileBrowser: " + ex);
					this.threadHadException = true;
					this.threadException = ex.Message;
				}
			}
			else if (this.useFlatten)
			{
				List<FileEntry> list3 = new List<FileEntry>();
				try
				{
					if (this.fileFormat != null)
					{
						string regex = "\\.(" + this.fileFormat + ")$";
						if (this.includeRegularDirsInFlatten)
						{
							FileManager.FindAllFilesRegex(this.currentPath, regex, list3, false);
						}
						else
						{
							FileManager.FindVarFilesRegex(this.currentPath, regex, list3);
						}
					}
					else if (this.includeRegularDirsInFlatten)
					{
						FileManager.FindAllFiles(this.currentPath, "*", list3, false);
					}
					else
					{
						FileManager.FindVarFiles(this.currentPath, "*", list3);
					}
					foreach (FileEntry fileEntry in list3)
					{
						if (fileEntry is VarFileEntry)
						{
							VarFileEntry varFileEntry = fileEntry as VarFileEntry;
							if (!varFileEntry.Package.isNewestEnabledVersion)
							{
								continue;
							}
						}
						if (fileEntry.Exists || FileManager.IsPackage(fileEntry.Path))
						{
							if (!this.forceOnlyShowTemplates || fileEntry.HasFlagFile("template"))
							{
								FileBrowser.FileAndDirInfo item2 = new FileBrowser.FileAndDirInfo(fileEntry);
								list.Add(item2);
							}
						}
						else
						{
							UnityEngine.Debug.LogError("Unable to read file " + fileEntry.FullPath);
							this.threadHadException = true;
							this.threadException = "Unable to read file " + fileEntry.FullPath;
						}
					}
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogError("uFileBrowser: " + ex2);
					this.threadHadException = true;
					this.threadException = ex2.Message;
				}
				list = this.FilterFormat(list, true);
			}
			else
			{
				DirectoryEntry directoryEntry2 = FileManager.GetDirectoryEntry(this.currentPath, false);
				if (!this.selectDirectory || !this.showFiles)
				{
					if (this.selectDirectory)
					{
						goto IL_38B;
					}
				}
				try
				{
					List<FileEntry> files = directoryEntry2.Files;
					foreach (FileEntry fileEntry2 in files)
					{
						if (fileEntry2.Exists || FileManager.IsPackage(fileEntry2.Path))
						{
							if (!this.forceOnlyShowTemplates || fileEntry2.HasFlagFile("template"))
							{
								FileBrowser.FileAndDirInfo item3 = new FileBrowser.FileAndDirInfo(fileEntry2);
								list.Add(item3);
							}
						}
						else
						{
							UnityEngine.Debug.LogError("Unable to read file " + fileEntry2.FullPath);
							this.threadHadException = true;
							this.threadException = "Unable to read file " + fileEntry2.FullPath;
						}
					}
				}
				catch (Exception ex3)
				{
					UnityEngine.Debug.LogError("uFileBrowser: " + ex3);
					this.threadHadException = true;
					this.threadException = ex3.Message;
				}
				list = this.FilterFormat(list, false);
				IL_38B:
				if (this.showDirs)
				{
					try
					{
						string empty = this.currentPath;
						if (empty == ".")
						{
							empty = string.Empty;
						}
						List<DirectoryEntry> subDirectories = directoryEntry2.SubDirectories;
						foreach (DirectoryEntry directoryEntry3 in subDirectories)
						{
							if (directoryEntry3 is VarDirectoryEntry)
							{
								if (this.browseVarFilesAsDirectories)
								{
									VarDirectoryEntry varDirectoryEntry = directoryEntry3 as VarDirectoryEntry;
									if (this.currentPackageFilter != null && varDirectoryEntry.Package.RootDirectory == varDirectoryEntry)
									{
										if (varDirectoryEntry.Package.HasMatchingDirectories(this.currentPackageFilter))
										{
											FileBrowser.FileAndDirInfo item4 = new FileBrowser.FileAndDirInfo(directoryEntry3, empty);
											list2.Add(item4);
										}
									}
									else
									{
										FileBrowser.FileAndDirInfo item5 = new FileBrowser.FileAndDirInfo(directoryEntry3, empty);
										list2.Add(item5);
									}
								}
							}
							else
							{
								FileBrowser.FileAndDirInfo item6 = new FileBrowser.FileAndDirInfo(directoryEntry3, empty);
								list2.Add(item6);
							}
						}
					}
					catch (Exception ex4)
					{
						UnityEngine.Debug.LogError("uFileBrowser: " + ex4.Message);
						this.threadHadException = true;
						this.threadException = ex4.Message;
					}
				}
			}
			this.cachedFiles = list;
			this.cachedDirs = list2;
			this.lastCacheFileFormat = this.fileFormat;
			this.lastCacheFileRemovePrefix = this.fileRemovePrefix;
			this.lastCacheUseFlatten = this.useFlatten;
			this.lastCacheIncludeRegularDirsInFlatten = this.includeRegularDirsInFlatten;
			this.lastCachePackageFilter = this.currentPackageFilter;
			this.lastCacheTime = DateTime.Now;
			this.lastCacheShowDirs = this.showDirs;
			this.lastCacheForceOnlyShowTemplates = this.forceOnlyShowTemplates;
			this.lastCacheDir = this.currentPath;
			this.cacheDirty = false;
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000A3368 File Offset: 0x000A1768
		private void UpdateFileList()
		{
			if (!this.cacheDirty)
			{
				if (this.currentPath != this.lastCacheDir)
				{
					this.cacheDirty = true;
				}
				else if (this.useFlatten != this.lastCacheUseFlatten)
				{
					this.cacheDirty = true;
				}
				else if (this.fileFormat != this.lastCacheFileFormat)
				{
					this.cacheDirty = true;
				}
				else if (this.fileRemovePrefix != this.lastCacheFileRemovePrefix)
				{
					this.cacheDirty = true;
				}
				else if (FileManager.lastPackageRefreshTime > this.lastCacheTime)
				{
					this.cacheDirty = true;
				}
				else if (this.lastCachePackageFilter != this.currentPackageFilter)
				{
					this.cacheDirty = true;
				}
				else if (this.useFlatten && FileManager.CheckIfDirectoryChanged(this.currentPath, this.lastCacheTime, true))
				{
					this.cacheDirty = true;
				}
				else if (this.useFlatten && this.includeRegularDirsInFlatten != this.lastCacheIncludeRegularDirsInFlatten)
				{
					this.cacheDirty = true;
				}
				else if (!this.useFlatten && FileManager.CheckIfDirectoryChanged(this.currentPath, this.lastCacheTime, false))
				{
					this.cacheDirty = true;
				}
				else if (!this.useFlatten && this.showDirs != this.lastCacheShowDirs)
				{
					this.cacheDirty = true;
				}
				else if (this.forceOnlyShowTemplates != this.lastCacheForceOnlyShowTemplates)
				{
					this.cacheDirty = true;
				}
			}
			if (this.cacheDirty)
			{
				int num = 0;
				this.ClearSearch();
				if (this.cachedFiles != null)
				{
					num += this.cachedFiles.Count;
					foreach (FileBrowser.FileAndDirInfo fileAndDirInfo in this.cachedFiles)
					{
						if (fileAndDirInfo.button != null)
						{
							UnityEngine.Object.Destroy(fileAndDirInfo.button.gameObject);
						}
					}
				}
				if (this.cachedDirs != null)
				{
					num += this.cachedDirs.Count;
					foreach (FileBrowser.FileAndDirInfo fileAndDirInfo2 in this.cachedDirs)
					{
						if (fileAndDirInfo2.button != null)
						{
							UnityEngine.Object.Destroy(fileAndDirInfo2.button.gameObject);
						}
					}
				}
				this.displayedFileButtons.Clear();
				this.threadHadException = false;
				this.UpdateFileListCacheThreadSafe();
				if (this.threadHadException && this.statusField != null)
				{
					this.statusField.text = this.threadException;
				}
				if (this.dirOption != null)
				{
					this.dirOption.gameObject.SetActive(this.showDirs);
				}
				this.ClearImageQueue();
				int num2 = 0;
				foreach (FileBrowser.FileAndDirInfo fileAndDirInfo3 in this.cachedFiles)
				{
					fileAndDirInfo3.button = this.CreateFileButton(fileAndDirInfo3.Name, fileAndDirInfo3.FullName, false, fileAndDirInfo3.isWriteable, fileAndDirInfo3.isHidden, fileAndDirInfo3.isHiddenModifiable, fileAndDirInfo3.isFavorite, fileAndDirInfo3.isTemplate, fileAndDirInfo3.isTemplateModifiable);
					fileAndDirInfo3.button.gameObject.SetActive(false);
					if (this.manageContentTransform)
					{
						fileAndDirInfo3.button.transform.SetParent(this.fileContent, false);
					}
					num2++;
				}
				foreach (FileBrowser.FileAndDirInfo fileAndDirInfo4 in this.cachedDirs)
				{
					fileAndDirInfo4.button = this.CreateFileButton(fileAndDirInfo4.Name, fileAndDirInfo4.FullName, true, fileAndDirInfo4.isWriteable, fileAndDirInfo4.isHidden, fileAndDirInfo4.isHiddenModifiable, false, false, false);
					fileAndDirInfo4.button.gameObject.SetActive(false);
					if (this.manageContentTransform)
					{
						fileAndDirInfo4.button.transform.SetParent(this.fileContent, false);
					}
					num2++;
				}
			}
			this.SyncSort();
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000A381C File Offset: 0x000A1C1C
		private void UpdateDirectoryList()
		{
			if (!this.directoryButtonPrefab)
			{
				return;
			}
			if (this.dirButtons == null)
			{
				this.dirButtons = new List<DirectoryButton>();
			}
			else
			{
				for (int i = 0; i < this.dirButtons.Count; i++)
				{
					UnityEngine.Object.Destroy(this.dirButtons[i].gameObject);
				}
				this.dirButtons.Clear();
			}
			if (this.shortCutButtons == null)
			{
				this.shortCutButtons = new List<ShortCutButton>();
			}
			else
			{
				for (int j = 0; j < this.shortCutButtons.Count; j++)
				{
					UnityEngine.Object.Destroy(this.shortCutButtons[j].gameObject);
				}
				this.shortCutButtons.Clear();
			}
			if (this.dirSpacers == null)
			{
				this.dirSpacers = new List<GameObject>();
			}
			else
			{
				for (int k = 0; k < this.dirSpacers.Count; k++)
				{
					UnityEngine.Object.Destroy(this.dirSpacers[k]);
				}
				this.dirSpacers.Clear();
			}
			if (this.shortCutsOverflowIndicator != null)
			{
				this.shortCutsOverflowIndicator.SetActive(false);
			}
			if (this.shortCuts != null)
			{
				foreach (ShortCut shortCut in this.shortCuts)
				{
					if (shortCut.package != null && shortCut.package != string.Empty)
					{
						if (!string.IsNullOrEmpty(this._shortCutsSearchLower) && !shortCut.package.ToLowerInvariant().Contains(this._shortCutsSearchLower))
						{
							continue;
						}
						if (shortCut.creator != null && shortCut.creator != string.Empty)
						{
							this.AddShortCutsCreator(shortCut.creator);
							if (this._shortCutsCreatorFilter != "All" && this._shortCutsCreatorFilter != shortCut.creator)
							{
								continue;
							}
						}
					}
					if (this._showHidden || !shortCut.isHidden)
					{
						if (!this._onlyShowLatest || shortCut.isLatest)
						{
							this.CreateShortCutButton(shortCut, this.shortCutButtons.Count);
							if (this.shortCutButtons.Count >= 100)
							{
								if (this.shortCutsOverflowIndicator != null)
								{
									this.shortCutsOverflowIndicator.SetActive(true);
								}
								break;
							}
						}
					}
				}
			}
			this.FinalizeShortCutsCreators();
			if (!this.useFlatten && !string.IsNullOrEmpty(this.currentPath))
			{
				if (Regex.IsMatch(this.currentPath, "^[A-Za-z]:\\\\"))
				{
					this.CreateDirectoryButton("My Computer", string.Empty, string.Empty, this.dirButtons.Count);
					this.CreateDirectorySpacer();
				}
				else if (this.showInstallFolderInDirectoryList && !FileManager.IsDirectoryInPackage(this.currentPath))
				{
					this.CreateDirectoryButton("Root", string.Empty, ".", this.dirButtons.Count);
				}
				string[] array = this.currentPath.Split(new char[]
				{
					this.slash[0]
				});
				string text = string.Empty;
				for (int l = 0; l < array.Length; l++)
				{
					if (!string.IsNullOrEmpty(array[l]) && array[l] != ".")
					{
						if (text == string.Empty)
						{
							text = array[l];
						}
						else
						{
							text = text + this.slash + array[l];
						}
						string text2 = string.Empty;
						string text3 = string.Empty;
						string packageFolder = FileManager.PackageFolder;
						if (array[l].Contains(packageFolder))
						{
							text3 = Regex.Replace(array[l], ".*:/", string.Empty);
							text2 = Regex.Replace(array[l], ":/.*", string.Empty);
							text2 = text2.Replace(packageFolder, string.Empty);
							text2 = text2.TrimStart(new char[]
							{
								'/',
								'\\'
							});
						}
						else
						{
							text3 = array[l] + this.slash;
						}
						this.CreateDirectoryButton(text2, text3, text, this.dirButtons.Count);
					}
				}
			}
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x000A3CC0 File Offset: 0x000A20C0
		private void Awake()
		{
			this.slash = Path.DirectorySeparatorChar.ToString();
			this.drives = new List<string>(Directory.GetLogicalDrives());
			this.displayedFileButtons = new HashSet<FileButton>();
			this.queuedThumbnails = new HashSet<ImageLoaderThreaded.QueuedImage>();
			if (this.renameFieldAction != null)
			{
				InputFieldAction inputFieldAction = this.renameFieldAction;
				inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.OnRenameConfirm));
			}
			if (this.onlyShowLatestToggle != null)
			{
				this.onlyShowLatestToggle.isOn = this._onlyShowLatest;
			}
			if (this.shortCutsCreatorFilterPopup != null)
			{
				this.shortCutsCreatorFilterPopup.currentValueNoCallback = this._shortCutsCreatorFilter;
				UIPopup uipopup = this.shortCutsCreatorFilterPopup;
				uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetShortCutsCreatorFilter));
			}
			if (this.shortCutsSearchField != null)
			{
				this.shortCutsSearchField.text = this._shortCutsSearch;
				this.shortCutsSearchField.onValueChanged.AddListener(new UnityAction<string>(this.ShortCutsSearchChanged));
			}
			if (this.shortCutsSearchClearButton != null)
			{
				this.shortCutsSearchClearButton.onClick.AddListener(new UnityAction(this.ClearShortCutsSearchField));
			}
			if (this.showHiddenToggle != null)
			{
				this.showHiddenToggle.isOn = this._showHidden;
				this.showHiddenToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetShowHidden));
			}
			if (this.onlyFavoritesToggle != null)
			{
				this.onlyFavoritesToggle.isOn = this._onlyFavorites;
				this.onlyFavoritesToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetOnlyFavorites));
			}
			if (this.onlyTemplatesToggle != null)
			{
				this.onlyTemplatesToggle.isOn = this._onlyTemplates;
				this.onlyTemplatesToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetOnlyTemplates));
			}
			if (this.limitSlider != null)
			{
				this.limitSlider.value = (float)this._limit;
				this.limitSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetLimit));
			}
			this._limitXMultiple = this._limit * this.limitMultiple;
			if (this.limitValueText != null)
			{
				this.limitValueText.text = this._limitXMultiple.ToString("F0");
			}
			if (this.openInExplorerButton != null)
			{
				this.openInExplorerButton.onClick.AddListener(new UnityAction(this.OpenInExplorer));
			}
			if (this.openPackageButton != null)
			{
				this.openPackageButton.onClick.AddListener(new UnityAction(this.OpenPackageInManager));
			}
			if (this.openOnHubButton != null)
			{
				this.openOnHubButton.onClick.AddListener(new UnityAction(this.OpenOnHub));
			}
			if (this.promotionalButton != null)
			{
				this.promotionalButton.onClick.AddListener(new UnityAction(this.GoToPromotionalLink));
			}
			if (this.sortByPopup != null)
			{
				this.sortByPopup.currentValueNoCallback = this._sortBy.ToString();
				UIPopup uipopup2 = this.sortByPopup;
				uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetSortBy));
			}
			if (this.directoryOptionPopup != null)
			{
				this.directoryOptionPopup.currentValueNoCallback = this._directoryOption.ToString();
				UIPopup uipopup3 = this.directoryOptionPopup;
				uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetDirectoryOption));
			}
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000A40AE File Offset: 0x000A24AE
		[CompilerGenerated]
		private static int <SortFilesAndDirs>m__0(FileBrowser.FileAndDirInfo a, FileBrowser.FileAndDirInfo b)
		{
			return a.Name.CompareTo(b.Name);
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000A40C1 File Offset: 0x000A24C1
		[CompilerGenerated]
		private static int <SortFilesAndDirs>m__1(FileBrowser.FileAndDirInfo a, FileBrowser.FileAndDirInfo b)
		{
			return b.Name.CompareTo(a.Name);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000A40D4 File Offset: 0x000A24D4
		[CompilerGenerated]
		private static int <SortFilesAndDirs>m__2(FileBrowser.FileAndDirInfo a, FileBrowser.FileAndDirInfo b)
		{
			return b.LastWriteTime.CompareTo(a.LastWriteTime);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000A40F8 File Offset: 0x000A24F8
		[CompilerGenerated]
		private static int <SortFilesAndDirs>m__3(FileBrowser.FileAndDirInfo a, FileBrowser.FileAndDirInfo b)
		{
			return a.LastWriteTime.CompareTo(b.LastWriteTime);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000A411C File Offset: 0x000A251C
		[CompilerGenerated]
		private static int <SortFilesAndDirs>m__4(FileBrowser.FileAndDirInfo a, FileBrowser.FileAndDirInfo b)
		{
			return b.LastWriteTimePackage.CompareTo(a.LastWriteTimePackage);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000A4140 File Offset: 0x000A2540
		[CompilerGenerated]
		private static int <SortFilesAndDirs>m__5(FileBrowser.FileAndDirInfo a, FileBrowser.FileAndDirInfo b)
		{
			return a.LastWriteTimePackage.CompareTo(b.LastWriteTimePackage);
		}

		// Token: 0x040017DE RID: 6110
		public string defaultPath = string.Empty;

		// Token: 0x040017DF RID: 6111
		public bool selectDirectory;

		// Token: 0x040017E0 RID: 6112
		public bool showFiles;

		// Token: 0x040017E1 RID: 6113
		public bool showDirs = true;

		// Token: 0x040017E2 RID: 6114
		public bool canCancel = true;

		// Token: 0x040017E3 RID: 6115
		public bool selectOnClick = true;

		// Token: 0x040017E4 RID: 6116
		public bool browseVarFilesAsDirectories = true;

		// Token: 0x040017E5 RID: 6117
		public bool showInstallFolderInDirectoryList;

		// Token: 0x040017E6 RID: 6118
		public bool forceOnlyShowTemplates;

		// Token: 0x040017E7 RID: 6119
		public bool allowUseFileAsTemplateSelect;

		// Token: 0x040017E8 RID: 6120
		public string fileFormat = string.Empty;

		// Token: 0x040017E9 RID: 6121
		public bool hideExtension;

		// Token: 0x040017EA RID: 6122
		public string fileRemovePrefix;

		// Token: 0x040017EB RID: 6123
		[SerializeField]
		[HideInInspector]
		private string currentPath;

		// Token: 0x040017EC RID: 6124
		[SerializeField]
		[HideInInspector]
		private string search;

		// Token: 0x040017ED RID: 6125
		[HideInInspector]
		private string searchLower;

		// Token: 0x040017EE RID: 6126
		[SerializeField]
		[HideInInspector]
		private string slash;

		// Token: 0x040017EF RID: 6127
		[SerializeField]
		[HideInInspector]
		private List<string> drives;

		// Token: 0x040017F0 RID: 6128
		private List<DirectoryButton> dirButtons;

		// Token: 0x040017F1 RID: 6129
		private List<ShortCutButton> shortCutButtons;

		// Token: 0x040017F2 RID: 6130
		private List<GameObject> dirSpacers;

		// Token: 0x040017F3 RID: 6131
		private FileButton selected;

		// Token: 0x040017F4 RID: 6132
		private FileBrowserCallback callback;

		// Token: 0x040017F5 RID: 6133
		private FileBrowserFullCallback fullCallback;

		// Token: 0x040017F6 RID: 6134
		public List<ShortCut> shortCuts;

		// Token: 0x040017F7 RID: 6135
		public bool manageContentTransform;

		// Token: 0x040017F8 RID: 6136
		public Vector2 cellSize;

		// Token: 0x040017F9 RID: 6137
		public Vector2 cellSpacing;

		// Token: 0x040017FA RID: 6138
		public int columnCount;

		// Token: 0x040017FB RID: 6139
		public UIPopup directoryOptionPopup;

		// Token: 0x040017FC RID: 6140
		protected UserPreferences.DirectoryOption _directoryOption;

		// Token: 0x040017FD RID: 6141
		public UIPopup sortByPopup;

		// Token: 0x040017FE RID: 6142
		protected UserPreferences.SortBy _sortBy = UserPreferences.SortBy.NewToOld;

		// Token: 0x040017FF RID: 6143
		public GameObject overlay;

		// Token: 0x04001800 RID: 6144
		public GameObject window;

		// Token: 0x04001801 RID: 6145
		public GameObject fileButtonPrefab;

		// Token: 0x04001802 RID: 6146
		public GameObject directoryButtonPrefab;

		// Token: 0x04001803 RID: 6147
		public GameObject directorySpacerPrefab;

		// Token: 0x04001804 RID: 6148
		public Text titleText;

		// Token: 0x04001805 RID: 6149
		public RectTransform fileContent;

		// Token: 0x04001806 RID: 6150
		public ScrollRect filesScrollRect;

		// Token: 0x04001807 RID: 6151
		public RectTransform dirContent;

		// Token: 0x04001808 RID: 6152
		public RectTransform dirOption;

		// Token: 0x04001809 RID: 6153
		public GameObject shortCutButtonPrefab;

		// Token: 0x0400180A RID: 6154
		public RectTransform shortCutContent;

		// Token: 0x0400180B RID: 6155
		public Button openInExplorerButton;

		// Token: 0x0400180C RID: 6156
		public Button openPackageButton;

		// Token: 0x0400180D RID: 6157
		public Button openOnHubButton;

		// Token: 0x0400180E RID: 6158
		public Button promotionalButton;

		// Token: 0x0400180F RID: 6159
		public Text promotionalButtonText;

		// Token: 0x04001810 RID: 6160
		public Toggle keepOpenToggle;

		// Token: 0x04001811 RID: 6161
		[SerializeField]
		protected bool _keepOpen;

		// Token: 0x04001812 RID: 6162
		public Toggle onlyShowLatestToggle;

		// Token: 0x04001813 RID: 6163
		protected bool _onlyShowLatest = true;

		// Token: 0x04001814 RID: 6164
		protected HashSet<string> shortCutsCreators;

		// Token: 0x04001815 RID: 6165
		protected List<string> shortCutsCreatorsList;

		// Token: 0x04001816 RID: 6166
		public UIPopup shortCutsCreatorFilterPopup;

		// Token: 0x04001817 RID: 6167
		public GameObject shortCutsOverflowIndicator;

		// Token: 0x04001818 RID: 6168
		protected string _shortCutsCreatorFilter = "All";

		// Token: 0x04001819 RID: 6169
		public InputField shortCutsSearchField;

		// Token: 0x0400181A RID: 6170
		public Button shortCutsSearchClearButton;

		// Token: 0x0400181B RID: 6171
		protected string _shortCutsSearch;

		// Token: 0x0400181C RID: 6172
		protected string _shortCutsSearchLower;

		// Token: 0x0400181D RID: 6173
		protected bool _ignoreShortCutsSearchChange;

		// Token: 0x0400181E RID: 6174
		public Toggle showHiddenToggle;

		// Token: 0x0400181F RID: 6175
		protected bool _showHidden;

		// Token: 0x04001820 RID: 6176
		public Toggle onlyFavoritesToggle;

		// Token: 0x04001821 RID: 6177
		protected bool _onlyFavorites;

		// Token: 0x04001822 RID: 6178
		public Toggle onlyTemplatesToggle;

		// Token: 0x04001823 RID: 6179
		protected bool _onlyTemplates;

		// Token: 0x04001824 RID: 6180
		protected int _totalPages;

		// Token: 0x04001825 RID: 6181
		protected int _page = 1;

		// Token: 0x04001826 RID: 6182
		public Slider limitSlider;

		// Token: 0x04001827 RID: 6183
		public Text limitValueText;

		// Token: 0x04001828 RID: 6184
		[SerializeField]
		protected int _limit = 5;

		// Token: 0x04001829 RID: 6185
		public int limitMultiple = 100;

		// Token: 0x0400182A RID: 6186
		protected int _limitXMultiple = 500;

		// Token: 0x0400182B RID: 6187
		public Text showingCountText;

		// Token: 0x0400182C RID: 6188
		public InputField currentPathField;

		// Token: 0x0400182D RID: 6189
		public InputField searchField;

		// Token: 0x0400182E RID: 6190
		public Button searchCancelButton;

		// Token: 0x0400182F RID: 6191
		public Button cancelButton;

		// Token: 0x04001830 RID: 6192
		public Button selectButton;

		// Token: 0x04001831 RID: 6193
		public Text selectButtonText;

		// Token: 0x04001832 RID: 6194
		public Transform renameContainer;

		// Token: 0x04001833 RID: 6195
		public InputField renameField;

		// Token: 0x04001834 RID: 6196
		public InputFieldAction renameFieldAction;

		// Token: 0x04001835 RID: 6197
		protected FileButton renameFileButton;

		// Token: 0x04001836 RID: 6198
		public Transform deleteContainer;

		// Token: 0x04001837 RID: 6199
		public InputField deleteField;

		// Token: 0x04001838 RID: 6200
		protected FileButton deleteFileButton;

		// Token: 0x04001839 RID: 6201
		public Text statusField;

		// Token: 0x0400183A RID: 6202
		public Text fileHighlightField;

		// Token: 0x0400183B RID: 6203
		public InputField fileEntryField;

		// Token: 0x0400183C RID: 6204
		public Sprite folderIcon;

		// Token: 0x0400183D RID: 6205
		public Sprite defaultIcon;

		// Token: 0x0400183E RID: 6206
		public List<FileIcon> fileIcons = new List<FileIcon>();

		// Token: 0x0400183F RID: 6207
		private HashSet<ImageLoaderThreaded.QueuedImage> queuedThumbnails;

		// Token: 0x04001840 RID: 6208
		public FileBrowser.ShowHandler showHandler;

		// Token: 0x04001841 RID: 6209
		public FileBrowser.ShowFullHandler showFullHandler;

		// Token: 0x04001842 RID: 6210
		public bool clearCurrentPathOnHide = true;

		// Token: 0x04001843 RID: 6211
		protected Dictionary<string, float> directoryScrollPositions;

		// Token: 0x04001844 RID: 6212
		protected string currentPackageUid;

		// Token: 0x04001845 RID: 6213
		protected string currentPackageFilter;

		// Token: 0x04001846 RID: 6214
		protected bool useFlatten;

		// Token: 0x04001847 RID: 6215
		protected bool includeRegularDirsInFlatten;

		// Token: 0x04001848 RID: 6216
		public FileBrowser.GotoDirectoryCallback gotoDirectoryCallback;

		// Token: 0x04001849 RID: 6217
		public FileBrowser.GotoDirectoryHandler gotoDirectoryHandler;

		// Token: 0x0400184A RID: 6218
		protected bool ignoreSearchChange;

		// Token: 0x0400184B RID: 6219
		protected List<FileBrowser.FileAndDirInfo> sortedFilesAndDirs;

		// Token: 0x0400184C RID: 6220
		protected HashSet<FileButton> displayedFileButtons;

		// Token: 0x0400184D RID: 6221
		protected bool cacheDirty = true;

		// Token: 0x0400184E RID: 6222
		protected string lastCacheDir;

		// Token: 0x0400184F RID: 6223
		protected bool lastCacheUseFlatten;

		// Token: 0x04001850 RID: 6224
		protected bool lastCacheIncludeRegularDirsInFlatten;

		// Token: 0x04001851 RID: 6225
		protected bool lastCacheShowDirs;

		// Token: 0x04001852 RID: 6226
		protected bool lastCacheForceOnlyShowTemplates;

		// Token: 0x04001853 RID: 6227
		protected DateTime lastCacheTime;

		// Token: 0x04001854 RID: 6228
		protected string lastCacheFileFormat;

		// Token: 0x04001855 RID: 6229
		protected string lastCacheFileRemovePrefix;

		// Token: 0x04001856 RID: 6230
		protected string lastCachePackageFilter;

		// Token: 0x04001857 RID: 6231
		protected List<FileBrowser.FileAndDirInfo> cachedFiles;

		// Token: 0x04001858 RID: 6232
		protected List<FileBrowser.FileAndDirInfo> cachedDirs;

		// Token: 0x04001859 RID: 6233
		protected bool threadHadException;

		// Token: 0x0400185A RID: 6234
		protected string threadException;

		// Token: 0x0400185B RID: 6235
		[CompilerGenerated]
		private static Comparison<FileBrowser.FileAndDirInfo> <>f__am$cache0;

		// Token: 0x0400185C RID: 6236
		[CompilerGenerated]
		private static Comparison<FileBrowser.FileAndDirInfo> <>f__am$cache1;

		// Token: 0x0400185D RID: 6237
		[CompilerGenerated]
		private static Comparison<FileBrowser.FileAndDirInfo> <>f__am$cache2;

		// Token: 0x0400185E RID: 6238
		[CompilerGenerated]
		private static Comparison<FileBrowser.FileAndDirInfo> <>f__am$cache3;

		// Token: 0x0400185F RID: 6239
		[CompilerGenerated]
		private static Comparison<FileBrowser.FileAndDirInfo> <>f__am$cache4;

		// Token: 0x04001860 RID: 6240
		[CompilerGenerated]
		private static Comparison<FileBrowser.FileAndDirInfo> <>f__am$cache5;

		// Token: 0x0200046B RID: 1131
		public class FileAndDirInfo
		{
			// Token: 0x06001CA6 RID: 7334 RVA: 0x000A4164 File Offset: 0x000A2564
			public FileAndDirInfo(DirectoryEntry dirEntry, string currentPath)
			{
				this.DirectoryEntry = dirEntry;
				this._isDirectory = true;
				this._isWriteable = (!(dirEntry is VarDirectoryEntry) && FileManager.IsSecureWritePath(dirEntry.FullPath));
				this.Name = dirEntry.Name;
				if (currentPath != string.Empty)
				{
					this.FullName = currentPath + "\\" + this.Name;
				}
				else
				{
					this.FullName = this.Name;
				}
				this.LastWriteTime = dirEntry.LastWriteTime;
				this.LastWriteTimePackage = this.LastWriteTime;
			}

			// Token: 0x06001CA7 RID: 7335 RVA: 0x000A4200 File Offset: 0x000A2600
			public FileAndDirInfo(FileEntry fEntry)
			{
				this.FileEntry = fEntry;
				this._isDirectory = false;
				this._isWriteable = (!(fEntry is VarFileEntry) && FileManager.IsSecureWritePath(fEntry.FullPath));
				this.Name = fEntry.Name;
				this.FullName = fEntry.Uid;
				this.LastWriteTime = fEntry.LastWriteTime;
				VarFileEntry varFileEntry = fEntry as VarFileEntry;
				if (varFileEntry != null)
				{
					this.LastWriteTimePackage = varFileEntry.Package.LastWriteTime;
				}
				else
				{
					this.LastWriteTimePackage = this.LastWriteTime;
				}
			}

			// Token: 0x17000319 RID: 793
			// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x000A4293 File Offset: 0x000A2693
			public bool isWriteable
			{
				get
				{
					return this._isWriteable;
				}
			}

			// Token: 0x1700031A RID: 794
			// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x000A429B File Offset: 0x000A269B
			public bool isTemplate
			{
				get
				{
					return this.FileEntry != null && this.FileEntry.HasFlagFile("template");
				}
			}

			// Token: 0x1700031B RID: 795
			// (get) Token: 0x06001CAA RID: 7338 RVA: 0x000A42BC File Offset: 0x000A26BC
			public bool isTemplateModifiable
			{
				get
				{
					if (this.FileEntry != null)
					{
						VarFileEntry varFileEntry = this.FileEntry as VarFileEntry;
						return varFileEntry == null || varFileEntry.IsFlagFileModifiable("template");
					}
					return true;
				}
			}

			// Token: 0x06001CAB RID: 7339 RVA: 0x000A42F5 File Offset: 0x000A26F5
			public void SetTemplate(bool b)
			{
				if (this._isWriteable && this.FileEntry != null)
				{
					this.FileEntry.SetFlagFile("template", b);
				}
			}

			// Token: 0x1700031C RID: 796
			// (get) Token: 0x06001CAC RID: 7340 RVA: 0x000A431E File Offset: 0x000A271E
			public bool isFavorite
			{
				get
				{
					return this.FileEntry != null && this.FileEntry.IsFavorite();
				}
			}

			// Token: 0x06001CAD RID: 7341 RVA: 0x000A4338 File Offset: 0x000A2738
			public void SetFavorite(bool b)
			{
				if (this._isWriteable && this.FileEntry != null)
				{
					this.FileEntry.SetFavorite(b);
				}
			}

			// Token: 0x1700031D RID: 797
			// (get) Token: 0x06001CAE RID: 7342 RVA: 0x000A435C File Offset: 0x000A275C
			public bool isHidden
			{
				get
				{
					if (this.FileEntry != null)
					{
						return this.FileEntry.IsHidden();
					}
					return this.DirectoryEntry != null && this.DirectoryEntry.IsHidden();
				}
			}

			// Token: 0x1700031E RID: 798
			// (get) Token: 0x06001CAF RID: 7343 RVA: 0x000A4390 File Offset: 0x000A2790
			public bool isHiddenModifiable
			{
				get
				{
					if (this.FileEntry != null)
					{
						VarFileEntry varFileEntry = this.FileEntry as VarFileEntry;
						return varFileEntry == null || varFileEntry.IsHiddenModifiable();
					}
					return true;
				}
			}

			// Token: 0x06001CB0 RID: 7344 RVA: 0x000A43C4 File Offset: 0x000A27C4
			public void SetHidden(bool b)
			{
				if (this._isWriteable && this.FileEntry != null)
				{
					this.FileEntry.SetHidden(b);
				}
			}

			// Token: 0x1700031F RID: 799
			// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x000A43E8 File Offset: 0x000A27E8
			public bool isDirectory
			{
				get
				{
					return this._isDirectory;
				}
			}

			// Token: 0x17000320 RID: 800
			// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x000A43F0 File Offset: 0x000A27F0
			// (set) Token: 0x06001CB3 RID: 7347 RVA: 0x000A43F8 File Offset: 0x000A27F8
			public FileEntry FileEntry
			{
				[CompilerGenerated]
				get
				{
					return this.<FileEntry>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<FileEntry>k__BackingField = value;
				}
			}

			// Token: 0x17000321 RID: 801
			// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x000A4401 File Offset: 0x000A2801
			// (set) Token: 0x06001CB5 RID: 7349 RVA: 0x000A4409 File Offset: 0x000A2809
			public DirectoryEntry DirectoryEntry
			{
				[CompilerGenerated]
				get
				{
					return this.<DirectoryEntry>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<DirectoryEntry>k__BackingField = value;
				}
			}

			// Token: 0x17000322 RID: 802
			// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x000A4412 File Offset: 0x000A2812
			// (set) Token: 0x06001CB7 RID: 7351 RVA: 0x000A441A File Offset: 0x000A281A
			public string Name
			{
				[CompilerGenerated]
				get
				{
					return this.<Name>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<Name>k__BackingField = value;
				}
			}

			// Token: 0x17000323 RID: 803
			// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x000A4423 File Offset: 0x000A2823
			// (set) Token: 0x06001CB9 RID: 7353 RVA: 0x000A442B File Offset: 0x000A282B
			public string FullName
			{
				[CompilerGenerated]
				get
				{
					return this.<FullName>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<FullName>k__BackingField = value;
				}
			}

			// Token: 0x17000324 RID: 804
			// (get) Token: 0x06001CBA RID: 7354 RVA: 0x000A4434 File Offset: 0x000A2834
			// (set) Token: 0x06001CBB RID: 7355 RVA: 0x000A443C File Offset: 0x000A283C
			public DateTime LastWriteTime
			{
				[CompilerGenerated]
				get
				{
					return this.<LastWriteTime>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<LastWriteTime>k__BackingField = value;
				}
			}

			// Token: 0x17000325 RID: 805
			// (get) Token: 0x06001CBC RID: 7356 RVA: 0x000A4445 File Offset: 0x000A2845
			// (set) Token: 0x06001CBD RID: 7357 RVA: 0x000A444D File Offset: 0x000A284D
			public DateTime LastWriteTimePackage
			{
				[CompilerGenerated]
				get
				{
					return this.<LastWriteTimePackage>k__BackingField;
				}
				[CompilerGenerated]
				protected set
				{
					this.<LastWriteTimePackage>k__BackingField = value;
				}
			}

			// Token: 0x04001861 RID: 6241
			protected bool _isWriteable;

			// Token: 0x04001862 RID: 6242
			protected bool _isDirectory;

			// Token: 0x04001863 RID: 6243
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private FileEntry <FileEntry>k__BackingField;

			// Token: 0x04001864 RID: 6244
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private DirectoryEntry <DirectoryEntry>k__BackingField;

			// Token: 0x04001865 RID: 6245
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <Name>k__BackingField;

			// Token: 0x04001866 RID: 6246
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private string <FullName>k__BackingField;

			// Token: 0x04001867 RID: 6247
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private DateTime <LastWriteTime>k__BackingField;

			// Token: 0x04001868 RID: 6248
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private DateTime <LastWriteTimePackage>k__BackingField;

			// Token: 0x04001869 RID: 6249
			public FileButton button;
		}

		// Token: 0x0200046C RID: 1132
		// (Invoke) Token: 0x06001CBF RID: 7359
		public delegate void ShowHandler(FileBrowserCallback callback, bool changeDirectory);

		// Token: 0x0200046D RID: 1133
		// (Invoke) Token: 0x06001CC3 RID: 7363
		public delegate void ShowFullHandler(FileBrowserFullCallback callback, bool changeDirectory);

		// Token: 0x0200046E RID: 1134
		// (Invoke) Token: 0x06001CC7 RID: 7367
		public delegate void GotoDirectoryCallback(string path);

		// Token: 0x0200046F RID: 1135
		// (Invoke) Token: 0x06001CCB RID: 7371
		public delegate void GotoDirectoryHandler(string path, string pkgFilter, bool flatten, bool includeRegularDirs);

		// Token: 0x02000F65 RID: 3941
		[CompilerGenerated]
		private sealed class <RenameProcess>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600739E RID: 29598 RVA: 0x000A4456 File Offset: 0x000A2856
			[DebuggerHidden]
			public <RenameProcess>c__Iterator0()
			{
			}

			// Token: 0x0600739F RID: 29599 RVA: 0x000A4460 File Offset: 0x000A2860
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					LookInputModule.SelectGameObject(this.renameField.gameObject);
					this.renameField.ActivateInputField();
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010EF RID: 4335
			// (get) Token: 0x060073A0 RID: 29600 RVA: 0x000A44D8 File Offset: 0x000A28D8
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010F0 RID: 4336
			// (get) Token: 0x060073A1 RID: 29601 RVA: 0x000A44E0 File Offset: 0x000A28E0
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073A2 RID: 29602 RVA: 0x000A44E8 File Offset: 0x000A28E8
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073A3 RID: 29603 RVA: 0x000A44F8 File Offset: 0x000A28F8
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006786 RID: 26502
			internal FileBrowser $this;

			// Token: 0x04006787 RID: 26503
			internal object $current;

			// Token: 0x04006788 RID: 26504
			internal bool $disposing;

			// Token: 0x04006789 RID: 26505
			internal int $PC;
		}

		// Token: 0x02000F66 RID: 3942
		[CompilerGenerated]
		private sealed class <DelaySetScroll>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073A4 RID: 29604 RVA: 0x000A44FF File Offset: 0x000A28FF
			[DebuggerHidden]
			public <DelaySetScroll>c__Iterator1()
			{
			}

			// Token: 0x060073A5 RID: 29605 RVA: 0x000A4508 File Offset: 0x000A2908
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.filesScrollRect.verticalNormalizedPosition = scrollPos;
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010F1 RID: 4337
			// (get) Token: 0x060073A6 RID: 29606 RVA: 0x000A4571 File Offset: 0x000A2971
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010F2 RID: 4338
			// (get) Token: 0x060073A7 RID: 29607 RVA: 0x000A4579 File Offset: 0x000A2979
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073A8 RID: 29608 RVA: 0x000A4581 File Offset: 0x000A2981
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073A9 RID: 29609 RVA: 0x000A4591 File Offset: 0x000A2991
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400678A RID: 26506
			internal float scrollPos;

			// Token: 0x0400678B RID: 26507
			internal FileBrowser $this;

			// Token: 0x0400678C RID: 26508
			internal object $current;

			// Token: 0x0400678D RID: 26509
			internal bool $disposing;

			// Token: 0x0400678E RID: 26510
			internal int $PC;
		}

		// Token: 0x02000F67 RID: 3943
		[CompilerGenerated]
		private sealed class <ActivateFileNameFieldProcess>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073AA RID: 29610 RVA: 0x000A4598 File Offset: 0x000A2998
			[DebuggerHidden]
			public <ActivateFileNameFieldProcess>c__Iterator2()
			{
			}

			// Token: 0x060073AB RID: 29611 RVA: 0x000A45A0 File Offset: 0x000A29A0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					LookInputModule.SelectGameObject(this.fileEntryField.gameObject);
					this.fileEntryField.ActivateInputField();
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010F3 RID: 4339
			// (get) Token: 0x060073AC RID: 29612 RVA: 0x000A4618 File Offset: 0x000A2A18
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010F4 RID: 4340
			// (get) Token: 0x060073AD RID: 29613 RVA: 0x000A4620 File Offset: 0x000A2A20
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073AE RID: 29614 RVA: 0x000A4628 File Offset: 0x000A2A28
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073AF RID: 29615 RVA: 0x000A4638 File Offset: 0x000A2A38
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400678F RID: 26511
			internal FileBrowser $this;

			// Token: 0x04006790 RID: 26512
			internal object $current;

			// Token: 0x04006791 RID: 26513
			internal bool $disposing;

			// Token: 0x04006792 RID: 26514
			internal int $PC;
		}
	}
}
