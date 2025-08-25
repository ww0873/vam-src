using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000CF9 RID: 3321
	public class PresetManagerControl : JSONStorable
	{
		// Token: 0x060064FA RID: 25850 RVA: 0x001A80A4 File Offset: 0x001A64A4
		public PresetManagerControl()
		{
		}

		// Token: 0x060064FB RID: 25851 RVA: 0x001A80BC File Offset: 0x001A64BC
		protected void SyncPresetBrowsePath(string url)
		{
			if (this.pm != null && url != null && url != string.Empty)
			{
				string presetNameFromFilePath = this.pm.GetPresetNameFromFilePath(url);
				if (presetNameFromFilePath != null)
				{
					this.presetNameJSON.val = presetNameFromFilePath;
					if ((this.forceLoadOnPresetBrowsePathSync || (this.loadPresetOnSelectJSON != null && this.loadPresetOnSelectJSON.val)) && this.pm.CheckPresetExistance())
					{
						if (this.forceUseMergeLoad)
						{
							this.MergeLoadPreset();
						}
						else if (this.forceUseRegularLoad)
						{
							this.LoadPreset();
						}
						else if (this.useMergeLoadJSON != null && this.useMergeLoadJSON.val)
						{
							this.MergeLoadPreset();
						}
						else
						{
							this.LoadPreset();
						}
						this.presetNameJSON.val = presetNameFromFilePath;
					}
				}
			}
		}

		// Token: 0x060064FC RID: 25852 RVA: 0x001A81A9 File Offset: 0x001A65A9
		public void OpenPresetBrowsePathInExplorer()
		{
			SuperController.singleton.OpenFolderInExplorer(this.presetBrowsePathJSON.val);
		}

		// Token: 0x060064FD RID: 25853 RVA: 0x001A81C0 File Offset: 0x001A65C0
		protected void SyncPresetLoadButton()
		{
			if (this.pm.CheckPresetExistance())
			{
				if (this.autoTypeLoadPresetAction != null && this.autoTypeLoadPresetAction.dynamicButton != null && this.autoTypeLoadPresetAction.dynamicButton.button != null)
				{
					this.autoTypeLoadPresetAction.dynamicButton.button.interactable = true;
				}
			}
			else if (this.autoTypeLoadPresetAction != null && this.autoTypeLoadPresetAction.dynamicButton != null && this.autoTypeLoadPresetAction.dynamicButton.button != null)
			{
				this.autoTypeLoadPresetAction.dynamicButton.button.interactable = false;
			}
		}

		// Token: 0x060064FE RID: 25854 RVA: 0x001A8288 File Offset: 0x001A6688
		protected void SyncPresetStoreButton()
		{
			if (this.pm.CheckPresetReadyForStore())
			{
				if (this.pm.CheckPresetExistance())
				{
					if (this.storePresetAction != null && this.storePresetAction.dynamicButton != null)
					{
						this.storePresetAction.dynamicButton.buttonColor = Color.red;
						if (this.storePresetAction.dynamicButton.button != null)
						{
							this.storePresetAction.dynamicButton.button.interactable = true;
						}
						if (this.storePresetAction.dynamicButton.buttonText != null)
						{
							this.storePresetAction.dynamicButton.buttonText.text = "Overwrite Preset";
						}
					}
					if (this.storePresetWithScreenshotAction != null && this.storePresetWithScreenshotAction.dynamicButton != null)
					{
						this.storePresetWithScreenshotAction.dynamicButton.buttonColor = Color.red;
						if (this.storePresetWithScreenshotAction.dynamicButton.button != null)
						{
							this.storePresetWithScreenshotAction.dynamicButton.button.interactable = true;
						}
						if (this.storePresetWithScreenshotAction.dynamicButton.buttonText != null)
						{
							this.storePresetWithScreenshotAction.dynamicButton.buttonText.text = "Overwrite Preset";
						}
					}
					if (this.storeOverlayPresetAction != null && this.storeOverlayPresetAction.dynamicButton != null)
					{
						this.storeOverlayPresetAction.dynamicButton.buttonColor = Color.red;
						if (this.storeOverlayPresetAction.dynamicButton.button != null)
						{
							this.storeOverlayPresetAction.dynamicButton.button.interactable = true;
						}
						if (this.storeOverlayPresetAction.dynamicButton.buttonText != null)
						{
							this.storeOverlayPresetAction.dynamicButton.buttonText.text = "Overwrite Overlay Preset";
						}
					}
					if (this.storeOverlayPresetWithScreenshotAction != null && this.storeOverlayPresetWithScreenshotAction.dynamicButton != null)
					{
						this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonColor = Color.red;
						if (this.storeOverlayPresetWithScreenshotAction.dynamicButton.button != null)
						{
							this.storeOverlayPresetWithScreenshotAction.dynamicButton.button.interactable = true;
						}
						if (this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonText != null)
						{
							this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonText.text = "Overwrite Overlay Preset";
						}
					}
				}
				else
				{
					if (this.storePresetAction != null && this.storePresetAction.dynamicButton != null)
					{
						this.storePresetAction.dynamicButton.buttonColor = Color.green;
						if (this.storePresetAction.dynamicButton.button != null)
						{
							this.storePresetAction.dynamicButton.button.interactable = true;
						}
						if (this.storePresetAction.dynamicButton.buttonText != null)
						{
							this.storePresetAction.dynamicButton.buttonText.text = "Create New Preset";
						}
					}
					if (this.storePresetWithScreenshotAction != null && this.storePresetWithScreenshotAction.dynamicButton != null)
					{
						this.storePresetWithScreenshotAction.dynamicButton.buttonColor = Color.green;
						if (this.storePresetWithScreenshotAction.dynamicButton.button != null)
						{
							this.storePresetWithScreenshotAction.dynamicButton.button.interactable = true;
						}
						if (this.storePresetWithScreenshotAction.dynamicButton.buttonText != null)
						{
							this.storePresetWithScreenshotAction.dynamicButton.buttonText.text = "Create New Preset";
						}
					}
					if (this.storeOverlayPresetAction != null && this.storeOverlayPresetAction.dynamicButton != null)
					{
						this.storeOverlayPresetAction.dynamicButton.buttonColor = Color.green;
						if (this.storeOverlayPresetAction.dynamicButton.button != null)
						{
							this.storeOverlayPresetAction.dynamicButton.button.interactable = true;
						}
						if (this.storeOverlayPresetAction.dynamicButton.buttonText != null)
						{
							this.storeOverlayPresetAction.dynamicButton.buttonText.text = "Create New Overlay Preset";
						}
					}
					if (this.storeOverlayPresetWithScreenshotAction != null && this.storeOverlayPresetWithScreenshotAction.dynamicButton != null)
					{
						this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonColor = Color.green;
						if (this.storeOverlayPresetWithScreenshotAction.dynamicButton.button != null)
						{
							this.storeOverlayPresetWithScreenshotAction.dynamicButton.button.interactable = true;
						}
						if (this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonText != null)
						{
							this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonText.text = "Create New Overlay Preset";
						}
					}
				}
			}
			else
			{
				bool flag = this.pm.IsPresetInPackage();
				if (this.storePresetAction != null && this.storePresetAction.dynamicButton != null)
				{
					this.storePresetAction.dynamicButton.buttonColor = Color.gray;
					if (this.storePresetAction.dynamicButton.button != null)
					{
						this.storePresetAction.dynamicButton.button.interactable = false;
					}
					if (this.storePresetAction.dynamicButton.buttonText != null)
					{
						if (flag)
						{
							this.storePresetAction.dynamicButton.buttonText.text = "Unavailable...Package Preset";
						}
						else
						{
							this.storePresetAction.dynamicButton.buttonText.text = "Create New Preset";
						}
					}
				}
				if (this.storePresetWithScreenshotAction != null && this.storePresetWithScreenshotAction.dynamicButton != null)
				{
					this.storePresetWithScreenshotAction.dynamicButton.buttonColor = Color.gray;
					if (this.storePresetWithScreenshotAction.dynamicButton.button != null)
					{
						this.storePresetWithScreenshotAction.dynamicButton.button.interactable = false;
					}
					if (this.storePresetWithScreenshotAction.dynamicButton.buttonText != null)
					{
						if (flag)
						{
							this.storePresetWithScreenshotAction.dynamicButton.buttonText.text = "Unavailable...Package Preset";
						}
						else
						{
							this.storePresetWithScreenshotAction.dynamicButton.buttonText.text = "Create New Preset";
						}
					}
				}
				if (this.storeOverlayPresetAction != null && this.storeOverlayPresetAction.dynamicButton != null)
				{
					this.storeOverlayPresetAction.dynamicButton.buttonColor = Color.gray;
					if (this.storeOverlayPresetAction.dynamicButton.button != null)
					{
						this.storeOverlayPresetAction.dynamicButton.button.interactable = false;
					}
					if (this.storeOverlayPresetAction.dynamicButton.buttonText != null)
					{
						if (flag)
						{
							this.storeOverlayPresetAction.dynamicButton.buttonText.text = "Unavailable...Package Preset";
						}
						else
						{
							this.storeOverlayPresetAction.dynamicButton.buttonText.text = "Create New Overlay Preset";
						}
					}
				}
				if (this.storeOverlayPresetWithScreenshotAction != null && this.storeOverlayPresetWithScreenshotAction.dynamicButton != null)
				{
					this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonColor = Color.gray;
					if (this.storeOverlayPresetWithScreenshotAction.dynamicButton.button != null)
					{
						this.storeOverlayPresetWithScreenshotAction.dynamicButton.button.interactable = false;
					}
					if (this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonText != null)
					{
						if (flag)
						{
							this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonText.text = "Unavailable...Package Preset";
						}
						else
						{
							this.storeOverlayPresetWithScreenshotAction.dynamicButton.buttonText.text = "Create New Overlay Preset";
						}
					}
				}
			}
		}

		// Token: 0x060064FF RID: 25855 RVA: 0x001A8AB0 File Offset: 0x001A6EB0
		protected void SyncPresetName(string s)
		{
			if (this.pm != null)
			{
				if (s.Contains("\\"))
				{
					this.presetNameJSON.val = s.Replace("\\", "/");
				}
				else
				{
					this.pm.presetName = s;
					this.favoriteJSON.valNoCallback = this.pm.IsFavorite();
					this.SyncPresetLoadButton();
					this.SyncPresetStoreButton();
				}
			}
		}

		// Token: 0x06006500 RID: 25856 RVA: 0x001A8B2C File Offset: 0x001A6F2C
		protected void SyncStorePresetName(bool b)
		{
			this.presetNameJSON.isStorable = b;
		}

		// Token: 0x06006501 RID: 25857 RVA: 0x001A8B3C File Offset: 0x001A6F3C
		protected void SyncLoadPresetWithName(string s)
		{
			if (this.pm != null)
			{
				this.loadPresetWithNameJSON.valNoCallback = string.Empty;
				this.pm.presetName = s;
				this.favoriteJSON.valNoCallback = this.pm.IsFavorite();
				this.SyncPresetLoadButton();
				this.SyncPresetStoreButton();
				this.LoadPreset();
			}
		}

		// Token: 0x06006502 RID: 25858 RVA: 0x001A8BA0 File Offset: 0x001A6FA0
		protected void SyncMergeLoadPresetWithName(string s)
		{
			if (this.pm != null)
			{
				this.mergeLoadPresetWithNameJSON.valNoCallback = string.Empty;
				this.pm.presetName = s;
				this.favoriteJSON.valNoCallback = this.pm.IsFavorite();
				this.SyncPresetLoadButton();
				this.SyncPresetStoreButton();
				this.MergeLoadPreset();
			}
		}

		// Token: 0x06006503 RID: 25859 RVA: 0x001A8C02 File Offset: 0x001A7002
		protected void LoadPresetWithPath(string p)
		{
			this.forceLoadOnPresetBrowsePathSync = true;
			this.forceUseRegularLoad = true;
			this.presetBrowsePathJSON.SetFilePath(p);
			this.forceUseRegularLoad = false;
			this.forceLoadOnPresetBrowsePathSync = false;
		}

		// Token: 0x06006504 RID: 25860 RVA: 0x001A8C2C File Offset: 0x001A702C
		protected void MergeLoadPresetWithPath(string p)
		{
			this.forceLoadOnPresetBrowsePathSync = true;
			this.forceUseMergeLoad = true;
			this.presetBrowsePathJSON.SetFilePath(p);
			this.forceUseMergeLoad = false;
			this.forceLoadOnPresetBrowsePathSync = false;
		}

		// Token: 0x06006505 RID: 25861 RVA: 0x001A8C58 File Offset: 0x001A7058
		protected void SyncStorePresetWithName(string s)
		{
			if (this.pm != null)
			{
				this.storePresetWithNameJSON.valNoCallback = string.Empty;
				this.pm.presetName = s;
				this.favoriteJSON.valNoCallback = this.pm.IsFavorite();
				this.SyncPresetLoadButton();
				this.SyncPresetStoreButton();
				this.StorePreset();
			}
		}

		// Token: 0x06006506 RID: 25862 RVA: 0x001A8CBA File Offset: 0x001A70BA
		protected void SyncFavorite(bool b)
		{
			this.pm.SetFavorite(b);
			this.RefreshFavoriteNames();
		}

		// Token: 0x06006507 RID: 25863 RVA: 0x001A8CCE File Offset: 0x001A70CE
		protected virtual void StorePreset()
		{
			this.StorePreset(false);
		}

		// Token: 0x06006508 RID: 25864 RVA: 0x001A8CD8 File Offset: 0x001A70D8
		protected virtual void StorePreset(bool skipSync)
		{
			if (this.pm != null)
			{
				if (this.pm.StorePreset(true, false))
				{
					if (!skipSync)
					{
						this.SyncFavorite(this.favoriteJSON.val);
						this.SyncPresetUI();
					}
					this.SetStatus("Stored preset " + this.pm.presetName);
				}
				else
				{
					this.SetStatus("Failed to store preset " + this.pm.presetName);
				}
			}
		}

		// Token: 0x06006509 RID: 25865 RVA: 0x001A8D60 File Offset: 0x001A7160
		protected virtual void StorePresetWithScreenshot()
		{
			this.StorePresetWithScreenshot(false);
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x001A8D6C File Offset: 0x001A716C
		protected virtual void StorePresetWithScreenshot(bool skipSync)
		{
			if (this.pm != null)
			{
				if (this.pm.StorePreset(true, true))
				{
					if (!skipSync)
					{
						this.SyncFavorite(this.favoriteJSON.val);
						this.SyncPresetUI();
					}
					this.SetStatus("Stored preset " + this.pm.presetName);
				}
				else
				{
					this.SetStatus("Failed to store preset " + this.pm.presetName);
				}
			}
		}

		// Token: 0x0600650B RID: 25867 RVA: 0x001A8DF4 File Offset: 0x001A71F4
		protected virtual void StoreOverlayPreset()
		{
			this.StoreOverlayPreset(false);
		}

		// Token: 0x0600650C RID: 25868 RVA: 0x001A8E00 File Offset: 0x001A7200
		protected virtual void StoreOverlayPreset(bool skipSync)
		{
			if (this.pm != null)
			{
				if (this.pm.StorePreset(false, false))
				{
					if (!skipSync)
					{
						this.SyncFavorite(this.favoriteJSON.val);
						this.SyncPresetUI();
					}
					this.SetStatus("Stored preset " + this.pm.presetName);
				}
				else
				{
					this.SetStatus("Failed to store preset " + this.pm.presetName);
				}
			}
		}

		// Token: 0x0600650D RID: 25869 RVA: 0x001A8E88 File Offset: 0x001A7288
		protected virtual void StoreOverlayPresetWithScreenshot()
		{
			this.StoreOverlayPresetWithScreenshot(false);
		}

		// Token: 0x0600650E RID: 25870 RVA: 0x001A8E94 File Offset: 0x001A7294
		protected virtual void StoreOverlayPresetWithScreenshot(bool skipSync)
		{
			if (this.pm != null)
			{
				if (this.pm.StorePreset(false, true))
				{
					if (!skipSync)
					{
						this.SyncFavorite(this.favoriteJSON.val);
						this.SyncPresetUI();
					}
					this.SetStatus("Stored preset " + this.pm.presetName);
				}
				else
				{
					this.SetStatus("Failed to store preset " + this.pm.presetName);
				}
			}
		}

		// Token: 0x0600650F RID: 25871 RVA: 0x001A8F1C File Offset: 0x001A731C
		protected void SyncStoreOptional(bool b)
		{
			if (this.pm != null)
			{
				this.pm.storeOptionalStorables = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x06006510 RID: 25872 RVA: 0x001A8F46 File Offset: 0x001A7346
		protected void SyncStoreOptional2(bool b)
		{
			if (this.pm != null)
			{
				this.pm.storeOptionalStorables2 = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x06006511 RID: 25873 RVA: 0x001A8F70 File Offset: 0x001A7370
		protected void SyncStoreOptional3(bool b)
		{
			if (this.pm != null)
			{
				this.pm.storeOptionalStorables3 = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x06006512 RID: 25874 RVA: 0x001A8F9A File Offset: 0x001A739A
		protected void SyncStorePresetBinary(bool b)
		{
			if (this.pm != null)
			{
				this.pm.storePresetBinary = b;
			}
		}

		// Token: 0x06006513 RID: 25875 RVA: 0x001A8FBC File Offset: 0x001A73BC
		protected virtual void LoadPreset()
		{
			if (this.pm != null)
			{
				this.isLoadingPreset = true;
				if (this.pm.LoadPresetPre(false))
				{
					if ((this.pm.itemType == PresetManager.ItemType.Atom || this.pm.itemType == PresetManager.ItemType.Custom) && this.containingAtom != null)
					{
						this.containingAtom.SetLastRestoredData(this.pm.lastLoadedJSON, this.pm.includeAppearance, this.pm.includePhysical);
					}
					if (this.pm.LoadPresetPost())
					{
						this.SetStatus("Loaded preset " + this.pm.presetName);
					}
					else
					{
						this.SetStatus("Failed to load preset " + this.pm.presetName);
					}
				}
				else
				{
					this.SetStatus("Failed to load preset " + this.pm.presetName);
				}
				this.isLoadingPreset = false;
				this.RefreshFavoriteNames();
			}
		}

		// Token: 0x06006514 RID: 25876 RVA: 0x001A90CC File Offset: 0x001A74CC
		protected virtual void MergeLoadPreset()
		{
			if (this.pm != null)
			{
				this.isLoadingPreset = true;
				if (this.pm.LoadPresetPre(true))
				{
					if ((this.pm.itemType == PresetManager.ItemType.Atom || this.pm.itemType == PresetManager.ItemType.Custom) && this.containingAtom != null)
					{
						this.containingAtom.SetLastRestoredData(this.pm.lastLoadedJSON, this.pm.includeAppearance, this.pm.includePhysical);
					}
					if (this.pm.LoadPresetPost())
					{
						this.SetStatus("Merge loaded preset " + this.pm.presetName);
					}
					else
					{
						this.SetStatus("Failed to load preset " + this.pm.presetName);
					}
				}
				else
				{
					this.SetStatus("Failed to load preset " + this.pm.presetName);
				}
				this.isLoadingPreset = false;
				this.RefreshFavoriteNames();
			}
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x001A91D9 File Offset: 0x001A75D9
		protected virtual void AutoTypeLoadPreset()
		{
			if (this.useMergeLoadJSON != null && this.useMergeLoadJSON.val)
			{
				this.MergeLoadPreset();
			}
			else
			{
				this.LoadPreset();
			}
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x001A9208 File Offset: 0x001A7608
		protected virtual void LoadDefaults()
		{
			if (this.pm != null)
			{
				this.isLoadingPreset = true;
				if (this.pm.LoadDefaultsPre())
				{
					if ((this.pm.itemType == PresetManager.ItemType.Atom || this.pm.itemType == PresetManager.ItemType.Custom) && this.containingAtom != null)
					{
						this.containingAtom.SetLastRestoredData(this.pm.lastLoadedJSON, this.pm.includeAppearance, this.pm.includePhysical);
					}
					if (this.pm.LoadPresetPost())
					{
						this.pm.RestorePresetBinary();
						this.SetStatus("Loaded defaults");
					}
					else
					{
						this.SetStatus("Failed to load defaults");
					}
				}
				else
				{
					this.SetStatus("Failed to load defaults");
				}
				this.isLoadingPreset = false;
			}
		}

		// Token: 0x06006517 RID: 25879 RVA: 0x001A92EC File Offset: 0x001A76EC
		public void LoadUserDefaults()
		{
			string presetName = this.pm.presetName;
			string package = this.pm.package;
			this.pm.presetName = "UserDefaults";
			if (this.pm.CheckPresetExistance())
			{
				this.LoadPreset();
			}
			this.pm.presetName = presetName;
			this.pm.package = package;
		}

		// Token: 0x06006518 RID: 25880 RVA: 0x001A9350 File Offset: 0x001A7750
		public void StoreUserDefaults()
		{
			string presetName = this.pm.presetName;
			string package = this.pm.package;
			this.pm.presetName = "UserDefaults";
			if (this.pm.CheckPresetReadyForStore())
			{
				this.StorePresetWithScreenshot(true);
			}
			this.pm.presetName = presetName;
			this.pm.package = package;
		}

		// Token: 0x06006519 RID: 25881 RVA: 0x001A93B4 File Offset: 0x001A77B4
		public void ClearUserDefaults()
		{
			string presetName = this.pm.presetName;
			string package = this.pm.package;
			this.pm.presetName = "UserDefaults";
			this.pm.DeletePreset();
			this.pm.presetName = presetName;
			this.pm.package = package;
		}

		// Token: 0x0600651A RID: 25882 RVA: 0x001A940C File Offset: 0x001A780C
		protected virtual void SetStatus(string status)
		{
			if (this.statusText != null)
			{
				this.statusText.text = status;
			}
			if (this.statusTextAlt != null)
			{
				this.statusTextAlt.text = status;
			}
		}

		// Token: 0x0600651B RID: 25883 RVA: 0x001A9448 File Offset: 0x001A7848
		protected void SyncFavoriteSelection(string s)
		{
			this.favoriteSelectionJSON.valNoCallback = string.Empty;
			this.presetNameJSON.val = s;
			if (this.pm != null && this.loadPresetOnSelectJSON != null && this.loadPresetOnSelectJSON.val && this.pm.CheckPresetExistance())
			{
				if (this.useMergeLoadJSON != null && this.useMergeLoadJSON.val)
				{
					this.MergeLoadPreset();
				}
				else
				{
					this.LoadPreset();
				}
				this.presetNameJSON.val = s;
			}
		}

		// Token: 0x0600651C RID: 25884 RVA: 0x001A94E5 File Offset: 0x001A78E5
		protected void RefreshFavoriteNames()
		{
			if (this.pm != null && this.favoriteSelectionJSON != null)
			{
				this.favoriteSelectionJSON.choices = this.pm.FindFavoriteNames();
			}
		}

		// Token: 0x0600651D RID: 25885 RVA: 0x001A9519 File Offset: 0x001A7919
		protected void SyncIncludeOptional(bool b)
		{
			if (this.pm != null)
			{
				this.pm.includeOptional = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x0600651E RID: 25886 RVA: 0x001A9543 File Offset: 0x001A7943
		protected void SyncIncludeOptional2(bool b)
		{
			if (this.pm != null)
			{
				this.pm.includeOptional2 = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x0600651F RID: 25887 RVA: 0x001A956D File Offset: 0x001A796D
		protected void SyncIncludeOptional3(bool b)
		{
			if (this.pm != null)
			{
				this.pm.includeOptional3 = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x001A9597 File Offset: 0x001A7997
		protected void SyncIncludeAppearance(bool b)
		{
			if (this.pm != null)
			{
				this.pm.includeAppearance = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x001A95C1 File Offset: 0x001A79C1
		protected void SyncIncludePhysical(bool b)
		{
			if (this.pm != null)
			{
				this.pm.includePhysical = b;
				this.pm.SyncParamsLocked();
			}
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x001A95EB File Offset: 0x001A79EB
		protected void SyncLockParams(bool b)
		{
			if (this.controlOverview != null)
			{
				this.controlOverview.SyncPresetManagerControlLockParams(this);
			}
			if (this.pm != null)
			{
				this.pm.paramsLocked = b;
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06006523 RID: 25891 RVA: 0x001A9627 File Offset: 0x001A7A27
		// (set) Token: 0x06006524 RID: 25892 RVA: 0x001A9634 File Offset: 0x001A7A34
		public bool lockParams
		{
			get
			{
				return this.lockParamsJSON.val;
			}
			set
			{
				this.lockParamsJSON.val = value;
			}
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x001A9644 File Offset: 0x001A7A44
		protected void BeginBrowse(JSONStorableUrl jsurl)
		{
			if (this.pm != null)
			{
				jsurl.fileRemovePrefix = this.pm.storeName + "_";
				this.pm.CreateStoreFolderPath();
				string storeFolderPath = this.pm.GetStoreFolderPath(false);
				string text = storeFolderPath;
				text = Regex.Replace(text, "/$", string.Empty);
				jsurl.suggestedPath = text;
				List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(storeFolderPath, false, false, true, true);
				jsurl.shortCuts = shortCutsForDirectory;
				if (this.useMergeLoadJSON.toggleAlt != null)
				{
					this.useMergeLoadJSON.toggleAlt.gameObject.SetActive(this.showMergeLoad);
				}
			}
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x001A96F2 File Offset: 0x001A7AF2
		protected void EndBrowse(JSONStorableUrl jsurl)
		{
			if (this.useMergeLoadJSON.toggleAlt != null)
			{
				this.useMergeLoadJSON.toggleAlt.gameObject.SetActive(false);
			}
		}

		// Token: 0x06006527 RID: 25895 RVA: 0x001A9720 File Offset: 0x001A7B20
		public void SyncPresetUI()
		{
			this.RefreshFavoriteNames();
			this.SyncPresetLoadButton();
			this.SyncPresetStoreButton();
		}

		// Token: 0x06006528 RID: 25896 RVA: 0x001A9734 File Offset: 0x001A7B34
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				PresetManagerControlUI componentInChildren = t.GetComponentInChildren<PresetManagerControlUI>(true);
				if (this.pm != null && componentInChildren != null)
				{
					this.presetBrowsePathJSON.RegisterFileBrowseButton(componentInChildren.browsePresetsButton, isAlt);
					this.openPresetBrowsePathInExplorerAction.RegisterButton(componentInChildren.openPresetBrowsePathInExplorerButton, isAlt);
					this.presetNameJSON.RegisterInputField(componentInChildren.presetNameField, isAlt);
					this.storePresetNameJSON.RegisterToggle(componentInChildren.storePresetNameToggle, isAlt);
					this.favoriteJSON.RegisterToggle(componentInChildren.favoriteToggle, isAlt);
					this.storePresetAction.RegisterButton(componentInChildren.storePresetButton, isAlt);
					this.storePresetWithScreenshotAction.RegisterButton(componentInChildren.storePresetWithScreenshotButton, isAlt);
					this.storeOverlayPresetAction.RegisterButton(componentInChildren.storeOverlayPresetButton, isAlt);
					this.storeOverlayPresetWithScreenshotAction.RegisterButton(componentInChildren.storeOverlayPresetWithScreenshotButton, isAlt);
					this.storeOptionalJSON.RegisterToggle(componentInChildren.storeOptionalToggle, isAlt);
					this.storeOptional2JSON.RegisterToggle(componentInChildren.storeOptional2Toggle, isAlt);
					this.storeOptional3JSON.RegisterToggle(componentInChildren.storeOptional3Toggle, isAlt);
					this.storePresetBinaryJSON.RegisterToggle(componentInChildren.storePresetBinaryToggle, isAlt);
					this.autoTypeLoadPresetAction.RegisterButton(componentInChildren.loadPresetButton, isAlt);
					this.loadDefaultsAction.RegisterButton(componentInChildren.loadDefaultsButton, isAlt);
					this.loadUserDefaultsAction.RegisterButton(componentInChildren.loadUserDefaultsButton, isAlt);
					this.storeUserDefaultsAction.RegisterButton(componentInChildren.storeUserDefaultsButton, isAlt);
					this.clearUserDefaultsAction.RegisterButton(componentInChildren.clearUserDefaultsButton, isAlt);
					if (isAlt)
					{
						this.statusTextAlt = componentInChildren.statusText;
					}
					else
					{
						this.statusText = componentInChildren.statusText;
					}
					this.SyncPresetLoadButton();
					this.SyncPresetStoreButton();
					this.loadPresetOnSelectJSON.RegisterToggle(componentInChildren.loadPresetOnSelectToggle, isAlt);
					if (!isAlt)
					{
						this.useMergeLoadJSON.RegisterToggle(componentInChildren.useMergeLoadToggle, false);
						if (componentInChildren.useMergeLoadToggle != null)
						{
							componentInChildren.useMergeLoadToggle.gameObject.SetActive(this.showMergeLoad);
						}
						this.useMergeLoadJSON.RegisterToggle(componentInChildren.useMergeLoadBrowserToggle, true);
						if (componentInChildren.useMergeLoadBrowserToggle != null)
						{
							componentInChildren.useMergeLoadBrowserToggle.gameObject.SetActive(false);
						}
					}
					this.favoriteSelectionJSON.RegisterPopup(componentInChildren.favoriteSelectionPopup, isAlt);
					this.includeOptionalJSON.RegisterToggle(componentInChildren.includeOptionalToggle, isAlt);
					this.includeOptional2JSON.RegisterToggle(componentInChildren.includeOptional2Toggle, isAlt);
					this.includeOptional3JSON.RegisterToggle(componentInChildren.includeOptional3Toggle, isAlt);
					this.includeAppearanceJSON.RegisterToggle(componentInChildren.includeAppearanceToggle, isAlt);
					this.includePhysicalJSON.RegisterToggle(componentInChildren.includePhysicalToggle, isAlt);
					this.lockParamsJSON.RegisterToggle(componentInChildren.lockParamsToggle, isAlt);
				}
			}
		}

		// Token: 0x06006529 RID: 25897 RVA: 0x001A99F0 File Offset: 0x001A7DF0
		public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
		{
			base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
			this.presetNameJSON.val = FileManager.NormalizeID(this.presetNameJSON.val);
			if (this.pm != null && !this.isLoadingPreset)
			{
				this.pm.RestorePresetBinary();
			}
		}

		// Token: 0x0600652A RID: 25898 RVA: 0x001A9A4C File Offset: 0x001A7E4C
		protected virtual void Init()
		{
			this.pm = base.GetComponent<PresetManager>();
			if (this.pm != null)
			{
				string str = string.Empty;
				if (this.pm.itemType == PresetManager.ItemType.Atom && this.containingAtom != null)
				{
					this.pm.customPath = this.containingAtom.type + "/";
					str = "Textures";
				}
				string storeFolderPath = this.pm.GetStoreFolderPath(true);
				if (storeFolderPath != null)
				{
					Directory.CreateDirectory(storeFolderPath);
					if (this.setMaterialOptionsTexturePaths)
					{
						MaterialOptions[] componentsInChildren = base.GetComponentsInChildren<MaterialOptions>(true);
						if (componentsInChildren != null && componentsInChildren.Length > 0)
						{
							string text = storeFolderPath + str;
							Directory.CreateDirectory(text);
							foreach (MaterialOptions materialOptions in componentsInChildren)
							{
								materialOptions.SetCustomTextureFolder(text);
							}
						}
					}
				}
				this.presetBrowsePathJSON = new JSONStorableUrl("presetBrowsePath", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPresetBrowsePath), "vap", this.pm.GetStoreFolderPath(true), true);
				this.presetBrowsePathJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				this.presetBrowsePathJSON.endBrowseWithObjectCallback = new JSONStorableUrl.EndBrowseWithObjectCallback(this.EndBrowse);
				this.presetBrowsePathJSON.allowFullComputerBrowse = false;
				this.presetBrowsePathJSON.allowBrowseAboveSuggestedPath = false;
				this.presetBrowsePathJSON.hideExtension = true;
				this.presetBrowsePathJSON.fileRemovePrefix = this.pm.storeName + "_";
				this.presetBrowsePathJSON.showDirs = true;
				this.presetBrowsePathJSON.isStorable = false;
				this.presetBrowsePathJSON.isRestorable = false;
				base.RegisterUrl(this.presetBrowsePathJSON);
				this.openPresetBrowsePathInExplorerAction = new JSONStorableAction("OpenPresetBrowsePathInExplorerAction", new JSONStorableAction.ActionCallback(this.OpenPresetBrowsePathInExplorer));
				base.RegisterAction(this.openPresetBrowsePathInExplorerAction);
				this.presetNameJSON = new JSONStorableString("presetName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPresetName));
				this.presetNameJSON.storeType = JSONStorableParam.StoreType.Full;
				this.presetNameJSON.isStorable = false;
				this.presetNameJSON.isRestorable = true;
				this.presetNameJSON.enableOnChange = true;
				base.RegisterString(this.presetNameJSON);
				this.storePresetNameJSON = new JSONStorableBool("storePresetName", false, new JSONStorableBool.SetBoolCallback(this.SyncStorePresetName));
				this.storePresetNameJSON.storeType = JSONStorableParam.StoreType.Full;
				this.storePresetNameJSON.isStorable = true;
				this.storePresetNameJSON.isRestorable = true;
				base.RegisterBool(this.storePresetNameJSON);
				this.favoriteJSON = new JSONStorableBool("favorite", false, new JSONStorableBool.SetBoolCallback(this.SyncFavorite));
				this.favoriteJSON.isStorable = false;
				this.favoriteJSON.isRestorable = false;
				base.RegisterBool(this.favoriteJSON);
				this.storePresetAction = new JSONStorableAction("StorePreset", new JSONStorableAction.ActionCallback(this.StorePreset));
				base.RegisterAction(this.storePresetAction);
				this.storePresetWithScreenshotAction = new JSONStorableAction("StorePresetWithScreenshot", new JSONStorableAction.ActionCallback(this.StorePresetWithScreenshot));
				base.RegisterAction(this.storePresetWithScreenshotAction);
				this.storeOverlayPresetAction = new JSONStorableAction("StoreOverlayPreset", new JSONStorableAction.ActionCallback(this.StoreOverlayPreset));
				base.RegisterAction(this.storeOverlayPresetAction);
				this.storeOverlayPresetWithScreenshotAction = new JSONStorableAction("StoreOverlayPresetWithScreenshot", new JSONStorableAction.ActionCallback(this.StoreOverlayPresetWithScreenshot));
				base.RegisterAction(this.storeOverlayPresetWithScreenshotAction);
				this.storePresetWithNameJSON = new JSONStorableString("StorePresetWithName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncStorePresetWithName));
				this.storePresetWithNameJSON.isStorable = false;
				this.storePresetWithNameJSON.isRestorable = false;
				base.RegisterString(this.storePresetWithNameJSON);
				this.storeOptionalJSON = new JSONStorableBool("storeOptional", this.pm.storeOptionalStorables, new JSONStorableBool.SetBoolCallback(this.SyncStoreOptional));
				this.storeOptionalJSON.isStorable = false;
				this.storeOptionalJSON.isRestorable = false;
				base.RegisterBool(this.storeOptionalJSON);
				this.storeOptional2JSON = new JSONStorableBool("storeOptional2", this.pm.storeOptionalStorables2, new JSONStorableBool.SetBoolCallback(this.SyncStoreOptional2));
				this.storeOptional2JSON.isStorable = false;
				this.storeOptional2JSON.isRestorable = false;
				base.RegisterBool(this.storeOptional2JSON);
				this.storeOptional3JSON = new JSONStorableBool("storeOptional3", this.pm.storeOptionalStorables3, new JSONStorableBool.SetBoolCallback(this.SyncStoreOptional3));
				this.storeOptional3JSON.isStorable = false;
				this.storeOptional3JSON.isRestorable = false;
				base.RegisterBool(this.storeOptional3JSON);
				this.storePresetBinaryJSON = new JSONStorableBool("storeBinary", this.pm.storePresetBinary, new JSONStorableBool.SetBoolCallback(this.SyncStorePresetBinary));
				this.storePresetBinaryJSON.isStorable = false;
				this.storePresetBinaryJSON.isRestorable = false;
				base.RegisterBool(this.storePresetBinaryJSON);
				this.loadPresetAction = new JSONStorableAction("LoadPreset", new JSONStorableAction.ActionCallback(this.LoadPreset));
				base.RegisterAction(this.loadPresetAction);
				this.mergeLoadPresetAction = new JSONStorableAction("MergeLoadPreset", new JSONStorableAction.ActionCallback(this.MergeLoadPreset));
				base.RegisterAction(this.mergeLoadPresetAction);
				this.autoTypeLoadPresetAction = new JSONStorableAction("AutoTypeLoadPreset", new JSONStorableAction.ActionCallback(this.AutoTypeLoadPreset));
				base.RegisterAction(this.autoTypeLoadPresetAction);
				this.loadPresetWithPathUrlJSON = new JSONStorableUrl("loadPresetWithPathUrl", string.Empty, "vap", this.pm.GetStoreFolderPath(true));
				this.loadPresetWithPathUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				this.loadPresetWithPathUrlJSON.allowFullComputerBrowse = false;
				this.loadPresetWithPathUrlJSON.allowBrowseAboveSuggestedPath = false;
				this.loadPresetWithPathUrlJSON.hideExtension = true;
				this.loadPresetWithPathUrlJSON.fileRemovePrefix = this.pm.storeName + "_";
				this.loadPresetWithPathUrlJSON.showDirs = true;
				this.loadPresetWithPathJSON = new JSONStorableActionPresetFilePath("LoadPresetWithPath", new JSONStorableActionPresetFilePath.PresetFilePathActionCallback(this.LoadPresetWithPath), this.loadPresetWithPathUrlJSON);
				base.RegisterPresetFilePathAction(this.loadPresetWithPathJSON);
				this.mergeLoadPresetWithPathUrlJSON = new JSONStorableUrl("loadPresetWithPathUrl", string.Empty, "vap", this.pm.GetStoreFolderPath(true));
				this.mergeLoadPresetWithPathUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
				this.mergeLoadPresetWithPathUrlJSON.allowFullComputerBrowse = false;
				this.mergeLoadPresetWithPathUrlJSON.allowBrowseAboveSuggestedPath = false;
				this.mergeLoadPresetWithPathUrlJSON.hideExtension = true;
				this.mergeLoadPresetWithPathUrlJSON.fileRemovePrefix = this.pm.storeName + "_";
				this.mergeLoadPresetWithPathUrlJSON.showDirs = true;
				this.mergeLoadPresetWithPathJSON = new JSONStorableActionPresetFilePath("MergeLoadPresetWithPath", new JSONStorableActionPresetFilePath.PresetFilePathActionCallback(this.MergeLoadPresetWithPath), this.mergeLoadPresetWithPathUrlJSON);
				base.RegisterPresetFilePathAction(this.mergeLoadPresetWithPathJSON);
				this.loadPresetWithNameJSON = new JSONStorableString("LoadPresetWithName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncLoadPresetWithName));
				this.loadPresetWithNameJSON.isStorable = false;
				this.loadPresetWithNameJSON.isRestorable = false;
				base.RegisterString(this.loadPresetWithNameJSON);
				this.mergeLoadPresetWithNameJSON = new JSONStorableString("MergeLoadPresetWithName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncMergeLoadPresetWithName));
				this.mergeLoadPresetWithNameJSON.isStorable = false;
				this.mergeLoadPresetWithNameJSON.isRestorable = false;
				base.RegisterString(this.mergeLoadPresetWithNameJSON);
				this.loadDefaultsAction = new JSONStorableAction("LoadDefaults", new JSONStorableAction.ActionCallback(this.LoadDefaults));
				base.RegisterAction(this.loadDefaultsAction);
				this.storeUserDefaultsAction = new JSONStorableAction("StoreUserDefaults", new JSONStorableAction.ActionCallback(this.StoreUserDefaults));
				base.RegisterAction(this.storeUserDefaultsAction);
				this.clearUserDefaultsAction = new JSONStorableAction("ClearUserDefaults", new JSONStorableAction.ActionCallback(this.ClearUserDefaults));
				base.RegisterAction(this.clearUserDefaultsAction);
				this.loadUserDefaultsAction = new JSONStorableAction("LoadUserDefaults", new JSONStorableAction.ActionCallback(this.LoadUserDefaults));
				base.RegisterAction(this.loadUserDefaultsAction);
				this.loadPresetOnSelectJSON = new JSONStorableBool("loadPresetOnSelect", true);
				this.loadPresetOnSelectJSON.isStorable = false;
				this.loadPresetOnSelectJSON.isRestorable = false;
				base.RegisterBool(this.loadPresetOnSelectJSON);
				this.useMergeLoadJSON = new JSONStorableBool("useMergeLoad", false);
				this.useMergeLoadJSON.isStorable = false;
				this.useMergeLoadJSON.isRestorable = false;
				base.RegisterBool(this.useMergeLoadJSON);
				this.favoriteSelectionJSON = new JSONStorableStringChooser("favoriteSelection", null, string.Empty, "Favorite Selection", new JSONStorableStringChooser.SetStringCallback(this.SyncFavoriteSelection));
				this.favoriteSelectionJSON.isStorable = false;
				this.favoriteSelectionJSON.isRestorable = false;
				this.includeOptionalJSON = new JSONStorableBool("includeOptional", this.pm.includeOptional, new JSONStorableBool.SetBoolCallback(this.SyncIncludeOptional));
				this.includeOptionalJSON.isStorable = false;
				this.includeOptionalJSON.isRestorable = false;
				base.RegisterBool(this.includeOptionalJSON);
				this.includeOptional2JSON = new JSONStorableBool("includeOptional2", this.pm.includeOptional2, new JSONStorableBool.SetBoolCallback(this.SyncIncludeOptional2));
				this.includeOptional2JSON.isStorable = false;
				this.includeOptional2JSON.isRestorable = false;
				base.RegisterBool(this.includeOptional2JSON);
				this.includeOptional3JSON = new JSONStorableBool("includeOptional3", this.pm.includeOptional3, new JSONStorableBool.SetBoolCallback(this.SyncIncludeOptional3));
				this.includeOptional3JSON.isStorable = false;
				this.includeOptional3JSON.isRestorable = false;
				base.RegisterBool(this.includeOptional3JSON);
				this.includeAppearanceJSON = new JSONStorableBool("includeAppearance", this.pm.includeAppearance, new JSONStorableBool.SetBoolCallback(this.SyncIncludeAppearance));
				this.includeAppearanceJSON.isStorable = false;
				this.includeAppearanceJSON.isRestorable = false;
				base.RegisterBool(this.includeAppearanceJSON);
				this.includePhysicalJSON = new JSONStorableBool("includePhysical", this.pm.includePhysical, new JSONStorableBool.SetBoolCallback(this.SyncIncludePhysical));
				this.includePhysicalJSON.isStorable = false;
				this.includePhysicalJSON.isRestorable = false;
				base.RegisterBool(this.includePhysicalJSON);
				this.lockParamsJSON = new JSONStorableBool("lockParams", false, new JSONStorableBool.SetBoolCallback(this.SyncLockParams));
				this.RefreshFavoriteNames();
				base.RegisterStringChooser(this.favoriteSelectionJSON);
			}
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x001AA4A2 File Offset: 0x001A88A2
		public override void PreRestore()
		{
			if (!this.isLoadingPreset && this.presetBrowsePathJSON != null)
			{
				this.presetBrowsePathJSON.val = string.Empty;
			}
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x001AA4CA File Offset: 0x001A88CA
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

		// Token: 0x040054AD RID: 21677
		protected PresetManager pm;

		// Token: 0x040054AE RID: 21678
		public PresetManagerControlOverview controlOverview;

		// Token: 0x040054AF RID: 21679
		public bool setMaterialOptionsTexturePaths = true;

		// Token: 0x040054B0 RID: 21680
		protected bool forceUseRegularLoad;

		// Token: 0x040054B1 RID: 21681
		protected bool forceUseMergeLoad;

		// Token: 0x040054B2 RID: 21682
		protected bool forceLoadOnPresetBrowsePathSync;

		// Token: 0x040054B3 RID: 21683
		protected JSONStorableUrl presetBrowsePathJSON;

		// Token: 0x040054B4 RID: 21684
		protected JSONStorableAction openPresetBrowsePathInExplorerAction;

		// Token: 0x040054B5 RID: 21685
		protected JSONStorableString presetNameJSON;

		// Token: 0x040054B6 RID: 21686
		protected JSONStorableBool storePresetNameJSON;

		// Token: 0x040054B7 RID: 21687
		protected JSONStorableString loadPresetWithNameJSON;

		// Token: 0x040054B8 RID: 21688
		protected JSONStorableString mergeLoadPresetWithNameJSON;

		// Token: 0x040054B9 RID: 21689
		protected JSONStorableActionPresetFilePath loadPresetWithPathJSON;

		// Token: 0x040054BA RID: 21690
		protected JSONStorableUrl loadPresetWithPathUrlJSON;

		// Token: 0x040054BB RID: 21691
		protected JSONStorableActionPresetFilePath mergeLoadPresetWithPathJSON;

		// Token: 0x040054BC RID: 21692
		protected JSONStorableUrl mergeLoadPresetWithPathUrlJSON;

		// Token: 0x040054BD RID: 21693
		protected JSONStorableString storePresetWithNameJSON;

		// Token: 0x040054BE RID: 21694
		protected JSONStorableBool favoriteJSON;

		// Token: 0x040054BF RID: 21695
		protected JSONStorableAction storePresetAction;

		// Token: 0x040054C0 RID: 21696
		protected JSONStorableAction storePresetWithScreenshotAction;

		// Token: 0x040054C1 RID: 21697
		protected JSONStorableAction storeOverlayPresetAction;

		// Token: 0x040054C2 RID: 21698
		protected JSONStorableAction storeOverlayPresetWithScreenshotAction;

		// Token: 0x040054C3 RID: 21699
		protected JSONStorableBool storeOptionalJSON;

		// Token: 0x040054C4 RID: 21700
		protected JSONStorableBool storeOptional2JSON;

		// Token: 0x040054C5 RID: 21701
		protected JSONStorableBool storeOptional3JSON;

		// Token: 0x040054C6 RID: 21702
		protected JSONStorableBool storePresetBinaryJSON;

		// Token: 0x040054C7 RID: 21703
		protected bool isLoadingPreset;

		// Token: 0x040054C8 RID: 21704
		protected JSONStorableAction loadPresetAction;

		// Token: 0x040054C9 RID: 21705
		protected JSONStorableAction mergeLoadPresetAction;

		// Token: 0x040054CA RID: 21706
		protected JSONStorableAction autoTypeLoadPresetAction;

		// Token: 0x040054CB RID: 21707
		protected JSONStorableAction loadDefaultsAction;

		// Token: 0x040054CC RID: 21708
		protected JSONStorableAction loadUserDefaultsAction;

		// Token: 0x040054CD RID: 21709
		protected JSONStorableAction storeUserDefaultsAction;

		// Token: 0x040054CE RID: 21710
		protected JSONStorableAction clearUserDefaultsAction;

		// Token: 0x040054CF RID: 21711
		protected Text statusText;

		// Token: 0x040054D0 RID: 21712
		protected Text statusTextAlt;

		// Token: 0x040054D1 RID: 21713
		protected JSONStorableBool loadPresetOnSelectJSON;

		// Token: 0x040054D2 RID: 21714
		public bool showMergeLoad = true;

		// Token: 0x040054D3 RID: 21715
		protected JSONStorableBool useMergeLoadJSON;

		// Token: 0x040054D4 RID: 21716
		protected JSONStorableStringChooser favoriteSelectionJSON;

		// Token: 0x040054D5 RID: 21717
		protected JSONStorableBool includeOptionalJSON;

		// Token: 0x040054D6 RID: 21718
		protected JSONStorableBool includeOptional2JSON;

		// Token: 0x040054D7 RID: 21719
		protected JSONStorableBool includeOptional3JSON;

		// Token: 0x040054D8 RID: 21720
		protected JSONStorableBool includeAppearanceJSON;

		// Token: 0x040054D9 RID: 21721
		protected JSONStorableBool includePhysicalJSON;

		// Token: 0x040054DA RID: 21722
		protected JSONStorableBool lockParamsJSON;
	}
}
