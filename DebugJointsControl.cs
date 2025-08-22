using System;
using UnityEngine;

// Token: 0x02000BCD RID: 3021
public class DebugJointsControl : JSONStorable
{
	// Token: 0x060055DA RID: 21978 RVA: 0x001F64E7 File Offset: 0x001F48E7
	public DebugJointsControl()
	{
	}

	// Token: 0x060055DB RID: 21979 RVA: 0x001F64EF File Offset: 0x001F48EF
	protected void SyncDebugJoints(bool b)
	{
		if (this.debugJoints != null)
		{
			this.debugJoints.showJoints = b;
		}
	}

	// Token: 0x060055DC RID: 21980 RVA: 0x001F650E File Offset: 0x001F490E
	protected void Init()
	{
		this.debugJointsJSON = new JSONStorableBool("debugJoints", false, new JSONStorableBool.SetBoolCallback(this.SyncDebugJoints));
		this.debugJointsJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.debugJointsJSON);
	}

	// Token: 0x060055DD RID: 21981 RVA: 0x001F6548 File Offset: 0x001F4948
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			DebugJointsControlUI componentInChildren = t.GetComponentInChildren<DebugJointsControlUI>(true);
			if (componentInChildren != null)
			{
				this.debugJointsJSON.RegisterToggle(componentInChildren.debugJointsToggle, isAlt);
			}
		}
	}

	// Token: 0x060055DE RID: 21982 RVA: 0x001F6587 File Offset: 0x001F4987
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

	// Token: 0x040046FE RID: 18174
	public DebugJoints debugJoints;

	// Token: 0x040046FF RID: 18175
	protected JSONStorableBool debugJointsJSON;
}
