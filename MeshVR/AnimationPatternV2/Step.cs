using System;
using UnityEngine;

namespace MeshVR.AnimationPatternV2
{
	// Token: 0x02000B54 RID: 2900
	public class Step
	{
		// Token: 0x06005103 RID: 20739 RVA: 0x001D4739 File Offset: 0x001D2B39
		public Step()
		{
		}

		// Token: 0x040040E3 RID: 16611
		public float timeStep;

		// Token: 0x040040E4 RID: 16612
		public Vector3 position;

		// Token: 0x040040E5 RID: 16613
		public Quaternion rotation;

		// Token: 0x040040E6 RID: 16614
		public bool positionOn;

		// Token: 0x040040E7 RID: 16615
		public bool rotationOn;
	}
}
