using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000876 RID: 2166
	public class UserAndRoomList : DeserializableList<UserAndRoom>
	{
		// Token: 0x06003721 RID: 14113 RVA: 0x0010D2A0 File Offset: 0x0010B6A0
		public UserAndRoomList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_UserAndRoomArray_GetSize(a));
			this._Data = new List<UserAndRoom>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new UserAndRoom(CAPI.ovr_UserAndRoomArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_UserAndRoomArray_GetNextUrl(a);
		}
	}
}
