using System;

namespace Leap.Unity
{
	// Token: 0x020006AF RID: 1711
	public abstract class MultiTypedReference<BaseType> where BaseType : class
	{
		// Token: 0x06002941 RID: 10561 RVA: 0x000E0F1C File Offset: 0x000DF31C
		protected MultiTypedReference()
		{
		}

		// Token: 0x06002942 RID: 10562
		public abstract void Clear();

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06002943 RID: 10563
		// (set) Token: 0x06002944 RID: 10564
		public abstract BaseType Value { get; set; }
	}
}
