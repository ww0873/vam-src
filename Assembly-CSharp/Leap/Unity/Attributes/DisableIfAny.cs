using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x0200067E RID: 1662
	public class DisableIfAny : DisableIfBase
	{
		// Token: 0x06002854 RID: 10324 RVA: 0x000DE9A9 File Offset: 0x000DCDA9
		public DisableIfAny(string propertyName1, string propertyName2, object areEqualTo = null, object areNotEqualTo = null) : base(areEqualTo, areNotEqualTo, false, new string[]
		{
			propertyName1,
			propertyName2
		})
		{
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x000DE9C3 File Offset: 0x000DCDC3
		public DisableIfAny(string propertyName1, string propertyName2, string propertyName3, object areEqualTo = null, object areNotEqualTo = null) : base(areEqualTo, areNotEqualTo, false, new string[]
		{
			propertyName1,
			propertyName2,
			propertyName3
		})
		{
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000DE9E2 File Offset: 0x000DCDE2
		public DisableIfAny(string propertyName1, string propertyName2, string propertyName3, string propertyName4, object areEqualTo = null, object areNotEqualTo = null) : base(areEqualTo, areNotEqualTo, false, new string[]
		{
			propertyName1,
			propertyName2,
			propertyName3,
			propertyName4
		})
		{
		}
	}
}
