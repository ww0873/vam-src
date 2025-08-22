using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200083C RID: 2108
	public class AchievementProgress
	{
		// Token: 0x060036CB RID: 14027 RVA: 0x0010C1BC File Offset: 0x0010A5BC
		public AchievementProgress(IntPtr o)
		{
			this.Bitfield = CAPI.ovr_AchievementProgress_GetBitfield(o);
			this.Count = CAPI.ovr_AchievementProgress_GetCount(o);
			this.IsUnlocked = CAPI.ovr_AchievementProgress_GetIsUnlocked(o);
			this.Name = CAPI.ovr_AchievementProgress_GetName(o);
			this.UnlockTime = CAPI.ovr_AchievementProgress_GetUnlockTime(o);
		}

		// Token: 0x040027E8 RID: 10216
		public readonly string Bitfield;

		// Token: 0x040027E9 RID: 10217
		public readonly ulong Count;

		// Token: 0x040027EA RID: 10218
		public readonly bool IsUnlocked;

		// Token: 0x040027EB RID: 10219
		public readonly string Name;

		// Token: 0x040027EC RID: 10220
		public readonly DateTime UnlockTime;
	}
}
