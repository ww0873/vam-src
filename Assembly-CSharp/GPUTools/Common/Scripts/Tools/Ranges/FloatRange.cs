using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Ranges
{
	// Token: 0x020009D4 RID: 2516
	[Serializable]
	public struct FloatRange
	{
		// Token: 0x06003F80 RID: 16256 RVA: 0x0012F1E3 File Offset: 0x0012D5E3
		public FloatRange(float min, float max)
		{
			this.Min = min;
			this.Max = max;
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0012F1F3 File Offset: 0x0012D5F3
		public float GetRandom()
		{
			return UnityEngine.Random.Range(this.Min, this.Max);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0012F206 File Offset: 0x0012D606
		public float GetLerp(float t)
		{
			return Mathf.Lerp(this.Min, this.Max, t);
		}

		// Token: 0x04003012 RID: 12306
		public float Min;

		// Token: 0x04003013 RID: 12307
		public float Max;
	}
}
