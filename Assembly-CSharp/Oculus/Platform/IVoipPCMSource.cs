using System;

namespace Oculus.Platform
{
	// Token: 0x020007F0 RID: 2032
	public interface IVoipPCMSource
	{
		// Token: 0x060035C0 RID: 13760
		int GetPCM(float[] dest, int length);

		// Token: 0x060035C1 RID: 13761
		void SetSenderID(ulong senderID);

		// Token: 0x060035C2 RID: 13762
		void Update();

		// Token: 0x060035C3 RID: 13763
		int PeekSizeElements();
	}
}
