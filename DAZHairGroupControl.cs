using System;
using System.Collections.Generic;
using MeshVR;
using UnityEngine;

// Token: 0x02000AD4 RID: 2772
public class DAZHairGroupControl : JSONStorable
{
	// Token: 0x0600499E RID: 18846 RVA: 0x0017AB94 File Offset: 0x00178F94
	public DAZHairGroupControl()
	{
	}

	// Token: 0x0600499F RID: 18847 RVA: 0x0017AB9C File Offset: 0x00178F9C
	protected void SyncHairGroup()
	{
		if (this._hairItem != null)
		{
			this.lockedJSON = new JSONStorableBool("locked", this._hairItem.locked, new JSONStorableBool.SetBoolCallback(this.SyncLocked));
			this.lockedJSON.isStorable = false;
			this.lockedJSON.isRestorable = false;
			base.RegisterBool(this.lockedJSON);
			this.disableAnatomyJSON = new JSONStorableBool("disableAnatomy", this._hairItem.disableAnatomy, new JSONStorableBool.SetBoolCallback(this.SyncDisableAnatomy));
			base.RegisterBool(this.disableAnatomyJSON);
		}
	}

	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x060049A0 RID: 18848 RVA: 0x0017AC38 File Offset: 0x00179038
	// (set) Token: 0x060049A1 RID: 18849 RVA: 0x0017AC40 File Offset: 0x00179040
	public DAZHairGroup hairItem
	{
		get
		{
			return this._hairItem;
		}
		set
		{
			if (this._hairItem != value)
			{
				this._hairItem = value;
				this.SyncHairGroup();
			}
		}
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x0017AC60 File Offset: 0x00179060
	public void Delete()
	{
		if (this.hairItem != null)
		{
			this.hairItem.transform.SetParent(null);
			UnityEngine.Object.Destroy(this.hairItem.gameObject);
		}
	}

	// Token: 0x060049A3 RID: 18851 RVA: 0x0017AC94 File Offset: 0x00179094
	public void RefreshHairItemThumbnail(string dynamicItemPath)
	{
		if (this._hairItem != null)
		{
			this._hairItem.RefreshHairItemThumbnail(dynamicItemPath);
		}
	}

	// Token: 0x060049A4 RID: 18852 RVA: 0x0017ACB3 File Offset: 0x001790B3
	public void RefreshHairItems()
	{
		if (this._hairItem != null)
		{
			this._hairItem.RefreshHairItems();
		}
	}

	// Token: 0x060049A5 RID: 18853 RVA: 0x0017ACD1 File Offset: 0x001790D1
	public bool IsHairItemUIDAvailable(string uid)
	{
		return this._hairItem != null && this._hairItem.IsHairUIDAvailable(uid);
	}

	// Token: 0x060049A6 RID: 18854 RVA: 0x0017ACF2 File Offset: 0x001790F2
	public HashSet<string> GetAllHairOtherTags()
	{
		if (this._hairItem != null)
		{
			return this._hairItem.GetAllHairOtherTags();
		}
		return null;
	}

	// Token: 0x060049A7 RID: 18855 RVA: 0x0017AD12 File Offset: 0x00179112
	protected void SyncLocked(bool b)
	{
		if (this._hairItem != null)
		{
			this._hairItem.locked = b;
		}
		if (this.presetManagerControl != null)
		{
			this.presetManagerControl.lockParams = b;
		}
	}

	// Token: 0x060049A8 RID: 18856 RVA: 0x0017AD4E File Offset: 0x0017914E
	protected void SyncDisableAnatomy(bool b)
	{
		if (this._hairItem != null)
		{
			this._hairItem.disableAnatomy = b;
			this._hairItem.SyncHairAdjustments();
		}
	}

	// Token: 0x060049A9 RID: 18857 RVA: 0x0017AD78 File Offset: 0x00179178
	protected void Init()
	{
		this.presetManagerControl = base.GetComponent<PresetManagerControl>();
	}

	// Token: 0x060049AA RID: 18858 RVA: 0x0017AD88 File Offset: 0x00179188
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			DAZDynamicItemLabelUI componentInChildren = t.GetComponentInChildren<DAZDynamicItemLabelUI>(true);
			if (this._hairItem != null && componentInChildren != null && componentInChildren.label != null)
			{
				componentInChildren.label.text = this._hairItem.displayName;
			}
			ObjectChoice[] componentsInChildren = base.GetComponentsInChildren<ObjectChoice>(true);
			foreach (ObjectChoice objectChoice in componentsInChildren)
			{
				JSONStorable[] componentsInChildren2 = objectChoice.GetComponentsInChildren<JSONStorable>(true);
				foreach (JSONStorable jsonstorable in componentsInChildren2)
				{
					if (isAlt)
					{
						jsonstorable.SetUIAlt(t);
					}
					else
					{
						jsonstorable.SetUI(t);
					}
				}
			}
			DAZHairGroupControlUI componentInChildren2 = t.GetComponentInChildren<DAZHairGroupControlUI>(true);
			if (componentInChildren2 != null)
			{
				if (this.lockedJSON != null)
				{
					if (componentInChildren2.lockedToggle != null)
					{
						componentInChildren2.lockedToggle.gameObject.SetActive(true);
					}
					this.lockedJSON.RegisterToggle(componentInChildren2.lockedToggle, isAlt);
				}
				else if (componentInChildren2.lockedToggle != null)
				{
					componentInChildren2.lockedToggle.gameObject.SetActive(false);
				}
				if (this.disableAnatomyJSON != null)
				{
					if (componentInChildren2.disableAnatomyToggle != null)
					{
						componentInChildren2.disableAnatomyToggle.gameObject.SetActive(true);
					}
					this.disableAnatomyJSON.RegisterToggle(componentInChildren2.disableAnatomyToggle, isAlt);
				}
				else if (componentInChildren2.disableAnatomyToggle != null)
				{
					componentInChildren2.disableAnatomyToggle.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060049AB RID: 18859 RVA: 0x0017AF48 File Offset: 0x00179348
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

	// Token: 0x0400381B RID: 14363
	protected DAZHairGroup _hairItem;

	// Token: 0x0400381C RID: 14364
	protected PresetManagerControl presetManagerControl;

	// Token: 0x0400381D RID: 14365
	public JSONStorableBool lockedJSON;

	// Token: 0x0400381E RID: 14366
	public JSONStorableBool disableAnatomyJSON;
}
