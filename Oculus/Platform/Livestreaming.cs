using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000880 RID: 2176
	public static class Livestreaming
	{
		// Token: 0x0600374C RID: 14156 RVA: 0x0010D941 File Offset: 0x0010BD41
		public static void SetStatusUpdateNotificationCallback(Message<LivestreamingStatus>.Callback callback)
		{
			Callback.SetNotificationCallback<LivestreamingStatus>(Message.MessageType.Notification_Livestreaming_StatusChange, callback);
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x0010D94E File Offset: 0x0010BD4E
		public static Request<LivestreamingStatus> GetStatus()
		{
			if (Core.IsInitialized())
			{
				return new Request<LivestreamingStatus>(CAPI.ovr_Livestreaming_GetStatus());
			}
			return null;
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x0010D966 File Offset: 0x0010BD66
		public static Request<LivestreamingStatus> PauseStream()
		{
			if (Core.IsInitialized())
			{
				return new Request<LivestreamingStatus>(CAPI.ovr_Livestreaming_PauseStream());
			}
			return null;
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x0010D97E File Offset: 0x0010BD7E
		public static Request<LivestreamingStatus> ResumeStream()
		{
			if (Core.IsInitialized())
			{
				return new Request<LivestreamingStatus>(CAPI.ovr_Livestreaming_ResumeStream());
			}
			return null;
		}
	}
}
