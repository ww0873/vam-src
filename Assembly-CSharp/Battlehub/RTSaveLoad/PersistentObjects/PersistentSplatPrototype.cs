using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001BC RID: 444
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSplatPrototype : PersistentData
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x00038ED5 File Offset: 0x000372D5
		public PersistentSplatPrototype()
		{
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00038EE0 File Offset: 0x000372E0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SplatPrototype splatPrototype = (SplatPrototype)obj;
			splatPrototype.texture = (Texture2D)objects.Get(this.texture);
			splatPrototype.normalMap = (Texture2D)objects.Get(this.normalMap);
			splatPrototype.tileSize = this.tileSize;
			splatPrototype.tileOffset = this.tileOffset;
			splatPrototype.specular = this.specular;
			splatPrototype.metallic = this.metallic;
			splatPrototype.smoothness = this.smoothness;
			return splatPrototype;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00038F74 File Offset: 0x00037374
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SplatPrototype splatPrototype = (SplatPrototype)obj;
			this.texture = splatPrototype.texture.GetMappedInstanceID();
			this.normalMap = splatPrototype.normalMap.GetMappedInstanceID();
			this.tileSize = splatPrototype.tileSize;
			this.tileOffset = splatPrototype.tileOffset;
			this.specular = splatPrototype.specular;
			this.metallic = splatPrototype.metallic;
			this.smoothness = splatPrototype.smoothness;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00038FF4 File Offset: 0x000373F4
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.texture, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.normalMap, dependencies, objects, allowNulls);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00039020 File Offset: 0x00037420
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			SplatPrototype splatPrototype = (SplatPrototype)obj;
			base.AddDependency(splatPrototype.texture, dependencies);
			base.AddDependency(splatPrototype.normalMap, dependencies);
		}

		// Token: 0x04000A24 RID: 2596
		public long texture;

		// Token: 0x04000A25 RID: 2597
		public long normalMap;

		// Token: 0x04000A26 RID: 2598
		public Vector2 tileSize;

		// Token: 0x04000A27 RID: 2599
		public Vector2 tileOffset;

		// Token: 0x04000A28 RID: 2600
		public Color specular;

		// Token: 0x04000A29 RID: 2601
		public float metallic;

		// Token: 0x04000A2A RID: 2602
		public float smoothness;
	}
}
