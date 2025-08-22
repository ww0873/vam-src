using System;
using UnityEngine;

// Token: 0x02000A98 RID: 2712
public class AnimatedDAZBoneDirectDrives : JSONStorable
{
	// Token: 0x0600466C RID: 18028 RVA: 0x00144B09 File Offset: 0x00142F09
	public AnimatedDAZBoneDirectDrives()
	{
	}

	// Token: 0x0600466D RID: 18029 RVA: 0x00144B18 File Offset: 0x00142F18
	protected void SyncOn(bool b)
	{
		this._on = b;
		if (!this._on)
		{
			foreach (AnimatedDAZBoneDirectDrive animatedDAZBoneDirectDrive in this.drives)
			{
				animatedDAZBoneDirectDrive.RestoreBoneControl();
			}
		}
	}

	// Token: 0x0600466E RID: 18030 RVA: 0x00144B5C File Offset: 0x00142F5C
	public void AutoSetDrives()
	{
		this.drives = base.GetComponentsInChildren<AnimatedDAZBoneDirectDrive>(true);
	}

	// Token: 0x0600466F RID: 18031 RVA: 0x00144B6C File Offset: 0x00142F6C
	protected void Init()
	{
		this.onJSON = new JSONStorableBool("on", this._on, new JSONStorableBool.SetBoolCallback(this.SyncOn));
		this.onJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.onJSON);
		foreach (AnimatedDAZBoneDirectDrive animatedDAZBoneDirectDrive in this.drives)
		{
			animatedDAZBoneDirectDrive.Init();
		}
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x00144BD8 File Offset: 0x00142FD8
	protected void OnDisable()
	{
		foreach (AnimatedDAZBoneDirectDrive animatedDAZBoneDirectDrive in this.drives)
		{
			animatedDAZBoneDirectDrive.RestoreBoneControl();
		}
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x00144C0C File Offset: 0x0014300C
	protected void Start()
	{
		foreach (AnimatedDAZBoneDirectDrive animatedDAZBoneDirectDrive in this.drives)
		{
			animatedDAZBoneDirectDrive.InitParent();
		}
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x00144C40 File Offset: 0x00143040
	protected void Update()
	{
		if (this._on)
		{
			foreach (AnimatedDAZBoneDirectDrive animatedDAZBoneDirectDrive in this.drives)
			{
				animatedDAZBoneDirectDrive.Prep();
				animatedDAZBoneDirectDrive.ThreadedUpdate();
				animatedDAZBoneDirectDrive.Finish();
			}
		}
	}

	// Token: 0x06004673 RID: 18035 RVA: 0x00144C8C File Offset: 0x0014308C
	protected override void InitUI(Transform t, bool isAlt)
	{
		this.Awake();
		if (t != null)
		{
			AnimatedDAZBoneDirectDrivesUI componentInChildren = t.GetComponentInChildren<AnimatedDAZBoneDirectDrivesUI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.RegisterToggle(componentInChildren.onToggle, isAlt);
			}
		}
	}

	// Token: 0x06004674 RID: 18036 RVA: 0x00144CD1 File Offset: 0x001430D1
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

	// Token: 0x040033D3 RID: 13267
	[SerializeField]
	protected bool _on = true;

	// Token: 0x040033D4 RID: 13268
	protected JSONStorableBool onJSON;

	// Token: 0x040033D5 RID: 13269
	[HideInInspector]
	public AnimatedDAZBoneDirectDrive[] drives;
}
