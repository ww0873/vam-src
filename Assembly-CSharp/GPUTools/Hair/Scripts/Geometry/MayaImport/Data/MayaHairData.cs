using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Data
{
	// Token: 0x02000A02 RID: 2562
	[Serializable]
	public class MayaHairData
	{
		// Token: 0x060040DA RID: 16602 RVA: 0x00134AEB File Offset: 0x00132EEB
		public MayaHairData()
		{
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00134AF3 File Offset: 0x00132EF3
		public bool Validate(bool log)
		{
			if (this.Indices == null || this.Indices.Length == 0)
			{
				if (log)
				{
					Debug.LogError("Maya data was not generated succesfuly");
				}
				return false;
			}
			return true;
		}

		// Token: 0x040030CF RID: 12495
		[SerializeField]
		public int Segments;

		// Token: 0x040030D0 RID: 12496
		[SerializeField]
		public List<Vector3> Lines;

		// Token: 0x040030D1 RID: 12497
		[SerializeField]
		public List<Vector3> TringlesCenters;

		// Token: 0x040030D2 RID: 12498
		[SerializeField]
		public int[] HairRootToScalpMap;

		// Token: 0x040030D3 RID: 12499
		[SerializeField]
		public int[] Indices;

		// Token: 0x040030D4 RID: 12500
		[SerializeField]
		public List<Vector3> Vertices;
	}
}
