using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004EC RID: 1260
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Graphic))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Graphic")]
	public class CUIGraphic : BaseMeshEffect
	{
		// Token: 0x06001FD6 RID: 8150 RVA: 0x000B5120 File Offset: 0x000B3520
		public CUIGraphic()
		{
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x000B514C File Offset: 0x000B354C
		public bool IsCurved
		{
			get
			{
				return this.isCurved;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000B5154 File Offset: 0x000B3554
		public bool IsLockWithRatio
		{
			get
			{
				return this.isLockWithRatio;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x000B515C File Offset: 0x000B355C
		public RectTransform RectTrans
		{
			get
			{
				return this.rectTrans;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x000B5164 File Offset: 0x000B3564
		public Graphic UIGraphic
		{
			get
			{
				return this.uiGraphic;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x000B516C File Offset: 0x000B356C
		public CUIGraphic RefCUIGraphic
		{
			get
			{
				return this.refCUIGraphic;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x000B5174 File Offset: 0x000B3574
		public CUIBezierCurve[] RefCurves
		{
			get
			{
				return this.refCurves;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x000B517C File Offset: 0x000B357C
		public Vector3_Array2D[] RefCurvesControlRatioPoints
		{
			get
			{
				return this.refCurvesControlRatioPoints;
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000B5184 File Offset: 0x000B3584
		protected void solveDoubleEquationWithVector(float _x_1, float _y_1, float _x_2, float _y_2, Vector3 _constant_1, Vector3 _contant_2, out Vector3 _x, out Vector3 _y)
		{
			if (Mathf.Abs(_x_1) > Mathf.Abs(_x_2))
			{
				Vector3 vector = _constant_1 * _x_2 / _x_1;
				float num = _y_1 * _x_2 / _x_1;
				_y = (_contant_2 - vector) / (_y_2 - num);
				if (_x_2 != 0f)
				{
					_x = (vector - num * _y) / _x_2;
				}
				else
				{
					_x = (_constant_1 - _y_1 * _y) / _x_1;
				}
			}
			else
			{
				Vector3 vector = _contant_2 * _x_1 / _x_2;
				float num = _y_2 * _x_1 / _x_2;
				_x = (_constant_1 - vector) / (_y_1 - num);
				if (_x_1 != 0f)
				{
					_y = (vector - num * _x) / _x_1;
				}
				else
				{
					_y = (_contant_2 - _y_2 * _x) / _x_2;
				}
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000B52A4 File Offset: 0x000B36A4
		protected UIVertex uiVertexLerp(UIVertex _a, UIVertex _b, float _time)
		{
			return new UIVertex
			{
				position = Vector3.Lerp(_a.position, _b.position, _time),
				normal = Vector3.Lerp(_a.normal, _b.normal, _time),
				tangent = Vector3.Lerp(_a.tangent, _b.tangent, _time),
				uv0 = Vector2.Lerp(_a.uv0, _b.uv0, _time),
				uv1 = Vector2.Lerp(_a.uv1, _b.uv1, _time),
				color = Color.Lerp(_a.color, _b.color, _time)
			};
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000B537C File Offset: 0x000B377C
		protected UIVertex uiVertexBerp(UIVertex v_bottomLeft, UIVertex v_topLeft, UIVertex v_topRight, UIVertex v_bottomRight, float _xTime, float _yTime)
		{
			UIVertex b = this.uiVertexLerp(v_topLeft, v_topRight, _xTime);
			UIVertex a = this.uiVertexLerp(v_bottomLeft, v_bottomRight, _xTime);
			return this.uiVertexLerp(a, b, _yTime);
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000B53AC File Offset: 0x000B37AC
		protected void tessellateQuad(List<UIVertex> _quads, int _thisQuadIdx)
		{
			UIVertex v_bottomLeft = _quads[_thisQuadIdx];
			UIVertex v_topLeft = _quads[_thisQuadIdx + 1];
			UIVertex v_topRight = _quads[_thisQuadIdx + 2];
			UIVertex v_bottomRight = _quads[_thisQuadIdx + 3];
			float num = 100f / this.resolution;
			int num2 = Mathf.Max(1, Mathf.CeilToInt((v_topLeft.position - v_bottomLeft.position).magnitude / num));
			int num3 = Mathf.Max(1, Mathf.CeilToInt((v_topRight.position - v_topLeft.position).magnitude / num));
			int num4 = 0;
			for (int i = 0; i < num3; i++)
			{
				int j = 0;
				while (j < num2)
				{
					_quads.Add(default(UIVertex));
					_quads.Add(default(UIVertex));
					_quads.Add(default(UIVertex));
					_quads.Add(default(UIVertex));
					float xTime = (float)i / (float)num3;
					float yTime = (float)j / (float)num2;
					float xTime2 = (float)(i + 1) / (float)num3;
					float yTime2 = (float)(j + 1) / (float)num2;
					_quads[_quads.Count - 4] = this.uiVertexBerp(v_bottomLeft, v_topLeft, v_topRight, v_bottomRight, xTime, yTime);
					_quads[_quads.Count - 3] = this.uiVertexBerp(v_bottomLeft, v_topLeft, v_topRight, v_bottomRight, xTime, yTime2);
					_quads[_quads.Count - 2] = this.uiVertexBerp(v_bottomLeft, v_topLeft, v_topRight, v_bottomRight, xTime2, yTime2);
					_quads[_quads.Count - 1] = this.uiVertexBerp(v_bottomLeft, v_topLeft, v_topRight, v_bottomRight, xTime2, yTime);
					j++;
					num4++;
				}
			}
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000B5554 File Offset: 0x000B3954
		protected void tessellateGraphic(List<UIVertex> _verts)
		{
			for (int i = 0; i < _verts.Count; i += 6)
			{
				this.reuse_quads.Add(_verts[i]);
				this.reuse_quads.Add(_verts[i + 1]);
				this.reuse_quads.Add(_verts[i + 2]);
				this.reuse_quads.Add(_verts[i + 4]);
			}
			int num = this.reuse_quads.Count / 4;
			for (int j = 0; j < num; j++)
			{
				this.tessellateQuad(this.reuse_quads, j * 4);
			}
			this.reuse_quads.RemoveRange(0, num * 4);
			_verts.Clear();
			for (int k = 0; k < this.reuse_quads.Count; k += 4)
			{
				_verts.Add(this.reuse_quads[k]);
				_verts.Add(this.reuse_quads[k + 1]);
				_verts.Add(this.reuse_quads[k + 2]);
				_verts.Add(this.reuse_quads[k + 2]);
				_verts.Add(this.reuse_quads[k + 3]);
				_verts.Add(this.reuse_quads[k]);
			}
			this.reuse_quads.Clear();
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000B56A5 File Offset: 0x000B3AA5
		protected override void OnRectTransformDimensionsChange()
		{
			if (this.isLockWithRatio)
			{
				this.UpdateCurveControlPointPositions();
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000B56B8 File Offset: 0x000B3AB8
		public void Refresh()
		{
			this.ReportSet();
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				if (cuibezierCurve.ControlPoints != null)
				{
					Vector3[] controlPoints = cuibezierCurve.ControlPoints;
					for (int j = 0; j < CUIBezierCurve.CubicBezierCurvePtNum; j++)
					{
						Vector3 value = controlPoints[j];
						value.x = (value.x + this.rectTrans.rect.width * this.rectTrans.pivot.x) / this.rectTrans.rect.width;
						value.y = (value.y + this.rectTrans.rect.height * this.rectTrans.pivot.y) / this.rectTrans.rect.height;
						this.refCurvesControlRatioPoints[i][j] = value;
					}
				}
			}
			if (this.uiGraphic != null)
			{
				this.uiGraphic.enabled = false;
				this.uiGraphic.enabled = true;
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000B57FA File Offset: 0x000B3BFA
		protected override void Awake()
		{
			base.Awake();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000B5808 File Offset: 0x000B3C08
		protected override void OnEnable()
		{
			base.OnEnable();
			this.OnRectTransformDimensionsChange();
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000B5818 File Offset: 0x000B3C18
		public virtual void ReportSet()
		{
			if (this.rectTrans == null)
			{
				this.rectTrans = base.GetComponent<RectTransform>();
			}
			if (this.refCurves == null)
			{
				this.refCurves = new CUIBezierCurve[2];
			}
			bool flag = true;
			for (int i = 0; i < 2; i++)
			{
				flag &= (this.refCurves[i] != null);
			}
			if (!(flag & this.refCurves.Length == 2))
			{
				CUIBezierCurve[] array = this.refCurves;
				for (int j = 0; j < 2; j++)
				{
					if (this.refCurves[j] == null)
					{
						GameObject gameObject = new GameObject();
						gameObject.transform.SetParent(base.transform);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localEulerAngles = Vector3.zero;
						if (j == 0)
						{
							gameObject.name = "BottomRefCurve";
						}
						else
						{
							gameObject.name = "TopRefCurve";
						}
						array[j] = gameObject.AddComponent<CUIBezierCurve>();
					}
					else
					{
						array[j] = this.refCurves[j];
					}
					array[j].ReportSet();
				}
				this.refCurves = array;
			}
			if (this.refCurvesControlRatioPoints == null)
			{
				this.refCurvesControlRatioPoints = new Vector3_Array2D[this.refCurves.Length];
				for (int k = 0; k < this.refCurves.Length; k++)
				{
					this.refCurvesControlRatioPoints[k].array = new Vector3[this.refCurves[k].ControlPoints.Length];
				}
				this.FixTextToRectTrans();
				this.Refresh();
			}
			for (int l = 0; l < 2; l++)
			{
				this.refCurves[l].OnRefresh = new Action(this.Refresh);
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000B59E4 File Offset: 0x000B3DE4
		public void FixTextToRectTrans()
		{
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				for (int j = 0; j < CUIBezierCurve.CubicBezierCurvePtNum; j++)
				{
					if (cuibezierCurve.ControlPoints != null)
					{
						Vector3[] controlPoints = cuibezierCurve.ControlPoints;
						if (i == 0)
						{
							controlPoints[j].y = -this.rectTrans.rect.height * this.rectTrans.pivot.y;
						}
						else
						{
							controlPoints[j].y = this.rectTrans.rect.height - this.rectTrans.rect.height * this.rectTrans.pivot.y;
						}
						controlPoints[j].x = this.rectTrans.rect.width * (float)j / (float)(CUIBezierCurve.CubicBezierCurvePtNum - 1);
						Vector3[] array = controlPoints;
						int num = j;
						array[num].x = array[num].x - this.rectTrans.rect.width * this.rectTrans.pivot.x;
						controlPoints[j].z = 0f;
					}
				}
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000B5B40 File Offset: 0x000B3F40
		public void ReferenceCUIForBCurves()
		{
			Vector3 localPosition = this.rectTrans.localPosition;
			localPosition.x += -this.rectTrans.rect.width * this.rectTrans.pivot.x + this.refCUIGraphic.rectTrans.rect.width * this.refCUIGraphic.rectTrans.pivot.x;
			localPosition.y += -this.rectTrans.rect.height * this.rectTrans.pivot.y + this.refCUIGraphic.rectTrans.rect.height * this.refCUIGraphic.rectTrans.pivot.y;
			Vector3 vector = new Vector3(localPosition.x / this.refCUIGraphic.RectTrans.rect.width, localPosition.y / this.refCUIGraphic.RectTrans.rect.height, localPosition.z);
			Vector3 vector2 = new Vector3((localPosition.x + this.rectTrans.rect.width) / this.refCUIGraphic.RectTrans.rect.width, (localPosition.y + this.rectTrans.rect.height) / this.refCUIGraphic.RectTrans.rect.height, localPosition.z);
			this.refCurves[0].ControlPoints[0] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector.x, vector.y) - this.rectTrans.localPosition;
			this.refCurves[0].ControlPoints[3] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector2.x, vector.y) - this.rectTrans.localPosition;
			this.refCurves[1].ControlPoints[0] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector.x, vector2.y) - this.rectTrans.localPosition;
			this.refCurves[1].ControlPoints[3] = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector2.x, vector2.y) - this.rectTrans.localPosition;
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				float yTime = (i != 0) ? vector2.y : vector.y;
				Vector3 bcurveSandwichSpacePoint = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector.x, yTime);
				Vector3 bcurveSandwichSpacePoint2 = this.refCUIGraphic.GetBCurveSandwichSpacePoint(vector2.x, yTime);
				float num = 0.25f;
				float num2 = 0.75f;
				Vector3 bcurveSandwichSpacePoint3 = this.refCUIGraphic.GetBCurveSandwichSpacePoint((vector.x * 0.75f + vector2.x * 0.25f) / 1f, yTime);
				Vector3 bcurveSandwichSpacePoint4 = this.refCUIGraphic.GetBCurveSandwichSpacePoint((vector.x * 0.25f + vector2.x * 0.75f) / 1f, yTime);
				float x_ = 3f * num2 * num2 * num;
				float y_ = 3f * num2 * num * num;
				float x_2 = 3f * num * num * num2;
				float y_2 = 3f * num * num2 * num2;
				Vector3 constant_ = bcurveSandwichSpacePoint3 - Mathf.Pow(num2, 3f) * bcurveSandwichSpacePoint - Mathf.Pow(num, 3f) * bcurveSandwichSpacePoint2;
				Vector3 contant_ = bcurveSandwichSpacePoint4 - Mathf.Pow(num, 3f) * bcurveSandwichSpacePoint - Mathf.Pow(num2, 3f) * bcurveSandwichSpacePoint2;
				Vector3 a;
				Vector3 a2;
				this.solveDoubleEquationWithVector(x_, y_, x_2, y_2, constant_, contant_, out a, out a2);
				cuibezierCurve.ControlPoints[1] = a - this.rectTrans.localPosition;
				cuibezierCurve.ControlPoints[2] = a2 - this.rectTrans.localPosition;
			}
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x000B5FE0 File Offset: 0x000B43E0
		public override void ModifyMesh(Mesh _mesh)
		{
			if (!this.IsActive())
			{
				return;
			}
			using (VertexHelper vertexHelper = new VertexHelper(_mesh))
			{
				this.ModifyMesh(vertexHelper);
				vertexHelper.FillMesh(_mesh);
			}
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x000B6030 File Offset: 0x000B4430
		public override void ModifyMesh(VertexHelper _vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			_vh.GetUIVertexStream(list);
			this.modifyVertices(list);
			_vh.Clear();
			_vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000B606C File Offset: 0x000B446C
		protected virtual void modifyVertices(List<UIVertex> _verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			this.tessellateGraphic(_verts);
			if (!this.isCurved)
			{
				return;
			}
			for (int i = 0; i < _verts.Count; i++)
			{
				UIVertex value = _verts[i];
				float xTime = (value.position.x + this.rectTrans.rect.width * this.rectTrans.pivot.x) / this.rectTrans.rect.width;
				float yTime = (value.position.y + this.rectTrans.rect.height * this.rectTrans.pivot.y) / this.rectTrans.rect.height;
				Vector3 bcurveSandwichSpacePoint = this.GetBCurveSandwichSpacePoint(xTime, yTime);
				value.position.x = bcurveSandwichSpacePoint.x;
				value.position.y = bcurveSandwichSpacePoint.y;
				value.position.z = bcurveSandwichSpacePoint.z;
				_verts[i] = value;
			}
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000B619C File Offset: 0x000B459C
		public void UpdateCurveControlPointPositions()
		{
			this.ReportSet();
			for (int i = 0; i < this.refCurves.Length; i++)
			{
				CUIBezierCurve cuibezierCurve = this.refCurves[i];
				for (int j = 0; j < this.refCurves[i].ControlPoints.Length; j++)
				{
					Vector3 vector = this.refCurvesControlRatioPoints[i][j];
					vector.x = vector.x * this.rectTrans.rect.width - this.rectTrans.rect.width * this.rectTrans.pivot.x;
					vector.y = vector.y * this.rectTrans.rect.height - this.rectTrans.rect.height * this.rectTrans.pivot.y;
					cuibezierCurve.ControlPoints[j] = vector;
				}
			}
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000B62B0 File Offset: 0x000B46B0
		public Vector3 GetBCurveSandwichSpacePoint(float _xTime, float _yTime)
		{
			return this.refCurves[0].GetPoint(_xTime) * (1f - _yTime) + this.refCurves[1].GetPoint(_xTime) * _yTime;
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000B62E5 File Offset: 0x000B46E5
		public Vector3 GetBCurveSandwichSpaceTangent(float _xTime, float _yTime)
		{
			return this.refCurves[0].GetTangent(_xTime) * (1f - _yTime) + this.refCurves[1].GetTangent(_xTime) * _yTime;
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x000B631A File Offset: 0x000B471A
		// Note: this type is marked as 'beforefieldinit'.
		static CUIGraphic()
		{
		}

		// Token: 0x04001ACF RID: 6863
		public static readonly int bottomCurveIdx;

		// Token: 0x04001AD0 RID: 6864
		public static readonly int topCurveIdx = 1;

		// Token: 0x04001AD1 RID: 6865
		[Tooltip("Set true to make the curve/morph to work. Set false to quickly see the original UI.")]
		[SerializeField]
		protected bool isCurved = true;

		// Token: 0x04001AD2 RID: 6866
		[Tooltip("Set true to dynamically change the curve according to the dynamic change of the UI layout")]
		[SerializeField]
		protected bool isLockWithRatio = true;

		// Token: 0x04001AD3 RID: 6867
		[Tooltip("Pick a higher resolution to improve the quality of the curved graphic.")]
		[SerializeField]
		[Range(0.01f, 30f)]
		protected float resolution = 5f;

		// Token: 0x04001AD4 RID: 6868
		protected RectTransform rectTrans;

		// Token: 0x04001AD5 RID: 6869
		[Tooltip("Put in the Graphic you want to curve/morph here.")]
		[SerializeField]
		protected Graphic uiGraphic;

		// Token: 0x04001AD6 RID: 6870
		[Tooltip("Put in the reference Graphic that will be used to tune the bezier curves. Think button image and text.")]
		[SerializeField]
		protected CUIGraphic refCUIGraphic;

		// Token: 0x04001AD7 RID: 6871
		[Tooltip("Do not touch this unless you are sure what you are doing. The curves are (re)generated automatically.")]
		[SerializeField]
		protected CUIBezierCurve[] refCurves;

		// Token: 0x04001AD8 RID: 6872
		[HideInInspector]
		[SerializeField]
		protected Vector3_Array2D[] refCurvesControlRatioPoints;

		// Token: 0x04001AD9 RID: 6873
		protected List<UIVertex> reuse_quads = new List<UIVertex>();
	}
}
