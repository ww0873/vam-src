using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000886 RID: 2182
	public static class Voip
	{
		// Token: 0x06003776 RID: 14198 RVA: 0x0010E00E File Offset: 0x0010C40E
		public static void Start(ulong userID)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Voip_Start(userID);
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x0010E020 File Offset: 0x0010C420
		public static void Accept(ulong userID)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Voip_Accept(userID);
			}
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x0010E032 File Offset: 0x0010C432
		public static void Stop(ulong userID)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Voip_Stop(userID);
			}
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x0010E044 File Offset: 0x0010C444
		public static void SetVoipConnectRequestCallback(Message<NetworkingPeer>.Callback callback)
		{
			if (Core.IsInitialized())
			{
				Callback.SetNotificationCallback<NetworkingPeer>(Message.MessageType.Notification_Voip_ConnectRequest, callback);
			}
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x0010E05B File Offset: 0x0010C45B
		public static void SetVoipStateChangeCallback(Message<NetworkingPeer>.Callback callback)
		{
			if (Core.IsInitialized())
			{
				Callback.SetNotificationCallback<NetworkingPeer>(Message.MessageType.Notification_Voip_StateChange, callback);
			}
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x0010E072 File Offset: 0x0010C472
		public static void SetMicrophoneFilterCallback(CAPI.FilterCallback callback)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Voip_SetMicrophoneFilterCallbackWithFixedSizeBuffer(callback, (UIntPtr)480UL);
			}
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x0010E08F File Offset: 0x0010C48F
		public static void SetMicrophoneMuted(VoipMuteState state)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Voip_SetMicrophoneMuted(state);
			}
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x0010E0A1 File Offset: 0x0010C4A1
		public static VoipMuteState GetSystemVoipMicrophoneMuted()
		{
			if (Core.IsInitialized())
			{
				return CAPI.ovr_Voip_GetSystemVoipMicrophoneMuted();
			}
			return VoipMuteState.Unknown;
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x0010E0B4 File Offset: 0x0010C4B4
		public static SystemVoipStatus GetSystemVoipStatus()
		{
			if (Core.IsInitialized())
			{
				return CAPI.ovr_Voip_GetSystemVoipStatus();
			}
			return SystemVoipStatus.Unknown;
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x0010E0C7 File Offset: 0x0010C4C7
		public static void SetSystemVoipStateNotificationCallback(Message<SystemVoipState>.Callback callback)
		{
			if (Core.IsInitialized())
			{
				Callback.SetNotificationCallback<SystemVoipState>(Message.MessageType.Notification_Voip_SystemVoipState, callback);
			}
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x0010E0DE File Offset: 0x0010C4DE
		public static Request<SystemVoipState> SetSystemVoipSuppressed(bool suppressed)
		{
			if (Core.IsInitialized())
			{
				return new Request<SystemVoipState>(CAPI.ovr_Voip_SetSystemVoipSuppressed(suppressed));
			}
			return null;
		}
	}
}
