using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000023 RID: 35
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x0000830E File Offset: 0x0000670E
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x04000114 RID: 276
		public readonly float min;
	}
}
