using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F0 RID: 1008
	[ExecuteInEditMode]
	public class ObiBezierCurve : ObiCurve
	{
		// Token: 0x0600198C RID: 6540 RVA: 0x0008DF92 File Offset: 0x0008C392
		public ObiBezierCurve()
		{
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0008DF9C File Offset: 0x0008C39C
		public override void Awake()
		{
			this.minPoints = 4;
			this.unusedPoints = 2;
			this.pointStride = 3;
			base.Awake();
			if (this.controlPointModes == null)
			{
				this.controlPointModes = new List<ObiBezierCurve.BezierCPMode>
				{
					ObiBezierCurve.BezierCPMode.Free,
					ObiBezierCurve.BezierCPMode.Free
				};
			}
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0008DFEC File Offset: 0x0008C3EC
		protected override void SetClosed(bool closed)
		{
			if (this.closed == closed)
			{
				return;
			}
			if (!this.closed && closed)
			{
				this.lastOpenCP = this.controlPoints[0];
				this.lastOpenCPMode = this.controlPointModes[0];
				this.controlPoints[0] = this.controlPoints[this.controlPoints.Count - 1];
				this.controlPointModes[0] = this.controlPointModes[this.controlPointModes.Count - 1];
			}
			else
			{
				this.controlPoints[0] = this.lastOpenCP;
				this.controlPointModes[0] = this.lastOpenCPMode;
			}
			this.closed = closed;
			this.EnforceMode(0);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0008E0BA File Offset: 0x0008C4BA
		public override int GetNumSpans()
		{
			return (this.controlPoints.Count + (this.controlPoints.Count - 4) / 3) / 4;
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0008E0D9 File Offset: 0x0008C4D9
		public bool IsHandle(int index)
		{
			return index % 3 != 0;
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0008E0E4 File Offset: 0x0008C4E4
		public int GetHandleControlPointIndex(int index)
		{
			if (index < 0 || index >= this.controlPoints.Count)
			{
				return -1;
			}
			if (index % 3 == 1)
			{
				return index - 1;
			}
			if (index % 3 == 2)
			{
				return index + 1;
			}
			return index;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0008E11C File Offset: 0x0008C51C
		public List<int> GetHandleIndicesForControlPoint(int index)
		{
			List<int> list = new List<int>();
			if (index < 0 || index >= this.controlPoints.Count)
			{
				return list;
			}
			if (!this.IsHandle(index))
			{
				if (this.closed)
				{
					if (index == 0)
					{
						list.Add(1);
						list.Add(this.controlPoints.Count - 2);
					}
					else if (index == this.controlPoints.Count - 1)
					{
						list.Add(1);
						list.Add(index - 1);
					}
					else
					{
						list.Add(index + 1);
						list.Add(index - 1);
					}
				}
				else
				{
					if (index > 0)
					{
						list.Add(index - 1);
					}
					if (index + 1 < this.controlPoints.Count)
					{
						list.Add(index + 1);
					}
				}
			}
			return list;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0008E1F4 File Offset: 0x0008C5F4
		public override void DisplaceControlPoint(int index, Vector3 delta)
		{
			if (index < 0 || index >= this.controlPoints.Count)
			{
				return;
			}
			List<Vector3> controlPoints;
			if (!this.IsHandle(index))
			{
				if (this.closed)
				{
					if (index == 0)
					{
						(controlPoints = this.controlPoints)[1] = controlPoints[1] + delta;
						int index2;
						(controlPoints = this.controlPoints)[index2 = this.controlPoints.Count - 2] = controlPoints[index2] + delta;
						int index3;
						(controlPoints = this.controlPoints)[index3 = this.controlPoints.Count - 1] = controlPoints[index3] + delta;
					}
					else if (index == this.controlPoints.Count - 1)
					{
						(controlPoints = this.controlPoints)[0] = controlPoints[0] + delta;
						(controlPoints = this.controlPoints)[1] = controlPoints[1] + delta;
						int index4;
						(controlPoints = this.controlPoints)[index4 = index - 1] = controlPoints[index4] + delta;
					}
					else
					{
						int index5;
						(controlPoints = this.controlPoints)[index5 = index - 1] = controlPoints[index5] + delta;
						int index6;
						(controlPoints = this.controlPoints)[index6 = index + 1] = controlPoints[index6] + delta;
					}
				}
				else
				{
					if (index > 0)
					{
						int index7;
						(controlPoints = this.controlPoints)[index7 = index - 1] = controlPoints[index7] + delta;
					}
					if (index + 1 < this.controlPoints.Count)
					{
						int index8;
						(controlPoints = this.controlPoints)[index8 = index + 1] = controlPoints[index8] + delta;
					}
				}
			}
			(controlPoints = this.controlPoints)[index] = controlPoints[index] + delta;
			this.EnforceMode(index);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0008E3E0 File Offset: 0x0008C7E0
		public override int GetSpanControlPointForMu(float mu, out float spanMu)
		{
			int numSpans = this.GetNumSpans();
			spanMu = mu * (float)numSpans;
			int num = (mu < 1f) ? ((int)spanMu) : (numSpans - 1);
			spanMu -= (float)num;
			return num * 3;
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0008E41C File Offset: 0x0008C81C
		public ObiBezierCurve.BezierCPMode GetControlPointMode(int index)
		{
			int index2 = (index + 1) / 3;
			return this.controlPointModes[index2];
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0008E43C File Offset: 0x0008C83C
		public void SetControlPointMode(int index, ObiBezierCurve.BezierCPMode mode)
		{
			int num = (index + 1) / 3;
			this.controlPointModes[num] = mode;
			if (this.closed)
			{
				if (num == 0)
				{
					this.controlPointModes[this.controlPointModes.Count - 1] = mode;
				}
				else if (num == this.controlPointModes.Count - 1)
				{
					this.controlPointModes[0] = mode;
				}
			}
			this.EnforceMode(index);
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0008E4B4 File Offset: 0x0008C8B4
		public void EnforceMode(int index)
		{
			int num = (index + 1) / 3;
			ObiBezierCurve.BezierCPMode bezierCPMode = this.controlPointModes[num];
			if (bezierCPMode == ObiBezierCurve.BezierCPMode.Free || (!this.closed && (num == 0 || num == this.controlPointModes.Count - 1)))
			{
				return;
			}
			int num2 = num * 3;
			int num3;
			int num4;
			if (index <= num2)
			{
				num3 = num2 - 1;
				if (num3 < 0)
				{
					num3 = this.controlPoints.Count - 2;
				}
				num4 = num2 + 1;
				if (num4 >= this.controlPoints.Count)
				{
					num4 = 1;
				}
			}
			else
			{
				num3 = num2 + 1;
				if (num3 >= this.controlPoints.Count)
				{
					num3 = 1;
				}
				num4 = num2 - 1;
				if (num4 < 0)
				{
					num4 = this.controlPoints.Count - 2;
				}
			}
			Vector3 a = this.controlPoints[num2];
			Vector3 b = a - this.controlPoints[num3];
			if (bezierCPMode == ObiBezierCurve.BezierCPMode.Aligned)
			{
				b = b.normalized * Vector3.Distance(a, this.controlPoints[num4]);
			}
			this.controlPoints[num4] = a + b;
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0008E5D8 File Offset: 0x0008C9D8
		public void AddSpan()
		{
			int index = this.controlPoints.Count - 1;
			Vector3 a = this.controlPoints[index];
			this.controlPoints.Add(a + Vector3.right * 0.5f);
			this.controlPoints.Add(a + Vector3.right);
			this.controlPoints.Add(a + Vector3.right * 1.5f);
			this.controlPointModes.Add(ObiBezierCurve.BezierCPMode.Free);
			this.EnforceMode(index);
			if (this.closed)
			{
				this.controlPoints[this.controlPoints.Count - 1] = this.controlPoints[0];
				this.controlPointModes[this.controlPointModes.Count - 1] = this.controlPointModes[0];
				this.EnforceMode(0);
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0008E6C4 File Offset: 0x0008CAC4
		public void RemoveCurvePoint(int curvePoint)
		{
			if (this.controlPoints.Count <= 4)
			{
				return;
			}
			int num = Mathf.Max(0, curvePoint * 3 - 1);
			int count = 3;
			if (num == this.controlPoints.Count - 2)
			{
				num--;
			}
			this.controlPoints.RemoveRange(num, count);
			this.controlPointModes.RemoveAt(curvePoint);
			if (this.closed)
			{
				if (num == this.controlPoints.Count)
				{
					this.controlPoints[0] = this.controlPoints[this.controlPoints.Count - 1];
					this.controlPointModes[0] = this.controlPointModes[this.controlPointModes.Count - 1];
				}
				else if (num == 0)
				{
					this.controlPoints[this.controlPoints.Count - 1] = this.controlPoints[0];
					this.controlPointModes[this.controlPointModes.Count - 1] = this.controlPointModes[0];
				}
			}
			this.EnforceMode(num);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0008E7E0 File Offset: 0x0008CBE0
		protected override float Evaluate1D(float y0, float y1, float y2, float y3, float mu)
		{
			float num = 1f - mu;
			return num * num * num * y0 + 3f * num * num * mu * y1 + 3f * num * mu * mu * y2 + mu * mu * mu * y3;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0008E828 File Offset: 0x0008CC28
		protected override float EvaluateFirstDerivative1D(float y0, float y1, float y2, float y3, float mu)
		{
			float num = 1f - mu;
			return 3f * num * num * (y1 - y0) + 6f * num * mu * (y2 - y1) + 3f * mu * mu * (y3 - y2);
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0008E86C File Offset: 0x0008CC6C
		protected override float EvaluateSecondDerivative1D(float y0, float y1, float y2, float y3, float mu)
		{
			float num = 1f - mu;
			return 3f * num * num * (y1 - y0) + 6f * num * mu * (y2 - y1) + 3f * mu * mu * (y3 - y2);
		}

		// Token: 0x040014DC RID: 5340
		[HideInInspector]
		public List<ObiBezierCurve.BezierCPMode> controlPointModes;

		// Token: 0x040014DD RID: 5341
		[HideInInspector]
		public ObiBezierCurve.BezierCPMode lastOpenCPMode;

		// Token: 0x040014DE RID: 5342
		[HideInInspector]
		public Vector3 lastOpenCP;

		// Token: 0x020003F1 RID: 1009
		public enum BezierCPMode
		{
			// Token: 0x040014E0 RID: 5344
			Free,
			// Token: 0x040014E1 RID: 5345
			Aligned,
			// Token: 0x040014E2 RID: 5346
			Mirrored
		}
	}
}
