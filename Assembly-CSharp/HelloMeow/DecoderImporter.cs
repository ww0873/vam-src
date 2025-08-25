using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace HelloMeow
{
	// Token: 0x020002FD RID: 765
	public abstract class DecoderImporter : AudioImporter
	{
		// Token: 0x06001218 RID: 4632 RVA: 0x00063461 File Offset: 0x00061861
		protected DecoderImporter()
		{
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00063469 File Offset: 0x00061869
		// (set) Token: 0x0600121A RID: 4634 RVA: 0x00063471 File Offset: 0x00061871
		protected DecoderImporter.AudioInfo info
		{
			[CompilerGenerated]
			get
			{
				return this.<info>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<info>k__BackingField = value;
			}
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0006347C File Offset: 0x0006187C
		private void FillBuffer(float[] buffer)
		{
			int samples;
			for (int i = 0; i < buffer.Length; i += samples)
			{
				samples = this.GetSamples(buffer, i, Mathf.Min(buffer.Length - i, 4096));
				if (samples == -1 || samples == 0)
				{
					break;
				}
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x000634C8 File Offset: 0x000618C8
		protected IEnumerator LoadAudioClip(int samples)
		{
			DecoderImporter.<LoadAudioClip>c__Iterator0.<LoadAudioClip>c__AnonStorey3 <LoadAudioClip>c__AnonStorey = new DecoderImporter.<LoadAudioClip>c__Iterator0.<LoadAudioClip>c__AnonStorey3();
			<LoadAudioClip>c__AnonStorey.<>f__ref$0 = this;
			samples = Mathf.Clamp(samples, 0, this.info.lengthSamples);
			<LoadAudioClip>c__AnonStorey.buffer = new float[samples];
			Thread bufferThread = new Thread(new ThreadStart(<LoadAudioClip>c__AnonStorey.<>m__0));
			bufferThread.Start();
			while (bufferThread.IsAlive)
			{
				base.OnProgress(this.GetProgress());
				yield return null;
			}
			AudioClip audioClip = AudioClip.Create(string.Empty, this.info.lengthSamples / this.info.channels, this.info.channels, this.info.sampleRate, false);
			audioClip.SetData(<LoadAudioClip>c__AnonStorey.buffer, 0);
			base.OnLoaded(audioClip);
			yield break;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x000634EC File Offset: 0x000618EC
		protected override IEnumerator Load(string uri)
		{
			yield return base.StartCoroutine(this.Initialize(uri));
			if (base.isError)
			{
				yield break;
			}
			this.info = this.GetInfo();
			yield return base.StartCoroutine(this.LoadAudioClip(this.info.lengthSamples));
			base.OnProgress(1f);
			this.Cleanup();
			yield break;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00063510 File Offset: 0x00061910
		protected override IEnumerator LoadStreaming(string uri, int initialLength)
		{
			yield return base.StartCoroutine(this.Initialize(uri));
			if (base.isError)
			{
				yield break;
			}
			this.info = this.GetInfo();
			int loadedIndex = initialLength * this.info.sampleRate * this.info.channels;
			loadedIndex = Mathf.Clamp(loadedIndex, 44100, this.info.lengthSamples);
			yield return base.StartCoroutine(this.LoadAudioClip(loadedIndex));
			int bufferSize = this.info.sampleRate * this.info.channels / 10;
			float[] buffer = new float[bufferSize];
			int index = loadedIndex;
			while (index < this.info.lengthSamples)
			{
				int read = this.GetSamples(buffer, 0, bufferSize);
				if (read == -1 || read == 0)
				{
					break;
				}
				base.audioClip.SetData(buffer, index / this.info.channels);
				index += read;
				base.OnProgress(this.GetProgress());
				yield return null;
			}
			base.OnProgress(1f);
			this.Cleanup();
			yield break;
		}

		// Token: 0x0600121F RID: 4639
		protected abstract IEnumerator Initialize(string uri);

		// Token: 0x06001220 RID: 4640
		protected abstract int GetSamples(float[] buffer, int offset, int count);

		// Token: 0x06001221 RID: 4641
		protected abstract float GetProgress();

		// Token: 0x06001222 RID: 4642 RVA: 0x00063539 File Offset: 0x00061939
		protected virtual void Cleanup()
		{
		}

		// Token: 0x06001223 RID: 4643
		protected abstract DecoderImporter.AudioInfo GetInfo();

		// Token: 0x04000F72 RID: 3954
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DecoderImporter.AudioInfo <info>k__BackingField;

		// Token: 0x020002FE RID: 766
		protected class AudioInfo
		{
			// Token: 0x06001224 RID: 4644 RVA: 0x0006353B File Offset: 0x0006193B
			public AudioInfo(int lengthSamples, int sampleRate, int channels)
			{
				this.lengthSamples = lengthSamples;
				this.sampleRate = sampleRate;
				this.channels = channels;
			}

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06001225 RID: 4645 RVA: 0x00063558 File Offset: 0x00061958
			// (set) Token: 0x06001226 RID: 4646 RVA: 0x00063560 File Offset: 0x00061960
			public int lengthSamples
			{
				[CompilerGenerated]
				get
				{
					return this.<lengthSamples>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<lengthSamples>k__BackingField = value;
				}
			}

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x06001227 RID: 4647 RVA: 0x00063569 File Offset: 0x00061969
			// (set) Token: 0x06001228 RID: 4648 RVA: 0x00063571 File Offset: 0x00061971
			public int sampleRate
			{
				[CompilerGenerated]
				get
				{
					return this.<sampleRate>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<sampleRate>k__BackingField = value;
				}
			}

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x06001229 RID: 4649 RVA: 0x0006357A File Offset: 0x0006197A
			// (set) Token: 0x0600122A RID: 4650 RVA: 0x00063582 File Offset: 0x00061982
			public int channels
			{
				[CompilerGenerated]
				get
				{
					return this.<channels>k__BackingField;
				}
				[CompilerGenerated]
				private set
				{
					this.<channels>k__BackingField = value;
				}
			}

			// Token: 0x04000F73 RID: 3955
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private int <lengthSamples>k__BackingField;

			// Token: 0x04000F74 RID: 3956
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private int <sampleRate>k__BackingField;

			// Token: 0x04000F75 RID: 3957
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private int <channels>k__BackingField;
		}

		// Token: 0x02000EF6 RID: 3830
		[CompilerGenerated]
		private sealed class <LoadAudioClip>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600726D RID: 29293 RVA: 0x0006358B File Offset: 0x0006198B
			[DebuggerHidden]
			public <LoadAudioClip>c__Iterator0()
			{
			}

			// Token: 0x0600726E RID: 29294 RVA: 0x00063594 File Offset: 0x00061994
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					<LoadAudioClip>c__AnonStorey = new DecoderImporter.<LoadAudioClip>c__Iterator0.<LoadAudioClip>c__AnonStorey3();
					<LoadAudioClip>c__AnonStorey.<>f__ref$0 = this;
					samples = Mathf.Clamp(samples, 0, base.info.lengthSamples);
					<LoadAudioClip>c__AnonStorey.buffer = new float[samples];
					bufferThread = new Thread(new ThreadStart(<LoadAudioClip>c__AnonStorey.<>m__0));
					bufferThread.Start();
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (bufferThread.IsAlive)
				{
					base.OnProgress(this.GetProgress());
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				audioClip = AudioClip.Create(string.Empty, base.info.lengthSamples / base.info.channels, base.info.channels, base.info.sampleRate, false);
				audioClip.SetData(<LoadAudioClip>c__AnonStorey.buffer, 0);
				base.OnLoaded(audioClip);
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010C5 RID: 4293
			// (get) Token: 0x0600726F RID: 29295 RVA: 0x00063703 File Offset: 0x00061B03
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010C6 RID: 4294
			// (get) Token: 0x06007270 RID: 29296 RVA: 0x0006370B File Offset: 0x00061B0B
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007271 RID: 29297 RVA: 0x00063713 File Offset: 0x00061B13
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007272 RID: 29298 RVA: 0x00063723 File Offset: 0x00061B23
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006652 RID: 26194
			internal int samples;

			// Token: 0x04006653 RID: 26195
			internal Thread <bufferThread>__0;

			// Token: 0x04006654 RID: 26196
			internal AudioClip <audioClip>__0;

			// Token: 0x04006655 RID: 26197
			internal DecoderImporter $this;

			// Token: 0x04006656 RID: 26198
			internal object $current;

			// Token: 0x04006657 RID: 26199
			internal bool $disposing;

			// Token: 0x04006658 RID: 26200
			internal int $PC;

			// Token: 0x04006659 RID: 26201
			private DecoderImporter.<LoadAudioClip>c__Iterator0.<LoadAudioClip>c__AnonStorey3 $locvar0;

			// Token: 0x02000EF9 RID: 3833
			private sealed class <LoadAudioClip>c__AnonStorey3
			{
				// Token: 0x0600727F RID: 29311 RVA: 0x0006372A File Offset: 0x00061B2A
				public <LoadAudioClip>c__AnonStorey3()
				{
				}

				// Token: 0x06007280 RID: 29312 RVA: 0x00063732 File Offset: 0x00061B32
				internal void <>m__0()
				{
					this.<>f__ref$0.$this.FillBuffer(this.buffer);
				}

				// Token: 0x0400666A RID: 26218
				internal float[] buffer;

				// Token: 0x0400666B RID: 26219
				internal DecoderImporter.<LoadAudioClip>c__Iterator0 <>f__ref$0;
			}
		}

		// Token: 0x02000EF7 RID: 3831
		[CompilerGenerated]
		private sealed class <Load>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007273 RID: 29299 RVA: 0x0006374A File Offset: 0x00061B4A
			[DebuggerHidden]
			public <Load>c__Iterator1()
			{
			}

			// Token: 0x06007274 RID: 29300 RVA: 0x00063754 File Offset: 0x00061B54
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = base.StartCoroutine(this.Initialize(uri));
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (!base.isError)
					{
						base.info = this.GetInfo();
						this.$current = base.StartCoroutine(base.LoadAudioClip(base.info.lengthSamples));
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2U:
					base.OnProgress(1f);
					this.Cleanup();
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010C7 RID: 4295
			// (get) Token: 0x06007275 RID: 29301 RVA: 0x0006384C File Offset: 0x00061C4C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010C8 RID: 4296
			// (get) Token: 0x06007276 RID: 29302 RVA: 0x00063854 File Offset: 0x00061C54
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007277 RID: 29303 RVA: 0x0006385C File Offset: 0x00061C5C
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007278 RID: 29304 RVA: 0x0006386C File Offset: 0x00061C6C
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400665A RID: 26202
			internal string uri;

			// Token: 0x0400665B RID: 26203
			internal DecoderImporter $this;

			// Token: 0x0400665C RID: 26204
			internal object $current;

			// Token: 0x0400665D RID: 26205
			internal bool $disposing;

			// Token: 0x0400665E RID: 26206
			internal int $PC;
		}

		// Token: 0x02000EF8 RID: 3832
		[CompilerGenerated]
		private sealed class <LoadStreaming>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007279 RID: 29305 RVA: 0x00063873 File Offset: 0x00061C73
			[DebuggerHidden]
			public <LoadStreaming>c__Iterator2()
			{
			}

			// Token: 0x0600727A RID: 29306 RVA: 0x0006387C File Offset: 0x00061C7C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = base.StartCoroutine(this.Initialize(uri));
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					if (base.isError)
					{
						return false;
					}
					base.info = this.GetInfo();
					loadedIndex = initialLength * base.info.sampleRate * base.info.channels;
					loadedIndex = Mathf.Clamp(loadedIndex, 44100, base.info.lengthSamples);
					this.$current = base.StartCoroutine(base.LoadAudioClip(loadedIndex));
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2U:
					bufferSize = base.info.sampleRate * base.info.channels / 10;
					buffer = new float[bufferSize];
					index = loadedIndex;
					break;
				case 3U:
					break;
				default:
					return false;
				}
				if (index < base.info.lengthSamples)
				{
					read = this.GetSamples(buffer, 0, bufferSize);
					if (read != -1 && read != 0)
					{
						base.audioClip.SetData(buffer, index / base.info.channels);
						index += read;
						base.OnProgress(this.GetProgress());
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
				}
				base.OnProgress(1f);
				this.Cleanup();
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010C9 RID: 4297
			// (get) Token: 0x0600727B RID: 29307 RVA: 0x00063AD5 File Offset: 0x00061ED5
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010CA RID: 4298
			// (get) Token: 0x0600727C RID: 29308 RVA: 0x00063ADD File Offset: 0x00061EDD
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600727D RID: 29309 RVA: 0x00063AE5 File Offset: 0x00061EE5
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600727E RID: 29310 RVA: 0x00063AF5 File Offset: 0x00061EF5
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400665F RID: 26207
			internal string uri;

			// Token: 0x04006660 RID: 26208
			internal int initialLength;

			// Token: 0x04006661 RID: 26209
			internal int <loadedIndex>__0;

			// Token: 0x04006662 RID: 26210
			internal int <bufferSize>__0;

			// Token: 0x04006663 RID: 26211
			internal float[] <buffer>__0;

			// Token: 0x04006664 RID: 26212
			internal int <index>__0;

			// Token: 0x04006665 RID: 26213
			internal int <read>__1;

			// Token: 0x04006666 RID: 26214
			internal DecoderImporter $this;

			// Token: 0x04006667 RID: 26215
			internal object $current;

			// Token: 0x04006668 RID: 26216
			internal bool $disposing;

			// Token: 0x04006669 RID: 26217
			internal int $PC;
		}
	}
}
