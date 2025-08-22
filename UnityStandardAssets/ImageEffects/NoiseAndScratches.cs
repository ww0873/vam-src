using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E83 RID: 3715
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Noise/Noise and Scratches")]
	public class NoiseAndScratches : MonoBehaviour
	{
		// Token: 0x06007109 RID: 28937 RVA: 0x002AEB24 File Offset: 0x002ACF24
		public NoiseAndScratches()
		{
		}

		// Token: 0x0600710A RID: 28938 RVA: 0x002AEB8C File Offset: 0x002ACF8C
		protected void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (this.shaderRGB == null || this.shaderYUV == null)
			{
				Debug.Log("Noise shaders are not set up! Disabling noise effect.");
				base.enabled = false;
			}
			else if (!this.shaderRGB.isSupported)
			{
				base.enabled = false;
			}
			else if (!this.shaderYUV.isSupported)
			{
				this.rgbFallback = true;
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x0600710B RID: 28939 RVA: 0x002AEC18 File Offset: 0x002AD018
		protected Material material
		{
			get
			{
				if (this.m_MaterialRGB == null)
				{
					this.m_MaterialRGB = new Material(this.shaderRGB);
					this.m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
				}
				if (this.m_MaterialYUV == null && !this.rgbFallback)
				{
					this.m_MaterialYUV = new Material(this.shaderYUV);
					this.m_MaterialYUV.hideFlags = HideFlags.HideAndDontSave;
				}
				return (this.rgbFallback || this.monochrome) ? this.m_MaterialRGB : this.m_MaterialYUV;
			}
		}

		// Token: 0x0600710C RID: 28940 RVA: 0x002AECB5 File Offset: 0x002AD0B5
		protected void OnDisable()
		{
			if (this.m_MaterialRGB)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialRGB);
			}
			if (this.m_MaterialYUV)
			{
				UnityEngine.Object.DestroyImmediate(this.m_MaterialYUV);
			}
		}

		// Token: 0x0600710D RID: 28941 RVA: 0x002AECF0 File Offset: 0x002AD0F0
		private void SanitizeParameters()
		{
			this.grainIntensityMin = Mathf.Clamp(this.grainIntensityMin, 0f, 5f);
			this.grainIntensityMax = Mathf.Clamp(this.grainIntensityMax, 0f, 5f);
			this.scratchIntensityMin = Mathf.Clamp(this.scratchIntensityMin, 0f, 5f);
			this.scratchIntensityMax = Mathf.Clamp(this.scratchIntensityMax, 0f, 5f);
			this.scratchFPS = Mathf.Clamp(this.scratchFPS, 1f, 30f);
			this.scratchJitter = Mathf.Clamp(this.scratchJitter, 0f, 1f);
			this.grainSize = Mathf.Clamp(this.grainSize, 0.1f, 50f);
		}

		// Token: 0x0600710E RID: 28942 RVA: 0x002AEDBC File Offset: 0x002AD1BC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.SanitizeParameters();
			if (this.scratchTimeLeft <= 0f)
			{
				this.scratchTimeLeft = UnityEngine.Random.value * 2f / this.scratchFPS;
				this.scratchX = UnityEngine.Random.value;
				this.scratchY = UnityEngine.Random.value;
			}
			this.scratchTimeLeft -= Time.deltaTime;
			Material material = this.material;
			material.SetTexture("_GrainTex", this.grainTexture);
			material.SetTexture("_ScratchTex", this.scratchTexture);
			float num = 1f / this.grainSize;
			material.SetVector("_GrainOffsetScale", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, (float)Screen.width / (float)this.grainTexture.width * num, (float)Screen.height / (float)this.grainTexture.height * num));
			material.SetVector("_ScratchOffsetScale", new Vector4(this.scratchX + UnityEngine.Random.value * this.scratchJitter, this.scratchY + UnityEngine.Random.value * this.scratchJitter, (float)Screen.width / (float)this.scratchTexture.width, (float)Screen.height / (float)this.scratchTexture.height));
			material.SetVector("_Intensity", new Vector4(UnityEngine.Random.Range(this.grainIntensityMin, this.grainIntensityMax), UnityEngine.Random.Range(this.scratchIntensityMin, this.scratchIntensityMax), 0f, 0f));
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x04006468 RID: 25704
		public bool monochrome = true;

		// Token: 0x04006469 RID: 25705
		private bool rgbFallback;

		// Token: 0x0400646A RID: 25706
		[Range(0f, 5f)]
		public float grainIntensityMin = 0.1f;

		// Token: 0x0400646B RID: 25707
		[Range(0f, 5f)]
		public float grainIntensityMax = 0.2f;

		// Token: 0x0400646C RID: 25708
		[Range(0.1f, 50f)]
		public float grainSize = 2f;

		// Token: 0x0400646D RID: 25709
		[Range(0f, 5f)]
		public float scratchIntensityMin = 0.05f;

		// Token: 0x0400646E RID: 25710
		[Range(0f, 5f)]
		public float scratchIntensityMax = 0.25f;

		// Token: 0x0400646F RID: 25711
		[Range(1f, 30f)]
		public float scratchFPS = 10f;

		// Token: 0x04006470 RID: 25712
		[Range(0f, 1f)]
		public float scratchJitter = 0.01f;

		// Token: 0x04006471 RID: 25713
		public Texture grainTexture;

		// Token: 0x04006472 RID: 25714
		public Texture scratchTexture;

		// Token: 0x04006473 RID: 25715
		public Shader shaderRGB;

		// Token: 0x04006474 RID: 25716
		public Shader shaderYUV;

		// Token: 0x04006475 RID: 25717
		private Material m_MaterialRGB;

		// Token: 0x04006476 RID: 25718
		private Material m_MaterialYUV;

		// Token: 0x04006477 RID: 25719
		private float scratchTimeLeft;

		// Token: 0x04006478 RID: 25720
		private float scratchX;

		// Token: 0x04006479 RID: 25721
		private float scratchY;
	}
}
