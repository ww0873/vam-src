using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B6 RID: 950
	[DisallowMultipleComponent]
	public class ObiPinConstraints : ObiBatchedConstraints
	{
		// Token: 0x0600183F RID: 6207 RVA: 0x00089C7B File Offset: 0x0008807B
		public ObiPinConstraints()
		{
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00089C99 File Offset: 0x00088099
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Pin;
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00089C9C File Offset: 0x0008809C
		public override List<ObiConstraintBatch> GetBatches()
		{
			List<ObiPinConstraintBatch> list = this.batches;
			if (ObiPinConstraints.<>f__am$cache0 == null)
			{
				ObiPinConstraints.<>f__am$cache0 = new Converter<ObiPinConstraintBatch, ObiConstraintBatch>(ObiPinConstraints.<GetBatches>m__0);
			}
			return list.ConvertAll<ObiConstraintBatch>(ObiPinConstraints.<>f__am$cache0);
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x00089CC6 File Offset: 0x000880C6
		public override void Clear()
		{
			base.RemoveFromSolver(null);
			this.batches.Clear();
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x00089CDB File Offset: 0x000880DB
		public void AddBatch(ObiPinConstraintBatch batch)
		{
			if (batch != null && batch.GetConstraintType() == this.GetConstraintType())
			{
				this.batches.Add(batch);
			}
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00089D00 File Offset: 0x00088100
		public void RemoveBatch(ObiPinConstraintBatch batch)
		{
			this.batches.Remove(batch);
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x00089D10 File Offset: 0x00088110
		public void OnDrawGizmosSelected()
		{
			if (!this.visualize)
			{
				return;
			}
			Gizmos.color = Color.cyan;
			foreach (ObiPinConstraintBatch obiPinConstraintBatch in this.batches)
			{
				foreach (int index in obiPinConstraintBatch.ActiveConstraints)
				{
					Vector3 to = obiPinConstraintBatch.pinBodies[index].transform.TransformPoint(obiPinConstraintBatch.pinOffsets[index]);
					Gizmos.DrawLine(this.actor.GetParticlePosition(obiPinConstraintBatch.pinIndices[index]), to);
				}
			}
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x00089E04 File Offset: 0x00088204
		[CompilerGenerated]
		private static ObiConstraintBatch <GetBatches>m__0(ObiPinConstraintBatch x)
		{
			return x;
		}

		// Token: 0x040013BB RID: 5051
		[Range(0f, 1f)]
		[Tooltip("Pin resistance to stretching. Lower values will yield more elastic pin constraints.")]
		public float stiffness = 1f;

		// Token: 0x040013BC RID: 5052
		[SerializeField]
		[HideInInspector]
		private List<ObiPinConstraintBatch> batches = new List<ObiPinConstraintBatch>();

		// Token: 0x040013BD RID: 5053
		[CompilerGenerated]
		private static Converter<ObiPinConstraintBatch, ObiConstraintBatch> <>f__am$cache0;
	}
}
