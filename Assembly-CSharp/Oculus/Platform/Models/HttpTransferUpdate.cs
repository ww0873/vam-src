using System;
using System.Runtime.InteropServices;

namespace Oculus.Platform.Models
{
	// Token: 0x0200084B RID: 2123
	public class HttpTransferUpdate
	{
		// Token: 0x060036EE RID: 14062 RVA: 0x0010C4E8 File Offset: 0x0010A8E8
		public HttpTransferUpdate(IntPtr o)
		{
			this.ID = CAPI.ovr_HttpTransferUpdate_GetID(o);
			this.IsCompleted = CAPI.ovr_HttpTransferUpdate_IsCompleted(o);
			long num = (long)((ulong)CAPI.ovr_HttpTransferUpdate_GetSize(o));
			this.Payload = new byte[num];
			Marshal.Copy(CAPI.ovr_Packet_GetBytes(o), this.Payload, 0, (int)num);
		}

		// Token: 0x04002814 RID: 10260
		public readonly ulong ID;

		// Token: 0x04002815 RID: 10261
		public readonly byte[] Payload;

		// Token: 0x04002816 RID: 10262
		public readonly bool IsCompleted;
	}
}
