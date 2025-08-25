using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Types.Joints
{
	// Token: 0x02000A7E RID: 2686
	public struct GPPointJoint
	{
		// Token: 0x060045B6 RID: 17846 RVA: 0x0013F748 File Offset: 0x0013DB48
		public GPPointJoint(int bodyId, int matrixId, Vector3 point, float rigidity)
		{
			this.BodyId = bodyId;
			this.Point = point;
			this.MatrixId = matrixId;
			this.Rigidity = rigidity;
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x0013F767 File Offset: 0x0013DB67
		public static int Size()
		{
			return 24;
		}

		// Token: 0x04003361 RID: 13153
		public int BodyId;

		// Token: 0x04003362 RID: 13154
		public int MatrixId;

		// Token: 0x04003363 RID: 13155
		public Vector3 Point;

		// Token: 0x04003364 RID: 13156
		public float Rigidity;
	}
}
