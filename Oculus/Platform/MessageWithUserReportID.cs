using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000830 RID: 2096
	public class MessageWithUserReportID : Message<UserReportID>
	{
		// Token: 0x060036A8 RID: 13992 RVA: 0x0010BCCD File Offset: 0x0010A0CD
		public MessageWithUserReportID(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x0010BCD6 File Offset: 0x0010A0D6
		public override UserReportID GetUserReportID()
		{
			return base.Data;
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x0010BCE0 File Offset: 0x0010A0E0
		protected override UserReportID GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetUserReportID(obj);
			return new UserReportID(o);
		}
	}
}
