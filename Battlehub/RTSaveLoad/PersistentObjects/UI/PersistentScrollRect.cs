using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000222 RID: 546
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentScrollRect : PersistentUIBehaviour
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x00043C2B File Offset: 0x0004202B
		public PersistentScrollRect()
		{
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00043C34 File Offset: 0x00042034
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ScrollRect scrollRect = (ScrollRect)obj;
			scrollRect.content = (RectTransform)objects.Get(this.content);
			scrollRect.horizontal = this.horizontal;
			scrollRect.vertical = this.vertical;
			scrollRect.movementType = this.movementType;
			scrollRect.elasticity = this.elasticity;
			scrollRect.inertia = this.inertia;
			scrollRect.decelerationRate = this.decelerationRate;
			scrollRect.scrollSensitivity = this.scrollSensitivity;
			scrollRect.viewport = (RectTransform)objects.Get(this.viewport);
			scrollRect.horizontalScrollbar = (Scrollbar)objects.Get(this.horizontalScrollbar);
			scrollRect.verticalScrollbar = (Scrollbar)objects.Get(this.verticalScrollbar);
			scrollRect.horizontalScrollbarVisibility = this.horizontalScrollbarVisibility;
			scrollRect.verticalScrollbarVisibility = this.verticalScrollbarVisibility;
			scrollRect.horizontalScrollbarSpacing = this.horizontalScrollbarSpacing;
			scrollRect.verticalScrollbarSpacing = this.verticalScrollbarSpacing;
			this.onValueChanged.WriteTo(scrollRect.onValueChanged, objects);
			scrollRect.velocity = this.velocity;
			scrollRect.normalizedPosition = this.normalizedPosition;
			scrollRect.horizontalNormalizedPosition = this.horizontalNormalizedPosition;
			scrollRect.verticalNormalizedPosition = this.verticalNormalizedPosition;
			return scrollRect;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00043D80 File Offset: 0x00042180
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ScrollRect scrollRect = (ScrollRect)obj;
			this.content = scrollRect.content.GetMappedInstanceID();
			this.horizontal = scrollRect.horizontal;
			this.vertical = scrollRect.vertical;
			this.movementType = scrollRect.movementType;
			this.elasticity = scrollRect.elasticity;
			this.inertia = scrollRect.inertia;
			this.decelerationRate = scrollRect.decelerationRate;
			this.scrollSensitivity = scrollRect.scrollSensitivity;
			this.viewport = scrollRect.viewport.GetMappedInstanceID();
			this.horizontalScrollbar = scrollRect.horizontalScrollbar.GetMappedInstanceID();
			this.verticalScrollbar = scrollRect.verticalScrollbar.GetMappedInstanceID();
			this.horizontalScrollbarVisibility = scrollRect.horizontalScrollbarVisibility;
			this.verticalScrollbarVisibility = scrollRect.verticalScrollbarVisibility;
			this.horizontalScrollbarSpacing = scrollRect.horizontalScrollbarSpacing;
			this.verticalScrollbarSpacing = scrollRect.verticalScrollbarSpacing;
			this.onValueChanged = new PersistentUnityEventBase();
			this.onValueChanged.ReadFrom(scrollRect.onValueChanged);
			this.velocity = scrollRect.velocity;
			this.normalizedPosition = scrollRect.normalizedPosition;
			this.horizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition;
			this.verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00043EB8 File Offset: 0x000422B8
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.content, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.viewport, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.horizontalScrollbar, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.verticalScrollbar, dependencies, objects, allowNulls);
			this.onValueChanged.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00043F18 File Offset: 0x00042318
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ScrollRect scrollRect = (ScrollRect)obj;
			base.AddDependency(scrollRect.content, dependencies);
			base.AddDependency(scrollRect.viewport, dependencies);
			base.AddDependency(scrollRect.horizontalScrollbar, dependencies);
			base.AddDependency(scrollRect.verticalScrollbar, dependencies);
			PersistentUnityEventBase persistentUnityEventBase = new PersistentUnityEventBase();
			persistentUnityEventBase.GetDependencies(scrollRect.onValueChanged, dependencies);
		}

		// Token: 0x04000C37 RID: 3127
		public long content;

		// Token: 0x04000C38 RID: 3128
		public bool horizontal;

		// Token: 0x04000C39 RID: 3129
		public bool vertical;

		// Token: 0x04000C3A RID: 3130
		public ScrollRect.MovementType movementType;

		// Token: 0x04000C3B RID: 3131
		public float elasticity;

		// Token: 0x04000C3C RID: 3132
		public bool inertia;

		// Token: 0x04000C3D RID: 3133
		public float decelerationRate;

		// Token: 0x04000C3E RID: 3134
		public float scrollSensitivity;

		// Token: 0x04000C3F RID: 3135
		public long viewport;

		// Token: 0x04000C40 RID: 3136
		public long horizontalScrollbar;

		// Token: 0x04000C41 RID: 3137
		public long verticalScrollbar;

		// Token: 0x04000C42 RID: 3138
		public ScrollRect.ScrollbarVisibility horizontalScrollbarVisibility;

		// Token: 0x04000C43 RID: 3139
		public ScrollRect.ScrollbarVisibility verticalScrollbarVisibility;

		// Token: 0x04000C44 RID: 3140
		public float horizontalScrollbarSpacing;

		// Token: 0x04000C45 RID: 3141
		public float verticalScrollbarSpacing;

		// Token: 0x04000C46 RID: 3142
		public PersistentUnityEventBase onValueChanged;

		// Token: 0x04000C47 RID: 3143
		public Vector2 velocity;

		// Token: 0x04000C48 RID: 3144
		public Vector2 normalizedPosition;

		// Token: 0x04000C49 RID: 3145
		public float horizontalNormalizedPosition;

		// Token: 0x04000C4A RID: 3146
		public float verticalNormalizedPosition;
	}
}
