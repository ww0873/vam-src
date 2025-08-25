using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000843 RID: 2115
	public class AssetFileDownloadUpdate
	{
		// Token: 0x060036D2 RID: 14034 RVA: 0x0010C31E File Offset: 0x0010A71E
		public AssetFileDownloadUpdate(IntPtr o)
		{
			this.AssetFileId = CAPI.ovr_AssetFileDownloadUpdate_GetAssetFileId(o);
			this.BytesTotal = CAPI.ovr_AssetFileDownloadUpdate_GetBytesTotal(o);
			this.BytesTransferred = CAPI.ovr_AssetFileDownloadUpdate_GetBytesTransferred(o);
			this.Completed = CAPI.ovr_AssetFileDownloadUpdate_GetCompleted(o);
		}

		// Token: 0x040027F8 RID: 10232
		public readonly ulong AssetFileId;

		// Token: 0x040027F9 RID: 10233
		public readonly uint BytesTotal;

		// Token: 0x040027FA RID: 10234
		public readonly uint BytesTransferred;

		// Token: 0x040027FB RID: 10235
		public readonly bool Completed;
	}
}
