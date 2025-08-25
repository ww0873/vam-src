using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x0200020B RID: 523
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentCloth : PersistentComponent
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x00040DBB File Offset: 0x0003F1BB
		public PersistentCloth()
		{
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00040DC4 File Offset: 0x0003F1C4
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Cloth cloth = (Cloth)obj;
			cloth.sleepThreshold = this.sleepThreshold;
			cloth.bendingStiffness = this.bendingStiffness;
			cloth.stretchingStiffness = this.stretchingStiffness;
			cloth.damping = this.damping;
			cloth.externalAcceleration = this.externalAcceleration;
			cloth.randomAcceleration = this.randomAcceleration;
			cloth.useGravity = this.useGravity;
			cloth.enabled = this.enabled;
			cloth.friction = this.friction;
			cloth.collisionMassScale = this.collisionMassScale;
			cloth.useVirtualParticles = this.useVirtualParticles;
			cloth.coefficients = this.coefficients;
			cloth.worldVelocityScale = this.worldVelocityScale;
			cloth.worldAccelerationScale = this.worldAccelerationScale;
			cloth.capsuleColliders = base.Resolve<CapsuleCollider, UnityEngine.Object>(this.capsuleColliders, objects);
			return cloth;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00040EA8 File Offset: 0x0003F2A8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Cloth cloth = (Cloth)obj;
			this.sleepThreshold = cloth.sleepThreshold;
			this.bendingStiffness = cloth.bendingStiffness;
			this.stretchingStiffness = cloth.stretchingStiffness;
			this.damping = cloth.damping;
			this.externalAcceleration = cloth.externalAcceleration;
			this.randomAcceleration = cloth.randomAcceleration;
			this.useGravity = cloth.useGravity;
			this.enabled = cloth.enabled;
			this.friction = cloth.friction;
			this.collisionMassScale = cloth.collisionMassScale;
			this.useVirtualParticles = cloth.useVirtualParticles;
			this.coefficients = cloth.coefficients;
			this.worldVelocityScale = cloth.worldVelocityScale;
			this.worldAccelerationScale = cloth.worldAccelerationScale;
			this.capsuleColliders = cloth.capsuleColliders.GetMappedInstanceID();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00040F83 File Offset: 0x0003F383
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependencies<T>(this.capsuleColliders, dependencies, objects, allowNulls);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00040FA0 File Offset: 0x0003F3A0
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Cloth cloth = (Cloth)obj;
			base.AddDependencies(cloth.capsuleColliders, dependencies);
		}

		// Token: 0x04000BAB RID: 2987
		public float sleepThreshold;

		// Token: 0x04000BAC RID: 2988
		public float bendingStiffness;

		// Token: 0x04000BAD RID: 2989
		public float stretchingStiffness;

		// Token: 0x04000BAE RID: 2990
		public float damping;

		// Token: 0x04000BAF RID: 2991
		public Vector3 externalAcceleration;

		// Token: 0x04000BB0 RID: 2992
		public Vector3 randomAcceleration;

		// Token: 0x04000BB1 RID: 2993
		public bool useGravity;

		// Token: 0x04000BB2 RID: 2994
		public bool enabled;

		// Token: 0x04000BB3 RID: 2995
		public float friction;

		// Token: 0x04000BB4 RID: 2996
		public float collisionMassScale;

		// Token: 0x04000BB5 RID: 2997
		public float useContinuousCollision;

		// Token: 0x04000BB6 RID: 2998
		public float useVirtualParticles;

		// Token: 0x04000BB7 RID: 2999
		public ClothSkinningCoefficient[] coefficients;

		// Token: 0x04000BB8 RID: 3000
		public float worldVelocityScale;

		// Token: 0x04000BB9 RID: 3001
		public float worldAccelerationScale;

		// Token: 0x04000BBA RID: 3002
		public bool solverFrequency;

		// Token: 0x04000BBB RID: 3003
		public long[] capsuleColliders;
	}
}
