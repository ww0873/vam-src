using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A5 RID: 1445
	public class PlayerInventory : MonoBehaviour
	{
		// Token: 0x06002442 RID: 9282 RVA: 0x000D1A15 File Offset: 0x000CFE15
		public PlayerInventory()
		{
			if (PlayerInventory.<>f__am$cache0 == null)
			{
				PlayerInventory.<>f__am$cache0 = new Action<int>(PlayerInventory.<coinCollected>m__0);
			}
			this.coinCollected = PlayerInventory.<>f__am$cache0;
			base..ctor();
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x000D1A40 File Offset: 0x000CFE40
		// (set) Token: 0x06002444 RID: 9284 RVA: 0x000D1A47 File Offset: 0x000CFE47
		public static PlayerInventory Instance
		{
			[CompilerGenerated]
			get
			{
				return PlayerInventory.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				PlayerInventory.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06002445 RID: 9285 RVA: 0x000D1A4F File Offset: 0x000CFE4F
		// (set) Token: 0x06002446 RID: 9286 RVA: 0x000D1A57 File Offset: 0x000CFE57
		public int NumCoins
		{
			[CompilerGenerated]
			get
			{
				return this.<NumCoins>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<NumCoins>k__BackingField = value;
			}
		}

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06002447 RID: 9287 RVA: 0x000D1A60 File Offset: 0x000CFE60
		// (remove) Token: 0x06002448 RID: 9288 RVA: 0x000D1A98 File Offset: 0x000CFE98
		public event Action<int> coinCollected
		{
			add
			{
				Action<int> action = this.coinCollected;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<int>>(ref this.coinCollected, (Action<int>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<int> action = this.coinCollected;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<int>>(ref this.coinCollected, (Action<int>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x000D1ACE File Offset: 0x000CFECE
		public void Awake()
		{
			PlayerInventory.Instance = this;
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x000D1AD6 File Offset: 0x000CFED6
		public void AddCoin()
		{
			this.NumCoins++;
			this.coinCollected(this.NumCoins);
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x000D1AF7 File Offset: 0x000CFEF7
		[CompilerGenerated]
		private static void <coinCollected>m__0(int coins)
		{
		}

		// Token: 0x04001E7F RID: 7807
		public HUDManager hud;

		// Token: 0x04001E80 RID: 7808
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static PlayerInventory <Instance>k__BackingField;

		// Token: 0x04001E81 RID: 7809
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <NumCoins>k__BackingField;

		// Token: 0x04001E82 RID: 7810
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<int> coinCollected;

		// Token: 0x04001E83 RID: 7811
		[CompilerGenerated]
		private static Action<int> <>f__am$cache0;
	}
}
