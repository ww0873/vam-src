using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Weelco.VRInput
{
	// Token: 0x02000591 RID: 1425
	public class VRInputEventData : PointerEventData
	{
		// Token: 0x060023D1 RID: 9169 RVA: 0x000CFEA4 File Offset: 0x000CE2A4
		public VRInputEventData(EventSystem e) : base(e)
		{
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000CFEAD File Offset: 0x000CE2AD
		public override void Reset()
		{
			this.current = null;
			this.controller = null;
			base.Reset();
		}

		// Token: 0x04001E30 RID: 7728
		public GameObject current;

		// Token: 0x04001E31 RID: 7729
		public IUIPointer controller;
	}
}
