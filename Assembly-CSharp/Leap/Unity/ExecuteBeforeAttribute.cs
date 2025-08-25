using System;

namespace Leap.Unity
{
	// Token: 0x0200072A RID: 1834
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	[Obsolete]
	public class ExecuteBeforeAttribute : Attribute
	{
		// Token: 0x06002CAC RID: 11436 RVA: 0x000EFA24 File Offset: 0x000EDE24
		public ExecuteBeforeAttribute(Type beforeType)
		{
		}

		// Token: 0x040023A4 RID: 9124
		public Type beforeType;
	}
}
