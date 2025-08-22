using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000630 RID: 1584
	[Serializable]
	public class SmoothedQuaternion
	{
		// Token: 0x060026E0 RID: 9952 RVA: 0x000D9DD8 File Offset: 0x000D81D8
		public SmoothedQuaternion()
		{
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x000D9DF2 File Offset: 0x000D81F2
		public void SetBlend(float blend, float deltaTime = 1f)
		{
			this.delay = deltaTime * blend / (1f - blend);
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000D9E08 File Offset: 0x000D8208
		public Quaternion Update(Quaternion input, float deltaTime = 1f)
		{
			if (deltaTime > 0f && !this.reset)
			{
				float num = this.delay / deltaTime;
				float num2 = num / (1f + num);
				this.value = Quaternion.Slerp(this.value, input, 1f - num2);
			}
			else
			{
				this.value = input;
				this.reset = false;
			}
			return this.value;
		}

		// Token: 0x040020F2 RID: 8434
		public Quaternion value = Quaternion.identity;

		// Token: 0x040020F3 RID: 8435
		public float delay;

		// Token: 0x040020F4 RID: 8436
		public bool reset = true;
	}
}
