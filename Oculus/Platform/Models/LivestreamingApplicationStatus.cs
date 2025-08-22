using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000851 RID: 2129
	public class LivestreamingApplicationStatus
	{
		// Token: 0x060036F4 RID: 14068 RVA: 0x0010C73E File Offset: 0x0010AB3E
		public LivestreamingApplicationStatus(IntPtr o)
		{
			this.StreamingEnabled = CAPI.ovr_LivestreamingApplicationStatus_GetStreamingEnabled(o);
		}

		// Token: 0x04002828 RID: 10280
		public readonly bool StreamingEnabled;
	}
}
