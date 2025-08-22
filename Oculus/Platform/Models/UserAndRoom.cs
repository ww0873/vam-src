using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000875 RID: 2165
	public class UserAndRoom
	{
		// Token: 0x06003720 RID: 14112 RVA: 0x0010D240 File Offset: 0x0010B640
		public UserAndRoom(IntPtr o)
		{
			IntPtr intPtr = CAPI.ovr_UserAndRoom_GetRoom(o);
			this.Room = new Room(intPtr);
			if (intPtr == IntPtr.Zero)
			{
				this.RoomOptional = null;
			}
			else
			{
				this.RoomOptional = this.Room;
			}
			this.User = new User(CAPI.ovr_UserAndRoom_GetUser(o));
		}

		// Token: 0x04002889 RID: 10377
		public readonly Room RoomOptional;

		// Token: 0x0400288A RID: 10378
		[Obsolete("Deprecated in favor of RoomOptional")]
		public readonly Room Room;

		// Token: 0x0400288B RID: 10379
		public readonly User User;
	}
}
