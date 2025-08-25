using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200012B RID: 299
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentAnimatorOverrideController : PersistentRuntimeAnimatorController
	{
		// Token: 0x06000716 RID: 1814 RVA: 0x00031389 File Offset: 0x0002F789
		public PersistentAnimatorOverrideController()
		{
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00031394 File Offset: 0x0002F794
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			AnimatorOverrideController animatorOverrideController = (AnimatorOverrideController)obj;
			animatorOverrideController.runtimeAnimatorController = (RuntimeAnimatorController)objects.Get(this.runtimeAnimatorController);
			return animatorOverrideController;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000313D4 File Offset: 0x0002F7D4
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			AnimatorOverrideController animatorOverrideController = (AnimatorOverrideController)obj;
			this.runtimeAnimatorController = animatorOverrideController.runtimeAnimatorController.GetMappedInstanceID();
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00031407 File Offset: 0x0002F807
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.runtimeAnimatorController, dependencies, objects, allowNulls);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00031424 File Offset: 0x0002F824
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			AnimatorOverrideController animatorOverrideController = (AnimatorOverrideController)obj;
			base.AddDependency(animatorOverrideController.runtimeAnimatorController, dependencies);
		}

		// Token: 0x0400077E RID: 1918
		public long runtimeAnimatorController;
	}
}
