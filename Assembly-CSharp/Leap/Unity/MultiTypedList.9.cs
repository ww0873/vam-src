using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006AE RID: 1710
	public class MultiTypedList<BaseType, A, B, C, D, E, F, G, H> : MultiTypedList<BaseType, A, B, C, D, E, F, G> where A : BaseType where B : BaseType where C : BaseType where D : BaseType where E : BaseType where F : BaseType where G : BaseType where H : BaseType
	{
		// Token: 0x0600293D RID: 10557 RVA: 0x000E0EAF File Offset: 0x000DF2AF
		public MultiTypedList()
		{
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000E0EC2 File Offset: 0x000DF2C2
		protected override MultiTypedList.Key addInternal(BaseType obj)
		{
			return (!(obj is H)) ? base.addInternal(obj) : base.addHelper(this._h, obj, 7);
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000E0EEE File Offset: 0x000DF2EE
		protected override IList getList(int id)
		{
			return (id != 7) ? base.getList(id) : this._h;
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000E0F09 File Offset: 0x000DF309
		public override void Clear()
		{
			base.Clear();
			this._h.Clear();
		}

		// Token: 0x040021E4 RID: 8676
		[SerializeField]
		private List<H> _h = new List<H>();
	}
}
