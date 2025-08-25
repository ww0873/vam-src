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
	// Token: 0x0200028C RID: 652
	[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
	public class VirtualizingItemContainer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06000EA4 RID: 3748 RVA: 0x0005500E File Offset: 0x0005340E
		public VirtualizingItemContainer()
		{
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06000EA5 RID: 3749 RVA: 0x0005502C File Offset: 0x0005342C
		// (remove) Token: 0x06000EA6 RID: 3750 RVA: 0x00055060 File Offset: 0x00053460
		public static event EventHandler Selected
		{
			add
			{
				EventHandler eventHandler = VirtualizingItemContainer.Selected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.Selected, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = VirtualizingItemContainer.Selected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.Selected, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06000EA7 RID: 3751 RVA: 0x00055094 File Offset: 0x00053494
		// (remove) Token: 0x06000EA8 RID: 3752 RVA: 0x000550C8 File Offset: 0x000534C8
		public static event EventHandler Unselected
		{
			add
			{
				EventHandler eventHandler = VirtualizingItemContainer.Unselected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.Unselected, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = VirtualizingItemContainer.Unselected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.Unselected, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06000EA9 RID: 3753 RVA: 0x000550FC File Offset: 0x000534FC
		// (remove) Token: 0x06000EAA RID: 3754 RVA: 0x00055130 File Offset: 0x00053530
		public static event VirtualizingItemEventHandler PointerDown
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerDown;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerDown, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerDown;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerDown, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06000EAB RID: 3755 RVA: 0x00055164 File Offset: 0x00053564
		// (remove) Token: 0x06000EAC RID: 3756 RVA: 0x00055198 File Offset: 0x00053598
		public static event VirtualizingItemEventHandler PointerUp
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerUp;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerUp, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerUp;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerUp, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06000EAD RID: 3757 RVA: 0x000551CC File Offset: 0x000535CC
		// (remove) Token: 0x06000EAE RID: 3758 RVA: 0x00055200 File Offset: 0x00053600
		public static event VirtualizingItemEventHandler DoubleClick
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.DoubleClick;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.DoubleClick, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.DoubleClick;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.DoubleClick, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06000EAF RID: 3759 RVA: 0x00055234 File Offset: 0x00053634
		// (remove) Token: 0x06000EB0 RID: 3760 RVA: 0x00055268 File Offset: 0x00053668
		public static event VirtualizingItemEventHandler PointerEnter
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerEnter;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerEnter, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerEnter;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerEnter, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06000EB1 RID: 3761 RVA: 0x0005529C File Offset: 0x0005369C
		// (remove) Token: 0x06000EB2 RID: 3762 RVA: 0x000552D0 File Offset: 0x000536D0
		public static event VirtualizingItemEventHandler PointerExit
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerExit;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerExit, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.PointerExit;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.PointerExit, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06000EB3 RID: 3763 RVA: 0x00055304 File Offset: 0x00053704
		// (remove) Token: 0x06000EB4 RID: 3764 RVA: 0x00055338 File Offset: 0x00053738
		public static event VirtualizingItemEventHandler BeginDrag
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.BeginDrag;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.BeginDrag, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.BeginDrag;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.BeginDrag, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06000EB5 RID: 3765 RVA: 0x0005536C File Offset: 0x0005376C
		// (remove) Token: 0x06000EB6 RID: 3766 RVA: 0x000553A0 File Offset: 0x000537A0
		public static event VirtualizingItemEventHandler Drag
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.Drag;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.Drag, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.Drag;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.Drag, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06000EB7 RID: 3767 RVA: 0x000553D4 File Offset: 0x000537D4
		// (remove) Token: 0x06000EB8 RID: 3768 RVA: 0x00055408 File Offset: 0x00053808
		public static event VirtualizingItemEventHandler Drop
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.Drop;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.Drop, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.Drop;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.Drop, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06000EB9 RID: 3769 RVA: 0x0005543C File Offset: 0x0005383C
		// (remove) Token: 0x06000EBA RID: 3770 RVA: 0x00055470 File Offset: 0x00053870
		public static event VirtualizingItemEventHandler EndDrag
		{
			add
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.EndDrag;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.EndDrag, (VirtualizingItemEventHandler)Delegate.Combine(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
			remove
			{
				VirtualizingItemEventHandler virtualizingItemEventHandler = VirtualizingItemContainer.EndDrag;
				VirtualizingItemEventHandler virtualizingItemEventHandler2;
				do
				{
					virtualizingItemEventHandler2 = virtualizingItemEventHandler;
					virtualizingItemEventHandler = Interlocked.CompareExchange<VirtualizingItemEventHandler>(ref VirtualizingItemContainer.EndDrag, (VirtualizingItemEventHandler)Delegate.Remove(virtualizingItemEventHandler2, value), virtualizingItemEventHandler);
				}
				while (virtualizingItemEventHandler != virtualizingItemEventHandler2);
			}
		}

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06000EBB RID: 3771 RVA: 0x000554A4 File Offset: 0x000538A4
		// (remove) Token: 0x06000EBC RID: 3772 RVA: 0x000554D8 File Offset: 0x000538D8
		public static event EventHandler BeginEdit
		{
			add
			{
				EventHandler eventHandler = VirtualizingItemContainer.BeginEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.BeginEdit, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = VirtualizingItemContainer.BeginEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.BeginEdit, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06000EBD RID: 3773 RVA: 0x0005550C File Offset: 0x0005390C
		// (remove) Token: 0x06000EBE RID: 3774 RVA: 0x00055540 File Offset: 0x00053940
		public static event EventHandler EndEdit
		{
			add
			{
				EventHandler eventHandler = VirtualizingItemContainer.EndEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.EndEdit, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = VirtualizingItemContainer.EndEdit;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref VirtualizingItemContainer.EndEdit, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x00055574 File Offset: 0x00053974
		public LayoutElement LayoutElement
		{
			get
			{
				return this.m_layoutElement;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0005557C File Offset: 0x0005397C
		public RectTransform RectTransform
		{
			get
			{
				return this.m_rectTransform;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00055584 File Offset: 0x00053984
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x0005558C File Offset: 0x0005398C
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
						if (VirtualizingItemContainer.Selected != null)
						{
							VirtualizingItemContainer.Selected(this, EventArgs.Empty);
						}
					}
					else if (VirtualizingItemContainer.Unselected != null)
					{
						VirtualizingItemContainer.Unselected(this, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x000555F0 File Offset: 0x000539F0
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x000555F8 File Offset: 0x000539F8
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
						if (VirtualizingItemContainer.BeginEdit != null)
						{
							VirtualizingItemContainer.BeginEdit(this, EventArgs.Empty);
						}
					}
					else if (VirtualizingItemContainer.EndEdit != null)
					{
						VirtualizingItemContainer.EndEdit(this, EventArgs.Empty);
					}
				}
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x000556D2 File Offset: 0x00053AD2
		private VirtualizingItemsControl ItemsControl
		{
			get
			{
				if (this.m_itemsControl == null)
				{
					this.m_itemsControl = base.GetComponentInParent<VirtualizingItemsControl>();
				}
				return this.m_itemsControl;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x000556F7 File Offset: 0x00053AF7
		// (set) Token: 0x06000EC7 RID: 3783 RVA: 0x000556FF File Offset: 0x00053AFF
		public virtual object Item
		{
			get
			{
				return this.m_item;
			}
			set
			{
				this.m_item = value;
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00055708 File Offset: 0x00053B08
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

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0005576D File Offset: 0x00053B6D
		private void Start()
		{
			this.StartOverride();
			this.ItemsControl.UpdateContainerSize(this);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00055781 File Offset: 0x00053B81
		private void OnDestroy()
		{
			base.StopAllCoroutines();
			this.m_coBeginEdit = null;
			this.OnDestroyOverride();
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00055796 File Offset: 0x00053B96
		protected virtual void AwakeOverride()
		{
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00055798 File Offset: 0x00053B98
		protected virtual void StartOverride()
		{
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x0005579A File Offset: 0x00053B9A
		protected virtual void OnDestroyOverride()
		{
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0005579C File Offset: 0x00053B9C
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
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00055810 File Offset: 0x00053C10
		private IEnumerator CoBeginEdit()
		{
			yield return new WaitForSeconds(0.5f);
			this.m_coBeginEdit = null;
			this.IsEditing = this.CanEdit;
			yield break;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0005582C File Offset: 0x00053C2C
		void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
		{
			this.m_canBeginEdit = (this.m_isSelected && this.ItemsControl != null && this.ItemsControl.SelectedItemsCount == 1 && this.ItemsControl.CanEdit);
			if (VirtualizingItemContainer.PointerDown != null)
			{
				VirtualizingItemContainer.PointerDown(this, eventData);
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00055890 File Offset: 0x00053C90
		void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
		{
			if (eventData.clickCount == 2)
			{
				if (VirtualizingItemContainer.DoubleClick != null)
				{
					VirtualizingItemContainer.DoubleClick(this, eventData);
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
				if (VirtualizingItemContainer.PointerUp != null)
				{
					VirtualizingItemContainer.PointerUp(this, eventData);
				}
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00055934 File Offset: 0x00053D34
		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				ExecuteEvents.ExecuteHierarchy<IBeginDragHandler>(base.transform.parent.gameObject, eventData, ExecuteEvents.beginDragHandler);
				return;
			}
			this.m_canBeginEdit = false;
			if (VirtualizingItemContainer.BeginDrag != null)
			{
				VirtualizingItemContainer.BeginDrag(this, eventData);
			}
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00055986 File Offset: 0x00053D86
		void IDropHandler.OnDrop(PointerEventData eventData)
		{
			if (VirtualizingItemContainer.Drop != null)
			{
				VirtualizingItemContainer.Drop(this, eventData);
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0005599E File Offset: 0x00053D9E
		void IDragHandler.OnDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				ExecuteEvents.ExecuteHierarchy<IDragHandler>(base.transform.parent.gameObject, eventData, ExecuteEvents.dragHandler);
				return;
			}
			if (VirtualizingItemContainer.Drag != null)
			{
				VirtualizingItemContainer.Drag(this, eventData);
			}
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000559DE File Offset: 0x00053DDE
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				ExecuteEvents.ExecuteHierarchy<IEndDragHandler>(base.transform.parent.gameObject, eventData, ExecuteEvents.endDragHandler);
				return;
			}
			if (VirtualizingItemContainer.EndDrag != null)
			{
				VirtualizingItemContainer.EndDrag(this, eventData);
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00055A1E File Offset: 0x00053E1E
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			if (VirtualizingItemContainer.PointerEnter != null)
			{
				VirtualizingItemContainer.PointerEnter(this, eventData);
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00055A36 File Offset: 0x00053E36
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			if (VirtualizingItemContainer.PointerExit != null)
			{
				VirtualizingItemContainer.PointerExit(this, eventData);
			}
		}

		// Token: 0x04000DD6 RID: 3542
		public bool CanDrag = true;

		// Token: 0x04000DD7 RID: 3543
		public bool CanEdit = true;

		// Token: 0x04000DD8 RID: 3544
		public bool CanDrop = true;

		// Token: 0x04000DD9 RID: 3545
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler Selected;

		// Token: 0x04000DDA RID: 3546
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler Unselected;

		// Token: 0x04000DDB RID: 3547
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler PointerDown;

		// Token: 0x04000DDC RID: 3548
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler PointerUp;

		// Token: 0x04000DDD RID: 3549
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler DoubleClick;

		// Token: 0x04000DDE RID: 3550
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler PointerEnter;

		// Token: 0x04000DDF RID: 3551
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler PointerExit;

		// Token: 0x04000DE0 RID: 3552
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler BeginDrag;

		// Token: 0x04000DE1 RID: 3553
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler Drag;

		// Token: 0x04000DE2 RID: 3554
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler Drop;

		// Token: 0x04000DE3 RID: 3555
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static VirtualizingItemEventHandler EndDrag;

		// Token: 0x04000DE4 RID: 3556
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler BeginEdit;

		// Token: 0x04000DE5 RID: 3557
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static EventHandler EndEdit;

		// Token: 0x04000DE6 RID: 3558
		public GameObject ItemPresenter;

		// Token: 0x04000DE7 RID: 3559
		public GameObject EditorPresenter;

		// Token: 0x04000DE8 RID: 3560
		private LayoutElement m_layoutElement;

		// Token: 0x04000DE9 RID: 3561
		private RectTransform m_rectTransform;

		// Token: 0x04000DEA RID: 3562
		protected bool m_isSelected;

		// Token: 0x04000DEB RID: 3563
		private bool m_isEditing;

		// Token: 0x04000DEC RID: 3564
		private VirtualizingItemsControl m_itemsControl;

		// Token: 0x04000DED RID: 3565
		private object m_item;

		// Token: 0x04000DEE RID: 3566
		private bool m_canBeginEdit;

		// Token: 0x04000DEF RID: 3567
		private IEnumerator m_coBeginEdit;

		// Token: 0x02000EE2 RID: 3810
		[CompilerGenerated]
		private sealed class <CoBeginEdit>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600720B RID: 29195 RVA: 0x00055A4E File Offset: 0x00053E4E
			[DebuggerHidden]
			public <CoBeginEdit>c__Iterator0()
			{
			}

			// Token: 0x0600720C RID: 29196 RVA: 0x00055A58 File Offset: 0x00053E58
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

			// Token: 0x170010A9 RID: 4265
			// (get) Token: 0x0600720D RID: 29197 RVA: 0x00055AD6 File Offset: 0x00053ED6
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010AA RID: 4266
			// (get) Token: 0x0600720E RID: 29198 RVA: 0x00055ADE File Offset: 0x00053EDE
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600720F RID: 29199 RVA: 0x00055AE6 File Offset: 0x00053EE6
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007210 RID: 29200 RVA: 0x00055AF6 File Offset: 0x00053EF6
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040065E5 RID: 26085
			internal VirtualizingItemContainer $this;

			// Token: 0x040065E6 RID: 26086
			internal object $current;

			// Token: 0x040065E7 RID: 26087
			internal bool $disposing;

			// Token: 0x040065E8 RID: 26088
			internal int $PC;
		}
	}
}
