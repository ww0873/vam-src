using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MVR;
using UnityEngine;

// Token: 0x02000B2B RID: 2859
[ExecuteInEditMode]
public class SetAnchorFromVertex : PhysicsSimulatorJSONStorable
{
	// Token: 0x06004E35 RID: 20021 RVA: 0x001B8DB0 File Offset: 0x001B71B0
	public SetAnchorFromVertex()
	{
	}

	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x06004E36 RID: 20022 RVA: 0x001B8E03 File Offset: 0x001B7203
	// (set) Token: 0x06004E37 RID: 20023 RVA: 0x001B8E0B File Offset: 0x001B720B
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			if (this._skin != value)
			{
				this._skin = value;
				this.InitSkin();
			}
		}
	}

	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x06004E38 RID: 20024 RVA: 0x001B8E2B File Offset: 0x001B722B
	// (set) Token: 0x06004E39 RID: 20025 RVA: 0x001B8E33 File Offset: 0x001B7233
	public bool isEnabled
	{
		[CompilerGenerated]
		get
		{
			return this.<isEnabled>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<isEnabled>k__BackingField = value;
		}
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001B8E3C File Offset: 0x001B723C
	public void ClickVertex(int vid)
	{
		if (this.targetVertex == vid)
		{
			this.targetVertex = -1;
		}
		else
		{
			this.targetVertex = vid;
		}
	}

	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x06004E3B RID: 20027 RVA: 0x001B8E60 File Offset: 0x001B7260
	protected Dictionary<int, int> uvVertToBaseVertDict
	{
		get
		{
			if (this._uvVertToBaseVertDict == null)
			{
				if (this.skin != null && this.skin.dazMesh != null)
				{
					this._uvVertToBaseVertDict = this.skin.dazMesh.uvVertToBaseVert;
				}
				else
				{
					this._uvVertToBaseVertDict = new Dictionary<int, int>();
				}
			}
			return this._uvVertToBaseVertDict;
		}
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x001B8ECC File Offset: 0x001B72CC
	public int GetBaseVertex(int vid)
	{
		int num;
		if (this.skin != null && this.skin.dazMesh != null && this.uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		return vid;
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x001B8F17 File Offset: 0x001B7317
	public bool IsBaseVertex(int vid)
	{
		return !(this.skin != null) || !(this.skin.dazMesh != null) || !this.uvVertToBaseVertDict.ContainsKey(vid);
	}

	// Token: 0x06004E3E RID: 20030 RVA: 0x001B8F54 File Offset: 0x001B7354
	protected override void SyncCollisionEnabled()
	{
		base.SyncCollisionEnabled();
		if (this.joint != null)
		{
			Rigidbody component = this.joint.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.detectCollisions = (this._collisionEnabled && !this._resetSimulation);
			}
		}
	}

	// Token: 0x06004E3F RID: 20031 RVA: 0x001B8FB0 File Offset: 0x001B73B0
	protected override void SyncUseInterpolation()
	{
		base.SyncUseInterpolation();
		if (this.joint != null)
		{
			Rigidbody component = this.joint.GetComponent<Rigidbody>();
			if (component != null)
			{
				if (this._useInterpolation)
				{
					component.interpolation = RigidbodyInterpolation.Interpolate;
				}
				else
				{
					component.interpolation = RigidbodyInterpolation.None;
				}
			}
		}
	}

	// Token: 0x06004E40 RID: 20032 RVA: 0x001B900C File Offset: 0x001B740C
	public void PrepThreadUpdate(bool isForThread = true)
	{
		if (this.dazBone == null && isForThread)
		{
			this.connectedBodyWorldToLocalMatrix = this.joint.connectedBody.transform.worldToLocalMatrix;
		}
		this.currentAnchor = this.joint.connectedAnchor;
	}

	// Token: 0x06004E41 RID: 20033 RVA: 0x001B905C File Offset: 0x001B745C
	public void DoThreadUpdate(Vector3[] vertsToUse, bool isRunningOnThread = true)
	{
		Vector3 v = vertsToUse[this.targetVertex];
		if (NaNUtils.IsVector3Valid(v))
		{
			this.target = v;
		}
		else
		{
			this.detectedPhysicsCorruptionOnThread = true;
			this.physicsCorruptionType = "Vertex";
		}
		if (isRunningOnThread)
		{
			if (this.dazBone != null)
			{
				if (NaNUtils.IsMatrixValid(this.dazBone.worldToLocalMatrix))
				{
					this.newAnchor = this.dazBone.worldToLocalMatrix.MultiplyPoint3x4(this.target);
				}
				else
				{
					this.detectedPhysicsCorruptionOnThread = true;
					this.physicsCorruptionType = "Matrix";
				}
			}
			else if (NaNUtils.IsMatrixValid(this.connectedBodyWorldToLocalMatrix))
			{
				this.newAnchor = this.connectedBodyWorldToLocalMatrix.MultiplyPoint3x4(this.target);
			}
			else
			{
				this.detectedPhysicsCorruptionOnThread = true;
				this.physicsCorruptionType = "Matrix";
			}
		}
		else
		{
			this.newAnchor = this.joint.connectedBody.transform.InverseTransformPoint(this.target);
		}
		if (!this.setX)
		{
			this.newAnchor.x = this.currentAnchor.x;
		}
		if (!this.setY)
		{
			this.newAnchor.y = this.currentAnchor.y;
		}
		if (!this.setZ)
		{
			this.newAnchor.z = this.currentAnchor.z;
		}
	}

	// Token: 0x06004E42 RID: 20034 RVA: 0x001B91D0 File Offset: 0x001B75D0
	public void FinishThreadUpdate()
	{
		if (this.detectedPhysicsCorruptionOnThread)
		{
			if (this.containingAtom != null)
			{
				this.containingAtom.AlertPhysicsCorruption("SetAnchorFromVertex " + this.physicsCorruptionType + " " + base.name);
			}
			this.detectedPhysicsCorruptionOnThread = false;
		}
		this.SyncAnchorToTarget();
	}

	// Token: 0x06004E43 RID: 20035 RVA: 0x001B922C File Offset: 0x001B762C
	protected void SyncAnchorToTarget()
	{
		this.joint.connectedAnchor = this.newAnchor;
		if (Application.isPlaying)
		{
			if (this._resetSimulation)
			{
				base.transform.localPosition = this.newAnchor;
				base.transform.localRotation = this.initialLocalRotation;
				if (this.jointRB != null)
				{
					this.jointRB.velocity = Vector3.zero;
					this.jointRB.angularVelocity = Vector3.zero;
				}
			}
		}
		else
		{
			base.transform.localPosition = this.newAnchor;
		}
	}

	// Token: 0x06004E44 RID: 20036 RVA: 0x001B92C8 File Offset: 0x001B76C8
	protected void InitSkin()
	{
		if (this._skin != null && Application.isPlaying)
		{
			if (!this._skin.postSkinVerts[this.targetVertex])
			{
				this._skin.postSkinVerts[this.targetVertex] = true;
				this.skin.postSkinVertsChanged = true;
			}
			if (!this.initialLocalRotationWasInit)
			{
				this.initialLocalRotation = base.transform.localRotation;
				this.initialLocalRotationWasInit = true;
			}
			if (this.dazBone == null)
			{
				this.dazBone = this.joint.connectedBody.transform.GetComponent<DAZBone>();
			}
		}
	}

	// Token: 0x06004E45 RID: 20037 RVA: 0x001B9378 File Offset: 0x001B7778
	protected override void Update()
	{
		if (this.doUpdate)
		{
			base.Update();
			if (this.joint != null && this._skin != null && this.targetVertex != -1)
			{
				bool flag = true;
				Vector3[] array;
				if (Application.isPlaying)
				{
					array = this.skin.rawSkinnedVerts;
					flag = this.skin.postSkinVertsReady[this.targetVertex];
				}
				else
				{
					array = this.skin.dazMesh.morphedUVVertices;
				}
				if (array != null && this.targetVertex < array.Length && flag)
				{
					this.PrepThreadUpdate(false);
					this.DoThreadUpdate(array, false);
					this.FinishThreadUpdate();
				}
			}
		}
	}

	// Token: 0x06004E46 RID: 20038 RVA: 0x001B9434 File Offset: 0x001B7834
	protected void OnEnable()
	{
		this.isEnabled = true;
	}

	// Token: 0x06004E47 RID: 20039 RVA: 0x001B943D File Offset: 0x001B783D
	protected void OnDisable()
	{
		this.isEnabled = false;
	}

	// Token: 0x04003DE6 RID: 15846
	[HideInInspector]
	public Transform skinTransform;

	// Token: 0x04003DE7 RID: 15847
	protected DAZSkinV2 _skin;

	// Token: 0x04003DE8 RID: 15848
	[HideInInspector]
	public int subMeshSelection;

	// Token: 0x04003DE9 RID: 15849
	public int targetVertex = -1;

	// Token: 0x04003DEA RID: 15850
	public bool doUpdate = true;

	// Token: 0x04003DEB RID: 15851
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <isEnabled>k__BackingField;

	// Token: 0x04003DEC RID: 15852
	public ConfigurableJoint joint;

	// Token: 0x04003DED RID: 15853
	public Rigidbody jointRB;

	// Token: 0x04003DEE RID: 15854
	protected bool initialLocalRotationWasInit;

	// Token: 0x04003DEF RID: 15855
	protected Quaternion initialLocalRotation;

	// Token: 0x04003DF0 RID: 15856
	public bool setX = true;

	// Token: 0x04003DF1 RID: 15857
	public bool setY = true;

	// Token: 0x04003DF2 RID: 15858
	public bool setZ = true;

	// Token: 0x04003DF3 RID: 15859
	public bool showHandles = true;

	// Token: 0x04003DF4 RID: 15860
	public bool showBackfaceHandles;

	// Token: 0x04003DF5 RID: 15861
	public float handleSize = 0.0002f;

	// Token: 0x04003DF6 RID: 15862
	protected Dictionary<int, int> _uvVertToBaseVertDict;

	// Token: 0x04003DF7 RID: 15863
	public Vector3 target;

	// Token: 0x04003DF8 RID: 15864
	protected Vector3 currentAnchor;

	// Token: 0x04003DF9 RID: 15865
	protected Vector3 newAnchor;

	// Token: 0x04003DFA RID: 15866
	protected DAZBone dazBone;

	// Token: 0x04003DFB RID: 15867
	protected Matrix4x4 connectedBodyWorldToLocalMatrix;

	// Token: 0x04003DFC RID: 15868
	protected bool detectedPhysicsCorruptionOnThread;

	// Token: 0x04003DFD RID: 15869
	protected string physicsCorruptionType = string.Empty;
}
