using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000807 RID: 2055
	public class MessageWithAssetFileDownloadCancelResult : Message<AssetFileDownloadCancelResult>
	{
		// Token: 0x0600362D RID: 13869 RVA: 0x0010B495 File Offset: 0x00109895
		public MessageWithAssetFileDownloadCancelResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x0010B49E File Offset: 0x0010989E
		public override AssetFileDownloadCancelResult GetAssetFileDownloadCancelResult()
		{
			return base.Data;
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x0010B4A8 File Offset: 0x001098A8
		protected override AssetFileDownloadCancelResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetAssetFileDownloadCancelResult(obj);
			return new AssetFileDownloadCancelResult(o);
		}
	}
}
