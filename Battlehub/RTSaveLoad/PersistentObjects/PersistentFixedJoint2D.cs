using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000167 RID: 359
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentFixedJoint2D : PersistentAnchoredJoint2D
	{
		// Token: 0x06000801 RID: 2049 RVA: 0x00034714 File Offset: 0x00032B14
		public PersistentFixedJoint2D()
		{
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0003471C File Offset: 0x00032B1C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			FixedJoint2D fixedJoint2D = (FixedJoint2D)obj;
			fixedJoint2D.dampingRatio = this.dampingRatio;
			fixedJoint2D.frequency = this.frequency;
			return fixedJoint2D;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0003475C File Offset: 0x00032B5C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			FixedJoint2D fixedJoint2D = (FixedJoint2D)obj;
			this.dampingRatio = fixedJoint2D.dampingRatio;
			this.frequency = fixedJoint2D.frequency;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00034796 File Offset: 0x00032B96
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000896 RID: 2198
		public float dampingRatio;

		// Token: 0x04000897 RID: 2199
		public float frequency;
	}
}
