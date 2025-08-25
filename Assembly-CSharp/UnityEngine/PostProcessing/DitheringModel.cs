using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0000E97E File Offset: 0x0000CD7E
		public DitheringModel()
		{
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000E991 File Offset: 0x0000CD91
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000E999 File Offset: 0x0000CD99
		public DitheringModel.Settings settings
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

		// Token: 0x060001B8 RID: 440 RVA: 0x0000E9A2 File Offset: 0x0000CDA2
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x04000295 RID: 661
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x02000073 RID: 115
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000054 RID: 84
			// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000E9B0 File Offset: 0x0000CDB0
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
