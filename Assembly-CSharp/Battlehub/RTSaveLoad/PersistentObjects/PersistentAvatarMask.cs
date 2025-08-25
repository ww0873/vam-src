using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200013F RID: 319
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAvatarMask : PersistentObject
	{
		// Token: 0x06000755 RID: 1877 RVA: 0x00032355 File Offset: 0x00030755
		public PersistentAvatarMask()
		{
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00032360 File Offset: 0x00030760
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AvatarMask avatarMask = (AvatarMask)obj;
			avatarMask.transformCount = this.transformCount;
			return avatarMask;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00032394 File Offset: 0x00030794
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AvatarMask avatarMask = (AvatarMask)obj;
			this.transformCount = avatarMask.transformCount;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000323C2 File Offset: 0x000307C2
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040007D5 RID: 2005
		public int transformCount;
	}
}
