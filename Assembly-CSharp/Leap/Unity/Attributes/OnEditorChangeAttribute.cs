using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000689 RID: 1673
	public class OnEditorChangeAttribute : CombinablePropertyAttribute
	{
		// Token: 0x06002864 RID: 10340 RVA: 0x000DEB2E File Offset: 0x000DCF2E
		public OnEditorChangeAttribute(string methodName)
		{
			this.methodName = methodName;
		}

		// Token: 0x040021B8 RID: 8632
		public readonly string methodName;
	}
}
