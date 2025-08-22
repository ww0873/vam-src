using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000841 RID: 2113
	public class AssetFileDownloadCancelResult
	{
		// Token: 0x060036D0 RID: 14032 RVA: 0x0010C2EA File Offset: 0x0010A6EA
		public AssetFileDownloadCancelResult(IntPtr o)
		{
			this.AssetFileId = CAPI.ovr_AssetFileDownloadCancelResult_GetAssetFileId(o);
			this.Success = CAPI.ovr_AssetFileDownloadCancelResult_GetSuccess(o);
		}

		// Token: 0x040027F5 RID: 10229
		public readonly ulong AssetFileId;

		// Token: 0x040027F6 RID: 10230
		public readonly bool Success;
	}
}
