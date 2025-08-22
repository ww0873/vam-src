using System;

namespace Leap.Unity.Attributes
{
	// Token: 0x02000685 RID: 1669
	public class InspectorNameAttribute : CombinablePropertyAttribute, IFullPropertyDrawer
	{
		// Token: 0x0600285F RID: 10335 RVA: 0x000DEAC5 File Offset: 0x000DCEC5
		public InspectorNameAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x040021B0 RID: 8624
		public readonly string name;
	}
}
