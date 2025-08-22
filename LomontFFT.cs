using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Token: 0x02000426 RID: 1062
public class LomontFFT
{
	// Token: 0x06001A97 RID: 6807 RVA: 0x000950B9 File Offset: 0x000934B9
	public LomontFFT()
	{
		this.A = 0;
		this.B = 1;
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000950D0 File Offset: 0x000934D0
	public void FFT(double[] data, bool forward)
	{
		int i = data.Length;
		if ((i & i - 1) != 0)
		{
			throw new ArgumentException("data length " + i + " in FFT is not a power of 2");
		}
		i /= 2;
		LomontFFT.Reverse(data, i);
		double num = (double)((!forward) ? (-(double)this.B) : this.B);
		int num2 = 1;
		while (i > num2)
		{
			int num3 = 2 * num2;
			double num4 = num * 3.141592653589793 / (double)num2;
			double num5 = 1.0;
			double num6 = 0.0;
			double num7 = Math.Cos(num4);
			double num8 = Math.Sin(num4);
			for (int j = 0; j < num3; j += 2)
			{
				for (int k = j; k < 2 * i; k += 2 * num3)
				{
					int num9 = k + num3;
					double num10 = num5 * data[num9] - num6 * data[num9 + 1];
					double num11 = num6 * data[num9] + num5 * data[num9 + 1];
					data[num9] = data[k] - num10;
					data[num9 + 1] = data[k + 1] - num11;
					data[k] += num10;
					data[k + 1] = data[k + 1] + num11;
				}
				double num12 = num5;
				num5 = num5 * num7 - num6 * num8;
				num6 = num6 * num7 + num12 * num8;
			}
			num2 = num3;
		}
		this.Scale(data, i, forward);
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x00095230 File Offset: 0x00093630
	public void TableFFT(double[] data, bool forward)
	{
		int i = data.Length;
		if ((i & i - 1) != 0)
		{
			throw new ArgumentException("data length " + i + " in FFT is not a power of 2");
		}
		i /= 2;
		LomontFFT.Reverse(data, i);
		if (this.cosTable == null || this.cosTable.Length != i)
		{
			this.Initialize(i);
		}
		double num = (double)((!forward) ? (-(double)this.B) : this.B);
		int num2 = 1;
		int num3 = 0;
		while (i > num2)
		{
			int num4 = 2 * num2;
			for (int j = 0; j < num4; j += 2)
			{
				double num5 = this.cosTable[num3];
				double num6 = num * this.sinTable[num3++];
				for (int k = j; k < 2 * i; k += 2 * num4)
				{
					int num7 = k + num4;
					double num8 = num5 * data[num7] - num6 * data[num7 + 1];
					double num9 = num6 * data[num7] + num5 * data[num7 + 1];
					data[num7] = data[k] - num8;
					data[num7 + 1] = data[k + 1] - num9;
					data[k] += num8;
					data[k + 1] = data[k + 1] + num9;
				}
			}
			num2 = num4;
		}
		this.Scale(data, i, forward);
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x0009537C File Offset: 0x0009377C
	public void RealFFT(double[] data, bool forward)
	{
		int num = data.Length;
		if ((num & num - 1) != 0)
		{
			throw new ArgumentException("data length " + num + " in FFT is not a power of 2");
		}
		double num2 = -1.0;
		if (forward)
		{
			this.TableFFT(data, true);
			num2 = 1.0;
			if (this.A != 1)
			{
				double num3 = Math.Pow(2.0, (double)(this.A - 1) / 2.0);
				for (int i = 0; i < data.Length; i++)
				{
					data[i] *= num3;
				}
			}
		}
		double num4 = (double)this.B * num2 * 2.0 * 3.141592653589793 / (double)num;
		double num5 = Math.Cos(num4);
		double num6 = Math.Sin(num4);
		double num7 = num5;
		double num8 = num6;
		for (int j = 1; j <= num / 4; j++)
		{
			int num9 = num / 2 - j;
			double num10 = data[2 * num9];
			double num11 = data[2 * num9 + 1];
			double num12 = data[2 * j];
			double num13 = data[2 * j + 1];
			double num14 = (num12 - num10) * num8;
			double num15 = (num13 + num11) * num7;
			double num16 = (num12 - num10) * num7;
			double num17 = (num13 + num11) * num8;
			double num18 = num12 + num10;
			double num19 = num13 - num11;
			data[2 * j] = 0.5 * (num18 + num2 * (num14 + num15));
			data[2 * j + 1] = 0.5 * (num19 + num2 * (num17 - num16));
			data[2 * num9] = 0.5 * (num18 - num2 * (num15 + num14));
			data[2 * num9 + 1] = 0.5 * (num2 * (num17 - num16) - num19);
			double num20 = num7;
			num7 = num7 * num5 - num8 * num6;
			num8 = num20 * num6 + num8 * num5;
		}
		if (forward)
		{
			double num21 = data[0];
			data[0] += data[1];
			data[1] = num21 - data[1];
		}
		else
		{
			double num22 = data[0];
			data[0] = 0.5 * (num22 + data[1]);
			data[1] = 0.5 * (num22 - data[1]);
			this.TableFFT(data, false);
			double num23 = Math.Pow(2.0, (double)(-(double)(this.A + 1)) / 2.0) * 2.0;
			for (int k = 0; k < data.Length; k++)
			{
				data[k] *= num23;
			}
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06001A9B RID: 6811 RVA: 0x00095610 File Offset: 0x00093A10
	// (set) Token: 0x06001A9C RID: 6812 RVA: 0x00095618 File Offset: 0x00093A18
	public int A
	{
		[CompilerGenerated]
		get
		{
			return this.<A>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<A>k__BackingField = value;
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06001A9D RID: 6813 RVA: 0x00095621 File Offset: 0x00093A21
	// (set) Token: 0x06001A9E RID: 6814 RVA: 0x00095629 File Offset: 0x00093A29
	public int B
	{
		[CompilerGenerated]
		get
		{
			return this.<B>k__BackingField;
		}
		[CompilerGenerated]
		set
		{
			this.<B>k__BackingField = value;
		}
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x00095634 File Offset: 0x00093A34
	private void Scale(double[] data, int n, bool forward)
	{
		if (forward && this.A != 1)
		{
			double num = Math.Pow((double)n, (double)(this.A - 1) / 2.0);
			for (int i = 0; i < data.Length; i++)
			{
				data[i] *= num;
			}
		}
		if (!forward && this.A != -1)
		{
			double num2 = Math.Pow((double)n, (double)(-(double)(this.A + 1)) / 2.0);
			for (int j = 0; j < data.Length; j++)
			{
				data[j] *= num2;
			}
		}
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x000956DC File Offset: 0x00093ADC
	private void Initialize(int size)
	{
		this.cosTable = new double[size];
		this.sinTable = new double[size];
		int num = 1;
		int num2 = 0;
		while (size > num)
		{
			int num3 = 2 * num;
			double num4 = 3.141592653589793 / (double)num;
			double num5 = 1.0;
			double num6 = 0.0;
			double num7 = Math.Sin(num4);
			double num8 = Math.Sin(num4 / 2.0);
			num8 = -2.0 * num8 * num8;
			for (int i = 0; i < num3; i += 2)
			{
				this.cosTable[num2] = num5;
				this.sinTable[num2++] = num6;
				double num9 = num5;
				num5 = num5 * num8 - num6 * num7 + num5;
				num6 = num6 * num8 + num9 * num7 + num6;
			}
			num = num3;
		}
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x000957BC File Offset: 0x00093BBC
	private static void Reverse(double[] data, int n)
	{
		int i = 0;
		int num = 0;
		int num2 = n / 2;
		for (;;)
		{
			double num3 = data[i + 2];
			data[i + 2] = data[num + n];
			data[num + n] = num3;
			num3 = data[i + 3];
			data[i + 3] = data[num + n + 1];
			data[num + n + 1] = num3;
			if (i > num)
			{
				num3 = data[i];
				data[i] = data[num];
				data[num] = num3;
				num3 = data[i + 1];
				data[i + 1] = data[num + 1];
				data[num + 1] = num3;
				num3 = data[i + n + 2];
				data[i + n + 2] = data[num + n + 2];
				data[num + n + 2] = num3;
				num3 = data[i + n + 3];
				data[i + n + 3] = data[num + n + 3];
				data[num + n + 3] = num3;
			}
			num += 4;
			if (num >= n)
			{
				break;
			}
			int num4 = num2;
			while (i >= num4)
			{
				i -= num4;
				num4 /= 2;
			}
			i += num4;
		}
	}

	// Token: 0x040015B9 RID: 5561
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int <A>k__BackingField;

	// Token: 0x040015BA RID: 5562
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private int <B>k__BackingField;

	// Token: 0x040015BB RID: 5563
	private double[] cosTable;

	// Token: 0x040015BC RID: 5564
	private double[] sinTable;
}
