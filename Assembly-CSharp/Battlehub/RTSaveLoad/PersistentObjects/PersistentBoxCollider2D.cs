using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000148 RID: 328
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentBoxCollider2D : PersistentCollider2D
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x00032C04 File Offset: 0x00031004
		public PersistentBoxCollider2D()
		{
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00032C0C File Offset: 0x0003100C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			BoxCollider2D boxCollider2D = (BoxCollider2D)obj;
			boxCollider2D.size = this.size;
			boxCollider2D.edgeRadius = this.edgeRadius;
			boxCollider2D.autoTiling = this.autoTiling;
			return boxCollider2D;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00032C58 File Offset: 0x00031058
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			BoxCollider2D boxCollider2D = (BoxCollider2D)obj;
			this.size = boxCollider2D.size;
			this.edgeRadius = boxCollider2D.edgeRadius;
			this.autoTiling = boxCollider2D.autoTiling;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00032C9E File Offset: 0x0003109E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040007E0 RID: 2016
		public Vector2 size;

		// Token: 0x040007E1 RID: 2017
		public float edgeRadius;

		// Token: 0x040007E2 RID: 2018
		public bool autoTiling;
	}
}
