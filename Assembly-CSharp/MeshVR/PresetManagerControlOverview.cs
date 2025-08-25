using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000CFA RID: 3322
	public class PresetManagerControlOverview : JSONStorable
	{
		// Token: 0x0600652D RID: 25901 RVA: 0x002630ED File Offset: 0x002614ED
		public PresetManagerControlOverview()
		{
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x002630F8 File Offset: 0x002614F8
		public void SyncPresetManagerControlLockParams(PresetManagerControl pmc)
		{
			Toggle toggle;
			if (this.presetManagerControlLockParamsToToggle.TryGetValue(pmc, out toggle))
			{
				toggle.isOn = pmc.lockParams;
			}
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x00263124 File Offset: 0x00261524
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				PresetManagerControlOverviewUI componentInChildren = t.GetComponentInChildren<PresetManagerControlOverviewUI>(true);
				if (componentInChildren != null && componentInChildren.toggles != null)
				{
					UIDynamicToggle[] toggles = componentInChildren.toggles;
					int num = this.presetManagerControls.Length;
					for (int i = 0; i < toggles.Length; i++)
					{
						if (toggles[i] != null)
						{
							if (i < num)
							{
								PresetManagerControlOverview.<InitUI>c__AnonStorey0 <InitUI>c__AnonStorey = new PresetManagerControlOverview.<InitUI>c__AnonStorey0();
								<InitUI>c__AnonStorey.presetManagerControl = this.presetManagerControls[i];
								toggles[i].gameObject.SetActive(true);
								toggles[i].label = <InitUI>c__AnonStorey.presetManagerControl.storeId;
								Toggle toggle = toggles[i].toggle;
								this.presetManagerControlLockParamsToToggle.Add(<InitUI>c__AnonStorey.presetManagerControl, toggle);
								toggle.isOn = <InitUI>c__AnonStorey.presetManagerControl.lockParams;
								toggle.onValueChanged.AddListener(new UnityAction<bool>(<InitUI>c__AnonStorey.<>m__0));
							}
							else
							{
								toggles[i].gameObject.SetActive(false);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x00263232 File Offset: 0x00261632
		protected virtual void Init()
		{
			this.presetManagerControlLockParamsToToggle = new Dictionary<PresetManagerControl, Toggle>();
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x0026323F File Offset: 0x0026163F
		protected override void Awake()
		{
			if (!this.awakecalled)
			{
				base.Awake();
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}

		// Token: 0x040054DB RID: 21723
		public PresetManagerControl[] presetManagerControls;

		// Token: 0x040054DC RID: 21724
		protected Dictionary<PresetManagerControl, Toggle> presetManagerControlLockParamsToToggle;

		// Token: 0x0200102A RID: 4138
		[CompilerGenerated]
		private sealed class <InitUI>c__AnonStorey0
		{
			// Token: 0x06007727 RID: 30503 RVA: 0x00263264 File Offset: 0x00261664
			public <InitUI>c__AnonStorey0()
			{
			}

			// Token: 0x06007728 RID: 30504 RVA: 0x0026326C File Offset: 0x0026166C
			internal void <>m__0(bool b)
			{
				this.presetManagerControl.lockParams = b;
			}

			// Token: 0x04006B39 RID: 27449
			internal PresetManagerControl presetManagerControl;
		}
	}
}
