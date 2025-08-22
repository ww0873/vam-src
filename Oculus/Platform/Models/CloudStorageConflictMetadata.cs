using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000844 RID: 2116
	public class CloudStorageConflictMetadata
	{
		// Token: 0x060036D3 RID: 14035 RVA: 0x0010C356 File Offset: 0x0010A756
		public CloudStorageConflictMetadata(IntPtr o)
		{
			this.Local = new CloudStorageMetadata(CAPI.ovr_CloudStorageConflictMetadata_GetLocal(o));
			this.Remote = new CloudStorageMetadata(CAPI.ovr_CloudStorageConflictMetadata_GetRemote(o));
		}

		// Token: 0x040027FC RID: 10236
		public readonly CloudStorageMetadata Local;

		// Token: 0x040027FD RID: 10237
		public readonly CloudStorageMetadata Remote;
	}
}
