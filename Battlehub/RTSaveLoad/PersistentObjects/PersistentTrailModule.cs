using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000125 RID: 293
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTrailModule : PersistentData
	{
		// Token: 0x060006FA RID: 1786 RVA: 0x00030651 File Offset: 0x0002EA51
		public PersistentTrailModule()
		{
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0003065C File Offset: 0x0002EA5C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.TrailModule trailModule = (ParticleSystem.TrailModule)obj;
			trailModule.enabled = this.enabled;
			trailModule.ratio = this.ratio;
			trailModule.lifetime = base.Write<ParticleSystem.MinMaxCurve>(trailModule.lifetime, this.lifetime, objects);
			trailModule.lifetimeMultiplier = this.lifetimeMultiplier;
			trailModule.minVertexDistance = this.minVertexDistance;
			trailModule.textureMode = (ParticleSystemTrailTextureMode)this.textureMode;
			trailModule.worldSpace = this.worldSpace;
			trailModule.dieWithParticles = this.dieWithParticles;
			trailModule.sizeAffectsWidth = this.sizeAffectsWidth;
			trailModule.sizeAffectsLifetime = this.sizeAffectsLifetime;
			trailModule.inheritParticleColor = this.inheritParticleColor;
			trailModule.colorOverLifetime = base.Write<ParticleSystem.MinMaxGradient>(trailModule.colorOverLifetime, this.colorOverLifetime, objects);
			trailModule.widthOverTrail = base.Write<ParticleSystem.MinMaxCurve>(trailModule.widthOverTrail, this.widthOverTrail, objects);
			trailModule.widthOverTrailMultiplier = this.widthOverTrailMultiplier;
			trailModule.colorOverTrail = base.Write<ParticleSystem.MinMaxGradient>(trailModule.colorOverTrail, this.colorOverTrail, objects);
			return trailModule;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00030784 File Offset: 0x0002EB84
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.TrailModule trailModule = (ParticleSystem.TrailModule)obj;
			this.enabled = trailModule.enabled;
			this.ratio = trailModule.ratio;
			this.lifetime = base.Read<PersistentMinMaxCurve>(this.lifetime, trailModule.lifetime);
			this.lifetimeMultiplier = trailModule.lifetimeMultiplier;
			this.minVertexDistance = trailModule.minVertexDistance;
			this.textureMode = (uint)trailModule.textureMode;
			this.worldSpace = trailModule.worldSpace;
			this.dieWithParticles = trailModule.dieWithParticles;
			this.sizeAffectsWidth = trailModule.sizeAffectsWidth;
			this.sizeAffectsLifetime = trailModule.sizeAffectsLifetime;
			this.inheritParticleColor = trailModule.inheritParticleColor;
			this.colorOverLifetime = base.Read<PersistentMinMaxGradient>(this.colorOverLifetime, trailModule.colorOverLifetime);
			this.widthOverTrail = base.Read<PersistentMinMaxCurve>(this.widthOverTrail, trailModule.widthOverTrail);
			this.widthOverTrailMultiplier = trailModule.widthOverTrailMultiplier;
			this.colorOverTrail = base.Read<PersistentMinMaxGradient>(this.colorOverTrail, trailModule.colorOverTrail);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x000308B0 File Offset: 0x0002ECB0
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.lifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxGradient>(this.colorOverLifetime, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.widthOverTrail, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxGradient>(this.colorOverTrail, dependencies, objects, allowNulls);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00030904 File Offset: 0x0002ED04
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.TrailModule trailModule = (ParticleSystem.TrailModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.lifetime, trailModule.lifetime, dependencies);
			base.GetDependencies<PersistentMinMaxGradient>(this.colorOverLifetime, trailModule.colorOverLifetime, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.widthOverTrail, trailModule.widthOverTrail, dependencies);
			base.GetDependencies<PersistentMinMaxGradient>(this.colorOverTrail, trailModule.colorOverTrail, dependencies);
		}

		// Token: 0x0400074A RID: 1866
		public bool enabled;

		// Token: 0x0400074B RID: 1867
		public float ratio;

		// Token: 0x0400074C RID: 1868
		public PersistentMinMaxCurve lifetime;

		// Token: 0x0400074D RID: 1869
		public float lifetimeMultiplier;

		// Token: 0x0400074E RID: 1870
		public float minVertexDistance;

		// Token: 0x0400074F RID: 1871
		public uint textureMode;

		// Token: 0x04000750 RID: 1872
		public bool worldSpace;

		// Token: 0x04000751 RID: 1873
		public bool dieWithParticles;

		// Token: 0x04000752 RID: 1874
		public bool sizeAffectsWidth;

		// Token: 0x04000753 RID: 1875
		public bool sizeAffectsLifetime;

		// Token: 0x04000754 RID: 1876
		public bool inheritParticleColor;

		// Token: 0x04000755 RID: 1877
		public PersistentMinMaxGradient colorOverLifetime;

		// Token: 0x04000756 RID: 1878
		public PersistentMinMaxCurve widthOverTrail;

		// Token: 0x04000757 RID: 1879
		public float widthOverTrailMultiplier;

		// Token: 0x04000758 RID: 1880
		public PersistentMinMaxGradient colorOverTrail;
	}
}
