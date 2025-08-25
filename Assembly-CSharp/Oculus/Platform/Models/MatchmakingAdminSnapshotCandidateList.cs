using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000857 RID: 2135
	public class MatchmakingAdminSnapshotCandidateList : DeserializableList<MatchmakingAdminSnapshotCandidate>
	{
		// Token: 0x060036FA RID: 14074 RVA: 0x0010C858 File Offset: 0x0010AC58
		public MatchmakingAdminSnapshotCandidateList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_MatchmakingAdminSnapshotCandidateArray_GetSize(a));
			this._Data = new List<MatchmakingAdminSnapshotCandidate>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new MatchmakingAdminSnapshotCandidate(CAPI.ovr_MatchmakingAdminSnapshotCandidateArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
		}
	}
}
