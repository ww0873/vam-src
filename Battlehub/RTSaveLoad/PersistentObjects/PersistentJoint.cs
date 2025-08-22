using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200017B RID: 379
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1072, typeof(PersistentHingeJoint))]
	[ProtoInclude(1073, typeof(PersistentSpringJoint))]
	[ProtoInclude(1074, typeof(PersistentFixedJoint))]
	[ProtoInclude(1075, typeof(PersistentCharacterJoint))]
	[ProtoInclude(1076, typeof(PersistentConfigurableJoint))]
	[Serializable]
	public class PersistentJoint : PersistentComponent
	{
		// Token: 0x06000845 RID: 2117 RVA: 0x000338A1 File Offset: 0x00031CA1
		public PersistentJoint()
		{
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x000338AC File Offset: 0x00031CAC
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Joint joint = (Joint)obj;
			joint.connectedBody = (Rigidbody)objects.Get(this.connectedBody);
			joint.axis = this.axis;
			joint.anchor = this.anchor;
			joint.connectedAnchor = this.connectedAnchor;
			joint.autoConfigureConnectedAnchor = this.autoConfigureConnectedAnchor;
			joint.breakForce = this.breakForce;
			joint.breakTorque = this.breakTorque;
			joint.enableCollision = this.enableCollision;
			joint.enablePreprocessing = this.enablePreprocessing;
			return joint;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0003394C File Offset: 0x00031D4C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Joint joint = (Joint)obj;
			this.connectedBody = joint.connectedBody.GetMappedInstanceID();
			this.axis = joint.axis;
			this.anchor = joint.anchor;
			this.connectedAnchor = joint.connectedAnchor;
			this.autoConfigureConnectedAnchor = joint.autoConfigureConnectedAnchor;
			this.breakForce = joint.breakForce;
			this.breakTorque = joint.breakTorque;
			this.enableCollision = joint.enableCollision;
			this.enablePreprocessing = joint.enablePreprocessing;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x000339DF File Offset: 0x00031DDF
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.connectedBody, dependencies, objects, allowNulls);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000339FC File Offset: 0x00031DFC
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Joint joint = (Joint)obj;
			base.AddDependency(joint.connectedBody, dependencies);
		}

		// Token: 0x040008F7 RID: 2295
		public long connectedBody;

		// Token: 0x040008F8 RID: 2296
		public Vector3 axis;

		// Token: 0x040008F9 RID: 2297
		public Vector3 anchor;

		// Token: 0x040008FA RID: 2298
		public Vector3 connectedAnchor;

		// Token: 0x040008FB RID: 2299
		public bool autoConfigureConnectedAnchor;

		// Token: 0x040008FC RID: 2300
		public float breakForce;

		// Token: 0x040008FD RID: 2301
		public float breakTorque;

		// Token: 0x040008FE RID: 2302
		public bool enableCollision;

		// Token: 0x040008FF RID: 2303
		public bool enablePreprocessing;
	}
}
