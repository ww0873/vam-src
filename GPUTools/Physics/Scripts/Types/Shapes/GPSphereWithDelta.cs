using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
	// Token: 0x02000A84 RID: 2692
	public struct GPSphereWithDelta
	{
		// Token: 0x060045C2 RID: 17858 RVA: 0x0013F86E File Offset: 0x0013DC6E
		public GPSphereWithDelta(Vector3 position, float radius)
		{
			this.Position = position;
			this.Radius = radius;
			this.Friction = 1f;
			this.Delta = Vector3.zero;
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x0013F894 File Offset: 0x0013DC94
		public static int Size()
		{
			return 32;
		}

		// Token: 0x0400337F RID: 13183
		public Vector3 Position;

		// Token: 0x04003380 RID: 13184
		public float Radius;

		// Token: 0x04003381 RID: 13185
		public float Friction;

		// Token: 0x04003382 RID: 13186
		public Vector3 Delta;
	}
}
