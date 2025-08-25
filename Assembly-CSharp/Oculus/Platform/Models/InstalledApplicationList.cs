using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200084D RID: 2125
	public class InstalledApplicationList : DeserializableList<InstalledApplication>
	{
		// Token: 0x060036F0 RID: 14064 RVA: 0x0010C590 File Offset: 0x0010A990
		public InstalledApplicationList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_InstalledApplicationArray_GetSize(a));
			this._Data = new List<InstalledApplication>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new InstalledApplication(CAPI.ovr_InstalledApplicationArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
		}
	}
}
