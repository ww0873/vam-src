using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000846 RID: 2118
	public class CloudStorageMetadata
	{
		// Token: 0x060036D5 RID: 14037 RVA: 0x0010C3B8 File Offset: 0x0010A7B8
		public CloudStorageMetadata(IntPtr o)
		{
			this.Bucket = CAPI.ovr_CloudStorageMetadata_GetBucket(o);
			this.Counter = CAPI.ovr_CloudStorageMetadata_GetCounter(o);
			this.DataSize = CAPI.ovr_CloudStorageMetadata_GetDataSize(o);
			this.ExtraData = CAPI.ovr_CloudStorageMetadata_GetExtraData(o);
			this.Key = CAPI.ovr_CloudStorageMetadata_GetKey(o);
			this.SaveTime = CAPI.ovr_CloudStorageMetadata_GetSaveTime(o);
			this.Status = CAPI.ovr_CloudStorageMetadata_GetStatus(o);
			this.VersionHandle = CAPI.ovr_CloudStorageMetadata_GetVersionHandle(o);
		}

		// Token: 0x04002802 RID: 10242
		public readonly string Bucket;

		// Token: 0x04002803 RID: 10243
		public readonly long Counter;

		// Token: 0x04002804 RID: 10244
		public readonly uint DataSize;

		// Token: 0x04002805 RID: 10245
		public readonly string ExtraData;

		// Token: 0x04002806 RID: 10246
		public readonly string Key;

		// Token: 0x04002807 RID: 10247
		public readonly ulong SaveTime;

		// Token: 0x04002808 RID: 10248
		public readonly CloudStorageDataStatus Status;

		// Token: 0x04002809 RID: 10249
		public readonly string VersionHandle;
	}
}
