using System;

namespace Leap.Unity
{
	// Token: 0x0200072B RID: 1835
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[Obsolete]
	public class ExecuteAfterAttribute : Attribute
	{
		// Token: 0x06002CAD RID: 11437 RVA: 0x000EFA2C File Offset: 0x000EDE2C
		public ExecuteAfterAttribute(Type afterType)
		{
		}

		// Token: 0x040023A5 RID: 9125
		public Type afterType;
	}
}
