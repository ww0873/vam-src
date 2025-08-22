using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Rendering;

namespace Battlehub.RTSaveLoad.PersistentObjects.Rendering
{
	// Token: 0x020001B9 RID: 441
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSortingGroup : PersistentBehaviour
	{
		// Token: 0x06000919 RID: 2329 RVA: 0x00038D95 File Offset: 0x00037195
		public PersistentSortingGroup()
		{
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00038DA0 File Offset: 0x000371A0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			SortingGroup sortingGroup = (SortingGroup)obj;
			sortingGroup.sortingLayerName = this.sortingLayerName;
			sortingGroup.sortingLayerID = this.sortingLayerID;
			sortingGroup.sortingOrder = this.sortingOrder;
			return sortingGroup;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00038DEC File Offset: 0x000371EC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			SortingGroup sortingGroup = (SortingGroup)obj;
			this.sortingLayerName = sortingGroup.sortingLayerName;
			this.sortingLayerID = sortingGroup.sortingLayerID;
			this.sortingOrder = sortingGroup.sortingOrder;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00038E32 File Offset: 0x00037232
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A1F RID: 2591
		public string sortingLayerName;

		// Token: 0x04000A20 RID: 2592
		public int sortingLayerID;

		// Token: 0x04000A21 RID: 2593
		public int sortingOrder;
	}
}
