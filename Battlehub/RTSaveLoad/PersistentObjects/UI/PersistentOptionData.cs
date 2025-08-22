using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000220 RID: 544
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentOptionData : PersistentData
	{
		// Token: 0x06000AEA RID: 2794 RVA: 0x00043A03 File Offset: 0x00041E03
		public PersistentOptionData()
		{
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00043A0C File Offset: 0x00041E0C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Dropdown.OptionData optionData = (Dropdown.OptionData)obj;
			optionData.text = this.text;
			optionData.image = (Sprite)objects.Get(this.image);
			return optionData;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00043A56 File Offset: 0x00041E56
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.image, dependencies, objects, allowNulls);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00043A70 File Offset: 0x00041E70
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Dropdown.OptionData optionData = (Dropdown.OptionData)obj;
			this.text = optionData.text;
			this.image = optionData.image.GetMappedInstanceID();
		}

		// Token: 0x04000C2F RID: 3119
		public string text;

		// Token: 0x04000C30 RID: 3120
		public long image;
	}
}
