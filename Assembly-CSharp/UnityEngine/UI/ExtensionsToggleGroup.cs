using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x02000546 RID: 1350
	[AddComponentMenu("UI/Extensions/Extensions Toggle Group")]
	[DisallowMultipleComponent]
	public class ExtensionsToggleGroup : UIBehaviour
	{
		// Token: 0x0600226C RID: 8812 RVA: 0x000C4F57 File Offset: 0x000C3357
		protected ExtensionsToggleGroup()
		{
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600226D RID: 8813 RVA: 0x000C4F80 File Offset: 0x000C3380
		// (set) Token: 0x0600226E RID: 8814 RVA: 0x000C4F88 File Offset: 0x000C3388
		public bool AllowSwitchOff
		{
			get
			{
				return this.m_AllowSwitchOff;
			}
			set
			{
				this.m_AllowSwitchOff = value;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x000C4F91 File Offset: 0x000C3391
		// (set) Token: 0x06002270 RID: 8816 RVA: 0x000C4F99 File Offset: 0x000C3399
		public ExtensionsToggle SelectedToggle
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedToggle>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SelectedToggle>k__BackingField = value;
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000C4FA2 File Offset: 0x000C33A2
		private void ValidateToggleIsInGroup(ExtensionsToggle toggle)
		{
			if (toggle == null || !this.m_Toggles.Contains(toggle))
			{
				throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[]
				{
					toggle,
					this
				}));
			}
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000C4FE0 File Offset: 0x000C33E0
		public void NotifyToggleOn(ExtensionsToggle toggle)
		{
			this.ValidateToggleIsInGroup(toggle);
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				if (this.m_Toggles[i] == toggle)
				{
					this.SelectedToggle = toggle;
				}
				else
				{
					this.m_Toggles[i].IsOn = false;
				}
			}
			this.onToggleGroupChanged.Invoke(this.AnyTogglesOn());
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000C5056 File Offset: 0x000C3456
		public void UnregisterToggle(ExtensionsToggle toggle)
		{
			if (this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Remove(toggle);
				toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.NotifyToggleChanged));
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000C508D File Offset: 0x000C348D
		private void NotifyToggleChanged(bool isOn)
		{
			this.onToggleGroupToggleChanged.Invoke(isOn);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000C509B File Offset: 0x000C349B
		public void RegisterToggle(ExtensionsToggle toggle)
		{
			if (!this.m_Toggles.Contains(toggle))
			{
				this.m_Toggles.Add(toggle);
				toggle.onValueChanged.AddListener(new UnityAction<bool>(this.NotifyToggleChanged));
			}
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000C50D1 File Offset: 0x000C34D1
		public bool AnyTogglesOn()
		{
			List<ExtensionsToggle> toggles = this.m_Toggles;
			if (ExtensionsToggleGroup.<>f__am$cache0 == null)
			{
				ExtensionsToggleGroup.<>f__am$cache0 = new Predicate<ExtensionsToggle>(ExtensionsToggleGroup.<AnyTogglesOn>m__0);
			}
			return toggles.Find(ExtensionsToggleGroup.<>f__am$cache0) != null;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000C5101 File Offset: 0x000C3501
		public IEnumerable<ExtensionsToggle> ActiveToggles()
		{
			IEnumerable<ExtensionsToggle> toggles = this.m_Toggles;
			if (ExtensionsToggleGroup.<>f__am$cache1 == null)
			{
				ExtensionsToggleGroup.<>f__am$cache1 = new Func<ExtensionsToggle, bool>(ExtensionsToggleGroup.<ActiveToggles>m__1);
			}
			return toggles.Where(ExtensionsToggleGroup.<>f__am$cache1);
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000C512C File Offset: 0x000C352C
		public void SetAllTogglesOff()
		{
			bool allowSwitchOff = this.m_AllowSwitchOff;
			this.m_AllowSwitchOff = true;
			for (int i = 0; i < this.m_Toggles.Count; i++)
			{
				this.m_Toggles[i].IsOn = false;
			}
			this.m_AllowSwitchOff = allowSwitchOff;
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000C517C File Offset: 0x000C357C
		public void HasTheGroupToggle(bool value)
		{
			Debug.Log("Testing, the group has toggled [" + value + "]");
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000C5198 File Offset: 0x000C3598
		public void HasAToggleFlipped(bool value)
		{
			Debug.Log("Testing, a toggle has toggled [" + value + "]");
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000C51B4 File Offset: 0x000C35B4
		[CompilerGenerated]
		private static bool <AnyTogglesOn>m__0(ExtensionsToggle x)
		{
			return x.IsOn;
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000C51BC File Offset: 0x000C35BC
		[CompilerGenerated]
		private static bool <ActiveToggles>m__1(ExtensionsToggle x)
		{
			return x.IsOn;
		}

		// Token: 0x04001C8E RID: 7310
		[SerializeField]
		private bool m_AllowSwitchOff;

		// Token: 0x04001C8F RID: 7311
		private List<ExtensionsToggle> m_Toggles = new List<ExtensionsToggle>();

		// Token: 0x04001C90 RID: 7312
		public ExtensionsToggleGroup.ToggleGroupEvent onToggleGroupChanged = new ExtensionsToggleGroup.ToggleGroupEvent();

		// Token: 0x04001C91 RID: 7313
		public ExtensionsToggleGroup.ToggleGroupEvent onToggleGroupToggleChanged = new ExtensionsToggleGroup.ToggleGroupEvent();

		// Token: 0x04001C92 RID: 7314
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ExtensionsToggle <SelectedToggle>k__BackingField;

		// Token: 0x04001C93 RID: 7315
		[CompilerGenerated]
		private static Predicate<ExtensionsToggle> <>f__am$cache0;

		// Token: 0x04001C94 RID: 7316
		[CompilerGenerated]
		private static Func<ExtensionsToggle, bool> <>f__am$cache1;

		// Token: 0x02000547 RID: 1351
		[Serializable]
		public class ToggleGroupEvent : UnityEvent<bool>
		{
			// Token: 0x0600227D RID: 8829 RVA: 0x000C51C4 File Offset: 0x000C35C4
			public ToggleGroupEvent()
			{
			}
		}
	}
}
