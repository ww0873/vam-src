using System;

namespace Leap.Unity
{
	// Token: 0x020006A3 RID: 1699
	public abstract class MultiTypedList
	{
		// Token: 0x06002906 RID: 10502 RVA: 0x000E080F File Offset: 0x000DEC0F
		protected MultiTypedList()
		{
		}

		// Token: 0x020006A4 RID: 1700
		[Serializable]
		public struct Key
		{
			// Token: 0x040021D7 RID: 8663
			public int id;

			// Token: 0x040021D8 RID: 8664
			public int index;
		}
	}
}
