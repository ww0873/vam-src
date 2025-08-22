using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000853 RID: 2131
	public class LivestreamingStatus
	{
		// Token: 0x060036F6 RID: 14070 RVA: 0x0010C768 File Offset: 0x0010AB68
		public LivestreamingStatus(IntPtr o)
		{
			this.CommentsVisible = CAPI.ovr_LivestreamingStatus_GetCommentsVisible(o);
			this.IsPaused = CAPI.ovr_LivestreamingStatus_GetIsPaused(o);
			this.LivestreamingEnabled = CAPI.ovr_LivestreamingStatus_GetLivestreamingEnabled(o);
			this.LivestreamingType = CAPI.ovr_LivestreamingStatus_GetLivestreamingType(o);
			this.MicEnabled = CAPI.ovr_LivestreamingStatus_GetMicEnabled(o);
		}

		// Token: 0x0400282A RID: 10282
		public readonly bool CommentsVisible;

		// Token: 0x0400282B RID: 10283
		public readonly bool IsPaused;

		// Token: 0x0400282C RID: 10284
		public readonly bool LivestreamingEnabled;

		// Token: 0x0400282D RID: 10285
		public readonly int LivestreamingType;

		// Token: 0x0400282E RID: 10286
		public readonly bool MicEnabled;
	}
}
