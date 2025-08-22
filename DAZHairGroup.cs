using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GPUTools.Hair.Scripts.Runtime.Physics;
using GPUTools.Skinner.Scripts.Providers;
using MeshVR;
using MVR.FileManagement;
using UnityEngine;

// Token: 0x02000AD3 RID: 2771
[ExecuteInEditMode]
public class DAZHairGroup : DAZDynamicItem
{
	// Token: 0x06004990 RID: 18832 RVA: 0x0017A514 File Offset: 0x00178914
	public DAZHairGroup()
	{
	}

	// Token: 0x06004991 RID: 18833 RVA: 0x0017A51C File Offset: 0x0017891C
	public void SyncHairAdjustments()
	{
		if (this.characterSelector != null)
		{
			this.characterSelector.SyncHairAdjustments();
		}
	}

	// Token: 0x06004992 RID: 18834 RVA: 0x0017A53A File Offset: 0x0017893A
	public void RefreshHairItems()
	{
		if (this.characterSelector != null)
		{
			this.characterSelector.RefreshDynamicHair();
		}
	}

	// Token: 0x06004993 RID: 18835 RVA: 0x0017A558 File Offset: 0x00178958
	public void RefreshHairItemThumbnail(string thumbPath)
	{
		if (this.characterSelector != null)
		{
			this.characterSelector.InvalidateDynamicHairItemThumbnail(thumbPath);
			this.characterSelector.RefreshDynamicHairThumbnails();
		}
	}

	// Token: 0x06004994 RID: 18836 RVA: 0x0017A582 File Offset: 0x00178982
	public bool IsHairUIDAvailable(string uid)
	{
		return this.characterSelector != null && this.characterSelector.IsHairUIDAvailable(uid);
	}

	// Token: 0x06004995 RID: 18837 RVA: 0x0017A5A3 File Offset: 0x001789A3
	protected override void SyncOtherTags()
	{
		this.otherTags = this.GetAllHairOtherTags();
		base.SyncOtherTags();
	}

	// Token: 0x06004996 RID: 18838 RVA: 0x0017A5B7 File Offset: 0x001789B7
	public HashSet<string> GetAllHairOtherTags()
	{
		if (this.characterSelector != null)
		{
			return this.characterSelector.GetHairOtherTags();
		}
		return null;
	}

	// Token: 0x06004997 RID: 18839 RVA: 0x0017A5D8 File Offset: 0x001789D8
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
					"Custom/Hair/Female/Builtin/",
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
					"Custom/Hair/Male/Builtin/",
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
					"Custom/Hair/Neutral/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".hide"
				});
			}
		}
	}

	// Token: 0x06004998 RID: 18840 RVA: 0x0017A718 File Offset: 0x00178B18
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
					"Custom/Hair/Female/Builtin/",
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
					"Custom/Hair/Male/Builtin/",
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
					"Custom/Hair/Neutral/Builtin/",
					this.assetName,
					"/",
					this.assetName,
					".prefs"
				});
			}
		}
	}

	// Token: 0x06004999 RID: 18841 RVA: 0x0017A860 File Offset: 0x00178C60
	public void SetLocked(bool b)
	{
		this.locked = b;
		if (this.hairGroupControls != null)
		{
			foreach (DAZHairGroupControl dazhairGroupControl in this.hairGroupControls)
			{
				if (dazhairGroupControl.lockedJSON != null)
				{
					dazhairGroupControl.lockedJSON.val = b;
				}
			}
		}
	}

	// Token: 0x0600499A RID: 18842 RVA: 0x0017A8B8 File Offset: 0x00178CB8
	protected override void InitInstance()
	{
		base.InitInstance();
		this.hairGroupControls = base.GetComponentsInChildren<DAZHairGroupControl>(true);
		if (this.hairGroupControls != null)
		{
			foreach (DAZHairGroupControl dazhairGroupControl in this.hairGroupControls)
			{
				dazhairGroupControl.hairItem = this;
			}
		}
		if (base.skin != null)
		{
			PreCalcMeshProviderHolder[] componentsInChildren = base.GetComponentsInChildren<PreCalcMeshProviderHolder>(true);
			foreach (PreCalcMeshProviderHolder preCalcMeshProviderHolder in componentsInChildren)
			{
				preCalcMeshProviderHolder.provider = base.skin;
			}
		}
		DAZDynamic componentInChildren = base.GetComponentInChildren<DAZDynamic>(true);
		if (componentInChildren != null)
		{
			if (this.gender == DAZDynamicItem.Gender.Female)
			{
				componentInChildren.itemType = PresetManager.ItemType.HairFemale;
			}
			else if (this.gender == DAZDynamicItem.Gender.Male)
			{
				componentInChildren.itemType = PresetManager.ItemType.HairMale;
			}
			else
			{
				componentInChildren.itemType = PresetManager.ItemType.HairNeutral;
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

	// Token: 0x0600499B RID: 18843 RVA: 0x0017A9F8 File Offset: 0x00178DF8
	protected override void Connect()
	{
		base.Connect();
		DAZHairMesh[] componentsInChildren = base.GetComponentsInChildren<DAZHairMesh>(true);
		foreach (DAZHairMesh dazhairMesh in componentsInChildren)
		{
			dazhairMesh.capsuleColliders = this.hairColliders;
			DAZSkinV2MeshSelection component = dazhairMesh.gameObject.GetComponent<DAZSkinV2MeshSelection>();
			if (component != null && base.skin != null)
			{
				component.meshTransform = base.skin.transform;
				component.skin = base.skin;
			}
			dazhairMesh.Reset();
		}
		PreCalcMeshProviderHolder[] componentsInChildren2 = base.GetComponentsInChildren<PreCalcMeshProviderHolder>(true);
		foreach (PreCalcMeshProviderHolder preCalcMeshProviderHolder in componentsInChildren2)
		{
			preCalcMeshProviderHolder.provider = base.skin;
		}
		this.ResetPhysics();
		if (this.rootBonesForSkinning != null)
		{
			DAZSkinV2[] componentsInChildren3 = base.GetComponentsInChildren<DAZSkinV2>(true);
			foreach (DAZSkinV2 dazskinV in componentsInChildren3)
			{
				dazskinV.root = this.rootBonesForSkinning;
			}
		}
	}

	// Token: 0x0600499C RID: 18844 RVA: 0x0017AB14 File Offset: 0x00178F14
	public override void PartialResetPhysics()
	{
		if (base.enabled)
		{
			GPHairPhysics[] componentsInChildren = base.GetComponentsInChildren<GPHairPhysics>(true);
			foreach (GPHairPhysics gphairPhysics in componentsInChildren)
			{
				gphairPhysics.PartialResetPhysics();
			}
		}
	}

	// Token: 0x0600499D RID: 18845 RVA: 0x0017AB54 File Offset: 0x00178F54
	public override void ResetPhysics()
	{
		if (base.enabled)
		{
			GPHairPhysics[] componentsInChildren = base.GetComponentsInChildren<GPHairPhysics>(true);
			foreach (GPHairPhysics gphairPhysics in componentsInChildren)
			{
				gphairPhysics.ResetPhysics();
			}
		}
	}

	// Token: 0x04003818 RID: 14360
	public CapsuleCollider[] hairColliders;

	// Token: 0x04003819 RID: 14361
	public DAZBones rootBonesForSkinning;

	// Token: 0x0400381A RID: 14362
	protected DAZHairGroupControl[] hairGroupControls;
}
