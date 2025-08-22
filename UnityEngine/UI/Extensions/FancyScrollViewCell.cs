using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200050D RID: 1293
	public class FancyScrollViewCell<TData, TContext> : MonoBehaviour where TContext : class
	{
		// Token: 0x0600209E RID: 8350 RVA: 0x000AA87B File Offset: 0x000A8C7B
		public FancyScrollViewCell()
		{
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000AA883 File Offset: 0x000A8C83
		public virtual void SetContext(TContext context)
		{
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000AA885 File Offset: 0x000A8C85
		public virtual void UpdateContent(TData itemData)
		{
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000AA887 File Offset: 0x000A8C87
		public virtual void UpdatePosition(float position)
		{
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000AA889 File Offset: 0x000A8C89
		public virtual void SetVisible(bool visible)
		{
			base.gameObject.SetActive(visible);
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x000AA897 File Offset: 0x000A8C97
		// (set) Token: 0x060020A4 RID: 8356 RVA: 0x000AA89F File Offset: 0x000A8C9F
		public int DataIndex
		{
			[CompilerGenerated]
			get
			{
				return this.<DataIndex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DataIndex>k__BackingField = value;
			}
		}

		// Token: 0x04001B50 RID: 6992
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <DataIndex>k__BackingField;
	}
}
