using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200055C RID: 1372
	[AddComponentMenu("UI/Extensions/UI Highlightable Extension")]
	[RequireComponent(typeof(RectTransform), typeof(Graphic))]
	public class UIHighlightable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060022D8 RID: 8920 RVA: 0x000C7375 File Offset: 0x000C5775
		public UIHighlightable()
		{
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x000C73B0 File Offset: 0x000C57B0
		// (set) Token: 0x060022DA RID: 8922 RVA: 0x000C73B8 File Offset: 0x000C57B8
		public bool Interactable
		{
			get
			{
				return this.m_Interactable;
			}
			set
			{
				this.m_Interactable = value;
				this.HighlightInteractable(this.m_Graphic);
				this.OnInteractableChanged.Invoke(this.m_Interactable);
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x000C73DE File Offset: 0x000C57DE
		// (set) Token: 0x060022DC RID: 8924 RVA: 0x000C73E6 File Offset: 0x000C57E6
		public bool ClickToHold
		{
			get
			{
				return this.m_ClickToHold;
			}
			set
			{
				this.m_ClickToHold = value;
			}
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000C73EF File Offset: 0x000C57EF
		private void Awake()
		{
			this.m_Graphic = base.GetComponent<Graphic>();
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000C73FD File Offset: 0x000C57FD
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.Interactable && !this.m_Pressed)
			{
				this.m_Highlighted = true;
				this.m_Graphic.color = this.HighlightedColor;
			}
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000C742D File Offset: 0x000C582D
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.Interactable && !this.m_Pressed)
			{
				this.m_Highlighted = false;
				this.m_Graphic.color = this.NormalColor;
			}
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000C745D File Offset: 0x000C585D
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.Interactable)
			{
				this.m_Graphic.color = this.PressedColor;
				if (this.ClickToHold)
				{
					this.m_Pressed = !this.m_Pressed;
				}
			}
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000C7495 File Offset: 0x000C5895
		public void OnPointerUp(PointerEventData eventData)
		{
			if (!this.m_Pressed)
			{
				this.HighlightInteractable(this.m_Graphic);
			}
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x000C74B0 File Offset: 0x000C58B0
		private void HighlightInteractable(Graphic graphic)
		{
			if (this.m_Interactable)
			{
				if (this.m_Highlighted)
				{
					graphic.color = this.HighlightedColor;
				}
				else
				{
					graphic.color = this.NormalColor;
				}
			}
			else
			{
				graphic.color = this.DisabledColor;
			}
		}

		// Token: 0x04001CD9 RID: 7385
		private Graphic m_Graphic;

		// Token: 0x04001CDA RID: 7386
		private bool m_Highlighted;

		// Token: 0x04001CDB RID: 7387
		private bool m_Pressed;

		// Token: 0x04001CDC RID: 7388
		[SerializeField]
		[Tooltip("Can this panel be interacted with or is it disabled? (does not affect child components)")]
		private bool m_Interactable = true;

		// Token: 0x04001CDD RID: 7389
		[SerializeField]
		[Tooltip("Does the panel remain in the pressed state when clicked? (default false)")]
		private bool m_ClickToHold;

		// Token: 0x04001CDE RID: 7390
		[Tooltip("The default color for the panel")]
		public Color NormalColor = Color.grey;

		// Token: 0x04001CDF RID: 7391
		[Tooltip("The color for the panel when a mouse is over it or it is in focus")]
		public Color HighlightedColor = Color.yellow;

		// Token: 0x04001CE0 RID: 7392
		[Tooltip("The color for the panel when it is clicked/held")]
		public Color PressedColor = Color.green;

		// Token: 0x04001CE1 RID: 7393
		[Tooltip("The color for the panel when it is not interactable (see Interactable)")]
		public Color DisabledColor = Color.gray;

		// Token: 0x04001CE2 RID: 7394
		[Tooltip("Event for when the panel is enabled / disabled, to enable disabling / enabling of child or other gameobjects")]
		public UIHighlightable.InteractableChangedEvent OnInteractableChanged;

		// Token: 0x0200055D RID: 1373
		[Serializable]
		public class InteractableChangedEvent : UnityEvent<bool>
		{
			// Token: 0x060022E3 RID: 8931 RVA: 0x000C7501 File Offset: 0x000C5901
			public InteractableChangedEvent()
			{
			}
		}
	}
}
