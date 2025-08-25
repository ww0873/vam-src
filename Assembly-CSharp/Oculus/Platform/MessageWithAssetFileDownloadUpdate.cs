using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000809 RID: 2057
	public class MessageWithAssetFileDownloadUpdate : Message<AssetFileDownloadUpdate>
	{
		// Token: 0x06003633 RID: 13875 RVA: 0x0010B4FD File Offset: 0x001098FD
		public MessageWithAssetFileDownloadUpdate(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x0010B506 File Offset: 0x00109906
		public override AssetFileDownloadUpdate GetAssetFileDownloadUpdate()
		{
			return base.Data;
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x0010B510 File Offset: 0x00109910
		protected override AssetFileDownloadUpdate GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetAssetFileDownloadUpdate(obj);
			return new AssetFileDownloadUpdate(o);
		}
	}
}
