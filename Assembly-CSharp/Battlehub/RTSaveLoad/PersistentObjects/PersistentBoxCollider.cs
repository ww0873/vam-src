using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000147 RID: 327
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentBoxCollider : PersistentCollider
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x00032A34 File Offset: 0x00030E34
		public PersistentBoxCollider()
		{
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00032A3C File Offset: 0x00030E3C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			BoxCollider boxCollider = (BoxCollider)obj;
			boxCollider.center = this.center;
			boxCollider.size = this.size;
			return boxCollider;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00032A7C File Offset: 0x00030E7C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			BoxCollider boxCollider = (BoxCollider)obj;
			this.center = boxCollider.center;
			this.size = boxCollider.size;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x00032AB6 File Offset: 0x00030EB6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040007DE RID: 2014
		public Vector3 center;

		// Token: 0x040007DF RID: 2015
		public Vector3 size;
	}
}
