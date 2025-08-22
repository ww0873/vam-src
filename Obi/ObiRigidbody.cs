using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A7 RID: 935
	[ExecuteInEditMode]
	[RequireComponent(typeof(Rigidbody))]
	public class ObiRigidbody : MonoBehaviour
	{
		// Token: 0x060017A0 RID: 6048 RVA: 0x00086E90 File Offset: 0x00085290
		public ObiRigidbody()
		{
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x00086ED3 File Offset: 0x000852D3
		public IntPtr OniRigidbody
		{
			get
			{
				return this.oniRigidbody;
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00086EDB File Offset: 0x000852DB
		public void Awake()
		{
			this.unityRigidbody = base.GetComponent<Rigidbody>();
			this.oniRigidbody = Oni.CreateRigidbody();
			this.UpdateIfNeeded();
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00086EFA File Offset: 0x000852FA
		public void OnDestroy()
		{
			Oni.DestroyRigidbody(this.oniRigidbody);
			this.oniRigidbody = IntPtr.Zero;
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00086F14 File Offset: 0x00085314
		public void UpdateIfNeeded()
		{
			if (this.dirty)
			{
				this.velocity = this.unityRigidbody.velocity;
				this.angularVelocity = this.unityRigidbody.angularVelocity;
				this.adaptor.Set(this.unityRigidbody, this.kinematicForParticles);
				Oni.UpdateRigidbody(this.oniRigidbody, ref this.adaptor);
				this.dirty = false;
			}
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00086F80 File Offset: 0x00085380
		public void UpdateVelocities()
		{
			if (!this.dirty)
			{
				if (this.unityRigidbody.isKinematic || !this.kinematicForParticles)
				{
					Oni.GetRigidbodyVelocity(this.oniRigidbody, ref this.oniVelocities);
					this.unityRigidbody.velocity += this.oniVelocities.linearVelocity - this.velocity;
					this.unityRigidbody.angularVelocity += this.oniVelocities.angularVelocity - this.angularVelocity;
				}
				this.dirty = true;
			}
		}

		// Token: 0x04001371 RID: 4977
		public bool kinematicForParticles;

		// Token: 0x04001372 RID: 4978
		private IntPtr oniRigidbody = IntPtr.Zero;

		// Token: 0x04001373 RID: 4979
		private Rigidbody unityRigidbody;

		// Token: 0x04001374 RID: 4980
		private bool dirty = true;

		// Token: 0x04001375 RID: 4981
		private Oni.Rigidbody adaptor = default(Oni.Rigidbody);

		// Token: 0x04001376 RID: 4982
		private Oni.RigidbodyVelocities oniVelocities = default(Oni.RigidbodyVelocities);

		// Token: 0x04001377 RID: 4983
		private Vector3 velocity;

		// Token: 0x04001378 RID: 4984
		private Vector3 angularVelocity;
	}
}
