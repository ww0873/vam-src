using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006AD RID: 1709
	public class MultiTypedList<BaseType, A, B, C, D, E, F, G> : MultiTypedList<BaseType, A, B, C, D, E, F> where A : BaseType where B : BaseType where C : BaseType where D : BaseType where E : BaseType where F : BaseType where G : BaseType
	{
		// Token: 0x06002939 RID: 10553 RVA: 0x000E0E42 File Offset: 0x000DF242
		public MultiTypedList()
		{
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000E0E55 File Offset: 0x000DF255
		protected override MultiTypedList.Key addInternal(BaseType obj)
		{
			return (!(obj is G)) ? base.addInternal(obj) : base.addHelper(this._g, obj, 6);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000E0E81 File Offset: 0x000DF281
		protected override IList getList(int id)
		{
			return (id != 6) ? base.getList(id) : this._g;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x000E0E9C File Offset: 0x000DF29C
		public override void Clear()
		{
			base.Clear();
			this._g.Clear();
		}

		// Token: 0x040021E3 RID: 8675
		[SerializeField]
		private List<G> _g = new List<G>();
	}
}
