using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200016E RID: 366
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentGraphicRaycaster : PersistentBaseRaycaster
	{
		// Token: 0x06000819 RID: 2073 RVA: 0x00034B04 File Offset: 0x00032F04
		public PersistentGraphicRaycaster()
		{
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00034B0C File Offset: 0x00032F0C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			GraphicRaycaster graphicRaycaster = (GraphicRaycaster)obj;
			graphicRaycaster.ignoreReversedGraphics = this.ignoreReversedGraphics;
			graphicRaycaster.blockingObjects = (GraphicRaycaster.BlockingObjects)this.blockingObjects;
			return graphicRaycaster;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00034B4C File Offset: 0x00032F4C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			GraphicRaycaster graphicRaycaster = (GraphicRaycaster)obj;
			this.ignoreReversedGraphics = graphicRaycaster.ignoreReversedGraphics;
			this.blockingObjects = (uint)graphicRaycaster.blockingObjects;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00034B86 File Offset: 0x00032F86
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040008A3 RID: 2211
		public bool ignoreReversedGraphics;

		// Token: 0x040008A4 RID: 2212
		public uint blockingObjects;
	}
}
