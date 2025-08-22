using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B4 RID: 948
	[DisallowMultipleComponent]
	public class ObiBendingConstraints : ObiBatchedConstraints
	{
		// Token: 0x0600182F RID: 6191 RVA: 0x0008995B File Offset: 0x00087D5B
		public ObiBendingConstraints()
		{
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00089979 File Offset: 0x00087D79
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Bending;
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0008997C File Offset: 0x00087D7C
		public override List<ObiConstraintBatch> GetBatches()
		{
			List<ObiBendConstraintBatch> list = this.batches;
			if (ObiBendingConstraints.<>f__am$cache0 == null)
			{
				ObiBendingConstraints.<>f__am$cache0 = new Converter<ObiBendConstraintBatch, ObiConstraintBatch>(ObiBendingConstraints.<GetBatches>m__0);
			}
			return list.ConvertAll<ObiConstraintBatch>(ObiBendingConstraints.<>f__am$cache0);
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x000899A6 File Offset: 0x00087DA6
		public override void Clear()
		{
			base.RemoveFromSolver(null);
			this.batches.Clear();
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000899BB File Offset: 0x00087DBB
		public void AddBatch(ObiBendConstraintBatch batch)
		{
			if (batch != null && batch.GetConstraintType() == this.GetConstraintType())
			{
				this.batches.Add(batch);
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x000899E0 File Offset: 0x00087DE0
		public void RemoveBatch(ObiBendConstraintBatch batch)
		{
			this.batches.Remove(batch);
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000899F0 File Offset: 0x00087DF0
		public void OnDrawGizmosSelected()
		{
			if (!this.visualize)
			{
				return;
			}
			Gizmos.color = new Color(0.5f, 0f, 1f, 1f);
			foreach (ObiBendConstraintBatch obiBendConstraintBatch in this.batches)
			{
				foreach (int num in obiBendConstraintBatch.ActiveConstraints)
				{
					Gizmos.DrawLine(this.actor.GetParticlePosition(obiBendConstraintBatch.bendingIndices[num * 3]), this.actor.GetParticlePosition(obiBendConstraintBatch.bendingIndices[num * 3 + 1]));
				}
			}
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x00089AEC File Offset: 0x00087EEC
		[CompilerGenerated]
		private static ObiConstraintBatch <GetBatches>m__0(ObiBendConstraintBatch x)
		{
			return x;
		}

		// Token: 0x040013B2 RID: 5042
		[Tooltip("Bending offset. Leave at zero to keep the original bending amount.")]
		public float maxBending;

		// Token: 0x040013B3 RID: 5043
		[Range(0f, 1f)]
		[Tooltip("Cloth resistance to bending. Higher values will yield more stiff cloth.")]
		public float stiffness = 1f;

		// Token: 0x040013B4 RID: 5044
		[SerializeField]
		[HideInInspector]
		private List<ObiBendConstraintBatch> batches = new List<ObiBendConstraintBatch>();

		// Token: 0x040013B5 RID: 5045
		[CompilerGenerated]
		private static Converter<ObiBendConstraintBatch, ObiConstraintBatch> <>f__am$cache0;
	}
}
