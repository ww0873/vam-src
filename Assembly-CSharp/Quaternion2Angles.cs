using System;
using UnityEngine;

// Token: 0x02000C99 RID: 3225
public class Quaternion2Angles
{
	// Token: 0x06006130 RID: 24880 RVA: 0x00250359 File Offset: 0x0024E759
	public Quaternion2Angles()
	{
	}

	// Token: 0x06006131 RID: 24881 RVA: 0x00250364 File Offset: 0x0024E764
	public static Vector3 GetAngles(Quaternion q, Quaternion2Angles.RotationOrder ro)
	{
		float w = q.w;
		float num;
		float num2;
		float num3;
		float num4;
		switch (ro)
		{
		case Quaternion2Angles.RotationOrder.XYZ:
			num = 1f;
			num2 = q.x;
			num3 = q.y;
			num4 = q.z;
			break;
		case Quaternion2Angles.RotationOrder.XZY:
			num = -1f;
			num2 = q.x;
			num3 = q.z;
			num4 = q.y;
			break;
		case Quaternion2Angles.RotationOrder.YXZ:
			num = -1f;
			num2 = q.y;
			num3 = q.x;
			num4 = q.z;
			break;
		case Quaternion2Angles.RotationOrder.YZX:
			num = 1f;
			num2 = q.y;
			num3 = q.z;
			num4 = q.x;
			break;
		case Quaternion2Angles.RotationOrder.ZXY:
			num = 1f;
			num2 = q.z;
			num3 = q.x;
			num4 = q.y;
			break;
		case Quaternion2Angles.RotationOrder.ZYX:
			num = -1f;
			num2 = q.z;
			num3 = q.y;
			num4 = q.x;
			break;
		default:
			num = 1f;
			num2 = q.x;
			num3 = q.y;
			num4 = q.z;
			break;
		}
		float num5 = num2 * num2;
		float num6 = num3 * num3;
		float num7 = num4 * num4;
		float num8 = Mathf.Atan2(2f * (w * num2 - num * num3 * num4), 1f - 2f * (num5 + num6));
		float num9 = Mathf.Asin(2f * (w * num3 + num * num2 * num4));
		float num10 = Mathf.Atan2(2f * (w * num4 - num * num2 * num3), 1f - 2f * (num6 + num7));
		Vector3 zero;
		switch (ro)
		{
		case Quaternion2Angles.RotationOrder.XYZ:
			zero.x = num8;
			zero.y = num9;
			zero.z = num10;
			break;
		case Quaternion2Angles.RotationOrder.XZY:
			zero.x = num8;
			zero.z = num9;
			zero.y = num10;
			break;
		case Quaternion2Angles.RotationOrder.YXZ:
			zero.y = num8;
			zero.x = num9;
			zero.z = num10;
			break;
		case Quaternion2Angles.RotationOrder.YZX:
			zero.y = num8;
			zero.z = num9;
			zero.x = num10;
			break;
		case Quaternion2Angles.RotationOrder.ZXY:
			zero.z = num8;
			zero.x = num9;
			zero.y = num10;
			break;
		case Quaternion2Angles.RotationOrder.ZYX:
			zero.z = num8;
			zero.y = num9;
			zero.x = num10;
			break;
		default:
			zero = Vector3.zero;
			break;
		}
		return zero;
	}

	// Token: 0x06006132 RID: 24882 RVA: 0x00250600 File Offset: 0x0024EA00
	public static Quaternion EulerToQuaternion(Vector3 r, Quaternion2Angles.RotationOrder ro)
	{
		Quaternion quaternion = Quaternion.Euler(r.x, 0f, 0f);
		Quaternion quaternion2 = Quaternion.Euler(0f, r.y, 0f);
		Quaternion quaternion3 = Quaternion.Euler(0f, 0f, r.z);
		Quaternion result = quaternion;
		switch (ro)
		{
		case Quaternion2Angles.RotationOrder.XYZ:
			result = quaternion * quaternion2 * quaternion3;
			break;
		case Quaternion2Angles.RotationOrder.XZY:
			result = quaternion * quaternion3 * quaternion2;
			break;
		case Quaternion2Angles.RotationOrder.YXZ:
			result = quaternion2 * quaternion * quaternion3;
			break;
		case Quaternion2Angles.RotationOrder.YZX:
			result = quaternion2 * quaternion3 * quaternion;
			break;
		case Quaternion2Angles.RotationOrder.ZXY:
			result = quaternion3 * quaternion * quaternion2;
			break;
		case Quaternion2Angles.RotationOrder.ZYX:
			result = quaternion3 * quaternion2 * quaternion;
			break;
		}
		return result;
	}

	// Token: 0x02000C9A RID: 3226
	public enum RotationOrder
	{
		// Token: 0x04005116 RID: 20758
		XYZ,
		// Token: 0x04005117 RID: 20759
		XZY,
		// Token: 0x04005118 RID: 20760
		YXZ,
		// Token: 0x04005119 RID: 20761
		YZX,
		// Token: 0x0400511A RID: 20762
		ZXY,
		// Token: 0x0400511B RID: 20763
		ZYX
	}
}
