using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace HelloMeow
{
	// Token: 0x020002FA RID: 762
	public abstract class AudioImporter : MonoBehaviour
	{
		// Token: 0x060011EB RID: 4587 RVA: 0x00062FA6 File Offset: 0x000613A6
		protected AudioImporter()
		{
			this.operation = new ImportOperation(this);
		}

		// Token: 0x14000079 RID: 121
		// (add) Token: 0x060011EC RID: 4588 RVA: 0x00062FBC File Offset: 0x000613BC
		// (remove) Token: 0x060011ED RID: 4589 RVA: 0x00062FF4 File Offset: 0x000613F4
		public event Action<AudioClip> Loaded
		{
			add
			{
				Action<AudioClip> action = this.Loaded;
				Action<AudioClip> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<AudioClip>>(ref this.Loaded, (Action<AudioClip>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<AudioClip> action = this.Loaded;
				Action<AudioClip> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<AudioClip>>(ref this.Loaded, (Action<AudioClip>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x060011EE RID: 4590 RVA: 0x0006302C File Offset: 0x0006142C
		// (remove) Token: 0x060011EF RID: 4591 RVA: 0x00063064 File Offset: 0x00061464
		public event Action<float> Progress
		{
			add
			{
				Action<float> action = this.Progress;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<float>>(ref this.Progress, (Action<float>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<float> action = this.Progress;
				Action<float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<float>>(ref this.Progress, (Action<float>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x060011F0 RID: 4592 RVA: 0x0006309C File Offset: 0x0006149C
		// (remove) Token: 0x060011F1 RID: 4593 RVA: 0x000630D4 File Offset: 0x000614D4
		public event Action<string> Error
		{
			add
			{
				Action<string> action = this.Error;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<string>>(ref this.Error, (Action<string>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<string> action = this.Error;
				Action<string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<string>>(ref this.Error, (Action<string>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0006310A File Offset: 0x0006150A
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x00063112 File Offset: 0x00061512
		public string uri
		{
			[CompilerGenerated]
			get
			{
				return this.<uri>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<uri>k__BackingField = value;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x0006311B File Offset: 0x0006151B
		// (set) Token: 0x060011F5 RID: 4597 RVA: 0x00063123 File Offset: 0x00061523
		public AudioClip audioClip
		{
			[CompilerGenerated]
			get
			{
				return this.<audioClip>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<audioClip>k__BackingField = value;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x0006312C File Offset: 0x0006152C
		// (set) Token: 0x060011F7 RID: 4599 RVA: 0x00063134 File Offset: 0x00061534
		public float progress
		{
			[CompilerGenerated]
			get
			{
				return this.<progress>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<progress>k__BackingField = value;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0006313D File Offset: 0x0006153D
		// (set) Token: 0x060011F9 RID: 4601 RVA: 0x00063145 File Offset: 0x00061545
		public bool isLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<isLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<isLoaded>k__BackingField = value;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x0006314E File Offset: 0x0006154E
		// (set) Token: 0x060011FB RID: 4603 RVA: 0x00063156 File Offset: 0x00061556
		public bool isError
		{
			[CompilerGenerated]
			get
			{
				return this.<isError>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<isError>k__BackingField = value;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x0006315F File Offset: 0x0006155F
		// (set) Token: 0x060011FD RID: 4605 RVA: 0x00063167 File Offset: 0x00061567
		public string error
		{
			[CompilerGenerated]
			get
			{
				return this.<error>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<error>k__BackingField = value;
			}
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00063170 File Offset: 0x00061570
		public ImportOperation Import(string uri)
		{
			this.Cleanup();
			this.uri = this.GetUri(uri);
			base.StartCoroutine(this.Load(this.uri));
			return this.operation;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0006319E File Offset: 0x0006159E
		public ImportOperation ImportStreaming(string uri, int initialLength = 0)
		{
			this.Cleanup();
			this.uri = this.GetUri(uri);
			initialLength = Mathf.Max(1, initialLength);
			base.StartCoroutine(this.LoadStreaming(this.uri, initialLength));
			return this.operation;
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x000631D8 File Offset: 0x000615D8
		protected virtual IEnumerator Load(string uri)
		{
			yield return null;
			yield break;
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x000631EC File Offset: 0x000615EC
		protected virtual IEnumerator LoadStreaming(string uri, int initialLength)
		{
			yield return null;
			yield break;
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00063200 File Offset: 0x00061600
		protected virtual string GetName()
		{
			return Path.GetFileNameWithoutExtension(this.uri);
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0006320D File Offset: 0x0006160D
		private string GetUri(string uri)
		{
			if (uri.StartsWith("file://") || uri.StartsWith("http://") || uri.StartsWith("https://"))
			{
				return uri;
			}
			return "file://" + uri;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0006324C File Offset: 0x0006164C
		public void Prep()
		{
			this.Cleanup();
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00063254 File Offset: 0x00061654
		private void Cleanup()
		{
			base.StopAllCoroutines();
			this.uri = null;
			this.audioClip = null;
			this.isLoaded = false;
			this.isError = false;
			this.error = null;
			this.progress = 0f;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0006328A File Offset: 0x0006168A
		protected void OnLoaded(AudioClip audioClip)
		{
			audioClip.name = this.GetName();
			this.audioClip = audioClip;
			this.isLoaded = true;
			if (this.Loaded != null)
			{
				this.Loaded(audioClip);
			}
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x000632BD File Offset: 0x000616BD
		protected void OnProgress(float progress)
		{
			if (this.progress == progress)
			{
				return;
			}
			this.progress = progress;
			if (this.Progress != null)
			{
				this.Progress(progress);
			}
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x000632EA File Offset: 0x000616EA
		protected void OnError(string error)
		{
			this.isError = true;
			this.error = error;
			UnityEngine.Debug.LogError(error);
			if (this.Error != null)
			{
				this.Error(error);
			}
		}

		// Token: 0x04000F64 RID: 3940
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<AudioClip> Loaded;

		// Token: 0x04000F65 RID: 3941
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<float> Progress;

		// Token: 0x04000F66 RID: 3942
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<string> Error;

		// Token: 0x04000F67 RID: 3943
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <uri>k__BackingField;

		// Token: 0x04000F68 RID: 3944
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AudioClip <audioClip>k__BackingField;

		// Token: 0x04000F69 RID: 3945
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <progress>k__BackingField;

		// Token: 0x04000F6A RID: 3946
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <isLoaded>k__BackingField;

		// Token: 0x04000F6B RID: 3947
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <isError>k__BackingField;

		// Token: 0x04000F6C RID: 3948
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <error>k__BackingField;

		// Token: 0x04000F6D RID: 3949
		private ImportOperation operation;

		// Token: 0x02000EF0 RID: 3824
		[CompilerGenerated]
		private sealed class <Load>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600724D RID: 29261 RVA: 0x00063317 File Offset: 0x00061717
			[DebuggerHidden]
			public <Load>c__Iterator0()
			{
			}

			// Token: 0x0600724E RID: 29262 RVA: 0x00063320 File Offset: 0x00061720
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
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

			// Token: 0x170010BB RID: 4283
			// (get) Token: 0x0600724F RID: 29263 RVA: 0x00063373 File Offset: 0x00061773
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010BC RID: 4284
			// (get) Token: 0x06007250 RID: 29264 RVA: 0x0006337B File Offset: 0x0006177B
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007251 RID: 29265 RVA: 0x00063383 File Offset: 0x00061783
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007252 RID: 29266 RVA: 0x00063393 File Offset: 0x00061793
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006637 RID: 26167
			internal object $current;

			// Token: 0x04006638 RID: 26168
			internal bool $disposing;

			// Token: 0x04006639 RID: 26169
			internal int $PC;
		}

		// Token: 0x02000EF1 RID: 3825
		[CompilerGenerated]
		private sealed class <LoadStreaming>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007253 RID: 29267 RVA: 0x0006339A File Offset: 0x0006179A
			[DebuggerHidden]
			public <LoadStreaming>c__Iterator1()
			{
			}

			// Token: 0x06007254 RID: 29268 RVA: 0x000633A4 File Offset: 0x000617A4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = null;
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

			// Token: 0x170010BD RID: 4285
			// (get) Token: 0x06007255 RID: 29269 RVA: 0x000633F7 File Offset: 0x000617F7
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010BE RID: 4286
			// (get) Token: 0x06007256 RID: 29270 RVA: 0x000633FF File Offset: 0x000617FF
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007257 RID: 29271 RVA: 0x00063407 File Offset: 0x00061807
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007258 RID: 29272 RVA: 0x00063417 File Offset: 0x00061817
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400663A RID: 26170
			internal object $current;

			// Token: 0x0400663B RID: 26171
			internal bool $disposing;

			// Token: 0x0400663C RID: 26172
			internal int $PC;
		}
	}
}
