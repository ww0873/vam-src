using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B5 RID: 949
	[DisallowMultipleComponent]
	public class ObiDistanceConstraints : ObiBatchedConstraints
	{
		// Token: 0x06001837 RID: 6199 RVA: 0x00089AEF File Offset: 0x00087EEF
		public ObiDistanceConstraints()
		{
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00089B18 File Offset: 0x00087F18
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Distance;
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00089B1B File Offset: 0x00087F1B
		public override List<ObiConstraintBatch> GetBatches()
		{
			List<ObiDistanceConstraintBatch> list = this.batches;
			if (ObiDistanceConstraints.<>f__am$cache0 == null)
			{
				ObiDistanceConstraints.<>f__am$cache0 = new Converter<ObiDistanceConstraintBatch, ObiConstraintBatch>(ObiDistanceConstraints.<GetBatches>m__0);
			}
			return list.ConvertAll<ObiConstraintBatch>(ObiDistanceConstraints.<>f__am$cache0);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00089B45 File Offset: 0x00087F45
		public override void Clear()
		{
			base.RemoveFromSolver(null);
			this.batches.Clear();
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00089B5A File Offset: 0x00087F5A
		public void AddBatch(ObiDistanceConstraintBatch batch)
		{
			if (batch != null && batch.GetConstraintType() == this.GetConstraintType())
			{
				this.batches.Add(batch);
			}
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00089B7F File Offset: 0x00087F7F
		public void RemoveBatch(ObiDistanceConstraintBatch batch)
		{
			this.batches.Remove(batch);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00089B90 File Offset: 0x00087F90
		public void OnDrawGizmosSelected()
		{
			if (!this.visualize)
			{
				return;
			}
			Gizmos.color = Color.green;
			foreach (ObiDistanceConstraintBatch obiDistanceConstraintBatch in this.batches)
			{
				foreach (int num in obiDistanceConstraintBatch.ActiveConstraints)
				{
					Gizmos.DrawLine(this.actor.GetParticlePosition(obiDistanceConstraintBatch.springIndices[num * 2]), this.actor.GetParticlePosition(obiDistanceConstraintBatch.springIndices[num * 2 + 1]));
				}
			}
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00089C78 File Offset: 0x00088078
		[CompilerGenerated]
		private static ObiConstraintBatch <GetBatches>m__0(ObiDistanceConstraintBatch x)
		{
			return x;
		}

		// Token: 0x040013B6 RID: 5046
		[Tooltip("Scale of stretching constraints. Values > 1 will expand initial cloth size, values < 1 will make it shrink.")]
		public float stretchingScale = 1f;

		// Token: 0x040013B7 RID: 5047
		[Range(0f, 1f)]
		[Tooltip("Cloth resistance to stretching. Lower values will yield more elastic cloth.")]
		public float stiffness = 1f;

		// Token: 0x040013B8 RID: 5048
		[Range(0f, 1f)]
		[Tooltip("Amount of compression slack. 0 means total resistance to compression, 1 no resistance at all. 0.5 means constraints will allow a compression of up to 50% of their rest length.")]
		public float slack;

		// Token: 0x040013B9 RID: 5049
		[SerializeField]
		[HideInInspector]
		private List<ObiDistanceConstraintBatch> batches = new List<ObiDistanceConstraintBatch>();

		// Token: 0x040013BA RID: 5050
		[CompilerGenerated]
		private static Converter<ObiDistanceConstraintBatch, ObiConstraintBatch> <>f__am$cache0;
	}
}
