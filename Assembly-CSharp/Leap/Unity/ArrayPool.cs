using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200071F RID: 1823
	public static class ArrayPool<T>
	{
		// Token: 0x06002C6C RID: 11372 RVA: 0x000EE314 File Offset: 0x000EC714
		static ArrayPool()
		{
			ArrayPool<T>._bins[0] = new Stack<T[]>();
			for (int i = 0; i < 32; i++)
			{
				ArrayPool<T>._bins[1 << i] = new Stack<T[]>();
			}
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x000EE364 File Offset: 0x000EC764
		public static T[] Spawn(int minLength)
		{
			int num = Mathf.NextPowerOfTwo(minLength);
			Stack<T[]> stack = ArrayPool<T>._bins[num];
			if (stack.Count > 0)
			{
				return stack.Pop();
			}
			return new T[num];
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000EE3A0 File Offset: 0x000EC7A0
		public static void Recycle(T[] array)
		{
			Array.Clear(array, 0, array.Length);
			int key = Mathf.NextPowerOfTwo(array.Length + 1) / 2;
			ArrayPool<T>._bins[key].Push(array);
		}

		// Token: 0x04002376 RID: 9078
		private static Dictionary<int, Stack<T[]>> _bins = new Dictionary<int, Stack<T[]>>();
	}
}
