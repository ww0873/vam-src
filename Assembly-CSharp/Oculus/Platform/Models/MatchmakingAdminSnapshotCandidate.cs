using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000856 RID: 2134
	public class MatchmakingAdminSnapshotCandidate
	{
		// Token: 0x060036F9 RID: 14073 RVA: 0x0010C808 File Offset: 0x0010AC08
		public MatchmakingAdminSnapshotCandidate(IntPtr o)
		{
			this.CanMatch = CAPI.ovr_MatchmakingAdminSnapshotCandidate_GetCanMatch(o);
			this.MyTotalScore = CAPI.ovr_MatchmakingAdminSnapshotCandidate_GetMyTotalScore(o);
			this.TheirCurrentThreshold = CAPI.ovr_MatchmakingAdminSnapshotCandidate_GetTheirCurrentThreshold(o);
			this.TheirTotalScore = CAPI.ovr_MatchmakingAdminSnapshotCandidate_GetTheirTotalScore(o);
			this.TraceId = CAPI.ovr_MatchmakingAdminSnapshotCandidate_GetTraceId(o);
		}

		// Token: 0x04002834 RID: 10292
		public readonly bool CanMatch;

		// Token: 0x04002835 RID: 10293
		public readonly double MyTotalScore;

		// Token: 0x04002836 RID: 10294
		public readonly double TheirCurrentThreshold;

		// Token: 0x04002837 RID: 10295
		public readonly double TheirTotalScore;

		// Token: 0x04002838 RID: 10296
		public readonly string TraceId;
	}
}
