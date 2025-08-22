using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x020007E3 RID: 2019
	public static class Callback
	{
		// Token: 0x06003308 RID: 13064 RVA: 0x00109068 File Offset: 0x00107468
		internal static void SetNotificationCallback<T>(Message.MessageType type, Message<T>.Callback callback)
		{
			if (callback == null)
			{
				throw new Exception("Cannot provide a null notification callback.");
			}
			Callback.notificationCallbacks[type] = new Callback.RequestCallback<T>(callback);
			if (type == Message.MessageType.Notification_Room_InviteAccepted)
			{
				Callback.FlushRoomInviteNotificationQueue();
			}
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x0010909C File Offset: 0x0010749C
		internal static void SetNotificationCallback(Message.MessageType type, Message.Callback callback)
		{
			if (callback == null)
			{
				throw new Exception("Cannot provide a null notification callback.");
			}
			Callback.notificationCallbacks[type] = new Callback.RequestCallback(callback);
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x001090C0 File Offset: 0x001074C0
		internal static void OnComplete<T>(Request<T> request, Message<T>.Callback callback)
		{
			Callback.requestIDsToCallbacks[request.RequestID] = new Callback.RequestCallback<T>(callback);
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x001090D8 File Offset: 0x001074D8
		internal static void OnComplete(Request request, Message.Callback callback)
		{
			Callback.requestIDsToCallbacks[request.RequestID] = new Callback.RequestCallback(callback);
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x001090F0 File Offset: 0x001074F0
		internal static void RunCallbacks()
		{
			for (;;)
			{
				Message message = Message.PopMessage();
				if (message == null)
				{
					break;
				}
				Callback.HandleMessage(message);
			}
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x0010911C File Offset: 0x0010751C
		internal static void RunLimitedCallbacks(uint limit)
		{
			int num = 0;
			while ((long)num < (long)((ulong)limit))
			{
				Message message = Message.PopMessage();
				if (message == null)
				{
					break;
				}
				Callback.HandleMessage(message);
				num++;
			}
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00109154 File Offset: 0x00107554
		private static void FlushRoomInviteNotificationQueue()
		{
			Callback.hasRegisteredRoomInviteNotificationHandler = true;
			foreach (Message msg in Callback.pendingRoomInviteNotifications)
			{
				Callback.HandleMessage(msg);
			}
			Callback.pendingRoomInviteNotifications.Clear();
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x001091C0 File Offset: 0x001075C0
		private static void HandleMessage(Message msg)
		{
			Callback.RequestCallback requestCallback;
			if (Callback.requestIDsToCallbacks.TryGetValue(msg.RequestID, out requestCallback))
			{
				try
				{
					requestCallback.HandleMessage(msg);
				}
				finally
				{
					Callback.requestIDsToCallbacks.Remove(msg.RequestID);
				}
			}
			else if (Callback.notificationCallbacks.TryGetValue(msg.Type, out requestCallback))
			{
				requestCallback.HandleMessage(msg);
			}
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00109238 File Offset: 0x00107638
		// Note: this type is marked as 'beforefieldinit'.
		static Callback()
		{
		}

		// Token: 0x0400270A RID: 9994
		private static Dictionary<ulong, Callback.RequestCallback> requestIDsToCallbacks = new Dictionary<ulong, Callback.RequestCallback>();

		// Token: 0x0400270B RID: 9995
		private static Dictionary<Message.MessageType, Callback.RequestCallback> notificationCallbacks = new Dictionary<Message.MessageType, Callback.RequestCallback>();

		// Token: 0x0400270C RID: 9996
		private static bool hasRegisteredRoomInviteNotificationHandler = false;

		// Token: 0x0400270D RID: 9997
		private static List<Message> pendingRoomInviteNotifications = new List<Message>();

		// Token: 0x020007E4 RID: 2020
		private class RequestCallback
		{
			// Token: 0x06003311 RID: 13073 RVA: 0x0010925E File Offset: 0x0010765E
			public RequestCallback()
			{
			}

			// Token: 0x06003312 RID: 13074 RVA: 0x00109266 File Offset: 0x00107666
			public RequestCallback(Message.Callback callback)
			{
				this.messageCallback = callback;
			}

			// Token: 0x06003313 RID: 13075 RVA: 0x00109275 File Offset: 0x00107675
			public virtual void HandleMessage(Message msg)
			{
				if (this.messageCallback != null)
				{
					this.messageCallback(msg);
				}
			}

			// Token: 0x0400270E RID: 9998
			private Message.Callback messageCallback;
		}

		// Token: 0x020007E5 RID: 2021
		private sealed class RequestCallback<T> : Callback.RequestCallback
		{
			// Token: 0x06003314 RID: 13076 RVA: 0x0010928E File Offset: 0x0010768E
			public RequestCallback(Message<T>.Callback callback)
			{
				this.callback = callback;
			}

			// Token: 0x06003315 RID: 13077 RVA: 0x001092A0 File Offset: 0x001076A0
			public override void HandleMessage(Message msg)
			{
				if (this.callback != null)
				{
					if (!Callback.hasRegisteredRoomInviteNotificationHandler && msg.Type == Message.MessageType.Notification_Room_InviteAccepted)
					{
						Callback.pendingRoomInviteNotifications.Add(msg);
						return;
					}
					if (msg is Message<T>)
					{
						this.callback((Message<T>)msg);
					}
					else
					{
						Debug.LogError("Unable to handle message: " + msg.GetType());
					}
				}
			}

			// Token: 0x0400270F RID: 9999
			private Message<T>.Callback callback;
		}
	}
}
