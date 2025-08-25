using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x0200026D RID: 621
	[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
	public class ItemContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06000D48 RID: 3400 RVA: 0x00051C81 File Offset: 0x00050081
		public ItemContainer()
		{
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06000D49 RID: 3401 RVA: 0x00051CA0 File Offset: 0x000500A0
		// (remove) Token: 0x06000D4A RID: 3402 RVA: 0x00051CD4 File Offset: 0x000500D4
		public static event EventHandler Selected
		{
			add
			{
				EventHandler eventHandler = ItemContainer.Selected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.Selected, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = ItemContainer.Selected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.Selected, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06000D4B RID: 3403 RVA: 0x00051D08 File Offset: 0x00050108
		// (remove) Token: 0x06000D4C RID: 3404 RVA: 0x00051D3C File Offset: 0x0005013C
		public static event EventHandler Unselected
		{
			add
			{
				EventHandler eventHandler = ItemContainer.Unselected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.Unselected, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = ItemContainer.Unselected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.Unselected, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06000D4D RID: 3405 RVA: 0x00051D70 File Offset: 0x00050170
		// (remove) Token: 0x06000D4E RID: 3406 RVA: 0x00051DA4 File Offset: 0x000501A4
		public static event ItemEventHandler PointerDown
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerDown;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerDown, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerDown;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerDown, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06000D4F RID: 3407 RVA: 0x00051DD8 File Offset: 0x000501D8
		// (remove) Token: 0x06000D50 RID: 3408 RVA: 0x00051E0C File Offset: 0x0005020C
		public static event ItemEventHandler PointerUp
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerUp;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerUp, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerUp;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerUp, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06000D51 RID: 3409 RVA: 0x00051E40 File Offset: 0x00050240
		// (remove) Token: 0x06000D52 RID: 3410 RVA: 0x00051E74 File Offset: 0x00050274
		public static event ItemEventHandler DoubleClick
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.DoubleClick;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.DoubleClick, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.DoubleClick;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.DoubleClick, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06000D53 RID: 3411 RVA: 0x00051EA8 File Offset: 0x000502A8
		// (remove) Token: 0x06000D54 RID: 3412 RVA: 0x00051EDC File Offset: 0x000502DC
		public static event ItemEventHandler PointerEnter
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerEnter;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerEnter, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerEnter;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerEnter, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x14000046 RID: 70
		// (add) Token: 0x06000D55 RID: 3413 RVA: 0x00051F10 File Offset: 0x00050310
		// (remove) Token: 0x06000D56 RID: 3414 RVA: 0x00051F44 File Offset: 0x00050344
		public static event ItemEventHandler PointerExit
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerExit;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerExit, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.PointerExit;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.PointerExit, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x06000D57 RID: 3415 RVA: 0x00051F78 File Offset: 0x00050378
		// (remove) Token: 0x06000D58 RID: 3416 RVA: 0x00051FAC File Offset: 0x000503AC
		public static event ItemEventHandler BeginDrag
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.BeginDrag;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.BeginDrag, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.BeginDrag;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.BeginDrag, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06000D59 RID: 3417 RVA: 0x00051FE0 File Offset: 0x000503E0
		// (remove) Token: 0x06000D5A RID: 3418 RVA: 0x00052014 File Offset: 0x00050414
		public static event ItemEventHandler Drag
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.Drag;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.Drag, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.Drag;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.Drag, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06000D5B RID: 3419 RVA: 0x00052048 File Offset: 0x00050448
		// (remove) Token: 0x06000D5C RID: 3420 RVA: 0x0005207C File Offset: 0x0005047C
		public static event ItemEventHandler Drop
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.Drop;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.Drop, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.Drop;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.Drop, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06000D5D RID: 3421 RVA: 0x000520B0 File Offset: 0x000504B0
		// (remove) Token: 0x06000D5E RID: 3422 RVA: 0x000520E4 File Offset: 0x000504E4
		public static event ItemEventHandler EndDrag
		{
			add
			{
				ItemEventHandler itemEventHandler = ItemContainer.EndDrag;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.EndDrag, (ItemEventHandler)Delegate.Combine(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
			remove
			{
				ItemEventHandler itemEventHandler = ItemContainer.EndDrag;
				ItemEventHandler itemEventHandler2;
				do
				{
					itemEventHandler2 = itemEventHandler;
					itemEventHandler = Interlocked.CompareExchange<ItemEventHandler>(ref ItemContainer.EndDrag, (ItemEventHandler)Delegate.Remove(itemEventHandler2, value), itemEventHandler);
				}
				while (itemEventHandler != itemEventHandler2);
			}
		}

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06000D5F RID: 3423 RVA: 0x00052118 File Offset: 0x00050518
		// (remove) Token: 0x06000D60 RID: 3424 RVA: 0x0005214C File Offset: 0x0005054C
		public static event EventHandler BeginEdit
		{
			add
			{
				EventHandler eventHandler = ItemContainer.BeginEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.BeginEdit, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = ItemContainer.BeginEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.BeginEdit, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06000D61 RID: 3425 RVA: 0x00052180 File Offset: 0x00050580
		// (remove) Token: 0x06000D62 RID: 3426 RVA: 0x000521B4 File Offset: 0x000505B4
		public static event EventHandler EndEdit
		{
			add
			{
				EventHandler eventHandler = ItemContainer.EndEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.EndEdit, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = ItemContainer.EndEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref ItemContainer.EndEdit, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000D63 RID: 3427 RVA: 0x000521E8 File Offset: 0x000505E8
		public LayoutElement LayoutElement
		{
			get
			{
				return this.m_layoutElement;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x000521F0 File Offset: 0x000505F0
		public RectTransform RectTransform
		{
			get
			{
				return this.m_rectTransform;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x000521F8 File Offset: 0x000505F8
		// (set) Token: 0x06000D66 RID: 3430 RVA: 0x00052200 File Offset: 0x00050600
		public virtual bool IsSelected
		{
			get
			{
				return this.m_isSelected;
			}
			set
			{
				if (this.m_isSelected != value)
				{
					this.m_isSelected = value;
					if (this.m_isSelected)
					{
						if (ItemContainer.Selected != null)
						{
							ItemContainer.Selected(this, EventArgs.Empty);
						}
					}
					else if (ItemContainer.Unselected != null)
					{
						ItemContainer.Unselected(this, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x00052264 File Offset: 0x00050664
		// (set) Token: 0x06000D68 RID: 3432 RVA: 0x0005226C File Offset: 0x0005066C
		public bool IsEditing
		{
			get
			{
				return this.m_isEditing;
			}
			set
			{
				if (this.m_isEditing != value && this.m_isSelected)
				{
					this.m_isEditing = (value && this.m_isSelected);
					if (this.EditorPresenter != this.ItemPresenter)
					{
						if (this.EditorPresenter != null)
						{
							this.EditorPresenter.SetActive(this.m_isEditing);
						}
						if (this.ItemPresenter != null)
						{
							this.ItemPresenter.SetActive(!this.m_isEditing);
						}
					}
					if (this.m_isEditing)
					{
						if (ItemContainer.BeginEdit != null)
						{
							ItemContainer.BeginEdit(this, EventArgs.Empty);
						}
					}
					else if (ItemContainer.EndEdit != null)
					{
						ItemContainer.EndEdit(this, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x00052346 File Offset: 0x00050746
		private ItemsControl ItemsControl
		{
			get
			{
				if (this.m_itemsControl == null)
				{
					this.m_itemsControl = base.GetComponentInParent<ItemsControl>();
				}
				return this.m_itemsControl;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0005236B File Offset: 0x0005076B
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x00052373 File Offset: 0x00050773
		public object Item
		{
			[CompilerGenerated]
			get
			{
				return this.<Item>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Item>k__BackingField = value;
			}
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0005237C File Offset: 0x0005077C
		private void Awake()
		{
			this.m_rectTransform = base.GetComponent<RectTransform>();
			this.m_layoutElement = base.GetComponent<LayoutElement>();
			if (this.ItemPresenter == null)
			{
				this.ItemPresenter = base.gameObject;
			}
			if (this.EditorPresenter == null)
			{
				this.EditorPresenter = base.gameObject;
			}
			this.AwakeOverride();
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000523E1 File Offset: 0x000507E1
		private void Start()
		{
			this.StartOverride();
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000523E9 File Offset: 0x000507E9
		private void OnDestroy()
		{
			base.StopAllCoroutines();
			this.m_coBeginEdit = null;
			this.OnDestroyOverride();
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000523FE File Offset: 0x000507FE
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00052400 File Offset: 0x00050800
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00052402 File Offset: 0x00050802
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00052404 File Offset: 0x00050804
		public virtual void Clear()
		{
			this.m_isEditing = false;
			if (this.EditorPresenter != this.ItemPresenter)
			{
				if (this.EditorPresenter != null)
				{
					this.EditorPresenter.SetActive(this.m_isEditing);
				}
				if (this.ItemPresenter != null)
				{
					this.ItemPresenter.SetActive(!this.m_isEditing);
				}
			}
			this.m_isSelected = false;
			this.Item = null;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00052484 File Offset: 0x00050884
		private IEnumerator CoBeginEdit()
		{
			yield return new WaitForSeconds(0.5f);
			this.m_coBeginEdit = null;
			this.IsEditing = this.CanEdit;
			yield break;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x000524A0 File Offset: 0x000508A0
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			this.m_canBeginEdit = (this.m_isSelected && this.ItemsControl != null && this.ItemsControl.SelectedItemsCount == 1 && this.ItemsControl.CanEdit);
			if (ItemContainer.PointerDown != null)
			{
				ItemContainer.PointerDown(this, eventData);
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00052504 File Offset: 0x00050904
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (eventData.clickCount == 2)
			{
				if (ItemContainer.DoubleClick != null)
				{
					ItemContainer.DoubleClick(this, eventData);
				}
				if (this.CanEdit && this.m_coBeginEdit != null)
				{
					base.StopCoroutine(this.m_coBeginEdit);
					this.m_coBeginEdit = null;
				}
			}
			else
			{
				if (this.m_canBeginEdit && this.m_coBeginEdit == null)
				{
					this.m_coBeginEdit = this.CoBeginEdit();
					base.StartCoroutine(this.m_coBeginEdit);
				}
				if (ItemContainer.PointerUp != null)
				{
					ItemContainer.PointerUp(this, eventData);
				}
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x000525A8 File Offset: 0x000509A8
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(base.transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
				return;
			}
			this.m_canBeginEdit = false;
			if (ItemContainer.BeginDrag != null)
			{
				ItemContainer.BeginDrag(this, eventData);
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000525FA File Offset: 0x000509FA
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (ItemContainer.Drop != null)
			{
				ItemContainer.Drop(this, eventData);
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00052612 File Offset: 0x00050A12
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				ExecuteEvents.ExecuteHierarchy<IDragHandler>(base.transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
				return;
			}
			if (ItemContainer.Drag != null)
			{
				ItemContainer.Drag(this, eventData);
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00052652 File Offset: 0x00050A52
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				ExecuteEvents.ExecuteHierarchy<IEndDragHandler>(base.transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
				return;
			}
			if (ItemContainer.EndDrag != null)
			{
				ItemContainer.EndDrag(this, eventData);
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00052692 File Offset: 0x00050A92
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (ItemContainer.PointerEnter != null)
			{
				ItemContainer.PointerEnter(this, eventData);
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000526AA File Offset: 0x00050AAA
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (ItemContainer.PointerExit != null)
			{
				ItemContainer.PointerExit(this, eventData);
			}
		}

		// Token: 0x04000D2C RID: 3372
		public bool CanDrag = true;

		// Token: 0x04000D2D RID: 3373
		public bool CanEdit = true;

		// Token: 0x04000D2E RID: 3374
		public bool CanDrop = true;

		// Token: 0x04000D2F RID: 3375
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler Selected;

		// Token: 0x04000D30 RID: 3376
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler Unselected;

		// Token: 0x04000D31 RID: 3377
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler PointerDown;

		// Token: 0x04000D32 RID: 3378
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler PointerUp;

		// Token: 0x04000D33 RID: 3379
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler DoubleClick;

		// Token: 0x04000D34 RID: 3380
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler PointerEnter;

		// Token: 0x04000D35 RID: 3381
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler PointerExit;

		// Token: 0x04000D36 RID: 3382
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler BeginDrag;

		// Token: 0x04000D37 RID: 3383
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler Drag;

		// Token: 0x04000D38 RID: 3384
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler Drop;

		// Token: 0x04000D39 RID: 3385
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static ItemEventHandler EndDrag;

		// Token: 0x04000D3A RID: 3386
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler BeginEdit;

		// Token: 0x04000D3B RID: 3387
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler EndEdit;

		// Token: 0x04000D3C RID: 3388
		public GameObject ItemPresenter;

		// Token: 0x04000D3D RID: 3389
		public GameObject EditorPresenter;

		// Token: 0x04000D3E RID: 3390
		private LayoutElement m_layoutElement;

		// Token: 0x04000D3F RID: 3391
		private RectTransform m_rectTransform;

		// Token: 0x04000D40 RID: 3392
		protected bool m_isSelected;

		// Token: 0x04000D41 RID: 3393
		private bool m_isEditing;

		// Token: 0x04000D42 RID: 3394
		private ItemsControl m_itemsControl;

		// Token: 0x04000D43 RID: 3395
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object <Item>k__BackingField;

		// Token: 0x04000D44 RID: 3396
		private bool m_canBeginEdit;

		// Token: 0x04000D45 RID: 3397
		private IEnumerator m_coBeginEdit;

		// Token: 0x02000EE0 RID: 3808
		[CompilerGenerated]
		private sealed class <CoBeginEdit>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007203 RID: 29187 RVA: 0x000526C2 File Offset: 0x00050AC2
			[DebuggerHidden]
			public <CoBeginEdit>c__Iterator0()
			{
			}

			// Token: 0x06007204 RID: 29188 RVA: 0x000526CC File Offset: 0x00050ACC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForSeconds(0.5f);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.m_coBeginEdit = null;
					base.IsEditing = this.CanEdit;
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010A7 RID: 4263
			// (get) Token: 0x06007205 RID: 29189 RVA: 0x0005274A File Offset: 0x00050B4A
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010A8 RID: 4264
			// (get) Token: 0x06007206 RID: 29190 RVA: 0x00052752 File Offset: 0x00050B52
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007207 RID: 29191 RVA: 0x0005275A File Offset: 0x00050B5A
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007208 RID: 29192 RVA: 0x0005276A File Offset: 0x00050B6A
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040065E0 RID: 26080
			internal ItemContainer $this;

			// Token: 0x040065E1 RID: 26081
			internal object $current;

			// Token: 0x040065E2 RID: 26082
			internal bool $disposing;

			// Token: 0x040065E3 RID: 26083
			internal int $PC;
		}
	}
}
