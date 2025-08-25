using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000688 RID: 1672
	public class MinValue : CombinablePropertyAttribute, IPropertyConstrainer
	{
		// Token: 0x06002863 RID: 10339 RVA: 0x000DEB1F File Offset: 0x000DCF1F
		public MinValue(float minValue)
		{
			this.minValue = minValue;
		}

		// Token: 0x040021B7 RID: 8631
		public float minValue;
	}
}
