using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B7 RID: 439
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSkybox : PersistentBehaviour
	{
		// Token: 0x06000910 RID: 2320 RVA: 0x00038BDE File Offset: 0x00036FDE
		public PersistentSkybox()
		{
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00038BE8 File Offset: 0x00036FE8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Skybox skybox = (Skybox)obj;
			skybox.material = (Material)objects.Get(this.material);
			return skybox;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00038C28 File Offset: 0x00037028
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Skybox skybox = (Skybox)obj;
			this.material = skybox.material.GetMappedInstanceID();
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00038C5B File Offset: 0x0003705B
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.material, dependencies, objects, allowNulls);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00038C78 File Offset: 0x00037078
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Skybox skybox = (Skybox)obj;
			base.AddDependency(skybox.material, dependencies);
		}

		// Token: 0x04000A18 RID: 2584
		public long material;
	}
}
