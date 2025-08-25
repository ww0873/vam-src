using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AF8 RID: 2808
[ExecuteInEditMode]
public class DAZMorphSubBank : MonoBehaviour
{
	// Token: 0x06004BA4 RID: 19364 RVA: 0x001A55A1 File Offset: 0x001A39A1
	public DAZMorphSubBank()
	{
	}

	// Token: 0x17000ACD RID: 2765
	// (get) Token: 0x06004BA5 RID: 19365 RVA: 0x001A55B0 File Offset: 0x001A39B0
	public List<DAZMorph> morphs
	{
		get
		{
			return this._morphs;
		}
	}

	// Token: 0x17000ACE RID: 2766
	// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x001A55B8 File Offset: 0x001A39B8
	public List<DAZMorph> runtimeMorphs
	{
		get
		{
			return this._runtimeMorphs;
		}
	}

	// Token: 0x17000ACF RID: 2767
	// (get) Token: 0x06004BA7 RID: 19367 RVA: 0x001A55C0 File Offset: 0x001A39C0
	public List<DAZMorph> transientMorphs
	{
		get
		{
			return this._transientMorphs;
		}
	}

	// Token: 0x17000AD0 RID: 2768
	// (get) Token: 0x06004BA8 RID: 19368 RVA: 0x001A55C8 File Offset: 0x001A39C8
	public List<DAZMorph> packageMorphs
	{
		get
		{
			return this._packageMorphs;
		}
	}

	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x001A55D0 File Offset: 0x001A39D0
	public List<DAZMorph> combinedMorphs
	{
		get
		{
			if (this._combinedMorphs == null)
			{
				this.RebuildMorphsByNameAndUid();
			}
			return this._combinedMorphs;
		}
	}

	// Token: 0x06004BAA RID: 19370 RVA: 0x001A55EC File Offset: 0x001A39EC
	public bool ClearRuntimeMorphs()
	{
		bool flag = false;
		if (this._runtimeMorphs != null && this._runtimeMorphs.Count > 0)
		{
			flag = true;
			this._runtimeMorphs.Clear();
		}
		if (flag)
		{
			this.RebuildMorphsByNameAndUid();
		}
		return flag;
	}

	// Token: 0x06004BAB RID: 19371 RVA: 0x001A5634 File Offset: 0x001A3A34
	public bool ClearTransientMorphs()
	{
		bool flag = false;
		if (this._transientMorphs != null && this._transientMorphs.Count > 0)
		{
			flag = true;
			this._transientMorphs.Clear();
		}
		if (flag)
		{
			this.RebuildMorphsByNameAndUid();
		}
		return flag;
	}

	// Token: 0x06004BAC RID: 19372 RVA: 0x001A567C File Offset: 0x001A3A7C
	public bool ClearPackageMorphs()
	{
		bool flag = false;
		if (this._packageMorphs != null && this._packageMorphs.Count > 0)
		{
			flag = true;
			this._packageMorphs.Clear();
		}
		if (flag)
		{
			this.RebuildMorphsByNameAndUid();
		}
		return flag;
	}

	// Token: 0x06004BAD RID: 19373 RVA: 0x001A56C1 File Offset: 0x001A3AC1
	public void CompleteRuntimeMorphAdd()
	{
		if (this._addedRuntime)
		{
			this.RebuildMorphsByNameAndUid();
			this._addedRuntime = false;
		}
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x001A56DB File Offset: 0x001A3ADB
	public void CompleteTransientMorphAdd()
	{
		if (this._addedTransients)
		{
			this.RebuildMorphsByNameAndUid();
			this._addedTransients = false;
		}
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x001A56F5 File Offset: 0x001A3AF5
	public void CompletePackageMorphAdd()
	{
		if (this._addedPackage)
		{
			this.RebuildMorphsByNameAndUid();
			this._addedPackage = false;
		}
	}

	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x06004BB0 RID: 19376 RVA: 0x001A570F File Offset: 0x001A3B0F
	public int numMorphs
	{
		get
		{
			return this._morphs.Count;
		}
	}

	// Token: 0x06004BB1 RID: 19377 RVA: 0x001A571C File Offset: 0x001A3B1C
	protected void RebuildMorphsByNameAndUid()
	{
		if (this._morphs == null)
		{
			this._morphs = new List<DAZMorph>();
		}
		if (this._runtimeMorphs == null)
		{
			this._runtimeMorphs = new List<DAZMorph>();
		}
		if (this._transientMorphs == null)
		{
			this._transientMorphs = new List<DAZMorph>();
		}
		if (this._packageMorphs == null)
		{
			this._packageMorphs = new List<DAZMorph>();
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
		foreach (DAZMorph dazmorph in this._morphs)
		{
			dazmorph.morphSubBank = this;
			this._morphsByName.Add(dazmorph.morphName, dazmorph);
			if (!dazmorph.disable)
			{
				if (!this._morphsByUid.ContainsKey(dazmorph.uid))
				{
					this._morphsByUid.Add(dazmorph.uid, dazmorph);
				}
				else
				{
					Debug.LogError("Found duplicate morph uid " + dazmorph.uid);
				}
			}
		}
		foreach (DAZMorph dazmorph2 in this._packageMorphs)
		{
			dazmorph2.morphSubBank = this;
			if (!this._morphsByName.ContainsKey(dazmorph2.morphName))
			{
				this._morphsByName.Add(dazmorph2.morphName, dazmorph2);
			}
			if (!this._morphsByUid.ContainsKey(dazmorph2.uid))
			{
				this._morphsByUid.Add(dazmorph2.uid, dazmorph2);
			}
			else
			{
				Debug.LogError("Found duplicate morph uid " + dazmorph2.uid);
			}
		}
		foreach (DAZMorph dazmorph3 in this._runtimeMorphs)
		{
			dazmorph3.morphSubBank = this;
			if (!this._morphsByName.ContainsKey(dazmorph3.morphName))
			{
				this._morphsByName.Add(dazmorph3.morphName, dazmorph3);
			}
			if (!this._morphsByUid.ContainsKey(dazmorph3.uid))
			{
				this._morphsByUid.Add(dazmorph3.uid, dazmorph3);
			}
			else
			{
				Debug.LogError("Found duplicate morph uid " + dazmorph3.uid);
			}
		}
		foreach (DAZMorph dazmorph4 in this._transientMorphs)
		{
			dazmorph4.morphSubBank = this;
			if (!this._morphsByName.ContainsKey(dazmorph4.morphName))
			{
				this._morphsByName.Add(dazmorph4.morphName, dazmorph4);
			}
			if (!this._morphsByUid.ContainsKey(dazmorph4.uid))
			{
				this._morphsByUid.Add(dazmorph4.uid, dazmorph4);
			}
			else
			{
				Debug.LogError("Found duplicate morph uid " + dazmorph4.uid);
			}
		}
		this._combinedMorphs = this._morphsByUid.Values.ToList<DAZMorph>();
	}

	// Token: 0x06004BB2 RID: 19378 RVA: 0x001A5AC4 File Offset: 0x001A3EC4
	public DAZMorph GetMorph(string morphName)
	{
		if (this._morphsByName == null)
		{
			this.RebuildMorphsByNameAndUid();
		}
		DAZMorph result;
		if (this._morphsByName.TryGetValue(morphName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x001A5AF8 File Offset: 0x001A3EF8
	public DAZMorph GetMorphByUid(string morphUid)
	{
		if (this._morphsByUid == null)
		{
			this.RebuildMorphsByNameAndUid();
		}
		DAZMorph result;
		if (this._morphsByUid.TryGetValue(morphUid, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x001A5B2C File Offset: 0x001A3F2C
	public void AddMorph(DAZMorph dm)
	{
		if (this._morphs == null)
		{
			this._morphs = new List<DAZMorph>();
		}
		if (this._morphsByName == null || this._morphsByUid == null)
		{
			this.RebuildMorphsByNameAndUid();
		}
		dm.morphSubBank = this;
		DAZMorph dazmorph;
		if (dm.isTransient)
		{
			this._addedTransients = true;
			this._transientMorphs.Add(dm);
		}
		else if (dm.isInPackage)
		{
			this._addedPackage = true;
			this._packageMorphs.Add(dm);
		}
		else if (dm.isRuntime)
		{
			this._addedRuntime = true;
			this._runtimeMorphs.Add(dm);
		}
		else if (this._morphsByName.TryGetValue(dm.morphName, out dazmorph))
		{
			dm.CopyParameters(dazmorph, true);
			this._morphsByName.Remove(dm.morphName);
			this._morphsByName.Add(dm.morphName, dm);
			int num = this._morphs.IndexOf(dazmorph);
			if (num != -1)
			{
				this._morphs[num] = dm;
			}
			else
			{
				Debug.LogError("Should have found DAZMorph " + dm.morphName + " in morphs list during add");
			}
			num = this._combinedMorphs.IndexOf(dazmorph);
			if (num != -1)
			{
				this._combinedMorphs[num] = dm;
			}
			else
			{
				Debug.LogError("Should have found DAZMorph " + dm.morphName + " in morphs list during add");
			}
		}
		else
		{
			this._morphsByName.Add(dm.morphName, dm);
			this._morphs.Add(dm);
			this._combinedMorphs.Add(dm);
		}
	}

	// Token: 0x06004BB5 RID: 19381 RVA: 0x001A5CD0 File Offset: 0x001A40D0
	public void RemoveMorph(string morphName)
	{
		if (this._morphsByName == null)
		{
			this.RebuildMorphsByNameAndUid();
		}
		DAZMorph dazmorph;
		if (this._morphsByName.TryGetValue(morphName, out dazmorph))
		{
			if (dazmorph.isTransient)
			{
				Debug.LogError("RemoveMorph does not work with transient morphs. Use clear transient morphs instead.");
			}
			else if (dazmorph.isRuntime)
			{
				Debug.LogError("RemoveMorph does not work with runtime morphs. Use clear runtime morphs instead.");
			}
			else
			{
				this._morphsByName.Remove(morphName);
				this._morphs.Remove(dazmorph);
			}
		}
	}

	// Token: 0x06004BB6 RID: 19382 RVA: 0x001A5D4F File Offset: 0x001A414F
	private void OnEnable()
	{
		this.RebuildMorphsByNameAndUid();
	}

	// Token: 0x04003A6D RID: 14957
	public bool alwaysSetDirtyOnEditorChange = true;

	// Token: 0x04003A6E RID: 14958
	[SerializeField]
	protected List<DAZMorph> _morphs;

	// Token: 0x04003A6F RID: 14959
	protected List<DAZMorph> _runtimeMorphs;

	// Token: 0x04003A70 RID: 14960
	protected List<DAZMorph> _transientMorphs;

	// Token: 0x04003A71 RID: 14961
	protected List<DAZMorph> _packageMorphs;

	// Token: 0x04003A72 RID: 14962
	protected List<DAZMorph> _combinedMorphs;

	// Token: 0x04003A73 RID: 14963
	public bool simpleView;

	// Token: 0x04003A74 RID: 14964
	public bool useOverrideRegionName;

	// Token: 0x04003A75 RID: 14965
	public string overrideRegionName;

	// Token: 0x04003A76 RID: 14966
	protected Dictionary<string, DAZMorph> _morphsByName;

	// Token: 0x04003A77 RID: 14967
	protected Dictionary<string, DAZMorph> _morphsByUid;

	// Token: 0x04003A78 RID: 14968
	protected bool _addedRuntime;

	// Token: 0x04003A79 RID: 14969
	protected bool _addedTransients;

	// Token: 0x04003A7A RID: 14970
	protected bool _addedPackage;
}
