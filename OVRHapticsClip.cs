using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020008D8 RID: 2264
public class OVRHapticsClip
{
	// Token: 0x060038F7 RID: 14583 RVA: 0x00115464 File Offset: 0x00113864
	public OVRHapticsClip()
	{
		this.Capacity = OVRHaptics.Config.MaximumBufferSamplesCount;
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x0011548E File Offset: 0x0011388E
	public OVRHapticsClip(int capacity)
	{
		this.Capacity = ((capacity < 0) ? 0 : capacity);
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x001154C1 File Offset: 0x001138C1
	public OVRHapticsClip(byte[] samples, int samplesCount)
	{
		this.Samples = samples;
		this.Capacity = this.Samples.Length / OVRHaptics.Config.SampleSizeInBytes;
		this.Count = ((samplesCount < 0) ? 0 : samplesCount);
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x001154F8 File Offset: 0x001138F8
	public OVRHapticsClip(OVRHapticsClip a, OVRHapticsClip b)
	{
		int count = a.Count;
		if (b.Count > count)
		{
			count = b.Count;
		}
		this.Capacity = count;
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
		int num = 0;
		while (num < a.Count || num < b.Count)
		{
			if (OVRHaptics.Config.SampleSizeInBytes == 1)
			{
				byte sample = 0;
				if (num < a.Count && num < b.Count)
				{
					sample = (byte)Mathf.Clamp((int)(a.Samples[num] + b.Samples[num]), 0, 255);
				}
				else if (num < a.Count)
				{
					sample = a.Samples[num];
				}
				else if (num < b.Count)
				{
					sample = b.Samples[num];
				}
				this.WriteSample(sample);
			}
			num++;
		}
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x001155E4 File Offset: 0x001139E4
	public OVRHapticsClip(AudioClip audioClip, int channel = 0)
	{
		float[] array = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(array, 0);
		this.InitializeFromAudioFloatTrack(array, (double)audioClip.frequency, audioClip.channels, channel);
	}

	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x060038FC RID: 14588 RVA: 0x00115628 File Offset: 0x00113A28
	// (set) Token: 0x060038FD RID: 14589 RVA: 0x00115630 File Offset: 0x00113A30
	public int Count
	{
		[CompilerGenerated]
		get
		{
			return this.<Count>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<Count>k__BackingField = value;
		}
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x060038FE RID: 14590 RVA: 0x00115639 File Offset: 0x00113A39
	// (set) Token: 0x060038FF RID: 14591 RVA: 0x00115641 File Offset: 0x00113A41
	public int Capacity
	{
		[CompilerGenerated]
		get
		{
			return this.<Capacity>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<Capacity>k__BackingField = value;
		}
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06003900 RID: 14592 RVA: 0x0011564A File Offset: 0x00113A4A
	// (set) Token: 0x06003901 RID: 14593 RVA: 0x00115652 File Offset: 0x00113A52
	public byte[] Samples
	{
		[CompilerGenerated]
		get
		{
			return this.<Samples>k__BackingField;
		}
		[CompilerGenerated]
		private set
		{
			this.<Samples>k__BackingField = value;
		}
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x0011565C File Offset: 0x00113A5C
	public void WriteSample(byte sample)
	{
		if (this.Count >= this.Capacity)
		{
			return;
		}
		if (OVRHaptics.Config.SampleSizeInBytes == 1)
		{
			this.Samples[this.Count * OVRHaptics.Config.SampleSizeInBytes] = sample;
		}
		this.Count++;
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x001156A8 File Offset: 0x00113AA8
	public void Reset()
	{
		this.Count = 0;
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x001156B4 File Offset: 0x00113AB4
	private void InitializeFromAudioFloatTrack(float[] sourceData, double sourceFrequency, int sourceChannelCount, int sourceChannel)
	{
		double num = (sourceFrequency + 1E-06) / (double)OVRHaptics.Config.SampleRateHz;
		if (num < 1.0)
		{
			return;
		}
		int num2 = (int)num;
		double num3 = num - (double)num2;
		double num4 = 0.0;
		int num5 = sourceData.Length;
		this.Count = 0;
		this.Capacity = num5 / sourceChannelCount / num2 + 1;
		this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
		int i = sourceChannel % sourceChannelCount;
		while (i < num5)
		{
			if (OVRHaptics.Config.SampleSizeInBytes == 1)
			{
				this.WriteSample((byte)(Mathf.Clamp01(Mathf.Abs(sourceData[i])) * 255f));
			}
			i += num2 * sourceChannelCount;
			num4 += num3;
			if ((int)num4 > 0)
			{
				i += (int)num4 * sourceChannelCount;
				num4 -= (double)((int)num4);
			}
		}
	}

	// Token: 0x04002A13 RID: 10771
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int <Count>k__BackingField;

	// Token: 0x04002A14 RID: 10772
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int <Capacity>k__BackingField;

	// Token: 0x04002A15 RID: 10773
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private byte[] <Samples>k__BackingField;
}
