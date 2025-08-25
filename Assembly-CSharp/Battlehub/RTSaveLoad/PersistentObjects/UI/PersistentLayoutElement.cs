using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200017D RID: 381
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentLayoutElement : PersistentUIBehaviour
	{
		// Token: 0x0600084F RID: 2127 RVA: 0x00036005 File Offset: 0x00034405
		public PersistentLayoutElement()
		{
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00036010 File Offset: 0x00034410
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			LayoutElement layoutElement = (LayoutElement)obj;
			layoutElement.ignoreLayout = this.ignoreLayout;
			layoutElement.minWidth = this.minWidth;
			layoutElement.minHeight = this.minHeight;
			layoutElement.preferredWidth = this.preferredWidth;
			layoutElement.preferredHeight = this.preferredHeight;
			layoutElement.flexibleWidth = this.flexibleWidth;
			layoutElement.flexibleHeight = this.flexibleHeight;
			return layoutElement;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0003608C File Offset: 0x0003448C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			LayoutElement layoutElement = (LayoutElement)obj;
			this.ignoreLayout = layoutElement.ignoreLayout;
			this.minWidth = layoutElement.minWidth;
			this.minHeight = layoutElement.minHeight;
			this.preferredWidth = layoutElement.preferredWidth;
			this.preferredHeight = layoutElement.preferredHeight;
			this.flexibleWidth = layoutElement.flexibleWidth;
			this.flexibleHeight = layoutElement.flexibleHeight;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00036102 File Offset: 0x00034502
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000904 RID: 2308
		public bool ignoreLayout;

		// Token: 0x04000905 RID: 2309
		public float minWidth;

		// Token: 0x04000906 RID: 2310
		public float minHeight;

		// Token: 0x04000907 RID: 2311
		public float preferredWidth;

		// Token: 0x04000908 RID: 2312
		public float preferredHeight;

		// Token: 0x04000909 RID: 2313
		public float flexibleWidth;

		// Token: 0x0400090A RID: 2314
		public float flexibleHeight;
	}
}
