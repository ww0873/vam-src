using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Battlehub.RTCommon
{
	// Token: 0x020000A9 RID: 169
	public static class DragDrop
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000282 RID: 642 RVA: 0x000122D4 File Offset: 0x000106D4
		// (remove) Token: 0x06000283 RID: 643 RVA: 0x00012308 File Offset: 0x00010708
		public static event BeginDragEventHandler BeginDrag
		{
			add
			{
				BeginDragEventHandler beginDragEventHandler = DragDrop.BeginDrag;
				BeginDragEventHandler beginDragEventHandler2;
				do
				{
					beginDragEventHandler2 = beginDragEventHandler;
					beginDragEventHandler = Interlocked.CompareExchange<BeginDragEventHandler>(ref DragDrop.BeginDrag, (BeginDragEventHandler)Delegate.Combine(beginDragEventHandler2, value), beginDragEventHandler);
				}
				while (beginDragEventHandler != beginDragEventHandler2);
			}
			remove
			{
				BeginDragEventHandler beginDragEventHandler = DragDrop.BeginDrag;
				BeginDragEventHandler beginDragEventHandler2;
				do
				{
					beginDragEventHandler2 = beginDragEventHandler;
					beginDragEventHandler = Interlocked.CompareExchange<BeginDragEventHandler>(ref DragDrop.BeginDrag, (BeginDragEventHandler)Delegate.Remove(beginDragEventHandler2, value), beginDragEventHandler);
				}
				while (beginDragEventHandler != beginDragEventHandler2);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000284 RID: 644 RVA: 0x0001233C File Offset: 0x0001073C
		// (remove) Token: 0x06000285 RID: 645 RVA: 0x00012370 File Offset: 0x00010770
		public static event DropEventHandler Drop
		{
			add
			{
				DropEventHandler dropEventHandler = DragDrop.Drop;
				DropEventHandler dropEventHandler2;
				do
				{
					dropEventHandler2 = dropEventHandler;
					dropEventHandler = Interlocked.CompareExchange<DropEventHandler>(ref DragDrop.Drop, (DropEventHandler)Delegate.Combine(dropEventHandler2, value), dropEventHandler);
				}
				while (dropEventHandler != dropEventHandler2);
			}
			remove
			{
				DropEventHandler dropEventHandler = DragDrop.Drop;
				DropEventHandler dropEventHandler2;
				do
				{
					dropEventHandler2 = dropEventHandler;
					dropEventHandler = Interlocked.CompareExchange<DropEventHandler>(ref DragDrop.Drop, (DropEventHandler)Delegate.Remove(dropEventHandler2, value), dropEventHandler);
				}
				while (dropEventHandler != dropEventHandler2);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000286 RID: 646 RVA: 0x000123A4 File Offset: 0x000107A4
		// (set) Token: 0x06000287 RID: 647 RVA: 0x000123AB File Offset: 0x000107AB
		public static UnityEngine.Object[] DragItems
		{
			[CompilerGenerated]
			get
			{
				return DragDrop.<DragItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				DragDrop.<DragItems>k__BackingField = value;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000123B3 File Offset: 0x000107B3
		public static void Reset()
		{
			DragDrop.DragItems = null;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000289 RID: 649 RVA: 0x000123BB File Offset: 0x000107BB
		public static UnityEngine.Object DragItem
		{
			get
			{
				if (DragDrop.DragItems == null || DragDrop.DragItems.Length == 0)
				{
					return null;
				}
				return DragDrop.DragItems[0];
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000123DC File Offset: 0x000107DC
		public static void RaiseBeginDrag(UnityEngine.Object[] dragItems)
		{
			DragDrop.DragItems = dragItems;
			if (DragDrop.BeginDrag != null)
			{
				DragDrop.BeginDrag();
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000123F8 File Offset: 0x000107F8
		public static void RaiseDrop()
		{
			if (DragDrop.Drop != null)
			{
				DragDrop.Drop();
			}
			DragDrop.DragItems = null;
		}

		// Token: 0x04000363 RID: 867
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static BeginDragEventHandler BeginDrag;

		// Token: 0x04000364 RID: 868
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DropEventHandler Drop;

		// Token: 0x04000365 RID: 869
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static UnityEngine.Object[] <DragItems>k__BackingField;
	}
}
