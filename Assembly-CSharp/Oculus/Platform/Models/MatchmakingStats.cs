using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200085D RID: 2141
	public class MatchmakingStats
	{
		// Token: 0x06003700 RID: 14080 RVA: 0x0010CA58 File Offset: 0x0010AE58
		public MatchmakingStats(IntPtr o)
		{
			this.DrawCount = CAPI.ovr_MatchmakingStats_GetDrawCount(o);
			this.LossCount = CAPI.ovr_MatchmakingStats_GetLossCount(o);
			this.SkillLevel = CAPI.ovr_MatchmakingStats_GetSkillLevel(o);
			this.WinCount = CAPI.ovr_MatchmakingStats_GetWinCount(o);
		}

		// Token: 0x04002848 RID: 10312
		public readonly uint DrawCount;

		// Token: 0x04002849 RID: 10313
		public readonly uint LossCount;

		// Token: 0x0400284A RID: 10314
		public readonly uint SkillLevel;

		// Token: 0x0400284B RID: 10315
		public readonly uint WinCount;
	}
}
