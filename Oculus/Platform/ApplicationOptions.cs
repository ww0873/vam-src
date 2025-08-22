using System;

namespace Oculus.Platform
{
	// Token: 0x020007E1 RID: 2017
	public class ApplicationOptions
	{
		// Token: 0x06003300 RID: 13056 RVA: 0x00108DA1 File Offset: 0x001071A1
		public ApplicationOptions()
		{
			this.Handle = CAPI.ovr_ApplicationOptions_Create();
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x00108DB4 File Offset: 0x001071B4
		public void SetDeeplinkMessage(string value)
		{
			CAPI.ovr_ApplicationOptions_SetDeeplinkMessage(this.Handle, value);
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x00108DC2 File Offset: 0x001071C2
		public static explicit operator IntPtr(ApplicationOptions options)
		{
			return (options == null) ? IntPtr.Zero : options.Handle;
		}

		// Token: 0x06003303 RID: 13059 RVA: 0x00108DDC File Offset: 0x001071DC
		~ApplicationOptions()
		{
			CAPI.ovr_ApplicationOptions_Destroy(this.Handle);
		}

		// Token: 0x040026FF RID: 9983
		private IntPtr Handle;
	}
}
