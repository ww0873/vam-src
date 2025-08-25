using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A2 RID: 930
	public class ObiTerrainShapeTracker : ObiShapeTracker
	{
		// Token: 0x0600177A RID: 6010 RVA: 0x00086178 File Offset: 0x00084578
		public ObiTerrainShapeTracker(TerrainCollider collider)
		{
			this.collider = collider;
			this.adaptor.is2D = false;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Heightmap);
			this.UpdateHeightData();
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000861A8 File Offset: 0x000845A8
		public void UpdateHeightData()
		{
			TerrainCollider terrainCollider = this.collider as TerrainCollider;
			if (terrainCollider != null)
			{
				TerrainData terrainData = terrainCollider.terrainData;
				float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
				float[] array = new float[terrainData.heightmapWidth * terrainData.heightmapHeight];
				for (int i = 0; i < terrainData.heightmapHeight; i++)
				{
					for (int j = 0; j < terrainData.heightmapWidth; j++)
					{
						array[i * terrainData.heightmapWidth + j] = heights[i, j];
					}
				}
				Oni.UnpinMemory(this.dataHandle);
				this.dataHandle = Oni.PinMemory(array);
				this.heightmapDataHasChanged = true;
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00086268 File Offset: 0x00084668
		public override void UpdateIfNeeded()
		{
			TerrainCollider terrainCollider = this.collider as TerrainCollider;
			if (terrainCollider != null)
			{
				TerrainData terrainData = terrainCollider.terrainData;
				if (terrainData != null && (terrainData.size != this.size || terrainData.heightmapWidth != this.resolutionU || terrainData.heightmapHeight != this.resolutionV || this.heightmapDataHasChanged))
				{
					this.size = terrainData.size;
					this.resolutionU = terrainData.heightmapWidth;
					this.resolutionV = terrainData.heightmapHeight;
					this.heightmapDataHasChanged = false;
					this.adaptor.Set(this.size, this.resolutionU, this.resolutionV, this.dataHandle.AddrOfPinnedObject());
					Oni.UpdateShape(this.oniShape, ref this.adaptor);
				}
			}
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00086347 File Offset: 0x00084747
		public override void Destroy()
		{
			base.Destroy();
			Oni.UnpinMemory(this.dataHandle);
		}

		// Token: 0x04001357 RID: 4951
		private Vector3 size;

		// Token: 0x04001358 RID: 4952
		private int resolutionU;

		// Token: 0x04001359 RID: 4953
		private int resolutionV;

		// Token: 0x0400135A RID: 4954
		private GCHandle dataHandle;

		// Token: 0x0400135B RID: 4955
		private bool heightmapDataHasChanged;
	}
}
