using System;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x020008AB RID: 2219
	public class VoipAudioSourceHiLevel : MonoBehaviour
	{
		// Token: 0x060037DF RID: 14303 RVA: 0x0010EC7C File Offset: 0x0010D07C
		public VoipAudioSourceHiLevel()
		{
		}

		// Token: 0x17000617 RID: 1559
		// (set) Token: 0x060037E0 RID: 14304 RVA: 0x0010EC84 File Offset: 0x0010D084
		public ulong senderID
		{
			set
			{
				this.pcmSource.SetSenderID(value);
			}
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x0010EC92 File Offset: 0x0010D092
		protected void Stop()
		{
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x0010EC94 File Offset: 0x0010D094
		private VoipSampleRate SampleRateToEnum(int rate)
		{
			if (rate == 24000)
			{
				return VoipSampleRate.HZ24000;
			}
			if (rate == 44100)
			{
				return VoipSampleRate.HZ44100;
			}
			if (rate != 48000)
			{
				return VoipSampleRate.Unknown;
			}
			return VoipSampleRate.HZ48000;
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x0010ECC4 File Offset: 0x0010D0C4
		protected void Awake()
		{
			this.CreatePCMSource();
			if (this.audioSource == null)
			{
				this.audioSource = base.gameObject.AddComponent<AudioSource>();
			}
			this.audioSource.gameObject.AddComponent<VoipAudioSourceHiLevel.FilterReadDelegate>();
			VoipAudioSourceHiLevel.FilterReadDelegate component = this.audioSource.gameObject.GetComponent<VoipAudioSourceHiLevel.FilterReadDelegate>();
			component.parent = this;
			this.initialPlaybackDelayMS = 40;
			VoipAudioSourceHiLevel.audioSystemPlaybackFrequency = AudioSettings.outputSampleRate;
			CAPI.ovr_Voip_SetOutputSampleRate(this.SampleRateToEnum(VoipAudioSourceHiLevel.audioSystemPlaybackFrequency));
			if (VoipAudioSourceHiLevel.verboseLogging)
			{
				Debug.LogFormat("freq {0}", new object[]
				{
					VoipAudioSourceHiLevel.audioSystemPlaybackFrequency
				});
			}
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x0010ED6B File Offset: 0x0010D16B
		private void Start()
		{
			this.audioSource.Stop();
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x0010ED78 File Offset: 0x0010D178
		protected virtual void CreatePCMSource()
		{
			this.pcmSource = new VoipPCMSourceNative();
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x0010ED85 File Offset: 0x0010D185
		protected static int MSToElements(int ms)
		{
			return ms * VoipAudioSourceHiLevel.audioSystemPlaybackFrequency / 1000;
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x0010ED94 File Offset: 0x0010D194
		private void Update()
		{
			this.pcmSource.Update();
			if (!this.audioSource.isPlaying && this.pcmSource.PeekSizeElements() >= VoipAudioSourceHiLevel.MSToElements(this.initialPlaybackDelayMS))
			{
				if (VoipAudioSourceHiLevel.verboseLogging)
				{
					Debug.LogFormat("buffered {0} elements, starting playback", new object[]
					{
						this.pcmSource.PeekSizeElements()
					});
				}
				this.audioSource.Play();
			}
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x0010EE0F File Offset: 0x0010D20F
		// Note: this type is marked as 'beforefieldinit'.
		static VoipAudioSourceHiLevel()
		{
		}

		// Token: 0x04002912 RID: 10514
		private int initialPlaybackDelayMS;

		// Token: 0x04002913 RID: 10515
		public AudioSource audioSource;

		// Token: 0x04002914 RID: 10516
		public float peakAmplitude;

		// Token: 0x04002915 RID: 10517
		protected IVoipPCMSource pcmSource;

		// Token: 0x04002916 RID: 10518
		private static int audioSystemPlaybackFrequency;

		// Token: 0x04002917 RID: 10519
		private static bool verboseLogging;

		// Token: 0x020008AC RID: 2220
		public class FilterReadDelegate : MonoBehaviour
		{
			// Token: 0x060037E9 RID: 14313 RVA: 0x0010EE11 File Offset: 0x0010D211
			public FilterReadDelegate()
			{
			}

			// Token: 0x060037EA RID: 14314 RVA: 0x0010EE1C File Offset: 0x0010D21C
			private void Awake()
			{
				int num = (int)((uint)CAPI.ovr_Voip_GetOutputBufferMaxSize());
				this.scratchBuffer = new float[num];
			}

			// Token: 0x060037EB RID: 14315 RVA: 0x0010EE40 File Offset: 0x0010D240
			private void OnAudioFilterRead(float[] data, int channels)
			{
				int num = data.Length / channels;
				int num2 = num;
				if (num2 > this.scratchBuffer.Length)
				{
					Array.Clear(data, 0, data.Length);
					throw new Exception(string.Format("Audio system tried to pull {0} bytes, max voip internal ring buffer size {1}", num, this.scratchBuffer.Length));
				}
				int num3 = this.parent.pcmSource.PeekSizeElements();
				if (num3 < num2)
				{
					if (VoipAudioSourceHiLevel.verboseLogging)
					{
						Debug.LogFormat("Voip starved! Want {0}, but only have {1} available", new object[]
						{
							num2,
							num3
						});
					}
					return;
				}
				int pcm = this.parent.pcmSource.GetPCM(this.scratchBuffer, num2);
				if (pcm < num2)
				{
					Debug.LogWarningFormat("GetPCM() returned {0} samples, expected {1}", new object[]
					{
						pcm,
						num2
					});
					return;
				}
				int num4 = 0;
				float num5 = -1f;
				for (int i = 0; i < num; i++)
				{
					float num6 = this.scratchBuffer[i];
					for (int j = 0; j < channels; j++)
					{
						data[num4++] = num6;
						if (num6 > num5)
						{
							num5 = num6;
						}
					}
				}
				this.parent.peakAmplitude = num5;
			}

			// Token: 0x04002918 RID: 10520
			public VoipAudioSourceHiLevel parent;

			// Token: 0x04002919 RID: 10521
			private float[] scratchBuffer;
		}
	}
}
