using System;

namespace GPUTools.Physics.Scripts.Wind
{
	// Token: 0x02000A86 RID: 2694
	[Serializable]
	public struct NoiseOctave
	{
		// Token: 0x060045CA RID: 17866 RVA: 0x0013FB8B File Offset: 0x0013DF8B
		public NoiseOctave(float scale, float amplitude)
		{
			this.Scale = scale;
			this.Amplitude = amplitude;
		}

		// Token: 0x04003384 RID: 13188
		public float Scale;

		// Token: 0x04003385 RID: 13189
		public float Amplitude;
	}
}
