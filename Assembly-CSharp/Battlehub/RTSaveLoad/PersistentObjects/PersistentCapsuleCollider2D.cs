using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000150 RID: 336
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCapsuleCollider2D : PersistentCollider2D
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x000336D9 File Offset: 0x00031AD9
		public PersistentCapsuleCollider2D()
		{
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000336E4 File Offset: 0x00031AE4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CapsuleCollider2D capsuleCollider2D = (CapsuleCollider2D)obj;
			capsuleCollider2D.size = this.size;
			capsuleCollider2D.direction = (CapsuleDirection2D)this.direction;
			return capsuleCollider2D;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00033724 File Offset: 0x00031B24
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CapsuleCollider2D capsuleCollider2D = (CapsuleCollider2D)obj;
			this.size = capsuleCollider2D.size;
			this.direction = (uint)capsuleCollider2D.direction;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0003375E File Offset: 0x00031B5E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400082E RID: 2094
		public Vector2 size;

		// Token: 0x0400082F RID: 2095
		public uint direction;
	}
}
