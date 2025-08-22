using System;

namespace Oculus.Platform
{
	// Token: 0x020008AE RID: 2222
	public class VoipPCMSourceNative : IVoipPCMSource
	{
		// Token: 0x060037EC RID: 14316 RVA: 0x0010EF7D File Offset: 0x0010D37D
		public VoipPCMSourceNative()
		{
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x0010EF85 File Offset: 0x0010D385
		public int GetPCM(float[] dest, int length)
		{
			return (int)((uint)CAPI.ovr_Voip_GetPCMFloat(this.senderID, dest, (UIntPtr)((ulong)((long)length))));
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x0010EF9F File Offset: 0x0010D39F
		public void SetSenderID(ulong senderID)
		{
			this.senderID = senderID;
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x0010EFA8 File Offset: 0x0010D3A8
		public int PeekSizeElements()
		{
			return (int)((uint)CAPI.ovr_Voip_GetPCMSize(this.senderID));
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0010EFBA File Offset: 0x0010D3BA
		public void Update()
		{
		}

		// Token: 0x0400291E RID: 10526
		private ulong senderID;
	}
}
