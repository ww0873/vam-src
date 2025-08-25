using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200050F RID: 1295
	[AddComponentMenu("Layout/Extensions/Flow Layout Group")]
	public class FlowLayoutGroup : LayoutGroup
	{
		// Token: 0x060020A6 RID: 8358 RVA: 0x000BAC13 File Offset: 0x000B9013
		public FlowLayoutGroup()
		{
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000BAC28 File Offset: 0x000B9028
		public override void CalculateLayoutInputHorizontal()
		{
			base.CalculateLayoutInputHorizontal();
			float totalMin = this.GetGreatestMinimumChildWidth() + (float)base.padding.left + (float)base.padding.right;
			base.SetLayoutInputForAxis(totalMin, -1f, -1f, 0);
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000BAC70 File Offset: 0x000B9070
		public override void SetLayoutHorizontal()
		{
			this.SetLayout(base.rectTransform.rect.width, 0, false);
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000BAC9C File Offset: 0x000B909C
		public override void SetLayoutVertical()
		{
			this.SetLayout(base.rectTransform.rect.width, 1, false);
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000BACC8 File Offset: 0x000B90C8
		public override void CalculateLayoutInputVertical()
		{
			this._layoutHeight = this.SetLayout(base.rectTransform.rect.width, 1, true);
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x000BACF6 File Offset: 0x000B90F6
		protected bool IsCenterAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerCenter || base.childAlignment == TextAnchor.MiddleCenter || base.childAlignment == TextAnchor.UpperCenter;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x000BAD1C File Offset: 0x000B911C
		protected bool IsRightAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.UpperRight;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060020AD RID: 8365 RVA: 0x000BAD42 File Offset: 0x000B9142
		protected bool IsMiddleAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.MiddleLeft || base.childAlignment == TextAnchor.MiddleRight || base.childAlignment == TextAnchor.MiddleCenter;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060020AE RID: 8366 RVA: 0x000BAD68 File Offset: 0x000B9168
		protected bool IsLowerAlign
		{
			get
			{
				return base.childAlignment == TextAnchor.LowerLeft || base.childAlignment == TextAnchor.LowerRight || base.childAlignment == TextAnchor.LowerCenter;
			}
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000BAD90 File Offset: 0x000B9190
		public float SetLayout(float width, int axis, bool layoutInput)
		{
			float height = base.rectTransform.rect.height;
			float num = base.rectTransform.rect.width - (float)base.padding.left - (float)base.padding.right;
			float num2 = (!this.IsLowerAlign) ? ((float)base.padding.top) : ((float)base.padding.bottom);
			float num3 = 0f;
			float num4 = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				int index = (!this.IsLowerAlign) ? i : (base.rectChildren.Count - 1 - i);
				RectTransform rectTransform = base.rectChildren[index];
				float num5 = LayoutUtility.GetPreferredSize(rectTransform, 0);
				float preferredSize = LayoutUtility.GetPreferredSize(rectTransform, 1);
				num5 = Mathf.Min(num5, num);
				if (num3 + num5 > num)
				{
					num3 -= this.SpacingX;
					if (!layoutInput)
					{
						float yOffset = this.CalculateRowVerticalOffset(height, num2, num4);
						this.LayoutRow(this._rowList, num3, num4, num, (float)base.padding.left, yOffset, axis);
					}
					this._rowList.Clear();
					num2 += num4;
					num2 += this.SpacingY;
					num4 = 0f;
					num3 = 0f;
				}
				num3 += num5;
				this._rowList.Add(rectTransform);
				if (preferredSize > num4)
				{
					num4 = preferredSize;
				}
				if (i < base.rectChildren.Count - 1)
				{
					num3 += this.SpacingX;
				}
			}
			if (!layoutInput)
			{
				float yOffset2 = this.CalculateRowVerticalOffset(height, num2, num4);
				num3 -= this.SpacingX;
				this.LayoutRow(this._rowList, num3, num4, num - ((this._rowList.Count <= 1) ? 0f : this.SpacingX), (float)base.padding.left, yOffset2, axis);
			}
			this._rowList.Clear();
			num2 += num4;
			num2 += (float)((!this.IsLowerAlign) ? base.padding.bottom : base.padding.top);
			if (layoutInput && axis == 1)
			{
				base.SetLayoutInputForAxis(num2, num2, -1f, axis);
			}
			return num2;
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000BAFFC File Offset: 0x000B93FC
		private float CalculateRowVerticalOffset(float groupHeight, float yOffset, float currentRowHeight)
		{
			float result;
			if (this.IsLowerAlign)
			{
				result = groupHeight - yOffset - currentRowHeight;
			}
			else if (this.IsMiddleAlign)
			{
				result = groupHeight * 0.5f - this._layoutHeight * 0.5f + yOffset;
			}
			else
			{
				result = yOffset;
			}
			return result;
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000BB04C File Offset: 0x000B944C
		protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{
			float num = xOffset;
			if (!this.ChildForceExpandWidth && this.IsCenterAlign)
			{
				num += (maxWidth - rowWidth) * 0.5f;
			}
			else if (!this.ChildForceExpandWidth && this.IsRightAlign)
			{
				num += maxWidth - rowWidth;
			}
			float num2 = 0f;
			float num3 = 0f;
			if (this.ChildForceExpandWidth)
			{
				num2 = (maxWidth - rowWidth) / (float)this._rowList.Count;
			}
			else if (this.ExpandHorizontalSpacing)
			{
				num3 = (maxWidth - rowWidth) / (float)(this._rowList.Count - 1);
				if (this._rowList.Count > 1)
				{
					if (this.IsCenterAlign)
					{
						num -= num3 * 0.5f * (float)(this._rowList.Count - 1);
					}
					else if (this.IsRightAlign)
					{
						num -= num3 * (float)(this._rowList.Count - 1);
					}
				}
			}
			for (int i = 0; i < this._rowList.Count; i++)
			{
				int index = (!this.IsLowerAlign) ? i : (this._rowList.Count - 1 - i);
				RectTransform rect = this._rowList[index];
				float num4 = LayoutUtility.GetPreferredSize(rect, 0) + num2;
				float num5 = LayoutUtility.GetPreferredSize(rect, 1);
				if (this.ChildForceExpandHeight)
				{
					num5 = rowHeight;
				}
				num4 = Mathf.Min(num4, maxWidth);
				float num6 = yOffset;
				if (this.IsMiddleAlign)
				{
					num6 += (rowHeight - num5) * 0.5f;
				}
				else if (this.IsLowerAlign)
				{
					num6 += rowHeight - num5;
				}
				if (this.ExpandHorizontalSpacing && i > 0)
				{
					num += num3;
				}
				if (axis == 0)
				{
					base.SetChildAlongAxis(rect, 0, num, num4);
				}
				else
				{
					base.SetChildAlongAxis(rect, 1, num6, num5);
				}
				if (i < this._rowList.Count - 1)
				{
					num += num4 + this.SpacingX;
				}
			}
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000BB254 File Offset: 0x000B9654
		public float GetGreatestMinimumChildWidth()
		{
			float num = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				float minWidth = LayoutUtility.GetMinWidth(base.rectChildren[i]);
				num = Mathf.Max(minWidth, num);
			}
			return num;
		}

		// Token: 0x04001B51 RID: 6993
		public float SpacingX;

		// Token: 0x04001B52 RID: 6994
		public float SpacingY;

		// Token: 0x04001B53 RID: 6995
		public bool ExpandHorizontalSpacing;

		// Token: 0x04001B54 RID: 6996
		public bool ChildForceExpandWidth;

		// Token: 0x04001B55 RID: 6997
		public bool ChildForceExpandHeight;

		// Token: 0x04001B56 RID: 6998
		private float _layoutHeight;

		// Token: 0x04001B57 RID: 6999
		private readonly IList<RectTransform> _rowList = new List<RectTransform>();
	}
}
