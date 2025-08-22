using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace LeapInternal
{
	// Token: 0x02000623 RID: 1571
	public static class MemoryManager
	{
		// Token: 0x060026A6 RID: 9894 RVA: 0x000D8EB8 File Offset: 0x000D72B8
		[MonoPInvokeCallback(typeof(Allocate))]
		public static IntPtr Pin(uint size, eLeapAllocatorType typeHint, IntPtr state)
		{
			try
			{
				MemoryManager.PoolKey key = new MemoryManager.PoolKey
				{
					type = typeHint,
					size = size
				};
				Queue<object> queue;
				if (!MemoryManager._pooledMemory.TryGetValue(key, out queue))
				{
					queue = new Queue<object>();
					MemoryManager._pooledMemory[key] = queue;
				}
				object value;
				if (MemoryManager.EnablePooling && (long)queue.Count > (long)((ulong)MemoryManager.MinPoolSize))
				{
					value = queue.Dequeue();
				}
				else if (typeHint == eLeapAllocatorType.eLeapAllocatorType_Uint8 || typeHint != eLeapAllocatorType.eLeapAllocatorType_Float)
				{
					value = new byte[size];
				}
				else
				{
					value = new float[(size + 4U - 1U) / 4U];
				}
				GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
				IntPtr intPtr = handle.AddrOfPinnedObject();
				MemoryManager._activeMemory.Add(intPtr, new MemoryManager.ActiveMemoryInfo
				{
					handle = handle,
					key = key
				});
				return intPtr;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000D8FC8 File Offset: 0x000D73C8
		[MonoPInvokeCallback(typeof(Deallocate))]
		public static void Unpin(IntPtr ptr, IntPtr state)
		{
			try
			{
				MemoryManager.ActiveMemoryInfo activeMemoryInfo = MemoryManager._activeMemory[ptr];
				MemoryManager._pooledMemory[activeMemoryInfo.key].Enqueue(activeMemoryInfo.handle.Target);
				MemoryManager._activeMemory.Remove(ptr);
				activeMemoryInfo.handle.Free();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000D903C File Offset: 0x000D743C
		public static object GetPinnedObject(IntPtr ptr)
		{
			try
			{
				return MemoryManager._activeMemory[ptr].handle.Target;
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000D9080 File Offset: 0x000D7480
		// Note: this type is marked as 'beforefieldinit'.
		static MemoryManager()
		{
		}

		// Token: 0x040020C4 RID: 8388
		public static bool EnablePooling = false;

		// Token: 0x040020C5 RID: 8389
		public static uint MinPoolSize = 8U;

		// Token: 0x040020C6 RID: 8390
		private static Dictionary<IntPtr, MemoryManager.ActiveMemoryInfo> _activeMemory = new Dictionary<IntPtr, MemoryManager.ActiveMemoryInfo>();

		// Token: 0x040020C7 RID: 8391
		private static Dictionary<MemoryManager.PoolKey, Queue<object>> _pooledMemory = new Dictionary<MemoryManager.PoolKey, Queue<object>>();

		// Token: 0x02000624 RID: 1572
		private struct PoolKey : IEquatable<MemoryManager.PoolKey>
		{
			// Token: 0x060026AA RID: 9898 RVA: 0x000D90A2 File Offset: 0x000D74A2
			public override int GetHashCode()
			{
				return (int)(this.type | (eLeapAllocatorType)((int)this.size << 4));
			}

			// Token: 0x060026AB RID: 9899 RVA: 0x000D90B3 File Offset: 0x000D74B3
			public bool Equals(MemoryManager.PoolKey other)
			{
				return this.type == other.type && this.size == other.size;
			}

			// Token: 0x060026AC RID: 9900 RVA: 0x000D90D9 File Offset: 0x000D74D9
			public override bool Equals(object obj)
			{
				return obj is MemoryManager.PoolKey && this.Equals((MemoryManager.PoolKey)obj);
			}

			// Token: 0x040020C8 RID: 8392
			public eLeapAllocatorType type;

			// Token: 0x040020C9 RID: 8393
			public uint size;
		}

		// Token: 0x02000625 RID: 1573
		private struct ActiveMemoryInfo
		{
			// Token: 0x040020CA RID: 8394
			public GCHandle handle;

			// Token: 0x040020CB RID: 8395
			public MemoryManager.PoolKey key;
		}
	}
}
