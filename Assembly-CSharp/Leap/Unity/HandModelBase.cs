using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006E9 RID: 1769
	[ExecuteInEditMode]
	public abstract class HandModelBase : MonoBehaviour
	{
		// Token: 0x06002AC6 RID: 10950 RVA: 0x000E5892 File Offset: 0x000E3C92
		protected HandModelBase()
		{
		}

		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x06002AC7 RID: 10951 RVA: 0x000E589C File Offset: 0x000E3C9C
		// (remove) Token: 0x06002AC8 RID: 10952 RVA: 0x000E58D4 File Offset: 0x000E3CD4
		public event Action OnBegin
		{
			add
			{
				Action action = this.OnBegin;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.OnBegin, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.OnBegin;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.OnBegin, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x06002AC9 RID: 10953 RVA: 0x000E590C File Offset: 0x000E3D0C
		// (remove) Token: 0x06002ACA RID: 10954 RVA: 0x000E5944 File Offset: 0x000E3D44
		public event Action OnFinish
		{
			add
			{
				Action action = this.OnFinish;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.OnFinish, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.OnFinish;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.OnFinish, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000E597A File Offset: 0x000E3D7A
		public bool IsTracked
		{
			get
			{
				return this.isTracked;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002ACC RID: 10956
		// (set) Token: 0x06002ACD RID: 10957
		public abstract Chirality Handedness { get; set; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06002ACE RID: 10958
		public abstract ModelType HandModelType { get; }

		// Token: 0x06002ACF RID: 10959 RVA: 0x000E5982 File Offset: 0x000E3D82
		public virtual void InitHand()
		{
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000E5984 File Offset: 0x000E3D84
		public virtual void BeginHand()
		{
			if (this.OnBegin != null)
			{
				this.OnBegin();
			}
			this.isTracked = true;
		}

		// Token: 0x06002AD1 RID: 10961
		public abstract void UpdateHand();

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000E59A3 File Offset: 0x000E3DA3
		public virtual void FinishHand()
		{
			if (this.OnFinish != null)
			{
				this.OnFinish();
			}
			this.isTracked = false;
		}

		// Token: 0x06002AD3 RID: 10963
		public abstract Hand GetLeapHand();

		// Token: 0x06002AD4 RID: 10964
		public abstract void SetLeapHand(Hand hand);

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000E59C2 File Offset: 0x000E3DC2
		public virtual bool SupportsEditorPersistence()
		{
			return false;
		}

		// Token: 0x040022C2 RID: 8898
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBegin;

		// Token: 0x040022C3 RID: 8899
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnFinish;

		// Token: 0x040022C4 RID: 8900
		private bool isTracked;

		// Token: 0x040022C5 RID: 8901
		[NonSerialized]
		public HandModelManager.ModelGroup group;
	}
}
