using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000187 RID: 391
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentMask : PersistentUIBehaviour
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x000369B5 File Offset: 0x00034DB5
		public PersistentMask()
		{
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000369C0 File Offset: 0x00034DC0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Mask mask = (Mask)obj;
			mask.showMaskGraphic = this.showMaskGraphic;
			return mask;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000369F4 File Offset: 0x00034DF4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Mask mask = (Mask)obj;
			this.showMaskGraphic = mask.showMaskGraphic;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00036A22 File Offset: 0x00034E22
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000943 RID: 2371
		public bool showMaskGraphic;
	}
}
