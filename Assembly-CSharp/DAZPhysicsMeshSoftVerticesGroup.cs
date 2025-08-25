using System;
using System.Collections;
using System.Collections.Generic;
using GPUTools.Physics.Scripts.Behaviours;
using MVR;
using UnityEngine;

// Token: 0x02000B4B RID: 2891
[Serializable]
public class DAZPhysicsMeshSoftVerticesGroup
{
	// Token: 0x06005001 RID: 20481 RVA: 0x001C9024 File Offset: 0x001C7424
	public DAZPhysicsMeshSoftVerticesGroup()
	{
		this._softVerticesSets = new List<DAZPhysicsMeshSoftVerticesSet>();
	}

	// Token: 0x06005002 RID: 20482 RVA: 0x001C91E8 File Offset: 0x001C75E8
	protected void SyncOn()
	{
		if (this._on && !this._freeze)
		{
			this.ResetJoints();
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				if (dazphysicsMeshSoftVerticesSet.jointTransform != null)
				{
					dazphysicsMeshSoftVerticesSet.jointTransform.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			for (int j = 0; j < this._softVerticesSets.Count; j++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet2 = this._softVerticesSets[j];
				if (dazphysicsMeshSoftVerticesSet2.jointTransform != null)
				{
					dazphysicsMeshSoftVerticesSet2.jointTransform.gameObject.SetActive(false);
				}
				if (this._skin != null && !this._freeze)
				{
					if (this._skin.wasInit)
					{
						this._skin.postSkinMorphs[dazphysicsMeshSoftVerticesSet2.targetVertex] = this.zero;
						for (int k = 0; k < dazphysicsMeshSoftVerticesSet2.influenceVertices.Length; k++)
						{
							int num = dazphysicsMeshSoftVerticesSet2.influenceVertices[k];
							this._skin.postSkinMorphs[num] = this.zero;
						}
					}
					this._skin.postSkinVertsChanged = true;
				}
			}
		}
	}

	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x06005003 RID: 20483 RVA: 0x001C934A File Offset: 0x001C774A
	// (set) Token: 0x06005004 RID: 20484 RVA: 0x001C9352 File Offset: 0x001C7752
	public bool on
	{
		get
		{
			return this._on;
		}
		set
		{
			if (this._on != value)
			{
				this._on = value;
				this.SyncOn();
			}
		}
	}

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x06005005 RID: 20485 RVA: 0x001C936D File Offset: 0x001C776D
	// (set) Token: 0x06005006 RID: 20486 RVA: 0x001C9375 File Offset: 0x001C7775
	public bool freeze
	{
		get
		{
			return this._freeze;
		}
		set
		{
			if (this._freeze != value)
			{
				this._freeze = value;
				this.SyncOn();
			}
		}
	}

	// Token: 0x06005007 RID: 20487 RVA: 0x001C9390 File Offset: 0x001C7790
	public void ScaleChanged(float sc, float oneoversc)
	{
		this.scale = sc;
		this.oneoverscale = oneoversc;
		this.SyncJointParams();
		this.AdjustLinkJointDistances(true);
		this.triggerThreadedScaleChange = true;
	}

	// Token: 0x06005008 RID: 20488 RVA: 0x001C93B4 File Offset: 0x001C77B4
	public bool IsAllowedToCollideWithGroup(int groupNum)
	{
		if (this._otherGroupNumsCollisionAllowedHash == null)
		{
			this._otherGroupNumsCollisionAllowedHash = new HashSet<int>();
			foreach (int item in this.otherGroupNumsCollisionAllowed)
			{
				this._otherGroupNumsCollisionAllowedHash.Add(item);
			}
		}
		return this._otherGroupNumsCollisionAllowedHash.Contains(groupNum);
	}

	// Token: 0x06005009 RID: 20489 RVA: 0x001C9410 File Offset: 0x001C7810
	public void SyncCollisionEnabled()
	{
		for (int i = 0; i < this._softVerticesSets.Count; i++)
		{
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
			if (dazphysicsMeshSoftVerticesSet.jointRB != null)
			{
				dazphysicsMeshSoftVerticesSet.jointRB.detectCollisions = (this._collisionEnabled && !this._resetSimulation);
			}
		}
	}

	// Token: 0x17000B7F RID: 2943
	// (get) Token: 0x0600500A RID: 20490 RVA: 0x001C9479 File Offset: 0x001C7879
	// (set) Token: 0x0600500B RID: 20491 RVA: 0x001C9481 File Offset: 0x001C7881
	public bool collisionEnabled
	{
		get
		{
			return this._collisionEnabled;
		}
		set
		{
			if (this._collisionEnabled != value)
			{
				this._collisionEnabled = value;
				this.SyncCollisionEnabled();
			}
		}
	}

	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x0600500C RID: 20492 RVA: 0x001C949C File Offset: 0x001C789C
	public List<DAZPhysicsMeshSoftVerticesSet> softVerticesSets
	{
		get
		{
			return this._softVerticesSets;
		}
	}

	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x0600500D RID: 20493 RVA: 0x001C94A4 File Offset: 0x001C78A4
	// (set) Token: 0x0600500E RID: 20494 RVA: 0x001C94AC File Offset: 0x001C78AC
	public int currentSetIndex
	{
		get
		{
			return this._currentSetIndex;
		}
		set
		{
			if (this._currentSetIndex != value && value >= 0 && value < this._softVerticesSets.Count)
			{
				this._currentSetIndex = value;
			}
		}
	}

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x0600500F RID: 20495 RVA: 0x001C94D9 File Offset: 0x001C78D9
	// (set) Token: 0x06005010 RID: 20496 RVA: 0x001C9510 File Offset: 0x001C7910
	public DAZPhysicsMeshSoftVerticesSet currentSet
	{
		get
		{
			if (this._currentSetIndex >= 0 && this._currentSetIndex < this._softVerticesSets.Count)
			{
				return this._softVerticesSets[this._currentSetIndex];
			}
			return null;
		}
		set
		{
			if (value != this._softVerticesSets[this._currentSetIndex])
			{
				for (int i = 0; i < this._softVerticesSets.Count; i++)
				{
					if (value == this._softVerticesSets[i])
					{
						this._currentSetIndex = i;
						break;
					}
				}
			}
		}
	}

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x06005011 RID: 20497 RVA: 0x001C956E File Offset: 0x001C796E
	// (set) Token: 0x06005012 RID: 20498 RVA: 0x001C9576 File Offset: 0x001C7976
	public DAZPhysicsMeshSoftVerticesGroup.InfluenceType influenceType
	{
		get
		{
			return this._influenceType;
		}
		set
		{
			if (this._influenceType != value)
			{
				this._influenceType = value;
			}
		}
	}

	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x06005013 RID: 20499 RVA: 0x001C958B File Offset: 0x001C798B
	// (set) Token: 0x06005014 RID: 20500 RVA: 0x001C9594 File Offset: 0x001C7994
	public bool autoInfluenceAnchor
	{
		get
		{
			return this._autoInfluenceAnchor;
		}
		set
		{
			if (this._autoInfluenceAnchor != value)
			{
				this._autoInfluenceAnchor = value;
				for (int i = 0; i < this._softVerticesSets.Count; i++)
				{
					this._softVerticesSets[i].autoInfluenceAnchor = this._autoInfluenceAnchor;
				}
			}
		}
	}

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x06005015 RID: 20501 RVA: 0x001C95E7 File Offset: 0x001C79E7
	// (set) Token: 0x06005016 RID: 20502 RVA: 0x001C95EF File Offset: 0x001C79EF
	public float maxInfluenceDistance
	{
		get
		{
			return this._maxInfluenceDistance;
		}
		set
		{
			if (this._maxInfluenceDistance != value)
			{
				this._maxInfluenceDistance = value;
			}
		}
	}

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x06005017 RID: 20503 RVA: 0x001C9604 File Offset: 0x001C7A04
	// (set) Token: 0x06005018 RID: 20504 RVA: 0x001C960C File Offset: 0x001C7A0C
	public DAZPhysicsMeshSoftVerticesGroup.LookAtOption lookAtOption
	{
		get
		{
			return this._lookAtOption;
		}
		set
		{
			if (this._lookAtOption != value)
			{
				this._lookAtOption = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x06005019 RID: 20505 RVA: 0x001C9627 File Offset: 0x001C7A27
	// (set) Token: 0x0600501A RID: 20506 RVA: 0x001C962F File Offset: 0x001C7A2F
	public float falloffPower
	{
		get
		{
			return this._falloffPower;
		}
		set
		{
			if (this._falloffPower != value)
			{
				this._falloffPower = value;
			}
		}
	}

	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x0600501B RID: 20507 RVA: 0x001C9644 File Offset: 0x001C7A44
	// (set) Token: 0x0600501C RID: 20508 RVA: 0x001C964C File Offset: 0x001C7A4C
	public float weightMultiplier
	{
		get
		{
			return this._weightMultiplier;
		}
		set
		{
			if (this._weightMultiplier != value)
			{
				this._weightMultiplier = value;
			}
		}
	}

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x0600501D RID: 20509 RVA: 0x001C9661 File Offset: 0x001C7A61
	// (set) Token: 0x0600501E RID: 20510 RVA: 0x001C9669 File Offset: 0x001C7A69
	public float jointSpringNormal
	{
		get
		{
			return this._jointSpringNormal;
		}
		set
		{
			if (this._jointSpringNormal != value)
			{
				this._jointSpringNormal = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x0600501F RID: 20511 RVA: 0x001C9684 File Offset: 0x001C7A84
	// (set) Token: 0x06005020 RID: 20512 RVA: 0x001C968C File Offset: 0x001C7A8C
	public float jointDamperNormal
	{
		get
		{
			return this._jointDamperNormal;
		}
		set
		{
			if (this._jointDamperNormal != value)
			{
				this._jointDamperNormal = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x06005021 RID: 20513 RVA: 0x001C96A7 File Offset: 0x001C7AA7
	// (set) Token: 0x06005022 RID: 20514 RVA: 0x001C96AF File Offset: 0x001C7AAF
	public float jointSpringTangent
	{
		get
		{
			return this._jointSpringTangent;
		}
		set
		{
			if (this._jointSpringTangent != value)
			{
				this._jointSpringTangent = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x06005023 RID: 20515 RVA: 0x001C96CA File Offset: 0x001C7ACA
	// (set) Token: 0x06005024 RID: 20516 RVA: 0x001C96D2 File Offset: 0x001C7AD2
	public float jointDamperTangent
	{
		get
		{
			return this._jointDamperTangent;
		}
		set
		{
			if (this._jointDamperTangent != value)
			{
				this._jointDamperTangent = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x06005025 RID: 20517 RVA: 0x001C96ED File Offset: 0x001C7AED
	// (set) Token: 0x06005026 RID: 20518 RVA: 0x001C96F5 File Offset: 0x001C7AF5
	public float jointSpringTangent2
	{
		get
		{
			return this._jointSpringTangent2;
		}
		set
		{
			if (this._jointSpringTangent2 != value)
			{
				this._jointSpringTangent2 = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x06005027 RID: 20519 RVA: 0x001C9710 File Offset: 0x001C7B10
	// (set) Token: 0x06005028 RID: 20520 RVA: 0x001C9718 File Offset: 0x001C7B18
	public float jointDamperTangent2
	{
		get
		{
			return this._jointDamperTangent2;
		}
		set
		{
			if (this._jointDamperTangent2 != value)
			{
				this._jointDamperTangent2 = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x06005029 RID: 20521 RVA: 0x001C9733 File Offset: 0x001C7B33
	// (set) Token: 0x0600502A RID: 20522 RVA: 0x001C973B File Offset: 0x001C7B3B
	public float jointSpringMaxForce
	{
		get
		{
			return this._jointSpringMaxForce;
		}
		set
		{
			if (this._jointSpringMaxForce != value)
			{
				this._jointSpringMaxForce = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B90 RID: 2960
	// (get) Token: 0x0600502B RID: 20523 RVA: 0x001C9756 File Offset: 0x001C7B56
	// (set) Token: 0x0600502C RID: 20524 RVA: 0x001C975E File Offset: 0x001C7B5E
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
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B91 RID: 2961
	// (get) Token: 0x0600502D RID: 20525 RVA: 0x001C9779 File Offset: 0x001C7B79
	// (set) Token: 0x0600502E RID: 20526 RVA: 0x001C9781 File Offset: 0x001C7B81
	public DAZPhysicsMeshSoftVerticesGroup.ColliderOrient colliderOrient
	{
		get
		{
			return this._colliderOrient;
		}
		set
		{
			if (this._colliderOrient != value)
			{
				this._colliderOrient = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000B92 RID: 2962
	// (get) Token: 0x0600502F RID: 20527 RVA: 0x001C979C File Offset: 0x001C7B9C
	// (set) Token: 0x06005030 RID: 20528 RVA: 0x001C97A4 File Offset: 0x001C7BA4
	public DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType
	{
		get
		{
			return this._colliderType;
		}
		set
		{
			if (this._colliderType != value)
			{
				if (!Application.isPlaying)
				{
					this._colliderType = value;
				}
				else
				{
					Debug.LogWarning("Cannot change colliderType at runtime");
				}
			}
		}
	}

	// Token: 0x17000B93 RID: 2963
	// (get) Token: 0x06005031 RID: 20529 RVA: 0x001C97D2 File Offset: 0x001C7BD2
	public bool colliderSyncDirty
	{
		get
		{
			return this._colliderSyncDirty;
		}
	}

	// Token: 0x06005032 RID: 20530 RVA: 0x001C97DC File Offset: 0x001C7BDC
	public void SyncColliders()
	{
		if (this.wasInit)
		{
			this._colliderSyncDirty = false;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet colliders = this._softVerticesSets[i];
				this.SetColliders(colliders);
			}
		}
	}

	// Token: 0x06005033 RID: 20531 RVA: 0x001C982C File Offset: 0x001C7C2C
	public void SyncCollidersThreaded()
	{
		if (this.wasInit)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet collidersThreaded = this._softVerticesSets[i];
				this.SetCollidersThreaded(collidersThreaded);
			}
		}
	}

	// Token: 0x06005034 RID: 20532 RVA: 0x001C9874 File Offset: 0x001C7C74
	public void SyncCollidersThreadedFinish()
	{
		if (this.wasInit)
		{
			this._colliderSyncDirty = false;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet collidersThreadedFinish = this._softVerticesSets[i];
				this.SetCollidersThreadedFinish(collidersThreadedFinish);
			}
		}
	}

	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x06005035 RID: 20533 RVA: 0x001C98C3 File Offset: 0x001C7CC3
	// (set) Token: 0x06005036 RID: 20534 RVA: 0x001C98CB File Offset: 0x001C7CCB
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
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000B95 RID: 2965
	// (set) Token: 0x06005037 RID: 20535 RVA: 0x001C98E6 File Offset: 0x001C7CE6
	public float colliderRadiusNoSync
	{
		set
		{
			if (this._colliderRadius != value)
			{
				this._colliderRadius = value;
				this._colliderSyncDirty = true;
			}
		}
	}

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06005038 RID: 20536 RVA: 0x001C9902 File Offset: 0x001C7D02
	// (set) Token: 0x06005039 RID: 20537 RVA: 0x001C990A File Offset: 0x001C7D0A
	public float colliderLength
	{
		get
		{
			return this._colliderLength;
		}
		set
		{
			if (this._colliderLength != value)
			{
				this._colliderLength = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x0600503A RID: 20538 RVA: 0x001C9925 File Offset: 0x001C7D25
	// (set) Token: 0x0600503B RID: 20539 RVA: 0x001C992D File Offset: 0x001C7D2D
	public float colliderNormalOffset
	{
		get
		{
			return this._colliderNormalOffset;
		}
		set
		{
			if (this._colliderNormalOffset != value)
			{
				this._colliderNormalOffset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000B98 RID: 2968
	// (set) Token: 0x0600503C RID: 20540 RVA: 0x001C9948 File Offset: 0x001C7D48
	public float colliderNormalOffsetNoSync
	{
		set
		{
			if (this._colliderNormalOffset != value)
			{
				this._colliderNormalOffset = value;
				this._colliderSyncDirty = true;
			}
		}
	}

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x0600503D RID: 20541 RVA: 0x001C9964 File Offset: 0x001C7D64
	// (set) Token: 0x0600503E RID: 20542 RVA: 0x001C996C File Offset: 0x001C7D6C
	public float colliderAdditionalNormalOffset
	{
		get
		{
			return this._colliderAdditionalNormalOffset;
		}
		set
		{
			if (this._colliderAdditionalNormalOffset != value)
			{
				this._colliderAdditionalNormalOffset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x0600503F RID: 20543 RVA: 0x001C9987 File Offset: 0x001C7D87
	// (set) Token: 0x06005040 RID: 20544 RVA: 0x001C998F File Offset: 0x001C7D8F
	public float colliderTangentOffset
	{
		get
		{
			return this._colliderTangentOffset;
		}
		set
		{
			if (this._colliderTangentOffset != value)
			{
				this._colliderTangentOffset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x06005041 RID: 20545 RVA: 0x001C99AA File Offset: 0x001C7DAA
	// (set) Token: 0x06005042 RID: 20546 RVA: 0x001C99B2 File Offset: 0x001C7DB2
	public float colliderTangent2Offset
	{
		get
		{
			return this._colliderTangent2Offset;
		}
		set
		{
			if (this._colliderTangent2Offset != value)
			{
				this._colliderTangent2Offset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x06005043 RID: 20547 RVA: 0x001C99CD File Offset: 0x001C7DCD
	// (set) Token: 0x06005044 RID: 20548 RVA: 0x001C99D5 File Offset: 0x001C7DD5
	public string colliderLayer
	{
		get
		{
			return this._colliderLayer;
		}
		set
		{
			if (this._colliderLayer != value)
			{
				if (!Application.isPlaying)
				{
					this._colliderLayer = value;
				}
				else
				{
					Debug.LogWarning("Cannot change colliderLayer at runtime");
				}
			}
		}
	}

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x06005045 RID: 20549 RVA: 0x001C9A08 File Offset: 0x001C7E08
	// (set) Token: 0x06005046 RID: 20550 RVA: 0x001C9A10 File Offset: 0x001C7E10
	public bool useSecondCollider
	{
		get
		{
			return this._useSecondCollider;
		}
		set
		{
			if (this._useSecondCollider != value)
			{
				if (!Application.isPlaying)
				{
					this._useSecondCollider = value;
				}
				else
				{
					Debug.LogWarning("Cannot change useSecondCollider at runtime");
				}
			}
		}
	}

	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x06005047 RID: 20551 RVA: 0x001C9A3E File Offset: 0x001C7E3E
	// (set) Token: 0x06005048 RID: 20552 RVA: 0x001C9A46 File Offset: 0x001C7E46
	public bool addGPUCollider
	{
		get
		{
			return this._addGPUCollider;
		}
		set
		{
			if (this._addGPUCollider != value)
			{
				if (!Application.isPlaying)
				{
					this._addGPUCollider = value;
				}
				else
				{
					Debug.LogWarning("Cannot change addGPUCollider at runtime");
				}
			}
		}
	}

	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x06005049 RID: 20553 RVA: 0x001C9A74 File Offset: 0x001C7E74
	// (set) Token: 0x0600504A RID: 20554 RVA: 0x001C9A7C File Offset: 0x001C7E7C
	public bool addSecondGPUCollider
	{
		get
		{
			return this._addSecondGPUCollider;
		}
		set
		{
			if (this._addSecondGPUCollider != value)
			{
				if (!Application.isPlaying)
				{
					this._addSecondGPUCollider = value;
				}
				else
				{
					Debug.LogWarning("Cannot change addSecondGPUCollider at runtime");
				}
			}
		}
	}

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x0600504B RID: 20555 RVA: 0x001C9AAA File Offset: 0x001C7EAA
	// (set) Token: 0x0600504C RID: 20556 RVA: 0x001C9AB2 File Offset: 0x001C7EB2
	public float secondColliderRadius
	{
		get
		{
			return this._secondColliderRadius;
		}
		set
		{
			if (this._secondColliderRadius != value)
			{
				this._secondColliderRadius = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000BA1 RID: 2977
	// (set) Token: 0x0600504D RID: 20557 RVA: 0x001C9ACD File Offset: 0x001C7ECD
	public float secondColliderRadiusNoSync
	{
		set
		{
			if (this._secondColliderRadius != value)
			{
				this._secondColliderRadius = value;
				this._colliderSyncDirty = true;
			}
		}
	}

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x0600504E RID: 20558 RVA: 0x001C9AE9 File Offset: 0x001C7EE9
	// (set) Token: 0x0600504F RID: 20559 RVA: 0x001C9AF1 File Offset: 0x001C7EF1
	public float secondColliderLength
	{
		get
		{
			return this._secondColliderLength;
		}
		set
		{
			if (this._secondColliderLength != value)
			{
				this._secondColliderLength = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06005050 RID: 20560 RVA: 0x001C9B0C File Offset: 0x001C7F0C
	// (set) Token: 0x06005051 RID: 20561 RVA: 0x001C9B14 File Offset: 0x001C7F14
	public float secondColliderNormalOffset
	{
		get
		{
			return this._secondColliderNormalOffset;
		}
		set
		{
			if (this._secondColliderNormalOffset != value)
			{
				this._secondColliderNormalOffset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000BA4 RID: 2980
	// (set) Token: 0x06005052 RID: 20562 RVA: 0x001C9B2F File Offset: 0x001C7F2F
	public float secondColliderNormalOffsetNoSync
	{
		set
		{
			if (this._secondColliderNormalOffset != value)
			{
				this._secondColliderNormalOffset = value;
				this._colliderSyncDirty = true;
			}
		}
	}

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x06005053 RID: 20563 RVA: 0x001C9B4B File Offset: 0x001C7F4B
	// (set) Token: 0x06005054 RID: 20564 RVA: 0x001C9B53 File Offset: 0x001C7F53
	public float secondColliderAdditionalNormalOffset
	{
		get
		{
			return this._secondColliderAdditionalNormalOffset;
		}
		set
		{
			if (this._secondColliderAdditionalNormalOffset != value)
			{
				this._secondColliderAdditionalNormalOffset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x06005055 RID: 20565 RVA: 0x001C9B6E File Offset: 0x001C7F6E
	// (set) Token: 0x06005056 RID: 20566 RVA: 0x001C9B76 File Offset: 0x001C7F76
	public float secondColliderTangentOffset
	{
		get
		{
			return this._secondColliderTangentOffset;
		}
		set
		{
			if (this._secondColliderTangentOffset != value)
			{
				this._secondColliderTangentOffset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x06005057 RID: 20567 RVA: 0x001C9B91 File Offset: 0x001C7F91
	// (set) Token: 0x06005058 RID: 20568 RVA: 0x001C9B99 File Offset: 0x001C7F99
	public float secondColliderTangent2Offset
	{
		get
		{
			return this._secondColliderTangent2Offset;
		}
		set
		{
			if (this._secondColliderTangent2Offset != value)
			{
				this._secondColliderTangent2Offset = value;
				this.SyncColliders();
			}
		}
	}

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x06005059 RID: 20569 RVA: 0x001C9BB4 File Offset: 0x001C7FB4
	// (set) Token: 0x0600505A RID: 20570 RVA: 0x001C9BBC File Offset: 0x001C7FBC
	public Transform[] ignoreColliders
	{
		get
		{
			return this._ignoreColliders;
		}
		set
		{
			if (this._ignoreColliders != value)
			{
				this._ignoreColliders = value;
			}
		}
	}

	// Token: 0x17000BA9 RID: 2985
	// (get) Token: 0x0600505B RID: 20571 RVA: 0x001C9BD1 File Offset: 0x001C7FD1
	// (set) Token: 0x0600505C RID: 20572 RVA: 0x001C9BD9 File Offset: 0x001C7FD9
	public PhysicMaterial colliderMaterial
	{
		get
		{
			return this._colliderMaterial;
		}
		set
		{
			if (this._colliderMaterial != value)
			{
				this._colliderMaterial = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000BAA RID: 2986
	// (get) Token: 0x0600505D RID: 20573 RVA: 0x001C9BF9 File Offset: 0x001C7FF9
	// (set) Token: 0x0600505E RID: 20574 RVA: 0x001C9C01 File Offset: 0x001C8001
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

	// Token: 0x17000BAB RID: 2987
	// (get) Token: 0x0600505F RID: 20575 RVA: 0x001C9C16 File Offset: 0x001C8016
	// (set) Token: 0x06005060 RID: 20576 RVA: 0x001C9C1E File Offset: 0x001C801E
	public bool useUniformLimit
	{
		get
		{
			return this._useUniformLimit;
		}
		set
		{
			if (this._useUniformLimit != value)
			{
				this._useUniformLimit = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x06005061 RID: 20577 RVA: 0x001C9C39 File Offset: 0x001C8039
	// (set) Token: 0x06005062 RID: 20578 RVA: 0x001C9C41 File Offset: 0x001C8041
	public DAZPhysicsMeshSoftVerticesGroup.MovementType normalMovementType
	{
		get
		{
			return this._normalMovementType;
		}
		set
		{
			if (this._normalMovementType != value)
			{
				this._normalMovementType = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x06005063 RID: 20579 RVA: 0x001C9C5C File Offset: 0x001C805C
	// (set) Token: 0x06005064 RID: 20580 RVA: 0x001C9C64 File Offset: 0x001C8064
	public DAZPhysicsMeshSoftVerticesGroup.MovementType tangentMovementType
	{
		get
		{
			return this._tangentMovementType;
		}
		set
		{
			if (this._tangentMovementType != value)
			{
				this._tangentMovementType = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000BAE RID: 2990
	// (get) Token: 0x06005065 RID: 20581 RVA: 0x001C9C7F File Offset: 0x001C807F
	// (set) Token: 0x06005066 RID: 20582 RVA: 0x001C9C87 File Offset: 0x001C8087
	public DAZPhysicsMeshSoftVerticesGroup.MovementType tangent2MovementType
	{
		get
		{
			return this._tangent2MovementType;
		}
		set
		{
			if (this._tangent2MovementType != value)
			{
				this._tangent2MovementType = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000BAF RID: 2991
	// (get) Token: 0x06005067 RID: 20583 RVA: 0x001C9CA2 File Offset: 0x001C80A2
	// (set) Token: 0x06005068 RID: 20584 RVA: 0x001C9CAA File Offset: 0x001C80AA
	public Transform normalReference
	{
		get
		{
			return this._normalReference;
		}
		set
		{
			if (this._normalReference != value)
			{
				this._normalReference = value;
			}
		}
	}

	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06005069 RID: 20585 RVA: 0x001C9CC4 File Offset: 0x001C80C4
	// (set) Token: 0x0600506A RID: 20586 RVA: 0x001C9CCC File Offset: 0x001C80CC
	public float normalDistanceLimit
	{
		get
		{
			return this._normalDistanceLimit;
		}
		set
		{
			if (this._normalDistanceLimit != value)
			{
				this._normalDistanceLimit = value;
				if (this._useUniformLimit)
				{
					this.SyncJointParams();
				}
			}
		}
	}

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x0600506B RID: 20587 RVA: 0x001C9CF2 File Offset: 0x001C80F2
	// (set) Token: 0x0600506C RID: 20588 RVA: 0x001C9CFA File Offset: 0x001C80FA
	public float normalNegativeDistanceLimit
	{
		get
		{
			return this._normalNegativeDistanceLimit;
		}
		set
		{
			if (this._normalNegativeDistanceLimit != value)
			{
				this._normalNegativeDistanceLimit = value;
			}
		}
	}

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x0600506D RID: 20589 RVA: 0x001C9D0F File Offset: 0x001C810F
	// (set) Token: 0x0600506E RID: 20590 RVA: 0x001C9D17 File Offset: 0x001C8117
	public float tangentDistanceLimit
	{
		get
		{
			return this._tangentDistanceLimit;
		}
		set
		{
			if (this._tangentDistanceLimit != value)
			{
				this._tangentDistanceLimit = value;
			}
		}
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x0600506F RID: 20591 RVA: 0x001C9D2C File Offset: 0x001C812C
	// (set) Token: 0x06005070 RID: 20592 RVA: 0x001C9D34 File Offset: 0x001C8134
	public float tangentNegativeDistanceLimit
	{
		get
		{
			return this._tangentNegativeDistanceLimit;
		}
		set
		{
			if (this._tangentNegativeDistanceLimit != value)
			{
				this._tangentNegativeDistanceLimit = value;
			}
		}
	}

	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06005071 RID: 20593 RVA: 0x001C9D49 File Offset: 0x001C8149
	// (set) Token: 0x06005072 RID: 20594 RVA: 0x001C9D51 File Offset: 0x001C8151
	public float tangent2DistanceLimit
	{
		get
		{
			return this._tangent2DistanceLimit;
		}
		set
		{
			if (this._tangent2DistanceLimit != value)
			{
				this._tangent2DistanceLimit = value;
			}
		}
	}

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x06005073 RID: 20595 RVA: 0x001C9D66 File Offset: 0x001C8166
	// (set) Token: 0x06005074 RID: 20596 RVA: 0x001C9D6E File Offset: 0x001C816E
	public float tangent2NegativeDistanceLimit
	{
		get
		{
			return this._tangent2NegativeDistanceLimit;
		}
		set
		{
			if (this._tangent2NegativeDistanceLimit != value)
			{
				this._tangent2NegativeDistanceLimit = value;
			}
		}
	}

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06005075 RID: 20597 RVA: 0x001C9D83 File Offset: 0x001C8183
	// (set) Token: 0x06005076 RID: 20598 RVA: 0x001C9D8B File Offset: 0x001C818B
	public float linkSpring
	{
		get
		{
			return this._linkSpring;
		}
		set
		{
			if (this._linkSpring != value)
			{
				this._linkSpring = value;
				this.SyncLinkParams();
			}
		}
	}

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06005077 RID: 20599 RVA: 0x001C9DA6 File Offset: 0x001C81A6
	// (set) Token: 0x06005078 RID: 20600 RVA: 0x001C9DAE File Offset: 0x001C81AE
	public float linkDamper
	{
		get
		{
			return this._linkDamper;
		}
		set
		{
			if (this._linkDamper != value)
			{
				this._linkDamper = value;
				this.SyncLinkParams();
			}
		}
	}

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x06005079 RID: 20601 RVA: 0x001C9DC9 File Offset: 0x001C81C9
	// (set) Token: 0x0600507A RID: 20602 RVA: 0x001C9DD1 File Offset: 0x001C81D1
	public bool resetSimulation
	{
		get
		{
			return this._resetSimulation;
		}
		set
		{
			if (this._resetSimulation != value)
			{
				this._resetSimulation = value;
				this.SyncCollisionEnabled();
			}
		}
	}

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x0600507B RID: 20603 RVA: 0x001C9DEC File Offset: 0x001C81EC
	// (set) Token: 0x0600507C RID: 20604 RVA: 0x001C9DF4 File Offset: 0x001C81F4
	public bool useSimulation
	{
		get
		{
			return this._useSimulation;
		}
		set
		{
			if (this._useSimulation != value)
			{
				if (!Application.isPlaying)
				{
					this._useSimulation = value;
				}
				else
				{
					Debug.LogWarning("Cannot change useSimulation at runtime");
				}
			}
		}
	}

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x0600507D RID: 20605 RVA: 0x001C9E22 File Offset: 0x001C8222
	// (set) Token: 0x0600507E RID: 20606 RVA: 0x001C9E2A File Offset: 0x001C822A
	public bool clampVelocity
	{
		get
		{
			return this._clampVelocity;
		}
		set
		{
			if (this._clampVelocity != value)
			{
				this._clampVelocity = value;
			}
		}
	}

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x0600507F RID: 20607 RVA: 0x001C9E3F File Offset: 0x001C823F
	// (set) Token: 0x06005080 RID: 20608 RVA: 0x001C9E47 File Offset: 0x001C8247
	public float maxSimulationVelocity
	{
		get
		{
			return this._maxSimulationVelocity;
		}
		set
		{
			if (this._maxSimulationVelocity != value)
			{
				this._maxSimulationVelocity = value;
				this._maxSimulationVelocitySqr = value * value;
			}
		}
	}

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x06005081 RID: 20609 RVA: 0x001C9E65 File Offset: 0x001C8265
	// (set) Token: 0x06005082 RID: 20610 RVA: 0x001C9E6D File Offset: 0x001C826D
	public bool useInterpolation
	{
		get
		{
			return this._useInterpolation;
		}
		set
		{
			if (this._useInterpolation != value)
			{
				this._useInterpolation = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x06005083 RID: 20611 RVA: 0x001C9E88 File Offset: 0x001C8288
	// (set) Token: 0x06005084 RID: 20612 RVA: 0x001C9E90 File Offset: 0x001C8290
	public int solverIterations
	{
		get
		{
			return this._solverIterations;
		}
		set
		{
			if (this._solverIterations != value)
			{
				this._solverIterations = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x06005085 RID: 20613 RVA: 0x001C9EAB File Offset: 0x001C82AB
	// (set) Token: 0x06005086 RID: 20614 RVA: 0x001C9EB3 File Offset: 0x001C82B3
	public float jointBackForce
	{
		get
		{
			return this._jointBackForce;
		}
		set
		{
			if (this._jointBackForce != value)
			{
				this._jointBackForce = value;
			}
		}
	}

	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x06005087 RID: 20615 RVA: 0x001C9EC8 File Offset: 0x001C82C8
	// (set) Token: 0x06005088 RID: 20616 RVA: 0x001C9ED0 File Offset: 0x001C82D0
	public float jointBackForceThresholdDistance
	{
		get
		{
			return this._jointBackForceThresholdDistance;
		}
		set
		{
			if (this._jointBackForceThresholdDistance != value)
			{
				this._jointBackForceThresholdDistance = value;
			}
		}
	}

	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x06005089 RID: 20617 RVA: 0x001C9EE5 File Offset: 0x001C82E5
	// (set) Token: 0x0600508A RID: 20618 RVA: 0x001C9EED File Offset: 0x001C82ED
	public float jointBackForceMaxForce
	{
		get
		{
			return this._jointBackForceMaxForce;
		}
		set
		{
			if (this._jointBackForceMaxForce != value)
			{
				this._jointBackForceMaxForce = value;
			}
		}
	}

	// Token: 0x0600508B RID: 20619 RVA: 0x001C9F04 File Offset: 0x001C8304
	public int AddSet()
	{
		DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = new DAZPhysicsMeshSoftVerticesSet();
		dazphysicsMeshSoftVerticesSet.autoInfluenceAnchor = this._autoInfluenceAnchor;
		this._softVerticesSets.Add(dazphysicsMeshSoftVerticesSet);
		return this._softVerticesSets.Count - 1;
	}

	// Token: 0x0600508C RID: 20620 RVA: 0x001C9F3C File Offset: 0x001C833C
	public void RemoveSet(int index)
	{
		DAZPhysicsMeshSoftVerticesSet ss = this._softVerticesSets[index];
		this.ClearLinks(ss);
		this._softVerticesSets.RemoveAt(index);
		if (this._currentSetIndex >= this._softVerticesSets.Count)
		{
			this._currentSetIndex--;
		}
	}

	// Token: 0x0600508D RID: 20621 RVA: 0x001C9F90 File Offset: 0x001C8390
	public void ClearLinks(DAZPhysicsMeshSoftVerticesSet ss)
	{
		bool flag = false;
		foreach (DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet in this.softVerticesSets)
		{
			if (ss == dazphysicsMeshSoftVerticesSet)
			{
				flag = true;
			}
		}
		if (flag)
		{
			ss.links.Clear();
		}
	}

	// Token: 0x0600508E RID: 20622 RVA: 0x001CA004 File Offset: 0x001C8404
	public void MoveSet(int fromindex, int toindex)
	{
		if (toindex >= 0 && toindex < this._softVerticesSets.Count)
		{
			DAZPhysicsMeshSoftVerticesSet item = this._softVerticesSets[fromindex];
			this._softVerticesSets.RemoveAt(fromindex);
			this._softVerticesSets.Insert(toindex, item);
			if (this._currentSetIndex == fromindex)
			{
				this._currentSetIndex = toindex;
			}
		}
	}

	// Token: 0x0600508F RID: 20623 RVA: 0x001CA064 File Offset: 0x001C8464
	public DAZPhysicsMeshSoftVerticesSet GetSetByID(string uid, bool skipCheckParent = false)
	{
		for (int i = 0; i < this._softVerticesSets.Count; i++)
		{
			if (this._softVerticesSets[i].uid == uid)
			{
				return this._softVerticesSets[i];
			}
		}
		if (this.parent != null && !skipCheckParent)
		{
			return this.parent.GetSoftSetByID(uid);
		}
		return null;
	}

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x06005090 RID: 20624 RVA: 0x001CA0DB File Offset: 0x001C84DB
	// (set) Token: 0x06005091 RID: 20625 RVA: 0x001CA0E3 File Offset: 0x001C84E3
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
			}
		}
	}

	// Token: 0x06005092 RID: 20626 RVA: 0x001CA100 File Offset: 0x001C8500
	public void Init(Transform transform, DAZSkinV2 sk)
	{
		if (this.enabled && !this.wasInit && sk != null)
		{
			this.skin = sk;
			this.CreateJoints(transform);
			this.InitWeights();
			this.SyncOn();
			this.ResetJoints();
			this.wasInit = true;
		}
	}

	// Token: 0x06005093 RID: 20627 RVA: 0x001CA156 File Offset: 0x001C8556
	public void InitPass2()
	{
		if (this.enabled && !this.wasInit2)
		{
			this.CreateLinkJoints();
			this.wasInit2 = true;
		}
	}

	// Token: 0x06005094 RID: 20628 RVA: 0x001CA17C File Offset: 0x001C857C
	private void InitWeights()
	{
		if (this._influenceType != DAZPhysicsMeshSoftVerticesGroup.InfluenceType.HardCopy)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				if (dazphysicsMeshSoftVerticesSet.influenceVertices.Length > 0)
				{
					dazphysicsMeshSoftVerticesSet.influenceVerticesDistances = new float[dazphysicsMeshSoftVerticesSet.influenceVertices.Length];
					dazphysicsMeshSoftVerticesSet.influenceVerticesWeights = new float[dazphysicsMeshSoftVerticesSet.influenceVertices.Length];
					Vector3 vector;
					if (this.embedJoints)
					{
						if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
						{
							vector = dazphysicsMeshSoftVerticesSet.jointTransform.forward;
						}
						else
						{
							vector = dazphysicsMeshSoftVerticesSet.jointTransform.up;
						}
					}
					else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
					{
						vector = dazphysicsMeshSoftVerticesSet.kinematicTransform.forward;
					}
					else
					{
						vector = dazphysicsMeshSoftVerticesSet.kinematicTransform.up;
					}
					for (int j = 0; j < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; j++)
					{
						Vector3 rhs = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.influenceVertices[j]] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex];
						if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.Distance)
						{
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[j] = rhs.magnitude;
						}
						else if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.DistanceAlongMoveVector)
						{
							Vector3 vector2 = vector * Vector3.Dot(vector, rhs);
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[j] = vector2.magnitude;
						}
					}
				}
			}
		}
	}

	// Token: 0x06005095 RID: 20629 RVA: 0x001CA2F4 File Offset: 0x001C86F4
	public void SyncJointParams()
	{
		if (this.wasInit)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				this.SetMass(dazphysicsMeshSoftVerticesSet);
				this.SetSolverIterations(dazphysicsMeshSoftVerticesSet);
				this.SetInterpolation(dazphysicsMeshSoftVerticesSet);
				this.SetJointLimits(dazphysicsMeshSoftVerticesSet.joint, dazphysicsMeshSoftVerticesSet, false);
				this.SetJointDrive(dazphysicsMeshSoftVerticesSet.joint, dazphysicsMeshSoftVerticesSet);
				this.SetColliders(dazphysicsMeshSoftVerticesSet);
			}
		}
	}

	// Token: 0x06005096 RID: 20630 RVA: 0x001CA36C File Offset: 0x001C876C
	private void SyncLinkParams()
	{
		if (this.wasInit && this.useLinkJoints)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				for (int j = 0; j < dazphysicsMeshSoftVerticesSet.linkJoints.Length; j++)
				{
					SpringJoint springJoint = dazphysicsMeshSoftVerticesSet.linkJoints[j];
					if (springJoint != null)
					{
						springJoint.spring = this._linkSpring;
						springJoint.damper = this._linkDamper;
					}
				}
			}
		}
	}

	// Token: 0x06005097 RID: 20631 RVA: 0x001CA400 File Offset: 0x001C8800
	private void CreateLinkJoints()
	{
		if (this.useLinkJoints)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				GameObject gameObject = dazphysicsMeshSoftVerticesSet.jointTransform.gameObject;
				if (dazphysicsMeshSoftVerticesSet.links != null)
				{
					dazphysicsMeshSoftVerticesSet.linkJoints = new SpringJoint[this._softVerticesSets[i].links.Count];
					dazphysicsMeshSoftVerticesSet.linkJointDistances = new float[this._softVerticesSets[i].links.Count];
					for (int j = 0; j < dazphysicsMeshSoftVerticesSet.links.Count; j++)
					{
						DAZPhysicsMeshSoftVerticesSet setByID = this.GetSetByID(dazphysicsMeshSoftVerticesSet.links[j], false);
						if (setByID != null && setByID.jointRB != null)
						{
							SpringJoint springJoint = gameObject.AddComponent<SpringJoint>();
							dazphysicsMeshSoftVerticesSet.linkJoints[j] = springJoint;
							springJoint.spring = this._linkSpring;
							springJoint.damper = this._linkDamper;
							float magnitude = (dazphysicsMeshSoftVerticesSet.initialTargetPosition - setByID.initialTargetPosition).magnitude;
							dazphysicsMeshSoftVerticesSet.linkJointDistances[j] = magnitude;
							springJoint.minDistance = magnitude;
							springJoint.maxDistance = magnitude;
							springJoint.tolerance = 0.001f;
							springJoint.autoConfigureConnectedAnchor = false;
							springJoint.connectedBody = setByID.jointRB;
							springJoint.connectedAnchor = Vector3.zero;
						}
						else
						{
							dazphysicsMeshSoftVerticesSet.linkJoints[j] = null;
						}
					}
				}
			}
		}
	}

	// Token: 0x06005098 RID: 20632 RVA: 0x001CA588 File Offset: 0x001C8988
	private void GetCollidersRecursive(Transform rootTransform, Transform t, List<Collider> colliders)
	{
		if (t != rootTransform && t.GetComponent<Rigidbody>())
		{
			return;
		}
		foreach (Collider collider in t.GetComponents<Collider>())
		{
			if (collider != null)
			{
				colliders.Add(collider);
			}
		}
		IEnumerator enumerator = t.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform t2 = (Transform)obj;
				this.GetCollidersRecursive(rootTransform, t2, colliders);
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

	// Token: 0x06005099 RID: 20633 RVA: 0x001CA640 File Offset: 0x001C8A40
	public void InitColliders()
	{
		if (this.ignoreCollidersList == null)
		{
			this.ignoreCollidersList = new List<Collider>();
		}
		else
		{
			this.ignoreCollidersList.Clear();
		}
		if (this.allPossibleIgnoreCollidersList == null)
		{
			this.allPossibleIgnoreCollidersList = new List<Collider>();
			foreach (Transform transform in this._ignoreColliders)
			{
				this.GetCollidersRecursive(transform, transform, this.allPossibleIgnoreCollidersList);
			}
		}
		foreach (Collider collider in this.allPossibleIgnoreCollidersList)
		{
			if (collider != null && collider.gameObject.activeInHierarchy && collider.enabled)
			{
				this.ignoreCollidersList.Add(collider);
			}
		}
		for (int j = 0; j < this._softVerticesSets.Count; j++)
		{
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[j];
			if (dazphysicsMeshSoftVerticesSet.jointCollider != null)
			{
				foreach (Collider collider2 in this.ignoreCollidersList)
				{
					Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet.jointCollider, collider2);
					if (dazphysicsMeshSoftVerticesSet.jointCollider2 != null)
					{
						Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet.jointCollider2, collider2);
					}
				}
			}
			if (dazphysicsMeshSoftVerticesSet.jointCollider != null && dazphysicsMeshSoftVerticesSet.jointCollider2 != null)
			{
				Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet.jointCollider, dazphysicsMeshSoftVerticesSet.jointCollider2);
			}
		}
		foreach (AutoColliderGroup autoColliderGroup in this.ignoreAutoColliderGroups)
		{
			if (autoColliderGroup != null)
			{
				AutoCollider[] autoColliders = autoColliderGroup.GetAutoColliders();
				foreach (AutoCollider autoCollider in autoColliders)
				{
					if (autoCollider.jointCollider != null)
					{
						for (int m = 0; m < this._softVerticesSets.Count; m++)
						{
							DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet2 = this._softVerticesSets[m];
							if (dazphysicsMeshSoftVerticesSet2.jointCollider != null)
							{
								Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet2.jointCollider, autoCollider.jointCollider);
								if (dazphysicsMeshSoftVerticesSet2.jointCollider2 != null)
								{
									Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet2.jointCollider2, autoCollider.jointCollider);
								}
							}
						}
					}
				}
			}
		}
		for (int n = 0; n < this._softVerticesSets.Count - 1; n++)
		{
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet3 = this._softVerticesSets[n];
			for (int num = n + 1; num < this._softVerticesSets.Count; num++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet4 = this._softVerticesSets[num];
				if (dazphysicsMeshSoftVerticesSet3.jointCollider != null && dazphysicsMeshSoftVerticesSet4.jointCollider != null)
				{
					Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet3.jointCollider, dazphysicsMeshSoftVerticesSet4.jointCollider);
					if (dazphysicsMeshSoftVerticesSet3.jointCollider2 != null)
					{
						Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet3.jointCollider2, dazphysicsMeshSoftVerticesSet4.jointCollider);
						if (dazphysicsMeshSoftVerticesSet4.jointCollider2 != null)
						{
							Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet3.jointCollider2, dazphysicsMeshSoftVerticesSet4.jointCollider2);
						}
					}
					if (dazphysicsMeshSoftVerticesSet4.jointCollider2 != null)
					{
						Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet3.jointCollider, dazphysicsMeshSoftVerticesSet4.jointCollider2);
					}
				}
			}
		}
	}

	// Token: 0x0600509A RID: 20634 RVA: 0x001CAA14 File Offset: 0x001C8E14
	public void IgnoreOtherSoftGroupColliders(DAZPhysicsMeshSoftVerticesGroup otherGroup, bool ignore = true)
	{
		List<DAZPhysicsMeshSoftVerticesSet> softVerticesSets = otherGroup.softVerticesSets;
		for (int i = 0; i < softVerticesSets.Count; i++)
		{
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = softVerticesSets[i];
			for (int j = 0; j < this._softVerticesSets.Count; j++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet2 = this._softVerticesSets[j];
				if (dazphysicsMeshSoftVerticesSet.jointCollider != null && dazphysicsMeshSoftVerticesSet2.jointCollider != null)
				{
					Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet.jointCollider, dazphysicsMeshSoftVerticesSet2.jointCollider, ignore);
					if (dazphysicsMeshSoftVerticesSet.jointCollider2 != null)
					{
						Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet.jointCollider2, dazphysicsMeshSoftVerticesSet2.jointCollider, ignore);
						if (dazphysicsMeshSoftVerticesSet2.jointCollider2 != null)
						{
							Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet.jointCollider2, dazphysicsMeshSoftVerticesSet2.jointCollider2, ignore);
						}
					}
					if (dazphysicsMeshSoftVerticesSet2.jointCollider2 != null)
					{
						Physics.IgnoreCollision(dazphysicsMeshSoftVerticesSet.jointCollider, dazphysicsMeshSoftVerticesSet2.jointCollider2, ignore);
					}
				}
			}
		}
	}

	// Token: 0x0600509B RID: 20635 RVA: 0x001CAB15 File Offset: 0x001C8F15
	private void SetInterpolation(DAZPhysicsMeshSoftVerticesSet ss)
	{
		if (this._useInterpolation && this._useSimulation && !this.useCustomInterpolation)
		{
			ss.jointRB.interpolation = RigidbodyInterpolation.Interpolate;
		}
		else
		{
			ss.jointRB.interpolation = RigidbodyInterpolation.None;
		}
	}

	// Token: 0x0600509C RID: 20636 RVA: 0x001CAB58 File Offset: 0x001C8F58
	private void SetJointLimits(ConfigurableJoint joint, DAZPhysicsMeshSoftVerticesSet ss, bool forceUniform = false)
	{
		if (this._useUniformLimit || forceUniform)
		{
			if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
			{
				if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.yMotion = ConfigurableJointMotion.Locked;
				}
				else if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Free)
				{
					joint.yMotion = ConfigurableJointMotion.Free;
				}
				else
				{
					joint.yMotion = ConfigurableJointMotion.Limited;
				}
				if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.zMotion = ConfigurableJointMotion.Locked;
				}
				else if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Free)
				{
					joint.zMotion = ConfigurableJointMotion.Free;
				}
				else
				{
					joint.zMotion = ConfigurableJointMotion.Limited;
				}
			}
			else
			{
				if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.zMotion = ConfigurableJointMotion.Locked;
				}
				else if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Free)
				{
					joint.zMotion = ConfigurableJointMotion.Free;
				}
				else
				{
					joint.zMotion = ConfigurableJointMotion.Limited;
				}
				if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.yMotion = ConfigurableJointMotion.Locked;
				}
				else if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Free)
				{
					joint.yMotion = ConfigurableJointMotion.Free;
				}
				else
				{
					joint.yMotion = ConfigurableJointMotion.Limited;
				}
			}
			if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
			{
				joint.xMotion = ConfigurableJointMotion.Locked;
			}
			else if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Free)
			{
				joint.xMotion = ConfigurableJointMotion.Free;
			}
			else
			{
				joint.xMotion = ConfigurableJointMotion.Limited;
			}
			SoftJointLimit linearLimit = joint.linearLimit;
			linearLimit.limit = this._normalDistanceLimit * ss.limitMultiplier * this.scale;
			joint.linearLimit = linearLimit;
		}
		else
		{
			if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
			{
				if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.yMotion = ConfigurableJointMotion.Locked;
				}
				else
				{
					joint.yMotion = ConfigurableJointMotion.Free;
				}
				if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.zMotion = ConfigurableJointMotion.Locked;
				}
				else
				{
					joint.zMotion = ConfigurableJointMotion.Free;
				}
			}
			else
			{
				if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.zMotion = ConfigurableJointMotion.Locked;
				}
				else
				{
					joint.zMotion = ConfigurableJointMotion.Free;
				}
				if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
				{
					joint.yMotion = ConfigurableJointMotion.Locked;
				}
				else
				{
					joint.yMotion = ConfigurableJointMotion.Free;
				}
			}
			if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Lock)
			{
				joint.xMotion = ConfigurableJointMotion.Locked;
			}
			else
			{
				joint.xMotion = ConfigurableJointMotion.Free;
			}
		}
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
	}

	// Token: 0x0600509D RID: 20637 RVA: 0x001CAD84 File Offset: 0x001C9184
	private void SetJointDrive(ConfigurableJoint joint, DAZPhysicsMeshSoftVerticesSet ss)
	{
		JointDrive zDrive = default(JointDrive);
		zDrive.positionSpring = this._jointSpringNormal * ss.springMultiplier;
		zDrive.positionDamper = this._jointDamperNormal;
		zDrive.maximumForce = this._jointSpringMaxForce;
		JointDrive xDrive = default(JointDrive);
		xDrive.positionSpring = this._jointSpringTangent * ss.springMultiplier;
		xDrive.positionDamper = this._jointDamperTangent;
		xDrive.maximumForce = this._jointSpringMaxForce;
		JointDrive yDrive = default(JointDrive);
		yDrive.positionSpring = this._jointSpringTangent2 * ss.springMultiplier;
		yDrive.positionDamper = this._jointDamperTangent2;
		yDrive.maximumForce = this._jointSpringMaxForce;
		joint.xDrive = xDrive;
		joint.yDrive = yDrive;
		joint.zDrive = zDrive;
	}

	// Token: 0x0600509E RID: 20638 RVA: 0x001CAE48 File Offset: 0x001C9248
	private void SetCollidersThreaded(DAZPhysicsMeshSoftVerticesSet ss)
	{
		int direction = 0;
		Vector3 a;
		if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
		{
			DAZPhysicsMeshSoftVerticesGroup.ColliderOrient colliderOrient = this._colliderOrient;
			if (colliderOrient != DAZPhysicsMeshSoftVerticesGroup.ColliderOrient.Normal)
			{
				if (colliderOrient != DAZPhysicsMeshSoftVerticesGroup.ColliderOrient.Tangent)
				{
					if (colliderOrient == DAZPhysicsMeshSoftVerticesGroup.ColliderOrient.Tangent2)
					{
						direction = 0;
					}
				}
				else
				{
					direction = 1;
				}
			}
			else
			{
				direction = 2;
			}
			a.x = this._colliderTangent2Offset;
			a.y = this._colliderTangentOffset;
			a.z = this._colliderNormalOffset + this._colliderAdditionalNormalOffset;
		}
		else
		{
			direction = (int)this._colliderOrient;
			a.x = this._colliderTangent2Offset;
			a.z = this._colliderTangentOffset;
			a.y = this._colliderNormalOffset + this._colliderAdditionalNormalOffset;
		}
		DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType = this.colliderType;
		if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Capsule)
		{
			if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Sphere)
			{
				if (colliderType == DAZPhysicsMeshSoftVerticesGroup.ColliderType.Box)
				{
					ss.threadedColliderRadius = this._colliderRadius * 2f * ss.sizeMultiplier;
					ss.threadedColliderCenter = a * ss.sizeMultiplier;
				}
			}
			else
			{
				ss.threadedColliderRadius = this._colliderRadius * ss.sizeMultiplier;
				ss.threadedColliderCenter = a * ss.sizeMultiplier;
			}
		}
		else
		{
			ss.threadedColliderRadius = this._colliderRadius * ss.sizeMultiplier;
			ss.threadedColliderHeight = this._colliderLength * ss.sizeMultiplier;
			ss.threadedColliderCenter = a * ss.sizeMultiplier;
			if (ss.capsuleLineSphereCollider != null)
			{
				ss.capsuleLineSphereCollider.UpdateData(ss.threadedColliderRadius, ss.threadedColliderHeight, direction, ss.threadedColliderCenter);
			}
		}
		if (this._useSecondCollider)
		{
			Vector3 a2;
			if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
			{
				a2.x = this._secondColliderTangent2Offset;
				a2.y = this._secondColliderTangentOffset;
				a2.z = this._secondColliderNormalOffset + this._secondColliderAdditionalNormalOffset;
			}
			else
			{
				a2.x = this._secondColliderTangent2Offset;
				a2.z = this._secondColliderTangentOffset;
				a2.y = this._secondColliderNormalOffset + this._secondColliderAdditionalNormalOffset;
			}
			DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType2 = this.colliderType;
			if (colliderType2 != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Capsule)
			{
				if (colliderType2 != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Sphere)
				{
					if (colliderType2 == DAZPhysicsMeshSoftVerticesGroup.ColliderType.Box)
					{
						ss.threadedCollider2Radius = this._secondColliderRadius * 2f * ss.sizeMultiplier;
						ss.threadedCollider2Center = a2 * ss.sizeMultiplier;
					}
				}
				else
				{
					ss.threadedCollider2Radius = this._secondColliderRadius * ss.sizeMultiplier;
					ss.threadedCollider2Center = a2 * ss.sizeMultiplier;
				}
			}
			else
			{
				ss.threadedCollider2Radius = this._secondColliderRadius * ss.sizeMultiplier;
				ss.threadedCollider2Height = this._secondColliderLength * ss.sizeMultiplier;
				ss.threadedCollider2Center = a2 * ss.sizeMultiplier;
				if (ss.capsuleLineSphereCollider2 != null)
				{
					ss.capsuleLineSphereCollider2.UpdateData(ss.threadedCollider2Radius, ss.threadedCollider2Height, direction, ss.threadedCollider2Center);
				}
			}
		}
	}

	// Token: 0x0600509F RID: 20639 RVA: 0x001CB14C File Offset: 0x001C954C
	private void SetCollidersThreadedFinish(DAZPhysicsMeshSoftVerticesSet ss)
	{
		DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType = this.colliderType;
		if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Capsule)
		{
			if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Sphere)
			{
				if (colliderType == DAZPhysicsMeshSoftVerticesGroup.ColliderType.Box)
				{
					BoxCollider boxCollider = ss.jointCollider as BoxCollider;
					boxCollider.size = new Vector3(ss.threadedColliderRadius, ss.threadedColliderRadius, ss.threadedColliderRadius);
					boxCollider.center = ss.threadedColliderCenter;
				}
			}
			else
			{
				SphereCollider sphereCollider = ss.jointCollider as SphereCollider;
				sphereCollider.radius = ss.threadedColliderRadius;
				sphereCollider.center = ss.threadedColliderCenter;
			}
		}
		else
		{
			CapsuleCollider capsuleCollider = ss.jointCollider as CapsuleCollider;
			capsuleCollider.radius = ss.threadedColliderRadius;
			capsuleCollider.height = ss.threadedColliderHeight;
			capsuleCollider.center = ss.threadedColliderCenter;
		}
		if (this._useSecondCollider && ss.jointCollider2 != null)
		{
			DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType2 = this.colliderType;
			if (colliderType2 != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Capsule)
			{
				if (colliderType2 != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Sphere)
				{
					if (colliderType2 == DAZPhysicsMeshSoftVerticesGroup.ColliderType.Box)
					{
						BoxCollider boxCollider2 = ss.jointCollider2 as BoxCollider;
						boxCollider2.size = new Vector3(ss.threadedCollider2Radius, ss.threadedCollider2Radius, ss.threadedCollider2Radius);
						boxCollider2.center = ss.threadedCollider2Center;
					}
				}
				else
				{
					SphereCollider sphereCollider2 = ss.jointCollider2 as SphereCollider;
					sphereCollider2.radius = ss.threadedCollider2Radius;
					sphereCollider2.center = ss.threadedCollider2Center;
				}
			}
			else
			{
				CapsuleCollider capsuleCollider2 = ss.jointCollider2 as CapsuleCollider;
				capsuleCollider2.radius = ss.threadedCollider2Radius;
				capsuleCollider2.height = ss.threadedCollider2Height;
				capsuleCollider2.center = ss.threadedCollider2Center;
			}
		}
	}

	// Token: 0x060050A0 RID: 20640 RVA: 0x001CB2F4 File Offset: 0x001C96F4
	private void SetColliders(DAZPhysicsMeshSoftVerticesSet ss)
	{
		int direction = 0;
		Vector3 a;
		if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
		{
			DAZPhysicsMeshSoftVerticesGroup.ColliderOrient colliderOrient = this._colliderOrient;
			if (colliderOrient != DAZPhysicsMeshSoftVerticesGroup.ColliderOrient.Normal)
			{
				if (colliderOrient != DAZPhysicsMeshSoftVerticesGroup.ColliderOrient.Tangent)
				{
					if (colliderOrient == DAZPhysicsMeshSoftVerticesGroup.ColliderOrient.Tangent2)
					{
						direction = 0;
					}
				}
				else
				{
					direction = 1;
				}
			}
			else
			{
				direction = 2;
			}
			a.x = this._colliderTangent2Offset;
			a.y = this._colliderTangentOffset;
			a.z = this._colliderNormalOffset + this._colliderAdditionalNormalOffset;
		}
		else
		{
			direction = (int)this._colliderOrient;
			a.x = this._colliderTangent2Offset;
			a.z = this._colliderTangentOffset;
			a.y = this._colliderNormalOffset + this._colliderAdditionalNormalOffset;
		}
		DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType = this.colliderType;
		if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Capsule)
		{
			if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Sphere)
			{
				if (colliderType == DAZPhysicsMeshSoftVerticesGroup.ColliderType.Box)
				{
					BoxCollider boxCollider = ss.jointCollider as BoxCollider;
					float num = this._colliderRadius * 2f * ss.sizeMultiplier;
					boxCollider.size = new Vector3(num, num, num);
					boxCollider.center = a * ss.sizeMultiplier;
				}
			}
			else
			{
				SphereCollider sphereCollider = ss.jointCollider as SphereCollider;
				sphereCollider.radius = this._colliderRadius * ss.sizeMultiplier;
				sphereCollider.center = a * ss.sizeMultiplier;
			}
		}
		else
		{
			CapsuleCollider capsuleCollider = ss.jointCollider as CapsuleCollider;
			capsuleCollider.radius = this._colliderRadius * ss.sizeMultiplier;
			capsuleCollider.height = this._colliderLength * ss.sizeMultiplier;
			capsuleCollider.direction = direction;
			capsuleCollider.center = a * ss.sizeMultiplier;
			if (ss.capsuleLineSphereCollider != null)
			{
				ss.capsuleLineSphereCollider.UpdateData();
			}
		}
		if (this._colliderMaterial != null)
		{
			ss.jointCollider.sharedMaterial = this._colliderMaterial;
		}
		if (this._useSecondCollider && ss.jointCollider2 != null)
		{
			Vector3 a2;
			if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
			{
				a2.x = this._secondColliderTangent2Offset;
				a2.y = this._secondColliderTangentOffset;
				a2.z = this._secondColliderNormalOffset + this._secondColliderAdditionalNormalOffset;
			}
			else
			{
				a2.x = this._secondColliderTangent2Offset;
				a2.z = this._secondColliderTangentOffset;
				a2.y = this._secondColliderNormalOffset + this._secondColliderAdditionalNormalOffset;
			}
			DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType2 = this.colliderType;
			if (colliderType2 != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Capsule)
			{
				if (colliderType2 != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Sphere)
				{
					if (colliderType2 == DAZPhysicsMeshSoftVerticesGroup.ColliderType.Box)
					{
						BoxCollider boxCollider2 = ss.jointCollider2 as BoxCollider;
						float num2 = this._secondColliderRadius * 2f * ss.sizeMultiplier;
						boxCollider2.size = new Vector3(num2, num2, num2);
						boxCollider2.center = a2 * ss.sizeMultiplier;
					}
				}
				else
				{
					SphereCollider sphereCollider2 = ss.jointCollider2 as SphereCollider;
					sphereCollider2.radius = this._secondColliderRadius * ss.sizeMultiplier;
					sphereCollider2.center = a2 * ss.sizeMultiplier;
				}
			}
			else
			{
				CapsuleCollider capsuleCollider2 = ss.jointCollider2 as CapsuleCollider;
				capsuleCollider2.radius = this._secondColliderRadius * ss.sizeMultiplier;
				capsuleCollider2.height = this._secondColliderLength * ss.sizeMultiplier;
				capsuleCollider2.direction = direction;
				capsuleCollider2.center = a2 * ss.sizeMultiplier;
				if (ss.capsuleLineSphereCollider2 != null)
				{
					ss.capsuleLineSphereCollider2.UpdateData();
				}
			}
			ss.jointCollider2.sharedMaterial = this._colliderMaterial;
		}
	}

	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x060050A1 RID: 20641 RVA: 0x001CB699 File Offset: 0x001C9A99
	// (set) Token: 0x060050A2 RID: 20642 RVA: 0x001CB6A4 File Offset: 0x001C9AA4
	public bool multiplyMassByLimitMultiplier
	{
		get
		{
			return this._multiplyMassByLimitMultiplier;
		}
		set
		{
			this._multiplyMassByLimitMultiplier = value;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet mass = this._softVerticesSets[i];
				this.SetMass(mass);
			}
		}
	}

	// Token: 0x060050A3 RID: 20643 RVA: 0x001CB6E8 File Offset: 0x001C9AE8
	private void SetMass(DAZPhysicsMeshSoftVerticesSet ss)
	{
		if (ss.jointRB != null)
		{
			if (this.multiplyMassByLimitMultiplier)
			{
				ss.jointRB.mass = this._jointMass * ss.limitMultiplier;
			}
			else
			{
				ss.jointRB.mass = this._jointMass;
			}
		}
	}

	// Token: 0x060050A4 RID: 20644 RVA: 0x001CB73F File Offset: 0x001C9B3F
	private void SetSolverIterations(DAZPhysicsMeshSoftVerticesSet ss)
	{
		ss.jointRB.solverIterations = this._solverIterations;
	}

	// Token: 0x060050A5 RID: 20645 RVA: 0x001CB754 File Offset: 0x001C9B54
	private void CreateJoints(Transform transform)
	{
		if (this._normalReference != null)
		{
			this._startingNormalReferencePosition = this._normalReference.position;
			Transform transform2;
			if (this.name != null && this.name != string.Empty)
			{
				GameObject gameObject = new GameObject("PhysicsMesh" + this.name);
				transform2 = gameObject.transform;
				transform2.SetParent(transform);
				transform2.localScale = Vector3.one;
				transform2.position = transform.position;
				transform2.rotation = transform.rotation;
			}
			else
			{
				transform2 = transform;
			}
			Vector3 a;
			if (this._normalReference != null)
			{
				DAZBone component = this._normalReference.GetComponent<DAZBone>();
				if (component != null)
				{
					a = component.importWorldPosition;
				}
				else
				{
					a = transform2.position;
				}
			}
			else
			{
				a = Vector3.zero;
			}
			if (this.embedJoints)
			{
				GameObject gameObject2 = new GameObject("PhysicsMeshKRB" + this.name);
				this.embedRB = gameObject2.AddComponent<Rigidbody>();
				this.embedRB.isKinematic = true;
				this.embedTransform = this.embedRB.transform;
				this.embedTransform.SetParent(transform2);
				this.embedTransform.localScale = Vector3.one;
				this.embedTransform.position = this._normalReference.position;
				this.embedTransform.rotation = this._normalReference.rotation;
			}
			else
			{
				this.embedRB = null;
				this.embedTransform = null;
			}
			bool flag = this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				Vector3[] visibleMorphedUVVertices = this._skin.dazMesh.visibleMorphedUVVertices;
				Vector3[] morphedUVNormals = this._skin.dazMesh.morphedUVNormals;
				Vector3 vector;
				if (this.centerBetweenTargetAndAnchor)
				{
					vector = (visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex] + visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
				}
				else
				{
					vector = visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex];
				}
				dazphysicsMeshSoftVerticesSet.initialTargetPosition = vector;
				if (!this.embedJoints)
				{
					GameObject gameObject3 = new GameObject("PhysicsMeshKRB" + this.name + i);
					dazphysicsMeshSoftVerticesSet.kinematicTransform = gameObject3.transform;
					dazphysicsMeshSoftVerticesSet.kinematicTransform.SetParent(transform2);
					dazphysicsMeshSoftVerticesSet.kinematicTransform.localScale = Vector3.one;
					dazphysicsMeshSoftVerticesSet.kinematicRB = gameObject3.AddComponent<Rigidbody>();
					dazphysicsMeshSoftVerticesSet.kinematicRB.isKinematic = true;
					dazphysicsMeshSoftVerticesSet.kinematicTransform.position = vector;
				}
				this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
				this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] = true;
				if (flag)
				{
					this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
					if (this.centerBetweenTargetAndAnchor)
					{
						this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] = true;
					}
				}
				this._skin.postSkinVertsChanged = true;
				Quaternion rotation;
				if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.NormalReference || dazphysicsMeshSoftVerticesSet.forceLookAtReference)
				{
					rotation = Quaternion.LookRotation(a - vector, visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.anchorVertex] - visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex]);
				}
				else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal)
				{
					if (this.centerBetweenTargetAndAnchor)
					{
						rotation = Quaternion.LookRotation((-morphedUVNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - morphedUVNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f);
					}
					else
					{
						rotation = Quaternion.LookRotation(-morphedUVNormals[dazphysicsMeshSoftVerticesSet.targetVertex]);
					}
				}
				else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp)
				{
					if (this.centerBetweenTargetAndAnchor)
					{
						rotation = Quaternion.LookRotation((-morphedUVNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - morphedUVNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, a - vector);
					}
					else
					{
						rotation = Quaternion.LookRotation(-morphedUVNormals[dazphysicsMeshSoftVerticesSet.targetVertex], a - vector);
					}
				}
				else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp)
				{
					if (this.centerBetweenTargetAndAnchor)
					{
						rotation = Quaternion.LookRotation((-morphedUVNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - morphedUVNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.anchorVertex] - visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex]);
					}
					else
					{
						rotation = Quaternion.LookRotation(-morphedUVNormals[dazphysicsMeshSoftVerticesSet.targetVertex], visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.anchorVertex] - visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex]);
					}
				}
				else
				{
					rotation = Quaternion.LookRotation(visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.anchorVertex] - visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex], a - vector);
				}
				if (!this.embedJoints)
				{
					dazphysicsMeshSoftVerticesSet.kinematicTransform.rotation = rotation;
				}
				if (this._useSimulation && !this._useUniformLimit && !this.embedJoints)
				{
					GameObject gameObject4 = new GameObject("JointTracker");
					dazphysicsMeshSoftVerticesSet.jointTrackerTransform = gameObject4.transform;
					dazphysicsMeshSoftVerticesSet.jointTrackerTransform.SetParent(dazphysicsMeshSoftVerticesSet.kinematicTransform);
					dazphysicsMeshSoftVerticesSet.jointTrackerTransform.localScale = Vector3.one;
					dazphysicsMeshSoftVerticesSet.jointTrackerTransform.localPosition = Vector3.zero;
					dazphysicsMeshSoftVerticesSet.jointTrackerTransform.localRotation = Quaternion.identity;
				}
				GameObject gameObject5 = new GameObject("PhysicsMeshJoint" + this.name + i);
				dazphysicsMeshSoftVerticesSet.jointTransform = gameObject5.transform;
				if (this._useSimulation)
				{
					if (this.embedJoints)
					{
						dazphysicsMeshSoftVerticesSet.jointTransform.SetParent(transform2);
						dazphysicsMeshSoftVerticesSet.jointTransform.localScale = Vector3.one;
					}
					else
					{
						dazphysicsMeshSoftVerticesSet.jointTransform.SetParent(transform2);
						dazphysicsMeshSoftVerticesSet.jointTransform.localScale = Vector3.one;
					}
				}
				else if (this.embedJoints)
				{
					dazphysicsMeshSoftVerticesSet.jointTransform.SetParent(this.embedTransform);
					dazphysicsMeshSoftVerticesSet.jointTransform.localScale = Vector3.one;
				}
				else
				{
					dazphysicsMeshSoftVerticesSet.jointTransform.SetParent(dazphysicsMeshSoftVerticesSet.kinematicTransform);
					dazphysicsMeshSoftVerticesSet.jointTransform.localScale = Vector3.one;
				}
				dazphysicsMeshSoftVerticesSet.jointTransform.position = vector;
				dazphysicsMeshSoftVerticesSet.jointTransform.rotation = rotation;
				dazphysicsMeshSoftVerticesSet.jointRB = gameObject5.AddComponent<Rigidbody>();
				dazphysicsMeshSoftVerticesSet.jointRB.useGravity = false;
				dazphysicsMeshSoftVerticesSet.jointRB.drag = 0f;
				dazphysicsMeshSoftVerticesSet.jointRB.angularDrag = 0f;
				dazphysicsMeshSoftVerticesSet.jointRB.maxAngularVelocity = SuperController.singleton.maxAngularVelocity;
				dazphysicsMeshSoftVerticesSet.jointRB.maxDepenetrationVelocity = SuperController.singleton.maxDepenetrationVelocity;
				dazphysicsMeshSoftVerticesSet.jointRB.collisionDetectionMode = CollisionDetectionMode.Discrete;
				dazphysicsMeshSoftVerticesSet.jointRB.isKinematic = false;
				dazphysicsMeshSoftVerticesSet.jointRB.detectCollisions = false;
				dazphysicsMeshSoftVerticesSet.joint = gameObject5.AddComponent<ConfigurableJoint>();
				if (this.embedJoints)
				{
					dazphysicsMeshSoftVerticesSet.joint.connectedBody = this.embedRB;
					dazphysicsMeshSoftVerticesSet.joint.autoConfigureConnectedAnchor = false;
					dazphysicsMeshSoftVerticesSet.joint.anchor = Vector3.zero;
					dazphysicsMeshSoftVerticesSet.joint.connectedAnchor = this._normalReference.InverseTransformPoint(vector);
				}
				else
				{
					dazphysicsMeshSoftVerticesSet.joint.connectedBody = dazphysicsMeshSoftVerticesSet.kinematicRB;
					dazphysicsMeshSoftVerticesSet.joint.autoConfigureConnectedAnchor = false;
					dazphysicsMeshSoftVerticesSet.joint.anchor = Vector3.zero;
					dazphysicsMeshSoftVerticesSet.joint.connectedAnchor = Vector3.zero;
				}
				this.SetJointLimits(dazphysicsMeshSoftVerticesSet.joint, dazphysicsMeshSoftVerticesSet, false);
				this.SetJointDrive(dazphysicsMeshSoftVerticesSet.joint, dazphysicsMeshSoftVerticesSet);
				this.SetInterpolation(dazphysicsMeshSoftVerticesSet);
				DAZPhysicsMeshSoftVerticesGroup.ColliderType colliderType = this.colliderType;
				if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Capsule)
				{
					if (colliderType != DAZPhysicsMeshSoftVerticesGroup.ColliderType.Sphere)
					{
						if (colliderType == DAZPhysicsMeshSoftVerticesGroup.ColliderType.Box)
						{
							BoxCollider jointCollider = gameObject5.AddComponent<BoxCollider>();
							if (this._useSecondCollider)
							{
								jointCollider = gameObject5.AddComponent<BoxCollider>();
								dazphysicsMeshSoftVerticesSet.jointCollider2 = jointCollider;
							}
						}
					}
					else
					{
						SphereCollider sphereCollider = gameObject5.AddComponent<SphereCollider>();
						dazphysicsMeshSoftVerticesSet.jointCollider = sphereCollider;
						if (this._addGPUCollider)
						{
							GpuSphereCollider gpuSphereCollider = gameObject5.AddComponent<GpuSphereCollider>();
							gpuSphereCollider.sphereCollider = sphereCollider;
						}
						if (this._useSecondCollider)
						{
							sphereCollider = gameObject5.AddComponent<SphereCollider>();
							dazphysicsMeshSoftVerticesSet.jointCollider2 = sphereCollider;
							if (this._addSecondGPUCollider)
							{
								GpuSphereCollider gpuSphereCollider2 = gameObject5.AddComponent<GpuSphereCollider>();
								gpuSphereCollider2.sphereCollider = sphereCollider;
							}
						}
					}
				}
				else
				{
					CapsuleCollider capsuleCollider = gameObject5.AddComponent<CapsuleCollider>();
					dazphysicsMeshSoftVerticesSet.jointCollider = capsuleCollider;
					if (this._addGPUCollider)
					{
						CapsuleLineSphereCollider capsuleLineSphereCollider = gameObject5.AddComponent<CapsuleLineSphereCollider>();
						dazphysicsMeshSoftVerticesSet.capsuleLineSphereCollider = capsuleLineSphereCollider;
						capsuleLineSphereCollider.capsuleCollider = capsuleCollider;
					}
					if (this._useSecondCollider)
					{
						capsuleCollider = gameObject5.AddComponent<CapsuleCollider>();
						dazphysicsMeshSoftVerticesSet.jointCollider2 = capsuleCollider;
						if (this._addSecondGPUCollider)
						{
							CapsuleLineSphereCollider capsuleLineSphereCollider2 = gameObject5.AddComponent<CapsuleLineSphereCollider>();
							dazphysicsMeshSoftVerticesSet.capsuleLineSphereCollider2 = capsuleLineSphereCollider2;
							capsuleLineSphereCollider2.capsuleCollider = capsuleCollider;
						}
					}
				}
				if (this._colliderLayer != null && this._colliderLayer != string.Empty)
				{
					gameObject5.layer = LayerMask.NameToLayer(this._colliderLayer);
				}
				this.SetColliders(dazphysicsMeshSoftVerticesSet);
				this.SetMass(dazphysicsMeshSoftVerticesSet);
				this.SetSolverIterations(dazphysicsMeshSoftVerticesSet);
				dazphysicsMeshSoftVerticesSet.jointRB.centerOfMass = Vector3.zero;
				if (i == 0 && this.controller != null)
				{
					ConfigurableJoint configurableJoint = gameObject5.AddComponent<ConfigurableJoint>();
					configurableJoint.rotationDriveMode = RotationDriveMode.Slerp;
					configurableJoint.autoConfigureConnectedAnchor = false;
					configurableJoint.connectedAnchor = Vector3.zero;
					Rigidbody component2 = this.controller.GetComponent<Rigidbody>();
					this.controller.transform.position = gameObject5.transform.position;
					this.controller.transform.rotation = gameObject5.transform.rotation;
					if (component2 != null)
					{
						configurableJoint.connectedBody = component2;
					}
					this.controller.followWhenOffRB = dazphysicsMeshSoftVerticesSet.jointRB;
				}
			}
			this.InitColliders();
		}
		else
		{
			Debug.LogError("Can't create joints without up reference set");
		}
	}

	// Token: 0x060050A6 RID: 20646 RVA: 0x001CC270 File Offset: 0x001CA670
	public void AdjustInitialTargetPositionsFast(Vector3[] initVerts)
	{
		if (this.wasInit)
		{
			this.linkTargetsDirty = false;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				Vector3 vector;
				if (this.centerBetweenTargetAndAnchor)
				{
					vector = (initVerts[dazphysicsMeshSoftVerticesSet.targetVertex] + initVerts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
				}
				else
				{
					vector = initVerts[dazphysicsMeshSoftVerticesSet.targetVertex];
				}
				float sqrMagnitude = (vector - dazphysicsMeshSoftVerticesSet.initialTargetPosition).sqrMagnitude;
				if (sqrMagnitude >= 1E-06f)
				{
					dazphysicsMeshSoftVerticesSet.initialTargetPosition = vector;
					this.linkTargetsDirty = true;
					dazphysicsMeshSoftVerticesSet.linkTargetPositionDirty = true;
				}
				else
				{
					dazphysicsMeshSoftVerticesSet.linkTargetPositionDirty = false;
				}
			}
		}
	}

	// Token: 0x060050A7 RID: 20647 RVA: 0x001CC350 File Offset: 0x001CA750
	public void AdjustInitialTargetPositions()
	{
		if (this.wasInit)
		{
			Vector3[] visibleMorphedUVVertices = this._skin.dazMesh.visibleMorphedUVVertices;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				Vector3 initialTargetPosition;
				if (this.centerBetweenTargetAndAnchor)
				{
					initialTargetPosition = (visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex] + visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
				}
				else
				{
					initialTargetPosition = visibleMorphedUVVertices[dazphysicsMeshSoftVerticesSet.targetVertex];
				}
				dazphysicsMeshSoftVerticesSet.initialTargetPosition = initialTargetPosition;
			}
		}
	}

	// Token: 0x060050A8 RID: 20648 RVA: 0x001CC400 File Offset: 0x001CA800
	public void AdjustLinkJointDistancesFast()
	{
		if (this.wasInit && this.useLinkJoints && this.linkTargetsDirty)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				for (int j = 0; j < dazphysicsMeshSoftVerticesSet.links.Count; j++)
				{
					DAZPhysicsMeshSoftVerticesSet setByID = this.GetSetByID(dazphysicsMeshSoftVerticesSet.links[j], false);
					if (setByID != null && (dazphysicsMeshSoftVerticesSet.linkTargetPositionDirty || setByID.linkTargetPositionDirty || this.triggerThreadedScaleChange))
					{
						float num = (dazphysicsMeshSoftVerticesSet.initialTargetPosition - setByID.initialTargetPosition).magnitude * this.scale;
						dazphysicsMeshSoftVerticesSet.linkJointDistances[j] = num;
					}
				}
			}
		}
	}

	// Token: 0x060050A9 RID: 20649 RVA: 0x001CC4DC File Offset: 0x001CA8DC
	public void AdjustLinkJointDistancesFinishFast()
	{
		if (this.wasInit && this.useLinkJoints && this.linkTargetsDirty)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				for (int j = 0; j < dazphysicsMeshSoftVerticesSet.links.Count; j++)
				{
					DAZPhysicsMeshSoftVerticesSet setByID = this.GetSetByID(dazphysicsMeshSoftVerticesSet.links[j], false);
					if (setByID != null && (dazphysicsMeshSoftVerticesSet.linkTargetPositionDirty || setByID.linkTargetPositionDirty || this.triggerThreadedScaleChange))
					{
						SpringJoint springJoint = dazphysicsMeshSoftVerticesSet.linkJoints[j];
						if (springJoint != null)
						{
							float num = dazphysicsMeshSoftVerticesSet.linkJointDistances[j];
							springJoint.minDistance = num;
							springJoint.maxDistance = num;
						}
					}
				}
			}
			this.triggerThreadedScaleChange = false;
		}
	}

	// Token: 0x060050AA RID: 20650 RVA: 0x001CC5C4 File Offset: 0x001CA9C4
	public void AdjustLinkJointDistances(bool force = false)
	{
		if (this.wasInit && this.useLinkJoints)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				for (int j = 0; j < dazphysicsMeshSoftVerticesSet.links.Count; j++)
				{
					DAZPhysicsMeshSoftVerticesSet setByID = this.GetSetByID(dazphysicsMeshSoftVerticesSet.links[j], false);
					if (setByID != null && (dazphysicsMeshSoftVerticesSet.linkTargetPositionDirty || setByID.linkTargetPositionDirty || force))
					{
						float num = (dazphysicsMeshSoftVerticesSet.initialTargetPosition - setByID.initialTargetPosition).magnitude * this.scale;
						dazphysicsMeshSoftVerticesSet.linkJointDistances[j] = num;
						SpringJoint springJoint = dazphysicsMeshSoftVerticesSet.linkJoints[j];
						if (springJoint != null)
						{
							springJoint.minDistance = num;
							springJoint.maxDistance = num;
						}
					}
				}
			}
		}
	}

	// Token: 0x060050AB RID: 20651 RVA: 0x001CC6B8 File Offset: 0x001CAAB8
	public void ResetJoints()
	{
		if (this.wasInit)
		{
			this._appliedBackForce.x = 0f;
			this._appliedBackForce.y = 0f;
			this._appliedBackForce.z = 0f;
			this._bufferedBackForce.x = 0f;
			this._bufferedBackForce.y = 0f;
			this._bufferedBackForce.z = 0f;
			if (this.embedJoints)
			{
				this.embedTransform.position = this._normalReference.position;
				this.embedTransform.rotation = this._normalReference.rotation;
			}
			if (this.predictionTransform != null)
			{
				this.predictionTransformPosition1 = this.predictionTransform.position;
				this.predictionTransformPosition2 = this.predictionTransformPosition1;
				this.predictionTransformPosition3 = this.predictionTransformPosition1;
			}
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				if (this._skin.postSkinVertsReady[dazphysicsMeshSoftVerticesSet.targetVertex])
				{
					Vector3 vector;
					if (this.centerBetweenTargetAndAnchor)
					{
						vector = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex] + this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
					}
					else
					{
						vector = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex];
					}
					if (this.embedJoints)
					{
						dazphysicsMeshSoftVerticesSet.joint.connectedAnchor = this._normalReference.InverseTransformPoint(vector);
					}
					else
					{
						dazphysicsMeshSoftVerticesSet.lastKinematicPosition = vector;
						dazphysicsMeshSoftVerticesSet.kinematicTransform.position = vector;
					}
					dazphysicsMeshSoftVerticesSet.jointTransform.position = vector;
					dazphysicsMeshSoftVerticesSet.lastPosition = dazphysicsMeshSoftVerticesSet.jointTransform.position;
					dazphysicsMeshSoftVerticesSet.jointTargetPosition = vector;
					dazphysicsMeshSoftVerticesSet.lastJointTargetPosition = dazphysicsMeshSoftVerticesSet.jointTargetPosition;
					dazphysicsMeshSoftVerticesSet.jointTargetVelocity = Vector3.zero;
					if (this._useSimulation && !this._useUniformLimit && !this.embedJoints)
					{
						dazphysicsMeshSoftVerticesSet.jointTrackerTransform.position = dazphysicsMeshSoftVerticesSet.kinematicTransform.position;
					}
					if (!this.embedJoints)
					{
						Quaternion identity = Quaternion.identity;
						if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.NormalReference || dazphysicsMeshSoftVerticesSet.forceLookAtReference)
						{
							identity.SetLookRotation(this._normalReference.position - vector, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex]);
						}
						else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal)
						{
							if (this.centerBetweenTargetAndAnchor)
							{
								identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f);
							}
							else
							{
								identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex]);
							}
						}
						else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp)
						{
							if (this.centerBetweenTargetAndAnchor)
							{
								identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, this._normalReference.position - vector);
							}
							else
							{
								identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex], this._normalReference.position - vector);
							}
						}
						else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp)
						{
							if (this.centerBetweenTargetAndAnchor)
							{
								identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex]);
							}
							else
							{
								identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex], this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex]);
							}
						}
						else
						{
							identity.SetLookRotation(this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex], this.normalReference.position - vector);
						}
						dazphysicsMeshSoftVerticesSet.kinematicTransform.rotation = identity;
						dazphysicsMeshSoftVerticesSet.jointTransform.rotation = identity;
						dazphysicsMeshSoftVerticesSet.jointRB.velocity = Vector3.zero;
						dazphysicsMeshSoftVerticesSet.jointRB.angularVelocity = Vector3.zero;
					}
				}
			}
		}
	}

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x060050AC RID: 20652 RVA: 0x001CCC72 File Offset: 0x001CB072
	public Vector3 bufferedBackForce
	{
		get
		{
			return this._bufferedBackForce;
		}
	}

	// Token: 0x060050AD RID: 20653 RVA: 0x001CCC7C File Offset: 0x001CB07C
	public void PrepareUpdateJointsThreaded()
	{
		if (this._normalReference != null)
		{
			Vector3 position = this._normalReference.position;
			if (NaNUtils.IsVector3Valid(position))
			{
				this._normalReferencePosition = position;
			}
			else
			{
				this.parent.containingAtom.AlertPhysicsCorruption("PhysicsMesh normal reference " + this._normalReference.name);
			}
		}
	}

	// Token: 0x060050AE RID: 20654 RVA: 0x001CCCE4 File Offset: 0x001CB0E4
	public void UpdateJointTargetsThreadedFast(Vector3[] verts, Vector3[] normals)
	{
		for (int i = 0; i < this._softVerticesSets.Count; i++)
		{
			DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
			Vector3 vector;
			if (this.centerBetweenTargetAndAnchor)
			{
				vector = (verts[dazphysicsMeshSoftVerticesSet.targetVertex] + verts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
			}
			else
			{
				vector = verts[dazphysicsMeshSoftVerticesSet.targetVertex];
			}
			dazphysicsMeshSoftVerticesSet.lastJointTargetPosition = dazphysicsMeshSoftVerticesSet.jointTargetPosition;
			dazphysicsMeshSoftVerticesSet.jointTargetPosition = vector;
			dazphysicsMeshSoftVerticesSet.jointTargetVelocity = (dazphysicsMeshSoftVerticesSet.jointTargetPosition - dazphysicsMeshSoftVerticesSet.lastJointTargetPosition) / this.skinTimeDelta;
			Quaternion identity = Quaternion.identity;
			if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.NormalReference || dazphysicsMeshSoftVerticesSet.forceLookAtReference)
			{
				identity.SetLookRotation(this._normalReferencePosition - vector, verts[dazphysicsMeshSoftVerticesSet.anchorVertex] - verts[dazphysicsMeshSoftVerticesSet.targetVertex]);
			}
			else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal)
			{
				if (this.centerBetweenTargetAndAnchor)
				{
					identity.SetLookRotation((-normals[dazphysicsMeshSoftVerticesSet.targetVertex] - normals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f);
				}
				else
				{
					identity.SetLookRotation(-normals[dazphysicsMeshSoftVerticesSet.targetVertex]);
				}
			}
			else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp)
			{
				if (this.centerBetweenTargetAndAnchor)
				{
					identity.SetLookRotation((-normals[dazphysicsMeshSoftVerticesSet.targetVertex] - normals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, this._normalReferencePosition - vector);
				}
				else
				{
					identity.SetLookRotation(-normals[dazphysicsMeshSoftVerticesSet.targetVertex], this._normalReferencePosition - vector);
				}
			}
			else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp)
			{
				if (this.centerBetweenTargetAndAnchor)
				{
					identity.SetLookRotation((-normals[dazphysicsMeshSoftVerticesSet.targetVertex] - normals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, verts[dazphysicsMeshSoftVerticesSet.anchorVertex] - verts[dazphysicsMeshSoftVerticesSet.targetVertex]);
				}
				else
				{
					identity.SetLookRotation(-normals[dazphysicsMeshSoftVerticesSet.targetVertex], verts[dazphysicsMeshSoftVerticesSet.anchorVertex] - vector);
				}
			}
			else
			{
				identity.SetLookRotation(verts[dazphysicsMeshSoftVerticesSet.anchorVertex] - verts[dazphysicsMeshSoftVerticesSet.targetVertex], this._normalReferencePosition - vector);
			}
			dazphysicsMeshSoftVerticesSet.jointTargetLookAt = identity;
		}
	}

	// Token: 0x060050AF RID: 20655 RVA: 0x001CD008 File Offset: 0x001CB408
	public void UpdateJointTargetsThreaded()
	{
		if (this.wasInit && !this.embedJoints)
		{
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				Vector3 vector;
				if (this.centerBetweenTargetAndAnchor)
				{
					vector = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex] + this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
				}
				else
				{
					vector = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex];
				}
				dazphysicsMeshSoftVerticesSet.lastJointTargetPosition = dazphysicsMeshSoftVerticesSet.jointTargetPosition;
				dazphysicsMeshSoftVerticesSet.jointTargetPosition = vector;
				Quaternion identity = Quaternion.identity;
				if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.NormalReference || dazphysicsMeshSoftVerticesSet.forceLookAtReference)
				{
					identity.SetLookRotation(this._normalReferencePosition - vector, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex]);
				}
				else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal)
				{
					if (this.centerBetweenTargetAndAnchor)
					{
						identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f);
					}
					else
					{
						identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex]);
					}
				}
				else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp)
				{
					if (this.centerBetweenTargetAndAnchor)
					{
						identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, this._normalReferencePosition - vector);
					}
					else
					{
						identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex], this._normalReferencePosition - vector);
					}
				}
				else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp)
				{
					if (this.centerBetweenTargetAndAnchor)
					{
						identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex]);
					}
					else
					{
						identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet.targetVertex], this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - vector);
					}
				}
				else
				{
					identity.SetLookRotation(this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex], this._normalReferencePosition - vector);
				}
				dazphysicsMeshSoftVerticesSet.jointTargetLookAt = identity;
			}
		}
	}

	// Token: 0x060050B0 RID: 20656 RVA: 0x001CD3E0 File Offset: 0x001CB7E0
	public void UpdateJointsFast(bool predictOnly = false)
	{
		if (this.wasInit && this._normalReference != null && this._on && !this._freeze)
		{
			Vector3 a;
			a.x = 0f;
			a.y = 0f;
			a.z = 0f;
			float num = 0f;
			float num2 = this._jointBackForceThresholdDistance * this.scale;
			if (this.useJointBackForce && this._jointBackForce > 0f)
			{
				if (this._jointBackForceThresholdDistance == 0f)
				{
					num = 1E+20f;
				}
				else
				{
					num = 1f / num2;
				}
			}
			float num3 = this._normalDistanceLimit * this.scale;
			float num4 = this._normalNegativeDistanceLimit * this.scale;
			float num5 = this._tangentDistanceLimit * this.scale;
			float num6 = this._tangentNegativeDistanceLimit * this.scale;
			float num7 = this._tangent2DistanceLimit * this.scale;
			float num8 = this._tangent2NegativeDistanceLimit * this.scale;
			if (this.resetSimulation)
			{
				this.ResetJoints();
			}
			else
			{
				Vector3 vector;
				vector.x = 0f;
				vector.y = 0f;
				vector.z = 0f;
				if (this.predictionTransform != null)
				{
					int num9 = this.numPredictionFrames;
					if (num9 != 1)
					{
						if (num9 != 2)
						{
							if (num9 == 3)
							{
								vector = this.predictionTransform.position - this.predictionTransformPosition3;
							}
						}
						else
						{
							vector = this.predictionTransform.position - this.predictionTransformPosition2;
						}
					}
					else
					{
						vector = this.predictionTransform.position - this.predictionTransformPosition1;
					}
					this.predictionTransformPosition3 = this.predictionTransformPosition2;
					this.predictionTransformPosition2 = this.predictionTransformPosition1;
					this.predictionTransformPosition1 = this.predictionTransform.position;
				}
				float d = Time.fixedTime - this.skinTime;
				if (this.useSimulation)
				{
					for (int i = 0; i < this._softVerticesSets.Count; i++)
					{
						DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
						dazphysicsMeshSoftVerticesSet.lastKinematicPosition = dazphysicsMeshSoftVerticesSet.kinematicRB.position;
						dazphysicsMeshSoftVerticesSet.kinematicRB.MovePosition(dazphysicsMeshSoftVerticesSet.jointTargetPosition + dazphysicsMeshSoftVerticesSet.jointTargetVelocity * d);
						dazphysicsMeshSoftVerticesSet.kinematicRB.MoveRotation(dazphysicsMeshSoftVerticesSet.jointTargetLookAt);
						if (!this._useUniformLimit)
						{
							dazphysicsMeshSoftVerticesSet.jointTrackerTransform.position = dazphysicsMeshSoftVerticesSet.jointTransform.position;
							Vector3 localPosition = dazphysicsMeshSoftVerticesSet.jointTrackerTransform.localPosition;
							bool flag = false;
							if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num10 = num3 * dazphysicsMeshSoftVerticesSet.limitMultiplier;
								float num11 = num4 * dazphysicsMeshSoftVerticesSet.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition.z > num10)
									{
										localPosition.z = num10;
										flag = true;
									}
									else if (localPosition.z < -num11)
									{
										localPosition.z = -num11;
										flag = true;
									}
								}
								else if (localPosition.y > num10)
								{
									localPosition.y = num10;
									flag = true;
								}
								else if (localPosition.y < -num11)
								{
									localPosition.y = -num11;
									flag = true;
								}
							}
							if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num12 = num5 * dazphysicsMeshSoftVerticesSet.limitMultiplier;
								float num13 = num6 * dazphysicsMeshSoftVerticesSet.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition.y > num12)
									{
										localPosition.y = num12;
										flag = true;
									}
									else if (localPosition.y < -num13)
									{
										localPosition.y = -num13;
										flag = true;
									}
								}
								else if (localPosition.z > num12)
								{
									localPosition.z = num12;
									flag = true;
								}
								else if (localPosition.z < -num13)
								{
									localPosition.z = -num13;
									flag = true;
								}
							}
							if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num14 = num7 * dazphysicsMeshSoftVerticesSet.limitMultiplier;
								float num15 = num8 * dazphysicsMeshSoftVerticesSet.limitMultiplier;
								if (localPosition.x > num14)
								{
									localPosition.x = num14;
									flag = true;
								}
								else if (localPosition.x < -num15)
								{
									localPosition.x = -num15;
									flag = true;
								}
							}
							if (flag)
							{
								dazphysicsMeshSoftVerticesSet.jointTrackerTransform.localPosition = localPosition;
								dazphysicsMeshSoftVerticesSet.jointRB.position = dazphysicsMeshSoftVerticesSet.jointTrackerTransform.position;
							}
						}
						dazphysicsMeshSoftVerticesSet.lastPosition = dazphysicsMeshSoftVerticesSet.jointRB.position;
						if (this.useJointBackForce && this._jointBackForce > 0f)
						{
							Vector3 b = dazphysicsMeshSoftVerticesSet.lastPosition - dazphysicsMeshSoftVerticesSet.lastKinematicPosition;
							if (b.x > num2)
							{
								b.x -= num2;
							}
							else if (b.x < -num2)
							{
								b.x += num2;
							}
							else
							{
								b.x = 0f;
							}
							if (b.y > num2)
							{
								b.y -= num2;
							}
							else if (b.y < -num2)
							{
								b.y += num2;
							}
							else
							{
								b.y = 0f;
							}
							if (b.z > num2)
							{
								b.z -= num2;
							}
							else if (b.z < -num2)
							{
								b.z += num2;
							}
							else
							{
								b.z = 0f;
							}
							a += b;
						}
					}
				}
				else
				{
					for (int j = 0; j < this._softVerticesSets.Count; j++)
					{
						DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet2 = this._softVerticesSets[j];
						dazphysicsMeshSoftVerticesSet2.lastKinematicPosition = dazphysicsMeshSoftVerticesSet2.kinematicTransform.position;
						dazphysicsMeshSoftVerticesSet2.kinematicTransform.SetPositionAndRotation(dazphysicsMeshSoftVerticesSet2.jointTargetPosition + dazphysicsMeshSoftVerticesSet2.jointTargetVelocity * d, dazphysicsMeshSoftVerticesSet2.jointTargetLookAt);
						if (!this._useUniformLimit)
						{
							Vector3 localPosition2 = dazphysicsMeshSoftVerticesSet2.jointTransform.localPosition;
							bool flag2 = false;
							if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num16 = num3 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
								float num17 = num4 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition2.z > num16)
									{
										localPosition2.z = num16;
										flag2 = true;
									}
									else if (localPosition2.z < -num17)
									{
										localPosition2.z = -num17;
										flag2 = true;
									}
								}
								else if (localPosition2.y > num16)
								{
									localPosition2.y = num16;
									flag2 = true;
								}
								else if (localPosition2.y < -num17)
								{
									localPosition2.y = -num17;
									flag2 = true;
								}
							}
							if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num18 = num5 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
								float num19 = num6 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition2.y > num18)
									{
										localPosition2.y = num18;
										flag2 = true;
									}
									else if (localPosition2.y < -num19)
									{
										localPosition2.y = -num19;
										flag2 = true;
									}
								}
								else if (localPosition2.z > num18)
								{
									localPosition2.z = num18;
									flag2 = true;
								}
								else if (localPosition2.z < -num19)
								{
									localPosition2.z = -num19;
									flag2 = true;
								}
							}
							if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num20 = num7 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
								float num21 = num8 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
								if (localPosition2.x > num20)
								{
									localPosition2.x = num20;
									flag2 = true;
								}
								else if (localPosition2.x < -num21)
								{
									localPosition2.x = -num21;
									flag2 = true;
								}
							}
							if (flag2)
							{
								dazphysicsMeshSoftVerticesSet2.jointTransform.localPosition = localPosition2;
							}
						}
						dazphysicsMeshSoftVerticesSet2.lastPosition = dazphysicsMeshSoftVerticesSet2.jointRB.position;
						if (this.useJointBackForce && this._jointBackForce > 0f)
						{
							Vector3 a2 = dazphysicsMeshSoftVerticesSet2.lastPosition - dazphysicsMeshSoftVerticesSet2.lastKinematicPosition;
							float num22 = Mathf.Abs(a2.x) + Mathf.Abs(a2.y) + Mathf.Abs(a2.z);
							if (num22 > num2)
							{
								float d2 = Mathf.Clamp01((num22 - num2) * num);
								a += a2 * d2;
							}
						}
					}
				}
				if (this.useJointBackForce && this._jointBackForce > 0f && this.backForceRigidbody != null)
				{
					float d3;
					if (TimeControl.singleton != null && TimeControl.singleton.compensateFixedTimestep)
					{
						if (Mathf.Approximately(Time.timeScale, 0f))
						{
							d3 = 1f / Time.timeScale;
						}
						else
						{
							d3 = 1f;
						}
					}
					else
					{
						d3 = 1f;
					}
					Vector3 vector2 = a * this._jointBackForce * d3;
					this._appliedBackForce = Vector3.ClampMagnitude(vector2, this._jointBackForceMaxForce * this.scale);
				}
			}
		}
	}

	// Token: 0x060050B1 RID: 20657 RVA: 0x001CDD58 File Offset: 0x001CC158
	public void UpdateJoints()
	{
		if (this.wasInit && this._normalReference != null && this._on && !this._freeze)
		{
			Vector3 a;
			a.x = 0f;
			a.y = 0f;
			a.z = 0f;
			float num = 0f;
			float num2 = this._jointBackForceThresholdDistance * this.scale;
			if (this.useJointBackForce && this._jointBackForce > 0f)
			{
				if (this._jointBackForceThresholdDistance == 0f)
				{
					num = 1E+20f;
				}
				else
				{
					num = 1f / num2;
				}
			}
			float num3 = this._normalDistanceLimit * this.scale;
			float num4 = this._normalNegativeDistanceLimit * this.scale;
			float num5 = this._tangentDistanceLimit * this.scale;
			float num6 = this._tangentNegativeDistanceLimit * this.scale;
			float num7 = this._tangent2DistanceLimit * this.scale;
			float num8 = this._tangent2NegativeDistanceLimit * this.scale;
			if (this.resetSimulation)
			{
				this.ResetJoints();
			}
			else
			{
				Vector3 b;
				b.x = 0f;
				b.y = 0f;
				b.z = 0f;
				if (this.predictionTransform != null)
				{
					int num9 = this.numPredictionFrames;
					if (num9 != 1)
					{
						if (num9 != 2)
						{
							if (num9 == 3)
							{
								b = this.predictionTransform.position - this.predictionTransformPosition3;
							}
						}
						else
						{
							b = this.predictionTransform.position - this.predictionTransformPosition2;
						}
					}
					else
					{
						b = this.predictionTransform.position - this.predictionTransformPosition1;
					}
					this.predictionTransformPosition3 = this.predictionTransformPosition2;
					this.predictionTransformPosition2 = this.predictionTransformPosition1;
					this.predictionTransformPosition1 = this.predictionTransform.position;
				}
				if (this._useSimulation)
				{
					if (this.embedJoints)
					{
						this.embedRB.MovePosition(this._normalReference.position);
						this.embedRB.MoveRotation(this._normalReference.rotation);
						for (int i = 0; i < this._softVerticesSets.Count; i++)
						{
							DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
							Vector3 vector;
							if (this.centerBetweenTargetAndAnchor)
							{
								vector = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex] + this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
							}
							else
							{
								vector = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex];
							}
							dazphysicsMeshSoftVerticesSet.joint.connectedAnchor = this._normalReference.InverseTransformPoint(vector);
							dazphysicsMeshSoftVerticesSet.lastPosition = dazphysicsMeshSoftVerticesSet.jointRB.position;
							if (this._clampVelocity && dazphysicsMeshSoftVerticesSet.jointRB.velocity.sqrMagnitude > this._maxSimulationVelocitySqr)
							{
								dazphysicsMeshSoftVerticesSet.jointRB.velocity = dazphysicsMeshSoftVerticesSet.jointRB.velocity.normalized * this._maxSimulationVelocity;
							}
							if (this.useJointBackForce && this._jointBackForce > 0f)
							{
								Vector3 a2 = dazphysicsMeshSoftVerticesSet.jointTransform.position - vector;
								float num10 = Mathf.Abs(a2.x) + Mathf.Abs(a2.y) + Mathf.Abs(a2.z);
								if (num10 > num2)
								{
									float d = Mathf.Clamp01((num10 - num2) * num);
									a += a2 * d;
								}
							}
						}
					}
					else if (this.useThreading)
					{
						for (int j = 0; j < this._softVerticesSets.Count; j++)
						{
							DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet2 = this._softVerticesSets[j];
							dazphysicsMeshSoftVerticesSet2.lastKinematicPosition = dazphysicsMeshSoftVerticesSet2.kinematicTransform.position;
							dazphysicsMeshSoftVerticesSet2.kinematicRB.MovePosition(dazphysicsMeshSoftVerticesSet2.jointTargetPosition + b);
							dazphysicsMeshSoftVerticesSet2.kinematicRB.MoveRotation(dazphysicsMeshSoftVerticesSet2.jointTargetLookAt);
							if (!this._useUniformLimit)
							{
								dazphysicsMeshSoftVerticesSet2.jointTrackerTransform.position = dazphysicsMeshSoftVerticesSet2.jointTransform.position;
								Vector3 localPosition = dazphysicsMeshSoftVerticesSet2.jointTrackerTransform.localPosition;
								bool flag = false;
								if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
								{
									float num11 = num3 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
									float num12 = num4 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
									if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
									{
										if (localPosition.z > num11)
										{
											localPosition.z = num11;
											flag = true;
										}
										else if (localPosition.z < -num12)
										{
											localPosition.z = -num12;
											flag = true;
										}
									}
									else if (localPosition.y > num11)
									{
										localPosition.y = num11;
										flag = true;
									}
									else if (localPosition.y < -num12)
									{
										localPosition.y = -num12;
										flag = true;
									}
								}
								if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
								{
									float num13 = num5 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
									float num14 = num6 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
									if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
									{
										if (localPosition.y > num13)
										{
											localPosition.y = num13;
											flag = true;
										}
										else if (localPosition.y < -num14)
										{
											localPosition.y = -num14;
											flag = true;
										}
									}
									else if (localPosition.z > num13)
									{
										localPosition.z = num13;
										flag = true;
									}
									else if (localPosition.z < -num14)
									{
										localPosition.z = -num14;
										flag = true;
									}
								}
								if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
								{
									float num15 = num7 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
									float num16 = num8 * dazphysicsMeshSoftVerticesSet2.limitMultiplier;
									if (localPosition.x > num15)
									{
										localPosition.x = num15;
										flag = true;
									}
									else if (localPosition.x < -num16)
									{
										localPosition.x = -num16;
										flag = true;
									}
								}
								if (flag)
								{
									dazphysicsMeshSoftVerticesSet2.jointTrackerTransform.localPosition = localPosition;
									dazphysicsMeshSoftVerticesSet2.jointRB.position = dazphysicsMeshSoftVerticesSet2.jointTrackerTransform.position;
								}
							}
							dazphysicsMeshSoftVerticesSet2.lastPosition = dazphysicsMeshSoftVerticesSet2.jointRB.position;
							if (this.useJointBackForce && this._jointBackForce > 0f)
							{
								Vector3 b2 = dazphysicsMeshSoftVerticesSet2.jointTransform.position - dazphysicsMeshSoftVerticesSet2.lastJointTargetPosition - b;
								if (b2.x > num2)
								{
									b2.x -= num2;
								}
								else if (b2.x < -num2)
								{
									b2.x += num2;
								}
								else
								{
									b2.x = 0f;
								}
								if (b2.y > num2)
								{
									b2.y -= num2;
								}
								else if (b2.y < -num2)
								{
									b2.y += num2;
								}
								else
								{
									b2.y = 0f;
								}
								if (b2.z > num2)
								{
									b2.z -= num2;
								}
								else if (b2.z < -num2)
								{
									b2.z += num2;
								}
								else
								{
									b2.z = 0f;
								}
								a += b2;
							}
						}
					}
					else
					{
						for (int k = 0; k < this._softVerticesSets.Count; k++)
						{
							DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet3 = this._softVerticesSets[k];
							Vector3 vector2;
							if (this.centerBetweenTargetAndAnchor)
							{
								vector2 = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.targetVertex] + this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.anchorVertex]) * 0.5f;
							}
							else
							{
								vector2 = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.targetVertex];
							}
							Quaternion identity = Quaternion.identity;
							if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.NormalReference || dazphysicsMeshSoftVerticesSet3.forceLookAtReference)
							{
								identity.SetLookRotation(this._normalReference.position - vector2, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.targetVertex]);
							}
							else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal)
							{
								if (this.centerBetweenTargetAndAnchor)
								{
									identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.anchorVertex]) * 0.5f);
								}
								else
								{
									identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.targetVertex]);
								}
							}
							else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp)
							{
								if (this.centerBetweenTargetAndAnchor)
								{
									identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.anchorVertex]) * 0.5f, this._normalReference.position - vector2);
								}
								else
								{
									identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.targetVertex], this._normalReference.position - vector2);
								}
							}
							else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp)
							{
								if (this.centerBetweenTargetAndAnchor)
								{
									identity.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.anchorVertex]) * 0.5f, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.targetVertex]);
								}
								else
								{
									identity.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet3.targetVertex], this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.targetVertex]);
								}
							}
							else
							{
								identity.SetLookRotation(this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet3.targetVertex], this.normalReference.position - vector2);
							}
							dazphysicsMeshSoftVerticesSet3.lastKinematicPosition = dazphysicsMeshSoftVerticesSet3.kinematicTransform.position;
							dazphysicsMeshSoftVerticesSet3.kinematicRB.MovePosition(vector2 + b);
							dazphysicsMeshSoftVerticesSet3.kinematicRB.MoveRotation(identity);
							if (!this._useUniformLimit)
							{
								dazphysicsMeshSoftVerticesSet3.jointTrackerTransform.position = dazphysicsMeshSoftVerticesSet3.jointTransform.position;
								Vector3 localPosition2 = dazphysicsMeshSoftVerticesSet3.jointTrackerTransform.localPosition;
								bool flag2 = false;
								if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
								{
									float num17 = num3 * dazphysicsMeshSoftVerticesSet3.limitMultiplier;
									float num18 = num4 * dazphysicsMeshSoftVerticesSet3.limitMultiplier;
									if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
									{
										if (localPosition2.z > num17)
										{
											localPosition2.z = num17;
											flag2 = true;
										}
										else if (localPosition2.z < -num18)
										{
											localPosition2.z = -num18;
											flag2 = true;
										}
									}
									else if (localPosition2.y > num17)
									{
										localPosition2.y = num17;
										flag2 = true;
									}
									else if (localPosition2.y < -num18)
									{
										localPosition2.y = -num18;
										flag2 = true;
									}
								}
								if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
								{
									float num19 = num5 * dazphysicsMeshSoftVerticesSet3.limitMultiplier;
									float num20 = num6 * dazphysicsMeshSoftVerticesSet3.limitMultiplier;
									if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
									{
										if (localPosition2.y > num19)
										{
											localPosition2.y = num19;
											flag2 = true;
										}
										else if (localPosition2.y < -num20)
										{
											localPosition2.y = -num20;
											flag2 = true;
										}
									}
									else if (localPosition2.z > num19)
									{
										localPosition2.z = num19;
										flag2 = true;
									}
									else if (localPosition2.z < -num20)
									{
										localPosition2.z = -num20;
										flag2 = true;
									}
								}
								if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
								{
									float num21 = num7 * dazphysicsMeshSoftVerticesSet3.limitMultiplier;
									float num22 = num8 * dazphysicsMeshSoftVerticesSet3.limitMultiplier;
									if (localPosition2.x > num21)
									{
										localPosition2.x = num21;
										flag2 = true;
									}
									else if (localPosition2.x < -num22)
									{
										localPosition2.x = -num22;
										flag2 = true;
									}
								}
								if (flag2)
								{
									dazphysicsMeshSoftVerticesSet3.jointTrackerTransform.localPosition = localPosition2;
									dazphysicsMeshSoftVerticesSet3.jointRB.position = dazphysicsMeshSoftVerticesSet3.jointTrackerTransform.position;
								}
							}
							dazphysicsMeshSoftVerticesSet3.lastPosition = dazphysicsMeshSoftVerticesSet3.jointRB.position;
							if (this._clampVelocity && dazphysicsMeshSoftVerticesSet3.jointRB.velocity.sqrMagnitude > this._maxSimulationVelocitySqr)
							{
								dazphysicsMeshSoftVerticesSet3.jointRB.velocity = dazphysicsMeshSoftVerticesSet3.jointRB.velocity.normalized * this._maxSimulationVelocity;
							}
							if (this.useJointBackForce && this._jointBackForce > 0f)
							{
								Vector3 a3 = dazphysicsMeshSoftVerticesSet3.jointTransform.position - vector2 - b;
								float num23 = Mathf.Abs(a3.x) + Mathf.Abs(a3.y) + Mathf.Abs(a3.z);
								if (num23 > num2)
								{
									float d2 = Mathf.Clamp01((num23 - num2) * num);
									a += a3 * d2;
								}
							}
						}
					}
				}
				else if (this.useThreading)
				{
					for (int l = 0; l < this._softVerticesSets.Count; l++)
					{
						DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet4 = this._softVerticesSets[l];
						dazphysicsMeshSoftVerticesSet4.kinematicTransform.SetPositionAndRotation(dazphysicsMeshSoftVerticesSet4.jointTargetPosition + b, dazphysicsMeshSoftVerticesSet4.jointTargetLookAt);
						if (!this._useUniformLimit)
						{
							Vector3 localPosition3 = dazphysicsMeshSoftVerticesSet4.jointTransform.localPosition;
							bool flag3 = false;
							if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num24 = num3 * dazphysicsMeshSoftVerticesSet4.limitMultiplier;
								float num25 = num4 * dazphysicsMeshSoftVerticesSet4.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition3.z > num24)
									{
										localPosition3.z = num24;
										flag3 = true;
									}
									else if (localPosition3.z < -num25)
									{
										localPosition3.z = -num25;
										flag3 = true;
									}
								}
								else if (localPosition3.y > num24)
								{
									localPosition3.y = num24;
									flag3 = true;
								}
								else if (localPosition3.y < -num25)
								{
									localPosition3.y = -num25;
									flag3 = true;
								}
							}
							if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num26 = num5 * dazphysicsMeshSoftVerticesSet4.limitMultiplier;
								float num27 = num6 * dazphysicsMeshSoftVerticesSet4.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition3.y > num26)
									{
										localPosition3.y = num26;
										flag3 = true;
									}
									else if (localPosition3.y < -num27)
									{
										localPosition3.y = -num27;
										flag3 = true;
									}
								}
								else if (localPosition3.z > num26)
								{
									localPosition3.z = num26;
									flag3 = true;
								}
								else if (localPosition3.z < -num27)
								{
									localPosition3.z = -num27;
									flag3 = true;
								}
							}
							if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num28 = num7 * dazphysicsMeshSoftVerticesSet4.limitMultiplier;
								float num29 = num8 * dazphysicsMeshSoftVerticesSet4.limitMultiplier;
								if (localPosition3.x > num28)
								{
									localPosition3.x = num28;
									flag3 = true;
								}
								else if (localPosition3.x < -num29)
								{
									localPosition3.x = -num29;
									flag3 = true;
								}
							}
							if (flag3)
							{
								dazphysicsMeshSoftVerticesSet4.jointTransform.localPosition = localPosition3;
							}
						}
						if (this.useJointBackForce && this._jointBackForce > 0f)
						{
							Vector3 a4 = dazphysicsMeshSoftVerticesSet4.jointTransform.position - dazphysicsMeshSoftVerticesSet4.jointTargetPosition - b;
							float num30 = Mathf.Abs(a4.x) + Mathf.Abs(a4.y) + Mathf.Abs(a4.z);
							if (num30 > num2)
							{
								float d3 = Mathf.Clamp01((num30 - num2) * num);
								a += a4 * d3;
							}
						}
					}
				}
				else
				{
					for (int m = 0; m < this._softVerticesSets.Count; m++)
					{
						DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet5 = this._softVerticesSets[m];
						Vector3 vector3;
						if (this.centerBetweenTargetAndAnchor)
						{
							vector3 = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.targetVertex] + this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.anchorVertex]) * 0.5f;
						}
						else
						{
							vector3 = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.targetVertex];
						}
						Quaternion identity2 = Quaternion.identity;
						if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.NormalReference || dazphysicsMeshSoftVerticesSet5.forceLookAtReference)
						{
							identity2.SetLookRotation(this._normalReference.position - vector3, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.targetVertex]);
						}
						else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal)
						{
							if (this.centerBetweenTargetAndAnchor)
							{
								identity2.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.anchorVertex]) * 0.5f);
							}
							else
							{
								identity2.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.targetVertex]);
							}
						}
						else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp)
						{
							if (this.centerBetweenTargetAndAnchor)
							{
								identity2.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.anchorVertex]) * 0.5f, this._normalReference.position - vector3);
							}
							else
							{
								identity2.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.targetVertex], this._normalReference.position - vector3);
							}
						}
						else if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp)
						{
							if (this.centerBetweenTargetAndAnchor)
							{
								identity2.SetLookRotation((-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.targetVertex] - this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.anchorVertex]) * 0.5f, this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.targetVertex]);
							}
							else
							{
								identity2.SetLookRotation(-this._skin.postSkinNormals[dazphysicsMeshSoftVerticesSet5.targetVertex], this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.targetVertex]);
							}
						}
						else
						{
							identity2.SetLookRotation(this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.anchorVertex] - this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet5.targetVertex], this.normalReference.position - vector3);
						}
						dazphysicsMeshSoftVerticesSet5.kinematicTransform.SetPositionAndRotation(vector3 + b, identity2);
						if (!this._useUniformLimit)
						{
							Vector3 localPosition4 = dazphysicsMeshSoftVerticesSet5.jointTransform.localPosition;
							bool flag4 = false;
							if (this._normalMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num31 = num3 * dazphysicsMeshSoftVerticesSet5.limitMultiplier;
								float num32 = num4 * dazphysicsMeshSoftVerticesSet5.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition4.z > num31)
									{
										localPosition4.z = num31;
										flag4 = true;
									}
									else if (localPosition4.z < -num32)
									{
										localPosition4.z = -num32;
										flag4 = true;
									}
								}
								else if (localPosition4.y > num31)
								{
									localPosition4.y = num31;
									flag4 = true;
								}
								else if (localPosition4.y < -num32)
								{
									localPosition4.y = -num32;
									flag4 = true;
								}
							}
							if (this._tangentMovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num33 = num5 * dazphysicsMeshSoftVerticesSet5.limitMultiplier;
								float num34 = num6 * dazphysicsMeshSoftVerticesSet5.limitMultiplier;
								if (this.lookAtOption != DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
								{
									if (localPosition4.y > num33)
									{
										localPosition4.y = num33;
										flag4 = true;
									}
									else if (localPosition4.y < -num34)
									{
										localPosition4.y = -num34;
										flag4 = true;
									}
								}
								else if (localPosition4.z > num33)
								{
									localPosition4.z = num33;
									flag4 = true;
								}
								else if (localPosition4.z < -num34)
								{
									localPosition4.z = -num34;
									flag4 = true;
								}
							}
							if (this._tangent2MovementType == DAZPhysicsMeshSoftVerticesGroup.MovementType.Limit)
							{
								float num35 = num7 * dazphysicsMeshSoftVerticesSet5.limitMultiplier;
								float num36 = num8 * dazphysicsMeshSoftVerticesSet5.limitMultiplier;
								if (localPosition4.x > num35)
								{
									localPosition4.x = num35;
									flag4 = true;
								}
								else if (localPosition4.x < -num36)
								{
									localPosition4.x = -num36;
									flag4 = true;
								}
							}
							if (flag4)
							{
								dazphysicsMeshSoftVerticesSet5.jointTransform.localPosition = localPosition4;
							}
						}
						if (this.useJointBackForce && this._jointBackForce > 0f)
						{
							Vector3 a5 = dazphysicsMeshSoftVerticesSet5.jointTransform.position - vector3;
							float num37 = Mathf.Abs(a5.x) + Mathf.Abs(a5.y) + Mathf.Abs(a5.z);
							if (num37 > num2)
							{
								float d4 = Mathf.Clamp01((num37 - num2) * num);
								a += a5 * d4;
							}
						}
					}
				}
				if (this.useJointBackForce && this._jointBackForce > 0f && this.backForceRigidbody != null)
				{
					float d5;
					if (TimeControl.singleton != null && TimeControl.singleton.compensateFixedTimestep)
					{
						if (Mathf.Approximately(Time.timeScale, 0f))
						{
							d5 = 1f / Time.timeScale;
						}
						else
						{
							d5 = 1f;
						}
					}
					else
					{
						d5 = 1f;
					}
					Vector3 vector4 = a * this._jointBackForce * d5;
					this._appliedBackForce = Vector3.ClampMagnitude(vector4, this._jointBackForceMaxForce * this.scale);
				}
			}
		}
	}

	// Token: 0x060050B2 RID: 20658 RVA: 0x001CF61C File Offset: 0x001CDA1C
	public void ResetAdjustJoints()
	{
		if (this.wasInit && this.backForceAdjustJoints != null)
		{
			if (this.backForceAdjustJointsUseJoint2)
			{
				this.backForceAdjustJoints.additionalJoint2RotationX = 0f;
				this.backForceAdjustJoints.additionalJoint2RotationY = 0f;
				this.backForceAdjustJoints.additionalJoint2RotationZ = 0f;
			}
			else
			{
				this.backForceAdjustJoints.additionalJoint1RotationX = 0f;
				this.backForceAdjustJoints.additionalJoint1RotationY = 0f;
				this.backForceAdjustJoints.additionalJoint1RotationZ = 0f;
			}
		}
	}

	// Token: 0x060050B3 RID: 20659 RVA: 0x001CF6B8 File Offset: 0x001CDAB8
	public void ApplyBackForce()
	{
		if (this.wasInit && this.useJointBackForce && this._jointBackForce > 0f && this.backForceRigidbody != null)
		{
			float num = Time.fixedDeltaTime * 90f;
			this._bufferedBackForce = Vector3.Lerp(this._bufferedBackForce, this._appliedBackForce, this.backForceResponse * num);
			if (this.backForceAdjustJoints != null)
			{
				Vector3 vector = this.backForceRigidbody.transform.InverseTransformVector(this._bufferedBackForce);
				if (this.backForceAdjustJointsUseJoint2)
				{
					this.backForceAdjustJoints.additionalJoint2RotationX += vector.y;
					this.backForceAdjustJoints.additionalJoint2RotationX = Mathf.Clamp(this.backForceAdjustJoints.additionalJoint2RotationX, -this.backForceAdjustJointsMaxAngle, this.backForceAdjustJointsMaxAngle);
					this.backForceAdjustJoints.additionalJoint2RotationY -= vector.x;
					this.backForceAdjustJoints.additionalJoint2RotationY = Mathf.Clamp(this.backForceAdjustJoints.additionalJoint2RotationY, -this.backForceAdjustJointsMaxAngle, this.backForceAdjustJointsMaxAngle);
				}
				else
				{
					this.backForceAdjustJoints.additionalJoint1RotationX += vector.y;
					this.backForceAdjustJoints.additionalJoint1RotationX = Mathf.Clamp(this.backForceAdjustJoints.additionalJoint1RotationX, -this.backForceAdjustJointsMaxAngle, this.backForceAdjustJointsMaxAngle);
					this.backForceAdjustJoints.additionalJoint1RotationY -= vector.x;
					this.backForceAdjustJoints.additionalJoint1RotationY = Mathf.Clamp(this.backForceAdjustJoints.additionalJoint1RotationY, -this.backForceAdjustJointsMaxAngle, this.backForceAdjustJointsMaxAngle);
				}
				this.backForceAdjustJoints.SyncTargetRotation();
			}
			else
			{
				this.backForceRigidbody.AddForce(this._bufferedBackForce, ForceMode.Force);
			}
		}
	}

	// Token: 0x060050B4 RID: 20660 RVA: 0x001CF888 File Offset: 0x001CDC88
	public void PrepareMorphVerticesThreaded(float interpFactor)
	{
		if (this.wasInit && this._normalReference != null && this._on && !this._freeze)
		{
			bool flag = this._useSimulation && this._useInterpolation && this.useCustomInterpolation;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				if (dazphysicsMeshSoftVerticesSet.influenceVertices.Length > 0 && this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.DistanceAlongMoveVector)
				{
					if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
					{
						dazphysicsMeshSoftVerticesSet.primaryMove = dazphysicsMeshSoftVerticesSet.kinematicTransform.forward;
					}
					else
					{
						dazphysicsMeshSoftVerticesSet.primaryMove = dazphysicsMeshSoftVerticesSet.kinematicTransform.up;
					}
				}
				if (flag)
				{
					Vector3 a = Vector3.Lerp(dazphysicsMeshSoftVerticesSet.lastPosition, dazphysicsMeshSoftVerticesSet.jointRB.position, interpFactor);
					Vector3 b = Vector3.Lerp(dazphysicsMeshSoftVerticesSet.lastKinematicPosition, dazphysicsMeshSoftVerticesSet.kinematicTransform.position, interpFactor);
					dazphysicsMeshSoftVerticesSet.move = a - b;
				}
				else
				{
					dazphysicsMeshSoftVerticesSet.move = dazphysicsMeshSoftVerticesSet.jointTransform.position - dazphysicsMeshSoftVerticesSet.kinematicTransform.position;
				}
			}
		}
	}

	// Token: 0x060050B5 RID: 20661 RVA: 0x001CF9C8 File Offset: 0x001CDDC8
	public void PrepareMorphVerticesThreadedFast(float interpFactor)
	{
		if (this.wasInit && this._normalReference != null && this._on && !this._freeze)
		{
			bool flag = this._useSimulation && this._useInterpolation && this.useCustomInterpolation;
			this.lastSkinTime = this.skinTime;
			if (this._useInterpolation)
			{
				this.skinTime = Time.time;
			}
			else
			{
				this.skinTime = Time.fixedTime + Time.fixedDeltaTime;
			}
			this.skinTimeDelta = this.skinTime - this.lastSkinTime;
			if (this.skinTimeDelta <= 0f)
			{
				this.skinTimeDelta = Time.fixedDeltaTime;
			}
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				if (dazphysicsMeshSoftVerticesSet.influenceVertices.Length > 0 && this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.DistanceAlongMoveVector)
				{
					if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
					{
						dazphysicsMeshSoftVerticesSet.primaryMove = dazphysicsMeshSoftVerticesSet.kinematicTransform.forward;
					}
					else
					{
						dazphysicsMeshSoftVerticesSet.primaryMove = dazphysicsMeshSoftVerticesSet.kinematicTransform.up;
					}
				}
				if (flag)
				{
					dazphysicsMeshSoftVerticesSet.lastKinematicPositionThreaded = dazphysicsMeshSoftVerticesSet.lastKinematicPosition;
					dazphysicsMeshSoftVerticesSet.lastPositionThreaded = dazphysicsMeshSoftVerticesSet.lastPosition;
					dazphysicsMeshSoftVerticesSet.currentKinematicPosition = dazphysicsMeshSoftVerticesSet.kinematicTransform.position;
					dazphysicsMeshSoftVerticesSet.currentPosition = dazphysicsMeshSoftVerticesSet.jointRB.position;
					dazphysicsMeshSoftVerticesSet.interpFactor = interpFactor;
				}
				else
				{
					dazphysicsMeshSoftVerticesSet.move = dazphysicsMeshSoftVerticesSet.jointTransform.position - dazphysicsMeshSoftVerticesSet.kinematicTransform.position;
				}
			}
		}
	}

	// Token: 0x060050B6 RID: 20662 RVA: 0x001CFB70 File Offset: 0x001CDF70
	public void MorphVerticesThreadedFast(Vector3[] verts)
	{
		if (this.wasInit && this._on && !this._freeze)
		{
			bool flag = this._useSimulation && this._useInterpolation && this.useCustomInterpolation;
			float num = 1f / this._maxInfluenceDistance;
			bool flag2 = this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				Vector3 b;
				if (this.centerBetweenTargetAndAnchor)
				{
					b = (verts[dazphysicsMeshSoftVerticesSet.targetVertex] + verts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
				}
				else
				{
					b = verts[dazphysicsMeshSoftVerticesSet.targetVertex];
				}
				if (dazphysicsMeshSoftVerticesSet.influenceVertices.Length > 0 && this._influenceType != DAZPhysicsMeshSoftVerticesGroup.InfluenceType.HardCopy)
				{
					if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.DistanceAlongMoveVector)
					{
						for (int j = 0; j < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; j++)
						{
							Vector3 rhs = (verts[dazphysicsMeshSoftVerticesSet.influenceVertices[j]] - b) * this.oneoverscale;
							Vector3 vector = dazphysicsMeshSoftVerticesSet.primaryMove * Vector3.Dot(dazphysicsMeshSoftVerticesSet.primaryMove, rhs);
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[j] = vector.magnitude;
						}
					}
					else
					{
						for (int k = 0; k < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; k++)
						{
							Vector3 vector2 = (verts[dazphysicsMeshSoftVerticesSet.influenceVertices[k]] - b) * this.oneoverscale;
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[k] = vector2.magnitude;
						}
					}
				}
				if (!this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.anchorVertex])
				{
					this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				if (flag2 && !this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.targetVertex])
				{
					this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				if (!this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.targetVertex])
				{
					this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				if (flag)
				{
					Vector3 a = Vector3.Lerp(dazphysicsMeshSoftVerticesSet.lastPositionThreaded, dazphysicsMeshSoftVerticesSet.currentPosition, dazphysicsMeshSoftVerticesSet.interpFactor);
					Vector3 b2 = Vector3.Lerp(dazphysicsMeshSoftVerticesSet.lastKinematicPositionThreaded, dazphysicsMeshSoftVerticesSet.currentKinematicPosition, dazphysicsMeshSoftVerticesSet.interpFactor);
					dazphysicsMeshSoftVerticesSet.move = a - b2;
				}
				this._skin.postSkinMorphs[dazphysicsMeshSoftVerticesSet.targetVertex] = dazphysicsMeshSoftVerticesSet.move;
				if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.HardCopy)
				{
					if (this.autoInfluenceAnchor)
					{
						this._skin.postSkinMorphs[dazphysicsMeshSoftVerticesSet.anchorVertex] = dazphysicsMeshSoftVerticesSet.move;
					}
					for (int l = 0; l < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; l++)
					{
						int num2 = dazphysicsMeshSoftVerticesSet.influenceVertices[l];
						if (!this._skin.postSkinVerts[num2])
						{
							this._skin.postSkinVerts[num2] = true;
							this._skin.postSkinVertsChanged = true;
						}
						this._skin.postSkinMorphs[num2] = dazphysicsMeshSoftVerticesSet.move;
					}
				}
				else
				{
					for (int m = 0; m < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; m++)
					{
						int num3 = dazphysicsMeshSoftVerticesSet.influenceVertices[m];
						float num4 = dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[m];
						float f = Mathf.Min(1f, num4 * num);
						float num5 = 1f - Mathf.Pow(f, this.falloffPower);
						if (this.weightBias > 0f)
						{
							num5 = (1f - this.weightBias) * num5 + this.weightBias;
						}
						else
						{
							num5 = (1f + this.weightBias) * num5;
						}
						dazphysicsMeshSoftVerticesSet.influenceVerticesWeights[m] = num5;
						if (!this._skin.postSkinVerts[num3])
						{
							this._skin.postSkinVerts[num3] = true;
							this._skin.postSkinVertsChanged = true;
						}
						this._skin.postSkinMorphs[num3] = dazphysicsMeshSoftVerticesSet.move * num5 * this.weightMultiplier;
					}
				}
			}
		}
	}

	// Token: 0x060050B7 RID: 20663 RVA: 0x001D0044 File Offset: 0x001CE444
	public void MorphVerticesThreaded()
	{
		if (this.wasInit && this._on && !this._freeze)
		{
			float num = 1f / this._maxInfluenceDistance;
			bool flag = this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				Vector3 b;
				if (this.centerBetweenTargetAndAnchor)
				{
					b = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex] + this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
				}
				else
				{
					b = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex];
				}
				if (dazphysicsMeshSoftVerticesSet.influenceVertices.Length > 0 && this._influenceType != DAZPhysicsMeshSoftVerticesGroup.InfluenceType.HardCopy)
				{
					if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.DistanceAlongMoveVector)
					{
						for (int j = 0; j < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; j++)
						{
							Vector3 rhs = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.influenceVertices[j]] - b) * this.oneoverscale;
							Vector3 vector = dazphysicsMeshSoftVerticesSet.primaryMove * Vector3.Dot(dazphysicsMeshSoftVerticesSet.primaryMove, rhs);
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[j] = vector.magnitude;
						}
					}
					else
					{
						for (int k = 0; k < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; k++)
						{
							Vector3 vector2 = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.influenceVertices[k]] - b) * this.oneoverscale;
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[k] = vector2.magnitude;
						}
					}
				}
				if (!this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.anchorVertex])
				{
					this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				if (flag && !this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.targetVertex])
				{
					this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				if (!this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.targetVertex])
				{
					this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				this._skin.postSkinMorphs[dazphysicsMeshSoftVerticesSet.targetVertex] = dazphysicsMeshSoftVerticesSet.move;
				if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.HardCopy)
				{
					if (this.autoInfluenceAnchor)
					{
						this._skin.postSkinMorphs[dazphysicsMeshSoftVerticesSet.anchorVertex] = dazphysicsMeshSoftVerticesSet.move;
					}
					for (int l = 0; l < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; l++)
					{
						int num2 = dazphysicsMeshSoftVerticesSet.influenceVertices[l];
						if (!this._skin.postSkinVerts[num2])
						{
							this._skin.postSkinVerts[num2] = true;
							this._skin.postSkinVertsChanged = true;
						}
						this._skin.postSkinMorphs[num2] = dazphysicsMeshSoftVerticesSet.move;
					}
				}
				else
				{
					for (int m = 0; m < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; m++)
					{
						int num3 = dazphysicsMeshSoftVerticesSet.influenceVertices[m];
						float num4 = dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[m];
						float f = Mathf.Min(1f, num4 * num);
						float num5 = 1f - Mathf.Pow(f, this.falloffPower);
						if (this.weightBias > 0f)
						{
							num5 = (1f - this.weightBias) * num5 + this.weightBias;
						}
						else
						{
							num5 = (1f + this.weightBias) * num5;
						}
						dazphysicsMeshSoftVerticesSet.influenceVerticesWeights[m] = num5;
						if (!this._skin.postSkinVerts[num3])
						{
							this._skin.postSkinVerts[num3] = true;
							this._skin.postSkinVertsChanged = true;
						}
						this._skin.postSkinMorphs[num3] = dazphysicsMeshSoftVerticesSet.move * num5 * this.weightMultiplier;
					}
				}
			}
		}
	}

	// Token: 0x060050B8 RID: 20664 RVA: 0x001D04BC File Offset: 0x001CE8BC
	public void MorphVertices(float interpFactor)
	{
		if (this.wasInit && this._normalReference != null && this._on && !this._freeze)
		{
			float num = 1f / this._maxInfluenceDistance;
			bool flag = this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormal || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalRefUp || this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.VertexNormalAnchorUp;
			bool flag2 = this._useSimulation && this.useCustomInterpolation;
			for (int i = 0; i < this._softVerticesSets.Count; i++)
			{
				DAZPhysicsMeshSoftVerticesSet dazphysicsMeshSoftVerticesSet = this._softVerticesSets[i];
				Vector3 b;
				if (this.centerBetweenTargetAndAnchor)
				{
					b = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex] + this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.anchorVertex]) * 0.5f;
				}
				else
				{
					b = this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.targetVertex];
				}
				if (dazphysicsMeshSoftVerticesSet.influenceVertices.Length > 0 && this._influenceType != DAZPhysicsMeshSoftVerticesGroup.InfluenceType.HardCopy)
				{
					if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.DistanceAlongMoveVector)
					{
						Vector3 vector;
						if (this.lookAtOption == DAZPhysicsMeshSoftVerticesGroup.LookAtOption.Anchor)
						{
							vector = dazphysicsMeshSoftVerticesSet.kinematicTransform.forward;
						}
						else
						{
							vector = dazphysicsMeshSoftVerticesSet.kinematicTransform.up;
						}
						for (int j = 0; j < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; j++)
						{
							Vector3 rhs = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.influenceVertices[j]] - b) * this.oneoverscale;
							Vector3 vector2 = vector * Vector3.Dot(vector, rhs);
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[j] = vector2.magnitude;
						}
					}
					else
					{
						for (int k = 0; k < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; k++)
						{
							Vector3 vector3 = (this._skin.rawSkinnedVerts[dazphysicsMeshSoftVerticesSet.influenceVertices[k]] - b) * this.oneoverscale;
							dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[k] = vector3.magnitude;
						}
					}
				}
				Vector3 vector4;
				if (flag2)
				{
					Vector3 a = Vector3.Lerp(dazphysicsMeshSoftVerticesSet.lastPosition, dazphysicsMeshSoftVerticesSet.jointRB.position, interpFactor);
					vector4 = a - b;
				}
				else
				{
					vector4 = dazphysicsMeshSoftVerticesSet.jointTransform.position - b;
				}
				if (!this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.anchorVertex])
				{
					this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				if (flag)
				{
					if (!this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.targetVertex])
					{
						this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
						this._skin.postSkinVertsChanged = true;
					}
					if (this.centerBetweenTargetAndAnchor && !this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.anchorVertex])
					{
						this._skin.postSkinNormalVerts[dazphysicsMeshSoftVerticesSet.anchorVertex] = true;
					}
				}
				if (!this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.targetVertex])
				{
					this._skin.postSkinVerts[dazphysicsMeshSoftVerticesSet.targetVertex] = true;
					this._skin.postSkinVertsChanged = true;
				}
				this._skin.postSkinMorphs[dazphysicsMeshSoftVerticesSet.targetVertex] = vector4;
				if (this._influenceType == DAZPhysicsMeshSoftVerticesGroup.InfluenceType.HardCopy)
				{
					if (this.autoInfluenceAnchor)
					{
						this._skin.postSkinMorphs[dazphysicsMeshSoftVerticesSet.anchorVertex] = vector4;
					}
					for (int l = 0; l < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; l++)
					{
						int num2 = dazphysicsMeshSoftVerticesSet.influenceVertices[l];
						if (!this._skin.postSkinVerts[num2])
						{
							this._skin.postSkinVerts[num2] = true;
							this._skin.postSkinVertsChanged = true;
						}
						this._skin.postSkinMorphs[num2] = vector4;
					}
				}
				else
				{
					for (int m = 0; m < dazphysicsMeshSoftVerticesSet.influenceVertices.Length; m++)
					{
						int num3 = dazphysicsMeshSoftVerticesSet.influenceVertices[m];
						float num4 = dazphysicsMeshSoftVerticesSet.influenceVerticesDistances[m];
						float f = Mathf.Min(1f, num4 * num);
						float num5 = 1f - Mathf.Pow(f, this.falloffPower);
						if (this.weightBias > 0f)
						{
							num5 = (1f - this.weightBias) * num5 + this.weightBias;
						}
						else
						{
							num5 = (1f + this.weightBias) * num5;
						}
						dazphysicsMeshSoftVerticesSet.influenceVerticesWeights[m] = num5;
						if (!this._skin.postSkinVerts[num3])
						{
							this._skin.postSkinVerts[num3] = true;
							this._skin.postSkinVertsChanged = true;
						}
						this._skin.postSkinMorphs[num3] = vector4 * num5 * this.weightMultiplier;
					}
				}
			}
		}
	}

	// Token: 0x04004000 RID: 16384
	public DAZPhysicsMesh parent;

	// Token: 0x04004001 RID: 16385
	protected Vector3 zero = Vector3.zero;

	// Token: 0x04004002 RID: 16386
	protected bool _on = true;

	// Token: 0x04004003 RID: 16387
	protected bool _freeze;

	// Token: 0x04004004 RID: 16388
	public bool enabled = true;

	// Token: 0x04004005 RID: 16389
	protected float scale = 1f;

	// Token: 0x04004006 RID: 16390
	protected float oneoverscale = 1f;

	// Token: 0x04004007 RID: 16391
	protected bool triggerThreadedScaleChange;

	// Token: 0x04004008 RID: 16392
	public bool useParentSettings = true;

	// Token: 0x04004009 RID: 16393
	public bool useParentColliderSettings = true;

	// Token: 0x0400400A RID: 16394
	public bool useParentColliderSettingsForSecondCollider;

	// Token: 0x0400400B RID: 16395
	public float parentSettingSpringMultiplier = 1f;

	// Token: 0x0400400C RID: 16396
	public float parentSettingDamperMultiplier = 1f;

	// Token: 0x0400400D RID: 16397
	public int[] otherGroupNumsCollisionAllowed;

	// Token: 0x0400400E RID: 16398
	protected HashSet<int> _otherGroupNumsCollisionAllowedHash;

	// Token: 0x0400400F RID: 16399
	protected bool _collisionEnabled = true;

	// Token: 0x04004010 RID: 16400
	public string name;

	// Token: 0x04004011 RID: 16401
	public bool showSoftSets = true;

	// Token: 0x04004012 RID: 16402
	[SerializeField]
	protected List<DAZPhysicsMeshSoftVerticesSet> _softVerticesSets;

	// Token: 0x04004013 RID: 16403
	[SerializeField]
	private int _currentSetIndex;

	// Token: 0x04004014 RID: 16404
	[SerializeField]
	private DAZPhysicsMeshSoftVerticesGroup.InfluenceType _influenceType;

	// Token: 0x04004015 RID: 16405
	[SerializeField]
	private bool _autoInfluenceAnchor;

	// Token: 0x04004016 RID: 16406
	public bool centerBetweenTargetAndAnchor;

	// Token: 0x04004017 RID: 16407
	[SerializeField]
	private float _maxInfluenceDistance = 0.03f;

	// Token: 0x04004018 RID: 16408
	[SerializeField]
	private DAZPhysicsMeshSoftVerticesGroup.LookAtOption _lookAtOption;

	// Token: 0x04004019 RID: 16409
	[SerializeField]
	private float _falloffPower = 2f;

	// Token: 0x0400401A RID: 16410
	[SerializeField]
	private float _weightMultiplier = 1f;

	// Token: 0x0400401B RID: 16411
	[SerializeField]
	private float _jointSpringNormal = 10f;

	// Token: 0x0400401C RID: 16412
	[SerializeField]
	private float _jointDamperNormal = 1f;

	// Token: 0x0400401D RID: 16413
	[SerializeField]
	private float _jointSpringTangent = 10f;

	// Token: 0x0400401E RID: 16414
	[SerializeField]
	private float _jointDamperTangent = 1f;

	// Token: 0x0400401F RID: 16415
	[SerializeField]
	private float _jointSpringTangent2 = 10f;

	// Token: 0x04004020 RID: 16416
	[SerializeField]
	private float _jointDamperTangent2 = 1f;

	// Token: 0x04004021 RID: 16417
	[SerializeField]
	private float _jointSpringMaxForce = 10000000f;

	// Token: 0x04004022 RID: 16418
	[SerializeField]
	private float _jointMass = 0.01f;

	// Token: 0x04004023 RID: 16419
	[SerializeField]
	private DAZPhysicsMeshSoftVerticesGroup.ColliderOrient _colliderOrient;

	// Token: 0x04004024 RID: 16420
	[SerializeField]
	private DAZPhysicsMeshSoftVerticesGroup.ColliderType _colliderType;

	// Token: 0x04004025 RID: 16421
	protected bool _colliderSyncDirty;

	// Token: 0x04004026 RID: 16422
	[SerializeField]
	private float _colliderRadius = 0.003f;

	// Token: 0x04004027 RID: 16423
	[SerializeField]
	private float _colliderLength = 0.003f;

	// Token: 0x04004028 RID: 16424
	[SerializeField]
	private float _colliderNormalOffset;

	// Token: 0x04004029 RID: 16425
	[SerializeField]
	private float _colliderAdditionalNormalOffset;

	// Token: 0x0400402A RID: 16426
	[SerializeField]
	private float _colliderTangentOffset;

	// Token: 0x0400402B RID: 16427
	[SerializeField]
	private float _colliderTangent2Offset;

	// Token: 0x0400402C RID: 16428
	[SerializeField]
	private string _colliderLayer;

	// Token: 0x0400402D RID: 16429
	[SerializeField]
	private bool _useSecondCollider;

	// Token: 0x0400402E RID: 16430
	[SerializeField]
	private bool _addGPUCollider;

	// Token: 0x0400402F RID: 16431
	[SerializeField]
	private bool _addSecondGPUCollider;

	// Token: 0x04004030 RID: 16432
	[SerializeField]
	private float _secondColliderRadius = 0.003f;

	// Token: 0x04004031 RID: 16433
	[SerializeField]
	private float _secondColliderLength = 0.003f;

	// Token: 0x04004032 RID: 16434
	[SerializeField]
	private float _secondColliderNormalOffset;

	// Token: 0x04004033 RID: 16435
	[SerializeField]
	private float _secondColliderAdditionalNormalOffset;

	// Token: 0x04004034 RID: 16436
	[SerializeField]
	private float _secondColliderTangentOffset;

	// Token: 0x04004035 RID: 16437
	[SerializeField]
	private float _secondColliderTangent2Offset;

	// Token: 0x04004036 RID: 16438
	[SerializeField]
	private Transform[] _ignoreColliders;

	// Token: 0x04004037 RID: 16439
	public AutoColliderGroup[] ignoreAutoColliderGroups;

	// Token: 0x04004038 RID: 16440
	[SerializeField]
	private PhysicMaterial _colliderMaterial;

	// Token: 0x04004039 RID: 16441
	[SerializeField]
	private float _weightBias;

	// Token: 0x0400403A RID: 16442
	[SerializeField]
	private bool _useUniformLimit;

	// Token: 0x0400403B RID: 16443
	[SerializeField]
	private DAZPhysicsMeshSoftVerticesGroup.MovementType _normalMovementType;

	// Token: 0x0400403C RID: 16444
	[SerializeField]
	private DAZPhysicsMeshSoftVerticesGroup.MovementType _tangentMovementType;

	// Token: 0x0400403D RID: 16445
	[SerializeField]
	private DAZPhysicsMeshSoftVerticesGroup.MovementType _tangent2MovementType;

	// Token: 0x0400403E RID: 16446
	protected Vector3 _startingNormalReferencePosition;

	// Token: 0x0400403F RID: 16447
	[SerializeField]
	private Transform _normalReference;

	// Token: 0x04004040 RID: 16448
	private Vector3 _normalReferencePosition;

	// Token: 0x04004041 RID: 16449
	[SerializeField]
	private float _normalDistanceLimit = 0.015f;

	// Token: 0x04004042 RID: 16450
	[SerializeField]
	private float _normalNegativeDistanceLimit = 0.015f;

	// Token: 0x04004043 RID: 16451
	[SerializeField]
	private float _tangentDistanceLimit = 0.015f;

	// Token: 0x04004044 RID: 16452
	[SerializeField]
	private float _tangentNegativeDistanceLimit = 0.015f;

	// Token: 0x04004045 RID: 16453
	[SerializeField]
	private float _tangent2DistanceLimit = 0.015f;

	// Token: 0x04004046 RID: 16454
	[SerializeField]
	private float _tangent2NegativeDistanceLimit = 0.015f;

	// Token: 0x04004047 RID: 16455
	public bool useLinkJoints = true;

	// Token: 0x04004048 RID: 16456
	public bool tieLinkJointSpringAndDamperToNormalSpringAndDamper;

	// Token: 0x04004049 RID: 16457
	[SerializeField]
	private float _linkSpring = 1f;

	// Token: 0x0400404A RID: 16458
	[SerializeField]
	private float _linkDamper = 0.1f;

	// Token: 0x0400404B RID: 16459
	private bool _resetSimulation;

	// Token: 0x0400404C RID: 16460
	[SerializeField]
	private bool _useSimulation;

	// Token: 0x0400404D RID: 16461
	public bool useCustomInterpolation = true;

	// Token: 0x0400404E RID: 16462
	public bool embedJoints;

	// Token: 0x0400404F RID: 16463
	[SerializeField]
	private bool _clampVelocity;

	// Token: 0x04004050 RID: 16464
	[SerializeField]
	private float _maxSimulationVelocity = 1f;

	// Token: 0x04004051 RID: 16465
	private float _maxSimulationVelocitySqr = 1f;

	// Token: 0x04004052 RID: 16466
	[SerializeField]
	private bool _useInterpolation;

	// Token: 0x04004053 RID: 16467
	[SerializeField]
	private int _solverIterations = 15;

	// Token: 0x04004054 RID: 16468
	public Rigidbody backForceRigidbody;

	// Token: 0x04004055 RID: 16469
	public AdjustJoints backForceAdjustJoints;

	// Token: 0x04004056 RID: 16470
	public bool backForceAdjustJointsUseJoint2;

	// Token: 0x04004057 RID: 16471
	public float backForceAdjustJointsMaxAngle;

	// Token: 0x04004058 RID: 16472
	public FreeControllerV3 controller;

	// Token: 0x04004059 RID: 16473
	public bool useJointBackForce;

	// Token: 0x0400405A RID: 16474
	[SerializeField]
	private float _jointBackForce;

	// Token: 0x0400405B RID: 16475
	[SerializeField]
	private float _jointBackForceThresholdDistance;

	// Token: 0x0400405C RID: 16476
	[SerializeField]
	private float _jointBackForceMaxForce;

	// Token: 0x0400405D RID: 16477
	protected DAZSkinV2 _skin;

	// Token: 0x0400405E RID: 16478
	private bool wasInit;

	// Token: 0x0400405F RID: 16479
	private bool wasInit2;

	// Token: 0x04004060 RID: 16480
	protected List<Collider> allPossibleIgnoreCollidersList;

	// Token: 0x04004061 RID: 16481
	protected List<Collider> ignoreCollidersList;

	// Token: 0x04004062 RID: 16482
	[SerializeField]
	protected bool _multiplyMassByLimitMultiplier = true;

	// Token: 0x04004063 RID: 16483
	public Transform predictionTransform;

	// Token: 0x04004064 RID: 16484
	public int numPredictionFrames = 2;

	// Token: 0x04004065 RID: 16485
	protected Vector3 predictionTransformPosition1;

	// Token: 0x04004066 RID: 16486
	protected Vector3 predictionTransformPosition2;

	// Token: 0x04004067 RID: 16487
	protected Vector3 predictionTransformPosition3;

	// Token: 0x04004068 RID: 16488
	protected Vector3 softVertexBackForce;

	// Token: 0x04004069 RID: 16489
	protected Transform embedTransform;

	// Token: 0x0400406A RID: 16490
	protected Rigidbody embedRB;

	// Token: 0x0400406B RID: 16491
	protected const float linkAdjustDistanceThresholdSquared = 1E-06f;

	// Token: 0x0400406C RID: 16492
	protected bool linkTargetsDirty;

	// Token: 0x0400406D RID: 16493
	protected Vector3 _bufferedBackForce;

	// Token: 0x0400406E RID: 16494
	protected Vector3 _appliedBackForce;

	// Token: 0x0400406F RID: 16495
	public float backForceResponse = 1f;

	// Token: 0x04004070 RID: 16496
	public bool useThreading;

	// Token: 0x04004071 RID: 16497
	protected float lastSkinTime;

	// Token: 0x04004072 RID: 16498
	protected float skinTime;

	// Token: 0x04004073 RID: 16499
	protected float skinTimeDelta;

	// Token: 0x02000B4C RID: 2892
	public enum InfluenceType
	{
		// Token: 0x04004075 RID: 16501
		Distance,
		// Token: 0x04004076 RID: 16502
		DistanceAlongMoveVector,
		// Token: 0x04004077 RID: 16503
		HardCopy
	}

	// Token: 0x02000B4D RID: 2893
	public enum LookAtOption
	{
		// Token: 0x04004079 RID: 16505
		Anchor,
		// Token: 0x0400407A RID: 16506
		NormalReference,
		// Token: 0x0400407B RID: 16507
		VertexNormal,
		// Token: 0x0400407C RID: 16508
		VertexNormalRefUp,
		// Token: 0x0400407D RID: 16509
		VertexNormalAnchorUp
	}

	// Token: 0x02000B4E RID: 2894
	public enum ColliderOrient
	{
		// Token: 0x0400407F RID: 16511
		Tangent2,
		// Token: 0x04004080 RID: 16512
		Tangent,
		// Token: 0x04004081 RID: 16513
		Normal
	}

	// Token: 0x02000B4F RID: 2895
	public enum ColliderType
	{
		// Token: 0x04004083 RID: 16515
		Capsule,
		// Token: 0x04004084 RID: 16516
		Sphere,
		// Token: 0x04004085 RID: 16517
		Box
	}

	// Token: 0x02000B50 RID: 2896
	public enum MovementType
	{
		// Token: 0x04004087 RID: 16519
		Free,
		// Token: 0x04004088 RID: 16520
		Limit,
		// Token: 0x04004089 RID: 16521
		Lock
	}
}
