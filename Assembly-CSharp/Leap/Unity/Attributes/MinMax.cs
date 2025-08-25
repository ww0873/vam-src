using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000687 RID: 1671
	public class MinMax : CombinablePropertyAttribute, IFullPropertyDrawer
	{
		// Token: 0x06002861 RID: 10337 RVA: 0x000DEAE3 File Offset: 0x000DCEE3
		public MinMax(float min, float max)
		{
			this.min = min;
			this.max = max;
			this.isInt = false;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000DEB00 File Offset: 0x000DCF00
		public MinMax(int min, int max)
		{
			this.min = (float)min;
			this.max = (float)max;
			this.isInt = true;
		}

		// Token: 0x040021B2 RID: 8626
		public const float PERCENT_NUM = 0.2f;

		// Token: 0x040021B3 RID: 8627
		public const float SPACING = 3f;

		// Token: 0x040021B4 RID: 8628
		public readonly float min;

		// Token: 0x040021B5 RID: 8629
		public readonly float max;

		// Token: 0x040021B6 RID: 8630
		public readonly bool isInt;
	}
}
