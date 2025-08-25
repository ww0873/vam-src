using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A3 RID: 931
	[ExecuteInEditMode]
	[RequireComponent(typeof(Collider))]
	public class ObiCollider : ObiColliderBase
	{
		// Token: 0x0600177E RID: 6014 RVA: 0x000869EE File Offset: 0x00084DEE
		public ObiCollider()
		{
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x000869F8 File Offset: 0x00084DF8
		protected override void CreateTracker()
		{
			if (this.unityCollider is SphereCollider)
			{
				this.tracker = new ObiSphereShapeTracker((SphereCollider)this.unityCollider);
			}
			else if (this.unityCollider is BoxCollider)
			{
				this.tracker = new ObiBoxShapeTracker((BoxCollider)this.unityCollider);
			}
			else if (this.unityCollider is CapsuleCollider)
			{
				this.tracker = new ObiCapsuleShapeTracker((CapsuleCollider)this.unityCollider);
			}
			else if (this.unityCollider is CharacterController)
			{
				this.tracker = new ObiCapsuleShapeTracker((CharacterController)this.unityCollider);
			}
			else if (this.unityCollider is TerrainCollider)
			{
				this.tracker = new ObiTerrainShapeTracker((TerrainCollider)this.unityCollider);
			}
			else if (this.unityCollider is MeshCollider)
			{
				this.tracker = new ObiMeshShapeTracker((MeshCollider)this.unityCollider);
			}
			else
			{
				Debug.LogWarning("Collider type not supported by Obi.");
			}
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x00086B11 File Offset: 0x00084F11
		protected override bool IsUnityColliderEnabled()
		{
			return ((Collider)this.unityCollider).enabled;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00086B24 File Offset: 0x00084F24
		protected override void UpdateColliderAdaptor()
		{
			this.adaptor.Set((Collider)this.unityCollider, this.phase, this.thickness);
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

		// Token: 0x06001782 RID: 6018 RVA: 0x00086BD8 File Offset: 0x00084FD8
		protected override void Awake()
		{
			this.unityCollider = base.GetComponent<Collider>();
			if (this.unityCollider == null)
			{
				return;
			}
			base.Awake();
		}
	}
}
