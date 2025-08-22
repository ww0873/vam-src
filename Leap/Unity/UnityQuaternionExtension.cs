using System;
using LeapInternal;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000739 RID: 1849
	public static class UnityQuaternionExtension
	{
		// Token: 0x06002D28 RID: 11560 RVA: 0x000F0F2C File Offset: 0x000EF32C
		public static Quaternion ToQuaternion(this LeapQuaternion q)
		{
			return new Quaternion(q.x, q.y, q.z, q.w);
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000F0F4F File Offset: 0x000EF34F
		public static Quaternion ToQuaternion(this LEAP_QUATERNION q)
		{
			return new Quaternion(q.x, q.y, q.z, q.w);
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000F0F72 File Offset: 0x000EF372
		public static LeapQuaternion ToLeapQuaternion(this Quaternion q)
		{
			return new LeapQuaternion(q.x, q.y, q.z, q.w);
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x000F0F98 File Offset: 0x000EF398
		public static LEAP_QUATERNION ToCQuaternion(this Quaternion q)
		{
			return new LEAP_QUATERNION
			{
				x = q.x,
				y = q.y,
				z = q.z,
				w = q.w
			};
		}
	}
}
