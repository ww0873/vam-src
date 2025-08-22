using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000805 RID: 2053
	public class MessageWithApplicationVersion : Message<ApplicationVersion>
	{
		// Token: 0x06003627 RID: 13863 RVA: 0x0010B42D File Offset: 0x0010982D
		public MessageWithApplicationVersion(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x0010B436 File Offset: 0x00109836
		public override ApplicationVersion GetApplicationVersion()
		{
			return base.Data;
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x0010B440 File Offset: 0x00109840
		protected override ApplicationVersion GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetApplicationVersion(obj);
			return new ApplicationVersion(o);
		}
	}
}
