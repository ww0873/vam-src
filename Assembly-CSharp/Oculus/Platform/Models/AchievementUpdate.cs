using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200083E RID: 2110
	public class AchievementUpdate
	{
		// Token: 0x060036CD RID: 14029 RVA: 0x0010C272 File Offset: 0x0010A672
		public AchievementUpdate(IntPtr o)
		{
			this.JustUnlocked = CAPI.ovr_AchievementUpdate_GetJustUnlocked(o);
			this.Name = CAPI.ovr_AchievementUpdate_GetName(o);
		}

		// Token: 0x040027ED RID: 10221
		public readonly bool JustUnlocked;

		// Token: 0x040027EE RID: 10222
		public readonly string Name;
	}
}
