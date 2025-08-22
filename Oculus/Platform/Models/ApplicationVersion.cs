using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200083F RID: 2111
	public class ApplicationVersion
	{
		// Token: 0x060036CE RID: 14030 RVA: 0x0010C292 File Offset: 0x0010A692
		public ApplicationVersion(IntPtr o)
		{
			this.CurrentCode = CAPI.ovr_ApplicationVersion_GetCurrentCode(o);
			this.CurrentName = CAPI.ovr_ApplicationVersion_GetCurrentName(o);
			this.LatestCode = CAPI.ovr_ApplicationVersion_GetLatestCode(o);
			this.LatestName = CAPI.ovr_ApplicationVersion_GetLatestName(o);
		}

		// Token: 0x040027EF RID: 10223
		public readonly int CurrentCode;

		// Token: 0x040027F0 RID: 10224
		public readonly string CurrentName;

		// Token: 0x040027F1 RID: 10225
		public readonly int LatestCode;

		// Token: 0x040027F2 RID: 10226
		public readonly string LatestName;
	}
}
