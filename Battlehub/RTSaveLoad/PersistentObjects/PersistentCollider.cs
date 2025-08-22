using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000154 RID: 340
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1077, typeof(PersistentBoxCollider))]
	[ProtoInclude(1078, typeof(PersistentSphereCollider))]
	[ProtoInclude(1079, typeof(PersistentMeshCollider))]
	[ProtoInclude(1080, typeof(PersistentCapsuleCollider))]
	[ProtoInclude(1081, typeof(PersistentCharacterController))]
	[ProtoInclude(1082, typeof(PersistentWheelCollider))]
	[ProtoInclude(1083, typeof(PersistentTerrainCollider))]
	[Serializable]
	public class PersistentCollider : PersistentComponent
	{
		// Token: 0x060007A4 RID: 1956 RVA: 0x00032924 File Offset: 0x00030D24
		public PersistentCollider()
		{
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0003292C File Offset: 0x00030D2C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Collider collider = (Collider)obj;
			collider.enabled = this.enabled;
			collider.isTrigger = this.isTrigger;
			collider.contactOffset = this.contactOffset;
			collider.sharedMaterial = (PhysicMaterial)objects.Get(this.sharedMaterial);
			return collider;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00032990 File Offset: 0x00030D90
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Collider collider = (Collider)obj;
			this.enabled = collider.enabled;
			this.isTrigger = collider.isTrigger;
			this.contactOffset = collider.contactOffset;
			this.sharedMaterial = collider.sharedMaterial.GetMappedInstanceID();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000329E7 File Offset: 0x00030DE7
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sharedMaterial, dependencies, objects, allowNulls);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00032A04 File Offset: 0x00030E04
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Collider collider = (Collider)obj;
			base.AddDependency(collider.sharedMaterial, dependencies);
		}

		// Token: 0x04000844 RID: 2116
		public bool enabled;

		// Token: 0x04000845 RID: 2117
		public bool isTrigger;

		// Token: 0x04000846 RID: 2118
		public float contactOffset;

		// Token: 0x04000847 RID: 2119
		public long sharedMaterial;
	}
}
