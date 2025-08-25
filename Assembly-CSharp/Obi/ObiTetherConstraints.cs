using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B8 RID: 952
	[DisallowMultipleComponent]
	public class ObiTetherConstraints : ObiBatchedConstraints
	{
		// Token: 0x0600184F RID: 6223 RVA: 0x00089FF7 File Offset: 0x000883F7
		public ObiTetherConstraints()
		{
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x0008A020 File Offset: 0x00088420
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Tether;
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x0008A023 File Offset: 0x00088423
		public override List<ObiConstraintBatch> GetBatches()
		{
			List<ObiTetherConstraintBatch> list = this.batches;
			if (ObiTetherConstraints.<>f__am$cache0 == null)
			{
				ObiTetherConstraints.<>f__am$cache0 = new Converter<ObiTetherConstraintBatch, ObiConstraintBatch>(ObiTetherConstraints.<GetBatches>m__0);
			}
			return list.ConvertAll<ObiConstraintBatch>(ObiTetherConstraints.<>f__am$cache0);
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x0008A04D File Offset: 0x0008844D
		public override void Clear()
		{
			base.RemoveFromSolver(null);
			this.batches.Clear();
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x0008A062 File Offset: 0x00088462
		public void AddBatch(ObiTetherConstraintBatch batch)
		{
			if (batch != null && batch.GetConstraintType() == this.GetConstraintType())
			{
				this.batches.Add(batch);
			}
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x0008A087 File Offset: 0x00088487
		public void RemoveBatch(ObiTetherConstraintBatch batch)
		{
			this.batches.Remove(batch);
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x0008A098 File Offset: 0x00088498
		public void OnDrawGizmosSelected()
		{
			if (!this.visualize)
			{
				return;
			}
			Gizmos.color = Color.yellow;
			foreach (ObiTetherConstraintBatch obiTetherConstraintBatch in this.batches)
			{
				foreach (int num in obiTetherConstraintBatch.ActiveConstraints)
				{
					Gizmos.DrawLine(this.actor.GetParticlePosition(obiTetherConstraintBatch.tetherIndices[num * 2]), this.actor.GetParticlePosition(obiTetherConstraintBatch.tetherIndices[num * 2 + 1]));
				}
			}
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0008A180 File Offset: 0x00088580
		[CompilerGenerated]
		private static ObiConstraintBatch <GetBatches>m__0(ObiTetherConstraintBatch x)
		{
			return x;
		}

		// Token: 0x040013C1 RID: 5057
		[Range(0.1f, 2f)]
		[Tooltip("Scale of tether constraints. Values > 1 will expand initial tether length, values < 1 will make it shrink.")]
		public float tetherScale = 1f;

		// Token: 0x040013C2 RID: 5058
		[Range(0f, 1f)]
		[Tooltip("Tether resistance to stretching. Lower values will enforce tethers with more strenght.")]
		public float stiffness = 1f;

		// Token: 0x040013C3 RID: 5059
		[SerializeField]
		[HideInInspector]
		private List<ObiTetherConstraintBatch> batches = new List<ObiTetherConstraintBatch>();

		// Token: 0x040013C4 RID: 5060
		[CompilerGenerated]
		private static Converter<ObiTetherConstraintBatch, ObiConstraintBatch> <>f__am$cache0;
	}
}
