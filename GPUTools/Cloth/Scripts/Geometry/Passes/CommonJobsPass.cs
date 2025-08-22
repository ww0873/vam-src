using System;
using System.Collections.Generic;
using System.Linq;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x0200098D RID: 2445
	public class CommonJobsPass : ICacheCommand
	{
		// Token: 0x06003D0A RID: 15626 RVA: 0x00128490 File Offset: 0x00126890
		public CommonJobsPass(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x001284A0 File Offset: 0x001268A0
		public void Cache()
		{
			this.settings.GeometryData.ParticlesBlend = Enumerable.Repeat<float>(0f, this.settings.MeshProvider.MeshForImport.vertexCount).ToArray<float>();
			this.settings.GeometryData.ParticlesStrength = Enumerable.Repeat<float>(0.1f, this.settings.MeshProvider.MeshForImport.vertexCount).ToArray<float>();
			this.settings.GeometryData.AllTringles = this.GetAllTriangles();
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x0012852C File Offset: 0x0012692C
		private int[] GetAllTriangles()
		{
			Mesh meshForImport = this.settings.MeshProvider.MeshForImport;
			List<int> list = new List<int>();
			for (int i = 0; i < meshForImport.subMeshCount; i++)
			{
				list.AddRange(meshForImport.GetTriangles(i));
			}
			return list.ToArray();
		}

		// Token: 0x04002EFE RID: 12030
		private readonly ClothSettings settings;
	}
}
