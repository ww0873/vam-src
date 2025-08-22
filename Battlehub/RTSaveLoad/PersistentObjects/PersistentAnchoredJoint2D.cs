using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000127 RID: 295
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1093, typeof(PersistentSpringJoint2D))]
	[ProtoInclude(1094, typeof(PersistentDistanceJoint2D))]
	[ProtoInclude(1095, typeof(PersistentFrictionJoint2D))]
	[ProtoInclude(1096, typeof(PersistentHingeJoint2D))]
	[ProtoInclude(1097, typeof(PersistentSliderJoint2D))]
	[ProtoInclude(1098, typeof(PersistentFixedJoint2D))]
	[ProtoInclude(1099, typeof(PersistentWheelJoint2D))]
	[Serializable]
	public class PersistentAnchoredJoint2D : PersistentJoint2D
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x00030E74 File Offset: 0x0002F274
		public PersistentAnchoredJoint2D()
		{
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00030E7C File Offset: 0x0002F27C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AnchoredJoint2D anchoredJoint2D = (AnchoredJoint2D)obj;
			anchoredJoint2D.anchor = this.anchor;
			anchoredJoint2D.connectedAnchor = this.connectedAnchor;
			anchoredJoint2D.autoConfigureConnectedAnchor = this.autoConfigureConnectedAnchor;
			return anchoredJoint2D;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00030EC8 File Offset: 0x0002F2C8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AnchoredJoint2D anchoredJoint2D = (AnchoredJoint2D)obj;
			this.anchor = anchoredJoint2D.anchor;
			this.connectedAnchor = anchoredJoint2D.connectedAnchor;
			this.autoConfigureConnectedAnchor = anchoredJoint2D.autoConfigureConnectedAnchor;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00030F0E File Offset: 0x0002F30E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000761 RID: 1889
		public Vector2 anchor;

		// Token: 0x04000762 RID: 1890
		public Vector2 connectedAnchor;

		// Token: 0x04000763 RID: 1891
		public bool autoConfigureConnectedAnchor;
	}
}
