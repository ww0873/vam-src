using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000808 RID: 2056
	public class MessageWithAssetFileDownloadResult : Message<AssetFileDownloadResult>
	{
		// Token: 0x06003630 RID: 13872 RVA: 0x0010B4C9 File Offset: 0x001098C9
		public MessageWithAssetFileDownloadResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x0010B4D2 File Offset: 0x001098D2
		public override AssetFileDownloadResult GetAssetFileDownloadResult()
		{
			return base.Data;
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x0010B4DC File Offset: 0x001098DC
		protected override AssetFileDownloadResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetAssetFileDownloadResult(obj);
			return new AssetFileDownloadResult(o);
		}
	}
}
