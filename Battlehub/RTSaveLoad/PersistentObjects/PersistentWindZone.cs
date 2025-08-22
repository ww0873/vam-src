using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001D9 RID: 473
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentWindZone : PersistentComponent
	{
		// Token: 0x0600098A RID: 2442 RVA: 0x0003AE81 File Offset: 0x00039281
		public PersistentWindZone()
		{
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0003AE8C File Offset: 0x0003928C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			WindZone windZone = (WindZone)obj;
			windZone.mode = (WindZoneMode)this.mode;
			windZone.radius = this.radius;
			windZone.windMain = this.windMain;
			windZone.windTurbulence = this.windTurbulence;
			windZone.windPulseMagnitude = this.windPulseMagnitude;
			windZone.windPulseFrequency = this.windPulseFrequency;
			return windZone;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0003AEFC File Offset: 0x000392FC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			WindZone windZone = (WindZone)obj;
			this.mode = (uint)windZone.mode;
			this.radius = windZone.radius;
			this.windMain = windZone.windMain;
			this.windTurbulence = windZone.windTurbulence;
			this.windPulseMagnitude = windZone.windPulseMagnitude;
			this.windPulseFrequency = windZone.windPulseFrequency;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0003AF66 File Offset: 0x00039366
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000ACB RID: 2763
		public uint mode;

		// Token: 0x04000ACC RID: 2764
		public float radius;

		// Token: 0x04000ACD RID: 2765
		public float windMain;

		// Token: 0x04000ACE RID: 2766
		public float windTurbulence;

		// Token: 0x04000ACF RID: 2767
		public float windPulseMagnitude;

		// Token: 0x04000AD0 RID: 2768
		public float windPulseFrequency;
	}
}
