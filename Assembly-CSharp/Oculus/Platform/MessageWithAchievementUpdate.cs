using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000804 RID: 2052
	public class MessageWithAchievementUpdate : Message<AchievementUpdate>
	{
		// Token: 0x06003624 RID: 13860 RVA: 0x0010B3F9 File Offset: 0x001097F9
		public MessageWithAchievementUpdate(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x0010B402 File Offset: 0x00109802
		public override AchievementUpdate GetAchievementUpdate()
		{
			return base.Data;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x0010B40C File Offset: 0x0010980C
		protected override AchievementUpdate GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetAchievementUpdate(obj);
			return new AchievementUpdate(o);
		}
	}
}
