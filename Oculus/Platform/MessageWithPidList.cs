using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200081D RID: 2077
	public class MessageWithPidList : Message<PidList>
	{
		// Token: 0x0600366F RID: 13935 RVA: 0x0010B90D File Offset: 0x00109D0D
		public MessageWithPidList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x0010B916 File Offset: 0x00109D16
		public override PidList GetPidList()
		{
			return base.Data;
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x0010B920 File Offset: 0x00109D20
		protected override PidList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetPidArray(obj);
			return new PidList(a);
		}
	}
}
