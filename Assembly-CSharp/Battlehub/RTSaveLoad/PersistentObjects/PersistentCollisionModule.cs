using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x02000207 RID: 519
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCollisionModule : PersistentData
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x0003FCB9 File Offset: 0x0003E0B9
		public PersistentCollisionModule()
		{
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0003FCC4 File Offset: 0x0003E0C4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			ParticleSystem.CollisionModule collisionModule = (ParticleSystem.CollisionModule)obj;
			collisionModule.enabled = this.enabled;
			collisionModule.type = this.type;
			collisionModule.mode = this.mode;
			collisionModule.dampen = base.Write<ParticleSystem.MinMaxCurve>(collisionModule.dampen, this.dampen, objects);
			collisionModule.bounce = base.Write<ParticleSystem.MinMaxCurve>(collisionModule.bounce, this.bounce, objects);
			collisionModule.lifetimeLoss = base.Write<ParticleSystem.MinMaxCurve>(collisionModule.lifetimeLoss, this.lifetimeLoss, objects);
			collisionModule.dampenMultiplier = this.dampenMultiplier;
			collisionModule.bounceMultiplier = this.bounceMultiplier;
			collisionModule.lifetimeLossMultiplier = this.lifetimeLossMultiplier;
			collisionModule.minKillSpeed = this.minKillSpeed;
			collisionModule.maxKillSpeed = this.maxKillSpeed;
			collisionModule.collidesWith = this.collidesWith;
			collisionModule.enableDynamicColliders = this.enableDynamicColliders;
			collisionModule.maxCollisionShapes = this.maxCollisionShapes;
			collisionModule.quality = this.quality;
			collisionModule.voxelSize = this.voxelSize;
			collisionModule.radiusScale = this.radiusScale;
			collisionModule.sendCollisionMessages = this.sendCollisionMessages;
			if (this.planes == null)
			{
				for (int i = 0; i < collisionModule.maxPlaneCount; i++)
				{
					collisionModule.SetPlane(i, null);
				}
			}
			else
			{
				for (int j = 0; j < Mathf.Min(collisionModule.maxPlaneCount, this.planes.Length); j++)
				{
					collisionModule.SetPlane(j, (Transform)objects.Get(this.planes[j]));
				}
			}
			return collisionModule;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x0003FE78 File Offset: 0x0003E278
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.CollisionModule collisionModule = (ParticleSystem.CollisionModule)obj;
			this.enabled = collisionModule.enabled;
			this.type = collisionModule.type;
			this.mode = collisionModule.mode;
			this.dampen = base.Read<PersistentMinMaxCurve>(this.dampen, collisionModule.dampen);
			this.bounce = base.Read<PersistentMinMaxCurve>(this.bounce, collisionModule.bounce);
			this.lifetimeLoss = base.Read<PersistentMinMaxCurve>(this.lifetimeLoss, collisionModule.lifetimeLoss);
			this.dampenMultiplier = collisionModule.dampenMultiplier;
			this.bounceMultiplier = collisionModule.bounceMultiplier;
			this.lifetimeLossMultiplier = collisionModule.lifetimeLossMultiplier;
			this.minKillSpeed = collisionModule.minKillSpeed;
			this.maxKillSpeed = collisionModule.maxKillSpeed;
			this.collidesWith = collisionModule.collidesWith;
			this.enableDynamicColliders = collisionModule.enableDynamicColliders;
			this.maxCollisionShapes = collisionModule.maxCollisionShapes;
			this.quality = collisionModule.quality;
			this.voxelSize = collisionModule.voxelSize;
			this.radiusScale = collisionModule.radiusScale;
			this.sendCollisionMessages = collisionModule.sendCollisionMessages;
			if (collisionModule.maxPlaneCount > 20)
			{
				Debug.LogWarning("maxPlaneCount is expected to be 6 or at least <= 20");
			}
			this.planes = new long[collisionModule.maxPlaneCount];
			for (int i = 0; i < collisionModule.maxPlaneCount; i++)
			{
				this.planes[i] = collisionModule.GetPlane(i).GetMappedInstanceID();
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00040010 File Offset: 0x0003E410
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependencies<T>(this.planes, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.dampen, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.bounce, dependencies, objects, allowNulls);
			base.FindDependencies<T, PersistentMinMaxCurve>(this.lifetimeLoss, dependencies, objects, allowNulls);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00040064 File Offset: 0x0003E464
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			ParticleSystem.CollisionModule collisionModule = (ParticleSystem.CollisionModule)obj;
			UnityEngine.Object[] array = new UnityEngine.Object[collisionModule.maxPlaneCount];
			for (int i = 0; i < collisionModule.maxPlaneCount; i++)
			{
				array[i] = collisionModule.GetPlane(i);
			}
			base.AddDependencies(array, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.dampen, collisionModule.dampen, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.bounce, collisionModule.bounce, dependencies);
			base.GetDependencies<PersistentMinMaxCurve>(this.lifetimeLoss, collisionModule.lifetimeLoss, dependencies);
		}

		// Token: 0x04000B72 RID: 2930
		public bool enabled;

		// Token: 0x04000B73 RID: 2931
		public ParticleSystemCollisionType type;

		// Token: 0x04000B74 RID: 2932
		public ParticleSystemCollisionMode mode;

		// Token: 0x04000B75 RID: 2933
		public long[] planes;

		// Token: 0x04000B76 RID: 2934
		public PersistentMinMaxCurve dampen;

		// Token: 0x04000B77 RID: 2935
		public PersistentMinMaxCurve bounce;

		// Token: 0x04000B78 RID: 2936
		public PersistentMinMaxCurve lifetimeLoss;

		// Token: 0x04000B79 RID: 2937
		public float dampenMultiplier;

		// Token: 0x04000B7A RID: 2938
		public float bounceMultiplier;

		// Token: 0x04000B7B RID: 2939
		public float lifetimeLossMultiplier;

		// Token: 0x04000B7C RID: 2940
		public float minKillSpeed;

		// Token: 0x04000B7D RID: 2941
		public float maxKillSpeed;

		// Token: 0x04000B7E RID: 2942
		public LayerMask collidesWith;

		// Token: 0x04000B7F RID: 2943
		public bool enableDynamicColliders;

		// Token: 0x04000B80 RID: 2944
		public bool enableInteriorCollisions;

		// Token: 0x04000B81 RID: 2945
		public int maxCollisionShapes;

		// Token: 0x04000B82 RID: 2946
		public ParticleSystemCollisionQuality quality;

		// Token: 0x04000B83 RID: 2947
		public float voxelSize;

		// Token: 0x04000B84 RID: 2948
		public float radiusScale;

		// Token: 0x04000B85 RID: 2949
		public bool sendCollisionMessages;
	}
}
