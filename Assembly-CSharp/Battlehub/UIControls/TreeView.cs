using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Battlehub.UIControls
{
	// Token: 0x02000286 RID: 646
	public class TreeView : ItemsControl<TreeViewItemDataBindingArgs>
	{
		// Token: 0x06000E57 RID: 3671 RVA: 0x000538AC File Offset: 0x00051CAC
		public TreeView()
		{
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06000E58 RID: 3672 RVA: 0x000538C4 File Offset: 0x00051CC4
		// (remove) Token: 0x06000E59 RID: 3673 RVA: 0x000538FC File Offset: 0x00051CFC
		public event EventHandler<ItemExpandingArgs> ItemExpanding
		{
			add
			{
				EventHandler<ItemExpandingArgs> eventHandler = this.ItemExpanding;
				EventHandler<ItemExpandingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemExpandingArgs>>(ref this.ItemExpanding, (EventHandler<ItemExpandingArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemExpandingArgs> eventHandler = this.ItemExpanding;
				EventHandler<ItemExpandingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemExpandingArgs>>(ref this.ItemExpanding, (EventHandler<ItemExpandingArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00053932 File Offset: 0x00051D32
		protected override bool CanScroll
		{
			get
			{
				return base.CanScroll || this.CanReparent;
			}
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00053948 File Offset: 0x00051D48
		protected override void OnEnableOverride()
		{
			base.OnEnableOverride();
			TreeViewItem.ParentChanged += this.OnTreeViewItemParentChanged;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00053961 File Offset: 0x00051D61
		protected override void OnDisableOverride()
		{
			base.OnDisableOverride();
			TreeViewItem.ParentChanged -= this.OnTreeViewItemParentChanged;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0005397A File Offset: 0x00051D7A
		public TreeViewItem GetTreeViewItem(int siblingIndex)
		{
			return (TreeViewItem)base.GetItemContainer(siblingIndex);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00053988 File Offset: 0x00051D88
		public TreeViewItem GetTreeViewItem(object obj)
		{
			return (TreeViewItem)base.GetItemContainer(obj);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x00053998 File Offset: 0x00051D98
		public void AddChild(object parent, object item)
		{
			if (parent == null)
			{
				base.Add(item);
			}
			else
			{
				TreeViewItem treeViewItem = (TreeViewItem)base.GetItemContainer(parent);
				if (treeViewItem == null)
				{
					return;
				}
				int num = -1;
				if (treeViewItem.IsExpanded)
				{
					if (treeViewItem.HasChildren)
					{
						TreeViewItem treeViewItem2 = treeViewItem.LastDescendant();
						num = base.IndexOf(treeViewItem2.Item) + 1;
					}
					else
					{
						num = base.IndexOf(treeViewItem.Item) + 1;
					}
				}
				else
				{
					treeViewItem.CanExpand = true;
				}
				if (num > -1)
				{
					TreeViewItem treeViewItem3 = (TreeViewItem)this.Insert(num, item);
					treeViewItem3.Parent = treeViewItem;
				}
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00053A3C File Offset: 0x00051E3C
		public override void Remove(object item)
		{
			throw new NotSupportedException("This method is not supported for TreeView use RemoveChild instead");
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00053A48 File Offset: 0x00051E48
		public void RemoveChild(object parent, object item, bool isLastChild)
		{
			if (base.IsDropInProgress)
			{
				return;
			}
			if (parent == null)
			{
				base.Remove(item);
			}
			else if (base.GetItemContainer(item) != null)
			{
				base.Remove(item);
			}
			else if (isLastChild)
			{
				TreeViewItem treeViewItem = (TreeViewItem)base.GetItemContainer(parent);
				if (treeViewItem)
				{
					treeViewItem.CanExpand = false;
				}
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00053AB8 File Offset: 0x00051EB8
		public void ChangeParent(object parent, object item)
		{
			if (base.IsDropInProgress)
			{
				return;
			}
			ItemContainer itemContainer = base.GetItemContainer(item);
			if (itemContainer == null)
			{
				this.AddChild(parent, item);
				return;
			}
			ItemContainer itemContainer2 = base.GetItemContainer(parent);
			ItemContainer[] dragItems = new ItemContainer[]
			{
				itemContainer
			};
			if (this.CanDrop(dragItems, itemContainer2))
			{
				this.Drop(dragItems, itemContainer2, ItemDropAction.SetLastChild);
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00053B18 File Offset: 0x00051F18
		public bool IsExpanded(object item)
		{
			TreeViewItem treeViewItem = (TreeViewItem)base.GetItemContainer(item);
			return !(treeViewItem == null) && treeViewItem.IsExpanded;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00053B48 File Offset: 0x00051F48
		public void Expand(TreeViewItem item)
		{
			if (this.m_expandSilently)
			{
				return;
			}
			if (this.ItemExpanding != null)
			{
				ItemExpandingArgs itemExpandingArgs = new ItemExpandingArgs(item.Item);
				this.ItemExpanding(this, itemExpandingArgs);
				IEnumerable children = itemExpandingArgs.Children;
				int num = item.transform.GetSiblingIndex();
				int num2 = base.IndexOf(item.Item);
				item.CanExpand = (children != null);
				if (item.CanExpand)
				{
					IEnumerator enumerator = children.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							num++;
							num2++;
							base.InsertItem(num2, obj);
							TreeViewItem treeViewItem = (TreeViewItem)base.InstantiateItemContainer(num);
							treeViewItem.Item = obj;
							treeViewItem.Parent = item;
							this.DataBindItem(obj, treeViewItem);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					base.UpdateSelectedItemIndex();
				}
			}
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00053C4C File Offset: 0x0005204C
		public void Collapse(TreeViewItem item)
		{
			int siblingIndex = item.transform.GetSiblingIndex();
			int num = base.IndexOf(item.Item);
			this.Collapse(item, siblingIndex + 1, num + 1);
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00053C80 File Offset: 0x00052080
		private void Unselect(List<object> selectedItems, TreeViewItem item, ref int containerIndex, ref int itemIndex)
		{
			for (;;)
			{
				TreeViewItem treeViewItem = (TreeViewItem)base.GetItemContainer(containerIndex);
				if (treeViewItem == null || treeViewItem.Parent != item)
				{
					break;
				}
				containerIndex++;
				itemIndex++;
				selectedItems.Remove(treeViewItem.Item);
				this.Unselect(selectedItems, treeViewItem, ref containerIndex, ref itemIndex);
			}
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00053CE8 File Offset: 0x000520E8
		private void Collapse(TreeViewItem item, int containerIndex, int itemIndex)
		{
			for (;;)
			{
				TreeViewItem treeViewItem = (TreeViewItem)base.GetItemContainer(containerIndex);
				if (treeViewItem == null || treeViewItem.Parent != item)
				{
					break;
				}
				this.Collapse(treeViewItem, containerIndex + 1, itemIndex + 1);
				base.RemoveItemAt(itemIndex);
				base.DestroyItemContainer(containerIndex);
			}
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00053D44 File Offset: 0x00052144
		protected override ItemContainer InstantiateItemContainerOverride(GameObject container)
		{
			TreeViewItem treeViewItem = container.GetComponent<TreeViewItem>();
			if (treeViewItem == null)
			{
				treeViewItem = container.AddComponent<TreeViewItem>();
				treeViewItem.gameObject.name = "TreeViewItem";
			}
			return treeViewItem;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00053D7C File Offset: 0x0005217C
		protected override void DestroyItem(object item)
		{
			TreeViewItem treeViewItem = (TreeViewItem)base.GetItemContainer(item);
			if (treeViewItem != null)
			{
				this.Collapse(treeViewItem);
				base.DestroyItem(item);
				if (treeViewItem.Parent != null && !treeViewItem.Parent.HasChildren)
				{
					treeViewItem.Parent.CanExpand = false;
				}
			}
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00053DE0 File Offset: 0x000521E0
		public override void DataBindItem(object item, ItemContainer itemContainer)
		{
			TreeViewItemDataBindingArgs treeViewItemDataBindingArgs = new TreeViewItemDataBindingArgs();
			treeViewItemDataBindingArgs.Item = item;
			treeViewItemDataBindingArgs.ItemPresenter = ((!(itemContainer.ItemPresenter == null)) ? itemContainer.ItemPresenter : base.gameObject);
			treeViewItemDataBindingArgs.EditorPresenter = ((!(itemContainer.EditorPresenter == null)) ? itemContainer.EditorPresenter : base.gameObject);
			base.RaiseItemDataBinding(treeViewItemDataBindingArgs);
			TreeViewItem treeViewItem = (TreeViewItem)itemContainer;
			treeViewItem.CanExpand = treeViewItemDataBindingArgs.HasChildren;
			treeViewItem.CanEdit = treeViewItemDataBindingArgs.CanEdit;
			treeViewItem.CanDrag = treeViewItemDataBindingArgs.CanDrag;
			treeViewItem.CanDrop = treeViewItemDataBindingArgs.CanDrop;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00053E88 File Offset: 0x00052288
		protected override bool CanDrop(ItemContainer[] dragItems, ItemContainer dropTarget)
		{
			if (!base.CanDrop(dragItems, dropTarget))
			{
				return false;
			}
			TreeViewItem treeViewItem = (TreeViewItem)dropTarget;
			if (treeViewItem == null)
			{
				return true;
			}
			foreach (ItemContainer itemContainer in dragItems)
			{
				TreeViewItem ancestor = (TreeViewItem)itemContainer;
				if (treeViewItem.IsDescendantOf(ancestor))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00053EEC File Offset: 0x000522EC
		private void OnTreeViewItemParentChanged(object sender, ParentChangedEventArgs e)
		{
			TreeViewItem treeViewItem = (TreeViewItem)sender;
			if (!base.CanHandleEvent(treeViewItem))
			{
				return;
			}
			TreeViewItem oldParent = e.OldParent;
			if (base.DropMarker.Action != ItemDropAction.SetLastChild && base.DropMarker.Action != ItemDropAction.None)
			{
				if (oldParent != null && !oldParent.HasChildren)
				{
					oldParent.CanExpand = false;
				}
				return;
			}
			TreeViewItem newParent = e.NewParent;
			if (newParent != null)
			{
				if (newParent.CanExpand)
				{
					newParent.IsExpanded = true;
				}
				else
				{
					newParent.CanExpand = true;
					try
					{
						this.m_expandSilently = true;
						newParent.IsExpanded = true;
					}
					finally
					{
						this.m_expandSilently = false;
					}
				}
			}
			TreeViewItem treeViewItem2 = treeViewItem.FirstChild();
			TreeViewItem treeViewItem3;
			if (newParent != null)
			{
				treeViewItem3 = newParent.LastChild();
				if (treeViewItem3 == null)
				{
					treeViewItem3 = newParent;
				}
			}
			else
			{
				treeViewItem3 = (TreeViewItem)base.LastItemContainer();
			}
			if (treeViewItem3 != treeViewItem)
			{
				TreeViewItem treeViewItem4 = treeViewItem3.LastDescendant();
				if (treeViewItem4 != null)
				{
					treeViewItem3 = treeViewItem4;
				}
				if (!treeViewItem3.IsDescendantOf(treeViewItem))
				{
					base.SetNextSibling(treeViewItem3, treeViewItem);
				}
			}
			if (treeViewItem2 != null)
			{
				this.MoveSubtree(treeViewItem, treeViewItem2);
			}
			if (oldParent != null && !oldParent.HasChildren)
			{
				oldParent.CanExpand = false;
			}
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00054060 File Offset: 0x00052460
		private void MoveSubtree(TreeViewItem parent, TreeViewItem child)
		{
			int siblingIndex = parent.transform.GetSiblingIndex();
			int num = child.transform.GetSiblingIndex();
			bool flag = false;
			if (siblingIndex < num)
			{
				flag = true;
			}
			TreeViewItem treeViewItem = parent;
			while (child != null && child.IsDescendantOf(parent))
			{
				if (treeViewItem == child)
				{
					break;
				}
				base.SetNextSibling(treeViewItem, child);
				treeViewItem = child;
				if (flag)
				{
					num++;
				}
				child = (TreeViewItem)base.GetItemContainer(num);
			}
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000540E4 File Offset: 0x000524E4
		protected override void Drop(ItemContainer[] dragItems, ItemContainer dropTarget, ItemDropAction action)
		{
			TreeViewItem treeViewItem = (TreeViewItem)dropTarget;
			if (action == ItemDropAction.SetLastChild)
			{
				foreach (TreeViewItem treeViewItem2 in dragItems)
				{
					if (treeViewItem != treeViewItem2)
					{
						treeViewItem2.Parent = treeViewItem;
					}
				}
			}
			else if (action == ItemDropAction.SetPrevSibling)
			{
				for (int j = 0; j < dragItems.Length; j++)
				{
					this.SetPrevSibling(treeViewItem, dragItems[j]);
				}
			}
			else if (action == ItemDropAction.SetNextSibling)
			{
				for (int k = dragItems.Length - 1; k >= 0; k--)
				{
					this.SetNextSibling(treeViewItem, dragItems[k]);
				}
			}
			base.UpdateSelectedItemIndex();
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00054190 File Offset: 0x00052590
		protected override void SetNextSibling(ItemContainer sibling, ItemContainer nextSibling)
		{
			TreeViewItem treeViewItem = (TreeViewItem)sibling;
			TreeViewItem treeViewItem2 = treeViewItem.LastDescendant();
			if (treeViewItem2 == null)
			{
				treeViewItem2 = treeViewItem;
			}
			TreeViewItem treeViewItem3 = (TreeViewItem)nextSibling;
			TreeViewItem treeViewItem4 = treeViewItem3.FirstChild();
			base.SetNextSibling(treeViewItem2, nextSibling);
			if (treeViewItem4 != null)
			{
				this.MoveSubtree(treeViewItem3, treeViewItem4);
			}
			treeViewItem3.Parent = treeViewItem.Parent;
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x000541F0 File Offset: 0x000525F0
		protected override void SetPrevSibling(ItemContainer sibling, ItemContainer prevSibling)
		{
			TreeViewItem treeViewItem = (TreeViewItem)sibling;
			TreeViewItem treeViewItem2 = (TreeViewItem)prevSibling;
			TreeViewItem treeViewItem3 = treeViewItem2.FirstChild();
			base.SetPrevSibling(sibling, prevSibling);
			if (treeViewItem3 != null)
			{
				this.MoveSubtree(treeViewItem2, treeViewItem3);
			}
			treeViewItem2.Parent = treeViewItem.Parent;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0005423C File Offset: 0x0005263C
		public void UpdateIndent(object obj)
		{
			TreeViewItem treeViewItem = this.GetTreeViewItem(obj);
			if (treeViewItem == null)
			{
				return;
			}
			treeViewItem.UpdateIndent();
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00054264 File Offset: 0x00052664
		public void FixScrollRect()
		{
			Canvas.ForceUpdateCanvases();
			RectTransform component = this.Panel.GetComponent<RectTransform>();
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, component.rect.height - 0.01f);
		}

		// Token: 0x04000DC0 RID: 3520
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemExpandingArgs> ItemExpanding;

		// Token: 0x04000DC1 RID: 3521
		public int Indent = 20;

		// Token: 0x04000DC2 RID: 3522
		public bool CanReparent = true;

		// Token: 0x04000DC3 RID: 3523
		public bool AutoExpand;

		// Token: 0x04000DC4 RID: 3524
		private bool m_expandSilently;
	}
}
