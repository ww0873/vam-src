using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000862 RID: 2146
	public class Pid
	{
		// Token: 0x06003709 RID: 14089 RVA: 0x0010CBFB File Offset: 0x0010AFFB
		public Pid(IntPtr o)
		{
			this.Id = CAPI.ovr_Pid_GetId(o);
		}

		// Token: 0x04002859 RID: 10329
		public readonly string Id;
	}
}
