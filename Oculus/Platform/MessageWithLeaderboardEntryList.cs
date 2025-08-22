using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000810 RID: 2064
	public class MessageWithLeaderboardEntryList : Message<LeaderboardEntryList>
	{
		// Token: 0x06003648 RID: 13896 RVA: 0x0010B669 File Offset: 0x00109A69
		public MessageWithLeaderboardEntryList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x0010B672 File Offset: 0x00109A72
		public override LeaderboardEntryList GetLeaderboardEntryList()
		{
			return base.Data;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x0010B67C File Offset: 0x00109A7C
		protected override LeaderboardEntryList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetLeaderboardEntryArray(obj);
			return new LeaderboardEntryList(a);
		}
	}
}
