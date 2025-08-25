using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000112 RID: 274
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentExternalForcesModule : PersistentData
	{
		// Token: 0x060006A0 RID: 1696 RVA: 0x0002D662 File Offset: 0x0002BA62
		public PersistentExternalForcesModule()
		{
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002D66C File Offset: 0x0002BA6C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.ExternalForcesModule externalForcesModule = (ParticleSystem.ExternalForcesModule)obj;
			externalForcesModule.enabled = this.enabled;
			externalForcesModule.multiplier = this.multiplier;
			return externalForcesModule;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002D6B4 File Offset: 0x0002BAB4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ExternalForcesModule externalForcesModule = (ParticleSystem.ExternalForcesModule)obj;
			this.enabled = externalForcesModule.enabled;
			this.multiplier = externalForcesModule.multiplier;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0002D6F0 File Offset: 0x0002BAF0
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400066E RID: 1646
		public bool enabled;

		// Token: 0x0400066F RID: 1647
		public float multiplier;
	}
}
