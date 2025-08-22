using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000113 RID: 275
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentForceOverLifetimeModule : PersistentData
	{
		// Token: 0x060006A4 RID: 1700 RVA: 0x0002D6FB File Offset: 0x0002BAFB
		public PersistentForceOverLifetimeModule()
		{
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002D704 File Offset: 0x0002BB04
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule = (ParticleSystem.ForceOverLifetimeModule)obj;
			forceOverLifetimeModule.enabled = this.enabled;
			forceOverLifetimeModule.x = base.Write<ParticleSystem.MinMaxCurve>(forceOverLifetimeModule.x, this.x, objects);
			forceOverLifetimeModule.y = base.Write<ParticleSystem.MinMaxCurve>(forceOverLifetimeModule.y, this.y, objects);
			forceOverLifetimeModule.z = base.Write<ParticleSystem.MinMaxCurve>(forceOverLifetimeModule.z, this.z, objects);
			forceOverLifetimeModule.xMultiplier = this.xMultiplier;
			forceOverLifetimeModule.yMultiplier = this.yMultiplier;
			forceOverLifetimeModule.zMultiplier = this.zMultiplier;
			forceOverLifetimeModule.space = (ParticleSystemSimulationSpace)this.space;
			forceOverLifetimeModule.randomized = this.randomized;
			return forceOverLifetimeModule;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002D7D0 File Offset: 0x0002BBD0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule = (ParticleSystem.ForceOverLifetimeModule)obj;
			this.enabled = forceOverLifetimeModule.enabled;
			this.x = base.Read<PersistentMinMaxCurve>(this.x, forceOverLifetimeModule.x);
			this.y = base.Read<PersistentMinMaxCurve>(this.y, forceOverLifetimeModule.y);
			this.z = base.Read<PersistentMinMaxCurve>(this.z, forceOverLifetimeModule.z);
			this.xMultiplier = forceOverLifetimeModule.xMultiplier;
			this.yMultiplier = forceOverLifetimeModule.yMultiplier;
			this.zMultiplier = forceOverLifetimeModule.zMultiplier;
			this.space = (uint)forceOverLifetimeModule.space;
			this.randomized = forceOverLifetimeModule.randomized;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002D89A File Offset: 0x0002BC9A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.x, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.y, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.z, dependencies, objects, allowNulls);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002D8D4 File Offset: 0x0002BCD4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.ForceOverLifetimeModule forceOverLifetimeModule = (ParticleSystem.ForceOverLifetimeModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.x, forceOverLifetimeModule.x, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.y, forceOverLifetimeModule.y, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.z, forceOverLifetimeModule.z, dependencies);
		}

		// Token: 0x04000670 RID: 1648
		public bool enabled;

		// Token: 0x04000671 RID: 1649
		public PersistentMinMaxCurve x;

		// Token: 0x04000672 RID: 1650
		public PersistentMinMaxCurve y;

		// Token: 0x04000673 RID: 1651
		public PersistentMinMaxCurve z;

		// Token: 0x04000674 RID: 1652
		public float xMultiplier;

		// Token: 0x04000675 RID: 1653
		public float yMultiplier;

		// Token: 0x04000676 RID: 1654
		public float zMultiplier;

		// Token: 0x04000677 RID: 1655
		public uint space;

		// Token: 0x04000678 RID: 1656
		public bool randomized;
	}
}
