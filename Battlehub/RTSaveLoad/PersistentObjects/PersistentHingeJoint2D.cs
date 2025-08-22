using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000177 RID: 375
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentHingeJoint2D : PersistentAnchoredJoint2D
	{
		// Token: 0x06000837 RID: 2103 RVA: 0x00035BA5 File Offset: 0x00033FA5
		public PersistentHingeJoint2D()
		{
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00035BB0 File Offset: 0x00033FB0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			HingeJoint2D hingeJoint2D = (HingeJoint2D)obj;
			hingeJoint2D.useMotor = this.useMotor;
			hingeJoint2D.useLimits = this.useLimits;
			hingeJoint2D.motor = this.motor;
			hingeJoint2D.limits = this.limits;
			return hingeJoint2D;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00035C08 File Offset: 0x00034008
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			HingeJoint2D hingeJoint2D = (HingeJoint2D)obj;
			this.useMotor = hingeJoint2D.useMotor;
			this.useLimits = hingeJoint2D.useLimits;
			this.motor = hingeJoint2D.motor;
			this.limits = hingeJoint2D.limits;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00035C5A File Offset: 0x0003405A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040008E4 RID: 2276
		public bool useMotor;

		// Token: 0x040008E5 RID: 2277
		public bool useLimits;

		// Token: 0x040008E6 RID: 2278
		public JointMotor2D motor;

		// Token: 0x040008E7 RID: 2279
		public JointAngleLimits2D limits;
	}
}
