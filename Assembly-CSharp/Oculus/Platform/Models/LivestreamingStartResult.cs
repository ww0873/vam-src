using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000852 RID: 2130
	public class LivestreamingStartResult
	{
		// Token: 0x060036F5 RID: 14069 RVA: 0x0010C752 File Offset: 0x0010AB52
		public LivestreamingStartResult(IntPtr o)
		{
			this.StreamingResult = CAPI.ovr_LivestreamingStartResult_GetStreamingResult(o);
		}

		// Token: 0x04002829 RID: 10281
		public readonly LivestreamingStartStatus StreamingResult;
	}
}
