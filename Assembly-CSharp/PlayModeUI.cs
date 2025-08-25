using System;
using UnityEngine;

// Token: 0x02000DD7 RID: 3543
public class PlayModeUI : JSONStorable
{
	// Token: 0x06006DBB RID: 28091 RVA: 0x00293F82 File Offset: 0x00292382
	public PlayModeUI()
	{
	}

	// Token: 0x06006DBC RID: 28092 RVA: 0x00293F8A File Offset: 0x0029238A
	protected void SyncEnabled(bool b)
	{
		if (this.uiRoot != null)
		{
			this.uiRoot.gameObject.SetActive(b);
		}
	}

	// Token: 0x06006DBD RID: 28093 RVA: 0x00293FB0 File Offset: 0x002923B0
	protected void SyncAlwaysVisible(bool b)
	{
		if (this.visibilityControl != null)
		{
			if (b)
			{
				if (this.useWorldPlacementJSON == null || !this.useWorldPlacementJSON.val)
				{
					this.visibilityControl.keepVisible = b;
				}
				else
				{
					this.visibilityControl.keepVisible = false;
				}
			}
			else
			{
				this.visibilityControl.keepVisible = false;
			}
		}
	}

	// Token: 0x06006DBE RID: 28094 RVA: 0x0029401D File Offset: 0x0029241D
	protected void SyncUseWorldPlacement(bool b)
	{
		if (this.anchorMover != null)
		{
			this.anchorMover.enabled = b;
		}
		if (this.uiController != null)
		{
			this.uiController.enabled = !b;
		}
	}

	// Token: 0x06006DBF RID: 28095 RVA: 0x0029405C File Offset: 0x0029245C
	protected void Init()
	{
		this.enabledJSON = new JSONStorableBool("enabled", this.startingEnabled, new JSONStorableBool.SetBoolCallback(this.SyncEnabled));
		base.RegisterBool(this.enabledJSON);
		this.alwaysVisibleJSON = new JSONStorableBool("alwaysVisible", this.startingAlwaysVisible, new JSONStorableBool.SetBoolCallback(this.SyncAlwaysVisible));
		base.RegisterBool(this.alwaysVisibleJSON);
		this.useWorldPlacementJSON = new JSONStorableBool("useWorldPlacement", this.startingUseWorldPlacement, new JSONStorableBool.SetBoolCallback(this.SyncUseWorldPlacement));
		base.RegisterBool(this.useWorldPlacementJSON);
	}

	// Token: 0x06006DC0 RID: 28096 RVA: 0x002940F3 File Offset: 0x002924F3
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x04005F02 RID: 24322
	public Transform uiRoot;

	// Token: 0x04005F03 RID: 24323
	public FreeControllerV3 uiController;

	// Token: 0x04005F04 RID: 24324
	public UIVisibility visibilityControl;

	// Token: 0x04005F05 RID: 24325
	public MoveAndRotateAsHUDAnchor anchorMover;

	// Token: 0x04005F06 RID: 24326
	public Canvas canvas;

	// Token: 0x04005F07 RID: 24327
	public bool startingEnabled;

	// Token: 0x04005F08 RID: 24328
	protected JSONStorableBool enabledJSON;

	// Token: 0x04005F09 RID: 24329
	public bool startingAlwaysVisible;

	// Token: 0x04005F0A RID: 24330
	protected JSONStorableBool alwaysVisibleJSON;

	// Token: 0x04005F0B RID: 24331
	public bool startingUseWorldPlacement;

	// Token: 0x04005F0C RID: 24332
	protected JSONStorableBool useWorldPlacementJSON;
}
