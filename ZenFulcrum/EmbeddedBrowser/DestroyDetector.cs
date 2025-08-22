using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005AD RID: 1453
	internal class DestroyDetector : MonoBehaviour
	{
		// Token: 0x06002476 RID: 9334 RVA: 0x000D2C58 File Offset: 0x000D1058
		public DestroyDetector()
		{
			if (DestroyDetector.<>f__am$cache0 == null)
			{
				DestroyDetector.<>f__am$cache0 = new Action(DestroyDetector.<onDestroy>m__0);
			}
			this.onDestroy = DestroyDetector.<>f__am$cache0;
			base..ctor();
		}

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06002477 RID: 9335 RVA: 0x000D2C84 File Offset: 0x000D1084
		// (remove) Token: 0x06002478 RID: 9336 RVA: 0x000D2CBC File Offset: 0x000D10BC
		public event Action onDestroy
		{
			add
			{
				Action action = this.onDestroy;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.onDestroy, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = this.onDestroy;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action>(ref this.onDestroy, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x000D2CF2 File Offset: 0x000D10F2
		public void OnDestroy()
		{
			this.onDestroy();
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000D2CFF File Offset: 0x000D10FF
		[CompilerGenerated]
		private static void <onDestroy>m__0()
		{
		}

		// Token: 0x04001EA3 RID: 7843
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action onDestroy;

		// Token: 0x04001EA4 RID: 7844
		[CompilerGenerated]
		private static Action <>f__am$cache0;
	}
}
