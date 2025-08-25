using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200014F RID: 335
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCapsuleCollider : PersistentCollider
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x00033619 File Offset: 0x00031A19
		public PersistentCapsuleCollider()
		{
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00033624 File Offset: 0x00031A24
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CapsuleCollider capsuleCollider = (CapsuleCollider)obj;
			capsuleCollider.center = this.center;
			capsuleCollider.radius = this.radius;
			capsuleCollider.height = this.height;
			capsuleCollider.direction = this.direction;
			return capsuleCollider;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0003367C File Offset: 0x00031A7C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CapsuleCollider capsuleCollider = (CapsuleCollider)obj;
			this.center = capsuleCollider.center;
			this.radius = capsuleCollider.radius;
			this.height = capsuleCollider.height;
			this.direction = capsuleCollider.direction;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000336CE File Offset: 0x00031ACE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400082A RID: 2090
		public Vector3 center;

		// Token: 0x0400082B RID: 2091
		public float radius;

		// Token: 0x0400082C RID: 2092
		public float height;

		// Token: 0x0400082D RID: 2093
		public int direction;
	}
}
