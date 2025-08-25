using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B9 RID: 953
	[DisallowMultipleComponent]
	public class ObiVolumeConstraints : ObiBatchedConstraints
	{
		// Token: 0x06001857 RID: 6231 RVA: 0x0008A183 File Offset: 0x00088583
		public ObiVolumeConstraints()
		{
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x0008A1AC File Offset: 0x000885AC
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Volume;
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x0008A1AF File Offset: 0x000885AF
		public override List<ObiConstraintBatch> GetBatches()
		{
			List<ObiVolumeConstraintBatch> list = this.batches;
			if (ObiVolumeConstraints.<>f__am$cache0 == null)
			{
				ObiVolumeConstraints.<>f__am$cache0 = new Converter<ObiVolumeConstraintBatch, ObiConstraintBatch>(ObiVolumeConstraints.<GetBatches>m__0);
			}
			return list.ConvertAll<ObiConstraintBatch>(ObiVolumeConstraints.<>f__am$cache0);
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x0008A1D9 File Offset: 0x000885D9
		public override void Clear()
		{
			base.RemoveFromSolver(null);
			this.batches.Clear();
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x0008A1EE File Offset: 0x000885EE
		public void AddBatch(ObiVolumeConstraintBatch batch)
		{
			if (batch != null && batch.GetConstraintType() == this.GetConstraintType())
			{
				this.batches.Add(batch);
			}
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x0008A213 File Offset: 0x00088613
		public void RemoveBatch(ObiVolumeConstraintBatch batch)
		{
			this.batches.Remove(batch);
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0008A224 File Offset: 0x00088624
		public void OnDrawGizmosSelected()
		{
			if (!this.visualize)
			{
				return;
			}
			Gizmos.color = Color.red;
			foreach (ObiVolumeConstraintBatch obiVolumeConstraintBatch in this.batches)
			{
				foreach (int index in obiVolumeConstraintBatch.ActiveConstraints)
				{
					int num = obiVolumeConstraintBatch.firstTriangle[index];
					for (int i = 0; i < obiVolumeConstraintBatch.numTriangles[index]; i++)
					{
						int num2 = num + i;
						Vector3 particlePosition = this.actor.GetParticlePosition(obiVolumeConstraintBatch.triangleIndices[num2 * 3]);
						Vector3 particlePosition2 = this.actor.GetParticlePosition(obiVolumeConstraintBatch.triangleIndices[num2 * 3 + 1]);
						Vector3 particlePosition3 = this.actor.GetParticlePosition(obiVolumeConstraintBatch.triangleIndices[num2 * 3 + 2]);
						Gizmos.DrawLine(particlePosition, particlePosition2);
						Gizmos.DrawLine(particlePosition, particlePosition3);
						Gizmos.DrawLine(particlePosition2, particlePosition3);
					}
				}
			}
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0008A37C File Offset: 0x0008877C
		[CompilerGenerated]
		private static ObiConstraintBatch <GetBatches>m__0(ObiVolumeConstraintBatch x)
		{
			return x;
		}

		// Token: 0x040013C5 RID: 5061
		[Tooltip("Amount of pressure applied to the cloth.")]
		public float overpressure = 1f;

		// Token: 0x040013C6 RID: 5062
		[Range(0f, 1f)]
		[Tooltip("Stiffness of the volume constraints. Higher values will make the constraints to try harder to enforce the set volume.")]
		public float stiffness = 1f;

		// Token: 0x040013C7 RID: 5063
		[SerializeField]
		[HideInInspector]
		private List<ObiVolumeConstraintBatch> batches = new List<ObiVolumeConstraintBatch>();

		// Token: 0x040013C8 RID: 5064
		[CompilerGenerated]
		private static Converter<ObiVolumeConstraintBatch, ObiConstraintBatch> <>f__am$cache0;
	}
}
