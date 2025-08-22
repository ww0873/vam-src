using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x02000863 RID: 2147
	public class PidList : DeserializableList<Pid>
	{
		// Token: 0x0600370A RID: 14090 RVA: 0x0010CC10 File Offset: 0x0010B010
		public PidList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_PidArray_GetSize(a));
			this._Data = new List<Pid>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new Pid(CAPI.ovr_PidArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
		}
	}
}
