using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000123 RID: 291
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSubEmittersModule : PersistentData
	{
		// Token: 0x060006F1 RID: 1777 RVA: 0x0003034F File Offset: 0x0002E74F
		public PersistentSubEmittersModule()
		{
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00030358 File Offset: 0x0002E758
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.SubEmittersModule subEmittersModule = (ParticleSystem.SubEmittersModule)obj;
			subEmittersModule.enabled = this.enabled;
			return subEmittersModule;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00030394 File Offset: 0x0002E794
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			this.enabled = ((ParticleSystem.SubEmittersModule)obj).enabled;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000303C3 File Offset: 0x0002E7C3
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400073B RID: 1851
		public bool enabled;
	}
}
