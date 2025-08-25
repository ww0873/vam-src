using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000B81 RID: 2945
public class WAV
{
	// Token: 0x060052D2 RID: 21202 RVA: 0x001DEFF8 File Offset: 0x001DD3F8
	public WAV(byte[] wav)
	{
		this.ChannelCount = (int)wav[22];
		this.Frequency = WAV.bytesToInt(wav, 24);
		int i = 12;
		while (wav[i] != 100 || wav[i + 1] != 97 || wav[i + 2] != 116 || wav[i + 3] != 97)
		{
			i += 4;
			int num = (int)wav[i] + (int)wav[i + 1] * 256 + (int)wav[i + 2] * 65536 + (int)wav[i + 3] * 16777216;
			i += 4 + num;
		}
		i += 8;
		this.SampleCount = (wav.Length - i) / 2;
		if (this.ChannelCount == 2)
		{
			this.SampleCount /= 2;
		}
		this.LeftChannel = new float[this.SampleCount];
		if (this.ChannelCount == 2)
		{
			this.RightChannel = new float[this.SampleCount];
		}
		else
		{
			this.RightChannel = null;
		}
		int num2 = 0;
		while (i < wav.Length)
		{
			this.LeftChannel[num2] = WAV.bytesToFloat(wav[i], wav[i + 1]);
			i += 2;
			if (this.ChannelCount == 2)
			{
				this.RightChannel[num2] = WAV.bytesToFloat(wav[i], wav[i + 1]);
				i += 2;
			}
			num2++;
		}
	}

	// Token: 0x060052D3 RID: 21203 RVA: 0x001DF144 File Offset: 0x001DD544
	private static float bytesToFloat(byte firstByte, byte secondByte)
	{
		short num = (short)((int)secondByte << 8 | (int)firstByte);
		return (float)num / 32768f;
	}

	// Token: 0x060052D4 RID: 21204 RVA: 0x001DF160 File Offset: 0x001DD560
	private static int bytesToInt(byte[] bytes, int offset = 0)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			num |= (int)bytes[offset + i] << i * 8;
		}
		return num;
	}

	// Token: 0x17000C08 RID: 3080
	// (get) Token: 0x060052D5 RID: 21205 RVA: 0x001DF191 File Offset: 0x001DD591
	// (set) Token: 0x060052D6 RID: 21206 RVA: 0x001DF199 File Offset: 0x001DD599
	public float[] LeftChannel
	{
		[CompilerGenerated]
		get
		{
			return this.<LeftChannel>k__BackingField;
		}
		[CompilerGenerated]
		internal set
		{
			this.<LeftChannel>k__BackingField = value;
		}
	}

	// Token: 0x17000C09 RID: 3081
	// (get) Token: 0x060052D7 RID: 21207 RVA: 0x001DF1A2 File Offset: 0x001DD5A2
	// (set) Token: 0x060052D8 RID: 21208 RVA: 0x001DF1AA File Offset: 0x001DD5AA
	public float[] RightChannel
	{
		[CompilerGenerated]
		get
		{
			return this.<RightChannel>k__BackingField;
		}
		[CompilerGenerated]
		internal set
		{
			this.<RightChannel>k__BackingField = value;
		}
	}

	// Token: 0x17000C0A RID: 3082
	// (get) Token: 0x060052D9 RID: 21209 RVA: 0x001DF1B3 File Offset: 0x001DD5B3
	// (set) Token: 0x060052DA RID: 21210 RVA: 0x001DF1BB File Offset: 0x001DD5BB
	public int ChannelCount
	{
		[CompilerGenerated]
		get
		{
			return this.<ChannelCount>k__BackingField;
		}
		[CompilerGenerated]
		internal set
		{
			this.<ChannelCount>k__BackingField = value;
		}
	}

	// Token: 0x17000C0B RID: 3083
	// (get) Token: 0x060052DB RID: 21211 RVA: 0x001DF1C4 File Offset: 0x001DD5C4
	// (set) Token: 0x060052DC RID: 21212 RVA: 0x001DF1CC File Offset: 0x001DD5CC
	public int SampleCount
	{
		[CompilerGenerated]
		get
		{
			return this.<SampleCount>k__BackingField;
		}
		[CompilerGenerated]
		internal set
		{
			this.<SampleCount>k__BackingField = value;
		}
	}

	// Token: 0x17000C0C RID: 3084
	// (get) Token: 0x060052DD RID: 21213 RVA: 0x001DF1D5 File Offset: 0x001DD5D5
	// (set) Token: 0x060052DE RID: 21214 RVA: 0x001DF1DD File Offset: 0x001DD5DD
	public int Frequency
	{
		[CompilerGenerated]
		get
		{
			return this.<Frequency>k__BackingField;
		}
		[CompilerGenerated]
		internal set
		{
			this.<Frequency>k__BackingField = value;
		}
	}

	// Token: 0x060052DF RID: 21215 RVA: 0x001DF1E8 File Offset: 0x001DD5E8
	public override string ToString()
	{
		return string.Format("[WAV: LeftChannel={0}, RightChannel={1}, ChannelCount={2}, SampleCount={3}, Frequency={4}]", new object[]
		{
			this.LeftChannel,
			this.RightChannel,
			this.ChannelCount,
			this.SampleCount,
			this.Frequency
		});
	}

	// Token: 0x040042A6 RID: 17062
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float[] <LeftChannel>k__BackingField;

	// Token: 0x040042A7 RID: 17063
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float[] <RightChannel>k__BackingField;

	// Token: 0x040042A8 RID: 17064
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int <ChannelCount>k__BackingField;

	// Token: 0x040042A9 RID: 17065
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int <SampleCount>k__BackingField;

	// Token: 0x040042AA RID: 17066
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int <Frequency>k__BackingField;
}
