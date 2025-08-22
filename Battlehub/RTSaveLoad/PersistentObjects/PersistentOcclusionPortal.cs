using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000194 RID: 404
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentOcclusionPortal : PersistentComponent
	{
		// Token: 0x060008A6 RID: 2214 RVA: 0x00037239 File Offset: 0x00035639
		public PersistentOcclusionPortal()
		{
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00037244 File Offset: 0x00035644
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			OcclusionPortal occlusionPortal = (OcclusionPortal)obj;
			occlusionPortal.open = this.open;
			return occlusionPortal;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00037278 File Offset: 0x00035678
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			OcclusionPortal occlusionPortal = (OcclusionPortal)obj;
			this.open = occlusionPortal.open;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x000372A6 File Offset: 0x000356A6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000971 RID: 2417
		public bool open;
	}
}
