using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000855 RID: 2133
	public class MatchmakingAdminSnapshot
	{
		// Token: 0x060036F8 RID: 14072 RVA: 0x0010C7E3 File Offset: 0x0010ABE3
		public MatchmakingAdminSnapshot(IntPtr o)
		{
			this.Candidates = new MatchmakingAdminSnapshotCandidateList(CAPI.ovr_MatchmakingAdminSnapshot_GetCandidates(o));
			this.MyCurrentThreshold = CAPI.ovr_MatchmakingAdminSnapshot_GetMyCurrentThreshold(o);
		}

		// Token: 0x04002832 RID: 10290
		public readonly MatchmakingAdminSnapshotCandidateList Candidates;

		// Token: 0x04002833 RID: 10291
		public readonly double MyCurrentThreshold;
	}
}
