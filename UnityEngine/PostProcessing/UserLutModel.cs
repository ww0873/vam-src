using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000084 RID: 132
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x0000ECB2 File Offset: 0x0000D0B2
		public UserLutModel()
		{
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000ECC5 File Offset: 0x0000D0C5
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x0000ECCD File Offset: 0x0000D0CD
		public UserLutModel.Settings settings
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

		// Token: 0x060001D6 RID: 470 RVA: 0x0000ECD6 File Offset: 0x0000D0D6
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x040002C7 RID: 711
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x02000085 RID: 133
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000060 RID: 96
			// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000ECE4 File Offset: 0x0000D0E4
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x040002C8 RID: 712
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x040002C9 RID: 713
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
