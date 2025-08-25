using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000816 RID: 2070
	public class MessageWithMatchmakingEnqueueResult : Message<MatchmakingEnqueueResult>
	{
		// Token: 0x0600365A RID: 13914 RVA: 0x0010B7A1 File Offset: 0x00109BA1
		public MessageWithMatchmakingEnqueueResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x0010B7AA File Offset: 0x00109BAA
		public override MatchmakingEnqueueResult GetMatchmakingEnqueueResult()
		{
			return base.Data;
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x0010B7B4 File Offset: 0x00109BB4
		protected override MatchmakingEnqueueResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetMatchmakingEnqueueResult(obj);
			return new MatchmakingEnqueueResult(o);
		}
	}
}
