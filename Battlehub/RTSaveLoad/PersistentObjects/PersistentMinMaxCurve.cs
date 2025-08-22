using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200011A RID: 282
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMinMaxCurve : PersistentData
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x0002E956 File Offset: 0x0002CD56
		public PersistentMinMaxCurve()
		{
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002E960 File Offset: 0x0002CD60
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.MinMaxCurve minMaxCurve = (ParticleSystem.MinMaxCurve)obj;
			minMaxCurve.mode = (ParticleSystemCurveMode)this.mode;
			minMaxCurve.curveMultiplier = this.curveMultiplier;
			minMaxCurve.curveMax = base.Write<AnimationCurve>(minMaxCurve.curveMax, this.curveMax, objects);
			minMaxCurve.curveMin = base.Write<AnimationCurve>(minMaxCurve.curveMin, this.curveMin, objects);
			minMaxCurve.constantMax = this.constantMax;
			minMaxCurve.constantMin = this.constantMin;
			minMaxCurve.constant = this.constant;
			minMaxCurve.curve = base.Write<AnimationCurve>(minMaxCurve.curve, this.curve, objects);
			return minMaxCurve;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002EA20 File Offset: 0x0002CE20
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.MinMaxCurve minMaxCurve = (ParticleSystem.MinMaxCurve)obj;
			this.mode = (uint)minMaxCurve.mode;
			this.curveMultiplier = minMaxCurve.curveMultiplier;
			this.curveMax = base.Read<PersistentAnimationCurve>(this.curveMax, minMaxCurve.curveMax);
			this.curveMin = base.Read<PersistentAnimationCurve>(this.curveMin, minMaxCurve.curveMin);
			this.constantMax = minMaxCurve.constantMax;
			this.constantMin = minMaxCurve.constantMin;
			this.constant = minMaxCurve.constant;
			this.curve = base.Read<PersistentAnimationCurve>(this.curve, minMaxCurve.curve);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0002EACE File Offset: 0x0002CECE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentAnimationCurve>(this.curveMax, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentAnimationCurve>(this.curveMin, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentAnimationCurve>(this.curve, dependencies, objects, allowNulls);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002EB08 File Offset: 0x0002CF08
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.MinMaxCurve minMaxCurve = (ParticleSystem.MinMaxCurve)obj;
			base.GetDependencies<PersistentAnimationCurve>(this.curveMax, minMaxCurve.curveMax, dependencies);
			base.GetDependencies<PersistentAnimationCurve>(this.curveMin, minMaxCurve.curveMin, dependencies);
			base.GetDependencies<PersistentAnimationCurve>(this.curve, minMaxCurve.curve, dependencies);
		}

		// Token: 0x040006C2 RID: 1730
		public uint mode;

		// Token: 0x040006C3 RID: 1731
		public float curveMultiplier;

		// Token: 0x040006C4 RID: 1732
		public PersistentAnimationCurve curveMax;

		// Token: 0x040006C5 RID: 1733
		public PersistentAnimationCurve curveMin;

		// Token: 0x040006C6 RID: 1734
		public float constantMax;

		// Token: 0x040006C7 RID: 1735
		public float constantMin;

		// Token: 0x040006C8 RID: 1736
		public float constant;

		// Token: 0x040006C9 RID: 1737
		public PersistentAnimationCurve curve;
	}
}
