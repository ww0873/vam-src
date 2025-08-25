using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000724 RID: 1828
	[RequireComponent(typeof(SpriteRenderer))]
	public class ConnectionMonitor : MonoBehaviour
	{
		// Token: 0x06002C99 RID: 11417 RVA: 0x000EF3E8 File Offset: 0x000ED7E8
		public ConnectionMonitor()
		{
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000EF44D File Offset: 0x000ED84D
		private void Start()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			this.SetAlpha(0f);
			base.StartCoroutine(this.Monitor());
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000EF473 File Offset: 0x000ED873
		private void SetAlpha(float alpha)
		{
			this.spriteRenderer.color = Color.Lerp(Color.clear, this.onColor, alpha);
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000EF494 File Offset: 0x000ED894
		private void Update()
		{
			if (this.fadedIn > 0f)
			{
				Camera main = Camera.main;
				Vector3 position = main.transform.position + main.transform.forward * this.distanceToCamera;
				base.transform.position = position;
				base.transform.LookAt(main.transform);
			}
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000EF4FC File Offset: 0x000ED8FC
		private IEnumerator Monitor()
		{
			yield return new WaitForSecondsRealtime((float)this.monitorInterval);
			for (;;)
			{
				this.connected = this.provider.IsConnected();
				if (this.connected)
				{
					while ((double)this.fadedIn > 0.0)
					{
						this.fadedIn -= Time.deltaTime / this.fadeOutTime;
						this.fadedIn = Mathf.Clamp(this.fadedIn, 0f, 1f);
						this.SetAlpha(this.fadeCurve.Evaluate(this.fadedIn));
						yield return null;
					}
				}
				else
				{
					while ((double)this.fadedIn < 1.0)
					{
						this.fadedIn += Time.deltaTime / this.fadeOutTime;
						this.fadedIn = Mathf.Clamp(this.fadedIn, 0f, 1f);
						this.SetAlpha(this.fadeCurve.Evaluate(this.fadedIn));
						yield return null;
					}
				}
				yield return new WaitForSecondsRealtime((float)this.monitorInterval);
			}
			yield break;
		}

		// Token: 0x0400238A RID: 9098
		[Tooltip("The scene LeapServiceProvider.")]
		public LeapServiceProvider provider;

		// Token: 0x0400238B RID: 9099
		[Tooltip("How fast to make the connection notice sprite visible.")]
		[Range(0.1f, 10f)]
		public float fadeInTime = 1f;

		// Token: 0x0400238C RID: 9100
		[Tooltip("How fast to fade out the connection notice sprite.")]
		[Range(0.1f, 10f)]
		public float fadeOutTime = 1f;

		// Token: 0x0400238D RID: 9101
		[Tooltip("The easing curve for the fade in and out effect.")]
		public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x0400238E RID: 9102
		[Tooltip("How frequently to check the connection.")]
		public int monitorInterval = 2;

		// Token: 0x0400238F RID: 9103
		[Tooltip("A tint applied to the connection notice sprite when on.")]
		public Color onColor = Color.white;

		// Token: 0x04002390 RID: 9104
		[Tooltip("How far to place the sprite in front of the camera.")]
		public float distanceToCamera = 12f;

		// Token: 0x04002391 RID: 9105
		private float fadedIn;

		// Token: 0x04002392 RID: 9106
		private SpriteRenderer spriteRenderer;

		// Token: 0x04002393 RID: 9107
		private bool connected;

		// Token: 0x02000FAD RID: 4013
		[CompilerGenerated]
		private sealed class <Monitor>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060074C4 RID: 29892 RVA: 0x000EF517 File Offset: 0x000ED917
			[DebuggerHidden]
			public <Monitor>c__Iterator0()
			{
			}

			// Token: 0x060074C5 RID: 29893 RVA: 0x000EF520 File Offset: 0x000ED920
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForSecondsRealtime((float)this.monitorInterval);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					break;
				case 2U:
					goto IL_111;
				case 3U:
					goto IL_1BE;
				case 4U:
					break;
				default:
					return false;
				}
				this.connected = this.provider.IsConnected();
				if (!this.connected)
				{
					goto IL_1BE;
				}
				IL_111:
				if ((double)this.fadedIn <= 0.0)
				{
					goto IL_1D8;
				}
				this.fadedIn -= Time.deltaTime / this.fadeOutTime;
				this.fadedIn = Mathf.Clamp(this.fadedIn, 0f, 1f);
				base.SetAlpha(this.fadeCurve.Evaluate(this.fadedIn));
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
				IL_1BE:
				if ((double)this.fadedIn < 1.0)
				{
					this.fadedIn += Time.deltaTime / this.fadeOutTime;
					this.fadedIn = Mathf.Clamp(this.fadedIn, 0f, 1f);
					base.SetAlpha(this.fadeCurve.Evaluate(this.fadedIn));
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				IL_1D8:
				this.$current = new WaitForSecondsRealtime((float)this.monitorInterval);
				if (!this.$disposing)
				{
					this.$PC = 4;
				}
				return true;
			}

			// Token: 0x17001137 RID: 4407
			// (get) Token: 0x060074C6 RID: 29894 RVA: 0x000EF73F File Offset: 0x000EDB3F
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001138 RID: 4408
			// (get) Token: 0x060074C7 RID: 29895 RVA: 0x000EF747 File Offset: 0x000EDB47
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060074C8 RID: 29896 RVA: 0x000EF74F File Offset: 0x000EDB4F
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060074C9 RID: 29897 RVA: 0x000EF75F File Offset: 0x000EDB5F
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068D8 RID: 26840
			internal ConnectionMonitor $this;

			// Token: 0x040068D9 RID: 26841
			internal object $current;

			// Token: 0x040068DA RID: 26842
			internal bool $disposing;

			// Token: 0x040068DB RID: 26843
			internal int $PC;
		}
	}
}
