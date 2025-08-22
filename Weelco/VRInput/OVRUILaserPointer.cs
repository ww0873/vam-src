using System;
using UnityEngine;

namespace Weelco.VRInput
{
	// Token: 0x0200058B RID: 1419
	public class OVRUILaserPointer : IUILaserPointer
	{
		// Token: 0x060023B4 RID: 9140 RVA: 0x000CF138 File Offset: 0x000CD538
		public OVRUILaserPointer()
		{
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000CF140 File Offset: 0x000CD540
		public override bool ButtonDown()
		{
			return false;
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000CF143 File Offset: 0x000CD543
		public override bool ButtonUp()
		{
			return false;
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000CF146 File Offset: 0x000CD546
		public override void OnEnterControl(GameObject control)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000CF14D File Offset: 0x000CD54D
		public override void OnExitControl(GameObject control)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060023B9 RID: 9145 RVA: 0x000CF154 File Offset: 0x000CD554
		public override Transform target
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
