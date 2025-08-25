using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Battlehub.UIControls
{
	// Token: 0x02000299 RID: 665
	public class VirtualizingTreeView : VirtualizingItemsControl<TreeViewItemDataBindingArgs>
	{
		// Token: 0x06000F9D RID: 3997 RVA: 0x000597A9 File Offset: 0x00057BA9
		public VirtualizingTreeView()
		{
		}

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06000F9E RID: 3998 RVA: 0x000597C0 File Offset: 0x00057BC0
		// (remove) Token: 0x06000F9F RID: 3999 RVA: 0x000597F8 File Offset: 0x00057BF8
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0005982E File Offset: 0x00057C2E
		protected override bool CanScroll
		{
			get
			{
				return base.CanScroll || this.CanReparent;
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00059844 File Offset: 0x00057C44
		protected override void OnEnableOverride()
		{
			base.OnEnableOverride();
			TreeViewItemContainerData.ParentChanged += this.OnTreeViewItemParentChanged;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0005985D File Offset: 0x00057C5D
		protected override void OnDisableOverride()
		{
			base.OnDisableOverride();
			TreeViewItemContainerData.ParentChanged -= this.OnTreeViewItemParentChanged;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00059878 File Offset: 0x00057C78
		protected override ItemContainerData InstantiateItemContainerData(object item)
		{
			return new TreeViewItemContainerData
			{
				Item = item
			};
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x00059894 File Offset: 0x00057C94
		public void AddChild(object parent, object item)
		{
			if (parent == null)
			{
				base.Add(item);
			}
			else
			{
				VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(parent);
				if (virtualizingTreeViewItem == null)
				{
					return;
				}
				int num = -1;
				if (virtualizingTreeViewItem.IsExpanded)
				{
					if (virtualizingTreeViewItem.HasChildren)
					{
						TreeViewItemContainerData treeViewItemContainerData = virtualizingTreeViewItem.LastDescendant();
						num = base.IndexOf(treeViewItemContainerData.Item) + 1;
					}
					else
					{
						num = base.IndexOf(virtualizingTreeViewItem.Item) + 1;
					}
				}
				else
				{
					virtualizingTreeViewItem.CanExpand = true;
				}
				if (num > -1)
				{
					TreeViewItemContainerData treeViewItemContainerData2 = (TreeViewItemContainerData)this.Insert(num, item);
					VirtualizingTreeViewItem virtualizingTreeViewItem2 = (VirtualizingTreeViewItem)base.GetItemContainer(item);
					if (virtualizingTreeViewItem2 != null)
					{
						virtualizingTreeViewItem2.Parent = virtualizingTreeViewItem.TreeViewItemData;
					}
					else
					{
						treeViewItemContainerData2.Parent = virtualizingTreeViewItem.TreeViewItemData;
					}
				}
			}
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0005996A File Offset: 0x00057D6A
		public override void Remove(object item)
		{
			throw new NotSupportedException("This method is not supported for TreeView use RemoveChild instead");
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00059978 File Offset: 0x00057D78
		public void RemoveChild(object parent, object item, bool isLastChild)
		{
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
				VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(parent);
				if (virtualizingTreeViewItem)
				{
					virtualizingTreeViewItem.CanExpand = false;
				}
			}
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x000599DC File Offset: 0x00057DDC
		public void ChangeParent(object parent, object item)
		{
			if (base.IsDropInProgress)
			{
				return;
			}
			ItemContainerData itemContainerData = base.GetItemContainerData(item);
			if (itemContainerData == null)
			{
				return;
			}
			ItemContainerData itemContainerData2 = base.GetItemContainerData(parent);
			ItemContainerData[] dragItems = new ItemContainerData[]
			{
				itemContainerData
			};
			if (this.CanDrop(dragItems, itemContainerData2))
			{
				this.Drop(dragItems, itemContainerData2, ItemDropAction.SetLastChild);
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00059A30 File Offset: 0x00057E30
		public bool IsExpanded(object item)
		{
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)base.GetItemContainerData(item);
			return treeViewItemContainerData != null && treeViewItemContainerData.IsExpanded;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00059A58 File Offset: 0x00057E58
		public void Expand(object item)
		{
			if (this.m_expandSilently)
			{
				return;
			}
			if (this.ItemExpanding != null)
			{
				TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)base.GetItemContainerData(item);
				ItemExpandingArgs itemExpandingArgs = new ItemExpandingArgs(treeViewItemContainerData.Item);
				this.ItemExpanding(this, itemExpandingArgs);
				IEnumerable enumerable = itemExpandingArgs.Children.OfType<object>().ToArray<object>();
				int num = base.IndexOf(treeViewItemContainerData.Item);
				VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(treeViewItemContainerData.Item);
				if (virtualizingTreeViewItem != null)
				{
					virtualizingTreeViewItem.CanExpand = (enumerable != null);
				}
				else
				{
					treeViewItemContainerData.CanExpand = (enumerable != null);
				}
				if (treeViewItemContainerData.CanExpand)
				{
					IEnumerator enumerator = enumerable.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object item2 = enumerator.Current;
							num++;
							TreeViewItemContainerData treeViewItemContainerData2 = (TreeViewItemContainerData)this.Insert(num, item2);
							VirtualizingTreeViewItem virtualizingTreeViewItem2 = (VirtualizingTreeViewItem)base.GetItemContainer(item2);
							if (virtualizingTreeViewItem2 != null)
							{
								virtualizingTreeViewItem2.Parent = treeViewItemContainerData;
							}
							else
							{
								treeViewItemContainerData2.Parent = treeViewItemContainerData;
							}
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

		// Token: 0x06000FAA RID: 4010 RVA: 0x00059BA4 File Offset: 0x00057FA4
		public void Collapse(object item)
		{
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)base.GetItemContainerData(item);
			int num = base.IndexOf(treeViewItemContainerData.Item);
			List<object> list = new List<object>();
			this.Collapse(treeViewItemContainerData, num + 1, list);
			if (list.Count > 0)
			{
				bool unselect = false;
				base.DestroyItems(list.ToArray(), unselect);
			}
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00059BF8 File Offset: 0x00057FF8
		private void Collapse(object[] items)
		{
			List<object> list = new List<object>();
			for (int i = 0; i < items.Length; i++)
			{
				int num = base.IndexOf(items[i]);
				if (num >= 0)
				{
					TreeViewItemContainerData item = (TreeViewItemContainerData)base.GetItemContainerData(num);
					this.Collapse(item, num + 1, list);
				}
			}
			if (list.Count > 0)
			{
				bool unselect = false;
				base.DestroyItems(list.ToArray(), unselect);
			}
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00059C6C File Offset: 0x0005806C
		private void Collapse(TreeViewItemContainerData item, int itemIndex, List<object> itemsToDestroy)
		{
			for (;;)
			{
				TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)base.GetItemContainerData(itemIndex);
				if (treeViewItemContainerData == null || !treeViewItemContainerData.IsDescendantOf(item))
				{
					break;
				}
				itemsToDestroy.Add(treeViewItemContainerData.Item);
				itemIndex++;
			}
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00059CB4 File Offset: 0x000580B4
		public override void DataBindItem(object item, ItemContainerData containerData, VirtualizingItemContainer itemContainer)
		{
			itemContainer.Clear();
			TreeViewItemDataBindingArgs treeViewItemDataBindingArgs = new TreeViewItemDataBindingArgs();
			treeViewItemDataBindingArgs.Item = item;
			treeViewItemDataBindingArgs.ItemPresenter = ((!(itemContainer.ItemPresenter == null)) ? itemContainer.ItemPresenter : base.gameObject);
			treeViewItemDataBindingArgs.EditorPresenter = ((!(itemContainer.EditorPresenter == null)) ? itemContainer.EditorPresenter : base.gameObject);
			base.RaiseItemDataBinding(treeViewItemDataBindingArgs);
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)itemContainer;
			virtualizingTreeViewItem.CanExpand = treeViewItemDataBindingArgs.HasChildren;
			virtualizingTreeViewItem.CanEdit = treeViewItemDataBindingArgs.CanEdit;
			virtualizingTreeViewItem.CanDrag = treeViewItemDataBindingArgs.CanDrag;
			virtualizingTreeViewItem.UpdateIndent();
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x00059D5C File Offset: 0x0005815C
		private void OnTreeViewItemParentChanged(object sender, VirtualizingParentChangedEventArgs e)
		{
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)sender;
			TreeViewItemContainerData oldParent = e.OldParent;
			if (base.DropMarker.Action != ItemDropAction.SetLastChild && base.DropMarker.Action != ItemDropAction.None)
			{
				if (oldParent != null && !oldParent.HasChildren(this))
				{
					VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(oldParent.Item);
					if (virtualizingTreeViewItem != null)
					{
						virtualizingTreeViewItem.CanExpand = false;
					}
					else
					{
						oldParent.CanExpand = false;
					}
				}
				return;
			}
			TreeViewItemContainerData newParent = e.NewParent;
			VirtualizingTreeViewItem virtualizingTreeViewItem2 = null;
			if (newParent != null)
			{
				virtualizingTreeViewItem2 = (VirtualizingTreeViewItem)base.GetItemContainer(newParent.Item);
			}
			if (virtualizingTreeViewItem2 != null)
			{
				if (virtualizingTreeViewItem2.CanExpand)
				{
					virtualizingTreeViewItem2.IsExpanded = true;
				}
				else
				{
					virtualizingTreeViewItem2.CanExpand = true;
					try
					{
						this.m_expandSilently = true;
						virtualizingTreeViewItem2.IsExpanded = true;
					}
					finally
					{
						this.m_expandSilently = false;
					}
				}
			}
			else if (newParent != null)
			{
				newParent.CanExpand = true;
				newParent.IsExpanded = true;
			}
			TreeViewItemContainerData treeViewItemContainerData2 = treeViewItemContainerData.FirstChild(this);
			TreeViewItemContainerData treeViewItemContainerData3;
			if (virtualizingTreeViewItem2 != null)
			{
				treeViewItemContainerData3 = newParent.LastChild(this);
				if (treeViewItemContainerData3 == null)
				{
					treeViewItemContainerData3 = newParent;
				}
			}
			else
			{
				treeViewItemContainerData3 = (TreeViewItemContainerData)base.LastItemContainerData();
			}
			if (treeViewItemContainerData3 != treeViewItemContainerData)
			{
				TreeViewItemContainerData treeViewItemContainerData4 = treeViewItemContainerData3.LastDescendant(this);
				if (treeViewItemContainerData4 != null)
				{
					treeViewItemContainerData3 = treeViewItemContainerData4;
				}
				if (!treeViewItemContainerData3.IsDescendantOf(treeViewItemContainerData))
				{
					base.SetNextSiblingInternal(treeViewItemContainerData3, treeViewItemContainerData);
				}
			}
			if (treeViewItemContainerData2 != null)
			{
				this.MoveSubtree(treeViewItemContainerData, treeViewItemContainerData2);
			}
			if (oldParent != null && !oldParent.HasChildren(this))
			{
				VirtualizingTreeViewItem virtualizingTreeViewItem3 = (VirtualizingTreeViewItem)base.GetItemContainer(oldParent.Item);
				if (virtualizingTreeViewItem3 != null)
				{
					virtualizingTreeViewItem3.CanExpand = false;
				}
				else
				{
					oldParent.CanExpand = false;
				}
			}
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00059F3C File Offset: 0x0005833C
		private void MoveSubtree(TreeViewItemContainerData parent, TreeViewItemContainerData child)
		{
			int num = base.IndexOf(parent.Item);
			int num2 = base.IndexOf(child.Item);
			bool flag = false;
			if (num < num2)
			{
				flag = true;
			}
			TreeViewItemContainerData treeViewItemContainerData = parent;
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(treeViewItemContainerData.Item);
			if (virtualizingTreeViewItem != null)
			{
				virtualizingTreeViewItem.UpdateIndent();
			}
			while (child != null && child.IsDescendantOf(parent))
			{
				if (treeViewItemContainerData == child)
				{
					break;
				}
				base.SetNextSiblingInternal(treeViewItemContainerData, child);
				virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(child.Item);
				if (virtualizingTreeViewItem != null)
				{
					virtualizingTreeViewItem.UpdateIndent();
				}
				treeViewItemContainerData = child;
				if (flag)
				{
					num2++;
				}
				child = (TreeViewItemContainerData)base.GetItemContainerData(num2);
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0005A004 File Offset: 0x00058404
		protected override bool CanDrop(ItemContainerData[] dragItems, ItemContainerData dropTarget)
		{
			if (base.CanDrop(dragItems, dropTarget))
			{
				TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)dropTarget;
				foreach (TreeViewItemContainerData treeViewItemContainerData2 in dragItems)
				{
					if (treeViewItemContainerData == treeViewItemContainerData2 || (treeViewItemContainerData != null && treeViewItemContainerData.IsDescendantOf(treeViewItemContainerData2)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0005A060 File Offset: 0x00058460
		protected override void Drop(ItemContainerData[] dragItems, ItemContainerData dropTarget, ItemDropAction action)
		{
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)dropTarget;
			if (action == ItemDropAction.SetLastChild)
			{
				foreach (TreeViewItemContainerData treeViewItemContainerData2 in dragItems)
				{
					if (treeViewItemContainerData != null && (treeViewItemContainerData == treeViewItemContainerData2 || treeViewItemContainerData.IsDescendantOf(treeViewItemContainerData2)))
					{
						break;
					}
					this.SetParent(treeViewItemContainerData, treeViewItemContainerData2);
				}
			}
			else if (action == ItemDropAction.SetPrevSibling)
			{
				for (int j = 0; j < dragItems.Length; j++)
				{
					this.SetPrevSibling(treeViewItemContainerData, dragItems[j]);
				}
			}
			else if (action == ItemDropAction.SetNextSibling)
			{
				for (int k = dragItems.Length - 1; k >= 0; k--)
				{
					this.SetNextSiblingInternal(treeViewItemContainerData, dragItems[k]);
				}
			}
			base.UpdateSelectedItemIndex();
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0005A124 File Offset: 0x00058524
		protected override void SetNextSiblingInternal(ItemContainerData sibling, ItemContainerData nextSibling)
		{
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)sibling;
			TreeViewItemContainerData treeViewItemContainerData2 = treeViewItemContainerData.LastDescendant(this);
			if (treeViewItemContainerData2 == null)
			{
				treeViewItemContainerData2 = treeViewItemContainerData;
			}
			TreeViewItemContainerData treeViewItemContainerData3 = (TreeViewItemContainerData)nextSibling;
			TreeViewItemContainerData treeViewItemContainerData4 = treeViewItemContainerData3.FirstChild(this);
			base.SetNextSiblingInternal(treeViewItemContainerData2, nextSibling);
			if (treeViewItemContainerData4 != null)
			{
				this.MoveSubtree(treeViewItemContainerData3, treeViewItemContainerData4);
			}
			this.SetParent(treeViewItemContainerData.Parent, treeViewItemContainerData3);
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(treeViewItemContainerData3.Item);
			if (virtualizingTreeViewItem != null)
			{
				virtualizingTreeViewItem.UpdateIndent();
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0005A1A4 File Offset: 0x000585A4
		protected override void SetPrevSibling(ItemContainerData sibling, ItemContainerData prevSibling)
		{
			TreeViewItemContainerData treeViewItemContainerData = (TreeViewItemContainerData)sibling;
			TreeViewItemContainerData treeViewItemContainerData2 = (TreeViewItemContainerData)prevSibling;
			TreeViewItemContainerData treeViewItemContainerData3 = treeViewItemContainerData2.FirstChild(this);
			base.SetPrevSibling(sibling, prevSibling);
			if (treeViewItemContainerData3 != null)
			{
				this.MoveSubtree(treeViewItemContainerData2, treeViewItemContainerData3);
			}
			this.SetParent(treeViewItemContainerData.Parent, treeViewItemContainerData2);
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(treeViewItemContainerData2.Item);
			if (virtualizingTreeViewItem != null)
			{
				virtualizingTreeViewItem.UpdateIndent();
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0005A210 File Offset: 0x00058610
		private void SetParent(TreeViewItemContainerData parent, TreeViewItemContainerData child)
		{
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(child.Item);
			if (virtualizingTreeViewItem != null)
			{
				virtualizingTreeViewItem.Parent = parent;
			}
			else
			{
				child.Parent = parent;
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0005A250 File Offset: 0x00058650
		public void UpdateIndent(object obj)
		{
			VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(obj);
			if (virtualizingTreeViewItem == null)
			{
				return;
			}
			virtualizingTreeViewItem.UpdateIndent();
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0005A280 File Offset: 0x00058680
		protected override void DestroyItems(object[] items, bool unselect)
		{
			TreeViewItemContainerData[] array = items.Select(new Func<object, ItemContainerData>(this.<DestroyItems>m__0)).OfType<TreeViewItemContainerData>().ToArray<TreeViewItemContainerData>();
			IEnumerable<TreeViewItemContainerData> source = array;
			if (VirtualizingTreeView.<>f__am$cache0 == null)
			{
				VirtualizingTreeView.<>f__am$cache0 = new Func<TreeViewItemContainerData, bool>(VirtualizingTreeView.<DestroyItems>m__1);
			}
			IEnumerable<TreeViewItemContainerData> source2 = source.Where(VirtualizingTreeView.<>f__am$cache0);
			if (VirtualizingTreeView.<>f__am$cache1 == null)
			{
				VirtualizingTreeView.<>f__am$cache1 = new Func<TreeViewItemContainerData, TreeViewItemContainerData>(VirtualizingTreeView.<DestroyItems>m__2);
			}
			TreeViewItemContainerData[] array2 = source2.Select(VirtualizingTreeView.<>f__am$cache1).ToArray<TreeViewItemContainerData>();
			this.Collapse(items);
			base.DestroyItems(items, unselect);
			foreach (TreeViewItemContainerData treeViewItemContainerData in array2)
			{
				if (!treeViewItemContainerData.HasChildren(this))
				{
					VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem)base.GetItemContainer(treeViewItemContainerData.Item);
					if (virtualizingTreeViewItem != null)
					{
						virtualizingTreeViewItem.CanExpand = false;
					}
				}
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0005A357 File Offset: 0x00058757
		[CompilerGenerated]
		private ItemContainerData <DestroyItems>m__0(object item)
		{
			return base.GetItemContainerData(item);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0005A360 File Offset: 0x00058760
		[CompilerGenerated]
		private static bool <DestroyItems>m__1(TreeViewItemContainerData container)
		{
			return container.Parent != null;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0005A36E File Offset: 0x0005876E
		[CompilerGenerated]
		private static TreeViewItemContainerData <DestroyItems>m__2(TreeViewItemContainerData container)
		{
			return container.Parent;
		}

		// Token: 0x04000E40 RID: 3648
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemExpandingArgs> ItemExpanding;

		// Token: 0x04000E41 RID: 3649
		public int Indent = 20;

		// Token: 0x04000E42 RID: 3650
		public bool CanReparent = true;

		// Token: 0x04000E43 RID: 3651
		public bool AutoExpand;

		// Token: 0x04000E44 RID: 3652
		private bool m_expandSilently;

		// Token: 0x04000E45 RID: 3653
		[CompilerGenerated]
		private static Func<TreeViewItemContainerData, bool> <>f__am$cache0;

		// Token: 0x04000E46 RID: 3654
		[CompilerGenerated]
		private static Func<TreeViewItemContainerData, TreeViewItemContainerData> <>f__am$cache1;
	}
}
