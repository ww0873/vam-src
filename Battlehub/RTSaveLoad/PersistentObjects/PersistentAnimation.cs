using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000128 RID: 296
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAnimation : PersistentBehaviour
	{
		// Token: 0x06000708 RID: 1800 RVA: 0x00030F19 File Offset: 0x0002F319
		public PersistentAnimation()
		{
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00030F24 File Offset: 0x0002F324
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Animation animation = (Animation)obj;
			animation.clip = (AnimationClip)objects.Get(this.clip);
			animation.playAutomatically = this.playAutomatically;
			animation.wrapMode = (WrapMode)this.wrapMode;
			animation.animatePhysics = this.animatePhysics;
			animation.cullingType = (AnimationCullingType)this.cullingType;
			animation.localBounds = this.localBounds;
			return animation;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00030FA0 File Offset: 0x0002F3A0
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Animation animation = (Animation)obj;
			this.clip = animation.clip.GetMappedInstanceID();
			this.playAutomatically = animation.playAutomatically;
			this.wrapMode = (uint)animation.wrapMode;
			this.animatePhysics = animation.animatePhysics;
			this.cullingType = (uint)animation.cullingType;
			this.localBounds = animation.localBounds;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0003100F File Offset: 0x0002F40F
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.clip, dependencies, objects, allowNulls);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0003102C File Offset: 0x0002F42C
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Animation animation = (Animation)obj;
			base.AddDependency(animation.clip, dependencies);
		}

		// Token: 0x04000764 RID: 1892
		public long clip;

		// Token: 0x04000765 RID: 1893
		public bool playAutomatically;

		// Token: 0x04000766 RID: 1894
		public uint wrapMode;

		// Token: 0x04000767 RID: 1895
		public bool animatePhysics;

		// Token: 0x04000768 RID: 1896
		public uint cullingType;

		// Token: 0x04000769 RID: 1897
		public Bounds localBounds;
	}
}
