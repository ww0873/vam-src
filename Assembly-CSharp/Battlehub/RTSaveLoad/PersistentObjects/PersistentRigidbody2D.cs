using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001B0 RID: 432
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRigidbody2D : PersistentComponent
	{
		// Token: 0x060008FB RID: 2299 RVA: 0x00038715 File Offset: 0x00036B15
		public PersistentRigidbody2D()
		{
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00038720 File Offset: 0x00036B20
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Rigidbody2D rigidbody2D = (Rigidbody2D)obj;
			rigidbody2D.position = this.position;
			rigidbody2D.rotation = this.rotation;
			rigidbody2D.velocity = this.velocity;
			rigidbody2D.angularVelocity = this.angularVelocity;
			rigidbody2D.useAutoMass = this.useAutoMass;
			rigidbody2D.mass = this.mass;
			rigidbody2D.sharedMaterial = (PhysicsMaterial2D)objects.Get(this.sharedMaterial);
			rigidbody2D.centerOfMass = this.centerOfMass;
			rigidbody2D.inertia = this.inertia;
			rigidbody2D.drag = this.drag;
			rigidbody2D.angularDrag = this.angularDrag;
			rigidbody2D.gravityScale = this.gravityScale;
			rigidbody2D.bodyType = (RigidbodyType2D)this.bodyType;
			rigidbody2D.useFullKinematicContacts = this.useFullKinematicContacts;
			rigidbody2D.isKinematic = this.isKinematic;
			rigidbody2D.freezeRotation = this.freezeRotation;
			rigidbody2D.constraints = (RigidbodyConstraints2D)this.constraints;
			rigidbody2D.simulated = this.simulated;
			rigidbody2D.interpolation = (RigidbodyInterpolation2D)this.interpolation;
			rigidbody2D.sleepMode = (RigidbodySleepMode2D)this.sleepMode;
			rigidbody2D.collisionDetectionMode = (CollisionDetectionMode2D)this.collisionDetectionMode;
			return rigidbody2D;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00038850 File Offset: 0x00036C50
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Rigidbody2D rigidbody2D = (Rigidbody2D)obj;
			this.position = rigidbody2D.position;
			this.rotation = rigidbody2D.rotation;
			this.velocity = rigidbody2D.velocity;
			this.angularVelocity = rigidbody2D.angularVelocity;
			this.useAutoMass = rigidbody2D.useAutoMass;
			this.mass = rigidbody2D.mass;
			this.sharedMaterial = rigidbody2D.sharedMaterial.GetMappedInstanceID();
			this.centerOfMass = rigidbody2D.centerOfMass;
			this.inertia = rigidbody2D.inertia;
			this.drag = rigidbody2D.drag;
			this.angularDrag = rigidbody2D.angularDrag;
			this.gravityScale = rigidbody2D.gravityScale;
			this.bodyType = (uint)rigidbody2D.bodyType;
			this.useFullKinematicContacts = rigidbody2D.useFullKinematicContacts;
			this.isKinematic = rigidbody2D.isKinematic;
			this.freezeRotation = rigidbody2D.freezeRotation;
			this.constraints = (uint)rigidbody2D.constraints;
			this.simulated = rigidbody2D.simulated;
			this.interpolation = (uint)rigidbody2D.interpolation;
			this.sleepMode = (uint)rigidbody2D.sleepMode;
			this.collisionDetectionMode = (uint)rigidbody2D.collisionDetectionMode;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00038973 File Offset: 0x00036D73
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
			base.AddDependency<T>(this.sharedMaterial, dependencies, objects, allowNulls);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00038990 File Offset: 0x00036D90
		protected override void GetDependencies(Dictionary<long, UnityEngine.Object> dependencies, object obj)
		{
			base.GetDependencies(dependencies, obj);
			if (obj == null)
			{
				return;
			}
			Rigidbody2D rigidbody2D = (Rigidbody2D)obj;
			base.AddDependency(rigidbody2D.sharedMaterial, dependencies);
		}

		// Token: 0x040009F8 RID: 2552
		public Vector2 position;

		// Token: 0x040009F9 RID: 2553
		public float rotation;

		// Token: 0x040009FA RID: 2554
		public Vector2 velocity;

		// Token: 0x040009FB RID: 2555
		public float angularVelocity;

		// Token: 0x040009FC RID: 2556
		public bool useAutoMass;

		// Token: 0x040009FD RID: 2557
		public float mass;

		// Token: 0x040009FE RID: 2558
		public long sharedMaterial;

		// Token: 0x040009FF RID: 2559
		public Vector2 centerOfMass;

		// Token: 0x04000A00 RID: 2560
		public float inertia;

		// Token: 0x04000A01 RID: 2561
		public float drag;

		// Token: 0x04000A02 RID: 2562
		public float angularDrag;

		// Token: 0x04000A03 RID: 2563
		public float gravityScale;

		// Token: 0x04000A04 RID: 2564
		public uint bodyType;

		// Token: 0x04000A05 RID: 2565
		public bool useFullKinematicContacts;

		// Token: 0x04000A06 RID: 2566
		public bool isKinematic;

		// Token: 0x04000A07 RID: 2567
		public bool freezeRotation;

		// Token: 0x04000A08 RID: 2568
		public uint constraints;

		// Token: 0x04000A09 RID: 2569
		public bool simulated;

		// Token: 0x04000A0A RID: 2570
		public uint interpolation;

		// Token: 0x04000A0B RID: 2571
		public uint sleepMode;

		// Token: 0x04000A0C RID: 2572
		public uint collisionDetectionMode;
	}
}
