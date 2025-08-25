using System;

namespace Oculus.Platform
{
	// Token: 0x02000829 RID: 2089
	public class MessageWithString : Message<string>
	{
		// Token: 0x06003693 RID: 13971 RVA: 0x0010BB7D File Offset: 0x00109F7D
		public MessageWithString(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x0010BB86 File Offset: 0x00109F86
		public override string GetString()
		{
			return base.Data;
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x0010BB8E File Offset: 0x00109F8E
		protected override string GetDataFromMessage(IntPtr c_message)
		{
			return CAPI.ovr_Message_GetString(c_message);
		}
	}
}
