using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200085F RID: 2143
	public class OrgScopedID
	{
		// Token: 0x06003706 RID: 14086 RVA: 0x0010CAC8 File Offset: 0x0010AEC8
		public OrgScopedID(IntPtr o)
		{
			this.ID = CAPI.ovr_OrgScopedID_GetID(o);
		}

		// Token: 0x0400284E RID: 10318
		public readonly ulong ID;
	}
}
