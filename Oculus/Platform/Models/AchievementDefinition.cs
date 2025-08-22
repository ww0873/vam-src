using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200083A RID: 2106
	public class AchievementDefinition
	{
		// Token: 0x060036C9 RID: 14025 RVA: 0x0010C00F File Offset: 0x0010A40F
		public AchievementDefinition(IntPtr o)
		{
			this.Type = CAPI.ovr_AchievementDefinition_GetType(o);
			this.Name = CAPI.ovr_AchievementDefinition_GetName(o);
			this.BitfieldLength = CAPI.ovr_AchievementDefinition_GetBitfieldLength(o);
			this.Target = CAPI.ovr_AchievementDefinition_GetTarget(o);
		}

		// Token: 0x040027E4 RID: 10212
		public readonly AchievementType Type;

		// Token: 0x040027E5 RID: 10213
		public readonly string Name;

		// Token: 0x040027E6 RID: 10214
		public readonly uint BitfieldLength;

		// Token: 0x040027E7 RID: 10215
		public readonly ulong Target;
	}
}
