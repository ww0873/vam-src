using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200083B RID: 2107
	public class AchievementDefinitionList : DeserializableList<AchievementDefinition>
	{
		// Token: 0x060036CA RID: 14026 RVA: 0x0010C154 File Offset: 0x0010A554
		public AchievementDefinitionList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_AchievementDefinitionArray_GetSize(a));
			this._Data = new List<AchievementDefinition>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new AchievementDefinition(CAPI.ovr_AchievementDefinitionArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_AchievementDefinitionArray_GetNextUrl(a);
		}
	}
}
