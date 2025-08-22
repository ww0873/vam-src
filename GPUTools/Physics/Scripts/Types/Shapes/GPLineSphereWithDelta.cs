using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
	// Token: 0x02000A81 RID: 2689
	public struct GPLineSphereWithDelta
	{
		// Token: 0x060045BC RID: 17852 RVA: 0x0013F7BC File Offset: 0x0013DBBC
		public GPLineSphereWithDelta(Vector3 positionA, Vector3 positionB, float radiusA, float radiusB)
		{
			this.PositionA = positionA;
			this.PositionB = positionB;
			this.RadiusA = radiusA;
			this.RadiusB = radiusB;
			this.Friction = 1f;
			this.DeltaA = Vector3.zero;
			this.DeltaB = Vector3.zero;
			this.Delta = Vector3.zero;
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x0013F812 File Offset: 0x0013DC12
		public static int Size()
		{
			return 72;
		}

		// Token: 0x0400336E RID: 13166
		public Vector3 PositionA;

		// Token: 0x0400336F RID: 13167
		public Vector3 PositionB;

		// Token: 0x04003370 RID: 13168
		public float RadiusA;

		// Token: 0x04003371 RID: 13169
		public float RadiusB;

		// Token: 0x04003372 RID: 13170
		public float Friction;

		// Token: 0x04003373 RID: 13171
		public Vector3 DeltaA;

		// Token: 0x04003374 RID: 13172
		public Vector3 DeltaB;

		// Token: 0x04003375 RID: 13173
		public Vector3 Delta;
	}
}
