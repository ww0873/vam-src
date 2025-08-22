using System;
using UnityEngine;

// Token: 0x02000AAA RID: 2730
[ExecuteInEditMode]
public class DAZCharacter : JSONStorableDynamic
{
	// Token: 0x06004795 RID: 18325 RVA: 0x00150566 File Offset: 0x0014E966
	public DAZCharacter()
	{
	}

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x06004796 RID: 18326 RVA: 0x0015056E File Offset: 0x0014E96E
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
	}

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x06004797 RID: 18327 RVA: 0x00150576 File Offset: 0x0014E976
	public DAZSkinV2 skinForClothes
	{
		get
		{
			return this._skinForClothes;
		}
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x00150580 File Offset: 0x0014E980
	protected override void Connect()
	{
		DAZSkinV2 dazskinV = base.GetComponentInChildren<DAZMergedSkinV2>(true);
		DAZSkinV2 componentInChildren = base.GetComponentInChildren<DAZSkinV2>(true);
		if (dazskinV == null)
		{
			dazskinV = componentInChildren;
		}
		if (this.useBaseSkinForClothes)
		{
			this._skinForClothes = componentInChildren;
		}
		else
		{
			this._skinForClothes = dazskinV;
		}
		this._skin = dazskinV;
		if (this.rootBonesForSkinning != null)
		{
			DAZSkinV2[] componentsInChildren = base.GetComponentsInChildren<DAZSkinV2>(true);
			foreach (DAZSkinV2 dazskinV2 in componentsInChildren)
			{
				dazskinV2.root = this.rootBonesForSkinning;
			}
		}
		DAZCharacterMaterialOptions[] componentsInChildren2 = base.GetComponentsInChildren<DAZCharacterMaterialOptions>();
		foreach (DAZCharacterMaterialOptions dazcharacterMaterialOptions in componentsInChildren2)
		{
			dazcharacterMaterialOptions.skin = this._skin;
		}
		DAZSkinControl componentInChildren2 = base.GetComponentInChildren<DAZSkinControl>();
		if (componentInChildren2 != null)
		{
			componentInChildren2.skin = this._skin;
		}
	}

	// Token: 0x040034CF RID: 13519
	public string displayName;

	// Token: 0x040034D0 RID: 13520
	public string displayNameAlt;

	// Token: 0x040034D1 RID: 13521
	public string UVname;

	// Token: 0x040034D2 RID: 13522
	public Texture2D thumbnail;

	// Token: 0x040034D3 RID: 13523
	public DAZBones rootBonesForSkinning;

	// Token: 0x040034D4 RID: 13524
	public bool isMale;

	// Token: 0x040034D5 RID: 13525
	[SerializeField]
	protected DAZSkinV2 _skin;

	// Token: 0x040034D6 RID: 13526
	public bool useBaseSkinForClothes;

	// Token: 0x040034D7 RID: 13527
	[SerializeField]
	protected DAZSkinV2 _skinForClothes;
}
