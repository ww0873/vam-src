using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001BD RID: 445
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSpringJoint : PersistentJoint
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x0003905D File Offset: 0x0003745D
		public PersistentSpringJoint()
		{
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00039068 File Offset: 0x00037468
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SpringJoint springJoint = (SpringJoint)obj;
			springJoint.spring = this.spring;
			springJoint.damper = this.damper;
			springJoint.minDistance = this.minDistance;
			springJoint.maxDistance = this.maxDistance;
			springJoint.tolerance = this.tolerance;
			return springJoint;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x000390CC File Offset: 0x000374CC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SpringJoint springJoint = (SpringJoint)obj;
			this.spring = springJoint.spring;
			this.damper = springJoint.damper;
			this.minDistance = springJoint.minDistance;
			this.maxDistance = springJoint.maxDistance;
			this.tolerance = springJoint.tolerance;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0003912A File Offset: 0x0003752A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A2B RID: 2603
		public float spring;

		// Token: 0x04000A2C RID: 2604
		public float damper;

		// Token: 0x04000A2D RID: 2605
		public float minDistance;

		// Token: 0x04000A2E RID: 2606
		public float maxDistance;

		// Token: 0x04000A2F RID: 2607
		public float tolerance;
	}
}
