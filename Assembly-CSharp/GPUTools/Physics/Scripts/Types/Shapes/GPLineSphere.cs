using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
	// Token: 0x02000A80 RID: 2688
	public struct GPLineSphere
	{
		// Token: 0x060045BA RID: 17850 RVA: 0x0013F78D File Offset: 0x0013DB8D
		public GPLineSphere(Vector3 positionA, Vector3 positionB, float radiusA, float radiusB)
		{
			this.PositionA = positionA;
			this.PositionB = positionB;
			this.RadiusA = radiusA;
			this.RadiusB = radiusB;
			this.Friction = 1f;
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x0013F7B7 File Offset: 0x0013DBB7
		public static int Size()
		{
			return 36;
		}

		// Token: 0x04003369 RID: 13161
		public Vector3 PositionA;

		// Token: 0x0400336A RID: 13162
		public Vector3 PositionB;

		// Token: 0x0400336B RID: 13163
		public float RadiusA;

		// Token: 0x0400336C RID: 13164
		public float RadiusB;

		// Token: 0x0400336D RID: 13165
		public float Friction;
	}
}
