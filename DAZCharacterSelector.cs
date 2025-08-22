using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using GPUTools.Physics.Scripts.Behaviours;
using MeshVR;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AB2 RID: 2738
[ExecuteInEditMode]
public class DAZCharacterSelector : JSONStorable
{
	// Token: 0x060047DE RID: 18398 RVA: 0x001600B8 File Offset: 0x0015E4B8
	public DAZCharacterSelector()
	{
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x0016012E File Offset: 0x0015E52E
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x00160138 File Offset: 0x0015E538
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		JSONArray jsonarray = new JSONArray();
		if (includeAppearance || forceStore)
		{
			if (base.isPresetStore)
			{
				if (forceStore)
				{
					json["character"] = this.selectedCharacter.displayName;
					this.needsStore = true;
				}
			}
			else
			{
				json["character"] = this.selectedCharacter.displayName;
				this.needsStore = true;
			}
			bool flag = !base.isPresetStore;
			for (int i = 0; i < this.clothingItems.Length; i++)
			{
				DAZClothingItem dazclothingItem = this.clothingItems[i];
				if (((!base.isPresetStore || forceStore) && dazclothingItem.active) || (base.isPresetStore && dazclothingItem.active != dazclothingItem.startActive))
				{
					if (dazclothingItem.packageUid != null && dazclothingItem.packageUid != string.Empty && SuperController.singleton != null && SuperController.singleton.packageMode)
					{
						SuperController.singleton.AddVarPackageRefToVacPackage(dazclothingItem.packageUid);
					}
					JSONClass jsonclass = new JSONClass();
					jsonarray.Add(jsonclass);
					jsonclass["id"] = dazclothingItem.uid;
					if (dazclothingItem.internalUid != null && dazclothingItem.internalUid != string.Empty)
					{
						jsonclass["internalId"] = dazclothingItem.internalUid;
					}
					jsonclass["enabled"].AsBool = dazclothingItem.active;
					flag = true;
				}
			}
			if (flag || forceStore)
			{
				json["clothing"] = jsonarray;
				this.needsStore = true;
			}
			jsonarray = new JSONArray();
			bool flag2 = !base.isPresetStore;
			for (int j = 0; j < this.hairItems.Length; j++)
			{
				DAZHairGroup dazhairGroup = this.hairItems[j];
				if (((!base.isPresetStore || forceStore) && dazhairGroup.active) || (base.isPresetStore && dazhairGroup.active != dazhairGroup.startActive))
				{
					if (dazhairGroup.packageUid != null && dazhairGroup.packageUid != string.Empty && SuperController.singleton != null && SuperController.singleton.packageMode)
					{
						SuperController.singleton.AddVarPackageRefToVacPackage(dazhairGroup.packageUid);
					}
					JSONClass jsonclass2 = new JSONClass();
					jsonarray.Add(jsonclass2);
					jsonclass2["id"] = dazhairGroup.uid;
					if (dazhairGroup.internalUid != null && dazhairGroup.internalUid != string.Empty)
					{
						jsonclass2["internalId"] = dazhairGroup.internalUid;
					}
					jsonclass2["enabled"].AsBool = dazhairGroup.active;
					flag2 = true;
				}
			}
			if (flag2 || forceStore)
			{
				json["hair"] = jsonarray;
				this.needsStore = true;
			}
		}
		jsonarray = new JSONArray();
		bool flag3 = !base.isPresetStore;
		if (this.morphsControlUI != null)
		{
			List<DAZMorph> morphs = this.morphsControlUI.GetMorphs();
			if (morphs != null)
			{
				foreach (DAZMorph dazmorph in morphs)
				{
					bool isPoseControl = dazmorph.isPoseControl;
					if ((includePhysical && isPoseControl) || (includeAppearance && !isPoseControl))
					{
						JSONClass jsonclass3 = new JSONClass();
						if (dazmorph.StoreJSON(jsonclass3, false))
						{
							if (dazmorph.isRuntime && SuperController.singleton != null && SuperController.singleton.packageMode)
							{
								string text = dazmorph.metaLoadPath;
								text = Regex.Replace(text, ".*/Import/", "Import/");
								SuperController.singleton.AddFileToPackage(dazmorph.metaLoadPath, text);
								text = dazmorph.deltasLoadPath;
								text = Regex.Replace(text, ".*/Import/", "Import/");
								SuperController.singleton.AddFileToPackage(dazmorph.deltasLoadPath, text);
							}
							flag3 = true;
							jsonarray.Add(jsonclass3);
						}
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("morphsControl UI not set for " + base.name + " character " + this.selectedCharacter.displayName);
		}
		if (flag3 || forceStore)
		{
			json["morphs"] = jsonarray;
			this.needsStore = true;
		}
		if (((this._gender == DAZCharacterSelector.Gender.Female && this._useMaleMorphsOnFemale) || (this._gender == DAZCharacterSelector.Gender.Male && this._useFemaleMorphsOnMale)) && this.morphsControlUIOtherGender != null)
		{
			jsonarray = new JSONArray();
			bool flag4 = false;
			List<DAZMorph> morphs2 = this.morphsControlUIOtherGender.GetMorphs();
			if (morphs2 != null)
			{
				foreach (DAZMorph dazmorph2 in morphs2)
				{
					bool isPoseControl2 = dazmorph2.isPoseControl;
					if ((includePhysical && isPoseControl2) || (includeAppearance && !isPoseControl2))
					{
						JSONClass jsonclass4 = new JSONClass();
						if (dazmorph2.StoreJSON(jsonclass4, false))
						{
							if (dazmorph2.isRuntime && SuperController.singleton != null && SuperController.singleton.packageMode)
							{
								string text2 = dazmorph2.metaLoadPath;
								text2 = Regex.Replace(text2, ".*/Import/", "Import/");
								SuperController.singleton.AddFileToPackage(dazmorph2.metaLoadPath, text2);
								text2 = dazmorph2.deltasLoadPath;
								text2 = Regex.Replace(text2, ".*/Import/", "Import/");
								SuperController.singleton.AddFileToPackage(dazmorph2.deltasLoadPath, text2);
							}
							flag4 = true;
							jsonarray.Add(jsonclass4);
						}
					}
				}
			}
			if (flag4 || forceStore)
			{
				json["morphsOtherGender"] = jsonarray;
				this.needsStore = true;
			}
		}
		return json;
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x00160794 File Offset: 0x0015EB94
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		this.Init(false);
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		this.insideRestore = true;
		if (!base.appearanceLocked && restoreAppearance)
		{
			if (!base.IsCustomAppearanceParamLocked("character"))
			{
				if (jc["character"] != null)
				{
					string text = jc["character"];
					if (text == string.Empty)
					{
						this.SelectCharacterByName(this.startingCharacterName, true);
					}
					else
					{
						this.SelectCharacterByName(text, true);
					}
				}
				else if (setMissingToDefault)
				{
					this.SelectCharacterByName(this.startingCharacterName, true);
				}
			}
			if (!base.IsCustomAppearanceParamLocked("hair"))
			{
				if (jc["hair"] != null)
				{
					JSONArray asArray = jc["hair"].AsArray;
					if (asArray != null)
					{
						if (!base.mergeRestore)
						{
							this.ResetHair(true);
						}
						IEnumerator enumerator = asArray.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								JSONClass jsonclass = (JSONClass)obj;
								string id = jsonclass["id"];
								string text2 = jsonclass["internalId"];
								string itemId = FileManager.NormalizeID(id);
								bool asBool = jsonclass["enabled"].AsBool;
								DAZHairGroup hairItem = this.GetHairItem(itemId);
								if (hairItem == null && text2 != null)
								{
									hairItem = this.GetHairItem(text2);
									if (hairItem != null)
									{
										itemId = text2;
									}
								}
								this.SetActiveHairItem(itemId, asBool, true);
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
					}
					else
					{
						string text3 = jc["hair"];
						if (text3 == string.Empty)
						{
							if (!base.mergeRestore)
							{
								this.ResetHair(false);
							}
						}
						else
						{
							if (!base.mergeRestore)
							{
								this.ResetHair(true);
							}
							this.SetActiveHairItem(text3, true, true);
						}
					}
				}
				else if (setMissingToDefault && !base.mergeRestore)
				{
					this.ResetHair(false);
				}
			}
			if (!base.IsCustomAppearanceParamLocked("clothing"))
			{
				if (jc["clothing"] != null)
				{
					JSONArray asArray2 = jc["clothing"].AsArray;
					if (asArray2 != null)
					{
						if (!base.mergeRestore)
						{
							this.ResetClothing(true);
						}
						IEnumerator enumerator2 = asArray2.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								object obj2 = enumerator2.Current;
								JSONClass jsonclass2 = (JSONClass)obj2;
								string text4 = jsonclass2["id"];
								string itemId2;
								if (text4 == null)
								{
									itemId2 = jsonclass2["name"];
								}
								else
								{
									itemId2 = FileManager.NormalizeID(text4);
								}
								string text5 = jsonclass2["internalId"];
								bool asBool2 = jsonclass2["enabled"].AsBool;
								DAZClothingItem clothingItem = this.GetClothingItem(itemId2);
								if (clothingItem == null && text5 != null)
								{
									clothingItem = this.GetClothingItem(text5);
									if (clothingItem != null)
									{
										itemId2 = text5;
									}
								}
								this.SetActiveClothingItem(itemId2, asBool2, true);
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
					}
					else if (!base.mergeRestore)
					{
						this.ResetClothing(false);
					}
				}
				else if (setMissingToDefault && !base.mergeRestore)
				{
					this.ResetClothing(false);
				}
			}
			if (this.containingAtom.isPreparingToPutBackInPool)
			{
				foreach (DAZCharacter dazcharacter in this._femaleCharacters)
				{
					dazcharacter.ResetUnregisteredInstance();
				}
				foreach (DAZCharacter dazcharacter2 in this._maleCharacters)
				{
					dazcharacter2.ResetUnregisteredInstance();
				}
				foreach (DAZClothingItem dazclothingItem in this._femaleClothingItems)
				{
					dazclothingItem.ResetUnregisteredInstance();
				}
				foreach (DAZClothingItem dazclothingItem2 in this._maleClothingItems)
				{
					dazclothingItem2.ResetUnregisteredInstance();
				}
				foreach (DAZHairGroup dazhairGroup in this._femaleHairItems)
				{
					dazhairGroup.ResetUnregisteredInstance();
				}
				foreach (DAZHairGroup dazhairGroup2 in this._maleHairItems)
				{
					dazhairGroup2.ResetUnregisteredInstance();
				}
			}
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = base.appearanceLocked || base.IsCustomAppearanceParamLocked("morphs");
		bool flag4 = base.physicalLocked || base.IsCustomPhysicalParamLocked("morphs");
		bool flag5 = base.appearanceLocked || base.IsCustomAppearanceParamLocked("morphsOtherGender");
		bool flag6 = base.physicalLocked || base.IsCustomPhysicalParamLocked("morphsOtherGender");
		if (!flag3 || !flag4)
		{
			if (jc["morphs"] != null && this.morphsControlUI != null && !base.mergeRestore)
			{
				this.ResetMorphsToDefault(restorePhysical && !flag4, restoreAppearance && !flag3);
				flag = true;
			}
			else if (setMissingToDefault && !base.mergeRestore)
			{
				this.ResetMorphsToDefault(restorePhysical && !flag4, restoreAppearance && !flag3);
				flag = true;
			}
		}
		if (!flag5 || !flag6)
		{
			if (jc["morphsOtherGender"] != null && this.morphsControlUIOtherGender != null && !base.mergeRestore)
			{
				this.ResetMorphsOtherGender(restorePhysical && !flag6, restoreAppearance && !flag5);
				flag = true;
			}
			else if (setMissingToDefault && !base.mergeRestore)
			{
				this.ResetMorphsOtherGender(restorePhysical && !flag6, restoreAppearance && !flag5);
				flag = true;
			}
		}
		if (flag)
		{
			if (this._characterRun != null)
			{
				this._characterRun.SmoothApplyMorphsLite();
			}
			else
			{
				if (this.morphBank1 != null)
				{
					this.morphBank1.ApplyMorphsImmediate();
				}
				if (this.morphBank1OtherGender != null)
				{
					this.morphBank1OtherGender.ApplyMorphsImmediate();
				}
				if (this.morphBank2 != null)
				{
					this.morphBank2.ApplyMorphsImmediate();
				}
				if (this.morphBank3 != null)
				{
					this.morphBank3.ApplyMorphsImmediate();
				}
			}
		}
		if (this.containingAtom.isPreparingToPutBackInPool)
		{
			this.UnloadDemandActivatedMorphs();
		}
		if ((!flag3 || !flag4) && jc["morphs"] != null && this.morphsControlUI != null)
		{
			if (!flag3 && restoreAppearance && SuperController.singleton != null)
			{
				bool flag7 = false;
				if (this.morphBank1 != null && this.morphBank1.LoadTransientMorphs(SuperController.singleton.currentLoadDir))
				{
					flag7 = true;
				}
				if (this.morphBank2 != null && this.morphBank2.LoadTransientMorphs(SuperController.singleton.currentLoadDir))
				{
					flag7 = true;
				}
				if (this.morphBank3 != null && this.morphBank3.LoadTransientMorphs(SuperController.singleton.currentLoadDir))
				{
					flag7 = true;
				}
				if (flag7)
				{
					flag2 = true;
					this.ResyncMorphs(DAZCharacterSelector.ResyncMorphsOption.CurrentGender);
				}
			}
			JSONArray asArray3 = jc["morphs"].AsArray;
			if (asArray3 != null)
			{
				IEnumerator enumerator3 = asArray3.GetEnumerator();
				try
				{
					while (enumerator3.MoveNext())
					{
						object obj3 = enumerator3.Current;
						JSONClass jsonclass3 = (JSONClass)obj3;
						string text6 = jsonclass3["uid"];
						string text7 = jsonclass3["name"];
						DAZMorph dazmorph = null;
						if (text6 != null)
						{
							text6 = FileManager.NormalizeID(text6);
							dazmorph = this.morphsControlUI.GetMorphByUid(text6);
						}
						if (dazmorph == null && text7 != null)
						{
							dazmorph = this.morphsControlUI.GetMorphByDisplayName(text7);
						}
						if (dazmorph != null)
						{
							bool isPoseControl = dazmorph.isPoseControl;
							if ((!flag4 && restorePhysical && isPoseControl) || (!flag3 && restoreAppearance && !isPoseControl))
							{
								dazmorph.RestoreFromJSON(jsonclass3);
							}
						}
						else if (!this.morphsControlUI.IsBadMorph(text7))
						{
							if (text6 != null)
							{
								SuperController.LogError(string.Concat(new string[]
								{
									"Could not find morph by uid ",
									text6,
									" or name ",
									text7,
									" referenced in save file"
								}));
							}
							else if (text7 != null)
							{
								SuperController.LogError("Could not find morph by name " + text7 + " referenced in save file");
							}
						}
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
				if (this.morphsControlUI.CleanDemandActivatedMorphs())
				{
					flag2 = true;
				}
			}
		}
		if ((!flag5 || !flag6) && jc["morphsOtherGender"] != null && this.morphsControlUIOtherGender != null)
		{
			if (!flag5 && restoreAppearance && SuperController.singleton != null)
			{
				bool flag8 = false;
				if (this.morphBank1OtherGender != null && this.morphBank1OtherGender.LoadTransientMorphs(SuperController.singleton.currentLoadDir))
				{
					flag8 = true;
				}
				if (flag8)
				{
					flag2 = true;
					this.ResyncMorphs(DAZCharacterSelector.ResyncMorphsOption.OtherGender);
				}
			}
			JSONArray asArray4 = jc["morphsOtherGender"].AsArray;
			if (asArray4 != null)
			{
				IEnumerator enumerator4 = asArray4.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						object obj4 = enumerator4.Current;
						JSONClass jsonclass4 = (JSONClass)obj4;
						string text8 = jsonclass4["uid"];
						string text9 = jsonclass4["name"];
						DAZMorph dazmorph2 = null;
						if (text8 != null)
						{
							text8 = FileManager.NormalizeID(text8);
							dazmorph2 = this.morphsControlUIOtherGender.GetMorphByUid(text8);
						}
						if (dazmorph2 == null && text9 != null)
						{
							dazmorph2 = this.morphsControlUIOtherGender.GetMorphByDisplayName(text9);
						}
						if (dazmorph2 != null)
						{
							bool isPoseControl2 = dazmorph2.isPoseControl;
							if ((!flag6 && restorePhysical && isPoseControl2) || (!flag5 && restoreAppearance && !isPoseControl2))
							{
								dazmorph2.RestoreFromJSON(jsonclass4);
							}
						}
						else if (!this.morphsControlUIOtherGender.IsBadMorph(text9))
						{
							if (text8 != null)
							{
								SuperController.LogError(string.Concat(new string[]
								{
									"Could not find morph by uid ",
									text8,
									" or name ",
									text9,
									" referenced in save file"
								}));
							}
							else if (text9 != null)
							{
								SuperController.LogError("Could not find morph by name " + text9 + " referenced in save file");
							}
						}
					}
				}
				finally
				{
					IDisposable disposable4;
					if ((disposable4 = (enumerator4 as IDisposable)) != null)
					{
						disposable4.Dispose();
					}
				}
				if (this.morphsControlUIOtherGender.CleanDemandActivatedMorphs())
				{
					flag2 = true;
				}
			}
		}
		if (flag2)
		{
			this.ResyncMorphRegistry();
		}
		if (flag)
		{
			if (this._characterRun != null)
			{
				this._characterRun.SmoothApplyMorphs();
			}
			else
			{
				if (this.morphBank1 != null)
				{
					this.morphBank1.ApplyMorphsImmediate();
				}
				if (this.morphBank1OtherGender != null)
				{
					this.morphBank1OtherGender.ApplyMorphsImmediate();
				}
				if (this.morphBank2 != null)
				{
					this.morphBank2.ApplyMorphsImmediate();
				}
				if (this.morphBank3 != null)
				{
					this.morphBank3.ApplyMorphsImmediate();
				}
			}
		}
		this.insideRestore = false;
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x00161488 File Offset: 0x0015F888
	private IEnumerator ExportOBJHelper()
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		this._characterRun.doSnap = false;
		OBJExporter oe = base.GetComponent<OBJExporter>();
		Dictionary<int, bool> enabledMats = new Dictionary<int, bool>();
		for (int i = 0; i < this.exportSkin.materialsEnabled.Length; i++)
		{
			if (this.exportSkin.materialsEnabled[i])
			{
				enabledMats.Add(i, true);
			}
		}
		oe.Export(this.selectedCharacter.name + ".obj", this.exportSkin.GetMesh(), this._characterRun.snappedMorphedUVVertices, this._characterRun.snappedMorphedUVNormals, this.exportSkin.dazMesh.materials, enabledMats);
		oe.Export(this.selectedCharacter.name + "_skinned.obj", this.exportSkin.GetMesh(), this._characterRun.snappedSkinnedVertices, this._characterRun.snappedSkinnedNormals, this.exportSkin.dazMesh.materials, enabledMats);
		yield break;
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x001614A4 File Offset: 0x0015F8A4
	public void ExportCurrentCharacterOBJ()
	{
		OBJExporter component = base.GetComponent<OBJExporter>();
		DAZMergedSkinV2 componentInChildren = this.selectedCharacter.GetComponentInChildren<DAZMergedSkinV2>();
		if (componentInChildren != null && component != null && this._characterRun != null)
		{
			this.exportSkin = componentInChildren;
			this._characterRun.doSnap = true;
			base.StartCoroutine(this.ExportOBJHelper());
		}
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x0016150D File Offset: 0x0015F90D
	public void InitBones()
	{
		if (this.rootBones != null)
		{
			this.rootBones.Init();
			this.maleAnatomyComponents = this.rootBones.GetComponentsInChildren<DAZMaleAnatomy>(true);
		}
	}

	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x060047E5 RID: 18405 RVA: 0x0016153D File Offset: 0x0015F93D
	// (set) Token: 0x060047E6 RID: 18406 RVA: 0x00161545 File Offset: 0x0015F945
	public DAZCharacterSelector.Gender gender
	{
		get
		{
			return this._gender;
		}
		set
		{
			if (this._gender != value)
			{
				this._gender = value;
				this.SyncGender();
			}
		}
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x00161560 File Offset: 0x0015F960
	protected void SyncGender()
	{
		foreach (Transform transform in this.maleTransforms)
		{
			if (transform != null)
			{
				if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Male)
				{
					transform.gameObject.SetActive(true);
				}
				else
				{
					transform.gameObject.SetActive(false);
				}
			}
		}
		if (this.maleMorphBank1 != null)
		{
			if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Male)
			{
				this.maleMorphBank1.gameObject.SetActive(true);
			}
			else
			{
				this.maleMorphBank1.gameObject.SetActive(false);
			}
		}
		if (this.maleMorphBank2 != null)
		{
			if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Male)
			{
				this.maleMorphBank2.gameObject.SetActive(true);
			}
			else
			{
				this.maleMorphBank2.gameObject.SetActive(false);
			}
		}
		if (this.maleMorphBank3 != null)
		{
			if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Male)
			{
				this.maleMorphBank3.gameObject.SetActive(true);
			}
			else
			{
				this.maleMorphBank3.gameObject.SetActive(false);
			}
		}
		foreach (Transform transform2 in this.femaleTransforms)
		{
			if (transform2 != null)
			{
				if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Female)
				{
					transform2.gameObject.SetActive(true);
				}
				else
				{
					transform2.gameObject.SetActive(false);
				}
			}
		}
		if (this.femaleMorphBank1 != null)
		{
			if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Female)
			{
				this.femaleMorphBank1.gameObject.SetActive(true);
			}
			else
			{
				this.femaleMorphBank1.gameObject.SetActive(false);
			}
		}
		if (this.femaleMorphBank2 != null)
		{
			if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Female)
			{
				this.femaleMorphBank2.gameObject.SetActive(true);
			}
			else
			{
				this.femaleMorphBank2.gameObject.SetActive(false);
			}
		}
		if (this.femaleMorphBank3 != null)
		{
			if (this._gender == DAZCharacterSelector.Gender.Both || this._gender == DAZCharacterSelector.Gender.Female)
			{
				this.femaleMorphBank3.gameObject.SetActive(true);
			}
			else
			{
				this.femaleMorphBank3.gameObject.SetActive(false);
			}
		}
		this.ResyncMorphRegistry();
		if (this.rootBones != null)
		{
			if (this._gender == DAZCharacterSelector.Gender.Male)
			{
				this.rootBones.name = this.rootBonesNameMale;
				this.rootBones.isMale = true;
			}
			else if (this._gender == DAZCharacterSelector.Gender.Female)
			{
				this.rootBones.name = this.rootBonesNameFemale;
				this.rootBones.isMale = false;
			}
			else
			{
				this.rootBones.name = this.rootBonesName;
				this.rootBones.isMale = false;
			}
		}
		this.SyncColliders();
		this.Init(true);
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x001618B4 File Offset: 0x0015FCB4
	private void SyncAnatomy()
	{
		if (this._selectedCharacter != null)
		{
			bool flag = !this._disableAnatomy;
			if (this.clothingItems != null)
			{
				foreach (DAZClothingItem dazclothingItem in this.clothingItems)
				{
					if (dazclothingItem != null && dazclothingItem.active && dazclothingItem.disableAnatomy)
					{
						flag = false;
						break;
					}
				}
			}
			if (this.hairItems != null)
			{
				foreach (DAZHairGroup dazhairGroup in this.hairItems)
				{
					if (dazhairGroup != null && dazhairGroup.active && dazhairGroup.disableAnatomy)
					{
						flag = false;
						break;
					}
				}
			}
			DAZSkinV2 skin = this._selectedCharacter.skin;
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				if (this.maleAnatomyComponents != null && this.maleAnatomyComponents.Length > 0)
				{
					foreach (DAZMaleAnatomy dazmaleAnatomy in this.maleAnatomyComponents)
					{
						Rigidbody[] componentsInChildren = dazmaleAnatomy.GetComponentsInChildren<Rigidbody>();
						foreach (Rigidbody rigidbody in componentsInChildren)
						{
							rigidbody.detectCollisions = flag;
						}
					}
				}
				if (skin != null)
				{
					foreach (int num in this.maleAnatomyOnMaterialSlots)
					{
						if (skin.materialsEnabled.Length > num)
						{
							skin.materialsEnabled[num] = flag;
						}
					}
					foreach (int num2 in this.maleAnatomyOffMaterialSlots)
					{
						if (skin.materialsEnabled.Length > num2)
						{
							skin.materialsEnabled[num2] = !flag;
						}
					}
				}
				if (skin != null && skin.dazMesh != null)
				{
					foreach (int num4 in this.maleAnatomyOnMaterialSlots)
					{
						if (skin.dazMesh.materialsEnabled.Length > num4)
						{
							skin.dazMesh.materialsEnabled[num4] = flag;
						}
					}
					foreach (int num6 in this.maleAnatomyOffMaterialSlots)
					{
						if (skin.dazMesh.materialsEnabled.Length > num6)
						{
							skin.dazMesh.materialsEnabled[num6] = !flag;
						}
					}
				}
			}
			else
			{
				if (skin != null)
				{
					foreach (int num8 in this.femaleAnatomyOnMaterialSlots)
					{
						if (skin.materialsEnabled.Length > num8)
						{
							skin.materialsEnabled[num8] = flag;
						}
					}
					foreach (int num10 in this.femaleAnatomyOffMaterialSlots)
					{
						if (skin.materialsEnabled.Length > num10)
						{
							skin.materialsEnabled[num10] = !flag;
						}
					}
				}
				if (skin != null && skin.dazMesh != null)
				{
					foreach (int num12 in this.femaleAnatomyOnMaterialSlots)
					{
						if (skin.dazMesh.materialsEnabled.Length > num12)
						{
							skin.dazMesh.materialsEnabled[num12] = flag;
						}
					}
					foreach (int num14 in this.femaleAnatomyOffMaterialSlots)
					{
						if (skin.dazMesh.materialsEnabled.Length > num14)
						{
							skin.dazMesh.materialsEnabled[num14] = !flag;
						}
					}
				}
				if (this.femaleBreastAdjustJoints != null)
				{
					float num15 = 1f;
					bool springDamperMultiplierOn = false;
					foreach (DAZClothingItem dazclothingItem2 in this.clothingItems)
					{
						if (dazclothingItem2 != null && dazclothingItem2.active && dazclothingItem2.adjustFemaleBreastJointSpringAndDamper && dazclothingItem2.jointAdjustEnabled)
						{
							springDamperMultiplierOn = true;
							if (dazclothingItem2.breastJointSpringAndDamperMultiplier > num15)
							{
								num15 = dazclothingItem2.breastJointSpringAndDamperMultiplier;
							}
						}
					}
					this.femaleBreastAdjustJoints.springDamperMultiplierOn = springDamperMultiplierOn;
					this.femaleBreastAdjustJoints.springDamperMultiplier = num15;
				}
				if (this.femaleGluteAdjustJoints != null)
				{
					float num17 = 1f;
					bool springDamperMultiplierOn2 = false;
					foreach (DAZClothingItem dazclothingItem3 in this.clothingItems)
					{
						if (dazclothingItem3 != null && dazclothingItem3.active && dazclothingItem3.adjustFemaleGluteJointSpringAndDamper && dazclothingItem3.jointAdjustEnabled)
						{
							springDamperMultiplierOn2 = true;
							if (dazclothingItem3.breastJointSpringAndDamperMultiplier > num17)
							{
								num17 = dazclothingItem3.gluteJointSpringAndDamperMultiplier;
							}
						}
					}
					this.femaleGluteAdjustJoints.springDamperMultiplierOn = springDamperMultiplierOn2;
					this.femaleGluteAdjustJoints.springDamperMultiplier = num17;
				}
			}
		}
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x00161E16 File Offset: 0x00160216
	protected void SyncDisableAnatomy(bool b)
	{
		this._disableAnatomy = b;
		this.SyncAnatomy();
	}

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060047EA RID: 18410 RVA: 0x00161E28 File Offset: 0x00160228
	public DAZMorphBank morphBank1
	{
		get
		{
			DAZCharacterSelector.Gender gender = this.gender;
			if (gender == DAZCharacterSelector.Gender.Female)
			{
				return this.femaleMorphBank1;
			}
			if (gender != DAZCharacterSelector.Gender.Male)
			{
				return null;
			}
			return this.maleMorphBank1;
		}
	}

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x060047EB RID: 18411 RVA: 0x00161E60 File Offset: 0x00160260
	public DAZMorphBank morphBank1OtherGender
	{
		get
		{
			DAZCharacterSelector.Gender gender = this.gender;
			if (gender == DAZCharacterSelector.Gender.Female)
			{
				return this.maleMorphBank1;
			}
			if (gender != DAZCharacterSelector.Gender.Male)
			{
				return null;
			}
			return this.femaleMorphBank1;
		}
	}

	// Token: 0x17000A18 RID: 2584
	// (get) Token: 0x060047EC RID: 18412 RVA: 0x00161E98 File Offset: 0x00160298
	public DAZMorphBank morphBank2
	{
		get
		{
			DAZCharacterSelector.Gender gender = this.gender;
			if (gender == DAZCharacterSelector.Gender.Female)
			{
				return this.femaleMorphBank2;
			}
			if (gender != DAZCharacterSelector.Gender.Male)
			{
				return null;
			}
			return this.maleMorphBank2;
		}
	}

	// Token: 0x17000A19 RID: 2585
	// (get) Token: 0x060047ED RID: 18413 RVA: 0x00161ED0 File Offset: 0x001602D0
	public DAZMorphBank morphBank3
	{
		get
		{
			DAZCharacterSelector.Gender gender = this.gender;
			if (gender == DAZCharacterSelector.Gender.Female)
			{
				return this.femaleMorphBank3;
			}
			if (gender != DAZCharacterSelector.Gender.Male)
			{
				return null;
			}
			return this.maleMorphBank3;
		}
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x00161F06 File Offset: 0x00160306
	public void SetMorphAnimatable(DAZMorph dm)
	{
		SuperController.LogError("SetMorphAnimatable is deprecated");
	}

	// Token: 0x060047EF RID: 18415 RVA: 0x00161F14 File Offset: 0x00160314
	protected void InitMorphBanks()
	{
		if (Application.isPlaying)
		{
			if (this.morphBankContainer == null)
			{
				this.morphBankContainer = base.transform;
			}
			if (this.femaleMorphBank1 == null && this.femaleMorphBank1Prefab != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.femaleMorphBank1Prefab.gameObject);
				gameObject.transform.SetParent(this.morphBankContainer);
				this.femaleMorphBank1 = gameObject.GetComponent<DAZMorphBank>();
				this.femaleMorphBank1.morphBones = this.rootBones;
				this.femaleMorphBank1.Init();
				DAZMorphBank dazmorphBank = this.femaleMorphBank1;
				dazmorphBank.onMorphFavoriteChangedHandlers = (DAZMorphBank.MorphFavoriteChanged)Delegate.Combine(dazmorphBank.onMorphFavoriteChangedHandlers, new DAZMorphBank.MorphFavoriteChanged(this.MorphFavoriteChanged));
				if (this.morphsControlFemaleUI != null)
				{
					this.morphsControlFemaleUI.morphBank1 = this.femaleMorphBank1;
				}
				if (this.morphsControlFemaleUIAlt != null)
				{
					this.morphsControlFemaleUIAlt.morphBank1 = this.femaleMorphBank1;
				}
			}
			if (this.femaleMorphBank2 == null && this.femaleMorphBank2Prefab != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.femaleMorphBank2Prefab.gameObject);
				gameObject2.transform.SetParent(this.morphBankContainer);
				this.femaleMorphBank2 = gameObject2.GetComponent<DAZMorphBank>();
				this.femaleMorphBank2.morphBones = this.rootBones;
				this.femaleMorphBank2.Init();
				DAZMorphBank dazmorphBank2 = this.femaleMorphBank2;
				dazmorphBank2.onMorphFavoriteChangedHandlers = (DAZMorphBank.MorphFavoriteChanged)Delegate.Combine(dazmorphBank2.onMorphFavoriteChangedHandlers, new DAZMorphBank.MorphFavoriteChanged(this.MorphFavoriteChanged));
				if (this.morphsControlFemaleUI != null)
				{
					this.morphsControlFemaleUI.morphBank2 = this.femaleMorphBank2;
				}
				if (this.morphsControlFemaleUIAlt != null)
				{
					this.morphsControlFemaleUIAlt.morphBank2 = this.femaleMorphBank2;
				}
			}
			if (this.femaleMorphBank3 == null && this.femaleMorphBank3Prefab != null)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.femaleMorphBank3Prefab.gameObject);
				gameObject3.transform.SetParent(this.morphBankContainer);
				this.femaleMorphBank3 = gameObject3.GetComponent<DAZMorphBank>();
				this.femaleMorphBank3.morphBones = this.rootBones;
				this.femaleMorphBank3.Init();
				DAZMorphBank dazmorphBank3 = this.femaleMorphBank3;
				dazmorphBank3.onMorphFavoriteChangedHandlers = (DAZMorphBank.MorphFavoriteChanged)Delegate.Combine(dazmorphBank3.onMorphFavoriteChangedHandlers, new DAZMorphBank.MorphFavoriteChanged(this.MorphFavoriteChanged));
				if (this.morphsControlFemaleUI != null)
				{
					this.morphsControlFemaleUI.morphBank3 = this.femaleMorphBank3;
				}
				if (this.morphsControlFemaleUIAlt != null)
				{
					this.morphsControlFemaleUIAlt.morphBank3 = this.femaleMorphBank3;
				}
			}
			if (this.maleMorphBank1 == null && this.maleMorphBank1Prefab != null)
			{
				GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.maleMorphBank1Prefab.gameObject);
				gameObject4.transform.SetParent(this.morphBankContainer);
				this.maleMorphBank1 = gameObject4.GetComponent<DAZMorphBank>();
				this.maleMorphBank1.morphBones = this.rootBones;
				this.maleMorphBank1.Init();
				DAZMorphBank dazmorphBank4 = this.maleMorphBank1;
				dazmorphBank4.onMorphFavoriteChangedHandlers = (DAZMorphBank.MorphFavoriteChanged)Delegate.Combine(dazmorphBank4.onMorphFavoriteChangedHandlers, new DAZMorphBank.MorphFavoriteChanged(this.MorphFavoriteChanged));
				if (this.morphsControlMaleUI != null)
				{
					this.morphsControlMaleUI.morphBank1 = this.maleMorphBank1;
				}
				if (this.morphsControlMaleUIAlt != null)
				{
					this.morphsControlMaleUIAlt.morphBank1 = this.maleMorphBank1;
				}
			}
			if (this.maleMorphBank2 == null && this.maleMorphBank2Prefab != null)
			{
				GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(this.maleMorphBank2Prefab.gameObject);
				gameObject5.transform.SetParent(this.morphBankContainer);
				this.maleMorphBank2 = gameObject5.GetComponent<DAZMorphBank>();
				this.maleMorphBank2.morphBones = this.rootBones;
				this.maleMorphBank2.Init();
				DAZMorphBank dazmorphBank5 = this.maleMorphBank2;
				dazmorphBank5.onMorphFavoriteChangedHandlers = (DAZMorphBank.MorphFavoriteChanged)Delegate.Combine(dazmorphBank5.onMorphFavoriteChangedHandlers, new DAZMorphBank.MorphFavoriteChanged(this.MorphFavoriteChanged));
				if (this.morphsControlMaleUI != null)
				{
					this.morphsControlMaleUI.morphBank2 = this.maleMorphBank2;
				}
				if (this.morphsControlMaleUIAlt != null)
				{
					this.morphsControlMaleUIAlt.morphBank2 = this.maleMorphBank2;
				}
			}
			if (this.maleMorphBank3 == null && this.maleMorphBank3Prefab != null)
			{
				GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(this.maleMorphBank3Prefab.gameObject);
				gameObject6.transform.SetParent(this.morphBankContainer);
				this.maleMorphBank3 = gameObject6.GetComponent<DAZMorphBank>();
				this.maleMorphBank3.morphBones = this.rootBones;
				this.maleMorphBank3.Init();
				DAZMorphBank dazmorphBank6 = this.maleMorphBank3;
				dazmorphBank6.onMorphFavoriteChangedHandlers = (DAZMorphBank.MorphFavoriteChanged)Delegate.Combine(dazmorphBank6.onMorphFavoriteChangedHandlers, new DAZMorphBank.MorphFavoriteChanged(this.MorphFavoriteChanged));
				if (this.morphsControlMaleUI != null)
				{
					this.morphsControlMaleUI.morphBank3 = this.maleMorphBank3;
				}
				if (this.morphsControlMaleUIAlt != null)
				{
					this.morphsControlMaleUIAlt.morphBank3 = this.maleMorphBank3;
				}
			}
			this.ResyncMorphRegistry();
		}
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x0016245E File Offset: 0x0016085E
	protected void SyncOnlyShowFavoriteMorphsInParameterLists(bool b)
	{
		this._onlyShowFavoriteMorphsInParameterLists = b;
		this.ResyncMorphRegistry();
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x0016246D File Offset: 0x0016086D
	protected void MorphFavoriteChanged(DAZMorph dm)
	{
		if (this._onlyShowFavoriteMorphsInParameterLists)
		{
			this.ResyncMorphRegistry();
		}
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x00162480 File Offset: 0x00160880
	protected void ResyncMorphRegistry()
	{
		if (this.registeredMorphNames == null)
		{
			this.registeredMorphNames = new HashSet<string>();
		}
		base.MassDeregister(this.registeredMorphNames);
		this.registeredMorphNames.Clear();
		if (this._gender == DAZCharacterSelector.Gender.Female)
		{
			List<DAZMorph> morphs = this.morphsControlFemaleUI.GetMorphs();
			if (morphs != null)
			{
				foreach (DAZMorph dazmorph in morphs)
				{
					if (dazmorph.activeImmediate)
					{
						string resolvedDisplayName = dazmorph.resolvedDisplayName;
						string text = "morph: " + resolvedDisplayName;
						if (!this.registeredMorphNames.Contains(text))
						{
							dazmorph.jsonFloat.name = text;
							dazmorph.jsonFloat.altName = resolvedDisplayName;
							dazmorph.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph.favorite);
							base.RegisterFloat(dazmorph.jsonFloat);
							this.registeredMorphNames.Add(text);
						}
					}
				}
				foreach (DAZMorph dazmorph2 in morphs)
				{
					if (!dazmorph2.activeImmediate)
					{
						string resolvedDisplayName2 = dazmorph2.resolvedDisplayName;
						string text2 = "morph: " + resolvedDisplayName2;
						if (!this.registeredMorphNames.Contains(text2))
						{
							dazmorph2.jsonFloat.name = text2;
							dazmorph2.jsonFloat.altName = resolvedDisplayName2;
							dazmorph2.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph2.favorite);
							base.RegisterFloat(dazmorph2.jsonFloat);
							this.registeredMorphNames.Add(text2);
						}
					}
				}
			}
			List<DAZMorph> morphs2 = this.morphsControlMaleUI.GetMorphs();
			if (morphs2 != null)
			{
				foreach (DAZMorph dazmorph3 in morphs2)
				{
					if (dazmorph3.activeImmediate)
					{
						string resolvedDisplayName3 = dazmorph3.resolvedDisplayName;
						string text3 = "morphOtherGender: " + resolvedDisplayName3;
						if (!this.registeredMorphNames.Contains(text3))
						{
							dazmorph3.jsonFloat.name = text3;
							dazmorph3.jsonFloat.altName = resolvedDisplayName3;
							dazmorph3.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph3.favorite);
							base.RegisterFloat(dazmorph3.jsonFloat);
							this.registeredMorphNames.Add(text3);
						}
					}
				}
				foreach (DAZMorph dazmorph4 in morphs2)
				{
					if (!dazmorph4.activeImmediate)
					{
						string resolvedDisplayName4 = dazmorph4.resolvedDisplayName;
						string text4 = "morphOtherGender: " + resolvedDisplayName4;
						if (!this.registeredMorphNames.Contains(text4))
						{
							dazmorph4.jsonFloat.name = text4;
							dazmorph4.jsonFloat.altName = resolvedDisplayName4;
							dazmorph4.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph4.favorite);
							base.RegisterFloat(dazmorph4.jsonFloat);
							this.registeredMorphNames.Add(text4);
						}
					}
				}
			}
		}
		else if (this._gender == DAZCharacterSelector.Gender.Male)
		{
			List<DAZMorph> morphs3 = this.morphsControlMaleUI.GetMorphs();
			if (morphs3 != null)
			{
				foreach (DAZMorph dazmorph5 in morphs3)
				{
					if (dazmorph5.activeImmediate)
					{
						string resolvedDisplayName5 = dazmorph5.resolvedDisplayName;
						string text5 = "morph: " + resolvedDisplayName5;
						if (!this.registeredMorphNames.Contains(text5))
						{
							dazmorph5.jsonFloat.name = text5;
							dazmorph5.jsonFloat.altName = resolvedDisplayName5;
							dazmorph5.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph5.favorite);
							base.RegisterFloat(dazmorph5.jsonFloat);
							this.registeredMorphNames.Add(text5);
						}
					}
				}
				foreach (DAZMorph dazmorph6 in morphs3)
				{
					if (!dazmorph6.activeImmediate)
					{
						string resolvedDisplayName6 = dazmorph6.resolvedDisplayName;
						string text6 = "morph: " + resolvedDisplayName6;
						if (!this.registeredMorphNames.Contains(text6))
						{
							dazmorph6.jsonFloat.name = text6;
							dazmorph6.jsonFloat.altName = resolvedDisplayName6;
							dazmorph6.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph6.favorite);
							base.RegisterFloat(dazmorph6.jsonFloat);
							this.registeredMorphNames.Add(text6);
						}
					}
				}
			}
			List<DAZMorph> morphs4 = this.morphsControlFemaleUI.GetMorphs();
			if (morphs4 != null)
			{
				foreach (DAZMorph dazmorph7 in morphs4)
				{
					if (dazmorph7.activeImmediate)
					{
						string resolvedDisplayName7 = dazmorph7.resolvedDisplayName;
						string text7 = "morphOtherGender: " + resolvedDisplayName7;
						if (!this.registeredMorphNames.Contains(text7))
						{
							dazmorph7.jsonFloat.name = text7;
							dazmorph7.jsonFloat.altName = resolvedDisplayName7;
							dazmorph7.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph7.favorite);
							base.RegisterFloat(dazmorph7.jsonFloat);
							this.registeredMorphNames.Add(text7);
						}
					}
				}
				foreach (DAZMorph dazmorph8 in morphs4)
				{
					if (!dazmorph8.activeImmediate)
					{
						string resolvedDisplayName8 = dazmorph8.resolvedDisplayName;
						string text8 = "morphOtherGender: " + resolvedDisplayName8;
						if (!this.registeredMorphNames.Contains(text8))
						{
							dazmorph8.jsonFloat.name = text8;
							dazmorph8.jsonFloat.altName = resolvedDisplayName8;
							dazmorph8.jsonFloat.hidden = (this._onlyShowFavoriteMorphsInParameterLists && !dazmorph8.favorite);
							base.RegisterFloat(dazmorph8.jsonFloat);
							this.registeredMorphNames.Add(text8);
						}
					}
				}
			}
		}
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x00162BAC File Offset: 0x00160FAC
	protected void ResetMorphBanks()
	{
		if (this._characterRun == null)
		{
			if (this.morphBank1 != null)
			{
				this.morphBank1.ResetMorphs();
			}
			if (this.morphBank2 != null)
			{
				this.morphBank2.ResetMorphs();
			}
			if (this.morphBank3 != null)
			{
				this.morphBank3.ResetMorphs();
			}
		}
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x00162C20 File Offset: 0x00161020
	protected void ResetMorphsToDefault(bool resetPhysical, bool resetAppearance)
	{
		this.Init(false);
		if (this._characterRun != null)
		{
			this._characterRun.WaitForRunTask();
		}
		if (this.morphsControlUI != null)
		{
			List<DAZMorph> morphs = this.morphsControlUI.GetMorphs();
			if (morphs != null)
			{
				foreach (DAZMorph dazmorph in morphs)
				{
					bool isPoseControl = dazmorph.isPoseControl;
					if ((resetPhysical && isPoseControl) || (resetAppearance && !isPoseControl))
					{
						dazmorph.Reset();
					}
				}
			}
		}
		this.ResetMorphBanks();
	}

	// Token: 0x060047F5 RID: 18421 RVA: 0x00162CE4 File Offset: 0x001610E4
	public void ResetMorphsOtherGender(bool resetPhysical, bool resetAppearance)
	{
		if (((this._gender == DAZCharacterSelector.Gender.Female && this._useMaleMorphsOnFemale) || (this._gender == DAZCharacterSelector.Gender.Male && this._useFemaleMorphsOnMale)) && this.morphsControlUIOtherGender != null)
		{
			List<DAZMorph> morphs = this.morphsControlUIOtherGender.GetMorphs();
			if (morphs != null)
			{
				foreach (DAZMorph dazmorph in morphs)
				{
					bool isPoseControl = dazmorph.isPoseControl;
					if ((resetPhysical && isPoseControl) || (resetAppearance && !isPoseControl))
					{
						dazmorph.Reset();
					}
				}
			}
			if (this._characterRun == null && this.morphBank1OtherGender != null)
			{
				this.morphBank1OtherGender.ResetMorphs();
			}
		}
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x00162DD8 File Offset: 0x001611D8
	protected void SyncUseOtherGenderMorphs()
	{
		if (this._characterRun != null)
		{
			this._characterRun.useOtherGenderMorphs = ((this._gender == DAZCharacterSelector.Gender.Female && this._useMaleMorphsOnFemale) || (this._gender == DAZCharacterSelector.Gender.Male && this._useFemaleMorphsOnMale));
			this._characterRun.ResetMorphs();
		}
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x00162E3B File Offset: 0x0016123B
	protected void SyncUseMaleMorphsOnFemale(bool b)
	{
		this._useMaleMorphsOnFemale = b;
		if (this._gender == DAZCharacterSelector.Gender.Female && this.morphBank1OtherGender != null)
		{
			this.morphBank1OtherGender.ResetMorphs();
		}
		this.SyncUseOtherGenderMorphs();
	}

	// Token: 0x060047F8 RID: 18424 RVA: 0x00162E72 File Offset: 0x00161272
	protected void SyncUseFemaleMorphsOnMale(bool b)
	{
		this._useFemaleMorphsOnMale = b;
		if (this._gender == DAZCharacterSelector.Gender.Male && this.morphBank1OtherGender != null)
		{
			this.morphBank1OtherGender.ResetMorphs();
		}
		this.SyncUseOtherGenderMorphs();
	}

	// Token: 0x060047F9 RID: 18425 RVA: 0x00162EAC File Offset: 0x001612AC
	protected void ResyncMorphs(DAZCharacterSelector.ResyncMorphsOption resyncOption = DAZCharacterSelector.ResyncMorphsOption.All)
	{
		if (resyncOption == DAZCharacterSelector.ResyncMorphsOption.All || resyncOption == DAZCharacterSelector.ResyncMorphsOption.CurrentGender)
		{
			if (this.morphsControlUI != null)
			{
				this.morphsControlUI.ResyncMorphs();
				this.morphsControlUI.ForceCategoryRefresh();
			}
			if (this.morphsControlUIAlt != null)
			{
				this.morphsControlUIAlt.ResyncMorphs();
				this.morphsControlUIAlt.ForceCategoryRefresh();
			}
		}
		if ((resyncOption == DAZCharacterSelector.ResyncMorphsOption.All || resyncOption == DAZCharacterSelector.ResyncMorphsOption.OtherGender) && this.morphsControlUIOtherGender != null)
		{
			this.morphsControlUIOtherGender.ResyncMorphs();
			this.morphsControlUIOtherGender.ForceCategoryRefresh();
		}
	}

	// Token: 0x060047FA RID: 18426 RVA: 0x00162F48 File Offset: 0x00161348
	public bool RefreshPackageMorphs()
	{
		bool flag = false;
		if (this.femaleMorphBank1 != null && this.femaleMorphBank1.RefreshPackageMorphs())
		{
			flag = true;
		}
		if (this.femaleMorphBank2 != null && this.femaleMorphBank2.RefreshPackageMorphs())
		{
			flag = true;
		}
		if (this.femaleMorphBank3 != null && this.femaleMorphBank3.RefreshPackageMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank1 != null && this.maleMorphBank1.RefreshPackageMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank2 != null && this.maleMorphBank2.RefreshPackageMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank3 != null && this.maleMorphBank3.RefreshPackageMorphs())
		{
			flag = true;
		}
		if (flag)
		{
			this.ResyncMorphs(DAZCharacterSelector.ResyncMorphsOption.All);
			this.ResyncMorphRegistry();
		}
		return flag;
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x00163040 File Offset: 0x00161440
	public bool RefreshRuntimeMorphs()
	{
		bool flag = false;
		if (this.femaleMorphBank1 != null && this.femaleMorphBank1.RefreshRuntimeMorphs())
		{
			flag = true;
		}
		if (this.femaleMorphBank2 != null && this.femaleMorphBank2.RefreshRuntimeMorphs())
		{
			flag = true;
		}
		if (this.femaleMorphBank3 != null && this.femaleMorphBank3.RefreshRuntimeMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank1 != null && this.maleMorphBank1.RefreshRuntimeMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank2 != null && this.maleMorphBank2.RefreshRuntimeMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank3 != null && this.maleMorphBank3.RefreshRuntimeMorphs())
		{
			flag = true;
		}
		if (flag)
		{
			this.ResyncMorphs(DAZCharacterSelector.ResyncMorphsOption.All);
			this.ResyncMorphRegistry();
		}
		return flag;
	}

	// Token: 0x060047FC RID: 18428 RVA: 0x00163138 File Offset: 0x00161538
	public bool CleanDemandActivatedMorphs()
	{
		bool flag = false;
		if (this.morphsControlUI != null && this.morphsControlUI.CleanDemandActivatedMorphs())
		{
			flag = true;
			this.morphsControlUI.ResyncMorphs();
			this.morphsControlUI.ForceCategoryRefresh();
			if (this.morphsControlUIAlt != null)
			{
				this.morphsControlUIAlt.ResyncMorphs();
				this.morphsControlUIAlt.ForceCategoryRefresh();
			}
		}
		if (this.morphsControlUIOtherGender != null && this.morphsControlUIOtherGender.CleanDemandActivatedMorphs())
		{
			flag = true;
			this.morphsControlUIOtherGender.ResyncMorphs();
			this.morphsControlUIOtherGender.ForceCategoryRefresh();
		}
		if (flag)
		{
			this.ResyncMorphRegistry();
		}
		return flag;
	}

	// Token: 0x060047FD RID: 18429 RVA: 0x001631F0 File Offset: 0x001615F0
	public int GetRuntimeMorphDeltasLoadedCount()
	{
		int num = 0;
		if (this.femaleMorphBank1 != null)
		{
			num += this.femaleMorphBank1.GetRuntimeMorphDeltasLoadedCount();
		}
		if (this.femaleMorphBank2 != null)
		{
			num += this.femaleMorphBank2.GetRuntimeMorphDeltasLoadedCount();
		}
		if (this.femaleMorphBank3 != null)
		{
			num += this.femaleMorphBank3.GetRuntimeMorphDeltasLoadedCount();
		}
		if (this.maleMorphBank1 != null)
		{
			num += this.maleMorphBank1.GetRuntimeMorphDeltasLoadedCount();
		}
		if (this.maleMorphBank2 != null)
		{
			num += this.maleMorphBank2.GetRuntimeMorphDeltasLoadedCount();
		}
		if (this.maleMorphBank3 != null)
		{
			num += this.maleMorphBank3.GetRuntimeMorphDeltasLoadedCount();
		}
		return num;
	}

	// Token: 0x060047FE RID: 18430 RVA: 0x001632BC File Offset: 0x001616BC
	public void UnloadRuntimeMorphDeltas()
	{
		if (this.femaleMorphBank1 != null)
		{
			this.femaleMorphBank1.UnloadRuntimeMorphDeltas();
		}
		if (this.femaleMorphBank2 != null)
		{
			this.femaleMorphBank2.UnloadRuntimeMorphDeltas();
		}
		if (this.femaleMorphBank3 != null)
		{
			this.femaleMorphBank3.UnloadRuntimeMorphDeltas();
		}
		if (this.maleMorphBank1 != null)
		{
			this.maleMorphBank1.UnloadRuntimeMorphDeltas();
		}
		if (this.maleMorphBank2 != null)
		{
			this.maleMorphBank2.UnloadRuntimeMorphDeltas();
		}
		if (this.maleMorphBank3 != null)
		{
			this.maleMorphBank3.UnloadRuntimeMorphDeltas();
		}
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x00163374 File Offset: 0x00161774
	public void UnloadDemandActivatedMorphs()
	{
		bool flag = false;
		if (this.femaleMorphBank1 != null && this.femaleMorphBank1.UnloadDemandActivatedMorphs())
		{
			flag = true;
		}
		if (this.femaleMorphBank2 != null && this.femaleMorphBank2.UnloadDemandActivatedMorphs())
		{
			flag = true;
		}
		if (this.femaleMorphBank3 != null && this.femaleMorphBank3.UnloadDemandActivatedMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank1 != null && this.maleMorphBank1.UnloadDemandActivatedMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank2 != null && this.maleMorphBank2.UnloadDemandActivatedMorphs())
		{
			flag = true;
		}
		if (this.maleMorphBank3 != null && this.maleMorphBank3.UnloadDemandActivatedMorphs())
		{
			flag = true;
		}
		if (flag)
		{
			this.CleanDemandActivatedMorphs();
			if (this._characterRun != null)
			{
				this._characterRun.ResetMorphs();
			}
		}
	}

	// Token: 0x06004800 RID: 18432 RVA: 0x0016347E File Offset: 0x0016187E
	protected void SyncCharacterChoice(string choice)
	{
		this.SelectCharacterByName(choice, false);
	}

	// Token: 0x06004801 RID: 18433 RVA: 0x00163488 File Offset: 0x00161888
	public void UnloadInactiveCharacters()
	{
		if (this._femaleCharacters != null)
		{
			foreach (DAZCharacter dazcharacter in this._femaleCharacters)
			{
				dazcharacter.UnloadIfInactive();
			}
		}
		if (this._maleCharacters != null)
		{
			foreach (DAZCharacter dazcharacter2 in this._maleCharacters)
			{
				dazcharacter2.UnloadIfInactive();
			}
		}
		this.ValidateDynamicClothesSkin();
		this.ValidateDynamicHairSkin();
	}

	// Token: 0x06004802 RID: 18434 RVA: 0x0016350C File Offset: 0x0016190C
	public void UnloadDisabledCharacters()
	{
		if (this._femaleCharacters != null)
		{
			foreach (DAZCharacter dazcharacter in this._femaleCharacters)
			{
				dazcharacter.UnloadIfNotEnabled();
			}
		}
		if (this._maleCharacters != null)
		{
			foreach (DAZCharacter dazcharacter2 in this._maleCharacters)
			{
				dazcharacter2.UnloadIfNotEnabled();
			}
		}
		this.ValidateDynamicClothesSkin();
		this.ValidateDynamicHairSkin();
	}

	// Token: 0x06004803 RID: 18435 RVA: 0x00163590 File Offset: 0x00161990
	protected void SyncUnloadCharactersWhenSwitching()
	{
		foreach (DAZCharacter dazcharacter in this._femaleCharacters)
		{
			dazcharacter.unloadOnDisable = this._unloadCharactersWhenSwitching;
		}
		foreach (DAZCharacter dazcharacter2 in this._maleCharacters)
		{
			dazcharacter2.unloadOnDisable = this._unloadCharactersWhenSwitching;
		}
	}

	// Token: 0x06004804 RID: 18436 RVA: 0x001635FB File Offset: 0x001619FB
	protected void SyncUnloadCharactersWhenSwitching(bool b)
	{
		this._unloadCharactersWhenSwitching = b;
		this.SyncUnloadCharactersWhenSwitching();
	}

	// Token: 0x06004805 RID: 18437 RVA: 0x0016360C File Offset: 0x00161A0C
	protected void EarlyInitCharacters()
	{
		if (Application.isPlaying)
		{
			if (this.femaleCharactersPrefab != null && this.femaleCharactersContainer != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.femaleCharactersPrefab.gameObject);
				gameObject.transform.SetParent(this.femaleCharactersContainer);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
			}
			if (this.maleCharactersPrefab != null && this.maleCharactersContainer != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.maleCharactersPrefab.gameObject);
				gameObject2.transform.SetParent(this.maleCharactersContainer);
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
			}
			if (this.femalePlaceholderCharacter != null && !this.femalePlaceholderCharacter.gameObject.activeSelf)
			{
				this.femalePlaceholderCharacter.gameObject.SetActive(true);
				this.femalePlaceholderCharacter.gameObject.SetActive(false);
			}
			if (this.malePlaceholderCharacter != null && !this.malePlaceholderCharacter.gameObject.activeSelf)
			{
				this.malePlaceholderCharacter.gameObject.SetActive(true);
				this.malePlaceholderCharacter.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06004806 RID: 18438 RVA: 0x0016379C File Offset: 0x00161B9C
	public void InitCharacters()
	{
		if (this.characterChooserJSON != null)
		{
			base.DeregisterStringChooser(this.characterChooserJSON);
		}
		if (this.femaleCharactersContainer != null)
		{
			this._femaleCharacters = this.femaleCharactersContainer.GetComponentsInChildren<DAZCharacter>(true);
		}
		else
		{
			this._femaleCharacters = new DAZCharacter[0];
		}
		if (this.maleCharactersContainer != null)
		{
			this._maleCharacters = this.maleCharactersContainer.GetComponentsInChildren<DAZCharacter>(true);
		}
		else
		{
			this._maleCharacters = new DAZCharacter[0];
		}
		this.SyncUnloadCharactersWhenSwitching();
		this._characterByName = new Dictionary<string, DAZCharacter>();
		this._characters = new DAZCharacter[this._femaleCharacters.Length + this._maleCharacters.Length];
		int num = 0;
		for (int i = 0; i < this._femaleCharacters.Length; i++)
		{
			this._femaleCharacters[i].containingAtom = this.containingAtom;
			this._femaleCharacters[i].rootBonesForSkinning = this.rootBones;
			this._characters[num] = this._femaleCharacters[i];
			num++;
		}
		for (int j = 0; j < this._maleCharacters.Length; j++)
		{
			this._maleCharacters[j].containingAtom = this.containingAtom;
			this._maleCharacters[j].rootBonesForSkinning = this.rootBones;
			this._characters[num] = this._maleCharacters[j];
			num++;
		}
		List<string> list = new List<string>();
		string startingValue = string.Empty;
		foreach (DAZCharacter dazcharacter in this._characters)
		{
			if (dazcharacter != null)
			{
				if (this._characterByName.ContainsKey(dazcharacter.displayName))
				{
					UnityEngine.Debug.LogError("Character " + dazcharacter.displayName + " is a duplicate. Cannot add");
				}
				else
				{
					list.Add(dazcharacter.displayName);
					this._characterByName.Add(dazcharacter.displayName, dazcharacter);
					if (dazcharacter.gameObject.activeSelf)
					{
						startingValue = dazcharacter.displayName;
						this._selectedCharacter = dazcharacter;
					}
				}
			}
		}
		if (Application.isPlaying)
		{
			this.characterChooserJSON = new JSONStorableStringChooser("characterSelection", list, startingValue, "Character Selection", new JSONStorableStringChooser.SetStringCallback(this.SyncCharacterChoice));
			this.characterChooserJSON.isRestorable = false;
			this.characterChooserJSON.isStorable = false;
			base.RegisterStringChooser(this.characterChooserJSON);
		}
	}

	// Token: 0x17000A1A RID: 2586
	// (get) Token: 0x06004807 RID: 18439 RVA: 0x00163A0B File Offset: 0x00161E0B
	public DAZCharacter[] femaleCharacters
	{
		get
		{
			return this._femaleCharacters;
		}
	}

	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x06004808 RID: 18440 RVA: 0x00163A13 File Offset: 0x00161E13
	public DAZCharacter[] maleCharacters
	{
		get
		{
			return this._maleCharacters;
		}
	}

	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x06004809 RID: 18441 RVA: 0x00163A1B File Offset: 0x00161E1B
	public DAZCharacter[] characters
	{
		get
		{
			return this._characters;
		}
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x00163A24 File Offset: 0x00161E24
	public void SelectCharacterByName(string characterName, bool fromRestore = false)
	{
		if (this._characterByName == null)
		{
			this.Init(false);
		}
		DAZCharacter dazcharacter;
		if (this._characterByName.TryGetValue(characterName, out dazcharacter))
		{
			if (fromRestore)
			{
				dazcharacter.needsPostLoadJSONRestore = true;
			}
			this.selectedCharacter = dazcharacter;
		}
		if (this.characterChooserJSON != null)
		{
			this.characterChooserJSON.valNoCallback = characterName;
		}
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x00163A84 File Offset: 0x00161E84
	protected void ConnectSkin()
	{
		DAZSkinV2 skin = this._selectedCharacter.skin;
		DAZSkinV2 skinForClothes = this._selectedCharacter.skinForClothes;
		if (skin != null)
		{
			skin.Init();
			skin.ResetPostSkinMorphs();
			if (this._setDAZMorphs != null)
			{
				foreach (SetDAZMorph setDAZMorph in this._setDAZMorphs)
				{
					if (this.morphBank1 != null)
					{
						setDAZMorph.morphBank = this.morphBank1;
					}
					if (this.morphBank2 != null)
					{
						setDAZMorph.morphBankAlt = this.morphBank2;
					}
					if (this.morphBank3 != null)
					{
						setDAZMorph.morphBankAlt2 = this.morphBank3;
					}
				}
			}
			if (this.lipSync != null && this.morphBank1 != null)
			{
				this.lipSync.morphBank = this.morphBank1;
			}
			if (this._physicsMeshes != null)
			{
				foreach (DAZPhysicsMesh dazphysicsMesh in this._physicsMeshes)
				{
					if (dazphysicsMesh != null && dazphysicsMesh.isEnabled)
					{
						dazphysicsMesh.Init();
						dazphysicsMesh.skinTransform = skin.transform;
						dazphysicsMesh.skin = skin;
					}
				}
			}
			if (this._characterRun != null)
			{
				this._characterRun.morphBank1 = this.morphBank1;
				this._characterRun.morphBank1OtherGender = this.morphBank1OtherGender;
				if (this._loadedGenderChange)
				{
					this.SyncUseOtherGenderMorphs();
				}
				this._characterRun.morphBank2 = this.morphBank2;
				this._characterRun.morphBank3 = this.morphBank3;
				this._characterRun.skin = (DAZMergedSkinV2)skin;
				this._characterRun.bones = this.rootBones;
				this._characterRun.autoColliderUpdaters = this._autoColliderBatchUpdaters;
				this._characterRun.Connect(this._loadedGenderChange);
			}
			if (this._eyelidControl != null)
			{
				this._eyelidControl.morphBank = this.morphBank1;
			}
			if (this._autoColliderBatchUpdaters != null)
			{
				foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this._autoColliderBatchUpdaters)
				{
					autoColliderBatchUpdater.skin = skin;
				}
			}
			if (this._autoColliders != null)
			{
				foreach (AutoCollider autoCollider in this._autoColliders)
				{
					if (autoCollider != null)
					{
						autoCollider.skinTransform = skin.transform;
						autoCollider.skin = skin;
					}
				}
			}
			if (this._setAnchorFromVertexComps != null)
			{
				foreach (SetAnchorFromVertex setAnchorFromVertex in this._setAnchorFromVertexComps)
				{
					if (setAnchorFromVertex != null)
					{
						setAnchorFromVertex.skinTransform = skin.transform;
						setAnchorFromVertex.skin = skin;
					}
				}
			}
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				if (this.maleEyelashMaterialOptions != null)
				{
					this.maleEyelashMaterialOptions.skin = skin;
				}
			}
			else if (this.femaleEyelashMaterialOptions != null)
			{
				this.femaleEyelashMaterialOptions.skin = skin;
			}
			foreach (DAZCharacterMaterialOptions dazcharacterMaterialOptions in this._materialOptions)
			{
				if (dazcharacterMaterialOptions != null)
				{
					dazcharacterMaterialOptions.skin = skin;
					if (dazcharacterMaterialOptions.isPassthrough)
					{
						dazcharacterMaterialOptions.ConnectPassthroughBuckets();
					}
				}
			}
			this.ConnectCharacterMaterialOptionsUI();
			if (this.clothingItems != null)
			{
				foreach (DAZClothingItem dazclothingItem in this.clothingItems)
				{
					if (dazclothingItem != null)
					{
						dazclothingItem.skin = skinForClothes;
					}
				}
			}
			if (this.hairItems != null)
			{
				foreach (DAZHairGroup dazhairGroup in this.hairItems)
				{
					if (dazhairGroup != null)
					{
						dazhairGroup.skin = skin;
					}
				}
			}
		}
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x00163EC4 File Offset: 0x001622C4
	private IEnumerator DelayResume(AsyncFlag af, int count)
	{
		this.delayResumeFlag = af;
		for (int i = 0; i < count; i++)
		{
			yield return null;
		}
		af.Raise();
		yield break;
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x00163EF0 File Offset: 0x001622F0
	protected void OnCharacterPreloaded()
	{
		this._loadedGenderChange = false;
		if (this._loadedCharacter != null && this._loadedCharacter != this._selectedCharacter)
		{
			this._loadedCharacter.gameObject.SetActive(false);
			if (this._loadedCharacter.isMale != this._selectedCharacter.isMale)
			{
				this._loadedGenderChange = true;
			}
		}
		this._loadedCharacter = this._selectedCharacter;
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x00163F6C File Offset: 0x0016236C
	protected void OnCharacterLoaded()
	{
		DAZMesh[] componentsInChildren = this._selectedCharacter.GetComponentsInChildren<DAZMesh>(true);
		foreach (DAZMesh dazmesh in componentsInChildren)
		{
			if (this.morphBank1 != null && this.morphBank1.geometryId == dazmesh.geometryId)
			{
				this.morphBank1.connectedMesh = dazmesh;
				dazmesh.morphBank = this.morphBank1;
				if (this.morphBank1OtherGender != null)
				{
					this.morphBank1OtherGender.connectedMesh = dazmesh;
				}
			}
			if (this.morphBank2 != null && this.morphBank2.geometryId == dazmesh.geometryId)
			{
				this.morphBank2.connectedMesh = dazmesh;
				dazmesh.morphBank = this.morphBank2;
			}
			if (this.morphBank3 != null && this.morphBank3.geometryId == dazmesh.geometryId)
			{
				this.morphBank3.connectedMesh = dazmesh;
				dazmesh.morphBank = this.morphBank3;
			}
		}
		this.ConnectSkin();
		this.SyncAnatomy();
		if (this.onCharacterLoadedFlag != null)
		{
			base.StartCoroutine(this.DelayResume(this.onCharacterLoadedFlag, 5));
			this.onCharacterLoadedFlag = null;
		}
		if (this._unloadCharactersWhenSwitching)
		{
			base.StartCoroutine(this.UnloadUnusedAssetsDelayed());
		}
	}

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x0600480F RID: 18447 RVA: 0x001640D3 File Offset: 0x001624D3
	// (set) Token: 0x06004810 RID: 18448 RVA: 0x001640DC File Offset: 0x001624DC
	public DAZCharacter selectedCharacter
	{
		get
		{
			return this._selectedCharacter;
		}
		set
		{
			if (this._selectedCharacter != value)
			{
				if (this._selectedCharacter != null)
				{
					DAZCharacter selectedCharacter = this._selectedCharacter;
					selectedCharacter.onPreloadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Remove(selectedCharacter.onPreloadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterPreloaded));
					DAZCharacter selectedCharacter2 = this._selectedCharacter;
					selectedCharacter2.onLoadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Remove(selectedCharacter2.onLoadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterLoaded));
					if (this._loadedCharacter != this._selectedCharacter)
					{
						this._selectedCharacter.needsPostLoadJSONRestore = false;
						this._selectedCharacter.gameObject.SetActive(false);
					}
					this.DisconnectCharacterOptionsUI();
				}
				this._selectedCharacter = value;
				if (this._selectedCharacter != null)
				{
					bool flag = false;
					if (this._selectedCharacter.isMale)
					{
						if (this._gender != DAZCharacterSelector.Gender.Male)
						{
							this.gender = DAZCharacterSelector.Gender.Male;
							flag = true;
						}
					}
					else if (this._gender != DAZCharacterSelector.Gender.Female)
					{
						this.gender = DAZCharacterSelector.Gender.Female;
						flag = true;
					}
					if (this.characterSelectorUI != null)
					{
						this.characterSelectorUI.SetActiveCharacterToggleNoCallback(this._selectedCharacter.displayName);
					}
					if (this.characterSelectorUIAlt != null)
					{
						this.characterSelectorUIAlt.SetActiveCharacterToggleNoCallback(this._selectedCharacter.displayName);
					}
					if (Application.isPlaying)
					{
						if (flag && this._characterRun != null)
						{
							this._characterRun.Disconnect();
						}
						if (this.onCharacterLoadedFlag != null && !this.onCharacterLoadedFlag.Raised)
						{
							this.onCharacterLoadedFlag.Raise();
						}
						if (!this._selectedCharacter.gameObject.activeInHierarchy)
						{
							this.onCharacterLoadedFlag = new AsyncFlag("Character Load: " + this._selectedCharacter.displayName);
							if (SuperController.singleton != null && (this.containingAtom == null || this.containingAtom.on) && this.selectedCharacter.needsPostLoadJSONRestore && !base.isPresetRestore)
							{
								SuperController.singleton.ResetSimulation(this.onCharacterLoadedFlag, false);
							}
						}
					}
					DAZCharacter selectedCharacter3 = this._selectedCharacter;
					selectedCharacter3.onPreloadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Combine(selectedCharacter3.onPreloadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterPreloaded));
					DAZCharacter selectedCharacter4 = this._selectedCharacter;
					selectedCharacter4.onLoadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Combine(selectedCharacter4.onLoadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterLoaded));
					this._selectedCharacter.gameObject.SetActive(true);
				}
			}
			else if (this._selectedCharacter.needsPostLoadJSONRestore)
			{
				this._selectedCharacter.PostLoadJSONRestore();
			}
		}
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x0016439C File Offset: 0x0016279C
	protected void ConnectDynamicItem(DAZDynamicItem di)
	{
		di.containingAtom = this.containingAtom;
		di.UIbucket = this.UIBucketForDynamicItems;
		switch (di.drawRigidOnBoneType)
		{
		case DAZDynamicItem.BoneType.None:
			di.drawRigidOnBone = null;
			break;
		case DAZDynamicItem.BoneType.Hip:
			di.drawRigidOnBone = this.hipBone;
			break;
		case DAZDynamicItem.BoneType.Pelvis:
			di.drawRigidOnBone = this.pelvisBone;
			break;
		case DAZDynamicItem.BoneType.Chest:
			di.drawRigidOnBone = this.chestBone;
			break;
		case DAZDynamicItem.BoneType.Head:
			di.drawRigidOnBone = this.headBone;
			break;
		case DAZDynamicItem.BoneType.LeftHand:
			di.drawRigidOnBone = this.leftHandBone;
			break;
		case DAZDynamicItem.BoneType.RightHand:
			di.drawRigidOnBone = this.rightHandBone;
			break;
		case DAZDynamicItem.BoneType.LeftFoot:
			di.drawRigidOnBone = this.leftFootBone;
			break;
		case DAZDynamicItem.BoneType.RightFoot:
			di.drawRigidOnBone = this.rightFootBone;
			break;
		}
		switch (di.drawRigidOnBoneTypeLeft)
		{
		case DAZDynamicItem.BoneType.None:
			di.drawRigidOnBoneLeft = null;
			break;
		case DAZDynamicItem.BoneType.Hip:
			di.drawRigidOnBoneLeft = this.hipBone;
			break;
		case DAZDynamicItem.BoneType.Pelvis:
			di.drawRigidOnBoneLeft = this.pelvisBone;
			break;
		case DAZDynamicItem.BoneType.Chest:
			di.drawRigidOnBoneLeft = this.chestBone;
			break;
		case DAZDynamicItem.BoneType.Head:
			di.drawRigidOnBoneLeft = this.headBone;
			break;
		case DAZDynamicItem.BoneType.LeftHand:
			di.drawRigidOnBoneLeft = this.leftHandBone;
			break;
		case DAZDynamicItem.BoneType.RightHand:
			di.drawRigidOnBoneLeft = this.rightHandBone;
			break;
		case DAZDynamicItem.BoneType.LeftFoot:
			di.drawRigidOnBoneLeft = this.leftFootBone;
			break;
		case DAZDynamicItem.BoneType.RightFoot:
			di.drawRigidOnBoneLeft = this.rightFootBone;
			break;
		}
		switch (di.drawRigidOnBoneTypeRight)
		{
		case DAZDynamicItem.BoneType.None:
			di.drawRigidOnBoneRight = null;
			break;
		case DAZDynamicItem.BoneType.Hip:
			di.drawRigidOnBoneRight = this.hipBone;
			break;
		case DAZDynamicItem.BoneType.Pelvis:
			di.drawRigidOnBoneRight = this.pelvisBone;
			break;
		case DAZDynamicItem.BoneType.Chest:
			di.drawRigidOnBoneRight = this.chestBone;
			break;
		case DAZDynamicItem.BoneType.Head:
			di.drawRigidOnBoneRight = this.headBone;
			break;
		case DAZDynamicItem.BoneType.LeftHand:
			di.drawRigidOnBoneRight = this.leftHandBone;
			break;
		case DAZDynamicItem.BoneType.RightHand:
			di.drawRigidOnBoneRight = this.rightHandBone;
			break;
		case DAZDynamicItem.BoneType.LeftFoot:
			di.drawRigidOnBoneRight = this.leftFootBone;
			break;
		case DAZDynamicItem.BoneType.RightFoot:
			di.drawRigidOnBoneRight = this.rightFootBone;
			break;
		}
		switch (di.autoColliderReferenceBoneType)
		{
		case DAZDynamicItem.BoneType.None:
			di.autoColliderReference = null;
			break;
		case DAZDynamicItem.BoneType.Hip:
			if (this.hipBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.hipBone.transform;
			}
			break;
		case DAZDynamicItem.BoneType.Pelvis:
			if (this.pelvisBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.pelvisBone.transform;
			}
			break;
		case DAZDynamicItem.BoneType.Chest:
			if (this.chestBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.chestBone.transform;
			}
			break;
		case DAZDynamicItem.BoneType.Head:
			if (this.headBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.headBone.transform;
			}
			break;
		case DAZDynamicItem.BoneType.LeftHand:
			if (this.leftHandBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.leftHandBone.transform;
			}
			break;
		case DAZDynamicItem.BoneType.RightHand:
			if (this.rightHandBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.rightHandBone.transform;
			}
			break;
		case DAZDynamicItem.BoneType.LeftFoot:
			if (this.leftFootBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.leftFootBone.transform;
			}
			break;
		case DAZDynamicItem.BoneType.RightFoot:
			if (this.rightFootBone == null)
			{
				di.autoColliderReference = null;
			}
			else
			{
				di.autoColliderReference = this.rightFootBone.transform;
			}
			break;
		}
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x001647FC File Offset: 0x00162BFC
	protected void SyncCustomItems(DAZDynamicItem.Gender gender, DAZDynamicItem[] existingItems, string searchPath, Transform dynamicItemPrefab, Transform container)
	{
		if (this.alreadyReportedDuplicates == null)
		{
			this.alreadyReportedDuplicates = new HashSet<string>();
		}
		Dictionary<string, DAZDynamicItem> dictionary = new Dictionary<string, DAZDynamicItem>();
		foreach (DAZDynamicItem dazdynamicItem in existingItems)
		{
			if (dazdynamicItem.type == DAZDynamicItem.Type.Custom && dazdynamicItem.isDynamicRuntimeLoaded && !dictionary.ContainsKey(dazdynamicItem.uid))
			{
				dictionary.Add(dazdynamicItem.uid, dazdynamicItem);
			}
		}
		List<FileEntry> list = new List<FileEntry>();
		try
		{
			FileManager.FindAllFiles(searchPath, "*.vam", list, false);
			FileManager.SortFileEntriesByLastWriteTime(list);
		}
		catch (Exception arg)
		{
			SuperController.LogError("Exception during refresh of dynamic items " + arg);
		}
		Dictionary<string, bool> dictionary2 = new Dictionary<string, bool>();
		Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
		foreach (FileEntry fileEntry in list)
		{
			try
			{
				JSONNode jsonnode = null;
				if (fileEntry is VarFileEntry)
				{
					VarFileEntry varFileEntry = fileEntry as VarFileEntry;
					jsonnode = varFileEntry.Package.GetJSONCache(varFileEntry.InternalSlashPath);
				}
				if (jsonnode == null)
				{
					string aJSON = FileManager.ReadAllText(fileEntry);
					jsonnode = JSON.Parse(aJSON);
				}
				if (jsonnode != null)
				{
					JSONClass asObject = jsonnode.AsObject;
					string text = string.Empty;
					if (asObject["uid"] != null)
					{
						text = asObject["uid"];
					}
					string text2 = string.Empty;
					string backupId = string.Empty;
					string packageUid = null;
					string packageLicense = null;
					int num = -1;
					bool isLatestVersion = true;
					if (fileEntry is VarFileEntry)
					{
						text2 = fileEntry.Uid;
						VarFileEntry varFileEntry2 = fileEntry as VarFileEntry;
						num = varFileEntry2.Package.Version;
						backupId = varFileEntry2.InternalSlashPath;
						packageUid = varFileEntry2.Package.Uid;
						packageLicense = varFileEntry2.Package.LicenseType;
						isLatestVersion = varFileEntry2.Package.isNewestEnabledVersion;
					}
					else
					{
						text2 = fileEntry.Uid;
						backupId = fileEntry.SlashPath;
					}
					string text3;
					if (dictionary3.TryGetValue(text2, out text3))
					{
						if (!this.alreadyReportedDuplicates.Contains(string.Concat(new object[]
						{
							fileEntry,
							":",
							text2,
							":",
							text3
						})))
						{
							SuperController.LogError(string.Concat(new object[]
							{
								"Custom item ",
								fileEntry,
								" uses same UID ",
								text2,
								" as item ",
								text3,
								". Cannot add"
							}));
							this.alreadyReportedDuplicates.Add(string.Concat(new object[]
							{
								fileEntry,
								":",
								text2,
								":",
								text3
							}));
						}
					}
					else
					{
						dictionary3.Add(text2, fileEntry.Uid);
						string text4 = asObject["displayName"];
						if (text4 == null || text4 == string.Empty)
						{
							text4 = Regex.Replace(text, ".*:", string.Empty);
						}
						string creatorName = "None";
						if (asObject["creatorName"] != null)
						{
							creatorName = asObject["creatorName"];
						}
						if (text2 != string.Empty && text4 != string.Empty)
						{
							DAZDynamicItem component;
							if (dictionary.TryGetValue(text2, out component))
							{
								if (!dictionary2.ContainsKey(text2))
								{
									dictionary2.Add(text2, true);
								}
								component.displayName = text4;
								component.creatorName = creatorName;
								component.isLatestVersion = isLatestVersion;
								component.packageUid = packageUid;
								component.packageLicense = packageLicense;
								if (num != -1)
								{
									component.version = "v" + num;
								}
								else
								{
									component.version = string.Empty;
								}
								if (asObject["tags"] != null)
								{
									component.tags = asObject["tags"];
								}
								if (asObject["isRealItem"] != null)
								{
									component.isRealItem = asObject["isRealItem"].AsBool;
								}
							}
							else
							{
								Transform transform = UnityEngine.Object.Instantiate<Transform>(dynamicItemPrefab);
								transform.name = transform.name.Replace("(Clone)", string.Empty);
								transform.gameObject.SetActive(false);
								transform.parent = container;
								transform.localPosition = Vector3.zero;
								transform.localRotation = Quaternion.identity;
								transform.localScale = Vector3.one;
								component = transform.GetComponent<DAZDynamicItem>();
								if (component != null)
								{
									component.containingAtom = this.containingAtom;
									component.UIbucket = this.customUIBucket;
									component.gender = gender;
									component.uid = text2;
									component.backupId = backupId;
									component.internalUid = text;
									component.displayName = text4;
									component.creatorName = creatorName;
									component.packageUid = packageUid;
									component.packageLicense = packageLicense;
									component.isLatestVersion = isLatestVersion;
									if (num != -1)
									{
										component.version = "v" + num;
									}
									else
									{
										component.version = string.Empty;
									}
									if (asObject["tags"] != null)
									{
										component.tags = asObject["tags"];
									}
									if (asObject["isRealItem"] != null)
									{
										component.isRealItem = asObject["isRealItem"].AsBool;
									}
									if (this._selectedCharacter != null)
									{
										if (component.gender == DAZDynamicItem.Gender.Male && !this._selectedCharacter.isMale)
										{
											if (this.malePlaceholderCharacter != null)
											{
												component.skin = this.malePlaceholderCharacter.skin;
											}
										}
										else if (component.gender == DAZDynamicItem.Gender.Female && this._selectedCharacter.isMale)
										{
											if (this.femalePlaceholderCharacter != null)
											{
												component.skin = this.femalePlaceholderCharacter.skin;
											}
										}
										else
										{
											component.skin = this._selectedCharacter.skin;
										}
									}
									component.isDynamicRuntimeLoaded = true;
									component.dynamicRuntimeLoadPath = fileEntry.Uid;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SuperController.LogError(string.Concat(new object[]
				{
					"Exception while reading dynamic item metafile ",
					fileEntry,
					" ",
					ex
				}));
			}
		}
		foreach (DAZDynamicItem dazdynamicItem2 in dictionary.Values)
		{
			if (!dictionary2.ContainsKey(dazdynamicItem2.uid))
			{
				dazdynamicItem2.transform.SetParent(null);
				UnityEngine.Object.Destroy(dazdynamicItem2.gameObject);
			}
		}
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x00164F94 File Offset: 0x00163394
	public void SetActiveDynamicItem(DAZDynamicItem item, bool active, bool fromRestore = false)
	{
		if (item is DAZClothingItem)
		{
			this.SetActiveClothingItem(item as DAZClothingItem, active, fromRestore);
		}
		else if (item is DAZHairGroup)
		{
			this.SetActiveHairItem(item as DAZHairGroup, active, fromRestore);
		}
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x00164FD0 File Offset: 0x001633D0
	public void LoadDynamicCreatorItem(DAZDynamicItem item, DAZDynamic dd)
	{
		if (item.type == DAZDynamicItem.Type.Custom)
		{
			this.SetActiveDynamicItem(item, false, false);
			if (item.gender == DAZDynamicItem.Gender.Female)
			{
				if (item is DAZClothingItem)
				{
					this.LoadFemaleClothingCreatorItem(dd);
				}
				else if (item is DAZHairGroup)
				{
					this.LoadFemaleHairCreatorItem(dd);
				}
			}
			else if (item.gender == DAZDynamicItem.Gender.Male)
			{
				if (item is DAZClothingItem)
				{
					this.LoadMaleClothingCreatorItem(dd);
				}
				else if (item is DAZHairGroup)
				{
					this.LoadMaleHairCreatorItem(dd);
				}
			}
		}
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x00165064 File Offset: 0x00163464
	protected IEnumerator RefreshWhenHubClosed()
	{
		while (SuperController.singleton.HubOpen || SuperController.singleton.activeUI == SuperController.ActiveUI.PackageDownloader)
		{
			yield return null;
		}
		AsyncFlag af = new AsyncFlag("Clothing and Hair Refresh");
		SuperController.singleton.SetLoadingIconFlag(af);
		yield return null;
		float startt = GlobalStopwatch.GetElapsedMilliseconds();
		this.RefreshDynamicClothes();
		this.RefreshDynamicHair();
		float stopt = GlobalStopwatch.GetElapsedMilliseconds();
		UnityEngine.Debug.Log("Deferred Person refresh clothing and hair took " + (stopt - startt).ToString("F1") + " ms");
		this.refreshCoroutine = null;
		af.Raise();
		yield break;
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x00165080 File Offset: 0x00163480
	public void RefreshDynamicItems()
	{
		if (SuperController.singleton.HubOpen || SuperController.singleton.activeUI == SuperController.ActiveUI.PackageDownloader)
		{
			if (this.refreshCoroutine == null)
			{
				this.refreshCoroutine = SuperController.singleton.StartCoroutine(this.RefreshWhenHubClosed());
			}
		}
		else
		{
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			this.RefreshDynamicClothes();
			this.RefreshDynamicHair();
			float elapsedMilliseconds2 = GlobalStopwatch.GetElapsedMilliseconds();
			UnityEngine.Debug.Log("Person refresh clothing and hair took " + (elapsedMilliseconds2 - elapsedMilliseconds).ToString("F1") + " ms");
		}
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x00165110 File Offset: 0x00163510
	protected void ResetClothing(bool clearAll = false)
	{
		this.Init(false);
		foreach (DAZClothingItem dazclothingItem in this._femaleClothingItems)
		{
			if (clearAll)
			{
				this.SetActiveClothingItem(dazclothingItem, false, false);
			}
			else
			{
				this.SetActiveClothingItem(dazclothingItem, dazclothingItem.startActive, false);
			}
		}
		foreach (DAZClothingItem dazclothingItem2 in this._maleClothingItems)
		{
			if (clearAll)
			{
				this.SetActiveClothingItem(dazclothingItem2, false, false);
			}
			else
			{
				this.SetActiveClothingItem(dazclothingItem2, dazclothingItem2.startActive, false);
			}
		}
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x001651B0 File Offset: 0x001635B0
	protected void SyncClothingItem(JSONStorableBool clothingItemJSON)
	{
		string altName = clothingItemJSON.altName;
		this.SetActiveClothingItem(altName, clothingItemJSON.val, false);
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x001651D4 File Offset: 0x001635D4
	public bool IsClothingUIDAvailable(string uid)
	{
		bool flag = true;
		return (this._clothingItemById == null || !this._clothingItemById.ContainsKey(uid)) && flag;
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x00165204 File Offset: 0x00163604
	protected void ConnectClothingItem(DAZClothingItem dci)
	{
		DAZClothingItem.ColliderType colliderTypeLeft = dci.colliderTypeLeft;
		if (colliderTypeLeft != DAZClothingItem.ColliderType.None)
		{
			if (colliderTypeLeft == DAZClothingItem.ColliderType.Shoe)
			{
				dci.colliderLeft = this.leftShoeCollider;
			}
		}
		else
		{
			dci.colliderLeft = null;
		}
		DAZClothingItem.ColliderType colliderTypeRight = dci.colliderTypeRight;
		if (colliderTypeRight != DAZClothingItem.ColliderType.None)
		{
			if (colliderTypeRight == DAZClothingItem.ColliderType.Shoe)
			{
				dci.colliderRight = this.rightShoeCollider;
			}
		}
		else
		{
			dci.colliderRight = null;
		}
		switch (dci.driveXAngleTargetController1Type)
		{
		case DAZClothingItem.ControllerType.None:
			dci.driveXAngleTargetController1 = null;
			break;
		case DAZClothingItem.ControllerType.LeftFoot:
			dci.driveXAngleTargetController1 = this.leftFootController;
			break;
		case DAZClothingItem.ControllerType.RightFoot:
			dci.driveXAngleTargetController1 = this.rightFootController;
			break;
		case DAZClothingItem.ControllerType.LeftToe:
			dci.driveXAngleTargetController1 = this.leftToeController;
			break;
		case DAZClothingItem.ControllerType.RightToe:
			dci.driveXAngleTargetController1 = this.rightToeController;
			break;
		}
		switch (dci.driveXAngleTargetController2Type)
		{
		case DAZClothingItem.ControllerType.None:
			dci.driveXAngleTargetController2 = null;
			break;
		case DAZClothingItem.ControllerType.LeftFoot:
			dci.driveXAngleTargetController2 = this.leftFootController;
			break;
		case DAZClothingItem.ControllerType.RightFoot:
			dci.driveXAngleTargetController2 = this.rightFootController;
			break;
		case DAZClothingItem.ControllerType.LeftToe:
			dci.driveXAngleTargetController2 = this.leftToeController;
			break;
		case DAZClothingItem.ControllerType.RightToe:
			dci.driveXAngleTargetController2 = this.rightToeController;
			break;
		}
		switch (dci.drive2XAngleTargetController1Type)
		{
		case DAZClothingItem.ControllerType.None:
			dci.drive2XAngleTargetController1 = null;
			break;
		case DAZClothingItem.ControllerType.LeftFoot:
			dci.drive2XAngleTargetController1 = this.leftFootController;
			break;
		case DAZClothingItem.ControllerType.RightFoot:
			dci.drive2XAngleTargetController1 = this.rightFootController;
			break;
		case DAZClothingItem.ControllerType.LeftToe:
			dci.drive2XAngleTargetController1 = this.leftToeController;
			break;
		case DAZClothingItem.ControllerType.RightToe:
			dci.drive2XAngleTargetController1 = this.rightToeController;
			break;
		}
		switch (dci.drive2XAngleTargetController2Type)
		{
		case DAZClothingItem.ControllerType.None:
			dci.drive2XAngleTargetController2 = null;
			break;
		case DAZClothingItem.ControllerType.LeftFoot:
			dci.drive2XAngleTargetController2 = this.leftFootController;
			break;
		case DAZClothingItem.ControllerType.RightFoot:
			dci.drive2XAngleTargetController2 = this.rightFootController;
			break;
		case DAZClothingItem.ControllerType.LeftToe:
			dci.drive2XAngleTargetController2 = this.leftToeController;
			break;
		case DAZClothingItem.ControllerType.RightToe:
			dci.drive2XAngleTargetController2 = this.rightToeController;
			break;
		}
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x0016545C File Offset: 0x0016385C
	protected void EarlyInitClothingItems()
	{
		if (Application.isPlaying)
		{
			if (this.femaleClothingPrefab != null && this.femaleClothingContainer != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.femaleClothingPrefab.gameObject);
				gameObject.transform.SetParent(this.femaleClothingContainer);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				DAZClothingItem[] componentsInChildren = gameObject.GetComponentsInChildren<DAZClothingItem>(true);
				foreach (DAZClothingItem dazclothingItem in componentsInChildren)
				{
					this.ConnectDynamicItem(dazclothingItem);
					this.ConnectClothingItem(dazclothingItem);
				}
			}
			if (this.maleClothingPrefab != null && this.maleClothingContainer != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.maleClothingPrefab.gameObject);
				gameObject2.transform.SetParent(this.maleClothingContainer);
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
				DAZClothingItem[] componentsInChildren2 = gameObject2.GetComponentsInChildren<DAZClothingItem>(true);
				foreach (DAZClothingItem dazclothingItem2 in componentsInChildren2)
				{
					this.ConnectDynamicItem(dazclothingItem2);
					this.ConnectClothingItem(dazclothingItem2);
				}
			}
		}
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x001655D4 File Offset: 0x001639D4
	public void InvalidateDynamicClothingItemThumbnail(string thumbPath)
	{
		string b = thumbPath.Replace(".jpg", ".vam");
		foreach (DAZClothingItem dazclothingItem in this.clothingItems)
		{
			if (dazclothingItem.dynamicRuntimeLoadPath == b)
			{
				dazclothingItem.thumbnail = null;
			}
		}
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x0016562C File Offset: 0x00163A2C
	public void RefreshDynamicClothingThumbnails()
	{
		if (this.gender == DAZCharacterSelector.Gender.Female)
		{
			if (this.clothingSelectorFemaleUI != null)
			{
				this.clothingSelectorFemaleUI.RefreshThumbnails();
			}
		}
		else if (this.gender == DAZCharacterSelector.Gender.Male && this.clothingSelectorMaleUI != null)
		{
			this.clothingSelectorMaleUI.RefreshThumbnails();
		}
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x00165690 File Offset: 0x00163A90
	protected void ValidateDynamicClothesSkin()
	{
		if (this._selectedCharacter != null)
		{
			DAZClothingItem[] componentsInChildren = this.femaleClothingContainer.GetComponentsInChildren<DAZClothingItem>(true);
			foreach (DAZClothingItem dazclothingItem in componentsInChildren)
			{
				if (dazclothingItem.gender == DAZDynamicItem.Gender.Male && !this._selectedCharacter.isMale)
				{
					if (this.malePlaceholderCharacter != null)
					{
						dazclothingItem.skin = this.malePlaceholderCharacter.skin;
					}
				}
				else if (dazclothingItem.gender == DAZDynamicItem.Gender.Female && this._selectedCharacter.isMale)
				{
					if (this.femalePlaceholderCharacter != null)
					{
						dazclothingItem.skin = this.femalePlaceholderCharacter.skin;
					}
				}
				else
				{
					dazclothingItem.skin = this._selectedCharacter.skin;
				}
			}
			DAZClothingItem[] componentsInChildren2 = this.maleClothingContainer.GetComponentsInChildren<DAZClothingItem>(true);
			foreach (DAZClothingItem dazclothingItem2 in componentsInChildren2)
			{
				if (dazclothingItem2.gender == DAZDynamicItem.Gender.Male && !this._selectedCharacter.isMale)
				{
					if (this.malePlaceholderCharacter != null)
					{
						dazclothingItem2.skin = this.malePlaceholderCharacter.skin;
					}
				}
				else if (dazclothingItem2.gender == DAZDynamicItem.Gender.Female && this._selectedCharacter.isMale)
				{
					if (this.femalePlaceholderCharacter != null)
					{
						dazclothingItem2.skin = this.femalePlaceholderCharacter.skin;
					}
				}
				else
				{
					dazclothingItem2.skin = this._selectedCharacter.skin;
				}
			}
		}
	}

	// Token: 0x0600481F RID: 18463 RVA: 0x0016583C File Offset: 0x00163C3C
	public void RefreshDynamicClothes()
	{
		if (Application.isPlaying && this.dynamicClothingItemPrefab != null)
		{
			DAZClothingItem[] componentsInChildren = this.femaleClothingContainer.GetComponentsInChildren<DAZClothingItem>(true);
			this.SyncCustomItems(DAZDynamicItem.Gender.Female, componentsInChildren, "Custom/Clothing/Female/", this.dynamicClothingItemPrefab, this.femaleClothingContainer);
			DAZClothingItem[] componentsInChildren2 = this.maleClothingContainer.GetComponentsInChildren<DAZClothingItem>(true);
			this.SyncCustomItems(DAZDynamicItem.Gender.Male, componentsInChildren2, "Custom/Clothing/Male/", this.dynamicClothingItemPrefab, this.maleClothingContainer);
			this.InitClothingItems();
			if (this.clothingSelectorFemaleUI != null)
			{
				this.clothingSelectorFemaleUI.Resync();
			}
			if (this.clothingSelectorMaleUI != null)
			{
				this.clothingSelectorMaleUI.Resync();
			}
		}
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x001658F0 File Offset: 0x00163CF0
	public HashSet<string> GetClothingOtherTags()
	{
		if (this.gender == DAZCharacterSelector.Gender.Female)
		{
			if (this.clothingSelectorFemaleUI != null)
			{
				return this.clothingSelectorFemaleUI.GetOtherTags();
			}
		}
		else if (this.gender == DAZCharacterSelector.Gender.Male && this.clothingSelectorMaleUI != null)
		{
			return this.clothingSelectorMaleUI.GetOtherTags();
		}
		return null;
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x00165958 File Offset: 0x00163D58
	public void InitClothingItems()
	{
		if (this.maleClothingContainer != null)
		{
			this._maleClothingItems = this.maleClothingContainer.GetComponentsInChildren<DAZClothingItem>(true);
		}
		else
		{
			this._maleClothingItems = new DAZClothingItem[0];
		}
		if (this.femaleClothingContainer != null)
		{
			this._femaleClothingItems = this.femaleClothingContainer.GetComponentsInChildren<DAZClothingItem>(true);
		}
		else
		{
			this._femaleClothingItems = new DAZClothingItem[0];
		}
		this._clothingItemById = new Dictionary<string, DAZClothingItem>();
		this._clothingItemByBackupId = new Dictionary<string, DAZClothingItem>();
		if (Application.isPlaying)
		{
			if (this.clothingItemJSONs == null)
			{
				this.clothingItemJSONs = new Dictionary<string, JSONStorableBool>();
			}
			if (this.clothingItemToggleJSONs == null)
			{
				this.clothingItemToggleJSONs = new List<JSONStorableAction>();
			}
		}
		DAZClothingItem[] clothingItems = this.clothingItems;
		for (int i = 0; i < clothingItems.Length; i++)
		{
			DAZCharacterSelector.<InitClothingItems>c__AnonStorey6 <InitClothingItems>c__AnonStorey = new DAZCharacterSelector.<InitClothingItems>c__AnonStorey6();
			<InitClothingItems>c__AnonStorey.dc = clothingItems[i];
			<InitClothingItems>c__AnonStorey.$this = this;
			if (Application.isPlaying && !this.clothingItemJSONs.ContainsKey(<InitClothingItems>c__AnonStorey.dc.uid))
			{
				JSONStorableBool jsonstorableBool = new JSONStorableBool("clothing:" + <InitClothingItems>c__AnonStorey.dc.uid, <InitClothingItems>c__AnonStorey.dc.gameObject.activeSelf, new JSONStorableBool.SetJSONBoolCallback(this.SyncClothingItem));
				jsonstorableBool.altName = <InitClothingItems>c__AnonStorey.dc.uid;
				jsonstorableBool.isRestorable = false;
				jsonstorableBool.isStorable = false;
				base.RegisterBool(jsonstorableBool);
				this.clothingItemJSONs.Add(<InitClothingItems>c__AnonStorey.dc.uid, jsonstorableBool);
				JSONStorableAction jsonstorableAction = new JSONStorableAction("toggle:" + <InitClothingItems>c__AnonStorey.dc.uid, new JSONStorableAction.ActionCallback(<InitClothingItems>c__AnonStorey.<>m__0));
				base.RegisterAction(jsonstorableAction);
				this.clothingItemToggleJSONs.Add(jsonstorableAction);
			}
			<InitClothingItems>c__AnonStorey.dc.characterSelector = this;
			if (this._clothingItemById.ContainsKey(<InitClothingItems>c__AnonStorey.dc.uid))
			{
				UnityEngine.Debug.LogError("Duplicate uid found for clothing item " + <InitClothingItems>c__AnonStorey.dc.uid);
			}
			else
			{
				this._clothingItemById.Add(<InitClothingItems>c__AnonStorey.dc.uid, <InitClothingItems>c__AnonStorey.dc);
			}
			if (<InitClothingItems>c__AnonStorey.dc.internalUid != null && <InitClothingItems>c__AnonStorey.dc.internalUid != string.Empty && !this._clothingItemById.ContainsKey(<InitClothingItems>c__AnonStorey.dc.internalUid))
			{
				this._clothingItemById.Add(<InitClothingItems>c__AnonStorey.dc.internalUid, <InitClothingItems>c__AnonStorey.dc);
			}
			if (<InitClothingItems>c__AnonStorey.dc.backupId != null && <InitClothingItems>c__AnonStorey.dc.backupId != string.Empty && !this._clothingItemByBackupId.ContainsKey(<InitClothingItems>c__AnonStorey.dc.backupId))
			{
				this._clothingItemByBackupId.Add(<InitClothingItems>c__AnonStorey.dc.backupId, <InitClothingItems>c__AnonStorey.dc);
			}
			if (<InitClothingItems>c__AnonStorey.dc.gameObject.activeSelf)
			{
				<InitClothingItems>c__AnonStorey.dc.active = true;
			}
		}
		if (Application.isPlaying)
		{
			List<string> list = new List<string>();
			foreach (JSONStorableBool jsonstorableBool2 in this.clothingItemJSONs.Values)
			{
				string altName = jsonstorableBool2.altName;
				if (!this._clothingItemById.ContainsKey(altName))
				{
					base.DeregisterBool(jsonstorableBool2);
					list.Add(altName);
				}
			}
			foreach (string key in list)
			{
				this.clothingItemJSONs.Remove(key);
			}
			List<JSONStorableAction> list2 = new List<JSONStorableAction>();
			foreach (JSONStorableAction jsonstorableAction2 in this.clothingItemToggleJSONs)
			{
				string key2 = jsonstorableAction2.name.Replace("toggle:", string.Empty);
				if (!this._clothingItemById.ContainsKey(key2))
				{
					base.DeregisterAction(jsonstorableAction2);
				}
				else
				{
					list2.Add(jsonstorableAction2);
				}
			}
			this.clothingItemToggleJSONs = list2;
		}
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x00165DD8 File Offset: 0x001641D8
	public void UnloadInactiveClothingItems()
	{
		if (this._femaleClothingItems != null)
		{
			foreach (DAZClothingItem dazclothingItem in this._femaleClothingItems)
			{
				dazclothingItem.UnloadIfInactive();
			}
		}
		if (this._maleClothingItems != null)
		{
			foreach (DAZClothingItem dazclothingItem2 in this._maleClothingItems)
			{
				dazclothingItem2.UnloadIfInactive();
			}
		}
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x00165E50 File Offset: 0x00164250
	public void UnloadDisabledClothingItems()
	{
		if (this._femaleClothingItems != null)
		{
			foreach (DAZClothingItem dazclothingItem in this._femaleClothingItems)
			{
				dazclothingItem.UnloadIfNotEnabled();
			}
		}
		if (this._maleClothingItems != null)
		{
			foreach (DAZClothingItem dazclothingItem2 in this._maleClothingItems)
			{
				dazclothingItem2.UnloadIfNotEnabled();
			}
		}
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x00165EC5 File Offset: 0x001642C5
	public void SyncClothingAdjustments()
	{
		this.SyncAnatomy();
	}

	// Token: 0x06004825 RID: 18469 RVA: 0x00165ECD File Offset: 0x001642CD
	public void SyncHairAdjustments()
	{
		this.SyncAnatomy();
	}

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x06004826 RID: 18470 RVA: 0x00165ED5 File Offset: 0x001642D5
	public DAZClothingItem[] maleClothingItems
	{
		get
		{
			this.Init(false);
			return this._maleClothingItems;
		}
	}

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x06004827 RID: 18471 RVA: 0x00165EE4 File Offset: 0x001642E4
	public DAZClothingItem[] femaleClothingItems
	{
		get
		{
			this.Init(false);
			return this._femaleClothingItems;
		}
	}

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x06004828 RID: 18472 RVA: 0x00165EF3 File Offset: 0x001642F3
	public DAZClothingItem[] clothingItems
	{
		get
		{
			this.Init(false);
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this._maleClothingItems;
			}
			if (this.gender == DAZCharacterSelector.Gender.Female)
			{
				return this._femaleClothingItems;
			}
			return null;
		}
	}

	// Token: 0x06004829 RID: 18473 RVA: 0x00165F24 File Offset: 0x00164324
	public DAZClothingItem GetClothingItem(string itemId)
	{
		if (this._clothingItemById == null || this._clothingItemByBackupId == null)
		{
			this.Init(false);
		}
		DAZClothingItem result;
		if (this._clothingItemById.TryGetValue(itemId, out result))
		{
			return result;
		}
		if (this._clothingItemByBackupId.TryGetValue(itemId, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600482A RID: 18474 RVA: 0x00165F7C File Offset: 0x0016437C
	public void EnableUndressOnClothingItem(DAZClothingItem clothingItem)
	{
		if (clothingItem != null)
		{
			ClothSimControl[] componentsInChildren = clothingItem.GetComponentsInChildren<ClothSimControl>();
			foreach (ClothSimControl clothSimControl in componentsInChildren)
			{
				clothSimControl.AllowDetach();
			}
		}
	}

	// Token: 0x0600482B RID: 18475 RVA: 0x00165FBC File Offset: 0x001643BC
	public void EnableUndressAllClothingItems()
	{
		for (int i = 0; i < this.clothingItems.Length; i++)
		{
			DAZClothingItem dazclothingItem = this.clothingItems[i];
			ClothSimControl[] componentsInChildren = dazclothingItem.GetComponentsInChildren<ClothSimControl>();
			foreach (ClothSimControl clothSimControl in componentsInChildren)
			{
				clothSimControl.AllowDetach();
			}
		}
	}

	// Token: 0x0600482C RID: 18476 RVA: 0x0016601C File Offset: 0x0016441C
	public void RemoveAllClothing()
	{
		foreach (DAZClothingItem dazclothingItem in this.clothingItems)
		{
			if (!dazclothingItem.locked)
			{
				this.SetActiveClothingItem(dazclothingItem, false, false);
			}
		}
		if (this.clothingSelectorUI != null)
		{
			this.clothingSelectorUI.ResyncUIIfActiveFilterOn();
		}
	}

	// Token: 0x0600482D RID: 18477 RVA: 0x00166078 File Offset: 0x00164478
	public void RemoveAllRealClothing()
	{
		foreach (DAZClothingItem dazclothingItem in this.clothingItems)
		{
			if (dazclothingItem.isRealItem && !dazclothingItem.locked)
			{
				this.SetActiveClothingItem(dazclothingItem, false, false);
			}
		}
		if (this.clothingSelectorUI != null)
		{
			this.clothingSelectorUI.ResyncUIIfActiveFilterOn();
		}
	}

	// Token: 0x0600482E RID: 18478 RVA: 0x001660E0 File Offset: 0x001644E0
	public void LockActiveClothing()
	{
		foreach (DAZClothingItem dazclothingItem in this.clothingItems)
		{
			if (dazclothingItem.active)
			{
				dazclothingItem.SetLocked(true);
			}
		}
	}

	// Token: 0x0600482F RID: 18479 RVA: 0x00166120 File Offset: 0x00164520
	public void UnlockAllClothing()
	{
		foreach (DAZClothingItem dazclothingItem in this.clothingItems)
		{
			if (dazclothingItem.locked)
			{
				dazclothingItem.SetLocked(false);
			}
		}
	}

	// Token: 0x06004830 RID: 18480 RVA: 0x0016615E File Offset: 0x0016455E
	public void ToggleClothingItem(DAZClothingItem item)
	{
		this.SetActiveClothingItem(item, !item.active, false);
	}

	// Token: 0x06004831 RID: 18481 RVA: 0x00166174 File Offset: 0x00164574
	public void SetActiveClothingItem(string itemId, bool active, bool fromRestore = false)
	{
		DAZClothingItem item;
		if (this._clothingItemById != null && this._clothingItemById.TryGetValue(itemId, out item))
		{
			this.SetActiveClothingItem(item, active, fromRestore);
		}
		else if (this._clothingItemByBackupId != null && this._clothingItemByBackupId.TryGetValue(itemId, out item))
		{
			this.SetActiveClothingItem(item, active, fromRestore);
		}
		else
		{
			SuperController.LogError("Clothing item " + itemId + " is missing");
		}
	}

	// Token: 0x06004832 RID: 18482 RVA: 0x001661F0 File Offset: 0x001645F0
	private IEnumerator DelayLoadClothingCreatorItem(DAZClothingItem item, DAZDynamic source)
	{
		while (!item.ready)
		{
			yield return null;
		}
		yield return null;
		DAZRuntimeCreator drc = item.GetComponentInChildren<DAZRuntimeCreator>();
		if (drc != null)
		{
			drc.LoadFromPath(source.GetMetaStorePath());
			item.OpenUI();
		}
		yield break;
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x00166212 File Offset: 0x00164612
	public void LoadMaleClothingCreatorItem(DAZDynamic source)
	{
		if (this.maleClothingCreatorItem != null)
		{
			this.SetActiveClothingItem(this.maleClothingCreatorItem, true, false);
			base.StartCoroutine(this.DelayLoadClothingCreatorItem(this.maleClothingCreatorItem, source));
		}
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x00166247 File Offset: 0x00164647
	public void LoadFemaleClothingCreatorItem(DAZDynamic source)
	{
		if (this.femaleClothingCreatorItem != null)
		{
			this.SetActiveClothingItem(this.femaleClothingCreatorItem, true, false);
			base.StartCoroutine(this.DelayLoadClothingCreatorItem(this.femaleClothingCreatorItem, source));
		}
	}

	// Token: 0x06004835 RID: 18485 RVA: 0x0016627C File Offset: 0x0016467C
	public void SetActiveClothingItem(DAZClothingItem item, bool active, bool fromRestore = false)
	{
		if (item != null)
		{
			if (item.locked)
			{
				if (this.containingAtom.isPreparingToPutBackInPool)
				{
					item.SetLocked(false);
				}
				else if (this.insideRestore)
				{
					return;
				}
			}
			if (active && fromRestore)
			{
				item.needsPostLoadJSONRestore = true;
			}
			if (item.locked && !active)
			{
				item.SetLocked(false);
			}
			item.active = active;
			DAZClothingItem.ExclusiveRegion exclusiveRegion = item.exclusiveRegion;
			if (active && exclusiveRegion != DAZClothingItem.ExclusiveRegion.None)
			{
				for (int i = 0; i < this.clothingItems.Length; i++)
				{
					if (this.clothingItems[i] != item && item.gender == this.clothingItems[i].gender && this.clothingItems[i].exclusiveRegion == exclusiveRegion && this.clothingItems[i].active)
					{
						this.SetActiveClothingItem(this.clothingItems[i], false, false);
					}
				}
			}
			item.gameObject.SetActive(active);
			if (this.clothingSelectorUI != null)
			{
				this.clothingSelectorUI.SetDynamicItemToggle(item, active);
			}
			JSONStorableBool jsonstorableBool;
			if (this.clothingItemJSONs.TryGetValue(item.uid, out jsonstorableBool))
			{
				jsonstorableBool.val = active;
			}
			this.SyncAnatomy();
		}
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x001663D5 File Offset: 0x001647D5
	public void SetClothingItemLock(DAZClothingItem item, bool locked)
	{
		if (item != null)
		{
		}
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x001663E4 File Offset: 0x001647E4
	protected void ResetHair(bool clearAll = false)
	{
		this.Init(false);
		foreach (DAZHairGroup dazhairGroup in this._femaleHairItems)
		{
			if (clearAll)
			{
				this.SetActiveHairItem(dazhairGroup, false, false);
			}
			else
			{
				this.SetActiveHairItem(dazhairGroup, dazhairGroup.startActive, false);
			}
		}
		foreach (DAZHairGroup dazhairGroup2 in this._maleHairItems)
		{
			if (clearAll)
			{
				this.SetActiveHairItem(dazhairGroup2, false, false);
			}
			else
			{
				this.SetActiveHairItem(dazhairGroup2, dazhairGroup2.startActive, false);
			}
		}
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x00166484 File Offset: 0x00164884
	protected void SyncHairItem(JSONStorableBool hairItemJSON)
	{
		string altName = hairItemJSON.altName;
		this.SetActiveHairItem(altName, hairItemJSON.val, false);
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x001664A8 File Offset: 0x001648A8
	public bool IsHairUIDAvailable(string uid)
	{
		bool flag = true;
		return (this._hairItemById == null || !this._hairItemById.ContainsKey(uid)) && flag;
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x001664D8 File Offset: 0x001648D8
	protected void EarlyInitHairItems()
	{
		if (Application.isPlaying)
		{
			if (this.femaleHairPrefab != null && this.femaleHairContainer != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.femaleHairPrefab.gameObject);
				gameObject.transform.SetParent(this.femaleHairContainer);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				DAZDynamicItem[] componentsInChildren = gameObject.GetComponentsInChildren<DAZDynamicItem>(true);
				foreach (DAZDynamicItem di in componentsInChildren)
				{
					this.ConnectDynamicItem(di);
				}
			}
			if (this.maleHairPrefab != null && this.maleHairContainer != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.maleHairPrefab.gameObject);
				gameObject2.transform.SetParent(this.maleHairContainer);
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localScale = Vector3.one;
				DAZDynamicItem[] componentsInChildren2 = gameObject2.GetComponentsInChildren<DAZDynamicItem>(true);
				foreach (DAZDynamicItem di2 in componentsInChildren2)
				{
					this.ConnectDynamicItem(di2);
				}
			}
		}
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x00166640 File Offset: 0x00164A40
	public void InvalidateDynamicHairItemThumbnail(string thumbPath)
	{
		string b = Regex.Replace(thumbPath, "\\.jpg$", ".vam");
		foreach (DAZHairGroup dazhairGroup in this.hairItems)
		{
			if (dazhairGroup.dynamicRuntimeLoadPath == b)
			{
				dazhairGroup.thumbnail = null;
			}
		}
	}

	// Token: 0x0600483C RID: 18492 RVA: 0x00166698 File Offset: 0x00164A98
	public void RefreshDynamicHairThumbnails()
	{
		if (this.gender == DAZCharacterSelector.Gender.Female)
		{
			if (this.hairSelectorFemaleUI != null)
			{
				this.hairSelectorFemaleUI.RefreshThumbnails();
			}
		}
		else if (this.gender == DAZCharacterSelector.Gender.Male && this.hairSelectorMaleUI != null)
		{
			this.hairSelectorMaleUI.RefreshThumbnails();
		}
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x001666FC File Offset: 0x00164AFC
	protected void ValidateDynamicHairSkin()
	{
		if (this._selectedCharacter != null)
		{
			DAZHairGroup[] componentsInChildren = this.femaleHairContainer.GetComponentsInChildren<DAZHairGroup>(true);
			foreach (DAZHairGroup dazhairGroup in componentsInChildren)
			{
				if (dazhairGroup.gender == DAZDynamicItem.Gender.Male && !this._selectedCharacter.isMale)
				{
					if (this.malePlaceholderCharacter != null)
					{
						dazhairGroup.skin = this.malePlaceholderCharacter.skin;
					}
				}
				else if (dazhairGroup.gender == DAZDynamicItem.Gender.Female && this._selectedCharacter.isMale)
				{
					if (this.femalePlaceholderCharacter != null)
					{
						dazhairGroup.skin = this.femalePlaceholderCharacter.skin;
					}
				}
				else
				{
					dazhairGroup.skin = this._selectedCharacter.skin;
				}
			}
			DAZHairGroup[] componentsInChildren2 = this.maleHairContainer.GetComponentsInChildren<DAZHairGroup>(true);
			foreach (DAZHairGroup dazhairGroup2 in componentsInChildren2)
			{
				if (dazhairGroup2.gender == DAZDynamicItem.Gender.Male && !this._selectedCharacter.isMale)
				{
					if (this.malePlaceholderCharacter != null)
					{
						dazhairGroup2.skin = this.malePlaceholderCharacter.skin;
					}
				}
				else if (dazhairGroup2.gender == DAZDynamicItem.Gender.Female && this._selectedCharacter.isMale)
				{
					if (this.femalePlaceholderCharacter != null)
					{
						dazhairGroup2.skin = this.femalePlaceholderCharacter.skin;
					}
				}
				else
				{
					dazhairGroup2.skin = this._selectedCharacter.skin;
				}
			}
		}
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x001668A8 File Offset: 0x00164CA8
	public void RefreshDynamicHair()
	{
		if (Application.isPlaying && this.dynamicHairItemPrefab != null)
		{
			DAZHairGroup[] componentsInChildren = this.femaleHairContainer.GetComponentsInChildren<DAZHairGroup>(true);
			this.SyncCustomItems(DAZDynamicItem.Gender.Female, componentsInChildren, "Custom/Hair/Female/", this.dynamicHairItemPrefab, this.femaleHairContainer);
			DAZHairGroup[] componentsInChildren2 = this.maleHairContainer.GetComponentsInChildren<DAZHairGroup>(true);
			this.SyncCustomItems(DAZDynamicItem.Gender.Male, componentsInChildren2, "Custom/Hair/Male/", this.dynamicHairItemPrefab, this.maleHairContainer);
			this.InitHairItems();
			if (this.hairSelectorFemaleUI != null)
			{
				this.hairSelectorFemaleUI.Resync();
			}
			if (this.hairSelectorMaleUI != null)
			{
				this.hairSelectorMaleUI.Resync();
			}
		}
	}

	// Token: 0x0600483F RID: 18495 RVA: 0x0016695C File Offset: 0x00164D5C
	public HashSet<string> GetHairOtherTags()
	{
		if (this.gender == DAZCharacterSelector.Gender.Female)
		{
			if (this.hairSelectorFemaleUI != null)
			{
				return this.hairSelectorFemaleUI.GetOtherTags();
			}
		}
		else if (this.gender == DAZCharacterSelector.Gender.Male && this.hairSelectorMaleUI != null)
		{
			return this.hairSelectorMaleUI.GetOtherTags();
		}
		return null;
	}

	// Token: 0x06004840 RID: 18496 RVA: 0x001669C4 File Offset: 0x00164DC4
	public void InitHairItems()
	{
		if (this.maleHairContainer != null)
		{
			this._maleHairItems = this.maleHairContainer.GetComponentsInChildren<DAZHairGroup>(true);
		}
		else
		{
			this._maleHairItems = new DAZHairGroup[0];
		}
		if (this.femaleHairContainer != null)
		{
			this._femaleHairItems = this.femaleHairContainer.GetComponentsInChildren<DAZHairGroup>(true);
		}
		else
		{
			this._femaleHairItems = new DAZHairGroup[0];
		}
		this._hairItemById = new Dictionary<string, DAZHairGroup>();
		this._hairItemByBackupId = new Dictionary<string, DAZHairGroup>();
		if (Application.isPlaying)
		{
			if (this.hairItemJSONs == null)
			{
				this.hairItemJSONs = new Dictionary<string, JSONStorableBool>();
			}
			if (this.hairItemToggleJSONs == null)
			{
				this.hairItemToggleJSONs = new List<JSONStorableAction>();
			}
		}
		DAZHairGroup[] hairItems = this.hairItems;
		for (int i = 0; i < hairItems.Length; i++)
		{
			DAZCharacterSelector.<InitHairItems>c__AnonStorey7 <InitHairItems>c__AnonStorey = new DAZCharacterSelector.<InitHairItems>c__AnonStorey7();
			<InitHairItems>c__AnonStorey.dc = hairItems[i];
			<InitHairItems>c__AnonStorey.$this = this;
			if (Application.isPlaying && !this.hairItemJSONs.ContainsKey(<InitHairItems>c__AnonStorey.dc.uid))
			{
				JSONStorableBool jsonstorableBool = new JSONStorableBool("hair:" + <InitHairItems>c__AnonStorey.dc.uid, <InitHairItems>c__AnonStorey.dc.gameObject.activeSelf, new JSONStorableBool.SetJSONBoolCallback(this.SyncHairItem));
				jsonstorableBool.altName = <InitHairItems>c__AnonStorey.dc.uid;
				jsonstorableBool.isRestorable = false;
				jsonstorableBool.isStorable = false;
				base.RegisterBool(jsonstorableBool);
				this.hairItemJSONs.Add(<InitHairItems>c__AnonStorey.dc.uid, jsonstorableBool);
				JSONStorableAction jsonstorableAction = new JSONStorableAction("toggle:" + <InitHairItems>c__AnonStorey.dc.uid, new JSONStorableAction.ActionCallback(<InitHairItems>c__AnonStorey.<>m__0));
				base.RegisterAction(jsonstorableAction);
				this.hairItemToggleJSONs.Add(jsonstorableAction);
			}
			<InitHairItems>c__AnonStorey.dc.characterSelector = this;
			if (this._hairItemById.ContainsKey(<InitHairItems>c__AnonStorey.dc.uid))
			{
				UnityEngine.Debug.LogError("Duplicate uid found for hair item " + <InitHairItems>c__AnonStorey.dc.uid);
			}
			else
			{
				this._hairItemById.Add(<InitHairItems>c__AnonStorey.dc.uid, <InitHairItems>c__AnonStorey.dc);
			}
			if (<InitHairItems>c__AnonStorey.dc.internalUid != null && <InitHairItems>c__AnonStorey.dc.internalUid != string.Empty && !this._hairItemById.ContainsKey(<InitHairItems>c__AnonStorey.dc.internalUid))
			{
				this._hairItemById.Add(<InitHairItems>c__AnonStorey.dc.internalUid, <InitHairItems>c__AnonStorey.dc);
			}
			if (!this._hairItemByBackupId.ContainsKey(<InitHairItems>c__AnonStorey.dc.backupId))
			{
				this._hairItemByBackupId.Add(<InitHairItems>c__AnonStorey.dc.backupId, <InitHairItems>c__AnonStorey.dc);
			}
			if (<InitHairItems>c__AnonStorey.dc.gameObject.activeSelf)
			{
				<InitHairItems>c__AnonStorey.dc.active = true;
			}
		}
		if (Application.isPlaying)
		{
			List<string> list = new List<string>();
			foreach (JSONStorableBool jsonstorableBool2 in this.hairItemJSONs.Values)
			{
				string altName = jsonstorableBool2.altName;
				if (!this._hairItemById.ContainsKey(altName))
				{
					base.DeregisterBool(jsonstorableBool2);
					list.Add(altName);
				}
			}
			foreach (string key in list)
			{
				this.hairItemJSONs.Remove(key);
			}
			List<JSONStorableAction> list2 = new List<JSONStorableAction>();
			foreach (JSONStorableAction jsonstorableAction2 in this.hairItemToggleJSONs)
			{
				string key2 = jsonstorableAction2.name.Replace("toggle:", string.Empty);
				if (!this._hairItemById.ContainsKey(key2))
				{
					base.DeregisterAction(jsonstorableAction2);
				}
				else
				{
					list2.Add(jsonstorableAction2);
				}
			}
			this.hairItemToggleJSONs = list2;
		}
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x00166E18 File Offset: 0x00165218
	public void UnloadInactiveHairItems()
	{
		if (this._femaleHairItems != null)
		{
			foreach (DAZHairGroup dazhairGroup in this._femaleHairItems)
			{
				dazhairGroup.UnloadIfInactive();
			}
		}
		if (this._maleHairItems != null)
		{
			foreach (DAZHairGroup dazhairGroup2 in this._maleHairItems)
			{
				dazhairGroup2.UnloadIfInactive();
			}
		}
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x00166E90 File Offset: 0x00165290
	public void UnloadDisabledHairItems()
	{
		if (this._femaleHairItems != null)
		{
			foreach (DAZHairGroup dazhairGroup in this._femaleHairItems)
			{
				dazhairGroup.UnloadIfNotEnabled();
			}
		}
		if (this._maleHairItems != null)
		{
			foreach (DAZHairGroup dazhairGroup2 in this._maleHairItems)
			{
				dazhairGroup2.UnloadIfNotEnabled();
			}
		}
	}

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x06004843 RID: 18499 RVA: 0x00166F05 File Offset: 0x00165305
	public DAZHairGroup[] maleHairItems
	{
		get
		{
			this.Init(false);
			return this._maleHairItems;
		}
	}

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x06004844 RID: 18500 RVA: 0x00166F14 File Offset: 0x00165314
	public DAZHairGroup[] femaleHairItems
	{
		get
		{
			this.Init(false);
			return this._femaleHairItems;
		}
	}

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x06004845 RID: 18501 RVA: 0x00166F23 File Offset: 0x00165323
	public DAZHairGroup[] hairItems
	{
		get
		{
			this.Init(false);
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this._maleHairItems;
			}
			if (this.gender == DAZCharacterSelector.Gender.Female)
			{
				return this._femaleHairItems;
			}
			return null;
		}
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x00166F54 File Offset: 0x00165354
	public DAZHairGroup GetHairItem(string itemId)
	{
		if (this._hairItemById == null)
		{
			this.Init(false);
		}
		DAZHairGroup result;
		if (this._hairItemById.TryGetValue(itemId, out result))
		{
			return result;
		}
		if (this._hairItemByBackupId.TryGetValue(itemId, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x00166FA0 File Offset: 0x001653A0
	public void RemoveAllHair()
	{
		foreach (DAZHairGroup dazhairGroup in this.hairItems)
		{
			if (!dazhairGroup.locked)
			{
				this.SetActiveHairItem(dazhairGroup, false, false);
			}
		}
		if (this.hairSelectorUI != null)
		{
			this.hairSelectorUI.ResyncUIIfActiveFilterOn();
		}
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x00166FFC File Offset: 0x001653FC
	public void LockActiveHair()
	{
		foreach (DAZHairGroup dazhairGroup in this.hairItems)
		{
			if (dazhairGroup.active)
			{
				dazhairGroup.SetLocked(true);
			}
		}
	}

	// Token: 0x06004849 RID: 18505 RVA: 0x0016703C File Offset: 0x0016543C
	public void UnlockAllHair()
	{
		foreach (DAZHairGroup dazhairGroup in this.hairItems)
		{
			if (dazhairGroup.locked)
			{
				dazhairGroup.SetLocked(false);
			}
		}
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x0016707A File Offset: 0x0016547A
	public void ToggleHairItem(DAZHairGroup item)
	{
		this.SetActiveHairItem(item, !item.active, false);
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x00167090 File Offset: 0x00165490
	public void SetActiveHairItem(string itemId, bool active, bool fromRestore = false)
	{
		DAZHairGroup item;
		if (this._hairItemById != null && this._hairItemById.TryGetValue(itemId, out item))
		{
			this.SetActiveHairItem(item, active, fromRestore);
		}
		else if (this._hairItemByBackupId != null && this._hairItemByBackupId.TryGetValue(itemId, out item))
		{
			this.SetActiveHairItem(item, active, fromRestore);
		}
		else if (itemId != "Sim Hair")
		{
			SuperController.LogError("Hair item " + itemId + " is missing");
		}
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x0016711C File Offset: 0x0016551C
	private IEnumerator DelayLoadHairCreatorItem(DAZHairGroup item, DAZDynamic source)
	{
		while (!item.ready)
		{
			yield return null;
		}
		yield return null;
		DAZRuntimeCreator drc = item.GetComponentInChildren<DAZRuntimeCreator>();
		if (drc != null)
		{
			drc.LoadFromPath(source.GetMetaStorePath());
			item.OpenUI();
		}
		yield break;
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x0016713E File Offset: 0x0016553E
	public void LoadMaleHairCreatorItem(DAZDynamic source)
	{
		if (this.maleHairCreatorItem != null)
		{
			this.SetActiveHairItem(this.maleHairCreatorItem, true, false);
			base.StartCoroutine(this.DelayLoadHairCreatorItem(this.maleHairCreatorItem, source));
		}
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x00167173 File Offset: 0x00165573
	public void LoadFemaleHairCreatorItem(DAZDynamic source)
	{
		if (this.femaleHairCreatorItem != null)
		{
			this.SetActiveHairItem(this.femaleHairCreatorItem, true, false);
			base.StartCoroutine(this.DelayLoadHairCreatorItem(this.femaleHairCreatorItem, source));
		}
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x001671A8 File Offset: 0x001655A8
	public void SetActiveHairItem(DAZHairGroup item, bool active, bool fromRestore = false)
	{
		if (item != null)
		{
			if (item.locked)
			{
				if (this.containingAtom.isPreparingToPutBackInPool)
				{
					item.SetLocked(false);
				}
				else if (this.insideRestore)
				{
					return;
				}
			}
			if (active && fromRestore)
			{
				item.needsPostLoadJSONRestore = true;
			}
			if (item.locked && !active)
			{
				item.SetLocked(false);
			}
			item.active = active;
			item.gameObject.SetActive(active);
			if (this.hairSelectorUI != null)
			{
				this.hairSelectorUI.SetDynamicItemToggle(item, active);
			}
			JSONStorableBool jsonstorableBool;
			if (this.hairItemJSONs.TryGetValue(item.uid, out jsonstorableBool))
			{
				jsonstorableBool.val = active;
			}
			this.SyncAnatomy();
		}
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x00167278 File Offset: 0x00165678
	protected IEnumerator UnloadUnusedAssetsDelayed()
	{
		yield return null;
		yield return null;
		Resources.UnloadUnusedAssets();
		GC.Collect();
		yield break;
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x0016728C File Offset: 0x0016568C
	public void UnloadInactiveObjects()
	{
		this.UnloadInactiveCharacters();
		this.UnloadInactiveClothingItems();
		this.UnloadInactiveHairItems();
		this.UnloadRuntimeMorphDeltas();
		this.UnloadDemandActivatedMorphs();
		base.StartCoroutine(this.UnloadUnusedAssetsDelayed());
	}

	// Token: 0x06004852 RID: 18514 RVA: 0x001672B9 File Offset: 0x001656B9
	protected void SyncUseAuxBreastColliders(bool b)
	{
		this._useAuxBreastColliders = b;
		this.SyncColliders();
	}

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x06004853 RID: 18515 RVA: 0x001672C8 File Offset: 0x001656C8
	// (set) Token: 0x06004854 RID: 18516 RVA: 0x001672D0 File Offset: 0x001656D0
	public bool useAuxBreastColliders
	{
		get
		{
			return this._useAuxBreastColliders;
		}
		set
		{
			if (this.useAuxBreastCollidersJSON != null)
			{
				this.useAuxBreastCollidersJSON.val = value;
			}
			else if (this._useAuxBreastColliders != value)
			{
				this.SyncUseAuxBreastColliders(value);
			}
		}
	}

	// Token: 0x06004855 RID: 18517 RVA: 0x00167301 File Offset: 0x00165701
	protected void SyncUseAdvancedColliders(bool b)
	{
		this._useAdvancedColliders = b;
		this.SyncColliders();
	}

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x06004856 RID: 18518 RVA: 0x00167310 File Offset: 0x00165710
	// (set) Token: 0x06004857 RID: 18519 RVA: 0x00167318 File Offset: 0x00165718
	public bool useAdvancedColliders
	{
		get
		{
			return this._useAdvancedColliders;
		}
		set
		{
			if (this.useAdvancedCollidersJSON != null)
			{
				this.useAdvancedCollidersJSON.val = value;
			}
			else if (this._useAdvancedColliders != value)
			{
				this.SyncUseAdvancedColliders(value);
			}
		}
	}

	// Token: 0x06004858 RID: 18520 RVA: 0x0016734C File Offset: 0x0016574C
	protected void SyncColliders()
	{
		if (this._useAdvancedColliders)
		{
			foreach (Transform transform in this.regularCollidersFemale)
			{
				transform.gameObject.SetActive(false);
			}
			foreach (Transform transform2 in this.regularCollidersMale)
			{
				transform2.gameObject.SetActive(false);
			}
			foreach (Transform transform3 in this.regularColliders)
			{
				transform3.gameObject.SetActive(false);
			}
			if (this._gender == DAZCharacterSelector.Gender.Male)
			{
				foreach (Transform transform4 in this.advancedCollidersFemale)
				{
					transform4.gameObject.SetActive(false);
				}
				foreach (Transform transform5 in this.advancedCollidersMale)
				{
					transform5.gameObject.SetActive(true);
				}
			}
			else
			{
				foreach (Transform transform6 in this.advancedCollidersMale)
				{
					transform6.gameObject.SetActive(false);
				}
				foreach (Transform transform7 in this.advancedCollidersFemale)
				{
					transform7.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			foreach (Transform transform8 in this.advancedCollidersFemale)
			{
				transform8.gameObject.SetActive(false);
			}
			foreach (Transform transform9 in this.advancedCollidersMale)
			{
				transform9.gameObject.SetActive(false);
			}
			if (this._gender == DAZCharacterSelector.Gender.Male)
			{
				foreach (Transform transform10 in this.regularCollidersFemale)
				{
					transform10.gameObject.SetActive(false);
				}
				foreach (Transform transform11 in this.regularCollidersMale)
				{
					transform11.gameObject.SetActive(true);
				}
			}
			else
			{
				foreach (Transform transform12 in this.regularCollidersMale)
				{
					transform12.gameObject.SetActive(false);
				}
				foreach (Transform transform13 in this.regularCollidersFemale)
				{
					transform13.gameObject.SetActive(true);
				}
			}
			foreach (Transform transform14 in this.regularColliders)
			{
				transform14.gameObject.SetActive(true);
			}
		}
		foreach (Collider collider in this.auxBreastColliders)
		{
			collider.enabled = this.useAuxBreastColliders;
			CapsuleLineSphereCollider component = collider.GetComponent<CapsuleLineSphereCollider>();
			if (component != null)
			{
				component.enabled = this.useAuxBreastColliders;
			}
			GpuSphereCollider component2 = collider.GetComponent<GpuSphereCollider>();
			if (component2 != null)
			{
				component2.enabled = this.useAuxBreastColliders;
			}
		}
		if (Application.isPlaying)
		{
			foreach (IgnoreChildColliders ignoreChildColliders2 in this._ignoreChildColliders)
			{
				ignoreChildColliders2.SyncColliders();
			}
			foreach (DAZPhysicsMesh dazphysicsMesh in this._physicsMeshes)
			{
				dazphysicsMesh.InitColliders();
			}
			foreach (AutoColliderBatchUpdater autoColliderBatchUpdater in this._autoColliderBatchUpdaters)
			{
				if (autoColliderBatchUpdater != null)
				{
					autoColliderBatchUpdater.UpdateAutoColliders();
				}
			}
			foreach (AutoColliderGroup autoColliderGroup in this._autoColliderGroups)
			{
				if (autoColliderGroup != null && autoColliderGroup.isActiveAndEnabled)
				{
					autoColliderGroup.InitColliders();
				}
			}
			if (this._useAdvancedColliders)
			{
				this.containingAtom.ResetPhysics(false, true);
			}
		}
	}

	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x06004859 RID: 18521 RVA: 0x001677E4 File Offset: 0x00165BE4
	public GenerateDAZMorphsControlUI morphsControlUI
	{
		get
		{
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this.morphsControlMaleUI;
			}
			return this.morphsControlFemaleUI;
		}
	}

	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x0600485A RID: 18522 RVA: 0x001677FF File Offset: 0x00165BFF
	public GenerateDAZMorphsControlUI morphsControlUIOtherGender
	{
		get
		{
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this.morphsControlFemaleUI;
			}
			return this.morphsControlMaleUI;
		}
	}

	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x0600485B RID: 18523 RVA: 0x0016781A File Offset: 0x00165C1A
	public GenerateDAZMorphsControlUI morphsControlUIAlt
	{
		get
		{
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this.morphsControlMaleUIAlt;
			}
			return this.morphsControlFemaleUIAlt;
		}
	}

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x0600485C RID: 18524 RVA: 0x00167835 File Offset: 0x00165C35
	public GenerateDAZClothingSelectorUI clothingSelectorUI
	{
		get
		{
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this.clothingSelectorMaleUI;
			}
			return this.clothingSelectorFemaleUI;
		}
	}

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x0600485D RID: 18525 RVA: 0x00167850 File Offset: 0x00165C50
	public GenerateDAZHairSelectorUI hairSelectorUI
	{
		get
		{
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this.hairSelectorMaleUI;
			}
			return this.hairSelectorFemaleUI;
		}
	}

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x0600485E RID: 18526 RVA: 0x0016786B File Offset: 0x00165C6B
	public GenerateDAZHairSelectorUI hairSelectorUIAlt
	{
		get
		{
			if (this.gender == DAZCharacterSelector.Gender.Male)
			{
				return this.hairSelectorMaleUIAlt;
			}
			return this.hairSelectorFemaleUIAlt;
		}
	}

	// Token: 0x0600485F RID: 18527 RVA: 0x00167888 File Offset: 0x00165C88
	public void CopyUI()
	{
		if (this.copyUIFrom != null)
		{
			this.color1DisplayNameText = this.copyUIFrom.color1DisplayNameText;
			this.color1Picker = this.copyUIFrom.color1Picker;
			this.color1Container = this.copyUIFrom.color1Container;
			this.color2DisplayNameText = this.copyUIFrom.color2DisplayNameText;
			this.color2Picker = this.copyUIFrom.color2Picker;
			this.color2Container = this.copyUIFrom.color2Container;
			this.color3DisplayNameText = this.copyUIFrom.color3DisplayNameText;
			this.color3Picker = this.copyUIFrom.color3Picker;
			this.color3Container = this.copyUIFrom.color3Container;
			this.param1DisplayNameText = this.copyUIFrom.param1DisplayNameText;
			this.param1Slider = this.copyUIFrom.param1Slider;
			this.param1DisplayNameTextAlt = this.copyUIFrom.param1DisplayNameTextAlt;
			this.param1SliderAlt = this.copyUIFrom.param1SliderAlt;
			this.param2DisplayNameText = this.copyUIFrom.param2DisplayNameText;
			this.param2Slider = this.copyUIFrom.param2Slider;
			this.param2DisplayNameTextAlt = this.copyUIFrom.param2DisplayNameTextAlt;
			this.param2SliderAlt = this.copyUIFrom.param2SliderAlt;
			this.param3DisplayNameText = this.copyUIFrom.param3DisplayNameText;
			this.param3Slider = this.copyUIFrom.param3Slider;
			this.param3DisplayNameTextAlt = this.copyUIFrom.param3DisplayNameTextAlt;
			this.param3SliderAlt = this.copyUIFrom.param3SliderAlt;
			this.param4DisplayNameText = this.copyUIFrom.param4DisplayNameText;
			this.param4Slider = this.copyUIFrom.param4Slider;
			this.param4DisplayNameTextAlt = this.copyUIFrom.param4DisplayNameTextAlt;
			this.param4SliderAlt = this.copyUIFrom.param4SliderAlt;
			this.param5DisplayNameText = this.copyUIFrom.param5DisplayNameText;
			this.param5Slider = this.copyUIFrom.param5Slider;
			this.param5DisplayNameTextAlt = this.copyUIFrom.param5DisplayNameTextAlt;
			this.param5SliderAlt = this.copyUIFrom.param5SliderAlt;
			this.param6DisplayNameText = this.copyUIFrom.param6DisplayNameText;
			this.param6Slider = this.copyUIFrom.param6Slider;
			this.param6DisplayNameTextAlt = this.copyUIFrom.param6DisplayNameTextAlt;
			this.param6SliderAlt = this.copyUIFrom.param6SliderAlt;
			this.param7DisplayNameText = this.copyUIFrom.param7DisplayNameText;
			this.param7Slider = this.copyUIFrom.param7Slider;
			this.param7DisplayNameTextAlt = this.copyUIFrom.param7DisplayNameTextAlt;
			this.param7SliderAlt = this.copyUIFrom.param7SliderAlt;
			this.param8DisplayNameText = this.copyUIFrom.param8DisplayNameText;
			this.param8Slider = this.copyUIFrom.param8Slider;
			this.param8DisplayNameTextAlt = this.copyUIFrom.param8DisplayNameTextAlt;
			this.param8SliderAlt = this.copyUIFrom.param8SliderAlt;
			this.param9DisplayNameText = this.copyUIFrom.param9DisplayNameText;
			this.param9Slider = this.copyUIFrom.param9Slider;
			this.param9DisplayNameTextAlt = this.copyUIFrom.param9DisplayNameTextAlt;
			this.param9SliderAlt = this.copyUIFrom.param9SliderAlt;
			this.param10DisplayNameText = this.copyUIFrom.param10DisplayNameText;
			this.param10Slider = this.copyUIFrom.param10Slider;
			this.param10DisplayNameTextAlt = this.copyUIFrom.param10DisplayNameTextAlt;
			this.param10SliderAlt = this.copyUIFrom.param10SliderAlt;
			this.textureGroup1Popup = this.copyUIFrom.textureGroup1Popup;
			this.textureGroup1PopupAlt = this.copyUIFrom.textureGroup1PopupAlt;
			this.textureGroup2Popup = this.copyUIFrom.textureGroup2Popup;
			this.textureGroup2PopupAlt = this.copyUIFrom.textureGroup2PopupAlt;
			this.textureGroup3Popup = this.copyUIFrom.textureGroup3Popup;
			this.textureGroup3PopupAlt = this.copyUIFrom.textureGroup3PopupAlt;
			this.textureGroup4Popup = this.copyUIFrom.textureGroup4Popup;
			this.textureGroup4PopupAlt = this.copyUIFrom.textureGroup4PopupAlt;
			this.textureGroup5Popup = this.copyUIFrom.textureGroup5Popup;
			this.textureGroup5PopupAlt = this.copyUIFrom.textureGroup5PopupAlt;
		}
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x00167C94 File Offset: 0x00166094
	private void DisconnectCharacterOptionsUI()
	{
		if (this._selectedCharacter != null)
		{
			DAZCharacterMaterialOptions componentInChildren = this._selectedCharacter.GetComponentInChildren<DAZCharacterMaterialOptions>();
			if (componentInChildren != null)
			{
				componentInChildren.DeregisterUI();
			}
			DAZCharacterTextureControl componentInChildren2 = this._selectedCharacter.GetComponentInChildren<DAZCharacterTextureControl>();
			if (componentInChildren2 != null)
			{
				componentInChildren2.DeregisterUI();
				componentInChildren2.DeregisterUIAlt();
			}
		}
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x00167CF4 File Offset: 0x001660F4
	private void ConnectCharacterMaterialOptionsUI()
	{
		if (Application.isPlaying && this._selectedCharacter != null)
		{
			DAZCharacterMaterialOptions componentInChildren = this._selectedCharacter.GetComponentInChildren<DAZCharacterMaterialOptions>();
			if (componentInChildren != null)
			{
				componentInChildren.CheckAwake();
				componentInChildren.color1DisplayNameText = this.color1DisplayNameText;
				componentInChildren.color1Picker = this.color1Picker;
				componentInChildren.color1Container = this.color1Container;
				componentInChildren.color2DisplayNameText = this.color2DisplayNameText;
				componentInChildren.color2Picker = this.color2Picker;
				componentInChildren.color2Container = this.color2Container;
				componentInChildren.color3DisplayNameText = this.color3DisplayNameText;
				componentInChildren.color3Picker = this.color3Picker;
				componentInChildren.color3Container = this.color3Container;
				componentInChildren.param1DisplayNameText = this.param1DisplayNameText;
				componentInChildren.param1Slider = this.param1Slider;
				componentInChildren.param1DisplayNameTextAlt = this.param1DisplayNameTextAlt;
				componentInChildren.param1SliderAlt = this.param1SliderAlt;
				componentInChildren.param2DisplayNameText = this.param2DisplayNameText;
				componentInChildren.param2Slider = this.param2Slider;
				componentInChildren.param2DisplayNameTextAlt = this.param2DisplayNameTextAlt;
				componentInChildren.param2SliderAlt = this.param2SliderAlt;
				componentInChildren.param3DisplayNameText = this.param3DisplayNameText;
				componentInChildren.param3Slider = this.param3Slider;
				componentInChildren.param3DisplayNameTextAlt = this.param3DisplayNameTextAlt;
				componentInChildren.param3SliderAlt = this.param3SliderAlt;
				componentInChildren.param4DisplayNameText = this.param4DisplayNameText;
				componentInChildren.param4Slider = this.param4Slider;
				componentInChildren.param4DisplayNameTextAlt = this.param4DisplayNameTextAlt;
				componentInChildren.param4SliderAlt = this.param4SliderAlt;
				componentInChildren.param5DisplayNameText = this.param5DisplayNameText;
				componentInChildren.param5Slider = this.param5Slider;
				componentInChildren.param5DisplayNameTextAlt = this.param5DisplayNameTextAlt;
				componentInChildren.param5SliderAlt = this.param5SliderAlt;
				componentInChildren.param6DisplayNameText = this.param6DisplayNameText;
				componentInChildren.param6Slider = this.param6Slider;
				componentInChildren.param6DisplayNameTextAlt = this.param6DisplayNameTextAlt;
				componentInChildren.param6SliderAlt = this.param6SliderAlt;
				componentInChildren.param7DisplayNameText = this.param7DisplayNameText;
				componentInChildren.param7Slider = this.param7Slider;
				componentInChildren.param7DisplayNameTextAlt = this.param7DisplayNameTextAlt;
				componentInChildren.param7SliderAlt = this.param7SliderAlt;
				componentInChildren.param8DisplayNameText = this.param8DisplayNameText;
				componentInChildren.param8Slider = this.param8Slider;
				componentInChildren.param8DisplayNameTextAlt = this.param8DisplayNameTextAlt;
				componentInChildren.param8SliderAlt = this.param8SliderAlt;
				componentInChildren.param9DisplayNameText = this.param9DisplayNameText;
				componentInChildren.param9Slider = this.param9Slider;
				componentInChildren.param9DisplayNameTextAlt = this.param9DisplayNameTextAlt;
				componentInChildren.param9SliderAlt = this.param9SliderAlt;
				componentInChildren.param10DisplayNameText = this.param10DisplayNameText;
				componentInChildren.param10Slider = this.param10Slider;
				componentInChildren.param10DisplayNameTextAlt = this.param10DisplayNameTextAlt;
				componentInChildren.param10SliderAlt = this.param10SliderAlt;
				componentInChildren.textureGroup1Popup = this.textureGroup1Popup;
				componentInChildren.textureGroup1PopupAlt = this.textureGroup1PopupAlt;
				componentInChildren.textureGroup2Popup = this.textureGroup2Popup;
				componentInChildren.textureGroup2PopupAlt = this.textureGroup2PopupAlt;
				componentInChildren.textureGroup3Popup = this.textureGroup3Popup;
				componentInChildren.textureGroup3PopupAlt = this.textureGroup3PopupAlt;
				componentInChildren.textureGroup4Popup = this.textureGroup4Popup;
				componentInChildren.textureGroup4PopupAlt = this.textureGroup4PopupAlt;
				componentInChildren.textureGroup5Popup = this.textureGroup5Popup;
				componentInChildren.textureGroup5PopupAlt = this.textureGroup5PopupAlt;
				componentInChildren.restoreAllFromDefaultsAction.button = this.restoreMaterialFromDefaultsButton;
				componentInChildren.restoreAllFromDefaultsAction.buttonAlt = this.restoreMaterialFromDefaultsButtonAlt;
				componentInChildren.restoreAllFromStore1Action.button = this.restoreMaterialFromStore1Button;
				componentInChildren.restoreAllFromStore1Action.buttonAlt = this.restoreMaterialFromStore1ButtonAlt;
				componentInChildren.restoreAllFromStore2Action.button = this.restoreMaterialFromStore2Button;
				componentInChildren.restoreAllFromStore2Action.buttonAlt = this.restoreMaterialFromStore2ButtonAlt;
				componentInChildren.restoreAllFromStore3Action.button = this.restoreMaterialFromStore3Button;
				componentInChildren.restoreAllFromStore3Action.buttonAlt = this.restoreMaterialFromStore3ButtonAlt;
				componentInChildren.saveToStore1Action.button = this.saveMaterialToStore1Button;
				componentInChildren.saveToStore1Action.buttonAlt = this.saveMaterialToStore1ButtonAlt;
				componentInChildren.saveToStore2Action.button = this.saveMaterialToStore2Button;
				componentInChildren.saveToStore2Action.buttonAlt = this.saveMaterialToStore2ButtonAlt;
				componentInChildren.saveToStore3Action.button = this.saveMaterialToStore3Button;
				componentInChildren.saveToStore3Action.buttonAlt = this.saveMaterialToStore3ButtonAlt;
				componentInChildren.InitUI();
				componentInChildren.InitUIAlt();
			}
			DAZCharacterTextureControl componentInChildren2 = this._selectedCharacter.GetComponentInChildren<DAZCharacterTextureControl>();
			if (componentInChildren2 != null)
			{
				if (this.characterTextureUITab != null)
				{
					this.characterTextureUITab.gameObject.SetActive(true);
				}
				if (this.characterTextureUI != null)
				{
					componentInChildren2.SetUI(this.characterTextureUI.transform);
				}
				if (this.characterTextureUITabAlt != null)
				{
					this.characterTextureUITabAlt.gameObject.SetActive(true);
				}
				if (this.characterTextureUIAlt != null)
				{
					componentInChildren2.SetUIAlt(this.characterTextureUIAlt.transform);
				}
			}
			else
			{
				if (this.characterTextureUITab != null)
				{
					this.characterTextureUITab.gameObject.SetActive(false);
				}
				if (this.characterTextureUITabAlt != null)
				{
					this.characterTextureUITabAlt.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06004862 RID: 18530 RVA: 0x001681E4 File Offset: 0x001665E4
	public void InitComponents()
	{
		this._eyelidControl = base.GetComponentInChildren<DAZMeshEyelidControl>();
		this._characterRun = base.GetComponentInChildren<DAZCharacterRun>();
		if (this._characterRun != null)
		{
			this._characterRun.characterSelector = this;
		}
		this._physicsMeshes = base.GetComponentsInChildren<DAZPhysicsMesh>(true);
		this._setDAZMorphs = base.GetComponentsInChildren<SetDAZMorph>(true);
		this.lipSync = base.GetComponentInChildren<SpeechBlend>(true);
		this._autoColliderBatchUpdaters = this.rootBones.GetComponentsInChildren<AutoColliderBatchUpdater>(true);
		this._autoColliderGroups = this.rootBones.GetComponentsInChildren<AutoColliderGroup>(true);
		this._autoColliders = this.rootBones.GetComponentsInChildren<AutoCollider>(true);
		this._setAnchorFromVertexComps = this.rootBones.GetComponentsInChildren<SetAnchorFromVertex>(true);
		this._ignoreChildColliders = this.rootBones.GetComponentsInChildren<IgnoreChildColliders>(true);
		List<DAZCharacterMaterialOptions> list = new List<DAZCharacterMaterialOptions>();
		DAZCharacterMaterialOptions[] componentsInChildren = base.GetComponentsInChildren<DAZCharacterMaterialOptions>();
		foreach (DAZCharacterMaterialOptions dazcharacterMaterialOptions in componentsInChildren)
		{
			DAZCharacter component = dazcharacterMaterialOptions.GetComponent<DAZCharacter>();
			DAZSkinV2 component2 = dazcharacterMaterialOptions.GetComponent<DAZSkinV2>();
			if (component == null && component2 == null && dazcharacterMaterialOptions != this.femaleEyelashMaterialOptions && dazcharacterMaterialOptions != this.maleEyelashMaterialOptions)
			{
				list.Add(dazcharacterMaterialOptions);
			}
		}
		this._materialOptions = list.ToArray();
	}

	// Token: 0x06004863 RID: 18531 RVA: 0x00168334 File Offset: 0x00166734
	protected void InitJSONParams()
	{
		if (Application.isPlaying)
		{
			this.useAdvancedCollidersJSON = new JSONStorableBool("useAdvancedColliders", this._useAdvancedColliders, new JSONStorableBool.SetBoolCallback(this.SyncUseAdvancedColliders));
			this.useAdvancedCollidersJSON.storeType = JSONStorableParam.StoreType.Any;
			base.RegisterBool(this.useAdvancedCollidersJSON);
			this.useAuxBreastCollidersJSON = new JSONStorableBool("useAuxBreastColliders", this._useAuxBreastColliders, new JSONStorableBool.SetBoolCallback(this.SyncUseAuxBreastColliders));
			this.useAuxBreastCollidersJSON.storeType = JSONStorableParam.StoreType.Any;
			base.RegisterBool(this.useAuxBreastCollidersJSON);
			this.disableAnatomyJSON = new JSONStorableBool("disableAnatomy", this._disableAnatomy, new JSONStorableBool.SetBoolCallback(this.SyncDisableAnatomy));
			base.RegisterBool(this.disableAnatomyJSON);
			this.onlyShowFavoriteMorphsInParametersListsJSON = new JSONStorableBool("onlyShowFavoriteMorphsInParametersLists", this._onlyShowFavoriteMorphsInParameterLists, new JSONStorableBool.SetBoolCallback(this.SyncOnlyShowFavoriteMorphsInParameterLists));
			this.useMaleMorphsOnFemaleJSON = new JSONStorableBool("useMaleMorphsOnFemale", this._useMaleMorphsOnFemale, new JSONStorableBool.SetBoolCallback(this.SyncUseMaleMorphsOnFemale));
			base.RegisterBool(this.useMaleMorphsOnFemaleJSON);
			this.useFemaleMorphsOnMaleJSON = new JSONStorableBool("useFemaleMorphsOnMale", this._useFemaleMorphsOnMale, new JSONStorableBool.SetBoolCallback(this.SyncUseFemaleMorphsOnMale));
			base.RegisterBool(this.useFemaleMorphsOnMaleJSON);
			this.unloadCharactersWhenSwitchingJSON = new JSONStorableBool("unloadCharactersWhenSwitching", this._unloadCharactersWhenSwitching, new JSONStorableBool.SetBoolCallback(this.SyncUnloadCharactersWhenSwitching));
			this.unloadCharactersWhenSwitchingJSON.isStorable = false;
			this.unloadCharactersWhenSwitchingJSON.isRestorable = false;
			base.RegisterBool(this.unloadCharactersWhenSwitchingJSON);
			this.unloadInactiveObjectsJSONAction = new JSONStorableAction("UnloadInactiveObjects", new JSONStorableAction.ActionCallback(this.UnloadInactiveObjects));
			base.RegisterAction(this.unloadInactiveObjectsJSONAction);
			this.removeAllClothingAction = new JSONStorableAction("RemoveAllClothing", new JSONStorableAction.ActionCallback(this.RemoveAllClothing));
			base.RegisterAction(this.removeAllClothingAction);
			this.removeAllRealClothingAction = new JSONStorableAction("RemoveAllRealClothing", new JSONStorableAction.ActionCallback(this.RemoveAllRealClothing));
			base.RegisterAction(this.removeAllRealClothingAction);
			this.undressAllClothingAction = new JSONStorableAction("UndressAllClothing", new JSONStorableAction.ActionCallback(this.EnableUndressAllClothingItems));
			base.RegisterAction(this.undressAllClothingAction);
		}
	}

	// Token: 0x06004864 RID: 18532 RVA: 0x00168551 File Offset: 0x00166951
	protected void EarlyInit()
	{
		this.EarlyInitClothingItems();
		this.EarlyInitHairItems();
		this.EarlyInitCharacters();
	}

	// Token: 0x06004865 RID: 18533 RVA: 0x00168568 File Offset: 0x00166968
	public void Init(bool genderChange = false)
	{
		if (!this.wasInit || genderChange)
		{
			this.wasInit = true;
			if (!genderChange)
			{
				this.InitMorphBanks();
				this.InitBones();
				this.InitComponents();
				this.InitCharacters();
				this.RefreshDynamicClothes();
				this.RefreshDynamicHair();
			}
			this.InitClothingItems();
			this.InitHairItems();
			this.SyncAnatomy();
		}
	}

	// Token: 0x06004866 RID: 18534 RVA: 0x001685CC File Offset: 0x001669CC
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			DAZCharacterSelectorUI componentInChildren = this.UITransform.GetComponentInChildren<DAZCharacterSelectorUI>();
			if (componentInChildren != null)
			{
				this.useAdvancedCollidersJSON.toggle = componentInChildren.useAdvancedCollidersToggle;
				this.useAuxBreastCollidersJSON.toggle = componentInChildren.useAuxBreastCollidersToggle;
				this.disableAnatomyJSON.toggle = componentInChildren.disableAnatomyToggle;
				this.onlyShowFavoriteMorphsInParametersListsJSON.toggle = componentInChildren.onlyShowFavoriteMorphsInParameterListsToggle;
				this.useMaleMorphsOnFemaleJSON.toggle = componentInChildren.useMaleMorphsOnFemaleToggle;
				this.useFemaleMorphsOnMaleJSON.toggle = componentInChildren.useFemaleMorphsOnMaleToggle;
				this.unloadCharactersWhenSwitchingJSON.toggle = componentInChildren.unloadCharactersWhenSwitchingToggle;
				this.unloadInactiveObjectsJSONAction.button = componentInChildren.unloadInactiveObjectsButton;
				this.removeAllClothingAction.button = componentInChildren.removeAllClothingButton;
				this.removeAllClothingAction.buttonAlt = componentInChildren.removeAllClothingButtonAlt;
				this.removeAllRealClothingAction.button = componentInChildren.removeAllRealClothingButton;
				this.removeAllRealClothingAction.buttonAlt = componentInChildren.removeAllRealClothingButtonAlt;
				this.undressAllClothingAction.button = componentInChildren.undressAllClothingButton;
				this.undressAllClothingAction.buttonAlt = componentInChildren.undressAllClothingButtonAlt;
			}
		}
	}

	// Token: 0x06004867 RID: 18535 RVA: 0x001686F0 File Offset: 0x00166AF0
	protected void ReportMemory(List<string> reports)
	{
		int num = 0;
		if (this._femaleCharacters != null)
		{
			foreach (DAZCharacter dazcharacter in this._femaleCharacters)
			{
				if (dazcharacter.ready)
				{
					num++;
				}
			}
		}
		if (this._maleCharacters != null)
		{
			foreach (DAZCharacter dazcharacter2 in this._maleCharacters)
			{
				if (dazcharacter2.ready)
				{
					num++;
				}
			}
		}
		reports.Add(this.containingAtom.uid + " loaded characters count: " + num);
		int num2 = 0;
		if (this._femaleClothingItems != null)
		{
			foreach (DAZClothingItem dazclothingItem in this._femaleClothingItems)
			{
				if (dazclothingItem.ready)
				{
					num2++;
				}
			}
		}
		if (this._maleClothingItems != null)
		{
			foreach (DAZClothingItem dazclothingItem2 in this._maleClothingItems)
			{
				if (dazclothingItem2.ready)
				{
					num2++;
				}
			}
		}
		reports.Add(this.containingAtom.uid + " loaded clothing items count: " + num2);
		int num3 = 0;
		if (this._femaleHairItems != null)
		{
			foreach (DAZHairGroup dazhairGroup in this._femaleHairItems)
			{
				if (dazhairGroup.ready)
				{
					num3++;
				}
			}
		}
		if (this._maleHairItems != null)
		{
			foreach (DAZHairGroup dazhairGroup2 in this._maleHairItems)
			{
				if (dazhairGroup2.ready)
				{
					num3++;
				}
			}
		}
		reports.Add(this.containingAtom.uid + " loaded hair items count: " + num3);
		reports.Add(this.containingAtom.uid + " loaded runtime morph count: " + this.GetRuntimeMorphDeltasLoadedCount());
	}

	// Token: 0x06004868 RID: 18536 RVA: 0x00168920 File Offset: 0x00166D20
	protected void OptimizeMemory()
	{
		if (this.containingAtom.keepParamLocksWhenPuttingBackInPool && this.containingAtom.inPool)
		{
			if (!base.appearanceLocked && !base.IsCustomAppearanceParamLocked("character"))
			{
				this.UnloadInactiveCharacters();
			}
			else
			{
				this.UnloadDisabledCharacters();
			}
			if (!base.appearanceLocked && !base.IsCustomAppearanceParamLocked("clothing"))
			{
				this.UnloadInactiveClothingItems();
			}
			else
			{
				this.UnloadDisabledClothingItems();
			}
			if (base.appearanceLocked || base.IsCustomAppearanceParamLocked("hair"))
			{
				this.UnloadDisabledHairItems();
			}
		}
		else
		{
			this.UnloadInactiveCharacters();
			this.UnloadInactiveClothingItems();
			this.UnloadInactiveHairItems();
		}
		this.UnloadRuntimeMorphDeltas();
		this.UnloadDemandActivatedMorphs();
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x001689F0 File Offset: 0x00166DF0
	private void OnDisable()
	{
		if (this._selectedCharacter != null)
		{
			DAZCharacter selectedCharacter = this._selectedCharacter;
			selectedCharacter.onLoadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Remove(selectedCharacter.onLoadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterLoaded));
			DAZCharacter selectedCharacter2 = this._selectedCharacter;
			selectedCharacter2.onPreloadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Remove(selectedCharacter2.onPreloadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterPreloaded));
		}
		if (this.onCharacterLoadedFlag != null)
		{
			this.onCharacterLoadedFlag.Raise();
			this.onCharacterLoadedFlag = null;
		}
		if (this.delayResumeFlag != null)
		{
			this.delayResumeFlag.Raise();
		}
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x00168A90 File Offset: 0x00166E90
	private void OnEnable()
	{
		this.Init(false);
		if (this._selectedCharacter != null && (this._selectedCharacter != this._loadedCharacter || (this._characterRun != null && this._characterRun.skin == null)))
		{
			DAZCharacter selectedCharacter = this._selectedCharacter;
			selectedCharacter.onLoadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Combine(selectedCharacter.onLoadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterLoaded));
			DAZCharacter selectedCharacter2 = this._selectedCharacter;
			selectedCharacter2.onPreloadedHandlers = (JSONStorableDynamic.OnLoaded)Delegate.Combine(selectedCharacter2.onPreloadedHandlers, new JSONStorableDynamic.OnLoaded(this.OnCharacterPreloaded));
		}
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x00168B40 File Offset: 0x00166F40
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.EarlyInit();
			this.Init(false);
			this.InitJSONParams();
			this.InitUI();
		}
	}

	// Token: 0x0600486C RID: 18540 RVA: 0x00168B6C File Offset: 0x00166F6C
	private void Start()
	{
		this.SyncGender();
		if (Application.isPlaying)
		{
			this.SelectCharacterByName(this.startingCharacterName, false);
			this.ResetClothing(false);
			this.ResetHair(false);
			if (MemoryOptimizer.singleton != null)
			{
				MemoryOptimizer.singleton.RegisterMemoryOptimizerListener(new MemoryOptimizer.MemoryOptimizerCallback(this.OptimizeMemory));
				MemoryOptimizer.singleton.RegisterMemoryOptimizerReporter(new MemoryOptimizer.MemoryOptimizerReporter(this.ReportMemory));
			}
			FileManager.RegisterRefreshHandler(new OnRefresh(this.RefreshDynamicItems));
		}
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x00168BF4 File Offset: 0x00166FF4
	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			if (MemoryOptimizer.singleton != null)
			{
				MemoryOptimizer.singleton.DeregisterMemoryOptimizerListener(new MemoryOptimizer.MemoryOptimizerCallback(this.OptimizeMemory));
				MemoryOptimizer.singleton.DeregisterMemoryOptimizerReporter(new MemoryOptimizer.MemoryOptimizerReporter(this.ReportMemory));
			}
			FileManager.UnregisterRefreshHandler(new OnRefresh(this.RefreshDynamicItems));
		}
	}

	// Token: 0x0400359E RID: 13726
	protected string[] customParamNames = new string[]
	{
		"character",
		"clothing",
		"hair",
		"morphs",
		"morphsOtherGender"
	};

	// Token: 0x0400359F RID: 13727
	protected DAZSkinV2.SkinMethod saveSkinMethod;

	// Token: 0x040035A0 RID: 13728
	protected DAZSkinV2 exportSkin;

	// Token: 0x040035A1 RID: 13729
	public DAZBones rootBones;

	// Token: 0x040035A2 RID: 13730
	public string rootBonesName = "Genesis2";

	// Token: 0x040035A3 RID: 13731
	public string rootBonesNameMale = "Genesis2Male";

	// Token: 0x040035A4 RID: 13732
	public string rootBonesNameFemale = "Genesis2Female";

	// Token: 0x040035A5 RID: 13733
	[HideInInspector]
	[SerializeField]
	protected DAZCharacterSelector.Gender _gender;

	// Token: 0x040035A6 RID: 13734
	public Transform[] maleTransforms;

	// Token: 0x040035A7 RID: 13735
	public Transform[] femaleTransforms;

	// Token: 0x040035A8 RID: 13736
	public AdjustJoints femaleBreastAdjustJoints;

	// Token: 0x040035A9 RID: 13737
	public AdjustJoints femaleGluteAdjustJoints;

	// Token: 0x040035AA RID: 13738
	protected bool _disableAnatomy;

	// Token: 0x040035AB RID: 13739
	protected JSONStorableBool disableAnatomyJSON;

	// Token: 0x040035AC RID: 13740
	public int[] maleAnatomyOnMaterialSlots;

	// Token: 0x040035AD RID: 13741
	public int[] maleAnatomyOffMaterialSlots;

	// Token: 0x040035AE RID: 13742
	public int[] femaleAnatomyOnMaterialSlots;

	// Token: 0x040035AF RID: 13743
	public int[] femaleAnatomyOffMaterialSlots;

	// Token: 0x040035B0 RID: 13744
	private DAZMaleAnatomy[] maleAnatomyComponents;

	// Token: 0x040035B1 RID: 13745
	public Transform morphBankContainer;

	// Token: 0x040035B2 RID: 13746
	public Transform femaleMorphBank1Prefab;

	// Token: 0x040035B3 RID: 13747
	public Transform femaleMorphBank2Prefab;

	// Token: 0x040035B4 RID: 13748
	public Transform femaleMorphBank3Prefab;

	// Token: 0x040035B5 RID: 13749
	public Transform maleMorphBank1Prefab;

	// Token: 0x040035B6 RID: 13750
	public Transform maleMorphBank2Prefab;

	// Token: 0x040035B7 RID: 13751
	public Transform maleMorphBank3Prefab;

	// Token: 0x040035B8 RID: 13752
	public DAZMorphBank femaleMorphBank1;

	// Token: 0x040035B9 RID: 13753
	public DAZMorphBank femaleMorphBank2;

	// Token: 0x040035BA RID: 13754
	public DAZMorphBank femaleMorphBank3;

	// Token: 0x040035BB RID: 13755
	public DAZMorphBank maleMorphBank1;

	// Token: 0x040035BC RID: 13756
	public DAZMorphBank maleMorphBank2;

	// Token: 0x040035BD RID: 13757
	public DAZMorphBank maleMorphBank3;

	// Token: 0x040035BE RID: 13758
	protected HashSet<string> registeredMorphNames;

	// Token: 0x040035BF RID: 13759
	protected bool _onlyShowFavoriteMorphsInParameterLists = true;

	// Token: 0x040035C0 RID: 13760
	protected JSONStorableBool onlyShowFavoriteMorphsInParametersListsJSON;

	// Token: 0x040035C1 RID: 13761
	protected bool _useMaleMorphsOnFemale;

	// Token: 0x040035C2 RID: 13762
	protected JSONStorableBool useMaleMorphsOnFemaleJSON;

	// Token: 0x040035C3 RID: 13763
	protected bool _useFemaleMorphsOnMale;

	// Token: 0x040035C4 RID: 13764
	protected JSONStorableBool useFemaleMorphsOnMaleJSON;

	// Token: 0x040035C5 RID: 13765
	public Transform maleCharactersContainer;

	// Token: 0x040035C6 RID: 13766
	public Transform femaleCharactersContainer;

	// Token: 0x040035C7 RID: 13767
	public Transform femaleCharactersPrefab;

	// Token: 0x040035C8 RID: 13768
	public Transform maleCharactersPrefab;

	// Token: 0x040035C9 RID: 13769
	protected JSONStorableStringChooser characterChooserJSON;

	// Token: 0x040035CA RID: 13770
	protected bool _unloadCharactersWhenSwitching;

	// Token: 0x040035CB RID: 13771
	protected JSONStorableBool unloadCharactersWhenSwitchingJSON;

	// Token: 0x040035CC RID: 13772
	public string startingCharacterName;

	// Token: 0x040035CD RID: 13773
	private Dictionary<string, DAZCharacter> _characterByName;

	// Token: 0x040035CE RID: 13774
	private DAZCharacter[] _femaleCharacters;

	// Token: 0x040035CF RID: 13775
	private DAZCharacter[] _maleCharacters;

	// Token: 0x040035D0 RID: 13776
	private DAZCharacter[] _characters;

	// Token: 0x040035D1 RID: 13777
	private DAZCharacter _selectedCharacter;

	// Token: 0x040035D2 RID: 13778
	protected AsyncFlag delayResumeFlag;

	// Token: 0x040035D3 RID: 13779
	protected AsyncFlag onCharacterLoadedFlag;

	// Token: 0x040035D4 RID: 13780
	protected bool _loadedGenderChange;

	// Token: 0x040035D5 RID: 13781
	public DAZCharacter femalePlaceholderCharacter;

	// Token: 0x040035D6 RID: 13782
	public DAZCharacter malePlaceholderCharacter;

	// Token: 0x040035D7 RID: 13783
	protected DAZCharacter _loadedCharacter;

	// Token: 0x040035D8 RID: 13784
	public Transform UIBucketForDynamicItems;

	// Token: 0x040035D9 RID: 13785
	public DAZBone hipBone;

	// Token: 0x040035DA RID: 13786
	public DAZBone pelvisBone;

	// Token: 0x040035DB RID: 13787
	public DAZBone chestBone;

	// Token: 0x040035DC RID: 13788
	public DAZBone headBone;

	// Token: 0x040035DD RID: 13789
	public DAZBone leftHandBone;

	// Token: 0x040035DE RID: 13790
	public DAZBone rightHandBone;

	// Token: 0x040035DF RID: 13791
	public DAZBone leftFootBone;

	// Token: 0x040035E0 RID: 13792
	public DAZBone rightFootBone;

	// Token: 0x040035E1 RID: 13793
	protected HashSet<string> alreadyReportedDuplicates;

	// Token: 0x040035E2 RID: 13794
	protected Coroutine refreshCoroutine;

	// Token: 0x040035E3 RID: 13795
	public Transform maleClothingContainer;

	// Token: 0x040035E4 RID: 13796
	public Transform femaleClothingContainer;

	// Token: 0x040035E5 RID: 13797
	public Transform maleClothingPrefab;

	// Token: 0x040035E6 RID: 13798
	public Transform femaleClothingPrefab;

	// Token: 0x040035E7 RID: 13799
	public Transform dynamicClothingItemPrefab;

	// Token: 0x040035E8 RID: 13800
	public DAZClothingItem maleClothingCreatorItem;

	// Token: 0x040035E9 RID: 13801
	public DAZClothingItem femaleClothingCreatorItem;

	// Token: 0x040035EA RID: 13802
	public BoxCollider leftShoeCollider;

	// Token: 0x040035EB RID: 13803
	public BoxCollider rightShoeCollider;

	// Token: 0x040035EC RID: 13804
	public FreeControllerV3 leftFootController;

	// Token: 0x040035ED RID: 13805
	public FreeControllerV3 rightFootController;

	// Token: 0x040035EE RID: 13806
	public FreeControllerV3 leftToeController;

	// Token: 0x040035EF RID: 13807
	public FreeControllerV3 rightToeController;

	// Token: 0x040035F0 RID: 13808
	protected Dictionary<string, JSONStorableBool> clothingItemJSONs;

	// Token: 0x040035F1 RID: 13809
	protected List<JSONStorableAction> clothingItemToggleJSONs;

	// Token: 0x040035F2 RID: 13810
	protected Dictionary<string, DAZClothingItem> _clothingItemById;

	// Token: 0x040035F3 RID: 13811
	protected Dictionary<string, DAZClothingItem> _clothingItemByBackupId;

	// Token: 0x040035F4 RID: 13812
	protected DAZClothingItem[] _maleClothingItems;

	// Token: 0x040035F5 RID: 13813
	protected DAZClothingItem[] _femaleClothingItems;

	// Token: 0x040035F6 RID: 13814
	protected JSONStorableAction undressAllClothingAction;

	// Token: 0x040035F7 RID: 13815
	protected JSONStorableAction removeAllClothingAction;

	// Token: 0x040035F8 RID: 13816
	protected JSONStorableAction removeAllRealClothingAction;

	// Token: 0x040035F9 RID: 13817
	public Transform maleHairContainer;

	// Token: 0x040035FA RID: 13818
	public Transform femaleHairContainer;

	// Token: 0x040035FB RID: 13819
	public Transform maleHairPrefab;

	// Token: 0x040035FC RID: 13820
	public Transform femaleHairPrefab;

	// Token: 0x040035FD RID: 13821
	public Transform dynamicHairItemPrefab;

	// Token: 0x040035FE RID: 13822
	public DAZHairGroup maleHairCreatorItem;

	// Token: 0x040035FF RID: 13823
	public DAZHairGroup femaleHairCreatorItem;

	// Token: 0x04003600 RID: 13824
	protected Dictionary<string, JSONStorableBool> hairItemJSONs;

	// Token: 0x04003601 RID: 13825
	protected List<JSONStorableAction> hairItemToggleJSONs;

	// Token: 0x04003602 RID: 13826
	protected Dictionary<string, DAZHairGroup> _hairItemById;

	// Token: 0x04003603 RID: 13827
	protected Dictionary<string, DAZHairGroup> _hairItemByBackupId;

	// Token: 0x04003604 RID: 13828
	protected DAZHairGroup[] _maleHairItems;

	// Token: 0x04003605 RID: 13829
	protected DAZHairGroup[] _femaleHairItems;

	// Token: 0x04003606 RID: 13830
	protected JSONStorableAction unloadInactiveObjectsJSONAction;

	// Token: 0x04003607 RID: 13831
	public Collider[] auxBreastColliders;

	// Token: 0x04003608 RID: 13832
	protected JSONStorableBool useAuxBreastCollidersJSON;

	// Token: 0x04003609 RID: 13833
	[SerializeField]
	protected bool _useAuxBreastColliders = true;

	// Token: 0x0400360A RID: 13834
	public Transform[] regularColliders;

	// Token: 0x0400360B RID: 13835
	public Transform[] regularCollidersFemale;

	// Token: 0x0400360C RID: 13836
	public Transform[] regularCollidersMale;

	// Token: 0x0400360D RID: 13837
	public Transform[] advancedCollidersFemale;

	// Token: 0x0400360E RID: 13838
	public Transform[] advancedCollidersMale;

	// Token: 0x0400360F RID: 13839
	protected JSONStorableBool useAdvancedCollidersJSON;

	// Token: 0x04003610 RID: 13840
	[SerializeField]
	protected bool _useAdvancedColliders;

	// Token: 0x04003611 RID: 13841
	public Transform customUIBucket;

	// Token: 0x04003612 RID: 13842
	public GenerateDAZCharacterSelectorUI characterSelectorUI;

	// Token: 0x04003613 RID: 13843
	public GenerateDAZCharacterSelectorUI characterSelectorUIAlt;

	// Token: 0x04003614 RID: 13844
	public GenerateDAZMorphsControlUI morphsControlFemaleUI;

	// Token: 0x04003615 RID: 13845
	public GenerateDAZMorphsControlUI morphsControlFemaleUIAlt;

	// Token: 0x04003616 RID: 13846
	public GenerateDAZMorphsControlUI morphsControlMaleUI;

	// Token: 0x04003617 RID: 13847
	public GenerateDAZMorphsControlUI morphsControlMaleUIAlt;

	// Token: 0x04003618 RID: 13848
	public GenerateDAZClothingSelectorUI clothingSelectorFemaleUI;

	// Token: 0x04003619 RID: 13849
	public GenerateDAZClothingSelectorUI clothingSelectorMaleUI;

	// Token: 0x0400361A RID: 13850
	public GenerateDAZHairSelectorUI hairSelectorFemaleUI;

	// Token: 0x0400361B RID: 13851
	public GenerateDAZHairSelectorUI hairSelectorFemaleUIAlt;

	// Token: 0x0400361C RID: 13852
	public GenerateDAZHairSelectorUI hairSelectorMaleUI;

	// Token: 0x0400361D RID: 13853
	public GenerateDAZHairSelectorUI hairSelectorMaleUIAlt;

	// Token: 0x0400361E RID: 13854
	public DAZCharacterTextureControlUI characterTextureUI;

	// Token: 0x0400361F RID: 13855
	public Transform characterTextureUITab;

	// Token: 0x04003620 RID: 13856
	public DAZCharacterTextureControlUI characterTextureUIAlt;

	// Token: 0x04003621 RID: 13857
	public Transform characterTextureUITabAlt;

	// Token: 0x04003622 RID: 13858
	public DAZCharacterMaterialOptions copyUIFrom;

	// Token: 0x04003623 RID: 13859
	public Text color1DisplayNameText;

	// Token: 0x04003624 RID: 13860
	public HSVColorPicker color1Picker;

	// Token: 0x04003625 RID: 13861
	public RectTransform color1Container;

	// Token: 0x04003626 RID: 13862
	public Text color2DisplayNameText;

	// Token: 0x04003627 RID: 13863
	public HSVColorPicker color2Picker;

	// Token: 0x04003628 RID: 13864
	public RectTransform color2Container;

	// Token: 0x04003629 RID: 13865
	public Text color3DisplayNameText;

	// Token: 0x0400362A RID: 13866
	public HSVColorPicker color3Picker;

	// Token: 0x0400362B RID: 13867
	public RectTransform color3Container;

	// Token: 0x0400362C RID: 13868
	public Text param1DisplayNameText;

	// Token: 0x0400362D RID: 13869
	public Slider param1Slider;

	// Token: 0x0400362E RID: 13870
	public Text param1DisplayNameTextAlt;

	// Token: 0x0400362F RID: 13871
	public Slider param1SliderAlt;

	// Token: 0x04003630 RID: 13872
	public Text param2DisplayNameText;

	// Token: 0x04003631 RID: 13873
	public Slider param2Slider;

	// Token: 0x04003632 RID: 13874
	public Text param2DisplayNameTextAlt;

	// Token: 0x04003633 RID: 13875
	public Slider param2SliderAlt;

	// Token: 0x04003634 RID: 13876
	public Text param3DisplayNameText;

	// Token: 0x04003635 RID: 13877
	public Slider param3Slider;

	// Token: 0x04003636 RID: 13878
	public Text param3DisplayNameTextAlt;

	// Token: 0x04003637 RID: 13879
	public Slider param3SliderAlt;

	// Token: 0x04003638 RID: 13880
	public Text param4DisplayNameText;

	// Token: 0x04003639 RID: 13881
	public Slider param4Slider;

	// Token: 0x0400363A RID: 13882
	public Text param4DisplayNameTextAlt;

	// Token: 0x0400363B RID: 13883
	public Slider param4SliderAlt;

	// Token: 0x0400363C RID: 13884
	public Text param5DisplayNameText;

	// Token: 0x0400363D RID: 13885
	public Slider param5Slider;

	// Token: 0x0400363E RID: 13886
	public Text param5DisplayNameTextAlt;

	// Token: 0x0400363F RID: 13887
	public Slider param5SliderAlt;

	// Token: 0x04003640 RID: 13888
	public Text param6DisplayNameText;

	// Token: 0x04003641 RID: 13889
	public Slider param6Slider;

	// Token: 0x04003642 RID: 13890
	public Text param6DisplayNameTextAlt;

	// Token: 0x04003643 RID: 13891
	public Slider param6SliderAlt;

	// Token: 0x04003644 RID: 13892
	public Text param7DisplayNameText;

	// Token: 0x04003645 RID: 13893
	public Slider param7Slider;

	// Token: 0x04003646 RID: 13894
	public Text param7DisplayNameTextAlt;

	// Token: 0x04003647 RID: 13895
	public Slider param7SliderAlt;

	// Token: 0x04003648 RID: 13896
	public Text param8DisplayNameText;

	// Token: 0x04003649 RID: 13897
	public Slider param8Slider;

	// Token: 0x0400364A RID: 13898
	public Text param8DisplayNameTextAlt;

	// Token: 0x0400364B RID: 13899
	public Slider param8SliderAlt;

	// Token: 0x0400364C RID: 13900
	public Text param9DisplayNameText;

	// Token: 0x0400364D RID: 13901
	public Slider param9Slider;

	// Token: 0x0400364E RID: 13902
	public Text param9DisplayNameTextAlt;

	// Token: 0x0400364F RID: 13903
	public Slider param9SliderAlt;

	// Token: 0x04003650 RID: 13904
	public Text param10DisplayNameText;

	// Token: 0x04003651 RID: 13905
	public Slider param10Slider;

	// Token: 0x04003652 RID: 13906
	public Text param10DisplayNameTextAlt;

	// Token: 0x04003653 RID: 13907
	public Slider param10SliderAlt;

	// Token: 0x04003654 RID: 13908
	public UIPopup textureGroup1Popup;

	// Token: 0x04003655 RID: 13909
	public Text textureGroup1Text;

	// Token: 0x04003656 RID: 13910
	public UIPopup textureGroup1PopupAlt;

	// Token: 0x04003657 RID: 13911
	public Text textureGroup1TextAlt;

	// Token: 0x04003658 RID: 13912
	public UIPopup textureGroup2Popup;

	// Token: 0x04003659 RID: 13913
	public Text textureGroup2Text;

	// Token: 0x0400365A RID: 13914
	public UIPopup textureGroup2PopupAlt;

	// Token: 0x0400365B RID: 13915
	public Text textureGroup2TextAlt;

	// Token: 0x0400365C RID: 13916
	public UIPopup textureGroup3Popup;

	// Token: 0x0400365D RID: 13917
	public Text textureGroup3Text;

	// Token: 0x0400365E RID: 13918
	public UIPopup textureGroup3PopupAlt;

	// Token: 0x0400365F RID: 13919
	public Text textureGroup3TextAlt;

	// Token: 0x04003660 RID: 13920
	public UIPopup textureGroup4Popup;

	// Token: 0x04003661 RID: 13921
	public Text textureGroup4Text;

	// Token: 0x04003662 RID: 13922
	public UIPopup textureGroup4PopupAlt;

	// Token: 0x04003663 RID: 13923
	public Text textureGroup4TextAlt;

	// Token: 0x04003664 RID: 13924
	public UIPopup textureGroup5Popup;

	// Token: 0x04003665 RID: 13925
	public Text textureGroup5Text;

	// Token: 0x04003666 RID: 13926
	public UIPopup textureGroup5PopupAlt;

	// Token: 0x04003667 RID: 13927
	public Text textureGroup5TextAlt;

	// Token: 0x04003668 RID: 13928
	public Button restoreMaterialFromDefaultsButton;

	// Token: 0x04003669 RID: 13929
	public Button saveMaterialToStore1Button;

	// Token: 0x0400366A RID: 13930
	public Button restoreMaterialFromStore1Button;

	// Token: 0x0400366B RID: 13931
	public Button saveMaterialToStore2Button;

	// Token: 0x0400366C RID: 13932
	public Button restoreMaterialFromStore2Button;

	// Token: 0x0400366D RID: 13933
	public Button saveMaterialToStore3Button;

	// Token: 0x0400366E RID: 13934
	public Button restoreMaterialFromStore3Button;

	// Token: 0x0400366F RID: 13935
	public Button restoreMaterialFromDefaultsButtonAlt;

	// Token: 0x04003670 RID: 13936
	public Button saveMaterialToStore1ButtonAlt;

	// Token: 0x04003671 RID: 13937
	public Button restoreMaterialFromStore1ButtonAlt;

	// Token: 0x04003672 RID: 13938
	public Button saveMaterialToStore2ButtonAlt;

	// Token: 0x04003673 RID: 13939
	public Button restoreMaterialFromStore2ButtonAlt;

	// Token: 0x04003674 RID: 13940
	public Button saveMaterialToStore3ButtonAlt;

	// Token: 0x04003675 RID: 13941
	public Button restoreMaterialFromStore3ButtonAlt;

	// Token: 0x04003676 RID: 13942
	private DAZMeshEyelidControl _eyelidControl;

	// Token: 0x04003677 RID: 13943
	private DAZCharacterRun _characterRun;

	// Token: 0x04003678 RID: 13944
	private DAZPhysicsMesh[] _physicsMeshes;

	// Token: 0x04003679 RID: 13945
	private SetDAZMorph[] _setDAZMorphs;

	// Token: 0x0400367A RID: 13946
	private SpeechBlend lipSync;

	// Token: 0x0400367B RID: 13947
	private AutoColliderBatchUpdater[] _autoColliderBatchUpdaters;

	// Token: 0x0400367C RID: 13948
	private AutoColliderGroup[] _autoColliderGroups;

	// Token: 0x0400367D RID: 13949
	private AutoCollider[] _autoColliders;

	// Token: 0x0400367E RID: 13950
	private SetAnchorFromVertex[] _setAnchorFromVertexComps;

	// Token: 0x0400367F RID: 13951
	private IgnoreChildColliders[] _ignoreChildColliders;

	// Token: 0x04003680 RID: 13952
	private DAZCharacterMaterialOptions[] _materialOptions;

	// Token: 0x04003681 RID: 13953
	public DAZCharacterMaterialOptions femaleEyelashMaterialOptions;

	// Token: 0x04003682 RID: 13954
	public DAZCharacterMaterialOptions maleEyelashMaterialOptions;

	// Token: 0x04003683 RID: 13955
	private bool wasInit;

	// Token: 0x02000AB3 RID: 2739
	public enum Gender
	{
		// Token: 0x04003685 RID: 13957
		None,
		// Token: 0x04003686 RID: 13958
		Male,
		// Token: 0x04003687 RID: 13959
		Female,
		// Token: 0x04003688 RID: 13960
		Both
	}

	// Token: 0x02000AB4 RID: 2740
	protected enum ResyncMorphsOption
	{
		// Token: 0x0400368A RID: 13962
		All,
		// Token: 0x0400368B RID: 13963
		CurrentGender,
		// Token: 0x0400368C RID: 13964
		OtherGender
	}

	// Token: 0x02000FBF RID: 4031
	[CompilerGenerated]
	private sealed class <ExportOBJHelper>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007504 RID: 29956 RVA: 0x00168C58 File Offset: 0x00167058
		[DebuggerHidden]
		public <ExportOBJHelper>c__Iterator0()
		{
		}

		// Token: 0x06007505 RID: 29957 RVA: 0x00168C60 File Offset: 0x00167060
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
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
			case 3U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 4;
				}
				return true;
			case 4U:
				this._characterRun.doSnap = false;
				oe = base.GetComponent<OBJExporter>();
				enabledMats = new Dictionary<int, bool>();
				for (int i = 0; i < this.exportSkin.materialsEnabled.Length; i++)
				{
					if (this.exportSkin.materialsEnabled[i])
					{
						enabledMats.Add(i, true);
					}
				}
				oe.Export(base.selectedCharacter.name + ".obj", this.exportSkin.GetMesh(), this._characterRun.snappedMorphedUVVertices, this._characterRun.snappedMorphedUVNormals, this.exportSkin.dazMesh.materials, enabledMats);
				oe.Export(base.selectedCharacter.name + "_skinned.obj", this.exportSkin.GetMesh(), this._characterRun.snappedSkinnedVertices, this._characterRun.snappedSkinnedNormals, this.exportSkin.dazMesh.materials, enabledMats);
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x06007506 RID: 29958 RVA: 0x00168E64 File Offset: 0x00167264
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x06007507 RID: 29959 RVA: 0x00168E6C File Offset: 0x0016726C
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007508 RID: 29960 RVA: 0x00168E74 File Offset: 0x00167274
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007509 RID: 29961 RVA: 0x00168E84 File Offset: 0x00167284
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400691D RID: 26909
		internal OBJExporter <oe>__0;

		// Token: 0x0400691E RID: 26910
		internal Dictionary<int, bool> <enabledMats>__0;

		// Token: 0x0400691F RID: 26911
		internal DAZCharacterSelector $this;

		// Token: 0x04006920 RID: 26912
		internal object $current;

		// Token: 0x04006921 RID: 26913
		internal bool $disposing;

		// Token: 0x04006922 RID: 26914
		internal int $PC;
	}

	// Token: 0x02000FC0 RID: 4032
	[CompilerGenerated]
	private sealed class <DelayResume>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600750A RID: 29962 RVA: 0x00168E8B File Offset: 0x0016728B
		[DebuggerHidden]
		public <DelayResume>c__Iterator1()
		{
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x00168E94 File Offset: 0x00167294
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.delayResumeFlag = af;
				i = 0;
				break;
			case 1U:
				i++;
				break;
			default:
				return false;
			}
			if (i < count)
			{
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			af.Raise();
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x0600750C RID: 29964 RVA: 0x00168F2E File Offset: 0x0016732E
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x0600750D RID: 29965 RVA: 0x00168F36 File Offset: 0x00167336
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600750E RID: 29966 RVA: 0x00168F3E File Offset: 0x0016733E
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600750F RID: 29967 RVA: 0x00168F4E File Offset: 0x0016734E
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006923 RID: 26915
		internal AsyncFlag af;

		// Token: 0x04006924 RID: 26916
		internal int <i>__1;

		// Token: 0x04006925 RID: 26917
		internal int count;

		// Token: 0x04006926 RID: 26918
		internal DAZCharacterSelector $this;

		// Token: 0x04006927 RID: 26919
		internal object $current;

		// Token: 0x04006928 RID: 26920
		internal bool $disposing;

		// Token: 0x04006929 RID: 26921
		internal int $PC;
	}

	// Token: 0x02000FC1 RID: 4033
	[CompilerGenerated]
	private sealed class <RefreshWhenHubClosed>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007510 RID: 29968 RVA: 0x00168F55 File Offset: 0x00167355
		[DebuggerHidden]
		public <RefreshWhenHubClosed>c__Iterator2()
		{
		}

		// Token: 0x06007511 RID: 29969 RVA: 0x00168F60 File Offset: 0x00167360
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
			case 2U:
			{
				startt = GlobalStopwatch.GetElapsedMilliseconds();
				base.RefreshDynamicClothes();
				base.RefreshDynamicHair();
				stopt = GlobalStopwatch.GetElapsedMilliseconds();
				float t = stopt - startt;
				UnityEngine.Debug.Log("Deferred Person refresh clothing and hair took " + t.ToString("F1") + " ms");
				this.refreshCoroutine = null;
				af.Raise();
				this.$PC = -1;
				return false;
			}
			default:
				return false;
			}
			if (!SuperController.singleton.HubOpen && SuperController.singleton.activeUI != SuperController.ActiveUI.PackageDownloader)
			{
				af = new AsyncFlag("Clothing and Hair Refresh");
				SuperController.singleton.SetLoadingIconFlag(af);
				this.$current = null;
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

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x06007512 RID: 29970 RVA: 0x00169091 File Offset: 0x00167491
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x06007513 RID: 29971 RVA: 0x00169099 File Offset: 0x00167499
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007514 RID: 29972 RVA: 0x001690A1 File Offset: 0x001674A1
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007515 RID: 29973 RVA: 0x001690B1 File Offset: 0x001674B1
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400692A RID: 26922
		internal AsyncFlag <af>__0;

		// Token: 0x0400692B RID: 26923
		internal float <startt>__0;

		// Token: 0x0400692C RID: 26924
		internal float <stopt>__0;

		// Token: 0x0400692D RID: 26925
		internal float <t>__0;

		// Token: 0x0400692E RID: 26926
		internal DAZCharacterSelector $this;

		// Token: 0x0400692F RID: 26927
		internal object $current;

		// Token: 0x04006930 RID: 26928
		internal bool $disposing;

		// Token: 0x04006931 RID: 26929
		internal int $PC;
	}

	// Token: 0x02000FC2 RID: 4034
	[CompilerGenerated]
	private sealed class <InitClothingItems>c__AnonStorey6
	{
		// Token: 0x06007516 RID: 29974 RVA: 0x001690B8 File Offset: 0x001674B8
		public <InitClothingItems>c__AnonStorey6()
		{
		}

		// Token: 0x06007517 RID: 29975 RVA: 0x001690C0 File Offset: 0x001674C0
		internal void <>m__0()
		{
			this.$this.ToggleClothingItem(this.dc);
		}

		// Token: 0x04006932 RID: 26930
		internal DAZClothingItem dc;

		// Token: 0x04006933 RID: 26931
		internal DAZCharacterSelector $this;
	}

	// Token: 0x02000FC3 RID: 4035
	[CompilerGenerated]
	private sealed class <DelayLoadClothingCreatorItem>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007518 RID: 29976 RVA: 0x001690D3 File Offset: 0x001674D3
		[DebuggerHidden]
		public <DelayLoadClothingCreatorItem>c__Iterator3()
		{
		}

		// Token: 0x06007519 RID: 29977 RVA: 0x001690DC File Offset: 0x001674DC
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
			case 2U:
				drc = item.GetComponentInChildren<DAZRuntimeCreator>();
				if (drc != null)
				{
					drc.LoadFromPath(source.GetMetaStorePath());
					item.OpenUI();
				}
				this.$PC = -1;
				return false;
			default:
				return false;
			}
			if (item.ready)
			{
				this.$current = null;
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

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x0600751A RID: 29978 RVA: 0x001691A6 File Offset: 0x001675A6
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x0600751B RID: 29979 RVA: 0x001691AE File Offset: 0x001675AE
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600751C RID: 29980 RVA: 0x001691B6 File Offset: 0x001675B6
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600751D RID: 29981 RVA: 0x001691C6 File Offset: 0x001675C6
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006934 RID: 26932
		internal DAZClothingItem item;

		// Token: 0x04006935 RID: 26933
		internal DAZRuntimeCreator <drc>__0;

		// Token: 0x04006936 RID: 26934
		internal DAZDynamic source;

		// Token: 0x04006937 RID: 26935
		internal object $current;

		// Token: 0x04006938 RID: 26936
		internal bool $disposing;

		// Token: 0x04006939 RID: 26937
		internal int $PC;
	}

	// Token: 0x02000FC4 RID: 4036
	[CompilerGenerated]
	private sealed class <InitHairItems>c__AnonStorey7
	{
		// Token: 0x0600751E RID: 29982 RVA: 0x001691CD File Offset: 0x001675CD
		public <InitHairItems>c__AnonStorey7()
		{
		}

		// Token: 0x0600751F RID: 29983 RVA: 0x001691D5 File Offset: 0x001675D5
		internal void <>m__0()
		{
			this.$this.ToggleHairItem(this.dc);
		}

		// Token: 0x0400693A RID: 26938
		internal DAZHairGroup dc;

		// Token: 0x0400693B RID: 26939
		internal DAZCharacterSelector $this;
	}

	// Token: 0x02000FC5 RID: 4037
	[CompilerGenerated]
	private sealed class <DelayLoadHairCreatorItem>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007520 RID: 29984 RVA: 0x001691E8 File Offset: 0x001675E8
		[DebuggerHidden]
		public <DelayLoadHairCreatorItem>c__Iterator4()
		{
		}

		// Token: 0x06007521 RID: 29985 RVA: 0x001691F0 File Offset: 0x001675F0
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
			case 2U:
				drc = item.GetComponentInChildren<DAZRuntimeCreator>();
				if (drc != null)
				{
					drc.LoadFromPath(source.GetMetaStorePath());
					item.OpenUI();
				}
				this.$PC = -1;
				return false;
			default:
				return false;
			}
			if (item.ready)
			{
				this.$current = null;
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

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x06007522 RID: 29986 RVA: 0x001692BA File Offset: 0x001676BA
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x06007523 RID: 29987 RVA: 0x001692C2 File Offset: 0x001676C2
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007524 RID: 29988 RVA: 0x001692CA File Offset: 0x001676CA
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007525 RID: 29989 RVA: 0x001692DA File Offset: 0x001676DA
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400693C RID: 26940
		internal DAZHairGroup item;

		// Token: 0x0400693D RID: 26941
		internal DAZRuntimeCreator <drc>__0;

		// Token: 0x0400693E RID: 26942
		internal DAZDynamic source;

		// Token: 0x0400693F RID: 26943
		internal object $current;

		// Token: 0x04006940 RID: 26944
		internal bool $disposing;

		// Token: 0x04006941 RID: 26945
		internal int $PC;
	}

	// Token: 0x02000FC6 RID: 4038
	[CompilerGenerated]
	private sealed class <UnloadUnusedAssetsDelayed>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007526 RID: 29990 RVA: 0x001692E1 File Offset: 0x001676E1
		[DebuggerHidden]
		public <UnloadUnusedAssetsDelayed>c__Iterator5()
		{
		}

		// Token: 0x06007527 RID: 29991 RVA: 0x001692EC File Offset: 0x001676EC
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
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				Resources.UnloadUnusedAssets();
				GC.Collect();
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06007528 RID: 29992 RVA: 0x00169369 File Offset: 0x00167769
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x06007529 RID: 29993 RVA: 0x00169371 File Offset: 0x00167771
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600752A RID: 29994 RVA: 0x00169379 File Offset: 0x00167779
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600752B RID: 29995 RVA: 0x00169389 File Offset: 0x00167789
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006942 RID: 26946
		internal object $current;

		// Token: 0x04006943 RID: 26947
		internal bool $disposing;

		// Token: 0x04006944 RID: 26948
		internal int $PC;
	}
}
