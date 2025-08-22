using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F5 RID: 1013
	[RequireComponent(typeof(ObiSolver))]
	public class ObiParticleGridDebugger : MonoBehaviour
	{
		// Token: 0x060019C9 RID: 6601 RVA: 0x0008F2AC File Offset: 0x0008D6AC
		public ObiParticleGridDebugger()
		{
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x0008F2B4 File Offset: 0x0008D6B4
		private void OnEnable()
		{
			this.solver = base.GetComponent<ObiSolver>();
			this.solver.OnFrameEnd += this.Solver_OnFrameEnd;
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0008F2D9 File Offset: 0x0008D6D9
		private void OnDisable()
		{
			this.solver.OnFrameEnd -= this.Solver_OnFrameEnd;
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x0008F2F4 File Offset: 0x0008D6F4
		private void Solver_OnFrameEnd(object sender, EventArgs e)
		{
			int particleGridSize = Oni.GetParticleGridSize(this.solver.OniSolver);
			this.cells = new Oni.GridCell[particleGridSize];
			Oni.GetParticleGrid(this.solver.OniSolver, this.cells);
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x0008F334 File Offset: 0x0008D734
		private void OnDrawGizmos()
		{
			if (this.cells == null)
			{
				return;
			}
			foreach (Oni.GridCell gridCell in this.cells)
			{
				Gizmos.color = ((gridCell.count <= 0) ? Color.red : Color.yellow);
				Gizmos.DrawWireCube(gridCell.center, gridCell.size);
			}
		}

		// Token: 0x040014FC RID: 5372
		private ObiSolver solver;

		// Token: 0x040014FD RID: 5373
		private Oni.GridCell[] cells;
	}
}
