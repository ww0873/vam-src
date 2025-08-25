using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B1 RID: 945
	[Serializable]
	public class ObiVolumeConstraintBatch : ObiConstraintBatch
	{
		// Token: 0x06001809 RID: 6153 RVA: 0x00089054 File Offset: 0x00087454
		public ObiVolumeConstraintBatch(bool cooked, bool sharesParticles) : base(cooked, sharesParticles)
		{
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000890A0 File Offset: 0x000874A0
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Volume;
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x000890A4 File Offset: 0x000874A4
		public override void Clear()
		{
			this.activeConstraints.Clear();
			this.triangleIndices.Clear();
			this.firstTriangle.Clear();
			this.numTriangles.Clear();
			this.restVolumes.Clear();
			this.pressureStiffness.Clear();
			this.constraintCount = 0;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x000890FC File Offset: 0x000874FC
		public void AddConstraint(int[] triangles, float restVolume, float pressure, float stiffness)
		{
			this.activeConstraints.Add(this.constraintCount);
			this.firstTriangle.Add(this.triangleIndices.Count / 3);
			this.numTriangles.Add(triangles.Length / 3);
			this.triangleIndices.AddRange(triangles);
			this.restVolumes.Add(restVolume);
			this.pressureStiffness.Add(new Vector2(pressure, stiffness));
			this.constraintCount++;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0008917C File Offset: 0x0008757C
		public void RemoveConstraint(int index)
		{
			if (index < 0 || index >= base.ConstraintCount)
			{
				return;
			}
			this.activeConstraints.Remove(index);
			for (int i = 0; i < this.activeConstraints.Count; i++)
			{
				if (this.activeConstraints[i] > index)
				{
					List<int> activeConstraints;
					int index2;
					(activeConstraints = this.activeConstraints)[index2 = i] = activeConstraints[index2] - 1;
				}
			}
			this.triangleIndices.RemoveRange(this.firstTriangle[index], this.numTriangles[index]);
			this.firstTriangle.RemoveAt(index);
			this.numTriangles.RemoveAt(index);
			this.restVolumes.RemoveAt(index);
			this.pressureStiffness.RemoveAt(index);
			this.constraintCount--;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00089254 File Offset: 0x00087654
		public override List<int> GetConstraintsInvolvingParticle(int particleIndex)
		{
			List<int> list = new List<int>(4);
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				if (this.triangleIndices[i * 3] == particleIndex || this.triangleIndices[i * 3 + 1] == particleIndex || this.triangleIndices[i * 3 + 2] == particleIndex)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x000892C8 File Offset: 0x000876C8
		protected override void OnAddToSolver(ObiBatchedConstraints constraints)
		{
			this.solverIndices = new int[this.triangleIndices.Count];
			for (int i = 0; i < this.triangleIndices.Count; i++)
			{
				this.solverIndices[i] = constraints.Actor.particleIndices[this.triangleIndices[i]];
			}
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x00089327 File Offset: 0x00087727
		protected override void OnRemoveFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0008932C File Offset: 0x0008772C
		public override void PushDataToSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			ObiVolumeConstraints obiVolumeConstraints = (ObiVolumeConstraints)constraints;
			for (int i = 0; i < this.pressureStiffness.Count; i++)
			{
				this.pressureStiffness[i] = new Vector2(obiVolumeConstraints.overpressure, base.StiffnessToCompliance(obiVolumeConstraints.stiffness));
			}
			Oni.SetVolumeConstraints(this.batch, this.solverIndices, this.firstTriangle.ToArray(), this.numTriangles.ToArray(), this.restVolumes.ToArray(), this.pressureStiffness.ToArray(), base.ConstraintCount);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x000893F0 File Offset: 0x000877F0
		public override void PullDataFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x040013A4 RID: 5028
		[HideInInspector]
		public List<int> triangleIndices = new List<int>();

		// Token: 0x040013A5 RID: 5029
		[HideInInspector]
		public List<int> firstTriangle = new List<int>();

		// Token: 0x040013A6 RID: 5030
		[HideInInspector]
		public List<int> numTriangles = new List<int>();

		// Token: 0x040013A7 RID: 5031
		[HideInInspector]
		public List<float> restVolumes = new List<float>();

		// Token: 0x040013A8 RID: 5032
		[HideInInspector]
		public List<Vector2> pressureStiffness = new List<Vector2>();

		// Token: 0x040013A9 RID: 5033
		private int[] solverIndices;
	}
}
