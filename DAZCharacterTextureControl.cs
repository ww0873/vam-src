using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000AB6 RID: 2742
public class DAZCharacterTextureControl : JSONStorable
{
	// Token: 0x0600486F RID: 18543 RVA: 0x00169398 File Offset: 0x00167798
	public DAZCharacterTextureControl()
	{
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x001693AC File Offset: 0x001677AC
	public override void PostRestore(bool restorePhysical, bool restoreAppearance)
	{
		if (restoreAppearance)
		{
			DAZCharacterMaterialOptions component = base.GetComponent<DAZCharacterMaterialOptions>();
			if (component != null)
			{
				component.SetAllTextureGroupSetsToCurrent();
			}
		}
	}

	// Token: 0x06004871 RID: 18545 RVA: 0x001693D8 File Offset: 0x001677D8
	protected void RegisterTexture(Texture2D tex)
	{
		if (ImageLoaderThreaded.singleton != null && ImageLoaderThreaded.singleton.RegisterTextureUse(tex))
		{
			if (this.textureUseCount == null)
			{
				this.textureUseCount = new Dictionary<Texture2D, int>();
			}
			int num = 0;
			if (this.textureUseCount.TryGetValue(tex, out num))
			{
				this.textureUseCount.Remove(tex);
			}
			num++;
			this.textureUseCount.Add(tex, num);
		}
	}

	// Token: 0x06004872 RID: 18546 RVA: 0x00169450 File Offset: 0x00167850
	protected void DeregisterTexture(Texture2D tex)
	{
		int num = 0;
		if (ImageLoaderThreaded.singleton != null && this.textureUseCount != null && this.textureUseCount.TryGetValue(tex, out num))
		{
			ImageLoaderThreaded.singleton.DeregisterTextureUse(tex);
			this.textureUseCount.Remove(tex);
			num--;
			if (num > 0)
			{
				this.textureUseCount.Add(tex, num);
			}
		}
	}

	// Token: 0x06004873 RID: 18547 RVA: 0x001694C0 File Offset: 0x001678C0
	protected void DeregisterAllTextures()
	{
		if (ImageLoaderThreaded.singleton != null && this.textureUseCount != null)
		{
			foreach (Texture2D texture2D in this.textureUseCount.Keys)
			{
				int num = 0;
				if (this.textureUseCount.TryGetValue(texture2D, out num))
				{
					for (int i = 0; i < num; i++)
					{
						ImageLoaderThreaded.singleton.DeregisterTextureUse(texture2D);
					}
				}
			}
			this.textureUseCount.Clear();
		}
	}

	// Token: 0x06004874 RID: 18548 RVA: 0x00169574 File Offset: 0x00167974
	protected void ClearImageErrorCondition(DAZCharacterTextureControl.Region region, DAZCharacterTextureControl.TextureType textureType)
	{
		switch (region)
		{
		case DAZCharacterTextureControl.Region.Face:
			switch (textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.faceDiffuseUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.faceSpecularUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.faceGlossUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.faceNormalUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.faceDetailUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.faceDecalUrlHadErrorJSON.val = false;
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Torso:
			switch (textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.torsoDiffuseUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.torsoSpecularUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.torsoGlossUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.torsoNormalUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.torsoDetailUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.torsoDecalUrlHadErrorJSON.val = false;
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Limbs:
			switch (textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.limbsDiffuseUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.limbsSpecularUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.limbsGlossUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.limbsNormalUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.limbsDetailUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.limbsDecalUrlHadErrorJSON.val = false;
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Genitals:
			switch (textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.genitalsDiffuseUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.genitalsSpecularUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.genitalsGlossUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.genitalsNormalUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.genitalsDetailUrlHadErrorJSON.val = false;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.genitalsDecalUrlHadErrorJSON.val = false;
				break;
			}
			break;
		}
	}

	// Token: 0x06004875 RID: 18549 RVA: 0x001697D4 File Offset: 0x00167BD4
	protected void SetImageErrorCondition(ImageLoaderThreaded.QueuedImage qi)
	{
		DAZCharacterTextureControl.CharacterQueuedImage characterQueuedImage = (DAZCharacterTextureControl.CharacterQueuedImage)qi;
		switch (characterQueuedImage.region)
		{
		case DAZCharacterTextureControl.Region.Face:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.faceDiffuseUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.faceSpecularUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.faceGlossUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.faceNormalUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.faceDetailUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.faceDecalUrlHadErrorJSON.val = qi.hadError;
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Torso:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.torsoDiffuseUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.torsoSpecularUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.torsoGlossUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.torsoNormalUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.torsoDetailUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.torsoDecalUrlHadErrorJSON.val = qi.hadError;
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Limbs:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.limbsDiffuseUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.limbsSpecularUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.limbsGlossUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.limbsNormalUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.limbsDetailUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.limbsDecalUrlHadErrorJSON.val = qi.hadError;
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Genitals:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				this.genitalsDiffuseUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				this.genitalsSpecularUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				this.genitalsGlossUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				this.genitalsNormalUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				this.genitalsDetailUrlHadErrorJSON.val = qi.hadError;
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				this.genitalsDecalUrlHadErrorJSON.val = qi.hadError;
				break;
			}
			break;
		}
	}

	// Token: 0x06004876 RID: 18550 RVA: 0x00169ADC File Offset: 0x00167EDC
	protected bool CheckIfImageIsStillValid(ImageLoaderThreaded.QueuedImage qi)
	{
		bool result = true;
		DAZCharacterTextureControl.CharacterQueuedImage characterQueuedImage = qi as DAZCharacterTextureControl.CharacterQueuedImage;
		switch (characterQueuedImage.region)
		{
		case DAZCharacterTextureControl.Region.Face:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				result = (qi.imgPath == this.faceDiffuseUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				result = (qi.imgPath == this.faceSpecularUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				result = (qi.imgPath == this.faceGlossUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				result = (qi.imgPath == this.faceNormalUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				result = (qi.imgPath == this.faceDetailUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				result = (qi.imgPath == this.faceDecalUrlJSON.val);
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Torso:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				result = (qi.imgPath == this.torsoDiffuseUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				result = (qi.imgPath == this.torsoSpecularUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				result = (qi.imgPath == this.torsoGlossUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				result = (qi.imgPath == this.torsoNormalUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				result = (qi.imgPath == this.torsoDetailUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				result = (qi.imgPath == this.torsoDecalUrlJSON.val);
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Limbs:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				result = (qi.imgPath == this.limbsDiffuseUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				result = (qi.imgPath == this.limbsSpecularUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				result = (qi.imgPath == this.limbsGlossUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				result = (qi.imgPath == this.limbsNormalUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				result = (qi.imgPath == this.limbsDetailUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				result = (qi.imgPath == this.limbsDecalUrlJSON.val);
				break;
			}
			break;
		case DAZCharacterTextureControl.Region.Genitals:
			switch (characterQueuedImage.textureType)
			{
			case DAZCharacterTextureControl.TextureType.Diffuse:
				result = (qi.imgPath == this.genitalsDiffuseUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Specular:
				result = (qi.imgPath == this.genitalsSpecularUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Gloss:
				result = (qi.imgPath == this.genitalsGlossUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Normal:
				result = (qi.imgPath == this.genitalsNormalUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Detail:
				result = (qi.imgPath == this.genitalsDetailUrlJSON.val);
				break;
			case DAZCharacterTextureControl.TextureType.Decal:
				result = (qi.imgPath == this.genitalsDecalUrlJSON.val);
				break;
			}
			break;
		}
		return result;
	}

	// Token: 0x06004877 RID: 18551 RVA: 0x00169E78 File Offset: 0x00168278
	protected void OnImageLoaded(ImageLoaderThreaded.QueuedImage qi)
	{
		if (this == null)
		{
			return;
		}
		if (!this.CheckIfImageIsStillValid(qi))
		{
			return;
		}
		this.SetImageErrorCondition(qi);
		if (!qi.hadError)
		{
			DAZCharacterTextureControl.CharacterQueuedImage characterQueuedImage = qi as DAZCharacterTextureControl.CharacterQueuedImage;
			DAZMergedSkinV2 component = base.GetComponent<DAZMergedSkinV2>();
			if (component != null)
			{
				int[] array = null;
				switch (characterQueuedImage.region)
				{
				case DAZCharacterTextureControl.Region.Face:
					array = this.faceMaterialNums;
					break;
				case DAZCharacterTextureControl.Region.Torso:
					array = this.torsoMaterialNums;
					break;
				case DAZCharacterTextureControl.Region.Limbs:
					array = this.limbMaterialNums;
					break;
				case DAZCharacterTextureControl.Region.Genitals:
					array = this.genitalMaterialNums;
					break;
				}
				if (array != null)
				{
					foreach (int num in array)
					{
						Material material = component.GPUmaterials[num];
						Texture2D texture2D = null;
						bool flag = false;
						switch (characterQueuedImage.textureType)
						{
						case DAZCharacterTextureControl.TextureType.Diffuse:
							if (material.HasProperty("_MainTex"))
							{
								texture2D = (Texture2D)material.GetTexture("_MainTex");
								if (!this.origDiffuseTextures.ContainsKey(material))
								{
									this.origDiffuseTextures.Add(material, texture2D);
								}
								material.SetTexture("_MainTex", characterQueuedImage.tex);
								flag = true;
							}
							break;
						case DAZCharacterTextureControl.TextureType.Specular:
							if (material.HasProperty("_SpecTex"))
							{
								texture2D = (Texture2D)material.GetTexture("_SpecTex");
								if (!this.origSpecularTextures.ContainsKey(material))
								{
									this.origSpecularTextures.Add(material, texture2D);
								}
								material.SetTexture("_SpecTex", characterQueuedImage.tex);
								flag = true;
							}
							break;
						case DAZCharacterTextureControl.TextureType.Gloss:
							if (material.HasProperty("_GlossTex"))
							{
								texture2D = (Texture2D)material.GetTexture("_GlossTex");
								if (!this.origGlossTextures.ContainsKey(material))
								{
									this.origGlossTextures.Add(material, texture2D);
								}
								material.SetTexture("_GlossTex", characterQueuedImage.tex);
								flag = true;
							}
							break;
						case DAZCharacterTextureControl.TextureType.Normal:
							if (material.HasProperty("_BumpMap"))
							{
								texture2D = (Texture2D)material.GetTexture("_BumpMap");
								if (!this.origNormalTextures.ContainsKey(material))
								{
									this.origNormalTextures.Add(material, texture2D);
								}
								material.SetTexture("_BumpMap", characterQueuedImage.tex);
								flag = true;
							}
							break;
						case DAZCharacterTextureControl.TextureType.Detail:
							if (material.HasProperty("_DetailMap"))
							{
								texture2D = (Texture2D)material.GetTexture("_DetailMap");
								if (!this.origDetailTextures.ContainsKey(material))
								{
									this.origDetailTextures.Add(material, texture2D);
								}
								material.SetTexture("_DetailMap", characterQueuedImage.tex);
								flag = true;
							}
							break;
						case DAZCharacterTextureControl.TextureType.Decal:
							if (material.HasProperty("_DecalTex"))
							{
								texture2D = (Texture2D)material.GetTexture("_DecalTex");
								if (!this.origDecalTextures.ContainsKey(material))
								{
									this.origDecalTextures.Add(material, texture2D);
								}
								material.SetTexture("_DecalTex", characterQueuedImage.tex);
								flag = true;
							}
							break;
						}
						if (flag && characterQueuedImage.tex != null)
						{
							this.RegisterTexture(characterQueuedImage.tex);
						}
						if (texture2D != null)
						{
							this.DeregisterTexture(texture2D);
						}
					}
				}
				if (characterQueuedImage.region == DAZCharacterTextureControl.Region.Torso)
				{
					if (characterQueuedImage.textureType == DAZCharacterTextureControl.TextureType.Diffuse)
					{
						this.SyncDiffuseGenitalTexture();
					}
					else if (characterQueuedImage.textureType == DAZCharacterTextureControl.TextureType.Specular)
					{
						this.SyncSpecularGenitalTexture();
					}
					else if (characterQueuedImage.textureType == DAZCharacterTextureControl.TextureType.Gloss)
					{
						this.SyncGlossGenitalTexture();
					}
					else if (characterQueuedImage.textureType == DAZCharacterTextureControl.TextureType.Normal)
					{
						this.SyncNormalGenitalTexture();
					}
				}
			}
		}
		else
		{
			SuperController.LogError("Error during texture load: " + qi.errorText);
		}
	}

	// Token: 0x06004878 RID: 18552 RVA: 0x0016A274 File Offset: 0x00168674
	public void StartSyncImage(string url, DAZCharacterTextureControl.Region region, DAZCharacterTextureControl.TextureType ttype, bool forceReload)
	{
		if (url != null && url != string.Empty)
		{
			if (Regex.IsMatch(url, "^http"))
			{
				SuperController.LogError("Character texture loaded does not currently support http image urls");
			}
			else if (ImageLoaderThreaded.singleton != null)
			{
				DAZCharacterTextureControl.CharacterQueuedImage characterQueuedImage = new DAZCharacterTextureControl.CharacterQueuedImage();
				characterQueuedImage.imgPath = url;
				characterQueuedImage.forceReload = forceReload;
				bool createMipMaps = true;
				bool linear = false;
				bool isNormalMap = false;
				bool compress = true;
				switch (ttype)
				{
				case DAZCharacterTextureControl.TextureType.Specular:
				case DAZCharacterTextureControl.TextureType.Gloss:
					linear = true;
					break;
				case DAZCharacterTextureControl.TextureType.Normal:
				case DAZCharacterTextureControl.TextureType.Detail:
					linear = true;
					isNormalMap = true;
					compress = false;
					break;
				}
				characterQueuedImage.createMipMaps = createMipMaps;
				characterQueuedImage.linear = linear;
				characterQueuedImage.region = region;
				characterQueuedImage.textureType = ttype;
				characterQueuedImage.isNormalMap = isNormalMap;
				characterQueuedImage.compress = compress;
				characterQueuedImage.callback = new ImageLoaderThreaded.ImageLoaderCallback(this.OnImageLoaded);
				ImageLoaderThreaded.singleton.QueueImage(characterQueuedImage);
			}
		}
		else
		{
			DAZMergedSkinV2 component = base.GetComponent<DAZMergedSkinV2>();
			if (component != null)
			{
				int[] array = null;
				switch (region)
				{
				case DAZCharacterTextureControl.Region.Face:
					array = this.faceMaterialNums;
					break;
				case DAZCharacterTextureControl.Region.Torso:
					array = this.torsoMaterialNums;
					if (ttype == DAZCharacterTextureControl.TextureType.Diffuse)
					{
						this.SyncDiffuseGenitalTexture();
					}
					else if (ttype == DAZCharacterTextureControl.TextureType.Specular)
					{
						this.SyncSpecularGenitalTexture();
					}
					else if (ttype == DAZCharacterTextureControl.TextureType.Gloss)
					{
						this.SyncGlossGenitalTexture();
					}
					else if (ttype == DAZCharacterTextureControl.TextureType.Normal)
					{
						this.SyncNormalGenitalTexture();
					}
					break;
				case DAZCharacterTextureControl.Region.Limbs:
					array = this.limbMaterialNums;
					break;
				case DAZCharacterTextureControl.Region.Genitals:
					array = this.genitalMaterialNums;
					break;
				}
				if (array != null)
				{
					foreach (int num in array)
					{
						Material material = component.GPUmaterials[num];
						Texture2D texture2D = null;
						switch (ttype)
						{
						case DAZCharacterTextureControl.TextureType.Diffuse:
						{
							Texture2D value;
							if (material.HasProperty("_MainTex") && this.origDiffuseTextures.TryGetValue(material, out value))
							{
								texture2D = (Texture2D)material.GetTexture("_MainTex");
								material.SetTexture("_MainTex", value);
							}
							break;
						}
						case DAZCharacterTextureControl.TextureType.Specular:
						{
							Texture2D value2;
							if (material.HasProperty("_SpecTex") && this.origSpecularTextures.TryGetValue(material, out value2))
							{
								texture2D = (Texture2D)material.GetTexture("_SpecTex");
								material.SetTexture("_SpecTex", value2);
							}
							break;
						}
						case DAZCharacterTextureControl.TextureType.Gloss:
						{
							Texture2D value3;
							if (material.HasProperty("_GlossTex") && this.origGlossTextures.TryGetValue(material, out value3))
							{
								texture2D = (Texture2D)material.GetTexture("_GlossTex");
								material.SetTexture("_GlossTex", value3);
							}
							break;
						}
						case DAZCharacterTextureControl.TextureType.Normal:
						{
							Texture2D value4;
							if (material.HasProperty("_BumpMap") && this.origNormalTextures.TryGetValue(material, out value4))
							{
								texture2D = (Texture2D)material.GetTexture("_BumpMap");
								material.SetTexture("_BumpMap", value4);
							}
							break;
						}
						case DAZCharacterTextureControl.TextureType.Detail:
						{
							Texture2D value5;
							if (material.HasProperty("_DetailMap") && this.origDetailTextures.TryGetValue(material, out value5))
							{
								texture2D = (Texture2D)material.GetTexture("_DetailMap");
								material.SetTexture("_DetailMap", value5);
							}
							break;
						}
						case DAZCharacterTextureControl.TextureType.Decal:
						{
							Texture2D value6;
							if (material.HasProperty("_DecalTex") && this.origDecalTextures.TryGetValue(material, out value6))
							{
								texture2D = (Texture2D)material.GetTexture("_DecalTex");
								material.SetTexture("_DecalTex", value6);
							}
							break;
						}
						}
						if (texture2D != null)
						{
							this.DeregisterTexture(texture2D);
						}
					}
				}
			}
		}
	}

	// Token: 0x06004879 RID: 18553 RVA: 0x0016A644 File Offset: 0x00168A44
	protected void SyncUrlTextColorToError(JSONStorableUrl jsurl, bool hadError)
	{
		if (hadError)
		{
			if (jsurl.text != null)
			{
				jsurl.text.color = Color.red;
			}
			if (jsurl.textAlt != null)
			{
				jsurl.text.color = Color.red;
			}
		}
		else
		{
			if (jsurl.text != null)
			{
				jsurl.text.color = Color.white;
			}
			if (jsurl.textAlt != null)
			{
				jsurl.text.color = Color.white;
			}
		}
	}

	// Token: 0x0600487A RID: 18554 RVA: 0x0016A6E0 File Offset: 0x00168AE0
	protected void SyncFaceDiffuseHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.faceDiffuseUrlJSON, b);
	}

	// Token: 0x0600487B RID: 18555 RVA: 0x0016A6F0 File Offset: 0x00168AF0
	protected void SyncFaceDiffuseUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.ClearImageErrorCondition(DAZCharacterTextureControl.Region.Face, DAZCharacterTextureControl.TextureType.Diffuse);
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Face, DAZCharacterTextureControl.TextureType.Diffuse, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x0600487C RID: 18556 RVA: 0x0016A722 File Offset: 0x00168B22
	public bool hasFaceDiffuseTextureSet
	{
		get
		{
			return this.faceDiffuseUrlJSON != null && this.faceDiffuseUrlJSON.val != null && !(this.faceDiffuseUrlJSON.val == string.Empty);
		}
	}

	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x0600487D RID: 18557 RVA: 0x0016A75C File Offset: 0x00168B5C
	public bool hasFaceTextureSet
	{
		get
		{
			return (this.faceDiffuseUrlJSON != null && this.faceDiffuseUrlJSON.val != null && this.faceDiffuseUrlJSON.val != string.Empty) || (this.faceSpecularUrlJSON != null && this.faceSpecularUrlJSON.val != null && this.faceSpecularUrlJSON.val != string.Empty) || (this.faceGlossUrlJSON != null && this.faceGlossUrlJSON.val != null && this.faceGlossUrlJSON.val != string.Empty) || (this.faceNormalUrlJSON != null && this.faceNormalUrlJSON.val != null && this.faceNormalUrlJSON.val != string.Empty) || (this.faceDetailUrlJSON != null && this.faceDetailUrlJSON.val != null && this.faceDetailUrlJSON.val != string.Empty);
		}
	}

	// Token: 0x0600487E RID: 18558 RVA: 0x0016A87D File Offset: 0x00168C7D
	protected void SyncTorsoDiffuseHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.torsoDiffuseUrlJSON, b);
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x0016A88C File Offset: 0x00168C8C
	protected void SyncTorsoDiffuseUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Torso, DAZCharacterTextureControl.TextureType.Diffuse, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x06004880 RID: 18560 RVA: 0x0016A8B6 File Offset: 0x00168CB6
	public bool hasTorsoDiffuseTextureSet
	{
		get
		{
			return this.torsoDiffuseUrlJSON != null && this.torsoDiffuseUrlJSON.val != null && !(this.torsoDiffuseUrlJSON.val == string.Empty);
		}
	}

	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x06004881 RID: 18561 RVA: 0x0016A8F0 File Offset: 0x00168CF0
	public bool hasTorsoTextureSet
	{
		get
		{
			return (this.torsoDiffuseUrlJSON != null && this.torsoDiffuseUrlJSON.val != null && this.torsoDiffuseUrlJSON.val != string.Empty) || (this.torsoSpecularUrlJSON != null && this.torsoSpecularUrlJSON.val != null && this.torsoSpecularUrlJSON.val != string.Empty) || (this.torsoGlossUrlJSON != null && this.torsoGlossUrlJSON.val != null && this.torsoGlossUrlJSON.val != string.Empty) || (this.torsoNormalUrlJSON != null && this.torsoNormalUrlJSON.val != null && this.torsoNormalUrlJSON.val != string.Empty) || (this.torsoDetailUrlJSON != null && this.torsoDetailUrlJSON.val != null && this.torsoDetailUrlJSON.val != string.Empty);
		}
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x0016AA11 File Offset: 0x00168E11
	protected void SyncLimbsDiffuseHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.limbsDiffuseUrlJSON, b);
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x0016AA20 File Offset: 0x00168E20
	protected void SyncLimbsDiffuseUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Limbs, DAZCharacterTextureControl.TextureType.Diffuse, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x06004884 RID: 18564 RVA: 0x0016AA4A File Offset: 0x00168E4A
	public bool hasLimbsDiffuseTextureSet
	{
		get
		{
			return this.limbsDiffuseUrlJSON != null && this.limbsDiffuseUrlJSON.val != null && !(this.limbsDiffuseUrlJSON.val == string.Empty);
		}
	}

	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x06004885 RID: 18565 RVA: 0x0016AA84 File Offset: 0x00168E84
	public bool hasLimbsTextureSet
	{
		get
		{
			return (this.limbsDiffuseUrlJSON != null && this.limbsDiffuseUrlJSON.val != null && this.limbsDiffuseUrlJSON.val != string.Empty) || (this.limbsSpecularUrlJSON != null && this.limbsSpecularUrlJSON.val != null && this.limbsSpecularUrlJSON.val != string.Empty) || (this.limbsGlossUrlJSON != null && this.limbsGlossUrlJSON.val != null && this.limbsGlossUrlJSON.val != string.Empty) || (this.limbsNormalUrlJSON != null && this.limbsNormalUrlJSON.val != null && this.limbsNormalUrlJSON.val != string.Empty) || (this.limbsDetailUrlJSON != null && this.limbsDetailUrlJSON.val != null && this.limbsDetailUrlJSON.val != string.Empty);
		}
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x0016ABA5 File Offset: 0x00168FA5
	protected void SyncGenitalsDiffuseHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.genitalsDiffuseUrlJSON, b);
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x0016ABB4 File Offset: 0x00168FB4
	protected void SyncGenitalsDiffuseUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (!this.autoBlendGenitalTexturesJSON.val)
		{
			this.StartSyncImage(val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Diffuse, jsonstorableUrl.valueSetFromBrowse);
		}
	}

	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x06004888 RID: 18568 RVA: 0x0016ABEE File Offset: 0x00168FEE
	public bool hasGenitalsDiffuseTextureSet
	{
		get
		{
			return this.genitalsDiffuseUrlJSON != null && this.genitalsDiffuseUrlJSON.val != null && !(this.genitalsDiffuseUrlJSON.val == string.Empty);
		}
	}

	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06004889 RID: 18569 RVA: 0x0016AC28 File Offset: 0x00169028
	public bool hasGenitalsTextureSet
	{
		get
		{
			return (this.genitalsDiffuseUrlJSON != null && this.genitalsDiffuseUrlJSON.val != null && this.genitalsDiffuseUrlJSON.val != string.Empty) || (this.genitalsSpecularUrlJSON != null && this.genitalsSpecularUrlJSON.val != null && this.genitalsSpecularUrlJSON.val != string.Empty) || (this.genitalsGlossUrlJSON != null && this.genitalsGlossUrlJSON.val != null && this.genitalsGlossUrlJSON.val != string.Empty) || (this.genitalsNormalUrlJSON != null && this.genitalsNormalUrlJSON.val != null && this.genitalsNormalUrlJSON.val != string.Empty) || (this.genitalsDetailUrlJSON != null && this.genitalsDetailUrlJSON.val != null && this.genitalsDetailUrlJSON.val != string.Empty);
		}
	}

	// Token: 0x0600488A RID: 18570 RVA: 0x0016AD49 File Offset: 0x00169149
	protected void SyncFaceSpecularHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.faceSpecularUrlJSON, b);
	}

	// Token: 0x0600488B RID: 18571 RVA: 0x0016AD58 File Offset: 0x00169158
	protected void SyncFaceSpecularUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Face, DAZCharacterTextureControl.TextureType.Specular, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x0600488C RID: 18572 RVA: 0x0016AD82 File Offset: 0x00169182
	protected void SyncTorsoSpecularHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.torsoSpecularUrlJSON, b);
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x0016AD94 File Offset: 0x00169194
	protected void SyncTorsoSpecularUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Torso, DAZCharacterTextureControl.TextureType.Specular, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x0016ADBE File Offset: 0x001691BE
	protected void SyncLimbsSpecularHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.limbsSpecularUrlJSON, b);
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x0016ADD0 File Offset: 0x001691D0
	protected void SyncLimbsSpecularUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Limbs, DAZCharacterTextureControl.TextureType.Specular, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x0016ADFA File Offset: 0x001691FA
	protected void SyncGenitalsSpecularHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.genitalsSpecularUrlJSON, b);
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x0016AE0C File Offset: 0x0016920C
	protected void SyncGenitalsSpecularUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (!this.autoBlendGenitalTexturesJSON.val || !this.autoBlendGenitalSpecGlossNormalTexturesJSON.val)
		{
			this.StartSyncImage(val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Specular, jsonstorableUrl.valueSetFromBrowse);
		}
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x0016AE56 File Offset: 0x00169256
	protected void SyncFaceGlossHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.faceGlossUrlJSON, b);
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x0016AE68 File Offset: 0x00169268
	protected void SyncFaceGlossUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Face, DAZCharacterTextureControl.TextureType.Gloss, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x0016AE92 File Offset: 0x00169292
	protected void SyncTorsoGlossHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.torsoGlossUrlJSON, b);
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x0016AEA4 File Offset: 0x001692A4
	protected void SyncTorsoGlossUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Torso, DAZCharacterTextureControl.TextureType.Gloss, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x0016AECE File Offset: 0x001692CE
	protected void SyncLimbsGlossHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.limbsGlossUrlJSON, b);
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x0016AEE0 File Offset: 0x001692E0
	protected void SyncLimbsGlossUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Limbs, DAZCharacterTextureControl.TextureType.Gloss, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x0016AF0A File Offset: 0x0016930A
	protected void SyncGenitalsGlossHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.genitalsGlossUrlJSON, b);
	}

	// Token: 0x06004899 RID: 18585 RVA: 0x0016AF1C File Offset: 0x0016931C
	protected void SyncGenitalsGlossUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (!this.autoBlendGenitalTexturesJSON.val || !this.autoBlendGenitalSpecGlossNormalTexturesJSON.val)
		{
			this.StartSyncImage(val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Gloss, jsonstorableUrl.valueSetFromBrowse);
		}
	}

	// Token: 0x0600489A RID: 18586 RVA: 0x0016AF66 File Offset: 0x00169366
	protected void SyncFaceNormalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.faceNormalUrlJSON, b);
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x0016AF78 File Offset: 0x00169378
	protected void SyncFaceNormalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Face, DAZCharacterTextureControl.TextureType.Normal, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x0600489C RID: 18588 RVA: 0x0016AFA2 File Offset: 0x001693A2
	protected void SyncTorsoNormalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.torsoNormalUrlJSON, b);
	}

	// Token: 0x0600489D RID: 18589 RVA: 0x0016AFB4 File Offset: 0x001693B4
	protected void SyncTorsoNormalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Torso, DAZCharacterTextureControl.TextureType.Normal, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x0600489E RID: 18590 RVA: 0x0016AFDE File Offset: 0x001693DE
	protected void SyncLimbsNormalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.limbsNormalUrlJSON, b);
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x0016AFF0 File Offset: 0x001693F0
	protected void SyncLimbsNormalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Limbs, DAZCharacterTextureControl.TextureType.Normal, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048A0 RID: 18592 RVA: 0x0016B01A File Offset: 0x0016941A
	protected void SyncGenitalsNormalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.genitalsNormalUrlJSON, b);
	}

	// Token: 0x060048A1 RID: 18593 RVA: 0x0016B02C File Offset: 0x0016942C
	protected void SyncGenitalsNormalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		if (!this.autoBlendGenitalTexturesJSON.val || !this.autoBlendGenitalSpecGlossNormalTexturesJSON.val)
		{
			this.StartSyncImage(val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Normal, jsonstorableUrl.valueSetFromBrowse);
		}
	}

	// Token: 0x060048A2 RID: 18594 RVA: 0x0016B076 File Offset: 0x00169476
	protected void SyncFaceDetailHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.faceDetailUrlJSON, b);
	}

	// Token: 0x060048A3 RID: 18595 RVA: 0x0016B088 File Offset: 0x00169488
	protected void SyncFaceDetailUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Face, DAZCharacterTextureControl.TextureType.Detail, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048A4 RID: 18596 RVA: 0x0016B0B2 File Offset: 0x001694B2
	protected void SyncTorsoDetailHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.torsoDetailUrlJSON, b);
	}

	// Token: 0x060048A5 RID: 18597 RVA: 0x0016B0C4 File Offset: 0x001694C4
	protected void SyncTorsoDetailUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Torso, DAZCharacterTextureControl.TextureType.Detail, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048A6 RID: 18598 RVA: 0x0016B0EE File Offset: 0x001694EE
	protected void SyncLimbsDetailHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.limbsDetailUrlJSON, b);
	}

	// Token: 0x060048A7 RID: 18599 RVA: 0x0016B100 File Offset: 0x00169500
	protected void SyncLimbsDetailUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Limbs, DAZCharacterTextureControl.TextureType.Detail, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x0016B12A File Offset: 0x0016952A
	protected void SyncGenitalsDetailHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.genitalsDetailUrlJSON, b);
	}

	// Token: 0x060048A9 RID: 18601 RVA: 0x0016B13C File Offset: 0x0016953C
	protected void SyncGenitalsDetailUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Detail, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048AA RID: 18602 RVA: 0x0016B166 File Offset: 0x00169566
	protected void SyncFaceDecalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.faceDecalUrlJSON, b);
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x0016B178 File Offset: 0x00169578
	protected void SyncFaceDecalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Face, DAZCharacterTextureControl.TextureType.Decal, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048AC RID: 18604 RVA: 0x0016B1A2 File Offset: 0x001695A2
	protected void SyncTorsoDecalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.torsoDecalUrlJSON, b);
	}

	// Token: 0x060048AD RID: 18605 RVA: 0x0016B1B4 File Offset: 0x001695B4
	protected void SyncTorsoDecalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Torso, DAZCharacterTextureControl.TextureType.Decal, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048AE RID: 18606 RVA: 0x0016B1DE File Offset: 0x001695DE
	protected void SyncLimbsDecalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.limbsDecalUrlJSON, b);
	}

	// Token: 0x060048AF RID: 18607 RVA: 0x0016B1F0 File Offset: 0x001695F0
	protected void SyncLimbsDecalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Limbs, DAZCharacterTextureControl.TextureType.Decal, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048B0 RID: 18608 RVA: 0x0016B21A File Offset: 0x0016961A
	protected void SyncGenitalsDecalHadError(bool b)
	{
		this.SyncUrlTextColorToError(this.genitalsDecalUrlJSON, b);
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x0016B22C File Offset: 0x0016962C
	protected void SyncGenitalsDecalUrl(JSONStorableString jstr)
	{
		JSONStorableUrl jsonstorableUrl = jstr as JSONStorableUrl;
		string val = jsonstorableUrl.val;
		this.StartSyncImage(val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Decal, jsonstorableUrl.valueSetFromBrowse);
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x0016B258 File Offset: 0x00169658
	protected void DumpGeneratedGenitalTextures()
	{
		if (this.lastGeneratedDiffuseGenTexture != null)
		{
			byte[] bytes = this.lastGeneratedDiffuseGenTexture.EncodeToPNG();
			File.WriteAllBytes("GensDiffuse.png", bytes);
		}
		if (this.lastGeneratedSpecularGenTexture != null)
		{
			byte[] bytes2 = this.lastGeneratedSpecularGenTexture.EncodeToPNG();
			File.WriteAllBytes("GensSpecular.png", bytes2);
		}
		if (this.lastGeneratedGlossGenTexture != null)
		{
			byte[] bytes3 = this.lastGeneratedGlossGenTexture.EncodeToPNG();
			File.WriteAllBytes("GensGloss.png", bytes3);
		}
		if (this.lastGeneratedNormalGenTexture != null)
		{
			byte[] bytes4 = this.lastGeneratedNormalGenTexture.EncodeToPNG();
			File.WriteAllBytes("GensNormal.png", bytes4);
		}
	}

	// Token: 0x060048B3 RID: 18611 RVA: 0x0016B308 File Offset: 0x00169708
	protected Texture2D BlendGenitalTexture(Texture2D inTorsoTex, Texture2D inGenTex, bool linear, bool colorAdjust, bool isNormalMap = false)
	{
		Texture2D texture2D = new Texture2D(inTorsoTex.width, inTorsoTex.height, TextureFormat.RGBA32, true, linear);
		try
		{
			inTorsoTex.GetPixel(0, 0);
		}
		catch
		{
			return null;
		}
		try
		{
			Color[] pixels = inTorsoTex.GetPixels();
			Color[] pixels2 = inGenTex.GetPixels();
			Color[] pixels3 = this.genitalBlendMaskTexture.GetPixels();
			Color[] array = new Color[pixels.Length];
			bool flag = false;
			bool flag2 = false;
			if (isNormalMap)
			{
				if (inTorsoTex.format == TextureFormat.DXT5 || inTorsoTex.format == TextureFormat.BC7)
				{
					flag = true;
				}
				if (inGenTex.format == TextureFormat.DXT5 || inGenTex.format == TextureFormat.BC7)
				{
					flag2 = true;
				}
			}
			for (int i = 0; i < pixels.Length; i++)
			{
				Color a = pixels2[i];
				if (colorAdjust && (this.autoBlendGenitalLightenDarkenJSON.val != 0f || this.autoBlendGenitalHueOffsetJSON.val != 0f || this.autoBlendGenitalSaturationOffsetJSON.val != 0f))
				{
					HSVColor hsvColor = HSVColorPicker.RGBToHSV(a.r, a.g, a.b);
					hsvColor.V = Mathf.Clamp01(hsvColor.V + this.autoBlendGenitalLightenDarkenJSON.val);
					hsvColor.H = Mathf.Clamp01(hsvColor.H + this.autoBlendGenitalHueOffsetJSON.val);
					hsvColor.S = Mathf.Clamp01(hsvColor.S + this.autoBlendGenitalSaturationOffsetJSON.val);
					a = HSVColorPicker.HSVToRGB(hsvColor);
				}
				float r = pixels3[i].r;
				float num = 1f - pixels3[i].r;
				Color a2 = pixels[i];
				Color color;
				if (isNormalMap)
				{
					if (flag)
					{
						color.r = a2.a * num;
						color.g = a2.g * num;
					}
					else
					{
						color.r = a2.r * num;
						color.g = a2.g * num;
					}
					if (flag2)
					{
						color.r += a.a * r;
						color.g += a.g * r;
					}
					else
					{
						color.r += a.r * r;
						color.g += a.g * r;
					}
					color.b = 1f;
					color.a = 1f;
				}
				else
				{
					color = a2 * num + a * r;
					color.a = 1f;
				}
				array[i] = color;
			}
			texture2D.SetPixels(array);
			texture2D.Apply();
		}
		catch (UnityException arg)
		{
			SuperController.LogError("Exception while blending texture " + arg);
		}
		return texture2D;
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x0016B648 File Offset: 0x00169A48
	protected void SyncDiffuseGenitalTexture()
	{
		if (this.autoBlendGenitalTexturesJSON.val && this.torsoDiffuseUrlJSON.val != string.Empty)
		{
			DAZMergedSkinV2 component = base.GetComponent<DAZMergedSkinV2>();
			if (component != null && this.genitalBlendMaskTexture != null)
			{
				int num = this.torsoMaterialNums[0];
				Material material = component.GPUmaterials[num];
				int num2 = this.genitalMaterialNums[0];
				Material material2 = component.GPUmaterials[num2];
				if (material.HasProperty("_MainTex") && material2.HasProperty("_MainTex"))
				{
					Texture2D texture2D = (Texture2D)material.GetTexture("_MainTex");
					if (texture2D.width != 4096 && texture2D.height != 4096)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Genital diffuse texture blending only works if torso texture is 4096x4096. Torso diffuse texture is ",
							texture2D.width,
							" by ",
							texture2D.height
						}));
						return;
					}
					Texture2D texture2D2;
					if (!this.origDiffuseTextures.TryGetValue(material2, out texture2D2))
					{
						texture2D2 = (Texture2D)material2.GetTexture("_MainTex");
						this.origDiffuseTextures.Add(material2, texture2D2);
					}
					Texture2D texture2D3 = this.BlendGenitalTexture(texture2D, texture2D2, false, true, false);
					if (texture2D3 != null)
					{
						foreach (int num3 in this.genitalMaterialNums)
						{
							Material material3 = component.GPUmaterials[num3];
							if (material3.HasProperty("_MainTex"))
							{
								Texture2D texture2D4 = (Texture2D)material3.GetTexture("_MainTex");
								if (!this.origDiffuseTextures.ContainsKey(material3))
								{
									this.origDiffuseTextures.Add(material3, texture2D4);
								}
								material3.SetTexture("_MainTex", texture2D3);
								if (texture2D4 != null)
								{
									this.DeregisterTexture(texture2D4);
								}
							}
						}
						if (this.lastGeneratedDiffuseGenTexture != null)
						{
							UnityEngine.Object.Destroy(this.lastGeneratedDiffuseGenTexture);
						}
						this.lastGeneratedDiffuseGenTexture = texture2D3;
					}
				}
			}
		}
		else if (this.lastGeneratedDiffuseGenTexture != null)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedDiffuseGenTexture);
			this.lastGeneratedDiffuseGenTexture = null;
			this.StartSyncImage(this.genitalsDiffuseUrlJSON.val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Diffuse, false);
		}
	}

	// Token: 0x060048B5 RID: 18613 RVA: 0x0016B8AC File Offset: 0x00169CAC
	protected void SyncSpecularGenitalTexture()
	{
		if (this.autoBlendGenitalTexturesJSON.val && this.autoBlendGenitalSpecGlossNormalTexturesJSON.val && this.torsoSpecularUrlJSON.val != string.Empty)
		{
			DAZMergedSkinV2 component = base.GetComponent<DAZMergedSkinV2>();
			if (component != null && this.genitalBlendMaskTexture != null)
			{
				int num = this.torsoMaterialNums[0];
				Material material = component.GPUmaterials[num];
				int num2 = this.genitalMaterialNums[0];
				Material material2 = component.GPUmaterials[num2];
				if (material.HasProperty("_SpecTex") && material2.HasProperty("_SpecTex"))
				{
					Texture2D texture2D = (Texture2D)material.GetTexture("_SpecTex");
					if (texture2D.width != 4096 && texture2D.height != 4096)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Genital specular texture blending only works if torso texture is 4096x4096. Torso specular texture is ",
							texture2D.width,
							" by ",
							texture2D.height
						}));
						return;
					}
					Texture2D texture2D2;
					if (!this.origSpecularTextures.TryGetValue(material2, out texture2D2))
					{
						texture2D2 = (Texture2D)material2.GetTexture("_SpecTex");
						this.origSpecularTextures.Add(material2, texture2D2);
					}
					Texture2D texture2D3 = this.BlendGenitalTexture(texture2D, texture2D2, true, false, false);
					if (texture2D3 != null)
					{
						foreach (int num3 in this.genitalMaterialNums)
						{
							Material material3 = component.GPUmaterials[num3];
							if (material3.HasProperty("_SpecTex"))
							{
								Texture2D texture2D4 = (Texture2D)material3.GetTexture("_SpecTex");
								if (!this.origSpecularTextures.ContainsKey(material3))
								{
									this.origSpecularTextures.Add(material3, texture2D4);
								}
								material3.SetTexture("_SpecTex", texture2D3);
								if (texture2D4 != null)
								{
									this.DeregisterTexture(texture2D4);
								}
							}
						}
						if (this.lastGeneratedSpecularGenTexture != null)
						{
							UnityEngine.Object.Destroy(this.lastGeneratedSpecularGenTexture);
						}
						this.lastGeneratedSpecularGenTexture = texture2D3;
					}
				}
			}
		}
		else if (this.lastGeneratedSpecularGenTexture != null)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedSpecularGenTexture);
			this.lastGeneratedSpecularGenTexture = null;
			this.StartSyncImage(this.genitalsSpecularUrlJSON.val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Specular, false);
		}
	}

	// Token: 0x060048B6 RID: 18614 RVA: 0x0016BB20 File Offset: 0x00169F20
	protected void SyncGlossGenitalTexture()
	{
		if (this.autoBlendGenitalTexturesJSON.val && this.autoBlendGenitalSpecGlossNormalTexturesJSON.val && this.torsoGlossUrlJSON.val != string.Empty)
		{
			DAZMergedSkinV2 component = base.GetComponent<DAZMergedSkinV2>();
			if (component != null && this.genitalBlendMaskTexture != null)
			{
				int num = this.torsoMaterialNums[0];
				Material material = component.GPUmaterials[num];
				int num2 = this.genitalMaterialNums[0];
				Material material2 = component.GPUmaterials[num2];
				if (material.HasProperty("_GlossTex") && material2.HasProperty("_GlossTex"))
				{
					Texture2D texture2D = (Texture2D)material.GetTexture("_GlossTex");
					if (texture2D.width != 4096 && texture2D.height != 4096)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Genital gloss texture blending only works if torso texture is 4096x4096. Torso gloss texture is ",
							texture2D.width,
							" by ",
							texture2D.height
						}));
						return;
					}
					Texture2D texture2D2;
					if (!this.origGlossTextures.TryGetValue(material2, out texture2D2))
					{
						texture2D2 = (Texture2D)material2.GetTexture("_GlossTex");
						this.origGlossTextures.Add(material2, texture2D2);
					}
					Texture2D texture2D3 = this.BlendGenitalTexture(texture2D, texture2D2, true, false, false);
					if (texture2D3 != null)
					{
						foreach (int num3 in this.genitalMaterialNums)
						{
							Material material3 = component.GPUmaterials[num3];
							if (material3.HasProperty("_GlossTex"))
							{
								Texture2D texture2D4 = (Texture2D)material3.GetTexture("_GlossTex");
								if (!this.origGlossTextures.ContainsKey(material3))
								{
									this.origGlossTextures.Add(material3, texture2D4);
								}
								material3.SetTexture("_GlossTex", texture2D3);
								if (texture2D4 != null)
								{
									this.DeregisterTexture(texture2D4);
								}
							}
						}
						if (this.lastGeneratedGlossGenTexture != null)
						{
							UnityEngine.Object.Destroy(this.lastGeneratedGlossGenTexture);
						}
						this.lastGeneratedGlossGenTexture = texture2D3;
					}
				}
			}
		}
		else if (this.lastGeneratedGlossGenTexture != null)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedGlossGenTexture);
			this.lastGeneratedGlossGenTexture = null;
			this.StartSyncImage(this.genitalsGlossUrlJSON.val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Gloss, false);
		}
	}

	// Token: 0x060048B7 RID: 18615 RVA: 0x0016BD94 File Offset: 0x0016A194
	protected void SyncNormalGenitalTexture()
	{
		if (this.autoBlendGenitalTexturesJSON.val && this.autoBlendGenitalSpecGlossNormalTexturesJSON.val && this.torsoNormalUrlJSON.val != string.Empty)
		{
			DAZMergedSkinV2 component = base.GetComponent<DAZMergedSkinV2>();
			if (component != null && this.genitalBlendMaskTexture != null)
			{
				int num = this.torsoMaterialNums[0];
				Material material = component.GPUmaterials[num];
				int num2 = this.genitalMaterialNums[0];
				Material material2 = component.GPUmaterials[num2];
				if (material.HasProperty("_BumpMap") && material2.HasProperty("_BumpMap"))
				{
					Texture2D texture2D = (Texture2D)material.GetTexture("_BumpMap");
					if (texture2D.width != 4096 && texture2D.height != 4096)
					{
						SuperController.LogError(string.Concat(new object[]
						{
							"Genital normal texture blending only works if torso texture is 4096x4096. Torso normal texture is ",
							texture2D.width,
							" by ",
							texture2D.height
						}));
						return;
					}
					Texture2D texture2D2;
					if (!this.origNormalTextures.TryGetValue(material2, out texture2D2))
					{
						texture2D2 = (Texture2D)material2.GetTexture("_BumpMap");
						this.origNormalTextures.Add(material2, texture2D2);
					}
					Texture2D texture2D3 = this.BlendGenitalTexture(texture2D, texture2D2, true, false, true);
					if (texture2D3 != null)
					{
						foreach (int num3 in this.genitalMaterialNums)
						{
							Material material3 = component.GPUmaterials[num3];
							if (material3.HasProperty("_BumpMap"))
							{
								Texture2D texture2D4 = (Texture2D)material3.GetTexture("_BumpMap");
								if (!this.origNormalTextures.ContainsKey(material3))
								{
									this.origNormalTextures.Add(material3, texture2D4);
								}
								material3.SetTexture("_BumpMap", texture2D3);
								if (texture2D4 != null)
								{
									this.DeregisterTexture(texture2D4);
								}
							}
						}
						if (this.lastGeneratedNormalGenTexture != null)
						{
							UnityEngine.Object.Destroy(this.lastGeneratedNormalGenTexture);
						}
						this.lastGeneratedNormalGenTexture = texture2D3;
					}
				}
			}
		}
		else if (this.lastGeneratedNormalGenTexture != null)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedNormalGenTexture);
			this.lastGeneratedNormalGenTexture = null;
			this.StartSyncImage(this.genitalsNormalUrlJSON.val, DAZCharacterTextureControl.Region.Genitals, DAZCharacterTextureControl.TextureType.Normal, false);
		}
	}

	// Token: 0x060048B8 RID: 18616 RVA: 0x0016C005 File Offset: 0x0016A405
	protected void SyncAutoBlendGenitalTextures(bool b)
	{
		this.SyncDiffuseGenitalTexture();
		this.SyncSpecularGenitalTexture();
		this.SyncGlossGenitalTexture();
		this.SyncNormalGenitalTexture();
	}

	// Token: 0x060048B9 RID: 18617 RVA: 0x0016C01F File Offset: 0x0016A41F
	protected void SyncAutoBlendGenitalSpecGlossNormalTextures(bool b)
	{
		this.SyncSpecularGenitalTexture();
		this.SyncGlossGenitalTexture();
		this.SyncNormalGenitalTexture();
	}

	// Token: 0x060048BA RID: 18618 RVA: 0x0016C033 File Offset: 0x0016A433
	protected void SyncAutoBlendGenitalLightenDarken(float f)
	{
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x0016C035 File Offset: 0x0016A435
	protected void SyncAutoBlendGenitalHueOffset(float f)
	{
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x0016C037 File Offset: 0x0016A437
	protected void SyncAutoBlendGenitalSaturationOffset(float f)
	{
	}

	// Token: 0x060048BD RID: 18621 RVA: 0x0016C03C File Offset: 0x0016A43C
	public void FindTexturesInDirectory(string dir)
	{
		if (dir != null && dir != string.Empty)
		{
			dir = dir.Replace("\\\\", "\\");
			if (!FileManager.DirectoryExists(dir, false, false))
			{
				dir = Path.GetDirectoryName(dir);
			}
			if (FileManager.DirectoryExists(dir, false, false))
			{
				this._lastDir = dir;
				DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(dir, false);
				dir = directoryEntry.Uid;
				JSONStorableUrl.SetSuggestedPathGroupPath("DAZCharacterTexture", dir);
				string text = dir + "/faceD.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceDiffuseUrlJSON.val = text;
				}
				text = dir + "/faceS.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceSpecularUrlJSON.val = text;
				}
				text = dir + "/faceG.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceGlossUrlJSON.val = text;
				}
				text = dir + "/faceD.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceDiffuseUrlJSON.val = text;
				}
				text = dir + "/faceS.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceSpecularUrlJSON.val = text;
				}
				text = dir + "/faceG.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceGlossUrlJSON.val = text;
				}
				text = dir + "/faceN.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceNormalUrlJSON.val = text;
				}
				text = dir + "/faceN.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceNormalUrlJSON.val = text;
				}
				text = dir + "/faceN.tif";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceNormalUrlJSON.val = text;
				}
				text = dir + "/faceDecal.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.faceDecalUrlJSON.val = text;
				}
				text = dir + "/torsoD.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoDiffuseUrlJSON.val = text;
				}
				text = dir + "/torsoS.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoSpecularUrlJSON.val = text;
				}
				text = dir + "/torsoG.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoGlossUrlJSON.val = text;
				}
				text = dir + "/torsoD.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoDiffuseUrlJSON.val = text;
				}
				text = dir + "/torsoS.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoSpecularUrlJSON.val = text;
				}
				text = dir + "/torsoG.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoGlossUrlJSON.val = text;
				}
				text = dir + "/torsoN.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoNormalUrlJSON.val = text;
				}
				text = dir + "/torsoN.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoNormalUrlJSON.val = text;
				}
				text = dir + "/torsoN.tif";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoNormalUrlJSON.val = text;
				}
				text = dir + "/torsoDecal.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.torsoDecalUrlJSON.val = text;
				}
				text = dir + "/limbsD.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsDiffuseUrlJSON.val = text;
				}
				text = dir + "/limbsS.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsSpecularUrlJSON.val = text;
				}
				text = dir + "/limbsG.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsGlossUrlJSON.val = text;
				}
				text = dir + "/limbsD.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsDiffuseUrlJSON.val = text;
				}
				text = dir + "/limbsS.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsSpecularUrlJSON.val = text;
				}
				text = dir + "/limbsG.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsGlossUrlJSON.val = text;
				}
				text = dir + "/limbsN.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsNormalUrlJSON.val = text;
				}
				text = dir + "/limbsN.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsNormalUrlJSON.val = text;
				}
				text = dir + "/limbsN.tif";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsNormalUrlJSON.val = text;
				}
				text = dir + "/limbsDecal.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.limbsDecalUrlJSON.val = text;
				}
				text = dir + "/genitalsD.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsDiffuseUrlJSON.val = text;
				}
				text = dir + "/genitalsS.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsSpecularUrlJSON.val = text;
				}
				text = dir + "/genitalsG.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsGlossUrlJSON.val = text;
				}
				text = dir + "/genitalsD.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsDiffuseUrlJSON.val = text;
				}
				text = dir + "/genitalsS.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsSpecularUrlJSON.val = text;
				}
				text = dir + "/genitalsG.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsGlossUrlJSON.val = text;
				}
				text = dir + "/genitalsN.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsNormalUrlJSON.val = text;
				}
				text = dir + "/genitalsN.jpg";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsNormalUrlJSON.val = text;
				}
				text = dir + "/genitalsN.tif";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsNormalUrlJSON.val = text;
				}
				text = dir + "/genitalsDecal.png";
				if (FileManager.FileExists(text, false, false))
				{
					this.genitalsDecalUrlJSON.val = text;
				}
			}
		}
	}

	// Token: 0x060048BE RID: 18622 RVA: 0x0016C680 File Offset: 0x0016AA80
	public void DirectoryBrowse()
	{
		if (SuperController.singleton != null)
		{
			if (this._lastDir == null)
			{
				this._lastDir = "Custom/Atom/Person/Textures";
			}
			List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory("Custom/Atom/Person/Textures", true, true, false, false);
			shortCutsForDirectory.Insert(0, new ShortCut
			{
				displayName = "Root",
				path = Path.GetFullPath(".")
			});
			SuperController.singleton.GetDirectoryPathDialog(new FileBrowserCallback(this.FindTexturesInDirectory), this._lastDir, shortCutsForDirectory, false);
		}
	}

	// Token: 0x060048BF RID: 18623 RVA: 0x0016C70C File Offset: 0x0016AB0C
	protected void BeginBrowse(JSONStorableUrl jsurl)
	{
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory("Custom/Atom/Person/Textures", true, true, false, false);
		shortCutsForDirectory.Insert(0, new ShortCut
		{
			displayName = "Root",
			path = Path.GetFullPath(".")
		});
		jsurl.shortCuts = shortCutsForDirectory;
	}

	// Token: 0x060048C0 RID: 18624 RVA: 0x0016C758 File Offset: 0x0016AB58
	protected void Init()
	{
		this.faceDiffuseUrlHadErrorJSON = new JSONStorableBool("faceDiffuseUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncFaceDiffuseHadError));
		this.faceDiffuseUrlJSON = new JSONStorableUrl("faceDiffuseUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncFaceDiffuseUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.faceDiffuseUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.faceDiffuseUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.faceDiffuseUrlJSON);
		this.torsoDiffuseUrlHadErrorJSON = new JSONStorableBool("torsoDiffuseUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncTorsoDiffuseHadError));
		this.torsoDiffuseUrlJSON = new JSONStorableUrl("torsoDiffuseUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncTorsoDiffuseUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.torsoDiffuseUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.torsoDiffuseUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.torsoDiffuseUrlJSON);
		this.limbsDiffuseUrlHadErrorJSON = new JSONStorableBool("limbsDiffuseUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncLimbsDiffuseHadError));
		this.limbsDiffuseUrlJSON = new JSONStorableUrl("limbsDiffuseUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncLimbsDiffuseUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.limbsDiffuseUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.limbsDiffuseUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.limbsDiffuseUrlJSON);
		this.genitalsDiffuseUrlHadErrorJSON = new JSONStorableBool("genitalsDiffuseUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncGenitalsDiffuseHadError));
		this.genitalsDiffuseUrlJSON = new JSONStorableUrl("genitalsDiffuseUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncGenitalsDiffuseUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.genitalsDiffuseUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.genitalsDiffuseUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.genitalsDiffuseUrlJSON);
		this.faceSpecularUrlHadErrorJSON = new JSONStorableBool("faceSpecularUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncFaceSpecularHadError));
		this.faceSpecularUrlJSON = new JSONStorableUrl("faceSpecularUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncFaceSpecularUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.faceSpecularUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.faceSpecularUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.faceSpecularUrlJSON);
		this.torsoSpecularUrlHadErrorJSON = new JSONStorableBool("torsoSpecularUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncTorsoSpecularHadError));
		this.torsoSpecularUrlJSON = new JSONStorableUrl("torsoSpecularUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncTorsoSpecularUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.torsoSpecularUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.torsoSpecularUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.torsoSpecularUrlJSON);
		this.limbsSpecularUrlHadErrorJSON = new JSONStorableBool("limbsSpecularUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncLimbsSpecularHadError));
		this.limbsSpecularUrlJSON = new JSONStorableUrl("limbsSpecularUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncLimbsSpecularUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.limbsSpecularUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.limbsSpecularUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.limbsSpecularUrlJSON);
		this.genitalsSpecularUrlHadErrorJSON = new JSONStorableBool("genitalsSpecularUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncGenitalsSpecularHadError));
		this.genitalsSpecularUrlJSON = new JSONStorableUrl("genitalsSpecularUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncGenitalsSpecularUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.genitalsSpecularUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.genitalsSpecularUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.genitalsSpecularUrlJSON);
		this.faceGlossUrlHadErrorJSON = new JSONStorableBool("faceGlossUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncFaceGlossHadError));
		this.faceGlossUrlJSON = new JSONStorableUrl("faceGlossUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncFaceGlossUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.faceGlossUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.faceGlossUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.faceGlossUrlJSON);
		this.torsoGlossUrlHadErrorJSON = new JSONStorableBool("torsoGlossUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncTorsoGlossHadError));
		this.torsoGlossUrlJSON = new JSONStorableUrl("torsoGlossUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncTorsoGlossUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.torsoGlossUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.torsoGlossUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.torsoGlossUrlJSON);
		this.limbsGlossUrlHadErrorJSON = new JSONStorableBool("limbsGlossUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncLimbsGlossHadError));
		this.limbsGlossUrlJSON = new JSONStorableUrl("limbsGlossUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncLimbsGlossUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.limbsGlossUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.limbsGlossUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.limbsGlossUrlJSON);
		this.genitalsGlossUrlHadErrorJSON = new JSONStorableBool("genitalsGlossUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncGenitalsGlossHadError));
		this.genitalsGlossUrlJSON = new JSONStorableUrl("genitalsGlossUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncGenitalsGlossUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.genitalsGlossUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.genitalsGlossUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.genitalsGlossUrlJSON);
		this.faceNormalUrlHadErrorJSON = new JSONStorableBool("faceNormalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncFaceNormalHadError));
		this.faceNormalUrlJSON = new JSONStorableUrl("faceNormalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncFaceNormalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.faceNormalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.faceNormalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.faceNormalUrlJSON);
		this.torsoNormalUrlHadErrorJSON = new JSONStorableBool("torsoNormalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncTorsoNormalHadError));
		this.torsoNormalUrlJSON = new JSONStorableUrl("torsoNormalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncTorsoNormalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.torsoNormalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.torsoNormalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.torsoNormalUrlJSON);
		this.limbsNormalUrlHadErrorJSON = new JSONStorableBool("limbsNormalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncLimbsNormalHadError));
		this.limbsNormalUrlJSON = new JSONStorableUrl("limbsNormalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncLimbsNormalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.limbsNormalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.limbsNormalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.limbsNormalUrlJSON);
		this.genitalsNormalUrlHadErrorJSON = new JSONStorableBool("genitalsNormalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncGenitalsNormalHadError));
		this.genitalsNormalUrlJSON = new JSONStorableUrl("genitalsNormalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncGenitalsNormalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.genitalsNormalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.genitalsNormalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.genitalsNormalUrlJSON);
		this.faceDetailUrlHadErrorJSON = new JSONStorableBool("faceDetailUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncFaceDetailHadError));
		this.faceDetailUrlJSON = new JSONStorableUrl("faceDetailUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncFaceDetailUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.faceDetailUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.faceDetailUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.faceDetailUrlJSON);
		this.torsoDetailUrlHadErrorJSON = new JSONStorableBool("torsoDetailUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncTorsoDetailHadError));
		this.torsoDetailUrlJSON = new JSONStorableUrl("torsoDetailUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncTorsoDetailUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.torsoDetailUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.torsoDetailUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.torsoDetailUrlJSON);
		this.limbsDetailUrlHadErrorJSON = new JSONStorableBool("limbsDetailUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncLimbsDetailHadError));
		this.limbsDetailUrlJSON = new JSONStorableUrl("limbsDetailUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncLimbsDetailUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.limbsDetailUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.limbsDetailUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.limbsDetailUrlJSON);
		this.genitalsDetailUrlHadErrorJSON = new JSONStorableBool("genitalsDetailUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncGenitalsDetailHadError));
		this.genitalsDetailUrlJSON = new JSONStorableUrl("genitalsDetailUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncGenitalsDetailUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.genitalsDetailUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.genitalsDetailUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.genitalsDetailUrlJSON);
		this.faceDecalUrlHadErrorJSON = new JSONStorableBool("faceDecalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncFaceDecalHadError));
		this.faceDecalUrlJSON = new JSONStorableUrl("faceDecalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncFaceDecalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.faceDecalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.faceDecalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.faceDecalUrlJSON);
		this.torsoDecalUrlHadErrorJSON = new JSONStorableBool("torsoDecalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncTorsoDecalHadError));
		this.torsoDecalUrlJSON = new JSONStorableUrl("torsoDecalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncTorsoDecalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.torsoDecalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.torsoDecalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.torsoDecalUrlJSON);
		this.limbsDecalUrlHadErrorJSON = new JSONStorableBool("limbsDecalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncLimbsDecalHadError));
		this.limbsDecalUrlJSON = new JSONStorableUrl("limbsDecalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncLimbsDecalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.limbsDecalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.limbsDecalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.limbsDecalUrlJSON);
		this.genitalsDecalUrlHadErrorJSON = new JSONStorableBool("genitalsDecalUrlHadError", false, new JSONStorableBool.SetBoolCallback(this.SyncGenitalsDecalHadError));
		this.genitalsDecalUrlJSON = new JSONStorableUrl("genitalsDecalUrl", string.Empty, new JSONStorableString.SetJSONStringCallback(this.SyncGenitalsDecalUrl), "jpg|jpeg|png|tif|tiff", "Custom/Atom/Person/Textures", true);
		this.genitalsDecalUrlJSON.suggestedPathGroup = "DAZCharacterTexture";
		this.genitalsDecalUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		base.RegisterUrl(this.genitalsDecalUrlJSON);
		this.autoBlendGenitalTexturesJSON = new JSONStorableBool("autoBlendGenitalTextures", false, new JSONStorableBool.SetBoolCallback(this.SyncAutoBlendGenitalTextures));
		base.RegisterBool(this.autoBlendGenitalTexturesJSON);
		this.autoBlendGenitalSpecGlossNormalTexturesJSON = new JSONStorableBool("autoBlendGenitalSpecGlossNormalTextures", true, new JSONStorableBool.SetBoolCallback(this.SyncAutoBlendGenitalTextures));
		base.RegisterBool(this.autoBlendGenitalSpecGlossNormalTexturesJSON);
		this.autoBlendGenitalLightenDarkenJSON = new JSONStorableFloat("autoBlendGenitalLightenDarken", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncAutoBlendGenitalLightenDarken), -1f, 1f, true, true);
		base.RegisterFloat(this.autoBlendGenitalLightenDarkenJSON);
		this.autoBlendGenitalHueOffsetJSON = new JSONStorableFloat("autoBlendGenitalHueOffset", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncAutoBlendGenitalHueOffset), -1f, 1f, true, true);
		base.RegisterFloat(this.autoBlendGenitalHueOffsetJSON);
		this.autoBlendGenitalSaturationOffsetJSON = new JSONStorableFloat("autoBlendGenitalSaturationOffset", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncAutoBlendGenitalSaturationOffset), -1f, 1f, true, true);
		base.RegisterFloat(this.autoBlendGenitalSaturationOffsetJSON);
		this.origDiffuseTextures = new Dictionary<Material, Texture2D>();
		this.origSpecularTextures = new Dictionary<Material, Texture2D>();
		this.origGlossTextures = new Dictionary<Material, Texture2D>();
		this.origNormalTextures = new Dictionary<Material, Texture2D>();
		this.origDetailTextures = new Dictionary<Material, Texture2D>();
		this.origDecalTextures = new Dictionary<Material, Texture2D>();
	}

	// Token: 0x060048C1 RID: 18625 RVA: 0x0016D444 File Offset: 0x0016B844
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			DAZCharacterTextureControlUI componentInChildren = this.UITransform.GetComponentInChildren<DAZCharacterTextureControlUI>(true);
			if (componentInChildren != null)
			{
				this.uvLabel = componentInChildren.uvLabel;
				this.uvLabel.text = this.uvSetName;
				this.faceDiffuseUrlJSON.fileBrowseButton = componentInChildren.faceDiffuseFileBrowseButton;
				this.faceDiffuseUrlJSON.reloadButton = componentInChildren.faceDiffuseReloadButton;
				this.faceDiffuseUrlJSON.clearButton = componentInChildren.faceDiffuseClearButton;
				this.faceDiffuseUrlJSON.text = componentInChildren.faceDiffuseUrlText;
				this.torsoDiffuseUrlJSON.fileBrowseButton = componentInChildren.torsoDiffuseFileBrowseButton;
				this.torsoDiffuseUrlJSON.reloadButton = componentInChildren.torsoDiffuseReloadButton;
				this.torsoDiffuseUrlJSON.clearButton = componentInChildren.torsoDiffuseClearButton;
				this.torsoDiffuseUrlJSON.text = componentInChildren.torsoDiffuseUrlText;
				this.limbsDiffuseUrlJSON.fileBrowseButton = componentInChildren.limbsDiffuseFileBrowseButton;
				this.limbsDiffuseUrlJSON.reloadButton = componentInChildren.limbsDiffuseReloadButton;
				this.limbsDiffuseUrlJSON.clearButton = componentInChildren.limbsDiffuseClearButton;
				this.limbsDiffuseUrlJSON.text = componentInChildren.limbsDiffuseUrlText;
				this.genitalsDiffuseUrlJSON.fileBrowseButton = componentInChildren.genitalsDiffuseFileBrowseButton;
				this.genitalsDiffuseUrlJSON.reloadButton = componentInChildren.genitalsDiffuseReloadButton;
				this.genitalsDiffuseUrlJSON.clearButton = componentInChildren.genitalsDiffuseClearButton;
				this.genitalsDiffuseUrlJSON.text = componentInChildren.genitalsDiffuseUrlText;
				this.faceSpecularUrlJSON.fileBrowseButton = componentInChildren.faceSpecularFileBrowseButton;
				this.faceSpecularUrlJSON.reloadButton = componentInChildren.faceSpecularReloadButton;
				this.faceSpecularUrlJSON.clearButton = componentInChildren.faceSpecularClearButton;
				this.faceSpecularUrlJSON.text = componentInChildren.faceSpecularUrlText;
				this.torsoSpecularUrlJSON.fileBrowseButton = componentInChildren.torsoSpecularFileBrowseButton;
				this.torsoSpecularUrlJSON.reloadButton = componentInChildren.torsoSpecularReloadButton;
				this.torsoSpecularUrlJSON.clearButton = componentInChildren.torsoSpecularClearButton;
				this.torsoSpecularUrlJSON.text = componentInChildren.torsoSpecularUrlText;
				this.limbsSpecularUrlJSON.fileBrowseButton = componentInChildren.limbsSpecularFileBrowseButton;
				this.limbsSpecularUrlJSON.reloadButton = componentInChildren.limbsSpecularReloadButton;
				this.limbsSpecularUrlJSON.clearButton = componentInChildren.limbsSpecularClearButton;
				this.limbsSpecularUrlJSON.text = componentInChildren.limbsSpecularUrlText;
				this.genitalsSpecularUrlJSON.fileBrowseButton = componentInChildren.genitalsSpecularFileBrowseButton;
				this.genitalsSpecularUrlJSON.reloadButton = componentInChildren.genitalsSpecularReloadButton;
				this.genitalsSpecularUrlJSON.clearButton = componentInChildren.genitalsSpecularClearButton;
				this.genitalsSpecularUrlJSON.text = componentInChildren.genitalsSpecularUrlText;
				this.faceGlossUrlJSON.fileBrowseButton = componentInChildren.faceGlossFileBrowseButton;
				this.faceGlossUrlJSON.reloadButton = componentInChildren.faceGlossReloadButton;
				this.faceGlossUrlJSON.clearButton = componentInChildren.faceGlossClearButton;
				this.faceGlossUrlJSON.text = componentInChildren.faceGlossUrlText;
				this.torsoGlossUrlJSON.fileBrowseButton = componentInChildren.torsoGlossFileBrowseButton;
				this.torsoGlossUrlJSON.reloadButton = componentInChildren.torsoGlossReloadButton;
				this.torsoGlossUrlJSON.clearButton = componentInChildren.torsoGlossClearButton;
				this.torsoGlossUrlJSON.text = componentInChildren.torsoGlossUrlText;
				this.limbsGlossUrlJSON.fileBrowseButton = componentInChildren.limbsGlossFileBrowseButton;
				this.limbsGlossUrlJSON.reloadButton = componentInChildren.limbsGlossReloadButton;
				this.limbsGlossUrlJSON.clearButton = componentInChildren.limbsGlossClearButton;
				this.limbsGlossUrlJSON.text = componentInChildren.limbsGlossUrlText;
				this.genitalsGlossUrlJSON.fileBrowseButton = componentInChildren.genitalsGlossFileBrowseButton;
				this.genitalsGlossUrlJSON.reloadButton = componentInChildren.genitalsGlossReloadButton;
				this.genitalsGlossUrlJSON.clearButton = componentInChildren.genitalsGlossClearButton;
				this.genitalsGlossUrlJSON.text = componentInChildren.genitalsGlossUrlText;
				this.faceNormalUrlJSON.fileBrowseButton = componentInChildren.faceNormalFileBrowseButton;
				this.faceNormalUrlJSON.reloadButton = componentInChildren.faceNormalReloadButton;
				this.faceNormalUrlJSON.clearButton = componentInChildren.faceNormalClearButton;
				this.faceNormalUrlJSON.text = componentInChildren.faceNormalUrlText;
				this.torsoNormalUrlJSON.fileBrowseButton = componentInChildren.torsoNormalFileBrowseButton;
				this.torsoNormalUrlJSON.reloadButton = componentInChildren.torsoNormalReloadButton;
				this.torsoNormalUrlJSON.clearButton = componentInChildren.torsoNormalClearButton;
				this.torsoNormalUrlJSON.text = componentInChildren.torsoNormalUrlText;
				this.limbsNormalUrlJSON.fileBrowseButton = componentInChildren.limbsNormalFileBrowseButton;
				this.limbsNormalUrlJSON.reloadButton = componentInChildren.limbsNormalReloadButton;
				this.limbsNormalUrlJSON.clearButton = componentInChildren.limbsNormalClearButton;
				this.limbsNormalUrlJSON.text = componentInChildren.limbsNormalUrlText;
				this.genitalsNormalUrlJSON.fileBrowseButton = componentInChildren.genitalsNormalFileBrowseButton;
				this.genitalsNormalUrlJSON.reloadButton = componentInChildren.genitalsNormalReloadButton;
				this.genitalsNormalUrlJSON.clearButton = componentInChildren.genitalsNormalClearButton;
				this.genitalsNormalUrlJSON.text = componentInChildren.genitalsNormalUrlText;
				this.faceDetailUrlJSON.fileBrowseButton = componentInChildren.faceDetailFileBrowseButton;
				this.faceDetailUrlJSON.reloadButton = componentInChildren.faceDetailReloadButton;
				this.faceDetailUrlJSON.clearButton = componentInChildren.faceDetailClearButton;
				this.faceDetailUrlJSON.text = componentInChildren.faceDetailUrlText;
				this.torsoDetailUrlJSON.fileBrowseButton = componentInChildren.torsoDetailFileBrowseButton;
				this.torsoDetailUrlJSON.reloadButton = componentInChildren.torsoDetailReloadButton;
				this.torsoDetailUrlJSON.clearButton = componentInChildren.torsoDetailClearButton;
				this.torsoDetailUrlJSON.text = componentInChildren.torsoDetailUrlText;
				this.limbsDetailUrlJSON.fileBrowseButton = componentInChildren.limbsDetailFileBrowseButton;
				this.limbsDetailUrlJSON.reloadButton = componentInChildren.limbsDetailReloadButton;
				this.limbsDetailUrlJSON.clearButton = componentInChildren.limbsDetailClearButton;
				this.limbsDetailUrlJSON.text = componentInChildren.limbsDetailUrlText;
				this.genitalsDetailUrlJSON.fileBrowseButton = componentInChildren.genitalsDetailFileBrowseButton;
				this.genitalsDetailUrlJSON.reloadButton = componentInChildren.genitalsDetailReloadButton;
				this.genitalsDetailUrlJSON.clearButton = componentInChildren.genitalsDetailClearButton;
				this.genitalsDetailUrlJSON.text = componentInChildren.genitalsDetailUrlText;
				this.faceDecalUrlJSON.fileBrowseButton = componentInChildren.faceDecalFileBrowseButton;
				this.faceDecalUrlJSON.reloadButton = componentInChildren.faceDecalReloadButton;
				this.faceDecalUrlJSON.clearButton = componentInChildren.faceDecalClearButton;
				this.faceDecalUrlJSON.text = componentInChildren.faceDecalUrlText;
				this.torsoDecalUrlJSON.fileBrowseButton = componentInChildren.torsoDecalFileBrowseButton;
				this.torsoDecalUrlJSON.reloadButton = componentInChildren.torsoDecalReloadButton;
				this.torsoDecalUrlJSON.clearButton = componentInChildren.torsoDecalClearButton;
				this.torsoDecalUrlJSON.text = componentInChildren.torsoDecalUrlText;
				this.limbsDecalUrlJSON.fileBrowseButton = componentInChildren.limbsDecalFileBrowseButton;
				this.limbsDecalUrlJSON.reloadButton = componentInChildren.limbsDecalReloadButton;
				this.limbsDecalUrlJSON.clearButton = componentInChildren.limbsDecalClearButton;
				this.limbsDecalUrlJSON.text = componentInChildren.limbsDecalUrlText;
				this.genitalsDecalUrlJSON.fileBrowseButton = componentInChildren.genitalsDecalFileBrowseButton;
				this.genitalsDecalUrlJSON.reloadButton = componentInChildren.genitalsDecalReloadButton;
				this.genitalsDecalUrlJSON.clearButton = componentInChildren.genitalsDecalClearButton;
				this.genitalsDecalUrlJSON.text = componentInChildren.genitalsDecalUrlText;
				if (componentInChildren.autoBlendGenitalTexturesToggle != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						componentInChildren.autoBlendGenitalTexturesToggle.gameObject.SetActive(false);
					}
					else
					{
						componentInChildren.autoBlendGenitalTexturesToggle.gameObject.SetActive(true);
						this.autoBlendGenitalTexturesJSON.toggle = componentInChildren.autoBlendGenitalTexturesToggle;
					}
				}
				if (componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle.gameObject.SetActive(false);
					}
					else
					{
						componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle.gameObject.SetActive(true);
						this.autoBlendGenitalSpecGlossNormalTexturesJSON.toggle = componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle;
					}
				}
				this.dumpAutoGeneratedGenitalTextureButton = componentInChildren.dumpAutoGeneratedGenitalTexturesButton;
				if (this.dumpAutoGeneratedGenitalTextureButton != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						this.dumpAutoGeneratedGenitalTextureButton.gameObject.SetActive(false);
					}
					else
					{
						this.dumpAutoGeneratedGenitalTextureButton.gameObject.SetActive(true);
					}
					this.dumpAutoGeneratedGenitalTextureButton.onClick.AddListener(new UnityAction(this.DumpGeneratedGenitalTextures));
				}
				if (componentInChildren.autoBlendGenitalColorAdjustContainer != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						componentInChildren.autoBlendGenitalColorAdjustContainer.gameObject.SetActive(false);
					}
					else
					{
						componentInChildren.autoBlendGenitalColorAdjustContainer.gameObject.SetActive(true);
						this.autoBlendGenitalLightenDarkenJSON.slider = componentInChildren.autoBlendGenitalLightenDarkenSlider;
						this.autoBlendGenitalHueOffsetJSON.slider = componentInChildren.autoBlendGenitalHueOffsetSlider;
						this.autoBlendGenitalSaturationOffsetJSON.slider = componentInChildren.autoBlendGenitalSaturationOffsetSlider;
					}
				}
				this.autoBlendGenitalDiffuseTextureButton = componentInChildren.autoBlendGenitalDiffuseTextureButton;
				if (this.autoBlendGenitalDiffuseTextureButton != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						this.autoBlendGenitalDiffuseTextureButton.gameObject.SetActive(false);
					}
					else
					{
						this.autoBlendGenitalDiffuseTextureButton.gameObject.SetActive(true);
					}
					this.autoBlendGenitalDiffuseTextureButton.onClick.AddListener(new UnityAction(this.SyncDiffuseGenitalTexture));
				}
				this.directoryBrowseButton = componentInChildren.directoryBrowseButton;
				if (this.directoryBrowseButton != null)
				{
					this.directoryBrowseButton.onClick.AddListener(new UnityAction(this.DirectoryBrowse));
				}
				this.SyncFaceDiffuseHadError(this.faceDiffuseUrlHadErrorJSON.val);
				this.SyncTorsoDiffuseHadError(this.torsoDiffuseUrlHadErrorJSON.val);
				this.SyncLimbsDiffuseHadError(this.limbsDiffuseUrlHadErrorJSON.val);
				this.SyncGenitalsDiffuseHadError(this.genitalsDiffuseUrlHadErrorJSON.val);
				this.SyncFaceSpecularHadError(this.faceSpecularUrlHadErrorJSON.val);
				this.SyncTorsoSpecularHadError(this.torsoSpecularUrlHadErrorJSON.val);
				this.SyncLimbsSpecularHadError(this.limbsSpecularUrlHadErrorJSON.val);
				this.SyncGenitalsSpecularHadError(this.genitalsSpecularUrlHadErrorJSON.val);
				this.SyncFaceGlossHadError(this.faceGlossUrlHadErrorJSON.val);
				this.SyncTorsoGlossHadError(this.torsoGlossUrlHadErrorJSON.val);
				this.SyncLimbsGlossHadError(this.limbsGlossUrlHadErrorJSON.val);
				this.SyncGenitalsGlossHadError(this.genitalsGlossUrlHadErrorJSON.val);
				this.SyncFaceNormalHadError(this.faceNormalUrlHadErrorJSON.val);
				this.SyncTorsoNormalHadError(this.torsoNormalUrlHadErrorJSON.val);
				this.SyncLimbsNormalHadError(this.limbsNormalUrlHadErrorJSON.val);
				this.SyncGenitalsNormalHadError(this.genitalsNormalUrlHadErrorJSON.val);
				this.SyncFaceDetailHadError(this.faceDetailUrlHadErrorJSON.val);
				this.SyncTorsoDetailHadError(this.torsoDetailUrlHadErrorJSON.val);
				this.SyncLimbsDetailHadError(this.limbsDetailUrlHadErrorJSON.val);
				this.SyncGenitalsDetailHadError(this.genitalsDetailUrlHadErrorJSON.val);
				this.SyncFaceDecalHadError(this.faceDecalUrlHadErrorJSON.val);
				this.SyncTorsoDecalHadError(this.torsoDecalUrlHadErrorJSON.val);
				this.SyncLimbsDecalHadError(this.limbsDecalUrlHadErrorJSON.val);
				this.SyncGenitalsDecalHadError(this.genitalsDecalUrlHadErrorJSON.val);
			}
		}
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x0016DEDC File Offset: 0x0016C2DC
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			DAZCharacterTextureControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<DAZCharacterTextureControlUI>(true);
			if (componentInChildren != null)
			{
				this.uvLabelAlt = componentInChildren.uvLabel;
				this.uvLabelAlt.text = this.uvSetName;
				this.faceDiffuseUrlJSON.fileBrowseButtonAlt = componentInChildren.faceDiffuseFileBrowseButton;
				this.faceDiffuseUrlJSON.reloadButtonAlt = componentInChildren.faceDiffuseReloadButton;
				this.faceDiffuseUrlJSON.clearButtonAlt = componentInChildren.faceDiffuseClearButton;
				this.faceDiffuseUrlJSON.textAlt = componentInChildren.faceDiffuseUrlText;
				this.torsoDiffuseUrlJSON.fileBrowseButtonAlt = componentInChildren.torsoDiffuseFileBrowseButton;
				this.torsoDiffuseUrlJSON.reloadButtonAlt = componentInChildren.torsoDiffuseReloadButton;
				this.torsoDiffuseUrlJSON.clearButtonAlt = componentInChildren.torsoDiffuseClearButton;
				this.torsoDiffuseUrlJSON.textAlt = componentInChildren.torsoDiffuseUrlText;
				this.limbsDiffuseUrlJSON.fileBrowseButtonAlt = componentInChildren.limbsDiffuseFileBrowseButton;
				this.limbsDiffuseUrlJSON.reloadButtonAlt = componentInChildren.limbsDiffuseReloadButton;
				this.limbsDiffuseUrlJSON.clearButtonAlt = componentInChildren.limbsDiffuseClearButton;
				this.limbsDiffuseUrlJSON.textAlt = componentInChildren.limbsDiffuseUrlText;
				this.genitalsDiffuseUrlJSON.fileBrowseButtonAlt = componentInChildren.genitalsDiffuseFileBrowseButton;
				this.genitalsDiffuseUrlJSON.reloadButtonAlt = componentInChildren.genitalsDiffuseReloadButton;
				this.genitalsDiffuseUrlJSON.clearButtonAlt = componentInChildren.genitalsDiffuseClearButton;
				this.genitalsDiffuseUrlJSON.textAlt = componentInChildren.genitalsDiffuseUrlText;
				this.faceSpecularUrlJSON.fileBrowseButtonAlt = componentInChildren.faceSpecularFileBrowseButton;
				this.faceSpecularUrlJSON.reloadButtonAlt = componentInChildren.faceSpecularReloadButton;
				this.faceSpecularUrlJSON.clearButtonAlt = componentInChildren.faceSpecularClearButton;
				this.faceSpecularUrlJSON.textAlt = componentInChildren.faceSpecularUrlText;
				this.torsoSpecularUrlJSON.fileBrowseButtonAlt = componentInChildren.torsoSpecularFileBrowseButton;
				this.torsoSpecularUrlJSON.reloadButtonAlt = componentInChildren.torsoSpecularReloadButton;
				this.torsoSpecularUrlJSON.clearButtonAlt = componentInChildren.torsoSpecularClearButton;
				this.torsoSpecularUrlJSON.textAlt = componentInChildren.torsoSpecularUrlText;
				this.limbsSpecularUrlJSON.fileBrowseButtonAlt = componentInChildren.limbsSpecularFileBrowseButton;
				this.limbsSpecularUrlJSON.reloadButtonAlt = componentInChildren.limbsSpecularReloadButton;
				this.limbsSpecularUrlJSON.clearButtonAlt = componentInChildren.limbsSpecularClearButton;
				this.limbsSpecularUrlJSON.textAlt = componentInChildren.limbsSpecularUrlText;
				this.genitalsSpecularUrlJSON.fileBrowseButtonAlt = componentInChildren.genitalsSpecularFileBrowseButton;
				this.genitalsSpecularUrlJSON.reloadButtonAlt = componentInChildren.genitalsSpecularReloadButton;
				this.genitalsSpecularUrlJSON.clearButtonAlt = componentInChildren.genitalsSpecularClearButton;
				this.genitalsSpecularUrlJSON.textAlt = componentInChildren.genitalsSpecularUrlText;
				this.faceGlossUrlJSON.fileBrowseButtonAlt = componentInChildren.faceGlossFileBrowseButton;
				this.faceGlossUrlJSON.reloadButtonAlt = componentInChildren.faceGlossReloadButton;
				this.faceGlossUrlJSON.clearButtonAlt = componentInChildren.faceGlossClearButton;
				this.faceGlossUrlJSON.textAlt = componentInChildren.faceGlossUrlText;
				this.torsoGlossUrlJSON.fileBrowseButtonAlt = componentInChildren.torsoGlossFileBrowseButton;
				this.torsoGlossUrlJSON.reloadButtonAlt = componentInChildren.torsoGlossReloadButton;
				this.torsoGlossUrlJSON.clearButtonAlt = componentInChildren.torsoGlossClearButton;
				this.torsoGlossUrlJSON.textAlt = componentInChildren.torsoGlossUrlText;
				this.limbsGlossUrlJSON.fileBrowseButtonAlt = componentInChildren.limbsGlossFileBrowseButton;
				this.limbsGlossUrlJSON.reloadButtonAlt = componentInChildren.limbsGlossReloadButton;
				this.limbsGlossUrlJSON.clearButtonAlt = componentInChildren.limbsGlossClearButton;
				this.limbsGlossUrlJSON.textAlt = componentInChildren.limbsGlossUrlText;
				this.genitalsGlossUrlJSON.fileBrowseButtonAlt = componentInChildren.genitalsGlossFileBrowseButton;
				this.genitalsGlossUrlJSON.reloadButtonAlt = componentInChildren.genitalsGlossReloadButton;
				this.genitalsGlossUrlJSON.clearButtonAlt = componentInChildren.genitalsGlossClearButton;
				this.genitalsGlossUrlJSON.textAlt = componentInChildren.genitalsGlossUrlText;
				this.faceNormalUrlJSON.fileBrowseButtonAlt = componentInChildren.faceNormalFileBrowseButton;
				this.faceNormalUrlJSON.reloadButtonAlt = componentInChildren.faceNormalReloadButton;
				this.faceNormalUrlJSON.clearButtonAlt = componentInChildren.faceNormalClearButton;
				this.faceNormalUrlJSON.textAlt = componentInChildren.faceNormalUrlText;
				this.torsoNormalUrlJSON.fileBrowseButtonAlt = componentInChildren.torsoNormalFileBrowseButton;
				this.torsoNormalUrlJSON.reloadButtonAlt = componentInChildren.torsoNormalReloadButton;
				this.torsoNormalUrlJSON.clearButtonAlt = componentInChildren.torsoNormalClearButton;
				this.torsoNormalUrlJSON.textAlt = componentInChildren.torsoNormalUrlText;
				this.limbsNormalUrlJSON.fileBrowseButtonAlt = componentInChildren.limbsNormalFileBrowseButton;
				this.limbsNormalUrlJSON.reloadButtonAlt = componentInChildren.limbsNormalReloadButton;
				this.limbsNormalUrlJSON.clearButtonAlt = componentInChildren.limbsNormalClearButton;
				this.limbsNormalUrlJSON.textAlt = componentInChildren.limbsNormalUrlText;
				this.genitalsNormalUrlJSON.fileBrowseButtonAlt = componentInChildren.genitalsNormalFileBrowseButton;
				this.genitalsNormalUrlJSON.reloadButtonAlt = componentInChildren.genitalsNormalReloadButton;
				this.genitalsNormalUrlJSON.clearButtonAlt = componentInChildren.genitalsNormalClearButton;
				this.genitalsNormalUrlJSON.textAlt = componentInChildren.genitalsNormalUrlText;
				this.faceDetailUrlJSON.fileBrowseButtonAlt = componentInChildren.faceDetailFileBrowseButton;
				this.faceDetailUrlJSON.reloadButtonAlt = componentInChildren.faceDetailReloadButton;
				this.faceDetailUrlJSON.clearButtonAlt = componentInChildren.faceDetailClearButton;
				this.faceDetailUrlJSON.textAlt = componentInChildren.faceDetailUrlText;
				this.torsoDetailUrlJSON.fileBrowseButtonAlt = componentInChildren.torsoDetailFileBrowseButton;
				this.torsoDetailUrlJSON.reloadButtonAlt = componentInChildren.torsoDetailReloadButton;
				this.torsoDetailUrlJSON.clearButtonAlt = componentInChildren.torsoDetailClearButton;
				this.torsoDetailUrlJSON.textAlt = componentInChildren.torsoDetailUrlText;
				this.limbsDetailUrlJSON.fileBrowseButtonAlt = componentInChildren.limbsDetailFileBrowseButton;
				this.limbsDetailUrlJSON.reloadButtonAlt = componentInChildren.limbsDetailReloadButton;
				this.limbsDetailUrlJSON.clearButtonAlt = componentInChildren.limbsDetailClearButton;
				this.limbsDetailUrlJSON.textAlt = componentInChildren.limbsDetailUrlText;
				this.genitalsDetailUrlJSON.fileBrowseButtonAlt = componentInChildren.genitalsDetailFileBrowseButton;
				this.genitalsDetailUrlJSON.reloadButtonAlt = componentInChildren.genitalsDetailReloadButton;
				this.genitalsDetailUrlJSON.clearButtonAlt = componentInChildren.genitalsDetailClearButton;
				this.genitalsDetailUrlJSON.textAlt = componentInChildren.genitalsDetailUrlText;
				this.faceDecalUrlJSON.fileBrowseButtonAlt = componentInChildren.faceDecalFileBrowseButton;
				this.faceDecalUrlJSON.reloadButtonAlt = componentInChildren.faceDecalReloadButton;
				this.faceDecalUrlJSON.clearButtonAlt = componentInChildren.faceDecalClearButton;
				this.faceDecalUrlJSON.textAlt = componentInChildren.faceDecalUrlText;
				this.torsoDecalUrlJSON.fileBrowseButtonAlt = componentInChildren.torsoDecalFileBrowseButton;
				this.torsoDecalUrlJSON.reloadButtonAlt = componentInChildren.torsoDecalReloadButton;
				this.torsoDecalUrlJSON.clearButtonAlt = componentInChildren.torsoDecalClearButton;
				this.torsoDecalUrlJSON.textAlt = componentInChildren.torsoDecalUrlText;
				this.limbsDecalUrlJSON.fileBrowseButtonAlt = componentInChildren.limbsDecalFileBrowseButton;
				this.limbsDecalUrlJSON.reloadButtonAlt = componentInChildren.limbsDecalReloadButton;
				this.limbsDecalUrlJSON.clearButtonAlt = componentInChildren.limbsDecalClearButton;
				this.limbsDecalUrlJSON.textAlt = componentInChildren.limbsDecalUrlText;
				this.genitalsDecalUrlJSON.fileBrowseButtonAlt = componentInChildren.genitalsDecalFileBrowseButton;
				this.genitalsDecalUrlJSON.reloadButtonAlt = componentInChildren.genitalsDecalReloadButton;
				this.genitalsDecalUrlJSON.clearButtonAlt = componentInChildren.genitalsDecalClearButton;
				this.genitalsDecalUrlJSON.textAlt = componentInChildren.genitalsDecalUrlText;
				if (componentInChildren.autoBlendGenitalTexturesToggle != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						componentInChildren.autoBlendGenitalTexturesToggle.gameObject.SetActive(false);
					}
					else
					{
						componentInChildren.autoBlendGenitalTexturesToggle.gameObject.SetActive(true);
						this.autoBlendGenitalTexturesJSON.toggleAlt = componentInChildren.autoBlendGenitalTexturesToggle;
					}
				}
				if (componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle.gameObject.SetActive(false);
					}
					else
					{
						componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle.gameObject.SetActive(true);
						this.autoBlendGenitalSpecGlossNormalTexturesJSON.toggleAlt = componentInChildren.autoBlendGenitalSpecGlossNormalTexturesToggle;
					}
				}
				if (componentInChildren.autoBlendGenitalColorAdjustContainer != null)
				{
					if (this.genitalBlendMaskTexture == null)
					{
						componentInChildren.autoBlendGenitalColorAdjustContainer.gameObject.SetActive(false);
					}
					else
					{
						componentInChildren.autoBlendGenitalColorAdjustContainer.gameObject.SetActive(true);
						this.autoBlendGenitalLightenDarkenJSON.sliderAlt = componentInChildren.autoBlendGenitalLightenDarkenSlider;
						this.autoBlendGenitalHueOffsetJSON.sliderAlt = componentInChildren.autoBlendGenitalHueOffsetSlider;
						this.autoBlendGenitalSaturationOffsetJSON.sliderAlt = componentInChildren.autoBlendGenitalSaturationOffsetSlider;
					}
				}
				this.directoryBrowseButtonAlt = componentInChildren.directoryBrowseButton;
				if (this.directoryBrowseButtonAlt != null)
				{
					this.directoryBrowseButtonAlt.onClick.AddListener(new UnityAction(this.DirectoryBrowse));
				}
				this.SyncFaceDiffuseHadError(this.faceDiffuseUrlHadErrorJSON.val);
				this.SyncTorsoDiffuseHadError(this.torsoDiffuseUrlHadErrorJSON.val);
				this.SyncLimbsDiffuseHadError(this.limbsDiffuseUrlHadErrorJSON.val);
				this.SyncGenitalsDiffuseHadError(this.genitalsDiffuseUrlHadErrorJSON.val);
				this.SyncFaceSpecularHadError(this.faceSpecularUrlHadErrorJSON.val);
				this.SyncTorsoSpecularHadError(this.torsoSpecularUrlHadErrorJSON.val);
				this.SyncLimbsSpecularHadError(this.limbsSpecularUrlHadErrorJSON.val);
				this.SyncGenitalsSpecularHadError(this.genitalsSpecularUrlHadErrorJSON.val);
				this.SyncFaceGlossHadError(this.faceGlossUrlHadErrorJSON.val);
				this.SyncTorsoGlossHadError(this.torsoGlossUrlHadErrorJSON.val);
				this.SyncLimbsGlossHadError(this.limbsGlossUrlHadErrorJSON.val);
				this.SyncGenitalsGlossHadError(this.genitalsGlossUrlHadErrorJSON.val);
				this.SyncFaceNormalHadError(this.faceNormalUrlHadErrorJSON.val);
				this.SyncTorsoNormalHadError(this.torsoNormalUrlHadErrorJSON.val);
				this.SyncLimbsNormalHadError(this.limbsNormalUrlHadErrorJSON.val);
				this.SyncGenitalsNormalHadError(this.genitalsNormalUrlHadErrorJSON.val);
				this.SyncFaceDetailHadError(this.faceDetailUrlHadErrorJSON.val);
				this.SyncTorsoDetailHadError(this.torsoDetailUrlHadErrorJSON.val);
				this.SyncLimbsDetailHadError(this.limbsDetailUrlHadErrorJSON.val);
				this.SyncGenitalsDetailHadError(this.genitalsDetailUrlHadErrorJSON.val);
				this.SyncFaceDecalHadError(this.faceDecalUrlHadErrorJSON.val);
				this.SyncTorsoDecalHadError(this.torsoDecalUrlHadErrorJSON.val);
				this.SyncLimbsDecalHadError(this.limbsDecalUrlHadErrorJSON.val);
				this.SyncGenitalsDecalHadError(this.genitalsDecalUrlHadErrorJSON.val);
			}
		}
	}

	// Token: 0x060048C3 RID: 18627 RVA: 0x0016E894 File Offset: 0x0016CC94
	public void DeregisterUI()
	{
		this.UITransform = null;
		this.uvLabel = null;
		this.faceDiffuseUrlJSON.fileBrowseButton = null;
		this.faceDiffuseUrlJSON.clearButton = null;
		this.faceDiffuseUrlJSON.text = null;
		this.torsoDiffuseUrlJSON.fileBrowseButton = null;
		this.torsoDiffuseUrlJSON.clearButton = null;
		this.torsoDiffuseUrlJSON.text = null;
		this.limbsDiffuseUrlJSON.fileBrowseButton = null;
		this.limbsDiffuseUrlJSON.clearButton = null;
		this.limbsDiffuseUrlJSON.text = null;
		this.genitalsDiffuseUrlJSON.fileBrowseButton = null;
		this.genitalsDiffuseUrlJSON.clearButton = null;
		this.genitalsDiffuseUrlJSON.text = null;
		this.faceSpecularUrlJSON.fileBrowseButton = null;
		this.faceSpecularUrlJSON.clearButton = null;
		this.faceSpecularUrlJSON.text = null;
		this.torsoSpecularUrlJSON.fileBrowseButton = null;
		this.torsoSpecularUrlJSON.clearButton = null;
		this.torsoSpecularUrlJSON.text = null;
		this.limbsSpecularUrlJSON.fileBrowseButton = null;
		this.limbsSpecularUrlJSON.clearButton = null;
		this.limbsSpecularUrlJSON.text = null;
		this.genitalsSpecularUrlJSON.fileBrowseButton = null;
		this.genitalsSpecularUrlJSON.clearButton = null;
		this.genitalsSpecularUrlJSON.text = null;
		this.faceGlossUrlJSON.fileBrowseButton = null;
		this.faceGlossUrlJSON.clearButton = null;
		this.faceGlossUrlJSON.text = null;
		this.torsoGlossUrlJSON.fileBrowseButton = null;
		this.torsoGlossUrlJSON.clearButton = null;
		this.torsoGlossUrlJSON.text = null;
		this.limbsGlossUrlJSON.fileBrowseButton = null;
		this.limbsGlossUrlJSON.clearButton = null;
		this.limbsGlossUrlJSON.text = null;
		this.genitalsGlossUrlJSON.fileBrowseButton = null;
		this.genitalsGlossUrlJSON.clearButton = null;
		this.genitalsGlossUrlJSON.text = null;
		this.faceNormalUrlJSON.fileBrowseButton = null;
		this.faceNormalUrlJSON.clearButton = null;
		this.faceNormalUrlJSON.text = null;
		this.torsoNormalUrlJSON.fileBrowseButton = null;
		this.torsoNormalUrlJSON.clearButton = null;
		this.torsoNormalUrlJSON.text = null;
		this.limbsNormalUrlJSON.fileBrowseButton = null;
		this.limbsNormalUrlJSON.clearButton = null;
		this.limbsNormalUrlJSON.text = null;
		this.genitalsNormalUrlJSON.fileBrowseButton = null;
		this.genitalsNormalUrlJSON.clearButton = null;
		this.genitalsNormalUrlJSON.text = null;
		this.faceDetailUrlJSON.fileBrowseButton = null;
		this.faceDetailUrlJSON.clearButton = null;
		this.faceDetailUrlJSON.text = null;
		this.torsoDetailUrlJSON.fileBrowseButton = null;
		this.torsoDetailUrlJSON.clearButton = null;
		this.torsoDetailUrlJSON.text = null;
		this.limbsDetailUrlJSON.fileBrowseButton = null;
		this.limbsDetailUrlJSON.clearButton = null;
		this.limbsDetailUrlJSON.text = null;
		this.genitalsDetailUrlJSON.fileBrowseButton = null;
		this.genitalsDetailUrlJSON.clearButton = null;
		this.genitalsDetailUrlJSON.text = null;
		this.faceDecalUrlJSON.fileBrowseButton = null;
		this.faceDecalUrlJSON.clearButton = null;
		this.faceDecalUrlJSON.text = null;
		this.torsoDecalUrlJSON.fileBrowseButton = null;
		this.torsoDecalUrlJSON.clearButton = null;
		this.torsoDecalUrlJSON.text = null;
		this.limbsDecalUrlJSON.fileBrowseButton = null;
		this.limbsDecalUrlJSON.clearButton = null;
		this.limbsDecalUrlJSON.text = null;
		this.genitalsDecalUrlJSON.fileBrowseButton = null;
		this.genitalsDecalUrlJSON.clearButton = null;
		this.genitalsDecalUrlJSON.text = null;
		this.autoBlendGenitalTexturesJSON.toggle = null;
		this.autoBlendGenitalSpecGlossNormalTexturesJSON.toggle = null;
		if (this.dumpAutoGeneratedGenitalTextureButton != null)
		{
			this.dumpAutoGeneratedGenitalTextureButton.onClick.RemoveListener(new UnityAction(this.DumpGeneratedGenitalTextures));
			this.dumpAutoGeneratedGenitalTextureButton = null;
		}
		this.autoBlendGenitalLightenDarkenJSON.slider = null;
		this.autoBlendGenitalHueOffsetJSON.slider = null;
		this.autoBlendGenitalSaturationOffsetJSON.slider = null;
		if (this.autoBlendGenitalDiffuseTextureButton != null)
		{
			this.autoBlendGenitalDiffuseTextureButton.onClick.RemoveListener(new UnityAction(this.SyncDiffuseGenitalTexture));
			this.autoBlendGenitalDiffuseTextureButton = null;
		}
		if (this.directoryBrowseButton != null)
		{
			this.directoryBrowseButton.onClick.RemoveListener(new UnityAction(this.DirectoryBrowse));
		}
	}

	// Token: 0x060048C4 RID: 18628 RVA: 0x0016ECE0 File Offset: 0x0016D0E0
	public void DeregisterUIAlt()
	{
		this.UITransformAlt = null;
		this.uvLabelAlt = null;
		this.faceDiffuseUrlJSON.fileBrowseButtonAlt = null;
		this.faceDiffuseUrlJSON.clearButtonAlt = null;
		this.faceDiffuseUrlJSON.textAlt = null;
		this.torsoDiffuseUrlJSON.fileBrowseButtonAlt = null;
		this.torsoDiffuseUrlJSON.clearButtonAlt = null;
		this.torsoDiffuseUrlJSON.textAlt = null;
		this.limbsDiffuseUrlJSON.fileBrowseButtonAlt = null;
		this.limbsDiffuseUrlJSON.clearButtonAlt = null;
		this.limbsDiffuseUrlJSON.textAlt = null;
		this.genitalsDiffuseUrlJSON.fileBrowseButtonAlt = null;
		this.genitalsDiffuseUrlJSON.clearButtonAlt = null;
		this.genitalsDiffuseUrlJSON.textAlt = null;
		this.faceSpecularUrlJSON.fileBrowseButtonAlt = null;
		this.faceSpecularUrlJSON.clearButtonAlt = null;
		this.faceSpecularUrlJSON.textAlt = null;
		this.torsoSpecularUrlJSON.fileBrowseButtonAlt = null;
		this.torsoSpecularUrlJSON.clearButtonAlt = null;
		this.torsoSpecularUrlJSON.textAlt = null;
		this.limbsSpecularUrlJSON.fileBrowseButtonAlt = null;
		this.limbsSpecularUrlJSON.clearButtonAlt = null;
		this.limbsSpecularUrlJSON.textAlt = null;
		this.genitalsSpecularUrlJSON.fileBrowseButtonAlt = null;
		this.genitalsSpecularUrlJSON.clearButtonAlt = null;
		this.genitalsSpecularUrlJSON.textAlt = null;
		this.faceGlossUrlJSON.fileBrowseButtonAlt = null;
		this.faceGlossUrlJSON.clearButtonAlt = null;
		this.faceGlossUrlJSON.textAlt = null;
		this.torsoGlossUrlJSON.fileBrowseButtonAlt = null;
		this.torsoGlossUrlJSON.clearButtonAlt = null;
		this.torsoGlossUrlJSON.textAlt = null;
		this.limbsGlossUrlJSON.fileBrowseButtonAlt = null;
		this.limbsGlossUrlJSON.clearButtonAlt = null;
		this.limbsGlossUrlJSON.textAlt = null;
		this.genitalsGlossUrlJSON.fileBrowseButtonAlt = null;
		this.genitalsGlossUrlJSON.clearButtonAlt = null;
		this.genitalsGlossUrlJSON.textAlt = null;
		this.faceNormalUrlJSON.fileBrowseButtonAlt = null;
		this.faceNormalUrlJSON.clearButtonAlt = null;
		this.faceNormalUrlJSON.textAlt = null;
		this.torsoNormalUrlJSON.fileBrowseButtonAlt = null;
		this.torsoNormalUrlJSON.clearButtonAlt = null;
		this.torsoNormalUrlJSON.textAlt = null;
		this.limbsNormalUrlJSON.fileBrowseButtonAlt = null;
		this.limbsNormalUrlJSON.clearButtonAlt = null;
		this.limbsNormalUrlJSON.textAlt = null;
		this.genitalsNormalUrlJSON.fileBrowseButtonAlt = null;
		this.genitalsNormalUrlJSON.clearButtonAlt = null;
		this.genitalsNormalUrlJSON.textAlt = null;
		this.faceDetailUrlJSON.fileBrowseButtonAlt = null;
		this.faceDetailUrlJSON.clearButtonAlt = null;
		this.faceDetailUrlJSON.textAlt = null;
		this.torsoDetailUrlJSON.fileBrowseButtonAlt = null;
		this.torsoDetailUrlJSON.clearButtonAlt = null;
		this.torsoDetailUrlJSON.textAlt = null;
		this.limbsDetailUrlJSON.fileBrowseButtonAlt = null;
		this.limbsDetailUrlJSON.clearButtonAlt = null;
		this.limbsDetailUrlJSON.textAlt = null;
		this.genitalsDetailUrlJSON.fileBrowseButtonAlt = null;
		this.genitalsDetailUrlJSON.clearButtonAlt = null;
		this.genitalsDetailUrlJSON.textAlt = null;
		this.faceDecalUrlJSON.fileBrowseButtonAlt = null;
		this.faceDecalUrlJSON.clearButtonAlt = null;
		this.faceDecalUrlJSON.textAlt = null;
		this.torsoDecalUrlJSON.fileBrowseButtonAlt = null;
		this.torsoDecalUrlJSON.clearButtonAlt = null;
		this.torsoDecalUrlJSON.textAlt = null;
		this.limbsDecalUrlJSON.fileBrowseButtonAlt = null;
		this.limbsDecalUrlJSON.clearButtonAlt = null;
		this.limbsDecalUrlJSON.textAlt = null;
		this.genitalsDecalUrlJSON.fileBrowseButtonAlt = null;
		this.genitalsDecalUrlJSON.clearButtonAlt = null;
		this.genitalsDecalUrlJSON.textAlt = null;
		this.autoBlendGenitalTexturesJSON.toggleAlt = null;
		this.autoBlendGenitalSpecGlossNormalTexturesJSON.toggleAlt = null;
		this.autoBlendGenitalLightenDarkenJSON.sliderAlt = null;
		this.autoBlendGenitalHueOffsetJSON.sliderAlt = null;
		this.autoBlendGenitalSaturationOffsetJSON.sliderAlt = null;
		if (this.directoryBrowseButtonAlt != null)
		{
			this.directoryBrowseButtonAlt.onClick.RemoveListener(new UnityAction(this.DirectoryBrowse));
		}
	}

	// Token: 0x060048C5 RID: 18629 RVA: 0x0016F0C4 File Offset: 0x0016D4C4
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

	// Token: 0x060048C6 RID: 18630 RVA: 0x0016F0EC File Offset: 0x0016D4EC
	protected void OnDestroy()
	{
		this.DeregisterUI();
		this.DeregisterUIAlt();
		this.DeregisterAllTextures();
		if (this.lastGeneratedDiffuseGenTexture)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedDiffuseGenTexture);
		}
		if (this.lastGeneratedSpecularGenTexture)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedSpecularGenTexture);
		}
		if (this.lastGeneratedGlossGenTexture)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedGlossGenTexture);
		}
		if (this.lastGeneratedNormalGenTexture)
		{
			UnityEngine.Object.Destroy(this.lastGeneratedNormalGenTexture);
		}
	}

	// Token: 0x0400369B RID: 13979
	public int[] faceMaterialNums;

	// Token: 0x0400369C RID: 13980
	public int[] torsoMaterialNums;

	// Token: 0x0400369D RID: 13981
	public int[] limbMaterialNums;

	// Token: 0x0400369E RID: 13982
	public int[] genitalMaterialNums;

	// Token: 0x0400369F RID: 13983
	public Text uvLabel;

	// Token: 0x040036A0 RID: 13984
	public Text uvLabelAlt;

	// Token: 0x040036A1 RID: 13985
	public string uvSetName = "Base";

	// Token: 0x040036A2 RID: 13986
	public MeshRenderer debugRenderer;

	// Token: 0x040036A3 RID: 13987
	protected Dictionary<Material, Texture2D> origDiffuseTextures;

	// Token: 0x040036A4 RID: 13988
	protected Dictionary<Material, Texture2D> origSpecularTextures;

	// Token: 0x040036A5 RID: 13989
	protected Dictionary<Material, Texture2D> origGlossTextures;

	// Token: 0x040036A6 RID: 13990
	protected Dictionary<Material, Texture2D> origNormalTextures;

	// Token: 0x040036A7 RID: 13991
	protected Dictionary<Material, Texture2D> origDetailTextures;

	// Token: 0x040036A8 RID: 13992
	protected Dictionary<Material, Texture2D> origDecalTextures;

	// Token: 0x040036A9 RID: 13993
	protected Dictionary<Texture2D, int> textureUseCount;

	// Token: 0x040036AA RID: 13994
	protected JSONStorableBool faceDiffuseUrlHadErrorJSON;

	// Token: 0x040036AB RID: 13995
	protected JSONStorableUrl faceDiffuseUrlJSON;

	// Token: 0x040036AC RID: 13996
	protected JSONStorableBool torsoDiffuseUrlHadErrorJSON;

	// Token: 0x040036AD RID: 13997
	protected JSONStorableUrl torsoDiffuseUrlJSON;

	// Token: 0x040036AE RID: 13998
	protected JSONStorableBool limbsDiffuseUrlHadErrorJSON;

	// Token: 0x040036AF RID: 13999
	protected JSONStorableUrl limbsDiffuseUrlJSON;

	// Token: 0x040036B0 RID: 14000
	protected JSONStorableBool genitalsDiffuseUrlHadErrorJSON;

	// Token: 0x040036B1 RID: 14001
	protected JSONStorableUrl genitalsDiffuseUrlJSON;

	// Token: 0x040036B2 RID: 14002
	protected JSONStorableBool faceSpecularUrlHadErrorJSON;

	// Token: 0x040036B3 RID: 14003
	protected JSONStorableUrl faceSpecularUrlJSON;

	// Token: 0x040036B4 RID: 14004
	protected JSONStorableBool torsoSpecularUrlHadErrorJSON;

	// Token: 0x040036B5 RID: 14005
	protected JSONStorableUrl torsoSpecularUrlJSON;

	// Token: 0x040036B6 RID: 14006
	protected JSONStorableBool limbsSpecularUrlHadErrorJSON;

	// Token: 0x040036B7 RID: 14007
	protected JSONStorableUrl limbsSpecularUrlJSON;

	// Token: 0x040036B8 RID: 14008
	protected JSONStorableBool genitalsSpecularUrlHadErrorJSON;

	// Token: 0x040036B9 RID: 14009
	protected JSONStorableUrl genitalsSpecularUrlJSON;

	// Token: 0x040036BA RID: 14010
	protected JSONStorableBool faceGlossUrlHadErrorJSON;

	// Token: 0x040036BB RID: 14011
	protected JSONStorableUrl faceGlossUrlJSON;

	// Token: 0x040036BC RID: 14012
	protected JSONStorableBool torsoGlossUrlHadErrorJSON;

	// Token: 0x040036BD RID: 14013
	protected JSONStorableUrl torsoGlossUrlJSON;

	// Token: 0x040036BE RID: 14014
	protected JSONStorableBool limbsGlossUrlHadErrorJSON;

	// Token: 0x040036BF RID: 14015
	protected JSONStorableUrl limbsGlossUrlJSON;

	// Token: 0x040036C0 RID: 14016
	protected JSONStorableBool genitalsGlossUrlHadErrorJSON;

	// Token: 0x040036C1 RID: 14017
	protected JSONStorableUrl genitalsGlossUrlJSON;

	// Token: 0x040036C2 RID: 14018
	protected JSONStorableBool faceNormalUrlHadErrorJSON;

	// Token: 0x040036C3 RID: 14019
	protected JSONStorableUrl faceNormalUrlJSON;

	// Token: 0x040036C4 RID: 14020
	protected JSONStorableBool torsoNormalUrlHadErrorJSON;

	// Token: 0x040036C5 RID: 14021
	protected JSONStorableUrl torsoNormalUrlJSON;

	// Token: 0x040036C6 RID: 14022
	protected JSONStorableBool limbsNormalUrlHadErrorJSON;

	// Token: 0x040036C7 RID: 14023
	protected JSONStorableUrl limbsNormalUrlJSON;

	// Token: 0x040036C8 RID: 14024
	protected JSONStorableBool genitalsNormalUrlHadErrorJSON;

	// Token: 0x040036C9 RID: 14025
	protected JSONStorableUrl genitalsNormalUrlJSON;

	// Token: 0x040036CA RID: 14026
	protected JSONStorableBool faceDetailUrlHadErrorJSON;

	// Token: 0x040036CB RID: 14027
	protected JSONStorableUrl faceDetailUrlJSON;

	// Token: 0x040036CC RID: 14028
	protected JSONStorableBool torsoDetailUrlHadErrorJSON;

	// Token: 0x040036CD RID: 14029
	protected JSONStorableUrl torsoDetailUrlJSON;

	// Token: 0x040036CE RID: 14030
	protected JSONStorableBool limbsDetailUrlHadErrorJSON;

	// Token: 0x040036CF RID: 14031
	protected JSONStorableUrl limbsDetailUrlJSON;

	// Token: 0x040036D0 RID: 14032
	protected JSONStorableBool genitalsDetailUrlHadErrorJSON;

	// Token: 0x040036D1 RID: 14033
	protected JSONStorableUrl genitalsDetailUrlJSON;

	// Token: 0x040036D2 RID: 14034
	protected JSONStorableBool faceDecalUrlHadErrorJSON;

	// Token: 0x040036D3 RID: 14035
	protected JSONStorableUrl faceDecalUrlJSON;

	// Token: 0x040036D4 RID: 14036
	protected JSONStorableBool torsoDecalUrlHadErrorJSON;

	// Token: 0x040036D5 RID: 14037
	protected JSONStorableUrl torsoDecalUrlJSON;

	// Token: 0x040036D6 RID: 14038
	protected JSONStorableBool limbsDecalUrlHadErrorJSON;

	// Token: 0x040036D7 RID: 14039
	protected JSONStorableUrl limbsDecalUrlJSON;

	// Token: 0x040036D8 RID: 14040
	protected JSONStorableBool genitalsDecalUrlHadErrorJSON;

	// Token: 0x040036D9 RID: 14041
	protected JSONStorableUrl genitalsDecalUrlJSON;

	// Token: 0x040036DA RID: 14042
	protected Texture2D lastGeneratedDiffuseGenTexture;

	// Token: 0x040036DB RID: 14043
	protected Texture2D lastGeneratedSpecularGenTexture;

	// Token: 0x040036DC RID: 14044
	protected Texture2D lastGeneratedGlossGenTexture;

	// Token: 0x040036DD RID: 14045
	protected Texture2D lastGeneratedNormalGenTexture;

	// Token: 0x040036DE RID: 14046
	public Texture2D genitalBlendMaskTexture;

	// Token: 0x040036DF RID: 14047
	protected JSONStorableBool autoBlendGenitalTexturesJSON;

	// Token: 0x040036E0 RID: 14048
	protected JSONStorableBool autoBlendGenitalSpecGlossNormalTexturesJSON;

	// Token: 0x040036E1 RID: 14049
	protected Button dumpAutoGeneratedGenitalTextureButton;

	// Token: 0x040036E2 RID: 14050
	protected JSONStorableFloat autoBlendGenitalLightenDarkenJSON;

	// Token: 0x040036E3 RID: 14051
	protected JSONStorableFloat autoBlendGenitalHueOffsetJSON;

	// Token: 0x040036E4 RID: 14052
	protected JSONStorableFloat autoBlendGenitalSaturationOffsetJSON;

	// Token: 0x040036E5 RID: 14053
	protected Button autoBlendGenitalDiffuseTextureButton;

	// Token: 0x040036E6 RID: 14054
	public Button directoryBrowseButton;

	// Token: 0x040036E7 RID: 14055
	public Button directoryBrowseButtonAlt;

	// Token: 0x040036E8 RID: 14056
	protected string _lastDir;

	// Token: 0x02000AB7 RID: 2743
	public enum Region
	{
		// Token: 0x040036EA RID: 14058
		Face,
		// Token: 0x040036EB RID: 14059
		Torso,
		// Token: 0x040036EC RID: 14060
		Limbs,
		// Token: 0x040036ED RID: 14061
		Genitals
	}

	// Token: 0x02000AB8 RID: 2744
	public enum TextureType
	{
		// Token: 0x040036EF RID: 14063
		Diffuse,
		// Token: 0x040036F0 RID: 14064
		Specular,
		// Token: 0x040036F1 RID: 14065
		Gloss,
		// Token: 0x040036F2 RID: 14066
		Normal,
		// Token: 0x040036F3 RID: 14067
		Detail,
		// Token: 0x040036F4 RID: 14068
		Decal
	}

	// Token: 0x02000AB9 RID: 2745
	protected class CharacterQueuedImage : ImageLoaderThreaded.QueuedImage
	{
		// Token: 0x060048C7 RID: 18631 RVA: 0x001703BD File Offset: 0x0016E7BD
		public CharacterQueuedImage()
		{
		}

		// Token: 0x040036F5 RID: 14069
		public DAZCharacterTextureControl.Region region;

		// Token: 0x040036F6 RID: 14070
		public DAZCharacterTextureControl.TextureType textureType;
	}
}
