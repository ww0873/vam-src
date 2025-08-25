using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
public class GenerateDAZMorphsControlJSONStorable : JSONStorable
{
	// Token: 0x06004DF6 RID: 19958 RVA: 0x001B7061 File Offset: 0x001B5461
	public GenerateDAZMorphsControlJSONStorable()
	{
	}

	// Token: 0x06004DF7 RID: 19959 RVA: 0x001B707D File Offset: 0x001B547D
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x06004DF8 RID: 19960 RVA: 0x001B7088 File Offset: 0x001B5488
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includeAppearance || forceStore)
		{
			JSONArray jsonarray = new JSONArray();
			json["morphs"] = jsonarray;
			if (this.morphsControlUI != null)
			{
				List<DAZMorph> morphs = this.morphsControlUI.GetMorphs();
				if (morphs != null)
				{
					foreach (DAZMorph dazmorph in morphs)
					{
						JSONClass jsonclass = new JSONClass();
						if (dazmorph.StoreJSON(jsonclass, forceStore))
						{
							jsonarray.Add(jsonclass);
							this.needsStore = true;
						}
					}
				}
				else
				{
					Debug.LogWarning("morphDisplayNames not set for " + base.name);
				}
			}
			else
			{
				Debug.LogWarning("morphsControl UI not set for " + base.name);
			}
		}
		return json;
	}

	// Token: 0x06004DF9 RID: 19961 RVA: 0x001B7180 File Offset: 0x001B5580
	public void ResetMorphs()
	{
		if (this.morphsControlUI != null)
		{
			List<DAZMorph> morphs = this.morphsControlUI.GetMorphs();
			if (morphs != null)
			{
				foreach (DAZMorph dazmorph in morphs)
				{
					dazmorph.Reset();
				}
			}
		}
		if (this.morphBank != null)
		{
			this.morphBank.ResetMorphs();
		}
	}

	// Token: 0x06004DFA RID: 19962 RVA: 0x001B7218 File Offset: 0x001B5618
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.appearanceLocked && restoreAppearance)
		{
			if (!base.mergeRestore)
			{
				this.ResetMorphs();
			}
			if (!base.IsCustomAppearanceParamLocked("morphs"))
			{
				if (jc["morphs"] != null && this.morphsControlUI != null)
				{
					IEnumerator enumerator = jc["morphs"].AsArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							JSONClass jsonclass = (JSONClass)obj;
							string text = jsonclass["name"];
							if (text != null)
							{
								DAZMorph morphByDisplayName = this.morphsControlUI.GetMorphByDisplayName(text);
								if (morphByDisplayName != null)
								{
									morphByDisplayName.RestoreFromJSON(jsonclass);
								}
								else
								{
									SuperController.LogError("Could not find morph " + text + " referenced in save file");
								}
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
				}
				if (this.morphBank != null)
				{
					this.morphBank.ApplyMorphsImmediate();
				}
			}
		}
	}

	// Token: 0x06004DFB RID: 19963 RVA: 0x001B7350 File Offset: 0x001B5750
	public void Init(bool force = false)
	{
		if (!this.wasInit || force)
		{
			this.wasInit = true;
			List<DAZMorph> morphs = this.morphsControlUI.GetMorphs();
			if (morphs != null)
			{
				foreach (DAZMorph dazmorph in morphs)
				{
					base.RegisterFloat(dazmorph.jsonFloat);
				}
			}
		}
	}

	// Token: 0x06004DFC RID: 19964 RVA: 0x001B73D8 File Offset: 0x001B57D8
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init(false);
		}
	}

	// Token: 0x04003DB8 RID: 15800
	public GenerateDAZMorphsControlUI morphsControlUI;

	// Token: 0x04003DB9 RID: 15801
	public DAZMorphBank morphBank;

	// Token: 0x04003DBA RID: 15802
	protected string[] customParamNames = new string[]
	{
		"morphs"
	};

	// Token: 0x04003DBB RID: 15803
	private bool wasInit;
}
