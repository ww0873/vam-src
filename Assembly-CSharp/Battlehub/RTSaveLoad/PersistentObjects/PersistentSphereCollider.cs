using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001BB RID: 443
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSphereCollider : PersistentCollider
	{
		// Token: 0x0600091E RID: 2334 RVA: 0x00038E45 File Offset: 0x00037245
		public PersistentSphereCollider()
		{
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00038E50 File Offset: 0x00037250
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SphereCollider sphereCollider = (SphereCollider)obj;
			sphereCollider.center = this.center;
			sphereCollider.radius = this.radius;
			return sphereCollider;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00038E90 File Offset: 0x00037290
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SphereCollider sphereCollider = (SphereCollider)obj;
			this.center = sphereCollider.center;
			this.radius = sphereCollider.radius;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00038ECA File Offset: 0x000372CA
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A22 RID: 2594
		public Vector3 center;

		// Token: 0x04000A23 RID: 2595
		public float radius;
	}
}
