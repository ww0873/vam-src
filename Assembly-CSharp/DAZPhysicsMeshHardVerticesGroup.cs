using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B47 RID: 2887
[Serializable]
public class DAZPhysicsMeshHardVerticesGroup
{
	// Token: 0x06004FBD RID: 20413 RVA: 0x001C7D30 File Offset: 0x001C6130
	public DAZPhysicsMeshHardVerticesGroup()
	{
		this._vertices = new int[0];
	}

	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x06004FBE RID: 20414 RVA: 0x001C7D65 File Offset: 0x001C6165
	// (set) Token: 0x06004FBF RID: 20415 RVA: 0x001C7D6D File Offset: 0x001C616D
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			if (this.name != value)
			{
				this._name = value;
			}
		}
	}

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x06004FC0 RID: 20416 RVA: 0x001C7D87 File Offset: 0x001C6187
	// (set) Token: 0x06004FC1 RID: 20417 RVA: 0x001C7D8F File Offset: 0x001C618F
	public DAZBone bone
	{
		get
		{
			return this._bone;
		}
		set
		{
			if (this._bone != value)
			{
				this._bone = value;
			}
		}
	}

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x06004FC2 RID: 20418 RVA: 0x001C7DA9 File Offset: 0x001C61A9
	public int[] vertices
	{
		get
		{
			return this._vertices;
		}
	}

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x06004FC3 RID: 20419 RVA: 0x001C7DB1 File Offset: 0x001C61B1
	// (set) Token: 0x06004FC4 RID: 20420 RVA: 0x001C7DB9 File Offset: 0x001C61B9
	public bool useMorphedVertices
	{
		get
		{
			return this._useMorphedVertices;
		}
		set
		{
			if (this._useMorphedVertices != value)
			{
				this._useMorphedVertices = value;
			}
		}
	}

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x06004FC5 RID: 20421 RVA: 0x001C7DCE File Offset: 0x001C61CE
	// (set) Token: 0x06004FC6 RID: 20422 RVA: 0x001C7DD6 File Offset: 0x001C61D6
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
			}
		}
	}

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x06004FC7 RID: 20423 RVA: 0x001C7DEB File Offset: 0x001C61EB
	// (set) Token: 0x06004FC8 RID: 20424 RVA: 0x001C7DF3 File Offset: 0x001C61F3
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
				this._colliderLayer = value;
			}
		}
	}

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x001C7E0D File Offset: 0x001C620D
	// (set) Token: 0x06004FCA RID: 20426 RVA: 0x001C7E15 File Offset: 0x001C6215
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

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x06004FCB RID: 20427 RVA: 0x001C7E2A File Offset: 0x001C622A
	// (set) Token: 0x06004FCC RID: 20428 RVA: 0x001C7E32 File Offset: 0x001C6232
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
			}
		}
	}

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x06004FCD RID: 20429 RVA: 0x001C7E4C File Offset: 0x001C624C
	// (set) Token: 0x06004FCE RID: 20430 RVA: 0x001C7E54 File Offset: 0x001C6254
	public bool offsetCenterByRadius
	{
		get
		{
			return this._offsetCenterByRadius;
		}
		set
		{
			if (this._offsetCenterByRadius != value)
			{
				this._offsetCenterByRadius = value;
			}
		}
	}

	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x06004FCF RID: 20431 RVA: 0x001C7E69 File Offset: 0x001C6269
	// (set) Token: 0x06004FD0 RID: 20432 RVA: 0x001C7E71 File Offset: 0x001C6271
	public float colliderOffset
	{
		get
		{
			return this._colliderOffset;
		}
		set
		{
			if (this._colliderOffset != value)
			{
				this._colliderOffset = value;
			}
		}
	}

	// Token: 0x06004FD1 RID: 20433 RVA: 0x001C7E88 File Offset: 0x001C6288
	public void AddVertex(int vid)
	{
		int[] array = new int[this._vertices.Length + 1];
		bool flag = false;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			if (this._vertices[i] == vid)
			{
				flag = true;
				break;
			}
			array[i] = this._vertices[i];
		}
		if (!flag)
		{
			array[this._vertices.Length] = vid;
			this._vertices = array;
		}
	}

	// Token: 0x06004FD2 RID: 20434 RVA: 0x001C7EF8 File Offset: 0x001C62F8
	public void RemoveVertex(int vid)
	{
		int[] array = new int[this._vertices.Length - 1];
		bool flag = false;
		int num = 0;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			if (this._vertices[i] == vid)
			{
				flag = true;
			}
			else
			{
				array[num] = this._vertices[i];
				num++;
			}
		}
		if (flag)
		{
			this._vertices = array;
		}
	}

	// Token: 0x06004FD3 RID: 20435 RVA: 0x001C7F64 File Offset: 0x001C6364
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

	// Token: 0x06004FD4 RID: 20436 RVA: 0x001C8038 File Offset: 0x001C6438
	public void InitColliders()
	{
		if (this._colliders != null)
		{
			List<Collider> list = new List<Collider>();
			foreach (Transform transform in this._ignoreColliders)
			{
				this.GetCollidersRecursive(transform, transform, list);
			}
			foreach (Collider collider in list)
			{
				foreach (SphereCollider collider2 in this._colliders)
				{
					Physics.IgnoreCollision(collider2, collider);
				}
			}
		}
	}

	// Token: 0x06004FD5 RID: 20437 RVA: 0x001C80F4 File Offset: 0x001C64F4
	public void CreateColliders(Transform transform, DAZSkinV2 sk)
	{
		this.skin = sk;
		Vector3[] drawVerts = this.skin.drawVerts;
		Vector3[] drawNormals = this.skin.drawNormals;
		GameObject gameObject = new GameObject();
		this._colliderTransform = gameObject.transform;
		gameObject.transform.SetParent(transform);
		if (this._colliderLayer != null && this._colliderLayer != string.Empty)
		{
			int layer = LayerMask.NameToLayer(this._colliderLayer);
			gameObject.layer = layer;
		}
		this._colliders = new SphereCollider[this._vertices.Length];
		for (int i = 0; i < this._vertices.Length; i++)
		{
			int num = this._vertices[i];
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			this._colliders[i] = sphereCollider;
			sphereCollider.radius = this._colliderRadius;
			if (this._offsetCenterByRadius || this._colliderOffset != 0f)
			{
				float d = (!this._offsetCenterByRadius) ? this._colliderOffset : (this._colliderRadius + this._colliderOffset);
				sphereCollider.center = drawVerts[num] - drawNormals[num] * d;
			}
			else
			{
				sphereCollider.center = drawVerts[num];
			}
			if (this._colliderMaterial != null)
			{
				sphereCollider.material = this._colliderMaterial;
			}
		}
		this.InitColliders();
	}

	// Token: 0x06004FD6 RID: 20438 RVA: 0x001C8278 File Offset: 0x001C6678
	public void UpdateColliders()
	{
		this._colliderTransform.position = this.zeroPosition;
		this._colliderTransform.rotation = this.identityRotation;
		Vector3[] array;
		if (this._useMorphedVertices)
		{
			array = this.skin.drawVerts;
		}
		else
		{
			array = this.skin.rawSkinnedVerts;
		}
		Vector3[] drawNormals = this.skin.drawNormals;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			int num = this._vertices[i];
			this.skin.postSkinVerts[num] = true;
			SphereCollider sphereCollider = this._colliders[i];
			if (this._offsetCenterByRadius || this._colliderOffset != 0f)
			{
				float d = (!this._offsetCenterByRadius) ? this._colliderOffset : (this._colliderRadius + this._colliderOffset);
				sphereCollider.center = array[num] - drawNormals[num] * d;
			}
			else
			{
				sphereCollider.center = array[num];
			}
		}
	}

	// Token: 0x04003F9E RID: 16286
	[SerializeField]
	protected string _name;

	// Token: 0x04003F9F RID: 16287
	[SerializeField]
	protected DAZBone _bone;

	// Token: 0x04003FA0 RID: 16288
	[SerializeField]
	protected int[] _vertices;

	// Token: 0x04003FA1 RID: 16289
	[SerializeField]
	protected bool _useMorphedVertices;

	// Token: 0x04003FA2 RID: 16290
	[SerializeField]
	protected float _colliderRadius = 0.003f;

	// Token: 0x04003FA3 RID: 16291
	[SerializeField]
	private string _colliderLayer;

	// Token: 0x04003FA4 RID: 16292
	[SerializeField]
	private Transform[] _ignoreColliders;

	// Token: 0x04003FA5 RID: 16293
	[SerializeField]
	private PhysicMaterial _colliderMaterial;

	// Token: 0x04003FA6 RID: 16294
	[SerializeField]
	private bool _offsetCenterByRadius;

	// Token: 0x04003FA7 RID: 16295
	[SerializeField]
	private float _colliderOffset;

	// Token: 0x04003FA8 RID: 16296
	protected DAZSkinV2 skin;

	// Token: 0x04003FA9 RID: 16297
	protected SphereCollider[] _colliders;

	// Token: 0x04003FAA RID: 16298
	protected Transform _colliderTransform;

	// Token: 0x04003FAB RID: 16299
	protected Vector3 zeroPosition = Vector3.zero;

	// Token: 0x04003FAC RID: 16300
	protected Quaternion identityRotation = Quaternion.identity;
}
