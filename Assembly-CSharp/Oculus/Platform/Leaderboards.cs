using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000885 RID: 2181
	public static class Leaderboards
	{
		// Token: 0x06003771 RID: 14193 RVA: 0x0010DF66 File Offset: 0x0010C366
		public static Request<LeaderboardEntryList> GetNextEntries(LeaderboardEntryList list)
		{
			if (Core.IsInitialized())
			{
				return new Request<LeaderboardEntryList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 1310751961));
			}
			return null;
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x0010DF89 File Offset: 0x0010C389
		public static Request<LeaderboardEntryList> GetPreviousEntries(LeaderboardEntryList list)
		{
			if (Core.IsInitialized())
			{
				return new Request<LeaderboardEntryList>(CAPI.ovr_HTTP_GetWithMessageType(list.PreviousUrl, 1224858304));
			}
			return null;
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x0010DFAC File Offset: 0x0010C3AC
		public static Request<LeaderboardEntryList> GetEntries(string leaderboardName, int limit, LeaderboardFilterType filter, LeaderboardStartAt startAt)
		{
			if (Core.IsInitialized())
			{
				return new Request<LeaderboardEntryList>(CAPI.ovr_Leaderboard_GetEntries(leaderboardName, limit, filter, startAt));
			}
			return null;
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x0010DFC8 File Offset: 0x0010C3C8
		public static Request<LeaderboardEntryList> GetEntriesAfterRank(string leaderboardName, int limit, ulong afterRank)
		{
			if (Core.IsInitialized())
			{
				return new Request<LeaderboardEntryList>(CAPI.ovr_Leaderboard_GetEntriesAfterRank(leaderboardName, limit, afterRank));
			}
			return null;
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x0010DFE3 File Offset: 0x0010C3E3
		public static Request<bool> WriteEntry(string leaderboardName, long score, byte[] extraData = null, bool forceUpdate = false)
		{
			if (Core.IsInitialized())
			{
				return new Request<bool>(CAPI.ovr_Leaderboard_WriteEntry(leaderboardName, score, extraData, (uint)((extraData == null) ? 0 : extraData.Length), forceUpdate));
			}
			return null;
		}
	}
}
