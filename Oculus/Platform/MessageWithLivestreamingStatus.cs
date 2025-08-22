using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000813 RID: 2067
	public class MessageWithLivestreamingStatus : Message<LivestreamingStatus>
	{
		// Token: 0x06003651 RID: 13905 RVA: 0x0010B705 File Offset: 0x00109B05
		public MessageWithLivestreamingStatus(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x0010B70E File Offset: 0x00109B0E
		public override LivestreamingStatus GetLivestreamingStatus()
		{
			return base.Data;
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x0010B718 File Offset: 0x00109B18
		protected override LivestreamingStatus GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetLivestreamingStatus(obj);
			return new LivestreamingStatus(o);
		}
	}
}
