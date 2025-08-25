using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000803 RID: 2051
	public class MessageWithAchievementProgressList : Message<AchievementProgressList>
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x0010B3C5 File Offset: 0x001097C5
		public MessageWithAchievementProgressList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x0010B3CE File Offset: 0x001097CE
		public override AchievementProgressList GetAchievementProgressList()
		{
			return base.Data;
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x0010B3D8 File Offset: 0x001097D8
		protected override AchievementProgressList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetAchievementProgressArray(obj);
			return new AchievementProgressList(a);
		}
	}
}
