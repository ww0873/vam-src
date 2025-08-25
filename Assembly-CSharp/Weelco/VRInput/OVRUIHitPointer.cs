using System;
using UnityEngine;

namespace Weelco.VRInput
{
	// Token: 0x0200058A RID: 1418
	public class OVRUIHitPointer : IUIHitPointer
	{
		// Token: 0x060023AE RID: 9134 RVA: 0x000CF115 File Offset: 0x000CD515
		public OVRUIHitPointer()
		{
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x000CF11D File Offset: 0x000CD51D
		public override bool ButtonDown()
		{
			return false;
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000CF120 File Offset: 0x000CD520
		public override bool ButtonUp()
		{
			return false;
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x000CF123 File Offset: 0x000CD523
		public override void OnEnterControl(GameObject control)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000CF12A File Offset: 0x000CD52A
		public override void OnExitControl(GameObject control)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060023B3 RID: 9139 RVA: 0x000CF131 File Offset: 0x000CD531
		public override Transform target
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
