using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E66 RID: 3686
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Blur/Blur")]
	public class Blur : MonoBehaviour
	{
		// Token: 0x06007097 RID: 28823 RVA: 0x002A94CE File Offset: 0x002A78CE
		public Blur()
		{
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06007098 RID: 28824 RVA: 0x002A94E8 File Offset: 0x002A78E8
		protected Material material
		{
			get
			{
				if (Blur.m_Material == null)
				{
					Blur.m_Material = new Material(this.blurShader);
					Blur.m_Material.hideFlags = HideFlags.DontSave;
				}
				return Blur.m_Material;
			}
		}

		// Token: 0x06007099 RID: 28825 RVA: 0x002A951B File Offset: 0x002A791B
		protected void OnDisable()
		{
			if (Blur.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(Blur.m_Material);
			}
		}

		// Token: 0x0600709A RID: 28826 RVA: 0x002A9538 File Offset: 0x002A7938
		protected void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
				return;
			}
			if (!this.blurShader || !this.material.shader.isSupported)
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x0600709B RID: 28827 RVA: 0x002A9584 File Offset: 0x002A7984
		public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
		{
			float num = 0.5f + (float)iteration * this.blurSpread;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x0600709C RID: 28828 RVA: 0x002A9604 File Offset: 0x002A7A04
		private void DownSample4x(RenderTexture source, RenderTexture dest)
		{
			float num = 1f;
			Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
			{
				new Vector2(-num, -num),
				new Vector2(-num, num),
				new Vector2(num, num),
				new Vector2(num, -num)
			});
		}

		// Token: 0x0600709D RID: 28829 RVA: 0x002A967C File Offset: 0x002A7A7C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			int width = source.width / 4;
			int height = source.height / 4;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
			this.DownSample4x(source, renderTexture);
			for (int i = 0; i < this.iterations; i++)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
				this.FourTapCone(renderTexture, temporary, i);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0600709E RID: 28830 RVA: 0x002A96ED File Offset: 0x002A7AED
		// Note: this type is marked as 'beforefieldinit'.
		static Blur()
		{
		}

		// Token: 0x04006367 RID: 25447
		[Range(0f, 10f)]
		public int iterations = 3;

		// Token: 0x04006368 RID: 25448
		[Range(0f, 1f)]
		public float blurSpread = 0.6f;

		// Token: 0x04006369 RID: 25449
		public Shader blurShader;

		// Token: 0x0400636A RID: 25450
		private static Material m_Material;
	}
}
