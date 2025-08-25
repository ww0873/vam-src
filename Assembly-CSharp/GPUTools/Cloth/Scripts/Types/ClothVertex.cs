using System;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Types
{
	// Token: 0x020009AA RID: 2474
	public struct ClothVertex
	{
		// Token: 0x06003E69 RID: 15977 RVA: 0x0012CA51 File Offset: 0x0012AE51
		public ClothVertex(Vector3 position, Vector3 normal)
		{
			this.Position = position;
			this.LastPosition = position;
			this.Normal = normal;
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x0012CA68 File Offset: 0x0012AE68
		public static int Size()
		{
			return 36;
		}

		// Token: 0x04002FA4 RID: 12196
		public Vector3 Position;

		// Token: 0x04002FA5 RID: 12197
		public Vector3 LastPosition;

		// Token: 0x04002FA6 RID: 12198
		public Vector3 Normal;
	}
}
