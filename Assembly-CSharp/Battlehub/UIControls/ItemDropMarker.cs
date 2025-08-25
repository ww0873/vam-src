using System;
using UnityEngine;

namespace Battlehub.UIControls
{
	// Token: 0x0200026F RID: 623
	[RequireComponent(typeof(RectTransform))]
	public class ItemDropMarker : MonoBehaviour
	{
		// Token: 0x06000D7C RID: 3452 RVA: 0x00052771 File Offset: 0x00050B71
		public ItemDropMarker()
		{
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x00052779 File Offset: 0x00050B79
		protected Canvas ParentCanvas
		{
			get
			{
				return this.m_parentCanvas;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00052781 File Offset: 0x00050B81
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x00052789 File Offset: 0x00050B89
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

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x00052792 File Offset: 0x00050B92
		public RectTransform RectTransform
		{
			get
			{
				return this.m_rectTransform;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x0005279A File Offset: 0x00050B9A
		protected ItemContainer Item
		{
			get
			{
				return this.m_item;
			}
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000527A2 File Offset: 0x00050BA2
		private void Awake()
		{
			this.m_rectTransform = base.GetComponent<RectTransform>();
			this.SiblingGraphics.SetActive(true);
			this.m_parentCanvas = base.GetComponentInParent<Canvas>();
			this.m_itemsControl = base.GetComponentInParent<ItemsControl>();
			this.AwakeOverride();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000527DA File Offset: 0x00050BDA
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000527DC File Offset: 0x00050BDC
		public virtual void SetTraget(ItemContainer item)
		{
			base.gameObject.SetActive(item != null);
			this.m_item = item;
			if (this.m_item == null)
			{
				this.Action = ItemDropAction.None;
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00052810 File Offset: 0x00050C10
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

		// Token: 0x04000D4B RID: 3403
		private Canvas m_parentCanvas;

		// Token: 0x04000D4C RID: 3404
		private ItemsControl m_itemsControl;

		// Token: 0x04000D4D RID: 3405
		public GameObject SiblingGraphics;

		// Token: 0x04000D4E RID: 3406
		private ItemDropAction m_action;

		// Token: 0x04000D4F RID: 3407
		private RectTransform m_rectTransform;

		// Token: 0x04000D50 RID: 3408
		private ItemContainer m_item;
	}
}
