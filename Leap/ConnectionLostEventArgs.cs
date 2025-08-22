using System;

namespace Leap
{
	// Token: 0x020005CC RID: 1484
	public class ConnectionLostEventArgs : LeapEventArgs
	{
		// Token: 0x06002599 RID: 9625 RVA: 0x000D6CB7 File Offset: 0x000D50B7
		public ConnectionLostEventArgs() : base(LeapEvent.EVENT_CONNECTION_LOST)
		{
		}
	}
}
