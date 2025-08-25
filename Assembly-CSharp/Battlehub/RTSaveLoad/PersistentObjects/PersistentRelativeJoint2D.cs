using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001AA RID: 426
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRelativeJoint2D : PersistentJoint2D
	{
		// Token: 0x060008E8 RID: 2280 RVA: 0x000382B5 File Offset: 0x000366B5
		public PersistentRelativeJoint2D()
		{
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x000382C0 File Offset: 0x000366C0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			RelativeJoint2D relativeJoint2D = (RelativeJoint2D)obj;
			relativeJoint2D.maxForce = this.maxForce;
			relativeJoint2D.maxTorque = this.maxTorque;
			relativeJoint2D.correctionScale = this.correctionScale;
			relativeJoint2D.autoConfigureOffset = this.autoConfigureOffset;
			relativeJoint2D.linearOffset = this.linearOffset;
			relativeJoint2D.angularOffset = this.angularOffset;
			return relativeJoint2D;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00038330 File Offset: 0x00036730
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			RelativeJoint2D relativeJoint2D = (RelativeJoint2D)obj;
			this.maxForce = relativeJoint2D.maxForce;
			this.maxTorque = relativeJoint2D.maxTorque;
			this.correctionScale = relativeJoint2D.correctionScale;
			this.autoConfigureOffset = relativeJoint2D.autoConfigureOffset;
			this.linearOffset = relativeJoint2D.linearOffset;
			this.angularOffset = relativeJoint2D.angularOffset;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0003839A File Offset: 0x0003679A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040009C5 RID: 2501
		public float maxForce;

		// Token: 0x040009C6 RID: 2502
		public float maxTorque;

		// Token: 0x040009C7 RID: 2503
		public float correctionScale;

		// Token: 0x040009C8 RID: 2504
		public bool autoConfigureOffset;

		// Token: 0x040009C9 RID: 2505
		public Vector2 linearOffset;

		// Token: 0x040009CA RID: 2506
		public float angularOffset;
	}
}
