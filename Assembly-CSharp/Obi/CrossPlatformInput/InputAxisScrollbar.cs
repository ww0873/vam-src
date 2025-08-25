using System;
using UnityEngine;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000376 RID: 886
	public class InputAxisScrollbar : MonoBehaviour
	{
		// Token: 0x06001629 RID: 5673 RVA: 0x0007D660 File Offset: 0x0007BA60
		public InputAxisScrollbar()
		{
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0007D668 File Offset: 0x0007BA68
		private void Update()
		{
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0007D66A File Offset: 0x0007BA6A
		public void HandleInput(float value)
		{
			CrossPlatformInputManager.SetAxis(this.axis, value * 2f - 1f);
		}

		// Token: 0x04001263 RID: 4707
		public string axis;
	}
}
