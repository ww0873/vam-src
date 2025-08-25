using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200016D RID: 365
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1129, typeof(PersistentMaskableGraphic))]
	[Serializable]
	public class PersistentGraphic : PersistentUIBehaviour
	{
		// Token: 0x06000814 RID: 2068 RVA: 0x00034A09 File Offset: 0x00032E09
		public PersistentGraphic()
		{
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00034A14 File Offset: 0x00032E14
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Graphic graphic = (Graphic)obj;
			graphic.color = this.color;
			graphic.raycastTarget = this.raycastTarget;
			graphic.material = (Material)objects.Get(this.material);
			return graphic;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00034A6C File Offset: 0x00032E6C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Graphic graphic = (Graphic)obj;
			this.color = graphic.color;
			this.raycastTarget = graphic.raycastTarget;
			this.material = graphic.material.GetMappedInstanceID();
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00034AB7 File Offset: 0x00032EB7
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.material, dependencies, objects, allowNulls);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00034AD4 File Offset: 0x00032ED4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Graphic graphic = (Graphic)obj;
			base.AddDependency(graphic.material, dependencies);
		}

		// Token: 0x040008A0 RID: 2208
		public Color color;

		// Token: 0x040008A1 RID: 2209
		public bool raycastTarget;

		// Token: 0x040008A2 RID: 2210
		public long material;
	}
}
