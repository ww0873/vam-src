using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
internal static class OVRMessenger
{
	// Token: 0x06003290 RID: 12944 RVA: 0x0010745A File Offset: 0x0010585A
	public static void MarkAsPermanent(string eventType)
	{
		OVRMessenger.permanentMessages.Add(eventType);
	}

	// Token: 0x06003291 RID: 12945 RVA: 0x00107468 File Offset: 0x00105868
	public static void Cleanup()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, Delegate> keyValuePair in OVRMessenger.eventTable)
		{
			bool flag = false;
			foreach (string b in OVRMessenger.permanentMessages)
			{
				if (keyValuePair.Key == b)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string key in list)
		{
			OVRMessenger.eventTable.Remove(key);
		}
	}

	// Token: 0x06003292 RID: 12946 RVA: 0x00107584 File Offset: 0x00105984
	public static void PrintEventTable()
	{
		Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");
		foreach (KeyValuePair<string, Delegate> keyValuePair in OVRMessenger.eventTable)
		{
			Debug.Log(string.Concat(new object[]
			{
				"\t\t\t",
				keyValuePair.Key,
				"\t\t",
				keyValuePair.Value
			}));
		}
		Debug.Log("\n");
	}

	// Token: 0x06003293 RID: 12947 RVA: 0x00107620 File Offset: 0x00105A20
	public static void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
	{
		if (!OVRMessenger.eventTable.ContainsKey(eventType))
		{
			OVRMessenger.eventTable.Add(eventType, null);
		}
		Delegate @delegate = OVRMessenger.eventTable[eventType];
		if (@delegate != null && @delegate.GetType() != listenerBeingAdded.GetType())
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, @delegate.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x00107694 File Offset: 0x00105A94
	public static void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
	{
		if (!OVRMessenger.eventTable.ContainsKey(eventType))
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
		}
		Delegate @delegate = OVRMessenger.eventTable[eventType];
		if (@delegate == null)
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
		}
		if (@delegate.GetType() != listenerBeingRemoved.GetType())
		{
			throw new OVRMessenger.ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, @delegate.GetType().Name, listenerBeingRemoved.GetType().Name));
		}
	}

	// Token: 0x06003295 RID: 12949 RVA: 0x00107722 File Offset: 0x00105B22
	public static void OnListenerRemoved(string eventType)
	{
		if (OVRMessenger.eventTable[eventType] == null)
		{
			OVRMessenger.eventTable.Remove(eventType);
		}
	}

	// Token: 0x06003296 RID: 12950 RVA: 0x00107740 File Offset: 0x00105B40
	public static void OnBroadcasting(string eventType)
	{
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x00107742 File Offset: 0x00105B42
	public static OVRMessenger.BroadcastException CreateBroadcastSignatureException(string eventType)
	{
		return new OVRMessenger.BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x00107754 File Offset: 0x00105B54
	public static void AddListener(string eventType, OVRCallback handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback)Delegate.Combine((OVRCallback)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x00107783 File Offset: 0x00105B83
	public static void AddListener<T>(string eventType, OVRCallback<T> handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T>)Delegate.Combine((OVRCallback<T>)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x001077B2 File Offset: 0x00105BB2
	public static void AddListener<T, U>(string eventType, OVRCallback<T, U> handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U>)Delegate.Combine((OVRCallback<T, U>)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x0600329B RID: 12955 RVA: 0x001077E1 File Offset: 0x00105BE1
	public static void AddListener<T, U, V>(string eventType, OVRCallback<T, U, V> handler)
	{
		OVRMessenger.OnListenerAdding(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U, V>)Delegate.Combine((OVRCallback<T, U, V>)OVRMessenger.eventTable[eventType], handler);
	}

	// Token: 0x0600329C RID: 12956 RVA: 0x00107810 File Offset: 0x00105C10
	public static void RemoveListener(string eventType, OVRCallback handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback)Delegate.Remove((OVRCallback)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x0600329D RID: 12957 RVA: 0x00107845 File Offset: 0x00105C45
	public static void RemoveListener<T>(string eventType, OVRCallback<T> handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T>)Delegate.Remove((OVRCallback<T>)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x0010787A File Offset: 0x00105C7A
	public static void RemoveListener<T, U>(string eventType, OVRCallback<T, U> handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U>)Delegate.Remove((OVRCallback<T, U>)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x001078AF File Offset: 0x00105CAF
	public static void RemoveListener<T, U, V>(string eventType, OVRCallback<T, U, V> handler)
	{
		OVRMessenger.OnListenerRemoving(eventType, handler);
		OVRMessenger.eventTable[eventType] = (OVRCallback<T, U, V>)Delegate.Remove((OVRCallback<T, U, V>)OVRMessenger.eventTable[eventType], handler);
		OVRMessenger.OnListenerRemoved(eventType);
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x001078E4 File Offset: 0x00105CE4
	public static void Broadcast(string eventType)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback ovrcallback = @delegate as OVRCallback;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback();
		}
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x00107928 File Offset: 0x00105D28
	public static void Broadcast<T>(string eventType, T arg1)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback<T> ovrcallback = @delegate as OVRCallback<T>;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback(arg1);
		}
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x00107970 File Offset: 0x00105D70
	public static void Broadcast<T, U>(string eventType, T arg1, U arg2)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback<T, U> ovrcallback = @delegate as OVRCallback<T, U>;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback(arg1, arg2);
		}
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x001079B8 File Offset: 0x00105DB8
	public static void Broadcast<T, U, V>(string eventType, T arg1, U arg2, V arg3)
	{
		OVRMessenger.OnBroadcasting(eventType);
		Delegate @delegate;
		if (OVRMessenger.eventTable.TryGetValue(eventType, out @delegate))
		{
			OVRCallback<T, U, V> ovrcallback = @delegate as OVRCallback<T, U, V>;
			if (ovrcallback == null)
			{
				throw OVRMessenger.CreateBroadcastSignatureException(eventType);
			}
			ovrcallback(arg1, arg2, arg3);
		}
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x001079FF File Offset: 0x00105DFF
	// Note: this type is marked as 'beforefieldinit'.
	static OVRMessenger()
	{
	}

	// Token: 0x04002696 RID: 9878
	private static MessengerHelper messengerHelper = new GameObject("MessengerHelper").AddComponent<MessengerHelper>();

	// Token: 0x04002697 RID: 9879
	public static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	// Token: 0x04002698 RID: 9880
	public static List<string> permanentMessages = new List<string>();

	// Token: 0x020007CA RID: 1994
	public class BroadcastException : Exception
	{
		// Token: 0x060032A5 RID: 12965 RVA: 0x00107A29 File Offset: 0x00105E29
		public BroadcastException(string msg) : base(msg)
		{
		}
	}

	// Token: 0x020007CB RID: 1995
	public class ListenerException : Exception
	{
		// Token: 0x060032A6 RID: 12966 RVA: 0x00107A32 File Offset: 0x00105E32
		public ListenerException(string msg) : base(msg)
		{
		}
	}
}
