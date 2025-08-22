using System;
using UnityEngine;

// Token: 0x02000B45 RID: 2885
public class DAZPhysicsMeshesEnable : JSONStorable
{
	// Token: 0x06004FB7 RID: 20407 RVA: 0x001C7C2E File Offset: 0x001C602E
	public DAZPhysicsMeshesEnable()
	{
	}

	// Token: 0x06004FB8 RID: 20408 RVA: 0x001C7C40 File Offset: 0x001C6040
	protected void SyncEnabled(bool b)
	{
		this._enabled = b;
		if (this.physicsMeshesToControl != null)
		{
			foreach (DAZPhysicsMesh dazphysicsMesh in this.physicsMeshesToControl)
			{
				dazphysicsMesh.alternateOn = this._enabled;
			}
		}
	}

	// Token: 0x06004FB9 RID: 20409 RVA: 0x001C7C8A File Offset: 0x001C608A
	protected void Init()
	{
		this.enabledJSON = new JSONStorableBool("enabled", this._enabled, new JSONStorableBool.SetBoolCallback(this.SyncEnabled));
		base.RegisterBool(this.enabledJSON);
	}

	// Token: 0x06004FBA RID: 20410 RVA: 0x001C7CBC File Offset: 0x001C60BC
	protected override void InitUI(Transform t, bool isAlt)
	{
		base.InitUI(t, isAlt);
		if (t != null)
		{
			DAZPhysicsMeshesEnableUI componentInChildren = t.GetComponentInChildren<DAZPhysicsMeshesEnableUI>(true);
			if (componentInChildren != null)
			{
				this.enabledJSON.RegisterToggle(componentInChildren.enabledToggle, isAlt);
			}
		}
	}

	// Token: 0x06004FBB RID: 20411 RVA: 0x001C7D03 File Offset: 0x001C6103
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

	// Token: 0x04003F9A RID: 16282
	public DAZPhysicsMesh[] physicsMeshesToControl;

	// Token: 0x04003F9B RID: 16283
	protected bool _enabled = true;

	// Token: 0x04003F9C RID: 16284
	public JSONStorableBool enabledJSON;
}
