using System;
using UnityEngine;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000371 RID: 881
	public class ButtonHandler : MonoBehaviour
	{
		// Token: 0x060015F2 RID: 5618 RVA: 0x0007D367 File Offset: 0x0007B767
		public ButtonHandler()
		{
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0007D36F File Offset: 0x0007B76F
		private void OnEnable()
		{
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0007D371 File Offset: 0x0007B771
		public void SetDownState()
		{
			CrossPlatformInputManager.SetButtonDown(this.Name);
		}

		// Token: 0x060015F5 RID: 5621 RVA: 0x0007D37E File Offset: 0x0007B77E
		public void SetUpState()
		{
			CrossPlatformInputManager.SetButtonUp(this.Name);
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0007D38B File Offset: 0x0007B78B
		public void SetAxisPositiveState()
		{
			CrossPlatformInputManager.SetAxisPositive(this.Name);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0007D398 File Offset: 0x0007B798
		public void SetAxisNeutralState()
		{
			CrossPlatformInputManager.SetAxisZero(this.Name);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0007D3A5 File Offset: 0x0007B7A5
		public void SetAxisNegativeState()
		{
			CrossPlatformInputManager.SetAxisNegative(this.Name);
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0007D3B2 File Offset: 0x0007B7B2
		public void Update()
		{
		}

		// Token: 0x04001254 RID: 4692
		public string Name;
	}
}
