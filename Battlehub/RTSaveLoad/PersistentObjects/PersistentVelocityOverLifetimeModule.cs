using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000126 RID: 294
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentVelocityOverLifetimeModule : PersistentData
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x0003098B File Offset: 0x0002ED8B
		public PersistentVelocityOverLifetimeModule()
		{
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00030994 File Offset: 0x0002ED94
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = (ParticleSystem.VelocityOverLifetimeModule)obj;
			velocityOverLifetimeModule.enabled = this.enabled;
			velocityOverLifetimeModule.x = base.Write<ParticleSystem.MinMaxCurve>(velocityOverLifetimeModule.x, this.x, objects);
			velocityOverLifetimeModule.y = base.Write<ParticleSystem.MinMaxCurve>(velocityOverLifetimeModule.y, this.y, objects);
			velocityOverLifetimeModule.z = base.Write<ParticleSystem.MinMaxCurve>(velocityOverLifetimeModule.z, this.z, objects);
			velocityOverLifetimeModule.xMultiplier = this.xMultiplier;
			velocityOverLifetimeModule.yMultiplier = this.yMultiplier;
			velocityOverLifetimeModule.zMultiplier = this.zMultiplier;
			velocityOverLifetimeModule.space = (ParticleSystemSimulationSpace)this.space;
			return velocityOverLifetimeModule;
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00030A54 File Offset: 0x0002EE54
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = (ParticleSystem.VelocityOverLifetimeModule)obj;
			this.enabled = velocityOverLifetimeModule.enabled;
			this.x = base.Read<PersistentMinMaxCurve>(this.x, velocityOverLifetimeModule.x);
			this.y = base.Read<PersistentMinMaxCurve>(this.y, velocityOverLifetimeModule.y);
			this.z = base.Read<PersistentMinMaxCurve>(this.z, velocityOverLifetimeModule.z);
			this.xMultiplier = velocityOverLifetimeModule.xMultiplier;
			this.yMultiplier = velocityOverLifetimeModule.yMultiplier;
			this.zMultiplier = velocityOverLifetimeModule.zMultiplier;
			this.space = (uint)velocityOverLifetimeModule.space;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00030B11 File Offset: 0x0002EF11
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.x, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.y, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.z, dependencies, objects, allowNulls);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00030B4C File Offset: 0x0002EF4C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = (ParticleSystem.VelocityOverLifetimeModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.x, velocityOverLifetimeModule.x, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.y, velocityOverLifetimeModule.y, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.z, velocityOverLifetimeModule.z, dependencies);
		}

		// Token: 0x04000759 RID: 1881
		public bool enabled;

		// Token: 0x0400075A RID: 1882
		public PersistentMinMaxCurve x;

		// Token: 0x0400075B RID: 1883
		public PersistentMinMaxCurve y;

		// Token: 0x0400075C RID: 1884
		public PersistentMinMaxCurve z;

		// Token: 0x0400075D RID: 1885
		public float xMultiplier;

		// Token: 0x0400075E RID: 1886
		public float yMultiplier;

		// Token: 0x0400075F RID: 1887
		public float zMultiplier;

		// Token: 0x04000760 RID: 1888
		public uint space;
	}
}
