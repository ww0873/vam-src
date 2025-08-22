using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001C6 RID: 454
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTerrainCollider : PersistentCollider
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x00039995 File Offset: 0x00037D95
		public PersistentTerrainCollider()
		{
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x000399A0 File Offset: 0x00037DA0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			TerrainCollider terrainCollider = (TerrainCollider)obj;
			terrainCollider.terrainData = (TerrainData)objects.Get(this.terrainData);
			return terrainCollider;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000399E0 File Offset: 0x00037DE0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			TerrainCollider terrainCollider = (TerrainCollider)obj;
			this.terrainData = terrainCollider.terrainData.GetMappedInstanceID();
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00039A13 File Offset: 0x00037E13
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.terrainData, dependencies, objects, allowNulls);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00039A30 File Offset: 0x00037E30
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			TerrainCollider terrainCollider = (TerrainCollider)obj;
			base.AddDependency(terrainCollider.terrainData, dependencies);
		}

		// Token: 0x04000A68 RID: 2664
		public long terrainData;
	}
}
