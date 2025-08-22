using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000854 RID: 2132
	public class LivestreamingVideoStats
	{
		// Token: 0x060036F7 RID: 14071 RVA: 0x0010C7B7 File Offset: 0x0010ABB7
		public LivestreamingVideoStats(IntPtr o)
		{
			this.CommentCount = CAPI.ovr_LivestreamingVideoStats_GetCommentCount(o);
			this.ReactionCount = CAPI.ovr_LivestreamingVideoStats_GetReactionCount(o);
			this.TotalViews = CAPI.ovr_LivestreamingVideoStats_GetTotalViews(o);
		}

		// Token: 0x0400282F RID: 10287
		public readonly int CommentCount;

		// Token: 0x04002830 RID: 10288
		public readonly int ReactionCount;

		// Token: 0x04002831 RID: 10289
		public readonly string TotalViews;
	}
}
