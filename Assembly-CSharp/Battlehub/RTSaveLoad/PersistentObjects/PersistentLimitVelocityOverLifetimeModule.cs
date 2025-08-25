using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000118 RID: 280
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLimitVelocityOverLifetimeModule : PersistentData
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x0002DE6B File Offset: 0x0002C26B
		public PersistentLimitVelocityOverLifetimeModule()
		{
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002DE74 File Offset: 0x0002C274
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetimeModule = (ParticleSystem.LimitVelocityOverLifetimeModule)obj;
			limitVelocityOverLifetimeModule.enabled = this.enabled;
			limitVelocityOverLifetimeModule.limitX = base.Write<ParticleSystem.MinMaxCurve>(limitVelocityOverLifetimeModule.limitX, this.limitX, objects);
			limitVelocityOverLifetimeModule.limitXMultiplier = this.limitXMultiplier;
			limitVelocityOverLifetimeModule.limitY = base.Write<ParticleSystem.MinMaxCurve>(limitVelocityOverLifetimeModule.limitY, this.limitY, objects);
			limitVelocityOverLifetimeModule.limitYMultiplier = this.limitYMultiplier;
			limitVelocityOverLifetimeModule.limitZ = base.Write<ParticleSystem.MinMaxCurve>(limitVelocityOverLifetimeModule.limitZ, this.limitZ, objects);
			limitVelocityOverLifetimeModule.limitZMultiplier = this.limitZMultiplier;
			limitVelocityOverLifetimeModule.limit = base.Write<ParticleSystem.MinMaxCurve>(limitVelocityOverLifetimeModule.limit, this.limit, objects);
			limitVelocityOverLifetimeModule.limitMultiplier = this.limitMultiplier;
			limitVelocityOverLifetimeModule.dampen = this.dampen;
			limitVelocityOverLifetimeModule.separateAxes = this.separateAxes;
			limitVelocityOverLifetimeModule.space = (ParticleSystemSimulationSpace)this.space;
			return limitVelocityOverLifetimeModule;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002DF74 File Offset: 0x0002C374
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetimeModule = (ParticleSystem.LimitVelocityOverLifetimeModule)obj;
			this.enabled = limitVelocityOverLifetimeModule.enabled;
			this.limitX = base.Read<PersistentMinMaxCurve>(this.limitX, limitVelocityOverLifetimeModule.limitX);
			this.limitXMultiplier = limitVelocityOverLifetimeModule.limitXMultiplier;
			this.limitY = base.Read<PersistentMinMaxCurve>(this.limitY, limitVelocityOverLifetimeModule.limitY);
			this.limitYMultiplier = limitVelocityOverLifetimeModule.limitYMultiplier;
			this.limitZ = base.Read<PersistentMinMaxCurve>(this.limitZ, limitVelocityOverLifetimeModule.limitZ);
			this.limitZMultiplier = limitVelocityOverLifetimeModule.limitZMultiplier;
			this.limit = base.Read<PersistentMinMaxCurve>(this.limit, limitVelocityOverLifetimeModule.limit);
			this.limitMultiplier = limitVelocityOverLifetimeModule.limitMultiplier;
			this.dampen = limitVelocityOverLifetimeModule.dampen;
			this.separateAxes = limitVelocityOverLifetimeModule.separateAxes;
			this.space = (uint)limitVelocityOverLifetimeModule.space;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002E078 File Offset: 0x0002C478
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.limitX, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.limitY, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.limitZ, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.limit, dependencies, objects, allowNulls);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002E0CC File Offset: 0x0002C4CC
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetimeModule = (ParticleSystem.LimitVelocityOverLifetimeModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.limitX, limitVelocityOverLifetimeModule.limitX, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.limitY, limitVelocityOverLifetimeModule.limitY, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.limitZ, limitVelocityOverLifetimeModule.limitZ, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.limit, limitVelocityOverLifetimeModule.limit, dependencies);
		}

		// Token: 0x04000691 RID: 1681
		public bool enabled;

		// Token: 0x04000692 RID: 1682
		public PersistentMinMaxCurve limitX;

		// Token: 0x04000693 RID: 1683
		public float limitXMultiplier;

		// Token: 0x04000694 RID: 1684
		public PersistentMinMaxCurve limitY;

		// Token: 0x04000695 RID: 1685
		public float limitYMultiplier;

		// Token: 0x04000696 RID: 1686
		public PersistentMinMaxCurve limitZ;

		// Token: 0x04000697 RID: 1687
		public float limitZMultiplier;

		// Token: 0x04000698 RID: 1688
		public PersistentMinMaxCurve limit;

		// Token: 0x04000699 RID: 1689
		public float limitMultiplier;

		// Token: 0x0400069A RID: 1690
		public float dampen;

		// Token: 0x0400069B RID: 1691
		public bool separateAxes;

		// Token: 0x0400069C RID: 1692
		public uint space;
	}
}
