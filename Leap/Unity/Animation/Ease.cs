using System;

namespace Leap.Unity.Animation
{
	// Token: 0x02000635 RID: 1589
	public static class Ease
	{
		// Token: 0x02000636 RID: 1590
		public static class Quadratic
		{
			// Token: 0x060026FB RID: 9979 RVA: 0x000DAA57 File Offset: 0x000D8E57
			public static float InOut(float t)
			{
				t *= 2f;
				if (t < 1f)
				{
					return 0.5f * t * t;
				}
				t -= 1f;
				return -0.5f * (t * (t - 2f) - 1f);
			}
		}

		// Token: 0x02000637 RID: 1591
		public static class Cubic
		{
			// Token: 0x060026FC RID: 9980 RVA: 0x000DAA95 File Offset: 0x000D8E95
			public static float InOut(float t)
			{
				t *= 2f;
				if (t < 1f)
				{
					return 0.5f * t * t * t;
				}
				t -= 2f;
				return 0.5f * (t * t * t + 2f);
			}
		}

		// Token: 0x02000638 RID: 1592
		public static class Quartic
		{
			// Token: 0x060026FD RID: 9981 RVA: 0x000DAAD1 File Offset: 0x000D8ED1
			public static float InOut(float t)
			{
				t *= 2f;
				if (t < 1f)
				{
					return 0.5f * t * t * t * t;
				}
				t -= 2f;
				return -0.5f * (t * t * t * t - 2f);
			}
		}
	}
}
