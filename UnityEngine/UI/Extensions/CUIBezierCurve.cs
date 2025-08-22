using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004EB RID: 1259
	public class CUIBezierCurve : MonoBehaviour
	{
		// Token: 0x06001FCF RID: 8143 RVA: 0x000B4EC4 File Offset: 0x000B32C4
		public CUIBezierCurve()
		{
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x000B4ECC File Offset: 0x000B32CC
		public Vector3[] ControlPoints
		{
			get
			{
				return this.controlPoints;
			}
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000B4ED4 File Offset: 0x000B32D4
		public void Refresh()
		{
			if (this.OnRefresh != null)
			{
				this.OnRefresh();
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000B4EEC File Offset: 0x000B32EC
		public Vector3 GetPoint(float _time)
		{
			float num = 1f - _time;
			return num * num * num * this.controlPoints[0] + 3f * num * num * _time * this.controlPoints[1] + 3f * num * _time * _time * this.controlPoints[2] + _time * _time * _time * this.controlPoints[3];
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000B4F88 File Offset: 0x000B3388
		public Vector3 GetTangent(float _time)
		{
			float num = 1f - _time;
			return 3f * num * num * (this.controlPoints[1] - this.controlPoints[0]) + 6f * num * _time * (this.controlPoints[2] - this.controlPoints[1]) + 3f * _time * _time * (this.controlPoints[3] - this.controlPoints[2]);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000B5048 File Offset: 0x000B3448
		public void ReportSet()
		{
			if (this.controlPoints == null)
			{
				this.controlPoints = new Vector3[CUIBezierCurve.CubicBezierCurvePtNum];
				this.controlPoints[0] = new Vector3(0f, 0f, 0f);
				this.controlPoints[1] = new Vector3(0f, 1f, 0f);
				this.controlPoints[2] = new Vector3(1f, 1f, 0f);
				this.controlPoints[3] = new Vector3(1f, 0f, 0f);
			}
			bool flag = true;
			flag &= (this.controlPoints.Length == CUIBezierCurve.CubicBezierCurvePtNum);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x000B5118 File Offset: 0x000B3518
		// Note: this type is marked as 'beforefieldinit'.
		static CUIBezierCurve()
		{
		}

		// Token: 0x04001ACC RID: 6860
		public static readonly int CubicBezierCurvePtNum = 4;

		// Token: 0x04001ACD RID: 6861
		[SerializeField]
		protected Vector3[] controlPoints;

		// Token: 0x04001ACE RID: 6862
		public Action OnRefresh;
	}
}
