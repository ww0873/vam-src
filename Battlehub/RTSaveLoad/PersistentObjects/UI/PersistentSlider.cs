using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000224 RID: 548
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentSlider : PersistentSelectable
	{
		// Token: 0x06000AFD RID: 2813 RVA: 0x00043F82 File Offset: 0x00042382
		public PersistentSlider()
		{
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00043F8C File Offset: 0x0004238C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Slider slider = (Slider)obj;
			slider.fillRect = (RectTransform)objects.Get(this.fillRect);
			slider.handleRect = (RectTransform)objects.Get(this.handleRect);
			slider.direction = this.direction;
			slider.minValue = this.minValue;
			slider.maxValue = this.maxValue;
			slider.wholeNumbers = this.wholeNumbers;
			slider.value = this.value;
			slider.normalizedValue = this.normalizedValue;
			base.Write<Slider.SliderEvent>(slider.onValueChanged, this.onValueChanged, objects);
			return slider;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00044040 File Offset: 0x00042440
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Slider slider = (Slider)obj;
			this.fillRect = slider.fillRect.GetMappedInstanceID();
			this.handleRect = slider.handleRect.GetMappedInstanceID();
			this.direction = slider.direction;
			this.minValue = slider.minValue;
			this.maxValue = slider.maxValue;
			this.wholeNumbers = slider.wholeNumbers;
			this.value = slider.value;
			this.normalizedValue = slider.normalizedValue;
			base.Read(this.onValueChanged, slider.onValueChanged);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000440E0 File Offset: 0x000424E0
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.fillRect, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.handleRect, dependencies, objects, allowNulls);
			if (this.onValueChanged != null)
			{
				this.onValueChanged.FindDependencies<T>(dependencies, objects, allowNulls);
			}
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00044130 File Offset: 0x00042530
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Slider slider = (Slider)obj;
			base.AddDependency(slider.fillRect, dependencies);
			base.AddDependency(slider.handleRect, dependencies);
			PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
			persistentUnityEventBase.GetDependencies(slider.onValueChanged, dependencies);
		}

		// Token: 0x04000C53 RID: 3155
		public long fillRect;

		// Token: 0x04000C54 RID: 3156
		public long handleRect;

		// Token: 0x04000C55 RID: 3157
		public Slider.Direction direction;

		// Token: 0x04000C56 RID: 3158
		public float minValue;

		// Token: 0x04000C57 RID: 3159
		public float maxValue;

		// Token: 0x04000C58 RID: 3160
		public bool wholeNumbers;

		// Token: 0x04000C59 RID: 3161
		public float value;

		// Token: 0x04000C5A RID: 3162
		public float normalizedValue;

		// Token: 0x04000C5B RID: 3163
		public PersistentUnityEventBase onValueChanged;
	}
}
