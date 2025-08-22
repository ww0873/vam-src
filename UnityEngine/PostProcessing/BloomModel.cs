using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000059 RID: 89
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000E122 File Offset: 0x0000C522
		public BloomModel()
		{
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000E135 File Offset: 0x0000C535
		// (set) Token: 0x0600018A RID: 394 RVA: 0x0000E13D File Offset: 0x0000C53D
		public BloomModel.Settings settings
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

		// Token: 0x0600018B RID: 395 RVA: 0x0000E146 File Offset: 0x0000C546
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x04000231 RID: 561
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x0200005A RID: 90
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x1700003B RID: 59
			// (get) Token: 0x0600018D RID: 397 RVA: 0x0000E161 File Offset: 0x0000C561
			// (set) Token: 0x0600018C RID: 396 RVA: 0x0000E153 File Offset: 0x0000C553
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x1700003C RID: 60
			// (get) Token: 0x0600018E RID: 398 RVA: 0x0000E170 File Offset: 0x0000C570
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x04000232 RID: 562
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x04000233 RID: 563
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x04000234 RID: 564
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x04000235 RID: 565
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04000236 RID: 566
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x0200005B RID: 91
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x1700003D RID: 61
			// (get) Token: 0x0600018F RID: 399 RVA: 0x0000E1C0 File Offset: 0x0000C5C0
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x04000237 RID: 567
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x04000238 RID: 568
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x0200005C RID: 92
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700003E RID: 62
			// (get) Token: 0x06000190 RID: 400 RVA: 0x0000E1EC File Offset: 0x0000C5EC
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000239 RID: 569
			public BloomModel.BloomSettings bloom;

			// Token: 0x0400023A RID: 570
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
