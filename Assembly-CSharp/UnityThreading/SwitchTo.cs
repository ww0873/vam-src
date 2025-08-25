using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityThreading
{
	// Token: 0x0200034A RID: 842
	public class SwitchTo
	{
		// Token: 0x06001472 RID: 5234 RVA: 0x00075B03 File Offset: 0x00073F03
		private SwitchTo(SwitchTo.TargetType target)
		{
			this.Target = target;
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06001473 RID: 5235 RVA: 0x00075B12 File Offset: 0x00073F12
		// (set) Token: 0x06001474 RID: 5236 RVA: 0x00075B1A File Offset: 0x00073F1A
		public SwitchTo.TargetType Target
		{
			[CompilerGenerated]
			get
			{
				return this.<Target>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Target>k__BackingField = value;
			}
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00075B23 File Offset: 0x00073F23
		// Note: this type is marked as 'beforefieldinit'.
		static SwitchTo()
		{
		}

		// Token: 0x0400118D RID: 4493
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private SwitchTo.TargetType <Target>k__BackingField;

		// Token: 0x0400118E RID: 4494
		public static readonly SwitchTo MainThread = new SwitchTo(SwitchTo.TargetType.Main);

		// Token: 0x0400118F RID: 4495
		public static readonly SwitchTo Thread = new SwitchTo(SwitchTo.TargetType.Thread);

		// Token: 0x0200034B RID: 843
		public enum TargetType
		{
			// Token: 0x04001191 RID: 4497
			Main,
			// Token: 0x04001192 RID: 4498
			Thread
		}
	}
}
