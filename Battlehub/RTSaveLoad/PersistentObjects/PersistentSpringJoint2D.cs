using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001BE RID: 446
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSpringJoint2D : PersistentAnchoredJoint2D
	{
		// Token: 0x0600092B RID: 2347 RVA: 0x00039135 File Offset: 0x00037535
		public PersistentSpringJoint2D()
		{
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00039140 File Offset: 0x00037540
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SpringJoint2D springJoint2D = (SpringJoint2D)obj;
			springJoint2D.autoConfigureDistance = this.autoConfigureDistance;
			springJoint2D.distance = this.distance;
			springJoint2D.dampingRatio = this.dampingRatio;
			springJoint2D.frequency = this.frequency;
			return springJoint2D;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00039198 File Offset: 0x00037598
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SpringJoint2D springJoint2D = (SpringJoint2D)obj;
			this.autoConfigureDistance = springJoint2D.autoConfigureDistance;
			this.distance = springJoint2D.distance;
			this.dampingRatio = springJoint2D.dampingRatio;
			this.frequency = springJoint2D.frequency;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000391EA File Offset: 0x000375EA
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A30 RID: 2608
		public bool autoConfigureDistance;

		// Token: 0x04000A31 RID: 2609
		public float distance;

		// Token: 0x04000A32 RID: 2610
		public float dampingRatio;

		// Token: 0x04000A33 RID: 2611
		public float frequency;
	}
}
