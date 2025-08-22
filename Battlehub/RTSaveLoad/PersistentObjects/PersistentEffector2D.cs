using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000163 RID: 355
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1101, typeof(PersistentAreaEffector2D))]
	[ProtoInclude(1102, typeof(PersistentPlatformEffector2D))]
	[ProtoInclude(1103, typeof(PersistentBuoyancyEffector2D))]
	[ProtoInclude(1104, typeof(PersistentPointEffector2D))]
	[ProtoInclude(1105, typeof(PersistentSurfaceEffector2D))]
	[Serializable]
	public class PersistentEffector2D : PersistentBehaviour
	{
		// Token: 0x060007F6 RID: 2038 RVA: 0x00031454 File Offset: 0x0002F854
		public PersistentEffector2D()
		{
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0003145C File Offset: 0x0002F85C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Effector2D effector2D = (Effector2D)obj;
			effector2D.useColliderMask = this.useColliderMask;
			effector2D.colliderMask = this.colliderMask;
			return effector2D;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0003149C File Offset: 0x0002F89C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Effector2D effector2D = (Effector2D)obj;
			this.useColliderMask = effector2D.useColliderMask;
			this.colliderMask = effector2D.colliderMask;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x000314D6 File Offset: 0x0002F8D6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000891 RID: 2193
		public bool useColliderMask;

		// Token: 0x04000892 RID: 2194
		public int colliderMask;
	}
}
