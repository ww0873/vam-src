using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x0200078F RID: 1935
public class OvrAvatarPacket
{
	// Token: 0x060031BF RID: 12735 RVA: 0x00104AE4 File Offset: 0x00102EE4
	public OvrAvatarPacket()
	{
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x00104B18 File Offset: 0x00102F18
	public OvrAvatarPacket(OvrAvatarDriver.PoseFrame initialPose)
	{
		this.frameTimes.Add(0f);
		this.frames.Add(initialPose);
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x00104B74 File Offset: 0x00102F74
	private OvrAvatarPacket(List<float> frameTimes, List<OvrAvatarDriver.PoseFrame> frames, List<byte[]> audioPackets)
	{
		this.frameTimes = frameTimes;
		this.frames = frames;
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060031C2 RID: 12738 RVA: 0x00104BC1 File Offset: 0x00102FC1
	public float Duration
	{
		get
		{
			return this.frameTimes[this.frameTimes.Count - 1];
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060031C3 RID: 12739 RVA: 0x00104BDB File Offset: 0x00102FDB
	public OvrAvatarDriver.PoseFrame FinalFrame
	{
		get
		{
			return this.frames[this.frames.Count - 1];
		}
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x00104BF5 File Offset: 0x00102FF5
	public void AddFrame(OvrAvatarDriver.PoseFrame frame, float deltaSeconds)
	{
		this.frameTimes.Add(this.Duration + deltaSeconds);
		this.frames.Add(frame);
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x00104C18 File Offset: 0x00103018
	public OvrAvatarDriver.PoseFrame GetPoseFrame(float seconds)
	{
		if (this.frames.Count == 1)
		{
			return this.frames[0];
		}
		int num = 1;
		while (num < this.frameTimes.Count && this.frameTimes[num] < seconds)
		{
			num++;
		}
		OvrAvatarDriver.PoseFrame a = this.frames[num - 1];
		OvrAvatarDriver.PoseFrame b = this.frames[num];
		float num2 = this.frameTimes[num - 1];
		float num3 = this.frameTimes[num];
		float t = (seconds - num2) / (num3 - num2);
		return OvrAvatarDriver.PoseFrame.Interpolate(a, b, t);
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x00104CC0 File Offset: 0x001030C0
	public static OvrAvatarPacket Read(Stream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int num = binaryReader.ReadInt32();
		List<float> list = new List<float>(num);
		for (int i = 0; i < num; i++)
		{
			list.Add(binaryReader.ReadSingle());
		}
		List<OvrAvatarDriver.PoseFrame> list2 = new List<OvrAvatarDriver.PoseFrame>(num);
		for (int j = 0; j < num; j++)
		{
			list2.Add(binaryReader.ReadPoseFrame());
		}
		int num2 = binaryReader.ReadInt32();
		List<byte[]> list3 = new List<byte[]>(num2);
		for (int k = 0; k < num2; k++)
		{
			int count = binaryReader.ReadInt32();
			byte[] item = binaryReader.ReadBytes(count);
			list3.Add(item);
		}
		return new OvrAvatarPacket(list, list2, list3);
	}

	// Token: 0x060031C7 RID: 12743 RVA: 0x00104D78 File Offset: 0x00103178
	public void Write(Stream stream)
	{
		BinaryWriter binaryWriter = new BinaryWriter(stream);
		int count = this.frameTimes.Count;
		binaryWriter.Write(count);
		for (int i = 0; i < count; i++)
		{
			binaryWriter.Write(this.frameTimes[i]);
		}
		for (int j = 0; j < count; j++)
		{
			OvrAvatarDriver.PoseFrame frame = this.frames[j];
			binaryWriter.Write(frame);
		}
		int count2 = this.encodedAudioPackets.Count;
		binaryWriter.Write(count2);
		for (int k = 0; k < count2; k++)
		{
			byte[] array = this.encodedAudioPackets[k];
			binaryWriter.Write(array.Length);
			binaryWriter.Write(array);
		}
	}

	// Token: 0x040025AA RID: 9642
	public IntPtr ovrNativePacket = IntPtr.Zero;

	// Token: 0x040025AB RID: 9643
	private List<float> frameTimes = new List<float>();

	// Token: 0x040025AC RID: 9644
	private List<OvrAvatarDriver.PoseFrame> frames = new List<OvrAvatarDriver.PoseFrame>();

	// Token: 0x040025AD RID: 9645
	private List<byte[]> encodedAudioPackets = new List<byte[]>();
}
