using System;

namespace Oculus.Platform
{
	// Token: 0x02000833 RID: 2099
	public class MessageWithLeaderboardDidUpdate : Message<bool>
	{
		// Token: 0x060036B1 RID: 14001 RVA: 0x0010BD91 File Offset: 0x0010A191
		public MessageWithLeaderboardDidUpdate(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x0010BD9A File Offset: 0x0010A19A
		public override bool GetLeaderboardDidUpdate()
		{
			return base.Data;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x0010BDA4 File Offset: 0x0010A1A4
		protected override bool GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr obj2 = CAPI.ovr_Message_GetLeaderboardUpdateStatus(obj);
			return CAPI.ovr_LeaderboardUpdateStatus_GetDidUpdate(obj2);
		}
	}
}
