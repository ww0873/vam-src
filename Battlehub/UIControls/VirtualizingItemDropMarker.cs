using System;
using UnityEngine;

namespace Battlehub.UIControls
{
	// Token: 0x0200028D RID: 653
	[RequireComponent(typeof(RectTransform))]
	public class VirtualizingItemDropMarker : MonoBehaviour
	{
		// Token: 0x06000ED8 RID: 3800 RVA: 0x00055AFD File Offset: 0x00053EFD
		public VirtualizingItemDropMarker()
		{
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00055B05 File Offset: 0x00053F05
		protected Canvas ParentCanvas
		{
			get
			{
				return this.m_parentCanvas;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00055B0D File Offset: 0x00053F0D
		// (set) Token: 0x06000EDB RID: 3803 RVA: 0x00055B15 File Offset: 0x00053F15
		public virtual ItemDropAction Action
		{
			get
			{
				return this.m_action;
			}
			set
			{
				this.m_action = value;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x00055B1E File Offset: 0x00053F1E
		public RectTransform RectTransform
		{
			get
			{
				return this.m_rectTransform;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00055B26 File Offset: 0x00053F26
		protected VirtualizingItemContainer Item
		{
			get
			{
				return this.m_item;
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00055B2E File Offset: 0x00053F2E
		private void Awake()
		{
			this.m_rectTransform = base.GetComponent<RectTransform>();
			this.SiblingGraphics.SetActive(true);
			this.m_parentCanvas = base.GetComponentInParent<Canvas>();
			this.m_itemsControl = base.GetComponentInParent<VirtualizingItemsControl>();
			this.AwakeOverride();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00055B66 File Offset: 0x00053F66
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00055B68 File Offset: 0x00053F68
		public virtual void SetTraget(VirtualizingItemContainer item)
		{
			base.gameObject.SetActive(item != null);
			this.m_item = item;
			if (this.m_item == null)
			{
				this.Action = ItemDropAction.None;
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00055B9C File Offset: 0x00053F9C
		public virtual void SetPosition(Vector2 position)
		{
			if (this.m_item == null)
			{
				return;
			}
			if (!this.m_itemsControl.CanReorder)
			{
				return;
			}
			RectTransform rectTransform = this.Item.RectTransform;
			Vector2 sizeDelta = this.m_rectTransform.sizeDelta;
			sizeDelta.y = rectTransform.rect.height;
			this.m_rectTransform.sizeDelta = sizeDelta;
			Camera cam = null;
			if (this.ParentCanvas.renderMode == RenderMode.WorldSpace || this.ParentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				cam = this.m_itemsControl.Camera;
			}
			Vector2 vector;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, cam, out vector))
			{
				if (vector.y > -rectTransform.rect.height / 2f)
				{
					this.Action = ItemDropAction.SetPrevSibling;
					this.RectTransform.position = rectTransform.position;
				}
				else
				{
					this.Action = ItemDropAction.SetNextSibling;
					this.RectTransform.position = rectTransform.position;
					this.RectTransform.localPosition = this.RectTransform.localPosition - new Vector3(0f, rectTransform.rect.height * this.ParentCanvas.scaleFactor, 0f);
				}
			}
		}

		// Token: 0x04000DF0 RID: 3568
		private VirtualizingItemsControl m_itemsControl;

		// Token: 0x04000DF1 RID: 3569
		private Canvas m_parentCanvas;

		// Token: 0x04000DF2 RID: 3570
		public GameObject SiblingGraphics;

		// Token: 0x04000DF3 RID: 3571
		private ItemDropAction m_action;

		// Token: 0x04000DF4 RID: 3572
		protected RectTransform m_rectTransform;

		// Token: 0x04000DF5 RID: 3573
		private VirtualizingItemContainer m_item;
	}
}
