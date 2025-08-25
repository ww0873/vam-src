using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000686 RID: 1670
	public class MaxValue : CombinablePropertyAttribute, IPropertyConstrainer
	{
		// Token: 0x06002860 RID: 10336 RVA: 0x000DEAD4 File Offset: 0x000DCED4
		public MaxValue(float maxValue)
		{
			this.maxValue = maxValue;
		}

		// Token: 0x040021B1 RID: 8625
		public float maxValue;
	}
}
