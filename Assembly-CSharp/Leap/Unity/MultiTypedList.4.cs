using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006A9 RID: 1705
	public class MultiTypedList<BaseType, A, B, C> : MultiTypedList<BaseType, A, B> where A : BaseType where B : BaseType where C : BaseType
	{
		// Token: 0x06002929 RID: 10537 RVA: 0x000E0C8E File Offset: 0x000DF08E
		public MultiTypedList()
		{
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000E0CA1 File Offset: 0x000DF0A1
		protected override MultiTypedList.Key addInternal(BaseType obj)
		{
			return (!(obj is C)) ? base.addInternal(obj) : base.addHelper(this._c, obj, 2);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000E0CCD File Offset: 0x000DF0CD
		protected override IList getList(int id)
		{
			return (id != 2) ? base.getList(id) : this._c;
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000E0CE8 File Offset: 0x000DF0E8
		public override void Clear()
		{
			base.Clear();
			this._c.Clear();
		}

		// Token: 0x040021DF RID: 8671
		[SerializeField]
		private List<C> _c = new List<C>();
	}
}
