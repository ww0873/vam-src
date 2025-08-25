using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200014E RID: 334
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCanvasScaler : PersistentUIBehaviour
	{
		// Token: 0x0600078C RID: 1932 RVA: 0x000334C9 File Offset: 0x000318C9
		public PersistentCanvasScaler()
		{
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000334D4 File Offset: 0x000318D4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			CanvasScaler canvasScaler = (CanvasScaler)obj;
			canvasScaler.uiScaleMode = (CanvasScaler.ScaleMode)this.uiScaleMode;
			canvasScaler.referencePixelsPerUnit = this.referencePixelsPerUnit;
			canvasScaler.scaleFactor = this.scaleFactor;
			canvasScaler.referenceResolution = this.referenceResolution;
			canvasScaler.screenMatchMode = (CanvasScaler.ScreenMatchMode)this.screenMatchMode;
			canvasScaler.matchWidthOrHeight = this.matchWidthOrHeight;
			canvasScaler.physicalUnit = (CanvasScaler.Unit)this.physicalUnit;
			canvasScaler.fallbackScreenDPI = this.fallbackScreenDPI;
			canvasScaler.defaultSpriteDPI = this.defaultSpriteDPI;
			canvasScaler.dynamicPixelsPerUnit = this.dynamicPixelsPerUnit;
			return canvasScaler;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00033574 File Offset: 0x00031974
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			CanvasScaler canvasScaler = (CanvasScaler)obj;
			this.uiScaleMode = (uint)canvasScaler.uiScaleMode;
			this.referencePixelsPerUnit = canvasScaler.referencePixelsPerUnit;
			this.scaleFactor = canvasScaler.scaleFactor;
			this.referenceResolution = canvasScaler.referenceResolution;
			this.screenMatchMode = (uint)canvasScaler.screenMatchMode;
			this.matchWidthOrHeight = canvasScaler.matchWidthOrHeight;
			this.physicalUnit = (uint)canvasScaler.physicalUnit;
			this.fallbackScreenDPI = canvasScaler.fallbackScreenDPI;
			this.defaultSpriteDPI = canvasScaler.defaultSpriteDPI;
			this.dynamicPixelsPerUnit = canvasScaler.dynamicPixelsPerUnit;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0003360E File Offset: 0x00031A0E
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000820 RID: 2080
		public uint uiScaleMode;

		// Token: 0x04000821 RID: 2081
		public float referencePixelsPerUnit;

		// Token: 0x04000822 RID: 2082
		public float scaleFactor;

		// Token: 0x04000823 RID: 2083
		public Vector2 referenceResolution;

		// Token: 0x04000824 RID: 2084
		public uint screenMatchMode;

		// Token: 0x04000825 RID: 2085
		public float matchWidthOrHeight;

		// Token: 0x04000826 RID: 2086
		public uint physicalUnit;

		// Token: 0x04000827 RID: 2087
		public float fallbackScreenDPI;

		// Token: 0x04000828 RID: 2088
		public float defaultSpriteDPI;

		// Token: 0x04000829 RID: 2089
		public float dynamicPixelsPerUnit;
	}
}
