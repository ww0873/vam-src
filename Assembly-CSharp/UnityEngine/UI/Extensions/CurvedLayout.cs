using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000509 RID: 1289
	[AddComponentMenu("Layout/Extensions/Curved Layout")]
	public class CurvedLayout : LayoutGroup
	{
		// Token: 0x0600208D RID: 8333 RVA: 0x000BAA7D File Offset: 0x000B8E7D
		public CurvedLayout()
		{
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000BAA90 File Offset: 0x000B8E90
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CalculateRadial();
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000BAA9E File Offset: 0x000B8E9E
		public override void SetLayoutHorizontal()
		{
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000BAAA0 File Offset: 0x000B8EA0
		public override void SetLayoutVertical()
		{
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000BAAA2 File Offset: 0x000B8EA2
		public override void CalculateLayoutInputVertical()
		{
			this.CalculateRadial();
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000BAAAA File Offset: 0x000B8EAA
		public override void CalculateLayoutInputHorizontal()
		{
			this.CalculateRadial();
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000BAAB4 File Offset: 0x000B8EB4
		private void CalculateRadial()
		{
			this.m_Tracker.Clear();
			if (base.transform.childCount == 0)
			{
				return;
			}
			Vector2 pivot = new Vector2((float)(base.childAlignment % TextAnchor.MiddleLeft) * 0.5f, (float)(base.childAlignment / TextAnchor.MiddleLeft) * 0.5f);
			Vector3 a = new Vector3(base.GetStartOffset(0, base.GetTotalPreferredSize(0)), base.GetStartOffset(1, base.GetTotalPreferredSize(1)), 0f);
			float num = 0f;
			float num2 = 1f / (float)base.transform.childCount;
			Vector3 b = this.itemAxis.normalized * this.itemSize;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != null)
				{
					this.m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY);
					Vector3 a2 = a + b;
					a = (rectTransform.localPosition = a2 + (num - this.centerpoint) * this.CurveOffset);
					rectTransform.pivot = pivot;
					RectTransform rectTransform2 = rectTransform;
					Vector2 vector = new Vector2(0.5f, 0.5f);
					rectTransform.anchorMax = vector;
					rectTransform2.anchorMin = vector;
					num += num2;
				}
			}
		}

		// Token: 0x04001B44 RID: 6980
		public Vector3 CurveOffset;

		// Token: 0x04001B45 RID: 6981
		[Tooltip("axis along which to place the items, Normalized before use")]
		public Vector3 itemAxis;

		// Token: 0x04001B46 RID: 6982
		[Tooltip("size of each item along the Normalized axis")]
		public float itemSize;

		// Token: 0x04001B47 RID: 6983
		public float centerpoint = 0.5f;
	}
}
