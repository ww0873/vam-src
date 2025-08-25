using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GPUTools.Cloth.Scripts.Runtime.Physics;
using MeshVR;
using MVR.FileManagement;
using UnityEngine;

// Token: 0x02000ABB RID: 2747
[ExecuteInEditMode]
public class DAZClothingItem : DAZDynamicItem
{
	// Token: 0x060048FC RID: 18684 RVA: 0x001726B6 File Offset: 0x00170AB6
	public DAZClothingItem()
	{
	}

	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x060048FD RID: 18685 RVA: 0x001726DB File Offset: 0x00170ADB
	// (set) Token: 0x060048FE RID: 18686 RVA: 0x001726E3 File Offset: 0x00170AE3
	public bool jointAdjustEnabled
	{
		get
		{
			return this._jointAdjustEnabled;
		}
		set
		{
			if (this._jointAdjustEnabled != value)
			{
				this._jointAdjustEnabled = value;
				this.SyncClothingAdjustments();
			}
		}
	}

	// Token: 0x060048FF RID: 18687 RVA: 0x001726FE File Offset: 0x00170AFE
	public void SyncClothingAdjustments()
	{
		if (this.characterSelector != null)
		{
			this.characterSelector.SyncClothingAdjustments();
		}
	}

	// Token: 0x06004900 RID: 18688 RVA: 0x0017271C File Offset: 0x00170B1C
	public void RefreshClothingItems()
	{
		if (this.characterSelector != null)
		{
			this.characterSelector.RefreshDynamicClothes();
		}
	}

	// Token: 0x06004901 RID: 18689 RVA: 0x0017273A File Offset: 0x00170B3A
	public void RefreshClothingItemThumbnail(string thumbPath)
	{
		if (this.characterSelector != null)
		{
			this.characterSelector.InvalidateDynamicClothingItemThumbnail(thumbPath);
			this.characterSelector.RefreshDynamicClothingThumbnails();
		}
	}

	// Token: 0x06004902 RID: 18690 RVA: 0x00172764 File Offset: 0x00170B64
	public bool IsClothingUIDAvailable(string uid)
	{
		return this.characterSelector != null && this.characterSelector.IsClothingUIDAvailable(uid);
	}

	// Token: 0x06004903 RID: 18691 RVA: 0x00172785 File Offset: 0x00170B85
	protected override void SyncOtherTags()
	{
		this.otherTags = this.GetAllClothingOtherTags();
		base.SyncOtherTags();
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x00172799 File Offset: 0x00170B99
	public HashSet<string> GetAllClothingOtherTags()
	{
		if (this.characterSelector != null)
		{
			return this.characterSelector.GetClothingOtherTags();
		}
		return null;
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x001727BC File Offset: 0x00170BBC
	protected override void SetHidePath()
	{
		if (this.dynamicRuntimeLoadPath != null && this.dynamicRuntimeLoadPath != string.Empty)
		{
			FileEntry fileEntry = FileManager.GetFileEntry(this.dynamicRuntimeLoadPath, false);
			this.hidePath = fileEntry.hidePath;
		}
		else if (this.assetName != null && this.assetName != string.Empty)
		{
			if (this.gender == DAZDynamicItem.Gender.Female)
			{
				this.hidePath = string.Concat(new string[]
				{
					"Custom/Clothing/Female/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".hide"
				});
			}
			else if (this.gender == DAZDynamicItem.Gender.Male)
			{
				this.hidePath = string.Concat(new string[]
				{
					"Custom/Clothing/Male/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".hide"
				});
			}
			else
			{
				this.hidePath = string.Concat(new string[]
				{
					"Custom/Clothing/Neutral/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".hide"
				});
			}
		}
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x001728FC File Offset: 0x00170CFC
	protected override void SetUserPrefsPath()
	{
		if (this.dynamicRuntimeLoadPath != null && this.dynamicRuntimeLoadPath != string.Empty)
		{
			string input = FileManager.RemovePackageFromPath(this.dynamicRuntimeLoadPath);
			this.userPrefsPath = Regex.Replace(input, "\\.vam$", ".prefs");
		}
		else if (this.assetName != null && this.assetName != string.Empty)
		{
			if (this.gender == DAZDynamicItem.Gender.Female)
			{
				this.userPrefsPath = string.Concat(new string[]
				{
					"Custom/Clothing/Female/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".prefs"
				});
			}
			else if (this.gender == DAZDynamicItem.Gender.Male)
			{
				this.userPrefsPath = string.Concat(new string[]
				{
					"Custom/Clothing/Male/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".prefs"
				});
			}
			else
			{
				this.userPrefsPath = string.Concat(new string[]
				{
					"Custom/Clothing/Neutral/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".prefs"
				});
			}
		}
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x00172A44 File Offset: 0x00170E44
	public void SetLocked(bool b)
	{
		this.locked = b;
		if (this.clothingItemControls != null)
		{
			foreach (DAZClothingItemControl dazclothingItemControl in this.clothingItemControls)
			{
				if (dazclothingItemControl.lockedJSON != null)
				{
					dazclothingItemControl.lockedJSON.val = b;
				}
			}
		}
	}

	// Token: 0x06004908 RID: 18696 RVA: 0x00172A9C File Offset: 0x00170E9C
	protected override void InitInstance()
	{
		base.InitInstance();
		this.clothingItemControls = base.GetComponentsInChildren<DAZClothingItemControl>(true);
		if (this.clothingItemControls != null)
		{
			foreach (DAZClothingItemControl dazclothingItemControl in this.clothingItemControls)
			{
				dazclothingItemControl.clothingItem = this;
			}
		}
		DAZDynamic componentInChildren = base.GetComponentInChildren<DAZDynamic>(true);
		if (componentInChildren != null)
		{
			if (this.gender == DAZDynamicItem.Gender.Female)
			{
				componentInChildren.itemType = PresetManager.ItemType.ClothingFemale;
			}
			else if (this.gender == DAZDynamicItem.Gender.Male)
			{
				componentInChildren.itemType = PresetManager.ItemType.ClothingMale;
			}
			else
			{
				componentInChildren.itemType = PresetManager.ItemType.ClothingNeutral;
			}
			if (this.dynamicRuntimeLoadPath != null && this.dynamicRuntimeLoadPath != string.Empty)
			{
				componentInChildren.SetNamesFromPath(this.dynamicRuntimeLoadPath);
				componentInChildren.Load(false);
			}
		}
		PresetManagerControl componentInChildren2 = base.GetComponentInChildren<PresetManagerControl>(true);
		if (componentInChildren2 != null)
		{
			componentInChildren2.SyncPresetUI();
		}
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x00172B8C File Offset: 0x00170F8C
	public override void PartialResetPhysics()
	{
		if (base.enabled)
		{
			ClothPhysics[] componentsInChildren = base.GetComponentsInChildren<ClothPhysics>(true);
			foreach (ClothPhysics clothPhysics in componentsInChildren)
			{
				clothPhysics.PartialResetPhysics();
			}
		}
	}

	// Token: 0x0600490A RID: 18698 RVA: 0x00172BCC File Offset: 0x00170FCC
	public override void ResetPhysics()
	{
		if (base.enabled)
		{
			ClothPhysics[] componentsInChildren = base.GetComponentsInChildren<ClothPhysics>(true);
			foreach (ClothPhysics clothPhysics in componentsInChildren)
			{
				clothPhysics.ResetPhysics();
			}
		}
	}

	// Token: 0x0600490B RID: 18699 RVA: 0x00172C0C File Offset: 0x0017100C
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.colliderRight != null)
		{
			this.colliderRight.gameObject.SetActive(true);
			this.colliderRight.transform.localEulerAngles = this.colliderRightRotation;
			this.colliderRight.size = this.colliderDimensions;
			this.colliderRight.center = this.colliderRightCenter;
		}
		if (this.colliderLeft != null)
		{
			this.colliderLeft.gameObject.SetActive(true);
			this.colliderLeft.transform.localEulerAngles = this.colliderLeftRotation;
			this.colliderLeft.size = this.colliderDimensions;
			this.colliderLeft.center = this.colliderLeftCenter;
		}
		if (this.driveXAngleTargetController1 != null)
		{
			this.driveXAngleTargetController1.jointRotationDriveXTarget = this.driveXAngleTarget;
		}
		if (this.driveXAngleTargetController2 != null)
		{
			this.driveXAngleTargetController2.jointRotationDriveXTarget = this.driveXAngleTarget;
		}
		if (this.drive2XAngleTargetController1 != null)
		{
			this.drive2XAngleTargetController1.jointRotationDriveXTarget = this.drive2XAngleTarget;
		}
		if (this.drive2XAngleTargetController2 != null)
		{
			this.drive2XAngleTargetController2.jointRotationDriveXTarget = this.drive2XAngleTarget;
		}
	}

	// Token: 0x0600490C RID: 18700 RVA: 0x00172D5C File Offset: 0x0017115C
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.colliderRight != null)
		{
			this.colliderRight.gameObject.SetActive(false);
		}
		if (this.colliderLeft != null)
		{
			this.colliderLeft.gameObject.SetActive(false);
		}
		if (this.driveXAngleTargetController1 != null)
		{
			this.driveXAngleTargetController1.jointRotationDriveXTarget = 0f;
		}
		if (this.driveXAngleTargetController2 != null)
		{
			this.driveXAngleTargetController2.jointRotationDriveXTarget = 0f;
		}
		if (this.drive2XAngleTargetController1 != null)
		{
			this.drive2XAngleTargetController1.jointRotationDriveXTarget = 0f;
		}
		if (this.drive2XAngleTargetController2 != null)
		{
			this.drive2XAngleTargetController2.jointRotationDriveXTarget = 0f;
		}
	}

	// Token: 0x04003763 RID: 14179
	[SerializeField]
	protected bool _jointAdjustEnabled = true;

	// Token: 0x04003764 RID: 14180
	public bool adjustFemaleBreastJointSpringAndDamper;

	// Token: 0x04003765 RID: 14181
	public float breastJointSpringAndDamperMultiplier = 3f;

	// Token: 0x04003766 RID: 14182
	public bool adjustFemaleGluteJointSpringAndDamper;

	// Token: 0x04003767 RID: 14183
	public float gluteJointSpringAndDamperMultiplier = 3f;

	// Token: 0x04003768 RID: 14184
	public DAZClothingItem.ExclusiveRegion exclusiveRegion;

	// Token: 0x04003769 RID: 14185
	public DAZClothingItem.ColliderType colliderTypeRight;

	// Token: 0x0400376A RID: 14186
	public DAZClothingItem.ColliderType colliderTypeLeft;

	// Token: 0x0400376B RID: 14187
	public BoxCollider colliderRight;

	// Token: 0x0400376C RID: 14188
	public BoxCollider colliderLeft;

	// Token: 0x0400376D RID: 14189
	public Vector3 colliderDimensions;

	// Token: 0x0400376E RID: 14190
	public Vector3 colliderRightCenter;

	// Token: 0x0400376F RID: 14191
	public Vector3 colliderLeftCenter;

	// Token: 0x04003770 RID: 14192
	public Vector3 colliderRightRotation;

	// Token: 0x04003771 RID: 14193
	public Vector3 colliderLeftRotation;

	// Token: 0x04003772 RID: 14194
	public DAZClothingItem.ControllerType driveXAngleTargetController1Type;

	// Token: 0x04003773 RID: 14195
	public DAZClothingItem.ControllerType driveXAngleTargetController2Type;

	// Token: 0x04003774 RID: 14196
	public FreeControllerV3 driveXAngleTargetController1;

	// Token: 0x04003775 RID: 14197
	public FreeControllerV3 driveXAngleTargetController2;

	// Token: 0x04003776 RID: 14198
	public float driveXAngleTarget;

	// Token: 0x04003777 RID: 14199
	public DAZClothingItem.ControllerType drive2XAngleTargetController1Type;

	// Token: 0x04003778 RID: 14200
	public DAZClothingItem.ControllerType drive2XAngleTargetController2Type;

	// Token: 0x04003779 RID: 14201
	public FreeControllerV3 drive2XAngleTargetController1;

	// Token: 0x0400377A RID: 14202
	public FreeControllerV3 drive2XAngleTargetController2;

	// Token: 0x0400377B RID: 14203
	public float drive2XAngleTarget;

	// Token: 0x0400377C RID: 14204
	protected DAZClothingItemControl[] clothingItemControls;

	// Token: 0x02000ABC RID: 2748
	public enum ExclusiveRegion
	{
		// Token: 0x0400377E RID: 14206
		None,
		// Token: 0x0400377F RID: 14207
		UnderHip,
		// Token: 0x04003780 RID: 14208
		UnderChest,
		// Token: 0x04003781 RID: 14209
		Hip,
		// Token: 0x04003782 RID: 14210
		Chest,
		// Token: 0x04003783 RID: 14211
		Shoes,
		// Token: 0x04003784 RID: 14212
		Glasses,
		// Token: 0x04003785 RID: 14213
		Gloves,
		// Token: 0x04003786 RID: 14214
		Hat,
		// Token: 0x04003787 RID: 14215
		Legs
	}

	// Token: 0x02000ABD RID: 2749
	public enum ColliderType
	{
		// Token: 0x04003789 RID: 14217
		None,
		// Token: 0x0400378A RID: 14218
		Shoe
	}

	// Token: 0x02000ABE RID: 2750
	public enum ControllerType
	{
		// Token: 0x0400378C RID: 14220
		None,
		// Token: 0x0400378D RID: 14221
		LeftFoot,
		// Token: 0x0400378E RID: 14222
		RightFoot,
		// Token: 0x0400378F RID: 14223
		LeftToe,
		// Token: 0x04003790 RID: 14224
		RightToe
	}
}
