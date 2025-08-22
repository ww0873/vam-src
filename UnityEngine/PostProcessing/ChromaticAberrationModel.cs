using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x0600019A RID: 410 RVA: 0x0000E352 File Offset: 0x0000C752
		public ChromaticAberrationModel()
		{
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000E365 File Offset: 0x0000C765
		// (set) Token: 0x0600019C RID: 412 RVA: 0x0000E36D File Offset: 0x0000C76D
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x0600019D RID: 413 RVA: 0x0000E376 File Offset: 0x0000C776
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x04000251 RID: 593
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x02000063 RID: 99
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000045 RID: 69
			// (get) Token: 0x0600019E RID: 414 RVA: 0x0000E384 File Offset: 0x0000C784
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x04000252 RID: 594
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x04000253 RID: 595
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
