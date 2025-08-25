using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x020001CD RID: 461
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentToggle : PersistentSelectable
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x0003A305 File Offset: 0x00038705
		public PersistentToggle()
		{
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0003A310 File Offset: 0x00038710
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Toggle toggle = (Toggle)obj;
			toggle.group = (ToggleGroup)objects.Get(this.group);
			toggle.isOn = this.isOn;
			return toggle;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0003A35C File Offset: 0x0003875C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Toggle toggle = (Toggle)obj;
			this.group = toggle.group.GetMappedInstanceID();
			this.isOn = toggle.isOn;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0003A39B File Offset: 0x0003879B
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.group, dependencies, objects, allowNulls);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0003A3B8 File Offset: 0x000387B8
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Toggle toggle = (Toggle)obj;
			base.AddDependency(toggle.group, dependencies);
		}

		// Token: 0x04000A8F RID: 2703
		public long group;

		// Token: 0x04000A90 RID: 2704
		public bool isOn;
	}
}
