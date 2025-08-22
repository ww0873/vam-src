using System;
using System.Collections;
using System.Collections.Generic;
using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
[ExecuteInEditMode]
public class AutoCollider : PhysicsSimulator
{
	// Token: 0x0600467B RID: 18043 RVA: 0x00145B8C File Offset: 0x00143F8C
	public AutoCollider()
	{
	}

	// Token: 0x0600467C RID: 18044 RVA: 0x00145CC4 File Offset: 0x001440C4
	protected void SyncOn()
	{
		this._globalOn = AutoCollider.globalEnable;
		if (this._on && AutoCollider.globalEnable)
		{
			if (this.softJointType == AutoCollider.SoftJointType.FloatingKinematic)
			{
				this.ResetJointInternal();
			}
			if (this.jointTransform != null)
			{
				this.jointTransform.gameObject.SetActive(true);
			}
		}
		else
		{
			Vector3 zero = Vector3.zero;
			if (this.jointTransform != null)
			{
				this.jointTransform.gameObject.SetActive(false);
			}
			if (this._skin.wasInit && this.morphVertex)
			{
				this._skin.postSkinMorphs[this.targetVertex].x = 0f;
				this._skin.postSkinMorphs[this.targetVertex].y = 0f;
				this._skin.postSkinMorphs[this.targetVertex].z = 0f;
			}
		}
	}

	// Token: 0x170009AF RID: 2479
	// (get) Token: 0x0600467D RID: 18045 RVA: 0x00145DCC File Offset: 0x001441CC
	// (set) Token: 0x0600467E RID: 18046 RVA: 0x00145DD4 File Offset: 0x001441D4
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

	// Token: 0x170009B0 RID: 2480
	// (get) Token: 0x0600467F RID: 18047 RVA: 0x00145DEF File Offset: 0x001441EF
	// (set) Token: 0x06004680 RID: 18048 RVA: 0x00145DF7 File Offset: 0x001441F7
	public bool createSoftCollider
	{
		get
		{
			return this._createSoftCollider;
		}
		set
		{
			if (this._createSoftCollider != value)
			{
				this._createSoftCollider = value;
				this.CreateColliders();
			}
		}
	}

	// Token: 0x170009B1 RID: 2481
	// (get) Token: 0x06004681 RID: 18049 RVA: 0x00145E12 File Offset: 0x00144212
	// (set) Token: 0x06004682 RID: 18050 RVA: 0x00145E1A File Offset: 0x0014421A
	public AutoCollider.JointType jointType
	{
		get
		{
			return this._jointType;
		}
		set
		{
			if (this._jointType != value)
			{
				this._jointType = value;
				this.CreateColliders();
			}
		}
	}

	// Token: 0x170009B2 RID: 2482
	// (get) Token: 0x06004683 RID: 18051 RVA: 0x00145E35 File Offset: 0x00144235
	// (set) Token: 0x06004684 RID: 18052 RVA: 0x00145E3D File Offset: 0x0014423D
	public AutoCollider.SoftJointType softJointType
	{
		get
		{
			return this._softJointType;
		}
		set
		{
			if (this._softJointType != value)
			{
				this._softJointType = value;
				this.CreateColliders();
			}
		}
	}

	// Token: 0x170009B3 RID: 2483
	// (get) Token: 0x06004685 RID: 18053 RVA: 0x00145E58 File Offset: 0x00144258
	// (set) Token: 0x06004686 RID: 18054 RVA: 0x00145E60 File Offset: 0x00144260
	public bool createHardCollider
	{
		get
		{
			return this._createHardCollider;
		}
		set
		{
			if (this._createHardCollider != value)
			{
				this._createHardCollider = value;
				this.CreateColliders();
			}
		}
	}

	// Token: 0x170009B4 RID: 2484
	// (get) Token: 0x06004687 RID: 18055 RVA: 0x00145E7B File Offset: 0x0014427B
	// (set) Token: 0x06004688 RID: 18056 RVA: 0x00145E83 File Offset: 0x00144283
	public float softJointLimit
	{
		get
		{
			return this._softJointLimit;
		}
		set
		{
			if (this._softJointLimit != value)
			{
				this._softJointLimit = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009B5 RID: 2485
	// (get) Token: 0x06004689 RID: 18057 RVA: 0x00145E9E File Offset: 0x0014429E
	// (set) Token: 0x0600468A RID: 18058 RVA: 0x00145EA6 File Offset: 0x001442A6
	public float softJointLimitSpring
	{
		get
		{
			return this._softJointLimitSpring;
		}
		set
		{
			if (this._softJointLimitSpring != value)
			{
				this._softJointLimitSpring = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009B6 RID: 2486
	// (get) Token: 0x0600468B RID: 18059 RVA: 0x00145EC1 File Offset: 0x001442C1
	// (set) Token: 0x0600468C RID: 18060 RVA: 0x00145EC9 File Offset: 0x001442C9
	public float softJointLimitDamper
	{
		get
		{
			return this._softJointLimitDamper;
		}
		set
		{
			if (this._softJointLimitDamper != value)
			{
				this._softJointLimitDamper = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x00145EE4 File Offset: 0x001442E4
	protected override void SyncCollisionEnabled()
	{
		base.SyncCollisionEnabled();
		if (this.jointRB != null)
		{
			this.jointRB.detectCollisions = (this._collisionEnabled && !this._resetSimulation);
		}
	}

	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x0600468E RID: 18062 RVA: 0x00145F1F File Offset: 0x0014431F
	// (set) Token: 0x0600468F RID: 18063 RVA: 0x00145F27 File Offset: 0x00144327
	public float jointSpringLook
	{
		get
		{
			return this._jointSpringLook;
		}
		set
		{
			if (this._jointSpringLook != value)
			{
				this._jointSpringLook = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x06004690 RID: 18064 RVA: 0x00145F42 File Offset: 0x00144342
	// (set) Token: 0x06004691 RID: 18065 RVA: 0x00145F4A File Offset: 0x0014434A
	public float jointDamperLook
	{
		get
		{
			return this._jointDamperLook;
		}
		set
		{
			if (this._jointDamperLook != value)
			{
				this._jointDamperLook = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x06004692 RID: 18066 RVA: 0x00145F65 File Offset: 0x00144365
	// (set) Token: 0x06004693 RID: 18067 RVA: 0x00145F6D File Offset: 0x0014436D
	public float jointSpringUp
	{
		get
		{
			return this._jointSpringUp;
		}
		set
		{
			if (this._jointSpringUp != value)
			{
				this._jointSpringUp = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x06004694 RID: 18068 RVA: 0x00145F88 File Offset: 0x00144388
	// (set) Token: 0x06004695 RID: 18069 RVA: 0x00145F90 File Offset: 0x00144390
	public float jointDamperUp
	{
		get
		{
			return this._jointDamperUp;
		}
		set
		{
			if (this._jointDamperUp != value)
			{
				this._jointDamperUp = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x06004696 RID: 18070 RVA: 0x00145FAB File Offset: 0x001443AB
	// (set) Token: 0x06004697 RID: 18071 RVA: 0x00145FB3 File Offset: 0x001443B3
	public float jointSpringRight
	{
		get
		{
			return this._jointSpringRight;
		}
		set
		{
			if (this._jointSpringRight != value)
			{
				this._jointSpringRight = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x06004698 RID: 18072 RVA: 0x00145FCE File Offset: 0x001443CE
	// (set) Token: 0x06004699 RID: 18073 RVA: 0x00145FD6 File Offset: 0x001443D6
	public float jointDamperRight
	{
		get
		{
			return this._jointDamperRight;
		}
		set
		{
			if (this._jointDamperRight != value)
			{
				this._jointDamperRight = value;
				this.SyncJointParams();
			}
		}
	}

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x0600469A RID: 18074 RVA: 0x00145FF1 File Offset: 0x001443F1
	// (set) Token: 0x0600469B RID: 18075 RVA: 0x00145FF9 File Offset: 0x001443F9
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

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x0600469C RID: 18076 RVA: 0x00146014 File Offset: 0x00144414
	// (set) Token: 0x0600469D RID: 18077 RVA: 0x0014601C File Offset: 0x0014441C
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

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x0600469E RID: 18078 RVA: 0x00146037 File Offset: 0x00144437
	// (set) Token: 0x0600469F RID: 18079 RVA: 0x0014603F File Offset: 0x0014443F
	public AutoCollider.ColliderOrient colliderOrient
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
				this.AutoColliderSizeSet(false);
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x060046A0 RID: 18080 RVA: 0x00146061 File Offset: 0x00144461
	// (set) Token: 0x060046A1 RID: 18081 RVA: 0x00146069 File Offset: 0x00144469
	public AutoCollider.ColliderType colliderType
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

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x060046A2 RID: 18082 RVA: 0x00146097 File Offset: 0x00144497
	// (set) Token: 0x060046A3 RID: 18083 RVA: 0x0014609F File Offset: 0x0014449F
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
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x060046A4 RID: 18084 RVA: 0x001460BA File Offset: 0x001444BA
	// (set) Token: 0x060046A5 RID: 18085 RVA: 0x001460C2 File Offset: 0x001444C2
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
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x060046A6 RID: 18086 RVA: 0x001460DD File Offset: 0x001444DD
	// (set) Token: 0x060046A7 RID: 18087 RVA: 0x001460E5 File Offset: 0x001444E5
	public bool useAutoRadius
	{
		get
		{
			return this._useAutoRadius;
		}
		set
		{
			if (this._useAutoRadius != value)
			{
				this._useAutoRadius = value;
				this.AutoColliderSizeSet(false);
			}
		}
	}

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x060046A8 RID: 18088 RVA: 0x00146101 File Offset: 0x00144501
	// (set) Token: 0x060046A9 RID: 18089 RVA: 0x00146109 File Offset: 0x00144509
	public bool useAutoLength
	{
		get
		{
			return this._useAutoLength;
		}
		set
		{
			if (this._useAutoLength != value)
			{
				this._useAutoLength = value;
				this.AutoColliderSizeSet(false);
			}
		}
	}

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x060046AA RID: 18090 RVA: 0x00146125 File Offset: 0x00144525
	// (set) Token: 0x060046AB RID: 18091 RVA: 0x0014612D File Offset: 0x0014452D
	public AutoCollider.LookAtOption lookAtOption
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
				this.UpdateTransforms();
				this.AutoColliderSizeSet(false);
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x060046AC RID: 18092 RVA: 0x00146155 File Offset: 0x00144555
	// (set) Token: 0x060046AD RID: 18093 RVA: 0x0014615D File Offset: 0x0014455D
	public bool useAnchor2AsUp
	{
		get
		{
			return this._useAnchor2AsUp;
		}
		set
		{
			if (this._useAnchor2AsUp != value)
			{
				this._useAnchor2AsUp = value;
				this.UpdateTransforms();
			}
		}
	}

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x060046AE RID: 18094 RVA: 0x00146178 File Offset: 0x00144578
	// (set) Token: 0x060046AF RID: 18095 RVA: 0x00146180 File Offset: 0x00144580
	public float autoRadiusMultiplier
	{
		get
		{
			return this._autoRadiusMultiplier;
		}
		set
		{
			if (this._autoRadiusMultiplier != value)
			{
				this._autoRadiusMultiplier = value;
				this.AutoColliderSizeSet(true);
			}
		}
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x060046B0 RID: 18096 RVA: 0x0014619C File Offset: 0x0014459C
	// (set) Token: 0x060046B1 RID: 18097 RVA: 0x001461A4 File Offset: 0x001445A4
	public float autoRadiusBuffer
	{
		get
		{
			return this._autoRadiusBuffer;
		}
		set
		{
			if (this._autoRadiusBuffer != value)
			{
				this._autoRadiusBuffer = value;
				this.AutoColliderSizeSet(true);
			}
		}
	}

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x060046B2 RID: 18098 RVA: 0x001461C0 File Offset: 0x001445C0
	// (set) Token: 0x060046B3 RID: 18099 RVA: 0x001461C8 File Offset: 0x001445C8
	public float autoLengthBuffer
	{
		get
		{
			return this._autoLengthBuffer;
		}
		set
		{
			if (this._autoLengthBuffer != value)
			{
				this._autoLengthBuffer = value;
				this.AutoColliderSizeSet(true);
			}
		}
	}

	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x060046B4 RID: 18100 RVA: 0x001461E4 File Offset: 0x001445E4
	// (set) Token: 0x060046B5 RID: 18101 RVA: 0x001461EC File Offset: 0x001445EC
	public bool centerJoint
	{
		get
		{
			return this._centerJoint;
		}
		set
		{
			if (this._centerJoint != value)
			{
				this._centerJoint = value;
				this.UpdateTransforms();
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x060046B6 RID: 18102 RVA: 0x0014620D File Offset: 0x0014460D
	// (set) Token: 0x060046B7 RID: 18103 RVA: 0x00146215 File Offset: 0x00144615
	public float colliderLookOffset
	{
		get
		{
			return this._colliderLookOffset;
		}
		set
		{
			if (this._colliderLookOffset != value)
			{
				this._colliderLookOffset = value;
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x060046B8 RID: 18104 RVA: 0x00146230 File Offset: 0x00144630
	// (set) Token: 0x060046B9 RID: 18105 RVA: 0x00146238 File Offset: 0x00144638
	public float colliderUpOffset
	{
		get
		{
			return this._colliderUpOffset;
		}
		set
		{
			if (this._colliderUpOffset != value)
			{
				this._colliderUpOffset = value;
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009CD RID: 2509
	// (get) Token: 0x060046BA RID: 18106 RVA: 0x00146253 File Offset: 0x00144653
	// (set) Token: 0x060046BB RID: 18107 RVA: 0x0014625B File Offset: 0x0014465B
	public float colliderRightOffset
	{
		get
		{
			return this._colliderRightOffset;
		}
		set
		{
			if (this._colliderRightOffset != value)
			{
				this._colliderRightOffset = value;
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x060046BC RID: 18108 RVA: 0x00146276 File Offset: 0x00144676
	// (set) Token: 0x060046BD RID: 18109 RVA: 0x0014627E File Offset: 0x0014467E
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

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x060046BE RID: 18110 RVA: 0x001462B1 File Offset: 0x001446B1
	// (set) Token: 0x060046BF RID: 18111 RVA: 0x001462B9 File Offset: 0x001446B9
	public float hardColliderBuffer
	{
		get
		{
			return this._hardColliderBuffer;
		}
		set
		{
			if (this._hardColliderBuffer != value)
			{
				this._hardColliderBuffer = value;
				this.SetColliders();
			}
		}
	}

	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x060046C0 RID: 18112 RVA: 0x001462D4 File Offset: 0x001446D4
	// (set) Token: 0x060046C1 RID: 18113 RVA: 0x001462DC File Offset: 0x001446DC
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

	// Token: 0x170009D1 RID: 2513
	// (get) Token: 0x060046C2 RID: 18114 RVA: 0x001462F1 File Offset: 0x001446F1
	// (set) Token: 0x060046C3 RID: 18115 RVA: 0x001462F9 File Offset: 0x001446F9
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
				this.SetColliders();
			}
		}
	}

	// Token: 0x060046C4 RID: 18116 RVA: 0x00146319 File Offset: 0x00144719
	protected override void SyncUseInterpolation()
	{
		base.SyncUseInterpolation();
		this.SyncJointParams();
	}

	// Token: 0x170009D2 RID: 2514
	// (get) Token: 0x060046C5 RID: 18117 RVA: 0x00146327 File Offset: 0x00144727
	// (set) Token: 0x060046C6 RID: 18118 RVA: 0x0014632F File Offset: 0x0014472F
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

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x060046C7 RID: 18119 RVA: 0x00146344 File Offset: 0x00144744
	// (set) Token: 0x060046C8 RID: 18120 RVA: 0x0014634C File Offset: 0x0014474C
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

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x060046C9 RID: 18121 RVA: 0x00146361 File Offset: 0x00144761
	// (set) Token: 0x060046CA RID: 18122 RVA: 0x00146369 File Offset: 0x00144769
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

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x060046CB RID: 18123 RVA: 0x0014637E File Offset: 0x0014477E
	// (set) Token: 0x060046CC RID: 18124 RVA: 0x00146386 File Offset: 0x00144786
	public Transform skinTransform
	{
		get
		{
			return this._skinTransform;
		}
		set
		{
			if (this._skinTransform != value)
			{
				this._skinTransform = value;
			}
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x060046CD RID: 18125 RVA: 0x001463A0 File Offset: 0x001447A0
	// (set) Token: 0x060046CE RID: 18126 RVA: 0x001463A8 File Offset: 0x001447A8
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			this._skin = value;
			this._uvVertToBaseVertDict = null;
			if (this._skin != null)
			{
				this.wasInit = false;
				this.Init();
			}
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x060046CF RID: 18127 RVA: 0x001463D6 File Offset: 0x001447D6
	// (set) Token: 0x060046D0 RID: 18128 RVA: 0x001463DE File Offset: 0x001447DE
	public bool showUsedVerts
	{
		get
		{
			return this._showUsedVerts;
		}
		set
		{
			if (this._showUsedVerts != value)
			{
				this._showUsedVerts = value;
			}
		}
	}

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x060046D1 RID: 18129 RVA: 0x001463F4 File Offset: 0x001447F4
	protected Dictionary<int, int> uvVertToBaseVertDict
	{
		get
		{
			if (this._uvVertToBaseVertDict == null)
			{
				if (this._skin != null && this._skin.dazMesh != null)
				{
					this._uvVertToBaseVertDict = this._skin.dazMesh.uvVertToBaseVert;
				}
				else
				{
					this._uvVertToBaseVertDict = new Dictionary<int, int>();
				}
			}
			return this._uvVertToBaseVertDict;
		}
	}

	// Token: 0x060046D2 RID: 18130 RVA: 0x00146460 File Offset: 0x00144860
	public int GetBaseVertex(int vid)
	{
		int num;
		if (this._skin != null && this._skin.dazMesh != null && this.uvVertToBaseVertDict.TryGetValue(vid, out num))
		{
			vid = num;
		}
		return vid;
	}

	// Token: 0x060046D3 RID: 18131 RVA: 0x001464AB File Offset: 0x001448AB
	public bool IsBaseVertex(int vid)
	{
		return !(this._skin != null) || !(this._skin.dazMesh != null) || !this.uvVertToBaseVertDict.ContainsKey(vid);
	}

	// Token: 0x060046D4 RID: 18132 RVA: 0x001464E8 File Offset: 0x001448E8
	public void ClickVertex(int vid)
	{
		if (this._skin != null && this._skin.dazMesh != null)
		{
			int num;
			if (this.uvVertToBaseVertDict.TryGetValue(vid, out num))
			{
				vid = num;
			}
			if (this.targetVertex == -1 && this.anchorVertex1 != vid && this.anchorVertex2 != vid && this.oppositeVertex != vid)
			{
				this.targetVertex = vid;
			}
			else if (this.targetVertex == vid)
			{
				this.targetVertex = -1;
			}
			else if (this.anchorVertex1 == -1 && this.anchorVertex2 != vid && this.oppositeVertex != vid)
			{
				this.anchorVertex1 = vid;
			}
			else if (this.anchorVertex1 == vid)
			{
				this.anchorVertex1 = -1;
			}
			else if (this.anchorVertex2 == -1 && this.oppositeVertex != vid)
			{
				this.anchorVertex2 = vid;
			}
			else if (this.anchorVertex2 == vid)
			{
				this.anchorVertex2 = -1;
			}
			else if (this.oppositeVertex == -1)
			{
				this.oppositeVertex = vid;
			}
			else if (this.oppositeVertex == vid)
			{
				this.oppositeVertex = -1;
			}
			this.UpdateTransforms();
			this.AutoColliderSizeSet(false);
			this.FixNames();
		}
	}

	// Token: 0x060046D5 RID: 18133 RVA: 0x00146648 File Offset: 0x00144A48
	public void Init()
	{
		if (Application.isPlaying && !this.wasInit && this._skin != null && this.targetVertex != -1 && this.anchorVertex1 != -1)
		{
			if (this.debug)
			{
				Debug.Log("AutoCollider Init() " + base.name);
			}
			this.wasInit = true;
			this._skin.Init();
			this._skin.postSkinVerts[this.targetVertex] = true;
			this._skin.postSkinVerts[this.anchorVertex1] = true;
			if (this.anchorVertex2 != -1)
			{
				this._skin.postSkinVerts[this.anchorVertex2] = true;
			}
			if (this.oppositeVertex != -1)
			{
				this._skin.postSkinVerts[this.oppositeVertex] = true;
			}
			if ((this._softJointType == AutoCollider.SoftJointType.FloatingKinematic || (this._createHardCollider && this.hardPositionUpdateTrigger != AutoCollider.PositionUpdateTrigger.None)) && (this._lookAtOption == AutoCollider.LookAtOption.VertexNormal || (this._lookAtOption == AutoCollider.LookAtOption.Anchor1 && this.oppositeVertex == -1)))
			{
				this._skin.postSkinNormalVerts[this.targetVertex] = true;
			}
			if (this._softJointType == AutoCollider.SoftJointType.Direct && this._lookAtOption == AutoCollider.LookAtOption.VertexNormal && this._centerJoint)
			{
				this._skin.postSkinNormalVerts[this.targetVertex] = true;
			}
			this._skin.postSkinVertsChanged = true;
			if (this.jointTransform != null && !this.jointTransformInitialRotationWasInit)
			{
				this.jointTransformInitialRotation = this.jointTransform.localRotation;
				this.jointTransformInitialRotationWasInit = true;
			}
			if (this.bone == null && this.backForceRigidbody != null)
			{
				this.bone = this.backForceRigidbody.GetComponent<DAZBone>();
			}
		}
	}

	// Token: 0x060046D6 RID: 18134 RVA: 0x00146829 File Offset: 0x00144C29
	private void SyncJointParams()
	{
		this.SetMass();
		this.SetInterpolation();
		this.SetJointLimits();
		this.SetJointDrive();
		this.SetColliders();
	}

	// Token: 0x060046D7 RID: 18135 RVA: 0x0014684C File Offset: 0x00144C4C
	private void GetCollidersRecursive(Transform rootTransform, Transform t, List<Collider> colliders)
	{
		if (t != rootTransform && t.GetComponent<Rigidbody>())
		{
			return;
		}
		foreach (Collider collider in t.GetComponents<Collider>())
		{
			if (collider != null && collider.gameObject.activeInHierarchy && collider.enabled)
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

	// Token: 0x060046D8 RID: 18136 RVA: 0x00146920 File Offset: 0x00144D20
	public void InitColliders()
	{
		if (this.jointCollider != null)
		{
			List<Collider> list = new List<Collider>();
			foreach (Transform transform in this._ignoreColliders)
			{
				this.GetCollidersRecursive(transform, transform, list);
			}
			foreach (Collider collider in list)
			{
				Physics.IgnoreCollision(this.jointCollider, collider);
			}
		}
	}

	// Token: 0x060046D9 RID: 18137 RVA: 0x001469C0 File Offset: 0x00144DC0
	private void SetInterpolation()
	{
		if (this.jointRB != null)
		{
			if (this._useInterpolation)
			{
				if (this.morphVertex)
				{
					this.jointRB.interpolation = RigidbodyInterpolation.Interpolate;
				}
				else
				{
					this.jointRB.interpolation = RigidbodyInterpolation.None;
				}
			}
			else
			{
				this.jointRB.interpolation = RigidbodyInterpolation.None;
			}
		}
	}

	// Token: 0x060046DA RID: 18138 RVA: 0x00146A24 File Offset: 0x00144E24
	private void SetJointLimits()
	{
		if (this._jointType == AutoCollider.JointType.Configurable && this.joint != null)
		{
			if (this._softJointLimit != 0f)
			{
				if (this._lookAtOption == AutoCollider.LookAtOption.Opposite || this._lookAtOption == AutoCollider.LookAtOption.AnchorCenters)
				{
					if (this._colliderOrient == AutoCollider.ColliderOrient.Look)
					{
						this.joint.xMotion = ConfigurableJointMotion.Limited;
						this.joint.yMotion = ConfigurableJointMotion.Limited;
						this.joint.zMotion = ConfigurableJointMotion.Limited;
					}
					else
					{
						this.joint.xMotion = ConfigurableJointMotion.Limited;
						this.joint.yMotion = ConfigurableJointMotion.Limited;
						this.joint.zMotion = ConfigurableJointMotion.Limited;
					}
				}
				else
				{
					this.joint.xMotion = ConfigurableJointMotion.Limited;
					this.joint.yMotion = ConfigurableJointMotion.Limited;
					this.joint.zMotion = ConfigurableJointMotion.Limited;
				}
				SoftJointLimit linearLimit = this.joint.linearLimit;
				linearLimit.limit = this._softJointLimit * this._scale;
				this.joint.linearLimit = linearLimit;
				SoftJointLimitSpring linearLimitSpring = this.joint.linearLimitSpring;
				linearLimitSpring.spring = this._softJointLimitSpring * this.oneoverscale;
				linearLimitSpring.damper = this._softJointLimitDamper * this.oneoverscale;
				this.joint.linearLimitSpring = linearLimitSpring;
			}
			else
			{
				this.joint.xMotion = ConfigurableJointMotion.Free;
				this.joint.yMotion = ConfigurableJointMotion.Free;
				this.joint.zMotion = ConfigurableJointMotion.Free;
			}
			this.joint.angularXMotion = ConfigurableJointMotion.Locked;
			this.joint.angularYMotion = ConfigurableJointMotion.Locked;
			this.joint.angularZMotion = ConfigurableJointMotion.Locked;
			this.joint.projectionMode = JointProjectionMode.None;
			this.joint.projectionDistance = 0.01f;
			this.joint.projectionAngle = 1f;
		}
		else if (this._jointType == AutoCollider.JointType.Spring && this.springJoint != null)
		{
			this.springJoint.spring = this._softJointLimitSpring * this.oneoverscale;
			this.springJoint.damper = this._softJointLimitDamper * this.oneoverscale;
			this.springJoint.tolerance = 0f;
			this.springJoint.minDistance = 0f;
			this.springJoint.maxDistance = 0f;
		}
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x00146C60 File Offset: 0x00145060
	private void SetJointDrive()
	{
		if (this._jointType == AutoCollider.JointType.Configurable && this.joint != null)
		{
			float num = 1f / this._scale;
			JointDrive zDrive = default(JointDrive);
			zDrive.positionSpring = this._jointSpringLook * num;
			zDrive.positionDamper = this._jointDamperLook * num;
			zDrive.maximumForce = this._jointSpringMaxForce * num;
			JointDrive xDrive = default(JointDrive);
			xDrive.positionSpring = this._jointSpringUp * num;
			xDrive.positionDamper = this._jointDamperUp * num;
			xDrive.maximumForce = this._jointSpringMaxForce * num;
			JointDrive yDrive = default(JointDrive);
			yDrive.positionSpring = this._jointSpringRight * num;
			yDrive.positionDamper = this._jointDamperRight * num;
			yDrive.maximumForce = this._jointSpringMaxForce * num;
			this.joint.xDrive = xDrive;
			this.joint.yDrive = yDrive;
			this.joint.zDrive = zDrive;
		}
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x00146D5C File Offset: 0x0014515C
	private void SetColliders()
	{
		int direction = 0;
		Vector3 center;
		center.x = this._colliderRightOffset;
		center.y = this._colliderUpOffset;
		center.z = this._colliderLookOffset;
		AutoCollider.ColliderOrient colliderOrient = this._colliderOrient;
		if (colliderOrient != AutoCollider.ColliderOrient.Look)
		{
			if (colliderOrient != AutoCollider.ColliderOrient.Up)
			{
				if (colliderOrient == AutoCollider.ColliderOrient.Right)
				{
					direction = 0;
					if (!this._centerJoint)
					{
						center.z += this._colliderRadius;
					}
				}
			}
			else
			{
				direction = 1;
				if (!this._centerJoint)
				{
					center.z += this._colliderRadius;
				}
			}
		}
		else
		{
			direction = 2;
			if (this._lookAtOption == AutoCollider.LookAtOption.Anchor1)
			{
				center.y += this._colliderRadius;
			}
			else if (!this._centerJoint)
			{
				float num = this._colliderLength * 0.5f;
				if (num < this._colliderRadius)
				{
					num = this._colliderRadius;
				}
				center.z += num;
			}
		}
		if (this.jointCollider != null)
		{
			AutoCollider.ColliderType colliderType = this.colliderType;
			if (colliderType != AutoCollider.ColliderType.Capsule)
			{
				if (colliderType != AutoCollider.ColliderType.Sphere)
				{
					if (colliderType == AutoCollider.ColliderType.Box)
					{
						BoxCollider boxCollider = this.jointCollider as BoxCollider;
						float num2 = this._colliderRadius * 2f;
						boxCollider.size = new Vector3(num2, num2, num2);
						boxCollider.center = center;
					}
				}
				else
				{
					SphereCollider sphereCollider = this.jointCollider as SphereCollider;
					sphereCollider.radius = this._colliderRadius;
					sphereCollider.center = center;
				}
			}
			else
			{
				CapsuleCollider capsuleCollider = this.jointCollider as CapsuleCollider;
				capsuleCollider.radius = this._colliderRadius;
				capsuleCollider.height = this._colliderLength;
				capsuleCollider.direction = direction;
				capsuleCollider.center = center;
				CapsuleLineSphereCollider component = capsuleCollider.GetComponent<CapsuleLineSphereCollider>();
				if (component != null)
				{
					component.UpdateData();
				}
			}
			if (this._colliderMaterial != null)
			{
				this.jointCollider.sharedMaterial = this._colliderMaterial;
			}
		}
		if (this.hardCollider != null && (!Application.isPlaying || this.updateHardColliderSize))
		{
			AutoCollider.ColliderType colliderType2 = this.colliderType;
			if (colliderType2 != AutoCollider.ColliderType.Capsule)
			{
				if (colliderType2 != AutoCollider.ColliderType.Sphere)
				{
					if (colliderType2 == AutoCollider.ColliderType.Box)
					{
						BoxCollider boxCollider2 = this.hardCollider as BoxCollider;
						float num3 = (this._colliderRadius - this.hardColliderBuffer) * 2f;
						boxCollider2.size = new Vector3(num3, num3, num3);
						boxCollider2.center = center;
					}
				}
				else
				{
					SphereCollider sphereCollider2 = this.hardCollider as SphereCollider;
					sphereCollider2.radius = this._colliderRadius - this.hardColliderBuffer;
					sphereCollider2.center = center;
				}
			}
			else
			{
				CapsuleCollider capsuleCollider2 = this.hardCollider as CapsuleCollider;
				capsuleCollider2.radius = this._colliderRadius - this.hardColliderBuffer;
				capsuleCollider2.height = this._colliderLength - this.hardColliderBuffer * 2f;
				capsuleCollider2.direction = direction;
				capsuleCollider2.center = center;
				CapsuleLineSphereCollider component2 = capsuleCollider2.GetComponent<CapsuleLineSphereCollider>();
				if (component2 != null)
				{
					component2.UpdateData();
				}
			}
		}
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x0014709B File Offset: 0x0014549B
	private void SetMass()
	{
		if (this.jointRB != null)
		{
			this.jointRB.mass = this._jointMass;
		}
	}

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x060046DE RID: 18142 RVA: 0x001470BF File Offset: 0x001454BF
	public Vector3 bufferedBackForce
	{
		get
		{
			return this._bufferedBackForce;
		}
	}

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x060046DF RID: 18143 RVA: 0x001470C7 File Offset: 0x001454C7
	public Vector3 appliedBackForce
	{
		get
		{
			return this._appliedBackForce;
		}
	}

	// Token: 0x060046E0 RID: 18144 RVA: 0x001470D0 File Offset: 0x001454D0
	protected void FixNames()
	{
		if (this.jointTransform != null && this.jointTransform.name != base.name + "Joint")
		{
			this.jointTransform.name = base.name + "Joint";
		}
		if (this.kinematicTransform != null && this.kinematicTransform.name != base.name + "KO")
		{
			this.kinematicTransform.name = base.name + "KO";
		}
		if (this.hardTransform != null && this.hardTransform.name != base.name + "Hard")
		{
			this.hardTransform.name = base.name + "Hard";
		}
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x001471D0 File Offset: 0x001455D0
	public void CreateColliders()
	{
		if (!Application.isPlaying && this.targetVertex != -1 && this.anchorVertex1 != -1)
		{
            // 创建soft collider
			if (this._createSoftCollider)
			{
				if (this._softJointType == AutoCollider.SoftJointType.Direct)
				{
					if (this.kinematicTransform != null)
					{
						UnityEngine.Object.DestroyImmediate(this.kinematicTransform.gameObject);
						this.kinematicTransform = null;
						this.kinematicRB = null;
					}
				}
				// this._softJointType != AutoCollider.SoftJointType.Direct
                // FloatingKinematic
				else if (this.kinematicTransform == null)
				{
                    // 创建 kinematic go
                    // 创建一个go, 挂载Rigidbody
                    // 设置为，自己的孩子
                    // isKinematic = true
					GameObject gameObject = new GameObject(base.name + "KO");
					this.kinematicTransform = gameObject.transform;
					this.kinematicTransform.SetParent(base.transform);
					this.kinematicRB = gameObject.AddComponent<Rigidbody>();
					this.kinematicRB.isKinematic = true;
				}
				// this.kinematicTransform != null
				else
				{
                    // 已经有了Rigidbody isKinematic=true
                    // 只设置名称
					this.kinematicTransform.name = base.name + "KO";
				}

                // 创建joint go
				GameObject gameObject2;
				if (this.jointTransform == null)
				{
					gameObject2 = new GameObject(base.name + "Joint");
					this.jointTransform = gameObject2.transform;
				}
				else
				{
					this.jointTransform.name = base.name + "Joint";
					gameObject2 = this.jointTransform.gameObject;
				}

                // 给joint go 添加Rigidbody
				if (this.jointRB == null)
				{
					this.jointRB = this.jointTransform.gameObject.AddComponent<Rigidbody>();
				}

                // 要创建  Configurable joint
                // 给joint go，添加ConfigurableJoint
				if (this.joint == null && this._jointType == AutoCollider.JointType.Configurable)
				{
					this.joint = this.jointTransform.gameObject.AddComponent<ConfigurableJoint>();
                    // 同时销毁，没必要的，spring joint
					if (this.springJoint != null)
					{
						UnityEngine.Object.DestroyImmediate(this.springJoint);
						this.springJoint = null;
					}

					if (this.controller != null)
					{
                        // gameObject2 就是 joint go
                        // 设置 configurable joint
						ConfigurableJoint configurableJoint = gameObject2.AddComponent<ConfigurableJoint>();
						configurableJoint.rotationDriveMode = RotationDriveMode.Slerp;
						configurableJoint.autoConfigureConnectedAnchor = false;
						configurableJoint.connectedAnchor = Vector3.zero;

						Rigidbody component = this.controller.GetComponent<Rigidbody>();
						this.controller.transform.position = gameObject2.transform.position;
						this.controller.transform.rotation = gameObject2.transform.rotation;
						if (component != null)
						{
                            // configurable joint 控制 controler的 rigidbody???
							configurableJoint.connectedBody = component;
						}

                        // ????
						this.controller.followWhenOffRB = this.jointRB;
					}
				}

                // 要创建sprint joint
				if (this.springJoint == null && this._jointType == AutoCollider.JointType.Spring)
				{
					this.springJoint = this.jointTransform.gameObject.AddComponent<SpringJoint>();
					if (this.joint != null)
					{
						UnityEngine.Object.DestroyImmediate(this.joint);
						this.joint = null;
					}
				}

                // 给joint 添加 collider
                // 删除掉旧collider
				if (this.jointCollider != null)
				{
					bool flag = false;
					if (this.jointCollider.GetType() == typeof(CapsuleCollider) && this._colliderType != AutoCollider.ColliderType.Capsule)
					{
						flag = true;
					}
					if (this.jointCollider.GetType() == typeof(SphereCollider) && this._colliderType != AutoCollider.ColliderType.Sphere)
					{
						flag = true;
					}
					if (this.jointCollider.GetType() == typeof(BoxCollider) && this._colliderType != AutoCollider.ColliderType.Box)
					{
						flag = true;
					}
					if (flag)
					{
						UnityEngine.Object.DestroyImmediate(this.jointCollider);
						this.jointCollider = null;
					}
				}
				if (this.jointCollider == null)
				{
					AutoCollider.ColliderType colliderType = this.colliderType;
					if (colliderType != AutoCollider.ColliderType.Capsule)
					{
						if (colliderType != AutoCollider.ColliderType.Sphere)
						{
							if (colliderType == AutoCollider.ColliderType.Box)
							{
								BoxCollider boxCollider = gameObject2.AddComponent<BoxCollider>();
								this.jointCollider = boxCollider;
							}
						}
						else
						{
							SphereCollider sphereCollider = gameObject2.AddComponent<SphereCollider>();
							this.jointCollider = sphereCollider;
						}
					}
					else
					{
						CapsuleCollider capsuleCollider = gameObject2.AddComponent<CapsuleCollider>();
						this.jointCollider = capsuleCollider;
					}
				}

				this.jointRB.useGravity = false;
				this.jointRB.drag = 0.1f;
				this.jointRB.angularDrag = 0f;
				this.jointRB.collisionDetectionMode = CollisionDetectionMode.Discrete;
				this.jointRB.isKinematic = false;

				if (this._softJointType == AutoCollider.SoftJointType.Direct)
				{
					this.jointTransform.SetParent(base.transform);
					if (this.joint != null)
					{
						this.joint.autoConfigureConnectedAnchor = false;
					}
					if (this.springJoint != null)
					{
						this.springJoint.autoConfigureConnectedAnchor = false;
					}
					this.UpdateTransforms();
					this.UpdateAnchor();
				}
				else
				{
					this.UpdateTransforms();
					this.jointTransform.SetParent(this.kinematicTransform);
					this.jointTransform.position = this.kinematicTransform.position;
					if (this.joint != null)
					{
						this.joint.connectedBody = this.kinematicRB;
						this.joint.autoConfigureConnectedAnchor = false;
						this.joint.anchor = Vector3.zero;
						this.joint.connectedAnchor = Vector3.zero;
					}
					if (this.springJoint != null)
					{
						this.springJoint.connectedBody = this.kinematicRB;
						this.springJoint.autoConfigureConnectedAnchor = false;
						this.springJoint.anchor = Vector3.zero;
						this.springJoint.connectedAnchor = Vector3.zero;
					}
					this.jointTransform.rotation = this.kinematicTransform.rotation;
				}
				if (this._colliderLayer != null && this._colliderLayer != string.Empty)
				{
					if (this.joint != null)
					{
						this.joint.gameObject.layer = LayerMask.NameToLayer(this._colliderLayer);
					}
					if (this.springJoint != null)
					{
						this.springJoint.gameObject.layer = LayerMask.NameToLayer(this._colliderLayer);
					}
				}
				else
				{
					if (this.joint != null)
					{
						this.joint.gameObject.layer = base.gameObject.layer;
					}
					if (this.springJoint != null)
					{
						this.springJoint.gameObject.layer = base.gameObject.layer;
					}
				}
			}
			else
			{
				if (this.jointTransform != null)
				{
					UnityEngine.Object.DestroyImmediate(this.jointTransform.gameObject);
					this.jointTransform = null;
					this.jointRB = null;
					this.joint = null;
					this.springJoint = null;
					this.jointCollider = null;
				}
				if (this.kinematicTransform != null)
				{
					UnityEngine.Object.DestroyImmediate(this.kinematicTransform.gameObject);
					this.kinematicTransform = null;
					this.kinematicRB = null;
				}
			}
			if (this._createHardCollider)
			{
				GameObject gameObject3;
				if (this.hardTransform == null)
				{
					gameObject3 = new GameObject(base.name + "Hard");
					this.hardTransform = gameObject3.transform;
				}
				else
				{
					gameObject3 = this.hardTransform.gameObject;
					this.hardTransform.name = base.name + "Hard";
				}
				this.hardTransform.SetParent(base.transform);
				this.UpdateTransforms();
				if (this._colliderLayer != null && this._colliderLayer != string.Empty)
				{
					gameObject3.layer = LayerMask.NameToLayer(this._colliderLayer);
				}
				else
				{
					gameObject3.layer = base.gameObject.layer;
				}
				if (this.hardCollider != null)
				{
					bool flag2 = false;
					if (this.hardCollider.GetType() == typeof(CapsuleCollider) && this._colliderType != AutoCollider.ColliderType.Capsule)
					{
						flag2 = true;
					}
					if (this.hardCollider.GetType() == typeof(SphereCollider) && this._colliderType != AutoCollider.ColliderType.Sphere)
					{
						flag2 = true;
					}
					if (this.hardCollider.GetType() == typeof(BoxCollider) && this._colliderType != AutoCollider.ColliderType.Box)
					{
						flag2 = true;
					}
					if (flag2)
					{
						UnityEngine.Object.DestroyImmediate(this.hardCollider);
						this.hardCollider = null;
					}
				}
				if (this.hardCollider == null)
				{
					AutoCollider.ColliderType colliderType2 = this.colliderType;
					if (colliderType2 != AutoCollider.ColliderType.Capsule)
					{
						if (colliderType2 != AutoCollider.ColliderType.Sphere)
						{
							if (colliderType2 == AutoCollider.ColliderType.Box)
							{
								BoxCollider boxCollider2 = gameObject3.AddComponent<BoxCollider>();
								this.hardCollider = boxCollider2;
							}
						}
						else
						{
							SphereCollider sphereCollider2 = gameObject3.AddComponent<SphereCollider>();
							this.hardCollider = sphereCollider2;
						}
					}
					else
					{
						CapsuleCollider capsuleCollider2 = gameObject3.AddComponent<CapsuleCollider>();
						this.hardCollider = capsuleCollider2;
					}
				}
			}
			else if (this.hardTransform != null)
			{
				UnityEngine.Object.DestroyImmediate(this.hardTransform.gameObject);
				this.hardTransform = null;
				this.hardCollider = null;
			}
			this.SetJointLimits();
			this.SetJointDrive();
			this.SetColliders();
			this.SetMass();
			if (this.jointRB != null)
			{
				this.jointRB.centerOfMass = Vector3.zero;
			}
			this.InitColliders();
		}
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x00147AE0 File Offset: 0x00145EE0
	public void UpdateAnchorTarget()
	{
		if (this._softJointType == AutoCollider.SoftJointType.Direct && this.backForceRigidbody != null)
		{
			bool flag = true;
			Vector3[] array;
			Vector3[] array2;
			if (Application.isPlaying)
			{
				array = this._skin.rawSkinnedVerts;
				array2 = this._skin.postSkinNormals;
				flag = this._skin.postSkinVertsReady[this.targetVertex];
			}
			else
			{
				array = this._skin.dazMesh.morphedUVVertices;
				array2 = this._skin.dazMesh.morphedUVNormals;
			}
			if (flag)
			{
				if (this._centerJoint)
				{
					if (this.lookAtOption == AutoCollider.LookAtOption.Opposite && this.oppositeVertex != -1)
					{
						this.anchorTarget = (array[this.targetVertex] + array[this.oppositeVertex]) * 0.5f;
					}
					else if (this.lookAtOption == AutoCollider.LookAtOption.AnchorCenters && this.anchorVertex1 != -1 && this.anchorVertex2 != -1)
					{
						this.anchorTarget = (array[this.anchorVertex1] + array[this.anchorVertex2]) * 0.5f;
					}
					else if (this.lookAtOption == AutoCollider.LookAtOption.VertexNormal)
					{
						if (this._colliderOrient == AutoCollider.ColliderOrient.Look)
						{
							float num = this._colliderLength * 0.5f * this._scale;
							if (num < this._colliderRadius * this._scale)
							{
								num = this._colliderRadius * this._scale;
							}
							this.anchorTarget = array[this.targetVertex] + array2[this.targetVertex] * -num;
						}
						else
						{
							this.anchorTarget = array[this.targetVertex] + array2[this.targetVertex] * -this._colliderRadius * this._scale;
						}
					}
				}
				else
				{
					this.anchorTarget = array[this.targetVertex];
				}
			}
		}
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x00147D14 File Offset: 0x00146114
	protected void UpdateAnchor()
	{
		if (this._softJointType == AutoCollider.SoftJointType.Direct && this.backForceRigidbody != null)
		{
			this.UpdateAnchorTarget();
			if (this.joint != null)
			{
				this.joint.connectedAnchor = this.backForceRigidbody.transform.InverseTransformPoint(this.anchorTarget);
			}
			if (this.springJoint != null)
			{
				this.springJoint.connectedAnchor = this.backForceRigidbody.transform.InverseTransformPoint(this.anchorTarget);
			}
		}
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x00147DA8 File Offset: 0x001461A8
	public void UpdateHardTransformPositionFast(Vector3[] verts, Vector3[] norms)
	{
		if (this._createHardCollider && this.hardPositionUpdateTrigger == AutoCollider.PositionUpdateTrigger.MorphChangeOnly && this.targetVertex != -1 && this.anchorVertex1 != -1)
		{
			switch (this._lookAtOption)
			{
			case AutoCollider.LookAtOption.VertexNormal:
				if (this._centerJoint)
				{
					if (this._colliderOrient == AutoCollider.ColliderOrient.Look)
					{
						float num = this._colliderLength * 0.5f;
						if (num < this._colliderRadius)
						{
							num = this._colliderRadius;
						}
						this.hardPositionTarget = verts[this.targetVertex] + norms[this.targetVertex] * -num;
					}
					else
					{
						this.hardPositionTarget = verts[this.targetVertex] + norms[this.targetVertex] * -this._colliderRadius;
					}
				}
				else
				{
					this.hardPositionTarget = verts[this.targetVertex];
				}
				break;
			case AutoCollider.LookAtOption.Anchor1:
				this.hardPositionTarget = verts[this.targetVertex];
				break;
			case AutoCollider.LookAtOption.Opposite:
				if (this.oppositeVertex != -1)
				{
					if (this._centerJoint)
					{
						this.hardPositionTarget = (verts[this.targetVertex] + verts[this.oppositeVertex]) * 0.5f;
					}
					else
					{
						this.hardPositionTarget = verts[this.targetVertex];
					}
				}
				break;
			case AutoCollider.LookAtOption.AnchorCenters:
				if (this.anchorVertex2 != -1)
				{
					Vector3 vector = (verts[this.anchorVertex1] + verts[this.anchorVertex2]) * 0.5f;
					if (this._centerJoint)
					{
						this.hardPositionTarget = vector;
					}
					else
					{
						this.hardPositionTarget = verts[this.targetVertex];
					}
				}
				break;
			case AutoCollider.LookAtOption.Reference:
				this.hardPositionTarget = verts[this.targetVertex];
				break;
			}
		}
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x00147FEC File Offset: 0x001463EC
	public void UpdateTransforms()
	{
		if (this.targetVertex != -1 && this.anchorVertex1 != -1)
		{
			Transform transform = null;
			bool flag = false;
			if (this._createSoftCollider && this._softJointType == AutoCollider.SoftJointType.Direct)
			{
				if (this.joint != null && !Application.isPlaying)
				{
					transform = this.jointTransform;
					this.joint.connectedBody = null;
				}
				if (this.springJoint != null && !Application.isPlaying)
				{
					transform = this.jointTransform;
					this.springJoint.connectedBody = null;
				}
			}
			else if (this.kinematicTransform != null)
			{
				transform = this.kinematicTransform;
			}
			else if (this.hardTransform != null && (this.hardPositionUpdateTrigger == AutoCollider.PositionUpdateTrigger.Always || (this.hardPositionUpdateTrigger == AutoCollider.PositionUpdateTrigger.MorphChangeOnly && this.morphsChanged) || !Application.isPlaying))
			{
				transform = this.hardTransform;
				if (this.hardParent != null)
				{
					flag = true;
				}
			}
			if (transform != null)
			{
				bool flag2 = true;
				Vector3[] array;
				Vector3[] array2;
				if (Application.isPlaying)
				{
					array = this._skin.rawSkinnedVerts;
					array2 = this._skin.postSkinNormals;
					flag2 = this._skin.postSkinVertsReady[this.targetVertex];
				}
				else
				{
					array = this._skin.dazMesh.morphedUVVertices;
					array2 = this._skin.dazMesh.morphedUVNormals;
				}
				if (flag2)
				{
					switch (this._lookAtOption)
					{
					case AutoCollider.LookAtOption.VertexNormal:
					{
						if (this._centerJoint)
						{
							if (flag)
							{
								if (this._colliderOrient == AutoCollider.ColliderOrient.Look)
								{
									float num = this._colliderLength * 0.5f;
									if (num < this._colliderRadius)
									{
										num = this._colliderRadius;
									}
									transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(array[this.targetVertex] + array2[this.targetVertex] * -num);
								}
								else
								{
									transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(array[this.targetVertex] + array2[this.targetVertex] * -this._colliderRadius);
								}
							}
							else if (this._colliderOrient == AutoCollider.ColliderOrient.Look)
							{
								float num2 = this._colliderLength * 0.5f;
								if (num2 < this._colliderRadius)
								{
									num2 = this._colliderRadius;
								}
								transform.position = array[this.targetVertex] + array2[this.targetVertex] * -num2;
							}
							else
							{
								transform.position = array[this.targetVertex] + array2[this.targetVertex] * -this._colliderRadius;
							}
						}
						else if (flag)
						{
							transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(array[this.targetVertex]);
						}
						else
						{
							transform.position = array[this.targetVertex];
						}
						Quaternion rotation;
						if (this._useAnchor2AsUp)
						{
							Vector3 upwards = Vector3.Cross(array2[this.targetVertex], array[this.anchorVertex2] - array[this.targetVertex]);
							rotation = Quaternion.LookRotation(-array2[this.targetVertex], upwards);
						}
						else
						{
							rotation = Quaternion.LookRotation(-array2[this.targetVertex], array[this.anchorVertex1] - array[this.targetVertex]);
						}
						transform.rotation = rotation;
						break;
					}
					case AutoCollider.LookAtOption.Anchor1:
					{
						if (flag)
						{
							transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(array[this.targetVertex]);
						}
						else
						{
							transform.position = array[this.targetVertex];
						}
						Quaternion rotation;
						if (this.oppositeVertex != -1)
						{
							rotation = Quaternion.LookRotation(array[this.anchorVertex1] - array[this.targetVertex], array[this.oppositeVertex] - array[this.targetVertex]);
						}
						else
						{
							rotation = Quaternion.LookRotation(array[this.anchorVertex1] - array[this.targetVertex], -array2[this.targetVertex]);
						}
						transform.rotation = rotation;
						break;
					}
					case AutoCollider.LookAtOption.Opposite:
						if (this.oppositeVertex != -1)
						{
							if (this._centerJoint)
							{
								if (flag)
								{
									transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4((array[this.targetVertex] + array[this.oppositeVertex]) * 0.5f);
								}
								else
								{
									transform.position = (array[this.targetVertex] + array[this.oppositeVertex]) * 0.5f;
								}
							}
							else if (flag)
							{
								transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(array[this.targetVertex]);
							}
							else
							{
								transform.position = array[this.targetVertex];
							}
							Quaternion rotation = Quaternion.LookRotation(array[this.oppositeVertex] - array[this.targetVertex], array[this.anchorVertex1] - array[this.targetVertex]);
							transform.rotation = rotation;
						}
						break;
					case AutoCollider.LookAtOption.AnchorCenters:
						if (this.anchorVertex2 != -1)
						{
							Vector3 vector = (array[this.anchorVertex1] + array[this.anchorVertex2]) * 0.5f;
							Quaternion rotation;
							if (this._centerJoint)
							{
								if (flag)
								{
									transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(vector);
								}
								else
								{
									transform.position = vector;
								}
								rotation = Quaternion.LookRotation(array[this.targetVertex] - vector, array[this.anchorVertex1] - vector);
							}
							else
							{
								if (flag)
								{
									transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(array[this.targetVertex]);
								}
								else
								{
									transform.position = array[this.targetVertex];
								}
								rotation = Quaternion.LookRotation(vector - array[this.targetVertex], array[this.anchorVertex1] - array[this.targetVertex]);
							}
							transform.rotation = rotation;
						}
						break;
					case AutoCollider.LookAtOption.Reference:
					{
						if (flag)
						{
							transform.localPosition = this.hardParent.morphedWorldToLocalMatrix.MultiplyPoint3x4(array[this.targetVertex]);
						}
						else
						{
							transform.position = array[this.targetVertex];
						}
						Quaternion rotation = Quaternion.LookRotation(this.reference.position - array[this.targetVertex], array[this.anchorVertex1] - array[this.targetVertex]);
						transform.rotation = rotation;
						break;
					}
					}
					if (this.hardTransform != null && transform != this.hardTransform && (this.hardPositionUpdateTrigger == AutoCollider.PositionUpdateTrigger.Always || (this.hardPositionUpdateTrigger == AutoCollider.PositionUpdateTrigger.MorphChangeOnly && this.morphsChanged) || !Application.isPlaying))
					{
						this.hardTransform.position = transform.position;
						this.hardTransform.rotation = transform.rotation;
					}
					if (this._softJointType == AutoCollider.SoftJointType.Direct && !Application.isPlaying)
					{
						if (this.joint != null)
						{
							this.joint.connectedBody = this.backForceRigidbody;
						}
						if (this.springJoint != null)
						{
							this.springJoint.connectedBody = this.backForceRigidbody;
						}
					}
				}
			}
		}
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x00148950 File Offset: 0x00146D50
	public void ResetJointPhysics()
	{
		if (this.joint != null)
		{
			this.jointTransform.localPosition = this.joint.connectedAnchor;
			this.jointTransform.localRotation = this.jointTransformInitialRotation;
		}
		else if (this.jointTransform != null)
		{
			this.jointTransform.position = this.anchorTarget;
			this.jointTransform.localRotation = this.jointTransformInitialRotation;
		}
		if (this.jointRB != null)
		{
			this.jointRB.velocity = Vector3.zero;
			this.jointRB.angularVelocity = Vector3.zero;
		}
		if (this.otherTransformToReset != null)
		{
			this.otherTransformToReset.localPosition = Vector3.zero;
			this.otherTransformToReset.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x00148A30 File Offset: 0x00146E30
	protected void ResetJointInternal()
	{
		if (this.wasInit)
		{
			if (this._softJointType == AutoCollider.SoftJointType.FloatingKinematic)
			{
				this.UpdateTransforms();
				if (this.jointTransform != null)
				{
					this.jointTransform.position = this.kinematicTransform.position;
					this.jointTransform.localRotation = this.jointTransformInitialRotation;
				}
			}
			else
			{
				this.UpdateAnchor();
				this.ResetJointPhysics();
			}
		}
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x00148AA2 File Offset: 0x00146EA2
	public void ResetBackForceTrigger()
	{
		this.applyBackForce = true;
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x00148AAB File Offset: 0x00146EAB
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		this.oneoverscale = 1f / this._scale;
		this.SetJointLimits();
		this.SetJointDrive();
		this.AutoColliderSizeSet(false);
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x00148ADC File Offset: 0x00146EDC
	public void DoUpdate()
	{
		if (this._on)
		{
			if (this.resetSimulation)
			{
				this.ResetJointInternal();
			}
			else
			{
				this.UpdateAnchor();
				this.UpdateTransforms();
				if (this.resizeTrigger == AutoCollider.ResizeTrigger.Always || this.morphsChanged)
				{
					if (this.debug)
					{
						Debug.Log("Morph changed - apply and resize");
					}
					this.morphsChanged = false;
					this.AutoColliderSizeSet(false);
				}
				if (this.jointTransform != null && this._softJointType == AutoCollider.SoftJointType.FloatingKinematic && this.backForceRigidbody != null && this._jointBackForce > 0f)
				{
					if (this.applyBackForce)
					{
						this.applyBackForce = false;
						Vector3 a = (this.jointTransform.position - this.kinematicTransform.position) * this.oneoverscale;
						float magnitude = a.magnitude;
						float num = magnitude - this._jointBackForceThresholdDistance;
						if (num > 0f)
						{
							float d = num / magnitude;
							a *= d;
							float d2;
							if (TimeControl.singleton != null && TimeControl.singleton.compensateFixedTimestep)
							{
								if (!Mathf.Approximately(Time.timeScale, 0f))
								{
									d2 = 1f / Time.timeScale;
								}
								else
								{
									d2 = 1f;
								}
							}
							else
							{
								d2 = 1f;
							}
							Vector3 vector = a * this._jointBackForce * d2;
							this._appliedBackForce = Vector3.ClampMagnitude(vector, this._jointBackForceMaxForce);
						}
						else
						{
							this._appliedBackForce.x = 0f;
							this._appliedBackForce.y = 0f;
							this._appliedBackForce.z = 0f;
						}
					}
					float num2 = Time.fixedDeltaTime * 90f;
					float num3 = 0.5f;
					this._bufferedBackForce = Vector3.Lerp(this._bufferedBackForce, this._appliedBackForce, num3 * num2);
					this.backForceRigidbody.AddForce(this._bufferedBackForce, ForceMode.Force);
				}
			}
		}
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x00148CE8 File Offset: 0x001470E8
	public void AutoColliderSizeSetFast(Vector3[] verts)
	{
		if (this.targetVertex != -1 && this.anchorVertex1 != -1 && (this._useAutoRadius || this._useAutoLength))
		{
			this.oneoverscale = 1f;
			bool flag = false;
			if (verts[this.targetVertex] != this.currentTargetVertexPosition)
			{
				flag = true;
			}
			else if (verts[this.anchorVertex1] != this.currentAnchorVertex1Position)
			{
				flag = true;
			}
			else if (this.anchorVertex2 != -1 && verts[this.anchorVertex2] != this.currentAnchorVertex2Position)
			{
				flag = true;
			}
			else if (this.oppositeVertex != -1 && verts[this.oppositeVertex] != this.currentOppositePosition)
			{
				flag = true;
			}
			if (flag)
			{
				if (this.debug)
				{
					Debug.Log("Verts Changed");
				}
				this.currentTargetVertexPosition = verts[this.targetVertex];
				this.currentAnchorVertex1Position = verts[this.anchorVertex1];
				if (this.anchorVertex2 != -1)
				{
					this.currentAnchorVertex2Position = verts[this.anchorVertex2];
				}
				if (this.oppositeVertex != -1)
				{
					this.currentOppositePosition = verts[this.oppositeVertex];
				}
				if (this.lookAtOption == AutoCollider.LookAtOption.VertexNormal)
				{
					AutoCollider.ColliderOrient colliderOrient = this._colliderOrient;
					if (colliderOrient != AutoCollider.ColliderOrient.Look)
					{
						if (colliderOrient != AutoCollider.ColliderOrient.Up)
						{
							if (colliderOrient == AutoCollider.ColliderOrient.Right)
							{
								if (this._useAutoLength && this.anchorVertex2 != -1)
								{
									float num = (verts[this.anchorVertex2] - verts[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
									if (Mathf.Abs(num - this._colliderLength) >= 0.001f)
									{
										this._colliderLength = num;
										this.colliderDirty = true;
									}
								}
								if (this._useAutoRadius)
								{
									float num2 = ((verts[this.anchorVertex1] - verts[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
									if (Mathf.Abs(num2 - this._colliderRadius) >= 0.001f)
									{
										this._colliderRadius = num2;
										this.colliderDirty = true;
									}
								}
							}
						}
						else
						{
							if (this._useAutoLength)
							{
								float num3 = (verts[this.anchorVertex1] - verts[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
								if (Mathf.Abs(num3 - this._colliderLength) >= 0.001f)
								{
									this._colliderLength = num3;
									this.colliderDirty = true;
								}
							}
							if (this._useAutoRadius && this.anchorVertex2 != -1)
							{
								float num4 = ((verts[this.anchorVertex2] - verts[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								if (Mathf.Abs(num4 - this._colliderRadius) >= 0.001f)
								{
									this._colliderRadius = num4;
									this.colliderDirty = true;
								}
							}
						}
					}
					else if (this._useAutoRadius)
					{
						float num5 = ((verts[this.anchorVertex1] - verts[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
						if (Mathf.Abs(num5 - this._colliderRadius) >= 0.001f)
						{
							this._colliderRadius = num5;
							this.colliderDirty = true;
						}
					}
				}
				else if (this.lookAtOption == AutoCollider.LookAtOption.Opposite)
				{
					AutoCollider.ColliderOrient colliderOrient2 = this._colliderOrient;
					if (colliderOrient2 != AutoCollider.ColliderOrient.Look)
					{
						if (colliderOrient2 == AutoCollider.ColliderOrient.Up)
						{
							if (this._useAutoLength)
							{
								if (this.anchorVertex2 != -1)
								{
									float num6 = (verts[this.anchorVertex2] - verts[this.anchorVertex1]).magnitude * this.oneoverscale - this._autoLengthBuffer;
									if (Mathf.Abs(num6 - this._colliderLength) >= 0.001f)
									{
										this._colliderLength = num6;
										this.colliderDirty = true;
									}
								}
								else
								{
									float num7 = (verts[this.anchorVertex1] - verts[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
									if (Mathf.Abs(num7 - this._colliderLength) >= 0.001f)
									{
										this._colliderLength = num7;
										this.colliderDirty = true;
									}
								}
							}
							if (this._useAutoRadius && this.oppositeVertex != -1)
							{
								float num8 = ((verts[this.oppositeVertex] - verts[this.targetVertex]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								if (Mathf.Abs(num8 - this._colliderRadius) >= 0.001f)
								{
									this._colliderRadius = num8;
									this.colliderDirty = true;
								}
							}
						}
					}
					else
					{
						if (this._useAutoLength && this.oppositeVertex != -1)
						{
							float num9 = (verts[this.oppositeVertex] - verts[this.targetVertex]).magnitude * this.oneoverscale - this._autoLengthBuffer;
							if (Mathf.Abs(num9 - this._colliderLength) >= 0.001f)
							{
								this._colliderLength = num9;
								this.colliderDirty = true;
							}
						}
						if (this._useAutoRadius)
						{
							if (this.anchorVertex2 != -1)
							{
								float num10 = ((verts[this.anchorVertex2] - verts[this.anchorVertex1]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								if (Mathf.Abs(num10 - this._colliderRadius) >= 0.001f)
								{
									this._colliderRadius = num10;
									this.colliderDirty = true;
								}
							}
							else
							{
								float num11 = ((verts[this.anchorVertex1] - verts[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								if (Mathf.Abs(num11 - this._colliderRadius) >= 0.001f)
								{
									this._colliderRadius = num11;
									this.colliderDirty = true;
								}
							}
						}
					}
				}
				else if (this.lookAtOption == AutoCollider.LookAtOption.Anchor1)
				{
					AutoCollider.ColliderOrient colliderOrient3 = this._colliderOrient;
					if (colliderOrient3 == AutoCollider.ColliderOrient.Look)
					{
						if (this._useAutoLength)
						{
							float num12 = (verts[this.anchorVertex1] - verts[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
							if (Mathf.Abs(num12 - this._colliderLength) >= 0.001f)
							{
								this._colliderLength = num12;
								this.colliderDirty = true;
							}
						}
						if (this._useAutoRadius)
						{
							if (this.oppositeVertex != -1)
							{
								float num13 = ((verts[this.oppositeVertex] - verts[this.targetVertex]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								if (Mathf.Abs(num13 - this._colliderRadius) >= 0.001f)
								{
									this._colliderRadius = num13;
									this.colliderDirty = true;
								}
							}
							else if (this.anchorVertex2 != -1)
							{
								float num14 = ((verts[this.anchorVertex2] - verts[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								if (Mathf.Abs(num14 - this._colliderRadius) >= 0.001f)
								{
									this._colliderRadius = num14;
									this.colliderDirty = true;
								}
							}
						}
					}
				}
				else if (this.lookAtOption == AutoCollider.LookAtOption.AnchorCenters && this.anchorVertex2 != -1)
				{
					AutoCollider.ColliderOrient colliderOrient4 = this._colliderOrient;
					if (colliderOrient4 != AutoCollider.ColliderOrient.Look)
					{
						if (colliderOrient4 != AutoCollider.ColliderOrient.Up)
						{
							if (colliderOrient4 == AutoCollider.ColliderOrient.Right)
							{
								if (this._useAutoLength && this.oppositeVertex != -1)
								{
									float num15 = (verts[this.oppositeVertex] - verts[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
									if (Mathf.Abs(num15 - this._colliderLength) >= 0.001f)
									{
										this._colliderLength = num15;
										this.colliderDirty = true;
									}
								}
								if (this._useAutoRadius)
								{
									float num16 = ((verts[this.anchorVertex2] - verts[this.anchorVertex1]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
									if (Mathf.Abs(num16 - this._colliderRadius) >= 0.001f)
									{
										this._colliderRadius = num16;
										this.colliderDirty = true;
									}
								}
							}
						}
						else
						{
							if (this._useAutoLength)
							{
								float num17 = (verts[this.anchorVertex2] - verts[this.anchorVertex1]).magnitude * this.oneoverscale - this._autoLengthBuffer;
								if (Mathf.Abs(num17 - this._colliderLength) >= 0.001f)
								{
									this._colliderLength = num17;
									this.colliderDirty = true;
								}
							}
							if (this._useAutoRadius)
							{
								Vector3 a = (verts[this.anchorVertex2] + verts[this.anchorVertex1]) * 0.5f;
								float num18 = ((a - verts[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								if (Mathf.Abs(num18 - this._colliderRadius) >= 0.001f)
								{
									this._colliderRadius = num18;
									this.colliderDirty = true;
								}
							}
						}
					}
					else
					{
						if (this._useAutoLength)
						{
							Vector3 a2 = (verts[this.anchorVertex2] + verts[this.anchorVertex1]) * 0.5f;
							float num19 = (a2 - verts[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
							if (Mathf.Abs(num19 - this._colliderLength) >= 0.001f)
							{
								this._colliderLength = num19;
								this.colliderDirty = true;
							}
						}
						if (this._useAutoRadius)
						{
							float num20 = ((verts[this.anchorVertex2] - verts[this.anchorVertex1]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							if (Mathf.Abs(num20 - this._colliderRadius) >= 0.001f)
							{
								this._colliderRadius = num20;
								this.colliderDirty = true;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x00149974 File Offset: 0x00147D74
	public void AutoColliderSizeSetFinishFast()
	{
		this.colliderDirty = false;
		this.SetColliders();
		if (this.hardCollider != null && this.hardParent != null)
		{
			Matrix4x4 morphedWorldToLocalMatrix = this.hardParent.morphedWorldToLocalMatrix;
			this.hardCollider.transform.localPosition = morphedWorldToLocalMatrix.MultiplyPoint3x4(this.hardPositionTarget);
		}
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x001499DC File Offset: 0x00147DDC
	public void AutoColliderSizeSet(bool force = false)
	{
		if (this.targetVertex != -1 && this.anchorVertex1 != -1 && this._skin != null && (this._useAutoRadius || this._useAutoLength))
		{
			bool flag = force;
			Vector3[] array;
			Vector3[] array2;
			if (Application.isPlaying)
			{
				if (this.resizeTrigger == AutoCollider.ResizeTrigger.Always)
				{
					array = this._skin.rawSkinnedVerts;
					array2 = this._skin.postSkinNormals;
				}
				else
				{
					array = this._skin.dazMesh.visibleMorphedUVVertices;
					array2 = this._skin.dazMesh.morphedUVNormals;
					this.oneoverscale = 1f;
				}
			}
			else
			{
				array = this._skin.dazMesh.morphedUVVertices;
				array2 = this._skin.dazMesh.morphedUVNormals;
				flag = true;
				this.oneoverscale = 1f;
			}
			if (array[this.targetVertex] != this.currentTargetVertexPosition)
			{
				flag = true;
			}
			else if (array[this.anchorVertex1] != this.currentAnchorVertex1Position)
			{
				flag = true;
			}
			else if (this.anchorVertex2 != -1 && array[this.anchorVertex2] != this.currentAnchorVertex2Position)
			{
				flag = true;
			}
			else if (this.oppositeVertex != -1 && array[this.oppositeVertex] != this.currentOppositePosition)
			{
				flag = true;
			}
			if (flag)
			{
				this.currentTargetVertexPosition = array[this.targetVertex];
				this.currentAnchorVertex1Position = array[this.anchorVertex1];
				if (this.anchorVertex2 != -1)
				{
					this.currentAnchorVertex2Position = array[this.anchorVertex2];
				}
				if (this.oppositeVertex != -1)
				{
					this.currentOppositePosition = array[this.oppositeVertex];
				}
				if (this.lookAtOption == AutoCollider.LookAtOption.VertexNormal)
				{
					AutoCollider.ColliderOrient colliderOrient = this._colliderOrient;
					if (colliderOrient != AutoCollider.ColliderOrient.Look)
					{
						if (colliderOrient != AutoCollider.ColliderOrient.Up)
						{
							if (colliderOrient == AutoCollider.ColliderOrient.Right)
							{
								if (this._useAutoLength && this.anchorVertex2 != -1)
								{
									this._colliderLength = (array[this.anchorVertex2] - array[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
								}
								if (this._useAutoRadius)
								{
									this._colliderRadius = ((array[this.anchorVertex1] - array[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								}
							}
						}
						else
						{
							if (this._useAutoLength)
							{
								this._colliderLength = (array[this.anchorVertex1] - array[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
							}
							if (this._useAutoRadius && this.anchorVertex2 != -1)
							{
								this._colliderRadius = ((array[this.anchorVertex2] - array[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							}
						}
					}
					else if (this._useAutoRadius)
					{
						this._colliderRadius = ((array[this.anchorVertex1] - array[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
					}
				}
				else if (this.lookAtOption == AutoCollider.LookAtOption.Opposite)
				{
					AutoCollider.ColliderOrient colliderOrient2 = this._colliderOrient;
					if (colliderOrient2 != AutoCollider.ColliderOrient.Look)
					{
						if (colliderOrient2 == AutoCollider.ColliderOrient.Up)
						{
							if (this._useAutoLength)
							{
								if (this.anchorVertex2 != -1)
								{
									this._colliderLength = (array[this.anchorVertex2] - array[this.anchorVertex1]).magnitude * this.oneoverscale - this._autoLengthBuffer;
								}
								else
								{
									this._colliderLength = (array[this.anchorVertex1] - array[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
								}
							}
							if (this._useAutoRadius && this.oppositeVertex != -1)
							{
								this._colliderRadius = ((array[this.oppositeVertex] - array[this.targetVertex]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							}
						}
					}
					else
					{
						if (this._useAutoLength && this.oppositeVertex != -1)
						{
							this._colliderLength = (array[this.oppositeVertex] - array[this.targetVertex]).magnitude * this.oneoverscale - this._autoLengthBuffer;
						}
						if (this._useAutoRadius)
						{
							if (this.anchorVertex2 != -1)
							{
								this._colliderRadius = ((array[this.anchorVertex2] - array[this.anchorVertex1]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							}
							else
							{
								this._colliderRadius = ((array[this.anchorVertex1] - array[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							}
						}
					}
				}
				else if (this.lookAtOption == AutoCollider.LookAtOption.Anchor1)
				{
					AutoCollider.ColliderOrient colliderOrient3 = this._colliderOrient;
					if (colliderOrient3 == AutoCollider.ColliderOrient.Look)
					{
						if (this._useAutoLength)
						{
							this._colliderLength = (array[this.anchorVertex1] - array[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
						}
						if (this._useAutoRadius)
						{
							if (this.oppositeVertex != -1)
							{
								this._colliderRadius = ((array[this.oppositeVertex] - array[this.targetVertex]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							}
							else if (this.anchorVertex2 != -1)
							{
								this._colliderRadius = ((array[this.anchorVertex2] - array[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							}
						}
					}
				}
				else if (this.lookAtOption == AutoCollider.LookAtOption.AnchorCenters && this.anchorVertex2 != -1)
				{
					AutoCollider.ColliderOrient colliderOrient4 = this._colliderOrient;
					if (colliderOrient4 != AutoCollider.ColliderOrient.Look)
					{
						if (colliderOrient4 != AutoCollider.ColliderOrient.Up)
						{
							if (colliderOrient4 == AutoCollider.ColliderOrient.Right)
							{
								if (this._useAutoLength && this.oppositeVertex != -1)
								{
									this._colliderLength = (array[this.oppositeVertex] - array[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
								}
								if (this._useAutoRadius)
								{
									this._colliderRadius = ((array[this.anchorVertex2] - array[this.anchorVertex1]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
								}
							}
						}
						else
						{
							if (this._useAutoLength)
							{
								this._colliderLength = (array[this.anchorVertex2] - array[this.anchorVertex1]).magnitude * this.oneoverscale - this._autoLengthBuffer;
							}
							if (this._useAutoRadius)
							{
								Vector3 a = (array[this.anchorVertex2] + array[this.anchorVertex1]) * 0.5f;
								this._colliderRadius = ((a - array[this.targetVertex]).magnitude * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
							}
						}
					}
					else
					{
						if (this._useAutoLength)
						{
							Vector3 a2 = (array[this.anchorVertex2] + array[this.anchorVertex1]) * 0.5f;
							this._colliderLength = (a2 - array[this.targetVertex]).magnitude * 2f * this.oneoverscale - this._autoLengthBuffer;
						}
						if (this._useAutoRadius)
						{
							this._colliderRadius = ((array[this.anchorVertex2] - array[this.anchorVertex1]).magnitude * 0.5f * this.oneoverscale - this._autoRadiusBuffer) * this._autoRadiusMultiplier;
						}
					}
				}
				this.SetColliders();
				if (!Application.isPlaying)
				{
					this.UpdateTransforms();
					if (this.showUsedVerts && this.oppositeVertex != -1)
					{
						Vector3 lhs = array[this.oppositeVertex] - array[this.targetVertex];
						if (this.lookAtOption == AutoCollider.LookAtOption.VertexNormal)
						{
							float d = Vector3.Dot(lhs, array2[this.targetVertex]);
							Debug.DrawLine(array[this.targetVertex], array[this.targetVertex] + array2[this.targetVertex] * d, Color.yellow);
						}
						else
						{
							Debug.DrawLine(array[this.targetVertex], array[this.oppositeVertex], Color.yellow);
						}
					}
				}
			}
		}
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x0014A528 File Offset: 0x00148928
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			this.InitColliders();
			this.SetInterpolation();
			if (this._skin != null && this._skin.dazMesh != null)
			{
				this._skin.Init();
			}
		}
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x0014A57D File Offset: 0x0014897D
	private void Start()
	{
		this.Init();
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x0014A588 File Offset: 0x00148988
	protected override void Update()
	{
		base.Update();
		this.applyBackForce = true;
		if (Application.isPlaying)
		{
			if (this.resizeTrigger == AutoCollider.ResizeTrigger.MorphChangeOnly && (this._skin.dazMesh.visibleNonPoseVerticesChangedThisFrame || this._skin.dazMesh.visibleNonPoseVerticesChangedLastFrame))
			{
				if (this.debug)
				{
					Debug.Log("Morphs changed - trigger");
				}
				this.morphsChanged = true;
			}
			if (this.morphVertex && this.targetVertex != -1 && this.jointTransform != null)
			{
				Vector3 vector = this.jointTransform.position - this.anchorTarget;
				this._skin.postSkinMorphs[this.targetVertex] = vector;
			}
		}
		if ((this.debug || !Application.isPlaying) && this.showUsedVerts && this._skin != null && this._skin.dazMesh != null)
		{
			Vector3[] array;
			if (Application.isPlaying)
			{
				array = this._skin.rawSkinnedVerts;
			}
			else
			{
				array = this._skin.dazMesh.morphedUVVertices;
			}
			if (this.targetVertex != -1)
			{
				MyDebug.DrawWireCube(array[this.targetVertex], 0.002f, Color.green);
			}
			if (this.anchorVertex1 != -1)
			{
				MyDebug.DrawWireCube(array[this.anchorVertex1], 0.001f, Color.blue);
			}
			if (this.anchorVertex2 != -1)
			{
				MyDebug.DrawWireCube(array[this.anchorVertex2], 0.001f, Color.cyan);
			}
			if (this.oppositeVertex != -1)
			{
				MyDebug.DrawWireCube(array[this.oppositeVertex], 0.001f, Color.yellow);
			}
			if (this.debug)
			{
				array = this._skin.dazMesh.morphedUVVertices;
				if (this.targetVertex != -1)
				{
					MyDebug.DrawWireCube(array[this.targetVertex], 0.002f, Color.green);
				}
				if (this.anchorVertex1 != -1)
				{
					MyDebug.DrawWireCube(array[this.anchorVertex1], 0.001f, Color.blue);
				}
				if (this.anchorVertex2 != -1)
				{
					MyDebug.DrawWireCube(array[this.anchorVertex2], 0.001f, Color.cyan);
				}
				if (this.oppositeVertex != -1)
				{
					MyDebug.DrawWireCube(array[this.oppositeVertex], 0.001f, Color.yellow);
				}
			}
		}
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x0014A847 File Offset: 0x00148C47
	private void FixedUpdate()
	{
		if (this.wasInit)
		{
			if (this._globalOn != AutoCollider.globalEnable)
			{
				this.SyncOn();
			}
			this.DoUpdate();
		}
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x0014A870 File Offset: 0x00148C70
	// Note: this type is marked as 'beforefieldinit'.
	static AutoCollider()
	{
	}

	// Token: 0x040033DB RID: 13275
	public static bool globalEnable = true;

	// Token: 0x040033DC RID: 13276
	public bool ignoreGroupSettings;

	// Token: 0x040033DD RID: 13277
	public bool skipResetOnPause;

	// Token: 0x040033DE RID: 13278
	public int targetVertex = -1;

	// Token: 0x040033DF RID: 13279
	public int anchorVertex1 = -1;

	// Token: 0x040033E0 RID: 13280
	public int anchorVertex2 = -1;

	// Token: 0x040033E1 RID: 13281
	public int oppositeVertex = -1;

	// Token: 0x040033E2 RID: 13282
	public Transform reference;

	// Token: 0x040033E3 RID: 13283
	public Transform otherTransformToReset;

	// Token: 0x040033E4 RID: 13284
	public Transform kinematicTransform;

	// Token: 0x040033E5 RID: 13285
	public Rigidbody kinematicRB;

	// Token: 0x040033E6 RID: 13286
	public Transform jointTransform;

	// Token: 0x040033E7 RID: 13287
	public Rigidbody jointRB;

	// Token: 0x040033E8 RID: 13288
	public SpringJoint springJoint;

	// Token: 0x040033E9 RID: 13289
	public ConfigurableJoint joint;

	// Token: 0x040033EA RID: 13290
	public Collider jointCollider;

	// Token: 0x040033EB RID: 13291
	public Transform hardTransform;

	// Token: 0x040033EC RID: 13292
	public Collider hardCollider;

	// Token: 0x040033ED RID: 13293
	public bool debug;

	// Token: 0x040033EE RID: 13294
	public bool morphVertex;

	// Token: 0x040033EF RID: 13295
	public bool allowBatchUpdate = true;

	// Token: 0x040033F0 RID: 13296
	protected bool jointTransformInitialRotationWasInit;

	// Token: 0x040033F1 RID: 13297
	public Quaternion jointTransformInitialRotation;

	// Token: 0x040033F2 RID: 13298
	protected Vector3 currentTargetVertexPosition;

	// Token: 0x040033F3 RID: 13299
	protected Vector3 currentAnchorVertex1Position;

	// Token: 0x040033F4 RID: 13300
	protected Vector3 currentAnchorVertex2Position;

	// Token: 0x040033F5 RID: 13301
	protected Vector3 currentOppositePosition;

	// Token: 0x040033F6 RID: 13302
	protected bool _globalOn;

	// Token: 0x040033F7 RID: 13303
	protected bool _on = true;

	// Token: 0x040033F8 RID: 13304
	[SerializeField]
	protected bool _createSoftCollider = true;

	// Token: 0x040033F9 RID: 13305
	[SerializeField]
	protected AutoCollider.JointType _jointType;

	// Token: 0x040033FA RID: 13306
	[SerializeField]
	protected AutoCollider.SoftJointType _softJointType;

	// Token: 0x040033FB RID: 13307
	[SerializeField]
	protected bool _createHardCollider = true;

	// Token: 0x040033FC RID: 13308
	[SerializeField]
	protected float _softJointLimit;

	// Token: 0x040033FD RID: 13309
	[SerializeField]
	protected float _softJointLimitSpring;

	// Token: 0x040033FE RID: 13310
	[SerializeField]
	protected float _softJointLimitDamper;

	// Token: 0x040033FF RID: 13311
	[SerializeField]
	private float _jointSpringLook = 1000f;

	// Token: 0x04003400 RID: 13312
	[SerializeField]
	private float _jointDamperLook = 100f;

	// Token: 0x04003401 RID: 13313
	[SerializeField]
	private float _jointSpringUp = 1000f;

	// Token: 0x04003402 RID: 13314
	[SerializeField]
	private float _jointDamperUp = 100f;

	// Token: 0x04003403 RID: 13315
	[SerializeField]
	private float _jointSpringRight = 1000f;

	// Token: 0x04003404 RID: 13316
	[SerializeField]
	private float _jointDamperRight = 100f;

	// Token: 0x04003405 RID: 13317
	[SerializeField]
	private float _jointSpringMaxForce = 1E+18f;

	// Token: 0x04003406 RID: 13318
	[SerializeField]
	private float _jointMass = 0.1f;

	// Token: 0x04003407 RID: 13319
	[SerializeField]
	private AutoCollider.ColliderOrient _colliderOrient;

	// Token: 0x04003408 RID: 13320
	[SerializeField]
	private AutoCollider.ColliderType _colliderType;

	// Token: 0x04003409 RID: 13321
	[SerializeField]
	private float _colliderRadius = 0.003f;

	// Token: 0x0400340A RID: 13322
	[SerializeField]
	private float _colliderLength = 0.003f;

	// Token: 0x0400340B RID: 13323
	public AutoCollider.ResizeTrigger resizeTrigger;

	// Token: 0x0400340C RID: 13324
	[SerializeField]
	private bool _useAutoRadius;

	// Token: 0x0400340D RID: 13325
	[SerializeField]
	private bool _useAutoLength;

	// Token: 0x0400340E RID: 13326
	[SerializeField]
	private AutoCollider.LookAtOption _lookAtOption;

	// Token: 0x0400340F RID: 13327
	[SerializeField]
	private bool _useAnchor2AsUp;

	// Token: 0x04003410 RID: 13328
	[SerializeField]
	private float _autoRadiusMultiplier = 1f;

	// Token: 0x04003411 RID: 13329
	[SerializeField]
	private float _autoRadiusBuffer = 0.001f;

	// Token: 0x04003412 RID: 13330
	[SerializeField]
	private float _autoLengthBuffer = 0.001f;

	// Token: 0x04003413 RID: 13331
	[SerializeField]
	protected bool _centerJoint;

	// Token: 0x04003414 RID: 13332
	[SerializeField]
	private float _colliderLookOffset;

	// Token: 0x04003415 RID: 13333
	[SerializeField]
	private float _colliderUpOffset;

	// Token: 0x04003416 RID: 13334
	[SerializeField]
	private float _colliderRightOffset;

	// Token: 0x04003417 RID: 13335
	[SerializeField]
	private string _colliderLayer;

	// Token: 0x04003418 RID: 13336
	public AutoCollider.PositionUpdateTrigger hardPositionUpdateTrigger;

	// Token: 0x04003419 RID: 13337
	public bool updateHardColliderSize;

	// Token: 0x0400341A RID: 13338
	[SerializeField]
	protected float _hardColliderBuffer = 0.01f;

	// Token: 0x0400341B RID: 13339
	[SerializeField]
	private Transform[] _ignoreColliders;

	// Token: 0x0400341C RID: 13340
	[SerializeField]
	private PhysicMaterial _colliderMaterial;

	// Token: 0x0400341D RID: 13341
	public Rigidbody backForceRigidbody;

	// Token: 0x0400341E RID: 13342
	[SerializeField]
	private float _jointBackForce = 1000f;

	// Token: 0x0400341F RID: 13343
	[SerializeField]
	private float _jointBackForceThresholdDistance = 0.001f;

	// Token: 0x04003420 RID: 13344
	[SerializeField]
	private float _jointBackForceMaxForce = 100f;

	// Token: 0x04003421 RID: 13345
	[SerializeField]
	protected Transform _skinTransform;

	// Token: 0x04003422 RID: 13346
	[SerializeField]
	protected DAZSkinV2 _skin;

	// Token: 0x04003423 RID: 13347
	public FreeControllerV3 controller;

	// Token: 0x04003424 RID: 13348
	public bool showHandles = true;

	// Token: 0x04003425 RID: 13349
	public bool showBackfaceHandles;

	// Token: 0x04003426 RID: 13350
	[SerializeField]
	protected bool _showUsedVerts = true;

	// Token: 0x04003427 RID: 13351
	public float handleSize = 0.001f;

	// Token: 0x04003428 RID: 13352
	public int subMeshSelection = -1;

	// Token: 0x04003429 RID: 13353
	public int subMeshSelection2 = -1;

	// Token: 0x0400342A RID: 13354
	protected Dictionary<int, int> _uvVertToBaseVertDict;

	// Token: 0x0400342B RID: 13355
	private bool wasInit;

	// Token: 0x0400342C RID: 13356
	protected Vector3 _bufferedBackForce;

	// Token: 0x0400342D RID: 13357
	protected Vector3 _appliedBackForce;

	// Token: 0x0400342E RID: 13358
	public DAZBone bone;

	// Token: 0x0400342F RID: 13359
	public Vector3 anchorTarget;

	// Token: 0x04003430 RID: 13360
	public Vector3 transformedAnchorTarget;

	// Token: 0x04003431 RID: 13361
	public bool transformedAnchorTargetDirty;

	// Token: 0x04003432 RID: 13362
	protected Vector3 hardPositionTarget;

	// Token: 0x04003433 RID: 13363
	public DAZBone hardParent;

	// Token: 0x04003434 RID: 13364
	protected bool applyBackForce;

	// Token: 0x04003435 RID: 13365
	protected float oneoverscale = 1f;

	// Token: 0x04003436 RID: 13366
	public bool colliderDirty;

	// Token: 0x04003437 RID: 13367
	protected const float sizeAdjustThreshold = 0.001f;

	// Token: 0x04003438 RID: 13368
	protected bool morphsChanged;

	// Token: 0x02000A9C RID: 2716
	public enum JointType
	{
		// Token: 0x0400343A RID: 13370
		Configurable,
		// Token: 0x0400343B RID: 13371
		Spring
	}

	// Token: 0x02000A9D RID: 2717
	public enum SoftJointType
	{
		// Token: 0x0400343D RID: 13373
		FloatingKinematic,
		// Token: 0x0400343E RID: 13374
		Direct
	}

	// Token: 0x02000A9E RID: 2718
	public enum ColliderOrient
	{
		// Token: 0x04003440 RID: 13376
		Look,
		// Token: 0x04003441 RID: 13377
		Up,
		// Token: 0x04003442 RID: 13378
		Right
	}

	// Token: 0x02000A9F RID: 2719
	public enum ColliderType
	{
		// Token: 0x04003444 RID: 13380
		Capsule,
		// Token: 0x04003445 RID: 13381
		Sphere,
		// Token: 0x04003446 RID: 13382
		Box
	}

	// Token: 0x02000AA0 RID: 2720
	public enum ResizeTrigger
	{
		// Token: 0x04003448 RID: 13384
		MorphChangeOnly,
		// Token: 0x04003449 RID: 13385
		None,
		// Token: 0x0400344A RID: 13386
		Always
	}

	// Token: 0x02000AA1 RID: 2721
	public enum LookAtOption
	{
		// Token: 0x0400344C RID: 13388
		VertexNormal,
		// Token: 0x0400344D RID: 13389
		Anchor1,
		// Token: 0x0400344E RID: 13390
		Opposite,
		// Token: 0x0400344F RID: 13391
		AnchorCenters,
		// Token: 0x04003450 RID: 13392
		Reference
	}

	// Token: 0x02000AA2 RID: 2722
	public enum PositionUpdateTrigger
	{
		// Token: 0x04003452 RID: 13394
		MorphChangeOnly,
		// Token: 0x04003453 RID: 13395
		None,
		// Token: 0x04003454 RID: 13396
		Always
	}
}
