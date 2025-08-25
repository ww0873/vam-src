using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001A1 RID: 417
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPolygonCollider2D : PersistentCollider2D
	{
		// Token: 0x060008CD RID: 2253 RVA: 0x00037B21 File Offset: 0x00035F21
		public PersistentPolygonCollider2D()
		{
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00037B2C File Offset: 0x00035F2C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			PolygonCollider2D polygonCollider2D = (PolygonCollider2D)obj;
			polygonCollider2D.points = this.points;
			polygonCollider2D.pathCount = this.pathCount;
			polygonCollider2D.autoTiling = this.autoTiling;
			return polygonCollider2D;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00037B78 File Offset: 0x00035F78
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			PolygonCollider2D polygonCollider2D = (PolygonCollider2D)obj;
			this.points = polygonCollider2D.points;
			this.pathCount = polygonCollider2D.pathCount;
			this.autoTiling = polygonCollider2D.autoTiling;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00037BBE File Offset: 0x00035FBE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400099D RID: 2461
		public Vector2[] points;

		// Token: 0x0400099E RID: 2462
		public int pathCount;

		// Token: 0x0400099F RID: 2463
		public bool autoTiling;
	}
}
