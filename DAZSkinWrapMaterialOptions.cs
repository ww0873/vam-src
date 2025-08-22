using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B16 RID: 2838
public class DAZSkinWrapMaterialOptions : MaterialOptions
{
	// Token: 0x06004D75 RID: 19829 RVA: 0x001B2646 File Offset: 0x001B0A46
	public DAZSkinWrapMaterialOptions()
	{
	}

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x06004D76 RID: 19830 RVA: 0x001B2650 File Offset: 0x001B0A50
	// (set) Token: 0x06004D77 RID: 19831 RVA: 0x001B26B2 File Offset: 0x001B0AB2
	public DAZSkinWrap skinWrap
	{
		get
		{
			if (this._skinWrap == null)
			{
				DAZSkinWrap[] components = base.GetComponents<DAZSkinWrap>();
				foreach (DAZSkinWrap dazskinWrap in components)
				{
					if (dazskinWrap.enabled && dazskinWrap.draw)
					{
						this._skinWrap = dazskinWrap;
					}
				}
			}
			return this._skinWrap;
		}
		set
		{
			if (this._skinWrap != value)
			{
				this._skinWrap = value;
				this.SetAllParameters();
			}
		}
	}

	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x06004D78 RID: 19832 RVA: 0x001B26D2 File Offset: 0x001B0AD2
	// (set) Token: 0x06004D79 RID: 19833 RVA: 0x001B26DA File Offset: 0x001B0ADA
	public DAZSkinWrap skinWrap2
	{
		get
		{
			return this._skinWrap2;
		}
		set
		{
			if (this._skinWrap2 != value)
			{
				this._skinWrap2 = value;
				this.SetAllParameters();
			}
		}
	}

	// Token: 0x06004D7A RID: 19834 RVA: 0x001B26FC File Offset: 0x001B0AFC
	protected void OnSimTextureLoaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (this != null)
		{
			base.RegisterTexture(qi.tex);
			base.DeregisterTexture(this.customSimTexture);
			this.customSimTexture = qi.tex;
			this.customSimTextureIsNull = (qi.imgPath == "NULL");
			if (this.skinWrap != null && this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < this.skinWrap.simTextures.Length)
					{
						this.skinWrap.simTextures[num] = this.customSimTexture;
					}
				}
			}
			if (this.skinWrap2 != null && this.paramMaterialSlots2 != null)
			{
				for (int j = 0; j < this.paramMaterialSlots2.Length; j++)
				{
					int num2 = this.paramMaterialSlots2[j];
					if (num2 < this.skinWrap2.simTextures.Length)
					{
						this.skinWrap2.simTextures[num2] = this.customSimTexture;
					}
				}
			}
			if (this.simTextureLoadedHandlers != null)
			{
				this.simTextureLoadedHandlers();
			}
		}
	}

	// Token: 0x06004D7B RID: 19835 RVA: 0x001B2830 File Offset: 0x001B0C30
	protected void SyncCustomSimTextureUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (val != null && val != string.Empty)
		{
			if (Regex.IsMatch(val, "^http"))
			{
				SuperController.LogError("Texture load does not currently support http image urls");
			}
			else
			{
				base.QueueCustomTexture(val, jsonstorableUrl.valueSetFromBrowse, false, false, false, new ImageLoaderThreaded.ImageLoaderCallback(this.OnSimTextureLoaded));
			}
		}
		else
		{
			base.QueueCustomTexture(null, jsonstorableUrl.valueSetFromBrowse, false, false, false, new ImageLoaderThreaded.ImageLoaderCallback(this.OnSimTextureLoaded));
		}
		if (this.allowLinkToOtherMaterials && this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions = materialOptions as DAZSkinWrapMaterialOptions;
				if (dazskinWrapMaterialOptions != null && dazskinWrapMaterialOptions.customSimTextureUrlJSON != null)
				{
					dazskinWrapMaterialOptions.customSimTextureUrlJSON.val = this.customSimTextureUrlJSON.val;
				}
			}
		}
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x001B2954 File Offset: 0x001B0D54
	protected void SetCustomSimTextureToNull()
	{
		if (this.customSimTextureUrlJSON != null)
		{
			this.customSimTextureUrlJSON.val = "NULL";
		}
	}

	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x06004D7D RID: 19837 RVA: 0x001B2971 File Offset: 0x001B0D71
	public bool hasCustomSimTexture
	{
		get
		{
			return this.customSimTextureUrlJSON != null && this.customSimTextureUrlJSON.val != string.Empty;
		}
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x001B299C File Offset: 0x001B0D9C
	protected override void SetMaterialHide(bool b)
	{
		if (this.hideShader != null)
		{
			if (this.materialToOriginalShader == null)
			{
				this.materialToOriginalShader = new Dictionary<Material, Shader>();
			}
			if (this.skinWrap != null)
			{
				this.skinWrap.InitMaterials();
				if (this.useSimpleMaterial)
				{
					Material gpusimpleMaterial = this.skinWrap.GPUsimpleMaterial;
					this.SetMaterialHide(gpusimpleMaterial, b);
				}
				else if (this.paramMaterialSlots != null)
				{
					for (int i = 0; i < this.paramMaterialSlots.Length; i++)
					{
						int num = this.paramMaterialSlots[i];
						if (num < this.skinWrap.numMaterials)
						{
							Material m = this.skinWrap.GPUmaterials[num];
							this.SetMaterialHide(m, b);
						}
					}
				}
				this.skinWrap.FlushBuffers();
			}
			if (this.skinWrap2 != null)
			{
				this.skinWrap2.InitMaterials();
				if (this.useSimpleMaterial)
				{
					Material gpusimpleMaterial2 = this.skinWrap2.GPUsimpleMaterial;
					this.SetMaterialHide(gpusimpleMaterial2, b);
				}
				else if (this.paramMaterialSlots2 != null)
				{
					for (int j = 0; j < this.paramMaterialSlots2.Length; j++)
					{
						int num2 = this.paramMaterialSlots2[j];
						if (num2 < this.skinWrap2.numMaterials)
						{
							Material m2 = this.skinWrap2.GPUmaterials[num2];
							this.SetMaterialHide(m2, b);
						}
					}
				}
				this.skinWrap2.FlushBuffers();
			}
		}
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x001B2B1C File Offset: 0x001B0F1C
	protected override void SetMaterialRenderQueue(int q)
	{
		if (this.skinWrap != null)
		{
			this.skinWrap.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial = this.skinWrap.GPUsimpleMaterial;
				gpusimpleMaterial.renderQueue = q;
			}
			else if (this.paramMaterialSlots != null)
			{
				for (int i = 0; i < this.paramMaterialSlots.Length; i++)
				{
					int num = this.paramMaterialSlots[i];
					if (num < this.skinWrap.numMaterials)
					{
						Material material = this.skinWrap.GPUmaterials[num];
						material.renderQueue = q;
					}
				}
			}
		}
		if (this.skinWrap2 != null)
		{
			this.skinWrap2.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial2 = this.skinWrap2.GPUsimpleMaterial;
				gpusimpleMaterial2.renderQueue = q;
			}
			else if (this.paramMaterialSlots2 != null)
			{
				for (int j = 0; j < this.paramMaterialSlots2.Length; j++)
				{
					int num2 = this.paramMaterialSlots2[j];
					if (num2 < this.skinWrap2.numMaterials)
					{
						Material material2 = this.skinWrap2.GPUmaterials[num2];
						material2.renderQueue = q;
					}
				}
			}
		}
	}

	// Token: 0x06004D80 RID: 19840 RVA: 0x001B2C5C File Offset: 0x001B105C
	protected override void SetMaterialParam(string name, float value)
	{
		if (this.skinWrap != null)
		{
			this.skinWrap.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial = this.skinWrap.GPUsimpleMaterial;
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
					if (num < this.skinWrap.numMaterials)
					{
						Material material = this.skinWrap.GPUmaterials[num];
						if (material != null && material.HasProperty(name))
						{
							material.SetFloat(name, value);
						}
					}
				}
			}
		}
		if (this.skinWrap2 != null)
		{
			this.skinWrap2.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial2 = this.skinWrap2.GPUsimpleMaterial;
				if (gpusimpleMaterial2.HasProperty(name))
				{
					gpusimpleMaterial2.SetFloat(name, value);
				}
			}
			else if (this.paramMaterialSlots2 != null)
			{
				for (int j = 0; j < this.paramMaterialSlots2.Length; j++)
				{
					int num2 = this.paramMaterialSlots2[j];
					if (num2 < this.skinWrap2.numMaterials)
					{
						Material material2 = this.skinWrap2.GPUmaterials[num2];
						if (material2 != null && material2.HasProperty(name))
						{
							material2.SetFloat(name, value);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004D81 RID: 19841 RVA: 0x001B2DE8 File Offset: 0x001B11E8
	protected override void SetMaterialColor(string name, Color c)
	{
		if (this.skinWrap != null)
		{
			this.skinWrap.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial = this.skinWrap.GPUsimpleMaterial;
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
					if (num < this.skinWrap.numMaterials)
					{
						Material material = this.skinWrap.GPUmaterials[num];
						if (material != null && material.HasProperty(name))
						{
							material.SetColor(name, c);
						}
					}
				}
			}
		}
		if (this.skinWrap2 != null)
		{
			this.skinWrap2.InitMaterials();
			if (this.useSimpleMaterial)
			{
				Material gpusimpleMaterial2 = this.skinWrap2.GPUsimpleMaterial;
				if (gpusimpleMaterial2.HasProperty(name))
				{
					gpusimpleMaterial2.SetColor(name, c);
				}
			}
			else if (this.paramMaterialSlots2 != null)
			{
				for (int j = 0; j < this.paramMaterialSlots2.Length; j++)
				{
					int num2 = this.paramMaterialSlots2[j];
					if (num2 < this.skinWrap2.numMaterials)
					{
						Material material2 = this.skinWrap2.GPUmaterials[num2];
						if (material2 != null && material2.HasProperty(name))
						{
							material2.SetColor(name, c);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x001B2F74 File Offset: 0x001B1374
	protected override void SetMaterialTexture(int slot, string propName, Texture texture)
	{
		if (this.skinWrap != null)
		{
			this.skinWrap.InitMaterials();
			if (slot < this.skinWrap.numMaterials)
			{
				Material m = this.skinWrap.GPUmaterials[slot];
				this.SetMaterialTexture(m, propName, texture);
			}
		}
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x001B2FC8 File Offset: 0x001B13C8
	protected override void SetMaterialTexture2(int slot, string propName, Texture texture)
	{
		if (this.skinWrap2 != null)
		{
			this.skinWrap2.InitMaterials();
			if (slot < this.skinWrap2.numMaterials)
			{
				Material m = this.skinWrap2.GPUmaterials[slot];
				this.SetMaterialTexture(m, propName, texture);
			}
		}
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001B301C File Offset: 0x001B141C
	protected override void SetMaterialTextureScale(int slot, string propName, Vector2 scale)
	{
		if (this.skinWrap != null)
		{
			this.skinWrap.InitMaterials();
			if (slot < this.skinWrap.numMaterials)
			{
				Material m = this.skinWrap.GPUmaterials[slot];
				this.SetMaterialTextureScale(m, propName, scale);
			}
		}
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x001B3070 File Offset: 0x001B1470
	protected override void SetMaterialTextureScale2(int slot, string propName, Vector2 scale)
	{
		if (this.skinWrap2 != null)
		{
			this.skinWrap2.InitMaterials();
			if (slot < this.skinWrap2.numMaterials)
			{
				Material m = this.skinWrap2.GPUmaterials[slot];
				this.SetMaterialTextureScale(m, propName, scale);
			}
		}
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001B30C4 File Offset: 0x001B14C4
	protected override void SetMaterialTextureOffset(int slot, string propName, Vector2 offset)
	{
		if (this.skinWrap != null)
		{
			this.skinWrap.InitMaterials();
			if (slot < this.skinWrap.numMaterials)
			{
				Material m = this.skinWrap.GPUmaterials[slot];
				this.SetMaterialTextureOffset(m, propName, offset);
			}
		}
	}

	// Token: 0x06004D87 RID: 19847 RVA: 0x001B3118 File Offset: 0x001B1518
	protected override void SetMaterialTextureOffset2(int slot, string propName, Vector2 offset)
	{
		if (this.skinWrap2 != null)
		{
			this.skinWrap2.InitMaterials();
			if (slot < this.skinWrap2.numMaterials)
			{
				Material m = this.skinWrap2.GPUmaterials[slot];
				this.SetMaterialTextureOffset(m, propName, offset);
			}
		}
	}

	// Token: 0x06004D88 RID: 19848 RVA: 0x001B316C File Offset: 0x001B156C
	public override Mesh GetMesh()
	{
		Mesh result = null;
		if (this.skinWrap != null)
		{
			result = this.skinWrap.GetStartMesh();
		}
		return result;
	}

	// Token: 0x06004D89 RID: 19849 RVA: 0x001B3199 File Offset: 0x001B1599
	public override void SetCustomTextureFolder(string path)
	{
		base.SetCustomTextureFolder(path);
		if (this.customSimTextureUrlJSON != null)
		{
			this.customSimTextureUrlJSON.suggestedPath = this.customTextureFolder;
		}
	}

	// Token: 0x06004D8A RID: 19850 RVA: 0x001B31C0 File Offset: 0x001B15C0
	protected override void SyncLinkToOtherMaterials(bool b)
	{
		base.SyncLinkToOtherMaterials(b);
		if (this._linkToOtherMaterials)
		{
			foreach (MaterialOptions materialOptions in this.otherMaterialOptionsList)
			{
				DAZSkinWrapMaterialOptions dazskinWrapMaterialOptions = materialOptions as DAZSkinWrapMaterialOptions;
				if (dazskinWrapMaterialOptions != null && dazskinWrapMaterialOptions.customSimTextureUrlJSON != null && this.customSimTextureUrlJSON != null)
				{
					dazskinWrapMaterialOptions.customSimTextureUrlJSON.val = this.customSimTextureUrlJSON.val;
				}
			}
		}
	}

	// Token: 0x06004D8B RID: 19851 RVA: 0x001B3268 File Offset: 0x001B1668
	public override void SetStartingValues(Dictionary<Texture2D, string> textureToSourcePath)
	{
		if (this.skinWrap != null && this.materialForDefaults == null)
		{
			this.skinWrap.InitMaterials();
			DAZMesh componentInChildren = this.skinWrap.GetComponentInChildren<DAZMergedMesh>(true);
			if (componentInChildren == null)
			{
				componentInChildren = this.skinWrap.GetComponentInChildren<DAZMesh>(true);
			}
			if (this.paramMaterialSlots != null && this.paramMaterialSlots.Length > 0)
			{
				int num = this.paramMaterialSlots[0];
				if (componentInChildren != null && num < componentInChildren.numMaterials)
				{
					this.materialForDefaults = componentInChildren.materials[num];
				}
				if (this.skinWrap.simTextures != null && num < this.skinWrap.simTextures.Length)
				{
					this.defaultSimTexture = this.skinWrap.simTextures[num];
				}
			}
		}
		base.SetStartingValues(textureToSourcePath);
		if (Application.isPlaying)
		{
			if (this.customSimTextureUrlJSON != null)
			{
				base.DeregisterUrl(this.customSimTextureUrlJSON);
				this.customSimTextureUrlJSON = null;
			}
			this.customSimTextureUrlJSON = new JSONStorableUrl("simTexture", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncCustomSimTextureUrl), "jpg|jpeg|png|tif|tiff", this.customTextureFolder, true);
			this.customSimTextureUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(base.BeginBrowse);
			base.RegisterUrl(this.customSimTextureUrlJSON);
		}
	}

	// Token: 0x06004D8C RID: 19852 RVA: 0x001B33C8 File Offset: 0x001B17C8
	public override void InitUI()
	{
		base.InitUI();
		if (this.UITransform != null)
		{
			DAZSkinWrapMaterialOptionsUI componentInChildren = this.UITransform.GetComponentInChildren<DAZSkinWrapMaterialOptionsUI>();
			if (componentInChildren != null)
			{
				this.customSimTextureFileBrowseButton = componentInChildren.customSimTextureFileBrowseButton;
				this.customSimTextureReloadButton = componentInChildren.customSimTextureReloadButton;
				this.customSimTextureClearButton = componentInChildren.customSimTextureClearButton;
				this.customSimTextureNullButton = componentInChildren.customSimTextureNullButton;
				this.customSimTextureDefaultButton = componentInChildren.customSimTextureDefaultButton;
				this.customSimTextureUrlText = componentInChildren.customSimTextureUrlText;
			}
			if (this.customSimTextureUrlJSON != null)
			{
				this.customSimTextureUrlJSON.fileBrowseButton = this.customSimTextureFileBrowseButton;
				this.customSimTextureUrlJSON.reloadButton = this.customSimTextureReloadButton;
				this.customSimTextureUrlJSON.clearButton = this.customSimTextureClearButton;
				this.customSimTextureUrlJSON.defaultButton = this.customSimTextureDefaultButton;
				this.customSimTextureUrlJSON.text = this.customSimTextureUrlText;
				if (this.customSimTextureNullButton != null)
				{
					this.customSimTextureNullButton.onClick.AddListener(new UnityAction(this.SetCustomSimTextureToNull));
				}
			}
		}
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x001B34DC File Offset: 0x001B18DC
	public override void DeregisterUI()
	{
		base.DeregisterUI();
		if (this.customSimTextureUrlJSON != null)
		{
			this.customSimTextureUrlJSON.fileBrowseButton = null;
			this.customSimTextureUrlJSON.reloadButton = null;
			this.customSimTextureUrlJSON.clearButton = null;
			this.customSimTextureUrlJSON.defaultButton = null;
			if (this.customSimTextureNullButton != null)
			{
				this.customSimTextureNullButton.onClick.RemoveListener(new UnityAction(this.SetCustomSimTextureToNull));
			}
		}
	}

	// Token: 0x04003D41 RID: 15681
	[HideInInspector]
	[SerializeField]
	protected DAZSkinWrap _skinWrap;

	// Token: 0x04003D42 RID: 15682
	[HideInInspector]
	[SerializeField]
	protected DAZSkinWrap _skinWrap2;

	// Token: 0x04003D43 RID: 15683
	public bool useSimpleMaterial;

	// Token: 0x04003D44 RID: 15684
	public DAZSkinWrapMaterialOptions.SimTextureLoaded simTextureLoadedHandlers;

	// Token: 0x04003D45 RID: 15685
	public Button customSimTextureFileBrowseButton;

	// Token: 0x04003D46 RID: 15686
	public Button customSimTextureReloadButton;

	// Token: 0x04003D47 RID: 15687
	public Button customSimTextureClearButton;

	// Token: 0x04003D48 RID: 15688
	public Button customSimTextureNullButton;

	// Token: 0x04003D49 RID: 15689
	public Button customSimTextureDefaultButton;

	// Token: 0x04003D4A RID: 15690
	public Text customSimTextureUrlText;

	// Token: 0x04003D4B RID: 15691
	protected Texture2D defaultSimTexture;

	// Token: 0x04003D4C RID: 15692
	protected Texture2D customSimTexture;

	// Token: 0x04003D4D RID: 15693
	protected bool customSimTextureIsNull;

	// Token: 0x04003D4E RID: 15694
	protected JSONStorableUrl customSimTextureUrlJSON;

	// Token: 0x02000B17 RID: 2839
	// (Invoke) Token: 0x06004D8F RID: 19855
	public delegate void SimTextureLoaded();
}
