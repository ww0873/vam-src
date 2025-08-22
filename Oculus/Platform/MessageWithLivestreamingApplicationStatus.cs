using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000811 RID: 2065
	public class MessageWithLivestreamingApplicationStatus : Message<LivestreamingApplicationStatus>
	{
		// Token: 0x0600364B RID: 13899 RVA: 0x0010B69D File Offset: 0x00109A9D
		public MessageWithLivestreamingApplicationStatus(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x0010B6A6 File Offset: 0x00109AA6
		public override LivestreamingApplicationStatus GetLivestreamingApplicationStatus()
		{
			return base.Data;
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x0010B6B0 File Offset: 0x00109AB0
		protected override LivestreamingApplicationStatus GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetLivestreamingApplicationStatus(obj);
			return new LivestreamingApplicationStatus(o);
		}
	}
}
