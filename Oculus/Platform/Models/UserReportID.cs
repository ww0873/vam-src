using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000878 RID: 2168
	public class UserReportID
	{
		// Token: 0x06003723 RID: 14115 RVA: 0x0010D31A File Offset: 0x0010B71A
		public UserReportID(IntPtr o)
		{
			this.ID = CAPI.ovr_UserReportID_GetID(o);
		}

		// Token: 0x0400288D RID: 10381
		public readonly ulong ID;
	}
}
