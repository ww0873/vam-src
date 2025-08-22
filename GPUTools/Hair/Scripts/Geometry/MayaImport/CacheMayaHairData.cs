using System;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Commands;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport
{
	// Token: 0x020009FD RID: 2557
	public class CacheMayaHairData : CacheChainCommand
	{
		// Token: 0x060040C7 RID: 16583 RVA: 0x001343A2 File Offset: 0x001327A2
		public CacheMayaHairData(MayaHairGeometryImporter importer)
		{
			this.importer = importer;
			base.Add(new StripsToLinesMayaPass(importer));
			base.Add(new AssignHairLinesToTriangles(importer));
			base.Add(new MoveHairLinesToScalpVertices(importer));
			base.Add(new ComputeStandsToScalpVerticesMap(importer));
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x001343E4 File Offset: 0x001327E4
		protected override void OnCache()
		{
			base.OnCache();
			Debug.Log("segments:" + this.importer.Data.Segments);
			Debug.Log("vertices:" + this.importer.Data.Vertices.Count);
			Debug.Log("stands:" + this.importer.Data.Vertices.Count / this.importer.Data.Segments);
			Debug.Log("scalp triangles:" + this.importer.Data.Indices.Length / 3);
		}

		// Token: 0x040030C8 RID: 12488
		private readonly MayaHairGeometryImporter importer;
	}
}
