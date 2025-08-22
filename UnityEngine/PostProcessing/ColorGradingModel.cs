using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000064 RID: 100
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x0600019F RID: 415 RVA: 0x0000E3AE File Offset: 0x0000C7AE
		public ColorGradingModel()
		{
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000E3C1 File Offset: 0x0000C7C1
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000E3C9 File Offset: 0x0000C7C9
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000E3D8 File Offset: 0x0000C7D8
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000E3E0 File Offset: 0x0000C7E0
		public bool isDirty
		{
			[CompilerGenerated]
			get
			{
				return this.<isDirty>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<isDirty>k__BackingField = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000E3E9 File Offset: 0x0000C7E9
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000E3F1 File Offset: 0x0000C7F1
		public RenderTexture bakedLut
		{
			[CompilerGenerated]
			get
			{
				return this.<bakedLut>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<bakedLut>k__BackingField = value;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000E3FA File Offset: 0x0000C7FA
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000E40D File Offset: 0x0000C80D
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x04000254 RID: 596
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x04000255 RID: 597
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <isDirty>k__BackingField;

		// Token: 0x04000256 RID: 598
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private RenderTexture <bakedLut>k__BackingField;

		// Token: 0x02000065 RID: 101
		public enum Tonemapper
		{
			// Token: 0x04000258 RID: 600
			None,
			// Token: 0x04000259 RID: 601
			ACES,
			// Token: 0x0400025A RID: 602
			Neutral
		}

		// Token: 0x02000066 RID: 102
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17000049 RID: 73
			// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000E418 File Offset: 0x0000C818
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x0400025B RID: 603
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x0400025C RID: 604
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x0400025D RID: 605
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x0400025E RID: 606
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x0400025F RID: 607
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x04000260 RID: 608
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x04000261 RID: 609
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x02000067 RID: 103
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x1700004A RID: 74
			// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000E480 File Offset: 0x0000C880
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x04000262 RID: 610
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x04000263 RID: 611
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x04000264 RID: 612
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x04000265 RID: 613
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x04000266 RID: 614
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x04000267 RID: 615
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x02000068 RID: 104
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x1700004B RID: 75
			// (get) Token: 0x060001AA RID: 426 RVA: 0x0000E4E0 File Offset: 0x0000C8E0
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x04000268 RID: 616
			public Vector3 red;

			// Token: 0x04000269 RID: 617
			public Vector3 green;

			// Token: 0x0400026A RID: 618
			public Vector3 blue;

			// Token: 0x0400026B RID: 619
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x02000069 RID: 105
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x1700004C RID: 76
			// (get) Token: 0x060001AB RID: 427 RVA: 0x0000E550 File Offset: 0x0000C950
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x0400026C RID: 620
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x0400026D RID: 621
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x0400026E RID: 622
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x0200006A RID: 106
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x1700004D RID: 77
			// (get) Token: 0x060001AC RID: 428 RVA: 0x0000E58C File Offset: 0x0000C98C
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x0400026F RID: 623
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x04000270 RID: 624
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x04000271 RID: 625
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x0200006B RID: 107
		public enum ColorWheelMode
		{
			// Token: 0x04000273 RID: 627
			Linear,
			// Token: 0x04000274 RID: 628
			Log
		}

		// Token: 0x0200006C RID: 108
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x1700004E RID: 78
			// (get) Token: 0x060001AD RID: 429 RVA: 0x0000E5C8 File Offset: 0x0000C9C8
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000275 RID: 629
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x04000276 RID: 630
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x04000277 RID: 631
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x0200006D RID: 109
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060001AE RID: 430 RVA: 0x0000E600 File Offset: 0x0000CA00
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x04000278 RID: 632
			public ColorGradingCurve master;

			// Token: 0x04000279 RID: 633
			public ColorGradingCurve red;

			// Token: 0x0400027A RID: 634
			public ColorGradingCurve green;

			// Token: 0x0400027B RID: 635
			public ColorGradingCurve blue;

			// Token: 0x0400027C RID: 636
			public ColorGradingCurve hueVShue;

			// Token: 0x0400027D RID: 637
			public ColorGradingCurve hueVSsat;

			// Token: 0x0400027E RID: 638
			public ColorGradingCurve satVSsat;

			// Token: 0x0400027F RID: 639
			public ColorGradingCurve lumVSsat;

			// Token: 0x04000280 RID: 640
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x04000281 RID: 641
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x04000282 RID: 642
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04000283 RID: 643
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x04000284 RID: 644
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x0200006E RID: 110
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060001AF RID: 431 RVA: 0x0000E8B0 File Offset: 0x0000CCB0
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000285 RID: 645
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x04000286 RID: 646
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x04000287 RID: 647
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x04000288 RID: 648
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x04000289 RID: 649
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
