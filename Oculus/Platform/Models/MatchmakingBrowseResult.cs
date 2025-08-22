using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000858 RID: 2136
	public class MatchmakingBrowseResult
	{
		// Token: 0x060036FB RID: 14075 RVA: 0x0010C8B2 File Offset: 0x0010ACB2
		public MatchmakingBrowseResult(IntPtr o)
		{
			this.EnqueueResult = new MatchmakingEnqueueResult(CAPI.ovr_MatchmakingBrowseResult_GetEnqueueResult(o));
			this.Rooms = new RoomList(CAPI.ovr_MatchmakingBrowseResult_GetRooms(o));
		}

		// Token: 0x04002839 RID: 10297
		public readonly MatchmakingEnqueueResult EnqueueResult;

		// Token: 0x0400283A RID: 10298
		public readonly RoomList Rooms;
	}
}
