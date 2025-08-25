using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000806 RID: 2054
	public class MessageWithAssetFileDeleteResult : Message<AssetFileDeleteResult>
	{
		// Token: 0x0600362A RID: 13866 RVA: 0x0010B461 File Offset: 0x00109861
		public MessageWithAssetFileDeleteResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x0010B46A File Offset: 0x0010986A
		public override AssetFileDeleteResult GetAssetFileDeleteResult()
		{
			return base.Data;
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x0010B474 File Offset: 0x00109874
		protected override AssetFileDeleteResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetAssetFileDeleteResult(obj);
			return new AssetFileDeleteResult(o);
		}
	}
}
