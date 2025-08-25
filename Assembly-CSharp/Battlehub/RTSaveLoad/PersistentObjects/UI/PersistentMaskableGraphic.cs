using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200021E RID: 542
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1131, typeof(PersistentImage))]
	[ProtoInclude(1132, typeof(PersistentRawImage))]
	[ProtoInclude(1133, typeof(PersistentText))]
	[Serializable]
	public class PersistentMaskableGraphic : PersistentGraphic
	{
		// Token: 0x06000AE2 RID: 2786 RVA: 0x00035D45 File Offset: 0x00034145
		public PersistentMaskableGraphic()
		{
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00035D50 File Offset: 0x00034150
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			MaskableGraphic maskableGraphic = (MaskableGraphic)obj;
			this.onCullStateChanged.WriteTo(maskableGraphic.onCullStateChanged, objects);
			maskableGraphic.maskable = this.maskable;
			return maskableGraphic;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00035D98 File Offset: 0x00034198
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			MaskableGraphic maskableGraphic = (MaskableGraphic)obj;
			this.onCullStateChanged = new PersistentUnityEventBase();
			this.onCullStateChanged.ReadFrom(maskableGraphic.onCullStateChanged);
			this.maskable = maskableGraphic.maskable;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00035DE2 File Offset: 0x000341E2
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			this.onCullStateChanged.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00035DFC File Offset: 0x000341FC
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			MaskableGraphic maskableGraphic = (MaskableGraphic)obj;
			if (maskableGraphic == null)
			{
				return;
			}
			PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
			persistentUnityEventBase.GetDependencies(maskableGraphic.onCullStateChanged, dependencies);
		}

		// Token: 0x04000C28 RID: 3112
		public PersistentUnityEventBase onCullStateChanged;

		// Token: 0x04000C29 RID: 3113
		public bool maskable;
	}
}
