using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B7 RID: 951
	[DisallowMultipleComponent]
	public class ObiSkinConstraints : ObiBatchedConstraints
	{
		// Token: 0x06001847 RID: 6215 RVA: 0x00089E07 File Offset: 0x00088207
		public ObiSkinConstraints()
		{
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00089E25 File Offset: 0x00088225
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Skin;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00089E28 File Offset: 0x00088228
		public override List<ObiConstraintBatch> GetBatches()
		{
			List<ObiSkinConstraintBatch> list = this.batches;
			if (ObiSkinConstraints.<>f__am$cache0 == null)
			{
				ObiSkinConstraints.<>f__am$cache0 = new Converter<ObiSkinConstraintBatch, ObiConstraintBatch>(ObiSkinConstraints.<GetBatches>m__0);
			}
			return list.ConvertAll<ObiConstraintBatch>(ObiSkinConstraints.<>f__am$cache0);
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x00089E52 File Offset: 0x00088252
		public override void Clear()
		{
			base.RemoveFromSolver(null);
			this.batches.Clear();
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00089E67 File Offset: 0x00088267
		public void AddBatch(ObiSkinConstraintBatch batch)
		{
			if (batch != null && batch.GetConstraintType() == this.GetConstraintType())
			{
				this.batches.Add(batch);
			}
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x00089E8C File Offset: 0x0008828C
		public void RemoveBatch(ObiSkinConstraintBatch batch)
		{
			this.batches.Remove(batch);
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x00089E9C File Offset: 0x0008829C
		public void OnDrawGizmosSelected()
		{
			if (!this.visualize)
			{
				return;
			}
			Gizmos.color = Color.magenta;
			Matrix4x4 localToWorldMatrix = this.actor.Solver.transform.localToWorldMatrix;
			foreach (ObiSkinConstraintBatch obiSkinConstraintBatch in this.batches)
			{
				obiSkinConstraintBatch.PullDataFromSolver(this);
				foreach (int index in obiSkinConstraintBatch.ActiveConstraints)
				{
					Vector3 vector = obiSkinConstraintBatch.GetSkinPosition(index);
					if (!base.InSolver)
					{
						vector = base.transform.TransformPoint(vector);
					}
					else if (this.actor.Solver.simulateInLocalSpace)
					{
						vector = localToWorldMatrix.MultiplyPoint3x4(vector);
					}
					if (this.actor.invMasses[obiSkinConstraintBatch.skinIndices[index]] > 0f)
					{
						Gizmos.DrawLine(vector, this.actor.GetParticlePosition(obiSkinConstraintBatch.skinIndices[index]));
					}
				}
			}
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x00089FF4 File Offset: 0x000883F4
		[CompilerGenerated]
		private static ObiConstraintBatch <GetBatches>m__0(ObiSkinConstraintBatch x)
		{
			return x;
		}

		// Token: 0x040013BE RID: 5054
		[Range(0f, 1f)]
		[Tooltip("Skin constraints stiffness.")]
		public float stiffness = 1f;

		// Token: 0x040013BF RID: 5055
		[SerializeField]
		[HideInInspector]
		private List<ObiSkinConstraintBatch> batches = new List<ObiSkinConstraintBatch>();

		// Token: 0x040013C0 RID: 5056
		[CompilerGenerated]
		private static Converter<ObiSkinConstraintBatch, ObiConstraintBatch> <>f__am$cache0;
	}
}
