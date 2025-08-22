using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000831 RID: 2097
	public class MessageWithNetworkingPeer : Message<NetworkingPeer>
	{
		// Token: 0x060036AB RID: 13995 RVA: 0x0010BD01 File Offset: 0x0010A101
		public MessageWithNetworkingPeer(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x0010BD0A File Offset: 0x0010A10A
		public override NetworkingPeer GetNetworkingPeer()
		{
			return base.Data;
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x0010BD14 File Offset: 0x0010A114
		protected override NetworkingPeer GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNetworkingPeer(c_message);
			return new NetworkingPeer(CAPI.ovr_NetworkingPeer_GetID(obj), CAPI.ovr_NetworkingPeer_GetState(obj));
		}
	}
}
