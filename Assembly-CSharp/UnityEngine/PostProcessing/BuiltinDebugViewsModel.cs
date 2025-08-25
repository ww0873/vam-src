using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200005D RID: 93
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x06000191 RID: 401 RVA: 0x0000E21A File Offset: 0x0000C61A
		public BuiltinDebugViewsModel()
		{
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000E22D File Offset: 0x0000C62D
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000E235 File Offset: 0x0000C635
		public BuiltinDebugViewsModel.Settings settings
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000E23E File Offset: 0x0000C63E
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000E27E File Offset: 0x0000C67E
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000E28B File Offset: 0x0000C68B
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x0400023B RID: 571
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x0200005E RID: 94
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17000041 RID: 65
			// (get) Token: 0x06000197 RID: 407 RVA: 0x0000E29C File Offset: 0x0000C69C
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x0400023C RID: 572
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x0200005F RID: 95
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000042 RID: 66
			// (get) Token: 0x06000198 RID: 408 RVA: 0x0000E2C0 File Offset: 0x0000C6C0
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x0400023D RID: 573
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x0400023E RID: 574
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x0400023F RID: 575
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x04000240 RID: 576
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x04000241 RID: 577
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x04000242 RID: 578
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x02000060 RID: 96
		public enum Mode
		{
			// Token: 0x04000244 RID: 580
			None,
			// Token: 0x04000245 RID: 581
			Depth,
			// Token: 0x04000246 RID: 582
			Normals,
			// Token: 0x04000247 RID: 583
			MotionVectors,
			// Token: 0x04000248 RID: 584
			AmbientOcclusion,
			// Token: 0x04000249 RID: 585
			EyeAdaptation,
			// Token: 0x0400024A RID: 586
			FocusPlane,
			// Token: 0x0400024B RID: 587
			PreGradingLog,
			// Token: 0x0400024C RID: 588
			LogLut,
			// Token: 0x0400024D RID: 589
			UserLut
		}

		// Token: 0x02000061 RID: 97
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000043 RID: 67
			// (get) Token: 0x06000199 RID: 409 RVA: 0x0000E31C File Offset: 0x0000C71C
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400024E RID: 590
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x0400024F RID: 591
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x04000250 RID: 592
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
