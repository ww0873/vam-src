using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200021F RID: 543
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentNavigation : PersistentData
	{
		// Token: 0x06000AE7 RID: 2791 RVA: 0x000438E5 File Offset: 0x00041CE5
		public PersistentNavigation()
		{
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x000438F0 File Offset: 0x00041CF0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Navigation navigation = (Navigation)obj;
			navigation.mode = this.mode;
			navigation.selectOnUp = (Selectable)objects.Get(this.selectOnUp);
			navigation.selectOnDown = (Selectable)objects.Get(this.selectOnDown);
			navigation.selectOnLeft = (Selectable)objects.Get(this.selectOnLeft);
			navigation.selectOnRight = (Selectable)objects.Get(this.selectOnRight);
			return navigation;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0004398C File Offset: 0x00041D8C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Navigation navigation = (Navigation)obj;
			this.mode = navigation.mode;
			this.selectOnUp = navigation.selectOnUp.GetMappedInstanceID();
			this.selectOnDown = navigation.selectOnDown.GetMappedInstanceID();
			this.selectOnLeft = navigation.selectOnLeft.GetMappedInstanceID();
			this.selectOnRight = navigation.selectOnRight.GetMappedInstanceID();
		}

		// Token: 0x04000C2A RID: 3114
		public Navigation.Mode mode;

		// Token: 0x04000C2B RID: 3115
		public long selectOnUp;

		// Token: 0x04000C2C RID: 3116
		public long selectOnDown;

		// Token: 0x04000C2D RID: 3117
		public long selectOnLeft;

		// Token: 0x04000C2E RID: 3118
		public long selectOnRight;
	}
}
