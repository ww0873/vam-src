using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000848 RID: 2120
	public class CloudStorageUpdateResponse
	{
		// Token: 0x060036D7 RID: 14039 RVA: 0x0010C492 File Offset: 0x0010A892
		public CloudStorageUpdateResponse(IntPtr o)
		{
			this.Bucket = CAPI.ovr_CloudStorageUpdateResponse_GetBucket(o);
			this.Key = CAPI.ovr_CloudStorageUpdateResponse_GetKey(o);
			this.Status = CAPI.ovr_CloudStorageUpdateResponse_GetStatus(o);
			this.VersionHandle = CAPI.ovr_CloudStorageUpdateResponse_GetVersionHandle(o);
		}

		// Token: 0x0400280A RID: 10250
		public readonly string Bucket;

		// Token: 0x0400280B RID: 10251
		public readonly string Key;

		// Token: 0x0400280C RID: 10252
		public readonly CloudStorageUpdateStatus Status;

		// Token: 0x0400280D RID: 10253
		public readonly string VersionHandle;
	}
}
