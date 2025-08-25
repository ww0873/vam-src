using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000C51 RID: 3153
public class SubScene : JSONStorable
{
	// Token: 0x06005BA8 RID: 23464 RVA: 0x0021AA64 File Offset: 0x00218E64
	public SubScene()
	{
	}

	// Token: 0x06005BA9 RID: 23465 RVA: 0x0021AAD6 File Offset: 0x00218ED6
	protected void SyncDrawContainedAtomsLines(bool b)
	{
		this._drawContainedAtomsLines = b;
	}

	// Token: 0x06005BAA RID: 23466 RVA: 0x0021AADF File Offset: 0x00218EDF
	public override void PostRestore()
	{
		if (this._loadOnRestoreFromOtherSubScene && this.containingAtom.isSubSceneRestore)
		{
			this.LoadSubScene();
		}
	}

	// Token: 0x06005BAB RID: 23467 RVA: 0x0021AB04 File Offset: 0x00218F04
	protected void SetNamesFromFilePath(string fpath, bool noCallback = false)
	{
		VarFileEntry varFileEntry = FileManager.GetVarFileEntry(fpath);
		string text = string.Empty;
		string text2 = fpath;
		if (varFileEntry != null)
		{
			text = varFileEntry.Package.Uid;
			text2 = varFileEntry.InternalSlashPath;
		}
		string text3 = text2.Replace(this.storeRoot, string.Empty);
		if (text3 == text2)
		{
			SuperController.LogError("SubScene path " + fpath + " is not in correct form");
		}
		else
		{
			string[] array = text3.Split(new char[]
			{
				'/'
			});
			if (array.Length < 3)
			{
				SuperController.LogError("SubScene path " + fpath + " is not in correct form");
			}
			else
			{
				string text4 = array[2];
				for (int i = 3; i < array.Length; i++)
				{
					text4 = text4 + "/" + array[i];
				}
				text4 = Regex.Replace(text4, "\\.json$", string.Empty);
				if (noCallback)
				{
					this.packageUidJSON.valNoCallback = text;
					this.creatorNameJSON.valNoCallback = array[0];
					this.signatureJSON.valNoCallback = array[1];
					this.storeNameJSON.valNoCallback = text4;
					this.storedCreatorNameJSON.valNoCallback = array[0];
				}
				else
				{
					this.packageUidJSON.val = text;
					this.creatorNameJSON.val = array[0];
					this.signatureJSON.val = array[1];
					this.storeNameJSON.val = text4;
					this.storedCreatorNameJSON.val = array[0];
				}
			}
		}
		this.SyncStoreButton();
		this.SyncLoadButton();
	}

	// Token: 0x06005BAC RID: 23468 RVA: 0x0021AC90 File Offset: 0x00219090
	protected void BeginBrowse(JSONStorableUrl jsurl)
	{
		FileManager.CreateDirectory(this.storeRoot);
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory(this.storeRoot, false, false, true, true);
		jsurl.shortCuts = shortCutsForDirectory;
	}

	// Token: 0x06005BAD RID: 23469 RVA: 0x0021ACC0 File Offset: 0x002190C0
	protected void SyncBrowsePath(string url)
	{
		this.SetNamesFromFilePath(url, false);
		if (this.autoSetSubSceneUIDToSignatureOnBrowseLoadJSON.val && this.browsePathJSON.valueSetFromBrowse)
		{
			SuperController.singleton.RenameAtom(this.containingAtom, this.signatureJSON.val);
		}
		this.LoadSubScene();
	}

	// Token: 0x06005BAE RID: 23470 RVA: 0x0021AD16 File Offset: 0x00219116
	protected void LoadSubSceneWithPath(string p)
	{
		this.browsePathJSON.SetFilePath(p);
	}

	// Token: 0x06005BAF RID: 23471 RVA: 0x0021AD24 File Offset: 0x00219124
	protected void SetStorePathFromParts()
	{
		this.storePathJSON.valNoCallback = this.GetStorePath(true) + ".json";
		this.SyncStoreButton();
		this.SyncLoadButton();
	}

	// Token: 0x06005BB0 RID: 23472 RVA: 0x0021AD4E File Offset: 0x0021914E
	protected void SyncStorePath(string url)
	{
		this.SetNamesFromFilePath(url, true);
	}

	// Token: 0x06005BB1 RID: 23473 RVA: 0x0021AD58 File Offset: 0x00219158
	protected void SyncPackageUid(string s)
	{
		this.SetStorePathFromParts();
	}

	// Token: 0x06005BB2 RID: 23474 RVA: 0x0021AD60 File Offset: 0x00219160
	protected void ClearPackageUid()
	{
		this.packageUidJSON.val = string.Empty;
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x0021AD72 File Offset: 0x00219172
	protected void SyncCreatorName(string s)
	{
		this.SetStorePathFromParts();
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x0021AD7A File Offset: 0x0021917A
	protected void SetToYourCreatorName()
	{
		if (UserPreferences.singleton != null)
		{
			this.creatorNameJSON.val = UserPreferences.singleton.creatorName;
		}
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x0021ADA1 File Offset: 0x002191A1
	protected void SyncSignature(string s)
	{
		this.storedCreatorNameJSON.val = string.Empty;
		this.SetStorePathFromParts();
	}

	// Token: 0x06005BB6 RID: 23478 RVA: 0x0021ADB9 File Offset: 0x002191B9
	protected void SyncStoreName(string s)
	{
		this.storedCreatorNameJSON.val = string.Empty;
		this.SetStorePathFromParts();
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x0021ADD1 File Offset: 0x002191D1
	protected void ClearSubScene()
	{
		this.PreRemove();
	}

	// Token: 0x06005BB8 RID: 23480 RVA: 0x0021ADD9 File Offset: 0x002191D9
	protected void UnparentAllAtoms()
	{
		this.containingAtom.RemoveAllChildAtoms();
	}

	// Token: 0x06005BB9 RID: 23481 RVA: 0x0021ADE6 File Offset: 0x002191E6
	protected void SyncLoadOnRestoreFromOtherSubScene(bool b)
	{
		this._loadOnRestoreFromOtherSubScene = b;
	}

	// Token: 0x06005BBA RID: 23482 RVA: 0x0021ADF0 File Offset: 0x002191F0
	protected void AddLooseAtomsToSubScene()
	{
		if (SuperController.singleton != null)
		{
			foreach (Atom atom in SuperController.singleton.GetAtoms())
			{
				if (atom.canBeParented && !atom.isSubSceneType && atom.type != "PlayerNavigationPanel" && atom.parentAtom == null)
				{
					atom.SetParentAtom(this.containingAtom);
				}
			}
		}
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x0021AEA4 File Offset: 0x002192A4
	protected void IsolateEditSubScene()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.StartIsolateEditSubScene(this);
		}
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x0021AEC4 File Offset: 0x002192C4
	public bool CheckReadyForStore()
	{
		return this.packageUidJSON != null && this.packageUidJSON.val == string.Empty && this.creatorNameJSON.val != null && this.creatorNameJSON.val != string.Empty && this.signatureJSON.val != null && this.signatureJSON.val != string.Empty && this.storeNameJSON.val != null && this.storeNameJSON.val != string.Empty;
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x0021AF78 File Offset: 0x00219378
	protected string GetStorePath(bool includePackage = true)
	{
		string text = string.Empty;
		if (this.packageUidJSON.val != string.Empty && includePackage)
		{
			text = this.packageUidJSON.val + ":/";
		}
		string text2 = text;
		return string.Concat(new string[]
		{
			text2,
			this.storeRoot,
			this.creatorNameJSON.val,
			"/",
			this.signatureJSON.val,
			"/",
			this.storeNameJSON.val
		});
	}

	// Token: 0x06005BBE RID: 23486 RVA: 0x0021B018 File Offset: 0x00219418
	public bool CheckExistance()
	{
		string path = this.GetStorePath(true) + ".json";
		return FileManager.FileExists(path, false, false);
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x0021B048 File Offset: 0x00219448
	protected void SyncStoreButton()
	{
		if (this.StoreSubSceneAction.dynamicButton != null)
		{
			if (this.CheckReadyForStore())
			{
				if (this.StoreSubSceneAction.dynamicButton.button != null)
				{
					this.StoreSubSceneAction.dynamicButton.button.interactable = true;
				}
				if (this.CheckExistance())
				{
					this.StoreSubSceneAction.dynamicButton.buttonColor = Color.red;
					if (this.StoreSubSceneAction.dynamicButton.buttonText != null)
					{
						this.StoreSubSceneAction.dynamicButton.buttonText.text = "Overwrite Existing SubScene";
					}
				}
				else
				{
					this.StoreSubSceneAction.dynamicButton.buttonColor = Color.green;
					if (this.StoreSubSceneAction.dynamicButton.buttonText != null)
					{
						this.StoreSubSceneAction.dynamicButton.buttonText.text = "Save New SubScene";
					}
				}
			}
			else
			{
				this.StoreSubSceneAction.dynamicButton.buttonColor = Color.gray;
				if (this.StoreSubSceneAction.dynamicButton.button != null)
				{
					this.StoreSubSceneAction.dynamicButton.button.interactable = false;
				}
				if (this.StoreSubSceneAction.dynamicButton.buttonText != null)
				{
					this.StoreSubSceneAction.dynamicButton.buttonText.text = "Not Ready For Save";
				}
			}
		}
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x0021B1CC File Offset: 0x002195CC
	public void StoreSubScene()
	{
		if (this.CheckReadyForStore())
		{
			try
			{
				string storePath = this.GetStorePath(true);
				string path = storePath + ".json";
				string directoryName = FileManager.GetDirectoryName(storePath, false);
				FileManager.CreateDirectory(directoryName);
				FileManager.SetSaveDirFromFilePath(path, true);
				JSONClass jsonclass = new JSONClass();
				jsonclass["setUnlistedParamsToDefault"].AsBool = true;
				this.drawContainedAtomsLinesJSON.StoreJSON(jsonclass, true, true, true);
				JSONClass jsonclass2 = new JSONClass();
				jsonclass["subScene"] = jsonclass2;
				this.containingAtom.StoreForSubScene(jsonclass2, true);
				JSONArray jsonarray = new JSONArray();
				jsonclass["atoms"] = jsonarray;
				foreach (Atom atom in this._atomsInSubScene)
				{
					foreach (FreeControllerV3 freeControllerV in atom.freeControllers)
					{
						freeControllerV.forceStorePositionRotationAsLocal = true;
					}
				}
				List<Atom> list = this._atomsInSubScene.ToList<Atom>();
				List<Atom> list2 = list;
				if (SubScene.<>f__am$cache0 == null)
				{
					SubScene.<>f__am$cache0 = new Comparison<Atom>(SubScene.<StoreSubScene>m__0);
				}
				list2.Sort(SubScene.<>f__am$cache0);
				foreach (Atom atom2 in list)
				{
					JSONClass jsonclass3 = new JSONClass();
					jsonarray.Add(jsonclass3);
					atom2.StoreForSubScene(jsonclass3, false);
				}
				foreach (Atom atom3 in this._atomsInSubScene)
				{
					foreach (FreeControllerV3 freeControllerV2 in atom3.freeControllers)
					{
						freeControllerV2.forceStorePositionRotationAsLocal = false;
					}
				}
				StringBuilder stringBuilder = new StringBuilder(100000);
				jsonclass.ToString(string.Empty, stringBuilder);
				string value = stringBuilder.ToString();
				StreamWriter streamWriter = FileManager.OpenStreamWriter(path);
				streamWriter.Write(value);
				streamWriter.Close();
				bool flag = true;
				if (flag)
				{
					string text = storePath + ".jpg";
					text = text.Replace('/', '\\');
					SuperController.singleton.DoSaveScreenshot(text, null);
				}
				this.SyncStoreButton();
				this.SyncLoadButton();
			}
			catch (Exception arg)
			{
				SuperController.LogError("Exception during StoreSubScene " + arg);
			}
		}
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x0021B4BC File Offset: 0x002198BC
	protected void SyncLoadButton()
	{
		bool flag = this.CheckExistance();
		if (this.LoadSubSceneAction.dynamicButton != null && this.LoadSubSceneAction.dynamicButton.button != null)
		{
			this.LoadSubSceneAction.dynamicButton.button.interactable = flag;
			if (this.LoadSubSceneAction.dynamicButton.buttonText != null)
			{
				if (flag)
				{
					this.LoadSubSceneAction.dynamicButton.buttonText.text = "Load SubScene";
				}
				else
				{
					this.LoadSubSceneAction.dynamicButton.buttonText.text = "Not Ready For Load";
				}
			}
		}
	}

	// Token: 0x06005BC2 RID: 23490 RVA: 0x0021B574 File Offset: 0x00219974
	protected IEnumerator LoadSubSceneCo(JSONClass inputJSON)
	{
		AsyncFlag loadFlag = new AsyncFlag("SubScene " + this.containingAtom.uid + " load");
		SuperController.singleton.ResetSimulation(loadFlag, true);
		bool setUnlistedParamsToDefault = true;
		if (inputJSON["setUnlistedParamsToDefault"] != null)
		{
			setUnlistedParamsToDefault = inputJSON["setUnlistedParamsToDefault"].AsBool;
		}
		this.drawContainedAtomsLinesJSON.RestoreFromJSON(inputJSON, true, true, setUnlistedParamsToDefault);
		this.containingAtom.PreRestoreForSubScene();
		foreach (Atom atom2 in this._atomsInSubScene)
		{
			atom2.isSubSceneRestore = true;
			atom2.PreRestoreForSubScene();
			atom2.isSubSceneRestore = false;
		}
		Dictionary<string, List<Atom>> typeToAtomPool = new Dictionary<string, List<Atom>>();
		Dictionary<string, Atom> existingAtomUidToAtom = new Dictionary<string, Atom>();
		Dictionary<string, Atom> newAtomUidToAtom = new Dictionary<string, Atom>();
		foreach (Atom atom3 in this._atomsInSubScene)
		{
			existingAtomUidToAtom.Add(atom3.uidWithoutSubScenePath, atom3);
			List<Atom> list;
			if (!typeToAtomPool.TryGetValue(atom3.type, out list))
			{
				list = new List<Atom>();
				typeToAtomPool.Add(atom3.type, list);
			}
			list.Add(atom3);
		}
		JSONArray jatoms = inputJSON["atoms"].AsArray;
		IEnumerator enumerator3 = jatoms.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj = enumerator3.Current;
				JSONClass jsonclass = (JSONClass)obj;
				string key = jsonclass["id"];
				string text = jsonclass["type"];
				Atom atom4;
				if (existingAtomUidToAtom.TryGetValue(key, out atom4) && atom4.type == text)
				{
					newAtomUidToAtom.Add(key, atom4);
					List<Atom> list2;
					if (typeToAtomPool.TryGetValue(text, out list2))
					{
						list2.Remove(atom4);
					}
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
		IEnumerator enumerator4 = jatoms.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				object obj2 = enumerator4.Current;
				JSONClass jsonclass2 = (JSONClass)obj2;
				string text2 = jsonclass2["id"];
				string key2 = jsonclass2["type"];
				List<Atom> list3;
				if (!newAtomUidToAtom.ContainsKey(text2) && typeToAtomPool.TryGetValue(key2, out list3) && list3.Count > 0)
				{
					Atom atom5 = list3[0];
					list3.RemoveAt(0);
					newAtomUidToAtom.Add(text2, atom5);
					atom5.SetUID(text2);
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator4 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		foreach (string key3 in typeToAtomPool.Keys)
		{
			List<Atom> list4;
			if (typeToAtomPool.TryGetValue(key3, out list4))
			{
				foreach (Atom atom6 in list4)
				{
					atom6.Remove();
				}
			}
		}
		string subScenePath = this.containingAtom.uid;
		IEnumerator enumerator7 = jatoms.GetEnumerator();
		try
		{
			while (enumerator7.MoveNext())
			{
				object obj3 = enumerator7.Current;
				JSONClass jatom = (JSONClass)obj3;
				string auid = jatom["id"];
				string atype = jatom["type"];
				if (!newAtomUidToAtom.ContainsKey(auid))
				{
					string newauid = subScenePath + "/" + auid;
					SuperController.singleton.ResetSimulation(loadFlag, true);
					yield return SuperController.singleton.AddAtomByType(atype, newauid, false, false, false);
					Atom atom = SuperController.singleton.GetAtomByUid(newauid);
					if (atom != null)
					{
						atom.SelectAtomParent(this.containingAtom);
						newAtomUidToAtom.Add(auid, atom);
					}
					else
					{
						SuperController.LogError("Could not add subscene atom " + newauid);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator7 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		yield return null;
		JSONClass ssjc = inputJSON["subScene"].AsObject;
		IEnumerator enumerator8 = jatoms.GetEnumerator();
		try
		{
			while (enumerator8.MoveNext())
			{
				object obj4 = enumerator8.Current;
				JSONClass jsonclass3 = (JSONClass)obj4;
				string text3 = jsonclass3["id"];
				Atom atom7;
				if (newAtomUidToAtom.TryGetValue(text3, out atom7))
				{
					if (jsonclass3["parentAtom"] != null)
					{
						string text4 = subScenePath + "/" + jsonclass3["parentAtom"];
						Atom atomByUid = SuperController.singleton.GetAtomByUid(text4);
						if (atomByUid == null)
						{
							SuperController.LogError("Could not find subscene atom parent " + text4 + " for subscene atom " + text3);
						}
						else
						{
							atom7.SelectAtomParent(atomByUid);
						}
					}
					else if (setUnlistedParamsToDefault)
					{
						atom7.SelectAtomParent(this.containingAtom);
					}
				}
				else
				{
					SuperController.LogError("Could not find subscene atom " + text3 + " after it should have been created");
				}
			}
		}
		finally
		{
			IDisposable disposable4;
			if ((disposable4 = (enumerator8 as IDisposable)) != null)
			{
				disposable4.Dispose();
			}
		}
		IEnumerator enumerator9 = jatoms.GetEnumerator();
		try
		{
			while (enumerator9.MoveNext())
			{
				object obj5 = enumerator9.Current;
				JSONClass jsonclass4 = (JSONClass)obj5;
				string key4 = jsonclass4["id"];
				Atom atom8;
				if (newAtomUidToAtom.TryGetValue(key4, out atom8))
				{
					atom8.isSubSceneRestore = true;
					atom8.RestoreTransform(jsonclass4, setUnlistedParamsToDefault);
					atom8.isSubSceneRestore = false;
				}
			}
		}
		finally
		{
			IDisposable disposable5;
			if ((disposable5 = (enumerator9 as IDisposable)) != null)
			{
				disposable5.Dispose();
			}
		}
		this.containingAtom.Restore(ssjc, true, true, false, null, false, true, setUnlistedParamsToDefault, true);
		IEnumerator enumerator10 = jatoms.GetEnumerator();
		try
		{
			while (enumerator10.MoveNext())
			{
				object obj6 = enumerator10.Current;
				JSONClass jsonclass5 = (JSONClass)obj6;
				string key5 = jsonclass5["id"];
				Atom atom9;
				if (newAtomUidToAtom.TryGetValue(key5, out atom9))
				{
					atom9.isSubSceneRestore = true;
					atom9.Restore(jsonclass5, true, true, true, null, false, true, setUnlistedParamsToDefault, false);
					atom9.isSubSceneRestore = false;
				}
			}
		}
		finally
		{
			IDisposable disposable6;
			if ((disposable6 = (enumerator10 as IDisposable)) != null)
			{
				disposable6.Dispose();
			}
		}
		this.containingAtom.LateRestore(ssjc, true, true, false, true, setUnlistedParamsToDefault, true);
		IEnumerator enumerator11 = jatoms.GetEnumerator();
		try
		{
			while (enumerator11.MoveNext())
			{
				object obj7 = enumerator11.Current;
				JSONClass jsonclass6 = (JSONClass)obj7;
				string key6 = jsonclass6["id"];
				Atom atom10;
				if (newAtomUidToAtom.TryGetValue(key6, out atom10))
				{
					atom10.isSubSceneRestore = true;
					atom10.LateRestore(jsonclass6, true, true, true, true, setUnlistedParamsToDefault, false);
					atom10.isSubSceneRestore = false;
				}
			}
		}
		finally
		{
			IDisposable disposable7;
			if ((disposable7 = (enumerator11 as IDisposable)) != null)
			{
				disposable7.Dispose();
			}
		}
		foreach (Atom atom11 in this._atomsInSubScene)
		{
			atom11.isSubSceneRestore = true;
			atom11.PostRestore();
			atom11.isSubSceneRestore = false;
		}
		yield return null;
		loadFlag.Raise();
		SuperController.singleton.NotifySubSceneLoad(this);
		yield break;
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x0021B598 File Offset: 0x00219998
	public void LoadSubScene()
	{
		if (this.CheckExistance())
		{
			JSONClass jsonclass = null;
			try
			{
				string storePath = this.GetStorePath(true);
				string path = storePath + ".json";
				FileManager.PushLoadDirFromFilePath(path, false);
				using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(path, true))
				{
					string aJSON = fileEntryStreamReader.ReadToEnd();
					JSONNode jsonnode = JSON.Parse(aJSON);
					jsonclass = jsonnode.AsObject;
				}
			}
			catch (Exception arg)
			{
				SuperController.LogError("Exception during LoadSubScene " + arg);
			}
			if (jsonclass != null)
			{
				SuperController.singleton.StartCoroutine(this.LoadSubSceneCo(jsonclass));
			}
		}
	}

	// Token: 0x17000D72 RID: 3442
	// (get) Token: 0x06005BC4 RID: 23492 RVA: 0x0021B658 File Offset: 0x00219A58
	public IEnumerable<Atom> atomsInSubScene
	{
		get
		{
			return this._atomsInSubScene;
		}
	}

	// Token: 0x06005BC5 RID: 23493 RVA: 0x0021B660 File Offset: 0x00219A60
	protected bool IsAtomInThisSubScene(Atom atom)
	{
		Atom parentAtom = atom.parentAtom;
		while (parentAtom != null)
		{
			if (parentAtom == this.containingAtom)
			{
				return true;
			}
			if (parentAtom.isSubSceneType)
			{
				return false;
			}
			parentAtom = parentAtom.parentAtom;
		}
		return false;
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x0021B6B0 File Offset: 0x00219AB0
	protected void OnAtomParentChanged(Atom atom, Atom newParent)
	{
		bool flag = this.IsAtomInThisSubScene(atom);
		if (flag)
		{
			this.AddAtomToSubScene(atom);
		}
		else
		{
			this.RemoveAtomFromSubScene(atom);
		}
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x0021B6E0 File Offset: 0x00219AE0
	protected void AddAtomToSubScene(Atom atom)
	{
		if (!this._atomsInSubScene.Contains(atom))
		{
			if (!atom.isSubSceneType)
			{
				foreach (Atom atom2 in atom.GetChildren())
				{
					this.AddAtomToSubScene(atom2);
				}
			}
			this._atomsInSubScene.Add(atom);
			foreach (MotionAnimationControl mac in atom.motionAnimationControls)
			{
				this.motionAnimationMaster.RegisterAnimationControl(mac);
			}
			if (SuperController.singleton != null)
			{
				SuperController.singleton.AtomSubSceneChanged(atom, this);
			}
		}
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x0021B7B4 File Offset: 0x00219BB4
	protected void RemoveAtomFromSubScene(Atom atom)
	{
		if (this._atomsInSubScene.Contains(atom))
		{
			if (!atom.isSubSceneType)
			{
				foreach (Atom atom2 in atom.GetChildren())
				{
					this.RemoveAtomFromSubScene(atom2);
				}
			}
			this._atomsInSubScene.Remove(atom);
			foreach (MotionAnimationControl mac in atom.motionAnimationControls)
			{
				this.motionAnimationMaster.DeregisterAnimationControl(mac);
			}
			if (SuperController.singleton != null && atom.containingSubScene == this)
			{
				SuperController.singleton.AtomSubSceneChanged(atom, null);
			}
		}
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x0021B898 File Offset: 0x00219C98
	protected void Init()
	{
		if (this.lineMaterial != null)
		{
			this.lineDrawer = new LineDrawer(this.lineMaterial);
		}
		this.browsePathJSON = new JSONStorableUrl("browsePath", string.Empty, new JSONStorableString.SetStringCallback(this.SyncBrowsePath), "json", this.storeRoot, true);
		this.browsePathJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		this.browsePathJSON.allowFullComputerBrowse = false;
		this.browsePathJSON.allowBrowseAboveSuggestedPath = false;
		this.browsePathJSON.hideExtension = false;
		this.browsePathJSON.showDirs = true;
		this.browsePathJSON.isRestorable = false;
		this.browsePathJSON.isStorable = false;
		base.RegisterUrl(this.browsePathJSON);
		this.autoSetSubSceneUIDToSignatureOnBrowseLoadJSON = new JSONStorableBool("autoSetSubSceneUIDToSignatureOnBrowseLoad", true);
		this.autoSetSubSceneUIDToSignatureOnBrowseLoadJSON.isRestorable = false;
		this.autoSetSubSceneUIDToSignatureOnBrowseLoadJSON.isStorable = false;
		base.RegisterBool(this.autoSetSubSceneUIDToSignatureOnBrowseLoadJSON);
		this.packageUidJSON = new JSONStorableString("packageUid", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPackageUid));
		this.packageUidJSON.isRestorable = false;
		this.packageUidJSON.isStorable = false;
		this.ClearPackageUidAction = new JSONStorableAction("ClearPackageUid", new JSONStorableAction.ActionCallback(this.ClearPackageUid));
		base.RegisterAction(this.ClearPackageUidAction);
		this.creatorNameJSON = new JSONStorableString("creatorName", UserPreferences.singleton.creatorName, new JSONStorableString.SetStringCallback(this.SyncCreatorName));
		this.creatorNameJSON.isRestorable = false;
		this.creatorNameJSON.isStorable = false;
		this.creatorNameJSON.enableOnChange = true;
		base.RegisterString(this.creatorNameJSON);
		this.SetToYourCreatorNameAction = new JSONStorableAction("SetToYourCreatorName", new JSONStorableAction.ActionCallback(this.SetToYourCreatorName));
		base.RegisterAction(this.SetToYourCreatorNameAction);
		this.storedCreatorNameJSON = new JSONStorableString("storedCreatorName", string.Empty);
		this.signatureJSON = new JSONStorableString("signature", string.Empty, new JSONStorableString.SetStringCallback(this.SyncSignature));
		this.signatureJSON.isRestorable = false;
		this.signatureJSON.isStorable = false;
		this.signatureJSON.enableOnChange = true;
		base.RegisterString(this.signatureJSON);
		this.storeNameJSON = new JSONStorableString("storeName", string.Empty, new JSONStorableString.SetStringCallback(this.SyncStoreName));
		this.storeNameJSON.isRestorable = false;
		this.storeNameJSON.isStorable = false;
		this.storeNameJSON.enableOnChange = true;
		base.RegisterString(this.storeNameJSON);
		this.storePathJSON = new JSONStorableUrl("storePath", string.Empty, new JSONStorableString.SetStringCallback(this.SyncStorePath));
		this.storePathJSON.storeType = JSONStorableParam.StoreType.Full;
		base.RegisterUrl(this.storePathJSON);
		this.StoreSubSceneAction = new JSONStorableAction("StoreSubScene", new JSONStorableAction.ActionCallback(this.StoreSubScene));
		base.RegisterAction(this.StoreSubSceneAction);
		this.LoadSubSceneAction = new JSONStorableAction("LoadSubScene", new JSONStorableAction.ActionCallback(this.LoadSubScene));
		base.RegisterAction(this.LoadSubSceneAction);
		this.loadSubSceneWithPathUrlJSON = new JSONStorableUrl("loadSubSceneWithPathUrl", string.Empty, "json", this.storeRoot);
		this.loadSubSceneWithPathUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		this.loadSubSceneWithPathUrlJSON.allowFullComputerBrowse = false;
		this.loadSubSceneWithPathUrlJSON.allowBrowseAboveSuggestedPath = false;
		this.loadSubSceneWithPathUrlJSON.hideExtension = false;
		this.loadSubSceneWithPathUrlJSON.showDirs = true;
		this.loadSubSceneWithPathJSON = new JSONStorableActionPresetFilePath("LoadSubSceneWithPath", new JSONStorableActionPresetFilePath.PresetFilePathActionCallback(this.LoadSubSceneWithPath), this.loadSubSceneWithPathUrlJSON);
		base.RegisterPresetFilePathAction(this.loadSubSceneWithPathJSON);
		this.ClearSubSceneAction = new JSONStorableAction("ClearSubScene", new JSONStorableAction.ActionCallback(this.ClearSubScene));
		base.RegisterAction(this.ClearSubSceneAction);
		this.UnparentAllAtomsAction = new JSONStorableAction("UnparentAllAtoms", new JSONStorableAction.ActionCallback(this.UnparentAllAtoms));
		base.RegisterAction(this.UnparentAllAtomsAction);
		this.loadOnRestoreFromOtherSubSceneJSON = new JSONStorableBool("loadOnRestoreFromOtherSubscene", this._loadOnRestoreFromOtherSubScene, new JSONStorableBool.SetBoolCallback(this.SyncLoadOnRestoreFromOtherSubScene));
		base.RegisterBool(this.loadOnRestoreFromOtherSubSceneJSON);
		this.AddLooseAtomsToSubSceneAction = new JSONStorableAction("AddLooseAtomsToSubScene", new JSONStorableAction.ActionCallback(this.AddLooseAtomsToSubScene));
		base.RegisterAction(this.AddLooseAtomsToSubSceneAction);
		this.IsolateEditSubSceneAction = new JSONStorableAction("IsolateEditSubScene", new JSONStorableAction.ActionCallback(this.IsolateEditSubScene));
		base.RegisterAction(this.IsolateEditSubSceneAction);
		this.drawContainedAtomsLinesJSON = new JSONStorableBool("drawContainedAtomsLines", this._drawContainedAtomsLines, new JSONStorableBool.SetBoolCallback(this.SyncDrawContainedAtomsLines));
		base.RegisterBool(this.drawContainedAtomsLinesJSON);
		this._atomsInSubScene = new HashSet<Atom>();
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomParentChangedHandlers = (SuperController.OnAtomParentChanged)Delegate.Combine(singleton.onAtomParentChangedHandlers, new SuperController.OnAtomParentChanged(this.OnAtomParentChanged));
		}
	}

	// Token: 0x06005BCA RID: 23498 RVA: 0x0021BD88 File Offset: 0x0021A188
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			SubSceneUI componentInChildren = t.GetComponentInChildren<SubSceneUI>(true);
			if (componentInChildren != null)
			{
				this.browsePathJSON.RegisterFileBrowseButton(componentInChildren.beginBrowseButton, isAlt);
				this.autoSetSubSceneUIDToSignatureOnBrowseLoadJSON.RegisterToggle(componentInChildren.autoSetSubSceneUIDToSignatureOnBrowseLoadToggle, isAlt);
				this.packageUidJSON.RegisterText(componentInChildren.packageUidText, isAlt);
				this.ClearPackageUidAction.RegisterButton(componentInChildren.clearPackageUidButton, isAlt);
				this.creatorNameJSON.RegisterInputField(componentInChildren.creatorNameInputField, isAlt);
				this.SetToYourCreatorNameAction.RegisterButton(componentInChildren.setToYourCreatorNameButton, isAlt);
				this.storedCreatorNameJSON.RegisterText(componentInChildren.storedCreatorNameText, isAlt);
				this.signatureJSON.RegisterInputField(componentInChildren.signatureInputField, isAlt);
				this.storeNameJSON.RegisterInputField(componentInChildren.storeNameInputField, isAlt);
				this.StoreSubSceneAction.RegisterButton(componentInChildren.storeSubSceneButton, isAlt);
				this.LoadSubSceneAction.RegisterButton(componentInChildren.loadSubSceneButton, isAlt);
				this.ClearSubSceneAction.RegisterButton(componentInChildren.clearSubSceneButton, isAlt);
				this.UnparentAllAtomsAction.RegisterButton(componentInChildren.unparentAllAtomsButton, isAlt);
				this.loadOnRestoreFromOtherSubSceneJSON.RegisterToggle(componentInChildren.loadOnRestoreFromOtherSubSceneToggle, isAlt);
				this.AddLooseAtomsToSubSceneAction.RegisterButton(componentInChildren.addLooseAtomsToSubSceneButton, isAlt);
				this.IsolateEditSubSceneAction.RegisterButton(componentInChildren.isolateEditSubSceneButton, isAlt);
				this.drawContainedAtomsLinesJSON.RegisterToggle(componentInChildren.drawContainedAtomsLinesToggle, isAlt);
				this.SyncLoadButton();
				this.SyncStoreButton();
			}
		}
	}

	// Token: 0x06005BCB RID: 23499 RVA: 0x0021BEF3 File Offset: 0x0021A2F3
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

	// Token: 0x06005BCC RID: 23500 RVA: 0x0021BF18 File Offset: 0x0021A318
	protected void Update()
	{
		if (this._drawContainedAtomsLines && this.lineMaterial != null && !this.containingAtom.mainController.hidden)
		{
			int count = this._atomsInSubScene.Count;
			if (this.lineDrawer == null || this.lineDrawer.numLines != count)
			{
				this.lineDrawer = new LineDrawer(count, this.lineMaterial);
			}
			int num = 0;
			foreach (Atom atom in this._atomsInSubScene)
			{
				this.lineDrawer.SetLinePoints(num, base.transform.position, atom.mainController.transform.position);
				num++;
			}
			this.lineDrawer.Draw(base.gameObject.layer);
		}
	}

	// Token: 0x06005BCD RID: 23501 RVA: 0x0021C01C File Offset: 0x0021A41C
	public override void PreRemove()
	{
		List<Atom> list = new List<Atom>(this._atomsInSubScene);
		foreach (Atom atom in list)
		{
			SuperController.singleton.RemoveAtom(atom);
		}
	}

	// Token: 0x06005BCE RID: 23502 RVA: 0x0021C084 File Offset: 0x0021A484
	protected void OnDestroy()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomParentChangedHandlers = (SuperController.OnAtomParentChanged)Delegate.Remove(singleton.onAtomParentChangedHandlers, new SuperController.OnAtomParentChanged(this.OnAtomParentChanged));
			if (this.isIsolateEditing)
			{
				SuperController.singleton.EndIsolateEditSubScene();
			}
		}
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x0021C0DC File Offset: 0x0021A4DC
	[CompilerGenerated]
	private static int <StoreSubScene>m__0(Atom a1, Atom a2)
	{
		return a1.uid.CompareTo(a2.uid);
	}

	// Token: 0x04004BA0 RID: 19360
	public MotionAnimationMaster motionAnimationMaster;

	// Token: 0x04004BA1 RID: 19361
	public Material lineMaterial;

	// Token: 0x04004BA2 RID: 19362
	protected LineDrawer lineDrawer;

	// Token: 0x04004BA3 RID: 19363
	protected bool _drawContainedAtomsLines;

	// Token: 0x04004BA4 RID: 19364
	protected JSONStorableBool drawContainedAtomsLinesJSON;

	// Token: 0x04004BA5 RID: 19365
	protected Dictionary<string, string> storeRootMap = new Dictionary<string, string>
	{
		{
			"Standard",
			"Custom/SubScene/"
		},
		{
			"Wizard Set",
			"Custom/Wizard/Sets/"
		},
		{
			"Wizard Scenario",
			"Custom/Wizard/Scenarios/"
		},
		{
			"Wizard Glue",
			"Custom/Wizard/Glue/"
		}
	};

	// Token: 0x04004BA6 RID: 19366
	protected string storeRoot = "Custom/SubScene/";

	// Token: 0x04004BA7 RID: 19367
	protected JSONStorableUrl browsePathJSON;

	// Token: 0x04004BA8 RID: 19368
	protected JSONStorableBool autoSetSubSceneUIDToSignatureOnBrowseLoadJSON;

	// Token: 0x04004BA9 RID: 19369
	protected JSONStorableActionPresetFilePath loadSubSceneWithPathJSON;

	// Token: 0x04004BAA RID: 19370
	protected JSONStorableUrl loadSubSceneWithPathUrlJSON;

	// Token: 0x04004BAB RID: 19371
	protected JSONStorableUrl storePathJSON;

	// Token: 0x04004BAC RID: 19372
	protected JSONStorableString packageUidJSON;

	// Token: 0x04004BAD RID: 19373
	protected JSONStorableAction ClearPackageUidAction;

	// Token: 0x04004BAE RID: 19374
	protected JSONStorableString creatorNameJSON;

	// Token: 0x04004BAF RID: 19375
	protected JSONStorableString storedCreatorNameJSON;

	// Token: 0x04004BB0 RID: 19376
	protected JSONStorableAction SetToYourCreatorNameAction;

	// Token: 0x04004BB1 RID: 19377
	protected JSONStorableString signatureJSON;

	// Token: 0x04004BB2 RID: 19378
	protected JSONStorableString storeNameJSON;

	// Token: 0x04004BB3 RID: 19379
	protected JSONStorableAction ClearSubSceneAction;

	// Token: 0x04004BB4 RID: 19380
	protected JSONStorableAction UnparentAllAtomsAction;

	// Token: 0x04004BB5 RID: 19381
	protected bool _loadOnRestoreFromOtherSubScene = true;

	// Token: 0x04004BB6 RID: 19382
	protected JSONStorableBool loadOnRestoreFromOtherSubSceneJSON;

	// Token: 0x04004BB7 RID: 19383
	protected JSONStorableAction AddLooseAtomsToSubSceneAction;

	// Token: 0x04004BB8 RID: 19384
	protected JSONStorableAction IsolateEditSubSceneAction;

	// Token: 0x04004BB9 RID: 19385
	public bool isIsolateEditing;

	// Token: 0x04004BBA RID: 19386
	protected JSONStorableAction StoreSubSceneAction;

	// Token: 0x04004BBB RID: 19387
	protected JSONStorableAction LoadSubSceneAction;

	// Token: 0x04004BBC RID: 19388
	protected HashSet<Atom> _atomsInSubScene;

	// Token: 0x04004BBD RID: 19389
	[CompilerGenerated]
	private static Comparison<Atom> <>f__am$cache0;

	// Token: 0x02001009 RID: 4105
	[CompilerGenerated]
	private sealed class <LoadSubSceneCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600769A RID: 30362 RVA: 0x0021C0EF File Offset: 0x0021A4EF
		[DebuggerHidden]
		public <LoadSubSceneCo>c__Iterator0()
		{
		}

		// Token: 0x0600769B RID: 30363 RVA: 0x0021C0F8 File Offset: 0x0021A4F8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			bool flag = false;
			switch (num)
			{
			case 0U:
				loadFlag = new AsyncFlag("SubScene " + this.containingAtom.uid + " load");
				SuperController.singleton.ResetSimulation(loadFlag, true);
				setUnlistedParamsToDefault = true;
				if (inputJSON["setUnlistedParamsToDefault"] != null)
				{
					setUnlistedParamsToDefault = inputJSON["setUnlistedParamsToDefault"].AsBool;
				}
				this.drawContainedAtomsLinesJSON.RestoreFromJSON(inputJSON, true, true, setUnlistedParamsToDefault);
				this.containingAtom.PreRestoreForSubScene();
				enumerator = this._atomsInSubScene.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Atom atom2 = enumerator.Current;
						atom2.isSubSceneRestore = true;
						atom2.PreRestoreForSubScene();
						atom2.isSubSceneRestore = false;
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				typeToAtomPool = new Dictionary<string, List<Atom>>();
				existingAtomUidToAtom = new Dictionary<string, Atom>();
				newAtomUidToAtom = new Dictionary<string, Atom>();
				enumerator2 = this._atomsInSubScene.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Atom atom3 = enumerator2.Current;
						existingAtomUidToAtom.Add(atom3.uidWithoutSubScenePath, atom3);
						List<Atom> list;
						if (!typeToAtomPool.TryGetValue(atom3.type, out list))
						{
							list = new List<Atom>();
							typeToAtomPool.Add(atom3.type, list);
						}
						list.Add(atom3);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
				jatoms = inputJSON["atoms"].AsArray;
				enumerator3 = jatoms.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						JSONClass jsonclass = (JSONClass)enumerator3.Current;
						string key = jsonclass["id"];
						string text = jsonclass["type"];
						Atom atom4;
						if (existingAtomUidToAtom.TryGetValue(key, out atom4) && atom4.type == text)
						{
							newAtomUidToAtom.Add(key, atom4);
							List<Atom> list2;
							if (typeToAtomPool.TryGetValue(text, out list2))
							{
								list2.Remove(atom4);
							}
						}
					}
				}
				finally
				{
					if ((disposable = (enumerator3 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				enumerator4 = jatoms.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						JSONClass jsonclass2 = (JSONClass)enumerator4.Current;
						string text2 = jsonclass2["id"];
						string key2 = jsonclass2["type"];
						List<Atom> list3;
						if (!newAtomUidToAtom.ContainsKey(text2) && typeToAtomPool.TryGetValue(key2, out list3) && list3.Count > 0)
						{
							Atom atom5 = list3[0];
							list3.RemoveAt(0);
							newAtomUidToAtom.Add(text2, atom5);
							atom5.SetUID(text2);
						}
					}
				}
				finally
				{
					if ((disposable2 = (enumerator4 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				enumerator5 = typeToAtomPool.Keys.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						string key3 = enumerator5.Current;
						List<Atom> list4;
						if (typeToAtomPool.TryGetValue(key3, out list4))
						{
							foreach (Atom atom6 in list4)
							{
								atom6.Remove();
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator5).Dispose();
				}
				subScenePath = this.containingAtom.uid;
				enumerator7 = jatoms.GetEnumerator();
				num = 4294967293U;
				break;
			case 1U:
				break;
			case 2U:
				ssjc = inputJSON["subScene"].AsObject;
				enumerator8 = jatoms.GetEnumerator();
				try
				{
					while (enumerator8.MoveNext())
					{
						JSONClass jsonclass3 = (JSONClass)enumerator8.Current;
						string text3 = jsonclass3["id"];
						Atom atom7;
						if (newAtomUidToAtom.TryGetValue(text3, out atom7))
						{
							if (jsonclass3["parentAtom"] != null)
							{
								string text4 = subScenePath + "/" + jsonclass3["parentAtom"];
								Atom atomByUid = SuperController.singleton.GetAtomByUid(text4);
								if (atomByUid == null)
								{
									SuperController.LogError("Could not find subscene atom parent " + text4 + " for subscene atom " + text3);
								}
								else
								{
									atom7.SelectAtomParent(atomByUid);
								}
							}
							else if (setUnlistedParamsToDefault)
							{
								atom7.SelectAtomParent(this.containingAtom);
							}
						}
						else
						{
							SuperController.LogError("Could not find subscene atom " + text3 + " after it should have been created");
						}
					}
				}
				finally
				{
					if ((disposable4 = (enumerator8 as IDisposable)) != null)
					{
						disposable4.Dispose();
					}
				}
				enumerator9 = jatoms.GetEnumerator();
				try
				{
					while (enumerator9.MoveNext())
					{
						JSONClass jsonclass4 = (JSONClass)enumerator9.Current;
						string key4 = jsonclass4["id"];
						Atom atom8;
						if (newAtomUidToAtom.TryGetValue(key4, out atom8))
						{
							atom8.isSubSceneRestore = true;
							atom8.RestoreTransform(jsonclass4, setUnlistedParamsToDefault);
							atom8.isSubSceneRestore = false;
						}
					}
				}
				finally
				{
					if ((disposable5 = (enumerator9 as IDisposable)) != null)
					{
						disposable5.Dispose();
					}
				}
				this.containingAtom.Restore(ssjc, true, true, false, null, false, true, setUnlistedParamsToDefault, true);
				enumerator10 = jatoms.GetEnumerator();
				try
				{
					while (enumerator10.MoveNext())
					{
						JSONClass jsonclass5 = (JSONClass)enumerator10.Current;
						string key5 = jsonclass5["id"];
						Atom atom9;
						if (newAtomUidToAtom.TryGetValue(key5, out atom9))
						{
							atom9.isSubSceneRestore = true;
							atom9.Restore(jsonclass5, true, true, true, null, false, true, setUnlistedParamsToDefault, false);
							atom9.isSubSceneRestore = false;
						}
					}
				}
				finally
				{
					if ((disposable6 = (enumerator10 as IDisposable)) != null)
					{
						disposable6.Dispose();
					}
				}
				this.containingAtom.LateRestore(ssjc, true, true, false, true, setUnlistedParamsToDefault, true);
				enumerator11 = jatoms.GetEnumerator();
				try
				{
					while (enumerator11.MoveNext())
					{
						JSONClass jsonclass6 = (JSONClass)enumerator11.Current;
						string key6 = jsonclass6["id"];
						Atom atom10;
						if (newAtomUidToAtom.TryGetValue(key6, out atom10))
						{
							atom10.isSubSceneRestore = true;
							atom10.LateRestore(jsonclass6, true, true, true, true, setUnlistedParamsToDefault, false);
							atom10.isSubSceneRestore = false;
						}
					}
				}
				finally
				{
					if ((disposable7 = (enumerator11 as IDisposable)) != null)
					{
						disposable7.Dispose();
					}
				}
				enumerator12 = this._atomsInSubScene.GetEnumerator();
				try
				{
					while (enumerator12.MoveNext())
					{
						Atom atom11 = enumerator12.Current;
						atom11.isSubSceneRestore = true;
						atom11.PostRestore();
						atom11.isSubSceneRestore = false;
					}
				}
				finally
				{
					((IDisposable)enumerator12).Dispose();
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
			case 3U:
				loadFlag.Raise();
				SuperController.singleton.NotifySubSceneLoad(this);
				this.$PC = -1;
				return false;
			default:
				return false;
			}
			try
			{
				switch (num)
				{
				case 1U:
					atom = SuperController.singleton.GetAtomByUid(newauid);
					if (atom != null)
					{
						atom.SelectAtomParent(this.containingAtom);
						newAtomUidToAtom.Add(auid, atom);
					}
					else
					{
						SuperController.LogError("Could not add subscene atom " + newauid);
					}
					break;
				}
				while (enumerator7.MoveNext())
				{
					jatom = (JSONClass)enumerator7.Current;
					auid = jatom["id"];
					atype = jatom["type"];
					if (!newAtomUidToAtom.ContainsKey(auid))
					{
						newauid = subScenePath + "/" + auid;
						SuperController.singleton.ResetSimulation(loadFlag, true);
						this.$current = SuperController.singleton.AddAtomByType(atype, newauid, false, false, false);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
			}
			finally
			{
				if (!flag)
				{
					if ((disposable3 = (enumerator7 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
			}
			this.$current = null;
			if (!this.$disposing)
			{
				this.$PC = 2;
			}
			return true;
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x0600769C RID: 30364 RVA: 0x0021CCB4 File Offset: 0x0021B0B4
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x0600769D RID: 30365 RVA: 0x0021CCBC File Offset: 0x0021B0BC
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600769E RID: 30366 RVA: 0x0021CCC4 File Offset: 0x0021B0C4
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
					if ((disposable3 = (enumerator7 as IDisposable)) != null)
					{
						disposable3.Dispose();
					}
				}
				break;
			}
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x0021CD3C File Offset: 0x0021B13C
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A63 RID: 27235
		internal AsyncFlag <loadFlag>__0;

		// Token: 0x04006A64 RID: 27236
		internal bool <setUnlistedParamsToDefault>__0;

		// Token: 0x04006A65 RID: 27237
		internal JSONClass inputJSON;

		// Token: 0x04006A66 RID: 27238
		internal HashSet<Atom>.Enumerator $locvar0;

		// Token: 0x04006A67 RID: 27239
		internal Dictionary<string, List<Atom>> <typeToAtomPool>__0;

		// Token: 0x04006A68 RID: 27240
		internal Dictionary<string, Atom> <existingAtomUidToAtom>__0;

		// Token: 0x04006A69 RID: 27241
		internal Dictionary<string, Atom> <newAtomUidToAtom>__0;

		// Token: 0x04006A6A RID: 27242
		internal HashSet<Atom>.Enumerator $locvar1;

		// Token: 0x04006A6B RID: 27243
		internal JSONArray <jatoms>__0;

		// Token: 0x04006A6C RID: 27244
		internal IEnumerator $locvar2;

		// Token: 0x04006A6D RID: 27245
		internal IDisposable $locvar3;

		// Token: 0x04006A6E RID: 27246
		internal IEnumerator $locvar4;

		// Token: 0x04006A6F RID: 27247
		internal IDisposable $locvar5;

		// Token: 0x04006A70 RID: 27248
		internal Dictionary<string, List<Atom>>.KeyCollection.Enumerator $locvar6;

		// Token: 0x04006A71 RID: 27249
		internal string <subScenePath>__0;

		// Token: 0x04006A72 RID: 27250
		internal IEnumerator $locvar8;

		// Token: 0x04006A73 RID: 27251
		internal JSONClass <jatom>__1;

		// Token: 0x04006A74 RID: 27252
		internal IDisposable $locvar9;

		// Token: 0x04006A75 RID: 27253
		internal string <auid>__2;

		// Token: 0x04006A76 RID: 27254
		internal string <atype>__2;

		// Token: 0x04006A77 RID: 27255
		internal string <newauid>__3;

		// Token: 0x04006A78 RID: 27256
		internal Atom <atom>__3;

		// Token: 0x04006A79 RID: 27257
		internal JSONClass <ssjc>__0;

		// Token: 0x04006A7A RID: 27258
		internal IEnumerator $locvarA;

		// Token: 0x04006A7B RID: 27259
		internal IDisposable $locvarB;

		// Token: 0x04006A7C RID: 27260
		internal IEnumerator $locvarC;

		// Token: 0x04006A7D RID: 27261
		internal IDisposable $locvarD;

		// Token: 0x04006A7E RID: 27262
		internal IEnumerator $locvarE;

		// Token: 0x04006A7F RID: 27263
		internal IDisposable $locvarF;

		// Token: 0x04006A80 RID: 27264
		internal IEnumerator $locvar10;

		// Token: 0x04006A81 RID: 27265
		internal IDisposable $locvar11;

		// Token: 0x04006A82 RID: 27266
		internal HashSet<Atom>.Enumerator $locvar12;

		// Token: 0x04006A83 RID: 27267
		internal SubScene $this;

		// Token: 0x04006A84 RID: 27268
		internal object $current;

		// Token: 0x04006A85 RID: 27269
		internal bool $disposing;

		// Token: 0x04006A86 RID: 27270
		internal int $PC;
	}
}
