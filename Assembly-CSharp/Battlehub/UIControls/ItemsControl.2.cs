using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000278 RID: 632
	public abstract class ItemsControl : MonoBehaviour, IPointerDownHandler, IDropHandler, IEventSystemHandler
	{
		// Token: 0x06000DBB RID: 3515 RVA: 0x0004F450 File Offset: 0x0004D850
		protected ItemsControl()
		{
		}

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06000DBC RID: 3516 RVA: 0x0004F4C0 File Offset: 0x0004D8C0
		// (remove) Token: 0x06000DBD RID: 3517 RVA: 0x0004F4F8 File Offset: 0x0004D8F8
		public event EventHandler<ItemArgs> ItemBeginDrag
		{
			add
			{
				EventHandler<ItemArgs> eventHandler = this.ItemBeginDrag;
				EventHandler<ItemArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemArgs>>(ref this.ItemBeginDrag, (EventHandler<ItemArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemArgs> eventHandler = this.ItemBeginDrag;
				EventHandler<ItemArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemArgs>>(ref this.ItemBeginDrag, (EventHandler<ItemArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06000DBE RID: 3518 RVA: 0x0004F530 File Offset: 0x0004D930
		// (remove) Token: 0x06000DBF RID: 3519 RVA: 0x0004F568 File Offset: 0x0004D968
		public event EventHandler<ItemDropCancelArgs> ItemBeginDrop
		{
			add
			{
				EventHandler<ItemDropCancelArgs> eventHandler = this.ItemBeginDrop;
				EventHandler<ItemDropCancelArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemDropCancelArgs>>(ref this.ItemBeginDrop, (EventHandler<ItemDropCancelArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemDropCancelArgs> eventHandler = this.ItemBeginDrop;
				EventHandler<ItemDropCancelArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemDropCancelArgs>>(ref this.ItemBeginDrop, (EventHandler<ItemDropCancelArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06000DC0 RID: 3520 RVA: 0x0004F5A0 File Offset: 0x0004D9A0
		// (remove) Token: 0x06000DC1 RID: 3521 RVA: 0x0004F5D8 File Offset: 0x0004D9D8
		public event EventHandler<ItemDropArgs> ItemDrop
		{
			add
			{
				EventHandler<ItemDropArgs> eventHandler = this.ItemDrop;
				EventHandler<ItemDropArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemDropArgs>>(ref this.ItemDrop, (EventHandler<ItemDropArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemDropArgs> eventHandler = this.ItemDrop;
				EventHandler<ItemDropArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemDropArgs>>(ref this.ItemDrop, (EventHandler<ItemDropArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06000DC2 RID: 3522 RVA: 0x0004F610 File Offset: 0x0004DA10
		// (remove) Token: 0x06000DC3 RID: 3523 RVA: 0x0004F648 File Offset: 0x0004DA48
		public event EventHandler<ItemArgs> ItemEndDrag
		{
			add
			{
				EventHandler<ItemArgs> eventHandler = this.ItemEndDrag;
				EventHandler<ItemArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemArgs>>(ref this.ItemEndDrag, (EventHandler<ItemArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemArgs> eventHandler = this.ItemEndDrag;
				EventHandler<ItemArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemArgs>>(ref this.ItemEndDrag, (EventHandler<ItemArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06000DC4 RID: 3524 RVA: 0x0004F680 File Offset: 0x0004DA80
		// (remove) Token: 0x06000DC5 RID: 3525 RVA: 0x0004F6B8 File Offset: 0x0004DAB8
		public event EventHandler<SelectionChangedArgs> SelectionChanged
		{
			add
			{
				EventHandler<SelectionChangedArgs> eventHandler = this.SelectionChanged;
				EventHandler<SelectionChangedArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<SelectionChangedArgs>>(ref this.SelectionChanged, (EventHandler<SelectionChangedArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<SelectionChangedArgs> eventHandler = this.SelectionChanged;
				EventHandler<SelectionChangedArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<SelectionChangedArgs>>(ref this.SelectionChanged, (EventHandler<SelectionChangedArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06000DC6 RID: 3526 RVA: 0x0004F6F0 File Offset: 0x0004DAF0
		// (remove) Token: 0x06000DC7 RID: 3527 RVA: 0x0004F728 File Offset: 0x0004DB28
		public event EventHandler<ItemArgs> ItemDoubleClick
		{
			add
			{
				EventHandler<ItemArgs> eventHandler = this.ItemDoubleClick;
				EventHandler<ItemArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemArgs>>(ref this.ItemDoubleClick, (EventHandler<ItemArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemArgs> eventHandler = this.ItemDoubleClick;
				EventHandler<ItemArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemArgs>>(ref this.ItemDoubleClick, (EventHandler<ItemArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06000DC8 RID: 3528 RVA: 0x0004F760 File Offset: 0x0004DB60
		// (remove) Token: 0x06000DC9 RID: 3529 RVA: 0x0004F798 File Offset: 0x0004DB98
		public event EventHandler<ItemsCancelArgs> ItemsRemoving
		{
			add
			{
				EventHandler<ItemsCancelArgs> eventHandler = this.ItemsRemoving;
				EventHandler<ItemsCancelArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemsCancelArgs>>(ref this.ItemsRemoving, (EventHandler<ItemsCancelArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemsCancelArgs> eventHandler = this.ItemsRemoving;
				EventHandler<ItemsCancelArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemsCancelArgs>>(ref this.ItemsRemoving, (EventHandler<ItemsCancelArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06000DCA RID: 3530 RVA: 0x0004F7D0 File Offset: 0x0004DBD0
		// (remove) Token: 0x06000DCB RID: 3531 RVA: 0x0004F808 File Offset: 0x0004DC08
		public event EventHandler<ItemsRemovedArgs> ItemsRemoved
		{
			add
			{
				EventHandler<ItemsRemovedArgs> eventHandler = this.ItemsRemoved;
				EventHandler<ItemsRemovedArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemsRemovedArgs>>(ref this.ItemsRemoved, (EventHandler<ItemsRemovedArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ItemsRemovedArgs> eventHandler = this.ItemsRemoved;
				EventHandler<ItemsRemovedArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ItemsRemovedArgs>>(ref this.ItemsRemoved, (EventHandler<ItemsRemovedArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x0004F83E File Offset: 0x0004DC3E
		protected virtual bool CanScroll
		{
			get
			{
				return this.CanReorder;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0004F846 File Offset: 0x0004DC46
		protected bool IsDropInProgress
		{
			get
			{
				return this.m_isDropInProgress;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0004F84E File Offset: 0x0004DC4E
		public object DropTarget
		{
			get
			{
				if (this.m_dropTarget == null)
				{
					return null;
				}
				return this.m_dropTarget.Item;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0004F86E File Offset: 0x0004DC6E
		public ItemDropAction DropAction
		{
			get
			{
				if (this.m_dropMarker == null)
				{
					return ItemDropAction.None;
				}
				return this.m_dropMarker.Action;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x0004F88E File Offset: 0x0004DC8E
		protected ItemDropMarker DropMarker
		{
			get
			{
				return this.m_dropMarker;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0004F896 File Offset: 0x0004DC96
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x0004F8A0 File Offset: 0x0004DCA0
		public IEnumerable Items
		{
			get
			{
				return this.m_items;
			}
			set
			{
				if (value == null)
				{
					this.m_items = null;
					this.m_scrollRect.verticalNormalizedPosition = 1f;
					this.m_scrollRect.horizontalNormalizedPosition = 0f;
				}
				else
				{
					this.m_items = value.OfType<object>().ToList<object>();
				}
				this.DataBind();
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x0004F8F6 File Offset: 0x0004DCF6
		public int ItemsCount
		{
			get
			{
				if (this.m_items == null)
				{
					return 0;
				}
				return this.m_items.Count;
			}
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0004F910 File Offset: 0x0004DD10
		protected void RemoveItemAt(int index)
		{
			this.m_items.RemoveAt(index);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0004F91E File Offset: 0x0004DD1E
		protected void RemoveItemContainerAt(int index)
		{
			this.m_itemContainers.RemoveAt(index);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0004F92C File Offset: 0x0004DD2C
		protected void InsertItem(int index, object value)
		{
			this.m_items.Insert(index, value);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0004F93B File Offset: 0x0004DD3B
		protected void InsertItemContainerAt(int index, ItemContainer container)
		{
			this.m_itemContainers.Insert(index, container);
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0004F94A File Offset: 0x0004DD4A
		public int SelectedItemsCount
		{
			get
			{
				if (this.m_selectedItems == null)
				{
					return 0;
				}
				return this.m_selectedItems.Count;
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0004F964 File Offset: 0x0004DD64
		public bool IsItemSelected(object obj)
		{
			return this.m_selectedItemsHS != null && this.m_selectedItemsHS.Contains(obj);
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0004F97F File Offset: 0x0004DD7F
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x0004F988 File Offset: 0x0004DD88
		public virtual IEnumerable SelectedItems
		{
			get
			{
				return this.m_selectedItems;
			}
			set
			{
				if (this.m_selectionLocked)
				{
					return;
				}
				this.m_selectionLocked = true;
				IList selectedItems = this.m_selectedItems;
				if (value != null)
				{
					this.m_selectedItems = value.OfType<object>().ToList<object>();
					this.m_selectedItemsHS = new HashSet<object>(this.m_selectedItems);
					for (int i = this.m_selectedItems.Count - 1; i >= 0; i--)
					{
						object obj = this.m_selectedItems[i];
						ItemContainer itemContainer = this.GetItemContainer(obj);
						if (itemContainer != null)
						{
							itemContainer.IsSelected = true;
						}
					}
					if (this.m_selectedItems.Count == 0)
					{
						this.m_selectedItemContainer = null;
						this.m_selectedIndex = -1;
					}
					else
					{
						this.m_selectedItemContainer = this.GetItemContainer(this.m_selectedItems[0]);
						this.m_selectedIndex = this.IndexOf(this.m_selectedItems[0]);
					}
				}
				else
				{
					this.m_selectedItems = null;
					this.m_selectedItemsHS = null;
					this.m_selectedItemContainer = null;
					this.m_selectedIndex = -1;
				}
				List<object> list = new List<object>();
				if (selectedItems != null)
				{
					for (int j = 0; j < selectedItems.Count; j++)
					{
						object obj2 = selectedItems[j];
						if (this.m_selectedItemsHS == null || !this.m_selectedItemsHS.Contains(obj2))
						{
							list.Add(obj2);
							ItemContainer itemContainer2 = this.GetItemContainer(obj2);
							if (itemContainer2 != null)
							{
								itemContainer2.IsSelected = false;
							}
						}
					}
				}
				if (this.SelectionChanged != null)
				{
					object[] newItems = (this.m_selectedItems != null) ? this.m_selectedItems.ToArray() : new object[0];
					this.SelectionChanged(this, new SelectionChangedArgs(list.ToArray(), newItems));
				}
				this.m_selectionLocked = false;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x0004FB57 File Offset: 0x0004DF57
		// (set) Token: 0x06000DDD RID: 3549 RVA: 0x0004FB82 File Offset: 0x0004DF82
		public object SelectedItem
		{
			get
			{
				if (this.m_selectedItems == null || this.m_selectedItems.Count == 0)
				{
					return null;
				}
				return this.m_selectedItems[0];
			}
			set
			{
				this.SelectedIndex = this.IndexOf(value);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x0004FB91 File Offset: 0x0004DF91
		// (set) Token: 0x06000DDF RID: 3551 RVA: 0x0004FBA8 File Offset: 0x0004DFA8
		public int SelectedIndex
		{
			get
			{
				if (this.SelectedItem == null)
				{
					return -1;
				}
				return this.m_selectedIndex;
			}
			set
			{
				if (this.m_selectedIndex == value)
				{
					return;
				}
				if (this.m_selectionLocked)
				{
					return;
				}
				this.m_selectionLocked = true;
				ItemContainer selectedItemContainer = this.m_selectedItemContainer;
				if (selectedItemContainer != null)
				{
					selectedItemContainer.IsSelected = false;
				}
				this.m_selectedIndex = value;
				object obj = null;
				if (this.m_selectedIndex >= 0 && this.m_selectedIndex < this.m_items.Count)
				{
					obj = this.m_items[this.m_selectedIndex];
					this.m_selectedItemContainer = this.GetItemContainer(obj);
					if (this.m_selectedItemContainer != null)
					{
						this.m_selectedItemContainer.IsSelected = true;
					}
				}
				object[] array;
				if (obj != null)
				{
					(array = new object[1])[0] = obj;
				}
				else
				{
					array = new object[0];
				}
				object[] array2 = array;
				foreach (object obj2 in (this.m_selectedItems != null) ? this.m_selectedItems.Except(array2).ToArray<object>() : new object[0])
				{
					ItemContainer itemContainer = this.GetItemContainer(obj2);
					if (itemContainer != null)
					{
						itemContainer.IsSelected = false;
					}
				}
				this.m_selectedItems = array2.ToList<object>();
				this.m_selectedItemsHS = new HashSet<object>(this.m_selectedItems);
				if (this.SelectionChanged != null)
				{
					object[] array3;
					this.SelectionChanged(this, new SelectionChangedArgs(array3, array2));
				}
				this.m_selectionLocked = false;
			}
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0004FD19 File Offset: 0x0004E119
		public int IndexOf(object obj)
		{
			if (this.m_items == null)
			{
				return -1;
			}
			if (obj == null)
			{
				return -1;
			}
			return this.m_items.IndexOf(obj);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0004FD3C File Offset: 0x0004E13C
		public virtual void SetIndex(object obj, int newIndex)
		{
			int num = this.IndexOf(obj);
			if (num == -1)
			{
				return;
			}
			if (num == this.m_selectedIndex)
			{
				this.m_selectedIndex = newIndex;
			}
			this.m_items.RemoveAt(num);
			this.m_items.Insert(newIndex, obj);
			ItemContainer itemContainer = this.m_itemContainers[num];
			this.m_itemContainers.RemoveAt(num);
			this.m_itemContainers.Insert(newIndex, itemContainer);
			itemContainer.transform.SetSiblingIndex(newIndex);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0004FDB8 File Offset: 0x0004E1B8
		public ItemContainer GetItemContainer(object obj)
		{
			ItemsControl.<GetItemContainer>c__AnonStorey0 <GetItemContainer>c__AnonStorey = new ItemsControl.<GetItemContainer>c__AnonStorey0();
			<GetItemContainer>c__AnonStorey.obj = obj;
			return this.m_itemContainers.Where(new Func<ItemContainer, bool>(<GetItemContainer>c__AnonStorey.<>m__0)).FirstOrDefault<ItemContainer>();
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0004FDEE File Offset: 0x0004E1EE
		public ItemContainer LastItemContainer()
		{
			if (this.m_itemContainers == null || this.m_itemContainers.Count == 0)
			{
				return null;
			}
			return this.m_itemContainers[this.m_itemContainers.Count - 1];
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0004FE25 File Offset: 0x0004E225
		public ItemContainer GetItemContainer(int siblingIndex)
		{
			if (siblingIndex < 0 || siblingIndex >= this.m_itemContainers.Count)
			{
				return null;
			}
			return this.m_itemContainers[siblingIndex];
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0004FE50 File Offset: 0x0004E250
		public void ExternalBeginDrag(Vector3 position)
		{
			if (!this.CanDrag)
			{
				return;
			}
			this.m_externalDragOperation = true;
			if (this.m_dropTarget == null)
			{
				return;
			}
			if ((this.m_dragItems != null || this.m_externalDragOperation) && this.m_scrollDir == ItemsControl.ScrollDir.None)
			{
				this.m_dropMarker.SetTraget(this.m_dropTarget);
			}
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0004FEB4 File Offset: 0x0004E2B4
		public void ExternalItemDrag(Vector3 position)
		{
			if (!this.CanDrag)
			{
				return;
			}
			if (this.m_dropTarget != null)
			{
				this.m_dropMarker.SetPosition(position);
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0004FEE4 File Offset: 0x0004E2E4
		public void ExternalItemDrop()
		{
			if (!this.CanDrag)
			{
				return;
			}
			this.m_externalDragOperation = false;
			this.m_dropMarker.SetTraget(null);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0004FF05 File Offset: 0x0004E305
		public ItemContainer Add(object item)
		{
			if (this.m_items == null)
			{
				this.m_items = new List<object>();
				this.m_itemContainers = new List<ItemContainer>();
			}
			return this.Insert(this.m_items.Count, item);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0004FF3C File Offset: 0x0004E33C
		public virtual ItemContainer Insert(int index, object item)
		{
			if (this.m_items == null)
			{
				this.m_items = new List<object>();
				this.m_itemContainers = new List<ItemContainer>();
			}
			object obj = this.m_items.ElementAtOrDefault(index);
			ItemContainer itemContainer = this.GetItemContainer(obj);
			int siblingIndex;
			if (itemContainer != null)
			{
				siblingIndex = this.m_itemContainers.IndexOf(itemContainer);
			}
			else
			{
				siblingIndex = this.m_itemContainers.Count;
			}
			this.m_items.Insert(index, item);
			itemContainer = this.InstantiateItemContainer(siblingIndex);
			if (itemContainer != null)
			{
				itemContainer.Item = item;
				this.DataBindItem(item, itemContainer);
			}
			return itemContainer;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0004FFDC File Offset: 0x0004E3DC
		public void SetNextSibling(object sibling, object nextSibling)
		{
			ItemContainer itemContainer = this.GetItemContainer(sibling);
			if (itemContainer == null)
			{
				return;
			}
			ItemContainer itemContainer2 = this.GetItemContainer(nextSibling);
			if (itemContainer2 == null)
			{
				return;
			}
			this.Drop(new ItemContainer[]
			{
				itemContainer2
			}, itemContainer, ItemDropAction.SetNextSibling);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00050028 File Offset: 0x0004E428
		public void SetPrevSibling(object sibling, object prevSibling)
		{
			ItemContainer itemContainer = this.GetItemContainer(sibling);
			if (itemContainer == null)
			{
				return;
			}
			ItemContainer itemContainer2 = this.GetItemContainer(prevSibling);
			if (itemContainer2 == null)
			{
				return;
			}
			this.Drop(new ItemContainer[]
			{
				itemContainer2
			}, itemContainer, ItemDropAction.SetPrevSibling);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00050071 File Offset: 0x0004E471
		public virtual void Remove(object item)
		{
			if (item == null)
			{
				return;
			}
			if (this.m_items == null)
			{
				return;
			}
			if (!this.m_items.Contains(item))
			{
				return;
			}
			this.DestroyItem(item);
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x000500A0 File Offset: 0x0004E4A0
		public void RemoveAt(int index)
		{
			if (this.m_items == null)
			{
				return;
			}
			if (index >= this.m_items.Count || index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Remove(this.m_items[index]);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x000500F0 File Offset: 0x0004E4F0
		private void Awake()
		{
			if (this.Panel == null)
			{
				this.Panel = base.transform;
			}
			this.m_itemContainers = base.GetComponentsInChildren<ItemContainer>().ToList<ItemContainer>();
			this.m_rtcListener = base.GetComponentInChildren<RectTransformChangeListener>();
			if (this.m_rtcListener != null)
			{
				this.m_rtcListener.RectTransformChanged += this.OnViewportRectTransformChanged;
			}
			this.m_dropMarker = base.GetComponentInChildren<ItemDropMarker>(true);
			this.m_scrollRect = base.GetComponent<ScrollRect>();
			if (this.Camera == null)
			{
				this.Camera = Camera.main;
			}
			this.m_prevCanDrag = this.CanDrag;
			this.OnCanDragChanged();
			this.AwakeOverride();
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000501AC File Offset: 0x0004E5AC
		private void Start()
		{
			this.m_canvas = base.GetComponentInParent<Canvas>();
			this.StartOverride();
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000501C0 File Offset: 0x0004E5C0
		private void Update()
		{
			if (this.m_scrollDir != ItemsControl.ScrollDir.None)
			{
				float num = this.m_scrollRect.content.rect.height - this.m_scrollRect.viewport.rect.height;
				float num2 = 0f;
				if (num > 0f)
				{
					num2 = this.ScrollSpeed / 10f * (1f / num);
				}
				float num3 = this.m_scrollRect.content.rect.width - this.m_scrollRect.viewport.rect.width;
				float num4 = 0f;
				if (num3 > 0f)
				{
					num4 = this.ScrollSpeed / 10f * (1f / num3);
				}
				if (this.m_scrollDir == ItemsControl.ScrollDir.Up)
				{
					this.m_scrollRect.verticalNormalizedPosition += num2;
					if (this.m_scrollRect.verticalNormalizedPosition > 1f)
					{
						this.m_scrollRect.verticalNormalizedPosition = 1f;
						this.m_scrollDir = ItemsControl.ScrollDir.None;
					}
				}
				else if (this.m_scrollDir == ItemsControl.ScrollDir.Down)
				{
					this.m_scrollRect.verticalNormalizedPosition -= num2;
					if (this.m_scrollRect.verticalNormalizedPosition < 0f)
					{
						this.m_scrollRect.verticalNormalizedPosition = 0f;
						this.m_scrollDir = ItemsControl.ScrollDir.None;
					}
				}
				else if (this.m_scrollDir == ItemsControl.ScrollDir.Left)
				{
					this.m_scrollRect.horizontalNormalizedPosition -= num4;
					if (this.m_scrollRect.horizontalNormalizedPosition < 0f)
					{
						this.m_scrollRect.horizontalNormalizedPosition = 0f;
						this.m_scrollDir = ItemsControl.ScrollDir.None;
					}
				}
				if (this.m_scrollDir == ItemsControl.ScrollDir.Right)
				{
					this.m_scrollRect.horizontalNormalizedPosition += num4;
					if (this.m_scrollRect.horizontalNormalizedPosition > 1f)
					{
						this.m_scrollRect.horizontalNormalizedPosition = 1f;
						this.m_scrollDir = ItemsControl.ScrollDir.None;
					}
				}
			}
			if (Input.GetKeyDown(this.RemoveKey))
			{
				this.RemoveSelectedItems();
			}
			if (Input.GetKeyDown(this.SelectAllKey) && Input.GetKey(this.RangeselectKey))
			{
				this.SelectedItems = this.m_items;
			}
			if (this.m_prevCanDrag != this.CanDrag)
			{
				this.OnCanDragChanged();
				this.m_prevCanDrag = this.CanDrag;
			}
			this.UpdateOverride();
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00050434 File Offset: 0x0004E834
		private void OnEnable()
		{
			ItemContainer.Selected += this.OnItemSelected;
			ItemContainer.Unselected += this.OnItemUnselected;
			ItemContainer.PointerUp += this.OnItemPointerUp;
			ItemContainer.PointerDown += this.OnItemPointerDown;
			ItemContainer.PointerEnter += this.OnItemPointerEnter;
			ItemContainer.PointerExit += this.OnItemPointerExit;
			ItemContainer.DoubleClick += this.OnItemDoubleClick;
			ItemContainer.BeginEdit += this.OnItemBeginEdit;
			ItemContainer.EndEdit += this.OnItemEndEdit;
			ItemContainer.BeginDrag += this.OnItemBeginDrag;
			ItemContainer.Drag += this.OnItemDrag;
			ItemContainer.Drop += this.OnItemDrop;
			ItemContainer.EndDrag += this.OnItemEndDrag;
			this.OnEnableOverride();
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00050528 File Offset: 0x0004E928
		private void OnDisable()
		{
			ItemContainer.Selected -= this.OnItemSelected;
			ItemContainer.Unselected -= this.OnItemUnselected;
			ItemContainer.PointerUp -= this.OnItemPointerUp;
			ItemContainer.PointerDown -= this.OnItemPointerDown;
			ItemContainer.PointerEnter -= this.OnItemPointerEnter;
			ItemContainer.PointerExit -= this.OnItemPointerExit;
			ItemContainer.DoubleClick -= this.OnItemDoubleClick;
			ItemContainer.BeginEdit -= this.OnItemBeginEdit;
			ItemContainer.EndEdit -= this.OnItemEndEdit;
			ItemContainer.BeginDrag -= this.OnItemBeginDrag;
			ItemContainer.Drag -= this.OnItemDrag;
			ItemContainer.Drop -= this.OnItemDrop;
			ItemContainer.EndDrag -= this.OnItemEndDrag;
			this.OnDisableOverride();
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0005061A File Offset: 0x0004EA1A
		private void OnDestroy()
		{
			if (this.m_rtcListener != null)
			{
				this.m_rtcListener.RectTransformChanged -= this.OnViewportRectTransformChanged;
			}
			this.OnDestroyOverride();
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0005064A File Offset: 0x0004EA4A
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0005064C File Offset: 0x0004EA4C
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0005064E File Offset: 0x0004EA4E
		protected virtual void UpdateOverride()
		{
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00050650 File Offset: 0x0004EA50
		protected virtual void OnEnableOverride()
		{
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00050652 File Offset: 0x0004EA52
		protected virtual void OnDisableOverride()
		{
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00050654 File Offset: 0x0004EA54
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00050658 File Offset: 0x0004EA58
		private void OnViewportRectTransformChanged()
		{
			if (this.ExpandChildrenHeight || this.ExpandChildrenWidth)
			{
				Rect rect = this.m_scrollRect.viewport.rect;
				if (rect.width != this.m_width || rect.height != this.m_height)
				{
					this.m_width = rect.width;
					this.m_height = rect.height;
					if (this.m_itemContainers != null)
					{
						for (int i = 0; i < this.m_itemContainers.Count; i++)
						{
							ItemContainer itemContainer = this.m_itemContainers[i];
							if (itemContainer != null)
							{
								if (this.ExpandChildrenWidth)
								{
									itemContainer.LayoutElement.minWidth = this.m_width;
								}
								if (this.ExpandChildrenHeight)
								{
									itemContainer.LayoutElement.minHeight = this.m_height;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00050744 File Offset: 0x0004EB44
		private void OnCanDragChanged()
		{
			for (int i = 0; i < this.m_itemContainers.Count; i++)
			{
				ItemContainer itemContainer = this.m_itemContainers[i];
				if (itemContainer != null)
				{
					itemContainer.CanDrag = this.CanDrag;
				}
			}
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00050794 File Offset: 0x0004EB94
		protected bool CanHandleEvent(object sender)
		{
			ItemContainer itemContainer = sender as ItemContainer;
			return itemContainer && itemContainer.transform.IsChildOf(this.Panel);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000507C8 File Offset: 0x0004EBC8
		private void OnItemSelected(object sender, EventArgs e)
		{
			if (this.m_selectionLocked)
			{
				return;
			}
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			ItemContainer.Unselected -= this.OnItemUnselected;
			if (Input.GetKey(this.MultiselectKey))
			{
				IList list = (this.m_selectedItems == null) ? new List<object>() : this.m_selectedItems.ToList<object>();
				list.Add(((ItemContainer)sender).Item);
				this.SelectedItems = list;
			}
			else if (Input.GetKey(this.RangeselectKey))
			{
				this.SelectRange((ItemContainer)sender);
			}
			else
			{
				this.SelectedIndex = this.IndexOf(((ItemContainer)sender).Item);
			}
			ItemContainer.Unselected += this.OnItemUnselected;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00050898 File Offset: 0x0004EC98
		private void SelectRange(ItemContainer itemContainer)
		{
			if (this.m_selectedItems != null && this.m_selectedItems.Count > 0)
			{
				List<object> list = new List<object>();
				int num = this.IndexOf(this.m_selectedItems[0]);
				object item = itemContainer.Item;
				int num2 = this.IndexOf(item);
				int num3 = Mathf.Min(num, num2);
				int num4 = Math.Max(num, num2);
				list.Add(this.m_selectedItems[0]);
				for (int i = num3; i < num; i++)
				{
					list.Add(this.m_items[i]);
				}
				for (int j = num + 1; j <= num4; j++)
				{
					list.Add(this.m_items[j]);
				}
				this.SelectedItems = list;
			}
			else
			{
				this.SelectedIndex = this.IndexOf(itemContainer.Item);
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00050984 File Offset: 0x0004ED84
		private void OnItemUnselected(object sender, EventArgs e)
		{
			if (this.m_selectionLocked)
			{
				return;
			}
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			IList list = (this.m_selectedItems == null) ? new List<object>() : this.m_selectedItems.ToList<object>();
			list.Remove(((ItemContainer)sender).Item);
			this.SelectedItems = list;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x000509E4 File Offset: 0x0004EDE4
		private void OnItemPointerDown(ItemContainer sender, PointerEventData e)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			if (this.m_externalDragOperation)
			{
				return;
			}
			this.m_dropMarker.SetTraget(null);
			this.m_dragItems = null;
			this.m_isDropInProgress = false;
			if (!this.SelectOnPointerUp)
			{
				if (Input.GetKey(this.RangeselectKey))
				{
					this.SelectRange(sender);
				}
				else if (Input.GetKey(this.MultiselectKey))
				{
					sender.IsSelected = !sender.IsSelected;
				}
				else
				{
					sender.IsSelected = true;
				}
			}
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00050A78 File Offset: 0x0004EE78
		private void OnItemPointerUp(ItemContainer sender, PointerEventData e)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			if (this.m_externalDragOperation)
			{
				return;
			}
			if (this.m_dragItems != null)
			{
				return;
			}
			if (this.SelectOnPointerUp)
			{
				if (!this.m_isDropInProgress)
				{
					if (Input.GetKey(this.RangeselectKey))
					{
						this.SelectRange(sender);
					}
					else if (Input.GetKey(this.MultiselectKey))
					{
						sender.IsSelected = !sender.IsSelected;
					}
					else
					{
						sender.IsSelected = true;
					}
				}
			}
			else if (!Input.GetKey(this.MultiselectKey) && !Input.GetKey(this.RangeselectKey) && this.m_selectedItems != null && this.m_selectedItems.Count > 1)
			{
				if (this.SelectedItem == sender.Item)
				{
					this.SelectedItem = null;
				}
				this.SelectedItem = sender.Item;
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00050B6C File Offset: 0x0004EF6C
		private void OnItemPointerEnter(ItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			this.m_dropTarget = sender;
			if ((this.m_dragItems != null || this.m_externalDragOperation) && this.m_scrollDir == ItemsControl.ScrollDir.None)
			{
				this.m_dropMarker.SetTraget(this.m_dropTarget);
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00050BBF File Offset: 0x0004EFBF
		private void OnItemPointerExit(ItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			this.m_dropTarget = null;
			if (this.m_dragItems != null || this.m_externalDragOperation)
			{
				this.m_dropMarker.SetTraget(null);
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00050BF7 File Offset: 0x0004EFF7
		private void OnItemDoubleClick(ItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			if (this.ItemDoubleClick != null)
			{
				this.ItemDoubleClick(this, new ItemArgs(new object[]
				{
					sender.Item
				}));
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00050C31 File Offset: 0x0004F031
		protected virtual void OnItemBeginEdit(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00050C33 File Offset: 0x0004F033
		protected virtual void OnItemEndEdit(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00050C38 File Offset: 0x0004F038
		private void OnItemBeginDrag(ItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			if (this.m_dropTarget != null)
			{
				this.m_dropMarker.SetTraget(this.m_dropTarget);
				this.m_dropMarker.SetPosition(eventData.position);
			}
			if (this.m_selectedItems != null && this.m_selectedItems.Contains(sender.Item))
			{
				this.m_dragItems = this.GetDragItems();
			}
			else
			{
				this.m_dragItems = new ItemContainer[]
				{
					sender
				};
			}
			if (this.ItemBeginDrag != null)
			{
				EventHandler<ItemArgs> itemBeginDrag = this.ItemBeginDrag;
				IEnumerable<ItemContainer> dragItems = this.m_dragItems;
				if (ItemsControl.<>f__am$cache0 == null)
				{
					ItemsControl.<>f__am$cache0 = new Func<ItemContainer, object>(ItemsControl.<OnItemBeginDrag>m__0);
				}
				itemBeginDrag(this, new ItemArgs(dragItems.Select(ItemsControl.<>f__am$cache0).ToArray<object>()));
			}
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00050D10 File Offset: 0x0004F110
		private void OnItemDrag(ItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			this.ExternalItemDrag(eventData.position);
			float height = this.m_scrollRect.viewport.rect.height;
			float width = this.m_scrollRect.viewport.rect.width;
			Camera cam = null;
			if (this.m_canvas.renderMode == RenderMode.WorldSpace || this.m_canvas.renderMode == RenderMode.ScreenSpaceCamera)
			{
				cam = this.Camera;
			}
			if (this.CanScroll)
			{
				Vector2 vector;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_scrollRect.viewport, eventData.position, cam, out vector))
				{
					if (vector.y >= 0f)
					{
						this.m_scrollDir = ItemsControl.ScrollDir.Up;
						this.m_dropMarker.SetTraget(null);
					}
					else if (vector.y < -height)
					{
						this.m_scrollDir = ItemsControl.ScrollDir.Down;
						this.m_dropMarker.SetTraget(null);
					}
					else if (vector.x <= 0f)
					{
						this.m_scrollDir = ItemsControl.ScrollDir.Left;
					}
					else if (vector.x >= width)
					{
						this.m_scrollDir = ItemsControl.ScrollDir.Right;
					}
					else
					{
						this.m_scrollDir = ItemsControl.ScrollDir.None;
					}
				}
			}
			else
			{
				this.m_scrollDir = ItemsControl.ScrollDir.None;
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00050E5C File Offset: 0x0004F25C
		private void OnItemDrop(ItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			this.m_isDropInProgress = true;
			try
			{
				if (this.CanDrop(this.m_dragItems, this.m_dropTarget))
				{
					bool flag = false;
					if (this.ItemBeginDrop != null)
					{
						IEnumerable<ItemContainer> dragItems = this.m_dragItems;
						if (ItemsControl.<>f__am$cache1 == null)
						{
							ItemsControl.<>f__am$cache1 = new Func<ItemContainer, object>(ItemsControl.<OnItemDrop>m__1);
						}
						ItemDropCancelArgs itemDropCancelArgs = new ItemDropCancelArgs(dragItems.Select(ItemsControl.<>f__am$cache1).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false);
						if (this.ItemBeginDrop != null)
						{
							this.ItemBeginDrop(this, itemDropCancelArgs);
							flag = itemDropCancelArgs.Cancel;
						}
					}
					if (!flag)
					{
						this.Drop(this.m_dragItems, this.m_dropTarget, this.m_dropMarker.Action);
						if (this.ItemDrop != null)
						{
							if (this.m_dragItems == null)
							{
								UnityEngine.Debug.LogWarning("m_dragItems");
							}
							if (this.m_dropTarget == null)
							{
								UnityEngine.Debug.LogWarning("m_dropTarget");
							}
							if (this.m_dropMarker == null)
							{
								UnityEngine.Debug.LogWarning("m_dropMarker");
							}
							if (this.m_dragItems != null && this.m_dropTarget != null && this.m_dropMarker != null)
							{
								EventHandler<ItemDropArgs> itemDrop = this.ItemDrop;
								IEnumerable<ItemContainer> dragItems2 = this.m_dragItems;
								if (ItemsControl.<>f__am$cache2 == null)
								{
									ItemsControl.<>f__am$cache2 = new Func<ItemContainer, object>(ItemsControl.<OnItemDrop>m__2);
								}
								itemDrop(this, new ItemDropArgs(dragItems2.Select(ItemsControl.<>f__am$cache2).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false));
							}
						}
					}
				}
				this.RaiseEndDrag();
			}
			finally
			{
				this.m_isDropInProgress = false;
			}
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00051034 File Offset: 0x0004F434
		private void OnItemEndDrag(ItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			this.RaiseEndDrag();
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0005104C File Offset: 0x0004F44C
		private void RaiseEndDrag()
		{
			if (this.m_dragItems != null)
			{
				if (this.ItemEndDrag != null)
				{
					EventHandler<ItemArgs> itemEndDrag = this.ItemEndDrag;
					IEnumerable<ItemContainer> dragItems = this.m_dragItems;
					if (ItemsControl.<>f__am$cache3 == null)
					{
						ItemsControl.<>f__am$cache3 = new Func<ItemContainer, object>(ItemsControl.<RaiseEndDrag>m__3);
					}
					itemEndDrag(this, new ItemArgs(dragItems.Select(ItemsControl.<>f__am$cache3).ToArray<object>()));
				}
				this.m_dropMarker.SetTraget(null);
				this.m_dragItems = null;
				this.m_scrollDir = ItemsControl.ScrollDir.None;
			}
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000510C8 File Offset: 0x0004F4C8
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (!this.CanReorder)
			{
				return;
			}
			if (this.m_dragItems == null)
			{
				GameObject pointerDrag = eventData.pointerDrag;
				if (pointerDrag != null)
				{
					ItemContainer component = pointerDrag.GetComponent<ItemContainer>();
					if (component != null && component.Item != null)
					{
						object item = component.Item;
						if (this.ItemDrop != null)
						{
							this.ItemDrop(this, new ItemDropArgs(new object[]
							{
								item
							}, null, ItemDropAction.SetLastChild, true));
						}
					}
				}
				return;
			}
			if (this.m_itemContainers != null && this.m_itemContainers.Count > 0)
			{
				this.m_dropTarget = this.m_itemContainers.Last<ItemContainer>();
				this.m_dropMarker.Action = ItemDropAction.SetNextSibling;
			}
			this.m_isDropInProgress = true;
			try
			{
				if (this.CanDrop(this.m_dragItems, this.m_dropTarget))
				{
					this.Drop(this.m_dragItems, this.m_dropTarget, this.m_dropMarker.Action);
					if (this.ItemDrop != null)
					{
						EventHandler<ItemDropArgs> itemDrop = this.ItemDrop;
						IEnumerable<ItemContainer> dragItems = this.m_dragItems;
						if (ItemsControl.<>f__am$cache4 == null)
						{
							ItemsControl.<>f__am$cache4 = new Func<ItemContainer, object>(ItemsControl.<OnDrop>m__4);
						}
						itemDrop(this, new ItemDropArgs(dragItems.Select(ItemsControl.<>f__am$cache4).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false));
					}
				}
				this.m_dropMarker.SetTraget(null);
				this.m_dragItems = null;
			}
			finally
			{
				this.m_isDropInProgress = false;
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00051254 File Offset: 0x0004F654
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.CanUnselectAll)
			{
				this.SelectedIndex = -1;
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00051268 File Offset: 0x0004F668
		protected virtual bool CanDrop(ItemContainer[] dragItems, ItemContainer dropTarget)
		{
			return dropTarget == null || (dragItems != null && !dragItems.Contains(dropTarget.Item));
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00051294 File Offset: 0x0004F694
		protected ItemContainer[] GetDragItems()
		{
			ItemContainer[] array = new ItemContainer[this.m_selectedItems.Count];
			if (this.m_selectedItems != null)
			{
				for (int i = 0; i < this.m_selectedItems.Count; i++)
				{
					array[i] = this.GetItemContainer(this.m_selectedItems[i]);
				}
			}
			IEnumerable<ItemContainer> source = array;
			if (ItemsControl.<>f__am$cache5 == null)
			{
				ItemsControl.<>f__am$cache5 = new Func<ItemContainer, int>(ItemsControl.<GetDragItems>m__5);
			}
			return source.OrderBy(ItemsControl.<>f__am$cache5).ToArray<ItemContainer>();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00051318 File Offset: 0x0004F718
		protected virtual void SetNextSibling(ItemContainer sibling, ItemContainer nextSibling)
		{
			int num = sibling.transform.GetSiblingIndex();
			int siblingIndex = nextSibling.transform.GetSiblingIndex();
			this.RemoveItemContainerAt(siblingIndex);
			this.RemoveItemAt(siblingIndex);
			if (siblingIndex > num)
			{
				num++;
			}
			nextSibling.transform.SetSiblingIndex(num);
			this.InsertItemContainerAt(num, nextSibling);
			this.InsertItem(num, nextSibling.Item);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00051378 File Offset: 0x0004F778
		protected virtual void SetPrevSibling(ItemContainer sibling, ItemContainer prevSibling)
		{
			int num = sibling.transform.GetSiblingIndex();
			int siblingIndex = prevSibling.transform.GetSiblingIndex();
			this.RemoveItemContainerAt(siblingIndex);
			this.RemoveItemAt(siblingIndex);
			if (siblingIndex < num)
			{
				num--;
			}
			prevSibling.transform.SetSiblingIndex(num);
			this.InsertItemContainerAt(num, prevSibling);
			this.InsertItem(num, prevSibling.Item);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000513D8 File Offset: 0x0004F7D8
		protected virtual void Drop(ItemContainer[] dragItems, ItemContainer dropTarget, ItemDropAction action)
		{
			if (action == ItemDropAction.SetPrevSibling)
			{
				foreach (ItemContainer prevSibling in dragItems)
				{
					this.SetPrevSibling(dropTarget, prevSibling);
				}
			}
			else if (action == ItemDropAction.SetNextSibling)
			{
				foreach (ItemContainer nextSibling in dragItems)
				{
					this.SetNextSibling(dropTarget, nextSibling);
				}
			}
			this.UpdateSelectedItemIndex();
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0005143E File Offset: 0x0004F83E
		protected void UpdateSelectedItemIndex()
		{
			this.m_selectedIndex = this.IndexOf(this.SelectedItem);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00051454 File Offset: 0x0004F854
		protected virtual void DataBind()
		{
			this.m_itemContainers = base.GetComponentsInChildren<ItemContainer>().ToList<ItemContainer>();
			if (this.m_items == null)
			{
				for (int i = 0; i < this.m_itemContainers.Count; i++)
				{
					UnityEngine.Object.DestroyImmediate(this.m_itemContainers[i].gameObject);
				}
			}
			else
			{
				int num = this.m_items.Count - this.m_itemContainers.Count;
				if (num > 0)
				{
					for (int j = 0; j < num; j++)
					{
						this.InstantiateItemContainer(this.m_itemContainers.Count);
					}
				}
				else
				{
					int num2 = this.m_itemContainers.Count + num;
					for (int k = this.m_itemContainers.Count - 1; k >= num2; k--)
					{
						this.DestroyItemContainer(k);
					}
				}
			}
			for (int l = 0; l < this.m_itemContainers.Count; l++)
			{
				ItemContainer itemContainer = this.m_itemContainers[l];
				if (itemContainer != null)
				{
					itemContainer.Clear();
				}
			}
			if (this.m_items != null)
			{
				for (int m = 0; m < this.m_items.Count; m++)
				{
					object item = this.m_items[m];
					ItemContainer itemContainer2 = this.m_itemContainers[m];
					itemContainer2.CanDrag = this.CanDrag;
					if (itemContainer2 != null)
					{
						itemContainer2.Item = item;
						this.DataBindItem(item, itemContainer2);
					}
				}
			}
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000515ED File Offset: 0x0004F9ED
		public virtual void DataBindItem(object item, ItemContainer itemContainer)
		{
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000515F0 File Offset: 0x0004F9F0
		protected ItemContainer InstantiateItemContainer(int siblingIndex)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ItemContainerPrefab);
			gameObject.name = "ItemContainer";
			gameObject.transform.SetParent(this.Panel, false);
			gameObject.transform.SetSiblingIndex(siblingIndex);
			ItemContainer itemContainer = this.InstantiateItemContainerOverride(gameObject);
			itemContainer.CanDrag = this.CanDrag;
			if (this.ExpandChildrenWidth)
			{
				itemContainer.LayoutElement.minWidth = this.m_width;
			}
			if (this.ExpandChildrenHeight)
			{
				itemContainer.LayoutElement.minHeight = this.m_height;
			}
			this.m_itemContainers.Insert(siblingIndex, itemContainer);
			return itemContainer;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0005168C File Offset: 0x0004FA8C
		protected void DestroyItemContainer(int siblingIndex)
		{
			if (this.m_itemContainers == null)
			{
				return;
			}
			if (siblingIndex >= 0 && siblingIndex < this.m_itemContainers.Count)
			{
				UnityEngine.Object.DestroyImmediate(this.m_itemContainers[siblingIndex].gameObject);
				this.m_itemContainers.RemoveAt(siblingIndex);
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000516E0 File Offset: 0x0004FAE0
		protected virtual ItemContainer InstantiateItemContainerOverride(GameObject container)
		{
			ItemContainer itemContainer = container.GetComponent<ItemContainer>();
			if (itemContainer == null)
			{
				itemContainer = container.AddComponent<ItemContainer>();
			}
			return itemContainer;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00051708 File Offset: 0x0004FB08
		public void RemoveSelectedItems()
		{
			if (this.m_selectedItems == null)
			{
				return;
			}
			object[] array;
			if (this.ItemsRemoving != null)
			{
				ItemsCancelArgs itemsCancelArgs = new ItemsCancelArgs(this.m_selectedItems.ToList<object>());
				this.ItemsRemoving(this, itemsCancelArgs);
				if (itemsCancelArgs.Items == null)
				{
					array = new object[0];
				}
				else
				{
					array = itemsCancelArgs.Items.ToArray();
				}
			}
			else
			{
				array = this.m_selectedItems.ToArray();
			}
			if (array.Length == 0)
			{
				return;
			}
			this.SelectedItems = null;
			foreach (object item in array)
			{
				this.DestroyItem(item);
			}
			if (this.ItemsRemoved != null)
			{
				this.ItemsRemoved(this, new ItemsRemovedArgs(array));
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000517CC File Offset: 0x0004FBCC
		protected virtual void DestroyItem(object item)
		{
			if (this.m_selectedItems != null && this.m_selectedItems.Contains(item))
			{
				this.m_selectedItems.Remove(item);
				this.m_selectedItemsHS.Remove(item);
				if (this.m_selectedItems.Count == 0)
				{
					this.m_selectedItemContainer = null;
					this.m_selectedIndex = -1;
				}
				else
				{
					this.m_selectedItemContainer = this.GetItemContainer(this.m_selectedItems[0]);
					this.m_selectedIndex = this.IndexOf(this.m_selectedItemContainer.Item);
				}
			}
			ItemContainer itemContainer = this.GetItemContainer(item);
			if (itemContainer != null)
			{
				int siblingIndex = itemContainer.transform.GetSiblingIndex();
				this.DestroyItemContainer(siblingIndex);
				this.m_items.Remove(item);
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00051895 File Offset: 0x0004FC95
		[CompilerGenerated]
		private static object <OnItemBeginDrag>m__0(ItemContainer di)
		{
			return di.Item;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0005189D File Offset: 0x0004FC9D
		[CompilerGenerated]
		private static object <OnItemDrop>m__1(ItemContainer di)
		{
			return di.Item;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000518A5 File Offset: 0x0004FCA5
		[CompilerGenerated]
		private static object <OnItemDrop>m__2(ItemContainer di)
		{
			return di.Item;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000518AD File Offset: 0x0004FCAD
		[CompilerGenerated]
		private static object <RaiseEndDrag>m__3(ItemContainer di)
		{
			return di.Item;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000518B5 File Offset: 0x0004FCB5
		[CompilerGenerated]
		private static object <OnDrop>m__4(ItemContainer di)
		{
			return di.Item;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000518BD File Offset: 0x0004FCBD
		[CompilerGenerated]
		private static int <GetDragItems>m__5(ItemContainer di)
		{
			return di.transform.GetSiblingIndex();
		}

		// Token: 0x04000D64 RID: 3428
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemArgs> ItemBeginDrag;

		// Token: 0x04000D65 RID: 3429
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemDropCancelArgs> ItemBeginDrop;

		// Token: 0x04000D66 RID: 3430
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemDropArgs> ItemDrop;

		// Token: 0x04000D67 RID: 3431
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemArgs> ItemEndDrag;

		// Token: 0x04000D68 RID: 3432
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<SelectionChangedArgs> SelectionChanged;

		// Token: 0x04000D69 RID: 3433
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemArgs> ItemDoubleClick;

		// Token: 0x04000D6A RID: 3434
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemsCancelArgs> ItemsRemoving;

		// Token: 0x04000D6B RID: 3435
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemsRemovedArgs> ItemsRemoved;

		// Token: 0x04000D6C RID: 3436
		public KeyCode MultiselectKey = KeyCode.LeftControl;

		// Token: 0x04000D6D RID: 3437
		public KeyCode RangeselectKey = KeyCode.LeftShift;

		// Token: 0x04000D6E RID: 3438
		public KeyCode SelectAllKey = KeyCode.A;

		// Token: 0x04000D6F RID: 3439
		public KeyCode RemoveKey = KeyCode.Delete;

		// Token: 0x04000D70 RID: 3440
		public bool SelectOnPointerUp;

		// Token: 0x04000D71 RID: 3441
		public bool CanUnselectAll = true;

		// Token: 0x04000D72 RID: 3442
		public bool CanEdit = true;

		// Token: 0x04000D73 RID: 3443
		private bool m_prevCanDrag;

		// Token: 0x04000D74 RID: 3444
		public bool CanDrag = true;

		// Token: 0x04000D75 RID: 3445
		public bool CanReorder = true;

		// Token: 0x04000D76 RID: 3446
		public bool ExpandChildrenWidth = true;

		// Token: 0x04000D77 RID: 3447
		public bool ExpandChildrenHeight;

		// Token: 0x04000D78 RID: 3448
		private bool m_isDropInProgress;

		// Token: 0x04000D79 RID: 3449
		[SerializeField]
		private GameObject ItemContainerPrefab;

		// Token: 0x04000D7A RID: 3450
		[SerializeField]
		protected Transform Panel;

		// Token: 0x04000D7B RID: 3451
		private Canvas m_canvas;

		// Token: 0x04000D7C RID: 3452
		public Camera Camera;

		// Token: 0x04000D7D RID: 3453
		public float ScrollSpeed = 100f;

		// Token: 0x04000D7E RID: 3454
		private ItemsControl.ScrollDir m_scrollDir;

		// Token: 0x04000D7F RID: 3455
		private ScrollRect m_scrollRect;

		// Token: 0x04000D80 RID: 3456
		private RectTransformChangeListener m_rtcListener;

		// Token: 0x04000D81 RID: 3457
		private float m_width;

		// Token: 0x04000D82 RID: 3458
		private float m_height;

		// Token: 0x04000D83 RID: 3459
		private List<ItemContainer> m_itemContainers;

		// Token: 0x04000D84 RID: 3460
		private ItemDropMarker m_dropMarker;

		// Token: 0x04000D85 RID: 3461
		private bool m_externalDragOperation;

		// Token: 0x04000D86 RID: 3462
		private ItemContainer m_dropTarget;

		// Token: 0x04000D87 RID: 3463
		private ItemContainer[] m_dragItems;

		// Token: 0x04000D88 RID: 3464
		private IList<object> m_items;

		// Token: 0x04000D89 RID: 3465
		private bool m_selectionLocked;

		// Token: 0x04000D8A RID: 3466
		private List<object> m_selectedItems;

		// Token: 0x04000D8B RID: 3467
		private HashSet<object> m_selectedItemsHS;

		// Token: 0x04000D8C RID: 3468
		private ItemContainer m_selectedItemContainer;

		// Token: 0x04000D8D RID: 3469
		private int m_selectedIndex = -1;

		// Token: 0x04000D8E RID: 3470
		[CompilerGenerated]
		private static Func<ItemContainer, object> <>f__am$cache0;

		// Token: 0x04000D8F RID: 3471
		[CompilerGenerated]
		private static Func<ItemContainer, object> <>f__am$cache1;

		// Token: 0x04000D90 RID: 3472
		[CompilerGenerated]
		private static Func<ItemContainer, object> <>f__am$cache2;

		// Token: 0x04000D91 RID: 3473
		[CompilerGenerated]
		private static Func<ItemContainer, object> <>f__am$cache3;

		// Token: 0x04000D92 RID: 3474
		[CompilerGenerated]
		private static Func<ItemContainer, object> <>f__am$cache4;

		// Token: 0x04000D93 RID: 3475
		[CompilerGenerated]
		private static Func<ItemContainer, int> <>f__am$cache5;

		// Token: 0x02000279 RID: 633
		private enum ScrollDir
		{
			// Token: 0x04000D95 RID: 3477
			None,
			// Token: 0x04000D96 RID: 3478
			Up,
			// Token: 0x04000D97 RID: 3479
			Down,
			// Token: 0x04000D98 RID: 3480
			Left,
			// Token: 0x04000D99 RID: 3481
			Right
		}

		// Token: 0x02000EE1 RID: 3809
		[CompilerGenerated]
		private sealed class <GetItemContainer>c__AnonStorey0
		{
			// Token: 0x06007209 RID: 29193 RVA: 0x000518CA File Offset: 0x0004FCCA
			public <GetItemContainer>c__AnonStorey0()
			{
			}

			// Token: 0x0600720A RID: 29194 RVA: 0x000518D2 File Offset: 0x0004FCD2
			internal bool <>m__0(ItemContainer ic)
			{
				return ic.Item == this.obj;
			}

			// Token: 0x040065E4 RID: 26084
			internal object obj;
		}
	}
}
