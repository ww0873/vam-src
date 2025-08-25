using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AA9 RID: 2729
public class DAZBones : ScaleChangeReceiverJSONStorable, PhysicsResetter
{
	// Token: 0x06004781 RID: 18305 RVA: 0x0014EA96 File Offset: 0x0014CE96
	public DAZBones()
	{
	}

	// Token: 0x06004782 RID: 18306 RVA: 0x0014EAA9 File Offset: 0x0014CEA9
	public override void PostRestore(bool restorePhysical, bool restoreAppearance)
	{
		if (restorePhysical)
		{
			this.Init();
			this.SetMorphedTransform(true);
		}
	}

	// Token: 0x06004783 RID: 18307 RVA: 0x0014EAC0 File Offset: 0x0014CEC0
	public void SnapAllBonesToControls()
	{
		if (this.dazBones != null)
		{
			foreach (DAZBone dazbone in this.dazBones)
			{
				if (dazbone.control != null && dazbone.control.control != null)
				{
					dazbone.transform.position = dazbone.control.control.position;
					dazbone.transform.rotation = dazbone.control.control.rotation;
					dazbone.SetResetVelocity();
				}
				else
				{
					dazbone.ResetToStartingLocalPositionRotation();
					dazbone.SetResetVelocity();
				}
			}
			foreach (DAZBone dazbone2 in this.dazBones)
			{
				dazbone2.RepairJoint();
			}
		}
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x0014EB98 File Offset: 0x0014CF98
	public void ResetPhysics()
	{
		this.SnapAllBonesToControls();
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x0014EBA0 File Offset: 0x0014CFA0
	public override void ScaleChanged(float scale)
	{
		float scale2 = this._scale;
		base.ScaleChanged(scale);
		if (this._wasInit)
		{
			this.SetMorphedTransform(true);
			if (this.resetSimulationOnScaleChange && SuperController.singleton != null && Mathf.Abs(scale2 - this._scale) > this.resetSimulationThreshold)
			{
				SuperController.singleton.ResetSimulation(5, "Bone Scale Change", true);
			}
		}
	}

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06004786 RID: 18310 RVA: 0x0014EC11 File Offset: 0x0014D011
	// (set) Token: 0x06004787 RID: 18311 RVA: 0x0014EC19 File Offset: 0x0014D019
	public bool isMale
	{
		get
		{
			return this._isMale;
		}
		set
		{
			if (this._isMale != value)
			{
				this._isMale = value;
				this.SetMorphedTransform(true);
			}
		}
	}

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x06004788 RID: 18312 RVA: 0x0014EC35 File Offset: 0x0014D035
	public Dictionary<string, float> morphGeneralScales
	{
		get
		{
			return this._morphGeneralScales;
		}
	}

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x06004789 RID: 18313 RVA: 0x0014EC3D File Offset: 0x0014D03D
	public float currentGeneralScale
	{
		get
		{
			return this._currentGeneralScale;
		}
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x0014EC48 File Offset: 0x0014D048
	public void SetGeneralScale(string morphName, float sc)
	{
		if (this._morphGeneralScales == null)
		{
			this._morphGeneralScales = new Dictionary<string, float>();
		}
		float num;
		if (this._morphGeneralScales.TryGetValue(morphName, out num))
		{
			this._morphGeneralScales.Remove(morphName);
		}
		if (sc != 0f)
		{
			this._morphGeneralScales.Add(morphName, sc);
		}
		this._currentGeneralScale = 0f;
		foreach (float num2 in this._morphGeneralScales.Values)
		{
			float num3 = num2;
			this._currentGeneralScale += num3;
		}
	}

	// Token: 0x0600478B RID: 18315 RVA: 0x0014ED0C File Offset: 0x0014D10C
	public DAZBone GetDAZBone(string boneName)
	{
		this.Init();
		if (this.boneNameToDAZBone == null)
		{
			return null;
		}
		DAZBone result;
		if (this.boneNameToDAZBone.TryGetValue(boneName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600478C RID: 18316 RVA: 0x0014ED44 File Offset: 0x0014D144
	public DAZBone GetDAZBoneById(string boneId)
	{
		this.Init();
		if (this.boneIdToDAZBone == null)
		{
			return null;
		}
		DAZBone result;
		if (this.boneIdToDAZBone.TryGetValue(boneId, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x0014ED7A File Offset: 0x0014D17A
	public void Reset()
	{
		this._wasInit = false;
		this.boneNameToDAZBone = null;
		this.boneListInOrder = null;
		this.boneIdToDAZBone = null;
		this.Init();
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x0014EDA0 File Offset: 0x0014D1A0
	private void InitBonesRecursive(DAZBone parentBone, Transform t)
	{
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				DAZBones component = transform.GetComponent<DAZBones>();
				if (component == null)
				{
					DAZBone component2 = transform.GetComponent<DAZBone>();
					if (component2 != null)
					{
						component2.Init();
						component2.dazBones = this;
						component2.parentBone = parentBone;
						if (this.boneNameToDAZBone.ContainsKey(component2.name))
						{
							Debug.LogError("Found duplicate bone " + component2.name);
						}
						else
						{
							this.boneNameToDAZBone.Add(component2.name, component2);
							this.boneListInOrder.Add(component2);
						}
						if (this.boneIdToDAZBone.ContainsKey(component2.id))
						{
							Debug.LogError("Found duplicate bone id " + component2.id);
						}
						else
						{
							this.boneIdToDAZBone.Add(component2.id, component2);
						}
						this.InitBonesRecursive(component2, component2.transform);
					}
					else
					{
						this.InitBonesRecursive(null, transform);
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

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x0600478F RID: 18319 RVA: 0x0014EEEC File Offset: 0x0014D2EC
	public bool wasInit
	{
		get
		{
			return this._wasInit;
		}
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x0014EEF4 File Offset: 0x0014D2F4
	public void Init()
	{
		if (!this._wasInit || this.boneNameToDAZBone == null)
		{
			this._wasInit = true;
			this.boneNameToDAZBone = new Dictionary<string, DAZBone>();
			this.boneListInOrder = new List<DAZBone>();
			this.boneIdToDAZBone = new Dictionary<string, DAZBone>();
			this.InitBonesRecursive(null, base.transform);
			this.dazBones = this.boneListInOrder.ToArray();
			this.SetMorphedTransform(true);
		}
	}

	// Token: 0x06004791 RID: 18321 RVA: 0x0014EF64 File Offset: 0x0014D364
	public void SetTransformsToImportValues()
	{
		if (this.dazBones != null)
		{
			foreach (DAZBone dazbone in this.dazBones)
			{
				dazbone.SetTransformToImportValues();
			}
		}
	}

	// Token: 0x06004792 RID: 18322 RVA: 0x0014EFA4 File Offset: 0x0014D3A4
	public void SetMorphedTransform(bool forceAllDirty = false)
	{
		if (this.dazBones != null)
		{
			if (this.dirtyBones == null)
			{
				this.dirtyBones = new List<DAZBone>();
			}
			else
			{
				this.dirtyBones.Clear();
			}
			if (this.parentDirtyBones == null)
			{
				this.parentDirtyBones = new Dictionary<DAZBone, bool>();
			}
			else
			{
				this.parentDirtyBones.Clear();
			}
			if (this.allDirtyBones == null)
			{
				this.allDirtyBones = new List<DAZBone>();
			}
			else
			{
				this.allDirtyBones.Clear();
			}
			if (Application.isPlaying)
			{
				foreach (DAZBone dazbone in this.dazBones)
				{
					if (dazbone.parentForMorphOffsets != null && dazbone.parentForMorphOffsets.transformDirty)
					{
						dazbone.transformDirty = true;
					}
					if (dazbone.transformDirty && dazbone.parentBone != null)
					{
						dazbone.parentBone.childDirty = true;
					}
				}
				foreach (DAZBone dazbone2 in this.dazBones)
				{
					if (forceAllDirty || dazbone2.transformDirty || (dazbone2.parentBone != null && dazbone2.parentBone.transformDirty))
					{
						this.dirtyBones.Add(dazbone2);
						this.allDirtyBones.Add(dazbone2);
						dazbone2.SaveTransform();
					}
					else if (dazbone2.childDirty)
					{
						this.parentDirtyBones.Add(dazbone2, true);
						this.allDirtyBones.Add(dazbone2);
						dazbone2.SaveTransform();
					}
				}
			}
			else
			{
				foreach (DAZBone item in this.dazBones)
				{
					this.dirtyBones.Add(item);
					this.allDirtyBones.Add(item);
				}
			}
			foreach (DAZBone dazbone3 in this.dirtyBones)
			{
				dazbone3.DetachJoint();
				dazbone3.SaveAndDetachParent();
				dazbone3.ResetScale();
			}
			foreach (DAZBone dazbone4 in this.allDirtyBones)
			{
				if (this.parentDirtyBones.ContainsKey(dazbone4))
				{
					dazbone4.SetTransformToMorphPositionAndRotation(this.useScale, this._scale);
				}
				else
				{
					dazbone4.SetMorphedTransform(this.useScale, this._scale);
				}
			}
			if (!Application.isPlaying)
			{
				foreach (DAZBone dazbone5 in this.dirtyBones)
				{
					dazbone5.ApplyOffsetTransform();
				}
			}
			foreach (DAZBone dazbone6 in this.dirtyBones)
			{
				if (dazbone6.parentBone != null && this.parentDirtyBones.ContainsKey(dazbone6.parentBone))
				{
					dazbone6.parentBone.SetTransformToMorphPositionAndRotation(this.useScale, this._scale);
				}
				dazbone6.RestoreParent();
				dazbone6.AttachJoint();
			}
			if (Application.isPlaying)
			{
				foreach (DAZBone dazbone7 in this.allDirtyBones)
				{
					dazbone7.RestoreTransform();
				}
			}
			else
			{
				foreach (DAZBone dazbone8 in this.allDirtyBones)
				{
					dazbone8.ApplyPresetLocalTransforms();
				}
			}
		}
		else
		{
			Debug.LogWarning("SetMorphedTransform called when bones were not init");
		}
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x0014F41C File Offset: 0x0014D81C
	private void OnEnable()
	{
		this.Init();
		this.SetMorphedTransform(true);
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x0014F42B File Offset: 0x0014D82B
	private void Start()
	{
		this.Init();
	}

	// Token: 0x040034C1 RID: 13505
	private Dictionary<string, DAZBone> boneNameToDAZBone;

	// Token: 0x040034C2 RID: 13506
	private List<DAZBone> boneListInOrder;

	// Token: 0x040034C3 RID: 13507
	private Dictionary<string, DAZBone> boneIdToDAZBone;

	// Token: 0x040034C4 RID: 13508
	public DAZBone[] dazBones;

	// Token: 0x040034C5 RID: 13509
	public bool useScale;

	// Token: 0x040034C6 RID: 13510
	public bool resetSimulationOnScaleChange;

	// Token: 0x040034C7 RID: 13511
	public float resetSimulationThreshold = 0.1f;

	// Token: 0x040034C8 RID: 13512
	[SerializeField]
	private bool _isMale;

	// Token: 0x040034C9 RID: 13513
	[SerializeField]
	private Dictionary<string, float> _morphGeneralScales;

	// Token: 0x040034CA RID: 13514
	private float _currentGeneralScale;

	// Token: 0x040034CB RID: 13515
	private bool _wasInit;

	// Token: 0x040034CC RID: 13516
	protected List<DAZBone> dirtyBones;

	// Token: 0x040034CD RID: 13517
	protected Dictionary<DAZBone, bool> parentDirtyBones;

	// Token: 0x040034CE RID: 13518
	protected List<DAZBone> allDirtyBones;
}
