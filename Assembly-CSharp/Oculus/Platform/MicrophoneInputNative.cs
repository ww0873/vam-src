using System;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x02000839 RID: 2105
	public class MicrophoneInputNative : IMicrophone
	{
		// Token: 0x060036C5 RID: 14021 RVA: 0x0010BF4C File Offset: 0x0010A34C
		public MicrophoneInputNative()
		{
			this.mic = CAPI.ovr_Microphone_Create();
			CAPI.ovr_Microphone_Start(this.mic);
			this.tempBuffer = new float[this.tempBufferSize];
			Debug.Log(this.mic);
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x0010BFA4 File Offset: 0x0010A3A4
		public float[] Update()
		{
			ulong num = (ulong)CAPI.ovr_Microphone_ReadData(this.mic, this.tempBuffer, (UIntPtr)((ulong)((long)this.tempBufferSize)));
			if (num > 0UL)
			{
				float[] array = new float[num];
				Array.Copy(this.tempBuffer, array, (int)num);
				return array;
			}
			return null;
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x0010BFF5 File Offset: 0x0010A3F5
		public void Start()
		{
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x0010BFF7 File Offset: 0x0010A3F7
		public void Stop()
		{
			CAPI.ovr_Microphone_Stop(this.mic);
			CAPI.ovr_Microphone_Destroy(this.mic);
		}

		// Token: 0x040027E1 RID: 10209
		private IntPtr mic;

		// Token: 0x040027E2 RID: 10210
		private int tempBufferSize = 9600;

		// Token: 0x040027E3 RID: 10211
		private float[] tempBuffer;
	}
}
