using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200019A RID: 410
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentPhysicsMaterial2D : PersistentObject
	{
		// Token: 0x060008BA RID: 2234 RVA: 0x00037859 File Offset: 0x00035C59
		public PersistentPhysicsMaterial2D()
		{
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00037864 File Offset: 0x00035C64
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			PhysicsMaterial2D physicsMaterial2D = (PhysicsMaterial2D)obj;
			physicsMaterial2D.bounciness = this.bounciness;
			physicsMaterial2D.friction = this.friction;
			return physicsMaterial2D;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x000378A4 File Offset: 0x00035CA4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			PhysicsMaterial2D physicsMaterial2D = (PhysicsMaterial2D)obj;
			this.bounciness = physicsMaterial2D.bounciness;
			this.friction = physicsMaterial2D.friction;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000378DE File Offset: 0x00035CDE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400098B RID: 2443
		public float bounciness;

		// Token: 0x0400098C RID: 2444
		public float friction;
	}
}
