using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000832 RID: 2098
	public class MessageWithPingResult : Message<PingResult>
	{
		// Token: 0x060036AE RID: 13998 RVA: 0x0010BD39 File Offset: 0x0010A139
		public MessageWithPingResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x0010BD42 File Offset: 0x0010A142
		public override PingResult GetPingResult()
		{
			return base.Data;
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x0010BD4C File Offset: 0x0010A14C
		protected override PingResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetPingResult(c_message);
			bool flag = CAPI.ovr_PingResult_IsTimeout(obj);
			return new PingResult(CAPI.ovr_PingResult_GetID(obj), (!flag) ? new ulong?(CAPI.ovr_PingResult_GetPingTimeUsec(obj)) : null);
		}
	}
}
