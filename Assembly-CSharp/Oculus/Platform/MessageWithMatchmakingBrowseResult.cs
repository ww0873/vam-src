using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000835 RID: 2101
	public class MessageWithMatchmakingBrowseResult : Message<MatchmakingBrowseResult>
	{
		// Token: 0x060036B7 RID: 14007 RVA: 0x0010BDF9 File Offset: 0x0010A1F9
		public MessageWithMatchmakingBrowseResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x0010BE02 File Offset: 0x0010A202
		public override MatchmakingEnqueueResult GetMatchmakingEnqueueResult()
		{
			return base.Data.EnqueueResult;
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x0010BE0F File Offset: 0x0010A20F
		public override RoomList GetRoomList()
		{
			return base.Data.Rooms;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x0010BE1C File Offset: 0x0010A21C
		protected override MatchmakingBrowseResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetMatchmakingBrowseResult(obj);
			return new MatchmakingBrowseResult(o);
		}
	}
}
