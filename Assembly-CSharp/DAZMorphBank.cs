using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using MeshVR;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000AF4 RID: 2804
[ExecuteInEditMode]
public class DAZMorphBank : MonoBehaviour
{
	// Token: 0x06004B4D RID: 19277 RVA: 0x001A109E File Offset: 0x0019F49E
	public DAZMorphBank()
	{
	}

	// Token: 0x17000AC8 RID: 2760
	// (get) Token: 0x06004B4E RID: 19278 RVA: 0x001A10D5 File Offset: 0x0019F4D5
	// (set) Token: 0x06004B4F RID: 19279 RVA: 0x001A10DD File Offset: 0x0019F4DD
	public DAZMesh connectedMesh
	{
		get
		{
			return this._connectedMesh;
		}
		set
		{
			if (this._connectedMesh != value)
			{
				this._connectedMesh = value;
				this.ResetMorphs();
			}
		}
	}

	// Token: 0x06004B50 RID: 19280 RVA: 0x001A10FD File Offset: 0x0019F4FD
	public void NotifyMorphFavoriteChanged(DAZMorph morph)
	{
		if (this.onMorphFavoriteChangedHandlers != null)
		{
			this.onMorphFavoriteChangedHandlers(morph);
		}
	}

	// Token: 0x06004B51 RID: 19281 RVA: 0x001A1118 File Offset: 0x0019F518
	protected void BuildMorphSubBanksByRegion()
	{
		if (this._morphSubBanksByRegion == null)
		{
			this._morphSubBanksByRegion = new Dictionary<string, DAZMorphSubBank>();
		}
		else
		{
			this._morphSubBanksByRegion.Clear();
		}
		DAZMorphSubBank[] componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
		foreach (DAZMorphSubBank dazmorphSubBank in componentsInChildren)
		{
			this._morphSubBanksByRegion.Add(dazmorphSubBank.name, dazmorphSubBank);
		}
	}

	// Token: 0x06004B52 RID: 19282 RVA: 0x001A1180 File Offset: 0x0019F580
	protected void BuildMorphsList()
	{
		if (this._morphs == null)
		{
			this._morphs = new List<DAZMorph>();
		}
		else
		{
			this._morphs.Clear();
		}
		if (this._unactivatedMorphs == null)
		{
			this._unactivatedMorphs = new List<DAZMorph>();
		}
		else
		{
			this._unactivatedMorphs.Clear();
		}
		foreach (DAZMorphSubBank dazmorphSubBank in this._morphSubBanksByRegion.Values)
		{
			foreach (DAZMorph dazmorph in dazmorphSubBank.combinedMorphs)
			{
				if (dazmorph.isDemandLoaded && !dazmorph.isDemandActivated)
				{
					this._unactivatedMorphs.Add(dazmorph);
				}
				else
				{
					this._morphs.Add(dazmorph);
				}
			}
		}
	}

	// Token: 0x17000AC9 RID: 2761
	// (get) Token: 0x06004B53 RID: 19283 RVA: 0x001A12A0 File Offset: 0x0019F6A0
	public List<DAZMorph> morphs
	{
		get
		{
			return this._morphs;
		}
	}

	// Token: 0x17000ACA RID: 2762
	// (get) Token: 0x06004B54 RID: 19284 RVA: 0x001A12A8 File Offset: 0x0019F6A8
	public int numMorphs
	{
		get
		{
			if (this._morphs == null)
			{
				return 0;
			}
			return this._morphs.Count;
		}
	}

	// Token: 0x06004B55 RID: 19285 RVA: 0x001A12C4 File Offset: 0x0019F6C4
	protected void BuildMorphsByNameAndUid()
	{
		if (this._builtInMorphsByName == null)
		{
			this._builtInMorphsByName = new Dictionary<string, DAZMorph>();
		}
		else
		{
			this._builtInMorphsByName.Clear();
		}
		if (this._builtInMorphsByUid == null)
		{
			this._builtInMorphsByUid = new Dictionary<string, DAZMorph>();
		}
		else
		{
			this._builtInMorphsByUid.Clear();
		}
		if (this._morphsByName == null)
		{
			this._morphsByName = new Dictionary<string, DAZMorph>();
		}
		else
		{
			this._morphsByName.Clear();
		}
		if (this._morphsByUid == null)
		{
			this._morphsByUid = new Dictionary<string, DAZMorph>();
		}
		else
		{
			this._morphsByUid.Clear();
		}
		if (this._morphsByDisplayName == null)
		{
			this._morphsByDisplayName = new Dictionary<string, DAZMorph>();
		}
		else
		{
			this._morphsByDisplayName.Clear();
		}
		foreach (DAZMorph dazmorph in this._morphs)
		{
			if (!dazmorph.disable)
			{
				DAZMorph dazmorph2;
				if (!this._morphsByUid.ContainsKey(dazmorph.uid))
				{
					this._morphsByUid.Add(dazmorph.uid, dazmorph);
				}
				else if (this._morphsByUid.TryGetValue(dazmorph.uid, out dazmorph2))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Duplicate morph by uid ",
						dazmorph.uid,
						" Region 1: ",
						dazmorph2.region,
						" Region 2: ",
						dazmorph.region
					}));
				}
				if (!dazmorph.isRuntime)
				{
					this._builtInMorphsByName.Add(dazmorph.morphName, dazmorph);
					this._builtInMorphsByUid.Add(dazmorph.uid, dazmorph);
				}
				if (!this._morphsByName.ContainsKey(dazmorph.morphName))
				{
					this._morphsByName.Add(dazmorph.morphName, dazmorph);
				}
				if (dazmorph.resolvedDisplayName != null)
				{
					if (!this._morphsByDisplayName.ContainsKey(dazmorph.resolvedDisplayName))
					{
						this._morphsByDisplayName.Add(dazmorph.resolvedDisplayName, dazmorph);
					}
				}
				else
				{
					Debug.LogError("Morph " + dazmorph.morphName + " has null display name");
				}
			}
		}
		foreach (DAZMorph dazmorph3 in this._unactivatedMorphs)
		{
			if (!dazmorph3.disable)
			{
				DAZMorph dazmorph4;
				if (!this._morphsByUid.ContainsKey(dazmorph3.uid))
				{
					this._morphsByUid.Add(dazmorph3.uid, dazmorph3);
				}
				else if (this._morphsByUid.TryGetValue(dazmorph3.uid, out dazmorph4))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Duplicate morph by uid ",
						dazmorph3.uid,
						" Region 1: ",
						dazmorph4.region,
						" Region 2: ",
						dazmorph3.region
					}));
				}
				if (!this._morphsByName.ContainsKey(dazmorph3.morphName))
				{
					this._morphsByName.Add(dazmorph3.morphName, dazmorph3);
				}
				if (dazmorph3.resolvedDisplayName != null)
				{
					if (!this._morphsByDisplayName.ContainsKey(dazmorph3.resolvedDisplayName))
					{
						this._morphsByDisplayName.Add(dazmorph3.resolvedDisplayName, dazmorph3);
					}
				}
				else
				{
					Debug.LogError("Morph " + dazmorph3.morphName + " has null display name");
				}
			}
		}
	}

	// Token: 0x06004B56 RID: 19286 RVA: 0x001A1688 File Offset: 0x0019FA88
	protected void DemandActivateMorph(DAZMorph dm)
	{
		if (dm.isDemandLoaded && !dm.isDemandActivated)
		{
			dm.morphBank = this;
			dm.Init();
			dm.isDemandActivated = true;
			this.demandActivatedMorphsDirty = true;
		}
	}

	// Token: 0x06004B57 RID: 19287 RVA: 0x001A16BB File Offset: 0x0019FABB
	public bool CleanDemandActivatedMorphs()
	{
		if (this.demandActivatedMorphsDirty)
		{
			this.BuildMorphsList();
			this.demandActivatedMorphsDirty = false;
			return true;
		}
		return false;
	}

	// Token: 0x06004B58 RID: 19288 RVA: 0x001A16D8 File Offset: 0x0019FAD8
	public bool UnloadDemandActivatedMorphs()
	{
		bool result = false;
		for (int i = 0; i < this._morphs.Count; i++)
		{
			DAZMorph dazmorph = this._morphs[i];
			if (dazmorph.isDemandActivated && dazmorph.isDemandLoaded && !dazmorph.active)
			{
				Debug.Log("Unload demand activated morph " + dazmorph.uid);
				if (dazmorph.deltasLoaded)
				{
					dazmorph.UnloadDeltas();
				}
				dazmorph.isDemandActivated = false;
				result = true;
			}
		}
		this.demandActivatedMorphsDirty = result;
		return result;
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x001A1768 File Offset: 0x0019FB68
	public int GetRuntimeMorphDeltasLoadedCount()
	{
		int num = 0;
		for (int i = 0; i < this._morphs.Count; i++)
		{
			DAZMorph dazmorph = this._morphs[i];
			if (dazmorph.isRuntime && dazmorph.deltasLoaded)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06004B5A RID: 19290 RVA: 0x001A17BC File Offset: 0x0019FBBC
	public void UnloadRuntimeMorphDeltas()
	{
		for (int i = 0; i < this._morphs.Count; i++)
		{
			DAZMorph dazmorph = this._morphs[i];
			if (dazmorph.isRuntime && dazmorph.deltasLoaded && !dazmorph.active)
			{
				Debug.Log("Unload morph deltas for " + dazmorph.uid);
				dazmorph.UnloadDeltas();
			}
		}
	}

	// Token: 0x06004B5B RID: 19291 RVA: 0x001A1830 File Offset: 0x0019FC30
	public DAZMorph GetMorph(string morphName)
	{
		this.Init();
		DAZMorph dazmorph;
		if (this._morphsByName.TryGetValue(morphName, out dazmorph))
		{
			this.DemandActivateMorph(dazmorph);
			return dazmorph;
		}
		return null;
	}

	// Token: 0x06004B5C RID: 19292 RVA: 0x001A1860 File Offset: 0x0019FC60
	public DAZMorph GetBuiltInMorph(string morphName)
	{
		this.Init();
		DAZMorph result;
		if (this._builtInMorphsByName.TryGetValue(morphName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004B5D RID: 19293 RVA: 0x001A188C File Offset: 0x0019FC8C
	public DAZMorph GetBuiltInMorphByUid(string morphName)
	{
		this.Init();
		DAZMorph result;
		if (this._builtInMorphsByUid.TryGetValue(morphName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004B5E RID: 19294 RVA: 0x001A18B8 File Offset: 0x0019FCB8
	public DAZMorph GetMorphByUid(string uid)
	{
		this.Init();
		DAZMorph dazmorph;
		if (this._morphsByUid.TryGetValue(uid, out dazmorph))
		{
			this.DemandActivateMorph(dazmorph);
			return dazmorph;
		}
		return null;
	}

	// Token: 0x06004B5F RID: 19295 RVA: 0x001A18E8 File Offset: 0x0019FCE8
	public DAZMorph GetMorphByDisplayName(string morphDisplayName)
	{
		this.Init();
		DAZMorph dazmorph;
		if (this._morphsByDisplayName.TryGetValue(morphDisplayName, out dazmorph))
		{
			this.DemandActivateMorph(dazmorph);
			return dazmorph;
		}
		return null;
	}

	// Token: 0x06004B60 RID: 19296 RVA: 0x001A1918 File Offset: 0x0019FD18
	protected void BuildMorphToRegionName()
	{
		if (this._morphToRegionName == null)
		{
			this._morphToRegionName = new Dictionary<string, string>();
		}
		else
		{
			this._morphToRegionName.Clear();
		}
		foreach (DAZMorphSubBank dazmorphSubBank in this._morphSubBanksByRegion.Values)
		{
			foreach (DAZMorph dazmorph in dazmorphSubBank.combinedMorphs)
			{
				string value;
				if (dazmorph.overrideRegion != null && dazmorph.overrideRegion != string.Empty)
				{
					value = dazmorph.overrideRegion;
				}
				else if (dazmorphSubBank.useOverrideRegionName)
				{
					value = dazmorphSubBank.overrideRegionName;
				}
				else
				{
					value = dazmorph.region;
				}
				if (!this._morphToRegionName.ContainsKey(dazmorph.morphName))
				{
					this._morphToRegionName.Add(dazmorph.morphName, value);
				}
			}
		}
	}

	// Token: 0x06004B61 RID: 19297 RVA: 0x001A1A54 File Offset: 0x0019FE54
	public string GetMorphRegionName(string morphName)
	{
		this.Init();
		string result;
		if (this._morphToRegionName.TryGetValue(morphName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004B62 RID: 19298 RVA: 0x001A1A7D File Offset: 0x0019FE7D
	public bool IsBadMorph(string name)
	{
		return name == null || (this.badMorphNames != null && this.badMorphNames.Contains(name));
	}

	// Token: 0x06004B63 RID: 19299 RVA: 0x001A1AA8 File Offset: 0x0019FEA8
	public bool AddMorphUsingSubBanks(DAZMorph dm)
	{
		if (this.badMorphNames == null)
		{
			this.badMorphNames = new HashSet<string>();
		}
		if (dm.numDeltas == 0 && dm.formulas.Length == 0)
		{
			if (dm.resolvedDisplayName != null && dm.resolvedDisplayName != string.Empty)
			{
				this.badMorphNames.Add(dm.resolvedDisplayName);
			}
			return false;
		}
		if (dm.min == dm.max)
		{
			if (dm.resolvedDisplayName != null && dm.resolvedDisplayName != string.Empty)
			{
				this.badMorphNames.Add(dm.resolvedDisplayName);
			}
			return false;
		}
		if (dm.resolvedDisplayName == null || dm.resolvedDisplayName == string.Empty)
		{
			return false;
		}
		if (this._morphSubBanksByRegion == null)
		{
			this.BuildMorphSubBanksByRegion();
		}
		if (dm.region == null || dm.region == string.Empty)
		{
			dm.region = "NoRegion";
		}
		DAZMorphSubBank dazmorphSubBank;
		if (!this._morphSubBanksByRegion.TryGetValue(dm.region, out dazmorphSubBank))
		{
			dazmorphSubBank = new GameObject(dm.region)
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<DAZMorphSubBank>();
			this._morphSubBanksByRegion.Add(dm.region, dazmorphSubBank);
		}
		if (dazmorphSubBank != null)
		{
			dazmorphSubBank.AddMorph(dm);
			if (this.wasInit && Application.isPlaying)
			{
				dm.Init();
			}
		}
		return true;
	}

	// Token: 0x06004B64 RID: 19300 RVA: 0x001A1C38 File Offset: 0x001A0038
	protected bool RuntimeImportMorph(FileEntry morphInfFile, bool isTransient = false, bool isDemandLoaded = false, bool forceReload = false)
	{
		bool result = false;
		string path = morphInfFile.Path;
		string text = path.Replace(".vmi", ".vmb");
		FileEntry fileEntry = FileManager.GetFileEntry(text, false);
		if (fileEntry != null)
		{
			try
			{
				JSONNode jsonnode = null;
				if (morphInfFile is VarFileEntry)
				{
					VarFileEntry varFileEntry = morphInfFile as VarFileEntry;
					jsonnode = varFileEntry.Package.GetJSONCache(varFileEntry.InternalSlashPath);
				}
				if (jsonnode == null)
				{
					string aJSON = FileManager.ReadAllText(morphInfFile);
					jsonnode = JSON.Parse(aJSON);
				}
				if (jsonnode != null)
				{
					DAZMorph dazmorph = new DAZMorph();
					dazmorph.morphBank = this;
					dazmorph.LoadMetaFromJSON(jsonnode);
					dazmorph.visible = true;
					dazmorph.isRuntime = true;
					dazmorph.isDemandLoaded = isDemandLoaded;
					if (morphInfFile is VarFileEntry)
					{
						VarFileEntry varFileEntry2 = morphInfFile as VarFileEntry;
						dazmorph.isInPackage = true;
						dazmorph.packageUid = varFileEntry2.Package.Uid;
						dazmorph.packageLicense = varFileEntry2.Package.LicenseType;
						dazmorph.isLatestVersion = varFileEntry2.Package.isNewestEnabledVersion;
						dazmorph.version = "v" + varFileEntry2.Package.Version;
					}
					else
					{
						dazmorph.isInPackage = false;
						dazmorph.packageUid = null;
						dazmorph.packageLicense = null;
						dazmorph.isLatestVersion = true;
						dazmorph.version = null;
					}
					dazmorph.metaLoadPath = morphInfFile.Path;
					dazmorph.isTransient = isTransient;
					dazmorph.deltasLoadPath = fileEntry.Path;
					dazmorph.uid = morphInfFile.Uid;
					if (isTransient)
					{
						if (this.GetMorphByDisplayName(dazmorph.resolvedDisplayName) == null)
						{
							result = true;
							this.AddMorphUsingSubBanks(dazmorph);
						}
					}
					else
					{
						DAZMorph morphByUid = this.GetMorphByUid(dazmorph.uid);
						if (morphByUid == null)
						{
							result = true;
							this.AddMorphUsingSubBanks(dazmorph);
						}
						else if (forceReload && morphByUid.isRuntime && !morphByUid.isInPackage)
						{
							Debug.Log("Force reload morph " + morphByUid.uid);
							morphByUid.UnloadDeltas();
							morphByUid.CopyParameters(dazmorph, false);
							morphByUid.formulas = dazmorph.formulas;
							morphByUid.appliedValue = 0f;
							morphByUid.active = false;
							result = true;
						}
					}
				}
				else if (SuperController.singleton != null)
				{
					SuperController.singleton.Error("Parse error while loading morph " + path, true, true);
				}
				else
				{
					Debug.LogError("Parse error while loading morph " + path);
				}
			}
			catch (Exception ex)
			{
				SuperController.LogError(string.Concat(new object[]
				{
					"Exception during read of morph binary file ",
					text,
					" ",
					ex
				}));
			}
		}
		else if (SuperController.singleton != null)
		{
			SuperController.singleton.Error("Missing morph deltas file " + text + ". Cannot import", true, true);
		}
		else
		{
			Debug.LogError("Missing morph deltas file " + text + ". Cannot import");
		}
		return result;
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x001A1F60 File Offset: 0x001A0360
	protected bool RuntimeImportFromDir(string path, bool isTransient = false, bool isDemandLoaded = false, bool forceReload = false)
	{
		DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(path, false);
		return this.RuntimeImportFromDir(directoryEntry, isTransient, isDemandLoaded, forceReload);
	}

	// Token: 0x06004B66 RID: 19302 RVA: 0x001A1F80 File Offset: 0x001A0380
	protected bool RuntimeImportFromDir(DirectoryEntry de, bool isTransient = false, bool isDemandLoaded = false, bool forceReload = false)
	{
		bool result = false;
		if (de != null)
		{
			List<DirectoryEntry> subDirectories = de.SubDirectories;
			foreach (DirectoryEntry de2 in subDirectories)
			{
				if (this.RuntimeImportFromDir(de2, isTransient, isDemandLoaded, forceReload))
				{
					result = true;
				}
			}
			bool flag = true;
			if (de is SystemDirectoryEntry)
			{
				List<FileEntry> files = de.GetFiles("*.dsf");
				foreach (FileEntry fileEntry in files)
				{
					string path = fileEntry.Path;
					string text = path.Replace(".dsf", string.Empty);
					string text2 = text + ".vmi";
					string path2 = text + ".vmb";
					if (!FileManager.FileExists(text2, true, true) || !FileManager.FileExists(path2, true, true))
					{
						Debug.Log("Compiling custom morphs from " + fileEntry);
						JSONNode jsonnode = DAZImport.ReadJSON(path);
						if (jsonnode != null)
						{
							JSONNode jsonnode2 = jsonnode["modifier_library"];
							if (jsonnode2 != null)
							{
								int num = 0;
								IEnumerator enumerator3 = jsonnode2.AsArray.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										object obj = enumerator3.Current;
										JSONNode mn = (JSONNode)obj;
										num++;
										DAZMorph dazmorph = new DAZMorph();
										dazmorph.Import(mn);
										string text3;
										if (num == 1)
										{
											text3 = text2;
										}
										else
										{
											text3 = string.Concat(new object[]
											{
												text,
												"_",
												num,
												".vmi"
											});
										}
										string path3 = text3.Replace(".vmi", ".vmb");
										bool flag2 = false;
										try
										{
											using (StreamWriter streamWriter = FileManager.OpenStreamWriter(text3))
											{
												JSONClass metaJSON = dazmorph.GetMetaJSON();
												if (metaJSON != null)
												{
													flag2 = true;
													StringBuilder stringBuilder = new StringBuilder(10000);
													metaJSON.ToString(string.Empty, stringBuilder);
													streamWriter.Write(stringBuilder.ToString());
													dazmorph.SaveDeltasToBinaryFile(path3);
												}
											}
											if (!flag2)
											{
												FileManager.DeleteFile(text3);
											}
										}
										catch (Exception ex)
										{
											Debug.LogError(string.Concat(new object[]
											{
												"Error during compile of morph ",
												fileEntry,
												" ",
												ex
											}));
										}
									}
								}
								finally
								{
									IDisposable disposable;
									if ((disposable = (enumerator3 as IDisposable)) != null)
									{
										disposable.Dispose();
									}
								}
							}
						}
					}
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				List<FileEntry> files2 = de.GetFiles("*.vmi");
				foreach (FileEntry morphInfFile in files2)
				{
					if (this.RuntimeImportMorph(morphInfFile, isTransient, isDemandLoaded, forceReload))
					{
						result = true;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06004B67 RID: 19303 RVA: 0x001A2324 File Offset: 0x001A0724
	public bool LoadTransientMorphs(string dir)
	{
		DAZMorphSubBank[] componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
		bool flag = false;
		foreach (DAZMorphSubBank dazmorphSubBank in componentsInChildren)
		{
			if (dazmorphSubBank.ClearTransientMorphs())
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.RebuildAllLookups();
		}
		bool flag2 = false;
		if (this.autoImportFolder != null && this.autoImportFolder != string.Empty)
		{
			string path = dir + "/" + this.autoImportFolder;
			if (this.RuntimeImportFromDir(path, true, false, false))
			{
				componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
				foreach (DAZMorphSubBank dazmorphSubBank2 in componentsInChildren)
				{
					dazmorphSubBank2.CompleteTransientMorphAdd();
				}
				flag2 = true;
			}
		}
		if (this.autoImportFolderLegacy != null && this.autoImportFolderLegacy != string.Empty)
		{
			string path2 = dir + "/" + this.autoImportFolderLegacy;
			if (this.RuntimeImportFromDir(path2, true, false, false))
			{
				componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
				foreach (DAZMorphSubBank dazmorphSubBank3 in componentsInChildren)
				{
					dazmorphSubBank3.CompleteTransientMorphAdd();
				}
				flag2 = true;
			}
		}
		if (flag2)
		{
			this.RebuildAllLookups();
		}
		return flag || flag2;
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x001A2480 File Offset: 0x001A0880
	public void ClearPackageMorphs()
	{
		DAZMorphSubBank[] componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
		bool flag = false;
		foreach (DAZMorphSubBank dazmorphSubBank in componentsInChildren)
		{
			if (dazmorphSubBank.ClearPackageMorphs())
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.RebuildAllLookups();
		}
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x001A24D0 File Offset: 0x001A08D0
	public void ClearTransientMorphs()
	{
		DAZMorphSubBank[] componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
		bool flag = false;
		foreach (DAZMorphSubBank dazmorphSubBank in componentsInChildren)
		{
			if (dazmorphSubBank.ClearTransientMorphs())
			{
				flag = true;
			}
		}
		if (flag)
		{
			this.RebuildAllLookups();
		}
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x001A251E File Offset: 0x001A091E
	public void RebuildAllLookups()
	{
		this.BuildMorphSubBanksByRegion();
		this.BuildMorphsList();
		this.BuildMorphsByNameAndUid();
		this.BuildMorphToRegionName();
	}

	// Token: 0x06004B6B RID: 19307 RVA: 0x001A2538 File Offset: 0x001A0938
	protected void InitMorphs()
	{
		if (this._morphs != null)
		{
			for (int i = 0; i < this._morphs.Count; i++)
			{
				this._morphs[i].morphBank = this;
				this._morphs[i].Init();
			}
		}
	}

	// Token: 0x06004B6C RID: 19308 RVA: 0x001A2590 File Offset: 0x001A0990
	public void ResetMorphs()
	{
		this.Init();
		if (this.useThreadedMorphing && Application.isPlaying)
		{
			this.triggerThreadResetMorphs = true;
		}
		else
		{
			this.ApplyMorphsInternal(false);
			if (this.connectedMesh != null)
			{
				this.connectedMesh.ResetMorphedVertices();
			}
			if (this._morphs != null)
			{
				for (int i = 0; i < this._morphs.Count; i++)
				{
					this._morphs[i].appliedValue = 0f;
					this._morphs[i].active = false;
					if (this._morphs[i].isDriven)
					{
						this._morphs[i].morphValue = 0f;
						this._morphs[i].SetDriven(false, string.Empty, true);
					}
				}
			}
			this.ZeroAllBoneMorphs();
		}
	}

	// Token: 0x06004B6D RID: 19309 RVA: 0x001A2684 File Offset: 0x001A0A84
	public void ResetMorphsFast(bool resetBones = true)
	{
		if (this._morphs != null)
		{
			for (int i = 0; i < this._morphs.Count; i++)
			{
				DAZMorph dazmorph = this._morphs[i];
				if (dazmorph.appliedValue != 0f)
				{
					dazmorph.appliedValue = 0f;
					dazmorph.active = false;
				}
				if (dazmorph.isDriven)
				{
					dazmorph.SetValueThreadSafe(0f);
					dazmorph.SetDriven(false, string.Empty, false);
					if (this.dirtyMorphs != null)
					{
						this.dirtyMorphs.Add(dazmorph);
					}
				}
			}
		}
		if (resetBones)
		{
			this.ZeroAllBoneMorphs();
		}
	}

	// Token: 0x06004B6E RID: 19310 RVA: 0x001A2730 File Offset: 0x001A0B30
	protected void MTTask(object info)
	{
		DAZMorphTaskInfo dazmorphTaskInfo = (DAZMorphTaskInfo)info;
		while (this._threadsRunning)
		{
			dazmorphTaskInfo.resetEvent.WaitOne(-1, true);
			if (dazmorphTaskInfo.kill)
			{
				break;
			}
			if (dazmorphTaskInfo.taskType == DAZMorphTaskType.ApplyMorphs)
			{
				Thread.Sleep(0);
				this.ApplyMorphsThreaded();
			}
			dazmorphTaskInfo.working = false;
		}
	}

	// Token: 0x06004B6F RID: 19311 RVA: 0x001A2794 File Offset: 0x001A0B94
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.applyMorphsTask != null)
		{
			this.applyMorphsTask.kill = true;
			this.applyMorphsTask.resetEvent.Set();
			while (this.applyMorphsTask.thread.IsAlive)
			{
			}
			this.applyMorphsTask = null;
		}
	}

	// Token: 0x06004B70 RID: 19312 RVA: 0x001A27F4 File Offset: 0x001A0BF4
	protected void StartThreads()
	{
		if (!this._threadsRunning)
		{
			this._threadsRunning = true;
			this.applyMorphsTask = new DAZMorphTaskInfo();
			this.applyMorphsTask.threadIndex = 0;
			this.applyMorphsTask.name = "ApplyMorphsTask";
			this.applyMorphsTask.resetEvent = new AutoResetEvent(false);
			this.applyMorphsTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.applyMorphsTask.thread.Priority = System.Threading.ThreadPriority.Lowest;
			this.applyMorphsTask.taskType = DAZMorphTaskType.ApplyMorphs;
			this.applyMorphsTask.thread.Start(this.applyMorphsTask);
		}
	}

	// Token: 0x06004B71 RID: 19313 RVA: 0x001A289A File Offset: 0x001A0C9A
	protected void ClearDirtyMorphs()
	{
		if (this.dirtyMorphs == null)
		{
			this.dirtyMorphs = new List<DAZMorph>();
		}
		else
		{
			this.dirtyMorphs.Clear();
		}
	}

	// Token: 0x06004B72 RID: 19314 RVA: 0x001A28C4 File Offset: 0x001A0CC4
	protected void CleanDirtyMorphs()
	{
		foreach (DAZMorph dazmorph in this.dirtyMorphs)
		{
			dazmorph.SyncJSON();
			dazmorph.SyncDrivenUI();
		}
	}

	// Token: 0x06004B73 RID: 19315 RVA: 0x001A2928 File Offset: 0x001A0D28
	public void ApplyMorphsThreadedStart()
	{
		if (this._morphs == null)
		{
			this._morphs = new List<DAZMorph>();
		}
		this.ClearDirtyMorphs();
		if (this.threadedChangedVertices == null)
		{
			this.numMaxThreadedChangedVertices = this._threadedMorphedUVVertices.Length;
			this.threadedChangedVertices = new int[this.numMaxThreadedChangedVertices];
		}
		this.checkAllThreadedVertices = false;
		this.numThreadedChangedVertices = 0;
		this.threadedVerticesChanged = false;
		this.visibleNonPoseThreadedVerticesChanged = false;
		this.bonesDirty = false;
		if (this.boneRotationsDirty == null)
		{
			this.boneRotationsDirty = new Dictionary<DAZBone, bool>();
		}
	}

	// Token: 0x06004B74 RID: 19316 RVA: 0x001A29B4 File Offset: 0x001A0DB4
	public void ApplyMorphsThreaded()
	{
		bool flag = true;
		int num = 5;
		while (flag)
		{
			num--;
			if (num == 0)
			{
				break;
			}
			flag = false;
			for (int i = 0; i < this._morphs.Count; i++)
			{
				DAZMorph dazmorph = this._morphs[i];
				if (!dazmorph.disable)
				{
					float num2 = dazmorph.morphValue;
					float appliedValue = dazmorph.appliedValue;
					bool flag2 = appliedValue != num2;
					bool flag3 = num2 != 0f;
					if (dazmorph.hasMorphValueFormulas && (flag2 || flag3))
					{
						foreach (DAZMorphFormula dazmorphFormula in dazmorph.formulas)
						{
							if (dazmorphFormula.targetType == DAZMorphFormulaTargetType.MorphValue)
							{
								DAZMorph morph = this.GetMorph(dazmorphFormula.target);
								if (morph != null)
								{
									if (dazmorph.wasZeroedKeepChildValues)
									{
										if (morph.SetDriven(false, string.Empty, false))
										{
											this.dirtyMorphs.Add(morph);
										}
									}
									else if (morph.SetValueThreadSafe(dazmorphFormula.multiplier * num2))
									{
										morph.SetDriven(flag3, dazmorph.displayName, false);
										this.dirtyMorphs.Add(morph);
									}
								}
							}
						}
					}
					dazmorph.wasZeroedKeepChildValues = false;
					if (this.enableMCMMorphs && dazmorph.hasMCMFormulas)
					{
						foreach (DAZMorphFormula dazmorphFormula2 in dazmorph.formulas)
						{
							if (dazmorphFormula2.targetType == DAZMorphFormulaTargetType.MCM)
							{
								DAZMorph morph2 = this.GetMorph(dazmorphFormula2.target);
								if (morph2 != null)
								{
									num2 = morph2.morphValue * dazmorphFormula2.multiplier;
									if (dazmorph.SetValueThreadSafe(num2))
									{
										dazmorph.SetDriven(true, morph2.displayName, false);
										this.dirtyMorphs.Add(dazmorph);
									}
								}
								else
								{
									num2 = 0f;
									if (dazmorph.SetValueThreadSafe(num2))
									{
										dazmorph.SetDriven(true, "Missing source morph", false);
										this.dirtyMorphs.Add(dazmorph);
									}
									num2 = dazmorph.morphValue;
								}
							}
							else if (dazmorphFormula2.targetType == DAZMorphFormulaTargetType.MCMMult)
							{
								DAZMorph morph3 = this.GetMorph(dazmorphFormula2.target);
								if (morph3 != null)
								{
									num2 = dazmorph.morphValue * morph3.morphValue;
								}
							}
						}
						flag2 = (appliedValue != num2);
						flag3 = (num2 != 0f);
					}
					if (flag2)
					{
						flag = true;
						if (dazmorph.isRuntime && !dazmorph.deltasLoaded)
						{
							dazmorph.LoadDeltas();
						}
						this.ApplyBoneMorphs(this.morphBones, dazmorph, false);
						this.ApplyBoneMorphs(this.morphBones2, dazmorph, false);
						if (dazmorph.deltas.Length > 0)
						{
							this.threadedVerticesChanged = true;
							if (dazmorph.visible && !dazmorph.isPoseControl)
							{
								this.visibleNonPoseThreadedVerticesChanged = true;
								float num3 = num2 - appliedValue;
								foreach (DAZMorphVertex dazmorphVertex in dazmorph.deltas)
								{
									if (dazmorphVertex.vertex < this._threadedMorphedUVVertices.Length)
									{
										if (this.numThreadedChangedVertices < this.numMaxThreadedChangedVertices)
										{
											this.threadedChangedVertices[this.numThreadedChangedVertices] = dazmorphVertex.vertex;
											this.numThreadedChangedVertices++;
										}
										else
										{
											this.checkAllThreadedVertices = true;
										}
										Vector3[] threadedMorphedUVVertices = this._threadedMorphedUVVertices;
										int vertex = dazmorphVertex.vertex;
										threadedMorphedUVVertices[vertex].x = threadedMorphedUVVertices[vertex].x + dazmorphVertex.delta.x * num3;
										Vector3[] threadedMorphedUVVertices2 = this._threadedMorphedUVVertices;
										int vertex2 = dazmorphVertex.vertex;
										threadedMorphedUVVertices2[vertex2].y = threadedMorphedUVVertices2[vertex2].y + dazmorphVertex.delta.y * num3;
										Vector3[] threadedMorphedUVVertices3 = this._threadedMorphedUVVertices;
										int vertex3 = dazmorphVertex.vertex;
										threadedMorphedUVVertices3[vertex3].z = threadedMorphedUVVertices3[vertex3].z + dazmorphVertex.delta.z * num3;
										Vector3[] threadedVisibleMorphedUVVertices = this._threadedVisibleMorphedUVVertices;
										int vertex4 = dazmorphVertex.vertex;
										threadedVisibleMorphedUVVertices[vertex4].x = threadedVisibleMorphedUVVertices[vertex4].x + dazmorphVertex.delta.x * num3;
										Vector3[] threadedVisibleMorphedUVVertices2 = this._threadedVisibleMorphedUVVertices;
										int vertex5 = dazmorphVertex.vertex;
										threadedVisibleMorphedUVVertices2[vertex5].y = threadedVisibleMorphedUVVertices2[vertex5].y + dazmorphVertex.delta.y * num3;
										Vector3[] threadedVisibleMorphedUVVertices3 = this._threadedVisibleMorphedUVVertices;
										int vertex6 = dazmorphVertex.vertex;
										threadedVisibleMorphedUVVertices3[vertex6].z = threadedVisibleMorphedUVVertices3[vertex6].z + dazmorphVertex.delta.z * num3;
									}
								}
							}
							else
							{
								float num4 = num2 - appliedValue;
								foreach (DAZMorphVertex dazmorphVertex2 in dazmorph.deltas)
								{
									if (dazmorphVertex2.vertex < this._threadedMorphedUVVertices.Length)
									{
										if (this.numThreadedChangedVertices < this.numMaxThreadedChangedVertices)
										{
											this.threadedChangedVertices[this.numThreadedChangedVertices] = dazmorphVertex2.vertex;
											this.numThreadedChangedVertices++;
										}
										else
										{
											this.checkAllThreadedVertices = true;
										}
										Vector3[] threadedMorphedUVVertices4 = this._threadedMorphedUVVertices;
										int vertex7 = dazmorphVertex2.vertex;
										threadedMorphedUVVertices4[vertex7].x = threadedMorphedUVVertices4[vertex7].x + dazmorphVertex2.delta.x * num4;
										Vector3[] threadedMorphedUVVertices5 = this._threadedMorphedUVVertices;
										int vertex8 = dazmorphVertex2.vertex;
										threadedMorphedUVVertices5[vertex8].y = threadedMorphedUVVertices5[vertex8].y + dazmorphVertex2.delta.y * num4;
										Vector3[] threadedMorphedUVVertices6 = this._threadedMorphedUVVertices;
										int vertex9 = dazmorphVertex2.vertex;
										threadedMorphedUVVertices6[vertex9].z = threadedMorphedUVVertices6[vertex9].z + dazmorphVertex2.delta.z * num4;
									}
								}
							}
						}
						dazmorph.appliedValue = num2;
						dazmorph.active = flag3;
					}
				}
			}
		}
	}

	// Token: 0x06004B75 RID: 19317 RVA: 0x001A2F60 File Offset: 0x001A1360
	public void PrepMorphsThreadedFast()
	{
		this.ClearDirtyMorphs();
	}

	// Token: 0x06004B76 RID: 19318 RVA: 0x001A2F68 File Offset: 0x001A1368
	public bool ApplyMorphsThreadedFast(Vector3[] verts, Vector3[] visibleNonPoseVerts, DAZBones bones)
	{
		bool flag = true;
		int num = 5;
		bool result = false;
		int num2 = verts.Length;
		while (flag)
		{
			num--;
			if (num == 0)
			{
				break;
			}
			flag = false;
			for (int i = 0; i < this._morphs.Count; i++)
			{
				DAZMorph dazmorph = this._morphs[i];
				if (!dazmorph.disable)
				{
					float num3 = dazmorph.morphValue;
					if (float.IsNaN(num3))
					{
						Debug.LogError("Detected NaN value for morph " + dazmorph.displayName);
					}
					else
					{
						float appliedValue = dazmorph.appliedValue;
						bool flag2 = appliedValue != num3;
						bool flag3 = num3 != 0f;
						if (dazmorph.hasMorphValueFormulas && (flag2 || flag3))
						{
							foreach (DAZMorphFormula dazmorphFormula in dazmorph.formulas)
							{
								if (dazmorphFormula.targetType == DAZMorphFormulaTargetType.MorphValue)
								{
									DAZMorph morph = this.GetMorph(dazmorphFormula.target);
									if (morph != null)
									{
										if (dazmorph.wasZeroedKeepChildValues)
										{
											if (morph.SetDriven(false, string.Empty, false))
											{
												this.dirtyMorphs.Add(morph);
											}
										}
										else if (morph.SetValueThreadSafe(dazmorphFormula.multiplier * num3))
										{
											morph.SetDriven(flag3, dazmorph.displayName, false);
											this.dirtyMorphs.Add(morph);
										}
									}
								}
							}
						}
						dazmorph.wasZeroedKeepChildValues = false;
						if (this.enableMCMMorphs && dazmorph.hasMCMFormulas)
						{
							foreach (DAZMorphFormula dazmorphFormula2 in dazmorph.formulas)
							{
								if (dazmorphFormula2.targetType == DAZMorphFormulaTargetType.MCM)
								{
									DAZMorph morph2 = this.GetMorph(dazmorphFormula2.target);
									if (morph2 != null)
									{
										num3 = morph2.morphValue * dazmorphFormula2.multiplier;
										if (dazmorph.SetValueThreadSafe(num3))
										{
											dazmorph.SetDriven(true, morph2.displayName, false);
											this.dirtyMorphs.Add(dazmorph);
										}
									}
									else
									{
										num3 = 0f;
										if (dazmorph.SetValueThreadSafe(num3))
										{
											dazmorph.SetDriven(true, "Missing source morph", false);
											this.dirtyMorphs.Add(dazmorph);
										}
									}
								}
								else if (dazmorphFormula2.targetType == DAZMorphFormulaTargetType.MCMMult)
								{
									DAZMorph morph3 = this.GetMorph(dazmorphFormula2.target);
									if (morph3 != null)
									{
										num3 = dazmorph.morphValue * morph3.morphValue;
									}
								}
							}
							flag2 = (appliedValue != num3);
							flag3 = (num3 != 0f);
						}
						if (flag2)
						{
							flag = true;
							if (dazmorph.isRuntime && !dazmorph.deltasLoaded)
							{
								dazmorph.LoadDeltas();
							}
							this.ApplyBoneMorphs(bones, dazmorph, false);
							if (dazmorph.deltas.Length > 0)
							{
								this.threadedVerticesChanged = true;
								float num4 = num3 - appliedValue;
								if (dazmorph.visible && !dazmorph.isPoseControl)
								{
									result = true;
									this.visibleNonPoseThreadedVerticesChanged = true;
									foreach (DAZMorphVertex dazmorphVertex in dazmorph.deltas)
									{
										if (dazmorphVertex.vertex < num2)
										{
											int vertex = dazmorphVertex.vertex;
											verts[vertex].x = verts[vertex].x + dazmorphVertex.delta.x * num4;
											int vertex2 = dazmorphVertex.vertex;
											verts[vertex2].y = verts[vertex2].y + dazmorphVertex.delta.y * num4;
											int vertex3 = dazmorphVertex.vertex;
											verts[vertex3].z = verts[vertex3].z + dazmorphVertex.delta.z * num4;
											int vertex4 = dazmorphVertex.vertex;
											visibleNonPoseVerts[vertex4].x = visibleNonPoseVerts[vertex4].x + dazmorphVertex.delta.x * num4;
											int vertex5 = dazmorphVertex.vertex;
											visibleNonPoseVerts[vertex5].y = visibleNonPoseVerts[vertex5].y + dazmorphVertex.delta.y * num4;
											int vertex6 = dazmorphVertex.vertex;
											visibleNonPoseVerts[vertex6].z = visibleNonPoseVerts[vertex6].z + dazmorphVertex.delta.z * num4;
										}
									}
								}
								else
								{
									foreach (DAZMorphVertex dazmorphVertex2 in dazmorph.deltas)
									{
										if (dazmorphVertex2.vertex < num2)
										{
											int vertex7 = dazmorphVertex2.vertex;
											verts[vertex7].x = verts[vertex7].x + dazmorphVertex2.delta.x * num4;
											int vertex8 = dazmorphVertex2.vertex;
											verts[vertex8].y = verts[vertex8].y + dazmorphVertex2.delta.y * num4;
											int vertex9 = dazmorphVertex2.vertex;
											verts[vertex9].z = verts[vertex9].z + dazmorphVertex2.delta.z * num4;
										}
									}
								}
							}
							dazmorph.appliedValue = num3;
							dazmorph.active = flag3;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06004B77 RID: 19319 RVA: 0x001A348B File Offset: 0x001A188B
	public void ApplyMorphsThreadedFastFinish()
	{
		this.CleanDirtyMorphs();
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x001A3494 File Offset: 0x001A1894
	public void ApplyMorphsThreadedFinish()
	{
		if (this.triggerThreadResetMorphs)
		{
			this.ApplyMorphsInternal(false);
			if (this.connectedMesh != null)
			{
				this.connectedMesh.ResetMorphedVertices();
				this._threadedMorphedUVVertices = (Vector3[])this.connectedMesh.UVVertices.Clone();
				this._threadedVisibleMorphedUVVertices = (Vector3[])this.connectedMesh.UVVertices.Clone();
			}
			else
			{
				Debug.LogWarning("ResetMorphs called when connected mesh was not set. Vertices were not reset.");
			}
			if (this._morphs != null)
			{
				for (int i = 0; i < this._morphs.Count; i++)
				{
					this._morphs[i].appliedValue = 0f;
					this._morphs[i].active = false;
				}
			}
			this.ZeroAllBoneMorphs();
			this.triggerThreadResetMorphs = false;
			this.visibleNonPoseVerticesChanged = true;
		}
		else
		{
			this.CleanDirtyMorphs();
			if (this.bonesDirty)
			{
				if (this.morphBones != null)
				{
					this.morphBones.SetMorphedTransform(false);
				}
				if (this.morphBones2 != null)
				{
					this.morphBones2.SetMorphedTransform(false);
				}
				this.bonesDirty = false;
			}
			foreach (DAZBone dazbone in this.boneRotationsDirty.Keys)
			{
				dazbone.SyncMorphBoneRotations(false);
			}
			this.boneRotationsDirty.Clear();
			this.visibleNonPoseVerticesChanged = this.visibleNonPoseThreadedVerticesChanged;
			if (this.threadedVerticesChanged && this.connectedMesh != null)
			{
				Vector3[] rawMorphedUVVertices = this.connectedMesh.rawMorphedUVVertices;
				Vector3[] visibleMorphedUVVertices = this.connectedMesh.visibleMorphedUVVertices;
				if (this.checkAllThreadedVertices)
				{
					int numBaseVertices = this.connectedMesh.numBaseVertices;
					for (int j = 0; j < numBaseVertices; j++)
					{
						rawMorphedUVVertices[j] = this._threadedMorphedUVVertices[j];
						visibleMorphedUVVertices[j] = this._threadedVisibleMorphedUVVertices[j];
					}
				}
				else
				{
					for (int k = 0; k < this.numThreadedChangedVertices; k++)
					{
						int num = this.threadedChangedVertices[k];
						rawMorphedUVVertices[num] = this._threadedMorphedUVVertices[num];
						visibleMorphedUVVertices[num] = this._threadedVisibleMorphedUVVertices[num];
					}
				}
				this.connectedMesh.ApplyMorphVertices(this.visibleNonPoseVerticesChanged);
			}
		}
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x001A3758 File Offset: 0x001A1B58
	protected void ApplyBoneMorphs(DAZBones bones, DAZMorph morph, bool zero = false)
	{
		if (bones != null)
		{
			foreach (DAZMorphFormula dazmorphFormula in morph.formulas)
			{
				switch (dazmorphFormula.targetType)
				{
				case DAZMorphFormulaTargetType.BoneCenterX:
				{
					this.bonesDirty = true;
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (zero)
						{
							dazbone.SetBoneXOffset(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneXOffset(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.BoneCenterY:
				{
					this.bonesDirty = true;
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (zero)
						{
							dazbone.SetBoneYOffset(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneYOffset(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.BoneCenterZ:
				{
					this.bonesDirty = true;
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (zero)
						{
							dazbone.SetBoneZOffset(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneZOffset(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.OrientationX:
				{
					this.bonesDirty = true;
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (zero)
						{
							dazbone.SetBoneOrientationXOffset(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneOrientationXOffset(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.OrientationY:
				{
					this.bonesDirty = true;
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (zero)
						{
							dazbone.SetBoneOrientationYOffset(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneOrientationYOffset(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.OrientationZ:
				{
					this.bonesDirty = true;
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (zero)
						{
							dazbone.SetBoneOrientationZOffset(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneOrientationZOffset(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.GeneralScale:
					if (zero)
					{
						bones.SetGeneralScale(this.geometryId + ":" + morph.morphName, 0f);
					}
					else
					{
						bones.SetGeneralScale(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
					}
					break;
				case DAZMorphFormulaTargetType.RotationX:
				{
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (!this.boneRotationsDirty.ContainsKey(dazbone))
						{
							this.boneRotationsDirty.Add(dazbone, true);
						}
						if (zero)
						{
							dazbone.SetBoneXRotation(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneXRotation(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.RotationY:
				{
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (!this.boneRotationsDirty.ContainsKey(dazbone))
						{
							this.boneRotationsDirty.Add(dazbone, true);
						}
						if (zero)
						{
							dazbone.SetBoneYRotation(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneYRotation(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				case DAZMorphFormulaTargetType.RotationZ:
				{
					DAZBone dazbone = bones.GetDAZBone(dazmorphFormula.target);
					if (dazbone != null)
					{
						if (!this.boneRotationsDirty.ContainsKey(dazbone))
						{
							this.boneRotationsDirty.Add(dazbone, true);
						}
						if (zero)
						{
							dazbone.SetBoneZRotation(this.geometryId + ":" + morph.morphName, 0f);
						}
						else
						{
							dazbone.SetBoneZRotation(this.geometryId + ":" + morph.morphName, dazmorphFormula.multiplier * morph.morphValue);
						}
					}
					break;
				}
				}
			}
		}
	}

	// Token: 0x06004B7A RID: 19322 RVA: 0x001A3CCC File Offset: 0x001A20CC
	protected void ApplyAllBoneMorphs()
	{
		for (int i = 0; i < this._morphs.Count; i++)
		{
			this.ApplyBoneMorphs(this.morphBones, this._morphs[i], false);
			this.ApplyBoneMorphs(this.morphBones2, this._morphs[i], false);
		}
		if (this.morphBones != null)
		{
			this.morphBones.SetMorphedTransform(true);
		}
		if (this.morphBones2 != null)
		{
			this.morphBones2.SetMorphedTransform(true);
		}
		this.bonesDirty = false;
	}

	// Token: 0x06004B7B RID: 19323 RVA: 0x001A3D68 File Offset: 0x001A2168
	protected void ZeroAllBoneMorphs()
	{
		if (this._morphs != null)
		{
			for (int i = 0; i < this._morphs.Count; i++)
			{
				this.ApplyBoneMorphs(this.morphBones, this._morphs[i], true);
				this.ApplyBoneMorphs(this.morphBones2, this._morphs[i], true);
			}
		}
		if (this.morphBones != null)
		{
			foreach (DAZBone dazbone in this.morphBones.dazBones)
			{
				dazbone.ForceClearMorphs();
			}
			this.morphBones.SetMorphedTransform(true);
		}
		if (this.morphBones2 != null)
		{
			foreach (DAZBone dazbone2 in this.morphBones2.dazBones)
			{
				dazbone2.ForceClearMorphs();
			}
			this.morphBones2.SetMorphedTransform(true);
		}
		this.bonesDirty = false;
	}

	// Token: 0x06004B7C RID: 19324 RVA: 0x001A3E70 File Offset: 0x001A2270
	protected void ApplyMorphsInternal(bool force = false)
	{
		if (this._morphs == null)
		{
			this._morphs = new List<DAZMorph>();
		}
		this.visibleNonPoseVerticesChanged = false;
		bool flag = false;
		bool flag2 = true;
		int num = 5;
		this.bonesDirty = false;
		Vector3[] array = null;
		Vector3[] array2 = null;
		Vector3[] array3 = null;
		if (this.connectedMesh != null)
		{
			array = this.connectedMesh.morphedBaseVertices;
			array2 = this.connectedMesh.rawMorphedUVVertices;
			array3 = this.connectedMesh.visibleMorphedUVVertices;
		}
		while (flag2)
		{
			num--;
			if (num == 0)
			{
				break;
			}
			flag2 = false;
			for (int i = 0; i < this._morphs.Count; i++)
			{
				DAZMorph dazmorph = this._morphs[i];
				if (!dazmorph.disable)
				{
					float num2 = dazmorph.morphValue;
					float appliedValue = dazmorph.appliedValue;
					bool flag3 = appliedValue != num2;
					bool flag4 = num2 != 0f;
					if (dazmorph.hasMorphValueFormulas && (flag3 || flag4))
					{
						foreach (DAZMorphFormula dazmorphFormula in dazmorph.formulas)
						{
							if (dazmorphFormula.targetType == DAZMorphFormulaTargetType.MorphValue)
							{
								DAZMorph morph = this.GetMorph(dazmorphFormula.target);
								if (morph != null)
								{
									if (dazmorph.wasZeroedKeepChildValues)
									{
										morph.SetDriven(false, string.Empty, true);
									}
									else
									{
										morph.morphValue = dazmorphFormula.multiplier * num2;
										morph.SetDriven(flag4, dazmorph.displayName, true);
									}
								}
							}
						}
					}
					dazmorph.wasZeroedKeepChildValues = false;
					if (this.enableMCMMorphs && dazmorph.hasMCMFormulas)
					{
						foreach (DAZMorphFormula dazmorphFormula2 in dazmorph.formulas)
						{
							if (dazmorphFormula2.targetType == DAZMorphFormulaTargetType.MCM)
							{
								DAZMorph morph2 = this.GetMorph(dazmorphFormula2.target);
								if (morph2 != null)
								{
									num2 = morph2.morphValue * dazmorphFormula2.multiplier;
									dazmorph.morphValue = num2;
									dazmorph.SetDriven(true, morph2.displayName, false);
								}
								else
								{
									num2 = 0f;
									dazmorph.morphValue = 0f;
								}
							}
							else if (dazmorphFormula2.targetType == DAZMorphFormulaTargetType.MCMMult)
							{
								DAZMorph morph3 = this.GetMorph(dazmorphFormula2.target);
								if (morph3 != null)
								{
									num2 = dazmorph.morphValue * morph3.morphValue;
								}
							}
						}
						flag3 = (appliedValue != num2);
						flag4 = (num2 != 0f);
					}
					if (flag3)
					{
						flag2 = true;
						if (dazmorph.isRuntime && !dazmorph.deltasLoaded)
						{
							dazmorph.LoadDeltas();
						}
						this.ApplyBoneMorphs(this.morphBones, dazmorph, false);
						this.ApplyBoneMorphs(this.morphBones2, dazmorph, false);
						if (dazmorph.deltas.Length > 0 && this.connectedMesh != null && array2 != null)
						{
							flag = true;
							if (dazmorph.visible && !dazmorph.isPoseControl)
							{
								this.visibleNonPoseVerticesChanged = true;
								float num3 = num2 - appliedValue;
								foreach (DAZMorphVertex dazmorphVertex in dazmorph.deltas)
								{
									if (dazmorphVertex.vertex < array2.Length)
									{
										Vector3[] array4 = array2;
										int vertex = dazmorphVertex.vertex;
										array4[vertex].x = array4[vertex].x + dazmorphVertex.delta.x * num3;
										array[dazmorphVertex.vertex].x = array2[dazmorphVertex.vertex].x;
										array3[dazmorphVertex.vertex].x = array2[dazmorphVertex.vertex].x;
										Vector3[] array5 = array2;
										int vertex2 = dazmorphVertex.vertex;
										array5[vertex2].y = array5[vertex2].y + dazmorphVertex.delta.y * num3;
										array[dazmorphVertex.vertex].y = array2[dazmorphVertex.vertex].y;
										array3[dazmorphVertex.vertex].y = array2[dazmorphVertex.vertex].y;
										Vector3[] array6 = array2;
										int vertex3 = dazmorphVertex.vertex;
										array6[vertex3].z = array6[vertex3].z + dazmorphVertex.delta.z * num3;
										array[dazmorphVertex.vertex].z = array2[dazmorphVertex.vertex].z;
										array3[dazmorphVertex.vertex].z = array2[dazmorphVertex.vertex].z;
										if (dazmorph.triggerNormalRecalc)
										{
											this.connectedMesh.morphedBaseDirtyVertices[dazmorphVertex.vertex] = true;
											this.connectedMesh.morphedNormalsDirty = true;
										}
										if (dazmorph.triggerTangentRecalc)
										{
											this.connectedMesh.morphedUVDirtyVertices[dazmorphVertex.vertex] = true;
											this.connectedMesh.morphedTangentsDirty = true;
										}
									}
								}
							}
							else
							{
								float num4 = num2 - appliedValue;
								foreach (DAZMorphVertex dazmorphVertex2 in dazmorph.deltas)
								{
									if (dazmorphVertex2.vertex < array2.Length)
									{
										Vector3[] array7 = array2;
										int vertex4 = dazmorphVertex2.vertex;
										array7[vertex4].x = array7[vertex4].x + dazmorphVertex2.delta.x * num4;
										array[dazmorphVertex2.vertex].x = array2[dazmorphVertex2.vertex].x;
										Vector3[] array8 = array2;
										int vertex5 = dazmorphVertex2.vertex;
										array8[vertex5].y = array8[vertex5].y + dazmorphVertex2.delta.y * num4;
										array[dazmorphVertex2.vertex].y = array2[dazmorphVertex2.vertex].y;
										Vector3[] array9 = array2;
										int vertex6 = dazmorphVertex2.vertex;
										array9[vertex6].z = array9[vertex6].z + dazmorphVertex2.delta.z * num4;
										array[dazmorphVertex2.vertex].z = array2[dazmorphVertex2.vertex].z;
										if (dazmorph.triggerNormalRecalc)
										{
											this.connectedMesh.morphedBaseDirtyVertices[dazmorphVertex2.vertex] = true;
											this.connectedMesh.morphedNormalsDirty = true;
										}
										if (dazmorph.triggerTangentRecalc)
										{
											this.connectedMesh.morphedUVDirtyVertices[dazmorphVertex2.vertex] = true;
											this.connectedMesh.morphedTangentsDirty = true;
										}
									}
								}
							}
						}
						dazmorph.appliedValue = num2;
						dazmorph.active = flag4;
					}
				}
			}
		}
		if (this.bonesDirty)
		{
			if (this.morphBones != null)
			{
				this.morphBones.SetMorphedTransform(true);
			}
			if (this.morphBones2 != null)
			{
				this.morphBones2.SetMorphedTransform(true);
			}
			this.bonesDirty = false;
		}
		if ((flag || force) && this.connectedMesh != null)
		{
			this.connectedMesh.ApplyMorphVertices(this.visibleNonPoseVerticesChanged);
		}
	}

	// Token: 0x06004B7D RID: 19325 RVA: 0x001A45B4 File Offset: 0x001A29B4
	public void ApplyMorphs(bool force = false)
	{
		if (this.connectedMesh && this.connectedMesh.wasInit)
		{
			this.connectedMesh.StartMorph();
			if (!Application.isPlaying || !this.useThreadedMorphing)
			{
				this.ApplyMorphsInternal(force);
			}
		}
	}

	// Token: 0x06004B7E RID: 19326 RVA: 0x001A4608 File Offset: 0x001A2A08
	public void ApplyMorphsImmediate()
	{
		if (Application.isPlaying && this.useThreadedMorphing && this._threadsRunning)
		{
			while (this.applyMorphsTask.working)
			{
				Thread.Sleep(0);
			}
			this.ApplyMorphsThreadedFinish();
			this.ApplyMorphsThreadedStart();
			this.applyMorphsTask.working = true;
			this.applyMorphsTask.resetEvent.Set();
			while (this.applyMorphsTask.working)
			{
				Thread.Sleep(0);
			}
			this.ApplyMorphsThreadedFinish();
			this.ApplyMorphsThreadedStart();
			this.applyMorphsTask.working = true;
			this.applyMorphsTask.resetEvent.Set();
			this.visibleNonPoseVerticesChanged = true;
			if (this.connectedMesh != null)
			{
				this.connectedMesh.ApplyMorphVertices(this.visibleNonPoseVerticesChanged);
			}
		}
		else
		{
			this.ApplyMorphsInternal(false);
		}
	}

	// Token: 0x06004B7F RID: 19327 RVA: 0x001A46FC File Offset: 0x001A2AFC
	private void Update()
	{
		if (this.connectedMesh && this.connectedMesh.wasInit && this.updateEnabled)
		{
			this.connectedMesh.StartMorph();
			if (Application.isPlaying && this.useThreadedMorphing)
			{
				this.StartThreads();
				this.totalFrameCount++;
				if (!this.applyMorphsTask.working)
				{
					this.ApplyMorphsThreadedFinish();
					this.ApplyMorphsThreadedStart();
					this.applyMorphsTask.working = true;
					this.applyMorphsTask.resetEvent.Set();
				}
				else if (OVRManager.isHmdPresent)
				{
					this.missedFrameCount++;
					Debug.LogWarning(string.Concat(new object[]
					{
						"ApplyMorphsTask did not complete in 1 frame. Missed ",
						this.missedFrameCount,
						" out of total ",
						this.totalFrameCount
					}));
					DebugHUD.Msg(string.Concat(new object[]
					{
						"ApplyMorphsTask miss ",
						this.missedFrameCount,
						" out of total ",
						this.totalFrameCount
					}));
					DebugHUD.Alert2();
				}
			}
			else
			{
				this.ApplyMorphsInternal(false);
			}
		}
	}

	// Token: 0x06004B80 RID: 19328 RVA: 0x001A484B File Offset: 0x001A2C4B
	public void ReInit()
	{
		this.RebuildAllLookups();
		this.ResetMorphs();
		this.ApplyMorphs(true);
	}

	// Token: 0x06004B81 RID: 19329 RVA: 0x001A4860 File Offset: 0x001A2C60
	public void Init()
	{
		if (!this.wasInit)
		{
			this.wasInit = true;
			if (this.boneRotationsDirty == null)
			{
				this.boneRotationsDirty = new Dictionary<DAZBone, bool>();
			}
			if (this.morphBones != null)
			{
				this.morphBones.Init();
			}
			if (this.morphBones2 != null)
			{
				this.morphBones2.Init();
			}
			this.RebuildAllLookups();
			if (Application.isPlaying)
			{
				this.RefreshRuntimeMorphs();
				this.RefreshPackageMorphs();
				this.InitMorphs();
			}
		}
	}

	// Token: 0x06004B82 RID: 19330 RVA: 0x001A48F4 File Offset: 0x001A2CF4
	public bool RefreshRuntimeMorphs()
	{
		bool result = false;
		if (this.wasInit && this.autoImportFolder != null && this.autoImportFolder != string.Empty && this.RuntimeImportFromDir(this.autoImportFolder, false, false, true))
		{
			DAZMorphSubBank[] componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
			foreach (DAZMorphSubBank dazmorphSubBank in componentsInChildren)
			{
				dazmorphSubBank.CompleteRuntimeMorphAdd();
			}
			this.RebuildAllLookups();
			result = true;
		}
		return result;
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x001A4978 File Offset: 0x001A2D78
	public bool RefreshPackageMorphs()
	{
		if (this.wasInit && this.autoImportFolder != null && this.autoImportFolder != string.Empty)
		{
			List<VarDirectoryEntry> list = FileManager.FindVarDirectories(this.autoImportFolder, true);
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (VarDirectoryEntry varDirectoryEntry in list)
			{
				hashSet.Add(varDirectoryEntry.Package.Uid);
				if (varDirectoryEntry.Package.Group.GetCustomOption("preloadMorphs"))
				{
					hashSet2.Add(varDirectoryEntry.Package.Uid);
				}
			}
			if (this.currentLoadedMorphPackageUids != null && this.currentPreloadMorphPackageUids != null && this.currentLoadedMorphPackageUids.SetEquals(hashSet) && this.currentPreloadMorphPackageUids.SetEquals(hashSet2))
			{
				return false;
			}
			this.currentLoadedMorphPackageUids = hashSet;
			this.currentPreloadMorphPackageUids = hashSet2;
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			Dictionary<string, float> dictionary3 = new Dictionary<string, float>();
			foreach (DAZMorph dazmorph in this._morphs)
			{
				if (dazmorph.isInPackage && dazmorph.morphValue != dazmorph.startValue)
				{
					dictionary.Add(dazmorph.uid, dazmorph.morphValue);
					dictionary2.Add(dazmorph.uid, dazmorph.resolvedDisplayName);
					if (!dictionary3.ContainsKey(dazmorph.resolvedDisplayName))
					{
						dictionary3.Add(dazmorph.resolvedDisplayName, dazmorph.morphValue);
					}
				}
			}
			DAZMorphSubBank[] componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
			bool flag = false;
			foreach (DAZMorphSubBank dazmorphSubBank in componentsInChildren)
			{
				if (dazmorphSubBank.ClearPackageMorphs())
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.RebuildAllLookups();
			}
			bool flag2 = false;
			foreach (VarDirectoryEntry varDirectoryEntry2 in list)
			{
				if (varDirectoryEntry2.Package.Group.GetCustomOption("preloadMorphs"))
				{
					if (this.RuntimeImportFromDir(varDirectoryEntry2, false, false, false))
					{
						flag2 = true;
					}
				}
				else if (this.RuntimeImportFromDir(varDirectoryEntry2, false, true, false))
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				componentsInChildren = base.GetComponentsInChildren<DAZMorphSubBank>();
				foreach (DAZMorphSubBank dazmorphSubBank2 in componentsInChildren)
				{
					dazmorphSubBank2.CompletePackageMorphAdd();
				}
				this.RebuildAllLookups();
				foreach (string text in dictionary.Keys)
				{
					DAZMorph dazmorph2 = this.GetMorphByUid(text);
					string morphDisplayName;
					if (dazmorph2 == null && dictionary2.TryGetValue(text, out morphDisplayName))
					{
						dazmorph2 = this.GetMorphByDisplayName(morphDisplayName);
					}
					float morphValue;
					if (dazmorph2 != null && dictionary.TryGetValue(text, out morphValue))
					{
						dazmorph2.morphValue = morphValue;
					}
				}
				this.CleanDemandActivatedMorphs();
			}
			if (flag || flag2)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x001A4D20 File Offset: 0x001A3120
	protected void EnableInit()
	{
		if (this.connectedMesh != null)
		{
			this.connectedMesh.Init();
			if (Application.isPlaying)
			{
				if (this._threadedMorphedUVVertices == null)
				{
					this._threadedMorphedUVVertices = (Vector3[])this.connectedMesh.UVVertices.Clone();
				}
				if (this._threadedVisibleMorphedUVVertices == null)
				{
					this._threadedVisibleMorphedUVVertices = (Vector3[])this.connectedMesh.UVVertices.Clone();
				}
			}
		}
		this.ResetMorphs();
		this.ApplyMorphs(true);
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x001A4DAC File Offset: 0x001A31AC
	private void OnDisable()
	{
		if (this.updateEnabled)
		{
			if (!Application.isPlaying)
			{
				this.wasInit = false;
			}
			this.ZeroAllBoneMorphs();
		}
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x001A4DD0 File Offset: 0x001A31D0
	private void OnEnable()
	{
		if (this.updateEnabled)
		{
			if (!Application.isPlaying)
			{
				this.Init();
			}
			this.EnableInit();
		}
	}

	// Token: 0x06004B87 RID: 19335 RVA: 0x001A4DF3 File Offset: 0x001A31F3
	private void Awake()
	{
		if (Application.isPlaying)
		{
			this.Init();
		}
	}

	// Token: 0x06004B88 RID: 19336 RVA: 0x001A4E05 File Offset: 0x001A3205
	protected void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
		}
	}

	// Token: 0x06004B89 RID: 19337 RVA: 0x001A4E17 File Offset: 0x001A3217
	protected void OnApplicationQuit()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
		}
	}

	// Token: 0x04003A2C RID: 14892
	[SerializeField]
	protected DAZMesh _connectedMesh;

	// Token: 0x04003A2D RID: 14893
	public string autoImportFolder = string.Empty;

	// Token: 0x04003A2E RID: 14894
	public string autoImportFolderLegacy = string.Empty;

	// Token: 0x04003A2F RID: 14895
	public string conflictingFolder = string.Empty;

	// Token: 0x04003A30 RID: 14896
	public string geometryId;

	// Token: 0x04003A31 RID: 14897
	public DAZBones morphBones;

	// Token: 0x04003A32 RID: 14898
	public DAZBones morphBones2;

	// Token: 0x04003A33 RID: 14899
	public bool enableMCMMorphs = true;

	// Token: 0x04003A34 RID: 14900
	public bool bonesDirty;

	// Token: 0x04003A35 RID: 14901
	public Dictionary<DAZBone, bool> boneRotationsDirty;

	// Token: 0x04003A36 RID: 14902
	public DAZMorphBank.MorphFavoriteChanged onMorphFavoriteChangedHandlers;

	// Token: 0x04003A37 RID: 14903
	protected Dictionary<string, DAZMorphSubBank> _morphSubBanksByRegion;

	// Token: 0x04003A38 RID: 14904
	protected List<DAZMorph> _morphs;

	// Token: 0x04003A39 RID: 14905
	protected List<DAZMorph> _unactivatedMorphs;

	// Token: 0x04003A3A RID: 14906
	protected Dictionary<string, DAZMorph> _builtInMorphsByName;

	// Token: 0x04003A3B RID: 14907
	protected Dictionary<string, DAZMorph> _builtInMorphsByUid;

	// Token: 0x04003A3C RID: 14908
	protected Dictionary<string, DAZMorph> _morphsByName;

	// Token: 0x04003A3D RID: 14909
	protected Dictionary<string, DAZMorph> _morphsByUid;

	// Token: 0x04003A3E RID: 14910
	protected Dictionary<string, DAZMorph> _morphsByDisplayName;

	// Token: 0x04003A3F RID: 14911
	protected bool demandActivatedMorphsDirty;

	// Token: 0x04003A40 RID: 14912
	protected Dictionary<string, string> _morphToRegionName;

	// Token: 0x04003A41 RID: 14913
	protected HashSet<string> badMorphNames;

	// Token: 0x04003A42 RID: 14914
	public bool updateEnabled = true;

	// Token: 0x04003A43 RID: 14915
	public bool useThreadedMorphing;

	// Token: 0x04003A44 RID: 14916
	protected DAZMorphTaskInfo applyMorphsTask;

	// Token: 0x04003A45 RID: 14917
	protected bool _threadsRunning;

	// Token: 0x04003A46 RID: 14918
	protected bool triggerThreadResetMorphs;

	// Token: 0x04003A47 RID: 14919
	protected bool threadedVerticesChanged;

	// Token: 0x04003A48 RID: 14920
	protected int[] threadedChangedVertices;

	// Token: 0x04003A49 RID: 14921
	protected int numThreadedChangedVertices;

	// Token: 0x04003A4A RID: 14922
	protected int numMaxThreadedChangedVertices;

	// Token: 0x04003A4B RID: 14923
	protected bool checkAllThreadedVertices;

	// Token: 0x04003A4C RID: 14924
	protected bool visibleNonPoseThreadedVerticesChanged;

	// Token: 0x04003A4D RID: 14925
	public bool visibleNonPoseVerticesChanged;

	// Token: 0x04003A4E RID: 14926
	protected Vector3[] _threadedMorphedUVVertices;

	// Token: 0x04003A4F RID: 14927
	protected Vector3[] _threadedVisibleMorphedUVVertices;

	// Token: 0x04003A50 RID: 14928
	public bool _threadedNormalsDirtyThisFrame;

	// Token: 0x04003A51 RID: 14929
	public bool _threadedTangentsDirtyThisFrame;

	// Token: 0x04003A52 RID: 14930
	protected List<DAZMorph> dirtyMorphs;

	// Token: 0x04003A53 RID: 14931
	protected int totalFrameCount;

	// Token: 0x04003A54 RID: 14932
	protected int missedFrameCount;

	// Token: 0x04003A55 RID: 14933
	protected bool wasInit;

	// Token: 0x04003A56 RID: 14934
	protected HashSet<string> currentLoadedMorphPackageUids;

	// Token: 0x04003A57 RID: 14935
	protected HashSet<string> currentPreloadMorphPackageUids;

	// Token: 0x02000AF5 RID: 2805
	// (Invoke) Token: 0x06004B8B RID: 19339
	public delegate void MorphFavoriteChanged(DAZMorph morph);
}
