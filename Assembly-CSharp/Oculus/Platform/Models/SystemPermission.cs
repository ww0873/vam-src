using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000871 RID: 2161
	public class SystemPermission
	{
		// Token: 0x0600371C RID: 14108 RVA: 0x0010D12E File Offset: 0x0010B52E
		public SystemPermission(IntPtr o)
		{
			this.HasPermission = CAPI.ovr_SystemPermission_GetHasPermission(o);
			this.PermissionGrantStatus = CAPI.ovr_SystemPermission_GetPermissionGrantStatus(o);
		}

		// Token: 0x0400287E RID: 10366
		public readonly bool HasPermission;

		// Token: 0x0400287F RID: 10367
		public readonly PermissionGrantStatus PermissionGrantStatus;
	}
}
