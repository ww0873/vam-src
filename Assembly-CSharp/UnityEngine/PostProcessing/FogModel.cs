using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000077 RID: 119
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0000EA83 File Offset: 0x0000CE83
		public FogModel()
		{
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000EA96 File Offset: 0x0000CE96
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000EA9E File Offset: 0x0000CE9E
		public FogModel.Settings settings
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

		// Token: 0x060001C2 RID: 450 RVA: 0x0000EAA7 File Offset: 0x0000CEA7
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x040002A5 RID: 677
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x02000078 RID: 120
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000058 RID: 88
			// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000EAB4 File Offset: 0x0000CEB4
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x040002A6 RID: 678
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
