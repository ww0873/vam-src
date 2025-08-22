using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000867 RID: 2151
	public class ProductList : DeserializableList<Product>
	{
		// Token: 0x06003712 RID: 14098 RVA: 0x0010CD14 File Offset: 0x0010B114
		public ProductList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_ProductArray_GetSize(a));
			this._Data = new List<Product>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new Product(CAPI.ovr_ProductArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_ProductArray_GetNextUrl(a);
		}
	}
}
