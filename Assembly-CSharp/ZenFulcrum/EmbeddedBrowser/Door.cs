using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A0 RID: 1440
	public class Door : MonoBehaviour
	{
		// Token: 0x0600241F RID: 9247 RVA: 0x000D0FB0 File Offset: 0x000CF3B0
		public Door()
		{
			if (Door.<>f__am$cache0 == null)
			{
				Door.<>f__am$cache0 = new Action<Door.OpenState>(Door.<stateChange>m__0);
			}
			this.stateChange = Door.<>f__am$cache0;
			base..ctor();
		}

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06002420 RID: 9248 RVA: 0x000D100C File Offset: 0x000CF40C
		// (remove) Token: 0x06002421 RID: 9249 RVA: 0x000D1044 File Offset: 0x000CF444
		public event Action<Door.OpenState> stateChange
		{
			add
			{
				Action<Door.OpenState> action = this.stateChange;
				Action<Door.OpenState> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<Door.OpenState>>(ref this.stateChange, (Action<Door.OpenState>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<Door.OpenState> action = this.stateChange;
				Action<Door.OpenState> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<Door.OpenState>>(ref this.stateChange, (Action<Door.OpenState>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x000D107A File Offset: 0x000CF47A
		// (set) Token: 0x06002423 RID: 9251 RVA: 0x000D1082 File Offset: 0x000CF482
		public Door.OpenState State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.stateChange(this._state);
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000D109C File Offset: 0x000CF49C
		public void Start()
		{
			Door.<Start>c__AnonStorey0 <Start>c__AnonStorey = new Door.<Start>c__AnonStorey0();
			<Start>c__AnonStorey.$this = this;
			this.closedPos = base.transform.position;
			this.openPos = base.transform.position + this.openOffset;
			this.State = Door.OpenState.Closed;
			<Start>c__AnonStorey.browser = base.GetComponentInChildren<Browser>();
			<Start>c__AnonStorey.browser.CallFunction("setRequiredCoins", new JSONNode[]
			{
				this.numCoins
			});
			<Start>c__AnonStorey.browser.RegisterFunction("toggleDoor", new Browser.JSCallback(<Start>c__AnonStorey.<>m__0));
			PlayerInventory.Instance.coinCollected += <Start>c__AnonStorey.<>m__1;
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000D114D File Offset: 0x000CF54D
		public void Toggle()
		{
			if (this.State == Door.OpenState.Open || this.State == Door.OpenState.Opening)
			{
				this.Close();
			}
			else
			{
				this.Open();
			}
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000D1177 File Offset: 0x000CF577
		public void Open()
		{
			if (this.State == Door.OpenState.Open)
			{
				return;
			}
			this.State = Door.OpenState.Opening;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000D118C File Offset: 0x000CF58C
		public void Close()
		{
			if (this.State == Door.OpenState.Closed)
			{
				return;
			}
			this.State = Door.OpenState.Closing;
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000D11A4 File Offset: 0x000CF5A4
		public void Update()
		{
			if (this.State == Door.OpenState.Opening)
			{
				float num = Vector3.Distance(base.transform.position, this.closedPos) / this.openOffset.magnitude;
				num = Mathf.Min(1f, num + Time.deltaTime / this.openSpeed);
				base.transform.position = Vector3.Lerp(this.closedPos, this.openPos, num);
				if (num >= 1f)
				{
					this.State = Door.OpenState.Open;
				}
			}
			else if (this.State == Door.OpenState.Closing)
			{
				float num2 = Vector3.Distance(base.transform.position, this.openPos) / this.openOffset.magnitude;
				num2 = Mathf.Min(1f, num2 + Time.deltaTime / this.openSpeed);
				base.transform.position = Vector3.Lerp(this.openPos, this.closedPos, num2);
				if (num2 >= 1f)
				{
					this.State = Door.OpenState.Closed;
				}
			}
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000D12A4 File Offset: 0x000CF6A4
		[CompilerGenerated]
		private static void <stateChange>m__0(Door.OpenState state)
		{
		}

		// Token: 0x04001E6B RID: 7787
		public Vector3 openOffset = new Vector3(0f, -6.1f, 0f);

		// Token: 0x04001E6C RID: 7788
		[Tooltip("Time to open or close, in seconds.")]
		public float openSpeed = 2f;

		// Token: 0x04001E6D RID: 7789
		[Tooltip("Number of coins needed to open the door.")]
		public int numCoins;

		// Token: 0x04001E6E RID: 7790
		private Vector3 closedPos;

		// Token: 0x04001E6F RID: 7791
		private Vector3 openPos;

		// Token: 0x04001E70 RID: 7792
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Door.OpenState> stateChange;

		// Token: 0x04001E71 RID: 7793
		private Door.OpenState _state;

		// Token: 0x04001E72 RID: 7794
		[CompilerGenerated]
		private static Action<Door.OpenState> <>f__am$cache0;

		// Token: 0x020005A1 RID: 1441
		public enum OpenState
		{
			// Token: 0x04001E74 RID: 7796
			Open,
			// Token: 0x04001E75 RID: 7797
			Closed,
			// Token: 0x04001E76 RID: 7798
			Opening,
			// Token: 0x04001E77 RID: 7799
			Closing
		}

		// Token: 0x02000F82 RID: 3970
		[CompilerGenerated]
		private sealed class <Start>c__AnonStorey0
		{
			// Token: 0x0600741F RID: 29727 RVA: 0x000D12A6 File Offset: 0x000CF6A6
			public <Start>c__AnonStorey0()
			{
			}

			// Token: 0x06007420 RID: 29728 RVA: 0x000D12B0 File Offset: 0x000CF6B0
			internal void <>m__0(JSONNode args)
			{
				string text = args[0].Check();
				if (text != null)
				{
					if (!(text == "open"))
					{
						if (!(text == "close"))
						{
							if (text == "toggle")
							{
								this.$this.Toggle();
							}
						}
						else
						{
							this.$this.Close();
						}
					}
					else
					{
						this.$this.Open();
					}
				}
			}

			// Token: 0x06007421 RID: 29729 RVA: 0x000D133A File Offset: 0x000CF73A
			internal void <>m__1(int coinCount)
			{
				this.browser.CallFunction("setCoinCoint", new JSONNode[]
				{
					coinCount
				});
			}

			// Token: 0x04006838 RID: 26680
			internal Browser browser;

			// Token: 0x04006839 RID: 26681
			internal Door $this;
		}
	}
}
