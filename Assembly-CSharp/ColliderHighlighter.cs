using System;
using Obi;
using UnityEngine;

// Token: 0x0200036D RID: 877
[RequireComponent(typeof(ObiSolver))]
public class ColliderHighlighter : MonoBehaviour
{
	// Token: 0x060015DE RID: 5598 RVA: 0x0007CEDF File Offset: 0x0007B2DF
	public ColliderHighlighter()
	{
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x0007CEE7 File Offset: 0x0007B2E7
	private void Awake()
	{
		this.solver = base.GetComponent<ObiSolver>();
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x0007CEF5 File Offset: 0x0007B2F5
	private void OnEnable()
	{
		this.solver.OnCollision += this.Solver_OnCollision;
	}

	// Token: 0x060015E1 RID: 5601 RVA: 0x0007CF0E File Offset: 0x0007B30E
	private void OnDisable()
	{
		this.solver.OnCollision -= this.Solver_OnCollision;
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x0007CF28 File Offset: 0x0007B328
	private void Solver_OnCollision(object sender, ObiSolver.ObiCollisionEventArgs e)
	{
		foreach (Oni.Contact contact in e.contacts)
		{
			if (contact.distance < 0.01f)
			{
				Collider collider = ObiColliderBase.idToCollider[contact.other] as Collider;
				Blinker component = collider.GetComponent<Blinker>();
				if (component)
				{
					component.Blink();
				}
			}
		}
	}

	// Token: 0x0400124A RID: 4682
	private ObiSolver solver;
}
