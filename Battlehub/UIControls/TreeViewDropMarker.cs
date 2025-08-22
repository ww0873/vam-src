using System;
using UnityEngine;

namespace Battlehub.UIControls
{
	// Token: 0x02000287 RID: 647
	[RequireComponent(typeof(RectTransform))]
	public class TreeViewDropMarker : ItemDropMarker
	{
		// Token: 0x06000E73 RID: 3699 RVA: 0x0005429D File Offset: 0x0005269D
		public TreeViewDropMarker()
		{
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x000542A5 File Offset: 0x000526A5
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x000542AD File Offset: 0x000526AD
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

		// Token: 0x06000E76 RID: 3702 RVA: 0x000542E1 File Offset: 0x000526E1
		protected override void AwakeOverride()
		{
			base.AwakeOverride();
			this.m_treeView = base.GetComponentInParent<TreeView>();
			this.m_siblingGraphicsRectTransform = this.SiblingGraphics.GetComponent<RectTransform>();
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00054308 File Offset: 0x00052708
		public override void SetTraget(ItemContainer item)
		{
			base.SetTraget(item);
			if (item == null)
			{
				return;
			}
			TreeViewItem treeViewItem = (TreeViewItem)item;
			if (treeViewItem != null)
			{
				this.m_siblingGraphicsRectTransform.offsetMin = new Vector2((float)treeViewItem.Indent, this.m_siblingGraphicsRectTransform.offsetMin.y);
			}
			else
			{
				this.m_siblingGraphicsRectTransform.offsetMin = new Vector2(0f, this.m_siblingGraphicsRectTransform.offsetMin.y);
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00054394 File Offset: 0x00052794
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
			TreeViewItem treeViewItem = (TreeViewItem)base.Item;
			Camera cam = null;
			if (base.ParentCanvas.renderMode == RenderMode.WorldSpace || base.ParentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				cam = this.m_treeView.Camera;
			}
			Vector2 vector;
			if (!this.m_treeView.CanReorder)
			{
				if (!treeViewItem.CanDrop)
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
				else if (vector.y < rectTransform.rect.height / 4f - rectTransform.rect.height && !treeViewItem.HasChildren)
				{
					this.Action = ItemDropAction.SetNextSibling;
					base.RectTransform.position = rectTransform.position;
					base.RectTransform.localPosition = base.RectTransform.localPosition - new Vector3(0f, rectTransform.rect.height * base.ParentCanvas.scaleFactor, 0f);
				}
				else
				{
					if (!treeViewItem.CanDrop)
					{
						return;
					}
					this.Action = ItemDropAction.SetLastChild;
					base.RectTransform.position = rectTransform.position;
				}
			}
		}

		// Token: 0x04000DC5 RID: 3525
		private TreeView m_treeView;

		// Token: 0x04000DC6 RID: 3526
		private RectTransform m_siblingGraphicsRectTransform;

		// Token: 0x04000DC7 RID: 3527
		public GameObject ChildGraphics;
	}
}
