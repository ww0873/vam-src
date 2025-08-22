using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200016B RID: 363
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentFrictionJoint2D : PersistentAnchoredJoint2D
	{
		// Token: 0x0600080C RID: 2060 RVA: 0x000348AC File Offset: 0x00032CAC
		public PersistentFrictionJoint2D()
		{
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000348B4 File Offset: 0x00032CB4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			FrictionJoint2D frictionJoint2D = (FrictionJoint2D)obj;
			frictionJoint2D.maxForce = this.maxForce;
			frictionJoint2D.maxTorque = this.maxTorque;
			return frictionJoint2D;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000348F4 File Offset: 0x00032CF4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			FrictionJoint2D frictionJoint2D = (FrictionJoint2D)obj;
			this.maxForce = frictionJoint2D.maxForce;
			this.maxTorque = frictionJoint2D.maxTorque;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0003492E File Offset: 0x00032D2E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400089B RID: 2203
		public float maxForce;

		// Token: 0x0400089C RID: 2204
		public float maxTorque;
	}
}
