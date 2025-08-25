using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000521 RID: 1313
	[AddComponentMenu("Layout/Extensions/Table Layout Group")]
	public class TableLayoutGroup : LayoutGroup
	{
		// Token: 0x06002129 RID: 8489 RVA: 0x000BDBC1 File Offset: 0x000BBFC1
		public TableLayoutGroup()
		{
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x000BDBEF File Offset: 0x000BBFEF
		// (set) Token: 0x0600212B RID: 8491 RVA: 0x000BDBF7 File Offset: 0x000BBFF7
		public TableLayoutGroup.Corner StartCorner
		{
			get
			{
				return this.startCorner;
			}
			set
			{
				base.SetProperty<TableLayoutGroup.Corner>(ref this.startCorner, value);
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x0600212C RID: 8492 RVA: 0x000BDC06 File Offset: 0x000BC006
		// (set) Token: 0x0600212D RID: 8493 RVA: 0x000BDC0E File Offset: 0x000BC00E
		public float[] ColumnWidths
		{
			get
			{
				return this.columnWidths;
			}
			set
			{
				base.SetProperty<float[]>(ref this.columnWidths, value);
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x000BDC1D File Offset: 0x000BC01D
		// (set) Token: 0x0600212F RID: 8495 RVA: 0x000BDC25 File Offset: 0x000BC025
		public float MinimumRowHeight
		{
			get
			{
				return this.minimumRowHeight;
			}
			set
			{
				base.SetProperty<float>(ref this.minimumRowHeight, value);
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x000BDC34 File Offset: 0x000BC034
		// (set) Token: 0x06002131 RID: 8497 RVA: 0x000BDC3C File Offset: 0x000BC03C
		public bool FlexibleRowHeight
		{
			get
			{
				return this.flexibleRowHeight;
			}
			set
			{
				base.SetProperty<bool>(ref this.flexibleRowHeight, value);
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x000BDC4B File Offset: 0x000BC04B
		// (set) Token: 0x06002133 RID: 8499 RVA: 0x000BDC53 File Offset: 0x000BC053
		public float ColumnSpacing
		{
			get
			{
				return this.columnSpacing;
			}
			set
			{
				base.SetProperty<float>(ref this.columnSpacing, value);
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06002134 RID: 8500 RVA: 0x000BDC62 File Offset: 0x000BC062
		// (set) Token: 0x06002135 RID: 8501 RVA: 0x000BDC6A File Offset: 0x000BC06A
		public float RowSpacing
		{
			get
			{
				return this.rowSpacing;
			}
			set
			{
				base.SetProperty<float>(ref this.rowSpacing, value);
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000BDC7C File Offset: 0x000BC07C
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			float num = (float)base.padding.horizontal;
			int num2 = Mathf.Min(base.rectChildren.Count, this.columnWidths.Length);
			for (int i = 0; i < num2; i++)
			{
				num += this.columnWidths[i];
				num += this.columnSpacing;
			}
			num -= this.columnSpacing;
			base.SetLayoutInputForAxis(num, num, 0f, 0);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000BDCF4 File Offset: 0x000BC0F4
		public override void CalculateLayoutInputVertical()
		{
			int num = this.columnWidths.Length;
			int num2 = Mathf.CeilToInt((float)base.rectChildren.Count / (float)num);
			this.preferredRowHeights = new float[num2];
			float num3 = (float)base.padding.vertical;
			float num4 = (float)base.padding.vertical;
			if (num2 > 1)
			{
				float num5 = (float)(num2 - 1) * this.rowSpacing;
				num3 += num5;
				num4 += num5;
			}
			if (this.flexibleRowHeight)
			{
				for (int i = 0; i < num2; i++)
				{
					float num6 = this.minimumRowHeight;
					float num7 = this.minimumRowHeight;
					for (int j = 0; j < num; j++)
					{
						int num8 = i * num + j;
						if (num8 == base.rectChildren.Count)
						{
							break;
						}
						num7 = Mathf.Max(LayoutUtility.GetPreferredHeight(base.rectChildren[num8]), num7);
						num6 = Mathf.Max(LayoutUtility.GetMinHeight(base.rectChildren[num8]), num6);
					}
					num3 += num6;
					num4 += num7;
					this.preferredRowHeights[i] = num7;
				}
			}
			else
			{
				for (int k = 0; k < num2; k++)
				{
					this.preferredRowHeights[k] = this.minimumRowHeight;
				}
				num3 += (float)num2 * this.minimumRowHeight;
				num4 = num3;
			}
			num4 = Mathf.Max(num3, num4);
			base.SetLayoutInputForAxis(num3, num4, 1f, 1);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000BDE74 File Offset: 0x000BC274
		public override void SetLayoutHorizontal()
		{
			if (this.columnWidths.Length == 0)
			{
				this.columnWidths = new float[1];
			}
			int num = this.columnWidths.Length;
			int num2 = (int)(this.startCorner % TableLayoutGroup.Corner.LowerLeft);
			float num3 = 0f;
			int num4 = Mathf.Min(base.rectChildren.Count, this.columnWidths.Length);
			for (int i = 0; i < num4; i++)
			{
				num3 += this.columnWidths[i];
				num3 += this.columnSpacing;
			}
			num3 -= this.columnSpacing;
			float num5 = base.GetStartOffset(0, num3);
			if (num2 == 1)
			{
				num5 += num3;
			}
			float num6 = num5;
			for (int j = 0; j < base.rectChildren.Count; j++)
			{
				int num7 = j % num;
				if (num7 == 0)
				{
					num6 = num5;
				}
				if (num2 == 1)
				{
					num6 -= this.columnWidths[num7];
				}
				base.SetChildAlongAxis(base.rectChildren[j], 0, num6, this.columnWidths[num7]);
				if (num2 == 1)
				{
					num6 -= this.columnSpacing;
				}
				else
				{
					num6 += this.columnWidths[num7] + this.columnSpacing;
				}
			}
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000BDFB0 File Offset: 0x000BC3B0
		public override void SetLayoutVertical()
		{
			int num = this.columnWidths.Length;
			int num2 = this.preferredRowHeights.Length;
			int num3 = (int)(this.startCorner / TableLayoutGroup.Corner.LowerLeft);
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				num4 += this.preferredRowHeights[i];
			}
			if (num2 > 1)
			{
				num4 += (float)(num2 - 1) * this.rowSpacing;
			}
			float num5 = base.GetStartOffset(1, num4);
			if (num3 == 1)
			{
				num5 += num4;
			}
			float num6 = num5;
			for (int j = 0; j < num2; j++)
			{
				if (num3 == 1)
				{
					num6 -= this.preferredRowHeights[j];
				}
				for (int k = 0; k < num; k++)
				{
					int num7 = j * num + k;
					if (num7 == base.rectChildren.Count)
					{
						break;
					}
					base.SetChildAlongAxis(base.rectChildren[num7], 1, num6, this.preferredRowHeights[j]);
				}
				if (num3 == 1)
				{
					num6 -= this.rowSpacing;
				}
				else
				{
					num6 += this.preferredRowHeights[j] + this.rowSpacing;
				}
			}
			this.preferredRowHeights = null;
		}

		// Token: 0x04001BC6 RID: 7110
		[SerializeField]
		protected TableLayoutGroup.Corner startCorner;

		// Token: 0x04001BC7 RID: 7111
		[SerializeField]
		protected float[] columnWidths = new float[]
		{
			96f
		};

		// Token: 0x04001BC8 RID: 7112
		[SerializeField]
		protected float minimumRowHeight = 32f;

		// Token: 0x04001BC9 RID: 7113
		[SerializeField]
		protected bool flexibleRowHeight = true;

		// Token: 0x04001BCA RID: 7114
		[SerializeField]
		protected float columnSpacing;

		// Token: 0x04001BCB RID: 7115
		[SerializeField]
		protected float rowSpacing;

		// Token: 0x04001BCC RID: 7116
		private float[] preferredRowHeights;

		// Token: 0x02000522 RID: 1314
		public enum Corner
		{
			// Token: 0x04001BCE RID: 7118
			UpperLeft,
			// Token: 0x04001BCF RID: 7119
			UpperRight,
			// Token: 0x04001BD0 RID: 7120
			LowerLeft,
			// Token: 0x04001BD1 RID: 7121
			LowerRight
		}
	}
}
