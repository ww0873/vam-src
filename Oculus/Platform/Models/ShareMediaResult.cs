using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000870 RID: 2160
	public class ShareMediaResult
	{
		// Token: 0x0600371B RID: 14107 RVA: 0x0010D11A File Offset: 0x0010B51A
		public ShareMediaResult(IntPtr o)
		{
			this.Status = CAPI.ovr_ShareMediaResult_GetStatus(o);
		}

		// Token: 0x0400287D RID: 10365
		public readonly ShareMediaStatus Status;
	}
}
