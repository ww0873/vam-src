using System;
using UnityEngine;

namespace UltimateGameTools.MeshSimplifier
{
	// Token: 0x0200047D RID: 1149
	[Serializable]
	public class RelevanceSphere
	{
		// Token: 0x06001D38 RID: 7480 RVA: 0x000A747F File Offset: 0x000A587F
		public RelevanceSphere()
		{
			this.m_v3Scale = Vector3.one;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000A7494 File Offset: 0x000A5894
		public void SetDefault(Transform target, float fRelevance)
		{
			this.m_bExpanded = true;
			this.m_v3Position = target.position + Vector3.up;
			this.m_v3Rotation = target.rotation.eulerAngles;
			this.m_v3Scale = Vector3.one;
			this.m_fRelevance = fRelevance;
		}

		// Token: 0x040018D3 RID: 6355
		public bool m_bExpanded;

		// Token: 0x040018D4 RID: 6356
		public Vector3 m_v3Position;

		// Token: 0x040018D5 RID: 6357
		public Vector3 m_v3Rotation;

		// Token: 0x040018D6 RID: 6358
		public Vector3 m_v3Scale;

		// Token: 0x040018D7 RID: 6359
		public float m_fRelevance;
	}
}
