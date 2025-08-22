using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000877 RID: 2167
	public class UserProof
	{
		// Token: 0x06003722 RID: 14114 RVA: 0x0010D306 File Offset: 0x0010B706
		public UserProof(IntPtr o)
		{
			this.Value = CAPI.ovr_UserProof_GetNonce(o);
		}

		// Token: 0x0400288C RID: 10380
		public readonly string Value;
	}
}
