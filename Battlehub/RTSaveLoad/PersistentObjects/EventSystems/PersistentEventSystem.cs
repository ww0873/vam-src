using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.RTSaveLoad.PersistentObjects.EventSystems
{
	// Token: 0x02000164 RID: 356
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentEventSystem : PersistentUIBehaviour
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x00034609 File Offset: 0x00032A09
		public PersistentEventSystem()
		{
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00034614 File Offset: 0x00032A14
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			EventSystem eventSystem = (EventSystem)obj;
			eventSystem.sendNavigationEvents = this.sendNavigationEvents;
			eventSystem.pixelDragThreshold = this.pixelDragThreshold;
			eventSystem.firstSelectedGameObject = (GameObject)objects.Get(this.firstSelectedGameObject);
			return eventSystem;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0003466C File Offset: 0x00032A6C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			EventSystem eventSystem = (EventSystem)obj;
			this.sendNavigationEvents = eventSystem.sendNavigationEvents;
			this.pixelDragThreshold = eventSystem.pixelDragThreshold;
			this.firstSelectedGameObject = eventSystem.firstSelectedGameObject.GetMappedInstanceID();
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x000346B7 File Offset: 0x00032AB7
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.firstSelectedGameObject, dependencies, objects, allowNulls);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x000346D4 File Offset: 0x00032AD4
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			EventSystem eventSystem = (EventSystem)obj;
			base.AddDependency(eventSystem.firstSelectedGameObject, dependencies);
		}

		// Token: 0x04000893 RID: 2195
		public bool sendNavigationEvents;

		// Token: 0x04000894 RID: 2196
		public int pixelDragThreshold;

		// Token: 0x04000895 RID: 2197
		public long firstSelectedGameObject;
	}
}
