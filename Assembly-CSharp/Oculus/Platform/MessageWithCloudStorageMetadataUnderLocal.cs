using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200080C RID: 2060
	public class MessageWithCloudStorageMetadataUnderLocal : Message<CloudStorageMetadata>
	{
		// Token: 0x0600363C RID: 13884 RVA: 0x0010B599 File Offset: 0x00109999
		public MessageWithCloudStorageMetadataUnderLocal(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x0010B5A2 File Offset: 0x001099A2
		public override CloudStorageMetadata GetCloudStorageMetadata()
		{
			return base.Data;
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x0010B5AC File Offset: 0x001099AC
		protected override CloudStorageMetadata GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetCloudStorageMetadata(obj);
			return new CloudStorageMetadata(o);
		}
	}
}
