using System;
using UnityEngine;

// Token: 0x02000B1D RID: 2845
public class DAZTongueControl : PhysicsSimulatorJSONStorable
{
	// Token: 0x06004DA5 RID: 19877 RVA: 0x001B4635 File Offset: 0x001B2A35
	public DAZTongueControl()
	{
	}

	// Token: 0x06004DA6 RID: 19878 RVA: 0x001B4644 File Offset: 0x001B2A44
	protected override void SyncCollisionEnabled()
	{
		base.SyncCollisionEnabled();
		this.SyncTongueCollision();
	}

	// Token: 0x06004DA7 RID: 19879 RVA: 0x001B4654 File Offset: 0x001B2A54
	protected override void SyncUseInterpolation()
	{
		base.SyncUseInterpolation();
		if (this.tongueCollisionRigidbodies != null)
		{
			foreach (Rigidbody rigidbody in this.tongueCollisionRigidbodies)
			{
				if (this._useInterpolation)
				{
					rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
				}
				else
				{
					rigidbody.interpolation = RigidbodyInterpolation.None;
				}
			}
		}
		if (this.tongueCollisionAutoGroups != null)
		{
			foreach (Transform transform in this.tongueCollisionAutoGroups)
			{
				Rigidbody[] componentsInChildren = transform.GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody rigidbody2 in componentsInChildren)
				{
					if (this._useInterpolation)
					{
						rigidbody2.interpolation = RigidbodyInterpolation.Interpolate;
					}
					else
					{
						rigidbody2.interpolation = RigidbodyInterpolation.None;
					}
				}
			}
		}
	}

	// Token: 0x06004DA8 RID: 19880 RVA: 0x001B4730 File Offset: 0x001B2B30
	protected void SyncTongueCollision()
	{
		if (this.tongueCollisionRigidbodies != null)
		{
			foreach (Rigidbody rigidbody in this.tongueCollisionRigidbodies)
			{
				rigidbody.detectCollisions = (this._tongueCollision && !this._resetSimulation);
			}
		}
		if (this.tongueCollisionAutoGroups != null)
		{
			foreach (Transform transform in this.tongueCollisionAutoGroups)
			{
				Rigidbody[] componentsInChildren = transform.GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody rigidbody2 in componentsInChildren)
				{
					rigidbody2.detectCollisions = (this._tongueCollision && !this._resetSimulation);
				}
			}
		}
	}

	// Token: 0x06004DA9 RID: 19881 RVA: 0x001B4800 File Offset: 0x001B2C00
	protected void SyncTongueCollision(bool b)
	{
		this._tongueCollision = b;
		this.SyncTongueCollision();
	}

	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x06004DAA RID: 19882 RVA: 0x001B480F File Offset: 0x001B2C0F
	// (set) Token: 0x06004DAB RID: 19883 RVA: 0x001B4817 File Offset: 0x001B2C17
	public bool tongueCollision
	{
		get
		{
			return this._tongueCollision;
		}
		set
		{
			if (this.tongueCollisionJSON != null)
			{
				this.tongueCollisionJSON.val = value;
			}
			else if (this._tongueCollision != value)
			{
				this.SyncTongueCollision(value);
			}
		}
	}

	// Token: 0x06004DAC RID: 19884 RVA: 0x001B4848 File Offset: 0x001B2C48
	protected void Init()
	{
		this.tongueCollisionJSON = new JSONStorableBool("tongueCollision", this._tongueCollision, new JSONStorableBool.SetBoolCallback(this.SyncTongueCollision));
		this.tongueCollisionJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.tongueCollisionJSON);
	}

	// Token: 0x06004DAD RID: 19885 RVA: 0x001B4884 File Offset: 0x001B2C84
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			DAZTongueControlUI componentInChildren = t.GetComponentInChildren<DAZTongueControlUI>(true);
			if (componentInChildren != null)
			{
				this.tongueCollisionJSON.RegisterToggle(componentInChildren.tongueCollisionToggle, isAlt);
			}
		}
	}

	// Token: 0x06004DAE RID: 19886 RVA: 0x001B48C3 File Offset: 0x001B2CC3
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x04003D65 RID: 15717
	public Transform[] tongueCollisionAutoGroups;

	// Token: 0x04003D66 RID: 15718
	public Rigidbody[] tongueCollisionRigidbodies;

	// Token: 0x04003D67 RID: 15719
	protected JSONStorableBool tongueCollisionJSON;

	// Token: 0x04003D68 RID: 15720
	[SerializeField]
	public bool _tongueCollision = true;
}
