using System;
using LeapInternal;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000738 RID: 1848
	public static class UnityVectorExtension
	{
		// Token: 0x06002D23 RID: 11555 RVA: 0x000F0E74 File Offset: 0x000EF274
		public static Vector3 ToVector3(this Vector vector)
		{
			return new Vector3(vector.x, vector.y, vector.z);
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000F0E90 File Offset: 0x000EF290
		public static Vector3 ToVector3(this LEAP_VECTOR vector)
		{
			return new Vector3(vector.x, vector.y, vector.z);
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000F0EAC File Offset: 0x000EF2AC
		public static Vector4 ToVector4(this Vector vector)
		{
			return new Vector4(vector.x, vector.y, vector.z, 0f);
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000F0ECD File Offset: 0x000EF2CD
		public static Vector ToVector(this Vector3 vector)
		{
			return new Vector(vector.x, vector.y, vector.z);
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000F0EEC File Offset: 0x000EF2EC
		public static LEAP_VECTOR ToCVector(this Vector3 vector)
		{
			return new LEAP_VECTOR
			{
				x = vector.x,
				y = vector.y,
				z = vector.z
			};
		}
	}
}
