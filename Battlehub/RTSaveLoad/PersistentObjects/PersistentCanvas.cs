using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200014B RID: 331
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCanvas : PersistentBehaviour
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x0003314C File Offset: 0x0003154C
		public PersistentCanvas()
		{
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00033154 File Offset: 0x00031554
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Canvas canvas = (Canvas)obj;
			canvas.renderMode = (RenderMode)this.renderMode;
			canvas.worldCamera = (Camera)objects.Get(this.worldCamera);
			canvas.scaleFactor = this.scaleFactor;
			canvas.referencePixelsPerUnit = this.referencePixelsPerUnit;
			canvas.overridePixelPerfect = this.overridePixelPerfect;
			canvas.pixelPerfect = this.pixelPerfect;
			canvas.planeDistance = this.planeDistance;
			canvas.overrideSorting = this.overrideSorting;
			canvas.sortingOrder = this.sortingOrder;
			canvas.targetDisplay = this.targetDisplay;
			canvas.normalizedSortingGridSize = this.normalizedSortingGridSize;
			canvas.sortingLayerID = this.sortingLayerID;
			canvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)this.additionalShaderChannels;
			canvas.sortingLayerName = this.sortingLayerName;
			return canvas;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00033230 File Offset: 0x00031630
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Canvas canvas = (Canvas)obj;
			this.renderMode = (uint)canvas.renderMode;
			this.worldCamera = canvas.worldCamera.GetMappedInstanceID();
			this.scaleFactor = canvas.scaleFactor;
			this.referencePixelsPerUnit = canvas.referencePixelsPerUnit;
			this.overridePixelPerfect = canvas.overridePixelPerfect;
			this.pixelPerfect = canvas.pixelPerfect;
			this.planeDistance = canvas.planeDistance;
			this.overrideSorting = canvas.overrideSorting;
			this.sortingOrder = canvas.sortingOrder;
			this.targetDisplay = canvas.targetDisplay;
			this.normalizedSortingGridSize = canvas.normalizedSortingGridSize;
			this.sortingLayerID = canvas.sortingLayerID;
			this.additionalShaderChannels = (uint)canvas.additionalShaderChannels;
			this.sortingLayerName = canvas.sortingLayerName;
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000332FF File Offset: 0x000316FF
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.worldCamera, dependencies, objects, allowNulls);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0003331C File Offset: 0x0003171C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Canvas canvas = (Canvas)obj;
			base.AddDependency(canvas.worldCamera, dependencies);
		}

		// Token: 0x0400080A RID: 2058
		public uint renderMode;

		// Token: 0x0400080B RID: 2059
		public long worldCamera;

		// Token: 0x0400080C RID: 2060
		public float scaleFactor;

		// Token: 0x0400080D RID: 2061
		public float referencePixelsPerUnit;

		// Token: 0x0400080E RID: 2062
		public bool overridePixelPerfect;

		// Token: 0x0400080F RID: 2063
		public bool pixelPerfect;

		// Token: 0x04000810 RID: 2064
		public float planeDistance;

		// Token: 0x04000811 RID: 2065
		public bool overrideSorting;

		// Token: 0x04000812 RID: 2066
		public int sortingOrder;

		// Token: 0x04000813 RID: 2067
		public int targetDisplay;

		// Token: 0x04000814 RID: 2068
		public float normalizedSortingGridSize;

		// Token: 0x04000815 RID: 2069
		public int sortingLayerID;

		// Token: 0x04000816 RID: 2070
		public uint additionalShaderChannels;

		// Token: 0x04000817 RID: 2071
		public string sortingLayerName;
	}
}
