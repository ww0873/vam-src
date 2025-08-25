using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000121 RID: 289
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSizeBySpeedModule : PersistentData
	{
		// Token: 0x060006E7 RID: 1767 RVA: 0x0002FDCB File Offset: 0x0002E1CB
		public PersistentSizeBySpeedModule()
		{
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002FDD4 File Offset: 0x0002E1D4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.SizeBySpeedModule sizeBySpeedModule = (ParticleSystem.SizeBySpeedModule)obj;
			sizeBySpeedModule.enabled = this.enabled;
			sizeBySpeedModule.size = base.Write<ParticleSystem.MinMaxCurve>(sizeBySpeedModule.size, this.size, objects);
			sizeBySpeedModule.sizeMultiplier = this.sizeMultiplier;
			sizeBySpeedModule.x = base.Write<ParticleSystem.MinMaxCurve>(sizeBySpeedModule.x, this.x, objects);
			sizeBySpeedModule.xMultiplier = this.xMultiplier;
			sizeBySpeedModule.y = base.Write<ParticleSystem.MinMaxCurve>(sizeBySpeedModule.y, this.y, objects);
			sizeBySpeedModule.yMultiplier = this.yMultiplier;
			sizeBySpeedModule.z = base.Write<ParticleSystem.MinMaxCurve>(sizeBySpeedModule.z, this.z, objects);
			sizeBySpeedModule.zMultiplier = this.zMultiplier;
			sizeBySpeedModule.separateAxes = this.separateAxes;
			sizeBySpeedModule.range = this.range;
			return sizeBySpeedModule;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0002FEC8 File Offset: 0x0002E2C8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.SizeBySpeedModule sizeBySpeedModule = (ParticleSystem.SizeBySpeedModule)obj;
			this.enabled = sizeBySpeedModule.enabled;
			this.size = base.Read<PersistentMinMaxCurve>(this.size, sizeBySpeedModule.size);
			this.sizeMultiplier = sizeBySpeedModule.sizeMultiplier;
			this.x = base.Read<PersistentMinMaxCurve>(this.x, sizeBySpeedModule.x);
			this.xMultiplier = sizeBySpeedModule.xMultiplier;
			this.y = base.Read<PersistentMinMaxCurve>(this.y, sizeBySpeedModule.y);
			this.yMultiplier = sizeBySpeedModule.yMultiplier;
			this.z = base.Read<PersistentMinMaxCurve>(this.z, sizeBySpeedModule.z);
			this.zMultiplier = sizeBySpeedModule.zMultiplier;
			this.separateAxes = sizeBySpeedModule.separateAxes;
			this.range = sizeBySpeedModule.range;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0002FFC0 File Offset: 0x0002E3C0
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.size, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.x, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.y, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.z, dependencies, objects, allowNulls);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00030014 File Offset: 0x0002E414
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.SizeBySpeedModule sizeBySpeedModule = (ParticleSystem.SizeBySpeedModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.size, sizeBySpeedModule.size, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.x, sizeBySpeedModule.x, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.y, sizeBySpeedModule.y, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.z, sizeBySpeedModule.z, dependencies);
		}

		// Token: 0x04000726 RID: 1830
		public bool enabled;

		// Token: 0x04000727 RID: 1831
		public PersistentMinMaxCurve size;

		// Token: 0x04000728 RID: 1832
		public float sizeMultiplier;

		// Token: 0x04000729 RID: 1833
		public PersistentMinMaxCurve x;

		// Token: 0x0400072A RID: 1834
		public float xMultiplier;

		// Token: 0x0400072B RID: 1835
		public PersistentMinMaxCurve y;

		// Token: 0x0400072C RID: 1836
		public float yMultiplier;

		// Token: 0x0400072D RID: 1837
		public PersistentMinMaxCurve z;

		// Token: 0x0400072E RID: 1838
		public float zMultiplier;

		// Token: 0x0400072F RID: 1839
		public bool separateAxes;

		// Token: 0x04000730 RID: 1840
		public Vector2 range;
	}
}
