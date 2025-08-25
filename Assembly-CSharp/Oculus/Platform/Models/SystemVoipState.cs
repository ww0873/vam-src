using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000872 RID: 2162
	public class SystemVoipState
	{
		// Token: 0x0600371D RID: 14109 RVA: 0x0010D14E File Offset: 0x0010B54E
		public SystemVoipState(IntPtr o)
		{
			this.MicrophoneMuted = CAPI.ovr_SystemVoipState_GetMicrophoneMuted(o);
			this.Status = CAPI.ovr_SystemVoipState_GetStatus(o);
		}

		// Token: 0x04002880 RID: 10368
		public readonly VoipMuteState MicrophoneMuted;

		// Token: 0x04002881 RID: 10369
		public readonly SystemVoipStatus Status;
	}
}
