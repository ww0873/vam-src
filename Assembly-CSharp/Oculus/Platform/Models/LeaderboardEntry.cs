using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200084F RID: 2127
	public class LeaderboardEntry
	{
		// Token: 0x060036F2 RID: 14066 RVA: 0x0010C66C File Offset: 0x0010AA6C
		public LeaderboardEntry(IntPtr o)
		{
			this.ExtraData = CAPI.ovr_LeaderboardEntry_GetExtraData(o);
			this.Rank = CAPI.ovr_LeaderboardEntry_GetRank(o);
			this.Score = CAPI.ovr_LeaderboardEntry_GetScore(o);
			this.Timestamp = CAPI.ovr_LeaderboardEntry_GetTimestamp(o);
			this.User = new User(CAPI.ovr_LeaderboardEntry_GetUser(o));
		}

		// Token: 0x04002822 RID: 10274
		public readonly byte[] ExtraData;

		// Token: 0x04002823 RID: 10275
		public readonly int Rank;

		// Token: 0x04002824 RID: 10276
		public readonly long Score;

		// Token: 0x04002825 RID: 10277
		public readonly DateTime Timestamp;

		// Token: 0x04002826 RID: 10278
		public readonly User User;
	}
}
