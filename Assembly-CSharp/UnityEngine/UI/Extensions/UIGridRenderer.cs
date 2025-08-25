using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200052D RID: 1325
	[AddComponentMenu("UI/Extensions/Primitives/UIGridRenderer")]
	public class UIGridRenderer : UILineRenderer
	{
		// Token: 0x060021AD RID: 8621 RVA: 0x000C1606 File Offset: 0x000BFA06
		public UIGridRenderer()
		{
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x000C161E File Offset: 0x000BFA1E
		// (set) Token: 0x060021AF RID: 8623 RVA: 0x000C1626 File Offset: 0x000BFA26
		public int GridColumns
		{
			get
			{
				return this.m_GridColumns;
			}
			set
			{
				if (this.m_GridColumns == value)
				{
					return;
				}
				this.m_GridColumns = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x000C1642 File Offset: 0x000BFA42
		// (set) Token: 0x060021B1 RID: 8625 RVA: 0x000C164A File Offset: 0x000BFA4A
		public int GridRows
		{
			get
			{
				return this.m_GridRows;
			}
			set
			{
				if (this.m_GridRows == value)
				{
					return;
				}
				this.m_GridRows = value;
				this.SetAllDirty();
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000C1668 File Offset: 0x000BFA68
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			this.relativeSize = true;
			int num = this.GridRows * 3 + 1;
			if (this.GridRows % 2 == 0)
			{
				num++;
			}
			num += this.GridColumns * 3 + 1;
			this.m_points = new Vector2[num];
			int num2 = 0;
			for (int i = 0; i < this.GridRows; i++)
			{
				float x = 1f;
				float x2 = 0f;
				if (i % 2 == 0)
				{
					x = 0f;
					x2 = 1f;
				}
				float y = (float)i / (float)this.GridRows;
				this.m_points[num2].x = x;
				this.m_points[num2].y = y;
				num2++;
				this.m_points[num2].x = x2;
				this.m_points[num2].y = y;
				num2++;
				this.m_points[num2].x = x2;
				this.m_points[num2].y = (float)(i + 1) / (float)this.GridRows;
				num2++;
			}
			if (this.GridRows % 2 == 0)
			{
				this.m_points[num2].x = 1f;
				this.m_points[num2].y = 1f;
				num2++;
			}
			this.m_points[num2].x = 0f;
			this.m_points[num2].y = 1f;
			num2++;
			for (int j = 0; j < this.GridColumns; j++)
			{
				float y2 = 1f;
				float y3 = 0f;
				if (j % 2 == 0)
				{
					y2 = 0f;
					y3 = 1f;
				}
				float x3 = (float)j / (float)this.GridColumns;
				this.m_points[num2].x = x3;
				this.m_points[num2].y = y2;
				num2++;
				this.m_points[num2].x = x3;
				this.m_points[num2].y = y3;
				num2++;
				this.m_points[num2].x = (float)(j + 1) / (float)this.GridColumns;
				this.m_points[num2].y = y3;
				num2++;
			}
			if (this.GridColumns % 2 == 0)
			{
				this.m_points[num2].x = 1f;
				this.m_points[num2].y = 1f;
			}
			else
			{
				this.m_points[num2].x = 1f;
				this.m_points[num2].y = 0f;
			}
			base.OnPopulateMesh(vh);
		}

		// Token: 0x04001BFD RID: 7165
		[SerializeField]
		private int m_GridColumns = 10;

		// Token: 0x04001BFE RID: 7166
		[SerializeField]
		private int m_GridRows = 10;
	}
}
