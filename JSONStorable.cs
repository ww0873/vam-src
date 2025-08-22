using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000CCC RID: 3276
public class JSONStorable : MonoBehaviour
{
	// Token: 0x06006280 RID: 25216 RVA: 0x001416C3 File Offset: 0x0013FAC3
	public JSONStorable()
	{
	}

	// Token: 0x17000E7F RID: 3711
	// (get) Token: 0x06006281 RID: 25217 RVA: 0x001416CC File Offset: 0x0013FACC
	public string storeId
	{
		get
		{
			if (this.overrideId == null || this.overrideId == string.Empty)
			{
				return base.name;
			}
			if (this.overrideId[0] != '+')
			{
				return this.overrideId;
			}
			if (!this.overrideId.StartsWith("+parent+", StringComparison.CurrentCultureIgnoreCase))
			{
				return base.name + this.overrideId.Substring(1);
			}
			if (base.transform.parent != null)
			{
				return base.transform.parent.name + base.name + this.overrideId.Substring(8);
			}
			return base.name + this.overrideId.Substring(8);
		}
	}

	// Token: 0x17000E80 RID: 3712
	// (get) Token: 0x06006282 RID: 25218 RVA: 0x0014179D File Offset: 0x0013FB9D
	// (set) Token: 0x06006283 RID: 25219 RVA: 0x001417A5 File Offset: 0x0013FBA5
	public string subScenePrefix
	{
		get
		{
			return this._subScenePrefix;
		}
		set
		{
			if (this._subScenePrefix != value)
			{
				this._subScenePrefix = value;
			}
		}
	}

	// Token: 0x06006284 RID: 25220 RVA: 0x001417C0 File Offset: 0x0013FBC0
	public string AtomUidToStoreAtomUid(string atomUid)
	{
		if (atomUid == null || this.subScenePrefix == null)
		{
			return atomUid;
		}
		string text = "^" + this.subScenePrefix;
		if (Regex.IsMatch(atomUid, text + "[^/]+$"))
		{
			return Regex.Replace(atomUid, text, string.Empty);
		}
		return null;
	}

	// Token: 0x06006285 RID: 25221 RVA: 0x00141815 File Offset: 0x0013FC15
	public string StoredAtomUidToAtomUid(string storedAtomUid)
	{
		if (this.subScenePrefix != null)
		{
			return this.subScenePrefix + storedAtomUid;
		}
		return storedAtomUid;
	}

	// Token: 0x06006286 RID: 25222 RVA: 0x00141830 File Offset: 0x0013FC30
	protected virtual void InitUI(Transform t, bool isAlt)
	{
	}

	// Token: 0x06006287 RID: 25223 RVA: 0x00141832 File Offset: 0x0013FC32
	public virtual void SetUI(Transform t)
	{
		if (this.UITransform != t)
		{
			this.UITransform = t;
			this.InitUI();
		}
	}

	// Token: 0x06006288 RID: 25224 RVA: 0x00141852 File Offset: 0x0013FC52
	public virtual void InitUI()
	{
		this.InitUI(this.UITransform, false);
	}

	// Token: 0x06006289 RID: 25225 RVA: 0x00141861 File Offset: 0x0013FC61
	public virtual void SetUIAlt(Transform t)
	{
		if (this.UITransformAlt != t)
		{
			this.UITransformAlt = t;
			this.InitUIAlt();
		}
	}

	// Token: 0x0600628A RID: 25226 RVA: 0x00141881 File Offset: 0x0013FC81
	public virtual void InitUIAlt()
	{
		this.InitUI(this.UITransformAlt, false);
	}

	// Token: 0x0600628B RID: 25227 RVA: 0x00141890 File Offset: 0x0013FC90
	public void RestoreAllFromDefaults()
	{
		this.RestoreFromJSON(new JSONClass(), true, true, null, true);
	}

	// Token: 0x0600628C RID: 25228 RVA: 0x001418A1 File Offset: 0x0013FCA1
	public void RestorePhysicalFromDefaults()
	{
		this.RestoreFromJSON(new JSONClass(), true, false, null, true);
	}

	// Token: 0x0600628D RID: 25229 RVA: 0x001418B2 File Offset: 0x0013FCB2
	public void RestoreAppearanceFromDefaults()
	{
		this.RestoreFromJSON(new JSONClass(), false, true, null, true);
	}

	// Token: 0x0600628E RID: 25230 RVA: 0x001418C3 File Offset: 0x0013FCC3
	public void RestoreFromStore1(bool restorePhysical = true, bool restoreAppearance = true)
	{
		if (JSONStorable.copyStore1 != null)
		{
			this.RestoreFromJSON(JSONStorable.copyStore1, restorePhysical, restoreAppearance, null, true);
		}
	}

	// Token: 0x0600628F RID: 25231 RVA: 0x001418E4 File Offset: 0x0013FCE4
	public void RestoreAllFromStore1()
	{
		this.RestoreFromStore1(true, true);
	}

	// Token: 0x06006290 RID: 25232 RVA: 0x001418EE File Offset: 0x0013FCEE
	public void RestorePhysicalFromStore1()
	{
		this.RestoreFromStore1(true, false);
	}

	// Token: 0x06006291 RID: 25233 RVA: 0x001418F8 File Offset: 0x0013FCF8
	public void RestoreAppearanceFromStore1()
	{
		this.RestoreFromStore1(false, true);
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x00141902 File Offset: 0x0013FD02
	public void SaveToStore1()
	{
		JSONStorable.copyStore1 = this.GetJSON(true, true, true);
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x00141912 File Offset: 0x0013FD12
	public void RestoreFromStore2(bool restorePhysical = true, bool restoreAppearance = true)
	{
		if (JSONStorable.copyStore2 != null)
		{
			this.RestoreFromJSON(JSONStorable.copyStore2, restorePhysical, restoreAppearance, null, true);
		}
	}

	// Token: 0x06006294 RID: 25236 RVA: 0x00141933 File Offset: 0x0013FD33
	public void RestoreAllFromStore2()
	{
		this.RestoreFromStore2(true, true);
	}

	// Token: 0x06006295 RID: 25237 RVA: 0x0014193D File Offset: 0x0013FD3D
	public void RestorePhysicalFromStore2()
	{
		this.RestoreFromStore2(true, false);
	}

	// Token: 0x06006296 RID: 25238 RVA: 0x00141947 File Offset: 0x0013FD47
	public void RestoreAppearanceFromStore2()
	{
		this.RestoreFromStore2(false, true);
	}

	// Token: 0x06006297 RID: 25239 RVA: 0x00141951 File Offset: 0x0013FD51
	public void SaveToStore2()
	{
		JSONStorable.copyStore2 = this.GetJSON(true, true, true);
	}

	// Token: 0x06006298 RID: 25240 RVA: 0x00141961 File Offset: 0x0013FD61
	public void RestoreFromStore3(bool restorePhysical = true, bool restoreAppearance = true)
	{
		if (JSONStorable.copyStore3 != null)
		{
			this.RestoreFromJSON(JSONStorable.copyStore3, restorePhysical, restoreAppearance, null, true);
		}
	}

	// Token: 0x06006299 RID: 25241 RVA: 0x00141982 File Offset: 0x0013FD82
	public void RestoreAllFromStore3()
	{
		this.RestoreFromStore3(true, true);
	}

	// Token: 0x0600629A RID: 25242 RVA: 0x0014198C File Offset: 0x0013FD8C
	public void RestorePhysicalFromStore3()
	{
		this.RestoreFromStore3(true, false);
	}

	// Token: 0x0600629B RID: 25243 RVA: 0x00141996 File Offset: 0x0013FD96
	public void RestoreAppearanceFromStore3()
	{
		this.RestoreFromStore3(false, true);
	}

	// Token: 0x0600629C RID: 25244 RVA: 0x001419A0 File Offset: 0x0013FDA0
	public void SaveToStore3()
	{
		JSONStorable.copyStore3 = this.GetJSON(true, true, true);
	}

	// Token: 0x0600629D RID: 25245 RVA: 0x001419B0 File Offset: 0x0013FDB0
	public bool HasParamsOrActions()
	{
		if (!this.awakecalled)
		{
			this.Awake();
		}
		return this.allParams.Count > 0 || this.actionNames.Count > 0 || this.audioClipActionNames.Count > 0 || this.stringChooserActionNames.Count > 0 || this.sceneFilePathActionNames.Count > 0 || this.presetFilePathActionNames.Count > 0;
	}

	// Token: 0x0600629E RID: 25246 RVA: 0x00141A37 File Offset: 0x0013FE37
	public List<string> GetAllParamAndActionNames()
	{
		if (!this.awakecalled)
		{
			this.Awake();
		}
		return this.allParamAndActionNames;
	}

	// Token: 0x0600629F RID: 25247 RVA: 0x00141A50 File Offset: 0x0013FE50
	public List<string> GetAllFloatAndColorParamNames()
	{
		if (!this.awakecalled)
		{
			this.Awake();
		}
		return this.allFloatAndColorParamNames;
	}

	// Token: 0x060062A0 RID: 25248 RVA: 0x00141A6C File Offset: 0x0013FE6C
	public JSONStorable.Type GetParamOrActionType(string name)
	{
		if (!this.awakecalled)
		{
			this.Awake();
		}
		JSONStorableParam jsonstorableParam;
		if (this.allParams.TryGetValue(name, out jsonstorableParam))
		{
			return jsonstorableParam.type;
		}
		if (this.allParamsByAltName.TryGetValue(name, out jsonstorableParam))
		{
			return jsonstorableParam.type;
		}
		if (this.IsAction(name))
		{
			return JSONStorable.Type.Action;
		}
		if (this.IsAudioClipAction(name))
		{
			return JSONStorable.Type.AudioClipAction;
		}
		if (this.IsStringChooserAction(name))
		{
			return JSONStorable.Type.StringChooserAction;
		}
		if (this.IsSceneFilePathAction(name))
		{
			return JSONStorable.Type.SceneFilePathAction;
		}
		if (this.IsPresetFilePathAction(name))
		{
			return JSONStorable.Type.PresetFilePathAction;
		}
		return JSONStorable.Type.None;
	}

	// Token: 0x060062A1 RID: 25249 RVA: 0x00141B0C File Offset: 0x0013FF0C
	public JSONStorableParam GetParam(string name)
	{
		JSONStorableParam result = null;
		if (this.allParams.TryGetValue(name, out result))
		{
			return result;
		}
		if (this.allParamsByAltName.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062A2 RID: 25250 RVA: 0x00141B46 File Offset: 0x0013FF46
	public virtual string[] GetCustomParamNames()
	{
		return null;
	}

	// Token: 0x17000E81 RID: 3713
	// (get) Token: 0x060062A3 RID: 25251 RVA: 0x00141B49 File Offset: 0x0013FF49
	// (set) Token: 0x060062A4 RID: 25252 RVA: 0x00141B51 File Offset: 0x0013FF51
	public bool physicalLocked
	{
		[CompilerGenerated]
		get
		{
			return this.<physicalLocked>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<physicalLocked>k__BackingField = value;
		}
	}

	// Token: 0x060062A5 RID: 25253 RVA: 0x00141B5A File Offset: 0x0013FF5A
	public void SetPhysicalLock(string physicalLockUid)
	{
		if (this.physicalLocks == null)
		{
			this.physicalLocks = new HashSet<string>();
		}
		if (!this.physicalLocks.Contains(physicalLockUid))
		{
			this.physicalLocks.Add(physicalLockUid);
		}
		this.physicalLocked = true;
	}

	// Token: 0x060062A6 RID: 25254 RVA: 0x00141B97 File Offset: 0x0013FF97
	public void ClearPhysicalLock(string physicalLockUid)
	{
		if (this.physicalLocks != null)
		{
			this.physicalLocks.Remove(physicalLockUid);
			if (this.physicalLocks.Count == 0)
			{
				this.physicalLocked = false;
			}
		}
	}

	// Token: 0x17000E82 RID: 3714
	// (get) Token: 0x060062A7 RID: 25255 RVA: 0x00141BC8 File Offset: 0x0013FFC8
	// (set) Token: 0x060062A8 RID: 25256 RVA: 0x00141BD0 File Offset: 0x0013FFD0
	public bool appearanceLocked
	{
		[CompilerGenerated]
		get
		{
			return this.<appearanceLocked>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<appearanceLocked>k__BackingField = value;
		}
	}

	// Token: 0x060062A9 RID: 25257 RVA: 0x00141BD9 File Offset: 0x0013FFD9
	public void SetAppearanceLock(string appearanceLockUid)
	{
		if (this.appearanceLocks == null)
		{
			this.appearanceLocks = new HashSet<string>();
		}
		if (!this.appearanceLocks.Contains(appearanceLockUid))
		{
			this.appearanceLocks.Add(appearanceLockUid);
		}
		this.appearanceLocked = true;
	}

	// Token: 0x060062AA RID: 25258 RVA: 0x00141C16 File Offset: 0x00140016
	public void ClearAppearanceLock(string appearanceLockUid)
	{
		if (this.appearanceLocks != null)
		{
			this.appearanceLocks.Remove(appearanceLockUid);
			if (this.appearanceLocks.Count == 0)
			{
				this.appearanceLocked = false;
			}
		}
	}

	// Token: 0x060062AB RID: 25259 RVA: 0x00141C48 File Offset: 0x00140048
	public void SetCustomAppearanceParamLock(string paramName, string lockUid)
	{
		if (this.customAppearanceParamLocks == null)
		{
			this.customAppearanceParamLocks = new Dictionary<string, HashSet<string>>();
		}
		HashSet<string> hashSet;
		if (!this.customAppearanceParamLocks.TryGetValue(paramName, out hashSet))
		{
			hashSet = new HashSet<string>();
			this.customAppearanceParamLocks.Add(paramName, hashSet);
		}
		if (!hashSet.Contains(lockUid))
		{
			hashSet.Add(lockUid);
		}
	}

	// Token: 0x060062AC RID: 25260 RVA: 0x00141CA8 File Offset: 0x001400A8
	public bool IsCustomAppearanceParamLocked(string paramName)
	{
		HashSet<string> hashSet;
		return this.customAppearanceParamLocks != null && this.customAppearanceParamLocks.TryGetValue(paramName, out hashSet) && hashSet.Count > 0;
	}

	// Token: 0x060062AD RID: 25261 RVA: 0x00141CE8 File Offset: 0x001400E8
	public void ClearCustomAppearanceParamLock(string paramName, string lockUid)
	{
		HashSet<string> hashSet;
		if (this.customAppearanceParamLocks != null && this.customAppearanceParamLocks.TryGetValue(paramName, out hashSet))
		{
			hashSet.Remove(lockUid);
		}
	}

	// Token: 0x060062AE RID: 25262 RVA: 0x00141D1C File Offset: 0x0014011C
	public void SetCustomPhysicalParamLock(string paramName, string lockUid)
	{
		if (this.customPhysicalParamLocks == null)
		{
			this.customPhysicalParamLocks = new Dictionary<string, HashSet<string>>();
		}
		HashSet<string> hashSet;
		if (!this.customPhysicalParamLocks.TryGetValue(paramName, out hashSet))
		{
			hashSet = new HashSet<string>();
			this.customPhysicalParamLocks.Add(paramName, hashSet);
		}
		if (!hashSet.Contains(lockUid))
		{
			hashSet.Add(lockUid);
		}
	}

	// Token: 0x060062AF RID: 25263 RVA: 0x00141D7C File Offset: 0x0014017C
	public bool IsCustomPhysicalParamLocked(string paramName)
	{
		HashSet<string> hashSet;
		return this.customPhysicalParamLocks != null && this.customPhysicalParamLocks.TryGetValue(paramName, out hashSet) && hashSet.Count > 0;
	}

	// Token: 0x060062B0 RID: 25264 RVA: 0x00141DBC File Offset: 0x001401BC
	public void ClearCustomPhysicalParamLock(string paramName, string lockUid)
	{
		HashSet<string> hashSet;
		if (this.customPhysicalParamLocks != null && this.customPhysicalParamLocks.TryGetValue(paramName, out hashSet))
		{
			hashSet.Remove(lockUid);
		}
	}

	// Token: 0x060062B1 RID: 25265 RVA: 0x00141DF0 File Offset: 0x001401F0
	public void ClearAllLocks()
	{
		if (this.physicalLocks != null)
		{
			this.physicalLocks.Clear();
		}
		this.physicalLocked = false;
		if (this.appearanceLocks != null)
		{
			this.appearanceLocks.Clear();
		}
		this.appearanceLocked = false;
		if (this.customAppearanceParamLocks != null)
		{
			this.customAppearanceParamLocks.Clear();
		}
		if (this.customPhysicalParamLocks != null)
		{
			this.customPhysicalParamLocks.Clear();
		}
	}

	// Token: 0x17000E83 RID: 3715
	// (get) Token: 0x060062B2 RID: 25266 RVA: 0x00141E63 File Offset: 0x00140263
	// (set) Token: 0x060062B3 RID: 25267 RVA: 0x00141E6B File Offset: 0x0014026B
	public bool mergeRestore
	{
		[CompilerGenerated]
		get
		{
			return this.<mergeRestore>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<mergeRestore>k__BackingField = value;
		}
	}

	// Token: 0x060062B4 RID: 25268 RVA: 0x00141E74 File Offset: 0x00140274
	protected void InitBoolParams()
	{
		this.boolParams = new Dictionary<string, JSONStorableBool>();
		this.boolParamNames = new List<string>();
	}

	// Token: 0x060062B5 RID: 25269 RVA: 0x00141E8C File Offset: 0x0014028C
	public void RegisterBool(JSONStorableBool param)
	{
		if (this.boolParams.ContainsKey(param.name))
		{
			UnityEngine.Debug.LogError("Tried registering param " + param.name + " that already exists");
		}
		else
		{
			this.boolParams.Add(param.name, param);
			if (!param.hidden)
			{
				this.boolParamNames.Add(param.name);
				this.allParamAndActionNames.Add(param.name);
			}
			param.storable = this;
		}
		if (!this.allParams.ContainsKey(param.name))
		{
			this.allParams.Add(param.name, param);
		}
	}

	// Token: 0x060062B6 RID: 25270 RVA: 0x00141F3C File Offset: 0x0014033C
	public void DeregisterBool(JSONStorableBool param)
	{
		if (param.storable == this)
		{
			if (!this.boolParams.ContainsKey(param.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering param " + param.name + " that does not exist");
			}
			else
			{
				this.allParams.Remove(param.name);
				this.boolParams.Remove(param.name);
				this.boolParamNames.Remove(param.name);
				this.allParamAndActionNames.Remove(param.name);
				param.storable = null;
			}
		}
	}

	// Token: 0x060062B7 RID: 25271 RVA: 0x00141FDE File Offset: 0x001403DE
	public List<string> GetBoolParamNames()
	{
		return this.boolParamNames;
	}

	// Token: 0x060062B8 RID: 25272 RVA: 0x00141FE6 File Offset: 0x001403E6
	public virtual bool IsBoolJSONParam(string name)
	{
		return this.boolParams.ContainsKey(name);
	}

	// Token: 0x060062B9 RID: 25273 RVA: 0x00141FF4 File Offset: 0x001403F4
	public JSONStorableBool GetBoolJSONParam(string name)
	{
		JSONStorableBool result;
		if (this.boolParams.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062BA RID: 25274 RVA: 0x00142018 File Offset: 0x00140418
	public virtual bool GetBoolParamValue(string param)
	{
		JSONStorableBool jsonstorableBool;
		if (this.boolParams.TryGetValue(param, out jsonstorableBool))
		{
			return jsonstorableBool.val;
		}
		UnityEngine.Debug.LogError("Tried to get param value for param " + param + " that does not exist");
		return false;
	}

	// Token: 0x060062BB RID: 25275 RVA: 0x00142058 File Offset: 0x00140458
	public virtual void SetBoolParamValue(string param, bool value)
	{
		JSONStorableBool jsonstorableBool;
		if (this.boolParams.TryGetValue(param, out jsonstorableBool))
		{
			jsonstorableBool.val = value;
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to set param value for param " + param + " that does not exist");
		}
	}

	// Token: 0x060062BC RID: 25276 RVA: 0x00142099 File Offset: 0x00140499
	protected void InitFloatParams()
	{
		this.floatParams = new Dictionary<string, JSONStorableFloat>();
		this.floatParamsByAltName = new Dictionary<string, JSONStorableFloat>();
		this.floatParamNames = new List<string>();
		this.floatParamNamesIncludingHidden = new List<string>();
	}

	// Token: 0x060062BD RID: 25277 RVA: 0x001420C8 File Offset: 0x001404C8
	public void RegisterFloat(JSONStorableFloat param)
	{
		if (this.floatParams.ContainsKey(param.name))
		{
			UnityEngine.Debug.LogError("Tried registering param " + param.name + " that already exists");
		}
		else
		{
			this.floatParams.Add(param.name, param);
			if (param.altName != null && !this.floatParamsByAltName.ContainsKey(param.altName))
			{
				this.floatParamsByAltName.Add(param.altName, param);
				param.registeredAltName = true;
				if (!this.allParamsByAltName.ContainsKey(param.altName))
				{
					this.allParamsByAltName.Add(param.altName, param);
				}
			}
			if (!param.hidden)
			{
				this.floatParamNames.Add(param.name);
				this.allParamAndActionNames.Add(param.name);
				this.allFloatAndColorParamNames.Add(param.name);
			}
			this.floatParamNamesIncludingHidden.Add(param.name);
			param.storable = this;
		}
		if (!this.allParams.ContainsKey(param.name))
		{
			this.allParams.Add(param.name, param);
		}
	}

	// Token: 0x060062BE RID: 25278 RVA: 0x001421FC File Offset: 0x001405FC
	public void DeregisterFloat(JSONStorableFloat param)
	{
		if (param.storable == this)
		{
			if (!this.floatParams.ContainsKey(param.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering param " + param.name + " that does not exist");
			}
			else
			{
				this.allParams.Remove(param.name);
				this.floatParams.Remove(param.name);
				this.floatParamNames.Remove(param.name);
				this.floatParamNamesIncludingHidden.Remove(param.name);
				this.allParamAndActionNames.Remove(param.name);
				this.allFloatAndColorParamNames.Remove(param.name);
				if (param.registeredAltName)
				{
					this.floatParamsByAltName.Remove(param.altName);
					this.allParamsByAltName.Remove(param.altName);
					param.registeredAltName = false;
				}
				param.storable = null;
			}
		}
	}

	// Token: 0x060062BF RID: 25279 RVA: 0x001422F8 File Offset: 0x001406F8
	public void MassDeregister(HashSet<string> paramNamesToDeregister)
	{
		List<string> list = new List<string>();
		foreach (string text in this.floatParamNamesIncludingHidden)
		{
			if (paramNamesToDeregister.Contains(text))
			{
				JSONStorableFloat jsonstorableFloat;
				if (this.floatParams.TryGetValue(text, out jsonstorableFloat))
				{
					jsonstorableFloat.storable = null;
					this.allParams.Remove(text);
					this.floatParams.Remove(text);
					if (jsonstorableFloat.registeredAltName)
					{
						this.floatParamsByAltName.Remove(jsonstorableFloat.altName);
						this.allParamsByAltName.Remove(jsonstorableFloat.altName);
						jsonstorableFloat.registeredAltName = false;
					}
				}
			}
			else
			{
				list.Add(text);
			}
		}
		this.floatParamNamesIncludingHidden = list;
		List<string> list2 = new List<string>();
		foreach (string item in this.floatParamNames)
		{
			if (!paramNamesToDeregister.Contains(item))
			{
				list2.Add(item);
			}
		}
		this.floatParamNames = list2;
		List<string> list3 = new List<string>();
		foreach (string item2 in this.allParamAndActionNames)
		{
			if (!paramNamesToDeregister.Contains(item2))
			{
				list3.Add(item2);
			}
		}
		this.allParamAndActionNames = list3;
		List<string> list4 = new List<string>();
		foreach (string item3 in this.allFloatAndColorParamNames)
		{
			if (!paramNamesToDeregister.Contains(item3))
			{
				list4.Add(item3);
			}
		}
		this.allFloatAndColorParamNames = list4;
	}

	// Token: 0x060062C0 RID: 25280 RVA: 0x00142520 File Offset: 0x00140920
	public List<string> GetFloatParamNames()
	{
		return this.floatParamNames;
	}

	// Token: 0x060062C1 RID: 25281 RVA: 0x00142528 File Offset: 0x00140928
	public virtual bool IsFloatJSONParam(string name)
	{
		return this.floatParams.ContainsKey(name) || this.floatParamsByAltName.ContainsKey(name);
	}

	// Token: 0x060062C2 RID: 25282 RVA: 0x0014254C File Offset: 0x0014094C
	public JSONStorableFloat GetFloatJSONParam(string name)
	{
		JSONStorableFloat result;
		if (this.floatParams.TryGetValue(name, out result) || this.floatParamsByAltName.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062C3 RID: 25283 RVA: 0x00142584 File Offset: 0x00140984
	public virtual float GetFloatParamValue(string param)
	{
		JSONStorableFloat jsonstorableFloat;
		if (this.floatParams.TryGetValue(param, out jsonstorableFloat) || this.floatParamsByAltName.TryGetValue(param, out jsonstorableFloat))
		{
			return jsonstorableFloat.val;
		}
		UnityEngine.Debug.LogError("Tried to get param value for param " + param + " that does not exist");
		return 0f;
	}

	// Token: 0x060062C4 RID: 25284 RVA: 0x001425D8 File Offset: 0x001409D8
	public virtual void SetFloatParamValue(string param, float value)
	{
		JSONStorableFloat jsonstorableFloat;
		if (this.floatParams.TryGetValue(param, out jsonstorableFloat) || this.floatParamsByAltName.TryGetValue(param, out jsonstorableFloat))
		{
			jsonstorableFloat.val = value;
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to set param value for param " + param + " that does not exist");
		}
	}

	// Token: 0x060062C5 RID: 25285 RVA: 0x0014262C File Offset: 0x00140A2C
	public virtual float GetFloatJSONParamMinValue(string param)
	{
		JSONStorableFloat jsonstorableFloat;
		if (this.floatParams.TryGetValue(param, out jsonstorableFloat) || this.floatParamsByAltName.TryGetValue(param, out jsonstorableFloat))
		{
			return jsonstorableFloat.min;
		}
		UnityEngine.Debug.LogError("Tried to get min param value for param " + param + " that does not exist");
		return 0f;
	}

	// Token: 0x060062C6 RID: 25286 RVA: 0x00142680 File Offset: 0x00140A80
	public virtual float GetFloatJSONParamMaxValue(string param)
	{
		JSONStorableFloat jsonstorableFloat;
		if (this.floatParams.TryGetValue(param, out jsonstorableFloat) || this.floatParamsByAltName.TryGetValue(param, out jsonstorableFloat))
		{
			return jsonstorableFloat.max;
		}
		UnityEngine.Debug.LogError("Tried to get max param value for param " + param + " that does not exist");
		return 0f;
	}

	// Token: 0x060062C7 RID: 25287 RVA: 0x001426D4 File Offset: 0x00140AD4
	protected void InitVector3Params()
	{
		this.vector3Params = new Dictionary<string, JSONStorableVector3>();
		this.vector3ParamNames = new List<string>();
	}

	// Token: 0x060062C8 RID: 25288 RVA: 0x001426EC File Offset: 0x00140AEC
	public void RegisterVector3(JSONStorableVector3 param)
	{
		if (this.vector3Params.ContainsKey(param.name))
		{
			UnityEngine.Debug.LogError("Tried registering param " + param.name + " that already exists");
		}
		else
		{
			this.vector3Params.Add(param.name, param);
			if (!param.hidden)
			{
				this.vector3ParamNames.Add(param.name);
				this.allParamAndActionNames.Add(param.name);
			}
			param.storable = this;
		}
		if (!this.allParams.ContainsKey(param.name))
		{
			this.allParams.Add(param.name, param);
		}
	}

	// Token: 0x060062C9 RID: 25289 RVA: 0x0014279C File Offset: 0x00140B9C
	public void DeregisterVector3(JSONStorableVector3 param)
	{
		if (param.storable == this)
		{
			if (!this.vector3Params.ContainsKey(param.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering param " + param.name + " that does not exist");
			}
			else
			{
				this.allParams.Remove(param.name);
				this.vector3Params.Remove(param.name);
				this.vector3ParamNames.Remove(param.name);
				this.allParamAndActionNames.Remove(param.name);
				param.storable = null;
			}
		}
	}

	// Token: 0x060062CA RID: 25290 RVA: 0x0014283E File Offset: 0x00140C3E
	public List<string> GetVector3ParamNames()
	{
		return this.vector3ParamNames;
	}

	// Token: 0x060062CB RID: 25291 RVA: 0x00142846 File Offset: 0x00140C46
	public virtual bool IsVector3JSONParam(string name)
	{
		return this.vector3Params.ContainsKey(name);
	}

	// Token: 0x060062CC RID: 25292 RVA: 0x00142854 File Offset: 0x00140C54
	public JSONStorableVector3 GetVector3JSONParam(string name)
	{
		JSONStorableVector3 result;
		if (this.vector3Params.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062CD RID: 25293 RVA: 0x00142878 File Offset: 0x00140C78
	public virtual Vector3 GetVector3ParamValue(string param)
	{
		JSONStorableVector3 jsonstorableVector;
		if (this.vector3Params.TryGetValue(param, out jsonstorableVector))
		{
			return jsonstorableVector.val;
		}
		UnityEngine.Debug.LogError("Tried to get param value for param " + param + " that does not exist");
		return Vector3.zero;
	}

	// Token: 0x060062CE RID: 25294 RVA: 0x001428BC File Offset: 0x00140CBC
	public virtual void SetVector3ParamValue(string param, Vector3 value)
	{
		JSONStorableVector3 jsonstorableVector;
		if (this.vector3Params.TryGetValue(param, out jsonstorableVector))
		{
			jsonstorableVector.val = value;
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to set param value for param " + param + " that does not exist");
		}
	}

	// Token: 0x060062CF RID: 25295 RVA: 0x00142900 File Offset: 0x00140D00
	public virtual Vector3 GetVector3JSONParamMinValue(string param)
	{
		JSONStorableVector3 jsonstorableVector;
		if (this.vector3Params.TryGetValue(param, out jsonstorableVector))
		{
			return jsonstorableVector.min;
		}
		UnityEngine.Debug.LogError("Tried to get min param value for param " + param + " that does not exist");
		return Vector3.zero;
	}

	// Token: 0x060062D0 RID: 25296 RVA: 0x00142944 File Offset: 0x00140D44
	public virtual Vector3 GetVector3JSONParamMaxValue(string param)
	{
		JSONStorableVector3 jsonstorableVector;
		if (this.vector3Params.TryGetValue(param, out jsonstorableVector))
		{
			return jsonstorableVector.max;
		}
		UnityEngine.Debug.LogError("Tried to get max param value for param " + param + " that does not exist");
		return Vector3.zero;
	}

	// Token: 0x060062D1 RID: 25297 RVA: 0x00142985 File Offset: 0x00140D85
	protected void InitStringParams()
	{
		this.stringParams = new Dictionary<string, JSONStorableString>();
		this.stringParamNames = new List<string>();
	}

	// Token: 0x060062D2 RID: 25298 RVA: 0x001429A0 File Offset: 0x00140DA0
	public void RegisterString(JSONStorableString param)
	{
		if (this.stringParams.ContainsKey(param.name))
		{
			UnityEngine.Debug.LogError("Tried registering param " + param.name + " that already exists");
		}
		else
		{
			this.stringParams.Add(param.name, param);
			if (!param.hidden)
			{
				this.stringParamNames.Add(param.name);
				this.allParamAndActionNames.Add(param.name);
			}
			param.storable = this;
		}
		if (!this.allParams.ContainsKey(param.name))
		{
			this.allParams.Add(param.name, param);
		}
	}

	// Token: 0x060062D3 RID: 25299 RVA: 0x00142A50 File Offset: 0x00140E50
	public void DeregisterString(JSONStorableString param)
	{
		if (param.storable == this)
		{
			if (!this.stringParams.ContainsKey(param.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering param " + param.name + " that does not exist");
			}
			else
			{
				this.allParams.Remove(param.name);
				this.stringParams.Remove(param.name);
				this.stringParamNames.Remove(param.name);
				this.allParamAndActionNames.Remove(param.name);
				param.storable = null;
			}
		}
	}

	// Token: 0x060062D4 RID: 25300 RVA: 0x00142AF2 File Offset: 0x00140EF2
	public List<string> GetStringParamNames()
	{
		return this.stringParamNames;
	}

	// Token: 0x060062D5 RID: 25301 RVA: 0x00142AFA File Offset: 0x00140EFA
	public virtual bool IsStringJSONParam(string name)
	{
		return this.stringParams.ContainsKey(name);
	}

	// Token: 0x060062D6 RID: 25302 RVA: 0x00142B08 File Offset: 0x00140F08
	public JSONStorableString GetStringJSONParam(string name)
	{
		JSONStorableString result;
		if (this.stringParams.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062D7 RID: 25303 RVA: 0x00142B2C File Offset: 0x00140F2C
	public virtual string GetStringParamValue(string param)
	{
		JSONStorableString jsonstorableString;
		if (this.stringParams.TryGetValue(param, out jsonstorableString))
		{
			return jsonstorableString.val;
		}
		UnityEngine.Debug.LogError("Tried to get param value for param " + param + " that does not exist");
		return null;
	}

	// Token: 0x060062D8 RID: 25304 RVA: 0x00142B6C File Offset: 0x00140F6C
	public virtual void SetStringParamValue(string param, string value)
	{
		JSONStorableString jsonstorableString;
		if (this.stringParams.TryGetValue(param, out jsonstorableString))
		{
			jsonstorableString.val = value;
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to set param value for param " + param + " that does not exist");
		}
	}

	// Token: 0x060062D9 RID: 25305 RVA: 0x00142BAD File Offset: 0x00140FAD
	protected void InitStringChooserParams()
	{
		this.stringChooserParams = new Dictionary<string, JSONStorableStringChooser>();
		this.stringChooserParamNames = new List<string>();
	}

	// Token: 0x060062DA RID: 25306 RVA: 0x00142BC8 File Offset: 0x00140FC8
	public void RegisterStringChooser(JSONStorableStringChooser param)
	{
		if (this.stringChooserParams.ContainsKey(param.name))
		{
			UnityEngine.Debug.LogError("Tried registering param " + param.name + " that already exists");
		}
		else
		{
			this.stringChooserParams.Add(param.name, param);
			if (!param.hidden)
			{
				this.stringChooserParamNames.Add(param.name);
				this.allParamAndActionNames.Add(param.name);
			}
			param.storable = this;
		}
		if (!this.allParams.ContainsKey(param.name))
		{
			this.allParams.Add(param.name, param);
		}
	}

	// Token: 0x060062DB RID: 25307 RVA: 0x00142C78 File Offset: 0x00141078
	public void DeregisterStringChooser(JSONStorableStringChooser param)
	{
		if (param.storable == this)
		{
			if (!this.stringChooserParams.ContainsKey(param.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering param " + param.name + " that does not exist");
			}
			else
			{
				this.allParams.Remove(param.name);
				this.stringChooserParams.Remove(param.name);
				this.stringChooserParamNames.Remove(param.name);
				this.allParamAndActionNames.Remove(param.name);
				param.storable = null;
			}
		}
	}

	// Token: 0x060062DC RID: 25308 RVA: 0x00142D1A File Offset: 0x0014111A
	public List<string> GetStringChooserParamNames()
	{
		return this.stringChooserParamNames;
	}

	// Token: 0x060062DD RID: 25309 RVA: 0x00142D22 File Offset: 0x00141122
	public virtual bool IsStringChooserJSONParam(string name)
	{
		return this.stringChooserParams.ContainsKey(name);
	}

	// Token: 0x060062DE RID: 25310 RVA: 0x00142D30 File Offset: 0x00141130
	public JSONStorableStringChooser GetStringChooserJSONParam(string name)
	{
		JSONStorableStringChooser result;
		if (this.stringChooserParams.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x00142D54 File Offset: 0x00141154
	public virtual string GetStringChooserParamValue(string param)
	{
		JSONStorableStringChooser jsonstorableStringChooser;
		if (this.stringChooserParams.TryGetValue(param, out jsonstorableStringChooser))
		{
			return jsonstorableStringChooser.val;
		}
		UnityEngine.Debug.LogError("Tried to get param value for param " + param + " that does not exist");
		return null;
	}

	// Token: 0x060062E0 RID: 25312 RVA: 0x00142D94 File Offset: 0x00141194
	public virtual void SetStringChooserParamValue(string param, string value)
	{
		JSONStorableStringChooser jsonstorableStringChooser;
		if (this.stringChooserParams.TryGetValue(param, out jsonstorableStringChooser))
		{
			jsonstorableStringChooser.val = value;
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to set param value for param " + param + " that does not exist");
		}
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x00142DD8 File Offset: 0x001411D8
	public virtual List<string> GetStringChooserJSONParamChoices(string param)
	{
		JSONStorableStringChooser jsonstorableStringChooser;
		if (this.stringChooserParams.TryGetValue(param, out jsonstorableStringChooser))
		{
			return jsonstorableStringChooser.choices;
		}
		UnityEngine.Debug.LogError("Tried to get param choices for param " + param + " that does not exist");
		return null;
	}

	// Token: 0x060062E2 RID: 25314 RVA: 0x00142E15 File Offset: 0x00141215
	protected void InitUrlParams()
	{
		this.urlParams = new Dictionary<string, JSONStorableUrl>();
		this.urlParamNames = new List<string>();
	}

	// Token: 0x060062E3 RID: 25315 RVA: 0x00142E30 File Offset: 0x00141230
	public void RegisterUrl(JSONStorableUrl param)
	{
		if (this.urlParams.ContainsKey(param.name))
		{
			UnityEngine.Debug.LogError("Tried registering param " + param.name + " that already exists");
		}
		else
		{
			this.urlParams.Add(param.name, param);
			if (!param.hidden)
			{
				this.urlParamNames.Add(param.name);
				this.allParamAndActionNames.Add(param.name);
			}
			param.storable = this;
		}
		if (!this.allParams.ContainsKey(param.name))
		{
			this.allParams.Add(param.name, param);
		}
	}

	// Token: 0x060062E4 RID: 25316 RVA: 0x00142EE0 File Offset: 0x001412E0
	public void DeregisterUrl(JSONStorableUrl param)
	{
		if (param.storable == this)
		{
			if (!this.urlParams.ContainsKey(param.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering param " + param.name + " that does not exist");
			}
			else
			{
				this.allParams.Remove(param.name);
				this.urlParams.Remove(param.name);
				this.urlParamNames.Remove(param.name);
				this.allParamAndActionNames.Remove(param.name);
				param.storable = null;
			}
		}
	}

	// Token: 0x060062E5 RID: 25317 RVA: 0x00142F82 File Offset: 0x00141382
	public List<string> GetUrlParamNames()
	{
		return this.urlParamNames;
	}

	// Token: 0x060062E6 RID: 25318 RVA: 0x00142F8A File Offset: 0x0014138A
	public virtual bool IsUrlJSONParam(string name)
	{
		return this.urlParams.ContainsKey(name);
	}

	// Token: 0x060062E7 RID: 25319 RVA: 0x00142F98 File Offset: 0x00141398
	public JSONStorableUrl GetUrlJSONParam(string name)
	{
		JSONStorableUrl result;
		if (this.urlParams.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062E8 RID: 25320 RVA: 0x00142FBC File Offset: 0x001413BC
	public virtual string GetUrlParamValue(string param)
	{
		JSONStorableUrl jsonstorableUrl;
		if (this.urlParams.TryGetValue(param, out jsonstorableUrl))
		{
			return jsonstorableUrl.val;
		}
		UnityEngine.Debug.LogError("Tried to get param value for param " + param + " that does not exist");
		return null;
	}

	// Token: 0x060062E9 RID: 25321 RVA: 0x00142FFC File Offset: 0x001413FC
	public virtual void SetUrlParamValue(string param, string value)
	{
		JSONStorableUrl jsonstorableUrl;
		if (this.urlParams.TryGetValue(param, out jsonstorableUrl))
		{
			jsonstorableUrl.val = value;
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to set param value for param " + param + " that does not exist");
		}
	}

	// Token: 0x060062EA RID: 25322 RVA: 0x0014303D File Offset: 0x0014143D
	protected void InitColorParams()
	{
		this.colorParams = new Dictionary<string, JSONStorableColor>();
		this.colorParamNames = new List<string>();
	}

	// Token: 0x060062EB RID: 25323 RVA: 0x00143058 File Offset: 0x00141458
	public void RegisterColor(JSONStorableColor param)
	{
		if (this.colorParams.ContainsKey(param.name))
		{
			UnityEngine.Debug.LogError("Tried registering param " + param.name + " that already exists");
		}
		else
		{
			this.colorParams.Add(param.name, param);
			if (!param.hidden)
			{
				this.colorParamNames.Add(param.name);
				this.allParamAndActionNames.Add(param.name);
				this.allFloatAndColorParamNames.Add(param.name);
			}
			param.storable = this;
		}
		if (!this.allParams.ContainsKey(param.name))
		{
			this.allParams.Add(param.name, param);
		}
	}

	// Token: 0x060062EC RID: 25324 RVA: 0x0014311C File Offset: 0x0014151C
	public void DeregisterColor(JSONStorableColor param)
	{
		if (param.storable == this)
		{
			if (!this.colorParams.ContainsKey(param.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering param " + param.name + " that does not exist");
			}
			else
			{
				this.allParams.Remove(param.name);
				this.colorParams.Remove(param.name);
				this.colorParamNames.Remove(param.name);
				this.allParamAndActionNames.Remove(param.name);
				this.allFloatAndColorParamNames.Remove(param.name);
				param.storable = null;
			}
		}
	}

	// Token: 0x060062ED RID: 25325 RVA: 0x001431D0 File Offset: 0x001415D0
	public List<string> GetColorParamNames()
	{
		return this.colorParamNames;
	}

	// Token: 0x060062EE RID: 25326 RVA: 0x001431D8 File Offset: 0x001415D8
	public virtual bool IsColorJSONParam(string name)
	{
		return this.colorParams.ContainsKey(name);
	}

	// Token: 0x060062EF RID: 25327 RVA: 0x001431E8 File Offset: 0x001415E8
	public JSONStorableColor GetColorJSONParam(string name)
	{
		JSONStorableColor result;
		if (this.colorParams.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062F0 RID: 25328 RVA: 0x0014320C File Offset: 0x0014160C
	public virtual HSVColor GetColorParamValue(string param)
	{
		JSONStorableColor jsonstorableColor;
		if (this.colorParams.TryGetValue(param, out jsonstorableColor))
		{
			return jsonstorableColor.val;
		}
		UnityEngine.Debug.LogError("Tried to get param value for param " + param + " that does not exist");
		this.hsvc.H = 0f;
		this.hsvc.S = 0f;
		this.hsvc.V = 0f;
		return this.hsvc;
	}

	// Token: 0x060062F1 RID: 25329 RVA: 0x00143280 File Offset: 0x00141680
	public virtual void SetColorParamValue(string param, HSVColor value)
	{
		JSONStorableColor jsonstorableColor;
		if (this.colorParams.TryGetValue(param, out jsonstorableColor))
		{
			jsonstorableColor.val = value;
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to set param value for param " + param + " that does not exist");
		}
	}

	// Token: 0x060062F2 RID: 25330 RVA: 0x001432C1 File Offset: 0x001416C1
	protected void InitActions()
	{
		this.actions = new Dictionary<string, JSONStorableAction>();
		this.actionNames = new List<string>();
	}

	// Token: 0x060062F3 RID: 25331 RVA: 0x001432DC File Offset: 0x001416DC
	public void RegisterAction(JSONStorableAction action)
	{
		if (this.actions.ContainsKey(action.name))
		{
			UnityEngine.Debug.LogError("Tried registering action " + action.name + " that already exists");
		}
		else
		{
			this.actions.Add(action.name, action);
			this.actionNames.Add(action.name);
			this.allParamAndActionNames.Add(action.name);
			action.storable = this;
		}
	}

	// Token: 0x060062F4 RID: 25332 RVA: 0x0014335C File Offset: 0x0014175C
	public void DeregisterAction(JSONStorableAction action)
	{
		if (action.storable == this)
		{
			if (!this.actions.ContainsKey(action.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering action " + action.name + " that does not exist");
			}
			else
			{
				this.actions.Remove(action.name);
				this.actionNames.Remove(action.name);
				this.allParamAndActionNames.Remove(action.name);
				action.storable = null;
			}
		}
	}

	// Token: 0x060062F5 RID: 25333 RVA: 0x001433EC File Offset: 0x001417EC
	public List<string> GetActionNames()
	{
		return this.actionNames;
	}

	// Token: 0x060062F6 RID: 25334 RVA: 0x001433F4 File Offset: 0x001417F4
	public virtual bool IsAction(string name)
	{
		return this.actions.ContainsKey(name);
	}

	// Token: 0x060062F7 RID: 25335 RVA: 0x00143404 File Offset: 0x00141804
	public JSONStorableAction GetAction(string name)
	{
		JSONStorableAction result;
		if (this.actions.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062F8 RID: 25336 RVA: 0x00143428 File Offset: 0x00141828
	public virtual void CallAction(string actionName)
	{
		JSONStorableAction jsonstorableAction;
		if (this.actions.TryGetValue(actionName, out jsonstorableAction))
		{
			if (jsonstorableAction.actionCallback != null)
			{
				jsonstorableAction.actionCallback();
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to call action " + actionName + " that does not exist");
		}
	}

	// Token: 0x060062F9 RID: 25337 RVA: 0x00143478 File Offset: 0x00141878
	protected void InitAudioClipActions()
	{
		this.audioClipActions = new Dictionary<string, JSONStorableActionAudioClip>();
		this.audioClipActionNames = new List<string>();
	}

	// Token: 0x060062FA RID: 25338 RVA: 0x00143490 File Offset: 0x00141890
	public void RegisterAudioClipAction(JSONStorableActionAudioClip action)
	{
		if (this.audioClipActions.ContainsKey(action.name))
		{
			UnityEngine.Debug.LogError("Tried registering action " + action.name + " that already exists");
		}
		else
		{
			this.audioClipActions.Add(action.name, action);
			this.audioClipActionNames.Add(action.name);
			this.allParamAndActionNames.Add(action.name);
			action.storable = this;
		}
	}

	// Token: 0x060062FB RID: 25339 RVA: 0x00143510 File Offset: 0x00141910
	public void DeregisterAudioClipAction(JSONStorableActionAudioClip action)
	{
		if (action.storable == this)
		{
			if (!this.actions.ContainsKey(action.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering action " + action.name + " that does not exist");
			}
			else
			{
				this.audioClipActions.Remove(action.name);
				this.audioClipActionNames.Remove(action.name);
				this.allParamAndActionNames.Remove(action.name);
				action.storable = null;
			}
		}
	}

	// Token: 0x060062FC RID: 25340 RVA: 0x001435A0 File Offset: 0x001419A0
	public List<string> GetAudioClipActionNames()
	{
		return this.audioClipActionNames;
	}

	// Token: 0x060062FD RID: 25341 RVA: 0x001435A8 File Offset: 0x001419A8
	public virtual bool IsAudioClipAction(string name)
	{
		return this.audioClipActions.ContainsKey(name);
	}

	// Token: 0x060062FE RID: 25342 RVA: 0x001435B8 File Offset: 0x001419B8
	public JSONStorableActionAudioClip GetAudioClipAction(string name)
	{
		JSONStorableActionAudioClip result;
		if (this.audioClipActions.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060062FF RID: 25343 RVA: 0x001435DC File Offset: 0x001419DC
	public virtual void CallAction(string actionName, NamedAudioClip nac)
	{
		JSONStorableActionAudioClip jsonstorableActionAudioClip;
		if (this.audioClipActions.TryGetValue(actionName, out jsonstorableActionAudioClip))
		{
			if (jsonstorableActionAudioClip.actionCallback != null)
			{
				jsonstorableActionAudioClip.actionCallback(nac);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to call action " + actionName + " that does not exist");
		}
	}

	// Token: 0x06006300 RID: 25344 RVA: 0x0014362D File Offset: 0x00141A2D
	protected void InitStringChooserActions()
	{
		this.stringChooserActions = new Dictionary<string, JSONStorableActionStringChooser>();
		this.stringChooserActionNames = new List<string>();
	}

	// Token: 0x06006301 RID: 25345 RVA: 0x00143648 File Offset: 0x00141A48
	public void RegisterStringChooserAction(JSONStorableActionStringChooser action)
	{
		if (this.stringChooserActions.ContainsKey(action.name))
		{
			UnityEngine.Debug.LogError("Tried registering action " + action.name + " that already exists");
		}
		else
		{
			this.stringChooserActions.Add(action.name, action);
			this.stringChooserActionNames.Add(action.name);
			this.allParamAndActionNames.Add(action.name);
			action.storable = this;
		}
	}

	// Token: 0x06006302 RID: 25346 RVA: 0x001436C8 File Offset: 0x00141AC8
	public void DeregisterStringChooserAction(JSONStorableActionStringChooser action)
	{
		if (action.storable == this)
		{
			if (!this.actions.ContainsKey(action.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering action " + action.name + " that does not exist");
			}
			else
			{
				this.stringChooserActions.Remove(action.name);
				this.stringChooserActionNames.Remove(action.name);
				this.allParamAndActionNames.Remove(action.name);
				action.storable = null;
			}
		}
	}

	// Token: 0x06006303 RID: 25347 RVA: 0x00143758 File Offset: 0x00141B58
	public List<string> GetStringChooserActionNames()
	{
		return this.stringChooserActionNames;
	}

	// Token: 0x06006304 RID: 25348 RVA: 0x00143760 File Offset: 0x00141B60
	public virtual bool IsStringChooserAction(string name)
	{
		return this.stringChooserActions.ContainsKey(name);
	}

	// Token: 0x06006305 RID: 25349 RVA: 0x00143770 File Offset: 0x00141B70
	public JSONStorableActionStringChooser GetStringChooserAction(string name)
	{
		JSONStorableActionStringChooser result;
		if (this.stringChooserActions.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x00143794 File Offset: 0x00141B94
	public virtual void CallStringChooserAction(string actionName, string choice)
	{
		JSONStorableActionStringChooser jsonstorableActionStringChooser;
		if (this.stringChooserActions.TryGetValue(actionName, out jsonstorableActionStringChooser))
		{
			if (jsonstorableActionStringChooser.actionCallback != null)
			{
				jsonstorableActionStringChooser.actionCallback(choice);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to call action " + actionName + " that does not exist");
		}
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x001437E5 File Offset: 0x00141BE5
	protected void InitSceneFilePathActions()
	{
		this.sceneFilePathActions = new Dictionary<string, JSONStorableActionSceneFilePath>();
		this.sceneFilePathActionNames = new List<string>();
	}

	// Token: 0x06006308 RID: 25352 RVA: 0x00143800 File Offset: 0x00141C00
	public void RegisterSceneFilePathAction(JSONStorableActionSceneFilePath action)
	{
		if (this.sceneFilePathActions.ContainsKey(action.name))
		{
			UnityEngine.Debug.LogError("Tried registering action " + action.name + " that already exists");
		}
		else
		{
			this.sceneFilePathActions.Add(action.name, action);
			this.sceneFilePathActionNames.Add(action.name);
			this.allParamAndActionNames.Add(action.name);
			action.storable = this;
		}
	}

	// Token: 0x06006309 RID: 25353 RVA: 0x00143880 File Offset: 0x00141C80
	public void DeregisterSceneFilePathAction(JSONStorableActionSceneFilePath action)
	{
		if (action.storable == this)
		{
			if (!this.actions.ContainsKey(action.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering action " + action.name + " that does not exist");
			}
			else
			{
				this.sceneFilePathActions.Remove(action.name);
				this.sceneFilePathActionNames.Remove(action.name);
				this.allParamAndActionNames.Remove(action.name);
				action.storable = null;
			}
		}
	}

	// Token: 0x0600630A RID: 25354 RVA: 0x00143910 File Offset: 0x00141D10
	public List<string> GetSceneFilePathActionNames()
	{
		return this.sceneFilePathActionNames;
	}

	// Token: 0x0600630B RID: 25355 RVA: 0x00143918 File Offset: 0x00141D18
	public virtual bool IsSceneFilePathAction(string name)
	{
		return this.sceneFilePathActions.ContainsKey(name);
	}

	// Token: 0x0600630C RID: 25356 RVA: 0x00143928 File Offset: 0x00141D28
	public JSONStorableActionSceneFilePath GetSceneFilePathAction(string name)
	{
		JSONStorableActionSceneFilePath result;
		if (this.sceneFilePathActions.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600630D RID: 25357 RVA: 0x0014394C File Offset: 0x00141D4C
	public virtual void CallAction(string actionName, string path)
	{
		JSONStorableActionSceneFilePath jsonstorableActionSceneFilePath;
		if (this.sceneFilePathActions.TryGetValue(actionName, out jsonstorableActionSceneFilePath))
		{
			if (jsonstorableActionSceneFilePath.actionCallback != null)
			{
				jsonstorableActionSceneFilePath.actionCallback(path);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to call action " + actionName + " that does not exist");
		}
	}

	// Token: 0x0600630E RID: 25358 RVA: 0x0014399D File Offset: 0x00141D9D
	protected void InitPresetFilePathActions()
	{
		this.presetFilePathActions = new Dictionary<string, JSONStorableActionPresetFilePath>();
		this.presetFilePathActionNames = new List<string>();
	}

	// Token: 0x0600630F RID: 25359 RVA: 0x001439B8 File Offset: 0x00141DB8
	public void RegisterPresetFilePathAction(JSONStorableActionPresetFilePath action)
	{
		if (this.presetFilePathActions.ContainsKey(action.name))
		{
			UnityEngine.Debug.LogError("Tried registering action " + action.name + " that already exists");
		}
		else
		{
			this.presetFilePathActions.Add(action.name, action);
			this.presetFilePathActionNames.Add(action.name);
			this.allParamAndActionNames.Add(action.name);
			action.storable = this;
		}
	}

	// Token: 0x06006310 RID: 25360 RVA: 0x00143A38 File Offset: 0x00141E38
	public void DeregisterPresetFilePathAction(JSONStorableActionPresetFilePath action)
	{
		if (action.storable == this)
		{
			if (!this.actions.ContainsKey(action.name))
			{
				UnityEngine.Debug.LogError("Tried deregistering action " + action.name + " that does not exist");
			}
			else
			{
				this.presetFilePathActions.Remove(action.name);
				this.presetFilePathActionNames.Remove(action.name);
				this.allParamAndActionNames.Remove(action.name);
				action.storable = null;
			}
		}
	}

	// Token: 0x06006311 RID: 25361 RVA: 0x00143AC8 File Offset: 0x00141EC8
	public List<string> GetPresetFilePathActionNames()
	{
		return this.presetFilePathActionNames;
	}

	// Token: 0x06006312 RID: 25362 RVA: 0x00143AD0 File Offset: 0x00141ED0
	public virtual bool IsPresetFilePathAction(string name)
	{
		return this.presetFilePathActions.ContainsKey(name);
	}

	// Token: 0x06006313 RID: 25363 RVA: 0x00143AE0 File Offset: 0x00141EE0
	public JSONStorableActionPresetFilePath GetPresetFilePathAction(string name)
	{
		JSONStorableActionPresetFilePath result;
		if (this.presetFilePathActions.TryGetValue(name, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06006314 RID: 25364 RVA: 0x00143B04 File Offset: 0x00141F04
	public virtual void CallPresetFileAction(string actionName, string path)
	{
		JSONStorableActionPresetFilePath jsonstorableActionPresetFilePath;
		if (this.presetFilePathActions.TryGetValue(actionName, out jsonstorableActionPresetFilePath))
		{
			if (jsonstorableActionPresetFilePath.actionCallback != null)
			{
				jsonstorableActionPresetFilePath.actionCallback(path);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Tried to call action " + actionName + " that does not exist");
		}
	}

	// Token: 0x17000E84 RID: 3716
	// (get) Token: 0x06006315 RID: 25365 RVA: 0x00143B55 File Offset: 0x00141F55
	// (set) Token: 0x06006316 RID: 25366 RVA: 0x00143B5D File Offset: 0x00141F5D
	public bool isPresetStore
	{
		get
		{
			return this._isPresetStore;
		}
		set
		{
			this._isPresetStore = value;
		}
	}

	// Token: 0x06006317 RID: 25367 RVA: 0x00143B68 File Offset: 0x00141F68
	public virtual JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		if (!this.awakecalled)
		{
			this.Awake();
		}
		JSONClass jsonclass = new JSONClass();
		jsonclass["id"] = this.storeId;
		this.needsStore = false;
		foreach (string key in this.boolParamNames)
		{
			JSONStorableBool jsonstorableBool;
			if (this.boolParams.TryGetValue(key, out jsonstorableBool) && jsonstorableBool.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore))
			{
				this.needsStore = true;
			}
		}
		foreach (string key2 in this.floatParamNames)
		{
			JSONStorableFloat jsonstorableFloat;
			if (this.floatParams.TryGetValue(key2, out jsonstorableFloat) && jsonstorableFloat.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore))
			{
				this.needsStore = true;
			}
		}
		foreach (string key3 in this.vector3ParamNames)
		{
			JSONStorableVector3 jsonstorableVector;
			if (this.vector3Params.TryGetValue(key3, out jsonstorableVector) && jsonstorableVector.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore))
			{
				this.needsStore = true;
			}
		}
		foreach (string key4 in this.stringParamNames)
		{
			JSONStorableString jsonstorableString;
			if (this.stringParams.TryGetValue(key4, out jsonstorableString) && jsonstorableString.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore))
			{
				this.needsStore = true;
			}
		}
		foreach (string key5 in this.stringChooserParamNames)
		{
			JSONStorableStringChooser jsonstorableStringChooser;
			if (this.stringChooserParams.TryGetValue(key5, out jsonstorableStringChooser) && jsonstorableStringChooser.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore))
			{
				this.needsStore = true;
			}
		}
		foreach (string key6 in this.urlParamNames)
		{
			JSONStorableUrl jsonstorableUrl;
			if (this.urlParams.TryGetValue(key6, out jsonstorableUrl) && jsonstorableUrl.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore))
			{
				this.needsStore = true;
			}
		}
		foreach (string key7 in this.colorParamNames)
		{
			JSONStorableColor jsonstorableColor;
			if (this.colorParams.TryGetValue(key7, out jsonstorableColor) && jsonstorableColor.StoreJSON(jsonclass, includePhysical, includeAppearance, forceStore))
			{
				this.needsStore = true;
			}
		}
		return jsonclass;
	}

	// Token: 0x17000E85 RID: 3717
	// (get) Token: 0x06006318 RID: 25368 RVA: 0x00143EC4 File Offset: 0x001422C4
	// (set) Token: 0x06006319 RID: 25369 RVA: 0x00143ECC File Offset: 0x001422CC
	public bool isPresetRestore
	{
		get
		{
			return this._isPresetRestore;
		}
		set
		{
			this._isPresetRestore = value;
		}
	}

	// Token: 0x0600631A RID: 25370 RVA: 0x00143ED8 File Offset: 0x001422D8
	public virtual void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		this.insideRestore = true;
		if (!this.awakecalled)
		{
			this.Awake();
		}
		bool flag = restorePhysical && !this.physicalLocked;
		bool flag2 = restoreAppearance && !this.appearanceLocked;
		if (flag || flag2)
		{
			foreach (string key in this.boolParamNames)
			{
				JSONStorableBool jsonstorableBool;
				if (this.boolParams.TryGetValue(key, out jsonstorableBool))
				{
					jsonstorableBool.RestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key2 in this.floatParamNames)
			{
				JSONStorableFloat jsonstorableFloat;
				if (this.floatParams.TryGetValue(key2, out jsonstorableFloat))
				{
					jsonstorableFloat.RestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key3 in this.vector3ParamNames)
			{
				JSONStorableVector3 jsonstorableVector;
				if (this.vector3Params.TryGetValue(key3, out jsonstorableVector))
				{
					jsonstorableVector.RestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key4 in this.stringParamNames)
			{
				JSONStorableString jsonstorableString;
				if (this.stringParams.TryGetValue(key4, out jsonstorableString))
				{
					jsonstorableString.RestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key5 in this.stringChooserParamNames)
			{
				JSONStorableStringChooser jsonstorableStringChooser;
				if (this.stringChooserParams.TryGetValue(key5, out jsonstorableStringChooser))
				{
					jsonstorableStringChooser.RestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key6 in this.urlParamNames)
			{
				JSONStorableUrl jsonstorableUrl;
				if (this.urlParams.TryGetValue(key6, out jsonstorableUrl))
				{
					jsonstorableUrl.RestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key7 in this.colorParamNames)
			{
				JSONStorableColor jsonstorableColor;
				if (this.colorParams.TryGetValue(key7, out jsonstorableColor))
				{
					jsonstorableColor.RestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
		}
		this.insideRestore = false;
	}

	// Token: 0x0600631B RID: 25371 RVA: 0x00144204 File Offset: 0x00142604
	public virtual void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		this.insideRestore = true;
		if (!this.awakecalled)
		{
			this.Awake();
		}
		bool flag = restorePhysical && !this.physicalLocked;
		bool flag2 = restoreAppearance && !this.appearanceLocked;
		if (flag || flag2)
		{
			foreach (string key in this.boolParamNames)
			{
				JSONStorableBool jsonstorableBool;
				if (this.boolParams.TryGetValue(key, out jsonstorableBool))
				{
					jsonstorableBool.LateRestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key2 in this.floatParamNames)
			{
				JSONStorableFloat jsonstorableFloat;
				if (this.floatParams.TryGetValue(key2, out jsonstorableFloat))
				{
					jsonstorableFloat.LateRestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key3 in this.vector3ParamNames)
			{
				JSONStorableVector3 jsonstorableVector;
				if (this.vector3Params.TryGetValue(key3, out jsonstorableVector))
				{
					jsonstorableVector.LateRestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key4 in this.stringParamNames)
			{
				JSONStorableString jsonstorableString;
				if (this.stringParams.TryGetValue(key4, out jsonstorableString))
				{
					jsonstorableString.LateRestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key5 in this.stringChooserParamNames)
			{
				JSONStorableStringChooser jsonstorableStringChooser;
				if (this.stringChooserParams.TryGetValue(key5, out jsonstorableStringChooser))
				{
					jsonstorableStringChooser.LateRestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key6 in this.urlParamNames)
			{
				JSONStorableUrl jsonstorableUrl;
				if (this.urlParams.TryGetValue(key6, out jsonstorableUrl))
				{
					jsonstorableUrl.LateRestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
			foreach (string key7 in this.colorParamNames)
			{
				JSONStorableColor jsonstorableColor;
				if (this.colorParams.TryGetValue(key7, out jsonstorableColor))
				{
					jsonstorableColor.LateRestoreFromJSON(jc, flag, flag2, setMissingToDefault);
				}
			}
		}
		this.insideRestore = false;
	}

	// Token: 0x0600631C RID: 25372 RVA: 0x00144530 File Offset: 0x00142930
	public virtual void Validate()
	{
	}

	// Token: 0x0600631D RID: 25373 RVA: 0x00144532 File Offset: 0x00142932
	public virtual void PreRestore()
	{
	}

	// Token: 0x0600631E RID: 25374 RVA: 0x00144534 File Offset: 0x00142934
	public virtual void PreRestore(bool restorePhysical, bool restoreAppearance)
	{
	}

	// Token: 0x0600631F RID: 25375 RVA: 0x00144536 File Offset: 0x00142936
	public virtual void PostRestore()
	{
	}

	// Token: 0x06006320 RID: 25376 RVA: 0x00144538 File Offset: 0x00142938
	public virtual void PostRestore(bool restorePhysical, bool restoreAppearance)
	{
	}

	// Token: 0x06006321 RID: 25377 RVA: 0x0014453C File Offset: 0x0014293C
	public virtual void SetDefaultsFromCurrent()
	{
		if (!this.awakecalled)
		{
			this.Awake();
		}
		foreach (string key in this.boolParamNames)
		{
			JSONStorableBool jsonstorableBool;
			if (this.boolParams.TryGetValue(key, out jsonstorableBool))
			{
				jsonstorableBool.SetDefaultFromCurrent();
			}
		}
		foreach (string key2 in this.floatParamNames)
		{
			JSONStorableFloat jsonstorableFloat;
			if (this.floatParams.TryGetValue(key2, out jsonstorableFloat))
			{
				jsonstorableFloat.SetDefaultFromCurrent();
			}
		}
		foreach (string key3 in this.vector3ParamNames)
		{
			JSONStorableVector3 jsonstorableVector;
			if (this.vector3Params.TryGetValue(key3, out jsonstorableVector))
			{
				jsonstorableVector.SetDefaultFromCurrent();
			}
		}
		foreach (string key4 in this.stringParamNames)
		{
			JSONStorableString jsonstorableString;
			if (this.stringParams.TryGetValue(key4, out jsonstorableString))
			{
				jsonstorableString.SetDefaultFromCurrent();
			}
		}
		foreach (string key5 in this.stringChooserParamNames)
		{
			JSONStorableStringChooser jsonstorableStringChooser;
			if (this.stringChooserParams.TryGetValue(key5, out jsonstorableStringChooser))
			{
				jsonstorableStringChooser.SetDefaultFromCurrent();
			}
		}
		foreach (string key6 in this.urlParamNames)
		{
			JSONStorableUrl jsonstorableUrl;
			if (this.urlParams.TryGetValue(key6, out jsonstorableUrl))
			{
				jsonstorableUrl.SetDefaultFromCurrent();
			}
		}
		foreach (string key7 in this.colorParamNames)
		{
			JSONStorableColor jsonstorableColor;
			if (this.colorParams.TryGetValue(key7, out jsonstorableColor))
			{
				jsonstorableColor.SetDefaultFromCurrent();
			}
		}
	}

	// Token: 0x06006322 RID: 25378 RVA: 0x00144800 File Offset: 0x00142C00
	public virtual void PreRemove()
	{
	}

	// Token: 0x06006323 RID: 25379 RVA: 0x00144802 File Offset: 0x00142C02
	public virtual void Remove()
	{
	}

	// Token: 0x06006324 RID: 25380 RVA: 0x00144804 File Offset: 0x00142C04
	protected virtual void InitJSONStorable()
	{
		if (!this._JSONWasInit)
		{
			this._JSONWasInit = true;
			this.allParams = new Dictionary<string, JSONStorableParam>();
			this.allParamsByAltName = new Dictionary<string, JSONStorableParam>();
			this.allParamAndActionNames = new List<string>();
			this.allFloatAndColorParamNames = new List<string>();
			this.InitBoolParams();
			this.InitFloatParams();
			this.InitVector3Params();
			this.InitStringParams();
			this.InitStringChooserParams();
			this.InitUrlParams();
			this.InitColorParams();
			this.InitActions();
			this.InitAudioClipActions();
			this.InitStringChooserActions();
			this.InitSceneFilePathActions();
			this.InitPresetFilePathActions();
			this.saveToStore1Action = new JSONStorableAction("SaveToStore1", new JSONStorableAction.ActionCallback(this.SaveToStore1));
			this.RegisterAction(this.saveToStore1Action);
			this.saveToStore2Action = new JSONStorableAction("SaveToStore2", new JSONStorableAction.ActionCallback(this.SaveToStore2));
			this.RegisterAction(this.saveToStore2Action);
			this.saveToStore3Action = new JSONStorableAction("SaveToStore3", new JSONStorableAction.ActionCallback(this.SaveToStore3));
			this.RegisterAction(this.saveToStore3Action);
			this.restoreAllFromStore1Action = new JSONStorableAction("RestoreAllFromStore1", new JSONStorableAction.ActionCallback(this.RestoreAllFromStore1));
			this.RegisterAction(this.restoreAllFromStore1Action);
			this.restorePhysicalFromStore1Action = new JSONStorableAction("RestorePhysicsFromStore1", new JSONStorableAction.ActionCallback(this.RestorePhysicalFromStore1));
			this.RegisterAction(this.restorePhysicalFromStore1Action);
			this.restoreAppearanceFromStore1Action = new JSONStorableAction("RestoreAppearanceFromStore1", new JSONStorableAction.ActionCallback(this.RestoreAppearanceFromStore1));
			this.RegisterAction(this.restoreAppearanceFromStore1Action);
			this.restoreAllFromStore2Action = new JSONStorableAction("RestoreAllFromStore2", new JSONStorableAction.ActionCallback(this.RestoreAllFromStore2));
			this.RegisterAction(this.restoreAllFromStore2Action);
			this.restorePhysicalFromStore2Action = new JSONStorableAction("RestorePhysicsFromStore2", new JSONStorableAction.ActionCallback(this.RestorePhysicalFromStore2));
			this.RegisterAction(this.restorePhysicalFromStore2Action);
			this.restoreAppearanceFromStore2Action = new JSONStorableAction("RestoreAppearanceFromStore2", new JSONStorableAction.ActionCallback(this.RestoreAppearanceFromStore2));
			this.RegisterAction(this.restoreAppearanceFromStore2Action);
			this.restoreAllFromStore3Action = new JSONStorableAction("RestoreAllFromStore3", new JSONStorableAction.ActionCallback(this.RestoreAllFromStore3));
			this.RegisterAction(this.restoreAllFromStore3Action);
			this.restorePhysicalFromStore3Action = new JSONStorableAction("RestorePhysicsFromStore3", new JSONStorableAction.ActionCallback(this.RestorePhysicalFromStore3));
			this.RegisterAction(this.restorePhysicalFromStore3Action);
			this.restoreAppearanceFromStore3Action = new JSONStorableAction("RestoreAppearanceFromStore3", new JSONStorableAction.ActionCallback(this.RestoreAppearanceFromStore3));
			this.RegisterAction(this.restoreAppearanceFromStore3Action);
			this.restoreAllFromDefaultsAction = new JSONStorableAction("RestoreAllFromDefaults", new JSONStorableAction.ActionCallback(this.RestoreAllFromDefaults));
			this.RegisterAction(this.restoreAllFromDefaultsAction);
			this.restorePhysicalFromDefaultsAction = new JSONStorableAction("RestorePhysicalFromDefaults", new JSONStorableAction.ActionCallback(this.RestorePhysicalFromDefaults));
			this.RegisterAction(this.restorePhysicalFromDefaultsAction);
			this.restoreAppearanceFromDefaultsAction = new JSONStorableAction("RestoreAppearanceFromDefaults", new JSONStorableAction.ActionCallback(this.RestoreAppearanceFromDefaults));
			this.RegisterAction(this.restoreAppearanceFromDefaultsAction);
		}
	}

	// Token: 0x06006325 RID: 25381 RVA: 0x00144AEF File Offset: 0x00142EEF
	protected virtual void Awake()
	{
		if (!this.awakecalled)
		{
			this.awakecalled = true;
			this.InitJSONStorable();
		}
	}

	// Token: 0x0400536B RID: 21355
	public Atom containingAtom;

	// Token: 0x0400536C RID: 21356
	public bool exclude;

	// Token: 0x0400536D RID: 21357
	public bool onlyStoreIfActive;

	// Token: 0x0400536E RID: 21358
	public bool needsStore;

	// Token: 0x0400536F RID: 21359
	public string overrideId;

	// Token: 0x04005370 RID: 21360
	protected string _subScenePrefix;

	// Token: 0x04005371 RID: 21361
	public Transform UITransform;

	// Token: 0x04005372 RID: 21362
	public Transform UITransformAlt;

	// Token: 0x04005373 RID: 21363
	protected static JSONClass copyStore1;

	// Token: 0x04005374 RID: 21364
	protected static JSONClass copyStore2;

	// Token: 0x04005375 RID: 21365
	protected static JSONClass copyStore3;

	// Token: 0x04005376 RID: 21366
	public JSONStorableAction saveToStore1Action;

	// Token: 0x04005377 RID: 21367
	public JSONStorableAction saveToStore2Action;

	// Token: 0x04005378 RID: 21368
	public JSONStorableAction saveToStore3Action;

	// Token: 0x04005379 RID: 21369
	public JSONStorableAction restoreAllFromStore1Action;

	// Token: 0x0400537A RID: 21370
	public JSONStorableAction restorePhysicalFromStore1Action;

	// Token: 0x0400537B RID: 21371
	public JSONStorableAction restoreAppearanceFromStore1Action;

	// Token: 0x0400537C RID: 21372
	public JSONStorableAction restoreAllFromStore2Action;

	// Token: 0x0400537D RID: 21373
	public JSONStorableAction restorePhysicalFromStore2Action;

	// Token: 0x0400537E RID: 21374
	public JSONStorableAction restoreAppearanceFromStore2Action;

	// Token: 0x0400537F RID: 21375
	public JSONStorableAction restoreAllFromStore3Action;

	// Token: 0x04005380 RID: 21376
	public JSONStorableAction restorePhysicalFromStore3Action;

	// Token: 0x04005381 RID: 21377
	public JSONStorableAction restoreAppearanceFromStore3Action;

	// Token: 0x04005382 RID: 21378
	public JSONStorableAction restoreAllFromDefaultsAction;

	// Token: 0x04005383 RID: 21379
	public JSONStorableAction restorePhysicalFromDefaultsAction;

	// Token: 0x04005384 RID: 21380
	public JSONStorableAction restoreAppearanceFromDefaultsAction;

	// Token: 0x04005385 RID: 21381
	protected Dictionary<string, JSONStorableParam> allParams;

	// Token: 0x04005386 RID: 21382
	protected Dictionary<string, JSONStorableParam> allParamsByAltName;

	// Token: 0x04005387 RID: 21383
	protected List<string> allParamAndActionNames;

	// Token: 0x04005388 RID: 21384
	protected List<string> allFloatAndColorParamNames;

	// Token: 0x04005389 RID: 21385
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <physicalLocked>k__BackingField;

	// Token: 0x0400538A RID: 21386
	protected HashSet<string> physicalLocks;

	// Token: 0x0400538B RID: 21387
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <appearanceLocked>k__BackingField;

	// Token: 0x0400538C RID: 21388
	protected HashSet<string> appearanceLocks;

	// Token: 0x0400538D RID: 21389
	protected Dictionary<string, HashSet<string>> customAppearanceParamLocks;

	// Token: 0x0400538E RID: 21390
	protected Dictionary<string, HashSet<string>> customPhysicalParamLocks;

	// Token: 0x0400538F RID: 21391
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <mergeRestore>k__BackingField;

	// Token: 0x04005390 RID: 21392
	protected Dictionary<string, JSONStorableBool> boolParams;

	// Token: 0x04005391 RID: 21393
	protected List<string> boolParamNames;

	// Token: 0x04005392 RID: 21394
	protected Dictionary<string, JSONStorableFloat> floatParams;

	// Token: 0x04005393 RID: 21395
	protected Dictionary<string, JSONStorableFloat> floatParamsByAltName;

	// Token: 0x04005394 RID: 21396
	protected List<string> floatParamNames;

	// Token: 0x04005395 RID: 21397
	protected List<string> floatParamNamesIncludingHidden;

	// Token: 0x04005396 RID: 21398
	protected Dictionary<string, JSONStorableVector3> vector3Params;

	// Token: 0x04005397 RID: 21399
	protected List<string> vector3ParamNames;

	// Token: 0x04005398 RID: 21400
	protected Dictionary<string, JSONStorableString> stringParams;

	// Token: 0x04005399 RID: 21401
	protected List<string> stringParamNames;

	// Token: 0x0400539A RID: 21402
	protected Dictionary<string, JSONStorableStringChooser> stringChooserParams;

	// Token: 0x0400539B RID: 21403
	protected List<string> stringChooserParamNames;

	// Token: 0x0400539C RID: 21404
	protected Dictionary<string, JSONStorableUrl> urlParams;

	// Token: 0x0400539D RID: 21405
	protected List<string> urlParamNames;

	// Token: 0x0400539E RID: 21406
	protected Dictionary<string, JSONStorableColor> colorParams;

	// Token: 0x0400539F RID: 21407
	protected List<string> colorParamNames;

	// Token: 0x040053A0 RID: 21408
	protected HSVColor hsvc;

	// Token: 0x040053A1 RID: 21409
	protected Dictionary<string, JSONStorableAction> actions;

	// Token: 0x040053A2 RID: 21410
	protected List<string> actionNames;

	// Token: 0x040053A3 RID: 21411
	protected Dictionary<string, JSONStorableActionAudioClip> audioClipActions;

	// Token: 0x040053A4 RID: 21412
	protected List<string> audioClipActionNames;

	// Token: 0x040053A5 RID: 21413
	protected Dictionary<string, JSONStorableActionStringChooser> stringChooserActions;

	// Token: 0x040053A6 RID: 21414
	protected List<string> stringChooserActionNames;

	// Token: 0x040053A7 RID: 21415
	protected Dictionary<string, JSONStorableActionSceneFilePath> sceneFilePathActions;

	// Token: 0x040053A8 RID: 21416
	protected List<string> sceneFilePathActionNames;

	// Token: 0x040053A9 RID: 21417
	protected Dictionary<string, JSONStorableActionPresetFilePath> presetFilePathActions;

	// Token: 0x040053AA RID: 21418
	protected List<string> presetFilePathActionNames;

	// Token: 0x040053AB RID: 21419
	protected bool _isPresetStore;

	// Token: 0x040053AC RID: 21420
	protected bool _isPresetRestore;

	// Token: 0x040053AD RID: 21421
	protected bool insideRestore;

	// Token: 0x040053AE RID: 21422
	protected bool _JSONWasInit;

	// Token: 0x040053AF RID: 21423
	protected bool awakecalled;

	// Token: 0x02000CCD RID: 3277
	public enum Type
	{
		// Token: 0x040053B1 RID: 21425
		None,
		// Token: 0x040053B2 RID: 21426
		Bool,
		// Token: 0x040053B3 RID: 21427
		Float,
		// Token: 0x040053B4 RID: 21428
		Vector3,
		// Token: 0x040053B5 RID: 21429
		String,
		// Token: 0x040053B6 RID: 21430
		Url,
		// Token: 0x040053B7 RID: 21431
		StringChooser,
		// Token: 0x040053B8 RID: 21432
		Color,
		// Token: 0x040053B9 RID: 21433
		Action,
		// Token: 0x040053BA RID: 21434
		AudioClipAction,
		// Token: 0x040053BB RID: 21435
		SceneFilePathAction,
		// Token: 0x040053BC RID: 21436
		PresetFilePathAction,
		// Token: 0x040053BD RID: 21437
		StringChooserAction
	}
}
