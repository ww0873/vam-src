using System;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Painter.Scripts;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x0200098C RID: 2444
	public class BuildPhysicsStrength : BuildChainCommand
	{
		// Token: 0x06003D05 RID: 15621 RVA: 0x00128295 File Offset: 0x00126695
		public BuildPhysicsStrength(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x001282A4 File Offset: 0x001266A4
		protected override void OnBuild()
		{
			if (this.settings.EditorStrengthType == ClothEditorType.Texture)
			{
				this.BlendFromTexture();
			}
			if (this.settings.EditorStrengthType == ClothEditorType.Painter)
			{
				this.BlendFromPainter();
			}
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x001282D4 File Offset: 0x001266D4
		private void BlendFromPainter()
		{
			if (this.settings.EditorStrengthPainter == null)
			{
				Debug.LogError("Painter field is uninitialized");
				return;
			}
			for (int i = 0; i < this.settings.GeometryData.ParticlesStrength.Length; i++)
			{
				Color color = this.settings.EditorStrengthPainter.Colors[i];
				this.SetBlend(i, color);
			}
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x0012834C File Offset: 0x0012674C
		private void BlendFromTexture()
		{
			Texture2D editorStrengthTexture = this.settings.EditorStrengthTexture;
			if (editorStrengthTexture != null)
			{
				Vector2[] uv = this.settings.MeshProvider.Mesh.uv;
				for (int i = 0; i < uv.Length; i++)
				{
					Vector2 vector = uv[i];
					Color pixelBilinear = editorStrengthTexture.GetPixelBilinear(vector.x, vector.y);
					this.SetBlend(i, pixelBilinear);
				}
				if (uv.Length == 0)
				{
					Debug.LogWarning("Add uv to mesh to use vertices blend");
				}
			}
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x001283DC File Offset: 0x001267DC
		private void SetBlend(int i, Color color)
		{
			if (this.settings.StrengthChannel == ColorChannel.R)
			{
				this.settings.GeometryData.ParticlesStrength[i] = color.r;
			}
			if (this.settings.StrengthChannel == ColorChannel.G)
			{
				this.settings.GeometryData.ParticlesStrength[i] = color.g;
			}
			if (this.settings.StrengthChannel == ColorChannel.B)
			{
				this.settings.GeometryData.ParticlesStrength[i] = color.b;
			}
			if (this.settings.StrengthChannel == ColorChannel.A)
			{
				this.settings.GeometryData.ParticlesStrength[i] = color.a;
			}
		}

		// Token: 0x04002EFD RID: 12029
		private readonly ClothSettings settings;
	}
}
