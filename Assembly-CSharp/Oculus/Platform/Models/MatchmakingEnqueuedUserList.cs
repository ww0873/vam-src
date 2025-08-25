using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200085A RID: 2138
	public class MatchmakingEnqueuedUserList : DeserializableList<MatchmakingEnqueuedUser>
	{
		// Token: 0x060036FD RID: 14077 RVA: 0x0010C93C File Offset: 0x0010AD3C
		public MatchmakingEnqueuedUserList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_MatchmakingEnqueuedUserArray_GetSize(a));
			this._Data = new List<MatchmakingEnqueuedUser>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new MatchmakingEnqueuedUser(CAPI.ovr_MatchmakingEnqueuedUserArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
		}
	}
}
