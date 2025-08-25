using System;
using UnityEngine;

// Token: 0x02000ACC RID: 2764
public class DAZDynamicItemLabel : JSONStorable
{
	// Token: 0x0600497C RID: 18812 RVA: 0x0017A063 File Offset: 0x00178463
	public DAZDynamicItemLabel()
	{
	}

	// Token: 0x0600497D RID: 18813 RVA: 0x0017A06B File Offset: 0x0017846B
	protected void Init()
	{
	}

	// Token: 0x0600497E RID: 18814 RVA: 0x0017A070 File Offset: 0x00178470
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			DAZDynamicItem componentInParent = base.GetComponentInParent<DAZDynamicItem>();
			DAZDynamicItemLabelUI componentInChildren = t.GetComponentInChildren<DAZDynamicItemLabelUI>(true);
			if (componentInParent != null && componentInChildren != null && componentInChildren.label != null)
			{
				componentInChildren.label.text = componentInParent.displayName;
			}
		}
	}

	// Token: 0x0600497F RID: 18815 RVA: 0x0017A0D2 File Offset: 0x001784D2
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
}
