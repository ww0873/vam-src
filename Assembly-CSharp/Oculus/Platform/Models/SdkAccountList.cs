using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200086F RID: 2159
	public class SdkAccountList : DeserializableList<SdkAccount>
	{
		// Token: 0x0600371A RID: 14106 RVA: 0x0010D0C0 File Offset: 0x0010B4C0
		public SdkAccountList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_SdkAccountArray_GetSize(a));
			this._Data = new List<SdkAccount>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new SdkAccount(CAPI.ovr_SdkAccountArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
		}
	}
}
