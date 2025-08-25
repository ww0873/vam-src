using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A4 RID: 1444
	public class HUDManager : MonoBehaviour
	{
		// Token: 0x0600242F RID: 9263 RVA: 0x000D15CE File Offset: 0x000CF9CE
		public HUDManager()
		{
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000D15D6 File Offset: 0x000CF9D6
		// (set) Token: 0x06002431 RID: 9265 RVA: 0x000D15DD File Offset: 0x000CF9DD
		public static HUDManager Instance
		{
			[CompilerGenerated]
			get
			{
				return HUDManager.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				HUDManager.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x000D15E5 File Offset: 0x000CF9E5
		// (set) Token: 0x06002433 RID: 9267 RVA: 0x000D15ED File Offset: 0x000CF9ED
		public Browser HUDBrowser
		{
			[CompilerGenerated]
			get
			{
				return this.<HUDBrowser>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HUDBrowser>k__BackingField = value;
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000D15F6 File Offset: 0x000CF9F6
		public void Awake()
		{
			HUDManager.Instance = this;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000D1600 File Offset: 0x000CFA00
		public void Start()
		{
			this.HUDBrowser = this.hud.GetComponent<Browser>();
			this.HUDBrowser.RegisterFunction("unpause", new Browser.JSCallback(this.<Start>m__0));
			this.HUDBrowser.RegisterFunction("browserMode", new Browser.JSCallback(this.<Start>m__1));
			Browser hudbrowser = this.HUDBrowser;
			string name = "quit";
			if (HUDManager.<>f__am$cache0 == null)
			{
				HUDManager.<>f__am$cache0 = new Browser.JSCallback(HUDManager.<Start>m__2);
			}
			hudbrowser.RegisterFunction(name, HUDManager.<>f__am$cache0);
			this.Unpause();
			PlayerInventory.Instance.coinCollected += this.<Start>m__3;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000D16A0 File Offset: 0x000CFAA0
		private IEnumerator Rehide()
		{
			while (!SplashScreen.isFinished)
			{
				yield return null;
			}
			Cursor.visible = false;
			yield return new WaitForSeconds(0.2f);
			Cursor.visible = true;
			yield return new WaitForSeconds(0.2f);
			Cursor.visible = false;
			yield break;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000D16B4 File Offset: 0x000CFAB4
		public void Unpause()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			this.EnableUserControls(true);
			Time.timeScale = 1f;
			this.haveMouse = true;
			this.HUDBrowser.CallFunction("setPaused", new JSONNode[]
			{
				false
			});
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000D1708 File Offset: 0x000CFB08
		public void Pause()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			this.haveMouse = false;
			Time.timeScale = 0f;
			this.EnableUserControls(false);
			this.HUDBrowser.CallFunction("setPaused", new JSONNode[]
			{
				true
			});
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000D1759 File Offset: 0x000CFB59
		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (this.haveMouse)
				{
					this.Pause();
				}
				else
				{
					this.Unpause();
				}
			}
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000D1783 File Offset: 0x000CFB83
		public void Say(string html, float dwellTime)
		{
			this.HUDBrowser.CallFunction("say", new JSONNode[]
			{
				html,
				dwellTime
			});
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000D17B0 File Offset: 0x000CFBB0
		protected void EnableUserControls(bool enableIt)
		{
			FPSCursorRenderer.Instance.EnableInput = enableIt;
			SimpleFPSController component = base.GetComponent<SimpleFPSController>();
			component.enabled = enableIt;
			this.hud.enableInput = !enableIt;
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000D17E5 File Offset: 0x000CFBE5
		public void LoadBrowseLevel(bool force = false)
		{
			base.StartCoroutine(this.LoadLevel(force));
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000D17F8 File Offset: 0x000CFBF8
		private IEnumerator LoadLevel(bool force = false)
		{
			if (!force)
			{
				if (HUDManager.<LoadLevel>c__Iterator1.<>f__am$cache0 == null)
				{
					HUDManager.<LoadLevel>c__Iterator1.<>f__am$cache0 = new Func<bool>(HUDManager.<LoadLevel>c__Iterator1.<>m__0);
				}
				yield return new WaitUntil(HUDManager.<LoadLevel>c__Iterator1.<>f__am$cache0);
			}
			this.Pause();
			SceneManager.LoadScene("SimpleBrowser");
			yield break;
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000D181A File Offset: 0x000CFC1A
		[CompilerGenerated]
		private void <Start>m__0(JSONNode args)
		{
			this.Unpause();
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000D1822 File Offset: 0x000CFC22
		[CompilerGenerated]
		private void <Start>m__1(JSONNode args)
		{
			this.LoadBrowseLevel(true);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000D182B File Offset: 0x000CFC2B
		[CompilerGenerated]
		private static void <Start>m__2(JSONNode args)
		{
			Application.Quit();
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000D1832 File Offset: 0x000CFC32
		[CompilerGenerated]
		private void <Start>m__3(int count)
		{
			this.HUDBrowser.CallFunction("setCoinCount", new JSONNode[]
			{
				count
			});
		}

		// Token: 0x04001E7A RID: 7802
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static HUDManager <Instance>k__BackingField;

		// Token: 0x04001E7B RID: 7803
		private bool haveMouse;

		// Token: 0x04001E7C RID: 7804
		public PointerUIGUI hud;

		// Token: 0x04001E7D RID: 7805
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Browser <HUDBrowser>k__BackingField;

		// Token: 0x04001E7E RID: 7806
		[CompilerGenerated]
		private static Browser.JSCallback <>f__am$cache0;

		// Token: 0x02000F85 RID: 3973
		[CompilerGenerated]
		private sealed class <Rehide>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600742C RID: 29740 RVA: 0x000D1854 File Offset: 0x000CFC54
			[DebuggerHidden]
			public <Rehide>c__Iterator0()
			{
			}

			// Token: 0x0600742D RID: 29741 RVA: 0x000D185C File Offset: 0x000CFC5C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					break;
				case 1U:
					break;
				case 2U:
					Cursor.visible = true;
					this.$current = new WaitForSeconds(0.2f);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3U:
					Cursor.visible = false;
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (SplashScreen.isFinished)
				{
					Cursor.visible = false;
					this.$current = new WaitForSeconds(0.2f);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x17001115 RID: 4373
			// (get) Token: 0x0600742E RID: 29742 RVA: 0x000D1920 File Offset: 0x000CFD20
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001116 RID: 4374
			// (get) Token: 0x0600742F RID: 29743 RVA: 0x000D1928 File Offset: 0x000CFD28
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007430 RID: 29744 RVA: 0x000D1930 File Offset: 0x000CFD30
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007431 RID: 29745 RVA: 0x000D1940 File Offset: 0x000CFD40
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006842 RID: 26690
			internal object $current;

			// Token: 0x04006843 RID: 26691
			internal bool $disposing;

			// Token: 0x04006844 RID: 26692
			internal int $PC;
		}

		// Token: 0x02000F86 RID: 3974
		[CompilerGenerated]
		private sealed class <LoadLevel>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007432 RID: 29746 RVA: 0x000D1947 File Offset: 0x000CFD47
			[DebuggerHidden]
			public <LoadLevel>c__Iterator1()
			{
			}

			// Token: 0x06007433 RID: 29747 RVA: 0x000D1950 File Offset: 0x000CFD50
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (!force)
					{
						if (HUDManager.<LoadLevel>c__Iterator1.<>f__am$cache0 == null)
						{
							HUDManager.<LoadLevel>c__Iterator1.<>f__am$cache0 = new Func<bool>(HUDManager.<LoadLevel>c__Iterator1.<>m__0);
						}
						this.$current = new WaitUntil(HUDManager.<LoadLevel>c__Iterator1.<>f__am$cache0);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1U:
					break;
				default:
					return false;
				}
				base.Pause();
				SceneManager.LoadScene("SimpleBrowser");
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001117 RID: 4375
			// (get) Token: 0x06007434 RID: 29748 RVA: 0x000D19E4 File Offset: 0x000CFDE4
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001118 RID: 4376
			// (get) Token: 0x06007435 RID: 29749 RVA: 0x000D19EC File Offset: 0x000CFDEC
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007436 RID: 29750 RVA: 0x000D19F4 File Offset: 0x000CFDF4
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007437 RID: 29751 RVA: 0x000D1A04 File Offset: 0x000CFE04
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007438 RID: 29752 RVA: 0x000D1A0B File Offset: 0x000CFE0B
			private static bool <>m__0()
			{
				return SayWordsOnTouch.ActiveSpeakers == 0;
			}

			// Token: 0x04006845 RID: 26693
			internal bool force;

			// Token: 0x04006846 RID: 26694
			internal HUDManager $this;

			// Token: 0x04006847 RID: 26695
			internal object $current;

			// Token: 0x04006848 RID: 26696
			internal bool $disposing;

			// Token: 0x04006849 RID: 26697
			internal int $PC;

			// Token: 0x0400684A RID: 26698
			private static Func<bool> <>f__am$cache0;
		}
	}
}
