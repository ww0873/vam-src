using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000155 RID: 341
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1087, typeof(PersistentCircleCollider2D))]
	[ProtoInclude(1088, typeof(PersistentBoxCollider2D))]
	[ProtoInclude(1089, typeof(PersistentEdgeCollider2D))]
	[ProtoInclude(1090, typeof(PersistentCapsuleCollider2D))]
	[ProtoInclude(1091, typeof(PersistentCompositeCollider2D))]
	[ProtoInclude(1092, typeof(PersistentPolygonCollider2D))]
	[Serializable]
	public class PersistentCollider2D : PersistentBehaviour
	{
		// Token: 0x060007A9 RID: 1961 RVA: 0x00032AC1 File Offset: 0x00030EC1
		public PersistentCollider2D()
		{
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00032ACC File Offset: 0x00030ECC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Collider2D collider2D = (Collider2D)obj;
			collider2D.density = this.density;
			collider2D.isTrigger = this.isTrigger;
			collider2D.usedByEffector = this.usedByEffector;
			collider2D.usedByComposite = this.usedByComposite;
			collider2D.offset = this.offset;
			collider2D.sharedMaterial = (PhysicsMaterial2D)objects.Get(this.sharedMaterial);
			return collider2D;
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00032B48 File Offset: 0x00030F48
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Collider2D collider2D = (Collider2D)obj;
			this.density = collider2D.density;
			this.isTrigger = collider2D.isTrigger;
			this.usedByEffector = collider2D.usedByEffector;
			this.usedByComposite = collider2D.usedByComposite;
			this.offset = collider2D.offset;
			this.sharedMaterial = collider2D.sharedMaterial.GetMappedInstanceID();
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00032BB7 File Offset: 0x00030FB7
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sharedMaterial, dependencies, objects, allowNulls);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00032BD4 File Offset: 0x00030FD4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Collider2D collider2D = (Collider2D)obj;
			base.AddDependency(collider2D.sharedMaterial, dependencies);
		}

		// Token: 0x04000848 RID: 2120
		public float density;

		// Token: 0x04000849 RID: 2121
		public bool isTrigger;

		// Token: 0x0400084A RID: 2122
		public bool usedByEffector;

		// Token: 0x0400084B RID: 2123
		public bool usedByComposite;

		// Token: 0x0400084C RID: 2124
		public Vector2 offset;

		// Token: 0x0400084D RID: 2125
		public long sharedMaterial;
	}
}
