using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200011E RID: 286
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRotationBySpeedModule : PersistentData
	{
		// Token: 0x060006D8 RID: 1752 RVA: 0x0002F4EA File Offset: 0x0002D8EA
		public PersistentRotationBySpeedModule()
		{
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002F4F4 File Offset: 0x0002D8F4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.RotationBySpeedModule rotationBySpeedModule = (ParticleSystem.RotationBySpeedModule)obj;
			rotationBySpeedModule.enabled = this.enabled;
			rotationBySpeedModule.x = base.Write<ParticleSystem.MinMaxCurve>(rotationBySpeedModule.x, this.x, objects);
			rotationBySpeedModule.xMultiplier = this.xMultiplier;
			rotationBySpeedModule.y = base.Write<ParticleSystem.MinMaxCurve>(rotationBySpeedModule.y, this.y, objects);
			rotationBySpeedModule.yMultiplier = this.yMultiplier;
			rotationBySpeedModule.z = base.Write<ParticleSystem.MinMaxCurve>(rotationBySpeedModule.z, this.z, objects);
			rotationBySpeedModule.zMultiplier = this.zMultiplier;
			rotationBySpeedModule.separateAxes = this.separateAxes;
			rotationBySpeedModule.range = this.range;
			return rotationBySpeedModule;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002F5C0 File Offset: 0x0002D9C0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.RotationBySpeedModule rotationBySpeedModule = (ParticleSystem.RotationBySpeedModule)obj;
			this.enabled = rotationBySpeedModule.enabled;
			this.x = base.Read<PersistentMinMaxCurve>(this.x, rotationBySpeedModule.x);
			this.xMultiplier = rotationBySpeedModule.xMultiplier;
			this.y = base.Read<PersistentMinMaxCurve>(this.y, rotationBySpeedModule.y);
			this.yMultiplier = rotationBySpeedModule.yMultiplier;
			this.z = base.Read<PersistentMinMaxCurve>(this.z, rotationBySpeedModule.z);
			this.zMultiplier = rotationBySpeedModule.zMultiplier;
			this.separateAxes = rotationBySpeedModule.separateAxes;
			this.range = rotationBySpeedModule.range;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002F68A File Offset: 0x0002DA8A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.x, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.y, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.z, dependencies, objects, allowNulls);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002F6C4 File Offset: 0x0002DAC4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.RotationBySpeedModule rotationBySpeedModule = (ParticleSystem.RotationBySpeedModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.x, rotationBySpeedModule.x, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.y, rotationBySpeedModule.y, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.z, rotationBySpeedModule.z, dependencies);
		}

		// Token: 0x040006F9 RID: 1785
		public bool enabled;

		// Token: 0x040006FA RID: 1786
		public PersistentMinMaxCurve x;

		// Token: 0x040006FB RID: 1787
		public float xMultiplier;

		// Token: 0x040006FC RID: 1788
		public PersistentMinMaxCurve y;

		// Token: 0x040006FD RID: 1789
		public float yMultiplier;

		// Token: 0x040006FE RID: 1790
		public PersistentMinMaxCurve z;

		// Token: 0x040006FF RID: 1791
		public float zMultiplier;

		// Token: 0x04000700 RID: 1792
		public bool separateAxes;

		// Token: 0x04000701 RID: 1793
		public Vector2 range;
	}
}
