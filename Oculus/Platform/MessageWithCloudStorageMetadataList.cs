using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200080D RID: 2061
	public class MessageWithCloudStorageMetadataList : Message<CloudStorageMetadataList>
	{
		// Token: 0x0600363F RID: 13887 RVA: 0x0010B5CD File Offset: 0x001099CD
		public MessageWithCloudStorageMetadataList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x0010B5D6 File Offset: 0x001099D6
		public override CloudStorageMetadataList GetCloudStorageMetadataList()
		{
			return base.Data;
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x0010B5E0 File Offset: 0x001099E0
		protected override CloudStorageMetadataList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetCloudStorageMetadataArray(obj);
			return new CloudStorageMetadataList(a);
		}
	}
}
