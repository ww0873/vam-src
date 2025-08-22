using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
	// Token: 0x020009B6 RID: 2486
	public static class ComputeUtils
	{
		// Token: 0x06003EEB RID: 16107 RVA: 0x0012E294 File Offset: 0x0012C694
		public static ComputeBuffer ToComputeBuffer<T>(this T[] array, int stride, ComputeBufferType type = ComputeBufferType.Default)
		{
			ComputeBuffer computeBuffer = new ComputeBuffer(array.Length, stride, type);
			computeBuffer.SetData(array);
			return computeBuffer;
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x0012E2B4 File Offset: 0x0012C6B4
		public static T[] ToArray<T>(this ComputeBuffer buffer)
		{
			T[] array = new T[buffer.count];
			buffer.GetData(array);
			return array;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x0012E2D8 File Offset: 0x0012C6D8
		public static void LogBuffer<T>(ComputeBuffer buffer)
		{
			T[] array = new T[buffer.count];
			buffer.GetData(array);
			for (int i = 0; i < array.Length; i++)
			{
				Debug.Log(string.Format("i:{0} val:{1}", i, array[i]));
			}
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x0012E330 File Offset: 0x0012C730
		public static void LogLargeBuffer<T>(ComputeBuffer buffer)
		{
			T[] array = new T[buffer.count];
			buffer.GetData(array);
			string text = string.Empty;
			for (int i = 1; i <= array.Length; i++)
			{
				text = text + "|" + array[i - 1];
				if (i % 12 == 0)
				{
					Debug.Log(string.Format("from i:{0} values:{1}", i, text));
					text = string.Empty;
				}
			}
		}
	}
}
