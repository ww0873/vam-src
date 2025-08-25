using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200080A RID: 2058
	public class MessageWithCloudStorageConflictMetadata : Message<CloudStorageConflictMetadata>
	{
		// Token: 0x06003636 RID: 13878 RVA: 0x0010B531 File Offset: 0x00109931
		public MessageWithCloudStorageConflictMetadata(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x0010B53A File Offset: 0x0010993A
		public override CloudStorageConflictMetadata GetCloudStorageConflictMetadata()
		{
			return base.Data;
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x0010B544 File Offset: 0x00109944
		protected override CloudStorageConflictMetadata GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetCloudStorageConflictMetadata(obj);
			return new CloudStorageConflictMetadata(o);
		}
	}
}
