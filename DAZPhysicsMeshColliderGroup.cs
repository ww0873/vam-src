using System;
using UnityEngine;

// Token: 0x02000B43 RID: 2883
[Serializable]
public class DAZPhysicsMeshColliderGroup
{
	// Token: 0x06004FAE RID: 20398 RVA: 0x001C7B7D File Offset: 0x001C5F7D
	public DAZPhysicsMeshColliderGroup()
	{
		this.colliders = new DAZPhysicsMeshCapsuleCollider[0];
		this._currentColliderIndex = -1;
		this._currentCollider = null;
	}

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x06004FAF RID: 20399 RVA: 0x001C7B9F File Offset: 0x001C5F9F
	// (set) Token: 0x06004FB0 RID: 20400 RVA: 0x001C7BA8 File Offset: 0x001C5FA8
	public int currentColliderIndex
	{
		get
		{
			return this._currentColliderIndex;
		}
		set
		{
			if (this._currentColliderIndex != value && this._currentColliderIndex >= 0 && this._currentColliderIndex < this.colliders.Length)
			{
				this._currentColliderIndex = value;
				this._currentCollider = this.colliders[this._currentColliderIndex];
			}
		}
	}

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x001C7BFA File Offset: 0x001C5FFA
	public DAZPhysicsMeshCapsuleCollider currentCollider
	{
		get
		{
			return this._currentCollider;
		}
	}

	// Token: 0x06004FB2 RID: 20402 RVA: 0x001C7C02 File Offset: 0x001C6002
	public void AddCollider()
	{
	}

	// Token: 0x06004FB3 RID: 20403 RVA: 0x001C7C04 File Offset: 0x001C6004
	public void RemoveCollider(int index)
	{
	}

	// Token: 0x06004FB4 RID: 20404 RVA: 0x001C7C06 File Offset: 0x001C6006
	public void MoveColider(int fromindex, int toindex)
	{
	}

	// Token: 0x04003F94 RID: 16276
	public string name;

	// Token: 0x04003F95 RID: 16277
	public DAZBone bone;

	// Token: 0x04003F96 RID: 16278
	public DAZPhysicsMeshCapsuleCollider[] colliders;

	// Token: 0x04003F97 RID: 16279
	[SerializeField]
	private int _currentColliderIndex;

	// Token: 0x04003F98 RID: 16280
	[SerializeField]
	private DAZPhysicsMeshCapsuleCollider _currentCollider;
}
