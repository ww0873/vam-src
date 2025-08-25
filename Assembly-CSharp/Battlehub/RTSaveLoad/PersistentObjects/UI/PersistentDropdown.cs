using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200021C RID: 540
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentDropdown : PersistentSelectable
	{
		// Token: 0x06000AD9 RID: 2777 RVA: 0x00043284 File Offset: 0x00041684
		public PersistentDropdown()
		{
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0004328C File Offset: 0x0004168C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Dropdown dropdown = (Dropdown)obj;
			dropdown.template = (RectTransform)objects.Get(this.template);
			dropdown.captionText = (Text)objects.Get(this.captionText);
			dropdown.captionImage = (Image)objects.Get(this.captionImage);
			dropdown.itemText = (Text)objects.Get(this.itemText);
			dropdown.itemImage = (Image)objects.Get(this.itemImage);
			dropdown.onValueChanged = base.Write<Dropdown.DropdownEvent>(dropdown.onValueChanged, this.onValueChanged, objects);
			dropdown.value = this.value;
			if (this.options != null)
			{
				List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
				for (int i = 0; i < this.options.Length; i++)
				{
					PersistentOptionData persistentOptionData = this.options[i];
					Dropdown.OptionData optionData = new Dropdown.OptionData();
					persistentOptionData.WriteTo(optionData, objects);
					list.Add(optionData);
				}
				dropdown.options = list;
			}
			else
			{
				dropdown.options = null;
			}
			return dropdown;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x000433AC File Offset: 0x000417AC
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.template, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.captionText, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.captionImage, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.itemText, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.itemImage, dependencies, objects, allowNulls);
			if (this.onValueChanged != null)
			{
				this.onValueChanged.FindDependencies<T>(dependencies, objects, allowNulls);
			}
			if (this.options != null)
			{
				for (int i = 0; i < this.options.Length; i++)
				{
					PersistentOptionData persistentOptionData = this.options[i];
					persistentOptionData.FindDependencies<T>(dependencies, objects, allowNulls);
				}
			}
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0004345C File Offset: 0x0004185C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Dropdown dropdown = (Dropdown)obj;
			this.template = dropdown.template.GetMappedInstanceID();
			this.captionText = dropdown.captionText.GetMappedInstanceID();
			this.captionImage = dropdown.captionImage.GetMappedInstanceID();
			this.itemText = dropdown.itemText.GetMappedInstanceID();
			this.itemImage = dropdown.itemImage.GetMappedInstanceID();
			this.onValueChanged = base.Read(this.onValueChanged, dropdown.onValueChanged);
			if (dropdown.options != null)
			{
				this.options = new PersistentOptionData[dropdown.options.Count];
				for (int i = 0; i < dropdown.options.Count; i++)
				{
					PersistentOptionData persistentOptionData = new PersistentOptionData();
					persistentOptionData.ReadFrom(dropdown.options[i]);
					this.options[i] = persistentOptionData;
				}
			}
			else
			{
				this.options = null;
			}
			this.value = dropdown.value;
		}

		// Token: 0x04000C09 RID: 3081
		public long template;

		// Token: 0x04000C0A RID: 3082
		public long captionText;

		// Token: 0x04000C0B RID: 3083
		public long captionImage;

		// Token: 0x04000C0C RID: 3084
		public long itemText;

		// Token: 0x04000C0D RID: 3085
		public long itemImage;

		// Token: 0x04000C0E RID: 3086
		public PersistentUnityEventBase onValueChanged;

		// Token: 0x04000C0F RID: 3087
		public PersistentOptionData[] options;

		// Token: 0x04000C10 RID: 3088
		public int value;
	}
}
