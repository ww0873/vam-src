using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Battlehub.UIControls
{
	// Token: 0x02000277 RID: 631
	public class ItemsControl<TDataBindingArgs> : ItemsControl where TDataBindingArgs : ItemDataBindingArgs, new()
	{
		// Token: 0x06000DB0 RID: 3504 RVA: 0x000518E2 File Offset: 0x0004FCE2
		public ItemsControl()
		{
		}

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06000DB1 RID: 3505 RVA: 0x000518EC File Offset: 0x0004FCEC
		// (remove) Token: 0x06000DB2 RID: 3506 RVA: 0x00051924 File Offset: 0x0004FD24
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

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06000DB3 RID: 3507 RVA: 0x0005195C File Offset: 0x0004FD5C
		// (remove) Token: 0x06000DB4 RID: 3508 RVA: 0x00051994 File Offset: 0x0004FD94
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

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06000DB5 RID: 3509 RVA: 0x000519CC File Offset: 0x0004FDCC
		// (remove) Token: 0x06000DB6 RID: 3510 RVA: 0x00051A04 File Offset: 0x0004FE04
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

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00051A3C File Offset: 0x0004FE3C
		protected override void OnItemBeginEdit(object sender, EventArgs e)
		{
			if (!base.CanHandleEvent(sender))
			{
				return;
			}
			ItemContainer itemContainer = (ItemContainer)sender;
			if (this.ItemBeginEdit != null)
			{
				TDataBindingArgs e2 = Activator.CreateInstance<TDataBindingArgs>();
				e2.Item = itemContainer.Item;
				e2.ItemPresenter = ((!(itemContainer.ItemPresenter == null)) ? itemContainer.ItemPresenter : base.gameObject);
				e2.EditorPresenter = ((!(itemContainer.EditorPresenter == null)) ? itemContainer.EditorPresenter : base.gameObject);
				this.ItemBeginEdit(this, e2);
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00051AEC File Offset: 0x0004FEEC
		protected override void OnItemEndEdit(object sender, EventArgs e)
		{
			if (!base.CanHandleEvent(sender))
			{
				return;
			}
			ItemContainer itemContainer = (ItemContainer)sender;
			if (this.ItemBeginEdit != null)
			{
				TDataBindingArgs e2 = Activator.CreateInstance<TDataBindingArgs>();
				e2.Item = itemContainer.Item;
				e2.ItemPresenter = ((!(itemContainer.ItemPresenter == null)) ? itemContainer.ItemPresenter : base.gameObject);
				e2.EditorPresenter = ((!(itemContainer.EditorPresenter == null)) ? itemContainer.EditorPresenter : base.gameObject);
				this.ItemEndEdit(this, e2);
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00051B9C File Offset: 0x0004FF9C
		public override void DataBindItem(object item, ItemContainer itemContainer)
		{
			TDataBindingArgs args = Activator.CreateInstance<TDataBindingArgs>();
			args.Item = item;
			args.ItemPresenter = ((!(itemContainer.ItemPresenter == null)) ? itemContainer.ItemPresenter : base.gameObject);
			args.EditorPresenter = ((!(itemContainer.EditorPresenter == null)) ? itemContainer.EditorPresenter : base.gameObject);
			this.RaiseItemDataBinding(args);
			itemContainer.CanEdit = args.CanEdit;
			itemContainer.CanDrag = args.CanDrag;
			itemContainer.CanDrop = args.CanDrop;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00051C5B File Offset: 0x0005005B
		protected void RaiseItemDataBinding(TDataBindingArgs args)
		{
			if (this.ItemDataBinding != null)
			{
				this.ItemDataBinding(this, args);
			}
		}

		// Token: 0x04000D61 RID: 3425
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<TDataBindingArgs> ItemDataBinding;

		// Token: 0x04000D62 RID: 3426
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<TDataBindingArgs> ItemBeginEdit;

		// Token: 0x04000D63 RID: 3427
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<TDataBindingArgs> ItemEndEdit;
	}
}
