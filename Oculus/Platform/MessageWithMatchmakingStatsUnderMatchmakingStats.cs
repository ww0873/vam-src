using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000818 RID: 2072
	public class MessageWithMatchmakingStatsUnderMatchmakingStats : Message<MatchmakingStats>
	{
		// Token: 0x06003660 RID: 13920 RVA: 0x0010B809 File Offset: 0x00109C09
		public MessageWithMatchmakingStatsUnderMatchmakingStats(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x0010B812 File Offset: 0x00109C12
		public override MatchmakingStats GetMatchmakingStats()
		{
			return base.Data;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x0010B81C File Offset: 0x00109C1C
		protected override MatchmakingStats GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetMatchmakingStats(obj);
			return new MatchmakingStats(o);
		}
	}
}
