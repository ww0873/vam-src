using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000859 RID: 2137
	public class MatchmakingEnqueuedUser
	{
		// Token: 0x060036FC RID: 14076 RVA: 0x0010C8DC File Offset: 0x0010ACDC
		public MatchmakingEnqueuedUser(IntPtr o)
		{
			this.CustomData = CAPI.DataStoreFromNative(CAPI.ovr_MatchmakingEnqueuedUser_GetCustomData(o));
			IntPtr intPtr = CAPI.ovr_MatchmakingEnqueuedUser_GetUser(o);
			this.User = new User(intPtr);
			if (intPtr == IntPtr.Zero)
			{
				this.UserOptional = null;
			}
			else
			{
				this.UserOptional = this.User;
			}
		}

		// Token: 0x0400283B RID: 10299
		public readonly Dictionary<string, string> CustomData;

		// Token: 0x0400283C RID: 10300
		public readonly User UserOptional;

		// Token: 0x0400283D RID: 10301
		[Obsolete("Deprecated in favor of UserOptional")]
		public readonly User User;
	}
}
