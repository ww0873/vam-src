using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200015A RID: 346
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentConstantForce : PersistentBehaviour
	{
		// Token: 0x060007BB RID: 1979 RVA: 0x00034001 File Offset: 0x00032401
		public PersistentConstantForce()
		{
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0003400C File Offset: 0x0003240C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ConstantForce constantForce = (ConstantForce)obj;
			constantForce.force = this.force;
			constantForce.relativeForce = this.relativeForce;
			constantForce.torque = this.torque;
			constantForce.relativeTorque = this.relativeTorque;
			return constantForce;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00034064 File Offset: 0x00032464
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ConstantForce constantForce = (ConstantForce)obj;
			this.force = constantForce.force;
			this.relativeForce = constantForce.relativeForce;
			this.torque = constantForce.torque;
			this.relativeTorque = constantForce.relativeTorque;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000340B6 File Offset: 0x000324B6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000872 RID: 2162
		public Vector3 force;

		// Token: 0x04000873 RID: 2163
		public Vector3 relativeForce;

		// Token: 0x04000874 RID: 2164
		public Vector3 torque;

		// Token: 0x04000875 RID: 2165
		public Vector3 relativeTorque;
	}
}
