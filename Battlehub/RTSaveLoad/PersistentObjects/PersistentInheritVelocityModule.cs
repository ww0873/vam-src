using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000115 RID: 277
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentInheritVelocityModule : PersistentData
	{
		// Token: 0x060006AD RID: 1709 RVA: 0x0002D9E9 File Offset: 0x0002BDE9
		public PersistentInheritVelocityModule()
		{
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0002D9F4 File Offset: 0x0002BDF4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.InheritVelocityModule inheritVelocityModule = (ParticleSystem.InheritVelocityModule)obj;
			inheritVelocityModule.enabled = this.enabled;
			inheritVelocityModule.mode = (ParticleSystemInheritVelocityMode)this.mode;
			inheritVelocityModule.curve = base.Write<ParticleSystem.MinMaxCurve>(inheritVelocityModule.curve, this.curve, objects);
			inheritVelocityModule.curveMultiplier = this.curveMultiplier;
			return inheritVelocityModule;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0002DA64 File Offset: 0x0002BE64
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.InheritVelocityModule inheritVelocityModule = (ParticleSystem.InheritVelocityModule)obj;
			this.enabled = inheritVelocityModule.enabled;
			this.mode = (uint)inheritVelocityModule.mode;
			this.curve = base.Read<PersistentMinMaxCurve>(this.curve, inheritVelocityModule.curve);
			this.curveMultiplier = inheritVelocityModule.curveMultiplier;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002DACB File Offset: 0x0002BECB
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.curve, dependencies, objects, allowNulls);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002DAE8 File Offset: 0x0002BEE8
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.InheritVelocityModule inheritVelocityModule = (ParticleSystem.InheritVelocityModule)obj;
			base.GetDependencies<PersistentMinMaxCurve>(this.curve, inheritVelocityModule.curve, dependencies);
		}

		// Token: 0x0400067C RID: 1660
		public bool enabled;

		// Token: 0x0400067D RID: 1661
		public uint mode;

		// Token: 0x0400067E RID: 1662
		public PersistentMinMaxCurve curve;

		// Token: 0x0400067F RID: 1663
		public float curveMultiplier;
	}
}
