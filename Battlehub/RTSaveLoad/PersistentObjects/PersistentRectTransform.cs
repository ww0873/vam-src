using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001A8 RID: 424
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRectTransform : PersistentTransform
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x00037EF0 File Offset: 0x000362F0
		public PersistentRectTransform()
		{
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00037EF8 File Offset: 0x000362F8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			RectTransform rectTransform = (RectTransform)obj;
			rectTransform.anchorMin = this.anchorMin;
			rectTransform.anchorMax = this.anchorMax;
			rectTransform.anchoredPosition3D = this.anchoredPosition3D;
			rectTransform.anchoredPosition = this.anchoredPosition;
			rectTransform.sizeDelta = this.sizeDelta;
			rectTransform.pivot = this.pivot;
			rectTransform.offsetMin = this.offsetMin;
			rectTransform.offsetMax = this.offsetMax;
			return rectTransform;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00037F80 File Offset: 0x00036380
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			RectTransform rectTransform = (RectTransform)obj;
			this.anchorMin = rectTransform.anchorMin;
			this.anchorMax = rectTransform.anchorMax;
			this.anchoredPosition3D = rectTransform.anchoredPosition3D;
			this.anchoredPosition = rectTransform.anchoredPosition;
			this.sizeDelta = rectTransform.sizeDelta;
			this.pivot = rectTransform.pivot;
			this.offsetMin = rectTransform.offsetMin;
			this.offsetMax = rectTransform.offsetMax;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00038002 File Offset: 0x00036402
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040009AA RID: 2474
		public Vector2 anchorMin;

		// Token: 0x040009AB RID: 2475
		public Vector2 anchorMax;

		// Token: 0x040009AC RID: 2476
		public Vector3 anchoredPosition3D;

		// Token: 0x040009AD RID: 2477
		public Vector2 anchoredPosition;

		// Token: 0x040009AE RID: 2478
		public Vector2 sizeDelta;

		// Token: 0x040009AF RID: 2479
		public Vector2 pivot;

		// Token: 0x040009B0 RID: 2480
		public Vector2 offsetMin;

		// Token: 0x040009B1 RID: 2481
		public Vector2 offsetMax;
	}
}
