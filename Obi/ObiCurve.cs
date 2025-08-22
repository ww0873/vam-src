using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F3 RID: 1011
	[ExecuteInEditMode]
	public abstract class ObiCurve : MonoBehaviour
	{
		// Token: 0x060019A6 RID: 6566 RVA: 0x0008D684 File Offset: 0x0008BA84
		protected ObiCurve()
		{
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060019A7 RID: 6567 RVA: 0x0008D6A8 File Offset: 0x0008BAA8
		// (set) Token: 0x060019A8 RID: 6568 RVA: 0x0008D6B0 File Offset: 0x0008BAB0
		public bool Closed
		{
			get
			{
				return this.closed;
			}
			set
			{
				this.SetClosed(value);
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060019A9 RID: 6569 RVA: 0x0008D6B9 File Offset: 0x0008BAB9
		public float Length
		{
			get
			{
				return this.totalSplineLenght;
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0008D6C4 File Offset: 0x0008BAC4
		public virtual void Awake()
		{
			if (this.controlPoints == null)
			{
				this.controlPoints = new List<Vector3>
				{
					Vector3.left,
					Vector3.zero,
					Vector3.right,
					Vector3.right * 2f
				};
			}
			if (this.arcLengthTable == null)
			{
				this.arcLengthTable = new List<float>();
				this.RecalculateSplineLenght(1E-05f, 7);
			}
		}

		// Token: 0x060019AB RID: 6571
		protected abstract void SetClosed(bool closed);

		// Token: 0x060019AC RID: 6572
		public abstract void DisplaceControlPoint(int index, Vector3 delta);

		// Token: 0x060019AD RID: 6573
		public abstract int GetNumSpans();

		// Token: 0x060019AE RID: 6574 RVA: 0x0008D744 File Offset: 0x0008BB44
		public float RecalculateSplineLenght(float acc, int maxevals)
		{
			this.totalSplineLenght = 0f;
			this.arcLengthTable.Clear();
			this.arcLengthTable.Add(0f);
			float num = 1f / (float)(this.arcLenghtSamples + 1);
			if (this.controlPoints.Count >= this.minPoints)
			{
				for (int i = 1; i < this.controlPoints.Count - this.unusedPoints; i += this.pointStride)
				{
					Vector3 vector = base.transform.TransformPoint(this.controlPoints[i - 1]);
					Vector3 vector2 = base.transform.TransformPoint(this.controlPoints[i]);
					Vector3 vector3 = base.transform.TransformPoint(this.controlPoints[i + 1]);
					Vector3 vector4 = base.transform.TransformPoint(this.controlPoints[i + 2]);
					for (int j = 0; j <= Mathf.Max(1, this.arcLenghtSamples); j++)
					{
						float num2 = (float)j * num;
						float num3 = (float)(j + 1) * num;
						float num4 = this.GaussLobattoIntegrationStep(vector, vector2, vector3, vector4, num2, num3, this.EvaluateFirstDerivative3D(vector, vector2, vector3, vector4, num2).magnitude, this.EvaluateFirstDerivative3D(vector, vector2, vector3, vector4, num3).magnitude, 0, maxevals, acc);
						this.totalSplineLenght += num4;
						this.arcLengthTable.Add(this.totalSplineLenght);
					}
				}
			}
			else
			{
				Debug.LogWarning("Catmull-Rom spline needs at least 4 control points to be defined.");
			}
			return this.totalSplineLenght;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0008D8D8 File Offset: 0x0008BCD8
		private float GaussLobattoIntegrationStep(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float a, float b, float fa, float fb, int nevals, int maxevals, float acc)
		{
			if (nevals >= maxevals)
			{
				return 0f;
			}
			float num = Mathf.Sqrt(0.6666667f);
			float num2 = 1f / Mathf.Sqrt(5f);
			float num3 = (b - a) / 2f;
			float num4 = (a + b) / 2f;
			float num5 = num4 - num * num3;
			float num6 = num4 - num2 * num3;
			float num7 = num4 + num2 * num3;
			float num8 = num4 + num * num3;
			nevals += 5;
			float magnitude = this.EvaluateFirstDerivative3D(p1, p2, p3, p4, num5).magnitude;
			float magnitude2 = this.EvaluateFirstDerivative3D(p1, p2, p3, p4, num6).magnitude;
			float magnitude3 = this.EvaluateFirstDerivative3D(p1, p2, p3, p4, num4).magnitude;
			float magnitude4 = this.EvaluateFirstDerivative3D(p1, p2, p3, p4, num7).magnitude;
			float magnitude5 = this.EvaluateFirstDerivative3D(p1, p2, p3, p4, num8).magnitude;
			float num9 = num3 / 6f * (fa + fb + 5f * (magnitude2 + magnitude4));
			float num10 = num3 / 1470f * (77f * (fa + fb) + 432f * (magnitude + magnitude5) + 625f * (magnitude2 + magnitude4) + 672f * magnitude3);
			if (num9 - num10 < acc || num5 <= a || b <= num8)
			{
				if (num4 <= a || b <= num4)
				{
					Debug.LogError("Spline integration reached an interval with no more machine numbers");
				}
				return num10;
			}
			return this.GaussLobattoIntegrationStep(p1, p2, p3, p4, a, num5, fa, magnitude, nevals, maxevals, acc) + this.GaussLobattoIntegrationStep(p1, p2, p3, p4, num5, num6, magnitude, magnitude2, nevals, maxevals, acc) + this.GaussLobattoIntegrationStep(p1, p2, p3, p4, num6, num4, magnitude2, magnitude3, nevals, maxevals, acc) + this.GaussLobattoIntegrationStep(p1, p2, p3, p4, num4, num7, magnitude3, magnitude4, nevals, maxevals, acc) + this.GaussLobattoIntegrationStep(p1, p2, p3, p4, num7, num8, magnitude4, magnitude5, nevals, maxevals, acc) + this.GaussLobattoIntegrationStep(p1, p2, p3, p4, num8, b, magnitude5, fb, nevals, maxevals, acc);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0008DAE8 File Offset: 0x0008BEE8
		public float GetMuAtLenght(float length)
		{
			if (length <= 0f)
			{
				return 0f;
			}
			if (length >= this.totalSplineLenght)
			{
				return 1f;
			}
			int i;
			for (i = 1; i < this.arcLengthTable.Count; i++)
			{
				if (length < this.arcLengthTable[i])
				{
					break;
				}
			}
			float num = (float)(i - 1) / (float)(this.arcLengthTable.Count - 1);
			float num2 = (float)i / (float)(this.arcLengthTable.Count - 1);
			float num3 = (length - this.arcLengthTable[i - 1]) / (this.arcLengthTable[i] - this.arcLengthTable[i - 1]);
			return num + (num2 - num) * num3;
		}

		// Token: 0x060019B1 RID: 6577
		public abstract int GetSpanControlPointForMu(float mu, out float spanMu);

		// Token: 0x060019B2 RID: 6578 RVA: 0x0008DBA8 File Offset: 0x0008BFA8
		public Vector3 GetPositionAt(float mu)
		{
			if (this.controlPoints.Count >= this.minPoints)
			{
				if (!float.IsNaN(mu))
				{
					float mu2;
					int spanControlPointForMu = this.GetSpanControlPointForMu(mu, out mu2);
					return this.Evaluate3D(this.controlPoints[spanControlPointForMu], this.controlPoints[spanControlPointForMu + 1], this.controlPoints[spanControlPointForMu + 2], this.controlPoints[spanControlPointForMu + 3], mu2);
				}
				return this.controlPoints[0];
			}
			else if (this.controlPoints.Count >= 2)
			{
				if (!float.IsNaN(mu))
				{
					return Vector3.Lerp(this.controlPoints[0], this.controlPoints[this.controlPoints.Count - 1], mu);
				}
				return this.controlPoints[0];
			}
			else
			{
				if (this.controlPoints.Count == 1)
				{
					return this.controlPoints[0];
				}
				throw new InvalidOperationException("Cannot get position in Catmull-Rom spline because it has zero control points.");
			}
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0008DCA8 File Offset: 0x0008C0A8
		public Vector3 GetFirstDerivativeAt(float mu)
		{
			if (this.controlPoints.Count >= this.minPoints)
			{
				if (!float.IsNaN(mu))
				{
					float mu2;
					int spanControlPointForMu = this.GetSpanControlPointForMu(mu, out mu2);
					return this.EvaluateFirstDerivative3D(this.controlPoints[spanControlPointForMu], this.controlPoints[spanControlPointForMu + 1], this.controlPoints[spanControlPointForMu + 2], this.controlPoints[spanControlPointForMu + 3], mu2);
				}
				return this.controlPoints[this.controlPoints.Count - 1] - this.controlPoints[0];
			}
			else
			{
				if (this.controlPoints.Count >= 2)
				{
					return this.controlPoints[this.controlPoints.Count - 1] - this.controlPoints[0];
				}
				throw new InvalidOperationException("Cannot get tangent in Catmull-Rom spline because it has zero or one control points.");
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0008DD90 File Offset: 0x0008C190
		public Vector3 GetSecondDerivativeAt(float mu)
		{
			if (this.controlPoints.Count < this.minPoints)
			{
				return Vector3.zero;
			}
			if (!float.IsNaN(mu))
			{
				float mu2;
				int spanControlPointForMu = this.GetSpanControlPointForMu(mu, out mu2);
				return this.EvaluateSecondDerivative3D(this.controlPoints[spanControlPointForMu], this.controlPoints[spanControlPointForMu + 1], this.controlPoints[spanControlPointForMu + 2], this.controlPoints[spanControlPointForMu + 3], mu2);
			}
			return Vector3.zero;
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0008DE14 File Offset: 0x0008C214
		private Vector3 Evaluate3D(Vector3 y0, Vector3 y1, Vector3 y2, Vector3 y3, float mu)
		{
			return new Vector3(this.Evaluate1D(y0.x, y1.x, y2.x, y3.x, mu), this.Evaluate1D(y0.y, y1.y, y2.y, y3.y, mu), this.Evaluate1D(y0.z, y1.z, y2.z, y3.z, mu));
		}

		// Token: 0x060019B6 RID: 6582
		protected abstract float Evaluate1D(float y0, float y1, float y2, float y3, float mu);

		// Token: 0x060019B7 RID: 6583 RVA: 0x0008DE94 File Offset: 0x0008C294
		private Vector3 EvaluateFirstDerivative3D(Vector3 y0, Vector3 y1, Vector3 y2, Vector3 y3, float mu)
		{
			return new Vector3(this.EvaluateFirstDerivative1D(y0.x, y1.x, y2.x, y3.x, mu), this.EvaluateFirstDerivative1D(y0.y, y1.y, y2.y, y3.y, mu), this.EvaluateFirstDerivative1D(y0.z, y1.z, y2.z, y3.z, mu));
		}

		// Token: 0x060019B8 RID: 6584
		protected abstract float EvaluateFirstDerivative1D(float y0, float y1, float y2, float y3, float mu);

		// Token: 0x060019B9 RID: 6585 RVA: 0x0008DF14 File Offset: 0x0008C314
		private Vector3 EvaluateSecondDerivative3D(Vector3 y0, Vector3 y1, Vector3 y2, Vector3 y3, float mu)
		{
			return new Vector3(this.EvaluateSecondDerivative1D(y0.x, y1.x, y2.x, y3.x, mu), this.EvaluateSecondDerivative1D(y0.y, y1.y, y2.y, y3.y, mu), this.EvaluateSecondDerivative1D(y0.z, y1.z, y2.z, y3.z, mu));
		}

		// Token: 0x060019BA RID: 6586
		protected abstract float EvaluateSecondDerivative1D(float y0, float y1, float y2, float y3, float mu);

		// Token: 0x040014E6 RID: 5350
		protected int arcLenghtSamples = 5;

		// Token: 0x040014E7 RID: 5351
		protected int minPoints = 4;

		// Token: 0x040014E8 RID: 5352
		protected int unusedPoints = 2;

		// Token: 0x040014E9 RID: 5353
		protected int pointStride = 1;

		// Token: 0x040014EA RID: 5354
		[HideInInspector]
		public List<Vector3> controlPoints;

		// Token: 0x040014EB RID: 5355
		[HideInInspector]
		[SerializeField]
		protected List<float> arcLengthTable;

		// Token: 0x040014EC RID: 5356
		[HideInInspector]
		[SerializeField]
		protected float totalSplineLenght;

		// Token: 0x040014ED RID: 5357
		[HideInInspector]
		[SerializeField]
		protected bool closed;
	}
}
