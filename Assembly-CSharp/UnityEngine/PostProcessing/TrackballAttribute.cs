using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000024 RID: 36
	public sealed class TrackballAttribute : PropertyAttribute
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x0000831D File Offset: 0x0000671D
		public TrackballAttribute(string method)
		{
			this.method = method;
		}

		// Token: 0x04000115 RID: 277
		public readonly string method;
	}
}
