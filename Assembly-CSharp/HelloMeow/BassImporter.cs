using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Un4seen.Bass;
using UnityEngine;

namespace HelloMeow
{
	// Token: 0x020002FC RID: 764
	[AddComponentMenu("Audio/Bass Importer")]
	public class BassImporter : DecoderImporter
	{
		// Token: 0x0600120D RID: 4621 RVA: 0x00063AFC File Offset: 0x00061EFC
		public BassImporter()
		{
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00063B1C File Offset: 0x00061F1C
		private void Awake()
		{
			BassNet.Registration("meshedvr@gmail.com", "2X22233718152222");
			Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
			BASSError basserror = Bass.BASS_ErrorGetCode();
			if (basserror != BASSError.BASS_OK && basserror != BASSError.BASS_ERROR_ALREADY)
			{
				base.OnError(basserror.ToString());
			}
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00063B71 File Offset: 0x00061F71
		private void OnApplicationQuit()
		{
			Bass.BASS_Free();
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00063B7C File Offset: 0x00061F7C
		protected override DecoderImporter.AudioInfo GetInfo()
		{
			BASS_CHANNELINFO bass_CHANNELINFO = Bass.BASS_ChannelGetInfo(this.handle);
			int lengthSamples = (int)Bass.BASS_ChannelGetLength(this.handle) / 4;
			return new DecoderImporter.AudioInfo(lengthSamples, bass_CHANNELINFO.freq, bass_CHANNELINFO.chans);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00063BB8 File Offset: 0x00061FB8
		protected override float GetProgress()
		{
			float num = (float)Bass.BASS_ChannelGetPosition(this.handle, BASSMode.BASS_POS_BYTE) / 4f;
			return num / (float)base.info.lengthSamples;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00063BE8 File Offset: 0x00061FE8
		protected override int GetSamples(float[] buffer, int offset, int count)
		{
			if (offset == 0)
			{
				return Bass.BASS_ChannelGetData(this.handle, buffer, count * 4) / 4;
			}
			if (this.offsetBuffer.Length != count)
			{
				this.offsetBuffer = new float[count];
			}
			int num = Bass.BASS_ChannelGetData(this.handle, this.offsetBuffer, Mathf.Min(count, buffer.Length - offset) * 4);
			if (num != -1 && num != 0)
			{
				num /= 4;
				Array.Copy(this.offsetBuffer, 0, buffer, offset, num);
			}
			return num;
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00063C68 File Offset: 0x00062068
		protected override IEnumerator Initialize(string uri)
		{
			BassImporter.<Initialize>c__Iterator0.<Initialize>c__AnonStorey3 <Initialize>c__AnonStorey = new BassImporter.<Initialize>c__Iterator0.<Initialize>c__AnonStorey3();
			<Initialize>c__AnonStorey.<>f__ref$0 = this;
			<Initialize>c__AnonStorey.uri = uri;
			this.Cleanup();
			Thread loadThread = new Thread(new ThreadStart(<Initialize>c__AnonStorey.<>m__0));
			loadThread.Start();
			while (loadThread.IsAlive)
			{
				yield return null;
			}
			if (this.handle == 0)
			{
				base.OnError("Could not open: " + <Initialize>c__AnonStorey.uri);
			}
			yield break;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00063C8A File Offset: 0x0006208A
		protected override void Cleanup()
		{
			if (this.handle != -1)
			{
				Bass.BASS_StreamFree(this.handle);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00063CA4 File Offset: 0x000620A4
		private void LoadChannel(string uri)
		{
			if (uri.StartsWith("file://"))
			{
				this.handle = Bass.BASS_StreamCreateFile(uri.Substring(7), 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
			}
			else
			{
				this.handle = Bass.BASS_StreamCreateURL(uri, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE, null, 0);
			}
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00063D00 File Offset: 0x00062100
		public IEnumerator SetData(IntPtr byteArray, long length)
		{
			this.handle = Bass.BASS_StreamCreateFile(byteArray, 0L, length, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
			if (this.handle == 0)
			{
				base.OnError("Could not decode mp3 bytes");
			}
			else
			{
				base.info = this.GetInfo();
				yield return base.StartCoroutine(base.LoadAudioClip(base.info.lengthSamples));
				Marshal.FreeHGlobal(byteArray);
				this.Cleanup();
			}
			yield break;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00063D2C File Offset: 0x0006212C
		public IEnumerator SetData(byte[] bytes)
		{
			IntPtr byteArray = Marshal.AllocHGlobal(bytes.Length);
			Marshal.Copy(bytes, 0, byteArray, bytes.Length);
			this.handle = Bass.BASS_StreamCreateFile(byteArray, 0L, (long)bytes.Length, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
			if (this.handle == 0)
			{
				base.OnError("Could not decode mp3 bytes");
			}
			else
			{
				base.info = this.GetInfo();
				yield return base.StartCoroutine(base.LoadAudioClip(base.info.lengthSamples));
				Marshal.FreeHGlobal(byteArray);
				this.Cleanup();
			}
			yield break;
		}

		// Token: 0x04000F6F RID: 3951
		private int handle = -1;

		// Token: 0x04000F70 RID: 3952
		private float[] offsetBuffer = new float[4096];

		// Token: 0x04000F71 RID: 3953
		protected IntPtr byteArray;

		// Token: 0x02000EF2 RID: 3826
		[CompilerGenerated]
		private sealed class <Initialize>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007259 RID: 29273 RVA: 0x00063D4E File Offset: 0x0006214E
			[DebuggerHidden]
			public <Initialize>c__Iterator0()
			{
			}

			// Token: 0x0600725A RID: 29274 RVA: 0x00063D58 File Offset: 0x00062158
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					<Initialize>c__AnonStorey = new BassImporter.<Initialize>c__Iterator0.<Initialize>c__AnonStorey3();
					<Initialize>c__AnonStorey.<>f__ref$0 = this;
					<Initialize>c__AnonStorey.uri = uri;
					this.Cleanup();
					loadThread = new Thread(new ThreadStart(<Initialize>c__AnonStorey.<>m__0));
					loadThread.Start();
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (loadThread.IsAlive)
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				if (this.handle == 0)
				{
					base.OnError("Could not open: " + <Initialize>c__AnonStorey.uri);
				}
				return false;
			}

			// Token: 0x170010BF RID: 4287
			// (get) Token: 0x0600725B RID: 29275 RVA: 0x00063E4F File Offset: 0x0006224F
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010C0 RID: 4288
			// (get) Token: 0x0600725C RID: 29276 RVA: 0x00063E57 File Offset: 0x00062257
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600725D RID: 29277 RVA: 0x00063E5F File Offset: 0x0006225F
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600725E RID: 29278 RVA: 0x00063E6F File Offset: 0x0006226F
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400663D RID: 26173
			internal string uri;

			// Token: 0x0400663E RID: 26174
			internal Thread <loadThread>__0;

			// Token: 0x0400663F RID: 26175
			internal BassImporter $this;

			// Token: 0x04006640 RID: 26176
			internal object $current;

			// Token: 0x04006641 RID: 26177
			internal bool $disposing;

			// Token: 0x04006642 RID: 26178
			internal int $PC;

			// Token: 0x04006643 RID: 26179
			private BassImporter.<Initialize>c__Iterator0.<Initialize>c__AnonStorey3 $locvar0;

			// Token: 0x02000EF5 RID: 3829
			private sealed class <Initialize>c__AnonStorey3
			{
				// Token: 0x0600726B RID: 29291 RVA: 0x00063E76 File Offset: 0x00062276
				public <Initialize>c__AnonStorey3()
				{
				}

				// Token: 0x0600726C RID: 29292 RVA: 0x00063E7E File Offset: 0x0006227E
				internal void <>m__0()
				{
					this.<>f__ref$0.$this.LoadChannel(this.uri);
				}

				// Token: 0x04006650 RID: 26192
				internal string uri;

				// Token: 0x04006651 RID: 26193
				internal BassImporter.<Initialize>c__Iterator0 <>f__ref$0;
			}
		}

		// Token: 0x02000EF3 RID: 3827
		[CompilerGenerated]
		private sealed class <SetData>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600725F RID: 29279 RVA: 0x00063E96 File Offset: 0x00062296
			[DebuggerHidden]
			public <SetData>c__Iterator1()
			{
			}

			// Token: 0x06007260 RID: 29280 RVA: 0x00063EA0 File Offset: 0x000622A0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.handle = Bass.BASS_StreamCreateFile(byteArray, 0L, length, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
					if (this.handle != 0)
					{
						base.info = this.GetInfo();
						this.$current = base.StartCoroutine(base.LoadAudioClip(base.info.lengthSamples));
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					base.OnError("Could not decode mp3 bytes");
					break;
				case 1U:
					Marshal.FreeHGlobal(byteArray);
					this.Cleanup();
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010C1 RID: 4289
			// (get) Token: 0x06007261 RID: 29281 RVA: 0x00063F8C File Offset: 0x0006238C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010C2 RID: 4290
			// (get) Token: 0x06007262 RID: 29282 RVA: 0x00063F94 File Offset: 0x00062394
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007263 RID: 29283 RVA: 0x00063F9C File Offset: 0x0006239C
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007264 RID: 29284 RVA: 0x00063FAC File Offset: 0x000623AC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006644 RID: 26180
			internal IntPtr byteArray;

			// Token: 0x04006645 RID: 26181
			internal long length;

			// Token: 0x04006646 RID: 26182
			internal BassImporter $this;

			// Token: 0x04006647 RID: 26183
			internal object $current;

			// Token: 0x04006648 RID: 26184
			internal bool $disposing;

			// Token: 0x04006649 RID: 26185
			internal int $PC;
		}

		// Token: 0x02000EF4 RID: 3828
		[CompilerGenerated]
		private sealed class <SetData>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007265 RID: 29285 RVA: 0x00063FB3 File Offset: 0x000623B3
			[DebuggerHidden]
			public <SetData>c__Iterator2()
			{
			}

			// Token: 0x06007266 RID: 29286 RVA: 0x00063FBC File Offset: 0x000623BC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					byteArray = Marshal.AllocHGlobal(bytes.Length);
					Marshal.Copy(bytes, 0, byteArray, bytes.Length);
					this.handle = Bass.BASS_StreamCreateFile(byteArray, 0L, (long)bytes.Length, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_DECODE);
					if (this.handle != 0)
					{
						base.info = this.GetInfo();
						this.$current = base.StartCoroutine(base.LoadAudioClip(base.info.lengthSamples));
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					base.OnError("Could not decode mp3 bytes");
					break;
				case 1U:
					Marshal.FreeHGlobal(byteArray);
					this.Cleanup();
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010C3 RID: 4291
			// (get) Token: 0x06007267 RID: 29287 RVA: 0x000640D8 File Offset: 0x000624D8
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010C4 RID: 4292
			// (get) Token: 0x06007268 RID: 29288 RVA: 0x000640E0 File Offset: 0x000624E0
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007269 RID: 29289 RVA: 0x000640E8 File Offset: 0x000624E8
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600726A RID: 29290 RVA: 0x000640F8 File Offset: 0x000624F8
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400664A RID: 26186
			internal byte[] bytes;

			// Token: 0x0400664B RID: 26187
			internal IntPtr <byteArray>__0;

			// Token: 0x0400664C RID: 26188
			internal BassImporter $this;

			// Token: 0x0400664D RID: 26189
			internal object $current;

			// Token: 0x0400664E RID: 26190
			internal bool $disposing;

			// Token: 0x0400664F RID: 26191
			internal int $PC;
		}
	}
}
