using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200052B RID: 1323
	[AddComponentMenu("UI/Extensions/Primitives/UI Circle")]
	public class UICircle : UIPrimitiveBase
	{
		// Token: 0x0600218A RID: 8586 RVA: 0x000BFDBD File Offset: 0x000BE1BD
		public UICircle()
		{
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000BFDEA File Offset: 0x000BE1EA
		// (set) Token: 0x0600218C RID: 8588 RVA: 0x000BFDF2 File Offset: 0x000BE1F2
		public int FillPercent
		{
			get
			{
				return this.m_fillPercent;
			}
			set
			{
				this.m_fillPercent = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x000BFE01 File Offset: 0x000BE201
		// (set) Token: 0x0600218E RID: 8590 RVA: 0x000BFE09 File Offset: 0x000BE209
		public bool Fill
		{
			get
			{
				return this.m_fill;
			}
			set
			{
				this.m_fill = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600218F RID: 8591 RVA: 0x000BFE18 File Offset: 0x000BE218
		// (set) Token: 0x06002190 RID: 8592 RVA: 0x000BFE20 File Offset: 0x000BE220
		public float Thickness
		{
			get
			{
				return this.m_thickness;
			}
			set
			{
				this.m_thickness = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000BFE30 File Offset: 0x000BE230
		private void Update()
		{
			this.m_thickness = Mathf.Clamp(this.m_thickness, 0f, base.rectTransform.rect.width / 2f);
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x000BFE6D File Offset: 0x000BE26D
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x000BFE75 File Offset: 0x000BE275
		public int Segments
		{
			get
			{
				return this.m_segments;
			}
			set
			{
				this.m_segments = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000BFE84 File Offset: 0x000BE284
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			float outer = -base.rectTransform.pivot.x * base.rectTransform.rect.width;
			float inner = -base.rectTransform.pivot.x * base.rectTransform.rect.width + this.m_thickness;
			vh.Clear();
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 vector = new Vector2(0f, 0f);
			Vector2 vector2 = new Vector2(0f, 1f);
			Vector2 vector3 = new Vector2(1f, 1f);
			Vector2 vector4 = new Vector2(1f, 0f);
			if (this.FixedToSegments)
			{
				float num = (float)this.m_fillPercent / 100f;
				float num2 = 360f / (float)this.m_segments;
				int num3 = (int)((float)(this.m_segments + 1) * num);
				for (int i = 0; i < num3; i++)
				{
					float f = 0.017453292f * ((float)i * num2);
					float c = Mathf.Cos(f);
					float s = Mathf.Sin(f);
					vector = new Vector2(0f, 1f);
					vector2 = new Vector2(1f, 1f);
					vector3 = new Vector2(1f, 0f);
					vector4 = new Vector2(0f, 0f);
					Vector2 vector5;
					Vector2 vector6;
					Vector2 vector7;
					Vector2 vector8;
					this.StepThroughPointsAndFill(outer, inner, ref zero, ref zero2, out vector5, out vector6, out vector7, out vector8, c, s);
					vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
					{
						vector5,
						vector6,
						vector7,
						vector8
					}, new Vector2[]
					{
						vector,
						vector2,
						vector3,
						vector4
					}));
				}
			}
			else
			{
				float width = base.rectTransform.rect.width;
				float height = base.rectTransform.rect.height;
				float num4 = (float)this.m_fillPercent / 100f * 6.2831855f / (float)this.m_segments;
				float num5 = 0f;
				for (int j = 0; j < this.m_segments + 1; j++)
				{
					float c2 = Mathf.Cos(num5);
					float s2 = Mathf.Sin(num5);
					Vector2 vector5;
					Vector2 vector6;
					Vector2 vector7;
					Vector2 vector8;
					this.StepThroughPointsAndFill(outer, inner, ref zero, ref zero2, out vector5, out vector6, out vector7, out vector8, c2, s2);
					vector = new Vector2(vector5.x / width + 0.5f, vector5.y / height + 0.5f);
					vector2 = new Vector2(vector6.x / width + 0.5f, vector6.y / height + 0.5f);
					vector3 = new Vector2(vector7.x / width + 0.5f, vector7.y / height + 0.5f);
					vector4 = new Vector2(vector8.x / width + 0.5f, vector8.y / height + 0.5f);
					vh.AddUIVertexQuad(base.SetVbo(new Vector2[]
					{
						vector5,
						vector6,
						vector7,
						vector8
					}, new Vector2[]
					{
						vector,
						vector2,
						vector3,
						vector4
					}));
					num5 += num4;
				}
			}
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000C0260 File Offset: 0x000BE660
		private void StepThroughPointsAndFill(float outer, float inner, ref Vector2 prevX, ref Vector2 prevY, out Vector2 pos0, out Vector2 pos1, out Vector2 pos2, out Vector2 pos3, float c, float s)
		{
			pos0 = prevX;
			pos1 = new Vector2(outer * c, outer * s);
			if (this.m_fill)
			{
				pos2 = Vector2.zero;
				pos3 = Vector2.zero;
			}
			else
			{
				pos2 = new Vector2(inner * c, inner * s);
				pos3 = prevY;
			}
			prevX = pos1;
			prevY = pos2;
		}

		// Token: 0x04001BEE RID: 7150
		[Tooltip("The circular fill percentage of the primitive, affected by FixedToSegments")]
		[Range(0f, 100f)]
		[SerializeField]
		private int m_fillPercent = 100;

		// Token: 0x04001BEF RID: 7151
		[Tooltip("Should the primitive fill draw by segments or absolute percentage")]
		public bool FixedToSegments;

		// Token: 0x04001BF0 RID: 7152
		[Tooltip("Draw the primitive filled or as a line")]
		[SerializeField]
		private bool m_fill = true;

		// Token: 0x04001BF1 RID: 7153
		[Tooltip("If not filled, the thickness of the primitive line")]
		[SerializeField]
		private float m_thickness = 5f;

		// Token: 0x04001BF2 RID: 7154
		[Tooltip("The number of segments to draw the primitive, more segments = smoother primitive")]
		[Range(0f, 360f)]
		[SerializeField]
		private int m_segments = 360;
	}
}
