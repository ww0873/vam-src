using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x020001B5 RID: 437
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1136, typeof(PersistentOutline))]
	[Serializable]
	public class PersistentShadow : PersistentBaseMeshEffect
	{
		// Token: 0x06000907 RID: 2311 RVA: 0x00037439 File Offset: 0x00035839
		public PersistentShadow()
		{
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00037444 File Offset: 0x00035844
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Shadow shadow = (Shadow)obj;
			shadow.effectColor = this.effectColor;
			shadow.effectDistance = this.effectDistance;
			shadow.useGraphicAlpha = this.useGraphicAlpha;
			return shadow;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00037490 File Offset: 0x00035890
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Shadow shadow = (Shadow)obj;
			this.effectColor = shadow.effectColor;
			this.effectDistance = shadow.effectDistance;
			this.useGraphicAlpha = shadow.useGraphicAlpha;
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x000374D6 File Offset: 0x000358D6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A0E RID: 2574
		public Color effectColor;

		// Token: 0x04000A0F RID: 2575
		public Vector2 effectDistance;

		// Token: 0x04000A10 RID: 2576
		public bool useGraphicAlpha;
	}
}
