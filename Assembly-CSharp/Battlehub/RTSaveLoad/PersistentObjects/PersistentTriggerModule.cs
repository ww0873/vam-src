using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200020A RID: 522
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentTriggerModule : PersistentData
	{
		// Token: 0x06000A7D RID: 2685 RVA: 0x00040B83 File Offset: 0x0003EF83
		public PersistentTriggerModule()
		{
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00040B8C File Offset: 0x0003EF8C
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.TriggerModule triggerModule = (ParticleSystem.TriggerModule)obj;
			triggerModule.enabled = this.enabled;
			triggerModule.inside = this.inside;
			triggerModule.outside = this.outside;
			triggerModule.enter = this.enter;
			triggerModule.exit = this.exit;
			triggerModule.radiusScale = this.radiusScale;
			if (this.colliders == null)
			{
				for (int i = 0; i < triggerModule.maxColliderCount; i++)
				{
					triggerModule.SetCollider(i, null);
				}
			}
			else
			{
				for (int j = 0; j < Mathf.Min(triggerModule.maxColliderCount, this.colliders.Length); j++)
				{
					object obj2 = objects.Get(this.colliders[j]);
					triggerModule.SetCollider(j, (Component)obj2);
				}
			}
			return triggerModule;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00040C7C File Offset: 0x0003F07C
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.TriggerModule triggerModule = (ParticleSystem.TriggerModule)obj;
			this.enabled = triggerModule.enabled;
			this.inside = triggerModule.inside;
			this.outside = triggerModule.outside;
			this.enter = triggerModule.enter;
			this.exit = triggerModule.exit;
			this.radiusScale = triggerModule.radiusScale;
			if (triggerModule.maxColliderCount > 20)
			{
				Debug.LogWarning("maxPlaneCount is expected to be 6 or at least <= 20");
			}
			this.colliders = new long[triggerModule.maxColliderCount];
			for (int i = 0; i < triggerModule.maxColliderCount; i++)
			{
				Component collider = triggerModule.GetCollider(i);
				this.colliders[i] = collider.GetMappedInstanceID();
			}
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00040D45 File Offset: 0x0003F145
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependencies<T>(this.colliders, dependencies, objects, allowNulls);
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00040D60 File Offset: 0x0003F160
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.TriggerModule triggerModule = (ParticleSystem.TriggerModule)obj;
			UnityEngine.Object[] array = new UnityEngine.Object[triggerModule.maxColliderCount];
			for (int i = 0; i < triggerModule.maxColliderCount; i++)
			{
				array[i] = triggerModule.GetCollider(i);
			}
			base.AddDependencies(array, dependencies);
		}

		// Token: 0x04000BA4 RID: 2980
		public bool enabled;

		// Token: 0x04000BA5 RID: 2981
		public ParticleSystemOverlapAction inside;

		// Token: 0x04000BA6 RID: 2982
		public ParticleSystemOverlapAction outside;

		// Token: 0x04000BA7 RID: 2983
		public ParticleSystemOverlapAction enter;

		// Token: 0x04000BA8 RID: 2984
		public ParticleSystemOverlapAction exit;

		// Token: 0x04000BA9 RID: 2985
		public float radiusScale;

		// Token: 0x04000BAA RID: 2986
		public long[] colliders;
	}
}
