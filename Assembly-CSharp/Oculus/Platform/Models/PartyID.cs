using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000861 RID: 2145
	public class PartyID
	{
		// Token: 0x06003708 RID: 14088 RVA: 0x0010CBE7 File Offset: 0x0010AFE7
		public PartyID(IntPtr o)
		{
			this.ID = CAPI.ovr_PartyID_GetID(o);
		}

		// Token: 0x04002858 RID: 10328
		public readonly ulong ID;
	}
}
