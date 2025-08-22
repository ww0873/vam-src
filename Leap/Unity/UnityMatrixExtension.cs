using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200073A RID: 1850
	public static class UnityMatrixExtension
	{
		// Token: 0x06002D2C RID: 11564 RVA: 0x000F0FE8 File Offset: 0x000EF3E8
		public static Quaternion CalculateRotation(this LeapTransform trs)
		{
			Vector3 upwards = trs.yBasis.ToVector3();
			Vector3 forward = -trs.zBasis.ToVector3();
			return Quaternion.LookRotation(forward, upwards);
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000F101C File Offset: 0x000EF41C
		public static LeapTransform GetLeapMatrix(this Transform t)
		{
			Vector scale = new Vector(t.lossyScale.x * UnityMatrixExtension.MM_TO_M, t.lossyScale.y * UnityMatrixExtension.MM_TO_M, t.lossyScale.z * UnityMatrixExtension.MM_TO_M);
			LeapTransform result = new LeapTransform(t.position.ToVector(), t.rotation.ToLeapQuaternion(), scale);
			result.MirrorZ();
			return result;
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000F1094 File Offset: 0x000EF494
		// Note: this type is marked as 'beforefieldinit'.
		static UnityMatrixExtension()
		{
		}

		// Token: 0x040023BF RID: 9151
		public static readonly Vector LEAP_UP = new Vector(0f, 1f, 0f);

		// Token: 0x040023C0 RID: 9152
		public static readonly Vector LEAP_FORWARD = new Vector(0f, 0f, -1f);

		// Token: 0x040023C1 RID: 9153
		public static readonly Vector LEAP_ORIGIN = new Vector(0f, 0f, 0f);

		// Token: 0x040023C2 RID: 9154
		public static readonly float MM_TO_M = 0.001f;
	}
}
