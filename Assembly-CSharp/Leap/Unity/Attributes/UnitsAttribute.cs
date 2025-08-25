using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x0200068C RID: 1676
	public class UnitsAttribute : CombinablePropertyAttribute, IAfterFieldAdditiveDrawer, IAdditiveDrawer
	{
		// Token: 0x06002867 RID: 10343 RVA: 0x000DEB78 File Offset: 0x000DCF78
		public UnitsAttribute(string unitsName)
		{
			this.unitsName = unitsName;
		}

		// Token: 0x040021BD RID: 8637
		public readonly string unitsName;
	}
}
