using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
	// Token: 0x02000A22 RID: 2594
	public struct TessRenderParticle
	{
		// Token: 0x0600431F RID: 17183 RVA: 0x0013B4D3 File Offset: 0x001398D3
		public static int Size()
		{
			return 56;
		}

		// Token: 0x040031D3 RID: 12755
		public Vector3 Position;

		// Token: 0x040031D4 RID: 12756
		public Vector3 Velocity;

		// Token: 0x040031D5 RID: 12757
		public Vector3 LightCenter;

		// Token: 0x040031D6 RID: 12758
		public Vector3 Color;

		// Token: 0x040031D7 RID: 12759
		public float Interpolation;

		// Token: 0x040031D8 RID: 12760
		public int RootIndex;
	}
}
