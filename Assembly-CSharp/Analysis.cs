using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000423 RID: 1059
[Serializable]
public class Analysis
{
	// Token: 0x06001A8C RID: 6796 RVA: 0x00094871 File Offset: 0x00092C71
	public Analysis()
	{
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001A8D RID: 6797 RVA: 0x0009488C File Offset: 0x00092C8C
	public Frame[] Frames
	{
		get
		{
			return this.frames;
		}
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x00094894 File Offset: 0x00092C94
	public void Init(int totalFrames, bool advancedAnalysis)
	{
		this.totalFrames = totalFrames;
		this.advancedAnalysis = advancedAnalysis;
		this.frames = new Frame[totalFrames];
		int num = 2047;
		if (this.end < this.start || this.start < 0 || this.end < 0 || this.start >= num || this.end > num)
		{
			Debug.LogError("Invalid range for analysis " + this.name + ". Range must be within Spectrum (fftWindowSize/2 - 1) and start cannot come after end.");
		}
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x0009491C File Offset: 0x00092D1C
	public void Analyze(float[] spectrum, int index)
	{
		this.frames[index].magnitude = this.Sum(spectrum, this.start, this.end);
		this.frames[index].magnitudeSmooth = this.frames[index].magnitude;
		int num = Mathf.Max(index - 10, 0);
		this.frames[num].flux = this.frames[num].magnitude - this.frames[Mathf.Max(num - 1, 0)].magnitude;
		this.Smooth(num, 10);
		num = Mathf.Max(index - 20, 0);
		this.frames[num].threshold = this.Threshold(this.frames, num, this.thresholdMultiplier, this.thresholdWindowSize);
		this.Smooth(num, 10);
		num = Mathf.Max(index - 30, 0);
		if (this.frames[num].flux > this.frames[num].threshold && this.frames[num].flux > this.frames[Mathf.Min(num + 1, this.frames.Length - 1)].flux && this.frames[num].flux > this.frames[Mathf.Max(num - 1, 0)].flux)
		{
			this.frames[num].onset = this.frames[num].flux;
		}
		num = Mathf.Max(index - 100, 0);
		this.Rank(num, 50);
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x00094AD0 File Offset: 0x00092ED0
	public void DrawDebugLines(int index, int h)
	{
		for (int i = 0; i < 299; i++)
		{
			if (i + 1 + index > this.totalFrames - 1)
			{
				break;
			}
			Vector3 vector = new Vector3((float)i, this.frames[i + index].magnitude + (float)(h * 100), 0f);
			Vector3 vector2 = new Vector3((float)(i + 1), this.frames[i + 1 + index].magnitude + (float)(h * 100), 0f);
			Debug.DrawLine(vector, vector2, Color.red);
			vector = new Vector3((float)i, this.frames[i + index].magnitudeSmooth + (float)(h * 100), 0f);
			vector2 = new Vector3((float)(i + 1), this.frames[i + 1 + index].magnitudeSmooth + (float)(h * 100), 0f);
			Debug.DrawLine(vector, vector2, Color.red);
			vector = new Vector3((float)i, this.frames[i + index].flux + (float)(h * 100), 0f);
			vector2 = new Vector3((float)(i + 1), this.frames[i + 1 + index].flux + (float)(h * 100), 0f);
			Debug.DrawLine(vector, vector2, Color.blue);
			vector = new Vector3((float)i, this.frames[i + index].threshold + (float)(h * 100), 0f);
			vector2 = new Vector3((float)(i + 1), this.frames[i + 1 + index].threshold + (float)(h * 100), 0f);
			Debug.DrawLine(vector, vector2, Color.blue);
			vector = new Vector3((float)i, this.frames[i + index].onset + (float)(h * 100), 0f);
			vector2 = new Vector3((float)(i + 1), this.frames[i + 1 + index].onset + (float)(h * 100), 0f);
			Debug.DrawLine(vector, vector2, Color.yellow);
			vector = new Vector3((float)i, (float)(-(float)this.frames[i + index].onsetRank + h * 100), 0f);
			vector2 = new Vector3((float)(i + 1), (float)(-(float)this.frames[i + 1 + index].onsetRank + h * 100), 0f);
			Debug.DrawLine(vector, vector2, Color.white);
		}
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x00094D40 File Offset: 0x00093140
	private float Threshold(Frame[] input, int index, float multiplier, int windowSize)
	{
		int num = Mathf.Max(0, index - windowSize);
		int num2 = Mathf.Min(input.Length - 1, index + windowSize);
		float num3 = 0f;
		for (int i = num; i <= num2; i++)
		{
			num3 += Mathf.Abs(input[i].flux);
		}
		num3 /= (float)(num2 - num);
		return num3 * multiplier;
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x00094DA0 File Offset: 0x000931A0
	private float Sum(float[] input, int start, int end)
	{
		float num = 0f;
		for (int i = start; i < end; i++)
		{
			if (this.advancedAnalysis)
			{
				num += input[i] * this.weightCurve.Evaluate((float)i);
			}
			else
			{
				num += input[i];
			}
		}
		return num;
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x00094DF0 File Offset: 0x000931F0
	private float Average(float[] input, int start, int end)
	{
		float num = 0f;
		for (int i = start; i < end; i++)
		{
			num += input[i];
		}
		return num / (float)(end - start);
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x00094E24 File Offset: 0x00093224
	private void Smooth(int index, int windowSize)
	{
		float num = 0f;
		for (int i = index - windowSize / 2; i < index + windowSize / 2; i++)
		{
			if (i > 0 && i < this.totalFrames)
			{
				num += this.frames[i].magnitudeSmooth;
			}
		}
		this.frames[index].magnitudeSmooth = num / (float)windowSize;
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x00094E90 File Offset: 0x00093290
	private void Rank(int index, int windowSize)
	{
		if (this.frames[index].onset == 0f)
		{
			return;
		}
		List<Frame> list = new List<Frame>();
		for (int i = 0; i < 5; i++)
		{
			int num = 0;
			int num2 = 0;
			for (int j = index - windowSize / 2; j < index - 1; j++)
			{
				if (j > 0 && j < this.totalFrames && this.frames[j].onset > 0f && !list.Contains(this.frames[j]))
				{
					num = j;
				}
			}
			for (int k = index + 1; k < index + windowSize / 2; k++)
			{
				if (k > 0 && k < this.totalFrames && this.frames[k].onset > 0f && !list.Contains(this.frames[k]))
				{
					num2 = k;
					break;
				}
			}
			if (this.frames[index].onset > this.frames[num].onset && this.frames[index].onset > this.frames[num2].onset)
			{
				this.frames[index].onsetRank = 5 - i;
				return;
			}
			if (this.frames[index].onset < this.frames[num].onset)
			{
				list.Add(this.frames[num]);
			}
			if (this.frames[index].onset < this.frames[num2].onset)
			{
				list.Add(this.frames[num2]);
			}
			if (num == 0 && num2 == 0)
			{
				this.frames[index].onsetRank = 5;
			}
		}
	}

	// Token: 0x040015AA RID: 5546
	public string name;

	// Token: 0x040015AB RID: 5547
	private float thresholdMultiplier = 1.5f;

	// Token: 0x040015AC RID: 5548
	private int thresholdWindowSize = 10;

	// Token: 0x040015AD RID: 5549
	private Frame[] frames;

	// Token: 0x040015AE RID: 5550
	public int start;

	// Token: 0x040015AF RID: 5551
	public int end;

	// Token: 0x040015B0 RID: 5552
	public AnimationCurve weightCurve;

	// Token: 0x040015B1 RID: 5553
	private int totalFrames;

	// Token: 0x040015B2 RID: 5554
	public bool advancedAnalysis;
}
