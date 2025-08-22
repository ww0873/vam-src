using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000288 RID: 648
	[RequireComponent(typeof(Toggle))]
	public class TreeViewExpander : MonoBehaviour
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x0005455C File Offset: 0x0005295C
		public TreeViewExpander()
		{
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x00054564 File Offset: 0x00052964
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x0005456C File Offset: 0x0005296C
		public bool CanExpand
		{
			get
			{
				return this.m_canExpand;
			}
			set
			{
				this.m_canExpand = value;
				if (!this.m_canExpand)
				{
					if (this.m_toggle != null)
					{
						this.m_toggle.isOn = false;
						this.m_toggle.enabled = false;
					}
					this.OffGraphic.enabled = false;
				}
				else if (this.m_toggle != null)
				{
					this.m_toggle.enabled = true;
					if (!this.IsOn)
					{
						this.OffGraphic.enabled = true;
					}
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x000545F9 File Offset: 0x000529F9
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x00054606 File Offset: 0x00052A06
		public bool IsOn
		{
			get
			{
				return this.m_toggle.isOn;
			}
			set
			{
				this.m_toggle.isOn = (value && this.m_canExpand);
			}
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00054624 File Offset: 0x00052A24
		private void Awake()
		{
			this.m_toggle = base.GetComponent<Toggle>();
			if (!this.m_canExpand)
			{
				this.m_toggle.isOn = false;
				this.m_toggle.enabled = false;
			}
			if (this.OffGraphic != null)
			{
				this.OffGraphic.enabled = (!this.m_toggle.isOn && this.m_canExpand);
			}
			this.m_toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000546B4 File Offset: 0x00052AB4
		private void OnEnable()
		{
			if (this.m_toggle != null)
			{
				if (this.OffGraphic != null)
				{
					this.OffGraphic.enabled = (!this.m_toggle.isOn && this.m_canExpand);
				}
				if (!this.m_canExpand)
				{
					this.m_toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
					this.m_toggle.isOn = true;
					this.m_toggle.isOn = false;
					this.m_toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
					this.m_toggle.enabled = false;
				}
			}
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0005476E File Offset: 0x00052B6E
		private void OnDestroy()
		{
			if (this.m_toggle != null)
			{
				this.m_toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
			}
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000547A0 File Offset: 0x00052BA0
		private void OnValueChanged(bool value)
		{
			if (!this.m_canExpand)
			{
				this.m_toggle.isOn = false;
				this.m_toggle.enabled = false;
			}
			if (this.OffGraphic != null)
			{
				this.OffGraphic.enabled = (!value && this.m_canExpand);
			}
		}

		// Token: 0x04000DC8 RID: 3528
		public Graphic OffGraphic;

		// Token: 0x04000DC9 RID: 3529
		private Toggle m_toggle;

		// Token: 0x04000DCA RID: 3530
		private bool m_canExpand;
	}
}
