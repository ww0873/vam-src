using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001D7 RID: 471
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentWheelCollider : PersistentCollider
	{
		// Token: 0x06000982 RID: 2434 RVA: 0x0003AC59 File Offset: 0x00039059
		public PersistentWheelCollider()
		{
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0003AC64 File Offset: 0x00039064
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			WheelCollider wheelCollider = (WheelCollider)obj;
			wheelCollider.center = this.center;
			wheelCollider.radius = this.radius;
			wheelCollider.suspensionDistance = this.suspensionDistance;
			wheelCollider.suspensionSpring = this.suspensionSpring;
			wheelCollider.forceAppPointDistance = this.forceAppPointDistance;
			wheelCollider.mass = this.mass;
			wheelCollider.wheelDampingRate = this.wheelDampingRate;
			wheelCollider.forwardFriction = this.forwardFriction;
			wheelCollider.sidewaysFriction = this.sidewaysFriction;
			wheelCollider.motorTorque = this.motorTorque;
			wheelCollider.brakeTorque = this.brakeTorque;
			wheelCollider.steerAngle = this.steerAngle;
			return wheelCollider;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0003AD1C File Offset: 0x0003911C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			WheelCollider wheelCollider = (WheelCollider)obj;
			this.center = wheelCollider.center;
			this.radius = wheelCollider.radius;
			this.suspensionDistance = wheelCollider.suspensionDistance;
			this.suspensionSpring = wheelCollider.suspensionSpring;
			this.forceAppPointDistance = wheelCollider.forceAppPointDistance;
			this.mass = wheelCollider.mass;
			this.wheelDampingRate = wheelCollider.wheelDampingRate;
			this.forwardFriction = wheelCollider.forwardFriction;
			this.sidewaysFriction = wheelCollider.sidewaysFriction;
			this.motorTorque = wheelCollider.motorTorque;
			this.brakeTorque = wheelCollider.brakeTorque;
			this.steerAngle = wheelCollider.steerAngle;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0003ADCE File Offset: 0x000391CE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000ABC RID: 2748
		public Vector3 center;

		// Token: 0x04000ABD RID: 2749
		public float radius;

		// Token: 0x04000ABE RID: 2750
		public float suspensionDistance;

		// Token: 0x04000ABF RID: 2751
		public JointSpring suspensionSpring;

		// Token: 0x04000AC0 RID: 2752
		public float forceAppPointDistance;

		// Token: 0x04000AC1 RID: 2753
		public float mass;

		// Token: 0x04000AC2 RID: 2754
		public float wheelDampingRate;

		// Token: 0x04000AC3 RID: 2755
		public WheelFrictionCurve forwardFriction;

		// Token: 0x04000AC4 RID: 2756
		public WheelFrictionCurve sidewaysFriction;

		// Token: 0x04000AC5 RID: 2757
		public float motorTorque;

		// Token: 0x04000AC6 RID: 2758
		public float brakeTorque;

		// Token: 0x04000AC7 RID: 2759
		public float steerAngle;
	}
}
