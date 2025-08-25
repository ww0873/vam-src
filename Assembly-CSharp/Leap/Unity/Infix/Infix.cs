using System;
using UnityEngine;

namespace Leap.Unity.Infix
{
	// Token: 0x02000733 RID: 1843
	public static class Infix
	{
		// Token: 0x06002CEE RID: 11502 RVA: 0x000F0A14 File Offset: 0x000EEE14
		public static float Clamped01(this float f)
		{
			return Mathf.Clamp01(f);
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x000F0A1C File Offset: 0x000EEE1C
		public static float Clamped(this float f, float min, float max)
		{
			return Mathf.Clamp(f, min, max);
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x000F0A26 File Offset: 0x000EEE26
		public static Vector3 RotatedBy(this Vector3 thisVector, Quaternion byQuaternion)
		{
			return byQuaternion * thisVector;
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x000F0A2F File Offset: 0x000EEE2F
		public static Vector3 MovedTowards(this Vector3 thisPosition, Vector3 otherPosition, float maxDistanceDelta)
		{
			return Vector3.MoveTowards(thisPosition, otherPosition, maxDistanceDelta);
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x000F0A39 File Offset: 0x000EEE39
		public static float Dot(this Vector3 a, Vector3 b)
		{
			return Vector3.Dot(a, b);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x000F0A42 File Offset: 0x000EEE42
		public static Vector3 Cross(this Vector3 a, Vector3 b)
		{
			return Vector3.Cross(a, b);
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x000F0A4B File Offset: 0x000EEE4B
		public static float Angle(this Vector3 a, Vector3 b)
		{
			return Vector3.Angle(a, b);
		}

		// Token: 0x06002CF5 RID: 11509 RVA: 0x000F0A54 File Offset: 0x000EEE54
		public static float SignedAngle(this Vector3 a, Vector3 b, Vector3 axis)
		{
			float num = (Vector3.Dot(Vector3.Cross(a, b), axis) >= 0f) ? 1f : -1f;
			return num * Vector3.Angle(a, b);
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000F0A91 File Offset: 0x000EEE91
		public static Vector3 GetRight(this Quaternion q)
		{
			return q * Vector3.right;
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x000F0A9E File Offset: 0x000EEE9E
		public static Vector3 GetUp(this Quaternion q)
		{
			return q * Vector3.up;
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000F0AAB File Offset: 0x000EEEAB
		public static Vector3 GetForward(this Quaternion q)
		{
			return q * Vector3.forward;
		}
	}
}
