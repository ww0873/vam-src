using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
	// Token: 0x02000A21 RID: 2593
	public struct RenderParticle
	{
		// Token: 0x0600431E RID: 17182 RVA: 0x0013B4CF File Offset: 0x001398CF
		public static int Size()
		{
			return 28;
		}

		// Token: 0x040031CE RID: 12750
		public Vector3 Color;

		// Token: 0x040031CF RID: 12751
		public float Interpolation;

		// Token: 0x040031D0 RID: 12752
		public float WavinessScale;

		// Token: 0x040031D1 RID: 12753
		public float WavinessFrequency;

		// Token: 0x040031D2 RID: 12754
		public int RootIndex;
	}
}
