using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200084E RID: 2126
	public class LaunchDetails
	{
		// Token: 0x060036F1 RID: 14065 RVA: 0x0010C5EC File Offset: 0x0010A9EC
		public LaunchDetails(IntPtr o)
		{
			this.DeeplinkMessage = CAPI.ovr_LaunchDetails_GetDeeplinkMessage(o);
			this.LaunchSource = CAPI.ovr_LaunchDetails_GetLaunchSource(o);
			this.LaunchType = CAPI.ovr_LaunchDetails_GetLaunchType(o);
			this.RoomID = CAPI.ovr_LaunchDetails_GetRoomID(o);
			IntPtr intPtr = CAPI.ovr_LaunchDetails_GetUsers(o);
			this.Users = new UserList(intPtr);
			if (intPtr == IntPtr.Zero)
			{
				this.UsersOptional = null;
			}
			else
			{
				this.UsersOptional = this.Users;
			}
		}

		// Token: 0x0400281C RID: 10268
		public readonly string DeeplinkMessage;

		// Token: 0x0400281D RID: 10269
		public readonly string LaunchSource;

		// Token: 0x0400281E RID: 10270
		public readonly LaunchType LaunchType;

		// Token: 0x0400281F RID: 10271
		public readonly ulong RoomID;

		// Token: 0x04002820 RID: 10272
		public readonly UserList UsersOptional;

		// Token: 0x04002821 RID: 10273
		[Obsolete("Deprecated in favor of UsersOptional")]
		public readonly UserList Users;
	}
}
