using System;
using System.Collections.Generic;
using UnityEngine.Sprites;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200052E RID: 1326
	[AddComponentMenu("UI/Extensions/Primitives/UILineRenderer")]
	[RequireComponent(typeof(RectTransform))]
	public class UILineRenderer : UIPrimitiveBase
	{
		// Token: 0x060021B3 RID: 8627 RVA: 0x000C0939 File Offset: 0x000BED39
		public UILineRenderer()
		{
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x000C0954 File Offset: 0x000BED54
		// (set) Token: 0x060021B5 RID: 8629 RVA: 0x000C095C File Offset: 0x000BED5C
		public float LineThickness
		{
			get
			{
				return this.lineThickness;
			}
			set
			{
				this.lineThickness = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x000C096B File Offset: 0x000BED6B
		// (set) Token: 0x060021B7 RID: 8631 RVA: 0x000C0973 File Offset: 0x000BED73
		public bool RelativeSize
		{
			get
			{
				return this.relativeSize;
			}
			set
			{
				this.relativeSize = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x000C0982 File Offset: 0x000BED82
		// (set) Token: 0x060021B9 RID: 8633 RVA: 0x000C098A File Offset: 0x000BED8A
		public bool LineList
		{
			get
			{
				return this.lineList;
			}
			set
			{
				this.lineList = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x060021BA RID: 8634 RVA: 0x000C0999 File Offset: 0x000BED99
		// (set) Token: 0x060021BB RID: 8635 RVA: 0x000C09A1 File Offset: 0x000BEDA1
		public bool LineCaps
		{
			get
			{
				return this.lineCaps;
			}
			set
			{
				this.lineCaps = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x060021BC RID: 8636 RVA: 0x000C09B0 File Offset: 0x000BEDB0
		// (set) Token: 0x060021BD RID: 8637 RVA: 0x000C09B8 File Offset: 0x000BEDB8
		public int BezierSegmentsPerCurve
		{
			get
			{
				return this.bezierSegmentsPerCurve;
			}
			set
			{
				this.bezierSegmentsPerCurve = value;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x000C09C1 File Offset: 0x000BEDC1
		// (set) Token: 0x060021BF RID: 8639 RVA: 0x000C09C9 File Offset: 0x000BEDC9
		public Vector2[] Points
		{
			get
			{
				return this.m_points;
			}
			set
			{
				if (this.m_points == value)
				{
					return;
				}
				this.m_points = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000C09E8 File Offset: 0x000BEDE8
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this.m_points == null)
			{
				return;
			}
			this.GeneratedUVs();
			Vector2[] array = this.m_points;
			if (this.BezierMode != UILineRenderer.BezierType.None && this.BezierMode != UILineRenderer.BezierType.Catenary && this.m_points.Length > 3)
			{
				BezierPath bezierPath = new BezierPath();
				bezierPath.SetControlPoints(array);
				bezierPath.SegmentsPerCurve = this.bezierSegmentsPerCurve;
				UILineRenderer.BezierType bezierMode = this.BezierMode;
				List<Vector2> list;
				if (bezierMode != UILineRenderer.BezierType.Basic)
				{
					if (bezierMode != UILineRenderer.BezierType.Improved)
					{
						list = bezierPath.GetDrawingPoints2();
					}
					else
					{
						list = bezierPath.GetDrawingPoints1();
					}
				}
				else
				{
					list = bezierPath.GetDrawingPoints0();
				}
				array = list.ToArray();
			}
			if (this.BezierMode == UILineRenderer.BezierType.Catenary && this.m_points.Length == 2)
			{
				array = new CableCurve(array)
				{
					slack = base.Resoloution,
					steps = this.BezierSegmentsPerCurve
				}.Points();
			}
			if (base.ImproveResolution != ResolutionMode.None)
			{
				array = base.IncreaseResolution(array);
			}
			float num = this.relativeSize ? base.rectTransform.rect.width : 1f;
			float num2 = this.relativeSize ? base.rectTransform.rect.height : 1f;
			float num3 = -base.rectTransform.pivot.x * num;
			float num4 = -base.rectTransform.pivot.y * num2;
			vh.Clear();
			List<UIVertex[]> list2 = new List<UIVertex[]>();
			if (this.lineList)
			{
				for (int i = 1; i < array.Length; i += 2)
				{
					Vector2 start = array[i - 1];
					Vector2 end = array[i];
					start = new Vector2(start.x * num + num3, start.y * num2 + num4);
					end = new Vector2(end.x * num + num3, end.y * num2 + num4);
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(start, end, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.End));
					}
				}
			}
			else
			{
				for (int j = 1; j < array.Length; j++)
				{
					Vector2 start2 = array[j - 1];
					Vector2 end2 = array[j];
					start2 = new Vector2(start2.x * num + num3, start2.y * num2 + num4);
					end2 = new Vector2(end2.x * num + num3, end2.y * num2 + num4);
					if (this.lineCaps && j == 1)
					{
						list2.Add(this.CreateLineCap(start2, end2, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(start2, end2, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps && j == array.Length - 1)
					{
						list2.Add(this.CreateLineCap(start2, end2, UILineRenderer.SegmentType.End));
					}
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				if (!this.lineList && k < list2.Count - 1)
				{
					Vector3 v = list2[k][1].position - list2[k][2].position;
					Vector3 v2 = list2[k + 1][2].position - list2[k + 1][1].position;
					float num5 = Vector2.Angle(v, v2) * 0.017453292f;
					float num6 = Mathf.Sign(Vector3.Cross(v.normalized, v2.normalized).z);
					float num7 = this.lineThickness / (2f * Mathf.Tan(num5 / 2f));
					Vector3 position = list2[k][2].position - v.normalized * num7 * num6;
					Vector3 position2 = list2[k][3].position + v.normalized * num7 * num6;
					UILineRenderer.JoinType joinType = this.LineJoins;
					if (joinType == UILineRenderer.JoinType.Miter)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.2617994f)
						{
							list2[k][2].position = position;
							list2[k][3].position = position2;
							list2[k + 1][0].position = position2;
							list2[k + 1][1].position = position;
						}
						else
						{
							joinType = UILineRenderer.JoinType.Bevel;
						}
					}
					if (joinType == UILineRenderer.JoinType.Bevel)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.5235988f)
						{
							if (num6 < 0f)
							{
								list2[k][2].position = position;
								list2[k + 1][1].position = position;
							}
							else
							{
								list2[k][3].position = position2;
								list2[k + 1][0].position = position2;
							}
						}
						UIVertex[] verts = new UIVertex[]
						{
							list2[k][2],
							list2[k][3],
							list2[k + 1][0],
							list2[k + 1][1]
						};
						vh.AddUIVertexQuad(verts);
					}
				}
				vh.AddUIVertexQuad(list2[k]);
			}
			if (vh.currentVertCount > 64000)
			{
				Debug.LogError("Max Verticies size is 64000, current mesh vertcies count is [" + vh.currentVertCount + "] - Cannot Draw");
				vh.Clear();
				return;
			}
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000C10AC File Offset: 0x000BF4AC
		private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			if (type == UILineRenderer.SegmentType.Start)
			{
				Vector2 start2 = start - (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(start2, start, UILineRenderer.SegmentType.Start);
			}
			if (type == UILineRenderer.SegmentType.End)
			{
				Vector2 end2 = end + (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(end, end2, UILineRenderer.SegmentType.End);
			}
			Debug.LogError("Bad SegmentType passed in to CreateLineCap. Must be SegmentType.Start or SegmentType.End");
			return null;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000C113C File Offset: 0x000BF53C
		private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			Vector2 vector = new Vector2(start.y - end.y, end.x - start.x);
			Vector2 b = vector.normalized * this.lineThickness / 2f;
			Vector2 vector2 = start - b;
			Vector2 vector3 = start + b;
			Vector2 vector4 = end + b;
			Vector2 vector5 = end - b;
			switch (type)
			{
			case UILineRenderer.SegmentType.Start:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.startUvs);
			case UILineRenderer.SegmentType.End:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.endUvs);
			case UILineRenderer.SegmentType.Full:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.fullUvs);
			}
			return base.SetVbo(new Vector2[]
			{
				vector2,
				vector3,
				vector4,
				vector5
			}, UILineRenderer.middleUvs);
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000C12E8 File Offset: 0x000BF6E8
		protected override void GeneratedUVs()
		{
			if (base.activeSprite != null)
			{
				Vector4 outerUV = DataUtility.GetOuterUV(base.activeSprite);
				Vector4 innerUV = DataUtility.GetInnerUV(base.activeSprite);
				UILineRenderer.UV_TOP_LEFT = new Vector2(outerUV.x, outerUV.y);
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(outerUV.x, outerUV.w);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(innerUV.x, innerUV.y);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.y);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(innerUV.x, innerUV.w);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.w);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(outerUV.z, outerUV.y);
				UILineRenderer.UV_BOTTOM_RIGHT = new Vector2(outerUV.z, outerUV.w);
			}
			else
			{
				UILineRenderer.UV_TOP_LEFT = Vector2.zero;
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(0f, 1f);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(1f, 0f);
				UILineRenderer.UV_BOTTOM_RIGHT = Vector2.one;
			}
			UILineRenderer.startUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_TOP_CENTER_LEFT
			};
			UILineRenderer.middleUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_TOP_CENTER_RIGHT
			};
			UILineRenderer.endUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
			UILineRenderer.fullUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x000C15AC File Offset: 0x000BF9AC
		protected override void ResolutionToNativeSize(float distance)
		{
			if (base.UseNativeSize)
			{
				this.m_Resolution = distance / (base.activeSprite.rect.width / base.pixelsPerUnit);
				this.lineThickness = base.activeSprite.rect.height / base.pixelsPerUnit;
			}
		}

		// Token: 0x04001BFF RID: 7167
		private const float MIN_MITER_JOIN = 0.2617994f;

		// Token: 0x04001C00 RID: 7168
		private const float MIN_BEVEL_NICE_JOIN = 0.5235988f;

		// Token: 0x04001C01 RID: 7169
		private static Vector2 UV_TOP_LEFT;

		// Token: 0x04001C02 RID: 7170
		private static Vector2 UV_BOTTOM_LEFT;

		// Token: 0x04001C03 RID: 7171
		private static Vector2 UV_TOP_CENTER_LEFT;

		// Token: 0x04001C04 RID: 7172
		private static Vector2 UV_TOP_CENTER_RIGHT;

		// Token: 0x04001C05 RID: 7173
		private static Vector2 UV_BOTTOM_CENTER_LEFT;

		// Token: 0x04001C06 RID: 7174
		private static Vector2 UV_BOTTOM_CENTER_RIGHT;

		// Token: 0x04001C07 RID: 7175
		private static Vector2 UV_TOP_RIGHT;

		// Token: 0x04001C08 RID: 7176
		private static Vector2 UV_BOTTOM_RIGHT;

		// Token: 0x04001C09 RID: 7177
		private static Vector2[] startUvs;

		// Token: 0x04001C0A RID: 7178
		private static Vector2[] middleUvs;

		// Token: 0x04001C0B RID: 7179
		private static Vector2[] endUvs;

		// Token: 0x04001C0C RID: 7180
		private static Vector2[] fullUvs;

		// Token: 0x04001C0D RID: 7181
		[SerializeField]
		[Tooltip("Points to draw lines between\n Can be improved using the Resolution Option")]
		internal Vector2[] m_points;

		// Token: 0x04001C0E RID: 7182
		[SerializeField]
		[Tooltip("Thickness of the line")]
		internal float lineThickness = 2f;

		// Token: 0x04001C0F RID: 7183
		[SerializeField]
		[Tooltip("Use the relative bounds of the Rect Transform (0,0 -> 0,1) or screen space coordinates")]
		internal bool relativeSize;

		// Token: 0x04001C10 RID: 7184
		[SerializeField]
		[Tooltip("Do the points identify a single line or split pairs of lines")]
		internal bool lineList;

		// Token: 0x04001C11 RID: 7185
		[SerializeField]
		[Tooltip("Add end caps to each line\nMultiple caps when used with Line List")]
		internal bool lineCaps;

		// Token: 0x04001C12 RID: 7186
		[SerializeField]
		[Tooltip("Resolution of the Bezier curve, different to line Resolution")]
		internal int bezierSegmentsPerCurve = 10;

		// Token: 0x04001C13 RID: 7187
		[Tooltip("The type of Join used between lines, Square/Mitre or Curved/Bevel")]
		public UILineRenderer.JoinType LineJoins;

		// Token: 0x04001C14 RID: 7188
		[Tooltip("Bezier method to apply to line, see docs for options\nCan't be used in conjunction with Resolution as Bezier already changes the resolution")]
		public UILineRenderer.BezierType BezierMode;

		// Token: 0x04001C15 RID: 7189
		[HideInInspector]
		public bool drivenExternally;

		// Token: 0x0200052F RID: 1327
		private enum SegmentType
		{
			// Token: 0x04001C17 RID: 7191
			Start,
			// Token: 0x04001C18 RID: 7192
			Middle,
			// Token: 0x04001C19 RID: 7193
			End,
			// Token: 0x04001C1A RID: 7194
			Full
		}

		// Token: 0x02000530 RID: 1328
		public enum JoinType
		{
			// Token: 0x04001C1C RID: 7196
			Bevel,
			// Token: 0x04001C1D RID: 7197
			Miter
		}

		// Token: 0x02000531 RID: 1329
		public enum BezierType
		{
			// Token: 0x04001C1F RID: 7199
			None,
			// Token: 0x04001C20 RID: 7200
			Quick,
			// Token: 0x04001C21 RID: 7201
			Basic,
			// Token: 0x04001C22 RID: 7202
			Improved,
			// Token: 0x04001C23 RID: 7203
			Catenary
		}
	}
}
