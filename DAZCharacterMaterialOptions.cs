using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AAB RID: 2731
public class DAZCharacterMaterialOptions : MaterialOptions
{
	// Token: 0x06004799 RID: 18329 RVA: 0x0015C9D8 File Offset: 0x0015ADD8
	public DAZCharacterMaterialOptions()
	{
	}

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x0600479A RID: 18330 RVA: 0x0015C9E0 File Offset: 0x0015ADE0
	// (set) Token: 0x0600479B RID: 18331 RVA: 0x0015C9E8 File Offset: 0x0015ADE8
	public DAZSkinV2 skin
	{
		get
		{
			return this._skin;
		}
		set
		{
			if (!this.isPassthrough && this._skin != value)
			{
				this._skin = value;
				if (Application.isPlaying)
				{
					this.SetAllParameters();
				}
			}
		}
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x0015CA20 File Offset: 0x0015AE20
	protected override void SetMaterialHide(bool b)
	{
		if (this.hideShader != null)
		{
			if (this.materialToOriginalShader == null)
			{
				this.materialToOriginalShader = new Dictionary<Material, Shader>();
			}
			if (this.skin != null)
			{
				if (this.useSimpleMaterial)
				{
					Material gpusimpleMaterial = this.skin.GPUsimpleMaterial;
					this.SetMaterialHide(gpusimpleMaterial, b);
				}
				else if (this.paramMaterialSlots != null)
				{
					for (int i = 0; i < this.paramMaterialSlots.Length; i++)
					{
						int num = this.paramMaterialSlots[i];
						if (num < this.skin.numMaterials)
						{
							Material material = this.skin.GPUmaterials[num];
							if (material != null)
							{
								this.SetMaterialHide(material, b);
							}
						}
					}
				}
				this.skin.FlushBuffers();
			}
		}
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x0015CAF4 File Offset: 0x0015AEF4
	protected override void SetMaterialRenderQueue(int q)
	{
		if (this.skin != null)
		{
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial = this.skin.GPUsimpleMaterial;
				gpusimpleMaterial.renderQueue = q;
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < this.skin.numMaterials)
					{
						Material material = this.skin.GPUmaterials[num];
						if (material != null)
						{
							material.renderQueue = q;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600479E RID: 18334 RVA: 0x0015CB94 File Offset: 0x0015AF94
	protected override void SetMaterialParam(string name, float value)
	{
		if (this.skin != null)
		{
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial = this.skin.GPUsimpleMaterial;
				if (gpusimpleMaterial.HasProperty(name))
				{
					gpusimpleMaterial.SetFloat(name, value);
				}
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < this.skin.numMaterials)
					{
						Material material = this.skin.GPUmaterials[num];
						if (material != null && material.HasProperty(name))
						{
							material.SetFloat(name, value);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x0015CC50 File Offset: 0x0015B050
	protected override void SetMaterialColor(string name, Color c)
	{
		if (this.skin != null)
		{
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial = this.skin.GPUsimpleMaterial;
				if (gpusimpleMaterial.HasProperty(name))
				{
					gpusimpleMaterial.SetColor(name, c);
				}
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < this.skin.numMaterials)
					{
						Material material = this.skin.GPUmaterials[num];
						if (material != null && material.HasProperty(name))
						{
							material.SetColor(name, c);
						}
					}
				}
			}
		}
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x0015CD0C File Offset: 0x0015B10C
	protected override void SetMaterialTexture(int slot, string propName, Texture texture)
	{
		if (this.paramMaterialSlots != null && this.skin != null && slot < this.skin.numMaterials)
		{
			Material m = this.skin.GPUmaterials[slot];
			this.SetMaterialTexture(m, propName, texture);
		}
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x0015CD60 File Offset: 0x0015B160
	protected override void SetMaterialTexture2(int slot, string propName, Texture texture)
	{
		if (this.paramMaterialSlots != null && this.skin != null && slot < this.skin.numMaterials)
		{
			Material m = this.skin.GPUmaterials[slot];
			this.SetMaterialTexture(m, propName, texture);
		}
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x0015CDB4 File Offset: 0x0015B1B4
	protected bool CheckNullifyTextureGroupSet(MaterialOptionTextureGroup textureGroup, DAZCharacterMaterialOptions.TextureGroupDisableZone disableZone)
	{
		bool result = false;
		switch (disableZone)
		{
		case DAZCharacterMaterialOptions.TextureGroupDisableZone.Face:
			if (textureGroup.mapTexturesToTextureNames)
			{
				if (this.characterTextureControl.hasFaceTextureSet)
				{
					result = true;
				}
			}
			else if (this.characterTextureControl.hasFaceDiffuseTextureSet)
			{
				result = true;
			}
			break;
		case DAZCharacterMaterialOptions.TextureGroupDisableZone.Limbs:
			if (textureGroup.mapTexturesToTextureNames)
			{
				if (this.characterTextureControl.hasLimbsTextureSet)
				{
					result = true;
				}
			}
			else if (this.characterTextureControl.hasLimbsDiffuseTextureSet)
			{
				result = true;
			}
			break;
		case DAZCharacterMaterialOptions.TextureGroupDisableZone.Torso:
			if (textureGroup.mapTexturesToTextureNames)
			{
				if (this.characterTextureControl.hasTorsoTextureSet)
				{
					result = true;
				}
			}
			else if (this.characterTextureControl.hasTorsoDiffuseTextureSet)
			{
				result = true;
			}
			break;
		case DAZCharacterMaterialOptions.TextureGroupDisableZone.Genitals:
			if (textureGroup.mapTexturesToTextureNames)
			{
				if (this.characterTextureControl.hasGenitalsTextureSet)
				{
					result = true;
				}
			}
			else if (this.characterTextureControl.hasGenitalsDiffuseTextureSet)
			{
				result = true;
			}
			break;
		case DAZCharacterMaterialOptions.TextureGroupDisableZone.TorsoAndFace:
			if (textureGroup.mapTexturesToTextureNames)
			{
				if (this.characterTextureControl.hasTorsoTextureSet || this.characterTextureControl.hasFaceTextureSet)
				{
					result = true;
				}
			}
			else if (this.characterTextureControl.hasTorsoDiffuseTextureSet || this.characterTextureControl.hasFaceDiffuseTextureSet)
			{
				result = true;
			}
			break;
		case DAZCharacterMaterialOptions.TextureGroupDisableZone.TorsoAndLimbs:
			if (textureGroup.mapTexturesToTextureNames)
			{
				if (this.characterTextureControl.hasTorsoTextureSet || this.characterTextureControl.hasLimbsTextureSet)
				{
					result = true;
				}
			}
			else if (this.characterTextureControl.hasTorsoDiffuseTextureSet || this.characterTextureControl.hasLimbsDiffuseTextureSet)
			{
				result = true;
			}
			break;
		case DAZCharacterMaterialOptions.TextureGroupDisableZone.TorsoAndGenitals:
			if (textureGroup.mapTexturesToTextureNames)
			{
				if (this.characterTextureControl.hasTorsoTextureSet || this.characterTextureControl.hasGenitalsTextureSet)
				{
					result = true;
				}
			}
			else if (this.characterTextureControl.hasTorsoDiffuseTextureSet || this.characterTextureControl.hasGenitalsDiffuseTextureSet)
			{
				result = true;
			}
			break;
		}
		return result;
	}

	// Token: 0x060047A3 RID: 18339 RVA: 0x0015CFDC File Offset: 0x0015B3DC
	protected override void SetTextureGroup1Set(string setName)
	{
		bool flag = false;
		if (this.textureGroup1DisableZone != DAZCharacterMaterialOptions.TextureGroupDisableZone.None && this.characterTextureControl != null)
		{
			flag = this.CheckNullifyTextureGroupSet(this.textureGroup1, this.textureGroup1DisableZone);
		}
		if (flag)
		{
			this.textureGroup1JSON.valNoCallback = this.currentTextureGroup1Set;
		}
		else
		{
			base.SetTextureGroup1Set(setName);
		}
	}

	// Token: 0x060047A4 RID: 18340 RVA: 0x0015D040 File Offset: 0x0015B440
	protected override void SetTextureGroup2Set(string setName)
	{
		bool flag = false;
		if (this.textureGroup2DisableZone != DAZCharacterMaterialOptions.TextureGroupDisableZone.None && this.characterTextureControl != null)
		{
			flag = this.CheckNullifyTextureGroupSet(this.textureGroup2, this.textureGroup2DisableZone);
		}
		if (flag)
		{
			this.textureGroup2JSON.valNoCallback = this.currentTextureGroup2Set;
		}
		else
		{
			base.SetTextureGroup2Set(setName);
		}
	}

	// Token: 0x060047A5 RID: 18341 RVA: 0x0015D0A4 File Offset: 0x0015B4A4
	protected override void SetTextureGroup3Set(string setName)
	{
		bool flag = false;
		if (this.textureGroup3DisableZone != DAZCharacterMaterialOptions.TextureGroupDisableZone.None && this.characterTextureControl != null)
		{
			flag = this.CheckNullifyTextureGroupSet(this.textureGroup3, this.textureGroup3DisableZone);
		}
		if (flag)
		{
			this.textureGroup3JSON.valNoCallback = this.currentTextureGroup3Set;
		}
		else
		{
			base.SetTextureGroup3Set(setName);
		}
	}

	// Token: 0x060047A6 RID: 18342 RVA: 0x0015D108 File Offset: 0x0015B508
	protected override void SetTextureGroup4Set(string setName)
	{
		bool flag = false;
		if (this.textureGroup4DisableZone != DAZCharacterMaterialOptions.TextureGroupDisableZone.None && this.characterTextureControl != null)
		{
			flag = this.CheckNullifyTextureGroupSet(this.textureGroup4, this.textureGroup4DisableZone);
		}
		if (flag)
		{
			this.textureGroup4JSON.valNoCallback = this.currentTextureGroup4Set;
		}
		else
		{
			base.SetTextureGroup4Set(setName);
		}
	}

	// Token: 0x060047A7 RID: 18343 RVA: 0x0015D16C File Offset: 0x0015B56C
	protected override void SetTextureGroup5Set(string setName)
	{
		bool flag = false;
		if (this.textureGroup5DisableZone != DAZCharacterMaterialOptions.TextureGroupDisableZone.None && this.characterTextureControl != null)
		{
			flag = this.CheckNullifyTextureGroupSet(this.textureGroup5, this.textureGroup5DisableZone);
		}
		if (flag)
		{
			this.textureGroup5JSON.valNoCallback = this.currentTextureGroup5Set;
		}
		else
		{
			base.SetTextureGroup5Set(setName);
		}
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x0015D1D0 File Offset: 0x0015B5D0
	public void SetAllTextureGroupSetsToCurrent()
	{
		if (this.textureGroup1 != null && this.textureGroup1.sets != null && this.textureGroup1.sets.Length > 0)
		{
			this.SetTextureGroup1Set(this.currentTextureGroup1Set);
		}
		if (this.textureGroup2 != null && this.textureGroup2.sets != null && this.textureGroup2.sets.Length > 0)
		{
			this.SetTextureGroup2Set(this.currentTextureGroup2Set);
		}
		if (this.textureGroup3 != null && this.textureGroup3.sets != null && this.textureGroup3.sets.Length > 0)
		{
			this.SetTextureGroup3Set(this.currentTextureGroup3Set);
		}
		if (this.textureGroup4 != null && this.textureGroup4.sets != null && this.textureGroup4.sets.Length > 0)
		{
			this.SetTextureGroup4Set(this.currentTextureGroup4Set);
		}
		if (this.textureGroup5 != null && this.textureGroup5.sets != null && this.textureGroup5.sets.Length > 0)
		{
			this.SetTextureGroup5Set(this.currentTextureGroup5Set);
		}
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x0015D300 File Offset: 0x0015B700
	public override Mesh GetMesh()
	{
		Mesh result = null;
		if (this._skin != null && this._skin.dazMesh != null)
		{
			result = this._skin.dazMesh.morphedUVMappedMesh;
		}
		return result;
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x0015D348 File Offset: 0x0015B748
	public override void SetStartingValues(Dictionary<Texture2D, string> textureToSourcePath)
	{
		this.characterTextureControl = base.GetComponent<DAZCharacterTextureControl>();
		if (this.skin != null && this.materialForDefaults == null)
		{
			DAZMesh componentInChildren = this.skin.GetComponentInChildren<DAZMergedMesh>(true);
			if (componentInChildren == null)
			{
				componentInChildren = this.skin.GetComponentInChildren<DAZMesh>(true);
			}
			if (componentInChildren != null && this.paramMaterialSlots != null && this.paramMaterialSlots.Length > 0)
			{
				int num = this.paramMaterialSlots[0];
				if (num < componentInChildren.numMaterials)
				{
					this.materialForDefaults = componentInChildren.materials[num];
				}
				else
				{
					Debug.LogError("Material slot out of range");
				}
			}
		}
		base.SetStartingValues(textureToSourcePath);
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x0015D408 File Offset: 0x0015B808
	protected void ConnectPassthroughBucket(Transform bucket)
	{
		if (bucket != null)
		{
			DAZCharacterMaterialOptions[] componentsInChildren = bucket.GetComponentsInChildren<DAZCharacterMaterialOptions>();
			foreach (DAZCharacterMaterialOptions dazcharacterMaterialOptions in componentsInChildren)
			{
				if (dazcharacterMaterialOptions.storeId == base.storeId)
				{
					dazcharacterMaterialOptions.SetCustomTextureFolder(this.customTextureFolder);
					dazcharacterMaterialOptions.UITransform = this.UITransform;
					dazcharacterMaterialOptions.InitUI();
				}
			}
		}
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x0015D476 File Offset: 0x0015B876
	public void ConnectPassthroughBuckets()
	{
		this.ConnectPassthroughBucket(this.passThroughToBucket1);
		this.ConnectPassthroughBucket(this.passThroughToBucket2);
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x0015D490 File Offset: 0x0015B890
	public override void InitUI()
	{
		if (this.isPassthrough)
		{
			this.ConnectPassthroughBuckets();
		}
		else
		{
			base.InitUI();
		}
	}

	// Token: 0x040034D8 RID: 13528
	[HideInInspector]
	public Transform skinContainer;

	// Token: 0x040034D9 RID: 13529
	[HideInInspector]
	[SerializeField]
	protected DAZSkinV2 _skin;

	// Token: 0x040034DA RID: 13530
	public bool isPassthrough;

	// Token: 0x040034DB RID: 13531
	public Transform passThroughToBucket1;

	// Token: 0x040034DC RID: 13532
	public Transform passThroughToBucket2;

	// Token: 0x040034DD RID: 13533
	public bool useSimpleMaterial;

	// Token: 0x040034DE RID: 13534
	public DAZCharacterMaterialOptions.TextureGroupDisableZone textureGroup1DisableZone;

	// Token: 0x040034DF RID: 13535
	public DAZCharacterMaterialOptions.TextureGroupDisableZone textureGroup2DisableZone;

	// Token: 0x040034E0 RID: 13536
	public DAZCharacterMaterialOptions.TextureGroupDisableZone textureGroup3DisableZone;

	// Token: 0x040034E1 RID: 13537
	public DAZCharacterMaterialOptions.TextureGroupDisableZone textureGroup4DisableZone;

	// Token: 0x040034E2 RID: 13538
	public DAZCharacterMaterialOptions.TextureGroupDisableZone textureGroup5DisableZone;

	// Token: 0x040034E3 RID: 13539
	protected DAZCharacterTextureControl characterTextureControl;

	// Token: 0x02000AAC RID: 2732
	public enum TextureGroupDisableZone
	{
		// Token: 0x040034E5 RID: 13541
		None,
		// Token: 0x040034E6 RID: 13542
		Face,
		// Token: 0x040034E7 RID: 13543
		Limbs,
		// Token: 0x040034E8 RID: 13544
		Torso,
		// Token: 0x040034E9 RID: 13545
		Genitals,
		// Token: 0x040034EA RID: 13546
		TorsoAndFace,
		// Token: 0x040034EB RID: 13547
		TorsoAndLimbs,
		// Token: 0x040034EC RID: 13548
		TorsoAndGenitals
	}
}
