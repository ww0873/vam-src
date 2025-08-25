using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.PersistentObjects
{
	// Token: 0x020001AF RID: 431
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentRigidbody : PersistentComponent
	{
		// Token: 0x060008F7 RID: 2295 RVA: 0x000384D5 File Offset: 0x000368D5
		public PersistentRigidbody()
		{
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x000384E0 File Offset: 0x000368E0
		public override object WriteTo(object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			obj = base.WriteTo(obj, objects);
			if (obj == null)
			{
				return null;
			}
			Rigidbody rigidbody = (Rigidbody)obj;
			rigidbody.velocity = this.velocity;
			rigidbody.angularVelocity = this.angularVelocity;
			rigidbody.drag = this.drag;
			rigidbody.angularDrag = this.angularDrag;
			rigidbody.mass = this.mass;
			rigidbody.useGravity = this.useGravity;
			rigidbody.maxDepenetrationVelocity = this.maxDepenetrationVelocity;
			rigidbody.isKinematic = this.isKinematic;
			rigidbody.freezeRotation = this.freezeRotation;
			rigidbody.constraints = (RigidbodyConstraints)this.constraints;
			rigidbody.collisionDetectionMode = (CollisionDetectionMode)this.collisionDetectionMode;
			rigidbody.centerOfMass = this.centerOfMass;
			rigidbody.detectCollisions = this.detectCollisions;
			rigidbody.position = this.position;
			rigidbody.rotation = this.rotation;
			rigidbody.interpolation = (RigidbodyInterpolation)this.interpolation;
			rigidbody.solverIterations = this.solverIterations;
			rigidbody.solverVelocityIterations = this.solverVelocityIterations;
			rigidbody.sleepThreshold = this.sleepThreshold;
			rigidbody.maxAngularVelocity = this.maxAngularVelocity;
			return rigidbody;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000385F8 File Offset: 0x000369F8
		public override void ReadFrom(object obj)
		{
			base.ReadFrom(obj);
			if (obj == null)
			{
				return;
			}
			Rigidbody rigidbody = (Rigidbody)obj;
			this.velocity = rigidbody.velocity;
			this.angularVelocity = rigidbody.angularVelocity;
			this.drag = rigidbody.drag;
			this.angularDrag = rigidbody.angularDrag;
			this.mass = rigidbody.mass;
			this.useGravity = rigidbody.useGravity;
			this.maxDepenetrationVelocity = rigidbody.maxDepenetrationVelocity;
			this.isKinematic = rigidbody.isKinematic;
			this.freezeRotation = rigidbody.freezeRotation;
			this.constraints = (uint)rigidbody.constraints;
			this.collisionDetectionMode = (uint)rigidbody.collisionDetectionMode;
			this.centerOfMass = rigidbody.centerOfMass;
			this.detectCollisions = rigidbody.detectCollisions;
			this.position = rigidbody.position;
			this.rotation = rigidbody.rotation;
			this.interpolation = (uint)rigidbody.interpolation;
			this.solverIterations = rigidbody.solverIterations;
			this.solverVelocityIterations = rigidbody.solverVelocityIterations;
			this.sleepThreshold = rigidbody.sleepThreshold;
			this.maxAngularVelocity = rigidbody.maxAngularVelocity;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0003870A File Offset: 0x00036B0A
		public override void FindDependencies<T>(Dictionary<long, T> dependencies, Dictionary<long, T> objects, bool allowNulls)
		{
			base.FindDependencies<T>(dependencies, objects, allowNulls);
		}

		// Token: 0x040009E4 RID: 2532
		public Vector3 velocity;

		// Token: 0x040009E5 RID: 2533
		public Vector3 angularVelocity;

		// Token: 0x040009E6 RID: 2534
		public float drag;

		// Token: 0x040009E7 RID: 2535
		public float angularDrag;

		// Token: 0x040009E8 RID: 2536
		public float mass;

		// Token: 0x040009E9 RID: 2537
		public bool useGravity;

		// Token: 0x040009EA RID: 2538
		public float maxDepenetrationVelocity;

		// Token: 0x040009EB RID: 2539
		public bool isKinematic;

		// Token: 0x040009EC RID: 2540
		public bool freezeRotation;

		// Token: 0x040009ED RID: 2541
		public uint constraints;

		// Token: 0x040009EE RID: 2542
		public uint collisionDetectionMode;

		// Token: 0x040009EF RID: 2543
		public Vector3 centerOfMass;

		// Token: 0x040009F0 RID: 2544
		public bool detectCollisions;

		// Token: 0x040009F1 RID: 2545
		public Vector3 position;

		// Token: 0x040009F2 RID: 2546
		public Quaternion rotation;

		// Token: 0x040009F3 RID: 2547
		public uint interpolation;

		// Token: 0x040009F4 RID: 2548
		public int solverIterations;

		// Token: 0x040009F5 RID: 2549
		public int solverVelocityIterations;

		// Token: 0x040009F6 RID: 2550
		public float sleepThreshold;

		// Token: 0x040009F7 RID: 2551
		public float maxAngularVelocity;
	}
}
