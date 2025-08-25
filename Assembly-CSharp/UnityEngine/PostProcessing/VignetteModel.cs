using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000086 RID: 134
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000ED0E File Offset: 0x0000D10E
		public VignetteModel()
		{
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000ED21 File Offset: 0x0000D121
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000ED29 File Offset: 0x0000D129
		public VignetteModel.Settings settings
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

		// Token: 0x060001DB RID: 475 RVA: 0x0000ED32 File Offset: 0x0000D132
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x040002CA RID: 714
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x02000087 RID: 135
		public enum Mode
		{
			// Token: 0x040002CC RID: 716
			Classic,
			// Token: 0x040002CD RID: 717
			Masked
		}

		// Token: 0x02000088 RID: 136
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000062 RID: 98
			// (get) Token: 0x060001DC RID: 476 RVA: 0x0000ED40 File Offset: 0x0000D140
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x040002CE RID: 718
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x040002CF RID: 719
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x040002D0 RID: 720
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x040002D1 RID: 721
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x040002D2 RID: 722
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x040002D3 RID: 723
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x040002D4 RID: 724
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x040002D5 RID: 725
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x040002D6 RID: 726
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
