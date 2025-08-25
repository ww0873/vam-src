using System;
using UnityEngine;

namespace Obi.CrossPlatformInput.PlatformSpecific
{
	// Token: 0x0200037B RID: 891
	public class StandaloneInput : VirtualInput
	{
		// Token: 0x06001648 RID: 5704 RVA: 0x0007DE3F File Offset: 0x0007C23F
		public StandaloneInput()
		{
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0007DE47 File Offset: 0x0007C247
		public override float GetAxis(string name, bool raw)
		{
			return (!raw) ? Input.GetAxis(name) : Input.GetAxisRaw(name);
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0007DE60 File Offset: 0x0007C260
		public override bool GetButton(string name)
		{
			return Input.GetButton(name);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0007DE68 File Offset: 0x0007C268
		public override bool GetButtonDown(string name)
		{
			return Input.GetButtonDown(name);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0007DE70 File Offset: 0x0007C270
		public override bool GetButtonUp(string name)
		{
			return Input.GetButtonUp(name);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0007DE78 File Offset: 0x0007C278
		public override void SetButtonDown(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0007DE84 File Offset: 0x0007C284
		public override void SetButtonUp(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0007DE90 File Offset: 0x0007C290
		public override void SetAxisPositive(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0007DE9C File Offset: 0x0007C29C
		public override void SetAxisNegative(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0007DEA8 File Offset: 0x0007C2A8
		public override void SetAxisZero(string name)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0007DEB4 File Offset: 0x0007C2B4
		public override void SetAxis(string name, float value)
		{
			throw new Exception(" This is not possible to be called for standalone input. Please check your platform and code where this is called");
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0007DEC0 File Offset: 0x0007C2C0
		public override Vector3 MousePosition()
		{
			return Input.mousePosition;
		}
	}
}
