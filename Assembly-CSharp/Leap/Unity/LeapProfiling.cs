using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Profiling;

namespace Leap.Unity
{
	// Token: 0x020006F9 RID: 1785
	public static class LeapProfiling
	{
		// Token: 0x06002B42 RID: 11074 RVA: 0x000E9694 File Offset: 0x000E7A94
		public static void Update()
		{
			if (LeapProfiling._samplersToCreateCount > 0)
			{
				object samplersToCreate = LeapProfiling._samplersToCreate;
				lock (samplersToCreate)
				{
					Dictionary<string, CustomSampler> dictionary = new Dictionary<string, CustomSampler>(LeapProfiling._samplers);
					while (LeapProfiling._samplersToCreate.Count > 0)
					{
						string text = LeapProfiling._samplersToCreate.Dequeue();
						dictionary[text] = CustomSampler.Create(text);
					}
					LeapProfiling._samplersToCreateCount = 0;
					LeapProfiling._samplers = dictionary;
				}
			}
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000E9718 File Offset: 0x000E7B18
		public static void BeginProfilingForThread(BeginProfilingForThreadArgs eventData)
		{
			object samplersToCreate = LeapProfiling._samplersToCreate;
			lock (samplersToCreate)
			{
				foreach (string item in eventData.blockNames)
				{
					LeapProfiling._samplersToCreate.Enqueue(item);
				}
				Interlocked.Add(ref LeapProfiling._samplersToCreateCount, eventData.blockNames.Length);
			}
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000E978C File Offset: 0x000E7B8C
		public static void EndProfilingForThread(EndProfilingForThreadArgs eventData)
		{
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x000E9790 File Offset: 0x000E7B90
		public static void BeginProfilingBlock(BeginProfilingBlockArgs eventData)
		{
			CustomSampler customSampler;
			if (LeapProfiling._samplers.TryGetValue(eventData.blockName, out customSampler))
			{
			}
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x000E97B8 File Offset: 0x000E7BB8
		public static void EndProfilingBlock(EndProfilingBlockArgs eventData)
		{
			CustomSampler customSampler;
			if (LeapProfiling._samplers.TryGetValue(eventData.blockName, out customSampler))
			{
			}
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x000E97DD File Offset: 0x000E7BDD
		// Note: this type is marked as 'beforefieldinit'.
		static LeapProfiling()
		{
		}

		// Token: 0x040022F8 RID: 8952
		private static Dictionary<string, CustomSampler> _samplers = new Dictionary<string, CustomSampler>();

		// Token: 0x040022F9 RID: 8953
		private static Queue<string> _samplersToCreate = new Queue<string>();

		// Token: 0x040022FA RID: 8954
		private static int _samplersToCreateCount = 0;
	}
}
