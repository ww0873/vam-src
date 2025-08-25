using System;
using Mono.Cecil;

namespace DynamicCSharp.Security
{
	// Token: 0x020002E7 RID: 743
	[Serializable]
	public abstract class Restriction
	{
		// Token: 0x0600118C RID: 4492 RVA: 0x00061260 File Offset: 0x0005F660
		protected Restriction()
		{
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600118D RID: 4493
		public abstract string Message { get; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600118E RID: 4494
		public abstract RestrictionMode Mode { get; }

		// Token: 0x0600118F RID: 4495
		public abstract bool Verify(ModuleDefinition module);
	}
}
