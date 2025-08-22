using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000873 RID: 2163
	public class User
	{
		// Token: 0x0600371E RID: 14110 RVA: 0x0010D170 File Offset: 0x0010B570
		public User(IntPtr o)
		{
			this.ID = CAPI.ovr_User_GetID(o);
			this.ImageURL = CAPI.ovr_User_GetImageUrl(o);
			this.InviteToken = CAPI.ovr_User_GetInviteToken(o);
			this.OculusID = CAPI.ovr_User_GetOculusID(o);
			this.Presence = CAPI.ovr_User_GetPresence(o);
			this.PresenceStatus = CAPI.ovr_User_GetPresenceStatus(o);
			this.SmallImageUrl = CAPI.ovr_User_GetSmallImageUrl(o);
		}

		// Token: 0x04002882 RID: 10370
		public readonly ulong ID;

		// Token: 0x04002883 RID: 10371
		public readonly string ImageURL;

		// Token: 0x04002884 RID: 10372
		public readonly string InviteToken;

		// Token: 0x04002885 RID: 10373
		public readonly string OculusID;

		// Token: 0x04002886 RID: 10374
		public readonly string Presence;

		// Token: 0x04002887 RID: 10375
		public readonly UserPresenceStatus PresenceStatus;

		// Token: 0x04002888 RID: 10376
		public readonly string SmallImageUrl;
	}
}
