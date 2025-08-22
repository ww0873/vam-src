using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000842 RID: 2114
	public class AssetFileDownloadResult
	{
		// Token: 0x060036D1 RID: 14033 RVA: 0x0010C30A File Offset: 0x0010A70A
		public AssetFileDownloadResult(IntPtr o)
		{
			this.Filepath = CAPI.ovr_AssetFileDownloadResult_GetFilepath(o);
		}

		// Token: 0x040027F7 RID: 10231
		public readonly string Filepath;
	}
}
