using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200014C RID: 332
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCanvasGroup : PersistentComponent
	{
		// Token: 0x06000784 RID: 1924 RVA: 0x0003334C File Offset: 0x0003174C
		public PersistentCanvasGroup()
		{
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00033354 File Offset: 0x00031754
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CanvasGroup canvasGroup = (CanvasGroup)obj;
			canvasGroup.alpha = this.alpha;
			canvasGroup.interactable = this.interactable;
			canvasGroup.blocksRaycasts = this.blocksRaycasts;
			canvasGroup.ignoreParentGroups = this.ignoreParentGroups;
			return canvasGroup;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000333AC File Offset: 0x000317AC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CanvasGroup canvasGroup = (CanvasGroup)obj;
			this.alpha = canvasGroup.alpha;
			this.interactable = canvasGroup.interactable;
			this.blocksRaycasts = canvasGroup.blocksRaycasts;
			this.ignoreParentGroups = canvasGroup.ignoreParentGroups;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x000333FE File Offset: 0x000317FE
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000818 RID: 2072
		public float alpha;

		// Token: 0x04000819 RID: 2073
		public bool interactable;

		// Token: 0x0400081A RID: 2074
		public bool blocksRaycasts;

		// Token: 0x0400081B RID: 2075
		public bool ignoreParentGroups;
	}
}
