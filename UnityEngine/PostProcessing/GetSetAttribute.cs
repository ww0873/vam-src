using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000022 RID: 34
	public sealed class GetSetAttribute : PropertyAttribute
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x000082FF File Offset: 0x000066FF
		public GetSetAttribute(string name)
		{
			this.name = name;
		}

		// Token: 0x04000112 RID: 274
		public readonly string name;

		// Token: 0x04000113 RID: 275
		public bool dirty;
	}
}
