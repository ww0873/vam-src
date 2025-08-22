using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000812 RID: 2066
	public class MessageWithLivestreamingStartResult : Message<LivestreamingStartResult>
	{
		// Token: 0x0600364E RID: 13902 RVA: 0x0010B6D1 File Offset: 0x00109AD1
		public MessageWithLivestreamingStartResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x0010B6DA File Offset: 0x00109ADA
		public override LivestreamingStartResult GetLivestreamingStartResult()
		{
			return base.Data;
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x0010B6E4 File Offset: 0x00109AE4
		protected override LivestreamingStartResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetLivestreamingStartResult(obj);
			return new LivestreamingStartResult(o);
		}
	}
}
