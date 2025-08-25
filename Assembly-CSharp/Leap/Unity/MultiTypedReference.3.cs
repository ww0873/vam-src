using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006B2 RID: 1714
	public class MultiTypedReference<BaseType, A, B, C> : MultiTypedReference<BaseType, A, B> where BaseType : class where A : BaseType where B : BaseType where C : BaseType
	{
		// Token: 0x0600294B RID: 10571 RVA: 0x000E1123 File Offset: 0x000DF523
		public MultiTypedReference()
		{
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000E1136 File Offset: 0x000DF536
		public override void Clear()
		{
			if (this._index == 2)
			{
				this._c.Clear();
			}
			base.Clear();
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000E1155 File Offset: 0x000DF555
		protected override BaseType internalGet()
		{
			if (this._index == 2)
			{
				return (BaseType)((object)this._c[0]);
			}
			return base.internalGet();
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000E1180 File Offset: 0x000DF580
		protected override void internalSetAfterClear(BaseType obj)
		{
			if (obj is C)
			{
				this._c.Add((C)((object)obj));
				this._index = 2;
			}
			else
			{
				base.internalSetAfterClear(obj);
			}
		}

		// Token: 0x040021E9 RID: 8681
		[SerializeField]
		private List<C> _c = new List<C>();
	}
}
