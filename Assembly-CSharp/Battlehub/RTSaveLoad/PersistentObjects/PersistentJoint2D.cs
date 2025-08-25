using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200017C RID: 380
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1084, typeof(PersistentAnchoredJoint2D))]
	[ProtoInclude(1085, typeof(PersistentRelativeJoint2D))]
	[ProtoInclude(1086, typeof(PersistentTargetJoint2D))]
	[Serializable]
	public class PersistentJoint2D : PersistentBehaviour
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x00030D61 File Offset: 0x0002F161
		public PersistentJoint2D()
		{
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00030D6C File Offset: 0x0002F16C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Joint2D joint2D = (Joint2D)obj;
			joint2D.connectedBody = (Rigidbody2D)objects.Get(this.connectedBody);
			joint2D.enableCollision = this.enableCollision;
			joint2D.breakForce = this.breakForce;
			joint2D.breakTorque = this.breakTorque;
			return joint2D;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00030DD0 File Offset: 0x0002F1D0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Joint2D joint2D = (Joint2D)obj;
			this.connectedBody = joint2D.connectedBody.GetMappedInstanceID();
			this.enableCollision = joint2D.enableCollision;
			this.breakForce = joint2D.breakForce;
			this.breakTorque = joint2D.breakTorque;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00030E27 File Offset: 0x0002F227
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.connectedBody, dependencies, objects, allowNulls);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00030E44 File Offset: 0x0002F244
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Joint2D joint2D = (Joint2D)obj;
			base.AddDependency(joint2D.connectedBody, dependencies);
		}

		// Token: 0x04000900 RID: 2304
		public long connectedBody;

		// Token: 0x04000901 RID: 2305
		public bool enableCollision;

		// Token: 0x04000902 RID: 2306
		public float breakForce;

		// Token: 0x04000903 RID: 2307
		public float breakTorque;
	}
}
