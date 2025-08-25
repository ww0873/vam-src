using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi.CrossPlatformInput
{
	// Token: 0x02000383 RID: 899
	public abstract class VirtualInput
	{
		// Token: 0x06001662 RID: 5730 RVA: 0x0007D9DC File Offset: 0x0007BDDC
		protected VirtualInput()
		{
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x0007DA05 File Offset: 0x0007BE05
		// (set) Token: 0x06001664 RID: 5732 RVA: 0x0007DA0D File Offset: 0x0007BE0D
		public Vector3 virtualMousePosition
		{
			[CompilerGenerated]
			get
			{
				return this.<virtualMousePosition>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<virtualMousePosition>k__BackingField = value;
			}
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0007DA16 File Offset: 0x0007BE16
		public bool AxisExists(string name)
		{
			return this.m_VirtualAxes.ContainsKey(name);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0007DA24 File Offset: 0x0007BE24
		public bool ButtonExists(string name)
		{
			return this.m_VirtualButtons.ContainsKey(name);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0007DA34 File Offset: 0x0007BE34
		public void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis)
		{
			if (this.m_VirtualAxes.ContainsKey(axis.name))
			{
				UnityEngine.Debug.LogError("There is already a virtual axis named " + axis.name + " registered.");
			}
			else
			{
				this.m_VirtualAxes.Add(axis.name, axis);
				if (!axis.matchWithInputManager)
				{
					this.m_AlwaysUseVirtual.Add(axis.name);
				}
			}
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0007DAA4 File Offset: 0x0007BEA4
		public void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button)
		{
			if (this.m_VirtualButtons.ContainsKey(button.name))
			{
				UnityEngine.Debug.LogError("There is already a virtual button named " + button.name + " registered.");
			}
			else
			{
				this.m_VirtualButtons.Add(button.name, button);
				if (!button.matchWithInputManager)
				{
					this.m_AlwaysUseVirtual.Add(button.name);
				}
			}
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0007DB14 File Offset: 0x0007BF14
		public void UnRegisterVirtualAxis(string name)
		{
			if (this.m_VirtualAxes.ContainsKey(name))
			{
				this.m_VirtualAxes.Remove(name);
			}
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0007DB34 File Offset: 0x0007BF34
		public void UnRegisterVirtualButton(string name)
		{
			if (this.m_VirtualButtons.ContainsKey(name))
			{
				this.m_VirtualButtons.Remove(name);
			}
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0007DB54 File Offset: 0x0007BF54
		public CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name)
		{
			return (!this.m_VirtualAxes.ContainsKey(name)) ? null : this.m_VirtualAxes[name];
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0007DB7C File Offset: 0x0007BF7C
		public void SetVirtualMousePositionX(float f)
		{
			this.virtualMousePosition = new Vector3(f, this.virtualMousePosition.y, this.virtualMousePosition.z);
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0007DBB4 File Offset: 0x0007BFB4
		public void SetVirtualMousePositionY(float f)
		{
			this.virtualMousePosition = new Vector3(this.virtualMousePosition.x, f, this.virtualMousePosition.z);
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0007DBEC File Offset: 0x0007BFEC
		public void SetVirtualMousePositionZ(float f)
		{
			this.virtualMousePosition = new Vector3(this.virtualMousePosition.x, this.virtualMousePosition.y, f);
		}

		// Token: 0x0600166F RID: 5743
		public abstract float GetAxis(string name, bool raw);

		// Token: 0x06001670 RID: 5744
		public abstract bool GetButton(string name);

		// Token: 0x06001671 RID: 5745
		public abstract bool GetButtonDown(string name);

		// Token: 0x06001672 RID: 5746
		public abstract bool GetButtonUp(string name);

		// Token: 0x06001673 RID: 5747
		public abstract void SetButtonDown(string name);

		// Token: 0x06001674 RID: 5748
		public abstract void SetButtonUp(string name);

		// Token: 0x06001675 RID: 5749
		public abstract void SetAxisPositive(string name);

		// Token: 0x06001676 RID: 5750
		public abstract void SetAxisNegative(string name);

		// Token: 0x06001677 RID: 5751
		public abstract void SetAxisZero(string name);

		// Token: 0x06001678 RID: 5752
		public abstract void SetAxis(string name, float value);

		// Token: 0x06001679 RID: 5753
		public abstract Vector3 MousePosition();

		// Token: 0x0400129A RID: 4762
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector3 <virtualMousePosition>k__BackingField;

		// Token: 0x0400129B RID: 4763
		protected Dictionary<string, CrossPlatformInputManager.VirtualAxis> m_VirtualAxes = new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();

		// Token: 0x0400129C RID: 4764
		protected Dictionary<string, CrossPlatformInputManager.VirtualButton> m_VirtualButtons = new Dictionary<string, CrossPlatformInputManager.VirtualButton>();

		// Token: 0x0400129D RID: 4765
		protected List<string> m_AlwaysUseVirtual = new List<string>();
	}
}
