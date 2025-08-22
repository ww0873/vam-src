using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Tools;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
	// Token: 0x020009FF RID: 2559
	public class ComputeStandsToScalpVerticesMap : CacheChainCommand
	{
		// Token: 0x060040CE RID: 16590 RVA: 0x00134672 File Offset: 0x00132A72
		public ComputeStandsToScalpVerticesMap(MayaHairGeometryImporter importer)
		{
			this.importer = importer;
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x00134681 File Offset: 0x00132A81
		protected override void OnCache()
		{
			this.importer.Data.HairRootToScalpMap = this.ProcessHairRootToScalpMap();
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x0013469C File Offset: 0x00132A9C
		private int[] ProcessHairRootToScalpMap()
		{
			if (this.importer.ScalpProvider.Type == ScalpMeshType.Skinned)
			{
				List<Vector3> scalpVertices = this.importer.ScalpProvider.Mesh.vertices.ToList<Vector3>();
				return ScalpProcessingTools.HairRootToScalpIndices(scalpVertices, this.importer.Data.Vertices, this.importer.Data.Segments, 1E-05f).ToArray();
			}
			return new int[this.importer.Data.Vertices.Count / this.importer.Data.Segments];
		}

		// Token: 0x040030CB RID: 12491
		private readonly MayaHairGeometryImporter importer;
	}
}
