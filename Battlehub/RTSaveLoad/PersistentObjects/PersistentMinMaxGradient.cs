using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200011B RID: 283
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMinMaxGradient : PersistentData
	{
		// Token: 0x060006CA RID: 1738 RVA: 0x0002EB67 File Offset: 0x0002CF67
		public PersistentMinMaxGradient()
		{
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002EB70 File Offset: 0x0002CF70
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.MinMaxGradient minMaxGradient = (ParticleSystem.MinMaxGradient)obj;
			minMaxGradient.mode = (ParticleSystemGradientMode)this.mode;
			minMaxGradient.gradientMax = base.Write<Gradient>(minMaxGradient.gradientMax, this.gradientMax, objects);
			minMaxGradient.gradientMin = base.Write<Gradient>(minMaxGradient.gradientMin, this.gradientMin, objects);
			minMaxGradient.colorMax = this.colorMax;
			minMaxGradient.colorMin = this.colorMin;
			minMaxGradient.color = this.color;
			minMaxGradient.gradient = base.Write<Gradient>(minMaxGradient.gradient, this.gradient, objects);
			return minMaxGradient;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002EC24 File Offset: 0x0002D024
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.MinMaxGradient minMaxGradient = (ParticleSystem.MinMaxGradient)obj;
			this.mode = (uint)minMaxGradient.mode;
			this.gradientMax = base.Read<PersistentGradient>(this.gradientMax, minMaxGradient.gradientMax);
			this.gradientMin = base.Read<PersistentGradient>(this.gradientMin, minMaxGradient.gradientMin);
			this.colorMax = minMaxGradient.colorMax;
			this.colorMin = minMaxGradient.colorMin;
			this.color = minMaxGradient.color;
			this.gradient = base.Read<PersistentGradient>(this.gradient, minMaxGradient.gradient);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002ECC5 File Offset: 0x0002D0C5
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGradient>(this.gradientMax, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGradient>(this.gradientMin, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentGradient>(this.gradient, dependencies, objects, allowNulls);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002ED00 File Offset: 0x0002D100
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.MinMaxGradient minMaxGradient = (ParticleSystem.MinMaxGradient)obj;
			base.GetDependencies<PersistentGradient>(this.gradientMax, minMaxGradient.gradientMax, dependencies);
			base.GetDependencies<PersistentGradient>(this.gradientMin, minMaxGradient.gradientMin, dependencies);
			base.GetDependencies<PersistentGradient>(this.gradient, minMaxGradient.gradient, dependencies);
		}

		// Token: 0x040006CA RID: 1738
		public uint mode;

		// Token: 0x040006CB RID: 1739
		public PersistentGradient gradientMax;

		// Token: 0x040006CC RID: 1740
		public PersistentGradient gradientMin;

		// Token: 0x040006CD RID: 1741
		public Color colorMax;

		// Token: 0x040006CE RID: 1742
		public Color colorMin;

		// Token: 0x040006CF RID: 1743
		public Color color;

		// Token: 0x040006D0 RID: 1744
		public PersistentGradient gradient;
	}
}
