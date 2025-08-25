using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200062F RID: 1583
	[Serializable]
	public class SmoothedFloat
	{
		// Token: 0x060026DD RID: 9949 RVA: 0x000D9D4C File Offset: 0x000D814C
		public SmoothedFloat()
		{
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000D9D5B File Offset: 0x000D815B
		public void SetBlend(float blend, float deltaTime = 1f)
		{
			this.delay = deltaTime * blend / (1f - blend);
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000D9D70 File Offset: 0x000D8170
		public float Update(float input, float deltaTime = 1f)
		{
			if (deltaTime > 0f && !this.reset)
			{
				float num = this.delay / deltaTime;
				float num2 = num / (1f + num);
				this.value = Mathf.Lerp(this.value, input, 1f - num2);
			}
			else
			{
				this.value = input;
				this.reset = false;
			}
			return this.value;
		}

		// Token: 0x040020EF RID: 8431
		public float value;

		// Token: 0x040020F0 RID: 8432
		public float delay;

		// Token: 0x040020F1 RID: 8433
		public bool reset = true;
	}
}
