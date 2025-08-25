using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x020007E0 RID: 2016
	public class AndroidPlatform
	{
		// Token: 0x060032FD RID: 13053 RVA: 0x00108D8D File Offset: 0x0010718D
		public AndroidPlatform()
		{
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x00108D95 File Offset: 0x00107195
		public bool Initialize(string appId)
		{
			return false;
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x00108D98 File Offset: 0x00107198
		public Request<PlatformInitialize> AsyncInitialize(string appId)
		{
			return new Request<PlatformInitialize>(0UL);
		}
	}
}
