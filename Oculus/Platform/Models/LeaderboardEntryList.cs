using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000850 RID: 2128
	public class LeaderboardEntryList : DeserializableList<LeaderboardEntry>
	{
		// Token: 0x060036F3 RID: 14067 RVA: 0x0010C6C0 File Offset: 0x0010AAC0
		public LeaderboardEntryList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_LeaderboardEntryArray_GetSize(a));
			this._Data = new List<LeaderboardEntry>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new LeaderboardEntry(CAPI.ovr_LeaderboardEntryArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this.TotalCount = CAPI.ovr_LeaderboardEntryArray_GetTotalCount(a);
			this._PreviousUrl = CAPI.ovr_LeaderboardEntryArray_GetPreviousUrl(a);
			this._NextUrl = CAPI.ovr_LeaderboardEntryArray_GetNextUrl(a);
		}

		// Token: 0x04002827 RID: 10279
		public readonly ulong TotalCount;
	}
}
