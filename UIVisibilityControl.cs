using System;
using UnityEngine;

// Token: 0x02000E10 RID: 3600
public class UIVisibilityControl : JSONStorable
{
	// Token: 0x06006F02 RID: 28418 RVA: 0x0029A4B0 File Offset: 0x002988B0
	public UIVisibilityControl()
	{
	}

	// Token: 0x06006F03 RID: 28419 RVA: 0x0029A4B8 File Offset: 0x002988B8
	private void Update()
	{
		if (SuperController.singleton != null && this.objectToControl != null)
		{
			this.objectToControl.SetActive(!this.onlyVisibleWhenMainUIOpenJSON.val || SuperController.singleton.MainHUDVisible);
		}
	}

	// Token: 0x06006F04 RID: 28420 RVA: 0x0029A510 File Offset: 0x00298910
	protected override void InitUI(Transform t, bool isAlt)
	{
		base.InitUI(t, isAlt);
		if (t != null)
		{
			UIVisibilityControlUI componentInChildren = t.GetComponentInChildren<UIVisibilityControlUI>(true);
			if (componentInChildren != null)
			{
				this.onlyVisibleWhenMainUIOpenJSON.RegisterToggle(componentInChildren.onlyVisibleWhenMainUIOpenToggle, false);
			}
		}
	}

	// Token: 0x06006F05 RID: 28421 RVA: 0x0029A557 File Offset: 0x00298957
	protected void Init()
	{
		this.onlyVisibleWhenMainUIOpenJSON = new JSONStorableBool("onlyVisibleWhenMainUIOpen", false);
		base.RegisterBool(this.onlyVisibleWhenMainUIOpenJSON);
	}

	// Token: 0x06006F06 RID: 28422 RVA: 0x0029A576 File Offset: 0x00298976
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

	// Token: 0x04006004 RID: 24580
	public GameObject objectToControl;

	// Token: 0x04006005 RID: 24581
	public JSONStorableBool onlyVisibleWhenMainUIOpenJSON;
}
