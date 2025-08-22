using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001D8 RID: 472
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentWheelJoint2D : PersistentAnchoredJoint2D
	{
		// Token: 0x06000986 RID: 2438 RVA: 0x0003ADD9 File Offset: 0x000391D9
		public PersistentWheelJoint2D()
		{
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0003ADE4 File Offset: 0x000391E4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			WheelJoint2D wheelJoint2D = (WheelJoint2D)obj;
			wheelJoint2D.suspension = this.suspension;
			wheelJoint2D.useMotor = this.useMotor;
			wheelJoint2D.motor = this.motor;
			return wheelJoint2D;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0003AE30 File Offset: 0x00039230
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			WheelJoint2D wheelJoint2D = (WheelJoint2D)obj;
			this.suspension = wheelJoint2D.suspension;
			this.useMotor = wheelJoint2D.useMotor;
			this.motor = wheelJoint2D.motor;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0003AE76 File Offset: 0x00039276
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000AC8 RID: 2760
		public JointSuspension2D suspension;

		// Token: 0x04000AC9 RID: 2761
		public bool useMotor;

		// Token: 0x04000ACA RID: 2762
		public JointMotor2D motor;
	}
}
