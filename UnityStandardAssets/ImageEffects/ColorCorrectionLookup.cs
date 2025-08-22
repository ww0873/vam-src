using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E6D RID: 3693
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (3D Lookup Texture)")]
	public class ColorCorrectionLookup : PostEffectsBase
	{
		// Token: 0x060070B6 RID: 28854 RVA: 0x002AAE8D File Offset: 0x002A928D
		public ColorCorrectionLookup()
		{
		}

		// Token: 0x060070B7 RID: 28855 RVA: 0x002AAEA0 File Offset: 0x002A92A0
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.material = base.CheckShaderAndCreateMaterial(this.shader, this.material);
			if (!this.isSupported || !SystemInfo.supports3DTextures)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060070B8 RID: 28856 RVA: 0x002AAEEE File Offset: 0x002A92EE
		private void OnDisable()
		{
			if (this.material)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		// Token: 0x060070B9 RID: 28857 RVA: 0x002AAF12 File Offset: 0x002A9312
		private void OnDestroy()
		{
			if (this.converted3DLut)
			{
				UnityEngine.Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = null;
		}

		// Token: 0x060070BA RID: 28858 RVA: 0x002AAF38 File Offset: 0x002A9338
		public void SetIdentityLut()
		{
			int num = 16;
			Color[] array = new Color[num * num * num];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
					}
				}
			}
			if (this.converted3DLut)
			{
				UnityEngine.Object.DestroyImmediate(this.converted3DLut);
			}
			this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
			this.converted3DLut.SetPixels(array);
			this.converted3DLut.Apply();
			this.basedOnTempTex = string.Empty;
		}

		// Token: 0x060070BB RID: 28859 RVA: 0x002AB038 File Offset: 0x002A9438
		public bool ValidDimensions(Texture2D tex2d)
		{
			if (!tex2d)
			{
				return false;
			}
			int height = tex2d.height;
			return height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
		}

		// Token: 0x060070BC RID: 28860 RVA: 0x002AB074 File Offset: 0x002A9474
		public void Convert(Texture2D temp2DTex, string path)
		{
			if (temp2DTex)
			{
				int num = temp2DTex.width * temp2DTex.height;
				num = temp2DTex.height;
				if (!this.ValidDimensions(temp2DTex))
				{
					Debug.LogWarning("The given 2D texture " + temp2DTex.name + " cannot be used as a 3D LUT.");
					this.basedOnTempTex = string.Empty;
					return;
				}
				Color[] pixels = temp2DTex.GetPixels();
				Color[] array = new Color[pixels.Length];
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num; j++)
					{
						for (int k = 0; k < num; k++)
						{
							int num2 = num - j - 1;
							array[i + j * num + k * num * num] = pixels[k * num + i + num2 * num * num];
						}
					}
				}
				if (this.converted3DLut)
				{
					UnityEngine.Object.DestroyImmediate(this.converted3DLut);
				}
				this.converted3DLut = new Texture3D(num, num, num, TextureFormat.ARGB32, false);
				this.converted3DLut.SetPixels(array);
				this.converted3DLut.Apply();
				this.basedOnTempTex = path;
			}
			else
			{
				Debug.LogError("Couldn't color correct with 3D LUT texture. Image Effect will be disabled.");
			}
		}

		// Token: 0x060070BD RID: 28861 RVA: 0x002AB1B4 File Offset: 0x002A95B4
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources() || !SystemInfo.supports3DTextures)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.converted3DLut == null)
			{
				this.SetIdentityLut();
			}
			int width = this.converted3DLut.width;
			this.converted3DLut.wrapMode = TextureWrapMode.Clamp;
			this.material.SetFloat("_Scale", (float)(width - 1) / (1f * (float)width));
			this.material.SetFloat("_Offset", 1f / (2f * (float)width));
			this.material.SetTexture("_ClutTex", this.converted3DLut);
			Graphics.Blit(source, destination, this.material, (QualitySettings.activeColorSpace != ColorSpace.Linear) ? 0 : 1);
		}

		// Token: 0x040063B5 RID: 25525
		public Shader shader;

		// Token: 0x040063B6 RID: 25526
		private Material material;

		// Token: 0x040063B7 RID: 25527
		public Texture3D converted3DLut;

		// Token: 0x040063B8 RID: 25528
		public string basedOnTempTex = string.Empty;
	}
}
