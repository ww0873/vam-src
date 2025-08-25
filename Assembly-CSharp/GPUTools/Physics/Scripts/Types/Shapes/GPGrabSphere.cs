using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
	// Token: 0x02000A7F RID: 2687
	public struct GPGrabSphere
	{
		// Token: 0x060045B8 RID: 17848 RVA: 0x0013F76B File Offset: 0x0013DB6B
		public GPGrabSphere(int id, Vector3 position, float radius)
		{
			this.ID = id;
			this.Position = position;
			this.Radius = radius;
			this.GrabbedThisFrame = 0;
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x0013F789 File Offset: 0x0013DB89
		public static int Size()
		{
			return 24;
		}

		// Token: 0x04003365 RID: 13157
		public int ID;

		// Token: 0x04003366 RID: 13158
		public Vector3 Position;

		// Token: 0x04003367 RID: 13159
		public float Radius;

		// Token: 0x04003368 RID: 13160
		public int GrabbedThisFrame;
	}
}
