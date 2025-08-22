using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B48 RID: 2888
public class DAZPhysicsMeshSelection : MonoBehaviour
{
	// Token: 0x06004FD7 RID: 20439 RVA: 0x001C8398 File Offset: 0x001C6798
	public DAZPhysicsMeshSelection()
	{
	}

	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x06004FD8 RID: 20440 RVA: 0x001C8410 File Offset: 0x001C6810
	// (set) Token: 0x06004FD9 RID: 20441 RVA: 0x001C8418 File Offset: 0x001C6818
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			this._skin = value;
			if (this._skin != null)
			{
				this.mesh = this._skin.dazMesh;
			}
		}
	}

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x06004FDA RID: 20442 RVA: 0x001C8443 File Offset: 0x001C6843
	// (set) Token: 0x06004FDB RID: 20443 RVA: 0x001C844B File Offset: 0x001C684B
	public int subMeshSelection
	{
		get
		{
			return this._subMeshSelection;
		}
		set
		{
			if (value != this._subMeshSelection)
			{
				this._subMeshSelection = value;
			}
		}
	}

	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x06004FDC RID: 20444 RVA: 0x001C8460 File Offset: 0x001C6860
	// (set) Token: 0x06004FDD RID: 20445 RVA: 0x001C8468 File Offset: 0x001C6868
	public int targetVertex
	{
		get
		{
			return this._targetVertex;
		}
		set
		{
			if (this._targetVertex != value)
			{
				this._changed = true;
				this._targetVertex = value;
				if (this.IsVertexInfluenced(this._targetVertex))
				{
					this.DeselectInfluencedVertex(this._targetVertex);
				}
			}
		}
	}

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x06004FDE RID: 20446 RVA: 0x001C84A1 File Offset: 0x001C68A1
	// (set) Token: 0x06004FDF RID: 20447 RVA: 0x001C84A9 File Offset: 0x001C68A9
	public int anchorVertex
	{
		get
		{
			return this._anchorVertex;
		}
		set
		{
			if (this._anchorVertex != value)
			{
				this._changed = true;
				this._anchorVertex = value;
			}
		}
	}

	// Token: 0x17000B73 RID: 2931
	// (get) Token: 0x06004FE0 RID: 20448 RVA: 0x001C84C5 File Offset: 0x001C68C5
	public List<int> influenceVertices
	{
		get
		{
			return this._influenceVertices;
		}
	}

	// Token: 0x17000B74 RID: 2932
	// (get) Token: 0x06004FE1 RID: 20449 RVA: 0x001C84CD File Offset: 0x001C68CD
	public bool changed
	{
		get
		{
			return this._changed;
		}
	}

	// Token: 0x06004FE2 RID: 20450 RVA: 0x001C84D5 File Offset: 0x001C68D5
	public void clearChanged()
	{
		this._changed = false;
	}

	// Token: 0x17000B75 RID: 2933
	// (get) Token: 0x06004FE3 RID: 20451 RVA: 0x001C84DE File Offset: 0x001C68DE
	// (set) Token: 0x06004FE4 RID: 20452 RVA: 0x001C84E6 File Offset: 0x001C68E6
	public float jointMass
	{
		get
		{
			return this._jointMass;
		}
		set
		{
			if (this._jointMass != value)
			{
				this._jointMass = value;
				if (this.jointRB != null)
				{
					this.jointRB.mass = this._jointMass;
				}
			}
		}
	}

	// Token: 0x17000B76 RID: 2934
	// (get) Token: 0x06004FE5 RID: 20453 RVA: 0x001C851D File Offset: 0x001C691D
	// (set) Token: 0x06004FE6 RID: 20454 RVA: 0x001C8525 File Offset: 0x001C6925
	public float colliderRadius
	{
		get
		{
			return this._colliderRadius;
		}
		set
		{
			if (this._colliderRadius != value)
			{
				this._colliderRadius = value;
				if (this.jointCollider != null)
				{
					this.jointCollider.radius = value;
				}
			}
		}
	}

	// Token: 0x17000B77 RID: 2935
	// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x001C8557 File Offset: 0x001C6957
	// (set) Token: 0x06004FE8 RID: 20456 RVA: 0x001C855F File Offset: 0x001C695F
	public float weightBias
	{
		get
		{
			return this._weightBias;
		}
		set
		{
			if (this._weightBias != value)
			{
				this._weightBias = value;
			}
		}
	}

	// Token: 0x17000B78 RID: 2936
	// (get) Token: 0x06004FE9 RID: 20457 RVA: 0x001C8574 File Offset: 0x001C6974
	// (set) Token: 0x06004FEA RID: 20458 RVA: 0x001C857C File Offset: 0x001C697C
	public bool usePrimaryLimit
	{
		get
		{
			return this._usePrimaryLimit;
		}
		set
		{
			if (this._usePrimaryLimit != value)
			{
				this._usePrimaryLimit = value;
				if (this.joint != null)
				{
					if (this._usePrimaryLimit)
					{
						this.joint.zMotion = ConfigurableJointMotion.Limited;
					}
					else
					{
						this.joint.zMotion = ConfigurableJointMotion.Free;
					}
				}
			}
		}
	}

	// Token: 0x17000B79 RID: 2937
	// (get) Token: 0x06004FEB RID: 20459 RVA: 0x001C85D5 File Offset: 0x001C69D5
	// (set) Token: 0x06004FEC RID: 20460 RVA: 0x001C85E0 File Offset: 0x001C69E0
	public bool useSecondaryLimit
	{
		get
		{
			return this._useSecondaryLimit;
		}
		set
		{
			if (this._useSecondaryLimit != value)
			{
				this._useSecondaryLimit = value;
				if (this.joint != null)
				{
					if (this._useSecondaryLimit)
					{
						this.joint.xMotion = ConfigurableJointMotion.Limited;
						this.joint.yMotion = ConfigurableJointMotion.Limited;
					}
					else
					{
						this.joint.xMotion = ConfigurableJointMotion.Free;
						this.joint.yMotion = ConfigurableJointMotion.Free;
					}
				}
			}
		}
	}

	// Token: 0x17000B7A RID: 2938
	// (get) Token: 0x06004FED RID: 20461 RVA: 0x001C8651 File Offset: 0x001C6A51
	// (set) Token: 0x06004FEE RID: 20462 RVA: 0x001C865C File Offset: 0x001C6A5C
	public float hardLimit
	{
		get
		{
			return this._hardLimit;
		}
		set
		{
			if (this._hardLimit != value)
			{
				this._hardLimit = value;
				if (this.joint != null)
				{
					SoftJointLimit linearLimit = this.joint.linearLimit;
					linearLimit.limit = this._hardLimit;
					this.joint.linearLimit = linearLimit;
				}
			}
		}
	}

	// Token: 0x06004FEF RID: 20463 RVA: 0x001C86B2 File Offset: 0x001C6AB2
	protected void InitList(bool force = false)
	{
		if (this._influenceVertices == null || force)
		{
			this._influenceVertices = new List<int>();
		}
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001C86D0 File Offset: 0x001C6AD0
	protected void InitDict(bool force = false)
	{
		this.InitList(force);
		if (this.influenceVerticesDict == null || force)
		{
			this.influenceVerticesDict = new Dictionary<int, bool>();
			foreach (int key in this.influenceVertices)
			{
				this.influenceVerticesDict.Add(key, true);
			}
		}
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001C8758 File Offset: 0x001C6B58
	public bool IsVertexInfluenced(int vid)
	{
		this.InitDict(false);
		return this.influenceVerticesDict.ContainsKey(vid);
	}

	// Token: 0x06004FF2 RID: 20466 RVA: 0x001C8770 File Offset: 0x001C6B70
	public void SelectInfluencedVertex(int vid)
	{
		this.InitDict(false);
		if (vid >= 0 && vid <= this.mesh.numUVVertices && !this.influenceVerticesDict.ContainsKey(vid) && vid != this._targetVertex)
		{
			this._changed = true;
			this._influenceVertices.Add(vid);
			this.influenceVerticesDict.Add(vid, true);
		}
	}

	// Token: 0x06004FF3 RID: 20467 RVA: 0x001C87D9 File Offset: 0x001C6BD9
	public void DeselectInfluencedVertex(int vid)
	{
		this.InitDict(false);
		if (this.influenceVerticesDict.ContainsKey(vid))
		{
			this._changed = true;
			this._influenceVertices.Remove(vid);
			this.influenceVerticesDict.Remove(vid);
		}
	}

	// Token: 0x06004FF4 RID: 20468 RVA: 0x001C8814 File Offset: 0x001C6C14
	public void ToggleInfluencedVertexSelection(int vid)
	{
		this.InitDict(false);
		this._changed = true;
		if (this.influenceVerticesDict.ContainsKey(vid))
		{
			this._influenceVertices.Remove(vid);
			this.influenceVerticesDict.Remove(vid);
		}
		else if (vid != this._targetVertex)
		{
			this._influenceVertices.Add(vid);
			this.influenceVerticesDict.Add(vid, true);
		}
	}

	// Token: 0x06004FF5 RID: 20469 RVA: 0x001C8884 File Offset: 0x001C6C84
	public void ClearInfluencedSelection()
	{
		this._changed = true;
		this.InitDict(true);
	}

	// Token: 0x06004FF6 RID: 20470 RVA: 0x001C8894 File Offset: 0x001C6C94
	private void Init()
	{
		if (!this.wasInit && this.skin != null && this.skin.rawSkinnedVerts != null)
		{
			this.wasInit = true;
			this.CreateJoint();
			this.InitWeights();
		}
	}

	// Token: 0x06004FF7 RID: 20471 RVA: 0x001C88E0 File Offset: 0x001C6CE0
	private void InitWeights()
	{
		this.influenceVerticesArray = new int[this.influenceVertices.Count];
		this.influenceVerticesWeights = new float[this.influenceVertices.Count];
		int num = 0;
		foreach (int num2 in this.influenceVertices)
		{
			this.influenceVerticesArray[num] = num2;
			float num3 = Mathf.Max(0f, 1f - (this.skin.rawSkinnedVerts[num2] - this.skin.rawSkinnedVerts[this.targetVertex]).magnitude / this.maxInfluenceDistance);
			this.influenceVerticesWeights[num] = num3;
			num++;
		}
	}

	// Token: 0x06004FF8 RID: 20472 RVA: 0x001C89D4 File Offset: 0x001C6DD4
	private void CreateJoint()
	{
		GameObject gameObject = new GameObject("PhysicsMeshKRB");
		this.kinematicTransform = gameObject.transform;
		this.kinematicTransform.SetParent(base.transform);
		this.kinematicRB = gameObject.AddComponent<Rigidbody>();
		this.kinematicRB.isKinematic = true;
		this.kinematicTransform.position = this.skin.rawSkinnedVerts[this.targetVertex];
		this.kinematicTransform.LookAt(this.skin.rawSkinnedVerts[this.anchorVertex]);
		GameObject gameObject2 = new GameObject("PhysicsMeshJoint");
		this.jointTransform = gameObject2.transform;
		this.jointTransform.SetParent(this.kinematicTransform);
		this.jointTransform.localPosition = Vector3.zero;
		this.jointTransform.localRotation = Quaternion.identity;
		this.jointRB = gameObject2.AddComponent<Rigidbody>();
		this.jointRB.mass = this._jointMass;
		this.jointRB.useGravity = false;
		this.jointRB.drag = 0f;
		this.jointRB.angularDrag = 0f;
		this.jointRB.interpolation = RigidbodyInterpolation.None;
		this.jointRB.collisionDetectionMode = CollisionDetectionMode.Discrete;
		this.jointRB.isKinematic = false;
		this.jointRB.constraints = RigidbodyConstraints.FreezeRotation;
		this.joint = gameObject2.AddComponent<ConfigurableJoint>();
		if (this._usePrimaryLimit)
		{
			this.joint.zMotion = ConfigurableJointMotion.Limited;
		}
		else
		{
			this.joint.zMotion = ConfigurableJointMotion.Free;
		}
		if (this._useSecondaryLimit)
		{
			this.joint.xMotion = ConfigurableJointMotion.Limited;
			this.joint.yMotion = ConfigurableJointMotion.Limited;
		}
		else
		{
			this.joint.xMotion = ConfigurableJointMotion.Free;
			this.joint.yMotion = ConfigurableJointMotion.Free;
		}
		this.joint.angularXMotion = ConfigurableJointMotion.Locked;
		this.joint.angularYMotion = ConfigurableJointMotion.Locked;
		this.joint.angularZMotion = ConfigurableJointMotion.Locked;
		JointDrive zDrive = default(JointDrive);
		zDrive.positionSpring = 4f;
		zDrive.positionDamper = 0.4f;
		zDrive.maximumForce = 100000f;
		JointDrive jointDrive = default(JointDrive);
		jointDrive.positionSpring = 40f;
		jointDrive.positionDamper = 4f;
		jointDrive.maximumForce = 100000f;
		this.joint.xDrive = jointDrive;
		this.joint.yDrive = jointDrive;
		this.joint.zDrive = zDrive;
		SoftJointLimit linearLimit = this.joint.linearLimit;
		linearLimit.limit = this._hardLimit;
		this.joint.linearLimit = linearLimit;
		this.joint.connectedBody = this.kinematicRB;
		this.jointCollider = gameObject2.AddComponent<SphereCollider>();
		this.jointCollider.radius = this._colliderRadius;
		if (this.jointMaterial != null)
		{
			this.jointCollider.sharedMaterial = this.jointMaterial;
		}
		gameObject2.layer = LayerMask.NameToLayer(this.colliderLayer);
	}

	// Token: 0x06004FF9 RID: 20473 RVA: 0x001C8CCC File Offset: 0x001C70CC
	private void UpdateJoint()
	{
		if (this.wasInit)
		{
			this.kinematicTransform.position = this.skin.rawSkinnedVerts[this.targetVertex];
			this.kinematicTransform.LookAt(this.skin.rawSkinnedVerts[this.anchorVertex]);
			Vector3 localPosition = this.jointTransform.localPosition;
			if (localPosition.z < 0f)
			{
				localPosition.z = 0f;
				this.jointTransform.localPosition = localPosition;
			}
		}
	}

	// Token: 0x06004FFA RID: 20474 RVA: 0x001C8D68 File Offset: 0x001C7168
	private void MorphVertices()
	{
		if (this.wasInit)
		{
			Vector3 vector = this.jointRB.position - this.skin.rawSkinnedVerts[this.targetVertex];
			this.skin.postSkinMorphs[this.targetVertex] = vector;
			for (int i = 0; i < this.influenceVerticesArray.Length; i++)
			{
				int num = this.influenceVerticesArray[i];
				float num2 = this.influenceVerticesWeights[i];
				if (this.weightBias > 0f)
				{
					num2 = (1f - this.weightBias) * num2 + this.weightBias;
				}
				else
				{
					num2 = (1f + this.weightBias) * num2;
				}
				this.skin.postSkinMorphs[num] = vector * num2 * 1.1f;
			}
		}
	}

	// Token: 0x06004FFB RID: 20475 RVA: 0x001C8E54 File Offset: 0x001C7254
	private void FixedUpdate()
	{
		this.Init();
		this.UpdateJoint();
		this.MorphVertices();
	}

	// Token: 0x04003FAD RID: 16301
	public Transform meshTransform;

	// Token: 0x04003FAE RID: 16302
	public DAZMesh mesh;

	// Token: 0x04003FAF RID: 16303
	[SerializeField]
	private DAZSkinV2 _skin;

	// Token: 0x04003FB0 RID: 16304
	public DAZPhysicsMeshSelection.SelectionMode selectionMode;

	// Token: 0x04003FB1 RID: 16305
	public string selectionName;

	// Token: 0x04003FB2 RID: 16306
	[SerializeField]
	protected int _subMeshSelection = -1;

	// Token: 0x04003FB3 RID: 16307
	[SerializeField]
	protected int _targetVertex = -1;

	// Token: 0x04003FB4 RID: 16308
	[SerializeField]
	protected int _anchorVertex = -1;

	// Token: 0x04003FB5 RID: 16309
	[SerializeField]
	protected List<int> _influenceVertices;

	// Token: 0x04003FB6 RID: 16310
	protected Dictionary<int, bool> influenceVerticesDict;

	// Token: 0x04003FB7 RID: 16311
	public int[] influenceVerticesArray;

	// Token: 0x04003FB8 RID: 16312
	public float[] influenceVerticesWeights;

	// Token: 0x04003FB9 RID: 16313
	public float maxInfluenceDistance = 0.02f;

	// Token: 0x04003FBA RID: 16314
	public bool showSelection;

	// Token: 0x04003FBB RID: 16315
	public bool showBackfaces;

	// Token: 0x04003FBC RID: 16316
	public bool showLinkLines;

	// Token: 0x04003FBD RID: 16317
	public float handleSize = 0.0005f;

	// Token: 0x04003FBE RID: 16318
	[SerializeField]
	protected bool _changed;

	// Token: 0x04003FBF RID: 16319
	public string colliderLayer = "SkinMorph";

	// Token: 0x04003FC0 RID: 16320
	public PhysicMaterial jointMaterial;

	// Token: 0x04003FC1 RID: 16321
	private Transform kinematicTransform;

	// Token: 0x04003FC2 RID: 16322
	private Rigidbody kinematicRB;

	// Token: 0x04003FC3 RID: 16323
	private Transform jointTransform;

	// Token: 0x04003FC4 RID: 16324
	private Rigidbody jointRB;

	// Token: 0x04003FC5 RID: 16325
	private ConfigurableJoint joint;

	// Token: 0x04003FC6 RID: 16326
	private SphereCollider jointCollider;

	// Token: 0x04003FC7 RID: 16327
	[SerializeField]
	private float _jointMass = 0.01f;

	// Token: 0x04003FC8 RID: 16328
	[SerializeField]
	private float _colliderRadius = 0.002f;

	// Token: 0x04003FC9 RID: 16329
	[SerializeField]
	private float _weightBias;

	// Token: 0x04003FCA RID: 16330
	[SerializeField]
	private bool _usePrimaryLimit = true;

	// Token: 0x04003FCB RID: 16331
	[SerializeField]
	private bool _useSecondaryLimit = true;

	// Token: 0x04003FCC RID: 16332
	[SerializeField]
	private float _hardLimit = 0.01f;

	// Token: 0x04003FCD RID: 16333
	private bool wasInit;

	// Token: 0x02000B49 RID: 2889
	public enum SelectionMode
	{
		// Token: 0x04003FCF RID: 16335
		Target,
		// Token: 0x04003FD0 RID: 16336
		Anchor,
		// Token: 0x04003FD1 RID: 16337
		Influenced
	}
}
