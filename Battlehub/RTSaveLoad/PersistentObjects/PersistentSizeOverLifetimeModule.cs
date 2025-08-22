using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000122 RID: 290
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSizeOverLifetimeModule : PersistentData
	{
		// Token: 0x060006EC RID: 1772 RVA: 0x0003009B File Offset: 0x0002E49B
		public PersistentSizeOverLifetimeModule()
		{
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x000300A4 File Offset: 0x0002E4A4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule = (ParticleSystem.SizeOverLifetimeModule)obj;
			sizeOverLifetimeModule.enabled = this.enabled;
			sizeOverLifetimeModule.size = base.Write<ParticleSystem.MinMaxCurve>(sizeOverLifetimeModule.size, this.size, objects);
			sizeOverLifetimeModule.sizeMultiplier = this.sizeMultiplier;
			sizeOverLifetimeModule.x = base.Write<ParticleSystem.MinMaxCurve>(sizeOverLifetimeModule.x, this.x, objects);
			sizeOverLifetimeModule.xMultiplier = this.xMultiplier;
			sizeOverLifetimeModule.y = base.Write<ParticleSystem.MinMaxCurve>(sizeOverLifetimeModule.y, this.y, objects);
			sizeOverLifetimeModule.yMultiplier = this.yMultiplier;
			sizeOverLifetimeModule.z = base.Write<ParticleSystem.MinMaxCurve>(sizeOverLifetimeModule.z, this.z, objects);
			sizeOverLifetimeModule.zMultiplier = this.zMultiplier;
			sizeOverLifetimeModule.separateAxes = this.separateAxes;
			return sizeOverLifetimeModule;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0003018C File Offset: 0x0002E58C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule = (ParticleSystem.SizeOverLifetimeModule)obj;
			this.enabled = sizeOverLifetimeModule.enabled;
			this.size = base.Read<PersistentMinMaxCurve>(this.size, sizeOverLifetimeModule.size);
			this.sizeMultiplier = sizeOverLifetimeModule.sizeMultiplier;
			this.x = base.Read<PersistentMinMaxCurve>(this.x, sizeOverLifetimeModule.x);
			this.xMultiplier = sizeOverLifetimeModule.xMultiplier;
			this.y = base.Read<PersistentMinMaxCurve>(this.y, sizeOverLifetimeModule.y);
			this.yMultiplier = sizeOverLifetimeModule.yMultiplier;
			this.z = base.Read<PersistentMinMaxCurve>(this.z, sizeOverLifetimeModule.z);
			this.zMultiplier = sizeOverLifetimeModule.zMultiplier;
			this.separateAxes = sizeOverLifetimeModule.separateAxes;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00030274 File Offset: 0x0002E674
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.size, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.x, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.y, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.z, dependencies, objects, allowNulls);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000302C8 File Offset: 0x0002E6C8
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.SizeOverLifetimeModule sizeOverLifetimeModule = (ParticleSystem.SizeOverLifetimeModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.size, sizeOverLifetimeModule.size, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.x, sizeOverLifetimeModule.x, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.y, sizeOverLifetimeModule.y, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.z, sizeOverLifetimeModule.z, dependencies);
		}

		// Token: 0x04000731 RID: 1841
		public bool enabled;

		// Token: 0x04000732 RID: 1842
		public PersistentMinMaxCurve size;

		// Token: 0x04000733 RID: 1843
		public float sizeMultiplier;

		// Token: 0x04000734 RID: 1844
		public PersistentMinMaxCurve x;

		// Token: 0x04000735 RID: 1845
		public float xMultiplier;

		// Token: 0x04000736 RID: 1846
		public PersistentMinMaxCurve y;

		// Token: 0x04000737 RID: 1847
		public float yMultiplier;

		// Token: 0x04000738 RID: 1848
		public PersistentMinMaxCurve z;

		// Token: 0x04000739 RID: 1849
		public float zMultiplier;

		// Token: 0x0400073A RID: 1850
		public bool separateAxes;
	}
}
