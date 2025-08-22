using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.Cubeman;
using Battlehub.RTCommon;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTHandles
{
	// Token: 0x020000EB RID: 235
	public class CubemenGame : Game
	{
		// Token: 0x060004E9 RID: 1257 RVA: 0x0001B36E File Offset: 0x0001976E
		public CubemenGame()
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001B376 File Offset: 0x00019776
		protected override void AwakeOverride()
		{
			this.m_playerCamera = UnityEngine.Object.FindObjectOfType<RTHandlesDemoSmoothFollow>();
			this.StartGame();
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001B38C File Offset: 0x0001978C
		private void StartGame()
		{
			this.m_gameOver = false;
			this.m_playerCamera = UnityEngine.Object.FindObjectOfType<RTHandlesDemoSmoothFollow>();
			if (this.m_playerCamera != null)
			{
				Canvas componentInChildren = base.GetComponentInChildren<Canvas>();
				Camera component = this.m_playerCamera.GetComponent<Camera>();
				componentInChildren.worldCamera = component;
				componentInChildren.planeDistance = component.nearClipPlane + 0.01f;
			}
			this.m_cubemans = new List<GameCharacter>();
			IEnumerable<CubemanUserControl> source = UnityEngine.Object.FindObjectsOfType<CubemanUserControl>();
			if (CubemenGame.<>f__am$cache0 == null)
			{
				CubemenGame.<>f__am$cache0 = new Func<CubemanUserControl, string>(CubemenGame.<StartGame>m__0);
			}
			CubemanUserControl[] array = source.OrderBy(CubemenGame.<>f__am$cache0).ToArray<CubemanUserControl>();
			for (int i = 0; i < array.Length; i++)
			{
				Rigidbody component2 = array[i].GetComponent<Rigidbody>();
				if (component2)
				{
					component2.isKinematic = false;
				}
				CubemanCharacter component3 = array[i].GetComponent<CubemanCharacter>();
				if (component3)
				{
					component3.Enabled = true;
				}
				GameCharacter gameCharacter = array[i].GetComponent<GameCharacter>();
				if (gameCharacter == null)
				{
					gameCharacter = array[i].gameObject.AddComponent<GameCharacter>();
				}
				if (gameCharacter != null)
				{
					gameCharacter.Game = this;
				}
				if (this.m_playerCamera != null)
				{
					gameCharacter.Camera = this.m_playerCamera.transform;
				}
				this.m_cubemans.Add(gameCharacter);
			}
			this.Begin();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001B4E1 File Offset: 0x000198E1
		protected override void OnActiveWindowChanged()
		{
			this.TryActivateCharacter();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001B4E9 File Offset: 0x000198E9
		private void TryActivateCharacter()
		{
			if (this.m_current != null)
			{
				this.m_current.IsActive = (RuntimeEditorApplication.ActiveWindowType == RuntimeWindowType.GameView || !RuntimeEditorApplication.IsOpened);
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001B520 File Offset: 0x00019920
		private void Update()
		{
			if (!RuntimeEditorApplication.IsActiveWindow(RuntimeWindowType.GameView) && RuntimeEditorApplication.IsOpened)
			{
				return;
			}
			if (InputController.GetKeyDown(KeyCode.Return))
			{
				this.SwitchPlayer(this.m_current, 0f, true);
			}
			else if (InputController.GetKeyDown(KeyCode.Backspace))
			{
				this.SwitchPlayer(this.m_current, 0f, false);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001B584 File Offset: 0x00019984
		private void UpdateScore()
		{
			this.TxtScore.text = string.Concat(new object[]
			{
				"Saved : ",
				this.m_score,
				" / ",
				this.m_total
			});
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001B5D3 File Offset: 0x000199D3
		private bool IsGameCompleted()
		{
			return this.m_cubemans.Count == 0;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001B5E4 File Offset: 0x000199E4
		private void Begin()
		{
			this.m_total = this.m_cubemans.Count;
			this.m_score = 0;
			if (this.m_total == 0)
			{
				this.TxtCompleted.gameObject.SetActive(true);
				this.TxtScore.gameObject.SetActive(false);
				this.TxtTip.gameObject.SetActive(false);
				this.TxtCompleted.text = "Game Over!";
				this.m_gameOver = true;
			}
			else
			{
				this.TxtCompleted.gameObject.SetActive(false);
				this.TxtScore.gameObject.SetActive(true);
				this.UpdateScore();
				this.SwitchPlayer(null, 0f, true);
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001B698 File Offset: 0x00019A98
		public void OnPlayerFinish(GameCharacter gameCharacter)
		{
			this.m_score++;
			this.UpdateScore();
			this.SwitchPlayer(gameCharacter, 1f, true);
			this.m_cubemans.Remove(gameCharacter);
			if (this.IsGameCompleted())
			{
				this.m_gameOver = true;
				this.TxtTip.gameObject.SetActive(false);
				base.StartCoroutine(this.ShowText("Congratulation! \n You have completed a great game "));
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001B708 File Offset: 0x00019B08
		private IEnumerator ShowText(string text)
		{
			yield return new WaitForSeconds(1.5f);
			if (this.m_gameOver)
			{
				this.TxtScore.gameObject.SetActive(false);
				this.TxtCompleted.gameObject.SetActive(true);
				this.TxtCompleted.text = text;
			}
			yield break;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001B72C File Offset: 0x00019B2C
		public void OnPlayerDie(GameCharacter gameCharacter)
		{
			this.m_gameOver = true;
			this.m_cubemans.Remove(gameCharacter);
			this.TxtTip.gameObject.SetActive(false);
			base.StartCoroutine(this.ShowText("Game Over!"));
			for (int i = 0; i < this.m_cubemans.Count; i++)
			{
				this.m_cubemans[i].IsActive = false;
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001B7A0 File Offset: 0x00019BA0
		public void SwitchPlayer(GameCharacter current, float delay, bool next)
		{
			if (this.m_gameOver)
			{
				return;
			}
			int num = 0;
			if (current != null)
			{
				current.IsActive = false;
				num = this.m_cubemans.IndexOf(current);
				if (next)
				{
					num++;
					if (num >= this.m_cubemans.Count)
					{
						num = 0;
					}
				}
				else
				{
					num--;
					if (num < 0)
					{
						num = this.m_cubemans.Count - 1;
					}
				}
			}
			this.m_current = this.m_cubemans[num];
			if (current == null)
			{
				this.ActivatePlayer();
			}
			else
			{
				base.StartCoroutine(this.ActivateNextPlayer(delay));
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001B850 File Offset: 0x00019C50
		private IEnumerator ActivateNextPlayer(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (this.m_gameOver)
			{
				yield break;
			}
			this.ActivatePlayer();
			yield break;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001B872 File Offset: 0x00019C72
		private void ActivatePlayer()
		{
			this.TryActivateCharacter();
			if (this.m_playerCamera != null)
			{
				this.m_playerCamera.target = this.m_current.transform;
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001B8A1 File Offset: 0x00019CA1
		[CompilerGenerated]
		private static string <StartGame>m__0(CubemanUserControl c)
		{
			return c.name;
		}

		// Token: 0x04000486 RID: 1158
		public Text TxtScore;

		// Token: 0x04000487 RID: 1159
		public Text TxtCompleted;

		// Token: 0x04000488 RID: 1160
		public Text TxtTip;

		// Token: 0x04000489 RID: 1161
		private int m_score;

		// Token: 0x0400048A RID: 1162
		private int m_total;

		// Token: 0x0400048B RID: 1163
		private bool m_gameOver;

		// Token: 0x0400048C RID: 1164
		private List<GameCharacter> m_cubemans;

		// Token: 0x0400048D RID: 1165
		private GameCharacter m_current;

		// Token: 0x0400048E RID: 1166
		private RTHandlesDemoSmoothFollow m_playerCamera;

		// Token: 0x0400048F RID: 1167
		[CompilerGenerated]
		private static Func<CubemanUserControl, string> <>f__am$cache0;

		// Token: 0x02000EA6 RID: 3750
		[CompilerGenerated]
		private sealed class <ShowText>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007165 RID: 29029 RVA: 0x0001B8A9 File Offset: 0x00019CA9
			[DebuggerHidden]
			public <ShowText>c__Iterator0()
			{
			}

			// Token: 0x06007166 RID: 29030 RVA: 0x0001B8B4 File Offset: 0x00019CB4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForSeconds(1.5f);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (this.m_gameOver)
					{
						this.TxtScore.gameObject.SetActive(false);
						this.TxtCompleted.gameObject.SetActive(true);
						this.TxtCompleted.text = text;
					}
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010A3 RID: 4259
			// (get) Token: 0x06007167 RID: 29031 RVA: 0x0001B962 File Offset: 0x00019D62
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010A4 RID: 4260
			// (get) Token: 0x06007168 RID: 29032 RVA: 0x0001B96A File Offset: 0x00019D6A
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007169 RID: 29033 RVA: 0x0001B972 File Offset: 0x00019D72
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600716A RID: 29034 RVA: 0x0001B982 File Offset: 0x00019D82
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006538 RID: 25912
			internal string text;

			// Token: 0x04006539 RID: 25913
			internal CubemenGame $this;

			// Token: 0x0400653A RID: 25914
			internal object $current;

			// Token: 0x0400653B RID: 25915
			internal bool $disposing;

			// Token: 0x0400653C RID: 25916
			internal int $PC;
		}

		// Token: 0x02000EA7 RID: 3751
		[CompilerGenerated]
		private sealed class <ActivateNextPlayer>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600716B RID: 29035 RVA: 0x0001B989 File Offset: 0x00019D89
			[DebuggerHidden]
			public <ActivateNextPlayer>c__Iterator1()
			{
			}

			// Token: 0x0600716C RID: 29036 RVA: 0x0001B994 File Offset: 0x00019D94
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForSeconds(delay);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (!this.m_gameOver)
					{
						base.ActivatePlayer();
						this.$PC = -1;
					}
					break;
				}
				return false;
			}

			// Token: 0x170010A5 RID: 4261
			// (get) Token: 0x0600716D RID: 29037 RVA: 0x0001BA11 File Offset: 0x00019E11
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010A6 RID: 4262
			// (get) Token: 0x0600716E RID: 29038 RVA: 0x0001BA19 File Offset: 0x00019E19
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600716F RID: 29039 RVA: 0x0001BA21 File Offset: 0x00019E21
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007170 RID: 29040 RVA: 0x0001BA31 File Offset: 0x00019E31
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400653D RID: 25917
			internal float delay;

			// Token: 0x0400653E RID: 25918
			internal CubemenGame $this;

			// Token: 0x0400653F RID: 25919
			internal object $current;

			// Token: 0x04006540 RID: 25920
			internal bool $disposing;

			// Token: 0x04006541 RID: 25921
			internal int $PC;
		}
	}
}
