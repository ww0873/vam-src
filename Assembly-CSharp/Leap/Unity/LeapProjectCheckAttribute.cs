using System;

namespace Leap.Unity
{
	// Token: 0x020006D9 RID: 1753
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class LeapProjectCheckAttribute : Attribute
	{
		// Token: 0x06002A2A RID: 10794 RVA: 0x000E3C48 File Offset: 0x000E2048
		public LeapProjectCheckAttribute(string header, int order)
		{
			this.header = header;
			this.order = order;
		}

		// Token: 0x04002267 RID: 8807
		public string header;

		// Token: 0x04002268 RID: 8808
		public int order;
	}
}
