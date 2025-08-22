using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200080B RID: 2059
	public class MessageWithCloudStorageData : Message<CloudStorageData>
	{
		// Token: 0x06003639 RID: 13881 RVA: 0x0010B565 File Offset: 0x00109965
		public MessageWithCloudStorageData(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x0010B56E File Offset: 0x0010996E
		public override CloudStorageData GetCloudStorageData()
		{
			return base.Data;
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x0010B578 File Offset: 0x00109978
		protected override CloudStorageData GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetCloudStorageData(obj);
			return new CloudStorageData(o);
		}
	}
}
