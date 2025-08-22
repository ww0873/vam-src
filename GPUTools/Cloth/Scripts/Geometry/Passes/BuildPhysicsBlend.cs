using System;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Painter.Scripts;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x0200098B RID: 2443
	public class BuildPhysicsBlend : BuildChainCommand
	{
		// Token: 0x06003CFD RID: 15613 RVA: 0x00127FD2 File Offset: 0x001263D2
		public BuildPhysicsBlend(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00127FE1 File Offset: 0x001263E1
		protected override void OnUpdateSettings()
		{
			this.OnBuild();
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x00127FEC File Offset: 0x001263EC
		protected override void OnBuild()
		{
			if (this.settings.EditorType == ClothEditorType.Texture)
			{
				this.BlendFromTexture();
			}
			if (this.settings.EditorType == ClothEditorType.Painter)
			{
				this.BlendFromPainter();
			}
			if (this.settings.EditorType == ClothEditorType.Provider)
			{
				this.BlendFromProvider();
			}
		}

		// Token: 0x06003D00 RID: 15616 RVA: 0x00128040 File Offset: 0x00126440
		private void BlendFromPainter()
		{
			if (this.settings.EditorPainter == null)
			{
				Debug.LogError("Painter field is uninitialized");
				return;
			}
			for (int i = 0; i < this.settings.GeometryData.ParticlesBlend.Length; i++)
			{
				Color color = this.settings.EditorPainter.Colors[i];
				this.SetBlend(i, color);
			}
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x001280B8 File Offset: 0x001264B8
		private void BlendFromTexture()
		{
			Texture2D editorTexture = this.settings.EditorTexture;
			Vector2[] uv = this.settings.MeshProvider.Mesh.uv;
			for (int i = 0; i < uv.Length; i++)
			{
				Vector2 vector = uv[i];
				Color pixelBilinear = editorTexture.GetPixelBilinear(vector.x, vector.y);
				this.SetBlend(i, pixelBilinear);
			}
			if (uv.Length == 0)
			{
				Debug.LogWarning("Add uv to mesh to use vertices blend");
			}
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x0012813C File Offset: 0x0012653C
		private void BlendFromProvider()
		{
			Color[] simColors = this.settings.MeshProvider.SimColors;
			if (simColors != null)
			{
				for (int i = 0; i < this.settings.GeometryData.ParticlesBlend.Length; i++)
				{
					Color color = simColors[i];
					this.SetBlend(i, color);
				}
			}
			else
			{
				for (int j = 0; j < this.settings.GeometryData.ParticlesBlend.Length; j++)
				{
					this.SetZeroBlend(j);
				}
			}
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x001281C8 File Offset: 0x001265C8
		private void SetBlend(int i, Color color)
		{
			if (this.settings.SimulateVsKinematicChannel == ColorChannel.R)
			{
				this.settings.GeometryData.ParticlesBlend[i] = color.r;
			}
			if (this.settings.SimulateVsKinematicChannel == ColorChannel.G)
			{
				this.settings.GeometryData.ParticlesBlend[i] = color.g;
			}
			if (this.settings.SimulateVsKinematicChannel == ColorChannel.B)
			{
				this.settings.GeometryData.ParticlesBlend[i] = color.b;
			}
			if (this.settings.SimulateVsKinematicChannel == ColorChannel.A)
			{
				this.settings.GeometryData.ParticlesBlend[i] = color.a;
			}
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x0012827C File Offset: 0x0012667C
		private void SetZeroBlend(int i)
		{
			this.settings.GeometryData.ParticlesBlend[i] = 0f;
		}

		// Token: 0x04002EFC RID: 12028
		private readonly ClothSettings settings;
	}
}
