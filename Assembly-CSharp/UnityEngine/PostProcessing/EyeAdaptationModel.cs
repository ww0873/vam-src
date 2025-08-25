using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000074 RID: 116
	[Serializable]
	public class EyeAdaptationModel : PostProcessingModel
	{
		// Token: 0x060001BA RID: 442 RVA: 0x0000E9C6 File Offset: 0x0000CDC6
		public EyeAdaptationModel()
		{
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000E9D9 File Offset: 0x0000CDD9
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0000E9E1 File Offset: 0x0000CDE1
		public EyeAdaptationModel.Settings settings
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

		// Token: 0x060001BD RID: 445 RVA: 0x0000E9EA File Offset: 0x0000CDEA
		public override void Reset()
		{
			this.m_Settings = EyeAdaptationModel.Settings.defaultSettings;
		}

		// Token: 0x04000296 RID: 662
		[SerializeField]
		private EyeAdaptationModel.Settings m_Settings = EyeAdaptationModel.Settings.defaultSettings;

		// Token: 0x02000075 RID: 117
		public enum EyeAdaptationType
		{
			// Token: 0x04000298 RID: 664
			Progressive,
			// Token: 0x04000299 RID: 665
			Fixed
		}

		// Token: 0x02000076 RID: 118
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000056 RID: 86
			// (get) Token: 0x060001BE RID: 446 RVA: 0x0000E9F8 File Offset: 0x0000CDF8
			public static EyeAdaptationModel.Settings defaultSettings
			{
				get
				{
					return new EyeAdaptationModel.Settings
					{
						lowPercent = 45f,
						highPercent = 95f,
						minLuminance = -5f,
						maxLuminance = 1f,
						keyValue = 0.25f,
						dynamicKeyValue = true,
						adaptationType = EyeAdaptationModel.EyeAdaptationType.Progressive,
						speedUp = 2f,
						speedDown = 1f,
						logMin = -8,
						logMax = 4
					};
				}
			}

			// Token: 0x0400029A RID: 666
			[Range(1f, 99f)]
			[Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float lowPercent;

			// Token: 0x0400029B RID: 667
			[Range(1f, 99f)]
			[Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			public float highPercent;

			// Token: 0x0400029C RID: 668
			[Tooltip("Minimum average luminance to consider for auto exposure (in EV).")]
			public float minLuminance;

			// Token: 0x0400029D RID: 669
			[Tooltip("Maximum average luminance to consider for auto exposure (in EV).")]
			public float maxLuminance;

			// Token: 0x0400029E RID: 670
			[Min(0f)]
			[Tooltip("Exposure bias. Use this to offset the global exposure of the scene.")]
			public float keyValue;

			// Token: 0x0400029F RID: 671
			[Tooltip("Set this to true to let Unity handle the key value automatically based on average luminance.")]
			public bool dynamicKeyValue;

			// Token: 0x040002A0 RID: 672
			[Tooltip("Use \"Progressive\" if you want the auto exposure to be animated. Use \"Fixed\" otherwise.")]
			public EyeAdaptationModel.EyeAdaptationType adaptationType;

			// Token: 0x040002A1 RID: 673
			[Min(0f)]
			[Tooltip("Adaptation speed from a dark to a light environment.")]
			public float speedUp;

			// Token: 0x040002A2 RID: 674
			[Min(0f)]
			[Tooltip("Adaptation speed from a light to a dark environment.")]
			public float speedDown;

			// Token: 0x040002A3 RID: 675
			[Range(-16f, -1f)]
			[Tooltip("Lower bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			public int logMin;

			// Token: 0x040002A4 RID: 676
			[Range(1f, 16f)]
			[Tooltip("Upper bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			public int logMax;
		}
	}
}
