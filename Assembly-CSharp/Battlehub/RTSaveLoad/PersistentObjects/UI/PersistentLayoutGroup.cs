using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200017E RID: 382
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1132, typeof(PersistentGridLayoutGroup))]
	[ProtoInclude(1133, typeof(PersistentHorizontalOrVerticalLayoutGroup))]
	[Serializable]
	public class PersistentLayoutGroup : PersistentUIBehaviour
	{
		// Token: 0x06000853 RID: 2131 RVA: 0x00034B99 File Offset: 0x00032F99
		public PersistentLayoutGroup()
		{
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00034BA4 File Offset: 0x00032FA4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			LayoutGroup layoutGroup = (LayoutGroup)obj;
			layoutGroup.padding = this.padding;
			layoutGroup.childAlignment = (TextAnchor)this.childAlignment;
			return layoutGroup;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00034BE4 File Offset: 0x00032FE4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			LayoutGroup layoutGroup = (LayoutGroup)obj;
			this.padding = layoutGroup.padding;
			this.childAlignment = (uint)layoutGroup.childAlignment;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00034C1E File Offset: 0x0003301E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x0400090B RID: 2315
		public RectOffset padding;

		// Token: 0x0400090C RID: 2316
		public uint childAlignment;
	}
}
