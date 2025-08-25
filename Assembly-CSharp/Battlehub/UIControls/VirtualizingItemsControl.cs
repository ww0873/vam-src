using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Battlehub.UIControls
{
	// Token: 0x0200028E RID: 654
	public class VirtualizingItemsControl<TDataBindingArgs> : VirtualizingItemsControl where TDataBindingArgs : ItemDataBindingArgs, new()
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x00057F2B File Offset: 0x0005632B
		public VirtualizingItemsControl()
		{
		}

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06000EE3 RID: 3811 RVA: 0x00057F34 File Offset: 0x00056334
		// (remove) Token: 0x06000EE4 RID: 3812 RVA: 0x00057F6C File Offset: 0x0005636C
		public event EventHandler<TDataBindingArgs> ItemDataBinding
		{
			add
			{
				EventHandler<TDataBindingArgs> eventHandler = this.ItemDataBinding;
				EventHandler<TDataBindingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<TDataBindingArgs>>(ref this.ItemDataBinding, (EventHandler<TDataBindingArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<TDataBindingArgs> eventHandler = this.ItemDataBinding;
				EventHandler<TDataBindingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<TDataBindingArgs>>(ref this.ItemDataBinding, (EventHandler<TDataBindingArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06000EE5 RID: 3813 RVA: 0x00057FA4 File Offset: 0x000563A4
		// (remove) Token: 0x06000EE6 RID: 3814 RVA: 0x00057FDC File Offset: 0x000563DC
		public event EventHandler<TDataBindingArgs> ItemBeginEdit
		{
			add
			{
				EventHandler<TDataBindingArgs> eventHandler = this.ItemBeginEdit;
				EventHandler<TDataBindingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<TDataBindingArgs>>(ref this.ItemBeginEdit, (EventHandler<TDataBindingArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<TDataBindingArgs> eventHandler = this.ItemBeginEdit;
				EventHandler<TDataBindingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<TDataBindingArgs>>(ref this.ItemBeginEdit, (EventHandler<TDataBindingArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06000EE7 RID: 3815 RVA: 0x00058014 File Offset: 0x00056414
		// (remove) Token: 0x06000EE8 RID: 3816 RVA: 0x0005804C File Offset: 0x0005644C
		public event EventHandler<TDataBindingArgs> ItemEndEdit
		{
			add
			{
				EventHandler<TDataBindingArgs> eventHandler = this.ItemEndEdit;
				EventHandler<TDataBindingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<TDataBindingArgs>>(ref this.ItemEndEdit, (EventHandler<TDataBindingArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<TDataBindingArgs> eventHandler = this.ItemEndEdit;
				EventHandler<TDataBindingArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<TDataBindingArgs>>(ref this.ItemEndEdit, (EventHandler<TDataBindingArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00058084 File Offset: 0x00056484
		protected override void OnItemBeginEdit(object sender, EventArgs e)
		{
			if (!base.CanHandleEvent(sender))
			{
				return;
			}
			VirtualizingItemContainer virtualizingItemContainer = (VirtualizingItemContainer)sender;
			if (this.ItemBeginEdit != null)
			{
				TDataBindingArgs e2 = Activator.CreateInstance<TDataBindingArgs>();
				e2.Item = virtualizingItemContainer.Item;
				e2.ItemPresenter = ((!(virtualizingItemContainer.ItemPresenter == null)) ? virtualizingItemContainer.ItemPresenter : base.gameObject);
				e2.EditorPresenter = ((!(virtualizingItemContainer.EditorPresenter == null)) ? virtualizingItemContainer.EditorPresenter : base.gameObject);
				this.ItemBeginEdit(this, e2);
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00058134 File Offset: 0x00056534
		protected override void OnItemEndEdit(object sender, EventArgs e)
		{
			if (!base.CanHandleEvent(sender))
			{
				return;
			}
			VirtualizingItemContainer virtualizingItemContainer = (VirtualizingItemContainer)sender;
			if (this.ItemBeginEdit != null)
			{
				TDataBindingArgs e2 = Activator.CreateInstance<TDataBindingArgs>();
				e2.Item = virtualizingItemContainer.Item;
				e2.ItemPresenter = ((!(virtualizingItemContainer.ItemPresenter == null)) ? virtualizingItemContainer.ItemPresenter : base.gameObject);
				e2.EditorPresenter = ((!(virtualizingItemContainer.EditorPresenter == null)) ? virtualizingItemContainer.EditorPresenter : base.gameObject);
				this.ItemEndEdit(this, e2);
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x000581E4 File Offset: 0x000565E4
		public override void DataBindItem(object item, ItemContainerData itemContainerData, VirtualizingItemContainer itemContainer)
		{
			TDataBindingArgs args = Activator.CreateInstance<TDataBindingArgs>();
			args.Item = item;
			args.ItemPresenter = ((!(itemContainer.ItemPresenter == null)) ? itemContainer.ItemPresenter : base.gameObject);
			args.EditorPresenter = ((!(itemContainer.EditorPresenter == null)) ? itemContainer.EditorPresenter : base.gameObject);
			itemContainer.Clear();
			this.RaiseItemDataBinding(args);
			itemContainer.CanEdit = args.CanEdit;
			itemContainer.CanDrag = args.CanDrag;
			itemContainer.CanDrop = args.CanDrop;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000582A9 File Offset: 0x000566A9
		protected void RaiseItemDataBinding(TDataBindingArgs args)
		{
			if (this.ItemDataBinding != null)
			{
				this.ItemDataBinding(this, args);
			}
		}

		// Token: 0x04000DF6 RID: 3574
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<TDataBindingArgs> ItemDataBinding;

		// Token: 0x04000DF7 RID: 3575
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<TDataBindingArgs> ItemBeginEdit;

		// Token: 0x04000DF8 RID: 3576
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<TDataBindingArgs> ItemEndEdit;
	}
}
