using System;
using System.IO;
using NAudio.Wave;
using UnityEngine;

// Token: 0x02000B80 RID: 2944
public static class NAudioPlayer
{
	// Token: 0x060052CE RID: 21198 RVA: 0x001DEE30 File Offset: 0x001DD230
	public static WAV WAVFromMp3Data(byte[] data)
	{
		MemoryStream inputStream = new MemoryStream(data);
		Mp3FileReader sourceStream = new Mp3FileReader(inputStream);
		WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(sourceStream);
		if (waveStream.TotalTime.TotalSeconds < 1200.0)
		{
			return new WAV(NAudioPlayer.AudioMemStream(waveStream).ToArray());
		}
		Debug.LogError("MP3 is too long (> 20mins) to convert to WAV as it will require too much memory and crash Unity.");
		return null;
	}

	// Token: 0x060052CF RID: 21199 RVA: 0x001DEE90 File Offset: 0x001DD290
	public static AudioClip AudioClipFromWAV(WAV wav)
	{
		int channelCount = wav.ChannelCount;
		AudioClip audioClip;
		if (channelCount == 1)
		{
			audioClip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
			audioClip.SetData(wav.LeftChannel, 0);
		}
		else
		{
			audioClip = AudioClip.Create("testSound", wav.SampleCount, 2, wav.Frequency, false);
			float[] array = new float[wav.LeftChannel.Length + wav.RightChannel.Length];
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < array.Length)
			{
				array[i] = wav.LeftChannel[num];
				num++;
				i++;
				array[i] = wav.RightChannel[num2];
				num2++;
				i++;
			}
			audioClip.SetData(array, 0);
		}
		return audioClip;
	}

	// Token: 0x060052D0 RID: 21200 RVA: 0x001DEF54 File Offset: 0x001DD354
	public static AudioClip AudioClipFromMp3Data(byte[] data)
	{
		WAV wav = NAudioPlayer.WAVFromMp3Data(data);
		if (wav != null)
		{
			return NAudioPlayer.AudioClipFromWAV(wav);
		}
		return null;
	}

	// Token: 0x060052D1 RID: 21201 RVA: 0x001DEF78 File Offset: 0x001DD378
	private static MemoryStream AudioMemStream(WaveStream waveStream)
	{
		MemoryStream memoryStream = new MemoryStream();
		using (WaveFileWriter waveFileWriter = new WaveFileWriter(memoryStream, waveStream.WaveFormat))
		{
			byte[] array = new byte[waveStream.Length];
			waveStream.Position = 0L;
			waveStream.Read(array, 0, Convert.ToInt32(waveStream.Length));
			waveFileWriter.Write(array, 0, array.Length);
			waveFileWriter.Flush();
		}
		return memoryStream;
	}
}
