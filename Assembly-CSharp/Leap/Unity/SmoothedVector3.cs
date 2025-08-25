using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000631 RID: 1585
	[Serializable]
	public class SmoothedVector3
	{
		// Token: 0x060026E3 RID: 9955 RVA: 0x000D9E70 File Offset: 0x000D8270
		public SmoothedVector3()
		{
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000D9E8A File Offset: 0x000D828A
		public void SetBlend(float blend, float deltaTime = 1f)
		{
			this.delay = deltaTime * blend / (1f - blend);
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000D9EA0 File Offset: 0x000D82A0
		public Vector3 Update(Vector3 input, float deltaTime = 1f)
		{
			if (deltaTime > 0f && !this.reset)
			{
				float num = this.delay / deltaTime;
				float num2 = num / (1f + num);
				this.value = Vector3.Lerp(this.value, input, 1f - num2);
			}
			else
			{
				this.value = input;
				this.reset = false;
			}
			return this.value;
		}

		// Token: 0x040020F5 RID: 8437
		public Vector3 value = Vector3.zero;

		// Token: 0x040020F6 RID: 8438
		public float delay;

		// Token: 0x040020F7 RID: 8439
		public bool reset = true;
	}
}
