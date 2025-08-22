using System;
using UnityEngine;

namespace Obi.CrossPlatformInput.PlatformSpecific
{
	// Token: 0x0200037A RID: 890
	public class MobileInput : VirtualInput
	{
		// Token: 0x0600163A RID: 5690 RVA: 0x0007DC21 File Offset: 0x0007C021
		public MobileInput()
		{
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0007DC29 File Offset: 0x0007C029
		private void AddButton(string name)
		{
			CrossPlatformInputManager.RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton(name));
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0007DC36 File Offset: 0x0007C036
		private void AddAxes(string name)
		{
			CrossPlatformInputManager.RegisterVirtualAxis(new CrossPlatformInputManager.VirtualAxis(name));
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0007DC43 File Offset: 0x0007C043
		public override float GetAxis(string name, bool raw)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			return this.m_VirtualAxes[name].GetValue;
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0007DC6E File Offset: 0x0007C06E
		public override void SetButtonDown(string name)
		{
			if (!this.m_VirtualButtons.ContainsKey(name))
			{
				this.AddButton(name);
			}
			this.m_VirtualButtons[name].Pressed();
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x0007DC99 File Offset: 0x0007C099
		public override void SetButtonUp(string name)
		{
			if (!this.m_VirtualButtons.ContainsKey(name))
			{
				this.AddButton(name);
			}
			this.m_VirtualButtons[name].Released();
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0007DCC4 File Offset: 0x0007C0C4
		public override void SetAxisPositive(string name)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(1f);
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0007DCF4 File Offset: 0x0007C0F4
		public override void SetAxisNegative(string name)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(-1f);
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0007DD24 File Offset: 0x0007C124
		public override void SetAxisZero(string name)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(0f);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0007DD54 File Offset: 0x0007C154
		public override void SetAxis(string name, float value)
		{
			if (!this.m_VirtualAxes.ContainsKey(name))
			{
				this.AddAxes(name);
			}
			this.m_VirtualAxes[name].Update(value);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0007DD80 File Offset: 0x0007C180
		public override bool GetButtonDown(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				return this.m_VirtualButtons[name].GetButtonDown;
			}
			this.AddButton(name);
			return this.m_VirtualButtons[name].GetButtonDown;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0007DDBD File Offset: 0x0007C1BD
		public override bool GetButtonUp(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				return this.m_VirtualButtons[name].GetButtonUp;
			}
			this.AddButton(name);
			return this.m_VirtualButtons[name].GetButtonUp;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x0007DDFA File Offset: 0x0007C1FA
		public override bool GetButton(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				return this.m_VirtualButtons[name].GetButton;
			}
			this.AddButton(name);
			return this.m_VirtualButtons[name].GetButton;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0007DE37 File Offset: 0x0007C237
		public override Vector3 MousePosition()
		{
			return base.virtualMousePosition;
		}
	}
}
