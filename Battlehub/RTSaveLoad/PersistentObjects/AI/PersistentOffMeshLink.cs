using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.AI;

namespace Battlehub.RTSaveLoad.PersistentObjects.AI
{
	// Token: 0x02000195 RID: 405
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentOffMeshLink : PersistentBehaviour
	{
		// Token: 0x060008AA RID: 2218 RVA: 0x000372B1 File Offset: 0x000356B1
		public PersistentOffMeshLink()
		{
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000372BC File Offset: 0x000356BC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			OffMeshLink offMeshLink = (OffMeshLink)obj;
			offMeshLink.activated = this.activated;
			offMeshLink.costOverride = this.costOverride;
			offMeshLink.biDirectional = this.biDirectional;
			offMeshLink.area = this.area;
			offMeshLink.autoUpdatePositions = this.autoUpdatePositions;
			offMeshLink.startTransform = (Transform)objects.Get(this.startTransform);
			offMeshLink.endTransform = (Transform)objects.Get(this.endTransform);
			return offMeshLink;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00037350 File Offset: 0x00035750
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			OffMeshLink offMeshLink = (OffMeshLink)obj;
			this.activated = offMeshLink.activated;
			this.costOverride = offMeshLink.costOverride;
			this.biDirectional = offMeshLink.biDirectional;
			this.area = offMeshLink.area;
			this.autoUpdatePositions = offMeshLink.autoUpdatePositions;
			this.startTransform = offMeshLink.startTransform.GetMappedInstanceID();
			this.endTransform = offMeshLink.endTransform.GetMappedInstanceID();
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x000373D0 File Offset: 0x000357D0
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.startTransform, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.endTransform, dependencies, objects, allowNulls);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000373FC File Offset: 0x000357FC
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			OffMeshLink offMeshLink = (OffMeshLink)obj;
			base.AddDependency(offMeshLink.startTransform, dependencies);
			base.AddDependency(offMeshLink.endTransform, dependencies);
		}

		// Token: 0x04000972 RID: 2418
		public bool activated;

		// Token: 0x04000973 RID: 2419
		public float costOverride;

		// Token: 0x04000974 RID: 2420
		public bool biDirectional;

		// Token: 0x04000975 RID: 2421
		public int area;

		// Token: 0x04000976 RID: 2422
		public bool autoUpdatePositions;

		// Token: 0x04000977 RID: 2423
		public long startTransform;

		// Token: 0x04000978 RID: 2424
		public long endTransform;
	}
}
