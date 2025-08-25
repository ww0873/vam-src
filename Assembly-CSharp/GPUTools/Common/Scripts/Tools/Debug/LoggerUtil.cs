using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Debug
{
	// Token: 0x020009C6 RID: 2502
	public class LoggerUtil
	{
		// Token: 0x06003F31 RID: 16177 RVA: 0x0012EB07 File Offset: 0x0012CF07
		public LoggerUtil()
		{
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x0012EB10 File Offset: 0x0012CF10
		public static void LogArray<T>(T[] list, int max)
		{
			for (int i = 0; i < Math.Min(list.Length, max); i++)
			{
				Debug.Log(list[i]);
			}
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0012EB48 File Offset: 0x0012CF48
		public static void LogList<T>(List<T> list, int max)
		{
			for (int i = 0; i < Math.Min(list.Count, max); i++)
			{
				Debug.Log(list[i]);
			}
		}
	}
}
