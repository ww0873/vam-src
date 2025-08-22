using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Shapes
{
	// Token: 0x02000A82 RID: 2690
	public struct GPLineSphereWithMatrixDelta
	{
		// Token: 0x060045BE RID: 17854 RVA: 0x0013F816 File Offset: 0x0013DC16
		public GPLineSphereWithMatrixDelta(Vector3 positionA, Vector3 positionB, float radiusA, float radiusB)
		{
			this.PositionA = positionA;
			this.PositionB = positionB;
			this.RadiusA = radiusA;
			this.RadiusB = radiusB;
			this.Friction = 1f;
			this.ChangeMatrix = Matrix4x4.identity;
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x0013F84B File Offset: 0x0013DC4B
		public static int Size()
		{
			return 100;
		}

		// Token: 0x04003376 RID: 13174
		public Vector3 PositionA;

		// Token: 0x04003377 RID: 13175
		public Vector3 PositionB;

		// Token: 0x04003378 RID: 13176
		public float RadiusA;

		// Token: 0x04003379 RID: 13177
		public float RadiusB;

		// Token: 0x0400337A RID: 13178
		public float Friction;

		// Token: 0x0400337B RID: 13179
		public Matrix4x4 ChangeMatrix;
	}
}
