using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000153 RID: 339
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCircleCollider2D : PersistentCollider2D
	{
		// Token: 0x060007A0 RID: 1952 RVA: 0x00033B79 File Offset: 0x00031F79
		public PersistentCircleCollider2D()
		{
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00033B84 File Offset: 0x00031F84
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CircleCollider2D circleCollider2D = (CircleCollider2D)obj;
			circleCollider2D.radius = this.radius;
			return circleCollider2D;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00033BB8 File Offset: 0x00031FB8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CircleCollider2D circleCollider2D = (CircleCollider2D)obj;
			this.radius = circleCollider2D.radius;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00033BE6 File Offset: 0x00031FE6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000843 RID: 2115
		public float radius;
	}
}
