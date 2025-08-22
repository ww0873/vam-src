using System;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x02000838 RID: 2104
	public class MicrophoneInput : IMicrophone
	{
		// Token: 0x060036C1 RID: 14017 RVA: 0x0010BEA8 File Offset: 0x0010A2A8
		public MicrophoneInput()
		{
			int num = 1;
			int num2 = 48000;
			this.microphoneClip = Microphone.Start(null, true, num, num2);
			this.micBufferSizeSamples = num * num2;
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x0010BEDB File Offset: 0x0010A2DB
		public void Start()
		{
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x0010BEDD File Offset: 0x0010A2DD
		public void Stop()
		{
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x0010BEE0 File Offset: 0x0010A2E0
		public float[] Update()
		{
			int position = Microphone.GetPosition(null);
			int num2;
			if (position < this.lastMicrophoneSample)
			{
				int num = this.micBufferSizeSamples - this.lastMicrophoneSample;
				num2 = num + position;
			}
			else
			{
				num2 = position - this.lastMicrophoneSample;
			}
			if (num2 == 0)
			{
				return null;
			}
			float[] array = new float[num2];
			this.microphoneClip.GetData(array, this.lastMicrophoneSample);
			this.lastMicrophoneSample = position;
			return array;
		}

		// Token: 0x040027DE RID: 10206
		private AudioClip microphoneClip;

		// Token: 0x040027DF RID: 10207
		private int lastMicrophoneSample;

		// Token: 0x040027E0 RID: 10208
		private int micBufferSizeSamples;
	}
}
