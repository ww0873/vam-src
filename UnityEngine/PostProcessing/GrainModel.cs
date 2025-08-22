using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000079 RID: 121
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000EAD2 File Offset: 0x0000CED2
		public GrainModel()
		{
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000EAE5 File Offset: 0x0000CEE5
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x0000EAED File Offset: 0x0000CEED
		public GrainModel.Settings settings
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

		// Token: 0x060001C7 RID: 455 RVA: 0x0000EAF6 File Offset: 0x0000CEF6
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x040002A7 RID: 679
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x0200007A RID: 122
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005A RID: 90
			// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000EB04 File Offset: 0x0000CF04
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x040002A8 RID: 680
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x040002A9 RID: 681
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x040002AA RID: 682
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x040002AB RID: 683
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
