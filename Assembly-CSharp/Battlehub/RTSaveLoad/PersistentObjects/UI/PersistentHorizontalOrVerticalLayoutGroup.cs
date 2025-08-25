using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000179 RID: 377
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1130, typeof(PersistentHorizontalLayoutGroup))]
	[ProtoInclude(1131, typeof(PersistentVerticalLayoutGroup))]
	[Serializable]
	public class PersistentHorizontalOrVerticalLayoutGroup : PersistentLayoutGroup
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x00035C65 File Offset: 0x00034065
		public PersistentHorizontalOrVerticalLayoutGroup()
		{
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00035C70 File Offset: 0x00034070
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)obj;
			horizontalOrVerticalLayoutGroup.spacing = this.spacing;
			horizontalOrVerticalLayoutGroup.childForceExpandWidth = this.childForceExpandWidth;
			horizontalOrVerticalLayoutGroup.childForceExpandHeight = this.childForceExpandHeight;
			horizontalOrVerticalLayoutGroup.childControlWidth = this.childControlWidth;
			horizontalOrVerticalLayoutGroup.childControlHeight = this.childControlHeight;
			return horizontalOrVerticalLayoutGroup;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00035CD4 File Offset: 0x000340D4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup = (HorizontalOrVerticalLayoutGroup)obj;
			this.spacing = horizontalOrVerticalLayoutGroup.spacing;
			this.childForceExpandWidth = horizontalOrVerticalLayoutGroup.childForceExpandWidth;
			this.childForceExpandHeight = horizontalOrVerticalLayoutGroup.childForceExpandHeight;
			this.childControlWidth = horizontalOrVerticalLayoutGroup.childControlWidth;
			this.childControlHeight = horizontalOrVerticalLayoutGroup.childControlHeight;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00035D32 File Offset: 0x00034132
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040008E8 RID: 2280
		public float spacing;

		// Token: 0x040008E9 RID: 2281
		public bool childForceExpandWidth;

		// Token: 0x040008EA RID: 2282
		public bool childForceExpandHeight;

		// Token: 0x040008EB RID: 2283
		public bool childControlWidth;

		// Token: 0x040008EC RID: 2284
		public bool childControlHeight;
	}
}
