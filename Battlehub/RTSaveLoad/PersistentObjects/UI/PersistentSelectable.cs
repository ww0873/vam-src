using System;
using System.Collections.Generic;
using Battlehub.RTSaveLoad.PersistentObjects.EventSystems;
using ProtoBuf;
using UnityEngine;
using UnityEngine.UI;

namespace Battlehub.RTSaveLoad.PersistentObjects.UI
{
	// Token: 0x02000223 RID: 547
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[ProtoInclude(1131, typeof(PersistentButton))]
	[ProtoInclude(1132, typeof(PersistentDropdown))]
	[ProtoInclude(1133, typeof(PersistentInputField))]
	[ProtoInclude(1134, typeof(PersistentScrollbar))]
	[ProtoInclude(1135, typeof(PersistentSlider))]
	[ProtoInclude(1136, typeof(PersistentToggle))]
	[Serializable]
	public class PersistentSelectable : PersistentUIBehaviour
	{
		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003A09C File Offset: 0x0003849C
		public PersistentSelectable()
		{
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003A0A4 File Offset: 0x000384A4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Selectable selectable = (Selectable)obj;
			selectable.navigation = default(Navigation);
			this.navigation.WriteTo(selectable.navigation, objects);
			selectable.transition = this.transition;
			selectable.colors = this.colors;
			selectable.spriteState = default(SpriteState);
			this.spriteState.WriteTo(selectable.spriteState, objects);
			selectable.animationTriggers = this.animationTriggers;
			selectable.targetGraphic = (Graphic)objects.Get(this.targetGraphic);
			selectable.interactable = this.interactable;
			selectable.image = (Image)objects.Get(this.image);
			return selectable;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003A178 File Offset: 0x00038578
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Selectable selectable = (Selectable)obj;
			this.navigation = new PersistentNavigation();
			this.navigation.ReadFrom(selectable.navigation);
			this.transition = selectable.transition;
			this.colors = selectable.colors;
			this.spriteState = new PersistentSpriteState();
			this.spriteState.ReadFrom(selectable.spriteState);
			this.animationTriggers = selectable.animationTriggers;
			this.targetGraphic = selectable.targetGraphic.GetMappedInstanceID();
			this.interactable = selectable.interactable;
			this.image = selectable.image.GetMappedInstanceID();
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003A230 File Offset: 0x00038630
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.targetGraphic, dependencies, objects, allowNulls);
			base.AddDependency<T>(this.image, dependencies, objects, allowNulls);
			if (this.navigation != null)
			{
				this.navigation.FindDependencies<T>(dependencies, objects, allowNulls);
			}
			if (this.spriteState != null)
			{
				this.spriteState.FindDependencies<T>(dependencies, objects, allowNulls);
			}
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003A298 File Offset: 0x00038698
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Selectable selectable = (Selectable)obj;
			base.AddDependency(selectable.targetGraphic, dependencies);
			base.AddDependency(selectable.image, dependencies);
			PersistentNavigation persistentNavigation = new PersistentNavigation();
			persistentNavigation.GetDependencies(selectable.navigation, dependencies);
			PersistentSpriteState persistentSpriteState = new PersistentSpriteState();
			persistentSpriteState.GetDependencies(selectable.spriteState, dependencies);
		}

		// Token: 0x04000C4B RID: 3147
		public PersistentNavigation navigation;

		// Token: 0x04000C4C RID: 3148
		public Selectable.Transition transition;

		// Token: 0x04000C4D RID: 3149
		public ColorBlock colors;

		// Token: 0x04000C4E RID: 3150
		public PersistentSpriteState spriteState;

		// Token: 0x04000C4F RID: 3151
		public AnimationTriggers animationTriggers;

		// Token: 0x04000C50 RID: 3152
		public long targetGraphic;

		// Token: 0x04000C51 RID: 3153
		public bool interactable;

		// Token: 0x04000C52 RID: 3154
		public long image;
	}
}
