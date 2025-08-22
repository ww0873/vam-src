using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
	// Token: 0x02000283 RID: 643
	public class RectTransformChangeListener : UIBehaviour
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x000537D1 File Offset: 0x00051BD1
		public RectTransformChangeListener()
		{
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06000E4C RID: 3660 RVA: 0x000537DC File Offset: 0x00051BDC
		// (remove) Token: 0x06000E4D RID: 3661 RVA: 0x00053814 File Offset: 0x00051C14
		public event RectTransformChanged RectTransformChanged
		{
			add
			{
				RectTransformChanged rectTransformChanged = this.RectTransformChanged;
				RectTransformChanged rectTransformChanged2;
				do
				{
					rectTransformChanged2 = rectTransformChanged;
					rectTransformChanged = Interlocked.CompareExchange<RectTransformChanged>(ref this.RectTransformChanged, (RectTransformChanged)Delegate.Combine(rectTransformChanged2, value), rectTransformChanged);
				}
				while (rectTransformChanged != rectTransformChanged2);
			}
			remove
			{
				RectTransformChanged rectTransformChanged = this.RectTransformChanged;
				RectTransformChanged rectTransformChanged2;
				do
				{
					rectTransformChanged2 = rectTransformChanged;
					rectTransformChanged = Interlocked.CompareExchange<RectTransformChanged>(ref this.RectTransformChanged, (RectTransformChanged)Delegate.Remove(rectTransformChanged2, value), rectTransformChanged);
				}
				while (rectTransformChanged != rectTransformChanged2);
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0005384A File Offset: 0x00051C4A
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.RectTransformChanged != null)
			{
				this.RectTransformChanged();
			}
		}

		// Token: 0x04000DBC RID: 3516
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private RectTransformChanged RectTransformChanged;
	}
}
