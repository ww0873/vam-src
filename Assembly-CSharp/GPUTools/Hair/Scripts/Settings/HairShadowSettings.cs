using System;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
	// Token: 0x02000A2C RID: 2604
	[Serializable]
	public class HairShadowSettings : HairSettingsBase
	{
		// Token: 0x06004339 RID: 17209 RVA: 0x0013BC20 File Offset: 0x0013A020
		public HairShadowSettings()
		{
		}

		// Token: 0x04003243 RID: 12867
		[SerializeField]
		public bool CastShadows = true;

		// Token: 0x04003244 RID: 12868
		[SerializeField]
		public bool ReseiveShadows = true;
	}
}
