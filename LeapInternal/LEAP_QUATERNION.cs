using System;
using System.Runtime.InteropServices;
using Leap;

namespace LeapInternal
{
	// Token: 0x0200060B RID: 1547
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_QUATERNION
	{
		// Token: 0x0600262A RID: 9770 RVA: 0x000D782A File Offset: 0x000D5C2A
		public LEAP_QUATERNION(LeapQuaternion q)
		{
			this.x = q.x;
			this.y = q.y;
			this.z = q.z;
			this.w = q.w;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000D7860 File Offset: 0x000D5C60
		public LeapQuaternion ToLeapQuaternion()
		{
			return new LeapQuaternion(this.x, this.y, this.z, this.w);
		}

		// Token: 0x04002068 RID: 8296
		public float x;

		// Token: 0x04002069 RID: 8297
		public float y;

		// Token: 0x0400206A RID: 8298
		public float z;

		// Token: 0x0400206B RID: 8299
		public float w;
	}
}
