using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x020001A6 RID: 422
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRawImage : PersistentMaskableGraphic
	{
		// Token: 0x060008D9 RID: 2265 RVA: 0x00037D54 File Offset: 0x00036154
		public PersistentRawImage()
		{
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00037D5C File Offset: 0x0003615C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			RawImage rawImage = (RawImage)obj;
			rawImage.texture = (Texture)objects.Get(this.texture);
			rawImage.uvRect = this.uvRect;
			return rawImage;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00037DA8 File Offset: 0x000361A8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			RawImage rawImage = (RawImage)obj;
			this.texture = rawImage.texture.GetMappedInstanceID();
			this.uvRect = rawImage.uvRect;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00037DE7 File Offset: 0x000361E7
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.texture, dependencies, objects, allowNulls);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00037E04 File Offset: 0x00036204
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			RawImage rawImage = (RawImage)obj;
			base.AddDependency(rawImage.texture, dependencies);
		}

		// Token: 0x040009A8 RID: 2472
		public long texture;

		// Token: 0x040009A9 RID: 2473
		public Rect uvRect;
	}
}
