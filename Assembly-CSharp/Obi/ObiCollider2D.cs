using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A4 RID: 932
	[ExecuteInEditMode]
	[RequireComponent(typeof(Collider2D))]
	public class ObiCollider2D : ObiColliderBase
	{
		// Token: 0x06001783 RID: 6019 RVA: 0x00086BFE File Offset: 0x00084FFE
		public ObiCollider2D()
		{
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00086C08 File Offset: 0x00085008
		protected override void CreateTracker()
		{
			if (this.unityCollider is CircleCollider2D)
			{
				this.tracker = new ObiCircleShapeTracker2D((CircleCollider2D)this.unityCollider);
			}
			else if (this.unityCollider is BoxCollider2D)
			{
				this.tracker = new ObiBoxShapeTracker2D((BoxCollider2D)this.unityCollider);
			}
			else if (this.unityCollider is CapsuleCollider2D)
			{
				this.tracker = new ObiCapsuleShapeTracker2D((CapsuleCollider2D)this.unityCollider);
			}
			else if (this.unityCollider is EdgeCollider2D)
			{
				this.tracker = new ObiEdgeShapeTracker2D((EdgeCollider2D)this.unityCollider);
			}
			else
			{
				Debug.LogWarning("Collider2D type not supported by Obi.");
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00086CCB File Offset: 0x000850CB
		protected override bool IsUnityColliderEnabled()
		{
			return ((Collider2D)this.unityCollider).enabled;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00086CE0 File Offset: 0x000850E0
		protected override void UpdateColliderAdaptor()
		{
			this.adaptor.Set((Collider2D)this.unityCollider, this.phase, this.thickness);
			foreach (ObiSolver obiSolver in this.solvers)
			{
				if (obiSolver.simulateInLocalSpace)
				{
					this.adaptor.SetSpaceTransform(obiSolver.transform);
					if (this.solvers.Count > 1)
					{
						Debug.LogWarning("ObiColliders used by ObiSolvers simulating in local space cannot be shared by multiple solvers.Please duplicate the collider if you want to use it in other solvers.");
						break;
					}
				}
			}
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00086D94 File Offset: 0x00085194
		protected override void Awake()
		{
			this.unityCollider = base.GetComponent<Collider2D>();
			if (this.unityCollider == null)
			{
				return;
			}
			base.Awake();
		}
	}
}
