using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x0200067F RID: 1663
	public class DisableIfAll : DisableIfBase
	{
		// Token: 0x06002857 RID: 10327 RVA: 0x000DEA06 File Offset: 0x000DCE06
		public DisableIfAll(string propertyName1, string propertyName2, object areEqualTo = null, object areNotEqualTo = null) : base(areEqualTo, areNotEqualTo, true, new string[]
		{
			propertyName1,
			propertyName2
		})
		{
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000DEA20 File Offset: 0x000DCE20
		public DisableIfAll(string propertyName1, string propertyName2, string propertyName3, object areEqualTo = null, object areNotEqualTo = null) : base(areEqualTo, areNotEqualTo, true, new string[]
		{
			propertyName1,
			propertyName2,
			propertyName3
		})
		{
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x000DEA3F File Offset: 0x000DCE3F
		public DisableIfAll(string propertyName1, string propertyName2, string propertyName3, string propertyName4, object areEqualTo = null, object areNotEqualTo = null) : base(areEqualTo, areNotEqualTo, true, new string[]
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
