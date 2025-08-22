using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200074D RID: 1869
	public static class TransformUtil
	{
		// Token: 0x06002F82 RID: 12162 RVA: 0x000F73A9 File Offset: 0x000F57A9
		public static Quaternion TransformRotation(this Transform transform, Quaternion rotation)
		{
			return transform.rotation * rotation;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000F73B7 File Offset: 0x000F57B7
		public static Quaternion InverseTransformRotation(this Transform transform, Quaternion rotation)
		{
			return Quaternion.Inverse(transform.rotation) * rotation;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000F73CA File Offset: 0x000F57CA
		public static void SetLocalX(this Transform transform, float localX)
		{
			transform.setLocalAxis(localX, 0);
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000F73D4 File Offset: 0x000F57D4
		public static void SetLocalY(this Transform transform, float localY)
		{
			transform.setLocalAxis(localY, 1);
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000F73DE File Offset: 0x000F57DE
		public static void SetLocalZ(this Transform transform, float localZ)
		{
			transform.setLocalAxis(localZ, 2);
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x000F73E8 File Offset: 0x000F57E8
		private static void setLocalAxis(this Transform transform, float value, int axis)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition[axis] = value;
			transform.localPosition = localPosition;
		}
	}
}
