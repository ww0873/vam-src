using System;

namespace GPUTools.Hair.Scripts.Settings.Abstract
{
	// Token: 0x02000A23 RID: 2595
	[Serializable]
	public class HairSettingsBase
	{
		// Token: 0x06004320 RID: 17184 RVA: 0x0013B4D7 File Offset: 0x001398D7
		public HairSettingsBase()
		{
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x0013B4DF File Offset: 0x001398DF
		public virtual bool Validate()
		{
			return true;
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x0013B4E2 File Offset: 0x001398E2
		public virtual void DrawGizmos()
		{
		}

		// Token: 0x040031D9 RID: 12761
		public bool IsVisible;
	}
}
