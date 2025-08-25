using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000847 RID: 2119
	public class CloudStorageMetadataList : DeserializableList<CloudStorageMetadata>
	{
		// Token: 0x060036D6 RID: 14038 RVA: 0x0010C42C File Offset: 0x0010A82C
		public CloudStorageMetadataList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_CloudStorageMetadataArray_GetSize(a));
			this._Data = new List<CloudStorageMetadata>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new CloudStorageMetadata(CAPI.ovr_CloudStorageMetadataArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_CloudStorageMetadataArray_GetNextUrl(a);
		}
	}
}
