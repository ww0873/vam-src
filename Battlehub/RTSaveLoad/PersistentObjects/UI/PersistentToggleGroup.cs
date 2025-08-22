using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x020001CE RID: 462
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentToggleGroup : PersistentUIBehaviour
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x0003A3E8 File Offset: 0x000387E8
		public PersistentToggleGroup()
		{
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0003A3F0 File Offset: 0x000387F0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ToggleGroup toggleGroup = (ToggleGroup)obj;
			toggleGroup.allowSwitchOff = this.allowSwitchOff;
			return toggleGroup;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0003A424 File Offset: 0x00038824
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ToggleGroup toggleGroup = (ToggleGroup)obj;
			this.allowSwitchOff = toggleGroup.allowSwitchOff;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0003A452 File Offset: 0x00038852
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x04000A91 RID: 2705
		public bool allowSwitchOff;
	}
}
