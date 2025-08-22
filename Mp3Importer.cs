using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class Mp3Importer
{
	// Token: 0x06001AA2 RID: 6818 RVA: 0x0009589D File Offset: 0x00093C9D
	public Mp3Importer()
	{
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x000958A8 File Offset: 0x00093CA8
	private AudioClip ImportFile(string filePath)
	{
		MPGImport.mpg123_init();
		this.handle_mpg = MPGImport.mpg123_new(null, this.errPtr);
		this.status = MPGImport.mpg123_open(this.handle_mpg, filePath);
		MPGImport.mpg123_getformat(this.handle_mpg, out this.rate, out this.channels, out this.encoding);
		this.intRate = this.rate.ToInt32();
		this.intChannels = this.channels.ToInt32();
		this.intEncoding = this.encoding.ToInt32();
		MPGImport.mpg123_id3(this.handle_mpg, out this.id3v1, out this.id3v2);
		MPGImport.mpg123_format_none(this.handle_mpg);
		MPGImport.mpg123_format(this.handle_mpg, this.intRate, this.intChannels, 208);
		this.FrameSize = MPGImport.mpg123_outblock(this.handle_mpg);
		byte[] array = new byte[this.FrameSize];
		this.lengthSamples = MPGImport.mpg123_length(this.handle_mpg);
		if (this.lengthSamples / this.intRate > 3000)
		{
			Debug.LogError("Audio file too big");
			return null;
		}
		if (this.lengthSamples / this.intRate > 2000)
		{
			Debug.LogWarning("Large audio file");
		}
		this.myClip = AudioClip.Create("myClip", this.lengthSamples, this.intChannels, this.intRate, false);
		int num = 0;
		while (MPGImport.mpg123_read(this.handle_mpg, array, this.FrameSize, out this.done) == 0)
		{
			float[] array2 = this.ByteToFloat(array);
			this.myClip.SetData(array2, num * array2.Length / 2);
			num++;
		}
		MPGImport.mpg123_close(this.handle_mpg);
		return this.myClip;
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x00095A5C File Offset: 0x00093E5C
	public static AudioClip Import(string filePath)
	{
		Mp3Importer mp3Importer = new Mp3Importer();
		return mp3Importer.ImportFile(filePath);
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x00095A78 File Offset: 0x00093E78
	public float[] IntToFloat(short[] from)
	{
		float[] array = new float[from.Length];
		for (int i = 0; i < from.Length; i++)
		{
			array[i] = (float)from[i] * 3.0517578E-05f;
		}
		return array;
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x00095AB4 File Offset: 0x00093EB4
	public short[] ByteToInt16(byte[] buffer)
	{
		short[] array = new short[1];
		int num = buffer.Length;
		if (num % 2 != 0)
		{
			Console.WriteLine("error");
			return array;
		}
		array = new short[num / 2];
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.Copy(buffer, 0, intPtr, num);
		Marshal.Copy(intPtr, array, 0, array.Length);
		Marshal.FreeHGlobal(intPtr);
		return array;
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x00095B0C File Offset: 0x00093F0C
	public float[] ByteToFloat(byte[] bArray)
	{
		short[] from = this.ByteToInt16(bArray);
		return this.IntToFloat(from);
	}

	// Token: 0x040015BD RID: 5565
	public IntPtr handle_mpg;

	// Token: 0x040015BE RID: 5566
	public IntPtr errPtr;

	// Token: 0x040015BF RID: 5567
	public IntPtr rate;

	// Token: 0x040015C0 RID: 5568
	public IntPtr channels;

	// Token: 0x040015C1 RID: 5569
	public IntPtr encoding;

	// Token: 0x040015C2 RID: 5570
	public IntPtr id3v1;

	// Token: 0x040015C3 RID: 5571
	public IntPtr id3v2;

	// Token: 0x040015C4 RID: 5572
	public IntPtr done;

	// Token: 0x040015C5 RID: 5573
	public int status;

	// Token: 0x040015C6 RID: 5574
	public int intRate;

	// Token: 0x040015C7 RID: 5575
	public int intChannels;

	// Token: 0x040015C8 RID: 5576
	public int intEncoding;

	// Token: 0x040015C9 RID: 5577
	public int FrameSize;

	// Token: 0x040015CA RID: 5578
	public int lengthSamples;

	// Token: 0x040015CB RID: 5579
	public AudioClip myClip;

	// Token: 0x040015CC RID: 5580
	private const float const_1_div_128_ = 0.0078125f;

	// Token: 0x040015CD RID: 5581
	private const float const_1_div_32768_ = 3.0517578E-05f;

	// Token: 0x040015CE RID: 5582
	private const double const_1_div_2147483648_ = 4.656612873077393E-10;
}
