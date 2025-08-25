using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200086B RID: 2155
	public class RoomList : DeserializableList<Room>
	{
		// Token: 0x06003716 RID: 14102 RVA: 0x0010CFA4 File Offset: 0x0010B3A4
		public RoomList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_RoomArray_GetSize(a));
			this._Data = new List<Room>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new Room(CAPI.ovr_RoomArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_RoomArray_GetNextUrl(a);
		}
	}
}
