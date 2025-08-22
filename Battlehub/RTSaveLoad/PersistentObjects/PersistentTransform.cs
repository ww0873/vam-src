using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200021A RID: 538
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1084, typeof(PersistentRectTransform))]
	[Serializable]
	public class PersistentTransform : PersistentComponent
	{
		// Token: 0x06000AD1 RID: 2769 RVA: 0x00037E3C File Offset: 0x0003623C
		public PersistentTransform()
		{
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00037E44 File Offset: 0x00036244
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			Transform transform = (Transform)obj;
			transform.SetParent((Transform)objects.Get(this.parent), false);
			transform.position = this.position;
			transform.rotation = this.rotation;
			transform.localScale = this.localScale;
			return transform;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x00037EA0 File Offset: 0x000362A0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			Transform transform = (Transform)obj;
			this.position = transform.position;
			this.rotation = transform.rotation;
			this.localScale = transform.localScale;
			this.parent = transform.parent.GetMappedInstanceID();
		}

		// Token: 0x04000C03 RID: 3075
		public Vector3 position;

		// Token: 0x04000C04 RID: 3076
		public Quaternion rotation;

		// Token: 0x04000C05 RID: 3077
		public Vector3 localScale;

		// Token: 0x04000C06 RID: 3078
		public long parent;

		// Token: 0x04000C07 RID: 3079
		public int hierarchyCapacity;
	}
}
