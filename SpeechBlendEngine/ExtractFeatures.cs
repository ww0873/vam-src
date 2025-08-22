using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SpeechBlendEngine
{
	// Token: 0x02000442 RID: 1090
	public class ExtractFeatures
	{
		// Token: 0x06001B1D RID: 6941 RVA: 0x00096C40 File Offset: 0x00095040
		public ExtractFeatures()
		{
			this.featureOutput = new ExtractFeatures.FeatureOutput(ExtractFeatures.NI);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00096C58 File Offset: 0x00095058
		public static float CalculateFres()
		{
			return (float)((double)ExtractFeatures.fs / (double)ExtractFeatures.N_freq_bins / 2.0);
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x00096C74 File Offset: 0x00095074
		private static float[] MeanNormalization(float[] spectrum)
		{
			float num = 0f;
			for (int i = 0; i < spectrum.Length; i++)
			{
				num += spectrum[i];
			}
			for (int j = 0; j < spectrum.Length; j++)
			{
				spectrum[j] /= num;
			}
			return spectrum;
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x00096CC4 File Offset: 0x000950C4
		public static float[] CreateCC_lifter(SpeechUtil.Accuracy accuracy)
		{
			int num = ExtractFeatures.C[(int)accuracy] + 1;
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (float)(1.0 + 0.5 * (double)ExtractFeatures.L * (double)Mathf.Sin(3.1415927f * (float)i / (float)ExtractFeatures.L));
			}
			return array;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00096D2C File Offset: 0x0009512C
		public static float[,] BuildExtractor(float frequency_resolution, int LF, int HF, SpeechUtil.Accuracy accuracy)
		{
			float[] array = new float[ExtractFeatures.N_freq_bins];
			for (int i = 0; i < ExtractFeatures.N_freq_bins; i++)
			{
				array[i] = (float)i * frequency_resolution;
			}
			float num = ExtractFeatures.fs / 2f;
			float[] array2 = new float[ExtractFeatures.N_freq_bins];
			for (int j = 0; j < ExtractFeatures.N_freq_bins; j++)
			{
				array2[j] = (float)j * num / (float)ExtractFeatures.N_freq_bins;
			}
			int[] array3 = new int[ExtractFeatures.M_FB[(int)accuracy] + 2];
			float num2 = (float)(((double)ExtractFeatures.Hertz2Mel((float)HF) - (double)ExtractFeatures.Hertz2Mel((float)LF)) / ((double)ExtractFeatures.M_FB[(int)accuracy] + 1.0));
			for (int k = 0; k < ExtractFeatures.M_FB[(int)accuracy] + 2; k++)
			{
				float f = ExtractFeatures.Mel2Hertz(ExtractFeatures.Hertz2Mel((float)LF) + (float)k * num2);
				array3[k] = Mathf.RoundToInt(f);
			}
			float[,] array4 = new float[ExtractFeatures.M_FB[(int)accuracy], ExtractFeatures.N_freq_bins];
			for (int l = 0; l < ExtractFeatures.M_FB[(int)accuracy]; l++)
			{
				for (int m = 0; m < ExtractFeatures.N_freq_bins; m++)
				{
					array4[l, m] = 0f;
				}
			}
			for (int n = 0; n < ExtractFeatures.M_FB[(int)accuracy]; n++)
			{
				for (int num3 = 0; num3 < ExtractFeatures.N_freq_bins; num3++)
				{
					if ((double)array2[num3] > (double)array3[n] & (double)array2[num3] < (double)array3[n + 1])
					{
						array4[n, num3] = (float)(((double)array2[num3] - (double)array3[n]) / ((double)array3[n + 1] - (double)array3[n]));
					}
					else if ((double)array2[num3] > (double)array3[n + 1] & (double)array2[num3] < (double)array3[n + 2])
					{
						array4[n, num3] = (float)(((double)array3[n + 2] - (double)array2[num3]) / ((double)array3[n + 2] - (double)array3[n + 1]));
					}
				}
			}
			return array4;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x00096F48 File Offset: 0x00095348
		public static float Mel2Hertz(float mel)
		{
			return (float)(700.0 * (double)Mathf.Exp(mel / 1127f) - 700.0);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00096F6C File Offset: 0x0009536C
		public static float Hertz2Mel(float hertz)
		{
			return 1127f * Mathf.Log((float)(1.0 + (double)hertz / 700.0));
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00096F90 File Offset: 0x00095390
		public static float[,] GenerateTransformer(SpeechUtil.Accuracy accuracy)
		{
			int num = ExtractFeatures.getC(accuracy) + 1;
			float[,] array = new float[num, ExtractFeatures.M_FB[(int)accuracy]];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < ExtractFeatures.M_FB[(int)accuracy]; j++)
				{
					float num2 = (float)i;
					float num3 = (float)(3.1415927410125732 * ((double)((float)j + 1f) - 0.5)) / (float)ExtractFeatures.M_FB[(int)accuracy];
					array[i, j] = Mathf.Sqrt(2f / (float)ExtractFeatures.M_FB[(int)accuracy]) * Mathf.Cos(num2 * num3);
				}
			}
			return array;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00097032 File Offset: 0x00095432
		public static int getC(SpeechUtil.Accuracy accuracy)
		{
			return ExtractFeatures.C[(int)accuracy];
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0009703B File Offset: 0x0009543B
		public static int getlf(SpeechUtil.Accuracy accuracy)
		{
			return ExtractFeatures.lf[(int)accuracy];
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00097044 File Offset: 0x00095444
		public static int gethf(SpeechUtil.Accuracy accuracy)
		{
			return ExtractFeatures.hf[(int)accuracy];
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x00097050 File Offset: 0x00095450
		public static float EqualizeDistance(float volume, AudioSource voiceAudioSource, AudioListener activeListener)
		{
			if ((double)voiceAudioSource.spatialBlend == 0.0)
			{
				return volume;
			}
			if (activeListener == null)
			{
				activeListener = Camera.allCameras[0].gameObject.GetComponent<AudioListener>();
			}
			float num = Vector3.Distance(activeListener.transform.position, voiceAudioSource.transform.position);
			float num2 = 0f;
			AudioRolloffMode rolloffMode = voiceAudioSource.rolloffMode;
			if (rolloffMode != AudioRolloffMode.Logarithmic)
			{
				if (rolloffMode != AudioRolloffMode.Linear)
				{
					if (rolloffMode == AudioRolloffMode.Custom)
					{
						if (num <= voiceAudioSource.minDistance || num == 0f)
						{
							num2 = 1f;
						}
						else if (num > voiceAudioSource.maxDistance)
						{
							num2 = 0f;
						}
						else
						{
							num2 = voiceAudioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff).Evaluate(num / voiceAudioSource.maxDistance);
						}
					}
				}
				else if (num <= voiceAudioSource.minDistance || num == 0f)
				{
					num2 = 1f;
				}
				else if (num > voiceAudioSource.maxDistance)
				{
					num2 = 0f;
				}
				else
				{
					num2 = Mathf.Lerp(1f, 0f, (num - voiceAudioSource.minDistance) / (voiceAudioSource.maxDistance - voiceAudioSource.minDistance));
				}
			}
			else if (num <= voiceAudioSource.minDistance || num == 0f)
			{
				num2 = 1f;
			}
			else
			{
				float num3 = voiceAudioSource.minDistance / num;
				num2 = num3;
			}
			if (num2 == 1f)
			{
				return volume;
			}
			if (num2 >= 0.01f)
			{
				float b = volume / num2;
				return Mathf.Lerp(volume, b, voiceAudioSource.spatialBlend);
			}
			return Mathf.Lerp(volume, 0f, voiceAudioSource.spatialBlend);
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x000971FB File Offset: 0x000955FB
		// (set) Token: 0x06001B2A RID: 6954 RVA: 0x00097203 File Offset: 0x00095603
		public ExtractFeatures.FeatureOutput featureOutput
		{
			[CompilerGenerated]
			get
			{
				return this.<featureOutput>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<featureOutput>k__BackingField = value;
			}
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0009720C File Offset: 0x0009560C
		private void InitSpectrumData()
		{
			if (this.spectrumData == null || this.spectrumData.Length != ExtractFeatures.N_freq_bins)
			{
				this.spectrumData = new float[ExtractFeatures.N_freq_bins];
			}
			else
			{
				for (int i = 0; i < this.spectrumData.Length; i++)
				{
					this.spectrumData[i] = 0f;
				}
			}
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x00097271 File Offset: 0x00095671
		public float[] GetUnitySpectrumData(AudioSource source, bool useHamming = true)
		{
			this.InitSpectrumData();
			if (useHamming)
			{
				source.GetSpectrumData(this.spectrumData, 0, FFTWindow.Hamming);
			}
			else
			{
				source.GetSpectrumData(this.spectrumData, 0, FFTWindow.Rectangular);
			}
			return this.spectrumData;
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x000972A8 File Offset: 0x000956A8
		private void InitSoundData(int numChannels)
		{
			int num = ExtractFeatures.N_freq_bins * 2;
			if (this.getData == null || this.getData.Length != numChannels * num)
			{
				this.getData = new float[numChannels * num];
			}
			if (this.soundData == null || this.soundData.Length != num)
			{
				this.soundData = new float[num];
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0009730C File Offset: 0x0009570C
		public float[] GetUnitySoundData(AudioSource source, float timeOffset)
		{
			int channels = source.clip.channels;
			this.InitSoundData(channels);
			float num = source.time + timeOffset;
			int num2 = Mathf.RoundToInt(num * (float)source.clip.frequency);
			bool flag = false;
			if (source.loop)
			{
				if (num >= source.clip.length)
				{
					while (num >= source.clip.length)
					{
						num -= source.clip.length;
					}
					num2 = Mathf.RoundToInt(num * (float)source.clip.frequency);
				}
			}
			else if (num2 + ExtractFeatures.N_freq_bins * 2 > source.clip.samples)
			{
				flag = true;
				for (int i = 0; i < this.soundData.Length; i++)
				{
					this.soundData[i] = 0f;
				}
			}
			if (!flag)
			{
				source.clip.GetData(this.getData, num2);
				for (int j = 0; j < this.soundData.Length; j++)
				{
					this.soundData[j] = this.getData[j * channels];
				}
			}
			return this.soundData;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0009743C File Offset: 0x0009583C
		public float[] ConvertSoundDataToSpectrumData(bool useHamming = true)
		{
			int num = ExtractFeatures.N_freq_bins * 2;
			this.InitSpectrumData();
			if (this.lomontFFT == null)
			{
				this.lomontFFT = new LomontFFT();
			}
			if (this.lomontFFTData == null || this.lomontFFTData.Length != num)
			{
				this.lomontFFTData = new double[num];
			}
			int num2 = this.soundData.Length;
			if (useHamming)
			{
				for (int i = 0; i < num2; i++)
				{
					float num3 = 0.54f - 0.46f * Mathf.Cos(6.2831855f * (float)i / (float)num2);
					this.lomontFFTData[i] = (double)(this.soundData[i] * num3);
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					this.lomontFFTData[j] = (double)this.soundData[j];
				}
			}
			this.lomontFFT.TableFFT(this.lomontFFTData, true);
			for (int k = 0; k < this.spectrumData.Length; k++)
			{
				int num4 = k * 2;
				float num5 = (float)this.lomontFFTData[num4];
				float num6 = (float)this.lomontFFTData[num4 + 1];
				this.spectrumData[k] = Mathf.Sqrt(num5 * num5 + num6 * num6);
			}
			return this.spectrumData;
		}

		// Token: 0x06001B30 RID: 6960 RVA: 0x00097580 File Offset: 0x00095980
		public float GetVolume(float[] data)
		{
			float num = 0f;
			for (int i = 0; i < data.Length; i++)
			{
				num += Mathf.Abs(data[i]);
			}
			return num / (float)data.Length;
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x000975BC File Offset: 0x000959BC
		public float[] ExtractSample(float[] spectrum, float[,] filterBanks, float[,] DCT_mat, float[] cep_lifter, ref float[,] cc_mem, int f_low, int f_high, SpeechUtil.Accuracy accuracy)
		{
			int num = ExtractFeatures.M_FB[(int)accuracy];
			int num2 = ExtractFeatures.C[(int)accuracy];
			float num3 = -1f;
			for (int i = 0; i < spectrum.Length; i++)
			{
				if ((double)spectrum[i] > (double)num3)
				{
					num3 = spectrum[i];
				}
			}
			for (int j = 0; j < spectrum.Length; j++)
			{
				spectrum[j] /= num3;
			}
			if (this.FB_energy_log == null || this.FB_energy_log.Length != num)
			{
				this.FB_energy_log = new float[num];
			}
			for (int k = 0; k < num; k++)
			{
				this.FB_energy_log[k] = 0f;
				for (int l = f_low; l < f_high; l++)
				{
					this.FB_energy_log[k] += filterBanks[k, l] * Mathf.Abs(spectrum[l]);
				}
				this.FB_energy_log[k] = Mathf.Log(this.FB_energy_log[k]);
			}
			return this.CepstralCoefficients(this.FB_energy_log, DCT_mat, ref cc_mem, cep_lifter, num2 + 1, num);
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x000976DC File Offset: 0x00095ADC
		private float[] CepstralCoefficients(float[] FB_energy_log, float[,] DCT_mat, ref float[,] cc_mem, float[] cep_lifter, int N, int M)
		{
			if (this.CCnumArray == null || this.CCnumArray.Length != N)
			{
				this.CCnumArray = new float[N];
			}
			if (this.CCcc == null || this.CCcc.Length != N)
			{
				this.CCcc = new float[N];
			}
			for (int i = 0; i < N; i++)
			{
				this.CCnumArray[i] = 0f;
				for (int j = 0; j < M; j++)
				{
					this.CCnumArray[i] += DCT_mat[i, j] * FB_energy_log[j];
				}
			}
			for (int k = 0; k < N; k++)
			{
				this.CCcc[k] = this.CCnumArray[k] * cep_lifter[k];
			}
			return this.DeltaCC(this.CCcc, ref cc_mem, N);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x000977C0 File Offset: 0x00095BC0
		private float[] DeltaCC(float[] cc, ref float[,] cc_mem, int N)
		{
			int num = N * 2;
			if (this.DeltaCCNumArray == null || this.DeltaCCNumArray.Length != num)
			{
				this.DeltaCCNumArray = new float[num];
			}
			for (int i = 0; i < N; i++)
			{
				this.DeltaCCNumArray[i] = cc[i];
			}
			for (int j = N; j < num; j++)
			{
				this.DeltaCCNumArray[j] = (float)(((double)cc[j - N] - (double)cc_mem[j - N, 1] + 2.0 * ((double)cc[j - N] - (double)cc_mem[j - N, 0])) / 8.0);
			}
			for (int k = 0; k < N; k++)
			{
				cc_mem[k, 0] = cc_mem[k, 1];
				cc_mem[k, 1] = cc[k];
			}
			return this.DeltaCCNumArray;
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x000978A0 File Offset: 0x00095CA0
		public ExtractFeatures.FeatureOutput Evaluate(float[] mfccs, VoiceProfile.VoiceType voiceType, SpeechUtil.Accuracy accuracy)
		{
			int num = (ExtractFeatures.C[(int)accuracy] + 1) * 2;
			int num2 = ExtractFeatures.no_phonemes[(int)accuracy];
			if (this.euclDistanceArray == null || this.euclDistanceArray.Length != num2)
			{
				this.euclDistanceArray = new ExtractFeatures.EuclideanDistance[num2];
			}
			for (int i = 0; i < num2; i++)
			{
				float num3 = 0f;
				for (int j = 0; j < num; j++)
				{
					num3 += Mathf.Pow(mfccs[j] - VoiceProfile.VQ_center(voiceType, j, i, accuracy), 2f);
				}
				this.euclDistanceArray[i] = new ExtractFeatures.EuclideanDistance
				{
					distance = Mathf.Sqrt(num3),
					number = i
				};
			}
			this.GetShortest(this.euclDistanceArray, ExtractFeatures.NI);
			this.InterpolateFeatures(this.euclDistanceArray);
			for (int k = 0; k < ExtractFeatures.NI; k++)
			{
				this.featureOutput.reg[k] = this.euclDistanceArray[k].number;
			}
			return this.featureOutput;
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x000979C0 File Offset: 0x00095DC0
		private int[] GetShortest(ExtractFeatures.EuclideanDistance[] euclDistanceArray, int N_list)
		{
			if (this.shortest == null || this.shortest.Length != N_list)
			{
				this.shortest = new int[N_list];
			}
			if (ExtractFeatures.<>f__am$cache0 == null)
			{
				ExtractFeatures.<>f__am$cache0 = new Comparison<ExtractFeatures.EuclideanDistance>(ExtractFeatures.<GetShortest>m__0);
			}
			Array.Sort<ExtractFeatures.EuclideanDistance>(euclDistanceArray, ExtractFeatures.<>f__am$cache0);
			int num = euclDistanceArray.Length;
			for (int i = 0; i < N_list; i++)
			{
				this.shortest[i] = euclDistanceArray[i].number;
			}
			return this.shortest;
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x00097A48 File Offset: 0x00095E48
		private void InterpolateFeatures(ExtractFeatures.EuclideanDistance[] ECD)
		{
			this.featureOutput.w[0] = 1f;
			float num = 1f;
			if ((double)ECD[0].distance > 5.0)
			{
				for (int i = 1; i < ExtractFeatures.NI; i++)
				{
					this.featureOutput.w[i] = (float)(1.0 / ((double)ECD[i].distance - (double)ECD[0].distance + 2.0)) * ExtractFeatures.ISF;
					num += this.featureOutput.w[i];
				}
				for (int j = 1; j < ExtractFeatures.NI; j++)
				{
					this.featureOutput.w[j] /= num;
				}
			}
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x00097B20 File Offset: 0x00095F20
		// Note: this type is marked as 'beforefieldinit'.
		static ExtractFeatures()
		{
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x00097BCD File Offset: 0x00095FCD
		[CompilerGenerated]
		private static int <GetShortest>m__0(ExtractFeatures.EuclideanDistance p1, ExtractFeatures.EuclideanDistance p2)
		{
			return p1.distance.CompareTo(p2.distance);
		}

		// Token: 0x040016C9 RID: 5833
		public static int no_visemes = 16;

		// Token: 0x040016CA RID: 5834
		private static int[] M_FB = new int[]
		{
			14,
			20,
			30
		};

		// Token: 0x040016CB RID: 5835
		private static int[] C = new int[]
		{
			10,
			12,
			16
		};

		// Token: 0x040016CC RID: 5836
		private static int L = 22;

		// Token: 0x040016CD RID: 5837
		private static int[] lf = new int[]
		{
			400,
			300,
			200
		};

		// Token: 0x040016CE RID: 5838
		private static int[] hf = new int[]
		{
			4200,
			4500,
			5000
		};

		// Token: 0x040016CF RID: 5839
		private static int[] no_phonemes = new int[]
		{
			16,
			32,
			64
		};

		// Token: 0x040016D0 RID: 5840
		private static int N_freq_bins = 4096;

		// Token: 0x040016D1 RID: 5841
		private static float fs = 48000f;

		// Token: 0x040016D2 RID: 5842
		public static int NI = 4;

		// Token: 0x040016D3 RID: 5843
		private static float ISF = 0.2f;

		// Token: 0x040016D4 RID: 5844
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ExtractFeatures.FeatureOutput <featureOutput>k__BackingField;

		// Token: 0x040016D5 RID: 5845
		private float[] spectrumData;

		// Token: 0x040016D6 RID: 5846
		private float[] getData;

		// Token: 0x040016D7 RID: 5847
		private float[] soundData;

		// Token: 0x040016D8 RID: 5848
		private LomontFFT lomontFFT;

		// Token: 0x040016D9 RID: 5849
		private double[] lomontFFTData;

		// Token: 0x040016DA RID: 5850
		private float[] FB_energy_log;

		// Token: 0x040016DB RID: 5851
		private float[] CCnumArray;

		// Token: 0x040016DC RID: 5852
		private float[] CCcc;

		// Token: 0x040016DD RID: 5853
		private float[] DeltaCCNumArray;

		// Token: 0x040016DE RID: 5854
		private ExtractFeatures.EuclideanDistance[] euclDistanceArray;

		// Token: 0x040016DF RID: 5855
		private int[] shortest;

		// Token: 0x040016E0 RID: 5856
		[CompilerGenerated]
		private static Comparison<ExtractFeatures.EuclideanDistance> <>f__am$cache0;

		// Token: 0x02000443 RID: 1091
		public class FeatureOutput
		{
			// Token: 0x06001B39 RID: 6969 RVA: 0x00097BE2 File Offset: 0x00095FE2
			public FeatureOutput(int sz)
			{
				this.reg = new int[sz];
				this.w = new float[sz];
				this.size = sz;
			}

			// Token: 0x040016E1 RID: 5857
			public int[] reg;

			// Token: 0x040016E2 RID: 5858
			public float[] w;

			// Token: 0x040016E3 RID: 5859
			public int size;
		}

		// Token: 0x02000444 RID: 1092
		private struct EuclideanDistance
		{
			// Token: 0x040016E4 RID: 5860
			public float distance;

			// Token: 0x040016E5 RID: 5861
			public int number;
		}
	}
}
