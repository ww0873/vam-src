using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F6 RID: 1014
	[ExecuteInEditMode]
	public class ObiParticleHandle : MonoBehaviour
	{
		// Token: 0x060019CE RID: 6606 RVA: 0x0008F3A9 File Offset: 0x0008D7A9
		public ObiParticleHandle()
		{
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x0008F3D2 File Offset: 0x0008D7D2
		public int ParticleCount
		{
			get
			{
				return this.handledParticleIndices.Count;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060019D1 RID: 6609 RVA: 0x0008F48B File Offset: 0x0008D88B
		// (set) Token: 0x060019D0 RID: 6608 RVA: 0x0008F3E0 File Offset: 0x0008D7E0
		public ObiActor Actor
		{
			get
			{
				return this.actor;
			}
			set
			{
				if (this.actor != value)
				{
					if (this.actor != null && this.actor.Solver != null)
					{
						this.actor.Solver.OnFrameBegin -= this.Actor_solver_OnFrameBegin;
					}
					this.actor = value;
					if (this.actor != null && this.actor.Solver != null)
					{
						this.actor.Solver.OnFrameBegin += this.Actor_solver_OnFrameBegin;
					}
				}
			}
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0008F494 File Offset: 0x0008D894
		private void OnEnable()
		{
			if (this.actor != null && this.actor.Solver != null)
			{
				this.actor.Solver.OnFrameBegin += this.Actor_solver_OnFrameBegin;
			}
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0008F4E4 File Offset: 0x0008D8E4
		private void OnDisable()
		{
			if (this.actor != null && this.actor.Solver != null)
			{
				this.actor.Solver.OnFrameBegin -= this.Actor_solver_OnFrameBegin;
				this.ResetInvMasses();
			}
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x0008F53C File Offset: 0x0008D93C
		private void ResetInvMasses()
		{
			if (this.actor.InSolver)
			{
				float[] array = new float[1];
				for (int i = 0; i < this.handledParticleIndices.Count; i++)
				{
					int destOffset = this.actor.particleIndices[this.handledParticleIndices[i]];
					array[0] = (this.actor.invMasses[this.handledParticleIndices[i]] = this.handledParticleInvMasses[i]);
					Oni.SetParticleInverseMasses(this.actor.Solver.OniSolver, array, 1, destOffset);
				}
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x0008F5D8 File Offset: 0x0008D9D8
		public void Clear()
		{
			this.ResetInvMasses();
			this.handledParticleIndices.Clear();
			this.handledParticlePositions.Clear();
			this.handledParticleInvMasses.Clear();
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x0008F601 File Offset: 0x0008DA01
		public void AddParticle(int index, Vector3 position, float invMass)
		{
			this.handledParticleIndices.Add(index);
			this.handledParticlePositions.Add(base.transform.InverseTransformPoint(position));
			this.handledParticleInvMasses.Add(invMass);
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x0008F634 File Offset: 0x0008DA34
		public void RemoveParticle(int index)
		{
			int num = this.handledParticleIndices.IndexOf(index);
			if (num > -1)
			{
				if (this.actor.InSolver)
				{
					int destOffset = this.actor.particleIndices[index];
					float[] invMasses = new float[]
					{
						this.actor.invMasses[index] = this.handledParticleInvMasses[num]
					};
					Oni.SetParticleInverseMasses(this.actor.Solver.OniSolver, invMasses, 1, destOffset);
				}
				this.handledParticleIndices.RemoveAt(num);
				this.handledParticlePositions.RemoveAt(num);
				this.handledParticleInvMasses.RemoveAt(num);
			}
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0008F6D8 File Offset: 0x0008DAD8
		private void Actor_solver_OnFrameBegin(object sender, EventArgs e)
		{
			if (this.actor.InSolver)
			{
				Vector4[] array = new Vector4[1];
				Vector4[] velocities = new Vector4[]
				{
					-this.actor.Solver.parameters.gravity * Time.fixedDeltaTime
				};
				float[] invMasses = new float[]
				{
					0.0001f
				};
				Matrix4x4 matrix4x;
				if (this.actor.Solver.simulateInLocalSpace)
				{
					matrix4x = this.actor.Solver.transform.worldToLocalMatrix * base.transform.localToWorldMatrix;
				}
				else
				{
					matrix4x = base.transform.localToWorldMatrix;
				}
				for (int i = 0; i < this.handledParticleIndices.Count; i++)
				{
					int destOffset = this.actor.particleIndices[this.handledParticleIndices[i]];
					Oni.SetParticleVelocities(this.actor.Solver.OniSolver, velocities, 1, destOffset);
					Oni.SetParticleInverseMasses(this.actor.Solver.OniSolver, invMasses, 1, destOffset);
					array[0] = matrix4x.MultiplyPoint3x4(this.handledParticlePositions[i]);
					Oni.SetParticlePositions(this.actor.Solver.OniSolver, array, 1, destOffset);
				}
			}
		}

		// Token: 0x040014FE RID: 5374
		[SerializeField]
		[HideInInspector]
		private ObiActor actor;

		// Token: 0x040014FF RID: 5375
		[SerializeField]
		[HideInInspector]
		private List<int> handledParticleIndices = new List<int>();

		// Token: 0x04001500 RID: 5376
		[SerializeField]
		[HideInInspector]
		private List<Vector3> handledParticlePositions = new List<Vector3>();

		// Token: 0x04001501 RID: 5377
		[SerializeField]
		[HideInInspector]
		private List<float> handledParticleInvMasses = new List<float>();

		// Token: 0x04001502 RID: 5378
		private const float HANDLED_PARTICLE_MASS = 0.0001f;
	}
}
