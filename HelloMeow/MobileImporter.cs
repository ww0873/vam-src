using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace HelloMeow
{
	// Token: 0x020002FF RID: 767
	[AddComponentMenu("Audio/MobileImporter")]
	public class MobileImporter : AudioImporter
	{
		// Token: 0x0600122B RID: 4651 RVA: 0x000640FF File Offset: 0x000624FF
		public MobileImporter()
		{
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x00064108 File Offset: 0x00062508
		protected override IEnumerator Load(string uri)
		{
			using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(uri.ToString(), AudioType.MPEG))
			{
				AsyncOperation operation = request.Send();
				while (!operation.isDone)
				{
					yield return null;
					base.OnProgress(request.downloadProgress);
				}
				if (request.isNetworkError)
				{
					base.OnError(request.error);
					yield break;
				}
				AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
				if (audioClip == null)
				{
					yield break;
				}
				base.OnLoaded(audioClip);
			}
			yield break;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x0006412C File Offset: 0x0006252C
		protected override IEnumerator LoadStreaming(string uri, int initialLength)
		{
			UnityEngine.Debug.LogWarning("MobileImporter does not support streaming.");
			yield return base.StartCoroutine(this.Load(uri));
			yield break;
		}

		// Token: 0x02000EFA RID: 3834
		[CompilerGenerated]
		private sealed class <Load>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007281 RID: 29313 RVA: 0x0006414E File Offset: 0x0006254E
			[DebuggerHidden]
			public <Load>c__Iterator0()
			{
			}

			// Token: 0x06007282 RID: 29314 RVA: 0x00064158 File Offset: 0x00062558
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0U:
					request = UnityWebRequestMultimedia.GetAudioClip(uri.ToString(), AudioType.MPEG);
					num = 4294967293U;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1U:
						base.OnProgress(request.downloadProgress);
						break;
					default:
						operation = request.Send();
						break;
					}
					if (!operation.isDone)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
					if (request.isNetworkError)
					{
						base.OnError(request.error);
						return false;
					}
					audioClip = DownloadHandlerAudioClip.GetContent(request);
					if (audioClip == null)
					{
						return false;
					}
					base.OnLoaded(audioClip);
				}
				finally
				{
					if (!flag)
					{
						this.<>__Finally0();
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010CB RID: 4299
			// (get) Token: 0x06007283 RID: 29315 RVA: 0x00064298 File Offset: 0x00062698
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010CC RID: 4300
			// (get) Token: 0x06007284 RID: 29316 RVA: 0x000642A0 File Offset: 0x000626A0
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007285 RID: 29317 RVA: 0x000642A8 File Offset: 0x000626A8
			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1U:
					try
					{
					}
					finally
					{
						this.<>__Finally0();
					}
					break;
				}
			}

			// Token: 0x06007286 RID: 29318 RVA: 0x000642FC File Offset: 0x000626FC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007287 RID: 29319 RVA: 0x00064303 File Offset: 0x00062703
			private void <>__Finally0()
			{
				if (request != null)
				{
					((IDisposable)request).Dispose();
				}
			}

			// Token: 0x0400666C RID: 26220
			internal string uri;

			// Token: 0x0400666D RID: 26221
			internal UnityWebRequest <request>__1;

			// Token: 0x0400666E RID: 26222
			internal AsyncOperation <operation>__2;

			// Token: 0x0400666F RID: 26223
			internal AudioClip <audioClip>__2;

			// Token: 0x04006670 RID: 26224
			internal MobileImporter $this;

			// Token: 0x04006671 RID: 26225
			internal object $current;

			// Token: 0x04006672 RID: 26226
			internal bool $disposing;

			// Token: 0x04006673 RID: 26227
			internal int $PC;
		}

		// Token: 0x02000EFB RID: 3835
		[CompilerGenerated]
		private sealed class <LoadStreaming>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007288 RID: 29320 RVA: 0x0006431B File Offset: 0x0006271B
			[DebuggerHidden]
			public <LoadStreaming>c__Iterator1()
			{
			}

			// Token: 0x06007289 RID: 29321 RVA: 0x00064324 File Offset: 0x00062724
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					UnityEngine.Debug.LogWarning("MobileImporter does not support streaming.");
					this.$current = base.StartCoroutine(this.Load(uri));
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010CD RID: 4301
			// (get) Token: 0x0600728A RID: 29322 RVA: 0x0006439C File Offset: 0x0006279C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010CE RID: 4302
			// (get) Token: 0x0600728B RID: 29323 RVA: 0x000643A4 File Offset: 0x000627A4
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600728C RID: 29324 RVA: 0x000643AC File Offset: 0x000627AC
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600728D RID: 29325 RVA: 0x000643BC File Offset: 0x000627BC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006674 RID: 26228
			internal string uri;

			// Token: 0x04006675 RID: 26229
			internal MobileImporter $this;

			// Token: 0x04006676 RID: 26230
			internal object $current;

			// Token: 0x04006677 RID: 26231
			internal bool $disposing;

			// Token: 0x04006678 RID: 26232
			internal int $PC;
		}
	}
}
