using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200007B RID: 123
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x0000EB46 File Offset: 0x0000CF46
		public MotionBlurModel()
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000EB59 File Offset: 0x0000CF59
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000EB61 File Offset: 0x0000CF61
		public MotionBlurModel.Settings settings
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

		// Token: 0x060001CC RID: 460 RVA: 0x0000EB6A File Offset: 0x0000CF6A
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x040002AC RID: 684
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x0200007C RID: 124
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005C RID: 92
			// (get) Token: 0x060001CD RID: 461 RVA: 0x0000EB78 File Offset: 0x0000CF78
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x040002AD RID: 685
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x040002AE RID: 686
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x040002AF RID: 687
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
