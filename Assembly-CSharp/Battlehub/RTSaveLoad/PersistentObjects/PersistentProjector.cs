using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001A4 RID: 420
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentProjector : PersistentBehaviour
	{
		// Token: 0x060008D3 RID: 2259 RVA: 0x00037BD9 File Offset: 0x00035FD9
		public PersistentProjector()
		{
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00037BE4 File Offset: 0x00035FE4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Projector projector = (Projector)obj;
			projector.nearClipPlane = this.nearClipPlane;
			projector.farClipPlane = this.farClipPlane;
			projector.fieldOfView = this.fieldOfView;
			projector.aspectRatio = this.aspectRatio;
			projector.orthographic = this.orthographic;
			projector.orthographicSize = this.orthographicSize;
			projector.ignoreLayers = this.ignoreLayers;
			projector.material = (Material)objects.Get(this.material);
			return projector;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00037C78 File Offset: 0x00036078
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Projector projector = (Projector)obj;
			this.nearClipPlane = projector.nearClipPlane;
			this.farClipPlane = projector.farClipPlane;
			this.fieldOfView = projector.fieldOfView;
			this.aspectRatio = projector.aspectRatio;
			this.orthographic = projector.orthographic;
			this.orthographicSize = projector.orthographicSize;
			this.ignoreLayers = projector.ignoreLayers;
			this.material = projector.material.GetMappedInstanceID();
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00037CFF File Offset: 0x000360FF
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.material, dependencies, objects, allowNulls);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00037D1C File Offset: 0x0003611C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Projector projector = (Projector)obj;
			base.AddDependency(projector.material, dependencies);
		}

		// Token: 0x040009A0 RID: 2464
		public float nearClipPlane;

		// Token: 0x040009A1 RID: 2465
		public float farClipPlane;

		// Token: 0x040009A2 RID: 2466
		public float fieldOfView;

		// Token: 0x040009A3 RID: 2467
		public float aspectRatio;

		// Token: 0x040009A4 RID: 2468
		public bool orthographic;

		// Token: 0x040009A5 RID: 2469
		public float orthographicSize;

		// Token: 0x040009A6 RID: 2470
		public int ignoreLayers;

		// Token: 0x040009A7 RID: 2471
		public long material;
	}
}
