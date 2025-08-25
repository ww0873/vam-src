using System;
using System.Collections;

namespace AssetBundles
{
	// Token: 0x02000097 RID: 151
	public abstract class AssetBundleLoadOperation : IEnumerator
	{
		// Token: 0x06000227 RID: 551 RVA: 0x00010453 File Offset: 0x0000E853
		protected AssetBundleLoadOperation()
		{
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0001045B File Offset: 0x0000E85B
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0001045E File Offset: 0x0000E85E
		public bool MoveNext()
		{
			return !this.IsDone();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00010469 File Offset: 0x0000E869
		public void Reset()
		{
		}

		// Token: 0x0600022B RID: 555
		public abstract bool Update();

		// Token: 0x0600022C RID: 556
		public abstract bool IsDone();
	}
}
