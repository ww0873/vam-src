using System;
using System.Collections.Generic;
using MeshVR;
using UnityEngine;

// Token: 0x02000ABF RID: 2751
public class DAZClothingItemControl : JSONStorable
{
	// Token: 0x0600490D RID: 18701 RVA: 0x00172E37 File Offset: 0x00171237
	public DAZClothingItemControl()
	{
	}

	// Token: 0x0600490E RID: 18702 RVA: 0x00172E40 File Offset: 0x00171240
	protected void SyncClothingItem()
	{
		if (this._clothingItem != null)
		{
			this.lockedJSON = new JSONStorableBool("locked", this.clothingItem.locked, new JSONStorableBool.SetBoolCallback(this.SyncLocked));
			this.lockedJSON.isStorable = false;
			this.lockedJSON.isRestorable = false;
			base.RegisterBool(this.lockedJSON);
			this.disableAnatomyJSON = new JSONStorableBool("disableAnatomy", this.clothingItem.disableAnatomy, new JSONStorableBool.SetBoolCallback(this.SyncDisableAnatomy));
			base.RegisterBool(this.disableAnatomyJSON);
			this.isRealClothingItemJSON = new JSONStorableBool("isRealClothingItem", this.clothingItem.isRealItem, new JSONStorableBool.SetBoolCallback(this.SyncIsRealClothingItem));
			base.RegisterBool(this.isRealClothingItemJSON);
			if (this._clothingItem.gender == DAZDynamicItem.Gender.Female)
			{
				this.enableJointSpringAndDamperAdjustJSON = new JSONStorableBool("enableJointSpringAndDamperAdjust", this.clothingItem.jointAdjustEnabled, new JSONStorableBool.SetBoolCallback(this.SyncEnableJointSpringAndDamperAdjust));
				base.RegisterBool(this.enableJointSpringAndDamperAdjustJSON);
				this.enableBreastJointAdjustJSON = new JSONStorableBool("enableBreastJointAdjust", this.clothingItem.adjustFemaleBreastJointSpringAndDamper, new JSONStorableBool.SetBoolCallback(this.SyncEnableBreastJointAdjust));
				base.RegisterBool(this.enableBreastJointAdjustJSON);
				this.enableGluteJointAdjustJSON = new JSONStorableBool("enableGluteJointAdjust", this.clothingItem.adjustFemaleGluteJointSpringAndDamper, new JSONStorableBool.SetBoolCallback(this.SyncEnableGluteJointAdjust));
				base.RegisterBool(this.enableGluteJointAdjustJSON);
				this.breastJointSpringAndDamperMultiplierJSON = new JSONStorableFloat("breastJointSpringAndDamperMultiplier", this.clothingItem.breastJointSpringAndDamperMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncBreastJointSpringAndDamperMultiplier), 1f, 10f, true, true);
				base.RegisterFloat(this.breastJointSpringAndDamperMultiplierJSON);
				this.gluteJointSpringAndDamperMultiplierJSON = new JSONStorableFloat("gluteJointSpringAndDamperMultiplier", this.clothingItem.gluteJointSpringAndDamperMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncGluteJointSpringAndDamperMultiplier), 1f, 10f, true, true);
				base.RegisterFloat(this.gluteJointSpringAndDamperMultiplierJSON);
			}
		}
	}

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x0600490F RID: 18703 RVA: 0x00173037 File Offset: 0x00171437
	// (set) Token: 0x06004910 RID: 18704 RVA: 0x0017303F File Offset: 0x0017143F
	public DAZClothingItem clothingItem
	{
		get
		{
			return this._clothingItem;
		}
		set
		{
			if (this._clothingItem != value)
			{
				this._clothingItem = value;
				this.SyncClothingItem();
			}
		}
	}

	// Token: 0x06004911 RID: 18705 RVA: 0x0017305F File Offset: 0x0017145F
	public void Delete()
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.transform.SetParent(null);
			UnityEngine.Object.Destroy(this.clothingItem.gameObject);
		}
	}

	// Token: 0x06004912 RID: 18706 RVA: 0x00173093 File Offset: 0x00171493
	public void RefreshClothingItemThumbnail(string dynamicItemPath)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.RefreshClothingItemThumbnail(dynamicItemPath);
		}
	}

	// Token: 0x06004913 RID: 18707 RVA: 0x001730B2 File Offset: 0x001714B2
	public void RefreshClothingItems()
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.RefreshClothingItems();
		}
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x001730D0 File Offset: 0x001714D0
	public bool IsClothingUIDAvailable(string uid)
	{
		return this.clothingItem != null && this.clothingItem.IsClothingUIDAvailable(uid);
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x001730F1 File Offset: 0x001714F1
	public HashSet<string> GetAllClothingOtherTags()
	{
		if (this.clothingItem != null)
		{
			return this.clothingItem.GetAllClothingOtherTags();
		}
		return null;
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x00173111 File Offset: 0x00171511
	protected void SyncLocked(bool b)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.locked = b;
		}
		if (this.presetManagerControl != null)
		{
			this.presetManagerControl.lockParams = b;
		}
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x0017314D File Offset: 0x0017154D
	protected void SyncDisableAnatomy(bool b)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.disableAnatomy = b;
			this.clothingItem.SyncClothingAdjustments();
		}
	}

	// Token: 0x06004918 RID: 18712 RVA: 0x00173177 File Offset: 0x00171577
	protected void SyncIsRealClothingItem(bool b)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.isRealItem = b;
		}
	}

	// Token: 0x06004919 RID: 18713 RVA: 0x00173196 File Offset: 0x00171596
	public void ResetIsRealClothingItem()
	{
		if (this.isRealClothingItemJSON != null)
		{
			this.isRealClothingItemJSON.val = true;
			this.isRealClothingItemJSON.defaultVal = true;
		}
	}

	// Token: 0x0600491A RID: 18714 RVA: 0x001731BB File Offset: 0x001715BB
	protected void SyncEnableJointSpringAndDamperAdjust(bool b)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.jointAdjustEnabled = b;
		}
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x001731DA File Offset: 0x001715DA
	protected void SyncEnableBreastJointAdjust(bool b)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.adjustFemaleBreastJointSpringAndDamper = b;
			this.clothingItem.SyncClothingAdjustments();
		}
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x00173204 File Offset: 0x00171604
	protected void SyncEnableGluteJointAdjust(bool b)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.adjustFemaleGluteJointSpringAndDamper = b;
			this.clothingItem.SyncClothingAdjustments();
		}
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x0017322E File Offset: 0x0017162E
	protected void SyncBreastJointSpringAndDamperMultiplier(float f)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.breastJointSpringAndDamperMultiplier = f;
			this.clothingItem.SyncClothingAdjustments();
		}
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x00173258 File Offset: 0x00171658
	protected void SyncGluteJointSpringAndDamperMultiplier(float f)
	{
		if (this.clothingItem != null)
		{
			this.clothingItem.gluteJointSpringAndDamperMultiplier = f;
			this.clothingItem.SyncClothingAdjustments();
		}
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x00173282 File Offset: 0x00171682
	protected void Init()
	{
		this.presetManagerControl = base.GetComponent<PresetManagerControl>();
	}

	// Token: 0x06004920 RID: 18720 RVA: 0x00173290 File Offset: 0x00171690
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			DAZDynamicItemLabelUI componentInChildren = t.GetComponentInChildren<DAZDynamicItemLabelUI>(true);
			if (this._clothingItem != null && componentInChildren != null && componentInChildren.label != null)
			{
				componentInChildren.label.text = this._clothingItem.displayName;
			}
			DAZClothingItemControlUI componentInChildren2 = t.GetComponentInChildren<DAZClothingItemControlUI>(true);
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
				if (this.isRealClothingItemJSON != null)
				{
					if (componentInChildren2.isRealClothingItemToggle != null)
					{
						componentInChildren2.isRealClothingItemToggle.gameObject.SetActive(true);
					}
					this.isRealClothingItemJSON.RegisterToggle(componentInChildren2.isRealClothingItemToggle, isAlt);
				}
				else if (componentInChildren2.isRealClothingItemToggle != null)
				{
					componentInChildren2.isRealClothingItemToggle.gameObject.SetActive(false);
				}
				if (this.enableJointSpringAndDamperAdjustJSON != null)
				{
					if (componentInChildren2.enableJointSpringAndDamperAdjustToggle != null)
					{
						componentInChildren2.enableJointSpringAndDamperAdjustToggle.gameObject.SetActive(true);
					}
					this.enableJointSpringAndDamperAdjustJSON.RegisterToggle(componentInChildren2.enableJointSpringAndDamperAdjustToggle, isAlt);
				}
				else if (componentInChildren2.enableJointSpringAndDamperAdjustToggle != null)
				{
					componentInChildren2.enableJointSpringAndDamperAdjustToggle.gameObject.SetActive(false);
				}
				if (this.enableBreastJointAdjustJSON != null)
				{
					if (componentInChildren2.enableBreastJointAdjustToggle != null)
					{
						componentInChildren2.enableBreastJointAdjustToggle.gameObject.SetActive(true);
					}
					this.enableBreastJointAdjustJSON.RegisterToggle(componentInChildren2.enableBreastJointAdjustToggle, isAlt);
				}
				else if (componentInChildren2.enableBreastJointAdjustToggle != null)
				{
					componentInChildren2.enableBreastJointAdjustToggle.gameObject.SetActive(false);
				}
				if (this.enableGluteJointAdjustJSON != null)
				{
					if (componentInChildren2.enableGluteJointAdjustToggle != null)
					{
						componentInChildren2.enableGluteJointAdjustToggle.gameObject.SetActive(true);
					}
					this.enableGluteJointAdjustJSON.RegisterToggle(componentInChildren2.enableGluteJointAdjustToggle, isAlt);
				}
				else if (componentInChildren2.enableGluteJointAdjustToggle != null)
				{
					componentInChildren2.enableGluteJointAdjustToggle.gameObject.SetActive(false);
				}
				if (this.breastJointSpringAndDamperMultiplierJSON != null)
				{
					if (componentInChildren2.breastJointSpringAndDamperMultiplierSlider != null)
					{
						componentInChildren2.breastJointSpringAndDamperMultiplierSlider.transform.parent.gameObject.SetActive(true);
					}
					this.breastJointSpringAndDamperMultiplierJSON.RegisterSlider(componentInChildren2.breastJointSpringAndDamperMultiplierSlider, isAlt);
				}
				else if (componentInChildren2.breastJointSpringAndDamperMultiplierSlider != null)
				{
					componentInChildren2.breastJointSpringAndDamperMultiplierSlider.transform.parent.gameObject.SetActive(false);
				}
				if (this.gluteJointSpringAndDamperMultiplierJSON != null)
				{
					if (componentInChildren2.gluteJointSpringAndDamperMultiplierSlider != null)
					{
						componentInChildren2.gluteJointSpringAndDamperMultiplierSlider.transform.parent.gameObject.SetActive(true);
					}
					this.gluteJointSpringAndDamperMultiplierJSON.RegisterSlider(componentInChildren2.gluteJointSpringAndDamperMultiplierSlider, isAlt);
				}
				else if (componentInChildren2.gluteJointSpringAndDamperMultiplierSlider != null)
				{
					componentInChildren2.gluteJointSpringAndDamperMultiplierSlider.transform.parent.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x00173661 File Offset: 0x00171A61
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

	// Token: 0x04003791 RID: 14225
	protected DAZClothingItem _clothingItem;

	// Token: 0x04003792 RID: 14226
	protected PresetManagerControl presetManagerControl;

	// Token: 0x04003793 RID: 14227
	public JSONStorableBool lockedJSON;

	// Token: 0x04003794 RID: 14228
	public JSONStorableBool disableAnatomyJSON;

	// Token: 0x04003795 RID: 14229
	protected JSONStorableBool isRealClothingItemJSON;

	// Token: 0x04003796 RID: 14230
	protected JSONStorableBool enableJointSpringAndDamperAdjustJSON;

	// Token: 0x04003797 RID: 14231
	protected JSONStorableBool enableBreastJointAdjustJSON;

	// Token: 0x04003798 RID: 14232
	protected JSONStorableBool enableGluteJointAdjustJSON;

	// Token: 0x04003799 RID: 14233
	protected JSONStorableFloat breastJointSpringAndDamperMultiplierJSON;

	// Token: 0x0400379A RID: 14234
	protected JSONStorableFloat gluteJointSpringAndDamperMultiplierJSON;
}
