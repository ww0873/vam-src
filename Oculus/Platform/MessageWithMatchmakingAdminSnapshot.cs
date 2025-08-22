using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000815 RID: 2069
	public class MessageWithMatchmakingAdminSnapshot : Message<MatchmakingAdminSnapshot>
	{
		// Token: 0x06003657 RID: 13911 RVA: 0x0010B76D File Offset: 0x00109B6D
		public MessageWithMatchmakingAdminSnapshot(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x0010B776 File Offset: 0x00109B76
		public override MatchmakingAdminSnapshot GetMatchmakingAdminSnapshot()
		{
			return base.Data;
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x0010B780 File Offset: 0x00109B80
		protected override MatchmakingAdminSnapshot GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetMatchmakingAdminSnapshot(obj);
			return new MatchmakingAdminSnapshot(o);
		}
	}
}
