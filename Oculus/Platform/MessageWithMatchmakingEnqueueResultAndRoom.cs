using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000817 RID: 2071
	public class MessageWithMatchmakingEnqueueResultAndRoom : Message<MatchmakingEnqueueResultAndRoom>
	{
		// Token: 0x0600365D RID: 13917 RVA: 0x0010B7D5 File Offset: 0x00109BD5
		public MessageWithMatchmakingEnqueueResultAndRoom(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x0010B7DE File Offset: 0x00109BDE
		public override MatchmakingEnqueueResultAndRoom GetMatchmakingEnqueueResultAndRoom()
		{
			return base.Data;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x0010B7E8 File Offset: 0x00109BE8
		protected override MatchmakingEnqueueResultAndRoom GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetMatchmakingEnqueueResultAndRoom(obj);
			return new MatchmakingEnqueueResultAndRoom(o);
		}
	}
}
