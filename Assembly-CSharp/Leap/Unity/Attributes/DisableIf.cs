using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x0200067D RID: 1661
	public class DisableIf : DisableIfBase
	{
		// Token: 0x06002853 RID: 10323 RVA: 0x000DE994 File Offset: 0x000DCD94
		public DisableIf(string propertyName, object isEqualTo = null, object isNotEqualTo = null) : base(isEqualTo, isNotEqualTo, true, new string[]
		{
			propertyName
		})
		{
		}
	}
}
