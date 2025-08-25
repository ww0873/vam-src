using System;
using UnityEngine;

namespace MVR
{
	// Token: 0x02000E14 RID: 3604
	public static class NaNUtils
	{
		// Token: 0x06006F0E RID: 28430 RVA: 0x0029A66C File Offset: 0x00298A6C
		public static bool IsMatrixValid(Matrix4x4 m)
		{
			return !float.IsNaN(m.m00) && !float.IsNaN(m.m01) && !float.IsNaN(m.m02) && !float.IsNaN(m.m03) && !float.IsNaN(m.m10) && !float.IsNaN(m.m11) && !float.IsNaN(m.m12) && !float.IsNaN(m.m13) && !float.IsNaN(m.m20) && !float.IsNaN(m.m21) && !float.IsNaN(m.m22) && !float.IsNaN(m.m23) && !float.IsNaN(m.m30) && !float.IsNaN(m.m31) && !float.IsNaN(m.m32) && !float.IsNaN(m.m33);
		}

		// Token: 0x06006F0F RID: 28431 RVA: 0x0029A78C File Offset: 0x00298B8C
		public static bool IsQuaternionValid(Quaternion q)
		{
			return !float.IsNaN(q.x) && !float.IsNaN(q.y) && !float.IsNaN(q.z) && !float.IsNaN(q.w);
		}

		// Token: 0x06006F10 RID: 28432 RVA: 0x0029A7E0 File Offset: 0x00298BE0
		public static bool IsVector3Valid(Vector3 v)
		{
			return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z);
		}
	}
}
