using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000869 RID: 2153
	public class PurchaseList : DeserializableList<Purchase>
	{
		// Token: 0x06003714 RID: 14100 RVA: 0x0010CDB4 File Offset: 0x0010B1B4
		public PurchaseList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_PurchaseArray_GetSize(a));
			this._Data = new List<Purchase>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new Purchase(CAPI.ovr_PurchaseArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_PurchaseArray_GetNextUrl(a);
		}
	}
}
