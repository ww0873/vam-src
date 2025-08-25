using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200082B RID: 2091
	public class MessageWithSystemVoipState : Message<SystemVoipState>
	{
		// Token: 0x06003699 RID: 13977 RVA: 0x0010BBC9 File Offset: 0x00109FC9
		public MessageWithSystemVoipState(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x0010BBD2 File Offset: 0x00109FD2
		public override SystemVoipState GetSystemVoipState()
		{
			return base.Data;
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x0010BBDC File Offset: 0x00109FDC
		protected override SystemVoipState GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetSystemVoipState(obj);
			return new SystemVoipState(o);
		}
	}
}
