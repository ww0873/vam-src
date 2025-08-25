using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000161 RID: 353
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentDistanceJoint2D : PersistentAnchoredJoint2D
	{
		// Token: 0x060007EE RID: 2030 RVA: 0x000344D1 File Offset: 0x000328D1
		public PersistentDistanceJoint2D()
		{
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000344DC File Offset: 0x000328DC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			DistanceJoint2D distanceJoint2D = (DistanceJoint2D)obj;
			distanceJoint2D.autoConfigureDistance = this.autoConfigureDistance;
			distanceJoint2D.distance = this.distance;
			distanceJoint2D.maxDistanceOnly = this.maxDistanceOnly;
			return distanceJoint2D;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00034528 File Offset: 0x00032928
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			DistanceJoint2D distanceJoint2D = (DistanceJoint2D)obj;
			this.autoConfigureDistance = distanceJoint2D.autoConfigureDistance;
			this.distance = distanceJoint2D.distance;
			this.maxDistanceOnly = distanceJoint2D.maxDistanceOnly;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0003456E File Offset: 0x0003296E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400088C RID: 2188
		public bool autoConfigureDistance;

		// Token: 0x0400088D RID: 2189
		public float distance;

		// Token: 0x0400088E RID: 2190
		public bool maxDistanceOnly;
	}
}
