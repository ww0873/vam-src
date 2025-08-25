using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000117 RID: 279
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLightsModule : PersistentData
	{
		// Token: 0x060006B6 RID: 1718 RVA: 0x0002DBED File Offset: 0x0002BFED
		public PersistentLightsModule()
		{
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002DBF8 File Offset: 0x0002BFF8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.LightsModule lightsModule = (ParticleSystem.LightsModule)obj;
			lightsModule.enabled = this.enabled;
			lightsModule.ratio = this.ratio;
			lightsModule.useRandomDistribution = this.useRandomDistribution;
			lightsModule.light = (Light)objects.Get(this.light);
			lightsModule.useParticleColor = this.useParticleColor;
			lightsModule.sizeAffectsRange = this.sizeAffectsRange;
			lightsModule.alphaAffectsIntensity = this.alphaAffectsIntensity;
			lightsModule.range = base.Write<ParticleSystem.MinMaxCurve>(lightsModule.range, this.range, objects);
			lightsModule.rangeMultiplier = this.rangeMultiplier;
			lightsModule.intensity = base.Write<ParticleSystem.MinMaxCurve>(lightsModule.intensity, this.intensity, objects);
			lightsModule.intensityMultiplier = this.intensityMultiplier;
			lightsModule.maxLights = this.maxLights;
			return lightsModule;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002DCE8 File Offset: 0x0002C0E8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.LightsModule lightsModule = (ParticleSystem.LightsModule)obj;
			this.enabled = lightsModule.enabled;
			this.ratio = lightsModule.ratio;
			this.useRandomDistribution = lightsModule.useRandomDistribution;
			this.light = lightsModule.light.GetMappedInstanceID();
			this.useParticleColor = lightsModule.useParticleColor;
			this.sizeAffectsRange = lightsModule.sizeAffectsRange;
			this.alphaAffectsIntensity = lightsModule.alphaAffectsIntensity;
			this.range = base.Read<PersistentMinMaxCurve>(this.range, lightsModule.range);
			this.rangeMultiplier = lightsModule.rangeMultiplier;
			this.intensity = base.Read<PersistentMinMaxCurve>(this.intensity, lightsModule.intensity);
			this.intensityMultiplier = lightsModule.intensityMultiplier;
			this.maxLights = lightsModule.maxLights;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002DDCD File Offset: 0x0002C1CD
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.light, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.range, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.intensity, dependencies, objects, allowNulls);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002DE08 File Offset: 0x0002C208
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.LightsModule lightsModule = (ParticleSystem.LightsModule)obj;
			base.AddDependency(lightsModule.light, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.range, lightsModule.range, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.intensity, lightsModule.intensity, dependencies);
		}

		// Token: 0x04000685 RID: 1669
		public bool enabled;

		// Token: 0x04000686 RID: 1670
		public float ratio;

		// Token: 0x04000687 RID: 1671
		public bool useRandomDistribution;

		// Token: 0x04000688 RID: 1672
		public long light;

		// Token: 0x04000689 RID: 1673
		public bool useParticleColor;

		// Token: 0x0400068A RID: 1674
		public bool sizeAffectsRange;

		// Token: 0x0400068B RID: 1675
		public bool alphaAffectsIntensity;

		// Token: 0x0400068C RID: 1676
		public PersistentMinMaxCurve range;

		// Token: 0x0400068D RID: 1677
		public float rangeMultiplier;

		// Token: 0x0400068E RID: 1678
		public PersistentMinMaxCurve intensity;

		// Token: 0x0400068F RID: 1679
		public float intensityMultiplier;

		// Token: 0x04000690 RID: 1680
		public int maxLights;
	}
}
