using System;

namespace Oculus.Platform
{
	// Token: 0x020008A8 RID: 2216
	public class UserOptions
	{
		// Token: 0x060037DA RID: 14298 RVA: 0x0010EBFF File Offset: 0x0010CFFF
		public UserOptions()
		{
			this.Handle = CAPI.ovr_UserOptions_Create();
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x0010EC12 File Offset: 0x0010D012
		public void SetMaxUsers(uint value)
		{
			CAPI.ovr_UserOptions_SetMaxUsers(this.Handle, value);
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x0010EC20 File Offset: 0x0010D020
		public void SetTimeWindow(TimeWindow value)
		{
			CAPI.ovr_UserOptions_SetTimeWindow(this.Handle, value);
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x0010EC2E File Offset: 0x0010D02E
		public static explicit operator IntPtr(UserOptions options)
		{
			return (options == null) ? IntPtr.Zero : options.Handle;
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x0010EC48 File Offset: 0x0010D048
		~UserOptions()
		{
			CAPI.ovr_UserOptions_Destroy(this.Handle);
		}

		// Token: 0x04002909 RID: 10505
		private IntPtr Handle;
	}
}
