using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006AA RID: 1706
	public class MultiTypedList<BaseType, A, B, C, D> : MultiTypedList<BaseType, A, B, C> where A : BaseType where B : BaseType where C : BaseType where D : BaseType
	{
		// Token: 0x0600292D RID: 10541 RVA: 0x000E0CFB File Offset: 0x000DF0FB
		public MultiTypedList()
		{
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000E0D0E File Offset: 0x000DF10E
		protected override MultiTypedList.Key addInternal(BaseType obj)
		{
			return (!(obj is D)) ? base.addInternal(obj) : base.addHelper(this._d, obj, 3);
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000E0D3A File Offset: 0x000DF13A
		protected override IList getList(int id)
		{
			return (id != 3) ? base.getList(id) : this._d;
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000E0D55 File Offset: 0x000DF155
		public override void Clear()
		{
			base.Clear();
			this._d.Clear();
		}

		// Token: 0x040021E0 RID: 8672
		[SerializeField]
		private List<D> _d = new List<D>();
	}
}
