using System;
using System.Runtime.InteropServices;
using Leap;

namespace LeapInternal
{
	// Token: 0x0200060A RID: 1546
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_VECTOR
	{
		// Token: 0x06002628 RID: 9768 RVA: 0x000D77E8 File Offset: 0x000D5BE8
		public LEAP_VECTOR(Vector leap)
		{
			this.x = leap.x;
			this.y = leap.y;
			this.z = leap.z;
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000D7811 File Offset: 0x000D5C11
		public Vector ToLeapVector()
		{
			return new Vector(this.x, this.y, this.z);
		}

		// Token: 0x04002065 RID: 8293
		public float x;

		// Token: 0x04002066 RID: 8294
		public float y;

		// Token: 0x04002067 RID: 8295
		public float z;
	}
}
