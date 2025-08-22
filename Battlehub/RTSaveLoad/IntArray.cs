using System;
using ProtoBuf;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000213 RID: 531
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class IntArray
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x0004218D File Offset: 0x0004058D
		public IntArray()
		{
		}

		// Token: 0x04000BF6 RID: 3062
		public int[] Array;
	}
}
