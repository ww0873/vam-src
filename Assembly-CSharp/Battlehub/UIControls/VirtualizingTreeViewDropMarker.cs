using System;
using UnityEngine;

namespace Battlehub.UIControls
{
	// Token: 0x0200029A RID: 666
	[RequireComponent(typeof(RectTransform))]
	public class VirtualizingTreeViewDropMarker : VirtualizingItemDropMarker
	{
		// Token: 0x06000FBA RID: 4026 RVA: 0x0005A376 File Offset: 0x00058776
		public VirtualizingTreeViewDropMarker()
		{
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0005A37E File Offset: 0x0005877E
		// (set) Token: 0x06000FBC RID: 4028 RVA: 0x0005A386 File Offset: 0x00058786
		public override ItemDropAction Action
		{
			get
			{
				return base.Action;
			}
			set
			{
				base.Action = value;
				this.ChildGraphics.SetActive(base.Action == ItemDropAction.SetLastChild);
				this.SiblingGraphics.SetActive(base.Action != ItemDropAction.SetLastChild);
			}
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0005A3BA File Offset: 0x000587BA
		protected override void AwakeOverride()
		{
			base.AwakeOverride();
			this.m_treeView = base.GetComponentInParent<VirtualizingTreeView>();
			this.m_siblingGraphicsRectTransform = this.SiblingGraphics.GetComponent<RectTransform>();
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0005A3E0 File Offset: 0x000587E0
		public override void SetTraget(VirtualizingItemContainer item)
		{
			base.SetTraget(item);
			if (item == null)
			{
				return;
			}
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)item;
			if (virtualizingTreeViewItem != null)
			{
				this.m_siblingGraphicsRectTransform.offsetMin = new Vector2(virtualizingTreeViewItem.Indent, this.m_siblingGraphicsRectTransform.offsetMin.y);
			}
			else
			{
				this.m_siblingGraphicsRectTransform.offsetMin = new Vector2(0f, this.m_siblingGraphicsRectTransform.offsetMin.y);
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0005A46C File Offset: 0x0005886C
		public override void SetPosition(Vector2 position)
		{
			if (base.Item == null)
			{
				return;
			}
			if (!this.m_treeView.CanReparent)
			{
				base.SetPosition(position);
				return;
			}
			RectTransform rectTransform = base.Item.RectTransform;
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.Item;
			Vector2 sizeDelta = this.m_rectTransform.sizeDelta;
			sizeDelta.y = rectTransform.rect.height;
			this.m_rectTransform.sizeDelta = sizeDelta;
			Camera cam = null;
			if (base.ParentCanvas.renderMode == RenderMode.WorldSpace || base.ParentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				cam = this.m_treeView.Camera;
			}
			Vector2 vector;
			if (!this.m_treeView.CanReorder)
			{
				if (!virtualizingTreeViewItem.CanDrop)
				{
					return;
				}
				this.Action = ItemDropAction.SetLastChild;
				base.RectTransform.position = rectTransform.position;
			}
			else if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, cam, out vector))
			{
				if (vector.y > -rectTransform.rect.height / 4f)
				{
					this.Action = ItemDropAction.SetPrevSibling;
					base.RectTransform.position = rectTransform.position;
				}
				else if (vector.y < rectTransform.rect.height / 4f - rectTransform.rect.height && !virtualizingTreeViewItem.HasChildren)
				{
					this.Action = ItemDropAction.SetNextSibling;
					base.RectTransform.position = rectTransform.position;
					base.RectTransform.localPosition = base.RectTransform.localPosition - new Vector3(0f, rectTransform.rect.height * base.ParentCanvas.scaleFactor, 0f);
				}
				else
				{
					if (!virtualizingTreeViewItem.CanDrop)
					{
						return;
					}
					this.Action = ItemDropAction.SetLastChild;
					base.RectTransform.position = rectTransform.position;
				}
			}
		}

		// Token: 0x04000E47 RID: 3655
		private VirtualizingTreeView m_treeView;

		// Token: 0x04000E48 RID: 3656
		private RectTransform m_siblingGraphicsRectTransform;

		// Token: 0x04000E49 RID: 3657
		public GameObject ChildGraphics;
	}
}
