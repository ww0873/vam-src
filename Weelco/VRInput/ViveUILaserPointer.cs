using System;
using UnityEngine;

namespace Weelco.VRInput
{
	// Token: 0x0200058D RID: 1421
	public class ViveUILaserPointer : IUILaserPointer
	{
		// Token: 0x060023C1 RID: 9153 RVA: 0x000CF1A5 File Offset: 0x000CD5A5
		public ViveUILaserPointer()
		{
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000CF1AD File Offset: 0x000CD5AD
		public override bool ButtonDown()
		{
			return false;
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000CF1B0 File Offset: 0x000CD5B0
		public override bool ButtonUp()
		{
			return false;
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000CF1B3 File Offset: 0x000CD5B3
		public override void OnEnterControl(GameObject control)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000CF1BA File Offset: 0x000CD5BA
		public override void OnExitControl(GameObject control)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x000CF1C1 File Offset: 0x000CD5C1
		public override Transform target
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
