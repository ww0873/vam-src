using System;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006B3 RID: 1715
	public class MultiTypedReference<BaseType, A, B, C, D> : MultiTypedReference<BaseType, A, B, C> where BaseType : class where A : BaseType where B : BaseType where C : BaseType where D : BaseType
	{
		// Token: 0x0600294F RID: 10575 RVA: 0x000E11BB File Offset: 0x000DF5BB
		public MultiTypedReference()
		{
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x000E11CE File Offset: 0x000DF5CE
		public override void Clear()
		{
			if (this._index == 3)
			{
				this._d.Clear();
			}
			base.Clear();
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x000E11ED File Offset: 0x000DF5ED
		protected override BaseType internalGet()
		{
			if (this._index == 3)
			{
				return (BaseType)((object)this._d[0]);
			}
			return base.internalGet();
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x000E1218 File Offset: 0x000DF618
		protected override void internalSetAfterClear(BaseType obj)
		{
			if (obj is D)
			{
				this._d.Add((D)((object)obj));
				this._index = 3;
			}
			else
			{
				base.internalSetAfterClear(obj);
			}
		}

		// Token: 0x040021EA RID: 8682
		[SerializeField]
		private List<D> _d = new List<D>();
	}
}
