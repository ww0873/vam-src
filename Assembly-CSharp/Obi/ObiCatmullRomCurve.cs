using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003F2 RID: 1010
	[ExecuteInEditMode]
	public class ObiCatmullRomCurve : ObiCurve
	{
		// Token: 0x0600199D RID: 6557 RVA: 0x0008E8AF File Offset: 0x0008CCAF
		public ObiCatmullRomCurve()
		{
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0008E8B7 File Offset: 0x0008CCB7
		public override void Awake()
		{
			this.minPoints = 4;
			this.unusedPoints = 2;
			base.Awake();
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0008E8D0 File Offset: 0x0008CCD0
		protected override void SetClosed(bool closed)
		{
			if (this.closed == closed)
			{
				return;
			}
			if (!this.closed && closed)
			{
				this.lastOpenCP0 = this.controlPoints[0];
				this.lastOpenCP1 = this.controlPoints[1];
				this.lastOpenCPN = this.controlPoints[this.controlPoints.Count - 1];
				this.controlPoints[0] = this.controlPoints[this.controlPoints.Count - 3];
				this.controlPoints[1] = this.controlPoints[this.controlPoints.Count - 2];
				this.controlPoints[this.controlPoints.Count - 1] = this.controlPoints[2];
			}
			else
			{
				this.controlPoints[0] = this.lastOpenCP0;
				this.controlPoints[1] = this.lastOpenCP1;
				this.controlPoints[this.controlPoints.Count - 1] = this.lastOpenCPN;
			}
			this.closed = closed;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0008E9F8 File Offset: 0x0008CDF8
		public override void DisplaceControlPoint(int index, Vector3 delta)
		{
			if (index < 0 || index >= this.controlPoints.Count)
			{
				return;
			}
			if (this.closed)
			{
				if (index == 0 || index == this.controlPoints.Count - 3)
				{
					List<Vector3> controlPoints;
					(controlPoints = this.controlPoints)[0] = controlPoints[0] + delta;
					int index2;
					(controlPoints = this.controlPoints)[index2 = this.controlPoints.Count - 3] = controlPoints[index2] + delta;
				}
				else if (index == 1 || index == this.controlPoints.Count - 2)
				{
					List<Vector3> controlPoints;
					(controlPoints = this.controlPoints)[1] = controlPoints[1] + delta;
					int index3;
					(controlPoints = this.controlPoints)[index3 = this.controlPoints.Count - 2] = controlPoints[index3] + delta;
				}
				else if (index == 2 || index == this.controlPoints.Count - 1)
				{
					List<Vector3> controlPoints;
					(controlPoints = this.controlPoints)[2] = controlPoints[2] + delta;
					int index4;
					(controlPoints = this.controlPoints)[index4 = this.controlPoints.Count - 1] = controlPoints[index4] + delta;
				}
				else
				{
					List<Vector3> controlPoints;
					(controlPoints = this.controlPoints)[index] = controlPoints[index] + delta;
				}
			}
			else
			{
				List<Vector3> controlPoints;
				(controlPoints = this.controlPoints)[index] = controlPoints[index] + delta;
			}
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0008EB94 File Offset: 0x0008CF94
		public override int GetNumSpans()
		{
			return this.controlPoints.Count - this.unusedPoints - 1;
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0008EBAC File Offset: 0x0008CFAC
		public override int GetSpanControlPointForMu(float mu, out float spanMu)
		{
			int numSpans = this.GetNumSpans();
			spanMu = mu * (float)numSpans;
			int num = (mu < 1f) ? ((int)spanMu) : (numSpans - 1);
			spanMu -= (float)num;
			return num;
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0008EBE8 File Offset: 0x0008CFE8
		protected override float Evaluate1D(float y0, float y1, float y2, float y3, float mu)
		{
			float num = mu * mu;
			float num2 = -0.5f * y0 + 1.5f * y1 - 1.5f * y2 + 0.5f * y3;
			float num3 = y0 - 2.5f * y1 + 2f * y2 - 0.5f * y3;
			float num4 = -0.5f * y0 + 0.5f * y2;
			return num2 * mu * num + num3 * num + num4 * mu + y1;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0008EC60 File Offset: 0x0008D060
		protected override float EvaluateFirstDerivative1D(float y0, float y1, float y2, float y3, float mu)
		{
			float num = mu * mu;
			float num2 = -0.5f * y0 + 1.5f * y1 - 1.5f * y2 + 0.5f * y3;
			float num3 = y0 - 2.5f * y1 + 2f * y2 - 0.5f * y3;
			float num4 = -0.5f * y0 + 0.5f * y2;
			return 3f * num2 * num + 2f * num3 * mu + num4;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0008ECD8 File Offset: 0x0008D0D8
		protected override float EvaluateSecondDerivative1D(float y0, float y1, float y2, float y3, float mu)
		{
			float num = -0.5f * y0 + 1.5f * y1 - 1.5f * y2 + 0.5f * y3;
			float num2 = y0 - 2.5f * y1 + 2f * y2 - 0.5f * y3;
			return 6f * num * mu + 2f * num2;
		}

		// Token: 0x040014E3 RID: 5347
		[HideInInspector]
		public Vector3 lastOpenCP0;

		// Token: 0x040014E4 RID: 5348
		[HideInInspector]
		public Vector3 lastOpenCP1;

		// Token: 0x040014E5 RID: 5349
		[HideInInspector]
		public Vector3 lastOpenCPN;
	}
}
