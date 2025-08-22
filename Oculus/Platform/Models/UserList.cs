using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000874 RID: 2164
	public class UserList : DeserializableList<User>
	{
		// Token: 0x0600371F RID: 14111 RVA: 0x0010D1D8 File Offset: 0x0010B5D8
		public UserList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_UserArray_GetSize(a));
			this._Data = new List<User>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new User(CAPI.ovr_UserArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_UserArray_GetNextUrl(a);
		}
	}
}
