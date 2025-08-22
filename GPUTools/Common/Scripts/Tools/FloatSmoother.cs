using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
	// Token: 0x020009CC RID: 2508
	public class FloatSmoother
	{
		// Token: 0x06003F4E RID: 16206 RVA: 0x0012EE48 File Offset: 0x0012D248
		public FloatSmoother(int bufferLength)
		{
			this.buffer = new float[bufferLength];
			for (int i = 0; i < bufferLength; i++)
			{
				this.buffer[i] = Time.fixedDeltaTime;
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x0012EE88 File Offset: 0x0012D288
		public void AddValue(float value)
		{
			for (int i = 0; i < this.buffer.Length - 1; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.buffer.Length - 1] = value;
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0012EED4 File Offset: 0x0012D2D4
		public float GetSmoothedValue()
		{
			float num = 0f;
			for (int i = 0; i < this.buffer.Length; i++)
			{
				num += this.buffer[i];
			}
			return num / (float)this.buffer.Length;
		}

		// Token: 0x04002FFF RID: 12287
		private float[] buffer;
	}
}
