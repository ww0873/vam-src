using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200080E RID: 2062
	public class MessageWithCloudStorageUpdateResponse : Message<CloudStorageUpdateResponse>
	{
		// Token: 0x06003642 RID: 13890 RVA: 0x0010B601 File Offset: 0x00109A01
		public MessageWithCloudStorageUpdateResponse(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x0010B60A File Offset: 0x00109A0A
		public override CloudStorageUpdateResponse GetCloudStorageUpdateResponse()
		{
			return base.Data;
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x0010B614 File Offset: 0x00109A14
		protected override CloudStorageUpdateResponse GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetCloudStorageUpdateResponse(obj);
			return new CloudStorageUpdateResponse(o);
		}
	}
}
