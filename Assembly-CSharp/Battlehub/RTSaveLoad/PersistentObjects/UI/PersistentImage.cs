using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200017A RID: 378
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentImage : PersistentMaskableGraphic
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x00035E38 File Offset: 0x00034238
		public PersistentImage()
		{
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00035E40 File Offset: 0x00034240
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Image image = (Image)obj;
			image.sprite = (Sprite)objects.Get(this.sprite);
			image.overrideSprite = (Sprite)objects.Get(this.overrideSprite);
			image.type = (Image.Type)this.type;
			image.preserveAspect = this.preserveAspect;
			image.fillCenter = this.fillCenter;
			image.fillMethod = (Image.FillMethod)this.fillMethod;
			image.fillAmount = this.fillAmount;
			image.fillClockwise = this.fillClockwise;
			image.fillOrigin = this.fillOrigin;
			image.alphaHitTestMinimumThreshold = this.alphaHitTestMinimumThreshold;
			return image;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00035EF8 File Offset: 0x000342F8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Image image = (Image)obj;
			this.sprite = image.sprite.GetMappedInstanceID();
			this.overrideSprite = image.overrideSprite.GetMappedInstanceID();
			this.type = (uint)image.type;
			this.preserveAspect = image.preserveAspect;
			this.fillCenter = image.fillCenter;
			this.fillMethod = (uint)image.fillMethod;
			this.fillAmount = image.fillAmount;
			this.fillClockwise = image.fillClockwise;
			this.fillOrigin = image.fillOrigin;
			this.alphaHitTestMinimumThreshold = image.alphaHitTestMinimumThreshold;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00035F9C File Offset: 0x0003439C
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sprite, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.overrideSprite, dependencies, objects, allowNulls);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00035FC8 File Offset: 0x000343C8
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Image image = (Image)obj;
			base.AddDependency(image.sprite, dependencies);
			base.AddDependency(image.overrideSprite, dependencies);
		}

		// Token: 0x040008ED RID: 2285
		public long sprite;

		// Token: 0x040008EE RID: 2286
		public long overrideSprite;

		// Token: 0x040008EF RID: 2287
		public uint type;

		// Token: 0x040008F0 RID: 2288
		public bool preserveAspect;

		// Token: 0x040008F1 RID: 2289
		public bool fillCenter;

		// Token: 0x040008F2 RID: 2290
		public uint fillMethod;

		// Token: 0x040008F3 RID: 2291
		public float fillAmount;

		// Token: 0x040008F4 RID: 2292
		public bool fillClockwise;

		// Token: 0x040008F5 RID: 2293
		public int fillOrigin;

		// Token: 0x040008F6 RID: 2294
		public float alphaHitTestMinimumThreshold;
	}
}
