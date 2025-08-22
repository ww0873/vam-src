using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000221 RID: 545
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentScrollbar : PersistentSelectable
	{
		// Token: 0x06000AEE RID: 2798 RVA: 0x00043AAF File Offset: 0x00041EAF
		public PersistentScrollbar()
		{
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00043AB8 File Offset: 0x00041EB8
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Scrollbar scrollbar = (Scrollbar)obj;
			scrollbar.handleRect = (RectTransform)objects.Get(this.handleRect);
			scrollbar.direction = this.direction;
			scrollbar.value = this.value;
			scrollbar.size = this.size;
			scrollbar.numberOfSteps = this.numberOfSteps;
			base.Write<Scrollbar.ScrollEvent>(scrollbar.onValueChanged, this.onValueChanged, objects);
			return scrollbar;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00043B3C File Offset: 0x00041F3C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Scrollbar scrollbar = (Scrollbar)obj;
			this.handleRect = scrollbar.handleRect.GetMappedInstanceID();
			this.direction = scrollbar.direction;
			this.value = scrollbar.value;
			this.size = scrollbar.size;
			this.numberOfSteps = scrollbar.numberOfSteps;
			base.Read(this.onValueChanged, scrollbar.onValueChanged);
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00043BB2 File Offset: 0x00041FB2
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.handleRect, dependencies, objects, allowNulls);
			if (this.onValueChanged != null)
			{
				this.onValueChanged.FindDependencies<T>(dependencies, objects, allowNulls);
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00043BE8 File Offset: 0x00041FE8
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Scrollbar scrollbar = (Scrollbar)obj;
			base.AddDependency(scrollbar.handleRect, dependencies);
			PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
			persistentUnityEventBase.GetDependencies(scrollbar.onValueChanged, dependencies);
		}

		// Token: 0x04000C31 RID: 3121
		public long handleRect;

		// Token: 0x04000C32 RID: 3122
		public Scrollbar.Direction direction;

		// Token: 0x04000C33 RID: 3123
		public float value;

		// Token: 0x04000C34 RID: 3124
		public float size;

		// Token: 0x04000C35 RID: 3125
		public int numberOfSteps;

		// Token: 0x04000C36 RID: 3126
		public PersistentUnityEventBase onValueChanged;
	}
}
