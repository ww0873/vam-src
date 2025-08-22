using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000889 RID: 2185
	public static class AssetFile
	{
		// Token: 0x0600378C RID: 14220 RVA: 0x0010E26F File Offset: 0x0010C66F
		public static Request<AssetFileDeleteResult> Delete(ulong assetFileID)
		{
			if (Core.IsInitialized())
			{
				return new Request<AssetFileDeleteResult>(CAPI.ovr_AssetFile_Delete(assetFileID));
			}
			return null;
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x0010E288 File Offset: 0x0010C688
		public static Request<AssetFileDownloadResult> Download(ulong assetFileID)
		{
			if (Core.IsInitialized())
			{
				return new Request<AssetFileDownloadResult>(CAPI.ovr_AssetFile_Download(assetFileID));
			}
			return null;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x0010E2A1 File Offset: 0x0010C6A1
		public static Request<AssetFileDownloadCancelResult> DownloadCancel(ulong assetFileID)
		{
			if (Core.IsInitialized())
			{
				return new Request<AssetFileDownloadCancelResult>(CAPI.ovr_AssetFile_DownloadCancel(assetFileID));
			}
			return null;
		}
	}
}
