using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
	// Token: 0x02000A83 RID: 2691
	public struct GPSphere
	{
		// Token: 0x060045C0 RID: 17856 RVA: 0x0013F84F File Offset: 0x0013DC4F
		public GPSphere(Vector3 position, float radius)
		{
			this.Position = position;
			this.Radius = radius;
			this.Friction = 1f;
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x0013F86A File Offset: 0x0013DC6A
		public static int Size()
		{
			return 20;
		}

		// Token: 0x0400337C RID: 13180
		public Vector3 Position;

		// Token: 0x0400337D RID: 13181
		public float Radius;

		// Token: 0x0400337E RID: 13182
		public float Friction;
	}
}
