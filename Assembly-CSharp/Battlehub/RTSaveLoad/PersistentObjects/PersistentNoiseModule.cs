using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200011C RID: 284
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentNoiseModule : PersistentData
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x0002ED5F File Offset: 0x0002D15F
		public PersistentNoiseModule()
		{
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002ED68 File Offset: 0x0002D168
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.NoiseModule noiseModule = (ParticleSystem.NoiseModule)obj;
			noiseModule.enabled = this.enabled;
			noiseModule.separateAxes = this.separateAxes;
			noiseModule.strength = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.strength, this.strength, objects);
			noiseModule.strengthMultiplier = this.strengthMultiplier;
			noiseModule.strengthX = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.strengthX, this.strengthX, objects);
			noiseModule.strengthXMultiplier = this.strengthXMultiplier;
			noiseModule.strengthY = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.strengthY, this.strengthY, objects);
			noiseModule.strengthYMultiplier = this.strengthYMultiplier;
			noiseModule.strengthZ = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.strengthZ, this.strengthZ, objects);
			noiseModule.strengthZMultiplier = this.strengthZMultiplier;
			noiseModule.frequency = this.frequency;
			noiseModule.damping = this.damping;
			noiseModule.octaveCount = this.octaveCount;
			noiseModule.octaveMultiplier = this.octaveMultiplier;
			noiseModule.octaveScale = this.octaveScale;
			noiseModule.quality = (ParticleSystemNoiseQuality)this.quality;
			noiseModule.scrollSpeed = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.scrollSpeed, this.scrollSpeed, objects);
			noiseModule.scrollSpeedMultiplier = this.scrollSpeedMultiplier;
			noiseModule.remapEnabled = this.remapEnabled;
			noiseModule.remap = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.remap, this.remap, objects);
			noiseModule.remapMultiplier = this.remapMultiplier;
			noiseModule.remapX = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.remapX, this.remapX, objects);
			noiseModule.remapXMultiplier = this.remapXMultiplier;
			noiseModule.remapY = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.remapY, this.remapY, objects);
			noiseModule.remapYMultiplier = this.remapYMultiplier;
			noiseModule.remapZ = base.Write<ParticleSystem.MinMaxCurve>(noiseModule.remapZ, this.remapZ, objects);
			noiseModule.remapZMultiplier = this.remapZMultiplier;
			return noiseModule;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002EF74 File Offset: 0x0002D374
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.NoiseModule noiseModule = (ParticleSystem.NoiseModule)obj;
			this.enabled = noiseModule.enabled;
			this.separateAxes = noiseModule.separateAxes;
			this.strength = base.Read<PersistentMinMaxCurve>(this.strength, noiseModule.strength);
			this.strengthMultiplier = noiseModule.strengthMultiplier;
			this.strengthX = base.Read<PersistentMinMaxCurve>(this.strengthX, noiseModule.strengthX);
			this.strengthXMultiplier = noiseModule.strengthXMultiplier;
			this.strengthY = base.Read<PersistentMinMaxCurve>(this.strengthY, noiseModule.strengthY);
			this.strengthYMultiplier = noiseModule.strengthYMultiplier;
			this.strengthZ = base.Read<PersistentMinMaxCurve>(this.strengthZ, noiseModule.strengthZ);
			this.strengthZMultiplier = noiseModule.strengthZMultiplier;
			this.frequency = noiseModule.frequency;
			this.damping = noiseModule.damping;
			this.octaveCount = noiseModule.octaveCount;
			this.octaveMultiplier = noiseModule.octaveMultiplier;
			this.octaveScale = noiseModule.octaveScale;
			this.quality = (uint)noiseModule.quality;
			this.scrollSpeed = base.Read<PersistentMinMaxCurve>(this.scrollSpeed, noiseModule.scrollSpeed);
			this.scrollSpeedMultiplier = noiseModule.scrollSpeedMultiplier;
			this.remapEnabled = noiseModule.remapEnabled;
			this.remap = base.Read<PersistentMinMaxCurve>(this.remap, noiseModule.remap);
			this.remapMultiplier = noiseModule.remapMultiplier;
			this.remapX = base.Read<PersistentMinMaxCurve>(this.remapX, noiseModule.remapX);
			this.remapXMultiplier = noiseModule.remapXMultiplier;
			this.remapY = base.Read<PersistentMinMaxCurve>(this.remapY, noiseModule.remapY);
			this.remapYMultiplier = noiseModule.remapYMultiplier;
			this.remapZ = base.Read<PersistentMinMaxCurve>(this.remapZ, noiseModule.remapZ);
			this.remapZMultiplier = noiseModule.remapZMultiplier;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002F190 File Offset: 0x0002D590
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.strength, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.strengthX, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.strengthY, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.strengthZ, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.scrollSpeed, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.remap, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.remapX, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.remapY, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.remapZ, dependencies, objects, allowNulls);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002F230 File Offset: 0x0002D630
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.NoiseModule noiseModule = (ParticleSystem.NoiseModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.strength, noiseModule.strength, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.strengthX, noiseModule.strengthX, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.strengthY, noiseModule.strengthY, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.strengthZ, noiseModule.strengthZ, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.scrollSpeed, noiseModule.scrollSpeed, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.remap, noiseModule.remap, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.remapX, noiseModule.remapX, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.remapY, noiseModule.remapY, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.remapZ, noiseModule.remapZ, dependencies);
		}

		// Token: 0x040006D1 RID: 1745
		public bool enabled;

		// Token: 0x040006D2 RID: 1746
		public bool separateAxes;

		// Token: 0x040006D3 RID: 1747
		public PersistentMinMaxCurve strength;

		// Token: 0x040006D4 RID: 1748
		public float strengthMultiplier;

		// Token: 0x040006D5 RID: 1749
		public PersistentMinMaxCurve strengthX;

		// Token: 0x040006D6 RID: 1750
		public float strengthXMultiplier;

		// Token: 0x040006D7 RID: 1751
		public PersistentMinMaxCurve strengthY;

		// Token: 0x040006D8 RID: 1752
		public float strengthYMultiplier;

		// Token: 0x040006D9 RID: 1753
		public PersistentMinMaxCurve strengthZ;

		// Token: 0x040006DA RID: 1754
		public float strengthZMultiplier;

		// Token: 0x040006DB RID: 1755
		public float frequency;

		// Token: 0x040006DC RID: 1756
		public bool damping;

		// Token: 0x040006DD RID: 1757
		public int octaveCount;

		// Token: 0x040006DE RID: 1758
		public float octaveMultiplier;

		// Token: 0x040006DF RID: 1759
		public float octaveScale;

		// Token: 0x040006E0 RID: 1760
		public uint quality;

		// Token: 0x040006E1 RID: 1761
		public PersistentMinMaxCurve scrollSpeed;

		// Token: 0x040006E2 RID: 1762
		public float scrollSpeedMultiplier;

		// Token: 0x040006E3 RID: 1763
		public bool remapEnabled;

		// Token: 0x040006E4 RID: 1764
		public PersistentMinMaxCurve remap;

		// Token: 0x040006E5 RID: 1765
		public float remapMultiplier;

		// Token: 0x040006E6 RID: 1766
		public PersistentMinMaxCurve remapX;

		// Token: 0x040006E7 RID: 1767
		public float remapXMultiplier;

		// Token: 0x040006E8 RID: 1768
		public PersistentMinMaxCurve remapY;

		// Token: 0x040006E9 RID: 1769
		public float remapYMultiplier;

		// Token: 0x040006EA RID: 1770
		public PersistentMinMaxCurve remapZ;

		// Token: 0x040006EB RID: 1771
		public float remapZMultiplier;
	}
}
