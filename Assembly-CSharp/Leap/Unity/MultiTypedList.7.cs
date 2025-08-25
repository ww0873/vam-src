using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006AC RID: 1708
	public class MultiTypedList<BaseType, A, B, C, D, E, F> : MultiTypedList<BaseType, A, B, C, D, E> where A : BaseType where B : BaseType where C : BaseType where D : BaseType where E : BaseType where F : BaseType
	{
		// Token: 0x06002935 RID: 10549 RVA: 0x000E0DD5 File Offset: 0x000DF1D5
		public MultiTypedList()
		{
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000E0DE8 File Offset: 0x000DF1E8
		protected override MultiTypedList.Key addInternal(BaseType obj)
		{
			return (!(obj is F)) ? base.addInternal(obj) : base.addHelper(this._f, obj, 5);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000E0E14 File Offset: 0x000DF214
		protected override IList getList(int id)
		{
			return (id != 5) ? base.getList(id) : this._f;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000E0E2F File Offset: 0x000DF22F
		public override void Clear()
		{
			base.Clear();
			this._f.Clear();
		}

		// Token: 0x040021E2 RID: 8674
		[SerializeField]
		private List<F> _f = new List<F>();
	}
}
