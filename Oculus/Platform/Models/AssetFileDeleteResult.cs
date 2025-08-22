using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000840 RID: 2112
	public class AssetFileDeleteResult
	{
		// Token: 0x060036CF RID: 14031 RVA: 0x0010C2CA File Offset: 0x0010A6CA
		public AssetFileDeleteResult(IntPtr o)
		{
			this.AssetFileId = CAPI.ovr_AssetFileDeleteResult_GetAssetFileId(o);
			this.Success = CAPI.ovr_AssetFileDeleteResult_GetSuccess(o);
		}

		// Token: 0x040027F3 RID: 10227
		public readonly ulong AssetFileId;

		// Token: 0x040027F4 RID: 10228
		public readonly bool Success;
	}
}
