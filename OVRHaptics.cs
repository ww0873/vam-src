using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public static class OVRHaptics
{
	// Token: 0x060038D7 RID: 14551 RVA: 0x00114CB8 File Offset: 0x001130B8
	static OVRHaptics()
	{
		OVRHaptics.Config.Load();
		OVRHaptics.m_outputs = new OVRHaptics.OVRHapticsOutput[]
		{
			new OVRHaptics.OVRHapticsOutput(1U),
			new OVRHaptics.OVRHapticsOutput(2U)
		};
		OVRHaptics.Channels = new OVRHaptics.OVRHapticsChannel[]
		{
			OVRHaptics.LeftChannel = new OVRHaptics.OVRHapticsChannel(0U),
			OVRHaptics.RightChannel = new OVRHaptics.OVRHapticsChannel(1U)
		};
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x00114D10 File Offset: 0x00113110
	public static void Process()
	{
		OVRHaptics.Config.Load();
		for (int i = 0; i < OVRHaptics.m_outputs.Length; i++)
		{
			OVRHaptics.m_outputs[i].Process();
		}
	}

	// Token: 0x040029FC RID: 10748
	public static readonly OVRHaptics.OVRHapticsChannel[] Channels;

	// Token: 0x040029FD RID: 10749
	public static readonly OVRHaptics.OVRHapticsChannel LeftChannel;

	// Token: 0x040029FE RID: 10750
	public static readonly OVRHaptics.OVRHapticsChannel RightChannel;

	// Token: 0x040029FF RID: 10751
	private static readonly OVRHaptics.OVRHapticsOutput[] m_outputs;

	// Token: 0x020008D4 RID: 2260
	public static class Config
	{
		// Token: 0x060038D9 RID: 14553 RVA: 0x00114D46 File Offset: 0x00113146
		static Config()
		{
			OVRHaptics.Config.Load();
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060038DA RID: 14554 RVA: 0x00114D4D File Offset: 0x0011314D
		// (set) Token: 0x060038DB RID: 14555 RVA: 0x00114D54 File Offset: 0x00113154
		public static int SampleRateHz
		{
			[CompilerGenerated]
			get
			{
				return OVRHaptics.Config.<SampleRateHz>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				OVRHaptics.Config.<SampleRateHz>k__BackingField = value;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060038DC RID: 14556 RVA: 0x00114D5C File Offset: 0x0011315C
		// (set) Token: 0x060038DD RID: 14557 RVA: 0x00114D63 File Offset: 0x00113163
		public static int SampleSizeInBytes
		{
			[CompilerGenerated]
			get
			{
				return OVRHaptics.Config.<SampleSizeInBytes>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				OVRHaptics.Config.<SampleSizeInBytes>k__BackingField = value;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060038DE RID: 14558 RVA: 0x00114D6B File Offset: 0x0011316B
		// (set) Token: 0x060038DF RID: 14559 RVA: 0x00114D72 File Offset: 0x00113172
		public static int MinimumSafeSamplesQueued
		{
			[CompilerGenerated]
			get
			{
				return OVRHaptics.Config.<MinimumSafeSamplesQueued>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				OVRHaptics.Config.<MinimumSafeSamplesQueued>k__BackingField = value;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060038E0 RID: 14560 RVA: 0x00114D7A File Offset: 0x0011317A
		// (set) Token: 0x060038E1 RID: 14561 RVA: 0x00114D81 File Offset: 0x00113181
		public static int MinimumBufferSamplesCount
		{
			[CompilerGenerated]
			get
			{
				return OVRHaptics.Config.<MinimumBufferSamplesCount>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				OVRHaptics.Config.<MinimumBufferSamplesCount>k__BackingField = value;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060038E2 RID: 14562 RVA: 0x00114D89 File Offset: 0x00113189
		// (set) Token: 0x060038E3 RID: 14563 RVA: 0x00114D90 File Offset: 0x00113190
		public static int OptimalBufferSamplesCount
		{
			[CompilerGenerated]
			get
			{
				return OVRHaptics.Config.<OptimalBufferSamplesCount>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				OVRHaptics.Config.<OptimalBufferSamplesCount>k__BackingField = value;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060038E4 RID: 14564 RVA: 0x00114D98 File Offset: 0x00113198
		// (set) Token: 0x060038E5 RID: 14565 RVA: 0x00114D9F File Offset: 0x0011319F
		public static int MaximumBufferSamplesCount
		{
			[CompilerGenerated]
			get
			{
				return OVRHaptics.Config.<MaximumBufferSamplesCount>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				OVRHaptics.Config.<MaximumBufferSamplesCount>k__BackingField = value;
			}
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x00114DA8 File Offset: 0x001131A8
		public static void Load()
		{
			OVRPlugin.HapticsDesc controllerHapticsDesc = OVRPlugin.GetControllerHapticsDesc(2U);
			OVRHaptics.Config.SampleRateHz = controllerHapticsDesc.SampleRateHz;
			OVRHaptics.Config.SampleSizeInBytes = controllerHapticsDesc.SampleSizeInBytes;
			OVRHaptics.Config.MinimumSafeSamplesQueued = controllerHapticsDesc.MinimumSafeSamplesQueued;
			OVRHaptics.Config.MinimumBufferSamplesCount = controllerHapticsDesc.MinimumBufferSamplesCount;
			OVRHaptics.Config.OptimalBufferSamplesCount = controllerHapticsDesc.OptimalBufferSamplesCount;
			OVRHaptics.Config.MaximumBufferSamplesCount = controllerHapticsDesc.MaximumBufferSamplesCount;
		}

		// Token: 0x04002A00 RID: 10752
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int <SampleRateHz>k__BackingField;

		// Token: 0x04002A01 RID: 10753
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int <SampleSizeInBytes>k__BackingField;

		// Token: 0x04002A02 RID: 10754
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int <MinimumSafeSamplesQueued>k__BackingField;

		// Token: 0x04002A03 RID: 10755
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int <MinimumBufferSamplesCount>k__BackingField;

		// Token: 0x04002A04 RID: 10756
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int <OptimalBufferSamplesCount>k__BackingField;

		// Token: 0x04002A05 RID: 10757
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int <MaximumBufferSamplesCount>k__BackingField;
	}

	// Token: 0x020008D5 RID: 2261
	public class OVRHapticsChannel
	{
		// Token: 0x060038E7 RID: 14567 RVA: 0x00114E04 File Offset: 0x00113204
		public OVRHapticsChannel(uint outputIndex)
		{
			this.m_output = OVRHaptics.m_outputs[(int)((UIntPtr)outputIndex)];
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x00114E1A File Offset: 0x0011321A
		public void Preempt(OVRHapticsClip clip)
		{
			this.m_output.Preempt(clip);
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x00114E28 File Offset: 0x00113228
		public void Queue(OVRHapticsClip clip)
		{
			this.m_output.Queue(clip);
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x00114E36 File Offset: 0x00113236
		public void Mix(OVRHapticsClip clip)
		{
			this.m_output.Mix(clip);
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x00114E44 File Offset: 0x00113244
		public void Clear()
		{
			this.m_output.Clear();
		}

		// Token: 0x04002A06 RID: 10758
		private OVRHaptics.OVRHapticsOutput m_output;
	}

	// Token: 0x020008D6 RID: 2262
	private class OVRHapticsOutput
	{
		// Token: 0x060038EC RID: 14572 RVA: 0x00114E54 File Offset: 0x00113254
		public OVRHapticsOutput(uint controller)
		{
			this.m_controller = controller;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x00114EA4 File Offset: 0x001132A4
		public void Process()
		{
			OVRPlugin.HapticsState controllerHapticsState = OVRPlugin.GetControllerHapticsState(this.m_controller);
			float num = Time.realtimeSinceStartup - this.m_prevSamplesQueuedTime;
			if (this.m_prevSamplesQueued > 0)
			{
				int num2 = this.m_prevSamplesQueued - (int)(num * (float)OVRHaptics.Config.SampleRateHz + 0.5f);
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (controllerHapticsState.SamplesQueued - num2 == 0)
				{
					this.m_numPredictionHits++;
				}
				else
				{
					this.m_numPredictionMisses++;
				}
				if (num2 > 0 && controllerHapticsState.SamplesQueued == 0)
				{
					this.m_numUnderruns++;
				}
				this.m_prevSamplesQueued = controllerHapticsState.SamplesQueued;
				this.m_prevSamplesQueuedTime = Time.realtimeSinceStartup;
			}
			int num3 = OVRHaptics.Config.OptimalBufferSamplesCount;
			if (this.m_lowLatencyMode)
			{
				float num4 = 1000f / (float)OVRHaptics.Config.SampleRateHz;
				float num5 = num * 1000f;
				int num6 = (int)Mathf.Ceil(num5 / num4);
				int num7 = OVRHaptics.Config.MinimumSafeSamplesQueued + num6;
				if (num7 < num3)
				{
					num3 = num7;
				}
			}
			if (controllerHapticsState.SamplesQueued > num3)
			{
				return;
			}
			if (num3 > OVRHaptics.Config.MaximumBufferSamplesCount)
			{
				num3 = OVRHaptics.Config.MaximumBufferSamplesCount;
			}
			if (num3 > controllerHapticsState.SamplesAvailable)
			{
				num3 = controllerHapticsState.SamplesAvailable;
			}
			int num8 = 0;
			int num9 = 0;
			while (num8 < num3 && num9 < this.m_pendingClips.Count)
			{
				int num10 = num3 - num8;
				int num11 = this.m_pendingClips[num9].Clip.Count - this.m_pendingClips[num9].ReadCount;
				if (num10 > num11)
				{
					num10 = num11;
				}
				if (num10 > 0)
				{
					int length = num10 * OVRHaptics.Config.SampleSizeInBytes;
					int byteOffset = num8 * OVRHaptics.Config.SampleSizeInBytes;
					int startIndex = this.m_pendingClips[num9].ReadCount * OVRHaptics.Config.SampleSizeInBytes;
					Marshal.Copy(this.m_pendingClips[num9].Clip.Samples, startIndex, this.m_nativeBuffer.GetPointer(byteOffset), length);
					this.m_pendingClips[num9].ReadCount += num10;
					num8 += num10;
				}
				num9++;
			}
			int num12 = this.m_pendingClips.Count - 1;
			while (num12 >= 0 && this.m_pendingClips.Count > 0)
			{
				if (this.m_pendingClips[num12].ReadCount >= this.m_pendingClips[num12].Clip.Count)
				{
					this.m_pendingClips.RemoveAt(num12);
				}
				num12--;
			}
			int num13 = num3 - (controllerHapticsState.SamplesQueued + num8);
			if (num13 < OVRHaptics.Config.MinimumBufferSamplesCount - num8)
			{
				num13 = OVRHaptics.Config.MinimumBufferSamplesCount - num8;
			}
			if (num13 > controllerHapticsState.SamplesAvailable)
			{
				num13 = controllerHapticsState.SamplesAvailable;
			}
			if (num13 > 0)
			{
				int length2 = num13 * OVRHaptics.Config.SampleSizeInBytes;
				int byteOffset2 = num8 * OVRHaptics.Config.SampleSizeInBytes;
				int startIndex2 = 0;
				Marshal.Copy(this.m_paddingClip.Samples, startIndex2, this.m_nativeBuffer.GetPointer(byteOffset2), length2);
				num8 += num13;
			}
			if (num8 > 0)
			{
				OVRPlugin.HapticsBuffer hapticsBuffer;
				hapticsBuffer.Samples = this.m_nativeBuffer.GetPointer(0);
				hapticsBuffer.SamplesCount = num8;
				OVRPlugin.SetControllerHaptics(this.m_controller, hapticsBuffer);
				this.m_prevSamplesQueued = OVRPlugin.GetControllerHapticsState(this.m_controller).SamplesQueued;
				this.m_prevSamplesQueuedTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x00115214 File Offset: 0x00113614
		public void Preempt(OVRHapticsClip clip)
		{
			this.m_pendingClips.Clear();
			this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x00115232 File Offset: 0x00113632
		public void Queue(OVRHapticsClip clip)
		{
			this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x00115248 File Offset: 0x00113648
		public void Mix(OVRHapticsClip clip)
		{
			int num = 0;
			int num2 = 0;
			int num3 = clip.Count;
			while (num3 > 0 && num < this.m_pendingClips.Count)
			{
				int num4 = this.m_pendingClips[num].Clip.Count - this.m_pendingClips[num].ReadCount;
				num3 -= num4;
				num2 += num4;
				num++;
			}
			if (num3 > 0)
			{
				num2 += num3;
			}
			if (num > 0)
			{
				OVRHapticsClip ovrhapticsClip = new OVRHapticsClip(num2);
				int i = 0;
				for (int j = 0; j < num; j++)
				{
					OVRHapticsClip clip2 = this.m_pendingClips[j].Clip;
					for (int k = this.m_pendingClips[j].ReadCount; k < clip2.Count; k++)
					{
						if (OVRHaptics.Config.SampleSizeInBytes == 1)
						{
							byte sample = 0;
							if (i < clip.Count && k < clip2.Count)
							{
								sample = (byte)Mathf.Clamp((int)(clip.Samples[i] + clip2.Samples[k]), 0, 255);
								i++;
							}
							else if (k < clip2.Count)
							{
								sample = clip2.Samples[k];
							}
							ovrhapticsClip.WriteSample(sample);
						}
					}
				}
				while (i < clip.Count)
				{
					if (OVRHaptics.Config.SampleSizeInBytes == 1)
					{
						ovrhapticsClip.WriteSample(clip.Samples[i]);
					}
					i++;
				}
				this.m_pendingClips[0] = new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(ovrhapticsClip);
				for (int l = 1; l < num; l++)
				{
					this.m_pendingClips.RemoveAt(1);
				}
			}
			else
			{
				this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
			}
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x00115426 File Offset: 0x00113826
		public void Clear()
		{
			this.m_pendingClips.Clear();
		}

		// Token: 0x04002A07 RID: 10759
		private bool m_lowLatencyMode = true;

		// Token: 0x04002A08 RID: 10760
		private int m_prevSamplesQueued;

		// Token: 0x04002A09 RID: 10761
		private float m_prevSamplesQueuedTime;

		// Token: 0x04002A0A RID: 10762
		private int m_numPredictionHits;

		// Token: 0x04002A0B RID: 10763
		private int m_numPredictionMisses;

		// Token: 0x04002A0C RID: 10764
		private int m_numUnderruns;

		// Token: 0x04002A0D RID: 10765
		private List<OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker> m_pendingClips = new List<OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker>();

		// Token: 0x04002A0E RID: 10766
		private uint m_controller;

		// Token: 0x04002A0F RID: 10767
		private OVRNativeBuffer m_nativeBuffer = new OVRNativeBuffer(OVRHaptics.Config.MaximumBufferSamplesCount * OVRHaptics.Config.SampleSizeInBytes);

		// Token: 0x04002A10 RID: 10768
		private OVRHapticsClip m_paddingClip = new OVRHapticsClip();

		// Token: 0x020008D7 RID: 2263
		private class ClipPlaybackTracker
		{
			// Token: 0x060038F2 RID: 14578 RVA: 0x00115433 File Offset: 0x00113833
			public ClipPlaybackTracker(OVRHapticsClip clip)
			{
				this.Clip = clip;
			}

			// Token: 0x17000636 RID: 1590
			// (get) Token: 0x060038F3 RID: 14579 RVA: 0x00115442 File Offset: 0x00113842
			// (set) Token: 0x060038F4 RID: 14580 RVA: 0x0011544A File Offset: 0x0011384A
			public int ReadCount
			{
				[CompilerGenerated]
				get
				{
					return this.<ReadCount>k__BackingField;
				}
				[CompilerGenerated]
				set
				{
					this.<ReadCount>k__BackingField = value;
				}
			}

			// Token: 0x17000637 RID: 1591
			// (get) Token: 0x060038F5 RID: 14581 RVA: 0x00115453 File Offset: 0x00113853
			// (set) Token: 0x060038F6 RID: 14582 RVA: 0x0011545B File Offset: 0x0011385B
			public OVRHapticsClip Clip
			{
				[CompilerGenerated]
				get
				{
					return this.<Clip>k__BackingField;
				}
				[CompilerGenerated]
				set
				{
					this.<Clip>k__BackingField = value;
				}
			}

			// Token: 0x04002A11 RID: 10769
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private int <ReadCount>k__BackingField;

			// Token: 0x04002A12 RID: 10770
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private OVRHapticsClip <Clip>k__BackingField;
		}
	}
}
