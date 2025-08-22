using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000051 RID: 81
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		// Token: 0x0600017F RID: 383 RVA: 0x0000DDA7 File Offset: 0x0000C1A7
		public AntialiasingModel()
		{
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000DDBA File Offset: 0x0000C1BA
		// (set) Token: 0x06000181 RID: 385 RVA: 0x0000DDC2 File Offset: 0x0000C1C2
		public AntialiasingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000DDCB File Offset: 0x0000C1CB
		public override void Reset()
		{
			this.m_Settings = AntialiasingModel.Settings.defaultSettings;
		}

		// Token: 0x04000216 RID: 534
		[SerializeField]
		private AntialiasingModel.Settings m_Settings = AntialiasingModel.Settings.defaultSettings;

		// Token: 0x02000052 RID: 82
		public enum Method
		{
			// Token: 0x04000218 RID: 536
			Fxaa,
			// Token: 0x04000219 RID: 537
			Taa
		}

		// Token: 0x02000053 RID: 83
		public enum FxaaPreset
		{
			// Token: 0x0400021B RID: 539
			ExtremePerformance,
			// Token: 0x0400021C RID: 540
			Performance,
			// Token: 0x0400021D RID: 541
			Default,
			// Token: 0x0400021E RID: 542
			Quality,
			// Token: 0x0400021F RID: 543
			ExtremeQuality
		}

		// Token: 0x02000054 RID: 84
		[Serializable]
		public struct FxaaQualitySettings
		{
			// Token: 0x06000183 RID: 387 RVA: 0x0000DDD8 File Offset: 0x0000C1D8
			// Note: this type is marked as 'beforefieldinit'.
			static FxaaQualitySettings()
			{
			}

			// Token: 0x04000220 RID: 544
			[Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")]
			[Range(0f, 1f)]
			public float subpixelAliasingRemovalAmount;

			// Token: 0x04000221 RID: 545
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.063f, 0.333f)]
			public float edgeDetectionThreshold;

			// Token: 0x04000222 RID: 546
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0f, 0.0833f)]
			public float minimumRequiredLuminance;

			// Token: 0x04000223 RID: 547
			public static AntialiasingModel.FxaaQualitySettings[] presets = new AntialiasingModel.FxaaQualitySettings[]
			{
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0f,
					edgeDetectionThreshold = 0.333f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.25f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.75f,
					edgeDetectionThreshold = 0.166f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.0625f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.063f,
					minimumRequiredLuminance = 0.0312f
				}
			};
		}

		// Token: 0x02000055 RID: 85
		[Serializable]
		public struct FxaaConsoleSettings
		{
			// Token: 0x06000184 RID: 388 RVA: 0x0000DF10 File Offset: 0x0000C310
			// Note: this type is marked as 'beforefieldinit'.
			static FxaaConsoleSettings()
			{
			}

			// Token: 0x04000224 RID: 548
			[Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")]
			[Range(0.33f, 0.5f)]
			public float subpixelSpreadAmount;

			// Token: 0x04000225 RID: 549
			[Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")]
			[Range(2f, 8f)]
			public float edgeSharpnessAmount;

			// Token: 0x04000226 RID: 550
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.125f, 0.25f)]
			public float edgeDetectionThreshold;

			// Token: 0x04000227 RID: 551
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0.04f, 0.06f)]
			public float minimumRequiredLuminance;

			// Token: 0x04000228 RID: 552
			public static AntialiasingModel.FxaaConsoleSettings[] presets = new AntialiasingModel.FxaaConsoleSettings[]
			{
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.05f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 4f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 2f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				}
			};
		}

		// Token: 0x02000056 RID: 86
		[Serializable]
		public struct FxaaSettings
		{
			// Token: 0x17000037 RID: 55
			// (get) Token: 0x06000185 RID: 389 RVA: 0x0000E084 File Offset: 0x0000C484
			public static AntialiasingModel.FxaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.FxaaSettings
					{
						preset = AntialiasingModel.FxaaPreset.Default
					};
				}
			}

			// Token: 0x04000229 RID: 553
			public AntialiasingModel.FxaaPreset preset;
		}

		// Token: 0x02000057 RID: 87
		[Serializable]
		public struct TaaSettings
		{
			// Token: 0x17000038 RID: 56
			// (get) Token: 0x06000186 RID: 390 RVA: 0x0000E0A4 File Offset: 0x0000C4A4
			public static AntialiasingModel.TaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.TaaSettings
					{
						jitterSpread = 0.75f,
						sharpen = 0.3f,
						stationaryBlending = 0.95f,
						motionBlending = 0.85f
					};
				}
			}

			// Token: 0x0400022A RID: 554
			[Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
			[Range(0.1f, 1f)]
			public float jitterSpread;

			// Token: 0x0400022B RID: 555
			[Tooltip("Controls the amount of sharpening applied to the color buffer.")]
			[Range(0f, 3f)]
			public float sharpen;

			// Token: 0x0400022C RID: 556
			[Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float stationaryBlending;

			// Token: 0x0400022D RID: 557
			[Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float motionBlending;
		}

		// Token: 0x02000058 RID: 88
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000039 RID: 57
			// (get) Token: 0x06000187 RID: 391 RVA: 0x0000E0EC File Offset: 0x0000C4EC
			public static AntialiasingModel.Settings defaultSettings
			{
				get
				{
					return new AntialiasingModel.Settings
					{
						method = AntialiasingModel.Method.Fxaa,
						fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings,
						taaSettings = AntialiasingModel.TaaSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400022E RID: 558
			public AntialiasingModel.Method method;

			// Token: 0x0400022F RID: 559
			public AntialiasingModel.FxaaSettings fxaaSettings;

			// Token: 0x04000230 RID: 560
			public AntialiasingModel.TaaSettings taaSettings;
		}
	}
}
