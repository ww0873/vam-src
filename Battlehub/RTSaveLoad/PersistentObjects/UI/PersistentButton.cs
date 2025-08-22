using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x0200021B RID: 539
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentButton : PersistentSelectable
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x000431A8 File Offset: 0x000415A8
		public PersistentButton()
		{
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000431B0 File Offset: 0x000415B0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Button button = (Button)obj;
			base.Write<Button.ButtonClickedEvent>(button.onClick, this.onClick, objects);
			return button;
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000431EC File Offset: 0x000415EC
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Button button = (Button)obj;
			base.Read(this.onClick, button.onClick);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x00043221 File Offset: 0x00041621
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			if (this.onClick != null)
			{
				this.onClick.FindDependencies<T>(dependencies, objects, allowNulls);
			}
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x00043248 File Offset: 0x00041648
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			Button button = (Button)obj;
			if (button == null)
			{
				return;
			}
			PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
			persistentUnityEventBase.GetDependencies(button.onClick, dependencies);
		}

		// Token: 0x04000C08 RID: 3080
		public PersistentUnityEventBase onClick;
	}
}
