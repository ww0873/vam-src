using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000176 RID: 374
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentHingeJoint : PersistentJoint
	{
		// Token: 0x06000833 RID: 2099 RVA: 0x00035AB8 File Offset: 0x00033EB8
		public PersistentHingeJoint()
		{
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00035AC0 File Offset: 0x00033EC0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			HingeJoint hingeJoint = (HingeJoint)obj;
			hingeJoint.motor = this.motor;
			hingeJoint.limits = this.limits;
			hingeJoint.spring = this.spring;
			hingeJoint.useMotor = this.useMotor;
			hingeJoint.useLimits = this.useLimits;
			hingeJoint.useSpring = this.useSpring;
			return hingeJoint;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00035B30 File Offset: 0x00033F30
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			HingeJoint hingeJoint = (HingeJoint)obj;
			this.motor = hingeJoint.motor;
			this.limits = hingeJoint.limits;
			this.spring = hingeJoint.spring;
			this.useMotor = hingeJoint.useMotor;
			this.useLimits = hingeJoint.useLimits;
			this.useSpring = hingeJoint.useSpring;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00035B9A File Offset: 0x00033F9A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040008DE RID: 2270
		public JointMotor motor;

		// Token: 0x040008DF RID: 2271
		public JointLimits limits;

		// Token: 0x040008E0 RID: 2272
		public JointSpring spring;

		// Token: 0x040008E1 RID: 2273
		public bool useMotor;

		// Token: 0x040008E2 RID: 2274
		public bool useLimits;

		// Token: 0x040008E3 RID: 2275
		public bool useSpring;
	}
}
