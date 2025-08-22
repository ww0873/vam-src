using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006AB RID: 1707
	public class MultiTypedList<BaseType, A, B, C, D, E> : MultiTypedList<BaseType, A, B, C, D> where A : BaseType where B : BaseType where C : BaseType where D : BaseType where E : BaseType
	{
		// Token: 0x06002931 RID: 10545 RVA: 0x000E0D68 File Offset: 0x000DF168
		public MultiTypedList()
		{
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000E0D7B File Offset: 0x000DF17B
		protected override MultiTypedList.Key addInternal(BaseType obj)
		{
			return (!(obj is E)) ? base.addInternal(obj) : base.addHelper(this._e, obj, 4);
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000E0DA7 File Offset: 0x000DF1A7
		protected override IList getList(int id)
		{
			return (id != 4) ? base.getList(id) : this._e;
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000E0DC2 File Offset: 0x000DF1C2
		public override void Clear()
		{
			base.Clear();
			this._e.Clear();
		}

		// Token: 0x040021E1 RID: 8673
		[SerializeField]
		private List<E> _e = new List<E>();
	}
}
