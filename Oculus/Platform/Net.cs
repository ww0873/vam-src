using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000884 RID: 2180
	public static class Net
	{
		// Token: 0x06003765 RID: 14181 RVA: 0x0010DE50 File Offset: 0x0010C250
		public static Packet ReadPacket()
		{
			if (!Core.IsInitialized())
			{
				return null;
			}
			IntPtr intPtr = CAPI.ovr_Net_ReadPacket();
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			return new Packet(intPtr);
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x0010DE87 File Offset: 0x0010C287
		public static bool SendPacket(ulong userID, byte[] bytes, SendPolicy policy)
		{
			return Core.IsInitialized() && CAPI.ovr_Net_SendPacket(userID, (UIntPtr)((ulong)((long)bytes.Length)), bytes, policy);
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x0010DEA6 File Offset: 0x0010C2A6
		public static void Connect(ulong userID)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Net_Connect(userID);
			}
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x0010DEB8 File Offset: 0x0010C2B8
		public static void Accept(ulong userID)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Net_Accept(userID);
			}
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x0010DECA File Offset: 0x0010C2CA
		public static void Close(ulong userID)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Net_Close(userID);
			}
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x0010DEDC File Offset: 0x0010C2DC
		public static bool IsConnected(ulong userID)
		{
			return Core.IsInitialized() && CAPI.ovr_Net_IsConnected(userID);
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x0010DEF1 File Offset: 0x0010C2F1
		public static bool SendPacketToCurrentRoom(byte[] bytes, SendPolicy policy)
		{
			return Core.IsInitialized() && CAPI.ovr_Net_SendPacketToCurrentRoom((UIntPtr)((ulong)((long)bytes.Length)), bytes, policy);
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x0010DF0F File Offset: 0x0010C30F
		public static bool AcceptForCurrentRoom()
		{
			return Core.IsInitialized() && CAPI.ovr_Net_AcceptForCurrentRoom();
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x0010DF22 File Offset: 0x0010C322
		public static void CloseForCurrentRoom()
		{
			if (Core.IsInitialized())
			{
				CAPI.ovr_Net_CloseForCurrentRoom();
			}
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x0010DF33 File Offset: 0x0010C333
		public static Request<PingResult> Ping(ulong userID)
		{
			if (Core.IsInitialized())
			{
				return new Request<PingResult>(CAPI.ovr_Net_Ping(userID));
			}
			return null;
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x0010DF4C File Offset: 0x0010C34C
		public static void SetPeerConnectRequestCallback(Message<NetworkingPeer>.Callback callback)
		{
			Callback.SetNotificationCallback<NetworkingPeer>(Message.MessageType.Notification_Networking_PeerConnectRequest, callback);
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x0010DF59 File Offset: 0x0010C359
		public static void SetConnectionStateChangedCallback(Message<NetworkingPeer>.Callback callback)
		{
			Callback.SetNotificationCallback<NetworkingPeer>(Message.MessageType.Notification_Networking_ConnectionStateChange, callback);
		}
	}
}
