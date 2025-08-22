using System;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public static class Util
{
	// Token: 0x06001B0F RID: 6927 RVA: 0x000961B0 File Offset: 0x000945B0
	public static void SpectrumMagnitude(float[] spectrum, float[] output)
	{
		for (int i = 0; i < output.Length; i++)
		{
			int num = i * 2 + 2;
			output[i] = Mathf.Sqrt(spectrum[num] * spectrum[num] + spectrum[num + 1] * spectrum[num + 1]);
		}
	}

	// Token: 0x06001B10 RID: 6928 RVA: 0x000961F4 File Offset: 0x000945F4
	public static void SpectrumMagnitude(double[] spectrum, float[] output)
	{
		for (int i = 0; i < output.Length; i++)
		{
			int num = i * 2 + 2;
			float num2 = (float)spectrum[num];
			float num3 = (float)spectrum[num + 1];
			output[i] = Mathf.Sqrt(num2 * num2 + num3 * num3);
		}
	}

	// Token: 0x06001B11 RID: 6929 RVA: 0x00096238 File Offset: 0x00094638
	public static void FloatsToDoubles(float[] input, double[] output)
	{
		for (int i = 0; i < input.Length; i++)
		{
			output[i] = (double)input[i];
		}
	}

	// Token: 0x06001B12 RID: 6930 RVA: 0x00096260 File Offset: 0x00094660
	public static void DoublesToFloats(double[] input, float[] output)
	{
		for (int i = 0; i < input.Length; i++)
		{
			output[i] = (float)input[i];
		}
	}
}
