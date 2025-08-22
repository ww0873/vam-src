using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000865 RID: 2149
	public class PlatformInitialize
	{
		// Token: 0x06003710 RID: 14096 RVA: 0x0010CCC5 File Offset: 0x0010B0C5
		public PlatformInitialize(IntPtr o)
		{
			this.Result = CAPI.ovr_PlatformInitialize_GetResult(o);
		}

		// Token: 0x0400285C RID: 10332
		public readonly PlatformInitializeResult Result;
	}
}
