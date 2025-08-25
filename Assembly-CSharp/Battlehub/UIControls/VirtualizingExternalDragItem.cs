using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
	// Token: 0x02000269 RID: 617
	public class VirtualizingExternalDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06000D2D RID: 3373 RVA: 0x0004EC9F File Offset: 0x0004D09F
		public VirtualizingExternalDragItem()
		{
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0004ECA7 File Offset: 0x0004D0A7
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			this.TreeView.ExternalBeginDrag(eventData.position);
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0004ECBF File Offset: 0x0004D0BF
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			this.TreeView.ExternalItemDrag(eventData.position);
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0004ECD7 File Offset: 0x0004D0D7
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (this.TreeView.DropTarget != null)
			{
				this.TreeView.AddChild(this.TreeView.DropTarget, new GameObject());
			}
			this.TreeView.ExternalItemDrop();
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0004ED10 File Offset: 0x0004D110
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (this.TreeView.DropTarget != null)
			{
				GameObject gameObject = (GameObject)this.TreeView.DropTarget;
				GameObject gameObject2 = new GameObject();
				VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)this.TreeView.GetItemContainer(this.TreeView.DropTarget);
				if (this.TreeView.DropAction == ItemDropAction.SetLastChild)
				{
					gameObject2.transform.SetParent(gameObject.transform);
					this.TreeView.AddChild(this.TreeView.DropTarget, gameObject2);
					virtualizingTreeViewItem.CanExpand = true;
					virtualizingTreeViewItem.IsExpanded = true;
				}
				else if (this.TreeView.DropAction != ItemDropAction.None)
				{
					int num;
					if (this.TreeView.DropAction == ItemDropAction.SetNextSibling)
					{
						num = this.TreeView.IndexOf(gameObject) + 1;
					}
					else
					{
						num = this.TreeView.IndexOf(gameObject);
					}
					gameObject2.transform.SetParent(gameObject.transform.parent);
					gameObject2.transform.SetSiblingIndex(num);
					TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)this.TreeView.Insert(num, gameObject2);
					VirtualizingTreeViewItem virtualizingTreeViewItem2 = (VirtualizingTreeViewItem)this.TreeView.GetItemContainer(gameObject2);
					if (virtualizingTreeViewItem2 != null)
					{
						virtualizingTreeViewItem2.Parent = virtualizingTreeViewItem.Parent;
					}
					else
					{
						treeViewItemContainerData.Parent = virtualizingTreeViewItem.Parent;
					}
				}
			}
			this.TreeView.ExternalItemDrop();
		}

		// Token: 0x04000D26 RID: 3366
		public VirtualizingTreeView TreeView;
	}
}
