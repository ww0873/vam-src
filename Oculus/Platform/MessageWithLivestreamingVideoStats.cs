using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000814 RID: 2068
	public class MessageWithLivestreamingVideoStats : Message<LivestreamingVideoStats>
	{
		// Token: 0x06003654 RID: 13908 RVA: 0x0010B739 File Offset: 0x00109B39
		public MessageWithLivestreamingVideoStats(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x0010B742 File Offset: 0x00109B42
		public override LivestreamingVideoStats GetLivestreamingVideoStats()
		{
			return base.Data;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x0010B74C File Offset: 0x00109B4C
		protected override LivestreamingVideoStats GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetLivestreamingVideoStats(obj);
			return new LivestreamingVideoStats(o);
		}
	}
}
