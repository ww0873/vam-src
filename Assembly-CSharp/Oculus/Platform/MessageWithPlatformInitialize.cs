using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000837 RID: 2103
	public class MessageWithPlatformInitialize : Message<PlatformInitialize>
	{
		// Token: 0x060036BE RID: 14014 RVA: 0x0010BE71 File Offset: 0x0010A271
		public MessageWithPlatformInitialize(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x0010BE7A File Offset: 0x0010A27A
		public override PlatformInitialize GetPlatformInitialize()
		{
			return base.Data;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x0010BE84 File Offset: 0x0010A284
		protected override PlatformInitialize GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetPlatformInitialize(obj);
			return new PlatformInitialize(o);
		}
	}
}
