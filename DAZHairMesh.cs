using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Token: 0x02000AD6 RID: 2774
public class DAZHairMesh : MonoBehaviour
{
	// Token: 0x060049AD RID: 18861 RVA: 0x0017AF78 File Offset: 0x00179378
	public DAZHairMesh()
	{
	}

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x060049AE RID: 18862 RVA: 0x0017B0B0 File Offset: 0x001794B0
	public bool needsInit
	{
		get
		{
			if (this._bundleTypeLive != this._bundleType)
			{
				return true;
			}
			if (this._hairDrawTypeLive != this._hairDrawType)
			{
				return true;
			}
			if (this._numberSegmentsLive != this._numberSegments)
			{
				return true;
			}
			if (this._numSubHairsMaxLive != this._numSubHairsMax)
			{
				return true;
			}
			if (this._numSubHairsMinLive != this._numSubHairsMin)
			{
				return true;
			}
			if (this._scalpSelectionLive != this._scalpSelection)
			{
				return true;
			}
			if (this._subHairNormalOffsetBendLive != this._subHairNormalOffsetBend)
			{
				return true;
			}
			if (this._subHairTangent1OffsetMaxLive != this._subHairTangent1OffsetMax)
			{
				return true;
			}
			if (this._subHairTangent2OffsetMaxLive != this._subHairTangent2OffsetMax)
			{
				return true;
			}
			if (this._createTangentsLive != this._createTangents)
			{
				return true;
			}
			if (this.colliderLayers.Length != this._colliderLayers.Length)
			{
				return true;
			}
			for (int i = 0; i < this.colliderLayers.Length; i++)
			{
				if (this.colliderLayers[i] != this._colliderLayers[i])
				{
					return true;
				}
			}
			return this._scalpSelection != null && this._scalpSelection.changed;
		}
	}

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x060049AF RID: 18863 RVA: 0x0017B1F0 File Offset: 0x001795F0
	// (set) Token: 0x060049B0 RID: 18864 RVA: 0x0017B1F8 File Offset: 0x001795F8
	public DAZSkinV2MeshSelection scalpSelection
	{
		get
		{
			return this._scalpSelectionLive;
		}
		set
		{
			this._scalpSelectionLive = value;
		}
	}

	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x060049B1 RID: 18865 RVA: 0x0017B201 File Offset: 0x00179601
	public DAZSkinV2MeshSelection scalpSelectionActive
	{
		get
		{
			return this._scalpSelection;
		}
	}

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x060049B2 RID: 18866 RVA: 0x0017B209 File Offset: 0x00179609
	// (set) Token: 0x060049B3 RID: 18867 RVA: 0x0017B211 File Offset: 0x00179611
	public HairStripV2.HairDrawType hairDrawType
	{
		get
		{
			return this._hairDrawTypeLive;
		}
		set
		{
			this._hairDrawTypeLive = value;
		}
	}

	// Token: 0x17000A46 RID: 2630
	// (get) Token: 0x060049B4 RID: 18868 RVA: 0x0017B21A File Offset: 0x0017961A
	public HairStripV2.HairDrawType hairDrawTypeActive
	{
		get
		{
			return this._hairDrawType;
		}
	}

	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x060049B5 RID: 18869 RVA: 0x0017B222 File Offset: 0x00179622
	// (set) Token: 0x060049B6 RID: 18870 RVA: 0x0017B22A File Offset: 0x0017962A
	public int numberSegments
	{
		get
		{
			return this._numberSegmentsLive;
		}
		set
		{
			if (this._hairDrawTypeLive == HairStripV2.HairDrawType.GPULines)
			{
				this._numberSegmentsLive = value;
			}
			else
			{
				this._numberSegmentsLive = value;
			}
		}
	}

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x060049B7 RID: 18871 RVA: 0x0017B24B File Offset: 0x0017964B
	public int numberSegmentsActive
	{
		get
		{
			return this._numberSegments;
		}
	}

	// Token: 0x17000A49 RID: 2633
	// (get) Token: 0x060049B8 RID: 18872 RVA: 0x0017B253 File Offset: 0x00179653
	// (set) Token: 0x060049B9 RID: 18873 RVA: 0x0017B25B File Offset: 0x0017965B
	public int numSubHairsMin
	{
		get
		{
			return this._numSubHairsMinLive;
		}
		set
		{
			this._numSubHairsMinLive = value;
			if (this._numSubHairsMinLive > this._numSubHairsMaxLive)
			{
				this._numSubHairsMinLive = this._numSubHairsMaxLive;
			}
		}
	}

	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x060049BA RID: 18874 RVA: 0x0017B281 File Offset: 0x00179681
	public int numSubHairsMinActive
	{
		get
		{
			return this._numSubHairsMin;
		}
	}

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x060049BB RID: 18875 RVA: 0x0017B289 File Offset: 0x00179689
	// (set) Token: 0x060049BC RID: 18876 RVA: 0x0017B291 File Offset: 0x00179691
	public int numSubHairsMax
	{
		get
		{
			return this._numSubHairsMaxLive;
		}
		set
		{
			this._numSubHairsMaxLive = value;
			if (this._numSubHairsMaxLive < this._numSubHairsMinLive)
			{
				this._numSubHairsMaxLive = this._numSubHairsMinLive;
			}
		}
	}

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x060049BD RID: 18877 RVA: 0x0017B2B7 File Offset: 0x001796B7
	public int numSubHairsMaxActive
	{
		get
		{
			return this._numSubHairsMax;
		}
	}

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x060049BE RID: 18878 RVA: 0x0017B2BF File Offset: 0x001796BF
	// (set) Token: 0x060049BF RID: 18879 RVA: 0x0017B2C7 File Offset: 0x001796C7
	public HairStripV2.HairBundleType bundleType
	{
		get
		{
			return this._bundleTypeLive;
		}
		set
		{
			this._bundleTypeLive = value;
		}
	}

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x060049C0 RID: 18880 RVA: 0x0017B2D0 File Offset: 0x001796D0
	public HairStripV2.HairBundleType bundleTypeActive
	{
		get
		{
			return this._bundleType;
		}
	}

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x060049C1 RID: 18881 RVA: 0x0017B2D8 File Offset: 0x001796D8
	// (set) Token: 0x060049C2 RID: 18882 RVA: 0x0017B2E0 File Offset: 0x001796E0
	public float subHairTangent1OffsetMax
	{
		get
		{
			return this._subHairTangent1OffsetMaxLive;
		}
		set
		{
			this._subHairTangent1OffsetMaxLive = value;
		}
	}

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x060049C3 RID: 18883 RVA: 0x0017B2E9 File Offset: 0x001796E9
	public float subHairTangent1OffsetMaxActive
	{
		get
		{
			return this._subHairTangent1OffsetMax;
		}
	}

	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x060049C4 RID: 18884 RVA: 0x0017B2F1 File Offset: 0x001796F1
	// (set) Token: 0x060049C5 RID: 18885 RVA: 0x0017B2F9 File Offset: 0x001796F9
	public float subHairTangent2OffsetMax
	{
		get
		{
			return this._subHairTangent2OffsetMaxLive;
		}
		set
		{
			this._subHairTangent2OffsetMaxLive = value;
		}
	}

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x060049C6 RID: 18886 RVA: 0x0017B302 File Offset: 0x00179702
	public float subHairTangent2OffsetMaxActive
	{
		get
		{
			return this._subHairTangent2OffsetMax;
		}
	}

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x060049C7 RID: 18887 RVA: 0x0017B30A File Offset: 0x0017970A
	// (set) Token: 0x060049C8 RID: 18888 RVA: 0x0017B312 File Offset: 0x00179712
	public float subHairNormalOffsetBend
	{
		get
		{
			return this._subHairNormalOffsetBendLive;
		}
		set
		{
			this._subHairNormalOffsetBendLive = value;
		}
	}

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x060049C9 RID: 18889 RVA: 0x0017B31B File Offset: 0x0017971B
	public float subHairNormalOffsetBendActive
	{
		get
		{
			return this._subHairNormalOffsetBend;
		}
	}

	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x060049CA RID: 18890 RVA: 0x0017B323 File Offset: 0x00179723
	// (set) Token: 0x060049CB RID: 18891 RVA: 0x0017B32B File Offset: 0x0017972B
	public bool createTangents
	{
		get
		{
			return this._createTangentsLive;
		}
		set
		{
			this._createTangentsLive = value;
		}
	}

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x060049CC RID: 18892 RVA: 0x0017B334 File Offset: 0x00179734
	public bool createTangentsActive
	{
		get
		{
			return this._createTangents;
		}
	}

	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x060049CD RID: 18893 RVA: 0x0017B33C File Offset: 0x0017973C
	public int numHairs
	{
		get
		{
			return this._numHairs;
		}
	}

	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x060049CE RID: 18894 RVA: 0x0017B344 File Offset: 0x00179744
	public int totalVertices
	{
		get
		{
			return this._totalVertices;
		}
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x0017B34C File Offset: 0x0017974C
	private void SetGlobalLiveVars()
	{
		this.globalSettings.scale = base.transform.lossyScale.x;
		this.globalSettings.oneoverscale = 1f / base.transform.lossyScale.x;
		this.globalSettings.drawFromAnchor = this.drawFromAnchor;
		this.globalSettings.hairLength = this.hairLength * this.globalSettings.scale;
		this.globalSettings.segmentLength = this.hairLength * this.globalSettings.scale / (float)this.numberSegments;
		this.globalSettings.quarterSegmentLength = this.globalSettings.segmentLength * this.globalSettings.scale * 0.25f;
		this.globalSettings.hairWidth = this.hairWidth * this.globalSettings.scale;
		this.globalSettings.hairHalfWidth = this.hairWidth * this.globalSettings.scale * 0.5f;
		this.globalSettings.roundSheetHairs = this.roundSheetHairs;
		this.globalSettings.sheetHairRoundness = this.sheetHairRoundness;
		this.globalSettings.hairMaterial = this.hairMaterial;
		if (this.useGravity)
		{
			this.globalSettings.gravityForce = Physics.gravity * this.gravityMultiplier + this.appliedForce;
		}
		else
		{
			this.globalSettings.gravityForce = this.appliedForce;
		}
		this.globalSettings.useExtendedColliders = this.useExtendedColliders;
		this.globalSettings.extendedColliders = this.extendedColliders;
		this.globalSettings.staticMoveDistance = this.staticMoveDistance * this.globalSettings.scale;
		this.globalSettings.staticMoveDistanceSqr = this.globalSettings.staticMoveDistance * this.globalSettings.staticMoveDistance;
		this.globalSettings.staticFriction = this.staticFriction;
		this.globalSettings.velocityFactor = this.velocityFactor;
		this.globalSettings.stiffnessRoot = this.stiffnessRoot;
		this.globalSettings.stiffnessEnd = this.stiffnessEnd;
		this.globalSettings.stiffnessVariance = this.stiffnessVariance;
		this.globalSettings.enableSimulation = (this.enableSimulation && (SuperController.singleton == null || !SuperController.singleton.freezeAnimation));
		this.globalSettings.deltaTime = this.deltaTime;
		this.globalSettings.deltaTimeSqr = this.deltaTimeSqr;
		this.globalSettings.invDeltaTime = this.invDeltaTime;
		this.globalSettings.invdtdampen = this.invDeltaTime * this.dampenFactor;
		this.globalSettings.slowCollidingPoints = this.slowCollidingPoints;
		this.globalSettings.dampenFactor = this.dampenFactor;
		this.globalSettings.clampAcceleration = this.clampAcceleration;
		this.globalSettings.clampVelocity = this.clampVelocity;
		this.globalSettings.accelerationClamp = this.accelerationClamp;
		this.globalSettings.velocityClamp = this.velocityClamp;
		this.globalSettings.castShadows = this.castShadows;
		this.globalSettings.receiveShadows = this.receiveShadows;
		this.globalSettings.debugWidth = this.debugWidth;
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x0017B6A4 File Offset: 0x00179AA4
	private void SetHairStripLiveVars(int i, int j, bool initial)
	{
		Vector3 vector = this.smv[j];
		Vector3 vector2 = this.smn[j];
		int num = this.referenceVerts[j];
		Vector3 vector3;
		vector3.x = this.smv[num].x - this.smv[j].x;
		vector3.y = this.smv[num].y - this.smv[j].y;
		vector3.z = this.smv[num].z - this.smv[j].z;
		float num2 = 1f / vector3.magnitude;
		vector3.x *= num2;
		vector3.y *= num2;
		vector3.z *= num2;
		Vector3 vector4;
		vector4.x = vector2.y * vector3.z - vector2.z * vector3.y;
		vector4.y = vector2.z * vector3.x - vector2.x * vector3.z;
		vector4.z = vector2.x * vector3.y - vector2.y * vector3.x;
		if (this.debug && this.debugHairNum == i)
		{
			Debug.DrawRay(vector, vector2, Color.blue);
			Debug.DrawRay(vector, vector3, Color.red);
			Debug.DrawRay(vector, vector4, Color.yellow);
		}
		Matrix4x4 inverse = this.hs[i].rootMatrix.inverse;
		this.hs[i].rootMatrix[0] = vector2.x;
		this.hs[i].rootMatrix[1] = vector2.y;
		this.hs[i].rootMatrix[2] = vector2.z;
		this.hs[i].rootMatrix[4] = vector3.x;
		this.hs[i].rootMatrix[5] = vector3.y;
		this.hs[i].rootMatrix[6] = vector3.z;
		this.hs[i].rootMatrix[8] = vector4.x;
		this.hs[i].rootMatrix[9] = vector4.y;
		this.hs[i].rootMatrix[10] = vector4.z;
		this.hs[i].rootMatrix[12] = vector.x;
		this.hs[i].rootMatrix[13] = vector.y;
		this.hs[i].rootMatrix[14] = vector.z;
		if (initial)
		{
			this.hs[i].rootChangeMatrix = Matrix4x4.identity;
		}
		else
		{
			this.hs[i].rootChangeMatrix = this.hs[i].rootMatrix * inverse;
		}
		if (this.anchorOffset == 0f)
		{
			this.anchorOffset = 0.0001f;
		}
		this.hs[i].anchor = vector;
		if (i == 0)
		{
			this.minX = vector.x;
			this.maxX = vector.x;
			this.minY = vector.y;
			this.maxY = vector.y;
			this.minZ = vector.z;
			this.maxZ = vector.z;
		}
		else
		{
			if (vector.x < this.minX)
			{
				this.minX = vector.x;
			}
			else if (vector.x > this.maxX)
			{
				this.maxX = vector.x;
			}
			if (vector.y < this.minY)
			{
				this.minY = vector.y;
			}
			else if (vector.y > this.maxY)
			{
				this.maxY = vector.y;
			}
			if (vector.z < this.minZ)
			{
				this.minZ = vector.z;
			}
			else if (vector.z > this.maxZ)
			{
				this.maxZ = vector.z;
			}
		}
		Vector3 root;
		root.x = vector.x + vector2.x * this.anchorOffset * this.globalSettings.scale;
		root.y = vector.y + vector2.y * this.anchorOffset * this.globalSettings.scale;
		root.z = vector.z + vector2.z * this.anchorOffset * this.globalSettings.scale;
		this.hs[i].root = root;
		this.hs[i].anchorToRoot = vector2;
		this.hs[i].anchorTangent = vector3;
		this.hs[i].anchorTangent2 = vector4;
		if (this.debug)
		{
			if (this.debugHairNum == i)
			{
				this.hs[i].debug = true;
				this.hs[i].enableDraw = this.drawHairs;
			}
			else
			{
				if (this.debugOnlyDrawDebugHair)
				{
					this.hs[i].enableDraw = false;
				}
				else
				{
					this.hs[i].enableDraw = true;
				}
				this.hs[i].debug = false;
			}
		}
		else
		{
			this.hs[i].debug = false;
			this.hs[i].enableDraw = this.drawHairs;
		}
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x0017BC78 File Offset: 0x0017A078
	private void CreateColliderMask()
	{
		this.colliderMask = 0;
		if (this._colliderLayers != null)
		{
			foreach (string layerName in this._colliderLayers)
			{
				this.colliderMask |= 1 << LayerMask.NameToLayer(layerName);
			}
		}
		else
		{
			this.colliderMask = -1;
		}
	}

	// Token: 0x060049D2 RID: 18898 RVA: 0x0017BCDC File Offset: 0x0017A0DC
	private void UpdateMeshCenterAndRadius()
	{
		this.meshCenter.x = (this.minX + this.maxX) * 0.5f;
		this.meshCenter.y = (this.minY + this.maxY) * 0.5f;
		this.meshCenter.z = (this.minZ + this.maxZ) * 0.5f;
		float num = this.meshCenter.x - this.minX;
		float num2 = this.meshCenter.y - this.minY;
		float num3 = this.meshCenter.z - this.minZ;
		if (num2 > num)
		{
			if (num3 > num2)
			{
				this.meshRadius = num3;
			}
			else
			{
				this.meshRadius = num2;
			}
		}
		else if (num3 > num)
		{
			this.meshRadius = num3;
		}
		else
		{
			this.meshRadius = num;
		}
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x0017BDBC File Offset: 0x0017A1BC
	private void FindCloseColliders()
	{
		this.closeColliders = Physics.OverlapSphere(this.meshCenter, this.meshRadius + this.hairLength * this.globalSettings.scale, this.colliderMask);
		int num = 0;
		foreach (Collider collider in this.closeColliders)
		{
			if (collider is SphereCollider)
			{
				SphereCollider sphereCollider = (SphereCollider)collider;
				this.closeColliderCenters[num] = collider.transform.TransformPoint(sphereCollider.center);
			}
			else if (collider is BoxCollider)
			{
				BoxCollider boxCollider = (BoxCollider)collider;
				this.closeColliderCenters[num] = collider.transform.TransformPoint(boxCollider.center);
			}
			else if (collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)collider;
				this.closeColliderCenters[num] = collider.transform.TransformPoint(capsuleCollider.center);
			}
			else
			{
				this.closeColliderCenters[num] = Vector3.zero;
			}
			num++;
			if (num >= 100)
			{
				break;
			}
		}
		this.globalSettings.colliders = this.closeColliders;
		this.globalSettings.colliderCenters = this.closeColliderCenters;
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x0017BF14 File Offset: 0x0017A314
	private void AddMesh(int numVertices, int numTrianglePoints)
	{
		Mesh item = new Mesh();
		this.hms.Add(item);
		Vector3[] item2 = new Vector3[numVertices];
		this.hmverts.Add(item2);
		Vector3[] item3 = new Vector3[numVertices];
		this.hmnormals.Add(item3);
		if (this.createTangents)
		{
			Vector4[] item4 = new Vector4[numVertices];
			this.hmtangents.Add(item4);
		}
		Vector2[] item5 = new Vector2[numVertices];
		this.hmuvs.Add(item5);
		int[] item6 = new int[numTrianglePoints];
		this.hmtriangles.Add(item6);
	}

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x060049D5 RID: 18901 RVA: 0x0017BFA1 File Offset: 0x0017A3A1
	public bool threadsRunning
	{
		get
		{
			return this._threadsRunning;
		}
	}

	// Token: 0x060049D6 RID: 18902 RVA: 0x0017BFAC File Offset: 0x0017A3AC
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.mainHairTask != null)
		{
			this.mainHairTask.kill = true;
			this.mainHairTask.resetEvent.Set();
			while (this.mainHairTask.thread.IsAlive)
			{
			}
			this.mainHairTask = null;
		}
	}

	// Token: 0x060049D7 RID: 18903 RVA: 0x0017C00C File Offset: 0x0017A40C
	protected void StartThreads()
	{
		if (!this._threadsRunning)
		{
			this._threadsRunning = true;
			this.mainHairTask = new DAZHairMesh.DAZHairMeshTaskInfo();
			this.mainHairTask.name = "MainHairTask";
			this.mainHairTask.resetEvent = new AutoResetEvent(false);
			this.mainHairTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.mainHairTask.thread.Priority = System.Threading.ThreadPriority.BelowNormal;
			this.mainHairTask.thread.Start(this.mainHairTask);
		}
	}

	// Token: 0x060049D8 RID: 18904 RVA: 0x0017C09A File Offset: 0x0017A49A
	protected void OnDestroy()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
		}
	}

	// Token: 0x060049D9 RID: 18905 RVA: 0x0017C0AC File Offset: 0x0017A4AC
	protected void OnApplicationQuit()
	{
		if (Application.isPlaying)
		{
			this.StopThreads();
		}
	}

	// Token: 0x060049DA RID: 18906 RVA: 0x0017C0C0 File Offset: 0x0017A4C0
	protected void MTTask(object info)
	{
		DAZHairMesh.DAZHairMeshTaskInfo dazhairMeshTaskInfo = (DAZHairMesh.DAZHairMeshTaskInfo)info;
		while (this._threadsRunning)
		{
			dazhairMeshTaskInfo.resetEvent.WaitOne(-1, true);
			if (dazhairMeshTaskInfo.kill)
			{
				break;
			}
			Thread.Sleep(0);
			this.UpdateHairStripsThreaded();
			dazhairMeshTaskInfo.working = false;
		}
	}

	// Token: 0x060049DB RID: 18907 RVA: 0x0017C119 File Offset: 0x0017A519
	public void Reset()
	{
		this.wasInit = false;
	}

	// Token: 0x060049DC RID: 18908 RVA: 0x0017C124 File Offset: 0x0017A524
	public void init()
	{
		this.initCount = 1;
		this.globalSettings = new HairGlobalSettings();
		this._scalpSelection = this._scalpSelectionLive;
		this._hairDrawType = this._hairDrawTypeLive;
		this._numberSegments = this._numberSegmentsLive;
		this._numSubHairsMin = this._numSubHairsMinLive;
		this._numSubHairsMax = this._numSubHairsMaxLive;
		this._bundleType = this._bundleTypeLive;
		this._subHairNormalOffsetBend = this._subHairNormalOffsetBendLive;
		this._subHairTangent1OffsetMax = this._subHairTangent1OffsetMaxLive;
		this._subHairTangent2OffsetMax = this._subHairTangent2OffsetMaxLive;
		this._createTangents = this._createTangentsLive;
		this._colliderLayers = this.colliderLayers;
		this.deltaTime = Time.deltaTime;
		this.globalSettings.deltaTime = this.deltaTime;
		this.deltaTimeSqr = this.deltaTime * this.deltaTime;
		this.globalSettings.deltaTimeSqr = this.deltaTimeSqr;
		this.invDeltaTime = 1f / this.deltaTime;
		this.globalSettings.invDeltaTime = this.invDeltaTime;
		if (this._scalpSelection != null && this._scalpSelection.skin != null && this._scalpSelection.skin.gameObject.activeInHierarchy)
		{
			this.vertexIndices = new List<int>(this._scalpSelection.selectedVertices);
			this._numHairs = this.vertexIndices.Count;
			this._scalpSelection.clearChanged();
			if (Application.isPlaying && this._scalpSelection.skin.wasInit)
			{
				this.wasInit = true;
				this.CreateColliderMask();
				this.closeColliderCenters = new Vector3[100];
				this.hms = new List<Mesh>();
				this.hmverts = new List<Vector3[]>();
				this.hmnormals = new List<Vector3[]>();
				this.hmtangents = new List<Vector4[]>();
				this.hmuvs = new List<Vector2[]>();
				this.hmtriangles = new List<int[]>();
				this.smv = (Vector3[])this._scalpSelection.skin.drawVerts.Clone();
				this.smn = (Vector3[])this._scalpSelection.skin.drawNormals.Clone();
				if (this._hairDrawType == HairStripV2.HairDrawType.GPULines)
				{
					this._createTangentsLive = true;
					this._createTangents = true;
				}
				bool[] array = new bool[this.smv.Length];
				for (int i = 0; i < this.vertexIndices.Count; i++)
				{
					int num = this.vertexIndices[i];
					if (num >= this.smv.Length)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"mesh selection vertex ",
							num,
							" is out of range of vertices: ",
							this.smv.Length
						}));
					}
					else
					{
						array[num] = true;
					}
				}
				int[] baseTriangles = this._scalpSelection.skin.dazMesh.baseTriangles;
				this.referenceVerts = new int[this.smv.Length];
				float[] array2 = new float[this.smv.Length];
				for (int j = 0; j < this.smv.Length; j++)
				{
					array2[j] = 100000f;
				}
				for (int k = 0; k < baseTriangles.Length; k += 3)
				{
					int num2 = baseTriangles[k];
					int num3 = baseTriangles[k + 1];
					int num4 = baseTriangles[k + 2];
					if (array[num2])
					{
						if (array[num3])
						{
							float num5 = Mathf.Abs(this.smv[num2].y - this.smv[num3].y);
							if (num5 < array2[num2])
							{
								array2[num2] = num5;
								this.referenceVerts[num2] = num3;
							}
							if (num5 < array2[num3])
							{
								array2[num3] = num5;
								this.referenceVerts[num3] = num2;
							}
						}
						if (array[num4])
						{
							float num6 = Mathf.Abs(this.smv[num2].y - this.smv[num4].y);
							if (num6 < array2[num2])
							{
								array2[num2] = num6;
								this.referenceVerts[num2] = num4;
							}
							if (num6 < array2[num4])
							{
								array2[num4] = num6;
								this.referenceVerts[num4] = num2;
							}
						}
					}
					if (array[num3] && array[num4])
					{
						float num7 = Mathf.Abs(this.smv[num3].y - this.smv[num4].y);
						if (num7 < array2[num3])
						{
							array2[num3] = num7;
							this.referenceVerts[num3] = num4;
						}
						if (num7 < array2[num4])
						{
							array2[num4] = num7;
							this.referenceVerts[num4] = num3;
						}
					}
				}
				this.hs = new HairStripV2[this._numHairs];
				this._totalVertices = 0;
				this.totalTrianglePoints = 0;
				this.globalSettings.ownMesh = false;
				this.globalSettings.hairDrawType = this._hairDrawType;
				this.globalSettings.numberSegments = this._numberSegments;
				this.globalSettings.invNumberSegments = 1f / (float)this._numberSegments;
				this.globalSettings.numHairsMin = this._numSubHairsMin;
				this.globalSettings.numHairsMax = this._numSubHairsMax;
				this.globalSettings.bundleType = this._bundleType;
				this.globalSettings.subHairXOffsetMax = this._subHairTangent1OffsetMax;
				this.globalSettings.subHairYOffsetMax = this._subHairTangent2OffsetMax;
				this.globalSettings.subHairZOffsetBend = this._subHairNormalOffsetBend;
				this.globalSettings.createTangents = this._createTangents;
				this.SetGlobalLiveVars();
				for (int l = 0; l < this._numHairs; l++)
				{
					this.hs[l] = new HairStripV2();
					this.hs[l].globalSettings = this.globalSettings;
					int j2 = this.vertexIndices[l];
					this.SetHairStripLiveVars(l, j2, true);
					this.hs[l].Init();
					if (this._totalVertices + this.hs[l].numVertices > this.maxVerticesPerMesh)
					{
						this.AddMesh(this._totalVertices, this.totalTrianglePoints);
						this._totalVertices = 0;
						this.totalTrianglePoints = 0;
					}
					this._totalVertices += this.hs[l].numVertices;
					this.totalTrianglePoints += this.hs[l].numTrianglePoints;
				}
				this.AddMesh(this._totalVertices, this.totalTrianglePoints);
				this.UpdateMeshCenterAndRadius();
				this.FindCloseColliders();
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				for (int m = 0; m < this._numHairs; m++)
				{
					if (num8 + this.hs[m].numVertices > this.maxVerticesPerMesh)
					{
						num10++;
						num8 = 0;
						num9 = 0;
					}
					this.hs[m].hmverts = this.hmverts[num10];
					this.hs[m].hmnormals = this.hmnormals[num10];
					if (this.createTangents)
					{
						this.hs[m].hmtangents = this.hmtangents[num10];
					}
					this.hs[m].hmtriangles = this.hmtriangles[num10];
					this.hs[m].hmuvs = this.hmuvs[num10];
					this.hs[m].Start(num8, num9);
					this.hs[m].Update(num8);
					num8 += this.hs[m].numVertices;
					num9 += this.hs[m].numTrianglePoints;
				}
				float num11 = (this.meshRadius + this.hairLength) * 2f;
				for (int n = 0; n < this.hmverts.Count; n++)
				{
					this.hms[n].vertices = this.hmverts[n];
					this.hms[n].normals = this.hmnormals[n];
					if (this.createTangents)
					{
						this.hms[n].tangents = this.hmtangents[n];
					}
					this.hms[n].uv = this.hmuvs[n];
					if (this._hairDrawType == HairStripV2.HairDrawType.LineStrip)
					{
						this.hms[n].SetIndices(this.hmtriangles[n], MeshTopology.LineStrip, 0);
					}
					else if (this._hairDrawType == HairStripV2.HairDrawType.GPULines)
					{
						this.hms[n].SetIndices(this.hmtriangles[n], MeshTopology.Quads, 0);
					}
					else if (this._hairDrawType == HairStripV2.HairDrawType.Lines)
					{
						this.hms[n].SetIndices(this.hmtriangles[n], MeshTopology.Lines, 0);
					}
					else
					{
						this.hms[n].triangles = this.hmtriangles[n];
					}
					this.hms[n].bounds = new Bounds(this.meshCenter, new Vector3(num11, num11, num11));
				}
			}
		}
	}

	// Token: 0x060049DD RID: 18909 RVA: 0x0017CA85 File Offset: 0x0017AE85
	public void MaterialInit()
	{
		if (!this.materialWasInit)
		{
			this.materialWasInit = true;
			if (this.hairMaterial != null)
			{
				this.hairMaterialRuntime = new Material(this.hairMaterial);
			}
		}
	}

	// Token: 0x060049DE RID: 18910 RVA: 0x0017CABC File Offset: 0x0017AEBC
	private void Start()
	{
		this.MaterialInit();
		if (this.colliderLayers == null)
		{
			this.colliderLayers = new string[0];
		}
		if (this._colliderLayers == null)
		{
			this._colliderLayers = new string[0];
		}
		if (this.capsuleColliders != null && this.capsuleColliders.Length > 0)
		{
			this.extendedColliders = new ExtendedCapsuleCollider[this.capsuleColliders.Length];
			for (int i = 0; i < this.capsuleColliders.Length; i++)
			{
				this.extendedColliders[i] = new ExtendedCapsuleCollider();
				this.extendedColliders[i].collider = this.capsuleColliders[i];
				this.extendedColliders[i].RecalculateVars();
			}
		}
	}

	// Token: 0x060049DF RID: 18911 RVA: 0x0017CB74 File Offset: 0x0017AF74
	private void Update()
	{
		if (this.extendedColliders != null && this.useExtendedColliders)
		{
			foreach (ExtendedCapsuleCollider extendedCapsuleCollider in this.extendedColliders)
			{
				extendedCapsuleCollider.UpdateEndpoints();
				extendedCapsuleCollider.RecalculateVars();
			}
		}
	}

	// Token: 0x060049E0 RID: 18912 RVA: 0x0017CBC4 File Offset: 0x0017AFC4
	private void UpdateHairStripsThreaded()
	{
		float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
		for (int i = 0; i < this._numHairs; i++)
		{
			int j = this.vertexIndices[i];
			if (this.initCount > 0)
			{
				this.SetHairStripLiveVars(i, j, true);
			}
			else
			{
				this.SetHairStripLiveVars(i, j, false);
			}
		}
		if (this.initCount > 0)
		{
			this.initCount--;
		}
		float elapsedMilliseconds2 = GlobalStopwatch.GetElapsedMilliseconds();
		this.setVarsTime = elapsedMilliseconds2 - elapsedMilliseconds;
		this.UpdateMeshCenterAndRadius();
		int num = 0;
		for (int k = 0; k < this._numHairs; k++)
		{
			if (num + this.hs[k].numVertices > this.maxVerticesPerMesh)
			{
				num = 0;
			}
			this.hs[k].UpdateThreadSafe(num);
			num += this.hs[k].numVertices;
		}
		float elapsedMilliseconds3 = GlobalStopwatch.GetElapsedMilliseconds();
		this.simTime = elapsedMilliseconds3 - elapsedMilliseconds2;
	}

	// Token: 0x060049E1 RID: 18913 RVA: 0x0017CCC0 File Offset: 0x0017B0C0
	private void StartFrame()
	{
		this.deltaTime = Time.deltaTime;
		this.deltaTimeSqr = this.deltaTime * this.deltaTime;
		this.invDeltaTime = 1f / this.deltaTime;
		this.simTime = 0f;
		this.SetGlobalLiveVars();
		this._scalpSelection.skin.allowPostSkinMorph = true;
		foreach (int num in this.vertexIndices)
		{
			if (!this._scalpSelection.skin.postSkinVerts[num])
			{
				this._scalpSelection.skin.postSkinVerts[num] = true;
				this._scalpSelection.skin.postSkinVertsChanged = true;
			}
			if (!this._scalpSelection.skin.postSkinNormalVerts[num])
			{
				this._scalpSelection.skin.postSkinNormalVerts[num] = true;
				this._scalpSelection.skin.postSkinVertsChanged = true;
			}
			this.smv[num] = this._scalpSelection.skin.rawSkinnedVerts[num];
			this.smn[num] = this._scalpSelection.skin.postSkinNormals[num];
		}
	}

	// Token: 0x060049E2 RID: 18914 RVA: 0x0017CE38 File Offset: 0x0017B238
	private void FinishFrame()
	{
		float num = (this.meshRadius + this.hairLength * this.globalSettings.scale) * 2f;
		for (int i = 0; i < this.hmverts.Count; i++)
		{
			this.hms[i].vertices = this.hmverts[i];
			if (this.debugAllPoints)
			{
				for (int j = 0; j < this.hmverts[i].Length; j++)
				{
					MyDebug.DrawWireCube(this.hmverts[i][j], 0.001f, Color.cyan);
				}
			}
			this.hms[i].normals = this.hmnormals[i];
			if (this.createTangents)
			{
				this.hms[i].tangents = this.hmtangents[i];
			}
			this.hms[i].bounds = new Bounds(this.meshCenter, new Vector3(num, num, num));
		}
	}

	// Token: 0x060049E3 RID: 18915 RVA: 0x0017CF58 File Offset: 0x0017B358
	private void LateUpdate()
	{
		if (!this.wasInit)
		{
			this.init();
		}
		if (this.wasInit && this._scalpSelection != null && this._scalpSelection.skin != null && this.hairMaterial != null)
		{
			if (this.useThreading)
			{
				this.StartThreads();
				if (!this.mainHairTask.working)
				{
					this.FinishFrame();
					this.StartFrame();
					this.mainHairTask.working = true;
					this.mainHairTask.resetEvent.Set();
				}
			}
			else
			{
				this.StartFrame();
				this.UpdateHairStripsThreaded();
				this.FinishFrame();
			}
			float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			if (this.drawHairs)
			{
				if (this.hairMaterialRuntime.GetFloat("_Scale") != this.globalSettings.scale)
				{
					this.hairMaterialRuntime.SetFloat("_Scale", this.globalSettings.scale);
				}
				for (int i = 0; i < this.hmverts.Count; i++)
				{
					Graphics.DrawMesh(this.hms[i], Matrix4x4.identity, this.hairMaterialRuntime, 0, null, 0, null, this.castShadows, this.receiveShadows);
				}
			}
			float elapsedMilliseconds2 = GlobalStopwatch.GetElapsedMilliseconds();
			this.drawTime = elapsedMilliseconds2 - elapsedMilliseconds;
		}
	}

	// Token: 0x04003821 RID: 14369
	[SerializeField]
	private DAZSkinV2MeshSelection _scalpSelectionLive;

	// Token: 0x04003822 RID: 14370
	[SerializeField]
	private DAZSkinV2MeshSelection _scalpSelection;

	// Token: 0x04003823 RID: 14371
	private List<int> vertexIndices;

	// Token: 0x04003824 RID: 14372
	[SerializeField]
	private HairStripV2.HairDrawType _hairDrawTypeLive = HairStripV2.HairDrawType.GPULines;

	// Token: 0x04003825 RID: 14373
	[SerializeField]
	private HairStripV2.HairDrawType _hairDrawType;

	// Token: 0x04003826 RID: 14374
	[SerializeField]
	private int _numberSegmentsLive = 3;

	// Token: 0x04003827 RID: 14375
	[SerializeField]
	private int _numberSegments;

	// Token: 0x04003828 RID: 14376
	[SerializeField]
	private int _numSubHairsMinLive = 1;

	// Token: 0x04003829 RID: 14377
	[SerializeField]
	private int _numSubHairsMin;

	// Token: 0x0400382A RID: 14378
	[SerializeField]
	private int _numSubHairsMaxLive = 1;

	// Token: 0x0400382B RID: 14379
	[SerializeField]
	private int _numSubHairsMax;

	// Token: 0x0400382C RID: 14380
	[SerializeField]
	private HairStripV2.HairBundleType _bundleTypeLive = HairStripV2.HairBundleType.Circular;

	// Token: 0x0400382D RID: 14381
	[SerializeField]
	private HairStripV2.HairBundleType _bundleType;

	// Token: 0x0400382E RID: 14382
	[SerializeField]
	private float _subHairTangent1OffsetMaxLive = 0.01f;

	// Token: 0x0400382F RID: 14383
	[SerializeField]
	private float _subHairTangent1OffsetMax;

	// Token: 0x04003830 RID: 14384
	[SerializeField]
	private float _subHairTangent2OffsetMaxLive = 0.01f;

	// Token: 0x04003831 RID: 14385
	[SerializeField]
	private float _subHairTangent2OffsetMax;

	// Token: 0x04003832 RID: 14386
	[SerializeField]
	private float _subHairNormalOffsetBendLive;

	// Token: 0x04003833 RID: 14387
	[SerializeField]
	private float _subHairNormalOffsetBend;

	// Token: 0x04003834 RID: 14388
	public string[] _colliderLayers;

	// Token: 0x04003835 RID: 14389
	public string[] colliderLayers;

	// Token: 0x04003836 RID: 14390
	public CapsuleCollider[] capsuleColliders;

	// Token: 0x04003837 RID: 14391
	public bool useExtendedColliders;

	// Token: 0x04003838 RID: 14392
	private ExtendedCapsuleCollider[] extendedColliders;

	// Token: 0x04003839 RID: 14393
	[SerializeField]
	private bool _createTangentsLive;

	// Token: 0x0400383A RID: 14394
	[SerializeField]
	private bool _createTangents;

	// Token: 0x0400383B RID: 14395
	public HairGlobalSettings globalSettings;

	// Token: 0x0400383C RID: 14396
	public bool drawHairs = true;

	// Token: 0x0400383D RID: 14397
	public bool drawFromAnchor = true;

	// Token: 0x0400383E RID: 14398
	public float anchorOffset = 0.005f;

	// Token: 0x0400383F RID: 14399
	public float hairLength = 0.15f;

	// Token: 0x04003840 RID: 14400
	public float hairWidth = 0.0005f;

	// Token: 0x04003841 RID: 14401
	public bool roundSheetHairs = true;

	// Token: 0x04003842 RID: 14402
	public float sheetHairRoundness = 0.5f;

	// Token: 0x04003843 RID: 14403
	public Material hairMaterial;

	// Token: 0x04003844 RID: 14404
	public Material hairMaterialRuntime;

	// Token: 0x04003845 RID: 14405
	public Vector3 appliedForce;

	// Token: 0x04003846 RID: 14406
	public bool useGravity = true;

	// Token: 0x04003847 RID: 14407
	public float gravityMultiplier = 0.1f;

	// Token: 0x04003848 RID: 14408
	public float staticMoveDistance = 0.0005f;

	// Token: 0x04003849 RID: 14409
	public bool staticFriction;

	// Token: 0x0400384A RID: 14410
	public float velocityFactor = 0.92f;

	// Token: 0x0400384B RID: 14411
	public float stiffnessRoot = 1f;

	// Token: 0x0400384C RID: 14412
	public float stiffnessEnd = 0.3f;

	// Token: 0x0400384D RID: 14413
	public float stiffnessVariance = 0.1f;

	// Token: 0x0400384E RID: 14414
	public bool enableCollision = true;

	// Token: 0x0400384F RID: 14415
	public bool enableSimulation = true;

	// Token: 0x04003850 RID: 14416
	public float slowCollidingPoints;

	// Token: 0x04003851 RID: 14417
	public float dampenFactor = 0.9f;

	// Token: 0x04003852 RID: 14418
	public bool clampAcceleration;

	// Token: 0x04003853 RID: 14419
	public bool clampVelocity;

	// Token: 0x04003854 RID: 14420
	public float accelerationClamp = 0.015f;

	// Token: 0x04003855 RID: 14421
	public float velocityClamp = 0.1f;

	// Token: 0x04003856 RID: 14422
	public bool castShadows = true;

	// Token: 0x04003857 RID: 14423
	public bool receiveShadows = true;

	// Token: 0x04003858 RID: 14424
	public bool debug;

	// Token: 0x04003859 RID: 14425
	public int debugHairNum;

	// Token: 0x0400385A RID: 14426
	public float debugWidth = 0.005f;

	// Token: 0x0400385B RID: 14427
	public bool debugOnlyDrawDebugHair = true;

	// Token: 0x0400385C RID: 14428
	public bool debugAllPoints;

	// Token: 0x0400385D RID: 14429
	private int _numHairs;

	// Token: 0x0400385E RID: 14430
	private List<Mesh> hms;

	// Token: 0x0400385F RID: 14431
	private List<Vector3[]> hmverts;

	// Token: 0x04003860 RID: 14432
	private List<Vector3[]> hmnormals;

	// Token: 0x04003861 RID: 14433
	private List<Vector4[]> hmtangents;

	// Token: 0x04003862 RID: 14434
	private List<Vector2[]> hmuvs;

	// Token: 0x04003863 RID: 14435
	private List<int[]> hmtriangles;

	// Token: 0x04003864 RID: 14436
	private Vector3[] smv;

	// Token: 0x04003865 RID: 14437
	private Vector3[] smn;

	// Token: 0x04003866 RID: 14438
	private int[] referenceVerts;

	// Token: 0x04003867 RID: 14439
	private List<int> postSkinVerts;

	// Token: 0x04003868 RID: 14440
	private HairStripV2[] hs;

	// Token: 0x04003869 RID: 14441
	private Vector3 meshCenter;

	// Token: 0x0400386A RID: 14442
	private float meshRadius;

	// Token: 0x0400386B RID: 14443
	private float minX;

	// Token: 0x0400386C RID: 14444
	private float minY;

	// Token: 0x0400386D RID: 14445
	private float minZ;

	// Token: 0x0400386E RID: 14446
	private float maxX;

	// Token: 0x0400386F RID: 14447
	private float maxY;

	// Token: 0x04003870 RID: 14448
	private float maxZ;

	// Token: 0x04003871 RID: 14449
	private Collider[] closeColliders;

	// Token: 0x04003872 RID: 14450
	private Vector3[] closeColliderCenters;

	// Token: 0x04003873 RID: 14451
	private int colliderMask;

	// Token: 0x04003874 RID: 14452
	private int _totalVertices;

	// Token: 0x04003875 RID: 14453
	private int totalTrianglePoints;

	// Token: 0x04003876 RID: 14454
	private bool wasInit;

	// Token: 0x04003877 RID: 14455
	private int initCount;

	// Token: 0x04003878 RID: 14456
	public float simTime;

	// Token: 0x04003879 RID: 14457
	public float setVarsTime;

	// Token: 0x0400387A RID: 14458
	public float drawTime;

	// Token: 0x0400387B RID: 14459
	private float deltaTime;

	// Token: 0x0400387C RID: 14460
	private float deltaTimeSqr;

	// Token: 0x0400387D RID: 14461
	private float invDeltaTime;

	// Token: 0x0400387E RID: 14462
	private int maxVerticesPerMesh = 40000;

	// Token: 0x0400387F RID: 14463
	public bool useThreading = true;

	// Token: 0x04003880 RID: 14464
	protected DAZHairMesh.DAZHairMeshTaskInfo mainHairTask;

	// Token: 0x04003881 RID: 14465
	protected bool _threadsRunning;

	// Token: 0x04003882 RID: 14466
	private bool materialWasInit;

	// Token: 0x02000AD7 RID: 2775
	public class DAZHairMeshTaskInfo
	{
		// Token: 0x060049E4 RID: 18916 RVA: 0x0017D0C1 File Offset: 0x0017B4C1
		public DAZHairMeshTaskInfo()
		{
		}

		// Token: 0x04003883 RID: 14467
		public string name;

		// Token: 0x04003884 RID: 14468
		public AutoResetEvent resetEvent;

		// Token: 0x04003885 RID: 14469
		public Thread thread;

		// Token: 0x04003886 RID: 14470
		public volatile bool working;

		// Token: 0x04003887 RID: 14471
		public volatile bool kill;
	}
}
