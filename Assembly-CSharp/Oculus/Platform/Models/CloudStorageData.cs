using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000845 RID: 2117
	public class CloudStorageData
	{
		// Token: 0x060036D4 RID: 14036 RVA: 0x0010C380 File Offset: 0x0010A780
		public CloudStorageData(IntPtr o)
		{
			this.Bucket = CAPI.ovr_CloudStorageData_GetBucket(o);
			this.Data = CAPI.ovr_CloudStorageData_GetData(o);
			this.DataSize = CAPI.ovr_CloudStorageData_GetDataSize(o);
			this.Key = CAPI.ovr_CloudStorageData_GetKey(o);
		}

		// Token: 0x040027FE RID: 10238
		public readonly string Bucket;

		// Token: 0x040027FF RID: 10239
		public readonly byte[] Data;

		// Token: 0x04002800 RID: 10240
		public readonly uint DataSize;

		// Token: 0x04002801 RID: 10241
		public readonly string Key;
	}
}
