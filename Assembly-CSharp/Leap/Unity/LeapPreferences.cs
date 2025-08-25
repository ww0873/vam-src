using System;

namespace Leap.Unity
{
	// Token: 0x020006D8 RID: 1752
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class LeapPreferences : Attribute
	{
		// Token: 0x06002A29 RID: 10793 RVA: 0x000E3C32 File Offset: 0x000E2032
		public LeapPreferences(string header, int order)
		{
			this.header = header;
			this.order = order;
		}

		// Token: 0x04002265 RID: 8805
		public readonly string header;

		// Token: 0x04002266 RID: 8806
		public readonly int order;
	}
}
