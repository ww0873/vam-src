using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200012D RID: 301
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAspectRatioFitter : PersistentUIBehaviour
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x00031669 File Offset: 0x0002FA69
		public PersistentAspectRatioFitter()
		{
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00031674 File Offset: 0x0002FA74
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AspectRatioFitter aspectRatioFitter = (AspectRatioFitter)obj;
			aspectRatioFitter.aspectMode = (AspectRatioFitter.AspectMode)this.aspectMode;
			aspectRatioFitter.aspectRatio = this.aspectRatio;
			return aspectRatioFitter;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000316B4 File Offset: 0x0002FAB4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AspectRatioFitter aspectRatioFitter = (AspectRatioFitter)obj;
			this.aspectMode = (uint)aspectRatioFitter.aspectMode;
			this.aspectRatio = aspectRatioFitter.aspectRatio;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000316EE File Offset: 0x0002FAEE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000786 RID: 1926
		public uint aspectMode;

		// Token: 0x04000787 RID: 1927
		public float aspectRatio;
	}
}
