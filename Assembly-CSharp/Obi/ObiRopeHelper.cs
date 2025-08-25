using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F9 RID: 1017
	[RequireComponent(typeof(ObiRope))]
	[RequireComponent(typeof(ObiCatmullRomCurve))]
	public class ObiRopeHelper : MonoBehaviour
	{
		// Token: 0x060019E2 RID: 6626 RVA: 0x000905D8 File Offset: 0x0008E9D8
		public ObiRopeHelper()
		{
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x000905E0 File Offset: 0x0008E9E0
		private void Start()
		{
			this.rope = base.GetComponent<ObiRope>();
			this.path = base.GetComponent<ObiCatmullRomCurve>();
			this.rope.Solver = this.solver;
			this.rope.ropePath = this.path;
			this.rope.Section = this.section;
			base.GetComponent<MeshRenderer>().material = this.material;
			Vector3 vector = base.transform.InverseTransformPoint(this.start.position);
			Vector3 vector2 = base.transform.InverseTransformPoint(this.end.position);
			Vector3 normalized = (vector2 - vector).normalized;
			this.path.controlPoints.Clear();
			this.path.controlPoints.Add(vector - normalized);
			this.path.controlPoints.Add(vector);
			this.path.controlPoints.Add(vector2);
			this.path.controlPoints.Add(vector2 + normalized);
			base.StartCoroutine(this.Setup());
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x000906F4 File Offset: 0x0008EAF4
		private IEnumerator Setup()
		{
			yield return base.StartCoroutine(this.rope.GeneratePhysicRepresentationForMesh());
			this.rope.AddToSolver(null);
			this.rope.invMasses[0] = 0f;
			this.rope.invMasses[this.rope.UsedParticles - 1] = 0f;
			Oni.SetParticleInverseMasses(this.solver.OniSolver, new float[1], 1, this.rope.particleIndices[0]);
			Oni.SetParticleInverseMasses(this.solver.OniSolver, new float[1], 1, this.rope.particleIndices[this.rope.UsedParticles - 1]);
			yield break;
		}

		// Token: 0x0400150E RID: 5390
		public ObiSolver solver;

		// Token: 0x0400150F RID: 5391
		public ObiRopeSection section;

		// Token: 0x04001510 RID: 5392
		public Material material;

		// Token: 0x04001511 RID: 5393
		public Transform start;

		// Token: 0x04001512 RID: 5394
		public Transform end;

		// Token: 0x04001513 RID: 5395
		private ObiRope rope;

		// Token: 0x04001514 RID: 5396
		private ObiCatmullRomCurve path;

		// Token: 0x02000F47 RID: 3911
		[CompilerGenerated]
		private sealed class <Setup>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600736A RID: 29546 RVA: 0x0009070F File Offset: 0x0008EB0F
			[DebuggerHidden]
			public <Setup>c__Iterator0()
			{
			}

			// Token: 0x0600736B RID: 29547 RVA: 0x00090718 File Offset: 0x0008EB18
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = base.StartCoroutine(this.rope.GeneratePhysicRepresentationForMesh());
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					this.rope.AddToSolver(null);
					this.rope.invMasses[0] = 0f;
					this.rope.invMasses[this.rope.UsedParticles - 1] = 0f;
					Oni.SetParticleInverseMasses(this.solver.OniSolver, new float[1], 1, this.rope.particleIndices[0]);
					Oni.SetParticleInverseMasses(this.solver.OniSolver, new float[1], 1, this.rope.particleIndices[this.rope.UsedParticles - 1]);
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x170010E9 RID: 4329
			// (get) Token: 0x0600736C RID: 29548 RVA: 0x00090845 File Offset: 0x0008EC45
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010EA RID: 4330
			// (get) Token: 0x0600736D RID: 29549 RVA: 0x0009084D File Offset: 0x0008EC4D
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600736E RID: 29550 RVA: 0x00090855 File Offset: 0x0008EC55
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600736F RID: 29551 RVA: 0x00090865 File Offset: 0x0008EC65
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400675F RID: 26463
			internal ObiRopeHelper $this;

			// Token: 0x04006760 RID: 26464
			internal object $current;

			// Token: 0x04006761 RID: 26465
			internal bool $disposing;

			// Token: 0x04006762 RID: 26466
			internal int $PC;
		}
	}
}
