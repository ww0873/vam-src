using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200085C RID: 2140
	public class MatchmakingEnqueueResultAndRoom
	{
		// Token: 0x060036FF RID: 14079 RVA: 0x0010CA2E File Offset: 0x0010AE2E
		public MatchmakingEnqueueResultAndRoom(IntPtr o)
		{
			this.MatchmakingEnqueueResult = new MatchmakingEnqueueResult(CAPI.ovr_MatchmakingEnqueueResultAndRoom_GetMatchmakingEnqueueResult(o));
			this.Room = new Room(CAPI.ovr_MatchmakingEnqueueResultAndRoom_GetRoom(o));
		}

		// Token: 0x04002846 RID: 10310
		public readonly MatchmakingEnqueueResult MatchmakingEnqueueResult;

		// Token: 0x04002847 RID: 10311
		public readonly Room Room;
	}
}
