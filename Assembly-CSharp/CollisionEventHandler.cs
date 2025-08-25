using System;
using Obi;
using UnityEngine;

// Token: 0x0200036E RID: 878
[RequireComponent(typeof(ObiSolver))]
public class CollisionEventHandler : MonoBehaviour
{
	// Token: 0x060015E3 RID: 5603 RVA: 0x0007CFA0 File Offset: 0x0007B3A0
	public CollisionEventHandler()
	{
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x0007CFA8 File Offset: 0x0007B3A8
	private void Awake()
	{
		this.solver = base.GetComponent<ObiSolver>();
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x0007CFB6 File Offset: 0x0007B3B6
	private void OnEnable()
	{
		this.solver.OnCollision += this.Solver_OnCollision;
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x0007CFCF File Offset: 0x0007B3CF
	private void OnDisable()
	{
		this.solver.OnCollision -= this.Solver_OnCollision;
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x0007CFE8 File Offset: 0x0007B3E8
	private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
	{
		this.frame = e;
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x0007CFF4 File Offset: 0x0007B3F4
	private void OnDrawGizmos()
	{
		if (this.solver == null || this.frame == null || this.frame.contacts == null)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		for (int i = 0; i < this.frame.contacts.Length; i++)
		{
			Gizmos.color = ((this.frame.contacts[i].distance >= 0.01f) ? Color.green : Color.red);
			Vector3 vector = this.frame.contacts[i].point;
			Vector3 vector2 = this.frame.contacts[i].normal;
			Gizmos.DrawSphere(vector, 0.025f);
			Gizmos.DrawRay(vector, vector2.normalized * 0.1f);
		}
	}

	// Token: 0x0400124B RID: 4683
	private ObiSolver solver;

	// Token: 0x0400124C RID: 4684
	private ObiSolver.ObiCollisionEventArgs frame;
}
