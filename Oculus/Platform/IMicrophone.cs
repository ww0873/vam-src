using System;

namespace Oculus.Platform
{
	// Token: 0x020007EF RID: 2031
	public interface IMicrophone
	{
		// Token: 0x060035BD RID: 13757
		void Start();

		// Token: 0x060035BE RID: 13758
		void Stop();

		// Token: 0x060035BF RID: 13759
		float[] Update();
	}
}
