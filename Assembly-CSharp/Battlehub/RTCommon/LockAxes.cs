using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000BA RID: 186
	public class LockAxes : MonoBehaviour
	{
		// Token: 0x0600030A RID: 778 RVA: 0x00014174 File Offset: 0x00012574
		public LockAxes()
		{
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0001417C File Offset: 0x0001257C
		public static LockObject Eval(LockAxes[] lockAxes)
		{
			LockObject lockObject = new LockObject();
			if (lockAxes != null)
			{
				LockObject lockObject2 = lockObject;
				if (LockAxes.<>f__am$cache0 == null)
				{
					LockAxes.<>f__am$cache0 = new Func<LockAxes, bool>(LockAxes.<Eval>m__0);
				}
				lockObject2.PositionX = lockAxes.Any(LockAxes.<>f__am$cache0);
				LockObject lockObject3 = lockObject;
				if (LockAxes.<>f__am$cache1 == null)
				{
					LockAxes.<>f__am$cache1 = new Func<LockAxes, bool>(LockAxes.<Eval>m__1);
				}
				lockObject3.PositionY = lockAxes.Any(LockAxes.<>f__am$cache1);
				LockObject lockObject4 = lockObject;
				if (LockAxes.<>f__am$cache2 == null)
				{
					LockAxes.<>f__am$cache2 = new Func<LockAxes, bool>(LockAxes.<Eval>m__2);
				}
				lockObject4.PositionZ = lockAxes.Any(LockAxes.<>f__am$cache2);
				LockObject lockObject5 = lockObject;
				if (LockAxes.<>f__am$cache3 == null)
				{
					LockAxes.<>f__am$cache3 = new Func<LockAxes, bool>(LockAxes.<Eval>m__3);
				}
				lockObject5.RotationX = lockAxes.Any(LockAxes.<>f__am$cache3);
				LockObject lockObject6 = lockObject;
				if (LockAxes.<>f__am$cache4 == null)
				{
					LockAxes.<>f__am$cache4 = new Func<LockAxes, bool>(LockAxes.<Eval>m__4);
				}
				lockObject6.RotationY = lockAxes.Any(LockAxes.<>f__am$cache4);
				LockObject lockObject7 = lockObject;
				if (LockAxes.<>f__am$cache5 == null)
				{
					LockAxes.<>f__am$cache5 = new Func<LockAxes, bool>(LockAxes.<Eval>m__5);
				}
				lockObject7.RotationZ = lockAxes.Any(LockAxes.<>f__am$cache5);
				LockObject lockObject8 = lockObject;
				if (LockAxes.<>f__am$cache6 == null)
				{
					LockAxes.<>f__am$cache6 = new Func<LockAxes, bool>(LockAxes.<Eval>m__6);
				}
				lockObject8.RotationScreen = lockAxes.Any(LockAxes.<>f__am$cache6);
				LockObject lockObject9 = lockObject;
				if (LockAxes.<>f__am$cache7 == null)
				{
					LockAxes.<>f__am$cache7 = new Func<LockAxes, bool>(LockAxes.<Eval>m__7);
				}
				lockObject9.ScaleX = lockAxes.Any(LockAxes.<>f__am$cache7);
				LockObject lockObject10 = lockObject;
				if (LockAxes.<>f__am$cache8 == null)
				{
					LockAxes.<>f__am$cache8 = new Func<LockAxes, bool>(LockAxes.<Eval>m__8);
				}
				lockObject10.ScaleY = lockAxes.Any(LockAxes.<>f__am$cache8);
				LockObject lockObject11 = lockObject;
				if (LockAxes.<>f__am$cache9 == null)
				{
					LockAxes.<>f__am$cache9 = new Func<LockAxes, bool>(LockAxes.<Eval>m__9);
				}
				lockObject11.ScaleZ = lockAxes.Any(LockAxes.<>f__am$cache9);
			}
			return lockObject;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00014330 File Offset: 0x00012730
		[CompilerGenerated]
		private static bool <Eval>m__0(LockAxes la)
		{
			return la.PositionX;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00014338 File Offset: 0x00012738
		[CompilerGenerated]
		private static bool <Eval>m__1(LockAxes la)
		{
			return la.PositionY;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00014340 File Offset: 0x00012740
		[CompilerGenerated]
		private static bool <Eval>m__2(LockAxes la)
		{
			return la.PositionZ;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00014348 File Offset: 0x00012748
		[CompilerGenerated]
		private static bool <Eval>m__3(LockAxes la)
		{
			return la.RotationX;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00014350 File Offset: 0x00012750
		[CompilerGenerated]
		private static bool <Eval>m__4(LockAxes la)
		{
			return la.RotationY;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00014358 File Offset: 0x00012758
		[CompilerGenerated]
		private static bool <Eval>m__5(LockAxes la)
		{
			return la.RotationZ;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00014360 File Offset: 0x00012760
		[CompilerGenerated]
		private static bool <Eval>m__6(LockAxes la)
		{
			return la.RotationScreen;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00014368 File Offset: 0x00012768
		[CompilerGenerated]
		private static bool <Eval>m__7(LockAxes la)
		{
			return la.ScaleX;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00014370 File Offset: 0x00012770
		[CompilerGenerated]
		private static bool <Eval>m__8(LockAxes la)
		{
			return la.ScaleY;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00014378 File Offset: 0x00012778
		[CompilerGenerated]
		private static bool <Eval>m__9(LockAxes la)
		{
			return la.ScaleZ;
		}

		// Token: 0x040003B9 RID: 953
		public bool PositionX;

		// Token: 0x040003BA RID: 954
		public bool PositionY;

		// Token: 0x040003BB RID: 955
		public bool PositionZ;

		// Token: 0x040003BC RID: 956
		public bool RotationX;

		// Token: 0x040003BD RID: 957
		public bool RotationY;

		// Token: 0x040003BE RID: 958
		public bool RotationZ;

		// Token: 0x040003BF RID: 959
		public bool RotationScreen;

		// Token: 0x040003C0 RID: 960
		public bool ScaleX;

		// Token: 0x040003C1 RID: 961
		public bool ScaleY;

		// Token: 0x040003C2 RID: 962
		public bool ScaleZ;

		// Token: 0x040003C3 RID: 963
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache0;

		// Token: 0x040003C4 RID: 964
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache1;

		// Token: 0x040003C5 RID: 965
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache2;

		// Token: 0x040003C6 RID: 966
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache3;

		// Token: 0x040003C7 RID: 967
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache4;

		// Token: 0x040003C8 RID: 968
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache5;

		// Token: 0x040003C9 RID: 969
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache6;

		// Token: 0x040003CA RID: 970
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache7;

		// Token: 0x040003CB RID: 971
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache8;

		// Token: 0x040003CC RID: 972
		[CompilerGenerated]
		private static Func<LockAxes, bool> <>f__am$cache9;
	}
}
