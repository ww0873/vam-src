using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	// Token: 0x02000542 RID: 1346
	[AddComponentMenu("UI/Extensions/Extensions Toggle", 31)]
	[RequireComponent(typeof(RectTransform))]
	public class ExtensionsToggle : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement, IEventSystemHandler
	{
		// Token: 0x06002255 RID: 8789 RVA: 0x000C4C3D File Offset: 0x000C303D
		protected ExtensionsToggle()
		{
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x000C4C62 File Offset: 0x000C3062
		// (set) Token: 0x06002257 RID: 8791 RVA: 0x000C4C6A File Offset: 0x000C306A
		public ExtensionsToggleGroup Group
		{
			get
			{
				return this.m_Group;
			}
			set
			{
				this.m_Group = value;
				this.SetToggleGroup(this.m_Group, true);
				this.PlayEffect(true);
			}
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000C4C87 File Offset: 0x000C3087
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06002259 RID: 8793 RVA: 0x000C4C89 File Offset: 0x000C3089
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x0600225A RID: 8794 RVA: 0x000C4C8B File Offset: 0x000C308B
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x000C4C8D File Offset: 0x000C308D
		protected override void OnEnable()
		{
			base.OnEnable();
			this.SetToggleGroup(this.m_Group, false);
			this.PlayEffect(true);
		}

		// Token: 0x0600225C RID: 8796 RVA: 0x000C4CA9 File Offset: 0x000C30A9
		protected override void OnDisable()
		{
			this.SetToggleGroup(null, false);
			base.OnDisable();
		}

		// Token: 0x0600225D RID: 8797 RVA: 0x000C4CBC File Offset: 0x000C30BC
		protected override void OnDidApplyAnimationProperties()
		{
			if (this.graphic != null)
			{
				bool flag = !Mathf.Approximately(this.graphic.canvasRenderer.GetColor().a, 0f);
				if (this.m_IsOn != flag)
				{
					this.m_IsOn = flag;
					this.Set(!flag);
				}
			}
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x000C4D24 File Offset: 0x000C3124
		private void SetToggleGroup(ExtensionsToggleGroup newGroup, bool setMemberValue)
		{
			ExtensionsToggleGroup group = this.m_Group;
			if (this.m_Group != null)
			{
				this.m_Group.UnregisterToggle(this);
			}
			if (setMemberValue)
			{
				this.m_Group = newGroup;
			}
			if (this.m_Group != null && this.IsActive())
			{
				this.m_Group.RegisterToggle(this);
			}
			if (newGroup != null && newGroup != group && this.IsOn && this.IsActive())
			{
				this.m_Group.NotifyToggleOn(this);
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x000C4DC4 File Offset: 0x000C31C4
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x000C4DCC File Offset: 0x000C31CC
		public bool IsOn
		{
			get
			{
				return this.m_IsOn;
			}
			set
			{
				this.Set(value);
			}
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000C4DD5 File Offset: 0x000C31D5
		private void Set(bool value)
		{
			this.Set(value, true);
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000C4DE0 File Offset: 0x000C31E0
		private void Set(bool value, bool sendCallback)
		{
			if (this.m_IsOn == value)
			{
				return;
			}
			this.m_IsOn = value;
			if (this.m_Group != null && this.IsActive() && (this.m_IsOn || (!this.m_Group.AnyTogglesOn() && !this.m_Group.AllowSwitchOff)))
			{
				this.m_IsOn = true;
				this.m_Group.NotifyToggleOn(this);
			}
			this.PlayEffect(this.toggleTransition == ExtensionsToggle.ToggleTransition.None);
			if (sendCallback)
			{
				this.onValueChanged.Invoke(this.m_IsOn);
				this.onToggleChanged.Invoke(this);
			}
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x000C4E90 File Offset: 0x000C3290
		private void PlayEffect(bool instant)
		{
			if (this.graphic == null)
			{
				return;
			}
			this.graphic.CrossFadeAlpha((!this.m_IsOn) ? 0f : 1f, (!instant) ? 0.1f : 0f, true);
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000C4EEA File Offset: 0x000C32EA
		protected override void Start()
		{
			this.PlayEffect(true);
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000C4EF3 File Offset: 0x000C32F3
		private void InternalToggle()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			this.IsOn = !this.IsOn;
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000C4F1B File Offset: 0x000C331B
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.InternalToggle();
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000C4F2F File Offset: 0x000C332F
		public virtual void OnSubmit(BaseEventData eventData)
		{
			this.InternalToggle();
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000C4F37 File Offset: 0x000C3337
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000C4F3F File Offset: 0x000C333F
		bool ICanvasElement.IsDestroyed()
		{
			return base.IsDestroyed();
		}

		// Token: 0x04001C84 RID: 7300
		public string UniqueID;

		// Token: 0x04001C85 RID: 7301
		public ExtensionsToggle.ToggleTransition toggleTransition = ExtensionsToggle.ToggleTransition.Fade;

		// Token: 0x04001C86 RID: 7302
		public Graphic graphic;

		// Token: 0x04001C87 RID: 7303
		[SerializeField]
		private ExtensionsToggleGroup m_Group;

		// Token: 0x04001C88 RID: 7304
		[Tooltip("Use this event if you only need the bool state of the toggle that was changed")]
		public ExtensionsToggle.ToggleEvent onValueChanged = new ExtensionsToggle.ToggleEvent();

		// Token: 0x04001C89 RID: 7305
		[Tooltip("Use this event if you need access to the toggle that was changed")]
		public ExtensionsToggle.ToggleEventObject onToggleChanged = new ExtensionsToggle.ToggleEventObject();

		// Token: 0x04001C8A RID: 7306
		[FormerlySerializedAs("m_IsActive")]
		[Tooltip("Is the toggle currently on or off?")]
		[SerializeField]
		private bool m_IsOn;

		// Token: 0x02000543 RID: 1347
		public enum ToggleTransition
		{
			// Token: 0x04001C8C RID: 7308
			None,
			// Token: 0x04001C8D RID: 7309
			Fade
		}

		// Token: 0x02000544 RID: 1348
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
			// Token: 0x0600226A RID: 8810 RVA: 0x000C4F47 File Offset: 0x000C3347
			public ToggleEvent()
			{
			}
		}

		// Token: 0x02000545 RID: 1349
		[Serializable]
		public class ToggleEventObject : UnityEvent<ExtensionsToggle>
		{
			// Token: 0x0600226B RID: 8811 RVA: 0x000C4F4F File Offset: 0x000C334F
			public ToggleEventObject()
			{
			}
		}
	}
}
