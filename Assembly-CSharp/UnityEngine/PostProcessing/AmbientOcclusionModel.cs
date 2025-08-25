using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200004E RID: 78
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x0600017A RID: 378 RVA: 0x0000DD1F File Offset: 0x0000C11F
		public AmbientOcclusionModel()
		{
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000DD32 File Offset: 0x0000C132
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000DD3A File Offset: 0x0000C13A
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x0600017D RID: 381 RVA: 0x0000DD43 File Offset: 0x0000C143
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x04000209 RID: 521
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x0200004F RID: 79
		public enum SampleCount
		{
			// Token: 0x0400020B RID: 523
			Lowest = 3,
			// Token: 0x0400020C RID: 524
			Low = 6,
			// Token: 0x0400020D RID: 525
			Medium = 10,
			// Token: 0x0400020E RID: 526
			High = 16
		}

		// Token: 0x02000050 RID: 80
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000035 RID: 53
			// (get) Token: 0x0600017E RID: 382 RVA: 0x0000DD50 File Offset: 0x0000C150
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						forceForwardCompatibility = false,
						ambientOnly = false,
						highPrecision = false
					};
				}
			}

			// Token: 0x0400020F RID: 527
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x04000210 RID: 528
			[Min(0.0001f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x04000211 RID: 529
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x04000212 RID: 530
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x04000213 RID: 531
			[Tooltip("Forces compatibility with Forward rendered objects when working with the Deferred rendering path.")]
			public bool forceForwardCompatibility;

			// Token: 0x04000214 RID: 532
			[Tooltip("Enables the ambient-only mode in that the effect only affects ambient lighting. This mode is only available with the Deferred rendering path and HDR rendering.")]
			public bool ambientOnly;

			// Token: 0x04000215 RID: 533
			[Tooltip("Toggles the use of a higher precision depth texture with the forward rendering path (may impact performances). Has no effect with the deferred rendering path.")]
			public bool highPrecision;
		}
	}
}
