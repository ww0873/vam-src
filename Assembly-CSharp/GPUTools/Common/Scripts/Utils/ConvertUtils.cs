using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
	// Token: 0x020009DB RID: 2523
	public static class ConvertUtils
	{
		// Token: 0x06003F9B RID: 16283 RVA: 0x0012F965 File Offset: 0x0012DD65
		public static Color ToColor(this Vector4 vector)
		{
			return new Color(vector.x, vector.y, vector.z, vector.w);
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x0012F988 File Offset: 0x0012DD88
		public static Vector4 ToVector(this Color color)
		{
			return new Color(color.r, color.g, color.b, color.a);
		}
	}
}
