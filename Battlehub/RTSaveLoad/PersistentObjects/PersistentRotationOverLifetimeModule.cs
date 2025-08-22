using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200011F RID: 287
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRotationOverLifetimeModule : PersistentData
	{
		// Token: 0x060006DD RID: 1757 RVA: 0x0002F732 File Offset: 0x0002DB32
		public PersistentRotationOverLifetimeModule()
		{
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002F73C File Offset: 0x0002DB3C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.RotationOverLifetimeModule rotationOverLifetimeModule = (ParticleSystem.RotationOverLifetimeModule)obj;
			rotationOverLifetimeModule.enabled = this.enabled;
			rotationOverLifetimeModule.x = base.Write<ParticleSystem.MinMaxCurve>(rotationOverLifetimeModule.x, this.x, objects);
			rotationOverLifetimeModule.xMultiplier = this.xMultiplier;
			rotationOverLifetimeModule.y = base.Write<ParticleSystem.MinMaxCurve>(rotationOverLifetimeModule.y, this.y, objects);
			rotationOverLifetimeModule.yMultiplier = this.yMultiplier;
			rotationOverLifetimeModule.z = base.Write<ParticleSystem.MinMaxCurve>(rotationOverLifetimeModule.z, this.z, objects);
			rotationOverLifetimeModule.zMultiplier = this.zMultiplier;
			rotationOverLifetimeModule.separateAxes = this.separateAxes;
			return rotationOverLifetimeModule;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002F7FC File Offset: 0x0002DBFC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.RotationOverLifetimeModule rotationOverLifetimeModule = (ParticleSystem.RotationOverLifetimeModule)obj;
			this.enabled = rotationOverLifetimeModule.enabled;
			this.x = base.Read<PersistentMinMaxCurve>(this.x, rotationOverLifetimeModule.x);
			this.xMultiplier = rotationOverLifetimeModule.xMultiplier;
			this.y = base.Read<PersistentMinMaxCurve>(this.y, rotationOverLifetimeModule.y);
			this.yMultiplier = rotationOverLifetimeModule.yMultiplier;
			this.z = base.Read<PersistentMinMaxCurve>(this.z, rotationOverLifetimeModule.z);
			this.zMultiplier = rotationOverLifetimeModule.zMultiplier;
			this.separateAxes = rotationOverLifetimeModule.separateAxes;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002F8B9 File Offset: 0x0002DCB9
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.x, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.y, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.z, dependencies, objects, allowNulls);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002F8F4 File Offset: 0x0002DCF4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.RotationOverLifetimeModule rotationOverLifetimeModule = (ParticleSystem.RotationOverLifetimeModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.x, rotationOverLifetimeModule.x, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.y, rotationOverLifetimeModule.y, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.z, rotationOverLifetimeModule.z, dependencies);
		}

		// Token: 0x04000702 RID: 1794
		public bool enabled;

		// Token: 0x04000703 RID: 1795
		public PersistentMinMaxCurve x;

		// Token: 0x04000704 RID: 1796
		public float xMultiplier;

		// Token: 0x04000705 RID: 1797
		public PersistentMinMaxCurve y;

		// Token: 0x04000706 RID: 1798
		public float yMultiplier;

		// Token: 0x04000707 RID: 1799
		public PersistentMinMaxCurve z;

		// Token: 0x04000708 RID: 1800
		public float zMultiplier;

		// Token: 0x04000709 RID: 1801
		public bool separateAxes;
	}
}
