using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200083D RID: 2109
	public class AchievementProgressList : DeserializableList<AchievementProgress>
	{
		// Token: 0x060036CC RID: 14028 RVA: 0x0010C20C File Offset: 0x0010A60C
		public AchievementProgressList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_AchievementProgressArray_GetSize(a));
			this._Data = new List<AchievementProgress>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new AchievementProgress(CAPI.ovr_AchievementProgressArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_AchievementProgressArray_GetNextUrl(a);
		}
	}
}
