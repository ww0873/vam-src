using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Battlehub.UIControls
{
	// Token: 0x02000273 RID: 627
	public class ItemDataBindingArgs : EventArgs
	{
		// Token: 0x06000D94 RID: 3476 RVA: 0x00052A12 File Offset: 0x00050E12
		public ItemDataBindingArgs()
		{
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00052A2F File Offset: 0x00050E2F
		// (set) Token: 0x06000D96 RID: 3478 RVA: 0x00052A37 File Offset: 0x00050E37
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00052A40 File Offset: 0x00050E40
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x00052A48 File Offset: 0x00050E48
		public GameObject ItemPresenter
		{
			[CompilerGenerated]
			get
			{
				return this.<ItemPresenter>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ItemPresenter>k__BackingField = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00052A51 File Offset: 0x00050E51
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x00052A59 File Offset: 0x00050E59
		public GameObject EditorPresenter
		{
			[CompilerGenerated]
			get
			{
				return this.<EditorPresenter>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EditorPresenter>k__BackingField = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00052A62 File Offset: 0x00050E62
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x00052A6A File Offset: 0x00050E6A
		public bool CanEdit
		{
			get
			{
				return this.m_canEdit;
			}
			set
			{
				this.m_canEdit = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00052A73 File Offset: 0x00050E73
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00052A7B File Offset: 0x00050E7B
		public bool CanDrag
		{
			get
			{
				return this.m_canDrag;
			}
			set
			{
				this.m_canDrag = value;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00052A84 File Offset: 0x00050E84
		// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00052A8C File Offset: 0x00050E8C
		public bool CanDrop
		{
			get
			{
				return this.m_canDrop;
			}
			set
			{
				this.m_canDrop = value;
			}
		}

		// Token: 0x04000D55 RID: 3413
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object <Item>k__BackingField;

		// Token: 0x04000D56 RID: 3414
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameObject <ItemPresenter>k__BackingField;

		// Token: 0x04000D57 RID: 3415
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameObject <EditorPresenter>k__BackingField;

		// Token: 0x04000D58 RID: 3416
		private bool m_canEdit = true;

		// Token: 0x04000D59 RID: 3417
		private bool m_canDrag = true;

		// Token: 0x04000D5A RID: 3418
		private bool m_canDrop = true;
	}
}
