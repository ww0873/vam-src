using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200010C RID: 268
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAnimationCurve : PersistentData
	{
		// Token: 0x06000685 RID: 1669 RVA: 0x0002D024 File Offset: 0x0002B424
		public PersistentAnimationCurve()
		{
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002D02C File Offset: 0x0002B42C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AnimationCurve animationCurve = (AnimationCurve)obj;
			animationCurve.keys = base.Write<Keyframe>(animationCurve.keys, this.keys, objects);
			animationCurve.preWrapMode = (WrapMode)this.preWrapMode;
			animationCurve.postWrapMode = (WrapMode)this.postWrapMode;
			return animationCurve;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002D084 File Offset: 0x0002B484
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AnimationCurve animationCurve = (AnimationCurve)obj;
			this.keys = base.Read<PersistentKeyframe, Keyframe>(this.keys, animationCurve.keys);
			this.preWrapMode = (uint)animationCurve.preWrapMode;
			this.postWrapMode = (uint)animationCurve.postWrapMode;
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0002D0D6 File Offset: 0x0002B4D6
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentKeyframe>(this.keys, dependencies, objects, allowNulls);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0002D0F0 File Offset: 0x0002B4F0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			AnimationCurve animationCurve = (AnimationCurve)obj;
			base.GetDependencies<PersistentKeyframe, Keyframe>(this.keys, animationCurve.keys, dependencies);
		}

		// Token: 0x04000653 RID: 1619
		public PersistentKeyframe[] keys;

		// Token: 0x04000654 RID: 1620
		public uint preWrapMode;

		// Token: 0x04000655 RID: 1621
		public uint postWrapMode;
	}
}
