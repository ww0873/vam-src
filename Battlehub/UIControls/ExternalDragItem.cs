using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
	// Token: 0x02000265 RID: 613
	public class ExternalDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06000D09 RID: 3337 RVA: 0x0004E02A File Offset: 0x0004C42A
		public ExternalDragItem()
		{
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0004E032 File Offset: 0x0004C432
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			this.TreeView.ExternalBeginDrag(eventData.position);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0004E04A File Offset: 0x0004C44A
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			this.TreeView.ExternalItemDrag(eventData.position);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0004E062 File Offset: 0x0004C462
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (this.TreeView.DropTarget != null)
			{
				this.TreeView.AddChild(this.TreeView.DropTarget, new GameObject());
			}
			this.TreeView.ExternalItemDrop();
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0004E09C File Offset: 0x0004C49C
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (this.TreeView.DropTarget != null)
			{
				GameObject gameObject = (GameObject)this.TreeView.DropTarget;
				GameObject gameObject2 = new GameObject();
				TreeViewItem treeViewItem = (TreeViewItem)this.TreeView.GetItemContainer(this.TreeView.DropTarget);
				if (this.TreeView.DropAction == ItemDropAction.SetLastChild)
				{
					gameObject2.transform.SetParent(gameObject.transform);
					this.TreeView.AddChild(this.TreeView.DropTarget, gameObject2);
					treeViewItem.CanExpand = true;
					treeViewItem.IsExpanded = true;
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
					TreeViewItem treeViewItem2 = (TreeViewItem)this.TreeView.Insert(num, gameObject2);
					treeViewItem2.Parent = treeViewItem.Parent;
				}
			}
			this.TreeView.ExternalItemDrop();
		}

		// Token: 0x04000D1D RID: 3357
		public TreeView TreeView;
	}
}
