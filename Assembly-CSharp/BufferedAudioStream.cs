using System;
using UnityEngine;

// Token: 0x020007E2 RID: 2018
public class BufferedAudioStream
{
	// Token: 0x06003304 RID: 13060 RVA: 0x00108E10 File Offset: 0x00107210
	public BufferedAudioStream(AudioSource audio)
	{
		this.audioBuffer = new float[12000];
		this.audio = audio;
		audio.loop = true;
		audio.clip = AudioClip.Create(string.Empty, 12000, 1, 48000, false);
		this.Stop();
	}

	// Token: 0x06003305 RID: 13061 RVA: 0x00108E64 File Offset: 0x00107264
	public void Update()
	{
		if (this.remainingBufferTime > 0f)
		{
			if (!this.audio.isPlaying && this.remainingBufferTime > 0.05f)
			{
				this.playbackDelayRemaining -= Time.deltaTime;
				if (this.playbackDelayRemaining <= 0f)
				{
					this.audio.Play();
				}
			}
			if (this.audio.isPlaying)
			{
				this.remainingBufferTime -= Time.deltaTime;
				if (this.remainingBufferTime < 0f)
				{
					this.remainingBufferTime = 0f;
				}
			}
		}
		if (this.remainingBufferTime <= 0f)
		{
			if (this.audio.isPlaying)
			{
				Debug.Log("Buffer empty, stopping " + DateTime.Now);
				this.Stop();
			}
			else if (this.writePos != 0)
			{
				Debug.LogError("writePos non zero while not playing, how did this happen?");
			}
		}
	}

	// Token: 0x06003306 RID: 13062 RVA: 0x00108F64 File Offset: 0x00107364
	private void Stop()
	{
		this.audio.Stop();
		this.audio.time = 0f;
		this.writePos = 0;
		this.playbackDelayRemaining = 0.05f;
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x00108F94 File Offset: 0x00107394
	public void AddData(float[] samples)
	{
		int num = samples.Length;
		if (this.writePos > this.audioBuffer.Length)
		{
			throw new Exception();
		}
		for (;;)
		{
			int num2 = num;
			int num3 = this.audioBuffer.Length - this.writePos;
			if (num2 > num3)
			{
				num2 = num3;
			}
			Array.Copy(samples, 0, this.audioBuffer, this.writePos, num2);
			num -= num2;
			this.writePos += num2;
			if (this.writePos > this.audioBuffer.Length)
			{
				break;
			}
			if (this.writePos == this.audioBuffer.Length)
			{
				this.writePos = 0;
			}
			if (num <= 0)
			{
				goto Block_5;
			}
		}
		throw new Exception();
		Block_5:
		this.remainingBufferTime += (float)samples.Length / 48000f;
		this.audio.clip.SetData(this.audioBuffer, 0);
	}

	// Token: 0x04002700 RID: 9984
	private const bool VerboseLogging = false;

	// Token: 0x04002701 RID: 9985
	private AudioSource audio;

	// Token: 0x04002702 RID: 9986
	private float[] audioBuffer;

	// Token: 0x04002703 RID: 9987
	private int writePos;

	// Token: 0x04002704 RID: 9988
	private const float bufferLengthSeconds = 0.25f;

	// Token: 0x04002705 RID: 9989
	private const int sampleRate = 48000;

	// Token: 0x04002706 RID: 9990
	private const int bufferSize = 12000;

	// Token: 0x04002707 RID: 9991
	private const float playbackDelayTimeSeconds = 0.05f;

	// Token: 0x04002708 RID: 9992
	private float playbackDelayRemaining;

	// Token: 0x04002709 RID: 9993
	private float remainingBufferTime;
}
