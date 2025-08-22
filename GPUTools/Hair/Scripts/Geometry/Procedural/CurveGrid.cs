using System;
using GPUTools.Common.Scripts.Utils;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Procedural
{
	// Token: 0x02000A05 RID: 2565
	[Serializable]
	public class CurveGrid
	{
		// Token: 0x060040F4 RID: 16628 RVA: 0x00134D5D File Offset: 0x0013315D
		public CurveGrid()
		{
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x00134D68 File Offset: 0x00133168
		public void GenerateControl()
		{
			this.ControlPoints = new Vector3[this.ControlSizeX * this.ControlSizeY];
			for (int i = 0; i < this.ControlSizeX; i++)
			{
				for (int j = 0; j < this.ControlSizeY; j++)
				{
					float x = (float)i / (float)this.ControlSizeX;
					float z = (float)j / (float)this.ControlSizeY;
					this.SetControl(i, j, new Vector3(x, 0f, z));
				}
			}
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x00134DE8 File Offset: 0x001331E8
		public void GenerateView()
		{
			for (int i = 0; i < this.ViewSizeX; i++)
			{
				for (int j = 0; j < this.ViewSizeY; j++)
				{
					float tX = (float)i / (float)this.ViewSizeX;
					float tY = (float)j / (float)this.ViewSizeY;
					this.GetSplinePoint(tX, tY);
				}
			}
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00134E44 File Offset: 0x00133244
		public Vector3 GetSplinePoint(float tX, float tY)
		{
			int num = (int)(tX * (float)this.ControlSizeX);
			int x = Mathf.Max(0, num - 1);
			int x2 = Mathf.Min(num, this.ControlSizeX - 1);
			int x3 = Mathf.Min(num + 1, this.ControlSizeX - 1);
			int num2 = (int)(tY * (float)this.ControlSizeY);
			int y = Mathf.Max(0, num2 - 1);
			int y2 = Mathf.Min(num2, this.ControlSizeY - 1);
			int y3 = Mathf.Min(num2 + 1, this.ControlSizeY - 1);
			Vector3 control = this.GetControl(x, y);
			Vector3 control2 = this.GetControl(x2, y);
			Vector3 control3 = this.GetControl(x3, y);
			Vector3 control4 = this.GetControl(x, y2);
			Vector3 control5 = this.GetControl(x2, y2);
			Vector3 control6 = this.GetControl(x3, y2);
			Vector3 control7 = this.GetControl(x, y3);
			Vector3 control8 = this.GetControl(x2, y3);
			Vector3 control9 = this.GetControl(x3, y3);
			Vector3 p = (control + control2) * 0.5f;
			Vector3 p2 = (control2 + control3) * 0.5f;
			Vector3 p3 = (control4 + control5) * 0.5f;
			Vector3 p4 = (control5 + control6) * 0.5f;
			Vector3 p5 = (control7 + control8) * 0.5f;
			Vector3 p6 = (control8 + control9) * 0.5f;
			float num3 = 1f / (float)this.ControlSizeX;
			float t = tX % num3 * (float)this.ControlSizeX;
			Vector3 bezierPoint = CurveUtils.GetBezierPoint(p, control2, p2, t);
			Vector3 bezierPoint2 = CurveUtils.GetBezierPoint(p3, control5, p4, t);
			Vector3 bezierPoint3 = CurveUtils.GetBezierPoint(p5, control8, p6, t);
			Vector3 p7 = (bezierPoint + bezierPoint2) * 0.5f;
			Vector3 p8 = (bezierPoint3 + bezierPoint2) * 0.5f;
			float num4 = 1f / (float)this.ControlSizeY;
			float t2 = tY % num4 * (float)this.ControlSizeY;
			return CurveUtils.GetBezierPoint(p7, bezierPoint2, p8, t2);
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00135041 File Offset: 0x00133441
		public void SetControl(int x, int y, Vector3 value)
		{
			this.ControlPoints[x * this.ControlSizeY + y] = value;
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x0013505E File Offset: 0x0013345E
		public Vector3 GetControl(int x, int y)
		{
			return this.ControlPoints[x * this.ControlSizeY + y];
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x0013507A File Offset: 0x0013347A
		public void SetView(int x, int y, Vector3 value)
		{
			this.ControlPoints[x * this.ViewSizeY + y] = value;
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00135097 File Offset: 0x00133497
		public Vector3 GetView(int x, int y)
		{
			return this.ControlPoints[x * this.ViewSizeX + y];
		}

		// Token: 0x040030DC RID: 12508
		[SerializeField]
		public Vector3[] ControlPoints;

		// Token: 0x040030DD RID: 12509
		[SerializeField]
		public int ControlSizeX;

		// Token: 0x040030DE RID: 12510
		[SerializeField]
		public int ControlSizeY;

		// Token: 0x040030DF RID: 12511
		[SerializeField]
		public int ViewSizeX;

		// Token: 0x040030E0 RID: 12512
		[SerializeField]
		public int ViewSizeY;
	}
}
