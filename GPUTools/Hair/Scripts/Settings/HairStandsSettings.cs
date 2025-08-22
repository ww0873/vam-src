using System;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Hair.Scripts.Settings.Abstract;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Settings
{
	// Token: 0x02000A2E RID: 2606
	[Serializable]
	public class HairStandsSettings : HairSettingsBase
	{
		// Token: 0x0600433A RID: 17210 RVA: 0x0013BC36 File Offset: 0x0013A036
		public HairStandsSettings()
		{
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600433B RID: 17211 RVA: 0x0013BC3E File Offset: 0x0013A03E
		public int Segments
		{
			get
			{
				return this.Provider.GetSegmentsNum();
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x0600433C RID: 17212 RVA: 0x0013BC4C File Offset: 0x0013A04C
		public Vector3 HeadCenterWorld
		{
			get
			{
				if (this.HeadCenterType == HairHeadCenterType.LocalPoint)
				{
					return (!(this.Provider != null)) ? Vector3.zero : this.Provider.transform.TransformPoint(this.HeadCenter);
				}
				return (!(this.HeadCenterTransform != null)) ? Vector3.one : this.HeadCenterTransform.position;
			}
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x0013BCBC File Offset: 0x0013A0BC
		public override bool Validate()
		{
			return this.Provider != null && this.Provider.Validate(true);
		}

		// Token: 0x04003248 RID: 12872
		public GeometryProviderBase Provider;

		// Token: 0x04003249 RID: 12873
		public HairHeadCenterType HeadCenterType;

		// Token: 0x0400324A RID: 12874
		public Transform HeadCenterTransform;

		// Token: 0x0400324B RID: 12875
		public Vector3 HeadCenter;
	}
}
