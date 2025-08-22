using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200052C RID: 1324
	[AddComponentMenu("UI/Extensions/Primitives/Cut Corners")]
	public class UICornerCut : UIPrimitiveBase
	{
		// Token: 0x06002196 RID: 8598 RVA: 0x000C02E9 File Offset: 0x000BE6E9
		public UICornerCut()
		{
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x000C030D File Offset: 0x000BE70D
		// (set) Token: 0x06002198 RID: 8600 RVA: 0x000C0315 File Offset: 0x000BE715
		public bool CutUL
		{
			get
			{
				return this.m_cutUL;
			}
			set
			{
				this.m_cutUL = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x000C0324 File Offset: 0x000BE724
		// (set) Token: 0x0600219A RID: 8602 RVA: 0x000C032C File Offset: 0x000BE72C
		public bool CutUR
		{
			get
			{
				return this.m_cutUR;
			}
			set
			{
				this.m_cutUR = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600219B RID: 8603 RVA: 0x000C033B File Offset: 0x000BE73B
		// (set) Token: 0x0600219C RID: 8604 RVA: 0x000C0343 File Offset: 0x000BE743
		public bool CutLL
		{
			get
			{
				return this.m_cutLL;
			}
			set
			{
				this.m_cutLL = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600219D RID: 8605 RVA: 0x000C0352 File Offset: 0x000BE752
		// (set) Token: 0x0600219E RID: 8606 RVA: 0x000C035A File Offset: 0x000BE75A
		public bool CutLR
		{
			get
			{
				return this.m_cutLR;
			}
			set
			{
				this.m_cutLR = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600219F RID: 8607 RVA: 0x000C0369 File Offset: 0x000BE769
		// (set) Token: 0x060021A0 RID: 8608 RVA: 0x000C0371 File Offset: 0x000BE771
		public bool MakeColumns
		{
			get
			{
				return this.m_makeColumns;
			}
			set
			{
				this.m_makeColumns = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x000C0380 File Offset: 0x000BE780
		// (set) Token: 0x060021A2 RID: 8610 RVA: 0x000C0388 File Offset: 0x000BE788
		public bool UseColorUp
		{
			get
			{
				return this.m_useColorUp;
			}
			set
			{
				this.m_useColorUp = value;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x000C0391 File Offset: 0x000BE791
		// (set) Token: 0x060021A4 RID: 8612 RVA: 0x000C0399 File Offset: 0x000BE799
		public Color32 ColorUp
		{
			get
			{
				return this.m_colorUp;
			}
			set
			{
				this.m_colorUp = value;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x000C03A2 File Offset: 0x000BE7A2
		// (set) Token: 0x060021A6 RID: 8614 RVA: 0x000C03AA File Offset: 0x000BE7AA
		public bool UseColorDown
		{
			get
			{
				return this.m_useColorDown;
			}
			set
			{
				this.m_useColorDown = value;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x000C03B3 File Offset: 0x000BE7B3
		// (set) Token: 0x060021A8 RID: 8616 RVA: 0x000C03BB File Offset: 0x000BE7BB
		public Color32 ColorDown
		{
			get
			{
				return this.m_colorDown;
			}
			set
			{
				this.m_colorDown = value;
			}
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000C03C4 File Offset: 0x000BE7C4
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			Rect rect = base.rectTransform.rect;
			Rect rect2 = rect;
			Color32 color = this.color;
			bool flag = this.m_cutUL | this.m_cutUR;
			bool flag2 = this.m_cutLL | this.m_cutLR;
			bool flag3 = this.m_cutLL | this.m_cutUL;
			bool flag4 = this.m_cutLR | this.m_cutUR;
			bool flag5 = flag || flag2;
			if (flag5 && this.cornerSize.sqrMagnitude > 0f)
			{
				vh.Clear();
				if (flag3)
				{
					rect2.xMin += this.cornerSize.x;
				}
				if (flag2)
				{
					rect2.yMin += this.cornerSize.y;
				}
				if (flag)
				{
					rect2.yMax -= this.cornerSize.y;
				}
				if (flag4)
				{
					rect2.xMax -= this.cornerSize.x;
				}
				if (this.m_makeColumns)
				{
					Vector2 vector = new Vector2(rect.xMin, (!this.m_cutUL) ? rect.yMax : rect2.yMax);
					Vector2 vector2 = new Vector2(rect.xMax, (!this.m_cutUR) ? rect.yMax : rect2.yMax);
					Vector2 vector3 = new Vector2(rect.xMin, (!this.m_cutLL) ? rect.yMin : rect2.yMin);
					Vector2 vector4 = new Vector2(rect.xMax, (!this.m_cutLR) ? rect.yMin : rect2.yMin);
					if (flag3)
					{
						UICornerCut.AddSquare(vector3, vector, new Vector2(rect2.xMin, rect.yMax), new Vector2(rect2.xMin, rect.yMin), rect, (!this.m_useColorUp) ? color : this.m_colorUp, vh);
					}
					if (flag4)
					{
						UICornerCut.AddSquare(vector2, vector4, new Vector2(rect2.xMax, rect.yMin), new Vector2(rect2.xMax, rect.yMax), rect, (!this.m_useColorDown) ? color : this.m_colorDown, vh);
					}
				}
				else
				{
					Vector2 vector = new Vector2((!this.m_cutUL) ? rect.xMin : rect2.xMin, rect.yMax);
					Vector2 vector2 = new Vector2((!this.m_cutUR) ? rect.xMax : rect2.xMax, rect.yMax);
					Vector2 vector3 = new Vector2((!this.m_cutLL) ? rect.xMin : rect2.xMin, rect.yMin);
					Vector2 vector4 = new Vector2((!this.m_cutLR) ? rect.xMax : rect2.xMax, rect.yMin);
					if (flag2)
					{
						UICornerCut.AddSquare(vector4, vector3, new Vector2(rect.xMin, rect2.yMin), new Vector2(rect.xMax, rect2.yMin), rect, (!this.m_useColorDown) ? color : this.m_colorDown, vh);
					}
					if (flag)
					{
						UICornerCut.AddSquare(vector, vector2, new Vector2(rect.xMax, rect2.yMax), new Vector2(rect.xMin, rect2.yMax), rect, (!this.m_useColorUp) ? color : this.m_colorUp, vh);
					}
				}
				if (this.m_makeColumns)
				{
					UICornerCut.AddSquare(new Rect(rect2.xMin, rect.yMin, rect2.width, rect.height), rect, color, vh);
				}
				else
				{
					UICornerCut.AddSquare(new Rect(rect.xMin, rect2.yMin, rect.width, rect2.height), rect, color, vh);
				}
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000C07DC File Offset: 0x000BEBDC
		private static void AddSquare(Rect rect, Rect rectUV, Color32 color32, VertexHelper vh)
		{
			int num = UICornerCut.AddVert(rect.xMin, rect.yMin, rectUV, color32, vh);
			int idx = UICornerCut.AddVert(rect.xMin, rect.yMax, rectUV, color32, vh);
			int num2 = UICornerCut.AddVert(rect.xMax, rect.yMax, rectUV, color32, vh);
			int idx2 = UICornerCut.AddVert(rect.xMax, rect.yMin, rectUV, color32, vh);
			vh.AddTriangle(num, idx, num2);
			vh.AddTriangle(num2, idx2, num);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000C0858 File Offset: 0x000BEC58
		private static void AddSquare(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Rect rectUV, Color32 color32, VertexHelper vh)
		{
			int num = UICornerCut.AddVert(a.x, a.y, rectUV, color32, vh);
			int idx = UICornerCut.AddVert(b.x, b.y, rectUV, color32, vh);
			int num2 = UICornerCut.AddVert(c.x, c.y, rectUV, color32, vh);
			int idx2 = UICornerCut.AddVert(d.x, d.y, rectUV, color32, vh);
			vh.AddTriangle(num, idx, num2);
			vh.AddTriangle(num2, idx2, num);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000C08E4 File Offset: 0x000BECE4
		private static int AddVert(float x, float y, Rect area, Color32 color32, VertexHelper vh)
		{
			Vector2 uv = new Vector2(Mathf.InverseLerp(area.xMin, area.xMax, x), Mathf.InverseLerp(area.yMin, area.yMax, y));
			vh.AddVert(new Vector3(x, y), color32, uv);
			return vh.currentVertCount - 1;
		}

		// Token: 0x04001BF3 RID: 7155
		public Vector2 cornerSize = new Vector2(16f, 16f);

		// Token: 0x04001BF4 RID: 7156
		[Header("Corners to cut")]
		[SerializeField]
		private bool m_cutUL = true;

		// Token: 0x04001BF5 RID: 7157
		[SerializeField]
		private bool m_cutUR;

		// Token: 0x04001BF6 RID: 7158
		[SerializeField]
		private bool m_cutLL;

		// Token: 0x04001BF7 RID: 7159
		[SerializeField]
		private bool m_cutLR;

		// Token: 0x04001BF8 RID: 7160
		[Tooltip("Up-Down colors become Left-Right colors")]
		[SerializeField]
		private bool m_makeColumns;

		// Token: 0x04001BF9 RID: 7161
		[Header("Color the cut bars differently")]
		[SerializeField]
		private bool m_useColorUp;

		// Token: 0x04001BFA RID: 7162
		[SerializeField]
		private Color32 m_colorUp;

		// Token: 0x04001BFB RID: 7163
		[SerializeField]
		private bool m_useColorDown;

		// Token: 0x04001BFC RID: 7164
		[SerializeField]
		private Color32 m_colorDown;
	}
}
