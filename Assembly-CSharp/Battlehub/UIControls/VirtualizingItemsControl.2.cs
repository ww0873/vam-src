using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
	// Token: 0x02000290 RID: 656
	public class VirtualizingItemsControl : MonoBehaviour, IPointerDownHandler, IDropHandler, IEventSystemHandler
	{
		// Token: 0x06000EF2 RID: 3826 RVA: 0x00055CE4 File Offset: 0x000540E4
		public VirtualizingItemsControl()
		{
		}

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06000EF3 RID: 3827 RVA: 0x00055D54 File Offset: 0x00054154
		// (remove) Token: 0x06000EF4 RID: 3828 RVA: 0x00055D8C File Offset: 0x0005418C
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

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06000EF5 RID: 3829 RVA: 0x00055DC4 File Offset: 0x000541C4
		// (remove) Token: 0x06000EF6 RID: 3830 RVA: 0x00055DFC File Offset: 0x000541FC
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

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06000EF7 RID: 3831 RVA: 0x00055E34 File Offset: 0x00054234
		// (remove) Token: 0x06000EF8 RID: 3832 RVA: 0x00055E6C File Offset: 0x0005426C
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

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06000EF9 RID: 3833 RVA: 0x00055EA4 File Offset: 0x000542A4
		// (remove) Token: 0x06000EFA RID: 3834 RVA: 0x00055EDC File Offset: 0x000542DC
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

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06000EFB RID: 3835 RVA: 0x00055F14 File Offset: 0x00054314
		// (remove) Token: 0x06000EFC RID: 3836 RVA: 0x00055F4C File Offset: 0x0005434C
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

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06000EFD RID: 3837 RVA: 0x00055F84 File Offset: 0x00054384
		// (remove) Token: 0x06000EFE RID: 3838 RVA: 0x00055FBC File Offset: 0x000543BC
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

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06000EFF RID: 3839 RVA: 0x00055FF4 File Offset: 0x000543F4
		// (remove) Token: 0x06000F00 RID: 3840 RVA: 0x0005602C File Offset: 0x0005442C
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

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06000F01 RID: 3841 RVA: 0x00056064 File Offset: 0x00054464
		// (remove) Token: 0x06000F02 RID: 3842 RVA: 0x0005609C File Offset: 0x0005449C
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

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x000560D2 File Offset: 0x000544D2
		protected virtual bool CanScroll
		{
			get
			{
				return this.CanReorder;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x000560DA File Offset: 0x000544DA
		protected bool IsDropInProgress
		{
			get
			{
				return this.m_isDropInProgress;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x000560E2 File Offset: 0x000544E2
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

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x00056102 File Offset: 0x00054502
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

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x00056122 File Offset: 0x00054522
		protected VirtualizingItemDropMarker DropMarker
		{
			get
			{
				return this.m_dropMarker;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x0005612A File Offset: 0x0005452A
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

		// Token: 0x06000F09 RID: 3849 RVA: 0x00056144 File Offset: 0x00054544
		public bool IsItemSelected(object obj)
		{
			return this.m_selectedItemsHS != null && this.m_selectedItemsHS.Contains(obj);
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0005615F File Offset: 0x0005455F
		// (set) Token: 0x06000F0B RID: 3851 RVA: 0x00056168 File Offset: 0x00054568
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
						ItemContainerData itemContainerData;
						if (this.m_itemContainerData.TryGetValue(obj, out itemContainerData))
						{
							itemContainerData.IsSelected = true;
						}
						VirtualizingItemContainer itemContainer = this.GetItemContainer(obj);
						if (itemContainer != null)
						{
							itemContainer.IsSelected = true;
						}
					}
					if (this.m_selectedItems.Count == 0)
					{
						this.m_selectedIndex = -1;
					}
					else
					{
						this.m_selectedIndex = this.IndexOf(this.m_selectedItems[0]);
					}
				}
				else
				{
					this.m_selectedItems = null;
					this.m_selectedItemsHS = null;
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
							ItemContainerData itemContainerData2;
							if (this.m_itemContainerData.TryGetValue(obj2, out itemContainerData2))
							{
								itemContainerData2.IsSelected = false;
							}
							list.Add(obj2);
							VirtualizingItemContainer itemContainer2 = this.GetItemContainer(obj2);
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

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x0005634A File Offset: 0x0005474A
		// (set) Token: 0x06000F0D RID: 3853 RVA: 0x00056375 File Offset: 0x00054775
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

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x00056384 File Offset: 0x00054784
		// (set) Token: 0x06000F0F RID: 3855 RVA: 0x0005639C File Offset: 0x0005479C
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
				ItemContainerData itemContainerData;
				if (this.SelectedItem != null && this.m_itemContainerData.TryGetValue(this.SelectedItem, out itemContainerData))
				{
					itemContainerData.IsSelected = false;
				}
				VirtualizingItemContainer itemContainer = this.GetItemContainer(this.SelectedItem);
				if (itemContainer != null)
				{
					itemContainer.IsSelected = false;
				}
				this.m_selectedIndex = value;
				object obj = null;
				if (this.m_selectedIndex >= 0 && this.m_selectedIndex < this.m_scrollRect.ItemsCount)
				{
					obj = this.m_scrollRect.Items[this.m_selectedIndex];
					ItemContainerData itemContainerData2;
					if (obj != null && this.m_itemContainerData.TryGetValue(obj, out itemContainerData2))
					{
						itemContainerData2.IsSelected = true;
					}
					VirtualizingItemContainer itemContainer2 = this.GetItemContainer(obj);
					if (itemContainer2 != null)
					{
						itemContainer2.IsSelected = true;
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
					ItemContainerData itemContainerData3;
					if (obj2 != null && this.m_itemContainerData.TryGetValue(obj2, out itemContainerData3))
					{
						itemContainerData3.IsSelected = false;
					}
					VirtualizingItemContainer itemContainer3 = this.GetItemContainer(obj2);
					if (itemContainer3 != null)
					{
						itemContainer3.IsSelected = false;
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

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x00056581 File Offset: 0x00054981
		// (set) Token: 0x06000F11 RID: 3857 RVA: 0x00056590 File Offset: 0x00054990
		public IEnumerable Items
		{
			get
			{
				return this.m_scrollRect.Items;
			}
			set
			{
				if (value == null)
				{
					this.m_scrollRect.Items = null;
					this.m_scrollRect.verticalNormalizedPosition = 1f;
					this.m_scrollRect.horizontalNormalizedPosition = 0f;
					this.m_itemContainerData = new Dictionary<object, ItemContainerData>();
				}
				else
				{
					List<object> list = value.OfType<object>().ToList<object>();
					this.m_itemContainerData = new Dictionary<object, ItemContainerData>();
					for (int i = 0; i < list.Count; i++)
					{
						this.m_itemContainerData.Add(list[i], this.InstantiateItemContainerData(list[i]));
					}
					this.m_scrollRect.Items = list;
				}
			}
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00056638 File Offset: 0x00054A38
		protected virtual ItemContainerData InstantiateItemContainerData(object item)
		{
			return new ItemContainerData
			{
				Item = item
			};
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00056654 File Offset: 0x00054A54
		private void Awake()
		{
			this.m_scrollRect = base.GetComponent<VirtualizingScrollRect>();
			if (this.m_scrollRect == null)
			{
				UnityEngine.Debug.LogError("Scroll Rect is required");
			}
			this.m_scrollRect.ItemDataBinding += this.OnScrollRectItemDataBinding;
			this.m_dropMarker = base.GetComponentInChildren<VirtualizingItemDropMarker>(true);
			this.m_rtcListener = base.GetComponentInChildren<RectTransformChangeListener>();
			if (this.m_rtcListener != null)
			{
				this.m_rtcListener.RectTransformChanged += this.OnViewportRectTransformChanged;
			}
			if (this.Camera == null)
			{
				this.Camera = Camera.main;
			}
			this.m_prevCanDrag = this.CanDrag;
			this.OnCanDragChanged();
			this.AwakeOverride();
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00056714 File Offset: 0x00054B14
		private void Start()
		{
			this.m_canvas = base.GetComponentInParent<Canvas>();
			this.StartOverride();
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00056728 File Offset: 0x00054B28
		private void Update()
		{
			if (this.m_scrollDir != VirtualizingItemsControl.ScrollDir.None)
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
				if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Up)
				{
					this.m_scrollRect.verticalNormalizedPosition += num2;
					if (this.m_scrollRect.verticalNormalizedPosition > 1f)
					{
						this.m_scrollRect.verticalNormalizedPosition = 1f;
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
					}
				}
				else if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Down)
				{
					this.m_scrollRect.verticalNormalizedPosition -= num2;
					if (this.m_scrollRect.verticalNormalizedPosition < 0f)
					{
						this.m_scrollRect.verticalNormalizedPosition = 0f;
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
					}
				}
				else if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Left)
				{
					this.m_scrollRect.horizontalNormalizedPosition -= num4;
					if (this.m_scrollRect.horizontalNormalizedPosition < 0f)
					{
						this.m_scrollRect.horizontalNormalizedPosition = 0f;
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
					}
				}
				if (this.m_scrollDir == VirtualizingItemsControl.ScrollDir.Right)
				{
					this.m_scrollRect.horizontalNormalizedPosition += num4;
					if (this.m_scrollRect.horizontalNormalizedPosition > 1f)
					{
						this.m_scrollRect.horizontalNormalizedPosition = 1f;
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
					}
				}
			}
			if (Input.GetKeyDown(this.RemoveKey))
			{
				this.RemoveSelectedItems();
			}
			if (Input.GetKeyDown(this.SelectAllKey) && Input.GetKey(this.RangeselectKey))
			{
				this.SelectedItems = this.m_scrollRect.Items;
			}
			if (this.m_prevCanDrag != this.CanDrag)
			{
				this.OnCanDragChanged();
				this.m_prevCanDrag = this.CanDrag;
			}
			this.UpdateOverride();
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x000569A0 File Offset: 0x00054DA0
		private void OnEnable()
		{
			VirtualizingItemContainer.Selected += this.OnItemSelected;
			VirtualizingItemContainer.Unselected += this.OnItemUnselected;
			VirtualizingItemContainer.PointerUp += this.OnItemPointerUp;
			VirtualizingItemContainer.PointerDown += this.OnItemPointerDown;
			VirtualizingItemContainer.PointerEnter += this.OnItemPointerEnter;
			VirtualizingItemContainer.PointerExit += this.OnItemPointerExit;
			VirtualizingItemContainer.DoubleClick += this.OnItemDoubleClick;
			VirtualizingItemContainer.BeginEdit += this.OnItemBeginEdit;
			VirtualizingItemContainer.EndEdit += this.OnItemEndEdit;
			VirtualizingItemContainer.BeginDrag += this.OnItemBeginDrag;
			VirtualizingItemContainer.Drag += this.OnItemDrag;
			VirtualizingItemContainer.Drop += this.OnItemDrop;
			VirtualizingItemContainer.EndDrag += this.OnItemEndDrag;
			this.OnEnableOverride();
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00056A94 File Offset: 0x00054E94
		private void OnDisable()
		{
			VirtualizingItemContainer.Selected -= this.OnItemSelected;
			VirtualizingItemContainer.Unselected -= this.OnItemUnselected;
			VirtualizingItemContainer.PointerUp -= this.OnItemPointerUp;
			VirtualizingItemContainer.PointerDown -= this.OnItemPointerDown;
			VirtualizingItemContainer.PointerEnter -= this.OnItemPointerEnter;
			VirtualizingItemContainer.PointerExit -= this.OnItemPointerExit;
			VirtualizingItemContainer.DoubleClick -= this.OnItemDoubleClick;
			VirtualizingItemContainer.BeginEdit -= this.OnItemBeginEdit;
			VirtualizingItemContainer.EndEdit -= this.OnItemEndEdit;
			VirtualizingItemContainer.BeginDrag -= this.OnItemBeginDrag;
			VirtualizingItemContainer.Drag -= this.OnItemDrag;
			VirtualizingItemContainer.Drop -= this.OnItemDrop;
			VirtualizingItemContainer.EndDrag -= this.OnItemEndDrag;
			this.OnDisableOverride();
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00056B88 File Offset: 0x00054F88
		private void OnDestroy()
		{
			if (this.m_scrollRect != null)
			{
				this.m_scrollRect.ItemDataBinding -= this.OnScrollRectItemDataBinding;
			}
			if (this.m_rtcListener != null)
			{
				this.m_rtcListener.RectTransformChanged -= this.OnViewportRectTransformChanged;
			}
			this.OnDestroyOverride();
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00056BEB File Offset: 0x00054FEB
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00056BED File Offset: 0x00054FED
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x00056BEF File Offset: 0x00054FEF
		protected virtual void UpdateOverride()
		{
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00056BF1 File Offset: 0x00054FF1
		protected virtual void OnEnableOverride()
		{
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00056BF3 File Offset: 0x00054FF3
		protected virtual void OnDisableOverride()
		{
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00056BF5 File Offset: 0x00054FF5
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00056BF8 File Offset: 0x00054FF8
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
			VirtualizingItemContainer.Unselected -= this.OnItemUnselected;
			if (Input.GetKey(this.MultiselectKey))
			{
				IList list = (this.m_selectedItems == null) ? new List<object>() : this.m_selectedItems.ToList<object>();
				list.Add(((VirtualizingItemContainer)sender).Item);
				this.SelectedItems = list;
			}
			else if (Input.GetKey(this.RangeselectKey))
			{
				this.SelectRange((VirtualizingItemContainer)sender);
			}
			else
			{
				this.SelectedIndex = this.IndexOf(((VirtualizingItemContainer)sender).Item);
			}
			VirtualizingItemContainer.Unselected += this.OnItemUnselected;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00056CC8 File Offset: 0x000550C8
		private void SelectRange(VirtualizingItemContainer itemContainer)
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
					list.Add(this.m_scrollRect.Items[i]);
				}
				for (int j = num + 1; j <= num4; j++)
				{
					list.Add(this.m_scrollRect.Items[j]);
				}
				this.SelectedItems = list;
			}
			else
			{
				this.SelectedIndex = this.IndexOf(itemContainer.Item);
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00056DBC File Offset: 0x000551BC
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
			list.Remove(((VirtualizingItemContainer)sender).Item);
			this.SelectedItems = list;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00056E1C File Offset: 0x0005521C
		private void OnItemPointerDown(VirtualizingItemContainer sender, PointerEventData eventData)
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

		// Token: 0x06000F23 RID: 3875 RVA: 0x00056EB0 File Offset: 0x000552B0
		private void OnItemPointerUp(VirtualizingItemContainer sender, PointerEventData eventData)
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

		// Token: 0x06000F24 RID: 3876 RVA: 0x00056FA4 File Offset: 0x000553A4
		private void OnItemPointerEnter(VirtualizingItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			this.m_dropTarget = sender;
			if ((this.m_dragItems != null || this.m_externalDragOperation) && this.m_scrollDir == VirtualizingItemsControl.ScrollDir.None)
			{
				this.m_dropMarker.SetTraget(this.m_dropTarget);
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00056FF7 File Offset: 0x000553F7
		private void OnItemPointerExit(VirtualizingItemContainer sender, PointerEventData eventData)
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

		// Token: 0x06000F26 RID: 3878 RVA: 0x0005702F File Offset: 0x0005542F
		private void OnItemDoubleClick(VirtualizingItemContainer sender, PointerEventData eventData)
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

		// Token: 0x06000F27 RID: 3879 RVA: 0x0005706C File Offset: 0x0005546C
		private void OnItemBeginDrag(VirtualizingItemContainer sender, PointerEventData eventData)
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
				this.m_dragItems = new ItemContainerData[]
				{
					this.m_itemContainerData[sender.Item]
				};
			}
			if (this.ItemBeginDrag != null)
			{
				EventHandler<ItemArgs> itemBeginDrag = this.ItemBeginDrag;
				IEnumerable<ItemContainerData> dragItems = this.m_dragItems;
				if (VirtualizingItemsControl.<>f__am$cache0 == null)
				{
					VirtualizingItemsControl.<>f__am$cache0 = new Func<ItemContainerData, object>(VirtualizingItemsControl.<OnItemBeginDrag>m__0);
				}
				itemBeginDrag(this, new ItemArgs(dragItems.Select(VirtualizingItemsControl.<>f__am$cache0).ToArray<object>()));
			}
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x00057154 File Offset: 0x00055554
		private void OnItemDrag(VirtualizingItemContainer sender, PointerEventData eventData)
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
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Up;
						this.m_dropMarker.SetTraget(null);
					}
					else if (vector.y < -height)
					{
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Down;
						this.m_dropMarker.SetTraget(null);
					}
					else if (vector.x <= 0f)
					{
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Left;
					}
					else if (vector.x >= width)
					{
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.Right;
					}
					else
					{
						this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
					}
				}
			}
			else
			{
				this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
			}
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x000572A0 File Offset: 0x000556A0
		private void OnItemDrop(VirtualizingItemContainer sender, PointerEventData eventData)
		{
			if (!this.CanHandleEvent(sender))
			{
				return;
			}
			if (this.m_dragItems == null)
			{
				return;
			}
			this.m_isDropInProgress = true;
			try
			{
				if (this.CanDrop(this.m_dragItems, this.GetItemContainerData(this.m_dropTarget.Item)))
				{
					bool flag = false;
					if (this.ItemBeginDrop != null)
					{
						IEnumerable<ItemContainerData> dragItems = this.m_dragItems;
						if (VirtualizingItemsControl.<>f__am$cache1 == null)
						{
							VirtualizingItemsControl.<>f__am$cache1 = new Func<ItemContainerData, object>(VirtualizingItemsControl.<OnItemDrop>m__1);
						}
						ItemDropCancelArgs itemDropCancelArgs = new ItemDropCancelArgs(dragItems.Select(VirtualizingItemsControl.<>f__am$cache1).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false);
						if (this.ItemBeginDrop != null)
						{
							this.ItemBeginDrop(this, itemDropCancelArgs);
							flag = itemDropCancelArgs.Cancel;
						}
					}
					if (!flag)
					{
						object[] array;
						if (this.m_dragItems != null)
						{
							IEnumerable<ItemContainerData> dragItems2 = this.m_dragItems;
							if (VirtualizingItemsControl.<>f__am$cache2 == null)
							{
								VirtualizingItemsControl.<>f__am$cache2 = new Func<ItemContainerData, object>(VirtualizingItemsControl.<OnItemDrop>m__2);
							}
							array = dragItems2.Select(VirtualizingItemsControl.<>f__am$cache2).ToArray<object>();
						}
						else
						{
							array = null;
						}
						object[] array2 = array;
						object obj = (!(this.m_dropTarget != null)) ? null : this.m_dropTarget.Item;
						ItemContainerData itemContainerData = this.GetItemContainerData(obj);
						this.Drop(this.m_dragItems, itemContainerData, this.m_dropMarker.Action);
						if (this.ItemDrop != null && array2 != null && obj != null && this.m_dropMarker != null)
						{
							this.ItemDrop(this, new ItemDropArgs(array2, obj, this.m_dropMarker.Action, false));
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

		// Token: 0x06000F2A RID: 3882 RVA: 0x00057468 File Offset: 0x00055868
		private void OnItemEndDrag(VirtualizingItemContainer sender, PointerEventData eventData)
		{
			if (this.m_dropTarget != null)
			{
				this.OnItemDrop(sender, eventData);
			}
			else
			{
				if (!this.CanHandleEvent(sender))
				{
					return;
				}
				this.RaiseEndDrag();
			}
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0005749C File Offset: 0x0005589C
		private void RaiseEndDrag()
		{
			if (this.m_dragItems != null)
			{
				if (this.ItemEndDrag != null)
				{
					EventHandler<ItemArgs> itemEndDrag = this.ItemEndDrag;
					IEnumerable<ItemContainerData> dragItems = this.m_dragItems;
					if (VirtualizingItemsControl.<>f__am$cache3 == null)
					{
						VirtualizingItemsControl.<>f__am$cache3 = new Func<ItemContainerData, object>(VirtualizingItemsControl.<RaiseEndDrag>m__3);
					}
					itemEndDrag(this, new ItemArgs(dragItems.Select(VirtualizingItemsControl.<>f__am$cache3).ToArray<object>()));
				}
				this.m_dropMarker.SetTraget(null);
				this.m_dragItems = null;
				this.m_scrollDir = VirtualizingItemsControl.ScrollDir.None;
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00057518 File Offset: 0x00055918
		private void OnViewportRectTransformChanged()
		{
			if (this.ExpandChildrenHeight || this.ExpandChildrenWidth)
			{
				Rect rect = this.m_scrollRect.viewport.rect;
				if (rect.width != this.m_width || rect.height != this.m_height)
				{
					this.m_width = rect.width;
					this.m_height = rect.height;
					this.SetContainersSize();
				}
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00057590 File Offset: 0x00055990
		private void SetContainersSize()
		{
			this.m_scrollRect.ForEachContainer(new Action<RectTransform>(this.<SetContainersSize>m__4));
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x000575AC File Offset: 0x000559AC
		public void UpdateContainerSize(VirtualizingItemContainer container)
		{
			if (container != null)
			{
				if (this.ExpandChildrenWidth)
				{
					container.LayoutElement.minWidth = this.m_width;
				}
				if (this.ExpandChildrenHeight)
				{
					container.LayoutElement.minHeight = this.m_height;
				}
			}
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x000575FD File Offset: 0x000559FD
		private void OnCanDragChanged()
		{
			this.m_scrollRect.ForEachContainer(new Action<RectTransform>(this.<OnCanDragChanged>m__5));
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00057618 File Offset: 0x00055A18
		protected bool CanHandleEvent(object sender)
		{
			VirtualizingItemContainer virtualizingItemContainer = sender as VirtualizingItemContainer;
			return virtualizingItemContainer && this.m_scrollRect.IsParentOf(virtualizingItemContainer.transform);
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0005764C File Offset: 0x00055A4C
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
			if (this.m_scrollRect.ItemsCount > 0)
			{
				RectTransform rectTransform = this.m_scrollRect.LastContainer();
				if (rectTransform != null)
				{
					this.m_dropTarget = rectTransform.GetComponent<VirtualizingItemContainer>();
					this.m_dropMarker.Action = ItemDropAction.SetNextSibling;
				}
			}
			this.m_isDropInProgress = true;
			try
			{
				ItemContainerData itemContainerData = this.GetItemContainerData(this.m_dropTarget.Item);
				if (this.CanDrop(this.m_dragItems, itemContainerData))
				{
					this.Drop(this.m_dragItems, itemContainerData, this.m_dropMarker.Action);
					if (this.ItemDrop != null)
					{
						EventHandler<ItemDropArgs> itemDrop = this.ItemDrop;
						IEnumerable<ItemContainerData> dragItems = this.m_dragItems;
						if (VirtualizingItemsControl.<>f__am$cache4 == null)
						{
							VirtualizingItemsControl.<>f__am$cache4 = new Func<ItemContainerData, object>(VirtualizingItemsControl.<OnDrop>m__6);
						}
						itemDrop(this, new ItemDropArgs(dragItems.Select(VirtualizingItemsControl.<>f__am$cache4).ToArray<object>(), this.m_dropTarget.Item, this.m_dropMarker.Action, false));
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

		// Token: 0x06000F32 RID: 3890 RVA: 0x000577EC File Offset: 0x00055BEC
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			if (this.CanUnselectAll)
			{
				this.SelectedIndex = -1;
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00057800 File Offset: 0x00055C00
		protected virtual void OnItemBeginEdit(object sender, EventArgs e)
		{
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00057802 File Offset: 0x00055C02
		protected virtual void OnItemEndEdit(object sender, EventArgs e)
		{
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00057804 File Offset: 0x00055C04
		public virtual void DataBindItem(object item, ItemContainerData itemContainerData, VirtualizingItemContainer itemContainer)
		{
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00057808 File Offset: 0x00055C08
		private void OnScrollRectItemDataBinding(RectTransform container, object item)
		{
			VirtualizingItemContainer component = container.GetComponent<VirtualizingItemContainer>();
			component.Item = item;
			this.m_selectionLocked = true;
			ItemContainerData itemContainerData = this.m_itemContainerData[item];
			itemContainerData.IsSelected = this.IsItemSelected(item);
			component.IsSelected = itemContainerData.IsSelected;
			component.CanDrag = this.CanDrag;
			this.m_selectionLocked = false;
			this.DataBindItem(item, itemContainerData, component);
			if (this.m_scrollRect.ItemsCount == 1)
			{
				this.SetContainersSize();
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00057883 File Offset: 0x00055C83
		public int IndexOf(object obj)
		{
			if (this.m_scrollRect.Items == null)
			{
				return -1;
			}
			if (obj == null)
			{
				return -1;
			}
			return this.m_scrollRect.Items.IndexOf(obj);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000578B0 File Offset: 0x00055CB0
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
			if (num < newIndex)
			{
				this.m_scrollRect.SetNextSibling(this.GetItemAt(newIndex), obj);
			}
			else
			{
				this.m_scrollRect.SetPrevSibling(this.GetItemAt(newIndex), obj);
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00057914 File Offset: 0x00055D14
		public ItemContainerData LastItemContainerData()
		{
			if (this.m_scrollRect.Items == null || this.m_scrollRect.ItemsCount == 0)
			{
				return null;
			}
			return this.GetItemContainerData(this.m_scrollRect.Items[this.m_scrollRect.ItemsCount - 1]);
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00057968 File Offset: 0x00055D68
		public VirtualizingItemContainer GetItemContainer(object item)
		{
			if (item == null)
			{
				return null;
			}
			RectTransform container = this.m_scrollRect.GetContainer(item);
			if (container == null)
			{
				return null;
			}
			return container.GetComponent<VirtualizingItemContainer>();
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x000579A0 File Offset: 0x00055DA0
		public ItemContainerData GetItemContainerData(object item)
		{
			if (item == null)
			{
				return null;
			}
			ItemContainerData result = null;
			this.m_itemContainerData.TryGetValue(item, out result);
			return result;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x000579C8 File Offset: 0x00055DC8
		public ItemContainerData GetItemContainerData(int siblingIndex)
		{
			if (siblingIndex < 0 || this.m_scrollRect.Items.Count <= siblingIndex)
			{
				return null;
			}
			object key = this.m_scrollRect.Items[siblingIndex];
			return this.m_itemContainerData[key];
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00057A12 File Offset: 0x00055E12
		protected virtual bool CanDrop(ItemContainerData[] dragItems, ItemContainerData dropTarget)
		{
			return dropTarget == null || (dragItems != null && !dragItems.Contains(dropTarget.Item));
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00057A38 File Offset: 0x00055E38
		protected ItemContainerData[] GetDragItems()
		{
			ItemContainerData[] array = new ItemContainerData[this.m_selectedItems.Count];
			if (this.m_selectedItems != null)
			{
				for (int i = 0; i < this.m_selectedItems.Count; i++)
				{
					array[i] = this.m_itemContainerData[this.m_selectedItems[i]];
				}
			}
			return array.OrderBy(new Func<ItemContainerData, int>(this.<GetDragItems>m__7)).ToArray<ItemContainerData>();
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00057AB0 File Offset: 0x00055EB0
		protected virtual void Drop(ItemContainerData[] dragItems, ItemContainerData dropTargetData, ItemDropAction action)
		{
			if (action == ItemDropAction.SetPrevSibling)
			{
				foreach (ItemContainerData prevSibling in dragItems)
				{
					this.SetPrevSibling(dropTargetData, prevSibling);
				}
			}
			else if (action == ItemDropAction.SetNextSibling)
			{
				foreach (ItemContainerData nextSibling in dragItems)
				{
					this.SetNextSiblingInternal(dropTargetData, nextSibling);
				}
			}
			this.UpdateSelectedItemIndex();
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00057B16 File Offset: 0x00055F16
		protected virtual void SetNextSiblingInternal(ItemContainerData sibling, ItemContainerData nextSibling)
		{
			this.m_scrollRect.SetNextSibling(sibling.Item, nextSibling.Item);
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00057B2F File Offset: 0x00055F2F
		protected virtual void SetPrevSibling(ItemContainerData sibling, ItemContainerData prevSibling)
		{
			this.m_scrollRect.SetPrevSibling(sibling.Item, prevSibling.Item);
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00057B48 File Offset: 0x00055F48
		protected void UpdateSelectedItemIndex()
		{
			this.m_selectedIndex = this.IndexOf(this.SelectedItem);
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00057B5C File Offset: 0x00055F5C
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
			if ((this.m_dragItems != null || this.m_externalDragOperation) && this.m_scrollDir == VirtualizingItemsControl.ScrollDir.None)
			{
				this.m_dropMarker.SetTraget(this.m_dropTarget);
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00057BC0 File Offset: 0x00055FC0
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

		// Token: 0x06000F45 RID: 3909 RVA: 0x00057BF0 File Offset: 0x00055FF0
		public void ExternalItemDrop()
		{
			if (!this.CanDrag)
			{
				return;
			}
			this.m_externalDragOperation = false;
			this.m_dropMarker.SetTraget(null);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x00057C14 File Offset: 0x00056014
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
			this.DestroyItems(array, true);
			if (this.ItemsRemoved != null)
			{
				this.ItemsRemoved(this, new ItemsRemovedArgs(array));
			}
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00057CB8 File Offset: 0x000560B8
		protected virtual void DestroyItems(object[] items, bool unselect)
		{
			if (unselect)
			{
				foreach (object item in items)
				{
					if (this.m_selectedItems != null && this.m_selectedItems.Contains(item))
					{
						this.m_selectedItems.Remove(item);
						this.m_selectedItemsHS.Remove(item);
						if (this.m_selectedItems.Count == 0)
						{
							this.m_selectedIndex = -1;
						}
						else
						{
							this.m_selectedIndex = this.IndexOf(this.m_selectedItems[0]);
						}
					}
				}
			}
			this.m_scrollRect.RemoveItems(items.Select(new Func<object, int>(this.<DestroyItems>m__8)).ToArray<int>(), true);
			foreach (object key in items)
			{
				this.m_itemContainerData.Remove(key);
			}
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00057D95 File Offset: 0x00056195
		public ItemContainerData Add(object item)
		{
			return this.Insert(this.m_scrollRect.ItemsCount, item);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00057DAC File Offset: 0x000561AC
		public virtual ItemContainerData Insert(int index, object item)
		{
			ItemContainerData itemContainerData = this.InstantiateItemContainerData(item);
			this.m_itemContainerData.Add(item, itemContainerData);
			this.m_scrollRect.InsertItem(index, item, true);
			return itemContainerData;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00057DE0 File Offset: 0x000561E0
		public void SetNextSibling(object sibling, object nextSibling)
		{
			ItemContainerData itemContainerData = this.GetItemContainerData(sibling);
			if (itemContainerData == null)
			{
				return;
			}
			ItemContainerData itemContainerData2 = this.GetItemContainerData(nextSibling);
			if (itemContainerData2 == null)
			{
				return;
			}
			this.Drop(new ItemContainerData[]
			{
				itemContainerData2
			}, itemContainerData, ItemDropAction.SetNextSibling);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x00057E20 File Offset: 0x00056220
		public void SetPrevSibling(object sibling, object prevSibling)
		{
			ItemContainerData itemContainerData = this.GetItemContainerData(sibling);
			if (itemContainerData == null)
			{
				return;
			}
			ItemContainerData itemContainerData2 = this.GetItemContainerData(prevSibling);
			if (itemContainerData2 == null)
			{
				return;
			}
			this.Drop(new ItemContainerData[]
			{
				itemContainerData2
			}, itemContainerData, ItemDropAction.SetPrevSibling);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00057E5D File Offset: 0x0005625D
		public virtual void Remove(object item)
		{
			this.DestroyItems(new object[]
			{
				item
			}, true);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x00057E70 File Offset: 0x00056270
		public object GetItemAt(int index)
		{
			if (index < 0 || index >= this.m_scrollRect.Items.Count)
			{
				return null;
			}
			return this.m_scrollRect.Items[index];
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00057EA2 File Offset: 0x000562A2
		[CompilerGenerated]
		private static object <OnItemBeginDrag>m__0(ItemContainerData di)
		{
			return di.Item;
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00057EAA File Offset: 0x000562AA
		[CompilerGenerated]
		private static object <OnItemDrop>m__1(ItemContainerData di)
		{
			return di.Item;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00057EB2 File Offset: 0x000562B2
		[CompilerGenerated]
		private static object <OnItemDrop>m__2(ItemContainerData di)
		{
			return di.Item;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x00057EBA File Offset: 0x000562BA
		[CompilerGenerated]
		private static object <RaiseEndDrag>m__3(ItemContainerData di)
		{
			return di.Item;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00057EC4 File Offset: 0x000562C4
		[CompilerGenerated]
		private void <SetContainersSize>m__4(RectTransform c)
		{
			VirtualizingItemContainer component = c.GetComponent<VirtualizingItemContainer>();
			this.UpdateContainerSize(component);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00057EE0 File Offset: 0x000562E0
		[CompilerGenerated]
		private void <OnCanDragChanged>m__5(RectTransform c)
		{
			VirtualizingItemContainer component = c.GetComponent<VirtualizingItemContainer>();
			if (component != null)
			{
				component.CanDrag = this.CanDrag;
			}
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00057F0C File Offset: 0x0005630C
		[CompilerGenerated]
		private static object <OnDrop>m__6(ItemContainerData di)
		{
			return di.Item;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00057F14 File Offset: 0x00056314
		[CompilerGenerated]
		private int <GetDragItems>m__7(ItemContainerData di)
		{
			return this.IndexOf(di.Item);
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00057F22 File Offset: 0x00056322
		[CompilerGenerated]
		private int <DestroyItems>m__8(object item)
		{
			return this.IndexOf(item);
		}

		// Token: 0x04000DFB RID: 3579
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemArgs> ItemBeginDrag;

		// Token: 0x04000DFC RID: 3580
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemDropCancelArgs> ItemBeginDrop;

		// Token: 0x04000DFD RID: 3581
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemDropArgs> ItemDrop;

		// Token: 0x04000DFE RID: 3582
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemArgs> ItemEndDrag;

		// Token: 0x04000DFF RID: 3583
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<SelectionChangedArgs> SelectionChanged;

		// Token: 0x04000E00 RID: 3584
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemArgs> ItemDoubleClick;

		// Token: 0x04000E01 RID: 3585
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemsCancelArgs> ItemsRemoving;

		// Token: 0x04000E02 RID: 3586
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ItemsRemovedArgs> ItemsRemoved;

		// Token: 0x04000E03 RID: 3587
		public KeyCode MultiselectKey = KeyCode.LeftControl;

		// Token: 0x04000E04 RID: 3588
		public KeyCode RangeselectKey = KeyCode.LeftShift;

		// Token: 0x04000E05 RID: 3589
		public KeyCode SelectAllKey = KeyCode.A;

		// Token: 0x04000E06 RID: 3590
		public KeyCode RemoveKey = KeyCode.Delete;

		// Token: 0x04000E07 RID: 3591
		public bool SelectOnPointerUp;

		// Token: 0x04000E08 RID: 3592
		public bool CanUnselectAll = true;

		// Token: 0x04000E09 RID: 3593
		public bool CanEdit = true;

		// Token: 0x04000E0A RID: 3594
		private bool m_prevCanDrag;

		// Token: 0x04000E0B RID: 3595
		public bool CanDrag = true;

		// Token: 0x04000E0C RID: 3596
		public bool CanReorder = true;

		// Token: 0x04000E0D RID: 3597
		public bool ExpandChildrenWidth = true;

		// Token: 0x04000E0E RID: 3598
		public bool ExpandChildrenHeight;

		// Token: 0x04000E0F RID: 3599
		private bool m_isDropInProgress;

		// Token: 0x04000E10 RID: 3600
		private Canvas m_canvas;

		// Token: 0x04000E11 RID: 3601
		public Camera Camera;

		// Token: 0x04000E12 RID: 3602
		public float ScrollSpeed = 100f;

		// Token: 0x04000E13 RID: 3603
		private VirtualizingItemsControl.ScrollDir m_scrollDir;

		// Token: 0x04000E14 RID: 3604
		private RectTransformChangeListener m_rtcListener;

		// Token: 0x04000E15 RID: 3605
		private float m_width;

		// Token: 0x04000E16 RID: 3606
		private float m_height;

		// Token: 0x04000E17 RID: 3607
		private VirtualizingItemDropMarker m_dropMarker;

		// Token: 0x04000E18 RID: 3608
		private bool m_externalDragOperation;

		// Token: 0x04000E19 RID: 3609
		private VirtualizingItemContainer m_dropTarget;

		// Token: 0x04000E1A RID: 3610
		private ItemContainerData[] m_dragItems;

		// Token: 0x04000E1B RID: 3611
		private bool m_selectionLocked;

		// Token: 0x04000E1C RID: 3612
		private List<object> m_selectedItems;

		// Token: 0x04000E1D RID: 3613
		private HashSet<object> m_selectedItemsHS;

		// Token: 0x04000E1E RID: 3614
		private int m_selectedIndex = -1;

		// Token: 0x04000E1F RID: 3615
		private Dictionary<object, ItemContainerData> m_itemContainerData;

		// Token: 0x04000E20 RID: 3616
		private VirtualizingScrollRect m_scrollRect;

		// Token: 0x04000E21 RID: 3617
		[CompilerGenerated]
		private static Func<ItemContainerData, object> <>f__am$cache0;

		// Token: 0x04000E22 RID: 3618
		[CompilerGenerated]
		private static Func<ItemContainerData, object> <>f__am$cache1;

		// Token: 0x04000E23 RID: 3619
		[CompilerGenerated]
		private static Func<ItemContainerData, object> <>f__am$cache2;

		// Token: 0x04000E24 RID: 3620
		[CompilerGenerated]
		private static Func<ItemContainerData, object> <>f__am$cache3;

		// Token: 0x04000E25 RID: 3621
		[CompilerGenerated]
		private static Func<ItemContainerData, object> <>f__am$cache4;

		// Token: 0x02000291 RID: 657
		private enum ScrollDir
		{
			// Token: 0x04000E27 RID: 3623
			None,
			// Token: 0x04000E28 RID: 3624
			Up,
			// Token: 0x04000E29 RID: 3625
			Down,
			// Token: 0x04000E2A RID: 3626
			Left,
			// Token: 0x04000E2B RID: 3627
			Right
		}
	}
}
