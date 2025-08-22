using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200015C RID: 348
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentContentSizeFitter : PersistentUIBehaviour
	{
		// Token: 0x060007C3 RID: 1987 RVA: 0x00034171 File Offset: 0x00032571
		public PersistentContentSizeFitter()
		{
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0003417C File Offset: 0x0003257C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ContentSizeFitter contentSizeFitter = (ContentSizeFitter)obj;
			contentSizeFitter.horizontalFit = (ContentSizeFitter.FitMode)this.horizontalFit;
			contentSizeFitter.verticalFit = (ContentSizeFitter.FitMode)this.verticalFit;
			return contentSizeFitter;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000341BC File Offset: 0x000325BC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ContentSizeFitter contentSizeFitter = (ContentSizeFitter)obj;
			this.horizontalFit = (uint)contentSizeFitter.horizontalFit;
			this.verticalFit = (uint)contentSizeFitter.verticalFit;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000341F6 File Offset: 0x000325F6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000879 RID: 2169
		public uint horizontalFit;

		// Token: 0x0400087A RID: 2170
		public uint verticalFit;
	}
}
