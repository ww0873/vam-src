using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000162 RID: 354
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentEdgeCollider2D : PersistentCollider2D
	{
		// Token: 0x060007F2 RID: 2034 RVA: 0x00034579 File Offset: 0x00032979
		public PersistentEdgeCollider2D()
		{
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00034584 File Offset: 0x00032984
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			EdgeCollider2D edgeCollider2D = (EdgeCollider2D)obj;
			edgeCollider2D.edgeRadius = this.edgeRadius;
			edgeCollider2D.points = this.points;
			return edgeCollider2D;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000345C4 File Offset: 0x000329C4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			EdgeCollider2D edgeCollider2D = (EdgeCollider2D)obj;
			this.edgeRadius = edgeCollider2D.edgeRadius;
			this.points = edgeCollider2D.points;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000345FE File Offset: 0x000329FE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400088F RID: 2191
		public float edgeRadius;

		// Token: 0x04000890 RID: 2192
		public Vector2[] points;
	}
}
