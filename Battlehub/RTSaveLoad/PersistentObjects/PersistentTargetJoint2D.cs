using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C4 RID: 452
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTargetJoint2D : PersistentJoint2D
	{
		// Token: 0x0600093E RID: 2366 RVA: 0x0003956D File Offset: 0x0003796D
		public PersistentTargetJoint2D()
		{
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00039578 File Offset: 0x00037978
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			TargetJoint2D targetJoint2D = (TargetJoint2D)obj;
			targetJoint2D.anchor = this.anchor;
			targetJoint2D.target = this.target;
			targetJoint2D.autoConfigureTarget = this.autoConfigureTarget;
			targetJoint2D.maxForce = this.maxForce;
			targetJoint2D.dampingRatio = this.dampingRatio;
			targetJoint2D.frequency = this.frequency;
			return targetJoint2D;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x000395E8 File Offset: 0x000379E8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			TargetJoint2D targetJoint2D = (TargetJoint2D)obj;
			this.anchor = targetJoint2D.anchor;
			this.target = targetJoint2D.target;
			this.autoConfigureTarget = targetJoint2D.autoConfigureTarget;
			this.maxForce = targetJoint2D.maxForce;
			this.dampingRatio = targetJoint2D.dampingRatio;
			this.frequency = targetJoint2D.frequency;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00039652 File Offset: 0x00037A52
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A49 RID: 2633
		public Vector2 anchor;

		// Token: 0x04000A4A RID: 2634
		public Vector2 target;

		// Token: 0x04000A4B RID: 2635
		public bool autoConfigureTarget;

		// Token: 0x04000A4C RID: 2636
		public float maxForce;

		// Token: 0x04000A4D RID: 2637
		public float dampingRatio;

		// Token: 0x04000A4E RID: 2638
		public float frequency;
	}
}
