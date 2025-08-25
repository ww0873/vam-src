using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200084C RID: 2124
	public class InstalledApplication
	{
		// Token: 0x060036EF RID: 14063 RVA: 0x0010C540 File Offset: 0x0010A940
		public InstalledApplication(IntPtr o)
		{
			this.ApplicationId = CAPI.ovr_InstalledApplication_GetApplicationId(o);
			this.PackageName = CAPI.ovr_InstalledApplication_GetPackageName(o);
			this.Status = CAPI.ovr_InstalledApplication_GetStatus(o);
			this.VersionCode = CAPI.ovr_InstalledApplication_GetVersionCode(o);
			this.VersionName = CAPI.ovr_InstalledApplication_GetVersionName(o);
		}

		// Token: 0x04002817 RID: 10263
		public readonly string ApplicationId;

		// Token: 0x04002818 RID: 10264
		public readonly string PackageName;

		// Token: 0x04002819 RID: 10265
		public readonly string Status;

		// Token: 0x0400281A RID: 10266
		public readonly int VersionCode;

		// Token: 0x0400281B RID: 10267
		public readonly string VersionName;
	}
}
