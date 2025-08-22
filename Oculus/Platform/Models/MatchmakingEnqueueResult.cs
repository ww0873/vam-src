using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200085B RID: 2139
	public class MatchmakingEnqueueResult
	{
		// Token: 0x060036FE RID: 14078 RVA: 0x0010C998 File Offset: 0x0010AD98
		public MatchmakingEnqueueResult(IntPtr o)
		{
			IntPtr intPtr = CAPI.ovr_MatchmakingEnqueueResult_GetAdminSnapshot(o);
			this.AdminSnapshot = new MatchmakingAdminSnapshot(intPtr);
			if (intPtr == IntPtr.Zero)
			{
				this.AdminSnapshotOptional = null;
			}
			else
			{
				this.AdminSnapshotOptional = this.AdminSnapshot;
			}
			this.AverageWait = CAPI.ovr_MatchmakingEnqueueResult_GetAverageWait(o);
			this.MatchesInLastHourCount = CAPI.ovr_MatchmakingEnqueueResult_GetMatchesInLastHourCount(o);
			this.MaxExpectedWait = CAPI.ovr_MatchmakingEnqueueResult_GetMaxExpectedWait(o);
			this.Pool = CAPI.ovr_MatchmakingEnqueueResult_GetPool(o);
			this.RecentMatchPercentage = CAPI.ovr_MatchmakingEnqueueResult_GetRecentMatchPercentage(o);
			this.RequestHash = CAPI.ovr_MatchmakingEnqueueResult_GetRequestHash(o);
		}

		// Token: 0x0400283E RID: 10302
		public readonly MatchmakingAdminSnapshot AdminSnapshotOptional;

		// Token: 0x0400283F RID: 10303
		[Obsolete("Deprecated in favor of AdminSnapshotOptional")]
		public readonly MatchmakingAdminSnapshot AdminSnapshot;

		// Token: 0x04002840 RID: 10304
		public readonly uint AverageWait;

		// Token: 0x04002841 RID: 10305
		public readonly uint MatchesInLastHourCount;

		// Token: 0x04002842 RID: 10306
		public readonly uint MaxExpectedWait;

		// Token: 0x04002843 RID: 10307
		public readonly string Pool;

		// Token: 0x04002844 RID: 10308
		public readonly uint RecentMatchPercentage;

		// Token: 0x04002845 RID: 10309
		public readonly string RequestHash;
	}
}
