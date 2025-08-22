using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

namespace MeshVR
{
	// Token: 0x02000CF4 RID: 3316
	public class PresetManager : MonoBehaviour
	{
		// Token: 0x060064C1 RID: 25793 RVA: 0x00173ABC File Offset: 0x00171EBC
		public PresetManager()
		{
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x060064C2 RID: 25794 RVA: 0x00173B4C File Offset: 0x00171F4C
		// (set) Token: 0x060064C3 RID: 25795 RVA: 0x00173B54 File Offset: 0x00171F54
		public string presetName
		{
			get
			{
				return this._presetName;
			}
			set
			{
				if (this._presetName != value)
				{
					this._presetName = value;
					if (this._presetName == null)
					{
						this.presetPackageName = string.Empty;
						this.presetPackagePath = string.Empty;
						this.presetSubName = string.Empty;
						this.presetSubPath = string.Empty;
					}
					else
					{
						string text = this._presetName;
						if (this._presetName.Contains(":"))
						{
							string[] array = this._presetName.Split(new char[]
							{
								':'
							});
							this.presetPackageName = array[0];
							this.presetPackagePath = array[0] + ":/";
							text = array[1];
						}
						else
						{
							this.presetPackageName = string.Empty;
							this.presetPackagePath = string.Empty;
						}
						if (text.Contains("/"))
						{
							this.presetSubPath = Path.GetDirectoryName(text) + "/";
							this.presetSubName = Path.GetFileName(text);
						}
						else
						{
							this.presetSubPath = string.Empty;
							this.presetSubName = text;
						}
					}
				}
			}
		}

		// Token: 0x060064C4 RID: 25796 RVA: 0x00173C6C File Offset: 0x0017206C
		public bool IsInPackage()
		{
			return this.package != string.Empty;
		}

		// Token: 0x060064C5 RID: 25797 RVA: 0x00173C80 File Offset: 0x00172080
		public string GetStoreRootPath(bool includePackage = true)
		{
			string result = null;
			string str = string.Empty;
			if (this.package != string.Empty && includePackage)
			{
				str = this.package + ":/";
			}
			switch (this.itemType)
			{
			case PresetManager.ItemType.None:
				result = str + this.storeRoot;
				break;
			case PresetManager.ItemType.Custom:
				result = str + this.storeRoot + this.customPath;
				break;
			case PresetManager.ItemType.ClothingFemale:
				result = str + this.storeRoot + "Clothing/Female/";
				break;
			case PresetManager.ItemType.ClothingMale:
				result = str + this.storeRoot + "Clothing/Male/";
				break;
			case PresetManager.ItemType.ClothingNeutral:
				result = str + this.storeRoot + "Clothing/Neutral/";
				break;
			case PresetManager.ItemType.Atom:
				result = str + this.storeRoot + "Atom/" + this.customPath;
				break;
			case PresetManager.ItemType.HairFemale:
				result = str + this.storeRoot + "Hair/Female/";
				break;
			case PresetManager.ItemType.HairMale:
				result = str + this.storeRoot + "Hair/Male/";
				break;
			case PresetManager.ItemType.HairNeutral:
				result = str + this.storeRoot + "Hair/Neutral/";
				break;
			}
			return result;
		}

		// Token: 0x060064C6 RID: 25798 RVA: 0x00173DCC File Offset: 0x001721CC
		public List<string> FindFavoriteNames()
		{
			List<string> list = new List<string>();
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && Directory.Exists(storeFolderPath))
				{
					string[] files = Directory.GetFiles(storeFolderPath, this.storeName + "_*.vap.fav", SearchOption.AllDirectories);
					foreach (string input in files)
					{
						string text = Regex.Replace(input, "\\.fav$", string.Empty);
						text = text.Replace("\\", "/");
						string presetNameFromFilePath = this.GetPresetNameFromFilePath(text);
						if (presetNameFromFilePath != null)
						{
							list.Add(presetNameFromFilePath);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060064C7 RID: 25799 RVA: 0x00173EAC File Offset: 0x001722AC
		public string[] PathToNames(string inpath)
		{
			string text = string.Empty;
			string text2 = string.Empty;
			string text3 = string.Empty;
			string text4 = string.Empty;
			string text5 = ":/";
			string text6;
			if (inpath.Contains(text5))
			{
				string[] array = inpath.Split(new string[]
				{
					text5
				}, StringSplitOptions.None);
				text4 = array[0];
				text6 = array[1];
			}
			else
			{
				text6 = inpath;
			}
			if (text6 != null && text6 != string.Empty)
			{
				text = Path.GetFileName(text6);
				if (text != null && text != string.Empty)
				{
					text = Regex.Replace(text, "\\.(vap|vam|vaj|vab)$", string.Empty);
					string directoryName = Path.GetDirectoryName(text6);
					if (directoryName != null && directoryName != string.Empty)
					{
						string text7 = Regex.Replace(directoryName, "^" + this.GetStoreRootPath(false), string.Empty);
						if (text7.Contains("/"))
						{
							text3 = Regex.Replace(text7, "/.*", string.Empty);
							text2 = Regex.Replace(text7, text3 + "/", string.Empty);
						}
						else
						{
							text2 = text7;
						}
					}
				}
			}
			return new string[]
			{
				text2,
				text3,
				text,
				text4
			};
		}

		// Token: 0x060064C8 RID: 25800 RVA: 0x00173FFC File Offset: 0x001723FC
		public void SetNamesFromPath(string path)
		{
			string[] array = this.PathToNames(path);
			this.storeFolderName = array[0];
			this.creatorName = array[1];
			this.storeName = array[2];
			this.package = array[3];
		}

		// Token: 0x060064C9 RID: 25801 RVA: 0x00174038 File Offset: 0x00172438
		public string GetStoreFolderPath(bool includePackage = true)
		{
			string text = null;
			string storeRootPath = this.GetStoreRootPath(includePackage);
			if (storeRootPath != null)
			{
				text = storeRootPath;
				if (this.creatorName != null && this.creatorName != string.Empty)
				{
					text = text + this.creatorName + "/";
				}
				if (this.storeFolderName != null && this.storeFolderName != string.Empty)
				{
					string str = Regex.Replace(this.storeFolderName, "/$", string.Empty);
					text = text + str + "/";
				}
			}
			return text;
		}

		// Token: 0x060064CA RID: 25802 RVA: 0x001740D0 File Offset: 0x001724D0
		public string GetStorePathBase()
		{
			string storeFolderPath = this.GetStoreFolderPath(true);
			return storeFolderPath + this.storeName;
		}

		// Token: 0x060064CB RID: 25803 RVA: 0x001740F4 File Offset: 0x001724F4
		public string GetPresetNameFromFilePath(string fpath)
		{
			VarFileEntry varFileEntry = FileManager.GetVarFileEntry(fpath);
			string text = string.Empty;
			string text2 = fpath;
			if (varFileEntry != null)
			{
				text = varFileEntry.Package.Uid + ":";
				text2 = varFileEntry.InternalSlashPath;
			}
			string storeFolderPath = this.GetStoreFolderPath(false);
			string text3 = text2.Replace(storeFolderPath, string.Empty);
			string text4 = null;
			if (text3 == text2)
			{
				SuperController.LogError("Preset path " + fpath + " is not compatible with store folder path " + storeFolderPath);
			}
			else
			{
				string str = string.Empty;
				string text5 = string.Empty;
				if (text3.Contains("/"))
				{
					str = Path.GetDirectoryName(text3) + "/";
					text5 = Path.GetFileName(text3);
				}
				else
				{
					str = string.Empty;
					text5 = text3;
				}
				string text6 = text5.Replace(this.storeName + "_", string.Empty);
				if (text6 == text5)
				{
					SuperController.LogError("Preset " + text5 + " is not a preset for current store " + this.storeName);
				}
				else
				{
					if (text6.Contains("__"))
					{
						text = Regex.Replace(text6, "__.*", string.Empty);
						if (FileManager.IsPackage(text))
						{
							text += ":";
							text6 = Regex.Replace(text6, ".*__", string.Empty);
						}
					}
					text4 = text6.Replace(".vap", string.Empty);
					text4 = text + str + text4;
				}
			}
			return text4;
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x0017427C File Offset: 0x0017267C
		protected void ClearLockStorablesInList(List<PresetManager.Storable> sts)
		{
			if (sts != null)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				foreach (PresetManager.Storable storable in sts)
				{
					JSONStorable storable2 = storable.storable;
					if (storable2 != null && (this.ignoreExclude || !storable2.exclude))
					{
						if (storable.specificKey != null && storable.specificKey != string.Empty)
						{
							JSONStorableParam param = storable2.GetParam(storable.specificKey);
							if (param != null)
							{
								param.ClearLock(storeFolderPath);
							}
							else
							{
								storable2.ClearCustomAppearanceParamLock(storable.specificKey, storeFolderPath);
								storable2.ClearCustomPhysicalParamLock(storable.specificKey, storeFolderPath);
							}
						}
						else
						{
							storable2.ClearAppearanceLock(storeFolderPath);
							storable2.ClearPhysicalLock(storeFolderPath);
						}
					}
				}
			}
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x00174374 File Offset: 0x00172774
		protected void LockStorablesInList(List<PresetManager.Storable> sts)
		{
			if (sts != null && this._paramsLocked && !this._tempUnlockParams)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				foreach (PresetManager.Storable storable in sts)
				{
					JSONStorable storable2 = storable.storable;
					if (storable2 != null && (this.ignoreExclude || !storable2.exclude) && storable2.gameObject.activeInHierarchy)
					{
						if (storable.specificKey != null && storable.specificKey != string.Empty)
						{
							JSONStorableParam param = storable2.GetParam(storable.specificKey);
							if (param != null)
							{
								param.SetLock(storeFolderPath);
							}
							else
							{
								if (this.includeAppearance)
								{
									storable2.SetCustomAppearanceParamLock(storable.specificKey, storeFolderPath);
								}
								if (this.includePhysical)
								{
									storable2.SetCustomPhysicalParamLock(storable.specificKey, storeFolderPath);
								}
							}
						}
						else
						{
							if (this.includeAppearance)
							{
								storable2.SetAppearanceLock(storeFolderPath);
							}
							if (this.includePhysical)
							{
								storable2.SetPhysicalLock(storeFolderPath);
							}
						}
					}
				}
			}
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x001744BC File Offset: 0x001728BC
		public void SyncParamsLocked()
		{
			this.ClearLockStorablesInList(this.storables);
			this.ClearLockStorablesInList(this.optionalStorables);
			this.ClearLockStorablesInList(this.optionalStorables2);
			this.ClearLockStorablesInList(this.optionalStorables3);
			this.LockStorablesInList(this.storables);
			if (this.includeOptional && this.storeOptionalStorables)
			{
				this.LockStorablesInList(this.optionalStorables);
			}
			if (this.includeOptional2 && this.storeOptionalStorables2)
			{
				this.LockStorablesInList(this.optionalStorables2);
			}
			if (this.includeOptional3 && this.storeOptionalStorables3)
			{
				this.LockStorablesInList(this.optionalStorables3);
			}
			this.RefreshDynamicStorables();
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x060064CF RID: 25807 RVA: 0x00174571 File Offset: 0x00172971
		// (set) Token: 0x060064D0 RID: 25808 RVA: 0x00174579 File Offset: 0x00172979
		protected bool tempUnlockParams
		{
			get
			{
				return this._tempUnlockParams;
			}
			set
			{
				if (this._tempUnlockParams != value)
				{
					this._tempUnlockParams = value;
					this.SyncParamsLocked();
				}
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x060064D1 RID: 25809 RVA: 0x00174594 File Offset: 0x00172994
		// (set) Token: 0x060064D2 RID: 25810 RVA: 0x0017459C File Offset: 0x0017299C
		public bool paramsLocked
		{
			get
			{
				return this._paramsLocked;
			}
			set
			{
				if (this._paramsLocked != value)
				{
					this._paramsLocked = value;
					this.SyncParamsLocked();
				}
			}
		}

		// Token: 0x060064D3 RID: 25811 RVA: 0x001745B8 File Offset: 0x001729B8
		protected void StoreStorablesInList(JSONClass jc, List<PresetManager.Storable> sts, bool storeAll)
		{
			if (sts != null)
			{
				foreach (PresetManager.Storable storable in sts)
				{
					JSONStorable storable2 = storable.storable;
					if (storable2 != null && (this.ignoreExclude || !storable2.exclude) && storable2.gameObject.activeInHierarchy)
					{
						try
						{
							storable2.isPresetStore = true;
							JSONClass json = storable2.GetJSON(this.includePhysical, this.includeAppearance, storeAll);
							storable2.isPresetStore = false;
							if (storable2.needsStore)
							{
								if (storable.specificKey != null && storable.specificKey != string.Empty)
								{
									JSONClass jsonclass = new JSONClass();
									foreach (string text in json.Keys)
									{
										if (text == storable.specificKey || text == "id")
										{
											jsonclass[text] = json[text];
										}
									}
									jc["storables"].Add(jsonclass);
								}
								else
								{
									jc["storables"].Add(json);
								}
							}
						}
						catch (Exception ex)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during Preset Store of ",
								storable2.storeId,
								": ",
								ex
							}));
						}
					}
				}
			}
		}

		// Token: 0x060064D4 RID: 25812 RVA: 0x001747A8 File Offset: 0x00172BA8
		protected virtual void StorePresetBinary()
		{
		}

		// Token: 0x060064D5 RID: 25813 RVA: 0x001747AC File Offset: 0x00172BAC
		protected void StoreStorables(JSONClass jc, bool storeAll)
		{
			if (jc != null)
			{
				this.RefreshDynamicStorables();
				jc["storables"] = new JSONArray();
				if (this.optionalFirst)
				{
					if (this.includeOptional && this.storeOptionalStorables)
					{
						this.StoreStorablesInList(jc, this.optionalStorables, storeAll);
					}
					if (this.includeOptional2 && this.storeOptionalStorables2)
					{
						this.StoreStorablesInList(jc, this.optionalStorables2, storeAll);
					}
					if (this.includeOptional3 && this.storeOptionalStorables3)
					{
						this.StoreStorablesInList(jc, this.optionalStorables3, storeAll);
					}
				}
				this.StoreStorablesInList(jc, this.storables, storeAll);
				this.StoreStorablesInList(jc, this.dynamicStorables, storeAll);
				if (!this.optionalFirst)
				{
					if (this.includeOptional && this.storeOptionalStorables)
					{
						this.StoreStorablesInList(jc, this.optionalStorables, storeAll);
					}
					if (this.includeOptional2 && this.storeOptionalStorables2)
					{
						this.StoreStorablesInList(jc, this.optionalStorables2, storeAll);
					}
					if (this.includeOptional3 && this.storeOptionalStorables3)
					{
						this.StoreStorablesInList(jc, this.optionalStorables3, storeAll);
					}
				}
			}
		}

		// Token: 0x060064D6 RID: 25814 RVA: 0x001748E8 File Offset: 0x00172CE8
		protected void PreRestoreStorable(JSONStorable js)
		{
			if (js != null && (this.ignoreExclude || !js.exclude))
			{
				js.isPresetRestore = true;
				js.mergeRestore = this.isMergeRestore;
				try
				{
					js.PreRestore();
					js.PreRestore(this.includePhysical, this.includeAppearance);
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during PreRestore of ",
						js.storeId,
						": ",
						ex
					}));
				}
				js.mergeRestore = false;
				js.isPresetRestore = false;
			}
		}

		// Token: 0x060064D7 RID: 25815 RVA: 0x00174998 File Offset: 0x00172D98
		protected void PreRestore()
		{
			if (this.optionalFirst)
			{
				if (this.includeOptional && this.optionalStorables != null)
				{
					foreach (PresetManager.Storable storable in this.optionalStorables)
					{
						JSONStorable storable2 = storable.storable;
						this.PreRestoreStorable(storable2);
					}
				}
				if (this.includeOptional2 && this.optionalStorables2 != null)
				{
					foreach (PresetManager.Storable storable3 in this.optionalStorables2)
					{
						JSONStorable storable4 = storable3.storable;
						this.PreRestoreStorable(storable4);
					}
				}
				if (this.includeOptional3 && this.optionalStorables3 != null)
				{
					foreach (PresetManager.Storable storable5 in this.optionalStorables3)
					{
						JSONStorable storable6 = storable5.storable;
						this.PreRestoreStorable(storable6);
					}
				}
			}
			if (this.storables != null)
			{
				foreach (PresetManager.Storable storable7 in this.storables)
				{
					JSONStorable storable8 = storable7.storable;
					this.PreRestoreStorable(storable8);
				}
			}
			if (this.dynamicStorables != null)
			{
				foreach (PresetManager.Storable storable9 in this.dynamicStorables)
				{
					JSONStorable storable10 = storable9.storable;
					this.PreRestoreStorable(storable10);
				}
			}
			if (!this.optionalFirst)
			{
				if (this.includeOptional && this.optionalStorables != null)
				{
					foreach (PresetManager.Storable storable11 in this.optionalStorables)
					{
						JSONStorable storable12 = storable11.storable;
						this.PreRestoreStorable(storable12);
					}
				}
				if (this.includeOptional2 && this.optionalStorables2 != null)
				{
					foreach (PresetManager.Storable storable13 in this.optionalStorables2)
					{
						JSONStorable storable14 = storable13.storable;
						this.PreRestoreStorable(storable14);
					}
				}
				if (this.includeOptional3 && this.optionalStorables3 != null)
				{
					foreach (PresetManager.Storable storable15 in this.optionalStorables3)
					{
						JSONStorable storable16 = storable15.storable;
						this.PreRestoreStorable(storable16);
					}
				}
			}
		}

		// Token: 0x060064D8 RID: 25816 RVA: 0x00174D0C File Offset: 0x0017310C
		protected void Restore(JSONClass jc)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			Dictionary<string, JSONStorable> dictionary2 = new Dictionary<string, JSONStorable>();
			if (this.includeOptional && this.optionalStorables != null)
			{
				foreach (PresetManager.Storable storable in this.optionalStorables)
				{
					JSONStorable storable2 = storable.storable;
					if (storable2 != null && (this.ignoreExclude || !storable2.exclude) && !dictionary2.ContainsKey(storable2.storeId))
					{
						dictionary2.Add(storable2.storeId, storable2);
					}
				}
			}
			if (this.includeOptional2 && this.optionalStorables2 != null)
			{
				foreach (PresetManager.Storable storable3 in this.optionalStorables2)
				{
					JSONStorable storable4 = storable3.storable;
					if (storable4 != null && (this.ignoreExclude || !storable4.exclude) && !dictionary2.ContainsKey(storable4.storeId))
					{
						dictionary2.Add(storable4.storeId, storable4);
					}
				}
			}
			if (this.includeOptional3 && this.optionalStorables3 != null)
			{
				foreach (PresetManager.Storable storable5 in this.optionalStorables3)
				{
					JSONStorable storable6 = storable5.storable;
					if (storable6 != null && (this.ignoreExclude || !storable6.exclude) && !dictionary2.ContainsKey(storable6.storeId))
					{
						dictionary2.Add(storable6.storeId, storable6);
					}
				}
			}
			if (this.storables != null)
			{
				foreach (PresetManager.Storable storable7 in this.storables)
				{
					JSONStorable storable8 = storable7.storable;
					if (storable8 != null && (this.ignoreExclude || !storable8.exclude) && !dictionary2.ContainsKey(storable8.storeId))
					{
						dictionary2.Add(storable8.storeId, storable8);
					}
				}
			}
			IEnumerator enumerator5 = jc["storables"].AsArray.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					object obj = enumerator5.Current;
					JSONClass jsonclass = (JSONClass)obj;
					string text = jsonclass["id"];
					JSONStorable storable9;
					if (dictionary2.TryGetValue(text, out storable9))
					{
						bool flag;
						if (!this.specificKeyStorables.TryGetValue(text, out flag))
						{
							flag = false;
						}
						storable9.isPresetRestore = true;
						storable9.mergeRestore = this.isMergeRestore;
						try
						{
							storable9.RestoreFromJSON(jsonclass, this.includePhysical, this.includeAppearance, null, !flag && this.setUnlistedParamsToDefault);
						}
						catch (Exception ex)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during Restore of ",
								storable9.storeId,
								": ",
								ex
							}));
						}
						storable9.mergeRestore = false;
						storable9.isPresetRestore = false;
						if (!dictionary.ContainsKey(jsonclass["id"]))
						{
							dictionary.Add(jsonclass["id"], true);
						}
					}
					else
					{
						foreach (PresetManager.Storable storable10 in this.dynamicStorables)
						{
							storable9 = storable10.storable;
							if (storable9 != null && storable9.storeId == text && (this.ignoreExclude || !storable9.exclude) && (!storable9.onlyStoreIfActive || storable9.gameObject.activeInHierarchy))
							{
								bool flag2;
								if (!this.specificKeyStorables.TryGetValue(text, out flag2))
								{
									flag2 = false;
								}
								storable9.isPresetRestore = true;
								storable9.mergeRestore = this.isMergeRestore;
								try
								{
									storable9.RestoreFromJSON(jsonclass, this.includePhysical, this.includeAppearance, null, !flag2 && this.setUnlistedParamsToDefault);
								}
								catch (Exception ex2)
								{
									SuperController.LogError(string.Concat(new object[]
									{
										"Exception during Restore of ",
										storable9.storeId,
										": ",
										ex2
									}));
								}
								storable9.mergeRestore = false;
								storable9.isPresetRestore = false;
								if (!dictionary.ContainsKey(jsonclass["id"]))
								{
									dictionary.Add(jsonclass["id"], true);
								}
								break;
							}
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator5 as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			JSONClass jc2 = new JSONClass();
			if (this.setUnlistedParamsToDefault)
			{
				foreach (PresetManager.Storable storable11 in this.storables)
				{
					JSONStorable storable12 = storable11.storable;
					if (storable12 != null && (this.ignoreExclude || !storable12.exclude) && !dictionary.ContainsKey(storable12.storeId) && !this.specificKeyStorables.ContainsKey(storable12.storeId))
					{
						storable12.isPresetRestore = true;
						try
						{
							storable12.RestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, null, true);
						}
						catch (Exception ex3)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during Restore of ",
								storable12.storeId,
								": ",
								ex3
							}));
						}
						storable12.isPresetRestore = false;
					}
				}
				foreach (PresetManager.Storable storable13 in this.dynamicStorables)
				{
					JSONStorable storable14 = storable13.storable;
					if (storable14 != null && (this.ignoreExclude || !storable14.exclude) && (!storable14.onlyStoreIfActive || storable14.gameObject.activeInHierarchy) && !dictionary.ContainsKey(storable14.storeId))
					{
						storable14.isPresetRestore = true;
						try
						{
							storable14.RestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, null, true);
						}
						catch (Exception ex4)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during Restore of ",
								storable14.storeId,
								": ",
								ex4
							}));
						}
						storable14.isPresetRestore = false;
					}
				}
				if (this.includeOptional && this.setOptionalToDefaultOnRestore)
				{
					foreach (PresetManager.Storable storable15 in this.optionalStorables)
					{
						JSONStorable storable16 = storable15.storable;
						if (storable16 != null && (this.ignoreExclude || !storable16.exclude) && !dictionary.ContainsKey(storable16.storeId) && !this.specificKeyStorables.ContainsKey(storable16.storeId))
						{
							storable16.isPresetRestore = true;
							try
							{
								storable16.RestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, null, true);
							}
							catch (Exception ex5)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during Restore of ",
									storable16.storeId,
									": ",
									ex5
								}));
							}
							storable16.isPresetRestore = false;
						}
					}
				}
				if (this.includeOptional2 && this.setOptional2ToDefaultOnRestore)
				{
					foreach (PresetManager.Storable storable17 in this.optionalStorables2)
					{
						JSONStorable storable18 = storable17.storable;
						if (storable18 != null && (this.ignoreExclude || !storable18.exclude) && !dictionary.ContainsKey(storable18.storeId) && !this.specificKeyStorables.ContainsKey(storable18.storeId))
						{
							storable18.isPresetRestore = true;
							try
							{
								storable18.RestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, null, true);
							}
							catch (Exception ex6)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during Restore of ",
									storable18.storeId,
									": ",
									ex6
								}));
							}
							storable18.isPresetRestore = false;
						}
					}
				}
				if (this.includeOptional3 && this.setOptional3ToDefaultOnRestore)
				{
					foreach (PresetManager.Storable storable19 in this.optionalStorables3)
					{
						JSONStorable storable20 = storable19.storable;
						if (storable20 != null && (this.ignoreExclude || !storable20.exclude) && !dictionary.ContainsKey(storable20.storeId) && !this.specificKeyStorables.ContainsKey(storable20.storeId))
						{
							storable20.isPresetRestore = true;
							try
							{
								storable20.RestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, null, true);
							}
							catch (Exception ex7)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during Restore of ",
									storable20.storeId,
									": ",
									ex7
								}));
							}
							storable20.isPresetRestore = false;
						}
					}
				}
			}
		}

		// Token: 0x060064D9 RID: 25817 RVA: 0x00175910 File Offset: 0x00173D10
		protected void LateRestore(JSONClass jc)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			Dictionary<string, JSONStorable> dictionary2 = new Dictionary<string, JSONStorable>();
			if (this.includeOptional && this.optionalStorables != null)
			{
				foreach (PresetManager.Storable storable in this.optionalStorables)
				{
					JSONStorable storable2 = storable.storable;
					if (storable2 != null && (this.ignoreExclude || !storable2.exclude) && !dictionary2.ContainsKey(storable2.storeId))
					{
						dictionary2.Add(storable2.storeId, storable2);
					}
				}
			}
			if (this.includeOptional2 && this.optionalStorables2 != null)
			{
				foreach (PresetManager.Storable storable3 in this.optionalStorables2)
				{
					JSONStorable storable4 = storable3.storable;
					if (storable4 != null && (this.ignoreExclude || !storable4.exclude) && !dictionary2.ContainsKey(storable4.storeId))
					{
						dictionary2.Add(storable4.storeId, storable4);
					}
				}
			}
			if (this.includeOptional3 && this.optionalStorables3 != null)
			{
				foreach (PresetManager.Storable storable5 in this.optionalStorables3)
				{
					JSONStorable storable6 = storable5.storable;
					if (storable6 != null && (this.ignoreExclude || !storable6.exclude) && !dictionary2.ContainsKey(storable6.storeId))
					{
						dictionary2.Add(storable6.storeId, storable6);
					}
				}
			}
			if (this.storables != null)
			{
				foreach (PresetManager.Storable storable7 in this.storables)
				{
					JSONStorable storable8 = storable7.storable;
					if (storable8 != null && (this.ignoreExclude || !storable8.exclude) && !dictionary2.ContainsKey(storable8.storeId))
					{
						dictionary2.Add(storable8.storeId, storable8);
					}
				}
			}
			IEnumerator enumerator5 = jc["storables"].AsArray.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					object obj = enumerator5.Current;
					JSONClass jsonclass = (JSONClass)obj;
					string text = jsonclass["id"];
					JSONStorable storable9;
					if (dictionary2.TryGetValue(text, out storable9))
					{
						bool flag;
						if (!this.specificKeyStorables.TryGetValue(text, out flag))
						{
							flag = false;
						}
						storable9.isPresetRestore = true;
						storable9.mergeRestore = this.isMergeRestore;
						try
						{
							storable9.LateRestoreFromJSON(jsonclass, this.includePhysical, this.includeAppearance, !flag && this.setUnlistedParamsToDefault);
						}
						catch (Exception ex)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during LateRestore of ",
								storable9.storeId,
								": ",
								ex
							}));
						}
						storable9.mergeRestore = false;
						storable9.isPresetRestore = false;
						if (!dictionary.ContainsKey(jsonclass["id"]))
						{
							dictionary.Add(jsonclass["id"], true);
						}
					}
					else
					{
						foreach (PresetManager.Storable storable10 in this.dynamicStorables)
						{
							storable9 = storable10.storable;
							if (storable9 != null && storable9.storeId == text && (this.ignoreExclude || !storable9.exclude) && (!storable9.onlyStoreIfActive || storable9.gameObject.activeInHierarchy))
							{
								bool flag2;
								if (!this.specificKeyStorables.TryGetValue(text, out flag2))
								{
									flag2 = false;
								}
								storable9.isPresetRestore = true;
								storable9.mergeRestore = this.isMergeRestore;
								try
								{
									storable9.LateRestoreFromJSON(jsonclass, this.includePhysical, this.includeAppearance, !flag2 && this.setUnlistedParamsToDefault);
								}
								catch (Exception ex2)
								{
									SuperController.LogError(string.Concat(new object[]
									{
										"Exception during LateRestore of ",
										storable9.storeId,
										": ",
										ex2
									}));
								}
								storable9.mergeRestore = false;
								storable9.isPresetRestore = false;
								if (!dictionary.ContainsKey(jsonclass["id"]))
								{
									dictionary.Add(jsonclass["id"], true);
								}
								break;
							}
						}
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator5 as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			JSONClass jc2 = new JSONClass();
			if (this.setUnlistedParamsToDefault)
			{
				foreach (PresetManager.Storable storable11 in this.storables)
				{
					JSONStorable storable12 = storable11.storable;
					if (storable12 != null && (this.ignoreExclude || !storable12.exclude) && !dictionary.ContainsKey(storable12.storeId) && !this.specificKeyStorables.ContainsKey(storable12.storeId))
					{
						storable12.isPresetRestore = true;
						try
						{
							storable12.LateRestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, true);
						}
						catch (Exception ex3)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during LateRestore of ",
								storable12.storeId,
								": ",
								ex3
							}));
						}
						storable12.isPresetRestore = false;
					}
				}
				foreach (PresetManager.Storable storable13 in this.dynamicStorables)
				{
					JSONStorable storable14 = storable13.storable;
					if (storable14 != null && (this.ignoreExclude || !storable14.exclude) && (!storable14.onlyStoreIfActive || storable14.gameObject.activeInHierarchy) && !dictionary.ContainsKey(storable14.storeId))
					{
						storable14.isPresetRestore = true;
						try
						{
							storable14.LateRestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, true);
						}
						catch (Exception ex4)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception during LateRestore of ",
								storable14.storeId,
								": ",
								ex4
							}));
						}
						storable14.isPresetRestore = false;
					}
				}
				if (this.includeOptional && this.setOptionalToDefaultOnRestore)
				{
					foreach (PresetManager.Storable storable15 in this.optionalStorables)
					{
						JSONStorable storable16 = storable15.storable;
						if (storable16 != null && (this.ignoreExclude || !storable16.exclude) && !dictionary.ContainsKey(storable16.storeId) && !this.specificKeyStorables.ContainsKey(storable16.storeId))
						{
							storable16.isPresetRestore = true;
							try
							{
								storable16.LateRestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, true);
							}
							catch (Exception ex5)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during LateRestore of ",
									storable16.storeId,
									": ",
									ex5
								}));
							}
							storable16.isPresetRestore = false;
						}
					}
				}
				if (this.includeOptional2 && this.setOptional2ToDefaultOnRestore)
				{
					foreach (PresetManager.Storable storable17 in this.optionalStorables2)
					{
						JSONStorable storable18 = storable17.storable;
						if (storable18 != null && (this.ignoreExclude || !storable18.exclude) && !dictionary.ContainsKey(storable18.storeId) && !this.specificKeyStorables.ContainsKey(storable18.storeId))
						{
							storable18.isPresetRestore = true;
							try
							{
								storable18.LateRestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, true);
							}
							catch (Exception ex6)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during LateRestore of ",
									storable18.storeId,
									": ",
									ex6
								}));
							}
							storable18.isPresetRestore = false;
						}
					}
				}
				if (this.includeOptional3 && this.setOptional3ToDefaultOnRestore)
				{
					foreach (PresetManager.Storable storable19 in this.optionalStorables3)
					{
						JSONStorable storable20 = storable19.storable;
						if (storable20 != null && (this.ignoreExclude || !storable20.exclude) && !dictionary.ContainsKey(storable20.storeId) && !this.specificKeyStorables.ContainsKey(storable20.storeId))
						{
							storable20.isPresetRestore = true;
							try
							{
								storable20.LateRestoreFromJSON(jc2, this.includePhysical, this.includeAppearance, true);
							}
							catch (Exception ex7)
							{
								SuperController.LogError(string.Concat(new object[]
								{
									"Exception during LateRestore of ",
									storable20.storeId,
									": ",
									ex7
								}));
							}
							storable20.isPresetRestore = false;
						}
					}
				}
			}
		}

		// Token: 0x060064DA RID: 25818 RVA: 0x0017650C File Offset: 0x0017490C
		protected void PostRestoreStorable(JSONStorable js)
		{
			if (js != null && (this.ignoreExclude || !js.exclude))
			{
				js.isPresetRestore = true;
				js.mergeRestore = this.isMergeRestore;
				try
				{
					js.PostRestore();
					js.PostRestore(this.includePhysical, this.includeAppearance);
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during PostRestore of ",
						js.storeId,
						": ",
						ex
					}));
				}
				js.mergeRestore = false;
				js.isPresetRestore = false;
			}
		}

		// Token: 0x060064DB RID: 25819 RVA: 0x001765BC File Offset: 0x001749BC
		protected void PostRestore()
		{
			if (this.optionalFirst)
			{
				if (this.includeOptional && this.optionalStorables != null)
				{
					foreach (PresetManager.Storable storable in this.optionalStorables)
					{
						JSONStorable storable2 = storable.storable;
						this.PostRestoreStorable(storable2);
					}
				}
				if (this.includeOptional2 && this.optionalStorables2 != null)
				{
					foreach (PresetManager.Storable storable3 in this.optionalStorables2)
					{
						JSONStorable storable4 = storable3.storable;
						this.PostRestoreStorable(storable4);
					}
				}
				if (this.includeOptional3 && this.optionalStorables3 != null)
				{
					foreach (PresetManager.Storable storable5 in this.optionalStorables3)
					{
						JSONStorable storable6 = storable5.storable;
						this.PostRestoreStorable(storable6);
					}
				}
			}
			if (this.storables != null)
			{
				foreach (PresetManager.Storable storable7 in this.storables)
				{
					JSONStorable storable8 = storable7.storable;
					this.PostRestoreStorable(storable8);
				}
			}
			if (this.dynamicStorables != null)
			{
				foreach (PresetManager.Storable storable9 in this.dynamicStorables)
				{
					JSONStorable storable10 = storable9.storable;
					this.PostRestoreStorable(storable10);
				}
			}
			if (!this.optionalFirst)
			{
				if (this.includeOptional && this.optionalStorables != null)
				{
					foreach (PresetManager.Storable storable11 in this.optionalStorables)
					{
						JSONStorable storable12 = storable11.storable;
						this.PostRestoreStorable(storable12);
					}
				}
				if (this.includeOptional2 && this.optionalStorables2 != null)
				{
					foreach (PresetManager.Storable storable13 in this.optionalStorables2)
					{
						JSONStorable storable14 = storable13.storable;
						this.PostRestoreStorable(storable14);
					}
				}
				if (this.includeOptional3 && this.optionalStorables3 != null)
				{
					foreach (PresetManager.Storable storable15 in this.optionalStorables3)
					{
						JSONStorable storable16 = storable15.storable;
						this.PostRestoreStorable(storable16);
					}
				}
			}
		}

		// Token: 0x060064DC RID: 25820 RVA: 0x00176930 File Offset: 0x00174D30
		protected void FilterStorables(JSONClass inputjc, JSONClass outputjc, List<PresetManager.Storable> sts)
		{
			Dictionary<string, PresetManager.Storable> dictionary = new Dictionary<string, PresetManager.Storable>();
			JSONArray asArray = inputjc["storables"].AsArray;
			JSONArray asArray2 = outputjc["storables"].AsArray;
			foreach (PresetManager.Storable storable in sts)
			{
				JSONStorable storable2 = storable.storable;
				if (storable2 != null && (this.ignoreExclude || !storable2.exclude))
				{
					if (storable.specificKey != null && storable.specificKey != string.Empty)
					{
						string key = storable.storable.storeId + ":" + storable.specificKey;
						if (!dictionary.ContainsKey(key))
						{
							dictionary.Add(key, storable);
						}
					}
					else if (!dictionary.ContainsKey(storable.storable.storeId))
					{
						dictionary.Add(storable.storable.storeId, storable);
					}
				}
			}
			Dictionary<string, JSONClass> dictionary2 = new Dictionary<string, JSONClass>();
			IEnumerator enumerator2 = asArray2.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj = enumerator2.Current;
					JSONClass jsonclass = (JSONClass)obj;
					if (jsonclass["id"] != null)
					{
						dictionary2.Add(jsonclass["id"], jsonclass);
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
			IEnumerator enumerator3 = asArray.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					object obj2 = enumerator3.Current;
					JSONClass jsonclass2 = (JSONClass)obj2;
					if (jsonclass2["id"] != null)
					{
						PresetManager.Storable storable3;
						if (dictionary.TryGetValue(jsonclass2["id"], out storable3))
						{
							JSONClass jsonclass3;
							if (storable3.storable != null && !dictionary2.TryGetValue(storable3.storable.storeId, out jsonclass3))
							{
								asArray2.Add(jsonclass2);
								dictionary2.Add(storable3.storable.storeId, jsonclass2);
							}
						}
						else
						{
							foreach (string str in jsonclass2.Keys)
							{
								if (dictionary.TryGetValue(jsonclass2["id"] + ":" + str, out storable3) && storable3.storable != null)
								{
									JSONClass jsonclass4;
									if (!dictionary2.TryGetValue(storable3.storable.storeId, out jsonclass4))
									{
										jsonclass4 = new JSONClass();
										jsonclass4["id"] = storable3.storable.storeId;
										asArray2.Add(jsonclass4);
										dictionary2.Add(storable3.storable.storeId, jsonclass4);
									}
									jsonclass4[storable3.specificKey] = jsonclass2[storable3.specificKey];
								}
							}
						}
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator3 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}

		// Token: 0x060064DD RID: 25821 RVA: 0x00176CE4 File Offset: 0x001750E4
		protected void RestoreStorables(JSONClass jc)
		{
			if (jc != null)
			{
				this.tempUnlockParams = true;
				this.RefreshDynamicStorables();
				this.PreRestore();
				this.Restore(jc);
				this.LateRestore(jc);
				this.PostRestore();
				if (this.postLoadEvent != null && (!this.onlyCallPostLoadEventIfIncludeOptional || this.includeOptional))
				{
					this.postLoadEvent.Invoke();
				}
				if (this.postLoadOptimizeEvent != null && UserPreferences.singleton != null && UserPreferences.singleton.optimizeMemoryOnPresetLoad)
				{
					this.postLoadOptimizeEvent.Invoke();
				}
				if (this.conditionalLoadEvents != null)
				{
					foreach (PresetManager.ConditionalLoadEvent conditionalLoadEvent in this.conditionalLoadEvents)
					{
						string flag = conditionalLoadEvent.flag;
						if (flag != null && flag != string.Empty)
						{
							if (jc[flag] != null && jc[flag].AsBool)
							{
								if (conditionalLoadEvent.ifEvent != null)
								{
									conditionalLoadEvent.ifEvent.Invoke();
								}
							}
							else if (conditionalLoadEvent.elseEvent != null)
							{
								conditionalLoadEvent.elseEvent.Invoke();
							}
						}
					}
				}
				this.tempUnlockParams = false;
			}
		}

		// Token: 0x060064DE RID: 25822 RVA: 0x00176E27 File Offset: 0x00175227
		public virtual void RestorePresetBinary()
		{
		}

		// Token: 0x060064DF RID: 25823 RVA: 0x00176E2C File Offset: 0x0017522C
		public void CreateStoreFolderPath()
		{
			string storeFolderPath = this.GetStoreFolderPath(false);
			if (storeFolderPath != null && Application.isPlaying)
			{
				FileManager.CreateDirectory(storeFolderPath);
			}
		}

		// Token: 0x060064E0 RID: 25824 RVA: 0x00176E58 File Offset: 0x00175258
		public bool CheckPresetReadyForStore()
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				if (this.IsPresetInPackage())
				{
					VarPackage varPackage = FileManager.GetPackage(this.presetPackageName);
					if (varPackage == null || !varPackage.IsSimulated)
					{
						return false;
					}
				}
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060064E1 RID: 25825 RVA: 0x00176F00 File Offset: 0x00175300
		public void DeletePreset()
		{
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				if (this.IsPresetInPackage())
				{
					VarPackage varPackage = FileManager.GetPackage(this.presetPackageName);
					if (varPackage == null || !varPackage.IsSimulated)
					{
						return;
					}
				}
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
				{
					string text = string.Concat(new string[]
					{
						storeFolderPath,
						this.presetSubPath,
						this.storeName,
						"_",
						this.presetSubName,
						".vap"
					});
					try
					{
						if (FileManager.FileExists(text, false, false))
						{
							FileManager.DeleteFile(text);
						}
						if (FileManager.FileExists(text + ".jpg", false, false))
						{
							FileManager.DeleteFile(text + ".jpg");
						}
					}
					catch (Exception ex)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception while trying to delete ",
							text,
							" ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x060064E2 RID: 25826 RVA: 0x00177058 File Offset: 0x00175458
		public bool CheckPresetExistance()
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
				{
					string path = string.Concat(new string[]
					{
						this.presetPackagePath,
						storeFolderPath,
						this.presetSubPath,
						this.storeName,
						"_",
						this.presetSubName,
						".vap"
					});
					result = FileManager.FileExists(path, false, false);
				}
			}
			return result;
		}

		// Token: 0x060064E3 RID: 25827 RVA: 0x00177120 File Offset: 0x00175520
		public bool IsPresetInPackage()
		{
			bool result = false;
			if (this.presetPackageName != null && this.presetPackageName != string.Empty)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060064E4 RID: 25828 RVA: 0x00177154 File Offset: 0x00175554
		public string GetFavoriteStorePath()
		{
			string result = null;
			string storeFolderPath = this.GetStoreFolderPath(false);
			if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
			{
				if (this.IsPresetInPackage())
				{
					result = string.Concat(new string[]
					{
						storeFolderPath,
						this.presetSubPath,
						this.storeName,
						"_",
						this.presetPackageName,
						"__",
						this.presetSubName,
						".vap.fav"
					});
				}
				else
				{
					result = string.Concat(new string[]
					{
						storeFolderPath,
						this.presetSubPath,
						this.storeName,
						"_",
						this.presetSubName,
						".vap.fav"
					});
				}
			}
			return result;
		}

		// Token: 0x060064E5 RID: 25829 RVA: 0x0017725C File Offset: 0x0017565C
		public bool IsFavorite()
		{
			string favoriteStorePath = this.GetFavoriteStorePath();
			return favoriteStorePath != null && FileManager.FileExists(favoriteStorePath, false, false);
		}

		// Token: 0x060064E6 RID: 25830 RVA: 0x00177288 File Offset: 0x00175688
		public void SetFavorite(bool b)
		{
			string favoriteStorePath = this.GetFavoriteStorePath();
			if (favoriteStorePath != null)
			{
				if (FileManager.FileExists(favoriteStorePath, false, false))
				{
					if (!b)
					{
						FileManager.DeleteFile(favoriteStorePath);
					}
				}
				else if (b)
				{
					string directoryName = FileManager.GetDirectoryName(favoriteStorePath, false);
					if (!FileManager.DirectoryExists(directoryName, false, false))
					{
						FileManager.CreateDirectory(directoryName);
					}
					FileManager.WriteAllText(favoriteStorePath, string.Empty);
				}
			}
		}

		// Token: 0x060064E7 RID: 25831 RVA: 0x001772EC File Offset: 0x001756EC
		public bool StorePreset(bool doScreenshot = false)
		{
			return this.StorePreset(true, doScreenshot);
		}

		// Token: 0x060064E8 RID: 25832 RVA: 0x001772F8 File Offset: 0x001756F8
		public bool StorePreset(bool storeAll, bool doScreenshot)
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string text = this.GetStoreFolderPath(false);
				if (text != null && text != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
				{
					this.CreateStoreFolderPath();
					if (this.presetPackageName != null)
					{
						VarPackage varPackage = FileManager.GetPackage(this.presetPackageName);
						if (varPackage != null && varPackage.IsSimulated)
						{
							text = varPackage.SlashPath + ":/" + text;
							FileManager.CreateDirectory(text);
						}
					}
					if (this.presetSubPath != null && this.presetSubPath != string.Empty)
					{
						FileManager.CreateDirectory(text + this.presetSubPath);
					}
					string text2 = string.Concat(new string[]
					{
						text,
						this.presetSubPath,
						this.storeName,
						"_",
						this.presetSubName,
						".vap"
					});
					JSONClass jsonclass = new JSONClass();
					jsonclass["setUnlistedParamsToDefault"].AsBool = storeAll;
					if (this.conditionalFlagsToStore != null && (this.storeConditionalFlagsAlways || (this.storeConditionalFlagsWhenStoreOptional && this.storeOptionalStorables) || (this.storeConditionalFlagsWhenStoreOptional2 && this.storeOptionalStorables2) || (this.storeConditionalFlagsWhenStoreOptional3 && this.storeOptionalStorables3)))
					{
						foreach (string aKey in this.conditionalFlagsToStore)
						{
							jsonclass[aKey].AsBool = true;
						}
					}
					FileManager.SetSaveDirFromFilePath(text2, true);
					this.StoreStorables(jsonclass, storeAll);
					StringBuilder stringBuilder = new StringBuilder(100000);
					jsonclass.ToString(string.Empty, stringBuilder);
					string value = stringBuilder.ToString();
					try
					{
						StreamWriter streamWriter = FileManager.OpenStreamWriter(text2);
						streamWriter.Write(value);
						streamWriter.Close();
						if (doScreenshot)
						{
							string text3 = string.Concat(new string[]
							{
								text,
								this.presetSubPath,
								this.storeName,
								"_",
								this.presetSubName,
								".jpg"
							});
							text3 = text3.Replace('/', '\\');
							SuperController.singleton.DoSaveScreenshot(text3, null);
						}
						result = true;
					}
					catch (Exception ex)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Exception while storing to ",
							text2,
							" ",
							ex
						}));
					}
					if (this.storePresetBinary)
					{
						this.StorePresetBinary();
					}
				}
				else
				{
					SuperController.LogError("Not all preset parameters set. Cannot store");
				}
			}
			else
			{
				SuperController.LogError("Item type set to None. Cannot store");
			}
			return result;
		}

		// Token: 0x060064E9 RID: 25833 RVA: 0x001775E8 File Offset: 0x001759E8
		public bool LoadPreset()
		{
			bool flag = this.LoadPresetPre(false);
			if (flag)
			{
				flag = this.LoadPresetPost();
				FileManager.PopLoadDir();
				return flag;
			}
			return false;
		}

		// Token: 0x060064EA RID: 25834 RVA: 0x00177614 File Offset: 0x00175A14
		public bool MergeLoadPreset()
		{
			bool flag = this.LoadPresetPre(true);
			if (flag)
			{
				flag = this.LoadPresetPost();
				FileManager.PopLoadDir();
				return flag;
			}
			return false;
		}

		// Token: 0x060064EB RID: 25835 RVA: 0x00177640 File Offset: 0x00175A40
		public void LoadPresetFromJSON(JSONClass inputJSON, bool isMerge = false)
		{
			string storeFolderPath = this.GetStoreFolderPath(false);
			bool flag = false;
			if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
			{
				string path = string.Concat(new string[]
				{
					this.presetPackagePath,
					storeFolderPath,
					this.presetSubPath,
					this.storeName,
					"_",
					this.presetSubName,
					".vap"
				});
				FileManager.PushLoadDirFromFilePath(path, false);
				flag = true;
			}
			this.LoadPresetPreFromJSON(inputJSON, isMerge);
			this.LoadPresetPost();
			if (flag)
			{
				FileManager.PopLoadDir();
			}
		}

		// Token: 0x060064EC RID: 25836 RVA: 0x00177718 File Offset: 0x00175B18
		protected void LoadPresetPreFromJSON(JSONClass inputJSON, bool isMerge = false)
		{
			this.lastLoadedJSON = inputJSON;
			if (isMerge)
			{
				this.isMergeRestore = true;
				this.setUnlistedParamsToDefault = false;
			}
			else
			{
				this.isMergeRestore = false;
				if (this.neverSetUnlistedParamsToDefault)
				{
					this.setUnlistedParamsToDefault = false;
				}
				else
				{
					this.setUnlistedParamsToDefault = true;
					if (inputJSON["setUnlistedParamsToDefault"] != null)
					{
						this.setUnlistedParamsToDefault = inputJSON["setUnlistedParamsToDefault"].AsBool;
						this.isMergeRestore = !this.setUnlistedParamsToDefault;
					}
				}
			}
			this.filteredJSON = new JSONClass();
			if (this.conditionalLoadEvents != null)
			{
				foreach (PresetManager.ConditionalLoadEvent conditionalLoadEvent in this.conditionalLoadEvents)
				{
					if (conditionalLoadEvent.flag != null && conditionalLoadEvent.flag != string.Empty && inputJSON[conditionalLoadEvent.flag] != null)
					{
						this.filteredJSON[conditionalLoadEvent.flag] = inputJSON[conditionalLoadEvent.flag];
					}
				}
			}
			if (this.setUnlistedParamsToDefault)
			{
				this.LoadDefaultsPreInternal();
			}
			else
			{
				this.filteredJSON["storables"] = new JSONArray();
			}
			if (this.optionalFirst)
			{
				if (this.includeOptional)
				{
					this.FilterStorables(inputJSON, this.filteredJSON, this.optionalStorables);
				}
				if (this.includeOptional2)
				{
					this.FilterStorables(inputJSON, this.filteredJSON, this.optionalStorables2);
				}
				if (this.includeOptional3)
				{
					this.FilterStorables(inputJSON, this.filteredJSON, this.optionalStorables3);
				}
			}
			this.FilterStorables(inputJSON, this.filteredJSON, this.storables);
			this.RefreshDynamicStorables();
			this.FilterStorables(inputJSON, this.filteredJSON, this.dynamicStorables);
			if (!this.optionalFirst)
			{
				if (this.includeOptional)
				{
					this.FilterStorables(inputJSON, this.filteredJSON, this.optionalStorables);
				}
				if (this.includeOptional2)
				{
					this.FilterStorables(inputJSON, this.filteredJSON, this.optionalStorables2);
				}
				if (this.includeOptional3)
				{
					this.FilterStorables(inputJSON, this.filteredJSON, this.optionalStorables3);
				}
			}
		}

		// Token: 0x060064ED RID: 25837 RVA: 0x00177950 File Offset: 0x00175D50
		public bool LoadPresetPre(bool isMerge = false)
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty && this._presetName != null && this._presetName != string.Empty)
				{
					string text = string.Concat(new string[]
					{
						this.presetPackagePath,
						storeFolderPath,
						this.presetSubPath,
						this.storeName,
						"_",
						this.presetSubName,
						".vap"
					});
					if (FileManager.FileExists(text, false, false))
					{
						string aJSON = string.Empty;
						try
						{
							FileManager.PushLoadDirFromFilePath(text, false);
							using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(text, true))
							{
								aJSON = fileEntryStreamReader.ReadToEnd();
								JSONNode jsonnode = JSON.Parse(aJSON);
								JSONClass asObject = jsonnode.AsObject;
								this.LoadPresetPreFromJSON(asObject, isMerge);
								result = true;
							}
						}
						catch (Exception ex)
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Exception while loading ",
								text,
								" ",
								ex
							}));
						}
					}
					else
					{
						SuperController.LogError("Could not load json " + text);
					}
					this.RestorePresetBinary();
				}
				else
				{
					SuperController.LogError("Not all preset parameters set. Cannot load");
				}
			}
			else
			{
				SuperController.LogError("Item type set to None. Cannot load");
			}
			return result;
		}

		// Token: 0x060064EE RID: 25838 RVA: 0x00177AF4 File Offset: 0x00175EF4
		protected void LoadDefaultsPreProcessStorables(List<PresetManager.Storable> sts, JSONArray outputStorables)
		{
			if (sts != null)
			{
				Dictionary<string, JSONClass> dictionary = new Dictionary<string, JSONClass>();
				IEnumerator enumerator = outputStorables.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						JSONClass jsonclass = (JSONClass)obj;
						if (jsonclass["id"] != null)
						{
							dictionary.Add(jsonclass["id"], jsonclass);
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
				foreach (PresetManager.Storable storable in sts)
				{
					JSONStorable storable2 = storable.storable;
					if ((this.ignoreExclude || !storable2.exclude) && storable.specificKey != null && storable.specificKey != string.Empty)
					{
						JSONClass jsonclass2;
						if (!dictionary.TryGetValue(storable2.storeId, out jsonclass2))
						{
							jsonclass2 = new JSONClass();
							jsonclass2["id"] = storable2.storeId;
							outputStorables.Add(jsonclass2);
							dictionary.Add(storable2.storeId, jsonclass2);
						}
						if (storable.isSpecificKeyAnObject)
						{
							JSONClass value = new JSONClass();
							jsonclass2[storable.specificKey] = value;
						}
						else
						{
							jsonclass2[storable.specificKey] = string.Empty;
						}
					}
				}
			}
		}

		// Token: 0x060064EF RID: 25839 RVA: 0x00177C94 File Offset: 0x00176094
		protected void LoadDefaultsPreInternal()
		{
			JSONArray jsonarray = new JSONArray();
			this.filteredJSON["storables"] = jsonarray;
			if (this.optionalFirst)
			{
				if (this.includeOptional && this.setOptionalToDefaultOnRestore)
				{
					this.LoadDefaultsPreProcessStorables(this.optionalStorables, jsonarray);
				}
				if (this.includeOptional2 && this.setOptional2ToDefaultOnRestore)
				{
					this.LoadDefaultsPreProcessStorables(this.optionalStorables2, jsonarray);
				}
				if (this.includeOptional3 && this.setOptional3ToDefaultOnRestore)
				{
					this.LoadDefaultsPreProcessStorables(this.optionalStorables3, jsonarray);
				}
			}
			this.LoadDefaultsPreProcessStorables(this.storables, jsonarray);
			if (!this.optionalFirst)
			{
				if (this.includeOptional && this.setOptionalToDefaultOnRestore)
				{
					this.LoadDefaultsPreProcessStorables(this.optionalStorables, jsonarray);
				}
				if (this.includeOptional2 && this.setOptional2ToDefaultOnRestore)
				{
					this.LoadDefaultsPreProcessStorables(this.optionalStorables2, jsonarray);
				}
				if (this.includeOptional3 && this.setOptional3ToDefaultOnRestore)
				{
					this.LoadDefaultsPreProcessStorables(this.optionalStorables3, jsonarray);
				}
			}
		}

		// Token: 0x060064F0 RID: 25840 RVA: 0x00177DB0 File Offset: 0x001761B0
		public bool LoadDefaultsPre()
		{
			bool result = false;
			if (this.itemType != PresetManager.ItemType.None)
			{
				string storeFolderPath = this.GetStoreFolderPath(false);
				if (storeFolderPath != null && storeFolderPath != string.Empty && this.storeName != null && this.storeName != string.Empty)
				{
					this.lastLoadedJSON = new JSONClass();
					if (this.conditionalFlagsToStore != null && this.setConditionalFlagsOnLoadDefaults)
					{
						foreach (string aKey in this.conditionalFlagsToStore)
						{
							this.lastLoadedJSON[aKey].AsBool = true;
						}
					}
					this.filteredJSON = this.lastLoadedJSON;
					this.isMergeRestore = false;
					this.setUnlistedParamsToDefault = true;
					this.LoadDefaultsPreInternal();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060064F1 RID: 25841 RVA: 0x00177E82 File Offset: 0x00176282
		public bool LoadPresetPost()
		{
			if (this.filteredJSON != null)
			{
				this.RestoreStorables(this.filteredJSON);
				return true;
			}
			return false;
		}

		// Token: 0x060064F2 RID: 25842 RVA: 0x00177EA4 File Offset: 0x001762A4
		protected void RefreshSpecificStorables(PresetManager.SpecificStorable[] spStorables, List<PresetManager.Storable> outputStorables)
		{
			foreach (PresetManager.SpecificStorable specificStorable in spStorables)
			{
				if (specificStorable.specificStorableBucket != null)
				{
					JSONStorable[] array;
					if (specificStorable.includeChildren)
					{
						array = specificStorable.specificStorableBucket.GetComponentsInChildren<JSONStorable>(true);
					}
					else
					{
						array = specificStorable.specificStorableBucket.GetComponents<JSONStorable>();
					}
					foreach (JSONStorable jsonstorable in array)
					{
						if (!(jsonstorable is PresetManagerControl))
						{
							if (specificStorable.storeId == string.Empty || specificStorable.storeId == jsonstorable.storeId)
							{
								PresetManager.Storable storable = new PresetManager.Storable();
								storable.storable = jsonstorable;
								if (specificStorable.specificKey != null && specificStorable.specificKey != string.Empty && !this.specificKeyStorables.ContainsKey(specificStorable.storeId))
								{
									this.specificKeyStorables.Add(specificStorable.storeId, true);
								}
								storable.specificKey = specificStorable.specificKey;
								storable.isSpecificKeyAnObject = specificStorable.isSpecificKeyAnObject;
								outputStorables.Add(storable);
								if (!this.regularStorables.ContainsKey(jsonstorable))
								{
									this.regularStorables.Add(jsonstorable, true);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060064F3 RID: 25843 RVA: 0x00178000 File Offset: 0x00176400
		public void RefreshStorables()
		{
			if (Application.isPlaying)
			{
				this.storables = new List<PresetManager.Storable>();
				this.optionalStorables = new List<PresetManager.Storable>();
				this.optionalStorables2 = new List<PresetManager.Storable>();
				this.optionalStorables3 = new List<PresetManager.Storable>();
				this.regularStorables = new Dictionary<JSONStorable, bool>();
				this.specificKeyStorables = new Dictionary<string, bool>();
				if (this.specificStorables != null && this.specificStorables.Length > 0)
				{
					this.RefreshSpecificStorables(this.specificStorables, this.storables);
				}
				if (this.optionalSpecificStorables != null && this.optionalSpecificStorables.Length > 0)
				{
					this.RefreshSpecificStorables(this.optionalSpecificStorables, this.optionalStorables);
				}
				if (this.optionalSpecificStorables2 != null && this.optionalSpecificStorables2.Length > 0)
				{
					this.RefreshSpecificStorables(this.optionalSpecificStorables2, this.optionalStorables2);
				}
				if (this.optionalSpecificStorables3 != null && this.optionalSpecificStorables3.Length > 0)
				{
					this.RefreshSpecificStorables(this.optionalSpecificStorables3, this.optionalStorables3);
				}
				if (this.useTransformAndChildren)
				{
					JSONStorable[] componentsInChildren = base.GetComponentsInChildren<JSONStorable>(true);
					foreach (JSONStorable jsonstorable in componentsInChildren)
					{
						if (!this.regularStorables.ContainsKey(jsonstorable) && !(jsonstorable is PresetManagerControl))
						{
							PresetManager.Storable storable = new PresetManager.Storable();
							storable.storable = jsonstorable;
							this.storables.Add(storable);
							this.regularStorables.Add(jsonstorable, true);
						}
					}
				}
			}
		}

		// Token: 0x060064F4 RID: 25844 RVA: 0x00178178 File Offset: 0x00176578
		protected void RefreshDynamicStorables()
		{
			if (this.dynamicStorables != null)
			{
				this.ClearLockStorablesInList(this.dynamicStorables);
			}
			this.dynamicStorables = new List<PresetManager.Storable>();
			foreach (Transform transform in this.dynamicStorablesBuckets)
			{
				JSONStorable[] componentsInChildren = transform.GetComponentsInChildren<JSONStorable>(true);
				foreach (JSONStorable jsonstorable in componentsInChildren)
				{
					if (!this.regularStorables.ContainsKey(jsonstorable))
					{
						PresetManager.Storable storable = new PresetManager.Storable();
						storable.storable = jsonstorable;
						this.dynamicStorables.Add(storable);
					}
				}
			}
			this.LockStorablesInList(this.dynamicStorables);
		}

		// Token: 0x060064F5 RID: 25845 RVA: 0x0017822C File Offset: 0x0017662C
		public void SyncMaterialOptions()
		{
			MaterialOptions[] array;
			if (this.includeChildrenMaterialOptions)
			{
				array = base.GetComponentsInChildren<MaterialOptions>(true);
			}
			else
			{
				array = base.GetComponents<MaterialOptions>();
			}
			string storeFolderPath = this.GetStoreFolderPath(true);
			foreach (MaterialOptions materialOptions in array)
			{
				materialOptions.SetCustomTextureFolder(storeFolderPath);
			}
		}

		// Token: 0x060064F6 RID: 25846 RVA: 0x00178286 File Offset: 0x00176686
		protected virtual void Awake()
		{
			this.RefreshStorables();
			this.RefreshDynamicStorables();
			this.SyncMaterialOptions();
		}

		// Token: 0x0400545D RID: 21597
		public UnityEvent postLoadEvent;

		// Token: 0x0400545E RID: 21598
		public UnityEvent postLoadOptimizeEvent;

		// Token: 0x0400545F RID: 21599
		public string[] conditionalFlagsToStore;

		// Token: 0x04005460 RID: 21600
		public bool setConditionalFlagsOnLoadDefaults = true;

		// Token: 0x04005461 RID: 21601
		public bool storeConditionalFlagsAlways = true;

		// Token: 0x04005462 RID: 21602
		public bool storeConditionalFlagsWhenStoreOptional;

		// Token: 0x04005463 RID: 21603
		public bool storeConditionalFlagsWhenStoreOptional2;

		// Token: 0x04005464 RID: 21604
		public bool storeConditionalFlagsWhenStoreOptional3;

		// Token: 0x04005465 RID: 21605
		public PresetManager.ConditionalLoadEvent[] conditionalLoadEvents;

		// Token: 0x04005466 RID: 21606
		public bool onlyCallPostLoadEventIfIncludeOptional;

		// Token: 0x04005467 RID: 21607
		public PresetManager.ItemType itemType;

		// Token: 0x04005468 RID: 21608
		public string storedCreatorName;

		// Token: 0x04005469 RID: 21609
		public string creatorName;

		// Token: 0x0400546A RID: 21610
		public string storeFolderName;

		// Token: 0x0400546B RID: 21611
		public string storeName;

		// Token: 0x0400546C RID: 21612
		protected string _presetName;

		// Token: 0x0400546D RID: 21613
		protected string presetPackageName = string.Empty;

		// Token: 0x0400546E RID: 21614
		protected string presetPackagePath = string.Empty;

		// Token: 0x0400546F RID: 21615
		protected string presetSubPath;

		// Token: 0x04005470 RID: 21616
		protected string presetSubName;

		// Token: 0x04005471 RID: 21617
		public string package = string.Empty;

		// Token: 0x04005472 RID: 21618
		public string customPath = string.Empty;

		// Token: 0x04005473 RID: 21619
		public bool storeOptionalStorables;

		// Token: 0x04005474 RID: 21620
		public bool storeOptionalStorables2;

		// Token: 0x04005475 RID: 21621
		public bool storeOptionalStorables3;

		// Token: 0x04005476 RID: 21622
		public bool storePresetBinary;

		// Token: 0x04005477 RID: 21623
		public bool setOptionalToDefaultOnRestore;

		// Token: 0x04005478 RID: 21624
		public bool setOptional2ToDefaultOnRestore;

		// Token: 0x04005479 RID: 21625
		public bool setOptional3ToDefaultOnRestore;

		// Token: 0x0400547A RID: 21626
		public bool useTransformAndChildren = true;

		// Token: 0x0400547B RID: 21627
		public bool includeOptional = true;

		// Token: 0x0400547C RID: 21628
		public bool includeOptional2 = true;

		// Token: 0x0400547D RID: 21629
		public bool includeOptional3 = true;

		// Token: 0x0400547E RID: 21630
		public bool includePhysical = true;

		// Token: 0x0400547F RID: 21631
		public bool includeAppearance = true;

		// Token: 0x04005480 RID: 21632
		public bool optionalFirst;

		// Token: 0x04005481 RID: 21633
		public PresetManager.SpecificStorable[] optionalSpecificStorables;

		// Token: 0x04005482 RID: 21634
		public PresetManager.SpecificStorable[] optionalSpecificStorables2;

		// Token: 0x04005483 RID: 21635
		public PresetManager.SpecificStorable[] optionalSpecificStorables3;

		// Token: 0x04005484 RID: 21636
		public PresetManager.SpecificStorable[] specificStorables;

		// Token: 0x04005485 RID: 21637
		public Transform[] dynamicStorablesBuckets;

		// Token: 0x04005486 RID: 21638
		public bool ignoreExclude;

		// Token: 0x04005487 RID: 21639
		protected string storeRoot = "Custom/";

		// Token: 0x04005488 RID: 21640
		protected List<PresetManager.Storable> storables;

		// Token: 0x04005489 RID: 21641
		protected List<PresetManager.Storable> optionalStorables;

		// Token: 0x0400548A RID: 21642
		protected List<PresetManager.Storable> optionalStorables2;

		// Token: 0x0400548B RID: 21643
		protected List<PresetManager.Storable> optionalStorables3;

		// Token: 0x0400548C RID: 21644
		protected List<PresetManager.Storable> dynamicStorables;

		// Token: 0x0400548D RID: 21645
		protected Dictionary<JSONStorable, bool> regularStorables;

		// Token: 0x0400548E RID: 21646
		protected Dictionary<string, bool> specificKeyStorables;

		// Token: 0x0400548F RID: 21647
		protected bool _tempUnlockParams;

		// Token: 0x04005490 RID: 21648
		protected bool _paramsLocked;

		// Token: 0x04005491 RID: 21649
		public JSONClass lastLoadedJSON;

		// Token: 0x04005492 RID: 21650
		public JSONClass filteredJSON;

		// Token: 0x04005493 RID: 21651
		protected bool isMergeRestore;

		// Token: 0x04005494 RID: 21652
		public bool neverSetUnlistedParamsToDefault;

		// Token: 0x04005495 RID: 21653
		protected bool setUnlistedParamsToDefault = true;

		// Token: 0x04005496 RID: 21654
		protected bool setUnlistedDynamicStorableParamsToDefault = true;

		// Token: 0x04005497 RID: 21655
		public bool includeChildrenMaterialOptions;

		// Token: 0x02000CF5 RID: 3317
		[Serializable]
		public class SpecificStorable
		{
			// Token: 0x060064F7 RID: 25847 RVA: 0x0017829A File Offset: 0x0017669A
			public SpecificStorable()
			{
			}

			// Token: 0x04005498 RID: 21656
			public Transform specificStorableBucket;

			// Token: 0x04005499 RID: 21657
			public string storeId;

			// Token: 0x0400549A RID: 21658
			public string specificKey;

			// Token: 0x0400549B RID: 21659
			public bool isSpecificKeyAnObject;

			// Token: 0x0400549C RID: 21660
			public bool includeChildren;
		}

		// Token: 0x02000CF6 RID: 3318
		public class Storable
		{
			// Token: 0x060064F8 RID: 25848 RVA: 0x001782A2 File Offset: 0x001766A2
			public Storable()
			{
			}

			// Token: 0x0400549D RID: 21661
			public JSONStorable storable;

			// Token: 0x0400549E RID: 21662
			public string specificKey;

			// Token: 0x0400549F RID: 21663
			public bool isSpecificKeyAnObject;
		}

		// Token: 0x02000CF7 RID: 3319
		public enum ItemType
		{
			// Token: 0x040054A1 RID: 21665
			None,
			// Token: 0x040054A2 RID: 21666
			Custom,
			// Token: 0x040054A3 RID: 21667
			ClothingFemale,
			// Token: 0x040054A4 RID: 21668
			ClothingMale,
			// Token: 0x040054A5 RID: 21669
			ClothingNeutral,
			// Token: 0x040054A6 RID: 21670
			Atom,
			// Token: 0x040054A7 RID: 21671
			HairFemale,
			// Token: 0x040054A8 RID: 21672
			HairMale,
			// Token: 0x040054A9 RID: 21673
			HairNeutral
		}

		// Token: 0x02000CF8 RID: 3320
		[Serializable]
		public class ConditionalLoadEvent
		{
			// Token: 0x060064F9 RID: 25849 RVA: 0x001782AA File Offset: 0x001766AA
			public ConditionalLoadEvent()
			{
			}

			// Token: 0x040054AA RID: 21674
			public string flag;

			// Token: 0x040054AB RID: 21675
			public UnityEvent ifEvent;

			// Token: 0x040054AC RID: 21676
			public UnityEvent elseEvent;
		}
	}
}
