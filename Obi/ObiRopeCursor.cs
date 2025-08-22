using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F8 RID: 1016
	public class ObiRopeCursor : MonoBehaviour
	{
		// Token: 0x060019DD RID: 6621 RVA: 0x0008FB0C File Offset: 0x0008DF0C
		public ObiRopeCursor()
		{
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x0008FB1C File Offset: 0x0008DF1C
		private int FindHotConstraint(ObiDistanceConstraintBatch distanceBatch, int constraint, int maxAmount)
		{
			if (this.direction)
			{
				int num = distanceBatch.springIndices[constraint * 2 + 1];
				for (int i = 1; i <= maxAmount; i++)
				{
					if (constraint + i == distanceBatch.ConstraintCount || distanceBatch.springIndices[(constraint + i) * 2] != num)
					{
						return constraint + i - 1;
					}
					num = distanceBatch.springIndices[(constraint + i) * 2 + 1];
				}
				return constraint + maxAmount;
			}
			int num2 = distanceBatch.springIndices[constraint * 2];
			for (int j = 1; j <= maxAmount; j++)
			{
				if (constraint - j < 0 || distanceBatch.springIndices[(constraint - j) * 2 + 1] != num2)
				{
					return constraint - j + 1;
				}
				num2 = distanceBatch.springIndices[(constraint - j) * 2];
			}
			return constraint - maxAmount;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x0008FBF4 File Offset: 0x0008DFF4
		private int AddParticles(int amount)
		{
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.rope.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			ObiBendConstraintBatch obiBendConstraintBatch = this.rope.BendingConstraints.GetBatches()[0] as ObiBendConstraintBatch;
			amount = Mathf.Min(amount, this.rope.PooledParticles);
			if (amount == 0)
			{
				return 0;
			}
			int constraintIndexAtNormalizedCoordinate = this.rope.GetConstraintIndexAtNormalizedCoordinate(this.normalizedCoord);
			this.rope.DistanceConstraints.RemoveFromSolver(null);
			this.rope.BendingConstraints.RemoveFromSolver(null);
			int[] array = new int[amount + 2];
			int num = 0;
			int num2 = 0;
			while (num < amount && num2 < this.rope.TotalParticles)
			{
				if (!this.rope.active[num2])
				{
					array[num + 1] = num2;
					this.rope.active[num2] = true;
					this.rope.invMasses[num2] = 10f;
					num++;
				}
				num2++;
			}
			Vector4[] velocities = new Vector4[]
			{
				Vector4.zero
			};
			Vector4[] array2 = new Vector4[1];
			Vector4[] array3 = new Vector4[1];
			if (this.direction)
			{
				array[0] = obiDistanceConstraintBatch.springIndices[constraintIndexAtNormalizedCoordinate * 2];
				array[array.Length - 1] = obiDistanceConstraintBatch.springIndices[constraintIndexAtNormalizedCoordinate * 2 + 1];
				this.normalizedCoord = (float)constraintIndexAtNormalizedCoordinate / (float)(obiDistanceConstraintBatch.ConstraintCount + amount);
				Oni.GetParticlePositions(this.rope.Solver.OniSolver, array2, 1, this.rope.particleIndices[array[0]]);
				Oni.GetParticlePositions(this.rope.Solver.OniSolver, array3, 1, this.rope.particleIndices[array[array.Length - 1]]);
				obiDistanceConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate, array[array.Length - 2], ObiDistanceConstraintBatch.DistanceIndexType.First, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate, array[array.Length - 2], ObiBendConstraintBatch.BendIndexType.First, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate - 1, array[1], ObiBendConstraintBatch.BendIndexType.Second, this.rope.Closed);
				for (int i = 1; i < array.Length - 1; i++)
				{
					Vector4[] positions = new Vector4[]
					{
						array2[0] + (array3[0] - array2[0]) * (float)i / (float)(array.Length - 1) * 0.5f
					};
					Oni.SetParticlePositions(this.rope.Solver.OniSolver, positions, 1, this.rope.particleIndices[array[i]]);
					Oni.SetParticleVelocities(this.rope.Solver.OniSolver, velocities, 1, this.rope.particleIndices[array[i]]);
					int constraintIndex = constraintIndexAtNormalizedCoordinate + i - 1;
					obiDistanceConstraintBatch.InsertConstraint(constraintIndex, array[i - 1], array[i], this.rope.InterparticleDistance, 0f, 0f);
					obiBendConstraintBatch.InsertConstraint(constraintIndex, array[i - 1], array[i + 1], array[i], 0f, 0f, 0f);
				}
			}
			else
			{
				array[0] = obiDistanceConstraintBatch.springIndices[constraintIndexAtNormalizedCoordinate * 2 + 1];
				array[array.Length - 1] = obiDistanceConstraintBatch.springIndices[constraintIndexAtNormalizedCoordinate * 2];
				this.normalizedCoord = (float)(constraintIndexAtNormalizedCoordinate + amount) / (float)(obiDistanceConstraintBatch.ConstraintCount + amount);
				Oni.GetParticlePositions(this.rope.Solver.OniSolver, array2, 1, this.rope.particleIndices[array[0]]);
				Oni.GetParticlePositions(this.rope.Solver.OniSolver, array3, 1, this.rope.particleIndices[array[array.Length - 1]]);
				obiDistanceConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate, array[array.Length - 2], ObiDistanceConstraintBatch.DistanceIndexType.Second, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate, array[1], ObiBendConstraintBatch.BendIndexType.First, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate - 1, array[array.Length - 2], ObiBendConstraintBatch.BendIndexType.Second, this.rope.Closed);
				for (int j = 1; j < array.Length - 1; j++)
				{
					Vector4[] positions2 = new Vector4[]
					{
						array2[0] + (array3[0] - array2[0]) * (float)j / (float)(array.Length - 1) * 0.5f
					};
					Oni.SetParticlePositions(this.rope.Solver.OniSolver, positions2, 1, this.rope.particleIndices[array[j]]);
					Oni.SetParticleVelocities(this.rope.Solver.OniSolver, velocities, 1, this.rope.particleIndices[array[j]]);
					obiDistanceConstraintBatch.InsertConstraint(constraintIndexAtNormalizedCoordinate + 1, array[j], array[j - 1], this.rope.InterparticleDistance, 0f, 0f);
					obiBendConstraintBatch.InsertConstraint(constraintIndexAtNormalizedCoordinate, array[j + 1], array[j - 1], array[j], 0f, 0f, 0f);
				}
			}
			this.rope.DistanceConstraints.AddToSolver(null);
			this.rope.BendingConstraints.AddToSolver(null);
			this.rope.PushDataToSolver(ParticleData.ACTIVE_STATUS);
			this.rope.UsedParticles += amount;
			this.rope.RegenerateRestPositions();
			return amount;
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x00090180 File Offset: 0x0008E580
		private int RemoveParticles(int amount)
		{
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.rope.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			ObiBendConstraintBatch obiBendConstraintBatch = this.rope.BendingConstraints.GetBatches()[0] as ObiBendConstraintBatch;
			amount = Mathf.Min(amount, this.rope.UsedParticles - 2);
			int constraintIndexAtNormalizedCoordinate = this.rope.GetConstraintIndexAtNormalizedCoordinate(this.normalizedCoord);
			int num = this.FindHotConstraint(obiDistanceConstraintBatch, constraintIndexAtNormalizedCoordinate, amount);
			amount = Mathf.Min(amount, Mathf.Abs(num - constraintIndexAtNormalizedCoordinate));
			if (amount == 0)
			{
				return 0;
			}
			this.rope.DistanceConstraints.RemoveFromSolver(null);
			this.rope.BendingConstraints.RemoveFromSolver(null);
			if (this.direction)
			{
				this.normalizedCoord = (float)constraintIndexAtNormalizedCoordinate / (float)(obiDistanceConstraintBatch.ConstraintCount - amount);
				obiDistanceConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate, obiDistanceConstraintBatch.springIndices[num * 2 + 1], ObiDistanceConstraintBatch.DistanceIndexType.Second, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate - 1, obiDistanceConstraintBatch.springIndices[num * 2 + 1], ObiBendConstraintBatch.BendIndexType.Second, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(num, obiDistanceConstraintBatch.springIndices[constraintIndexAtNormalizedCoordinate * 2], ObiBendConstraintBatch.BendIndexType.First, this.rope.Closed);
				for (int i = constraintIndexAtNormalizedCoordinate + amount; i > constraintIndexAtNormalizedCoordinate; i--)
				{
					this.rope.active[obiDistanceConstraintBatch.springIndices[i * 2]] = false;
					obiDistanceConstraintBatch.RemoveConstraint(i);
					obiBendConstraintBatch.RemoveConstraint(i - 1);
				}
			}
			else
			{
				this.normalizedCoord = (float)(constraintIndexAtNormalizedCoordinate - amount) / (float)(obiDistanceConstraintBatch.ConstraintCount - amount);
				obiDistanceConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate, obiDistanceConstraintBatch.springIndices[num * 2], ObiDistanceConstraintBatch.DistanceIndexType.First, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(constraintIndexAtNormalizedCoordinate, obiDistanceConstraintBatch.springIndices[num * 2], ObiBendConstraintBatch.BendIndexType.First, this.rope.Closed);
				obiBendConstraintBatch.SetParticleIndex(num - 1, obiDistanceConstraintBatch.springIndices[constraintIndexAtNormalizedCoordinate * 2 + 1], ObiBendConstraintBatch.BendIndexType.Second, this.rope.Closed);
				for (int j = constraintIndexAtNormalizedCoordinate - 1; j >= constraintIndexAtNormalizedCoordinate - amount; j--)
				{
					this.rope.active[obiDistanceConstraintBatch.springIndices[j * 2 + 1]] = false;
					obiDistanceConstraintBatch.RemoveConstraint(j);
					obiBendConstraintBatch.RemoveConstraint(j);
				}
			}
			this.rope.DistanceConstraints.AddToSolver(null);
			this.rope.BendingConstraints.AddToSolver(null);
			this.rope.PushDataToSolver(ParticleData.ACTIVE_STATUS);
			this.rope.UsedParticles -= amount;
			this.rope.RegenerateRestPositions();
			return amount;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x00090418 File Offset: 0x0008E818
		public void ChangeLength(float newLength)
		{
			if (this.rope == null)
			{
				return;
			}
			newLength = Mathf.Clamp(newLength, 0f, (float)(this.rope.TotalParticles - 1) * this.rope.InterparticleDistance);
			int constraintIndexAtNormalizedCoordinate = this.rope.GetConstraintIndexAtNormalizedCoordinate(this.normalizedCoord);
			ObiDistanceConstraintBatch obiDistanceConstraintBatch = this.rope.DistanceConstraints.GetBatches()[0] as ObiDistanceConstraintBatch;
			float num = newLength - this.rope.RestLength;
			float num2 = Mathf.Clamp(num, -obiDistanceConstraintBatch.restLengths[constraintIndexAtNormalizedCoordinate], this.rope.InterparticleDistance - obiDistanceConstraintBatch.restLengths[constraintIndexAtNormalizedCoordinate]);
			List<float> restLengths;
			int index;
			(restLengths = obiDistanceConstraintBatch.restLengths)[index = constraintIndexAtNormalizedCoordinate] = restLengths[index] + num2;
			num -= num2;
			int num3 = (num <= 0f) ? Mathf.FloorToInt(num / this.rope.InterparticleDistance) : Mathf.CeilToInt(num / this.rope.InterparticleDistance);
			float value = ObiUtils.Mod(num, this.rope.InterparticleDistance);
			if (num3 > 0)
			{
				if (this.AddParticles(num3) == 0)
				{
					value = this.rope.InterparticleDistance;
				}
				constraintIndexAtNormalizedCoordinate = this.rope.GetConstraintIndexAtNormalizedCoordinate(this.normalizedCoord);
				obiDistanceConstraintBatch.restLengths[constraintIndexAtNormalizedCoordinate] = value;
			}
			else if (num3 < 0)
			{
				if (this.RemoveParticles(-num3) == 0)
				{
					value = 0f;
				}
				constraintIndexAtNormalizedCoordinate = this.rope.GetConstraintIndexAtNormalizedCoordinate(this.normalizedCoord);
				obiDistanceConstraintBatch.restLengths[constraintIndexAtNormalizedCoordinate] = value;
			}
			obiDistanceConstraintBatch.PushDataToSolver(this.rope.DistanceConstraints);
			this.rope.RecalculateLenght();
		}

		// Token: 0x0400150B RID: 5387
		public ObiRope rope;

		// Token: 0x0400150C RID: 5388
		[Range(0f, 1f)]
		public float normalizedCoord;

		// Token: 0x0400150D RID: 5389
		public bool direction = true;
	}
}
