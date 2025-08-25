using System;
using System.Collections.Generic;
using GPUTools.Physics.Scripts.Behaviours;
using UnityEngine;

// Token: 0x02000B4A RID: 2890
[Serializable]
public class DAZPhysicsMeshSoftVerticesSet
{
	// Token: 0x06004FFC RID: 20476 RVA: 0x001C8E68 File Offset: 0x001C7268
	public DAZPhysicsMeshSoftVerticesSet()
	{
		this._uid = Guid.NewGuid().ToString();
		this.influenceVertices = new int[0];
		this._links = new List<string>();
	}

	// Token: 0x17000B7B RID: 2939
	// (get) Token: 0x06004FFD RID: 20477 RVA: 0x001C8EDC File Offset: 0x001C72DC
	public string uid
	{
		get
		{
			if (this._uid == null || this._uid == string.Empty)
			{
				this._uid = Guid.NewGuid().ToString();
			}
			return this._uid;
		}
	}

	// Token: 0x17000B7C RID: 2940
	// (get) Token: 0x06004FFE RID: 20478 RVA: 0x001C8F28 File Offset: 0x001C7328
	public List<string> links
	{
		get
		{
			if (this._links == null)
			{
				this._links = new List<string>();
			}
			return this._links;
		}
	}

	// Token: 0x06004FFF RID: 20479 RVA: 0x001C8F48 File Offset: 0x001C7348
	public void AddInfluenceVertex(int vid)
	{
		int[] array = new int[this.influenceVertices.Length + 1];
		bool flag = false;
		for (int i = 0; i < this.influenceVertices.Length; i++)
		{
			if (this.influenceVertices[i] == vid)
			{
				flag = true;
				break;
			}
			array[i] = this.influenceVertices[i];
		}
		if (!flag)
		{
			array[this.influenceVertices.Length] = vid;
			this.influenceVertices = array;
		}
	}

	// Token: 0x06005000 RID: 20480 RVA: 0x001C8FB8 File Offset: 0x001C73B8
	public void RemoveInfluenceVertex(int vid)
	{
		int[] array = new int[this.influenceVertices.Length - 1];
		bool flag = false;
		int num = 0;
		for (int i = 0; i < this.influenceVertices.Length; i++)
		{
			if (this.influenceVertices[i] == vid)
			{
				flag = true;
			}
			else
			{
				array[num] = this.influenceVertices[i];
				num++;
			}
		}
		if (flag)
		{
			this.influenceVertices = array;
		}
	}

	// Token: 0x04003FD2 RID: 16338
	[SerializeField]
	protected string _uid;

	// Token: 0x04003FD3 RID: 16339
	public int targetVertex = -1;

	// Token: 0x04003FD4 RID: 16340
	public int anchorVertex = -1;

	// Token: 0x04003FD5 RID: 16341
	public bool autoInfluenceAnchor;

	// Token: 0x04003FD6 RID: 16342
	public int[] influenceVertices;

	// Token: 0x04003FD7 RID: 16343
	public int highlightVertex;

	// Token: 0x04003FD8 RID: 16344
	[SerializeField]
	protected List<string> _links;

	// Token: 0x04003FD9 RID: 16345
	public float springMultiplier = 1f;

	// Token: 0x04003FDA RID: 16346
	public float sizeMultiplier = 1f;

	// Token: 0x04003FDB RID: 16347
	public float limitMultiplier = 1f;

	// Token: 0x04003FDC RID: 16348
	public bool forceLookAtReference;

	// Token: 0x04003FDD RID: 16349
	[NonSerialized]
	public Vector3 lastPosition;

	// Token: 0x04003FDE RID: 16350
	[NonSerialized]
	public Vector3 lastPositionThreaded;

	// Token: 0x04003FDF RID: 16351
	[NonSerialized]
	public Vector3 currentPosition;

	// Token: 0x04003FE0 RID: 16352
	[NonSerialized]
	public Vector3 lastKinematicPosition;

	// Token: 0x04003FE1 RID: 16353
	[NonSerialized]
	public Vector3 lastKinematicPositionThreaded;

	// Token: 0x04003FE2 RID: 16354
	[NonSerialized]
	public Vector3 currentKinematicPosition;

	// Token: 0x04003FE3 RID: 16355
	[NonSerialized]
	public float interpFactor;

	// Token: 0x04003FE4 RID: 16356
	[NonSerialized]
	public Transform kinematicTransform;

	// Token: 0x04003FE5 RID: 16357
	[NonSerialized]
	public Rigidbody kinematicRB;

	// Token: 0x04003FE6 RID: 16358
	[NonSerialized]
	public Transform jointTransform;

	// Token: 0x04003FE7 RID: 16359
	[NonSerialized]
	public Transform jointTrackerTransform;

	// Token: 0x04003FE8 RID: 16360
	[NonSerialized]
	public Rigidbody jointRB;

	// Token: 0x04003FE9 RID: 16361
	[NonSerialized]
	public ConfigurableJoint joint;

	// Token: 0x04003FEA RID: 16362
	[NonSerialized]
	public Collider jointCollider;

	// Token: 0x04003FEB RID: 16363
	[NonSerialized]
	public Collider jointCollider2;

	// Token: 0x04003FEC RID: 16364
	[NonSerialized]
	public float[] influenceVerticesDistances;

	// Token: 0x04003FED RID: 16365
	[NonSerialized]
	public float[] influenceVerticesWeights;

	// Token: 0x04003FEE RID: 16366
	[NonSerialized]
	public SpringJoint[] linkJoints;

	// Token: 0x04003FEF RID: 16367
	[NonSerialized]
	public float[] linkJointDistances;

	// Token: 0x04003FF0 RID: 16368
	[NonSerialized]
	public Vector3 initialTargetPosition;

	// Token: 0x04003FF1 RID: 16369
	[NonSerialized]
	public bool linkTargetPositionDirty;

	// Token: 0x04003FF2 RID: 16370
	[NonSerialized]
	public Vector3 jointTargetPosition;

	// Token: 0x04003FF3 RID: 16371
	[NonSerialized]
	public Vector3 jointTargetVelocity;

	// Token: 0x04003FF4 RID: 16372
	[NonSerialized]
	public Vector3 lastJointTargetPosition;

	// Token: 0x04003FF5 RID: 16373
	[NonSerialized]
	public Quaternion jointTargetLookAt;

	// Token: 0x04003FF6 RID: 16374
	[NonSerialized]
	public Vector3 primaryMove;

	// Token: 0x04003FF7 RID: 16375
	[NonSerialized]
	public Vector3 move;

	// Token: 0x04003FF8 RID: 16376
	[NonSerialized]
	public float threadedColliderRadius;

	// Token: 0x04003FF9 RID: 16377
	[NonSerialized]
	public float threadedColliderHeight;

	// Token: 0x04003FFA RID: 16378
	[NonSerialized]
	public Vector3 threadedColliderCenter;

	// Token: 0x04003FFB RID: 16379
	[NonSerialized]
	public float threadedCollider2Radius;

	// Token: 0x04003FFC RID: 16380
	[NonSerialized]
	public float threadedCollider2Height;

	// Token: 0x04003FFD RID: 16381
	[NonSerialized]
	public Vector3 threadedCollider2Center;

	// Token: 0x04003FFE RID: 16382
	[NonSerialized]
	public CapsuleLineSphereCollider capsuleLineSphereCollider;

	// Token: 0x04003FFF RID: 16383
	[NonSerialized]
	public CapsuleLineSphereCollider capsuleLineSphereCollider2;
}
