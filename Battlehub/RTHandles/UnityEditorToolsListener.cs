using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Battlehub.RTHandles
{
	// Token: 0x02000105 RID: 261
	public class UnityEditorToolsListener
	{
		// Token: 0x06000633 RID: 1587 RVA: 0x00028C40 File Offset: 0x00027040
		public UnityEditorToolsListener()
		{
		}

		// Token: 0x1400002D RID: 45
		// (add) Token: 0x06000634 RID: 1588 RVA: 0x00028C48 File Offset: 0x00027048
		// (remove) Token: 0x06000635 RID: 1589 RVA: 0x00028C7C File Offset: 0x0002707C
		public static event UnityEditorToolChanged ToolChanged
		{
			add
			{
				UnityEditorToolChanged unityEditorToolChanged = UnityEditorToolsListener.ToolChanged;
				UnityEditorToolChanged unityEditorToolChanged2;
				do
				{
					unityEditorToolChanged2 = unityEditorToolChanged;
					unityEditorToolChanged = Interlocked.CompareExchange<UnityEditorToolChanged>(ref UnityEditorToolsListener.ToolChanged, (UnityEditorToolChanged)Delegate.Combine(unityEditorToolChanged2, value), unityEditorToolChanged);
				}
				while (unityEditorToolChanged != unityEditorToolChanged2);
			}
			remove
			{
				UnityEditorToolChanged unityEditorToolChanged = UnityEditorToolsListener.ToolChanged;
				UnityEditorToolChanged unityEditorToolChanged2;
				do
				{
					unityEditorToolChanged2 = unityEditorToolChanged;
					unityEditorToolChanged = Interlocked.CompareExchange<UnityEditorToolChanged>(ref UnityEditorToolsListener.ToolChanged, (UnityEditorToolChanged)Delegate.Remove(unityEditorToolChanged2, value), unityEditorToolChanged);
				}
				while (unityEditorToolChanged != unityEditorToolChanged2);
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00028CB0 File Offset: 0x000270B0
		public static void Update()
		{
		}

		// Token: 0x040005F2 RID: 1522
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static UnityEditorToolChanged ToolChanged;
	}
}
