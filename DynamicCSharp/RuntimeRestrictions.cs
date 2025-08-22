using System;

namespace DynamicCSharp
{
	// Token: 0x020002D9 RID: 729
	public static class RuntimeRestrictions
	{
		// Token: 0x060010F7 RID: 4343 RVA: 0x0005F22C File Offset: 0x0005D62C
		public static void AddRuntimeNamespaceRestriction(string namespaceName)
		{
			DynamicCSharp.Settings.AddRuntimeNamespaceRestriction(namespaceName);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0005F239 File Offset: 0x0005D639
		public static void RemoveRuntimeNamespaceRestriction(string namespaceName)
		{
			DynamicCSharp.Settings.RemoveRuntimeNamespaceRestriction(namespaceName);
		}
	}
}
