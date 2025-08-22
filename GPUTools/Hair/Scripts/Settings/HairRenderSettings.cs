using System;
using GPUTools.Hair.Scripts.Settings.Abstract;
using GPUTools.Hair.Scripts.Settings.Colors;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
	// Token: 0x02000A2B RID: 2603
	[Serializable]
	public class HairRenderSettings : HairSettingsBase
	{
		// Token: 0x06004337 RID: 17207 RVA: 0x0013BA04 File Offset: 0x00139E04
		public HairRenderSettings()
		{
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06004338 RID: 17208 RVA: 0x0013BBF3 File Offset: 0x00139FF3
		public IColorProvider ColorProvider
		{
			get
			{
				if (this.ColorProviderType == ColorProviderType.RootTip)
				{
					return this.RootTipColorProvider;
				}
				if (this.ColorProviderType == ColorProviderType.List)
				{
					return this.ListColorProvider;
				}
				return this.GeometryColorProvider;
			}
		}

		// Token: 0x04003214 RID: 12820
		public ColorProviderType ColorProviderType;

		// Token: 0x04003215 RID: 12821
		public RootTipColorProvider RootTipColorProvider;

		// Token: 0x04003216 RID: 12822
		public ListColorProvider ListColorProvider;

		// Token: 0x04003217 RID: 12823
		public GeometryColorProvider GeometryColorProvider;

		// Token: 0x04003218 RID: 12824
		public Material material;

		// Token: 0x04003219 RID: 12825
		public float PrimarySpecular = 50f;

		// Token: 0x0400321A RID: 12826
		public float SecondarySpecular = 50f;

		// Token: 0x0400321B RID: 12827
		public Color SpecularColor = new Color(0.15f, 0.15f, 0.15f);

		// Token: 0x0400321C RID: 12828
		public float SpecularShift = 0.01f;

		// Token: 0x0400321D RID: 12829
		public float Diffuse = 0.75f;

		// Token: 0x0400321E RID: 12830
		public float FresnelPower = 10f;

		// Token: 0x0400321F RID: 12831
		public float FresnelAttenuation = 1f;

		// Token: 0x04003220 RID: 12832
		public float Length1 = 1f;

		// Token: 0x04003221 RID: 12833
		public float Length2 = 1f;

		// Token: 0x04003222 RID: 12834
		public float Length3 = 1f;

		// Token: 0x04003223 RID: 12835
		public bool UseWavinessCurves = true;

		// Token: 0x04003224 RID: 12836
		public float WavinessScale;

		// Token: 0x04003225 RID: 12837
		public AnimationCurve WavinessScaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04003226 RID: 12838
		public float WavinessFrequency;

		// Token: 0x04003227 RID: 12839
		public AnimationCurve WavinessFrequencyCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04003228 RID: 12840
		public Vector3 WavinessAxis = new Vector3(0.1f, 0f, 0.1f);

		// Token: 0x04003229 RID: 12841
		public float WavinessRoot = 1f;

		// Token: 0x0400322A RID: 12842
		public float WavinessMid = 1f;

		// Token: 0x0400322B RID: 12843
		public float WavinessTip = 1f;

		// Token: 0x0400322C RID: 12844
		public float WavinessMidpoint = 0.5f;

		// Token: 0x0400322D RID: 12845
		public float WavinessCurvePower = 1f;

		// Token: 0x0400322E RID: 12846
		public float WavinessScaleRandomness = 1f;

		// Token: 0x0400322F RID: 12847
		public float WavinessFrequencyRandomness = 1f;

		// Token: 0x04003230 RID: 12848
		public bool WavinessAllowReverse;

		// Token: 0x04003231 RID: 12849
		public bool WavinessAllowFlipAxis;

		// Token: 0x04003232 RID: 12850
		public float WavinessNormalAdjust;

		// Token: 0x04003233 RID: 12851
		public bool StyleModeShowCurls;

		// Token: 0x04003234 RID: 12852
		public bool UseInterpolationCurves = true;

		// Token: 0x04003235 RID: 12853
		public AnimationCurve InterpolationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);

		// Token: 0x04003236 RID: 12854
		public float InterpolationRoot = 1f;

		// Token: 0x04003237 RID: 12855
		public float InterpolationMid = 1f;

		// Token: 0x04003238 RID: 12856
		public float InterpolationTip = 1f;

		// Token: 0x04003239 RID: 12857
		public float InterpolationMidpoint = 0.5f;

		// Token: 0x0400323A RID: 12858
		public float InterpolationCurvePower = 1f;

		// Token: 0x0400323B RID: 12859
		public AnimationCurve WidthCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		// Token: 0x0400323C RID: 12860
		public float Volume;

		// Token: 0x0400323D RID: 12861
		public float BarycentricVolume = 0.015f;

		// Token: 0x0400323E RID: 12862
		public float RandomTexColorPower = 1f;

		// Token: 0x0400323F RID: 12863
		public float RandomTexColorOffset = 0.3f;

		// Token: 0x04003240 RID: 12864
		public float IBLFactor = 0.5f;

		// Token: 0x04003241 RID: 12865
		public float MaxSpread = 0.05f;

		// Token: 0x04003242 RID: 12866
		public float NormalRandomize;
	}
}
