using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D6E RID: 3438
public class HairSimControlTools : MonoBehaviour
{
	// Token: 0x06006A11 RID: 27153 RVA: 0x0027EEDC File Offset: 0x0027D2DC
	public HairSimControlTools()
	{
	}

	// Token: 0x06006A12 RID: 27154 RVA: 0x0027EEE4 File Offset: 0x0027D2E4
	public void SetOnlyToolsControllable(bool b)
	{
		if (SuperController.singleton != null)
		{
			if (b)
			{
				SuperController.singleton.SetOnlyShowControllers(this.controllers);
			}
			else
			{
				SuperController.singleton.SetOnlyShowControllers(null);
			}
		}
	}

	// Token: 0x06006A13 RID: 27155 RVA: 0x0027EF1C File Offset: 0x0027D31C
	public void SetHairStyleToolVisibility(bool tool1Visibility, bool tool2Visibility, bool tool3Visibility, bool tool4Visibility)
	{
		this.SetHairStyleTool1Visible(tool1Visibility);
		this.SetHairStyleTool2Visible(tool2Visibility);
		this.SetHairStyleTool3Visible(tool3Visibility);
		this.SetHairStyleTool4Visible(tool4Visibility);
	}

	// Token: 0x06006A14 RID: 27156 RVA: 0x0027EF3B File Offset: 0x0027D33B
	public void SetHairStyleTool1Visible(bool b)
	{
		if (this.hairStyleTool1 != null)
		{
			this.hairStyleTool1.gameObject.SetActive(b);
		}
	}

	// Token: 0x06006A15 RID: 27157 RVA: 0x0027EF5F File Offset: 0x0027D35F
	public void SetHairStyleTool2Visible(bool b)
	{
		if (this.hairStyleTool2 != null)
		{
			this.hairStyleTool2.gameObject.SetActive(b);
		}
	}

	// Token: 0x06006A16 RID: 27158 RVA: 0x0027EF83 File Offset: 0x0027D383
	public void SetHairStyleTool3Visible(bool b)
	{
		if (this.hairStyleTool3 != null)
		{
			this.hairStyleTool3.gameObject.SetActive(b);
		}
	}

	// Token: 0x06006A17 RID: 27159 RVA: 0x0027EFA7 File Offset: 0x0027D3A7
	public void SetHairStyleTool4Visible(bool b)
	{
		if (this.hairStyleTool4 != null)
		{
			this.hairStyleTool4.gameObject.SetActive(b);
		}
	}

	// Token: 0x06006A18 RID: 27160 RVA: 0x0027EFCB File Offset: 0x0027D3CB
	public void SetScalpMaskToolVisible(bool b)
	{
		if (this.hairScalpMaskTool != null)
		{
			this.hairScalpMaskTool.gameObject.SetActive(b);
		}
	}

	// Token: 0x06006A19 RID: 27161 RVA: 0x0027EFF0 File Offset: 0x0027D3F0
	public void SetAllowRigidityPaint(bool b)
	{
		if (this.StyleTool1 != null)
		{
			this.StyleTool1.allowRigidityPaint = b;
		}
		if (this.StyleTool2 != null)
		{
			this.StyleTool2.allowRigidityPaint = b;
		}
		if (this.StyleTool3 != null)
		{
			this.StyleTool3.allowRigidityPaint = b;
		}
		if (this.StyleTool4 != null)
		{
			this.StyleTool4.allowRigidityPaint = b;
		}
	}

	// Token: 0x06006A1A RID: 27162 RVA: 0x0027F074 File Offset: 0x0027D474
	private void Awake()
	{
		this.SetHairStyleToolVisibility(false, false, false, false);
		this.SetScalpMaskToolVisible(false);
		this.controllers = new HashSet<FreeControllerV3>();
		if (this.hairStyleTool1Controller != null)
		{
			this.controllers.Add(this.hairStyleTool1Controller);
		}
		if (this.hairStyleTool1UIController != null)
		{
			this.controllers.Add(this.hairStyleTool1UIController);
		}
		if (this.hairStyleTool2Controller != null)
		{
			this.controllers.Add(this.hairStyleTool2Controller);
		}
		if (this.hairStyleTool2UIController != null)
		{
			this.controllers.Add(this.hairStyleTool2UIController);
		}
		if (this.hairStyleTool3Controller != null)
		{
			this.controllers.Add(this.hairStyleTool3Controller);
		}
		if (this.hairStyleTool3UIController != null)
		{
			this.controllers.Add(this.hairStyleTool3UIController);
		}
		if (this.hairStyleTool4Controller != null)
		{
			this.controllers.Add(this.hairStyleTool4Controller);
		}
		if (this.hairStyleTool4UIController != null)
		{
			this.controllers.Add(this.hairStyleTool4UIController);
		}
		if (this.hairScalpMaskToolController != null)
		{
			this.controllers.Add(this.hairScalpMaskToolController);
		}
		if (this.hairScalpMaskToolUIController != null)
		{
			this.controllers.Add(this.hairScalpMaskToolUIController);
		}
	}

	// Token: 0x04005B86 RID: 23430
	public Transform hairStyleTool1;

	// Token: 0x04005B87 RID: 23431
	public HairSimStyleToolControl StyleTool1;

	// Token: 0x04005B88 RID: 23432
	public FreeControllerV3 hairStyleTool1Controller;

	// Token: 0x04005B89 RID: 23433
	public FreeControllerV3 hairStyleTool1UIController;

	// Token: 0x04005B8A RID: 23434
	public Transform hairStyleTool2;

	// Token: 0x04005B8B RID: 23435
	public HairSimStyleToolControl StyleTool2;

	// Token: 0x04005B8C RID: 23436
	public FreeControllerV3 hairStyleTool2Controller;

	// Token: 0x04005B8D RID: 23437
	public FreeControllerV3 hairStyleTool2UIController;

	// Token: 0x04005B8E RID: 23438
	public Transform hairStyleTool3;

	// Token: 0x04005B8F RID: 23439
	public HairSimStyleToolControl StyleTool3;

	// Token: 0x04005B90 RID: 23440
	public FreeControllerV3 hairStyleTool3Controller;

	// Token: 0x04005B91 RID: 23441
	public FreeControllerV3 hairStyleTool3UIController;

	// Token: 0x04005B92 RID: 23442
	public Transform hairStyleTool4;

	// Token: 0x04005B93 RID: 23443
	public HairSimStyleToolControl StyleTool4;

	// Token: 0x04005B94 RID: 23444
	public FreeControllerV3 hairStyleTool4Controller;

	// Token: 0x04005B95 RID: 23445
	public FreeControllerV3 hairStyleTool4UIController;

	// Token: 0x04005B96 RID: 23446
	public Transform hairScalpMaskTool;

	// Token: 0x04005B97 RID: 23447
	public FreeControllerV3 hairScalpMaskToolController;

	// Token: 0x04005B98 RID: 23448
	public FreeControllerV3 hairScalpMaskToolUIController;

	// Token: 0x04005B99 RID: 23449
	protected HashSet<FreeControllerV3> controllers;
}
