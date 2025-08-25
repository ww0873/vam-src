using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006B5 RID: 1717
	public static class Pool<T> where T : new()
	{
		// Token: 0x06002955 RID: 10581 RVA: 0x000E1254 File Offset: 0x000DF654
		public static T Spawn()
		{
			if (Pool<T>._pool == null)
			{
				Pool<T>._pool = new Stack<T>();
			}
			T t;
			if (Pool<T>._pool.Count > 0)
			{
				t = Pool<T>._pool.Pop();
			}
			else
			{
				t = Activator.CreateInstance<T>();
			}
			if (t is IPoolable)
			{
				(t as IPoolable).OnSpawn();
			}
			return t;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x000E12BC File Offset: 0x000DF6BC
		public static void Recycle(T t)
		{
			if (t == null)
			{
				Debug.LogError("Cannot recycle a null object.");
				return;
			}
			if (t is IPoolable)
			{
				(t as IPoolable).OnRecycle();
			}
			Pool<T>._pool.Push(t);
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x000E130A File Offset: 0x000DF70A
		// Note: this type is marked as 'beforefieldinit'.
		static Pool()
		{
		}

		// Token: 0x040021EB RID: 8683
		[ThreadStatic]
		private static Stack<T> _pool = new Stack<T>();
	}
}
